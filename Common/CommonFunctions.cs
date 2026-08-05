using elr_API.DTO.ReqDTO;
using Newtonsoft.Json;
using Serilog;
using System.Security.Cryptography;

namespace elr_API.Common
{
    public class CommonFunctions
    {
        //Token Type
        public const string ELR = "elr";

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
                if (db == "elr_db_test")
                {
                    return "Data Source=DESKTOP-S1C0QO9\\SQLEXPRESS;Initial Catalog=elr_db_test; User Id=vivek_dev; Password=Healthwatch@123; Encrypt=False;";
                }
                else return "";
            }
            else
            {
                //if (db == "elr_db_test")
                //{
                return "Server=DESKTOP-S1C0QO9\\SQLEXPRESS;Initial Catalog=elr_db_test; User Id=vivek_dev; Password=Healthwatch@123; Encrypt=False;";
                //return "Server=DESKTOP-S1C0QO9\\SQLEXPRESS;Initial Catalog=elr_db; User Id=vivek_dev; Password=Healthwatch@123; Encrypt=False;";
                //}
                //else return "";
            }
        }
        #endregion
        #region Get Folder Path
        public static string FolderPath(string FileFolder)
        {
            if (DataOnLive())
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Resources", FileFolder);
            }
            else
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Resources", FileFolder);
            }
        }
        #endregion

        public static async Task<string> UploadFile(IFormFile File, string FileName, string FolderPath)
        {
            if (File != null)
            {
                await checkDirectory(FolderPath);
                string filePath = Path.Combine(FolderPath, FileName);

                DeleteImage(filePath);

                using FileStream stream = new FileStream(filePath, FileMode.Create);

                await File.CopyToAsync(stream);
                return filePath;
            }
            return null;
        }
        public static bool DeleteImage(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }

            return false;
        }

        public static async Task<int> checkDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return 1;
        }

        public static async Task<string> MergeJsonFile(List<EcgJsonDTO> mergedData, string fileName, string folderPath)
        {
            await checkDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            DeleteImage(filePath);

            string json = JsonConvert.SerializeObject(
                mergedData,
                Formatting.Indented);

            await File.WriteAllTextAsync(filePath, json);

            return filePath;
        }


        #region Generate OTP

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
