using elr_API.DTO.ReqDTO.UserReq;
using elr_API.DTO.ResDTO;
using elr_API.IService.IUser_manager;
using Microsoft.AspNetCore.Mvc;


namespace elr_API.Controllers.User_manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        #region HttpPost = api/User/MasterEmployeeLogin
        [HttpPost("MasterEmployeeLogin")]
        public async Task<ActionResult<ResponseDTO>> MasterEmployeeLogin(MasterEmployeeLoginReqDTO obj)
        {
            return await _userService.MasterEmployeeLogin(obj);
        }
        #endregion
    }
}
