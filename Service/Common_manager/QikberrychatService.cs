using elr_API.Common;
using elr_API.DTO.ReqDTO;
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

                //List<Parameter> bodyparameters = new List<Parameter>();
                //bodyparameters.Add(new Parameter { type = "text", text = obj.otp.Value.ToString() });


                //WhatsappBasicReqDTO whatsappBasicReqDTO = new WhatsappBasicReqDTO();
                //whatsappBasicReqDTO.phone_no = obj.phone_no;
                //whatsappBasicReqDTO.template_name = "otp_for_logging"; ;
                //whatsappBasicReqDTO.bodyParameters = bodyparameters;

                //await WhatsappMessageCallAPI(whatsappBasicReqDTO);

                #endregion


                #region SMS API Calling

                SMSReqDTO sMSReqDTO = new SMSReqDTO()
                {
                    to = "91" + obj.phone_no,
                    template_id = "1107173838787487103",
                    message = $@"Vanakkam! Your OTP is {obj.otp} for looking in to Your Healthwatch Telediagnostics. Do NOT share it with anyone."";"
                };
                await SMSMessageCallAPI(sMSReqDTO);

                #endregion
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO { Status = false, Message = ErrorMessage };
        }


        #region Whatsapp API Method
        public async Task<WhatsappResDTO> WhatsappMessageCallAPI(WhatsappBasicReqDTO obj)
        {
            string query = string.Empty;
            try
            {
                List<Component> components = new List<Component>();

                if (obj.headerParameters != null && obj.headerParameters.Count() > 0)
                {
                    components.Add(new Component { type = "header", parameters = obj.headerParameters });
                }
                if (obj.bodyParameters != null && obj.bodyParameters.Count() > 0)
                {
                    components.Add(new Component { type = "body", parameters = obj.bodyParameters });

                }

                Template template = new Template();
                template.name = obj.template_name;
                template.language = "en";
                template.components = components;

                WhatsappReqDTO whatsappReqDTO = new WhatsappReqDTO();

                whatsappReqDTO.to_contact = "+91" + obj.phone_no;
                whatsappReqDTO.type = "template";
                whatsappReqDTO.template = template;




                var client = _httpClientFactory.CreateClient();

                var jsonRequestBody = JsonSerializer.Serialize(whatsappReqDTO);


                var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add("QIKCHAT-API-KEY", _configuration["Qikberrychat:QIKCHAT-API-KEY"]);


                var apiUrl = "https://api.qikchat.in/v1/messages";
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    var tokenResponse = JsonSerializer.Deserialize<WhatsappResDTO>(responseBody);

                    if (tokenResponse != null && tokenResponse.status && tokenResponse.data != null)
                    {
                        return new WhatsappResDTO(tokenResponse.data);
                    }

                    return new WhatsappResDTO(false, ResponseMessage.Failed);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return new WhatsappResDTO(false, "API call failed: " + errorResponse);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ex.ToString());
                ErrorMessage = ResponseMessage.Please_Try_Again;
            }

            return new WhatsappResDTO(false, ErrorMessage);
        }
        #endregion

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
