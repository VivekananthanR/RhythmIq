using elr_API.DTO.ResDTO.Token;
using elr_API.IService.IToken;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace elr_API.Service.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenResDTO GetToken(string user_id, string gu_id, string phone_no, string token_type, string login_gu_id)
        {
            var claims = new[]
            {
                new Claim("id", user_id),
                new Claim("gu_id", gu_id),
                new Claim("phone_no", phone_no),
                new Claim("token_type", token_type),
                new Claim("login_gu_id", login_gu_id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryTime = Convert.ToInt32(_configuration["Token:ExpiryTime"]);
            var expiresAt = DateTime.UtcNow.AddMilliseconds(expiryTime);
            var token = new JwtSecurityToken(
                _configuration["Token:Issuer"],
                _configuration["Token:Audience"],
                claims,
                expires: expiresAt,
                signingCredentials: signIn);
            return new TokenResDTO(new JwtSecurityTokenHandler().WriteToken(token), expiryTime);
        }
    }
}
