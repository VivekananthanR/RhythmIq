using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;

namespace elr_API.IService.IEntrolement
{
    public interface IEnrollment
    {
        Task<ResponseDTO> ElrGetValidEnrollmentId(long patient_enrollment_id);
        Task<ResponseDTO> ElrValidDeviceId(string device_no);
        Task<ResponseDTO> ElrValidPatchId(string patch_no);
        Task<ResponseDTO> ElrUploadPatientSignature(PatientEnrollmentDTO obj);
        Task<ResponseDTO> ElrStartHookUpPatient(long patient_enrollment_id);
        Task<ResponseDTO> ElrCompleteHookUpPatient(long patient_enrollment_id);
        Task<ResponseDTO> GetPatientEnrollment(PatientEnrollmentDTO obj);
        Task<List<PatientEnrollmentDTO>> PatientEnrollmentList(PatientEnrollmentDTO obj);
        Task<ResponseDTO> ElrAddUpdatePatientEnrollment(PatientEnrollmentDTO obj);
        Task<ResponseDTO> ElrUploadEnrollmentRawData(EnrollmentRawDataDTO obj);
        Task<ResponseDTO> GetRawDataList();
    }
}
