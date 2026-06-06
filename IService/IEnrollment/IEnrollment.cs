using elr_API.DTO.ReqDTO.EnrolmentReq;
using elr_API.DTO.ResDTO;

namespace elr_API.IService.IEntrolement
{
    public interface IEnrollment
    {
        Task<ResponseDTO> PatientVerifyLogin(PatientVerifyLoginReqDTO obj);
        Task<ResponseDTO> UploadEnrollementRawData(EnrollmentRawDataDTO obj);
        Task<ResponseDTO> GetRawDataList();
        Task<ResponseDTO> UpdateRawDataCheckStatus(EnrollmentRawDataDTO obj);
        Task<ResponseDTO> AddUpdateQueuedEvent(AddUpdateQueuedEventDTO obj);
        Task<ResponseDTO> GetQueuedEvent(AddUpdateQueuedEventDTO obj);
    }
}
