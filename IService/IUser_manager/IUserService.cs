using mct_API.DTO.ReqDTO;
using mct_API.DTO.ResDTO;

namespace mct_API.IService.IUser_manager
{
    public interface IUserService
    {
        public Task<ResponseDTO> MasterEmployeeLogin(MasterEmployeeLoginReqDTO obj);
    }
}
