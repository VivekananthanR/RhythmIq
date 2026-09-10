using elr_API.Common;

namespace elr_API.DTO.ResDTO.Token
{
    public class TokenResDTO
    {
        public string Token { get; set; }
        public int Token_ExpiryTime { get; set; }
        public TokenResDTO(string token, int token_ExpiryTime)
        {
            Token = token;
            Token_ExpiryTime = token_ExpiryTime;
        }
    }

    public class TokenClaims
    {
        public long id { get; set; }
        public Guid gu_id { get; set; }
        public string phone_no { get; set; }
        public string token_type { get; set; }
        public Guid login_gu_id { get; set; }

    }

    public class TokenValidationReqDTO
    {
        public long user_id { get; set; }
        public Guid gu_id { get; set; }
        public string phone_no { get; set; }
        public string token_type { get; set; }
        public Guid login_gu_id { get; set; }

        #region Query 
        internal static string user_select()
        {
            return $@"SELECT user_id FROM {Tbl.user_master} WHERE user_id= @user_id AND phone_no= @phone_no 
                    and login_gu_id = @login_gu_id AND is_active = 1;";
        }

        #endregion
    }
}
