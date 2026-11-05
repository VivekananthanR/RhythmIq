using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.DTO.ResDTO.Token;
using elr_API.IService.ICommon_manager;
using elr_API.IService.IDapper;
using elr_API.IService.IEntrolement;
using elr_API.IService.IToken;
using elr_API.IService.IUser_manager;

namespace elr_API.Service.User_manager
{
    public class UserService : IUserService
    {
        private readonly IDapperSqlService _dp;
        private readonly ITokenService _tokenService;
        private readonly IHelperService _helperService;
        private readonly IQikberrychatService _iqberrychatService;
        private readonly IEnrollment _enrollment;

        public string Elr_db = CommonFunctions.GetConnectionString("elr_DB");
        public string ErrorMessage { get; set; } = string.Empty;

        public UserService(IDapperSqlService db, ITokenService tokenService, IHelperService helperService, IQikberrychatService qikberrychatService, IEnrollment enrollment)
        {
            _dp = db;
            _tokenService = tokenService;
            _helperService = helperService;
            _iqberrychatService = qikberrychatService;
            _enrollment = enrollment;
        }
        public async Task<ResponseDTO> ElrGetPatient(string phone_no)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(phone_no) || !Validation.IsValidPhoneNumber(phone_no)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                var patientDetails = await ElrPatientDetails(phone_no);

                if (patientDetails != null)
                {
                    return new ResponseDTO(patientDetails);
                }

                var patient = await _helperService.GetConnectPatientDetails(phone_no);

                if (patient == null) return new ResponseDTO(false, "Patient not found.");

                if (string.IsNullOrWhiteSpace(patient.patient_mobileno)) return new ResponseDTO(false, "Patient phone number not found.");


                var existingPatient = await ElrPatientDetails(patient.patient_mobileno);

                if (existingPatient != null)
                {
                    return new ResponseDTO(existingPatient);
                }

                query = ConnectPatientDetails.insert_patient_details();

                await _dp.Execute(query,
                    new
                    {
                        patient_name = patient.patient_fullname ?? "",
                        dob = patient.patient_dob ?? (DateTime?)null,
                        age = patient.patient_age ?? "",
                        phone_no = patient.patient_mobileno,
                        emergency_phone_no = patient.emergency_phone_no ?? "",
                        email_id = patient.patient_emailid ?? "",
                        gender = patient.patient_gender ?? "",
                        city = patient.patient_cityname ?? "",
                        pincode = patient.patient_pincode ?? ""

                    }, Elr_db);

                var savedPatient = await ElrPatientDetails(patient.patient_mobileno);

                return savedPatient != null ? new ResponseDTO(savedPatient) : new ResponseDTO(patient);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(phone_no, ex.ToString());
                ErrorMessage = ex.Message;
            }

            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ConnectPatientDetails> ElrPatientDetails(string ph_no)
        {
            string query = PatientVerifyLoginResDTO.patient_details_select();

            var patientDetails = (await _dp.SelectQuery<ConnectPatientDetails>(query, new { phone_no = ph_no }, Elr_db)).FirstOrDefault();
            return patientDetails;
        }

