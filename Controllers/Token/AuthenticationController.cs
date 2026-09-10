using elr_API.Common;
using elr_API.DTO.ResDTO;
using elr_API.IService.IToken;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;

namespace elr_API.Controllers.Token
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService TokenService;
        public AuthenticationController(ITokenService tokenService)
        {
            TokenService = tokenService;
        }

        #region HttpGet = api/Authentication/RefreshToken?Authorization=
        [HttpGet("RefreshToken")]
        public ActionResult RefreshToken(string token)
        {
            if (TokenService == null) return Ok(new ResponseDTO(false, "Token Service is not available"));
            var authorized_token = token;
            var new_token = new JwtSecurityTokenHandler().ReadJwtToken(authorized_token).Claims;
            ListDictionary obj = new ListDictionary();
            foreach (var item in new_token)
            {
                obj[item.Type] = item.Value;
            }
            if (obj.Contains("id") && obj.Contains("gu_id") && obj.Contains("phone_no") && obj.Contains("token_type") && obj.Contains("login_gu_id"))
            {
                var tokenDetails = TokenService.GetToken(obj["id"].ToString(), obj["gu_id"].ToString(), obj["phone_no"].ToString(), obj["token_type"].ToString(), obj["login_gu_id"].ToString());
                return Ok(tokenDetails);
            }
            return Ok(new ResponseDTO(false, ResponseMessage.Access_Denied));
        }
        #endregion

    }
}
