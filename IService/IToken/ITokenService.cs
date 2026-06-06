using elr_API.DTO.ResDTO.Token;

namespace elr_API.IService.IToken
{
    public interface ITokenService
    {
        public TokenResDTO GetToken(string user_id, string gu_id, string phone_no, string token_type);
    }
}
