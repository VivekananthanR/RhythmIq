using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.DTO.ResDTO.Token;
using elr_API.IService.IHome_manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace elr_API.Controllers.Home_manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        #region HttpGet = api/Home/ElrGetDevice 

        [HttpPost("ElrGetDevice")]
        public async Task<ActionResult<ResponseDTO>> ElrGetDevice(DeviceReqDTO obj)
        {
            return await _homeService.ElrGetDevice(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrAddUpdateDevice
        [HttpPost("ElrAddUpdateDevice"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdateDevice([FromHeader] string authorization, DeviceResDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrAddUpdateDevice(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrAddUpdatePatch
        [HttpPost("ElrAddUpdatePatch"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdatePatch([FromHeader] string authorization, PatchResDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrAddUpdatePatch(obj);
        }
        #endregion




        #region HttpGet = api/Home/ElrGetMenu
        [HttpGet("ElrGetMenu"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetMenu()
        {
            return await _homeService.ElrGetMenu();
        }
        #endregion  

        #region HttpPost = api/Home/ElrAddUpdateMenu
        [HttpPost("ElrAddUpdateMenu"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdateMenu([FromHeader] string authorization, MenuDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrAddUpdateMenu(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrCheckPermissionMenuUpdate
        [HttpPost("ElrCheckPermissionMenuUpdate"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrCheckPermissionMenuUpdate(MenuDTO[] obj)
        {
            return await _homeService.ElrCheckPermissionMenuUpdate(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrUpdateMenuPriority
        [HttpPost("ElrUpdateMenuPriority")]
        public async Task<ActionResult<ResponseDTO>> ElrUpdateMenuPriority(MenuDTO[] obj)
        {
            return await _homeService.ElrUpdateMenuPriority(obj);
        }
        #endregion

        #region HttpPost = api/Home/GetCountry
        [HttpGet("GetCountry")]
        public async Task<ActionResult<ResponseDTO>> GetCountry()
        {
            return await _homeService.GetCountry();
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateCountry
        [HttpPost("AddUpdateCountry"), Authorize]
        public async Task<ActionResult<ResponseDTO>> AddUpdateCountry(CountryDTO obj)
        {
            return await _homeService.AddUpdateCountry(obj);
        }
        #endregion

        #region HttpPost = api/Home/GetState
        [HttpGet("GetState")]
        public async Task<ActionResult<ResponseDTO>> GetState()
        {
            return await _homeService.GetState();
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateState
        [HttpPost("AddUpdateState"), Authorize]
        public async Task<ActionResult<ResponseDTO>> AddUpdateState(StateDTO obj)
        {
            return await _homeService.AddUpdateState(obj);
        }
        #endregion

        #region HttpPost = api/Home/GetDistrict
        [HttpGet("GetDistrict")]
        public async Task<ActionResult<ResponseDTO>> GetDistrict()
        {
            return await _homeService.GetDistrict();
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateDistrict
        [HttpPost("AddUpdateDistrict"), Authorize]
        public async Task<ActionResult<ResponseDTO>> AddUpdateDistrict(DistrictDTO obj)
        {
            return await _homeService.AddUpdateDistrict(obj);
        }
        #endregion

        #region HttpPost= api/Home/GetUserType
        [HttpGet("GetUserType"), Authorize]
        public async Task<ActionResult<ResponseDTO>> GetUserType()
        {
            return await _homeService.GetUserType();
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateUserType
        [HttpPost("AddUpdateUserType"), Authorize]
        public async Task<ActionResult<ResponseDTO>> AddUpdateUserType([FromHeader] string authorization, UserTypeDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.AddUpdateUserType(obj);
        }
        #endregion

        #region HttpGet= api/User/ElrHospitalList?hospital_name= 
        [HttpGet("ElrHospitalList")]
        public async Task<ActionResult<ResponseDTO>> ElrHospitalList(string? hospital_name)
        {
            return await _homeService.ElrHospitalList(hospital_name);
        }
        #endregion

        //#region HttpPost = api/Home/GetHospital
        //[HttpPost("GetHospital")]
        //public async Task<ActionResult<ResponseDTO>> GetHospital(HospitalDTO obj)
        //{
        //    return await _homeService.GetHospital(obj);
        //}
        //#endregion

        #region HttpPost = api/Home/AddUpdateHospital
        [HttpPost("AddUpdateHospital"), Authorize]
        public async Task<ActionResult<ResponseDTO>> AddUpdateHospital([FromHeader] string authorization, AddUpdateHospitalReqDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.AddUpdateHospital(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrGetTest
        [HttpGet("ElrGetTest")]
        public async Task<ActionResult<ResponseDTO>> ElrGetTest()
        {
            return await _homeService.ElrGetTest();
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateTest
        [HttpPost("AddUpdateTest"), Authorize]
        public async Task<ActionResult<ResponseDTO>> AddUpdateTest([FromHeader] string authorization, AddUpdateTestDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.AddUpdateTest(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrGetTestPrices
        [HttpGet("ElrGetTestPrices"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetTestPrices()
        {
            return await _homeService.ElrGetTestPrices();
        }
        #endregion

        #region HttpPost = api/Home/ElrGetTestPriceByDays
        [HttpPost("ElrGetTestPriceByDays")]
        public async Task<ActionResult<ResponseDTO>> ElrGetTestPriceByDays(AddUpdateTestPricesDTO obj)
        {
            return await _homeService.ElrGetTestPriceByDays(obj);
        }
        #endregion


        #region HttpPost = api/Home/AddUpdateTestPrices
        [HttpPost("AddUpdateTestPrices"), Authorize]
        public async Task<ActionResult<ResponseDTO>> AddUpdateTestPrices([FromHeader] string authorization, AddUpdateTestPricesDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.AddUpdateTestPrices(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrAddUpdateSymtomType
        [HttpPost("ElrAddUpdateSymtomType"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdateSymtomType([FromHeader] string authorization, SymtomTypeDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrAddUpdateSymtomType(obj);
        }
        #endregion


        #region HttpGet = api/Home/ElrSymtomsTypeList
        [HttpGet("ElrSymtomsTypeList")]
        public async Task<ResponseDTO> ElrSymtomsTypeList()
        {
            return await _homeService.ElrSymtomsTypeList();
        }
        #endregion


        #region HttpPost = api/Home/GetLocation
        [HttpPost("GetLocation")]
        public async Task<ActionResult<ResponseDTO>> GetLocation(LocationDTO obj)
        {
            return await _homeService.GetLocation(obj);
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateLocation
        [HttpPost("AddUpdateLocation"), Authorize]
        public async Task<ActionResult<ResponseDTO>> AddUpdateLocation([FromHeader] string authorization, LocationDTO obj)
        {

            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.AddUpdateLocation(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrUploadProcessedData
        [HttpPost("ElrUploadProcessedData")]
        public async Task<ActionResult<ResponseDTO>> ElrUploadProcessedData([FromForm] ProcessedRawDataDTO obj)
        {
            return await _homeService.ElrUploadProcessedData(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrQueueAnomalyList
        [HttpPost("ElrQueueAnomalyList")]
        public async Task<ActionResult<ResponseDTO>> ElrQueueAnomalyList(QueuedAnomalyReqDTO obj)
        {
            return await _homeService.ElrQueueAnomalyList(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrUploadQueueAnomaly
        [HttpPost("ElrUploadQueueAnomaly")]
        public async Task<ActionResult<ResponseDTO>> ElrUploadQueueAnomaly([FromForm] QueuedAnomalyDTO obj)
        {
            return await _homeService.ElrUploadQueueAnomaly(obj);
        }
        #endregion

        #region  HttpPost = api/Home/ElrCloseEvent?eventAssignId=
        [HttpPost("ElrCloseEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrCloseEvent([FromHeader] string authorization, long eventAssignId)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);

            return await _homeService.ElrCloseEvent(eventAssignId, tokenClaims.id);
        }
        #endregion

        #region HttpPost = api/Home/ElrQueueNextEvent?queued_events_records_id=
        [HttpPost("ElrQueueNextEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrQueueNextEvent([FromHeader] string authorization, long queued_events_records_id)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrQueueNextEvent(tokenClaims.id, queued_events_records_id);
        }
        #endregion

        #region HttpGet= api/Home/ElrGetNextEvent
        [HttpGet("ElrGetNextEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetNextEvent([FromHeader] string authorization)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);

            return await _homeService.ElrGetNextEvent(tokenClaims.id);
        }
        #endregion


        #region HttpGet= api/Home/ElrTimeOutEvent?event_assign_id=
        [HttpGet("ElrTimeOutEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrTimeOutEvent([FromHeader] string authorization, long event_assign_id)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrTimeOutEvent(event_assign_id, tokenClaims.id);
        }
        #endregion

        #region HttpGet= api/Home/ElrPatientCurrentStrip?event_assign_id=
        [HttpGet("ElrPatientCurrentStrip"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrPatientCurrentStrip(long event_assign_id)
        {
            return await _homeService.ElrPatientCurrentStrip(event_assign_id);
        }
        #endregion

        #region HttpPost= api/Home/ElrSubStripEvent
        [HttpPost("ElrSubStripEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrSubStripEvent([FromHeader] string authorization, [FromForm] SubStripEventDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrSubStripEvent(obj);
        }
        #endregion

        #region HttpPost= api/Home/ElrInActiveSubStrip
        [HttpPost("ElrInActiveSubStrip"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrInActiveSubStrip([FromHeader] string authorization, long sub_strip_event_id)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrInActiveSubStrip(sub_strip_event_id, tokenClaims.id);
        }
        #endregion

        #region HttpPost= api/Home/ElrSubStripReport
        [HttpPost("ElrSubStripReport"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrSubStripReport(SubStripReqDTO obj)
        {
            return await _homeService.ElrSubStripReport(obj);
        }
        #endregion

        #region HttpPost= api/Home/ElrAddUpdateFindingEvent
        [HttpPost("ElrAddUpdateFindingEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdateFindingEvent([FromHeader] string authorization, FindingEventDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrAddUpdateFindingEvent(obj);
        }
        #endregion

        #region HttpGet= api/Home/ElrFindingList
        [HttpGet("ElrFindingList")]
        public async Task<ActionResult<ResponseDTO>> ElrFindingList()
        {
            return await _homeService.ElrFindingList();
        }
        #endregion

        #region HttpGet= api/Home/ElrGetParentStrip
        [HttpGet("ElrGetParentStrip")]
        public async Task<ActionResult<ResponseDTO>> ElrGetParentStrip(ParentStripDTO obj)
        {
            return await _homeService.ElrGetParentStrip(obj);
        }
        #endregion

        #region HttpPost= api/Home/ElrPushToQcEvent
        [HttpPost("ElrPushToQcEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrPushToQcEvent([FromHeader] string authorization, PushToQCDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrPushToQcEvent(obj);
        }
        #endregion

        #region HttpPost= api/Home/ElrCompletedFindingList
        [HttpPost("ElrCompletedFindingList"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrCompletedFindingList(CompletedFindingListReqDTO obj)
        {
            return await _homeService.ElrCompletedFindingList(obj);
        }
        #endregion


        #region HttpGet= api/Home/ElrGetCompleteQc
        [HttpGet("ElrGetCompleteQc")]
        public async Task<ActionResult<ResponseDTO>> ElrGetCompleteQc(long detection_id)
        {
            return await _homeService.ElrGetCompleteQc(detection_id);
        }
        #endregion

        #region HttpGet= api/Home/ElrGetNextQcDetection
        [HttpGet("ElrGetNextQcDetection"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetNextQcDetection()
        {
            return await _homeService.ElrGetNextQcDetection();
        }
        #endregion

        #region HttpPost = api/Home/ElrUpdateSubmitQcEvent
        [HttpPost("ElrUpdateSubmitQcEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrUpdateSubmitQcEvent([FromHeader] string authorization, ConfirmSubmitDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrUpdateSubmitQcEvent(obj);
        }
        #endregion

        #region HttpPost = api/Home/PickDetection
        [HttpPost("ElrQcPickDetection"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrQcPickDetection([FromHeader] string authorization, ConfirmSubmitDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.picked_by = tokenClaims.id;
            return await _homeService.ElrQcPickDetection(obj);
        }
        #endregion


        #region HttpPost = api/Home/ElrCompletePriorityReport
        [HttpPost("ElrCompletePriorityReport"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrCompletePriorityReport(PriorityCompleteReportReqDTO obj)
        {
            return await _homeService.ElrCompletePriorityReport(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrActivityLogList
        [HttpPost("ElrActivityLogList"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrActivityLogList([FromHeader] string authorization, ActivityLogReqDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrActivityLogList(obj);
        }
        #endregion



        //Testing event assign status
        #region HttpGet = api/Home/ElrGetByIdEventAssign
        [HttpGet("ElrGetByIdEventAssign"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetByIdEventAssign([FromHeader] string authorization)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrGetByIdEventAssign(tokenClaims.id);
        }
        #endregion

        #region HttpPost = api/Home/ElrFetchECG
        [HttpPost("ElrFetchECG"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrFetchECG([FromHeader] string authorization, FetchECGReqDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrFetchECG(obj);
        }
        #endregion


        #region HttpGet = api/Home/ElrGetFetchHistory?queued_events_records_id=
        [HttpGet("ElrGetFetchHistory"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetFetchHistory(long queued_events_records_id)
        {
            return await _homeService.ElrGetFetchHistory(queued_events_records_id);
        }
        #endregion

        #region HttpPost = api/Home/ElrFetchHistoryList
        [HttpPost("ElrFetchHistoryList"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrFetchHistoryList(FectchHistoryReqDTO obj)
        {
            return await _homeService.ElrFetchHistoryList(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrFetchHistoryExcel
        [HttpPost("ElrFetchHistoryExcel"), Authorize]
        public async Task<ActionResult<byte[]>> ElrFetchHistoryExcel(FectchHistoryReqDTO obj)
        {
            return await _homeService.ElrFetchHistoryExcel(obj);
        }
        #endregion

        #region HttpGet = api/Home/ElrGetByIdFetchHistory?queued_events_records_id =
        [HttpGet("ElrGetByIdFetchHistory"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetByIdFetchHistory(long queued_events_records_id)
        {
            return await _homeService.ElrGetByIdFetchHistory(queued_events_records_id);
        }
        #endregion    


        #region HttpGet = api/Home/ElrMyProgress?type =
        [HttpGet("ElrMyProgress"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrMyProgress([FromHeader] string authorization, string type, string progress_type)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrMyProgress(tokenClaims.id, type, progress_type);
        }
        #endregion

        #region HttpGet = api/Home/ElrGetReportType
        [HttpGet("ElrGetReportType"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetReportType()
        {
            return await _homeService.ElrGetReportType();
        }
        #endregion


        #region HttpPost = api/Home/ElrAddUpdateReportType
        [HttpPost("ElrAddUpdateReportType"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdateReportType([FromHeader] string authorization, ReportTypeDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrAddUpdateReportType(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrGetEventEcgSearch
        [HttpPost("ElrGetEventEcgSearch"), Authorize]
        public async Task<ResponseDTO> ElrGetEventEcgSearch(EventEcgSearchReqDTO obj)
        {
            return await _homeService.ElrGetEventEcgSearch(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrGetPatientReportList
        [HttpPost("ElrGetPatientReportList"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetPatientReportList(PatientReportReqDTO obj)
        {
            return await _homeService.ElrGetPatientReportList(obj);
        }
        #endregion


        #region HttpGet = api/Home/ElrGetNextPatientReport
        [HttpGet("ElrGetNextPatientReport"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetNextPatientReport([FromHeader] string authorization)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrGetNextPatientReport(tokenClaims.id);
        }
        #endregion

        #region HttpGet = api/Home/ElrGetNextPatientReportList
        [HttpGet("ElrGetNextPatientReportList"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetNextPatientReportList([FromHeader] string authorization, long patient_report_id)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrGetNextPatientReportList(patient_report_id, tokenClaims.id);
        }
        #endregion

        #region HttpPost = api/Home/ElrClosePatientReport
        [HttpGet("ElrClosePatientReport"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrClosePatientReport([FromHeader] string authorization, long report_assigned_id)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrClosePatientReport(report_assigned_id, tokenClaims.id);
        }
        #endregion


        #region HttpPost = api/Home/ElrAddUpdateReportGenerate
        [HttpPost("ElrAddUpdateReportGenerate"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdateReportGenerate([FromHeader] string authorization, PatientReportGenerateReqDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrAddUpdateReportGenerate(obj);
        }
        #endregion

        #region HttpGet = api/Home/ElrGetReportGenerateList?patient_id=
        [HttpGet("ElrGetReportGenerateList"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetReportGenerateList(long patient_id)
        {
            return await _homeService.ElrGetReportGenerateList(patient_id);
        }
        #endregion

        #region HttpPost = api/Home/ElrPatientReportSearchList
        [HttpPost("ElrPatientReportSearchList"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrPatientReportSearchList(PatientReportSearchReqDTO obj)
        {
            return await _homeService.ElrPatientReportSearchList(obj);
        }
        #endregion


        #region HttpPost = api/Home/ElrPublishReport
        [HttpPost("ElrPublishReport"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrPublishReport([FromHeader] string authorization, PatientReportGenerateReqDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;

            return await _homeService.ElrPublishReport(obj);
        }
        #endregion



    }

}
