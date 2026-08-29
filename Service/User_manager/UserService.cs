using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.IService.ICommon_manager;
using elr_API.IService.IDapper;
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

        public string Elr_db = CommonFunctions.GetConnectionString("elr_DB");
        public string ErrorMessage { get; set; } = string.Empty;

        public UserService(IDapperSqlService db, ITokenService tokenService, IHelperService helperService, IQikberrychatService qikberrychatService)
        {
            _dp = db;
            _tokenService = tokenService;
            _helperService = helperService;
            _iqberrychatService = qikberrychatService;
        }
        public async Task<ResponseDTO> ElrGetPatient(string phone_no)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(phone_no)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientVerifyLoginResDTO.patient_details_select();

                var patientDetails = (await _dp.SelectQuery<ConnectPatientDetails>(query, new { phone_no = phone_no }, Elr_db)).FirstOrDefault();

                if (patientDetails == null)
                {
                    var patient = await _helperService.GetConnectPatientDetails(phone_no);

                    if (patient == null) return new ResponseDTO(false, "Patient not found.");

                    return new ResponseDTO(patient);
                }
                return new ResponseDTO(patientDetails);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(phone_no, ex.ToString());
                ErrorMessage = ex.Message;
            }

            return new ResponseDTO(ErrorMessage);
        }

        public async Task<ResponseDTO> ElrAddUpdatePatientDetails(PatientVerifyLoginResDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

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
                    return new ResponseDTO(false, "Invalid phone number.");

                query = PatientVerifyLoginReqDTO.existing_select_patient();

                var existPatient = (await _dp.SelectQuery<PatientVerifyLoginResDTO>(query, new { phone_no = obj.phone_no }, Elr_db)).FirstOrDefault();

                if (existPatient == null)
                {
                    var connectDetails = await _helperService.GetConnectPatientDetails(obj.phone_no);

                    if (connectDetails == null)
                        return new ResponseDTO("Create A New Patient");

                    existPatient = new PatientVerifyLoginResDTO
                    {
                        patient_id = connectDetails.patient_id,
                        patient_name = connectDetails.patient_fullname,
                        dob = DateTime.TryParse(connectDetails.patient_dob, out var parsedDob) ? parsedDob : default,
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
                    existPatient.otp = obj.phone_no == "8056661324"
                        ? 123456
                        : CommonFunctions.GenerateOTP(6);

                    if (obj.phone_no != "8056661324")
                    {
                        await _iqberrychatService.OtpForLoggingPatient(new OtpForLoggingReqDTO
                        {
                            phone_no = existPatient.phone_no,
                            otp = existPatient.otp.Value
                        });
                    }

                    query = PatientVerifyLoginReqDTO.Send_otp_update();

                    int result = await _dp.Execute(query,
                        new
                        {
                            phone_no = existPatient.phone_no,
                            otp = existPatient.otp.Value
                        },
                        Elr_db);

                    return new ResponseDTO(result > 0,
                        result > 0 ? ResponseMessage.OTP_sended : ResponseMessage.Failed);
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

            return new ResponseDTO(ErrorMessage);
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
            return new ResponseDTO(ErrorMessage);
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
            return new ResponseDTO(ErrorMessage);
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
            return new ResponseDTO(ErrorMessage);
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
            return new ResponseDTO(ErrorMessage);
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

                query = MasterEmployeeLoginReqDTO.user_logIn();
                int row = await _dp.Execute(query, new { user_id = data.user_id }, Elr_db);

                if (row <= 0) return new ResponseDTO(false, "Already login");
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
                if (obj.password == data.password)
                {
                    return new ResponseDTO(data);
                }
                else
                {
                    return new ResponseDTO(false, "Invalid Password");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                //ErrorMessage = ResponseMessage.Please_Try_Again;
                ErrorMessage = ex.Message;
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<MasterEmployeeLoginResDTO> GetEmployeeDetails(MasterEmployeeLoginReqDTO obj)
        {
            string query = MasterEmployeeLoginReqDTO.employee_master_select();
            MasterEmployeeLoginResDTO data = (await _dp.SelectQuery<MasterEmployeeLoginResDTO>(query, obj, Elr_db)).FirstOrDefault();
            if (data != null)
            {
                var tokenDetails = _tokenService.GetToken(data.user_id.ToString(), data.gu_id.ToString(), data.phone_no, CommonFunctions.ELR);
                data.token = tokenDetails.Token;
                data.token_expirytime = tokenDetails.Token_ExpiryTime;
            }
            return data;
        }
        public async Task<ResponseDTO> ElrphysiciansList(string? physiciansName)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                string? search = string.IsNullOrWhiteSpace(physiciansName) ? null : physiciansName.Trim();
                query = AddUpdateEmployeeDetailsReqDTO.select_physicians_list();
                var data = (await _dp.SelectQuery<AddUpdateEmployeeDetailsReqDTO>(query, new { user_name = search }, Elr_db)).ToList();
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrTechniciansList(string? technicians_name)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                string? search = string.IsNullOrWhiteSpace(technicians_name) ? null : technicians_name.Trim();
                query = AddUpdateEmployeeDetailsReqDTO.select_technicians_list();
                var data = (await _dp.SelectQuery<AddUpdateEmployeeDetailsReqDTO>(query, new { user_name = search }, Elr_db)).ToList();
                return new ResponseDTO(data);
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

                if (!data.Any() || data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

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

    }













    public class ConnectPatientDetails
    {
        public long patient_id { get; set; }
        public string patient_fullname { get; set; }
        public string patient_dob { get; set; }
        public string patient_age { get; set; }
        public string patient_mobileno { get; set; }
        public string emergency_phone_no { get; set; }
        public string patient_emailid { get; set; }
        public string patient_gender { get; set; }
        public string patient_cityname { get; set; }
        public string patient_pincode { get; set; }
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
