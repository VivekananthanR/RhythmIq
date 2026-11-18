using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;

namespace elr_API.IService.IHome_manager
{
    public interface IHomeService
    {
        Task<ResponseDTO> ElrGetDevice(DeviceReqDTO obj);
        Task<ResponseDTO> ElrAddUpdateDevice(DeviceResDTO obj);
        Task<ResponseDTO> ElrAddUpdatePatch(PatchResDTO obj);



        Task<ResponseDTO> ElrGetMenu();
        Task<ResponseDTO> ElrAddUpdateMenu(MenuDTO obj);

        Task<ResponseDTO> ElrCheckPermissionMenuUpdate(MenuDTO[] obj);
        Task<ResponseDTO> ElrUpdateMenuPriority(MenuDTO[] obj);


        Task<ResponseDTO> GetCountry();
        Task<ResponseDTO> AddUpdateCountry(CountryDTO obj);

        Task<ResponseDTO> GetState();
        Task<ResponseDTO> AddUpdateState(StateDTO obj);

        Task<ResponseDTO> GetDistrict();
        Task<ResponseDTO> AddUpdateDistrict(DistrictDTO obj);

        Task<ResponseDTO> GetUserType();
        Task<ResponseDTO> AddUpdateUserType(UserTypeDTO obj);

        Task<ResponseDTO> ElrHospitalList(string? hospital_name);
        Task<ResponseDTO> AddUpdateHospital(AddUpdateHospitalReqDTO obj);
        //Task<ResponseDTO> GetHospital(HospitalDTO obj);
        //Task<List<HospitalDTO>> HospitalList(HospitalDTO obj);

        Task<ResponseDTO> ElrGetTest();
        //Task<List<AddUpdateTestDTO>> TestList(AddUpdateTestDTO obj);
        Task<ResponseDTO> AddUpdateTest(AddUpdateTestDTO obj);

        Task<ResponseDTO> ElrGetTestPrices();
        Task<ResponseDTO> ElrGetTestPriceByDays(AddUpdateTestPricesDTO obj);
        //Task<List<AddUpdateTestPricesDTO>> TestPricesList(AddUpdateTestPricesDTO obj);
        Task<ResponseDTO> AddUpdateTestPrices(AddUpdateTestPricesDTO obj);

        Task<ResponseDTO> ElrAddUpdateSymtomType(SymtomTypeDTO obj);
        Task<ResponseDTO> ElrSymtomsTypeList();

        Task<ResponseDTO> ElrAddUpdatePatientActivityType(PatientActivityTypeDTO obj);
        Task<ResponseDTO> ElrPatientActivityTypeList();

        Task<ResponseDTO> GetLocation(LocationDTO obj);
        Task<List<LocationDTO>> LocationList(LocationDTO obj);
        Task<ResponseDTO> AddUpdateLocation(LocationDTO obj);

        Task<ResponseDTO> ElrUploadProcessedData(ProcessedRawDataDTO obj);

        Task<ResponseDTO> ElrQueueAnomalyList(QueuedAnomalyReqDTO obj);
        Task<ResponseDTO> ElrUploadQueueAnomaly(QueuedAnomalyDTO obj);

        Task<List<SymptomDTO>> ElrSymptomList(long queued_events_records_id);


        //Next event & Next qc event
        Task<ResponseDTO> ElrCloseEvent(long eventAssignId, long empId);
        Task<ResponseDTO> ElrQueueNextEvent(long emp_id, long queued_events_records_id);
        Task<ResponseDTO> ElrGetNextEvent(long emp_id);
        Task<GetNextEventResDTO?> IsProcessingEvent(long emp_id);
        Task<GetNextEventResDTO?> GetNext_Queue_Event_Details(long queued_events_records_id);
        Task<List<FetchHistoryDTO>> GetQueueFetchHistory(long queued_events_records_id);
        Task<long> ElrsSelectIsFinalAssignId(long emp_id);
        Task<int> ElrUpdateFinalCompleted(long eventAssignId, long empId);


        Task<ResponseDTO> ElrTimeOutEvent(long event_assign_id, long emp_id);
        Task<ResponseDTO> ElrPatientCurrentStrip(long event_assign_id);

        Task<ResponseDTO> ElrSubStripEvent(SubStripEventDTO obj);
        Task<ResponseDTO> ElrInActiveSubStrip(long sub_strip_event_id, long emp_id);
        Task<ResponseDTO> ElrSubStripReport(SubStripReqDTO obj);

        Task<ResponseDTO> ElrAddUpdateFindingEvent(FindingEventDTO obj);
        Task<ResponseDTO> ElrFindingList();

        Task<List<FindingEventDTO>> ElrGetChildFinding(long parent_finding_id);
        Task<ResponseDTO> ElrGetParentStrip(ParentStripDTO obj);

        Task<ResponseDTO> ElrPushToQcEvent(PushToQCDTO obj);
        Task<ResponseDTO> ElrCompletedFindingList(CompletedFindingListReqDTO obj);
        Task<ResponseDTO> ElrGetCompleteQc(long detection_id);
        Task<ResponseDTO> ElrGetNextQcDetection();
        Task<ResponseDTO> ElrUpdateSubmitQcEvent(ConfirmSubmitDTO obj);
        Task<ResponseDTO> ElrQcPickDetection(ConfirmSubmitDTO obj);
        Task<ResponseDTO> ElrCompletePriorityReport(PriorityCompleteReportReqDTO obj);

        Task<ResponseDTO> ElrAddUpdateActivityLog(ActivityLogReqDTO obj);
        Task<ResponseDTO> ElrActivityLogList(ActivityLogReqDTO obj);

        Task<ResponseDTO> ElrGetByIdEventAssign(long emp_id);

        Task<ResponseDTO> ElrFetchECG(FetchECGReqDTO obj);
        Task<ResponseDTO> ElrGetFetchHistory(long queued_events_records_id);
        Task<ResponseDTO> ElrFetchHistoryList(FectchHistoryReqDTO obj);
        Task<ResponseDTO> ElrGetByIdFetchHistory(long queued_events_records_id);
        Task<byte[]> ElrFetchHistoryExcel(FectchHistoryReqDTO obj);

        Task<ResponseDTO> ElrMyProgress(long emp_id, string type, string progress_type);

        Task<ResponseDTO> ElrGetReportType();
        Task<ResponseDTO> ElrAddUpdateReportType(ReportTypeDTO obj);

        Task<ResponseDTO> ElrGetEventEcgSearch(EventEcgSearchReqDTO obj);

        Task<ResponseDTO> ElrGetPatientReportList(PatientReportReqDTO obj);
        Task<ResponseDTO> ElrGetNextPatientReport(long emp_id);
        Task<ResponseDTO> ElrGetNextPatientReportList(long patient_report_id, long emp_id);
        Task<ResponseDTO> ElrClosePatientReport(long report_assigned_id, long emp_id);

        Task<ResponseDTO> ElrAddUpdateReportGenerate(PatientReportGenerateReqDTO obj);

        Task<ResponseDTO> ElrGetReportGenerateList(long patient_id);
        Task<ResponseDTO> ElrPatientReportSearchList(PatientReportSearchReqDTO obj);
        Task<ResponseDTO> ElrPublishReport(PatientReportGenerateReqDTO obj);






    }
}
