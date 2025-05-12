using System.Security.Cryptography;
namespace DTC_BE.CodeBase
{
    public class Security
    {
        /// <summary>
        /// Mã hóa password
        /// </summary>
        /// <Modified>
        /// Name        Date        Comment
        /// Hùngnv       6/5/2009  Thêm mới
        /// </Modified>
        public static string EncryptPassword(string Password)
        {
            var _md5 = new MD5CryptoServiceProvider();
            var rawData = System.Text.ASCIIEncoding.ASCII.GetBytes(Password);
            var result = _md5.ComputeHash(rawData);
            return System.Convert.ToBase64String(result, 0, result.Length);
        }
    }
}
