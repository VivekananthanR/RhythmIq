namespace elr_API.DTO.ReqDTO
{
    public class QikberrychatReqDTO
    {
    }

    public class OtpForLoggingReqDTO
    {
        public string phone_no { get; set; }
        public int? otp { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Component
    {
        public string type { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class Document
    {
        public string link { get; set; }
        public string filename { get; set; }
    }

    public class Image
    {
        public string link { get; set; }
    }

    public class Parameter
    {
        public string type { get; set; }
        public string? text { get; set; }
        public Image? image { get; set; }
        public Video? video { get; set; }
        public Document? document { get; set; }
    }

    public class WhatsappReqDTO
    {
        public string to_contact { get; set; }
        public string type { get; set; }
        public Template template { get; set; }
    }

    public class Template
    {
        public string name { get; set; }
        public string language { get; set; }
        public List<Component> components { get; set; }
    }

    public class Video
    {
        public string link { get; set; }
    }


    public class WhatsappBasicReqDTO
    {
        public string phone_no { get; set; }
        public string template_name { get; set; }
        public List<Parameter> bodyParameters { get; set; }
        public List<Parameter>? headerParameters { get; set; }
    }



    public class WhatsappResDTO
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<WhatsappMessageData> data { get; set; }

        public WhatsappResDTO() { }

        public WhatsappResDTO(bool Status, string Message = "")
        {
            status = Status;
            message = Message;
            data = new List<WhatsappMessageData>();
        }

        public WhatsappResDTO(List<WhatsappMessageData> Data)
        {
            status = true;
            message = "Success";
            data = Data;
        }
    }

    public class WhatsappMessageData
    {
        public string Id { get; set; }
        public string Channel { get; set; }
        public string From { get; set; }
        public string Recipient { get; set; }
        public double Credits { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }


    public class SMSReqDTO
    {
        public string to { get; set; }
        public string sender { get; set; } = "CHCPHY";
        public string service { get; set; } = "SI";
        public string template_id { get; set; }
        public string message { get; set; }
        public string? shorten_url { get; set; } = "0";
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

    public class Error
    {
        public string to { get; set; }
    }

    public class SMSResDTO
    {
        public string message { get; set; }
        public Error? error { get; set; }
        public List<Datum> data { get; set; }

        public SMSResDTO() { }

        public SMSResDTO(string Message, Error Error)
        {
            error = Error;
            message = Message;
            data = new List<Datum>();
        }

        public SMSResDTO(List<Datum> Data)
        {
            data = Data;
        }
    }








}
