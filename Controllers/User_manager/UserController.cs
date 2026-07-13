using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.DTO.ResDTO.Token;
using elr_API.IService.IUser_manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace elr_API.Controllers.User_manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region HttpGet=api/User/ElrGetPatient?phone_no= 
        [HttpGet("ElrGetPatient")]
        public async Task<ActionResult<ResponseDTO>> ElrGetPatient(string phone_no)
        {
            return await _userService.ElrGetPatient(phone_no);
        }
        #endregion

        #region HttpPost = api/User/ElrAddUpdatePatientDetails
        [HttpPost("ElrAddUpdatePatientDetails"), Authorize]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdatePatientDetails([FromHeader] string authorization, [FromBody] PatientVerifyLoginResDTO obj)
        {
            TokenClaims tokenClaims = Validation.ClaimValues(authorization);
            obj.created_by = tokenClaims.id;
            return await _userService.ElrAddUpdatePatientDetails(obj);
        }
        #endregion

        #region HttpPost = api/User/ElrPatientVerifyLogin
        [HttpPost("ElrPatientVerifyLogin")]
        public async Task<ActionResult<ResponseDTO>> ElrPatientVerifyLogin(PatientVerifyLoginReqDTO obj)
        {
            return await _userService.ElrPatientVerifyLogin(obj);
        }
        #endregion

        #region HttpGet=api/User/ElrGetEmployeeNameList?user_name= 
        [HttpGet("ElrGetEmployeeNameList")]
        public async Task<ActionResult<ResponseDTO>> ElrGetEmployeeNameList(string user_name)
        {
            return await _userService.ElrGetEmployeeNameList(user_name);
        }
        #endregion

        #region HttpGet=api/User/ElrGetEmployee?emp_code=
        [HttpGet("ElrGetEmployee")]
        public async Task<ActionResult<ResponseDTO>> ElrGetEmployee(string emp_code)
        {
            return await _userService.ElrGetEmployee(emp_code);
        }
        #endregion

        #region HttpPost = api/User/ElrAddUpdateEmployeeDetails
        [HttpPost("ElrAddUpdateEmployeeDetails")]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdateEmployeeDetails(AddUpdateEmployeeDetailsReqDTO obj)
        {
            return await _userService.ElrAddUpdateEmployeeDetails(obj);
        }
        #endregion

        #region HttpPost = api/User/ElrEmployeeLogin
        [HttpPost("ElrEmployeeLogin")]
        public async Task<ActionResult<ResponseDTO>> ElrEmployeeLogin(MasterEmployeeLoginReqDTO obj)
        {
            return await _userService.ElrEmployeeLogin(obj);
        }
        #endregion

        #region HttpPost = api/User/ElrphysiciansList?physiciansName =
        [HttpGet("ElrphysiciansList")]
        public async Task<ResponseDTO> ElrphysiciansList(string? physiciansName)
        {
            return await _userService.ElrphysiciansList(physiciansName);
        }
        #endregion

        #region HttpGet = api/User/ElrTechniciansList?technicians_name =
        [HttpGet("ElrTechniciansList")]
        public async Task<ResponseDTO> ElrTechniciansList(string? technicians_name)
        {
            return await _userService.ElrTechniciansList(technicians_name);
        }
        #endregion

        #region HttpGet = api/User/ElrGetPatientEventPage?patient_id=
        [HttpGet("ElrGetPatientEventPage")]
        public async Task<ActionResult<ResponseDTO>> ElrGetPatientEventPage(long patient_id)
        {
            return await _userService.ElrGetPatientEventPage(patient_id);
        }
        #endregion
    }
}
