using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.DTO.ResDTO.Token;
using elr_API.IService.IDoctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace elr_API.Controllers.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }


        #region HttpPost = api/Doctor/ElrDoctorLogin
        [HttpPost("ElrDoctorLogin")]
        public async Task<ActionResult<ResponseDTO>> ElrDoctorLogin(PatientVerifyLoginReqDTO obj)
        {
            return await _doctorService.ElrDoctorLogin(obj);
        }
        #endregion

        #region HttpPost = api/Doctor/ElrDoctorLivePatientMonitoringList
        [HttpPost("ElrDoctorLivePatientMonitoringList"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrDoctorLivePatientMonitoringList([FromHeader] string authorization, DoctorLivePatientMonitoringReqDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.referred_physician_id = tokenClaims.id;

            return await _doctorService.ElrDoctorLivePatientMonitoringList(obj);
        }
        #endregion
    }
}
