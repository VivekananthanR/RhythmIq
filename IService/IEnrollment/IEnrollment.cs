using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;

namespace elr_API.IService.IEntrolement
{
    public interface IEnrollment
    {
        Task<ResponseDTO> ElrGetValidEnrollmentId(long patient_enrollment_id);
        Task<ResponseDTO> ElrValidDeviceId(string device_no);
        Task<ResponseDTO> ElrValidPatchId(string patch_no);
        Task<ResponseDTO> ElrUploadPatientDevice(PatientEnrollmentDTO obj);
        Task<ResponseDTO> ElrUploadSignature(PatientEnrollmentDTO obj);
        Task<ResponseDTO> ElrStartHookUpPatient(PatientEnrollmentReqDTO obj);
        Task<ResponseDTO> ElrSymtomsList();
        Task<ResponseDTO> ElrAddSymtoms(SymtomsDTO obj);
        Task<ResponseDTO> ElrCompleteHookUpPatient(long patient_enrollment_id);
        Task<ResponseDTO> ElrAddUpdatePatientEnrollment(PatientEnrollmentDTO obj);
        Task<ResponseDTO> ElrEnrollmentCreationList(long? patient_enrollment_id);
        Task<ResponseDTO> ElrUploadEnrollmentRawData(EnrollmentRawDataDTO obj);
        Task ElrPhythonProcessEnrollmentRawData();
        Task<ResponseDTO> ElrGetRawDataList();
    }
}
