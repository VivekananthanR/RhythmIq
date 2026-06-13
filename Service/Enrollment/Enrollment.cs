using elr_API.Common;
using elr_API.DTO.ReqDTO;
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

        public string ELR_db = CommonFunctions.GetConnectionString("elr_db");
        public string ErrorMessage = string.Empty;
        public Enrollment(IDapperSqlService db, IQikberrychatService iqberrychatService)
        {
            _db = db;
            _iqberrychatService = iqberrychatService;
        }

        public async Task<ResponseDTO> ElrGetValidEnrollmentId(long patient_enrollment_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (patient_enrollment_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (patient_enrollment_id > 0) query = PatientEnrollmentReqDTO.chack_valid_enrollment_id();

                var result = (await _db.SelectQuery<PatientEnrollmentReqDTO>(query, new { patient_enrollment_id = patient_enrollment_id }, ELR_db)).FirstOrDefault();

                return result != null ? new ResponseDTO(true, ResponseMessage.Success) : new ResponseDTO(false, ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrValidDeviceId(string device_no)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(device_no)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (device_no.Any()) query = DeviceDTO.check_valid_device();
                var result = (await _db.SelectQuery<DeviceDTO>(query, new { device_no = device_no }, ELR_db)).FirstOrDefault();
                return result != null ? new ResponseDTO(true, ResponseMessage.Success) : new ResponseDTO(false, ResponseMessage.Failed);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrValidPatchId(string patch_no)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(patch_no)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (patch_no.Any()) query = PatchDTO.check_valid_patch();
                var result = (await _db.SelectQuery<PatchDTO>(query, new { patch_no = patch_no }, ELR_db)).FirstOrDefault();
                return result != null ? new ResponseDTO(true, ResponseMessage.Success) : new ResponseDTO(false, ResponseMessage.Failed);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> GetPatientEnrollment(PatientEnrollmentDTO obj)
        {
            ErrorMessage = string.Empty;
            try { return new ResponseDTO(await PatientEnrollmentList(obj)); }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrUploadPatientSignature(PatientEnrollmentDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj.patient_enrollment_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.signature_file == null || obj.signature_file.Length == 0) return new ResponseDTO(false, ResponseMessage.File_Not_Uploaded);
                var extention = Path.GetExtension(obj.signature_file.FileName).ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(extention)) extention = ".jpg";
                var fileName = $"{obj.patient_enrollment_id}-{DateTime.Now:yyyyMMddHHmmssfff}{extention}";
                var fileFolder = CommonFunctions.FolderPath("Signature");
                var path = await CommonFunctions.UploadFile(obj.signature_file, fileName, fileFolder);
                obj.signature_img = path.Replace(Directory.GetCurrentDirectory() + "\\", "");

                query = PatientEnrollmentDTO.update_signature();
                await _db.Execute(query, new { patient_enrollment_id = obj.patient_enrollment_id, signature_img = obj.signature_img }, ELR_db);
                return new ResponseDTO(true, ResponseMessage.File_Uploaded);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrStartHookUpPatient(long patient_enrollment_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (patient_enrollment_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                query = PatientEnrollmentDTO.start_hookup_status(patient_enrollment_id);
                int row = await _db.Execute(query, new { patient_enrollment_id = patient_enrollment_id }, ELR_db);

                return new ResponseDTO((row > 0), row > 0 ? ResponseMessage.Saved_Successfully : "Patient Enrollment ID not found");
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrCompleteHookUpPatient(long patient_enrollment_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (patient_enrollment_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                query = PatientEnrollmentDTO.complete_hookup_status(patient_enrollment_id);
                int row = await _db.Execute(query, new { patient_enrollment_id = patient_enrollment_id }, ELR_db);

                return new ResponseDTO((row > 0), row > 0 ? ResponseMessage.Success : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<List<PatientEnrollmentDTO>> PatientEnrollmentList(PatientEnrollmentDTO obj)
        {
            string query = PatientEnrollmentDTO.select_enrollment();
            return (await _db.SelectQuery<PatientEnrollmentDTO>(query, obj, ELR_db)).ToList();
        }

        public async Task<ResponseDTO> ElrAddUpdatePatientEnrollment(PatientEnrollmentDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.patient_enrollment_id == 0) query = PatientEnrollmentDTO.insert_enrollment();
                else query = PatientEnrollmentDTO.update_enrollment();

                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.patient_enrollment_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrUploadEnrollmentRawData(EnrollmentRawDataDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (string.IsNullOrEmpty(obj.file_name.FileName)) return new ResponseDTO(false, ResponseMessage.File_Not_Uploaded);

                if (obj.file_name != null && obj.file_name.Length > 0)
                {
                    var extention = Path.GetExtension(obj.file_name.FileName).ToLowerInvariant();
                    if (extention != ".json") return new ResponseDTO(false, "Only JSON files are allowed.");

                    var fileName = $"{obj.patient_enrollment_id}-{DateTime.Now:yyyyMMddHHmmssfff}{extention}";

                    var folderName = CommonFunctions.FolderPath($"EnrollmentFiles\\Patient_{obj.patient_enrollment_id}");

                    var path = await CommonFunctions.UploadFile(obj.file_name, fileName, folderName);

                    obj.file_path = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }
                if (obj.enrollment_raw_data_id == 0)
                {
                    obj.waiting_datetime = DateTime.Now;

                    query = EnrollmentRawDataDTO.insert_raw_data();
                    int a = await _db.Execute(query, obj, ELR_db);

                    if (a > 0)
                    {
                        query = PatientEnrollmentDTO.monitoring_hookup_status(obj.patient_enrollment_id);
                        await _db.Execute(query, new { patient_enrollment_id = obj.patient_enrollment_id }, ELR_db);
                    }

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
                query = EnrollmentRawDataDTO.status_waiting_list();
                var data = (await _db.SelectQuery<EnrollmentRawDataDTO>(query, null, ELR_db)).FirstOrDefault();

                if (data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                var param = new
                {
                    enrollment_raw_data_id = data.enrollment_raw_data_id,
                    auto_check_status = "checking",
                    checking_datetime = DateTime.Now
                };

                query = EnrollmentRawDataDTO.update_status();
                await _db.Execute(query, param, ELR_db);

                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

    }
}
