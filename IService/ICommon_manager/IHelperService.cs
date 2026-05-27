using mct_API.Service.User_manager;

namespace mct_API.IService.ICommon_manager
{
    public interface IHelperService
    {
        Task<EmployeeMeterboxDetails> GetEmployeeDetails(string emp_code);
    }
}
