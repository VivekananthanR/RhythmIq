namespace elr_API.DTO.ReqDTO.QikberrychatReq
{
    public class QikberrychatReqDTO
    {
    }

    public class OtpForLoggingReqDTO
    {
        public string phone_no { get; set; }
        public int otp { get; set; }
    }
    public class SMSReqDTO
    {
        public string to { get; set; }
        public string sender { get; set; } = "HWATCH";
        public string service { get; set; } = "SI";
        public string template_id { get; set; }
        public string message { get; set; }
        public string? shorten_url { get; set; } = "0";
    }

    public class SMSResDTO
    {
        public string message { get; set; }
        public Error? error { get; set; }
        public List<Datum>? data { get; set; }

        public SMSResDTO() { }

        public SMSResDTO(string message, Error Error)
        {
            error = Error;
            data = new List<Datum>();
        }

        public SMSResDTO(List<Datum> Data)
        {
            data = Data;
        }
    }

    public class Error
    {
        public string to { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public string message_id { get; set; }
        public string mobile { get; set; }
        public int length { get; set; }
        public int units { get; set; }
        public double charges { get; set; }
    }
}
