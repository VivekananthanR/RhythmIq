using elr_API.Common;
using elr_API.IService.ICommon_manager;
using elr_API.Service.User_manager;
using System.Text.Json;

namespace elr_API.Service.Common_manager
{
    public class HelperService : IHelperService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string MeterboxApiBaseUrl = "http://api.chchealthcare.in/MobileAPI/api/MIS/GetEmployeeDetails?EmployeeId=";
        private readonly string ConnectApiBaseUrl = "http://192.168.97.68:2010/api/Dashboard/GetConnectPatientDetails?mobile_no=";
        private readonly string ConnectApiBaseUrl_live = "https://apihw.chchealthcare.in/MobileAPI/api/Dashboard/GetConnectPatientDetails?mobile_no=";



        public HelperService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<EmployeeMeterboxDetails> GetMeterBoxEmployeeDetails(string emp_code)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = $"{MeterboxApiBaseUrl}{emp_code}";
            var response = await client.GetAsync(apiUrl);
            var responseBody = await response.Content.ReadAsStringAsync();
            var employeeResponse = JsonSerializer.Deserialize<MeterBoxApiResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return employeeResponse.data.FirstOrDefault();
        }

        public async Task<ConnectPatientDetails?> GetConnectPatientDetails(string mobile_no)
        {
            var client = _httpClientFactory.CreateClient();

            var apiUrl = CommonFunctions.DataOnLive() ? $"{ConnectApiBaseUrl_live}{mobile_no}" : $"{ConnectApiBaseUrl}{mobile_no}";

            var response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync();

            var patientResponse = JsonSerializer.Deserialize<ConnectApiResponse>(
                responseBody,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (patientResponse == null || string.IsNullOrWhiteSpace(patientResponse.data)) return null;

            string decryptedJson = ResponseEncrypt.Decrypt(patientResponse.data);

            var patients = JsonSerializer.Deserialize<List<ConnectPatientDetails>>(
                decryptedJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return patients?.FirstOrDefault();
        }
    }
}
