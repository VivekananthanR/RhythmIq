using mct_API.Common;

namespace mct_API.DTO.ResDTO
{
    public class ResponseDTO
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; } = Array.Empty<object>();

        public ResponseDTO(object data)
        {
            Data = ResponseEncrypt.EncryptData(data);
            //Data = CommonFunctions.DataOnLive() ? ResponseEncrypt.EncryptData(data) : data;
        }

        public ResponseDTO(bool status, string message)
        {
            Status = status;
            Message = message;
        }
        public ResponseDTO() { }
    }
}
