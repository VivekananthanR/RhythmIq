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

        #endregion
    }
}
