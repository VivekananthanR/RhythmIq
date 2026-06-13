using elr_API.Service.User_manager;

namespace elr_API.IService.ICommon_manager
{
    public interface IHelperService
    {
        Task<EmployeeMeterboxDetails> GetMeterBoxEmployeeDetails(string emp_code);
        Task<ConnectPatientDetails> GetConnectPatientDetails(string phone_no);
    }
}
