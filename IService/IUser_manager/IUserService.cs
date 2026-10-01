using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.DTO.ResDTO.Token;

namespace elr_API.IService.IUser_manager
{
    public interface IUserService
    {

        Task<ResponseDTO> ElrGetPatient(string phone_no);
        Task<ResponseDTO> ElrEmployeeLogin(MasterEmployeeLoginReqDTO obj);
        Task<bool> TokenValidation(TokenValidationReqDTO obj);
        Task<ResponseDTO> ElrAddUpdatePatientDetails(PatientVerifyLoginResDTO obj);
        Task<ResponseDTO> ElrPatientVerifyLogin(PatientVerifyLoginReqDTO obj);
        Task<ResponseDTO> ElrGetEmployeeNameList(string user_name);
        Task<ResponseDTO> ElrGetEmployee(string emp_code);
        Task<ResponseDTO> ElrAddUpdateEmployeeDetails(AddUpdateEmployeeDetailsReqDTO obj);
        Task<ResponseDTO> ElrEmployeeLogOut(long user_id);
        Task<ResponseDTO> ElrphysiciansList();
        Task<ResponseDTO> ElrTechniciansList();
        Task<ResponseDTO> ElrGetPatientEventPage(long patient_id);

        Task<ResponseDTO> ElrGetPatientSearch(PatientEcgSearchReqDTO obj);
    }
}
