using System.Security.Cryptography;
using System.Text;

namespace VehicleRentalsCLI
{
    public static class EncryptionHelper
    {
        private static readonly byte[] Key;
        private static readonly byte[] IV;

        static EncryptionHelper()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                Key = sha256.ComputeHash(Encoding.UTF8.GetBytes("MySuperSecretVehicleRentalsKey"));
            }

            using (MD5 md5 = MD5.Create())
            {
                IV = md5.ComputeHash(Encoding.UTF8.GetBytes("VehicleRentalsIV"));
            }
        }

        //Password Hashing
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        //Credit Card Encryption
        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        //Credit Card Decryption
        public static string Decrypt(string cipherText)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch
            {
                //Handles unencrypted sample data
                return cipherText + " [Legacy Unencrypted Data]";
            }
        }
    }
}