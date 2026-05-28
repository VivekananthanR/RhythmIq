using mct_API.DTO.ResDTO.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace mct_API.Common
{
    public class Validation
    {
        #region Check is valid email
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Check is valid phone number

        public static bool IsValidPhoneNumber(string number)
        {
            string matchNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            if (number != null) return Regex.IsMatch(number, matchNumberPattern);
            return false;
        }
        #endregion

        #region Token Claims Validation

        public static TokenClaims ClaimValues(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                throw new ArgumentException("Invalid authorization header format");
            }
            try
            {
                var token = authorizationHeader.Replace("Bearer ", "");
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

                // Check if required claims exist and parse them
                if (!claims.ContainsKey("id") || !long.TryParse(claims["id"], out var id) || id <= 0)
                {
                    throw new ArgumentException("Invalid or missing id claim");
                }
                if (!claims.ContainsKey("gu_id") || !Guid.TryParse(claims["gu_id"], out var gu_id))
                {
                    throw new ArgumentException("Invalid or missing Guid claim");
                }
                if (!claims.ContainsKey("phone_no") || string.IsNullOrEmpty(claims["phone_no"]))
                {
                    throw new ArgumentException("Invalid or missing PhoneNumber claim");
                }
                if (!claims.ContainsKey("token_type") || string.IsNullOrEmpty(claims["token_type"]))
                {
                    throw new ArgumentException("Invalid or missing TokenType claim");
                }

                return new TokenClaims
                {
                    id = id,
                    gu_id = gu_id,
                    phone_no = claims["phone_no"],
                    token_type = claims["token_type"]
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error parsing token claims: {ex.Message}");
            }


        }
        #endregion
    }
}