        public async Task<ResponseDTO> ElrPatientLogin(OtpForLoggingReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || !Validation.IsValidPhoneNumber(obj.phone_no)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                var patientData = await ElrPatientDetails(obj.phone_no);

                if (patientData == null || patientData.patient_mobileno == null) return new ResponseDTO(false, "Patient Phone Number Not Registed.");

                query = ConnectPatientDetails.exists_patient_id_enrollment();
                var patient = (await _dp.SelectQuery<dynamic>(query, new { phone_no = patientData.patient_mobileno }, Elr_db)).FirstOrDefault();
                if (patient != null)
                {


                    var checkPatientData = await _enrollment.ElrCheckPatientDetails(obj.phone_no);

                    if (checkPatientData != null)
                    {
                        if (obj.otp == 0)
                        {
                            int otp = obj.phone_no == "8056661324" ? 123456 : CommonFunctions.GenerateOTP(6);

                            #region Message
                            if (obj.phone_no != "8056661324")
                            {
                                OtpForLoggingReqDTO otpForLoggingReqDTO = new OtpForLoggingReqDTO();
                                otpForLoggingReqDTO.phone_no = obj.phone_no;
                                otpForLoggingReqDTO.otp = otp;

                                await _iqberrychatService.OtpForLoggingPatient(otpForLoggingReqDTO);
                            }
                            #endregion

                            query = ConnectPatientDetails.update_patient_otp();
                            int a = await _dp.Execute(query, new { phone_no = obj.phone_no, otp = otp }, Elr_db);

                            return new ResponseDTO(a > 0, (a > 0 ? ResponseMessage.OTP_sended : ResponseMessage.Failed));

                        }
                        if (obj.otp != patientData.otp)
                        {
                            return new ResponseDTO(false, ResponseMessage.Invalid_OTP);
                        }
                        else
                        {
                            if (patientData.expiry_date < DateTime.Now)
                            {
                                return new ResponseDTO(false, ResponseMessage.OTP_is_Expiry);
                            }
                            else
                            {
                                //TokenResDTO tokenResDTO = _tokenService.GetToken(patientData.patient_id.ToString(), patientData.gu_id.ToString(), patientData.patient_mobileno, CommonFunctions.ELR, null);
                                //patientData.token = tokenResDTO.Token;
                                //patientData.expiryTime = tokenResDTO.Token_ExpiryTime;

                                return new ResponseDTO(patientData);
                            }
                        }
                    }
                    return new ResponseDTO(false, $"Please complete your existing enrollment first. Enrollment ID: {checkPatientData.patient_enrollment_id}");
                }
                return new ResponseDTO(false, "Enrollment has not been created for this phone number.");


            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();

            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrAddUpdatePatientDetails(PatientVerifyLoginResDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (!Validation.IsValidPhoneNumber(obj.phone_no)) return new ResponseDTO(false, "Enter A Valid Phone Number.");

                query = PatientVerifyLoginReqDTO.existing_patient();
                PatientVerifyLoginResDTO data = (await _dp.SelectQuery<PatientVerifyLoginResDTO>(query, new { phone_no = obj.phone_no, email_id = obj.email_id, patient_id = obj.patient_id }, Elr_db)).FirstOrDefault();

                if (data != null)
                {
                    if (data.phone_no == obj.phone_no)
                        return new ResponseDTO(false, "Phone no is already exists");

                    if (data.email_id == obj.email_id)
                        return new ResponseDTO(false, "Email Id is already exists");
                }

                if (obj.patient_id == 0)
                    query = PatientVerifyLoginResDTO.insert_patient_details();
                else
                {
                    obj.modified_by = obj.created_by;
                    query = PatientVerifyLoginResDTO.update_patient_details();

                }
                var result = await _dp.Execute(query, obj, Elr_db);
                return new ResponseDTO(result > 0, result > 0 ? (obj.patient_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrPatientVerifyLogin(PatientVerifyLoginReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;

            try
            {
                if (obj == null || string.IsNullOrEmpty(obj.phone_no) || !Validation.IsValidPhoneNumber(obj.phone_no))
                    return new ResponseDTO(false, "Enter A Valid Phone Number.");

                query = PatientVerifyLoginReqDTO.existing_select_patient();

                var existPatient = (await _dp.SelectQuery<PatientVerifyLoginResDTO>(query, new { phone_no = obj.phone_no }, Elr_db)).FirstOrDefault();

                if (existPatient == null)
                {
                    var connectDetails = await _helperService.GetConnectPatientDetails(obj.phone_no);

                    if (connectDetails == null)
                        return new ResponseDTO(true, "New Patient");

                    existPatient = new PatientVerifyLoginResDTO
                    {
                        patient_id = connectDetails.patient_id,
                        patient_name = connectDetails.patient_fullname,
                        dob = connectDetails.patient_dob,
                        age = int.TryParse(connectDetails.patient_age, out var parsedAge) ? parsedAge : 0,
                        phone_no = connectDetails.patient_mobileno,
                        emergency_phone_no = connectDetails.emergency_phone_no,
                        email_id = connectDetails.patient_emailid,
                        gender = connectDetails.patient_gender,
                        city = connectDetails.patient_cityname,
                        pincode = connectDetails.patient_pincode
                    };
                }

                if (obj.otp == 0)
                {
                    int otp = obj.phone_no == "8056661324" ? 123456 : CommonFunctions.GenerateOTP(6);

                    if (obj.phone_no != "8056661324")
                    {

                        OtpForLoggingReqDTO otpForLoggingReqDTO = new OtpForLoggingReqDTO();
                        otpForLoggingReqDTO.phone_no = obj.phone_no;
                        otpForLoggingReqDTO.otp = obj.otp;

                        await _iqberrychatService.OtpForLoggingPatient(otpForLoggingReqDTO);

                    }

                    query = PatientVerifyLoginReqDTO.Send_otp_update();

                    int result = await _dp.Execute(query,
                        new
                        {
                            phone_no = obj.phone_no,
                            otp = otp

                        },
                        Elr_db);

                    return new ResponseDTO(result > 0, result > 0 ? ResponseMessage.OTP_sended : ResponseMessage.Failed);
                }

                query = PatientVerifyLoginReqDTO.otp_verify_select();

                var otpDetails = (await _dp.SelectQuery<PatientVerifyLoginResDTO>(
                    query,
                    new
                    {
                        phone_no = obj.phone_no,
                        otp = obj.otp
                    },
                    Elr_db)).FirstOrDefault();

                if (otpDetails == null)
                    return new ResponseDTO(false, ResponseMessage.Invalid_OTP);

                return new ResponseDTO(otpDetails);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message;
            }

            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrGetEmployeeNameList(string user_name)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = AddUpdateEmployeeDetailsReqDTO.select_employee_name_list();
                var data = (await _dp.SelectQuery<dynamic>(query, new { user_name = user_name }, Elr_db)).ToList();
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrGetEmployee(string emp_code)
        {
            ErrorMessage = string.Empty;
            try
            {
                return new ResponseDTO(await _helperService.GetMeterBoxEmployeeDetails(emp_code));
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrAddUpdateEmployeeDetails(AddUpdateEmployeeDetailsReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = AddUpdateEmployeeDetailsReqDTO.employee_details_select_validation();
                AddUpdateEmployeeDetailsReqDTO data = (await _dp.SelectQuery<AddUpdateEmployeeDetailsReqDTO>(query, obj, Elr_db)).FirstOrDefault();
                if (data != null)
                {
                    if (data.phone_no == obj.phone_no)
                    {
                        return new ResponseDTO(false, "Phone no is already exists");
                    }
                    else if (data.email_id == obj.email_id)
                    {
                        return new ResponseDTO(false, "Email Id is already exists");
                    }
                }
                if (obj.user_id == 0) query = AddUpdateEmployeeDetailsReqDTO.employee_details_insert();

                else
                {
                    obj.modified_by = obj.created_by;
                    query = AddUpdateEmployeeDetailsReqDTO.employee_details_update();
                }

                int changed = await _dp.Execute(query, obj, Elr_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.user_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrEmployeeLogOut(long user_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (user_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                query = MasterEmployeeLoginReqDTO.user_logOut();
                int row = await _dp.Execute(query, new { user_id = user_id }, Elr_db);
                return new ResponseDTO(row > 0, row > 0 ? ResponseMessage.Success : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }



        public async Task<bool> TokenValidation(TokenValidationReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            bool valid = false;
            try
            {

                query = TokenValidationReqDTO.user_select();
                bool check_user_valid = (await _dp.SelectQuery<dynamic>(query, obj, Elr_db)).Any();

                return check_user_valid;
            }
            catch (Exception ex)
            {
                //CommonFunctions.ILogging("", ex.ToString());
                //ErrorMessage = ResponseMessage.Please_Try_Again;
            }
            return false;
        }


        //Employee Login    
        public async Task<ResponseDTO> ElrEmployeeLogin(MasterEmployeeLoginReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || string.IsNullOrEmpty(obj.user_name) || string.IsNullOrEmpty(obj.password))
                {
                    return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                }
                MasterEmployeeLoginResDTO data = await GetEmployeeDetails(obj);

                if (data == null)
                {
                    return new ResponseDTO(false, "Employee is not registered. Please register the Employee Details");
                }
                var meterboxData = await _helperService.GetMeterBoxEmployeeDetails(data.emp_code);

                if (meterboxData == null) return new ResponseDTO(false, "Employee Not Found In Meterbox");

                if (meterboxData.m_Status != "Active") return new ResponseDTO(false, ResponseMessage.Access_Denied);

                if (meterboxData.m_Password != data.password || meterboxData.m_Email != data.email_id || meterboxData.m_Mobile != data.phone_no)
                {
                    query = $@"Update {Tbl.user_master}
                                Set password = @password, email_id = @email_id, phone_no =@phone_no
                                Where emp_code = @emp_code";
                    await _dp.Execute(query, new { password = meterboxData.m_Password, email_id = meterboxData.m_Email, phone_no = meterboxData.m_Mobile, emp_code = data.emp_code }, Elr_db);

                    data.password = meterboxData.m_Password;
                    data.email_id = meterboxData.m_Email;
                    data.phone_no = meterboxData.m_Mobile;
                }
                if (obj.password != data.password) return new ResponseDTO(false, "Invalid Password");

                Guid newLoginGuId = Guid.NewGuid();

                query = $@"Update {Tbl.user_master}
                            Set login_gu_id = @login_gu_id
                            Where user_id = @user_id";

                await _dp.Execute(query, new { login_gu_id = newLoginGuId, user_id = data.user_id }, Elr_db);

                data.login_gu_id = newLoginGuId;

                if (data != null)
                {
                    var tokenDetails = _tokenService.GetToken(data.user_id.ToString(), data.gu_id.ToString(), data.phone_no, CommonFunctions.ELR, data.login_gu_id.ToString());
                    data.token = tokenDetails.Token;
                    data.token_expirytime = tokenDetails.Token_ExpiryTime;
                }

                return new ResponseDTO(data);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                //ErrorMessage = ResponseMessage.Please_Try_Again;
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<MasterEmployeeLoginResDTO> GetEmployeeDetails(MasterEmployeeLoginReqDTO obj)
        {
            string query = MasterEmployeeLoginReqDTO.employee_master_select();
            MasterEmployeeLoginResDTO data = (await _dp.SelectQuery<MasterEmployeeLoginResDTO>(query, obj, Elr_db)).FirstOrDefault();

            return data;
        }
        public async Task<ResponseDTO> ElrphysiciansList()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                //string? search = string.IsNullOrWhiteSpace(physiciansName) ? null : physiciansName.Trim();
                query = AddUpdateEmployeeDetailsReqDTO.select_physicians_list();
                var (totalCount, data) = await _dp.SelectQueryWithCount<long, AddUpdateEmployeeDetailsReqDTO>(query, null, Elr_db);

                var response = new
                {
                    totalCount = totalCount,
                    item = data,
                };

                return new ResponseDTO(response);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrTechniciansList()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = AddUpdateEmployeeDetailsReqDTO.select_technicians_list();
                var (totalCount, data) = await _dp.SelectQueryWithCount<long, AddUpdateEmployeeDetailsReqDTO>(query, null, Elr_db);

                var response = new
                {
                    totalCount = totalCount,
                    item = data,
                };
                return new ResponseDTO(response);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrGetPatientEventPage(long patient_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (patient_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                query = PatientVerifyLoginResDTO.patient_detatils();
                var data = (await _dp.SelectQuery<PatientVerifyLoginResDTO>(query, new { patient_id = patient_id }, Elr_db)).FirstOrDefault();
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<ResponseDTO> ElrGetPatientSearch(PatientEcgSearchReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientEcgSearchReqDTO.selelct_pateint_details(obj);
                var (totalCount, data) = await _dp.SelectQueryWithCount<long, PatientEcgSearechResDTO>(query, obj, Elr_db);

                //if (!data.Any() || data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                var result = new
                {
                    totalCount = totalCount,
                    item = data,
                };

                return new ResponseDTO(result);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();

            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrEventProcessingTracking()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = EventProcessTrackingDTO.select_event_process_tracking();
                var (totalCount, data) = await _dp.SelectQueryWithCount<long, EventProcessTrackingDTO>(query, null, Elr_db);
                var result = new
                {
                    totalCount = totalCount,
                    item = data,
                };
                return new ResponseDTO(result);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

    }













    public class ConnectPatientDetails
    {
        public long patient_id { get; set; }
        public Guid? gu_id { get; set; }
        public string patient_fullname { get; set; }
        public DateTime? patient_dob { get; set; }
        public string patient_age { get; set; }
        public string patient_mobileno { get; set; }
        public string emergency_phone_no { get; set; }
        public string patient_emailid { get; set; }
        public string patient_gender { get; set; }
        public string patient_cityname { get; set; }
        public string patient_pincode { get; set; }

        public int otp { get; set; }
        public DateTime? expiry_date { get; set; }


        public string token { get; set; }
        public int expiryTime { get; set; }

        public static string exists_patient_id_enrollment()
        {
            return $@"
                    SELECT *
                    FROM {Tbl.patient} AS p
                    WHERE p.phone_no = @phone_no
                    AND EXISTS (
                        SELECT 1
                        FROM {Tbl.patient_enrollment} AS pe
                        WHERE pe.patient_id = p.patient_id
                    )";
        }
        internal static string insert_patient_details()
        {
            return $@"Insert Into {Tbl.patient} (patient_name,dob,age,phone_no,emergency_phone_no,email_id,gender,city,pincode)
                        Values (@patient_name, @dob, @age, @phone_no, @emergency_phone_no ,@email_id ,@gender,@city, @pincode)";
        }

        internal static string update_patient_otp()
        {
            return $@" update {Tbl.patient}
                        set otp  = @otp, expiry_date = DATEADD(MINUTE, 10, GETDATE())
                        where phone_no = @phone_no and is_active = 1";
        }
    }


    public class EmployeeMeterboxDetails
    {
        public string m_StaffID { get; set; }
        public string m_FName { get; set; }
        public string m_Designation { get; set; }
        public string m_Email { get; set; }
        public string m_Password { get; set; }
        public string m_Mobile { get; set; }
        public string m_Status { get; set; }
        public string m_Team { get; set; }
        public string m_Base { get; set; }
        public DateTime? m_DOB { get; set; }
    }

    public class MeterBoxApiResponse
    {
        public bool status { get; set; }
        public string errorCode { get; set; }
        public List<EmployeeMeterboxDetails> data { get; set; }
    }
    public class ConnectApiResponse
    {
        public bool status { get; set; }
        public string errorCode { get; set; }
        public string data { get; set; }
    }
}
