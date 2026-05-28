
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace mct_API.Common
{
    public class ResponseEncrypt
    {
        private static readonly string enkey = "15935722001241278523985216325874";// 32-byte key for AES-256
        private static readonly string eniv = "1201240112242001";// 16-byte IV for AES


        public static object EncryptData(object plainData)
        {
            return Encrypt(plainData);
        }

        public static string EncrptString(string plainText)
        {
            return Encrypt(plainText);
        }

        private static string Encrypt(object plainData)
        {
            try
            {
                string plainText = JsonConvert.SerializeObject(plainData);
                return Encrypt(plainText);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        private static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(enkey);
                aesAlg.IV = Encoding.UTF8.GetBytes(eniv);

                ICryptoTransform encoder = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encoder, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }

        }
    }
}
