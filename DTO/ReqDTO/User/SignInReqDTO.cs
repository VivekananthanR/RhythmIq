using elr_API.Common;

namespace elr_API.DTO.ReqDTO.UserReq
{
    public class SignInReqDTO
    {
    }

    public class MasterEmployeeLoginReqDTO
    {

        public string user_name { get; set; }
        public string password { get; set; }


        #region Query
        internal static string employee_master_select()
        {
            return $@"Select * from {Tbl.user_master}  where (phone_no = @user_name OR email_id = @user_name OR emp_code = @user_name) and is_active = 1";
        }
        #endregion

    }
    public class MasterEmployeeLoginResDTO
    {
        public long user_id { get; set; }
        public Guid? gu_id { get; set; }
        public string user_name { get; set; }
        public string emp_code { get; set; }
        public string password { get; set; }
        public string phone_no { get; set; }
        public string email_id { get; set; }
        public int otp { get; set; }
        public DateTime? otp_expiry_time { get; set; }
        public bool is_active { get; set; }

        public long user_type_id { get; set; }
        public string user_type { get; set; }


        public string token { get; set; }
        public int token_expirytime { get; set; }
    }
}