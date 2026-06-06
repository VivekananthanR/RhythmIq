using elr_API.DTO.ReqDTO.QikberrychatReq;
using elr_API.DTO.ResDTO;

namespace elr_API.IService.ICommon_manager
{
    public interface IQikberrychatService
    {
        Task<ResponseDTO> OtpForLoggingPatient(OtpForLoggingReqDTO obj);
    }
}
