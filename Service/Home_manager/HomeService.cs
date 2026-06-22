using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.IService.IDapper;
using elr_API.IService.IHome_manager;
using System.Text.Json;

namespace elr_API.Service.Home_manager
{
    public class HomeService : IHomeService
    {
        private readonly IDapperSqlService _db;
        public string ErrorMessage = string.Empty;
        public string ELR_db = CommonFunctions.GetConnectionString("elr_db");
        public HomeService(IDapperSqlService db)
        {
            _db = db;
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
                else query = UserTypeDTO.update_user_type();

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
                else query = AddUpdateTestDTO.update_test();

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
                else query = AddUpdateTestPricesDTO.update_test_prices();

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

                    var fileName = $"{obj.patient_enrollment_id}-{DateTime.Now:yyyyMMddHHmmssfff}{extention}";

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
                        query = EnrollmentRawDataDTO.update_raw_data_status();
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
                if (data == null) return new ResponseDTO(false, "No Data");
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

                    var fileName = $"{obj.patient_enrollment_id}-{DateTime.Now:yyyyMMddHHmmssfff}{extention}";

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
        public async Task<ResponseDTO> ElrGetNextEvent(long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = EventAssignDTO.get_active_event();

                var active = await _db.QuerySingle<int>(query, new { emp_id }, ELR_db);

                if (active > 0) return new ResponseDTO(false, "Current event must be completed before taking next event");

                query = GetNextEventResDTO.get_next_event();

                long? queuedId = await _db.QuerySingle<long?>(query, null, ELR_db);

                if (!queuedId.HasValue) return new ResponseDTO(false, "No pending events");

                query = GetNextEventResDTO.get_event_details();

                var data = await _db.QuerySingle<GetNextEventResDTO>(query, new { queued_events_records_id = queuedId.Value }, ELR_db);

                if (data == null)
                    return new ResponseDTO(false, "No pending events");


                try
                {
                    query = EventAssignDTO.event_assign_insert();
                    await _db.Execute(query, new
                    {
                        patient_enrollment_id = data.patient_enrollment_id,
                        queued_events_records_id = data.queued_events_records_id,
                        assigned_by = emp_id,
                        created_by = emp_id,
                        work_status = "inprogress",
                        analysis_start_datetime = DateTime.Now,
                        last_activity_datetime = DateTime.Now
                    }, ELR_db);
                }
                catch
                {
                    query = QueuedAnomalyDTO.reset_to_pending();
                    await _db.Execute(query, new { data.queued_events_records_id }, ELR_db);
                    throw;
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

        public async Task<ResponseDTO> ElrTimeOutEvent(long queued_events_records_id, long emp_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                query = EventAssignDTO.time_out_reassign_event();
                await _db.Execute(query, new { queued_events_records_id = queued_events_records_id, modified_by = emp_id }, ELR_db);

                query = QueuedAnomalyDTO.reset_event_status();
                await _db.Execute(query, new { queued_events_records_id = queued_events_records_id }, ELR_db);

                return new ResponseDTO(true, "Event reassigned successfully");
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<ResponseDTO> ElrPatientCurrentStrip(long queued_events_records_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (queued_events_records_id <= 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = CurrentStripDTO.get_current_pateint_strip();

                var data = await _db.QuerySingleOrDefault<CurrentStripDTO>(query, new { queued_events_records_id = queued_events_records_id }, ELR_db);

                if (data == null)
                    return new ResponseDTO(false, "No current strip found");

                if (data != null)
                {
                    query = SubStripEventDTO.get_sub_strip();

                    data.sub_strip = (await _db.SelectQuery<SubStripEventResDTO>(query, new { queued_events_records_id = queued_events_records_id }, ELR_db)).ToList();
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
                if (data == null) return new ResponseDTO(false, "No Data");
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
                return new ResponseDTO(false, ex.Message);
            }
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
                return new ResponseDTO(false, ex.Message);
            }
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
                return new ResponseDTO(false, ex.Message);
            }
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
                var data = await _db.QuerySingle<ParentStripDTO>(query, new { sub_strip_event_id = obj.sub_strip_event_id }, ELR_db);
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.ToString());
                return new ResponseDTO(false, ex.Message);
            }

        }
    }
}
