using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.IService.ICommon_manager;
using elr_API.IService.IDapper;
using elr_API.IService.IEntrolement;
using System.Text.Json;

namespace elr_API.Service.Enrolment
{
    public class Enrollment : IEnrollment
    {
        private readonly IDapperSqlService _db;
        private readonly IQikberrychatService _iqberrychatService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public string ELR_db = CommonFunctions.GetConnectionString("elr_DB");
        public string ErrorMessage = string.Empty;
        public Enrollment(IDapperSqlService db, IQikberrychatService iqberrychatService, IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _db = db;
            _iqberrychatService = iqberrychatService;
            _httpClientFactory = httpClientFactory;
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        public async Task<ResponseDTO> ElrGetValidEnrollmentId(long patient_enrollment_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (patient_enrollment_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientEnrollmentReqDTO.check_enrollment_exists();
                var data = (await _db.SelectQuery<RequestPatientEnrollmentResDTO>(query, new { patient_enrollment_id = patient_enrollment_id }, ELR_db)).FirstOrDefault();

                if (data == null)
                {
                    return new ResponseDTO(false, "Patient Enrollment Id Not Found. Device hookup cannot be assigned");
                }
                else
                {
                    return new ResponseDTO(data);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrEmployeeDeviceAssignmentList(long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {

                query = EmployeeDeviceListDTO.select_device();
                var (totalCount, data) = await _db.SelectQueryWithCount<long, EmployeeDeviceListDTO>(query, new { current_emp_id = emp_id }, ELR_db);

                var rsponse = new
                {
                    totalCount = totalCount,
                    item = data,

                };
                return new ResponseDTO(rsponse);
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
                if (string.IsNullOrWhiteSpace(device_no)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = DeviceResDTO.check_valid_device();
                var deviceData = (await _db.SelectQuery<DeviceResDTO>(query, new { device_no = device_no }, ELR_db)).FirstOrDefault();

                if (deviceData == null) return new ResponseDTO(false, "Invalid Device Number.");

                if (deviceData.device_status != "active") return new ResponseDTO(false, $"Device is currently {deviceData.device_status}. It cannot be assigned.");

                return new ResponseDTO(true, ResponseMessage.Success);

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
                if (string.IsNullOrWhiteSpace(patch_no)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatchResDTO.check_valid_patch();
                var patchData = (await _db.SelectQuery<PatchResDTO>(query, new { patch_no = patch_no }, ELR_db)).FirstOrDefault();

                if (patchData == null) return new ResponseDTO(false, "Invalid Patch Number");

                if (patchData.patch_status != "active") return new ResponseDTO(false, $"Patch is currently {patchData.patch_status}. It cannot be assigned.");

                return new ResponseDTO(true, ResponseMessage.Success);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrEnrollmentCreationList(GetPatientEnrollmentListReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = RequestPatientEnrollmentResDTO.select_request_patient_enrollment();

                var parameters = new
                {
                    enrollment_status = JsonSerializer.Serialize(obj.enrollment_status ?? new List<string>()),
                    search = obj.search,
                    date = obj.date
                };

                var (totalCount, data) = await _db.SelectQueryWithCount<long, RequestPatientEnrollmentResDTO>(query, parameters, ELR_db);

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

                if (obj.sign_patient_file != null && obj.sign_patient_file.Length > 0)
                {
                    var extension = Path.GetExtension(obj.sign_patient_file.FileName)?.ToLowerInvariant();
                    if (string.IsNullOrWhiteSpace(extension)) extension = ".jpg";

                    var fileName = $"{obj.patient_enrollment_id}-patient-sign{extension}";
                    var fileFolder = CommonFunctions.FolderPath("Signature\\Patient");

                    var path = await CommonFunctions.UploadFile(obj.sign_patient_file, fileName, fileFolder);

                    obj.sign_patient_img = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }
                if (obj.sign_field_emp_file != null && obj.sign_field_emp_file.Length > 0)
                {
                    var extension = Path.GetExtension(obj.sign_field_emp_file.FileName)?.ToLowerInvariant();
                    if (string.IsNullOrWhiteSpace(extension)) extension = ".jpg";

                    var fileName = $"{obj.patient_enrollment_id}-fieldemp-sign{extension}";
                    var fileFolder = CommonFunctions.FolderPath("Signature\\FieldEmp");

                    var path = await CommonFunctions.UploadFile(obj.sign_field_emp_file, fileName, fileFolder);

                    obj.sign_field_emp_img = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }

                query = PatientEnrollmentDTO.update_signature();

                var result = await _db.Execute(query, new
                {
                    obj.patient_enrollment_id,
                    obj.sign_patient_img,
                    obj.sign_field_emp_img
                }, ELR_db);

                string message;

                if (!string.IsNullOrEmpty(obj.sign_patient_img) && !string.IsNullOrEmpty(obj.sign_field_emp_img))
                    message = "Both signatures uploaded successfully";

                else if (!string.IsNullOrEmpty(obj.sign_patient_img))
                    message = "Patient signature uploaded successfully";

                else
                    message = "Field employee signature uploaded successfully";

                return new ResponseDTO(result > 0, result > 0 ? message : "Failed to upload signature");
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrStartHookUpPatient(PatientEnrollmentReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || string.IsNullOrWhiteSpace(obj.device_no) || obj.patient_enrollment_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientEnrollmentDTO.check_enrollment_status();
                var enrollmentData = (await _db.SelectQuery<PatientEnrollmentDTO>(query, new { patient_enrollment_id = obj.patient_enrollment_id }, ELR_db)).FirstOrDefault();

                if (enrollmentData == null) return new ResponseDTO(false, $"Patient enrollment id {obj.patient_enrollment_id} not found.");

                query = DeviceResDTO.get_device();
                var deviceData = (await _db.SelectQuery<DeviceResDTO>(query, new { device_no = obj.device_no }, ELR_db)).FirstOrDefault();

                if (deviceData == null) return new ResponseDTO(false, $"Patient enrollment id {obj.patient_enrollment_id} is already assigned to device {obj.device_no}.");

                bool is_patient_hookuped;

                if (!string.IsNullOrEmpty(obj.emp_code))
                {
                    query = PatientEnrollmentReqDTO.get_user_id_by_emp_code();
                    obj.created_by = (await _db.SelectQuery<long>(query, new { emp_code = obj.emp_code.ToLower() }, ELR_db)).FirstOrDefault();

                    if (obj.created_by <= 0) return new ResponseDTO(false, "Invalid employee code.");
                    is_patient_hookuped = false;
                }
                else
                {
                    query = PatientEnrollmentReqDTO.get_pateint_id_emp_code();
                    obj.created_by = (await _db.SelectQuery<long>(query, new { patient_enrollment_id = obj.patient_enrollment_id }, ELR_db)).FirstOrDefault();
                    if (obj.created_by <= 0) return new ResponseDTO(false, "Pateint id not found.");
                    is_patient_hookuped = true;
                }
                if (enrollmentData.enrollment_status != "requested") return new ResponseDTO(false, $"Patient enrollment is already {enrollmentData.enrollment_status}.");

                query = DeviceAssignmentResDTO.insert_device_assignment_history();

                int assignmentRow = await _db.Execute(
                    query,
                    new
                    {
                        device_id = deviceData.device_id,
                        emp_id = deviceData.current_emp_id,
                        patient_enrollment_id = obj.patient_enrollment_id,
                        assignment_status = "assigned",
                        remarks = "Device assigned to patient during hookup",
                        created_by = obj.created_by
                    },
                    ELR_db);

                if (assignmentRow <= 0) return new ResponseDTO(false, $"Device {obj.device_no} is not available for hookup.");

                query = PatientEnrollmentDTO.start_hookup_status();
                int row = await _db.Execute(query,
                    new
                    {
                        patient_enrollment_id = obj.patient_enrollment_id,
                        device_id = deviceData.device_id,
                        actual_technician_id = obj.created_by,
                        is_patient_hookuped = is_patient_hookuped,
                        enrollment_status = "hookuped",
                    }, ELR_db);

                if (row <= 0) return new ResponseDTO(false, "Patient hookup started failed.");

                query = DeviceResDTO.update_device_status();
                await _db.Execute(query, new { device_id = deviceData.device_id, device_status = "engaged" }, ELR_db);

                return new ResponseDTO(true, "Patient hookup started successfully.");

            }

            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrSymtomsList(long patient_enrollment_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = SymptomDTO.check_patient_enrollment_id();
                long enrollmentId = (await _db.SelectQuery<long>(query, new { patient_enrollment_id = patient_enrollment_id }, ELR_db)).FirstOrDefault();

                if (enrollmentId <= 0) return new ResponseDTO(false, $"Patient enrollment id {patient_enrollment_id} not found.");

                query = SymtomsDTO.select_symtoms_list();
                var data = await _db.SelectQuery<SymtomsDTO>(query, new { patient_enrollment_id = enrollmentId }, ELR_db);

                return new ResponseDTO(data);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrAddSymtoms(SymtomsDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || obj.patient_enrollment_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.symptoms_id == 0)
                {
                    query = SymtomsDTO.insert_symtoms();
                    var result = await _db.Execute(query, obj, ELR_db);
                    return new ResponseDTO(result > 0, result > 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Failed);
                }

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrCompleteHookUpPatient(PatientEnrollmentReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj.patient_enrollment_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                bool is_patient_removal;

                if (!string.IsNullOrEmpty(obj.emp_code))
                {
                    query = PatientEnrollmentReqDTO.get_user_id_by_emp_code();
                    obj.created_by = (await _db.SelectQuery<long>(query, new { emp_code = obj.emp_code.ToLower() }, ELR_db)).FirstOrDefault();

                    if (obj.created_by <= 0) return new ResponseDTO(false, "Invalid employee code.");
                    is_patient_removal = false;
                }
                else
                {
                    query = PatientEnrollmentReqDTO.get_pateint_id_emp_code();

                    obj.created_by = (await _db.SelectQuery<long>(query, new { patient_enrollment_id = obj.patient_enrollment_id }, ELR_db)).FirstOrDefault();

                    if (obj.created_by <= 0) return new ResponseDTO(false, "Patient id not found.");

                    is_patient_removal = true;
                }

                query = PatientEnrollmentDTO.check_enrollment_status();

                var enrollmentData = (await _db.SelectQuery<PatientEnrollmentDTO>(query, new { patient_enrollment_id = obj.patient_enrollment_id }, ELR_db)).FirstOrDefault();

                if (enrollmentData == null) return new ResponseDTO(false, $"Patient enrollment id {obj.patient_enrollment_id} not found.");

                if (string.Equals(enrollmentData.enrollment_status, "completed", StringComparison.OrdinalIgnoreCase)) return new ResponseDTO(false, $"Patient enrollment id {obj.patient_enrollment_id} is already completed.");

                query = DeviceResDTO.get_device_by_enrollment();

                var deviceData = (await _db.SelectQuery<DeviceResDTO>(query, new { patient_enrollment_id = obj.patient_enrollment_id }, ELR_db)).FirstOrDefault();


                if (deviceData == null || deviceData.device_id <= 0) return new ResponseDTO(false, $"Processing device not found for patient enrollment id {obj.patient_enrollment_id}.");


                if (deviceData.current_emp_id <= 0) return new ResponseDTO(false, $"Employee not found for device {deviceData.device_id}.");

                query = PatientEnrollmentDTO.complete_hookup_status(obj.patient_enrollment_id);
                int row = await _db.Execute(query, new { patient_enrollment_id = obj.patient_enrollment_id, modified_by = obj.created_by, is_patient_removal = is_patient_removal }, ELR_db);

                if (row == 0) return new ResponseDTO(false, $"Failed to complete hookup status for patient enrollment id {obj.patient_enrollment_id}.");

                query = DeviceAssignmentResDTO.insert_device_assignment_history();

                int assignmentRow = await _db.Execute(
                    query,
                    new
                    {
                        device_id = deviceData.device_id,
                        emp_id = deviceData.current_emp_id,
                        patient_enrollment_id = obj.patient_enrollment_id,
                        assignment_status = "completed",
                        remarks = "Patient hookup completed",
                        created_by = obj.created_by
                    },
                    ELR_db);

                if (assignmentRow <= 0) return new ResponseDTO(false, $"Failed to complete device assignment for patient enrollment id {obj.patient_enrollment_id}.");

                query = DeviceResDTO.update_device_status_active();
                await _db.Execute(query, new { device_id = deviceData.device_id }, ELR_db);


                return new ResponseDTO(true, "Patient hookup completed successfully.");
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrAddUpdatePatientEnrollment(PatientEnrollmentDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientEnrollmentDTO.get_patient_details();
                obj.patient_id = (await _db.SelectQuery<long>(query, new { patient_id = obj.patient_id }, ELR_db)).FirstOrDefault();

                if (obj.patient_id == 0) return new ResponseDTO(false, $"Patient details not found for Id.");

                if (obj.patient_enrollment_id == 0) query = PatientEnrollmentDTO.insert_enrollment();
                else
                {
                    obj.modified_by = obj.created_by;
                    query = PatientEnrollmentDTO.update_enrollment();
                }
                int a = await _db.Execute(query, obj, ELR_db);

                return new ResponseDTO(a > 0, a > 0 ? (obj.patient_enrollment_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
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
                    //if (extention != ".json") return new ResponseDTO(false, "Only JSON files are allowed.");

                    var fileName = $"{obj.file_name.FileName}";

                    var folderName = CommonFunctions.FolderPath($"EnrollmentFiles\\{obj.patient_enrollment_id}");

                    var path = await CommonFunctions.UploadFile(obj.file_name, fileName, folderName);

                    obj.file_path = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }
                if (obj.enrollment_raw_data_id == 0)
                {
                    obj.waiting_datetime = DateTime.Now;

                    obj.auto_check_status = "waiting";

                    query = EnrollmentRawDataDTO.insert_raw_data();

                    long enrollmentRawDataId = await _db.ExecuteAndGetId(query, obj, ELR_db);

                    if (enrollmentRawDataId <= 0) return new ResponseDTO(false, ResponseMessage.Failed);

                    query = DeviceResDTO.get_assigned_device_by_enrollment();

                    var deviceData = (await _db.SelectQuery<DeviceResDTO>(query, new { patient_enrollment_id = obj.patient_enrollment_id }, ELR_db)).FirstOrDefault();

                    if (deviceData == null || deviceData.device_id <= 0) return new ResponseDTO(false, $"Assigned device not found for patient enrollment id {obj.patient_enrollment_id}.");

                    //query = DeviceAssignmentResDTO.insert_device_assignment_history();

                    //int assignmentRow = await _db.Execute(
                    //    query,
                    //    new
                    //    {
                    //        device_id = deviceData.device_id,
                    //        emp_id = deviceData.current_emp_id,
                    //        patient_enrollment_id = obj.patient_enrollment_id,
                    //        assignment_status = "processing",
                    //        remarks = "Enrollment raw data uploaded and processing started",
                    //        created_by = obj.created_by
                    //    },
                    //    ELR_db);

                    //if (assignmentRow <= 0) return new ResponseDTO(false, "Raw data uploaded but processing status failed.");

                    return new ResponseDTO(true, ResponseMessage.Saved_Successfully);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task ElrPhythonProcessEnrollmentRawData()
        {
            string query = string.Empty;

            query = EnrollmentRawDataDTO.GetWaitingRecords();

            var records = await _db.SelectQuery<EnrollmentRawDataDTO>(query, null, ELR_db);

            foreach (var item in records)
            {
                try
                {
                    query = EnrollmentRawDataDTO.update_checking_status();

                    var row = await _db.Execute(query, new { enrollment_raw_data_id = item.enrollment_raw_data_id }, ELR_db);

                    if (row == 0)
                    {
                        continue;
                    }

                    string physicalPath = Path.Combine(Directory.GetCurrentDirectory(), item.file_path);


                    if (!File.Exists(physicalPath))
                    {

                        query = EnrollmentRawDataDTO.update_waiting_status();

                        await _db.Execute(query, new { remarks = $"File not found : {physicalPath}", enrollment_raw_data_id = item.enrollment_raw_data_id }, ELR_db);

                        continue;
                    }

                    using var formData = new MultipartFormDataContent();

                    using var fileStream = File.OpenRead(physicalPath);

                    var fileContent = new StreamContent(fileStream);

                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    formData.Add(fileContent, "file", Path.GetFileName(physicalPath));
                    formData.Add(new StringContent(item.patient_enrollment_id.ToString()), "patient_enrollment_id");
                    formData.Add(new StringContent(item.enrollment_raw_data_id.ToString()), "enrollment_raw_data_id");

                    using var client = _httpClientFactory.CreateClient();

                    client.Timeout = TimeSpan.FromSeconds(30);

                    var url = CommonFunctions.DataOnLive() ? _configuration["PythonApi:ProcessUrlLive"] : _configuration["PythonApi:ProcessUrlLocal"];

                    var response = await client.PostAsync(url, formData);

                    string result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        query = EnrollmentRawDataDTO.update_completed_status();

                        await _db.Execute(query, new { enrollment_raw_data_id = item.enrollment_raw_data_id }, ELR_db);

                        query = PatientEnrollmentDTO.monitoring_hookup_status(item.patient_enrollment_id);

                        await _db.Execute(query, new { patient_enrollment_id = item.patient_enrollment_id }, ELR_db);
                    }
                    else
                    {
                        query = EnrollmentRawDataDTO.update_waiting_status();
                        await _db.Execute(query, new
                        {
                            remarks = string.IsNullOrWhiteSpace(result) ? "Python API request failed." : result,
                            enrollment_raw_data_id = item.enrollment_raw_data_id
                        }, ELR_db);
                    }
                }
                catch (Exception ex)
                {
                    await _db.Execute(
                        EnrollmentRawDataDTO.update_waiting_status(),
                        new
                        {
                            enrollment_raw_data_id = item.enrollment_raw_data_id
                        },
                        ELR_db);

                    CommonFunctions.ILogging(item, ex.ToString());

                    continue;
                }
            }
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

                query = EnrollmentRawDataDTO.update_checking_status();
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
