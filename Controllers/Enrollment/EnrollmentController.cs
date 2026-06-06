using elr_API.Common;
using elr_API.DTO.ReqDTO.EnrolmentReq;
using elr_API.DTO.ResDTO;
using elr_API.DTO.ResDTO.Token;
using elr_API.IService.IEntrolement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace elr_API.Controllers.Entrolement
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollment _enrollment;

        public EnrollmentController(IEnrollment enrollment)
        {
            _enrollment = enrollment;
        }
        #region HttpPost = api/Enrollment/PatientVerifyLogin
        [HttpPost("PatientVerifyLogin")]
        public async Task<ActionResult<ResponseDTO>> PatientVerifyLogin(PatientVerifyLoginReqDTO obj)
        {
            return await _enrollment.PatientVerifyLogin(obj);
        }
        #endregion

        #region HttpPost = api/Enrollment/UploadEnrollementRawData​
        [HttpPost("UploadEnrollementRawData"), Authorize]
        public async Task<ActionResult<ResponseDTO>> UploadEnrollementRawData([FromHeader] string Authorization, [FromForm] EnrollmentRawDataDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(Authorization);
            obj.created_by = tokenClaims.id;
            return await _enrollment.UploadEnrollementRawData(obj);
        }
        #endregion

        #region HttpGet = api/Enrollment/GetRawDataList
        [HttpGet("GetRawDataList")]
        public async Task<ActionResult<ResponseDTO>> GetRawDataList()
        {
            return await _enrollment.GetRawDataList();
        }
        #endregion

        #region HttpPost = api/Enrollment/UpdateRawDataCheckStatus
        [HttpPost("UpdateRawDataCheckStatus")]
        public async Task<ActionResult<ResponseDTO>> UpdateRawDataCheckStatus(EnrollmentRawDataDTO obj)
        {
            return await _enrollment.UpdateRawDataCheckStatus(obj);
        }
        #endregion
        #region HttpPost = api/Enrollment/AddUpdateQueuedEvent
        [HttpPost("AddUpdateQueuedEvent")]
        public async Task<ActionResult<ResponseDTO>> AddUpdateQueuedEvent([FromForm] AddUpdateQueuedEventDTO obj)
        {
            return await _enrollment.AddUpdateQueuedEvent(obj);
        }
        #endregion

        #region HttpGet = api/Enrollment/GetQueuedEvent
        [HttpGet("GetQueuedEvent")]
        public async Task<ActionResult<ResponseDTO>> GetQueuedEvent()
        {
            return await _enrollment.GetQueuedEvent();
        }
        #endregion
    }
}

