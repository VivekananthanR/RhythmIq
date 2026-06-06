using elr_API.Common;
using elr_API.DTO.ReqDTO.QikberrychatReq;
using elr_API.DTO.ResDTO;
using elr_API.IService.ICommon_manager;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace elr_API.Service.Common_manager
{
    public class QikberrychatService : IQikberrychatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public string ErrorMessage = string.Empty;

        public QikberrychatService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<ResponseDTO> OtpForLoggingPatient(OtpForLoggingReqDTO obj)
        {
            try
            {

                #region Whatsapp API calling

                #endregion

                #region SMS API Calling

                SMSReqDTO sMSReqDTO = new SMSReqDTO()
                {
                    to = "91" + obj.phone_no,
                    template_id = "1107174626427852375",
                    message = $@"Hello from Healthwatch Telediagnostics! Your OTP for mobile validation is {obj.otp}"
                };
                await SMSMessageCallAPI(sMSReqDTO);
                //await Task.Run(() =>
                //{
                //    SMSMessageCallAPI(sMSReqDTO).Wait();
                //});
                #endregion
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO { Status = false, Message = ErrorMessage };
        }

        #region SMS API Calling
        public async Task<SMSResDTO> SMSMessageCallAPI(SMSReqDTO obj)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var jsonReqBody = JsonSerializer.Serialize(obj);
                var content = new StringContent(jsonReqBody, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["Qikberrychat:Bearer-Token"]);

                var apiUrl = "https://rest.qikberry.ai/v1/sms/messages";
                var response = await client.PostAsync(apiUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                if ((response.IsSuccessStatusCode))
                {
                    var tokenResponse = JsonSerializer.Deserialize<SMSResDTO>(responseContent);
                    if (tokenResponse != null && tokenResponse.error == null)
                    {
                        return tokenResponse;
                    }
                    return new SMSResDTO("false", new Error { to = "Unexpected API response" });
                }
                else
                {
                    return new SMSResDTO("false", new Error { to = "API call failed: " + responseContent });
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                return new SMSResDTO("false", new Error { to = ResponseMessage.Please_Try_Again });
            }

        }

        #endregion
    }
}
