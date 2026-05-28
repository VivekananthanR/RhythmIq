using mct_API.DTO.ReqDTO;
using mct_API.DTO.ResDTO;
using mct_API.IService.IUser_manager;
using Microsoft.AspNetCore.Mvc;

namespace mct_API.Controllers.User_manager
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

        #region HttpPost = api/User/MasterEmployeeLogin
        [HttpPost("MasterEmployeeLogin")]
        public async Task<ActionResult<ResponseDTO>> MasterEmployeeLogin(MasterEmployeeLoginReqDTO obj)
        {
            return await _userService.MasterEmployeeLogin(obj);
        }
        #endregion
    }
}
