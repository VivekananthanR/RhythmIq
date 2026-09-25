using ClosedXML.Excel;
using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.IService.IDapper;
using elr_API.IService.IHome_manager;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace elr_API.Service.Home_manager
{
    public class HomeService : IHomeService
    {
        private readonly IDapperSqlService _db;
        public string ErrorMessage = string.Empty;
        public string ELR_db = CommonFunctions.GetConnectionString("elr_DB");
        public HomeService(IDapperSqlService db)
        {
            _db = db;
        }
        public async Task<ResponseDTO> ElrGetDevice(DeviceReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = DeviceResDTO.select_device_list();
                var (totalCount, data) = await _db.SelectQueryWithCount<long, DeviceResDTO>(query, obj, ELR_db);
                var response = new
                {
                    totalCount = totalCount,
                    items = data
                };
                return new ResponseDTO(response);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrAddUpdateDevice(DeviceResDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.device_id == 0) query = DeviceResDTO.insert_device();
                else
                {

                    obj.modified_by = obj.created_by;
                    query = DeviceResDTO.update_device();
                }

                int row = await _db.Execute(query, obj, ELR_db);

                return new ResponseDTO(row > 0, row > 0 ? (obj.device_id == 0) ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully : ResponseMessage.Failed);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<ResponseDTO> ElrAddUpdatePatch(PatchResDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.patch_id == 0) query = PatchResDTO.insert_patch();
                else
                {
                    obj.modified_by = obj.created_by;
                    query = PatchResDTO.update_patch();
                }

                int row = await _db.Execute(query, obj, ELR_db);

                return new ResponseDTO(row > 0, row > 0 ? (obj.patch_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> GetMenu()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = MenuDTO.select_menu();
                var data = (await _db.SelectQuery<MenuDTO>(query, null, ELR_db)).ToList();
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> AddUpdateMenu(MenuDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.menu_id == 0) query = MenuDTO.insert_menu();
                else query = MenuDTO.update_menu();
                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? ((obj.menu_id == 0) ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> GetCountry(CountryDTO obj)
        {
            ErrorMessage = string.Empty;
            try { return new ResponseDTO(await CountryList(obj)); }
            catch (Exception ex) { CommonFunctions.ILogging(obj, ex.ToString()); ErrorMessage = ex.Message; }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<List<CountryDTO>> CountryList(CountryDTO obj)
        {
            return (await _db.SelectQuery<CountryDTO>(CountryDTO.select_country(), obj, ELR_db)).ToList();
        }
        public async Task<ResponseDTO> AddUpdateCountry(CountryDTO obj)
        {
            ErrorMessage = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                string query = obj.country_id == 0 ? CountryDTO.insert_country() : CountryDTO.update_country();
                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.country_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex) { CommonFunctions.ILogging(obj, ex.ToString()); ErrorMessage = ex.Message; }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> GetState(StateDTO obj)
        {
            ErrorMessage = string.Empty;
            try { return new ResponseDTO(await StateList(obj)); }
            catch (Exception ex) { CommonFunctions.ILogging(obj, ex.ToString()); ErrorMessage = ex.Message; }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<List<StateDTO>> StateList(StateDTO obj)
        {
            return (await _db.SelectQuery<StateDTO>(StateDTO.select_state(), obj, ELR_db)).ToList();
        }
        public async Task<ResponseDTO> AddUpdateState(StateDTO obj)
        {
            ErrorMessage = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                string query = obj.state_id == 0 ? StateDTO.insert_state() : StateDTO.update_state();
                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.state_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex) { CommonFunctions.ILogging(obj, ex.ToString()); ErrorMessage = ex.Message; }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> GetDistrict(DistrictDTO obj)
        {
            ErrorMessage = string.Empty;
            try { return new ResponseDTO(await DistrictList(obj)); }
            catch (Exception ex) { CommonFunctions.ILogging(obj, ex.ToString()); ErrorMessage = ex.Message; }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<List<DistrictDTO>> DistrictList(DistrictDTO obj)
        {
            return (await _db.SelectQuery<DistrictDTO>(DistrictDTO.select_district(), obj, ELR_db)).ToList();
        }
        public async Task<ResponseDTO> AddUpdateDistrict(DistrictDTO obj)
        {
            ErrorMessage = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                string query = obj.district_id == 0 ? DistrictDTO.insert_district() : DistrictDTO.update_district();
                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.district_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex) { CommonFunctions.ILogging(obj, ex.ToString()); ErrorMessage = ex.Message; }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> GetUserType(UserTypeDTO obj)
        {
            ErrorMessage = string.Empty;
            try
            {
                return new ResponseDTO(await UserTypeList(obj));
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<List<UserTypeDTO>> UserTypeList(UserTypeDTO obj)
        {
            string query = UserTypeDTO.select_user_type();
            var data = (await _db.SelectQuery<UserTypeDTO>(query, obj, ELR_db)).ToList();
            return data;
        }

        public async Task<ResponseDTO> AddUpdateUserType(UserTypeDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.user_type_id == 0) query = UserTypeDTO.insert_user_type();
                else
                {
                    obj.modified_by = obj.created_by;
                    query = UserTypeDTO.update_user_type();
                }
                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.user_type_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrHospitalList(string? hospital_name)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                string? search = string.IsNullOrWhiteSpace(hospital_name) ? null : hospital_name.Trim();
                query = AddUpdateHospitalReqDTO.select_hospital();
                var data = (await _db.SelectQuery<AddUpdateHospitalReqDTO>(query, new { hospital_name = search }, ELR_db)).ToList();

                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        //public async Task<ResponseDTO> GetHospital(HospitalDTO obj)
        //{
        //    ErrorMessage = string.Empty;
        //    try { return new ResponseDTO(await HospitalList(obj)); }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.ILogging(obj, ex.ToString());
        //        ErrorMessage = ex.Message.ToString();
        //    }
        //    return new ResponseDTO(false, ErrorMessage);
        //}

        //public async Task<List<HospitalDTO>> HospitalList(HospitalDTO obj)
        //{
        //    string query = HospitalDTO.select_hospital();
        //    return (await _db.SelectQuery<HospitalDTO>(query, obj, ELR_db)).ToList();
        //}
        public async Task<ResponseDTO> AddUpdateHospital(AddUpdateHospitalReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.hospital_id == 0) query = AddUpdateHospitalReqDTO.insert_hospital();
                else query = AddUpdateHospitalReqDTO.update_hospital();

                var a = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(a > 0, a > 0 ? (obj.hospital_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrGetTest()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = AddUpdateTestDTO.select_test();
                var data = (await _db.SelectQuery<AddUpdateTestDTO>(query, null, ELR_db)).ToList();
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        //public async Task<List<AddUpdateTestDTO>> TestList(AddUpdateTestDTO obj)
        //{
        //    string query = AddUpdateTestDTO.select_test();
        //    var data = (await _db.SelectQuery<AddUpdateTestDTO>(query, obj, ELR_db)).ToList();
        //    return data;
        //}

        public async Task<ResponseDTO> AddUpdateTest(AddUpdateTestDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.test_id == 0) query = AddUpdateTestDTO.insert_test();
                else
                {
                    obj.modified_by = obj.created_by;
                    query = AddUpdateTestDTO.update_test();
                }
                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.test_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrGetTestPrices(AddUpdateTestPricesDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                return new ResponseDTO(await TestPricesList(obj));
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<List<AddUpdateTestPricesDTO>> TestPricesList(AddUpdateTestPricesDTO obj)
        {
            string query = AddUpdateTestPricesDTO.select_test_prices();
            var data = (await _db.SelectQuery<AddUpdateTestPricesDTO>(query, obj, ELR_db)).ToList();
            return data;
        }

        public async Task<ResponseDTO> ElrGetTestPriceByDays(AddUpdateTestPricesDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.duration_in_day < 1) return new ResponseDTO(false, "Days must be at least 1");

                query = AddUpdateTestPricesDTO.select_test_prices();
                var data = (await _db.SelectQuery<AddUpdateTestPricesDTO>(query, obj, ELR_db)).FirstOrDefault();
                if (data == null) return new ResponseDTO(false, "Test price not found");

                if (obj.duration_in_day > data.duration_in_day)
                    obj.duration_in_day = data.duration_in_day;

                data.test_price = data.test_price * obj.duration_in_day;
                data.duration_in_day = obj.duration_in_day;
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> AddUpdateTestPrices(AddUpdateTestPricesDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.test_prices_id == 0) query = AddUpdateTestPricesDTO.insert_test_prices();
                else
                {
                    obj.modified_by = obj.created_by;
                    query = AddUpdateTestPricesDTO.update_test_prices();
                }
                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.test_prices_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrSymtomsTypeList()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = SymtomsTypeDTO.select_symptoms_List();
                var data = await _db.SelectQuery<SymtomsTypeDTO>(query, null, ELR_db);
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<ResponseDTO> GetLocation(LocationDTO obj)
        {
            ErrorMessage = string.Empty;
            try { return new ResponseDTO(await LocationList(obj)); }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<List<LocationDTO>> LocationList(LocationDTO obj)
        {
            string query = LocationDTO.select_location();
            return (await _db.SelectQuery<LocationDTO>(query, obj, ELR_db)).ToList();
        }

        public async Task<ResponseDTO> AddUpdateLocation(LocationDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.location_id == 0) query = LocationDTO.insert_location();
                else query = LocationDTO.update_location();

                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.location_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrUploadProcessedData(ProcessedRawDataDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.event_processed_file != null && obj.event_processed_file.Length > 0)
                {
                    var extention = Path.GetExtension(obj.event_processed_file.FileName).ToLowerInvariant();
                    if (extention != ".json") return new ResponseDTO(false, "Only JSON files are allowed.");

                    var fileName = $"{obj.patient_enrollment_id}-{DateTime.Now:ddMMyyyyHHmmssfff}{extention}";

                    var folderName = CommonFunctions.FolderPath($"ECG-ProcessedData\\{obj.patient_enrollment_id}");

                    var path = await CommonFunctions.UploadFile(obj.event_processed_file, fileName, folderName);

                    obj.event_processed_path = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }
                if (obj.patient_base_line_id == 0)
                {
                    query = ProcessedRawDataDTO.insert_processed_data();
                    var result = await _db.Execute(query, obj, ELR_db);

                    if (result > 0)
                    {
                        query = EnrollmentRawDataDTO.update_completed_status();
                        await _db.Execute(query, new { enrollment_raw_data_id = obj.enrollment_raw_data_id }, ELR_db);
                    }

                    return new ResponseDTO(result > 0, result > 0 ? (obj.patient_base_line_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<ResponseDTO> ElrQueueAnomalyList(QueuedAnomalyReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = QueuedAnomalyReqDTO.queue_anomaly_list(obj);
                var (totalCount, data) = await _db.SelectQueryWithCount<long, QueuedAnomalyDTO>(query, obj, ELR_db);
                //if (data == null || !data.Any()) return new ResponseDTO(false, "No Anomaly Data");
                var response = new
                {
                    totalCount = totalCount,
                    items = data
                };
                return new ResponseDTO(response);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<ResponseDTO> ElrUploadQueueAnomaly(QueuedAnomalyDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.file_name != null && obj.file_name.Length > 0)
                {
                    var extention = Path.GetExtension(obj.file_name.FileName).ToLowerInvariant();
                    if (extention != ".json") return new ResponseDTO(false, "Only JSON files are allowed.");

                    var fileName = $"{obj.patient_enrollment_id}-{DateTime.Now:ddMMyyyyHHmmssfff}{extention}";

                    var folderName = CommonFunctions.FolderPath($"ECG-AnomalyData\\{obj.patient_enrollment_id}");

                    var path = await CommonFunctions.UploadFile(obj.file_name, fileName, folderName);

                    obj.event_file_path = path.Replace(Directory.GetCurrentDirectory() + "\\", "");
                }
                if (obj.queued_events_records_id == 0)
                {
                    obj.event_status = "pending";
                    query = QueuedAnomalyDTO.insert_queued_event();
                    int a = await _db.Execute(query, obj, ELR_db);
                    return new ResponseDTO(true, ResponseMessage.Success);

                }

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<List<SymptomDTO>> ElrSymptomList(long queued_events_records_id)
        {
            string query = SymptomDTO.get_event_symptoms();

            var data = (await _db.SelectQuery<SymptomDTO>(query, new { queued_events_records_id = queued_events_records_id }, ELR_db)).ToList();

            return data;
        }
        public async Task<GetNextEventResDTO?> IsProcessingEvent(long emp_id)
        {
            string query = EventAssignDTO.get_active_event();

            var result = (await _db.SelectQuery<GetNextEventResDTO>(query, new { assigned_by = emp_id }, ELR_db)).FirstOrDefault();

            return result;
        }

        public async Task<GetNextEventResDTO?> GetNext_Queue_Event_Details(long queued_events_records_id)
        {
            string query = GetNextEventResDTO.get_event_details();

            return (await _db.SelectQuery<GetNextEventResDTO>(query, new { queued_events_records_id = queued_events_records_id }, ELR_db)).FirstOrDefault();

        }
        public async Task<List<FetchHistoryDTO>> GetQueueFetchHistory(long queued_events_records_id)
        {
            string query = FetchHistoryDTO.get_fetch_history();
            return (await _db.SelectQuery<FetchHistoryDTO>(query, new { queued_events_records_id = queued_events_records_id }, ELR_db)).ToList();

        }

        public async Task<ResponseDTO> ElrQueueNextEvent(long emp_id, long queued_events_records_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (queued_events_records_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                var activeEvent = await IsProcessingEvent(emp_id);

                if (activeEvent != null)
                {
                    activeEvent.active = true;

                    return new ResponseDTO(false, "Current event must be completed before taking next event", activeEvent);
                }

                var data = await GetNext_Queue_Event_Details(queued_events_records_id);

                if (data == null) return new ResponseDTO(false, "Event is already taken");

                data.symptoms = await ElrSymptomList(queued_events_records_id);

                data.fetch_history = await GetQueueFetchHistory(data.queued_events_records_id);

                try
                {

                    query = EventAssignDTO.event_assign_insert();
                    data.event_assign_id = await _db.ExecuteAndGetId(query, new
                    {
                        patient_enrollment_id = data.patient_enrollment_id,
                        queued_events_records_id = data.queued_events_records_id,
                        assigned_by = emp_id,
                        created_by = emp_id,
                        work_status = "inprogress",
                        analysis_start_datetime = DateTime.Now,
                        last_activity_datetime = DateTime.Now
                    }, ELR_db);

                    query = QueuedAnomalyDTO.update_event_processing();
                    await _db.Execute(query, new { queued_events_records_id = data.queued_events_records_id, modified_by = emp_id }, ELR_db);

                    query = QueuedAnomalyDTO.get_modified_datetime_query();
                    data.modified_datetime = (await _db.SelectQuery<DateTime?>(query, new { queued_events_records_id = data.queued_events_records_id }, ELR_db)).FirstOrDefault();

                }
                catch
                {
                    query = QueuedAnomalyDTO.reset_event_status();
                    await _db.Execute(query, new { data.queued_events_records_id, data.modified_by }, ELR_db);
                    return new ResponseDTO(false, "Already assigined event to another employee.");
                }

                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message);
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<long> ElrsSelectIsFinalAssignId(long emp_id)
        {
            string query = EventAssignDTO.select_is_final_assignId();
            long eventAssignId = (await _db.SelectQuery<long>(query, new { assigned_by = emp_id }, ELR_db)).FirstOrDefault();
            return eventAssignId;
        }


        public async Task<int> ElrUpdateFinalCompleted(long eventAssignId, long empId)
        {
            string query = EventAssignDTO.update_is_final_completed();
            return await _db.Execute(query, new { event_assign_id = eventAssignId, assigned_by = empId }, ELR_db);
        }

        public async Task<ResponseDTO> ElrCloseEvent(long eventAssignId, long empId)
        {
            ErrorMessage = string.Empty;
            try
            {
                if (eventAssignId <= 0) return new ResponseDTO(false, "Event Assign Id Not Passing");
                int count = await ElrUpdateFinalCompleted(eventAssignId, empId);
                return new ResponseDTO(count > 0, count > 0 ? ResponseMessage.Success : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message);
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrAddUpdateSymtomType(SymtomTypeDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.symptoms_type_id == 0) query = SymtomTypeDTO.insert_symptoms_type();
                else
                {
                    obj.modified_by = obj.created_by;
                    query = SymtomTypeDTO.update_symptoms_type();
                }
                var changed = await _db.Execute(query, obj, ELR_db);

                return new ResponseDTO(changed > 0, changed > 0 ? (obj.symptoms_type_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrGetNextEvent(long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                var activeEvent = await IsProcessingEvent(emp_id);

                if (activeEvent != null)
                {
                    activeEvent.active = true;

                    return new ResponseDTO(false, "Current event must be completed before taking next event", activeEvent);
                }

                long eventAssignId = await ElrsSelectIsFinalAssignId(emp_id);

                if (eventAssignId > 0)
                {
                    await ElrUpdateFinalCompleted(eventAssignId, emp_id);
                }

                query = GetNextEventResDTO.get_next_event();

                var queuedEventId = (await _db.SelectQuery<long>(query, new { modified_by = emp_id }, ELR_db)).FirstOrDefault();

                if (queuedEventId <= 0) return new ResponseDTO(false, "No Pending Events");

                var data = await GetNext_Queue_Event_Details(queuedEventId);

                if (data == null) return new ResponseDTO(false, "Event is already taken");

                data.symptoms = await ElrSymptomList(data.queued_events_records_id);

                data.fetch_history = await GetQueueFetchHistory(data.queued_events_records_id);

                query = EventAssignDTO.event_assign_insert();
                data.event_assign_id = await _db.ExecuteAndGetId(query,
                    new
                    {
                        patient_enrollment_id = data.patient_enrollment_id,
                        queued_events_records_id = data.queued_events_records_id,
                        assigned_by = emp_id,
                        created_by = emp_id,
                        work_status = "inprogress",
                        analysis_start_datetime = DateTime.Now,
                        last_activity_datetime = DateTime.Now
                    }, ELR_db);

                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrTimeOutEvent(long event_assign_id, long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = EventAssignDTO.three_minute_timeout();
                var affectedRows = await _db.Execute(query, new { event_assign_id = event_assign_id, modified_by = emp_id }, ELR_db);
                if (affectedRows <= 0)
                {
                    return new ResponseDTO(false, "Event not found.");
                }
                return new ResponseDTO(true, "Event reassigned successfully.");
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrPatientCurrentStrip(long event_assign_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (event_assign_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = CurrentStripDTO.get_current_pateint_strip();

                var data = (await _db.SelectQuery<CurrentStripDTO>(query, new { event_assign_id = event_assign_id }, ELR_db)).FirstOrDefault();

                if (data == null)
                {
                    return new ResponseDTO(false, $"No record found for event_assign_id = {event_assign_id}");
                }

                query = SubStripEventDTO.get_sub_strip();

                data.sub_strip = (await _db.SelectQuery<SubStripEventResDTO>(query, new { event_assign_id = event_assign_id }, ELR_db)).ToList();

                if (data.parent_detection_id > 0)
                {
                    query = FindingDTO.get_parent_finding_by_detection();

                    data.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { detection_id = data.parent_detection_id }, ELR_db)).ToList();
                }
                else
                {
                    query = FindingDTO.get_parent_finding();

                    data.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { event_assign_id }, ELR_db)).ToList();
                }

                foreach (var strip in data.sub_strip)
                {
                    if (strip.detection_id > 0)
                    {
                        query = FindingDTO.get_sub_strip_finding_by_detection();

                        strip.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { detection_id = strip.detection_id, sub_strip_event_id = strip.sub_strip_event_id }, ELR_db)).ToList();
                    }
                    else
                    {
                        query = FindingDTO.get_sub_strip_finding();

                        strip.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { event_assign_id, sub_strip_event_id = strip.sub_strip_event_id }, ELR_db)).ToList();
                    }
                }

                bool parentCompleted = data.finding_list.Any();

                bool subStripCompleted = true;

                if (data.sub_strip.Any())
                {
                    subStripCompleted = data.sub_strip.All(x => x.finding_list.Any());
                }

                if (parentCompleted && subStripCompleted)
                {
                    data.event_status = "completed";
                }
                else
                {
                    data.event_status = "processing";
                }
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrSubStripEvent(SubStripEventDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || (obj.queued_events_records_id <= 0 || obj.event_assign_id <= 0)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.json_data.ValueKind == JsonValueKind.Undefined || obj.json_data.ValueKind == JsonValueKind.Null)
                {
                    return new ResponseDTO(false, "JSON data is required.");
                }


                string folderPath = CommonFunctions.FolderPath($"Sub-Strip\\{obj.queued_events_records_id}");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = $"{obj.queued_events_records_id}_{DateTime.Now:yyyyMMddHHmmssfff}.json";

                string fullPath = Path.Combine(folderPath, fileName);

                string json = obj.json_data.GetRawText();

                await File.WriteAllTextAsync(fullPath, json);

                obj.sub_strip_file_path = fullPath.Replace(Directory.GetCurrentDirectory() + "\\", "");

                query = SubStripEventDTO.insert_sub_strip();
                var subStripId = await _db.ExecuteAndGetId(query, obj, ELR_db);

                return new ResponseDTO(subStripId > 0, subStripId > 0 ? ResponseMessage.Success : ResponseMessage.Failed);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrInActiveSubStrip(long sub_strip_event_id, long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (sub_strip_event_id < 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = SubStripEventDTO.in_active_sub_strip();
                int rows = await _db.Execute(query, new { sub_strip_event_id = sub_strip_event_id, modified_by = emp_id }, ELR_db);

                //if (rows > 0)
                //{
                //    await _db.Execute(
                //        SubStripEventDTO.inactive_sub_strip_log(),
                //        new
                //        {
                //            sub_strip_event_id,
                //            modified_by = emp_id
                //        },
                //        ELR_db);
                //}
                return new ResponseDTO(rows > 0, rows > 0 ? ResponseMessage.Success : ResponseMessage.Failed);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrSubStripReport(SubStripReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null)
                    return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = SubStripReqDTO.get_sub_strip_report(obj);

                var (totalCount, data) = await _db.SelectQueryWithCount<long, SubStripEventResDTO>(query, obj, ELR_db);

                if (data == null || !data.Any()) return new ResponseDTO(false, "No Data");

                foreach (var item in data)
                {
                    if (item.detection_id > 0)
                    {
                        query = FindingDTO.get_sub_strip_finding_by_detection();

                        item.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { detection_id = item.detection_id, sub_strip_event_id = item.sub_strip_event_id }, ELR_db)).ToList();
                    }
                    else
                    {
                        query = FindingDTO.get_sub_strip_finding();

                        item.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { item.event_assign_id, item.sub_strip_event_id }, ELR_db)).ToList();
                    }
                }

                var response = new
                {
                    totalCount = totalCount,
                    items = data
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

        public async Task<ResponseDTO> ElrAddUpdateFindingEvent(FindingEventDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.event_finding_id == 0)
                {
                    query = FindingEventDTO.insert_finding_mast();
                }
                else
                {
                    obj.modified_by = obj.created_by;
                    query = FindingEventDTO.update_finding_mast();
                }
                var changed = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(changed > 0, changed > 0 ? (obj.event_finding_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrFindingList()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;

            try
            {
                query = FindingEventDTO.get_finding_mast();

                var data = (await _db.SelectQuery<FindingEventDTO>(query, null, ELR_db)).ToList();

                foreach (var item in data)
                {
                    item.child_list = await ElrGetChildFinding(item.event_finding_id);
                }

                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<List<FindingEventDTO>> ElrGetChildFinding(long parent_finding_id)
        {
            string query = FindingEventDTO.get_finding_parent_id();

            var children = (await _db.SelectQuery<FindingEventDTO>(query, new { parent_finding_id = parent_finding_id }, ELR_db)).ToList();

            foreach (var child in children)
            {
                child.child_list = await ElrGetChildFinding(child.event_finding_id);
            }

            return children;
        }
        public async Task<ResponseDTO> ElrGetParentStrip(ParentStripDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                query = ParentStripDTO.select_parent_strip();
                var data = (await _db.SelectQuery<ParentStripDTO>(query, new { sub_strip_event_id = obj.sub_strip_event_id }, ELR_db)).FirstOrDefault();
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrAddUpdateActivityLog(ActivityLogReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;

            try
            {
                if (obj == null || obj.queued_events_records_id == 0 || obj.detection_id == 0 || obj.created_by == 0 || string.IsNullOrWhiteSpace(obj.action_type)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = ActivityLogResDTO.insert_activity_log();

                long activityLogId = await _db.ExecuteAndGetId(query,
                    new
                    {
                        obj.queued_events_records_id,
                        obj.detection_id,
                        obj.action_type,
                        obj.created_by
                    },
                    ELR_db);

                if (activityLogId <= 0) return new ResponseDTO(false, ResponseMessage.Failed);


                if (obj.finding_list != null && obj.finding_list.Count > 0)
                {
                    query = ActivityLogResDTO.insert_activity_log_finding();

                    foreach (var finding in obj.finding_list)
                    {
                        await _db.Execute(
                            query,
                            new
                            {
                                activity_log_id = activityLogId,
                                finding.event_finding_id,
                                finding.value1,
                                finding.value2,
                                finding.finding_status,
                                created_by = obj.created_by
                            },
                            ELR_db);
                    }
                }

                return new ResponseDTO(true, ResponseMessage.Success);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }

            return new ResponseDTO(false, ErrorMessage);
        }
        //finding detection submit and push to qc
        public async Task<ResponseDTO> ElrPushToQcEvent(PushToQCDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                string detectionStatus = obj.is_experienced_user ? "submit" : "push-to-qc";

                query = PushToQCDTO.get_detection();

                long? detectionById = (await _db.SelectQuery<long?>(query,
                    new
                    {
                        obj.queued_events_records_id,
                        obj.sub_strip_event_id,
                        obj.event_assign_id
                    },
                    ELR_db)).FirstOrDefault();

                if (detectionById == null || detectionById == 0)
                {

                    query = PushToQCDTO.insert_detection();

                    long detection_id = await _db.ExecuteAndGetId(query,
                        new
                        {
                            obj.sub_strip_event_id,
                            obj.queued_events_records_id,
                            detection_status = detectionStatus,
                            created_by = obj.created_by,
                            event_assign_id = obj.event_assign_id
                        },
                        ELR_db);


                    query = PushToQCDTO.insert_sub_strip_finding();

                    foreach (var item in obj.finding_list)
                    {
                        long findingId = await _db.ExecuteAndGetId(query,
                                          new
                                          {
                                              detection_id = detection_id,
                                              obj.queued_events_records_id,
                                              obj.sub_strip_event_id,
                                              item.event_finding_id,
                                              item.value1,
                                              item.value2,
                                              created_by = obj.created_by,
                                              event_assign_id = obj.event_assign_id
                                          }, ELR_db);


                    }
                    await ElrAddUpdateActivityLog(new ActivityLogReqDTO
                    {
                        queued_events_records_id = obj.queued_events_records_id.Value,
                        detection_id = detection_id,
                        action_type = obj.is_experienced_user ? "Submit" : "PushToQC",
                        created_by = obj.created_by,
                        finding_list = obj.finding_list.Select(x => new ActivityLogFindingDTO
                        {
                            event_finding_id = x.event_finding_id,
                            value1 = x.value1,
                            value2 = x.value2,
                            finding_status = "Added"
                        }).ToList()
                    });

                    await UpdateEventStatus(obj);

                    return new ResponseDTO(true, ResponseMessage.Success, detection_id);
                }
                else
                {
                    query = PushToQCDTO.update_detection();
                    await _db.Execute(query, new { detection_id = detectionById, modified_by = obj.created_by }, ELR_db);

                    ////// Log
                    //query = PushToQCDTO.update_detection_log();
                    //await _db.Execute(query, new
                    //{
                    //    detection_id = detectionById,
                    //    modified_by = obj.created_by
                    //}, ELR_db);

                    query = PushToQCDTO.inactive_sub_strip_finding();
                    await _db.Execute(query, new { detection_id = detectionById, modified_by = obj.created_by }, ELR_db);

                    // Log
                    //query = PushToQCDTO.inactive_sub_strip_finding_log();
                    //await _db.Execute(query, new
                    //{
                    //    detection_id = detectionById,
                    //    modified_by = obj.created_by
                    //}, ELR_db);

                    query = PushToQCDTO.insert_sub_strip_finding();

                    foreach (var item in obj.finding_list)
                    {
                        long sub_strip_event_finding_id = await _db.ExecuteAndGetId(
                            query,
                            new
                            {
                                detection_id = detectionById.Value,
                                obj.queued_events_records_id,
                                obj.sub_strip_event_id,
                                item.event_finding_id,
                                item.value1,
                                item.value2,
                                created_by = obj.created_by,
                                event_assign_id = obj.event_assign_id
                            },
                            ELR_db);


                    }
                    await ElrAddUpdateActivityLog(new ActivityLogReqDTO
                    {
                        queued_events_records_id = obj.queued_events_records_id.Value,
                        detection_id = detectionById.Value,
                        action_type = obj.is_experienced_user ? "ReSubmit" : "Update",
                        created_by = obj.created_by,
                        finding_list = obj.finding_list.Select(x => new ActivityLogFindingDTO
                        {
                            event_finding_id = x.event_finding_id,
                            value1 = x.value1,
                            value2 = x.value2,
                            finding_status = "ReSubmit"
                        }).ToList()
                    });

                    await UpdateEventStatus(obj);

                    return new ResponseDTO(true, ResponseMessage.Updated_Successfully);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        private async Task UpdateEventStatus(PushToQCDTO obj)
        {
            string query;

            long totalStrips = 0;
            long completedStrips = 0;

            if (obj.sub_strip_event_id != null && obj.sub_strip_event_id != 0)
            {
                query = PushToQCDTO.get_total_active_sub_strips();

                totalStrips = (await _db.SelectQuery<long>(query, new { event_assign_id = obj.event_assign_id }, ELR_db)).FirstOrDefault();

                query = PushToQCDTO.get_completed_sub_strips();

                completedStrips = (await _db.SelectQuery<long>(query, new { event_assign_id = obj.event_assign_id }, ELR_db)).FirstOrDefault();
            }

            string workStatus;
            string eventStatus;

            bool isFinalCompleted = false;

            if (obj.sub_strip_event_id == null || obj.sub_strip_event_id == 0)
            {
                workStatus = obj.is_experienced_user ? "completed" : "partial-completed";
                eventStatus = "completed";

                isFinalCompleted = obj.is_experienced_user;
            }
            else if (totalStrips > 0 && totalStrips == completedStrips)
            {
                workStatus = obj.is_experienced_user ? "completed" : "partial-completed";
                eventStatus = "completed";

                isFinalCompleted = obj.is_experienced_user;
            }
            else
            {
                workStatus = "inprogress";
                eventStatus = "processing";

                isFinalCompleted = false;
            }

            query = EventAssignDTO.update_work_status_by_event_assign();

            await _db.Execute(query,
                new
                {
                    event_assign_id = obj.event_assign_id,
                    work_status = workStatus,
                    is_final_completed = isFinalCompleted,
                    modified_by = obj.created_by
                },
                ELR_db);

            query = QueuedAnomalyDTO.update_event_status_by_queue();

            await _db.Execute(query,
                new
                {
                    queued_events_records_id = obj.queued_events_records_id,
                    completed_by = obj.created_by,
                    event_status = eventStatus
                },
                ELR_db);
        }
        public async Task<ResponseDTO> ElrCompletedFindingList(CompletedFindingListReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;

            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = CompletedFindingListReqDTO.get_completed_findings(obj);

                var (totalCount, data) = await _db.SelectQueryWithCount<long, CompletedFindingListDTO>(query, obj, ELR_db);
                if (data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);
                var response = new
                {
                    totalCount = totalCount,
                    items = data
                };
                return new ResponseDTO(response);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(null, ex.ToString());
                ErrorMessage = ex.Message;
            }

            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrGetCompleteQc(long detection_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (detection_id < 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = CompletedFindingListResDTO.select_complete_qc_report();
                var data = (await _db.SelectQuery<CompletedFindingListResDTO>(query, new { detection_id = detection_id }, ELR_db)).FirstOrDefault();

                if (data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                data.symptoms = await ElrSymptomList(data.queued_events_records_id);

                query = FetchHistoryDTO.get_fetch_history();
                data.fetch_history = (await _db.SelectQuery<FetchHistoryDTO>(query, new { queued_events_records_id = data.queued_events_records_id }, ELR_db)).ToList();

                if (data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                var findingQuery = FindingDTO.select_complete_qc_report_findings();
                data.findings = (await _db.SelectQuery<FindingDTO>(findingQuery, new { detection_id = detection_id }, ELR_db)).ToList();

                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrUpdateSubmitQcEvent(ConfirmSubmitDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = ConfirmSubmitDTO.insert_detection();
                long newDetectionId = await _db.ExecuteAndGetId(query,
                     new
                     {
                         parent_detection_id = obj.detection_id,
                         obj.sub_strip_event_id,
                         obj.queued_events_records_id,
                         detection_status = "confirm-submit",
                         created_by = obj.created_by
                     }, ELR_db);

                query = ConfirmSubmitDTO.insert_sub_strip_finding();
                foreach (var item in obj.finding_list)
                {
                    await _db.Execute(query,
                        new
                        {
                            obj.sub_strip_event_id,
                            obj.queued_events_records_id,
                            item.event_finding_id,
                            detection_id = newDetectionId,
                            item.value1,
                            item.value2,
                            created_by = obj.created_by
                        }, ELR_db);
                }

                query = QueuedAnomalyDTO.update_event_status();

                await _db.Execute(
                    query,
                    new
                    {
                        queued_events_records_id = obj.queued_events_records_id,
                        completed_by = obj.created_by,
                        event_status = "completed"
                    },
                    ELR_db);

                query = EventAssignDTO.update_work_status();

                await _db.Execute(
                    query,
                    new
                    {
                        queued_events_records_id = obj.queued_events_records_id,
                        completed_by = obj.created_by,
                        work_status = "completed"
                    },
                    ELR_db);

                return new ResponseDTO(true, "QC Update TO Submitted Successfully.");

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrGetNextQcDetection()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = ConfirmSubmitDTO.get_next_qc_detection();

                var data = (await _db.SelectQuery<ConfirmSubmitDTO>(query, null, ELR_db)).FirstOrDefault();

                if (data == null) return new ResponseDTO(false, "No QC records available.");

                return new ResponseDTO(data);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrQcPickDetection(ConfirmSubmitDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj.detection_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);
                if (obj.qc_tech_detection == false)
                {
                    query = ConfirmSubmitDTO.release_pick_detection();

                    int row = await _db.Execute(query, new { detection_id = obj.detection_id, modified_by = obj.picked_by }, ELR_db);

                    return new ResponseDTO(true, "Detection released successfully");
                }
                query = ConfirmSubmitDTO.pick_detection();
                int rows = await _db.Execute(query, new { detection_id = obj.detection_id, picked_by = obj.picked_by }, ELR_db);
                if (rows == 0)
                {
                    return new ResponseDTO(false, "This detection is already taken");
                }
                return new ResponseDTO(true, "Detection assigned successfully");
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<ResponseDTO> ElrCompletePriorityReport(PriorityCompleteReportReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || obj.patient_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PriorityCompleteReportReqDTO.detection_complete_report(obj);
                var (totalCount, data) = await _db.SelectQueryWithCount<long, PriorityCompleteReportDTO>(query, obj, ELR_db);

                //if (data == null || !data.Any()) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                //foreach (var item in data)
                //{
                //    query = FindingDTO.get_parent_finding_list();
                //    item.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { event_assign_id = item.event_assign_id }, ELR_db)).ToList();

                //    query = SubStripEventResDTO.sub_strip_files_select();
                //    item.sub_strip_files = (await _db.SelectQuery<SubStripEventResDTO>(query, new { event_assign_id = item.event_assign_id }, ELR_db)).ToList();

                //    foreach (var strip in item.sub_strip_files)
                //    {
                //        query = FindingDTO.get_finding_sub_strip_list();
                //        strip.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { event_assign_id = strip.event_assign_id, sub_strip_event_id = strip.sub_strip_event_id }, ELR_db)).ToList();
                //    }
                //}

                foreach (var item in data)
                {
                    query = FindingDTO.get_parent_finding_list();

                    item.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { event_assign_id = item.event_assign_id }, ELR_db)).ToList();


                    query = SubStripEventResDTO.sub_strip_files_select();

                    item.sub_strip_files = (await _db.SelectQuery<SubStripEventResDTO>(query, new { event_assign_id = item.event_assign_id }, ELR_db)).ToList();


                    foreach (var strip in item.sub_strip_files)
                    {
                        query = FindingDTO.get_finding_sub_strip_list();

                        strip.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { event_assign_id = strip.event_assign_id, sub_strip_event_id = strip.sub_strip_event_id }, ELR_db)).ToList();
                    }
                }

                var response = new
                {
                    totalCount = totalCount,
                    items = data,
                };

                return new ResponseDTO(response);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrActivityLogList(ActivityLogReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || obj.queued_events_records_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = ActivityLogResDTO.select_activity_log(obj);
                var (totalCount, activityData) = await _db.SelectQueryWithCount<long, ActivityLogResDTO>(query, obj, ELR_db);

                if (activityData == null || activityData.Count() == 0) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                foreach (var activity in activityData)
                {
                    activity.finding_list = new List<ActivityLogFindingDTO>();

                    query = ActivityLogResDTO.select_activity_log_finding();

                    var findingData = await _db.SelectQuery<ActivityLogFindingDTO>(query, new { activity_log_id = activity.activity_log_id }, ELR_db);

                    if (findingData != null && findingData.Count() > 0)
                    {
                        foreach (var finding in findingData)
                        {
                            activity.finding_list.Add(finding);
                        }
                    }
                }
                var result = new
                {
                    totalCount = totalCount,
                    item = activityData,
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
        public async Task<ResponseDTO> ElrGetByIdEventAssign(long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;

            try
            {
                query = GetByIdEventAssignResDTO.select_by_event_assign_id();
                var data = (await _db.SelectQuery<GetByIdEventAssignResDTO>(query, new { assigned_by = emp_id }, ELR_db)).FirstOrDefault();
                if (data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrFetchECG(FetchECGReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = FetchECGReqDTO.get_current_event();
                var currentEventData = (await _db.SelectQuery<dynamic>(query, new { queued_events_records_id = obj.queued_events_records_id }, ELR_db)).FirstOrDefault();
                if (currentEventData == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                DateTime fromTime;
                DateTime toTime;

                if (obj.fetch_direction.Equals("before", StringComparison.OrdinalIgnoreCase))
                {
                    fromTime = currentEventData.event_start_datetime.AddMinutes(-obj.minutes);
                    toTime = currentEventData.event_start_datetime;
                }
                else if (obj.fetch_direction.Equals("after", StringComparison.OrdinalIgnoreCase))
                {
                    fromTime = currentEventData.event_end_datetime;
                    toTime = currentEventData.event_end_datetime.AddMinutes(obj.minutes);
                }
                else
                {
                    return new ResponseDTO(false, "Invalid fetch direction.");
                }

                query = FetchECGReqDTO.get_baseline_files();

                var baselineFiles = (await _db.SelectQuery<PatientBaseLineDTO>(
                    query,
                    new
                    {
                        patient_enrollment_id = currentEventData.patient_enrollment_id,
                        fromTime,
                        toTime
                    },
                    ELR_db)).ToList();

                if (!baselineFiles.Any()) return new ResponseDTO(false, "Processed ECG not found.");

                var mergedData = new List<EcgJsonDTO>();

                // Processed ECG Files
                foreach (var item in baselineFiles)
                {
                    string fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), item.event_processed_path);

                    if (!File.Exists(fullFilePath))
                        continue;

                    string json = await File.ReadAllTextAsync(fullFilePath);

                    var token = JToken.Parse(json);

                    var list = token.Type == JTokenType.Array ? token.ToObject<List<EcgJsonDTO>>() : new List<EcgJsonDTO>
                        {
                             token.ToObject<EcgJsonDTO>()
                        };

                    AddTrimmedData(list, mergedData, fromTime, toTime);

                }

                string detectionPath = currentEventData.event_file_path;

                if (!string.IsNullOrWhiteSpace(detectionPath))
                {
                    string fullDetectionPath = Path.Combine(Directory.GetCurrentDirectory(), detectionPath);

                    if (File.Exists(fullDetectionPath))
                    {
                        string json = await File.ReadAllTextAsync(fullDetectionPath);

                        var token = JToken.Parse(json);

                        var list = token.Type == JTokenType.Array ? token.ToObject<List<EcgJsonDTO>>() : new List<EcgJsonDTO>
                            {
                                token.ToObject<EcgJsonDTO>()
                            };

                        AddTrimmedData(list, mergedData, fromTime, toTime);
                    }
                }

                string fileName = $"{currentEventData.patient_enrollment_id}-{obj.queued_events_records_id}-{DateTime.Now:yyyyMMddHHmmssfff}.json";

                string folderPath = CommonFunctions.FolderPath($"ECG-FetchData\\{currentEventData.patient_enrollment_id}");

                string fullPath = await CommonFunctions.MergeJsonFile(mergedData, fileName, folderPath);

                if (string.IsNullOrEmpty(fullPath)) return new ResponseDTO(false, "JSON file not created.");

                string relativePath = fullPath.Replace(Directory.GetCurrentDirectory() + "\\", "");

                query = FetchECGReqDTO.check_fetch_exists();

                long existingFetch = (await _db.SelectQuery<long>(
                    query,
                    new
                    {
                        parent_queued_events_records_id = obj.queued_events_records_id,
                        event_source = obj.fetch_direction.Equals("before", StringComparison.OrdinalIgnoreCase) ? "fetch_before" : "fetch_after",
                        fetch_duration_minutes = obj.minutes
                    },
                    ELR_db)).FirstOrDefault();

                if (existingFetch > 0)
                {
                    return new ResponseDTO(false, $"ECG already fetched {obj.fetch_direction} {obj.minutes} minutes.");
                }

                query = FetchECGReqDTO.insert_fetch_event();

                long newQueuedEventId = await _db.ExecuteAndGetId(query,
                    new
                    {
                        patient_enrollment_id = currentEventData.patient_enrollment_id,
                        enrollment_raw_data_id = currentEventData.enrollment_raw_data_id,
                        priority_level = currentEventData.priority_level,
                        trigger_type = "Fetch-ECG",
                        bpm = currentEventData.bpm,
                        event_start_datetime = fromTime,
                        event_end_datetime = toTime,
                        event_file_path = relativePath,
                        parent_queued_events_records_id = obj.queued_events_records_id,
                        event_source = obj.fetch_direction.Equals("before", StringComparison.OrdinalIgnoreCase) ? "fetch_before" : "fetch_after",
                        fetch_duration_minutes = obj.minutes,
                        created_by = obj.created_by,
                        created_datetime = DateTime.Now,
                        event_status = "pending"
                    },
                    ELR_db);

                FetchECGResDTO res = new FetchECGResDTO
                {
                    queued_events_records_id = newQueuedEventId,
                    event_start_datetime = currentEventData.event_start_datetime,
                    event_end_datetime = currentEventData.event_end_datetime,
                    from_datetime = fromTime,
                    to_datetime = toTime,
                    merged_file_path = relativePath,
                    baseline_files = baselineFiles
                };

                return new ResponseDTO(res);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        private EcgJsonDTO TrimWaveform(EcgJsonDTO ecg, DateTime fromTime, DateTime toTime)
        {
            if (ecg?.waveform_snippet == null || ecg.waveform_snippet.timestamp_ms == null || ecg.waveform_snippet.timestamp_ms.Count == 0)
                return null;


            long fromMs = new DateTimeOffset(fromTime).ToUnixTimeMilliseconds();
            long toMs = new DateTimeOffset(toTime).ToUnixTimeMilliseconds();

            var indexes = new List<int>();

            for (int i = 0; i < ecg.waveform_snippet.timestamp_ms.Count; i++)
            {
                long ts = ecg.waveform_snippet.timestamp_ms[i];

                if (ts >= fromMs && ts <= toMs)
                {
                    indexes.Add(i);
                }
            }


            if (!indexes.Any())
                return null;

            int firstIndex = indexes.First();

            if (ecg.waveform_snippet.beat_positions != null && ecg.waveform_snippet.beat_labels != null)
            {
                var newBeatPositions = new List<long>();
                var newBeatLabels = new List<string>();

                for (int i = 0; i < ecg.waveform_snippet.beat_positions.Count; i++)
                {
                    long beatPosition = ecg.waveform_snippet.beat_positions[i];

                    if (beatPosition < 0 || beatPosition >= ecg.waveform_snippet.timestamp_ms.Count)
                        continue;

                    long beatTimestamp = ecg.waveform_snippet.timestamp_ms[(int)beatPosition];

                    if (beatTimestamp >= fromMs && beatTimestamp <= toMs)
                    {
                        newBeatPositions.Add(beatPosition - firstIndex);

                        if (i < ecg.waveform_snippet.beat_labels.Count)
                        {
                            newBeatLabels.Add(ecg.waveform_snippet.beat_labels[i]);
                        }
                    }
                }

                ecg.waveform_snippet.beat_positions = newBeatPositions;

                ecg.waveform_snippet.beat_labels = newBeatLabels;
            }

            ecg.waveform_snippet.timestamp_ms = indexes.Select(i => ecg.waveform_snippet.timestamp_ms[i]).ToList();

            if (ecg.waveform_snippet.voltage_mv != null)
            {
                ecg.waveform_snippet.voltage_mv = indexes.Select(i => ecg.waveform_snippet.voltage_mv[i]).ToList();
            }

            ecg.timestamp = DateTimeOffset.FromUnixTimeMilliseconds(ecg.waveform_snippet.timestamp_ms.First()).LocalDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff");

            ecg.end_timestamp = DateTimeOffset.FromUnixTimeMilliseconds(ecg.waveform_snippet.timestamp_ms.Last()).LocalDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff");

            return ecg;
        }
        private void AddTrimmedData(List<EcgJsonDTO> source, List<EcgJsonDTO> mergedData, DateTime fromTime, DateTime toTime)
        {
            if (source == null)
                return;

            foreach (var ecg in source)
            {
                DateTime start = DateTime.Parse(ecg.timestamp);
                DateTime end = DateTime.Parse(ecg.end_timestamp);

                if (end >= fromTime && start <= toTime)
                {
                    var ecgCopy = Newtonsoft.Json.JsonConvert.DeserializeObject<EcgJsonDTO>(
                        Newtonsoft.Json.JsonConvert.SerializeObject(ecg));

                    var trimmed = TrimWaveform(ecgCopy, fromTime, toTime);

                    if (trimmed != null)
                        mergedData.Add(trimmed);
                }
            }
        }
        public async Task<ResponseDTO> ElrGetFetchHistory(long queued_events_records_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = FetchHistoryDTO.get_event();

                var eventData = (await _db.SelectQuery<FetchHistoryDTO>(query, new { queued_events_records_id = queued_events_records_id }, ELR_db)).FirstOrDefault();

                if (eventData == null) return new ResponseDTO(false, "Event not found.");

                long parentId = eventData.parent_queued_events_records_id ?? eventData.queued_events_records_id;

                query = FetchHistoryDTO.get_fetch_history();

                eventData.fetch_list = (await _db.SelectQuery<FetchHistoryDTO>(query, new { queued_events_records_id = parentId }, ELR_db)).ToList();

                eventData.fetch_list ??= new List<FetchHistoryDTO>();

                return new ResponseDTO(eventData);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<ResponseDTO> ElrFetchHistoryList(FectchHistoryReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = FectchHistoryRespDTO.select_fetch_history_list(obj);
                var (totalCount, data) = await _db.SelectQueryWithCount<long, FectchHistoryRespDTO>(query, obj, ELR_db);

                //if (data == null || totalCount <= 0) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                var response = new
                {
                    totalCount = totalCount,
                    item = data
                };
                return new ResponseDTO(response);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }
        public async Task<byte[]> ElrFetchHistoryExcel(FectchHistoryReqDTO obj)
        {
            try
            {
                if (obj == null) return null;

                string query = FectchHistoryRespDTO.select_fetch_history_excel(obj);

                var data = await _db.SelectQuery<FectchHistoryRespDTO>(query, obj, ELR_db);

                if (data == null || !data.Any()) return null;

                using var workbook = new XLWorkbook();

                var worksheet = workbook.Worksheets.Add("Fetch History");

                worksheet.Cell(1, 1).Value = "Ecg ID";
                worksheet.Cell(1, 2).Value = "Patient name";
                worksheet.Cell(1, 3).Value = "Patient ID";
                worksheet.Cell(1, 4).Value = "Event Date & Time";
                worksheet.Cell(1, 5).Value = "Requested by";
                worksheet.Cell(1, 6).Value = "Reason";
                worksheet.Cell(1, 7).Value = "Status";
                worksheet.Cell(1, 8).Value = "Processed by";
                worksheet.Cell(1, 9).Value = "Processed Date & Time";

                int row = 2;

                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.queued_events_records_id;
                    worksheet.Cell(row, 2).Value = item.patient_name;
                    worksheet.Cell(row, 3).Value = item.patient_id;
                    worksheet.Cell(row, 4).Value = item.created_datetime;
                    worksheet.Cell(row, 5).Value = item.user_name;
                    worksheet.Cell(row, 6).Value = item.event_source;
                    worksheet.Cell(row, 7).Value = item.event_status;
                    worksheet.Cell(row, 8).Value = item.completed_user_name;
                    worksheet.Cell(row, 9).Value = item.completed_datetime;

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message);
                ErrorMessage = ex.Message;

                return null;
            }
        }
        public async Task<ResponseDTO> ElrGetByIdFetchHistory(long queued_events_records_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (queued_events_records_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = FectchHistoryRespDTO.get_fetch_history_by_id();
                var data = (await _db.SelectQuery<FectchHistoryRespDTO>(query, new { queued_events_records_id = queued_events_records_id }, ELR_db)).FirstOrDefault();

                if (data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                if (data != null)
                {
                    query = SymptomDTO.get_event_symptoms();

                    data.symptoms = (await _db.SelectQuery<SymptomDTO>(query, new { queued_events_records_id = queued_events_records_id }, ELR_db)).ToList();

                    query = FindingDTO.get_fetch_history_by_id();

                    data.finding_list = (await _db.SelectQuery<FindingDTO>(query, new { queued_events_records_id = queued_events_records_id }, ELR_db)).ToList();
                }
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();

            }
            return new ResponseDTO(false, ErrorMessage);
        }
        //public async Task<ResponseDTO> ElrMyProgress(long emp_id, string type, string progress_type)
        //{
        //    ErrorMessage = string.Empty;
        //    string query = string.Empty;
        //    try
        //    {

        //        query = MyProgressDTO.GetProgressSummary(type);
        //        var summary = (await _db.SelectQuery<MyProgressDTO>(query, new { assigned_by = emp_id }, ELR_db)).FirstOrDefault();

        //        if (summary == null)
        //        {
        //            summary = new MyProgressDTO
        //            {
        //                EventsSubmitted = 0,
        //                Productivity = 0,
        //                Chart = new List<MyProgressChartDTO>()
        //            };
        //        }
        //        query = MyProgressDTO.GetProgressChart(type);
        //        summary.Chart = (await _db.SelectQuery<MyProgressChartDTO>(query, new { assigned_by = emp_id }, ELR_db)).ToList();

        //        return new ResponseDTO(summary);

        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.ILogging("", ex.Message.ToString());
        //        ErrorMessage = ex.Message.ToString();

        //    }
        //    return new ResponseDTO(false, ErrorMessage);
        //}
        public async Task<ResponseDTO> ElrMyProgress(long emp_id, string type, string progress_type)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (progress_type.ToLower() == "report")
                {
                    query = MyProgressDTO.GetReportProgressSummary(type);
                }
                else
                {
                    query = MyProgressDTO.GetProgressSummary(type);
                }
                var summary = (await _db.SelectQuery<MyProgressDTO>(query, new { assigned_by = emp_id }, ELR_db)).FirstOrDefault();

                if (summary == null)
                {
                    summary = new MyProgressDTO
                    {
                        EventsSubmitted = 0,
                        Productivity = 0,
                        Chart = new List<MyProgressChartDTO>()
                    };
                }
                if (progress_type.ToLower() == "report")
                {
                    query = MyProgressDTO.GetReportProgressChart(type);
                }
                else
                {
                    query = MyProgressDTO.GetProgressChart(type);
                }

                summary.Chart = (await _db.SelectQuery<MyProgressChartDTO>(query, new { assigned_by = emp_id }, ELR_db)).ToList();

                return new ResponseDTO(summary);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }

            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrGetReportType()
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = ReportTypeDTO.get_report_type();
                var data = await _db.SelectQuery<ReportTypeDTO>(query, null, ELR_db);
                if (data == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();

            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrAddUpdateReportType(ReportTypeDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.report_type_id == 0) query = ReportTypeDTO.insert_report_type();

                else
                {
                    obj.modified_by = obj.created_by;
                    query = ReportTypeDTO.update_report_type();

                }
                int row = await _db.Execute(query, obj, ELR_db);
                return new ResponseDTO(row > 0, row > 0 ? (obj.report_type_id == 0 ? ResponseMessage.Saved_Successfully : ResponseMessage.Updated_Successfully) : ResponseMessage.Failed);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrGetEventEcgSearch(EventEcgSearchReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                if (obj.limit <= 0) return new ResponseDTO(false, "Limit must be greater than 0");

                var hasFindings = obj.finding_ids != null && obj.finding_ids.Any();

                var parameters = new
                {
                    obj.from_date,
                    obj.to_date,
                    obj.date_type,
                    obj.queued_events_records_id,
                    obj.user_id,
                    obj.finding_ids,
                    obj.limit,
                    obj.pageNumber,
                    obj.pageSize,
                    obj.offSet,
                    has_findings = hasFindings ? 1 : 0
                };

                query = EventEcgSearchResDTO.get_ecg_search(obj);
                var (totalCount, data) = await _db.SelectQueryWithCount<long, EventEcgSearchResDTO>(query, parameters, ELR_db);

                if (data == null || !data.Any()) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                foreach (var item in data)
                {
                    query = FindingDTO.get_event_findings();
                    item.findings_list = (await _db.SelectQuery<FindingDTO>(query, new { queued_events_records_id = item.queued_events_records_id }, ELR_db)).ToList();

                }

                var response = new
                {
                    TotalCount = totalCount,
                    item = data,
                };
                return new ResponseDTO(response);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<ResponseDTO> ElrGetPatientReportList(PatientReportReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientReportDTO.get_patient_report_list(obj);
                var (totalCount, data) = await _db.SelectQueryWithCount<long, PatientReportDTO>(query, obj, ELR_db);


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

        public async Task<ResponseDTO> ElrGetNextPatientReport(long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {

                query = PatientReportDTO.get_current_report();
                var currentReport = (await _db.SelectQuery<PatientReportDTO>(query, new { emp_id = emp_id }, ELR_db)).FirstOrDefault();
                if (currentReport != null) return new ResponseDTO(currentReport);

                query = PatientReportDTO.get_next_report();
                var nextReport = (await _db.SelectQuery<PatientReportDTO>(query, null, ELR_db)).FirstOrDefault();

                query = PatientReportDTO.insert_report_assigned();
                var assignedId = await _db.ExecuteAndGetId(query,
                                       new
                                       {
                                           patient_report_id = nextReport.patient_report_id,
                                           patient_enrollment_id = nextReport.patient_enrollment_id,
                                           created_by = emp_id
                                       },
                                       ELR_db);

                query = PatientReportDTO.update_report_inprogress();
                await _db.Execute(query, new { patient_report_id = nextReport.patient_report_id, modified_by = emp_id }, ELR_db);

                query = PatientReportDTO.current_report_assign_detatils();
                var data = await _db.SelectQuery<PatientReportDTO>(query, new { report_assigned_id = assignedId }, ELR_db);

                return new ResponseDTO(data);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();

            }
            return new ResponseDTO(false, ErrorMessage);

        }
        public async Task<ResponseDTO> ElrClosePatientReport(long report_assigned_id, long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (report_assigned_id == 0 || emp_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientReportDTO.select_assigned_report();
                var assignedReport = (await _db.SelectQuery<PatientReportDTO>(query, new { report_assigned_id = report_assigned_id }, ELR_db)).FirstOrDefault();

                if (assignedReport.assign_status == "processing")
                {
                    query = PatientReportDTO.close_patient_report_assgin();
                    await _db.Execute(query, new { report_assigned_id = report_assigned_id, modified_by = emp_id }, ELR_db);

                    query = PatientReportDTO.reset_patient_report_status();
                    await _db.Execute(query, new { report_assigned_id = report_assigned_id, modified_by = emp_id }, ELR_db);

                    return new ResponseDTO(true, ResponseMessage.Success);
                }

                return new ResponseDTO(true, $"Report status: {assignedReport.assign_status} No action.");

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);

        }

        public async Task<ResponseDTO> ElrGetNextPatientReportList(long patient_report_id, long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (patient_report_id == 0 || emp_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientReportDTO.get_current_report();
                var currentReport = (await _db.SelectQuery<PatientReportDTO>(query, new { emp_id = emp_id }, ELR_db)).FirstOrDefault();
                if (currentReport != null) return new ResponseDTO(false, "Please complete privious report.", currentReport);

                query = PatientReportDTO.get_next_patient_report_list();
                var reportData = (await _db.SelectQuery<PatientReportDTO>(query, new { patient_report_id = patient_report_id }, ELR_db)).FirstOrDefault();

                if (reportData != null)
                {
                    query = PatientReportDTO.insert_report_assigned();
                    var assignedId = await _db.ExecuteAndGetId(query, new { patient_report_id = reportData.patient_report_id, patient_enrollment_id = reportData.patient_enrollment_id, created_by = emp_id }, ELR_db);


                    query = PatientReportDTO.update_report_inprogress();
                    await _db.Execute(query, new { patient_report_id = reportData.patient_report_id, modified_by = emp_id }, ELR_db);

                    query = PatientReportDTO.current_report_assign_detatils();
                    var data = await _db.SelectQuery<PatientReportDTO>(query, new { report_assigned_id = assignedId }, ELR_db);

                    return new ResponseDTO(data);
                }
                return new ResponseDTO(false, "Report Already Taken.");

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();

            }
            return new ResponseDTO(false, ErrorMessage);
        }



        public async Task<ResponseDTO> ElrAddUpdateReportGenerate(PatientReportGenerateReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null || obj.report_assigned_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientReportGenerateReqDTO.check_patient_report_status();
                var assignData = (await _db.SelectQuery<PatientReportDTO>(query, new { report_assigned_id = obj.report_assigned_id, patient_report_id = obj.patient_report_id }, ELR_db)).FirstOrDefault();

                if (assignData != null && assignData.assign_status == "pending") return new ResponseDTO(false, $"Already Assign Id: {assignData.report_assigned_id},Report status: {assignData.assign_status}.");
                if (assignData != null && assignData.assign_status == "completed") return new ResponseDTO(false, $"Already completed, Report status: {assignData.assign_status}.");

                query = PatientReportGenerateReqDTO.existing_patient_report_generate();
                var existingStrips = await _db.SelectQuery<PatientReportGenerateStripDTO>(query, new { patient_report_id = obj.patient_report_id, report_assigned_id = obj.report_assigned_id }, ELR_db);

                if (obj.stripIds == null || !obj.stripIds.Any())
                {
                    if (existingStrips != null && existingStrips.Any())
                    {
                        query = PatientReportGenerateReqDTO.delete_all_patient_report_generate();

                        await _db.Execute(query, new { patient_report_id = obj.patient_report_id, report_assigned_id = obj.report_assigned_id, modified_by = obj.created_by }, ELR_db);
                    }

                    query = PatientReportGenerateReqDTO.update_assigned_status();
                    await _db.Execute(query, new { report_assigned_id = obj.report_assigned_id, patient_report_id = obj.patient_report_id, modified_by = obj.created_by }, ELR_db);

                    // Report should go back to pending
                    query = PatientReportGenerateReqDTO.update_patient_report_status_pending();
                    await _db.Execute(query, new { patient_report_id = obj.patient_report_id, modified_by = obj.created_by }, ELR_db);

                    query = PatientReportGenerateReqDTO.update_patient_generate_report();
                    await _db.Execute(query, new { patient_report_id = obj.patient_report_id, modified_by = obj.created_by }, ELR_db);


                    return new ResponseDTO(true, ResponseMessage.Success);
                }


                foreach (var existingStrip in existingStrips)
                {
                    bool isAvailable = obj.stripIds.Any(x => x.queued_events_records_id == existingStrip.queued_events_records_id && x.sub_strip_event_id == existingStrip.sub_strip_event_id);

                    if (!isAvailable)
                    {
                        query = PatientReportGenerateReqDTO.update_patient_report_generate();

                        await _db.Execute(
                            query,
                            new
                            {
                                patient_report_id = obj.patient_report_id,
                                report_assigned_id = obj.report_assigned_id,
                                queued_events_records_id = existingStrip.queued_events_records_id,
                                sub_strip_event_id = existingStrip.sub_strip_event_id,
                                modified_by = obj.created_by
                            },
                            ELR_db);
                    }
                }

                foreach (var strip in obj.stripIds)
                {
                    var isExisting = existingStrips.Any(x => x.queued_events_records_id == strip.queued_events_records_id && x.sub_strip_event_id == strip.sub_strip_event_id);

                    if (!isExisting)
                    {
                        query = PatientReportGenerateReqDTO.insert_patient_report_generate();

                        await _db.Execute(
                         query,
                         new
                         {
                             patient_report_id = obj.patient_report_id,
                             report_assigned_id = obj.report_assigned_id,
                             queued_events_records_id = strip.queued_events_records_id,
                             sub_strip_event_id = strip.sub_strip_event_id,
                             created_by = obj.created_by
                         },
                         ELR_db);
                    }
                }
                query = PatientReportDTO.update_assign_processing_status();
                await _db.Execute(query, new { report_assigned_id = obj.report_assigned_id, modified_by = obj.created_by }, ELR_db);


                query = PatientReportDTO.update_patient_report_status();
                await _db.Execute(query, new { patient_report_id = obj.patient_report_id, created_by = obj.created_by }, ELR_db);


                return new ResponseDTO(true, ResponseMessage.Success);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrGetReportGenerateList(long patient_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (patient_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientReportGenerateReqDTO.get_report_generate_list();
                var (totalCount, data) = await _db.SelectQueryWithCount<long, ReportGenerateListResDTO>(query, new { patient_id = patient_id }, ELR_db);

                foreach (var item in data)
                {
                    query = PatientReportGenerateReqDTO.existing_patient_report_generate();
                    item.stripList = (await _db.SelectQuery<PatientReportGenerateStripDTO>(query, new { patient_report_id = item.patient_report_id }, ELR_db)).ToList();

                }
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

        public async Task<ResponseDTO> ElrPatientReportSearchList(PatientReportSearchReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (obj == null) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientReportSearchReqDTO.patient_report_search_list(obj);

                var (totalCount, data) = await _db.SelectQueryWithCount<long, PatientReportDTO>(query, obj, ELR_db);

                foreach (var item in data)
                {
                    query = PatientReportGenerateReqDTO.existing_patient_report_generate();
                    item.report_strips = (await _db.SelectQuery<PatientReportGenerateStripDTO>(query, new { patient_report_id = item.patient_report_id }, ELR_db)).ToList();

                }

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
}

