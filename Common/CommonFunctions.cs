using Serilog;
using System.Security.Cryptography;

namespace mct_API.Common
{
    public class CommonFunctions
    {

        public const string MCT = "mct";

        #region live or local
        public static bool DataOnLive()
        {
            return false;// If its Local
            //return true//If its Live
        }

        #endregion

        #region Connection String For CommonFunctions
        public static string GetConnectionString(string db)
        {
            if (DataOnLive())
            {
                if (db == "mct_db")
                {
                    return "Data Source=DESKTOP-S1C0QO9\\SQLEXPRESS;Initial Catalog=mct_db; User Id=vivek_dev; Password=Healthwatch@123; Encrypt=False;";
                }
                else return "";
            }
            else
            {
                if (db == "mct_db")
                {
                    return "Server=DESKTOP-S1C0QO9\\SQLEXPRESS;Initial Catalog=mct_db; User Id=vivek_dev; Password=Healthwatch@123; Encrypt=False;";
                }
                else return "";
            }
        }
        #endregion


        #region Generate OTP
        //public static int GenerateOTP(int length)
        //{
        //    if (length < 1) return 0;

        //    int minValue = (int)Math.Pow(10, length - 1);
        //    int maxValue = (int)Math.Pow(10, length) - 1;

        //    Random  generator = new Random();
        //    return generator.Next(minValue, maxValue);  
        //}

        public static int GenerateOTP(int length)
        {
            if (length < 1)
                return 0;

            int minValue = (int)Math.Pow(10, length - 1);

            int maxValue = (int)Math.Pow(10, length);

            return RandomNumberGenerator.GetInt32(minValue, maxValue);
        }
        #endregion

        #region Save Error Log

        public static void ILogging(object? obj, string message)
        {
            Log.Error("(New Error) => Object = {@obj} and Message ={@message}", obj, message);
        }
        #endregion
    }

}
