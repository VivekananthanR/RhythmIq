using elr_API.DTO.ResDTO.Token;
using elr_API.IService.IUser_manager;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace elr_API.Service.Token
{
    public class TokenValidationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {

            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

                try
                {
                    var originalBodyStream = context.Response.Body;
                    // Validate the token and decode
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["Token:Key"]);

                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = _configuration["Token:Issuer"],
                        ValidAudience = _configuration["Token:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                    //var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var claims = principal.Claims.ToDictionary(c => c.Type, c => c.Value);

                    // Check if required claims exist and parse them
                    if (!claims.ContainsKey("id") || !long.TryParse(claims["id"], out long id) || id <= 0)
                    {
                        throw new ArgumentException("Invalid or missing ID");
                    }

                    if (!claims.ContainsKey("gu_id") || !Guid.TryParse(claims["gu_id"], out Guid guId))
                    {
                        throw new ArgumentException("Invalid or missing GUID");
                    }

                    if (!claims.ContainsKey("login_gu_id") || !Guid.TryParse(claims["login_gu_id"], out Guid login_gu_id))
                    {
                        throw new ArgumentException("Invalid or missing Login GUID");
                    }

                    if (!claims.ContainsKey("phone_no") || string.IsNullOrEmpty(claims["phone_no"]))
                    {
                        throw new ArgumentException("Missing Phone Number");
                    }

                    TokenValidationReqDTO tokenValidationReqDTO = new TokenValidationReqDTO();
                    tokenValidationReqDTO.user_id = id;
                    tokenValidationReqDTO.gu_id = guId;
                    tokenValidationReqDTO.login_gu_id = login_gu_id;
                    tokenValidationReqDTO.phone_no = claims["phone_no"];
                    tokenValidationReqDTO.token_type = claims["token_type"];

                    bool tokenvalid = false;

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                        tokenvalid = await userService.TokenValidation(tokenValidationReqDTO);

                    }

                    if (!tokenvalid)
                    {
                        var errorResponse = new
                        {
                            status = false,
                            errorCode = "Invalid Session Token",
                            data = ""
                        };

                        context.Response.StatusCode = 401;
                        context.Response.Body = originalBodyStream;
                        await context.Response.WriteAsJsonAsync(errorResponse);
                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid token.");
                    return;
                }
            }

            await _next(context);
        }

    }

}
