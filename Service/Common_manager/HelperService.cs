using elr_API.IService.ICommon_manager;
using elr_API.Service.User_manager;
using System.Text.Json;

namespace elr_API.Service.Common_manager
{
    public class HelperService : IHelperService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string MeterboxApiBaseUrl = "http://api.chchealthcare.in/MobileAPI/api/MIS/GetEmployeeDetails?EmployeeId=";

        public HelperService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<EmployeeMeterboxDetails> GetEmployeeDetails(string emp_code)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = $"{MeterboxApiBaseUrl}{emp_code}";
            var response = await client.GetAsync(apiUrl);
            var responseBody = await response.Content.ReadAsStringAsync();
            var employeeResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return employeeResponse.data.FirstOrDefault();
        }
    }
}
