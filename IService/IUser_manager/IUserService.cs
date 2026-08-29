using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;

namespace elr_API.IService.IUser_manager
{
    public interface IUserService
    {

        Task<ResponseDTO> ElrGetPatient(string phone_no);
        Task<ResponseDTO> ElrAddUpdatePatientDetails(PatientVerifyLoginResDTO obj);
        Task<ResponseDTO> ElrPatientVerifyLogin(PatientVerifyLoginReqDTO obj);
        Task<ResponseDTO> ElrGetEmployeeNameList(string user_name);
        Task<ResponseDTO> ElrGetEmployee(string emp_code);
        Task<ResponseDTO> ElrAddUpdateEmployeeDetails(AddUpdateEmployeeDetailsReqDTO obj);
        Task<ResponseDTO> ElrEmployeeLogin(MasterEmployeeLoginReqDTO obj);
        Task<ResponseDTO> ElrEmployeeLogOut(long user_id);
        Task<ResponseDTO> ElrphysiciansList(string? physiciansName);
        Task<ResponseDTO> ElrTechniciansList(string? technicians_name);
        Task<ResponseDTO> ElrGetPatientEventPage(long patient_id);

        Task<ResponseDTO> ElrGetPatientSearch(PatientEcgSearchReqDTO obj);
    }
}
