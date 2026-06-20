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

        #region HttpGet = api/Home/GetMenu
        [HttpGet("GetMenu")]
        public async Task<ActionResult<ResponseDTO>> GetMenu()
        {
            return await _homeService.GetMenu();
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateMenu
        [HttpPost("AddUpdateMenu")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateMenu(MenuDTO obj)
        {
            return await _homeService.AddUpdateMenu(obj);
        }
        #endregion

        #region HttpPost = api/Home/GetCountry
        [HttpPost("GetCountry")]
        public async Task<ActionResult<ResponseDTO>> GetCountry(CountryDTO obj)
        {
            return await _homeService.GetCountry(obj);
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateCountry
        [HttpPost("AddUpdateCountry")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateCountry(CountryDTO obj)
        {
            return await _homeService.AddUpdateCountry(obj);
        }
        #endregion

        #region HttpPost = api/Home/GetState
        [HttpPost("GetState")]
        public async Task<ActionResult<ResponseDTO>> GetState(StateDTO obj)
        {
            return await _homeService.GetState(obj);
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateState
        [HttpPost("AddUpdateState")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateState(StateDTO obj)
        {
            return await _homeService.AddUpdateState(obj);
        }
        #endregion

        #region HttpPost = api/Home/GetDistrict
        [HttpPost("GetDistrict")]
        public async Task<ActionResult<ResponseDTO>> GetDistrict(DistrictDTO obj)
        {
            return await _homeService.GetDistrict(obj);
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateDistrict
        [HttpPost("AddUpdateDistrict")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateDistrict(DistrictDTO obj)
        {
            return await _homeService.AddUpdateDistrict(obj);
        }
        #endregion

        #region HttpPost= api/Home/GetUserType
        [HttpPost("GetUserType")]
        public async Task<ActionResult<ResponseDTO>> GetUserType(UserTypeDTO obj)
        {
            return await _homeService.GetUserType(obj);
        }
        #endregion

        #region HttpPost = api/Home/AddUpdateUserType
        [HttpPost("AddUpdateUserType")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateUserType(UserTypeDTO obj)
        {
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
        [HttpPost("AddUpdateHospital")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateHospital(AddUpdateHospitalReqDTO obj)
        {
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
        [HttpPost("AddUpdateTest")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateTest(AddUpdateTestDTO obj)
        {
            return await _homeService.AddUpdateTest(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrGetTestPrices
        [HttpGet("ElrGetTestPrices")]
        public async Task<ActionResult<ResponseDTO>> ElrGetTestPrices(AddUpdateTestPricesDTO obj)
        {
            return await _homeService.ElrGetTestPrices(obj);
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
        [HttpPost("AddUpdateTestPrices")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateTestPrices(AddUpdateTestPricesDTO obj)
        {
            return await _homeService.AddUpdateTestPrices(obj);
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
        [HttpPost("AddUpdateLocation")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateLocation(LocationDTO obj)
        {
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

        #region HttpGet= api/Home/ElrGetNextEvent
        [HttpGet("ElrGetNextEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrGetNextEvent([FromHeader] string authorization)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrGetNextEvent(tokenClaims.id);
        }
        #endregion


        #region HttpGet= api/Home/ElrTimeOutEvent?queued_events_records_id=
        [HttpGet("ElrTimeOutEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrTimeOutEvent([FromHeader] string authorization, long queued_events_records_id)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            return await _homeService.ElrTimeOutEvent(queued_events_records_id, tokenClaims.id);
        }
        #endregion

        #region HttpGet= api/Home/ElrPatientCurrentStrip?queued_events_records_id=
        [HttpGet("ElrPatientCurrentStrip"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrPatientCurrentStrip(long queued_events_records_id)
        {
            return await _homeService.ElrPatientCurrentStrip(queued_events_records_id);
        }
        #endregion

        #region HttpPost= api/Home/ElrSubStripEvent
        [HttpPost("ElrSubStripEvent"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrSubStripEvent([FromHeader] string authorization, SubStripEventDTO obj)
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
        public async Task<ResponseDTO> ElrSubStripReport(SubStripReqDTO obj)
        {
            return await _homeService.ElrSubStripReport(obj);
        }
        #endregion

        #region HttpPost= api/Home/ElrAddUpdateFindingEvent
        [HttpPost("ElrAddUpdateFindingEvent"), Authorize]
        public async Task<ResponseDTO> ElrAddUpdateFindingEvent([FromHeader] string authorization, FindingEventDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _homeService.ElrAddUpdateFindingEvent(obj);
        }
        #endregion
    }

}
