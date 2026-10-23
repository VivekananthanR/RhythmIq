using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;

namespace elr_API.IService.IDoctor
{
    public interface IDoctorService
    {
        Task<ResponseDTO> ElrDoctorLogin(PatientVerifyLoginReqDTO obj);

        Task<ResponseDTO> ElrDoctorLivePatientMonitoringList(DoctorLivePatientMonitoringReqDTO obj);
    }
}
