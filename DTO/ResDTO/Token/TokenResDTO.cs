namespace mct_API.DTO.ResDTO.Token
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


        public class TokenClaims
        {
            public long id { get; set; }
            public Guid gu_id { get; set; }
            public string phone_no { get; set; }
            public string token_type { get; set; }

        }

    }
}
