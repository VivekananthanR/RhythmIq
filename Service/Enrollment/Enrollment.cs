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
        public async Task<ResponseDTO> ElrUploadPatientDevice(PatientEnrollmentDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if ((obj.hookup_start_device_file == null || obj.hookup_start_device_file.Length == 0) && (obj.hookup_end_device_file == null || obj.hookup_end_device_file.Length == 0))
                {
                    return new ResponseDTO(false, ResponseMessage.File_Not_Uploaded);
                }

                if (obj.hookup_start_device_file != null && obj.hookup_start_device_file.Length > 0)
                {
                    var extension = Path.GetExtension(obj.hookup_start_device_file.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(extension)) extension = ".jpg";
                    var fileName = $"{obj.patient_enrollment_id}_start{extension}";
                    var fileFolder = CommonFunctions.FolderPath("HookupDevice\\HookupStart");
                    var path = await CommonFunctions.UploadFile(obj.hookup_start_device_file, fileName, fileFolder);
                    obj.hookup_start_device_img = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }
                if (obj.hookup_end_device_file != null && obj.hookup_end_device_file.Length > 0)
                {
                    var extension = Path.GetExtension(obj.hookup_end_device_file.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(extension)) extension = ".jpg";
                    var fileName = $"{obj.patient_enrollment_id}_end{extension}";
                    var fileFolder = CommonFunctions.FolderPath("HookupDevice\\HookupEnd");
                    var path = await CommonFunctions.UploadFile(obj.hookup_end_device_file, fileName, fileFolder);
                    obj.hookup_end_device_img = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }
                query = PatientEnrollmentDTO.device_img_upload();
                var result = await _db.Execute(query, new
                {
                    obj.hookup_start_device_img,
                    obj.hookup_end_device_img,
                    obj.patient_enrollment_id
                }, ELR_db);

                if (result > 0)
                {
                    if (!string.IsNullOrEmpty(obj.hookup_start_device_img))
                        return new ResponseDTO(true, "Hookup start device uploaded successfully");

                    if (!string.IsNullOrEmpty(obj.hookup_end_device_img))
                        return new ResponseDTO(true, "Hookup end device uploaded successfully");
                }

                return new ResponseDTO(false, ResponseMessage.File_Not_Uploaded);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrUploadSignature(PatientEnrollmentDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj.patient_enrollment_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if ((obj.sign_patient_file == null || obj.sign_patient_file.Length == 0) && (obj.sign_field_emp_file == null || obj.sign_field_emp_file.Length == 0)) return new ResponseDTO(false, ResponseMessage.File_Not_Uploaded);

                if (obj.sign_patient_file != null || obj.sign_patient_file.Length > 0)
                {
                    var extension = Path.GetExtension(obj.sign_patient_file.FileName).ToLowerInvariant();
                    if (string.IsNullOrWhiteSpace(extension)) extension = ".jpg";
                    var fileName = $"{obj.patient_enrollment_id}-{DateTime.Now:yyyyMMddHHmmssfff}{extension}";
                    var fileFolder = CommonFunctions.FolderPath("Signature\\Patient");
                    var path = await CommonFunctions.UploadFile(obj.sign_patient_file, fileName, fileFolder);
                    obj.sign_patient_img = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }
                if (obj.sign_field_emp_file != null && obj.sign_field_emp_file.Length > 0)
                {
                    var extension = Path.GetExtension(obj.sign_field_emp_file.FileName).ToLowerInvariant();
                    if (string.IsNullOrWhiteSpace(extension)) extension = ".jpg";
                    var fileName = $"{obj.sign_field_emp_file}-{DateTime.Now:yyyyMMddHHmmssfff}{extension}";
                    var fileFolder = CommonFunctions.FolderPath("Signature\\FieldEmp");
                    var path = await CommonFunctions.UploadFile(obj.sign_patient_file, fileName, fileFolder);
                    obj.sign_field_emp_img = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }

                query = PatientEnrollmentDTO.update_signature();

                var result = await _db.Execute(query, new
                {
                    obj.patient_enrollment_id,
                    obj.sign_patient_img,
                    obj.sign_field_emp_img
                }, ELR_db);

                string message = !string.IsNullOrEmpty(obj.sign_patient_img) ? "Patient signature uploaded successfully" : "Field employee signature uploaded successfully";

                return new ResponseDTO(result > 0, result > 0 ? message : "Failed to upload signature");
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

        public async Task<ResponseDTO> ElrGetRawDataList()
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
