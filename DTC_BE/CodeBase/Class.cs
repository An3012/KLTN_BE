using System.Text;
using System.Security.Cryptography;

namespace DTC_BE.CodeBase
{
    public class SecurityService
    {
        private static string _UserNameChar = "abcdefghijklmnopqrstuvxyz0123456789";
        private static string _passwordChar = "abcdefghijklmnopqrstuvxyz0123456789!@#$%^";
        private static string _iFormatThamSoHeThong = "0123456789";

        public static string EncryptPassword(string password)
        {
            byte[] hashBytes = new UnicodeEncoding().GetBytes(password);
            byte[] cryptPassword = new SHA1CryptoServiceProvider().ComputeHash(hashBytes);
            return BitConverter.ToString(cryptPassword);
        }

        public static string EncryptPassword_Sha256(string? password)
        {
            try
            {
                if (!string.IsNullOrEmpty(password))
                    password = "123";
                // Chuyển password thành byte[]
                byte[] hashBytes = Encoding.Unicode.GetBytes(password);

                // Sử dụng SHA256 để băm mật khẩu
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] cryptPassword = sha256.ComputeHash(hashBytes);

                    // Trả về kết quả băm dưới dạng chuỗi (không có dấu '-')
                    return BitConverter.ToString(cryptPassword).Replace("-", "").ToLower();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool ValidatePassword(string password)
        {
            if (password == null)
                return false;

            if (password.Length < 6 || password.Length > 20)
                return false;

            password = password.ToLower();

            foreach (char c in password)
            {
                if (!_passwordChar.Contains(c))
                    return false;
            }

            return true;
        }

        public static bool ValidateThamSoHeThong(string sFormatThamSoHeThong)
        {
            if (sFormatThamSoHeThong == null)
                return false;

            foreach (char c in sFormatThamSoHeThong)
            {
                if (!_iFormatThamSoHeThong.Contains(c))
                    return false;
            }
            return true;
        }

        public static bool ValidateUserName(string UserName)
        {
            if (UserName == null)
                return false;

            if (UserName.Length < 1 || UserName.Length > 30)
                return false;

            UserName = UserName.ToLower();

            foreach (char c in UserName)
            {
                if (!_UserNameChar.Contains(c))
                    return false;
            }

            return true;
        }
    }
}
