using elr_API.Common;
using elr_API.DTO.ReqDTO.EnrolmentReq;
using elr_API.DTO.ReqDTO.QikberrychatReq;
using elr_API.DTO.ResDTO;
using elr_API.IService.ICommon_manager;
using elr_API.IService.IDapper;
using elr_API.IService.IEntrolement;

namespace elr_API.Service.Enrolment
{
    public class Enrollment : IEnrollment
    {
        private readonly IDapperSqlService _db;
        private readonly IQikberrychatService _iqberrychatService;

        public string Elr_db = CommonFunctions.GetConnectionString("elr_db");
        public string ErrorMessage { get; set; } = string.Empty;
        public Enrollment(IDapperSqlService db, IQikberrychatService iqberrychatService)
        {
            _db = db;
            _iqberrychatService = iqberrychatService;
        }


        public async Task<ResponseDTO> PatientVerifyLogin(PatientVerifyLoginReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || string.IsNullOrEmpty(obj.phone_no) || !Validation.IsValidPhoneNumber(obj.phone_no))
                    return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientVerifyLoginReqDTO.existing_select_patient(obj);

                var existPatient = (await _db.SelectQuery<PatientVerifyLoginResDTO>(query, null, Elr_db)).FirstOrDefault();

                if (existPatient == null && obj.otp == 0)
                    return new ResponseDTO("Create A New Patient");
                else
                {
                    if (obj.otp == 0)
                    {
                        existPatient.otp = CommonFunctions.GenerateOTP(6);

                        //For the Playstore console Demo Login purpuse we use common number
                        if (obj.phone_no == "8056661324") obj.otp = 123456;

                        OtpForLoggingReqDTO otpForLoggingReqDTO = new OtpForLoggingReqDTO
                        {
                            phone_no = existPatient.phone_no,
                            otp = existPatient.otp
                        };
                        await _iqberrychatService.OtpForLoggingPatient(otpForLoggingReqDTO);

                        query = PatientVerifyLoginReqDTO.Send_otp_update(existPatient);

                        int a = await _db.Execute(query, new { phone_no = existPatient.phone_no, otp = existPatient.otp }, Elr_db);

                        return new ResponseDTO(a > 0, a > 0 ? ResponseMessage.OTP_sended : ResponseMessage.Failed);
                    }

                    if (existPatient == null && obj.otp != 0) return new ResponseDTO(false, ResponseMessage.Invalid_OTP);

                    else
                    {
                        if (existPatient.expiry_date < DateTime.Now)
                        {
                            return new ResponseDTO(false, ResponseMessage.OTP_is_Expiry);
                        }
                        else
                        {
                            return new ResponseDTO(existPatient);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(ErrorMessage);
        }

        public async Task<ResponseDTO> UploadEnrollementRawData(EnrollmentRawDataDTO obj)
        {
            ErrorMessage = string.Empty;
            string qeury = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (string.IsNullOrEmpty(obj.file_name.FileName)) return new ResponseDTO(false, ResponseMessage.File_Not_Uploaded);

                if (obj.file_name != null && obj.file_name.Length > 0)
                {
                    var extention = Path.GetExtension(obj.file_name.FileName).ToLowerInvariant();
                    if (extention != ".json") return new ResponseDTO(false, "Only JSON files are allowed.");

                    var fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{extention}";

                    var folderName = CommonFunctions.FolderPath($"EnrollmentFiles\\Patient_{obj.patient_enrolment_id}");

                    var path = await CommonFunctions.UploadFile(obj.file_name, fileName, folderName);

                    obj.file_path = path;
                }
                if (obj.enrolment_raw_data_id == 0)
                {
                    obj.waiting_datetime = DateTime.Now;
                    qeury = EnrollmentRawDataDTO.insert_raw_data();
                    int a = await _db.Execute(qeury, obj, Elr_db);
                    return new ResponseDTO(a > 0, a > 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Failed);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> GetRawDataList()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = EnrollmentRawDataDTO.get_raw_data();
                var data = (await _db.SelectQuery<EnrollmentRawDataDTO>(query, null, Elr_db)).FirstOrDefault();

                if (data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                var param = new
                {
                    enrolment_raw_data_id = data.enrolment_raw_data_id,
                    auto_check_status = "checking",
                    checking_datetime = DateTime.Now
                };

                query = EnrollmentRawDataDTO.update_auto_check_status();
                await _db.Execute(query, param, Elr_db);

                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> UpdateRawDataCheckStatus(EnrollmentRawDataDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.enrolment_raw_data_id == 0) return new ResponseDTO(false, "Invalid  enrolment_raw_data_id");

                var status = obj.auto_check_status.ToLower();
                if (status != "completed") return new ResponseDTO(false, "Only completed status allowed.");

                var param = new
                {
                    enrolment_raw_data_id = obj.enrolment_raw_data_id,
                    auto_check_status = status,
                    completed_datetime = DateTime.Now
                };
                query = EnrollmentRawDataDTO.update_auto_check_status();
                int a = await _db.Execute(query, param, Elr_db);

                return new ResponseDTO(a > 0, a > 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Failed);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<ResponseDTO> AddUpdateQueuedEvent(AddUpdateQueuedEventDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (string.IsNullOrEmpty(obj.file_name.FileName)) return new ResponseDTO(false, ResponseMessage.File_Not_Uploaded);

                if (obj.file_name != null)
                {
                    var extention = Path.GetExtension(obj.file_name.FileName).ToLowerInvariant();
                    if (extention != ".json") return new ResponseDTO(false, "Only JSON files are allowed.");

                    var fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{extention}";
                    var folderName = CommonFunctions.FolderPath($"EventFiles\\Event_{obj.queued_events_records_id}");
                    var path = await CommonFunctions.UploadFile(obj.file_name, fileName, folderName);
                    obj.file_path = path;
                }
                if (obj.queued_events_records_id == 0)
                {
                    obj.entry_datetime = DateTime.Now;
                    obj.event_status = "pending";
                    query = AddUpdateQueuedEventDTO.insert_queued_event();
                    int a = await _db.Execute(query, obj, Elr_db);
                    return new ResponseDTO(a > 0, a > 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Failed);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<ResponseDTO> GetQueuedEvent(AddUpdateQueuedEventDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = AddUpdateQueuedEventDTO.get_queued_event(obj);
                var (totalCount, data) = await _db.SelectQueryWithCount<long, AddUpdateQueuedEventDTO>(query, obj, Elr_db);
                var response = new
                {
                    totalCount = totalCount,
                    items = data
                };
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

    }
}
