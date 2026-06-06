using elr_API.DTO.ReqDTO.UserReq;
using elr_API.DTO.ResDTO;

namespace elr_API.IService.IUser_manager
{
    public interface IUserService
    {
        public Task<ResponseDTO> MasterEmployeeLogin(MasterEmployeeLoginReqDTO obj);
    }
}
