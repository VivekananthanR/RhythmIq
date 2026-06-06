using elr_API.Service.User_manager;

namespace elr_API.IService.ICommon_manager
{
    public interface IHelperService
    {
        Task<EmployeeMeterboxDetails> GetEmployeeDetails(string emp_code);
    }
}
