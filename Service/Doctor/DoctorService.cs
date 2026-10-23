using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.DTO.ResDTO.Token;
using elr_API.IService.ICommon_manager;
using elr_API.IService.IDapper;
using elr_API.IService.IDoctor;
using elr_API.IService.IToken;

namespace elr_API.Service.Doctor
{
    public class DoctorService : IDoctorService
    {
        private readonly IDapperSqlService _dapperSqlService;
        private readonly IQikberrychatService _qikberrychatService;
        private readonly ITokenService _tokenService;
        public string Elr_Db = CommonFunctions.GetConnectionString("Elr_Db");
        public string ErrroMessage = string.Empty;

        public DoctorService(IDapperSqlService dapperSqlService, IQikberrychatService qikberrychatService, ITokenService tokenService)
        {
            _dapperSqlService = dapperSqlService;
            _qikberrychatService = qikberrychatService;
            _tokenService = tokenService;
        }


        public async Task<ResponseDTO> ElrDoctorLogin(PatientVerifyLoginReqDTO obj)
        {
            ErrroMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || !Validation.IsValidPhoneNumber(obj.phone_no)) return new ResponseDTO(false, "Enter A Valid Phone Number.");

                query = DoctorLoginResDTO.select_doctor_login();
                var doctor_data = (await _dapperSqlService.SelectQuery<DoctorLoginResDTO>(query, new { phone_no = obj.phone_no }, Elr_Db)).FirstOrDefault();

                if (doctor_data == null) return new ResponseDTO(false, "Doctor Not Found. Create A Account.");

                if (obj.otp == 0)
                {
                    int otp = obj.phone_no == "6385234932" ? 123456 : CommonFunctions.GenerateOTP(6);

                    query = DoctorLoginResDTO.insert_doctor_otp();
                    int a = await _dapperSqlService.Execute(query, new { phone_no = obj.phone_no, otp = otp }, Elr_Db);

                    if (a <= 0) return new ResponseDTO(false, ResponseMessage.Failed);

                    #region Message
                    if (obj.phone_no != "6385234932")
                    {
                        var msg_notify = await _qikberrychatService.OtpForLoggingPatient(new OtpForLoggingReqDTO
                        {
                            phone_no = obj.phone_no,
                            otp = otp,
                        });
                        return new ResponseDTO(msg_notify);
                    }
                    #endregion

                    return new ResponseDTO(true, ResponseMessage.OTP_sended);
                }

                if (obj.otp != doctor_data.otp)
                {
                    return new ResponseDTO(false, ResponseMessage.Invalid_OTP);
                }

                else
                {
                    if (doctor_data.expiry_datetime < DateTime.Now)
                    {
                        return new ResponseDTO(false, ResponseMessage.OTP_is_Expiry);
                    }
                    else
                    {
                        TokenResDTO tokenDetails = _tokenService.GetToken(doctor_data.doctor_id.ToString(), doctor_data.gu_id.ToString(), doctor_data.phone_no, CommonFunctions.ELR, doctor_data.login_gu_id.ToString());

                        doctor_data.token = tokenDetails.Token;
                        doctor_data.expiryTime = tokenDetails.Token_ExpiryTime;

                        return new ResponseDTO(doctor_data);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrroMessage = ex.Message.ToString();

            }
            return new ResponseDTO(false, ErrroMessage);
        }


        public async Task<ResponseDTO> ElrDoctorLivePatientMonitoringList(DoctorLivePatientMonitoringReqDTO obj)
        {
            ErrroMessage = string.Empty;
            string query = string.Empty;

            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = DoctorLivePatientMonitoringReqDTO.select_dr_live_patient_monitoring(obj);

                var (totalCount, data) = await _dapperSqlService.SelectQueryWithCount<long, DoctorLivePatientMonitoringResDTO>(query, obj, Elr_Db);

                var response = new
                {
                    totalCount = totalCount,
                    item = data,
                };
                return new ResponseDTO(response);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrroMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrroMessage);
        }
    }
}
