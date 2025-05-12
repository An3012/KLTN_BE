namespace DTC_BE.CodeBase
{
    public class HangSo
    {
        #region Hệ thống - Nhóm quyền
        public const int QuyenChuaCap = 1;
        public const int QuyenDuocCap = 2;
        #endregion

        #region Hệ thống - Tài khoản
        public const string Admin = "b7b2f831-18be-4a35-9c16-e5ba65dc79ef";
        #endregion

        #region AllowExtension file
        public const int AllowExtension_None = 0;
        public const int AllowExtension_ImportFile = 1;
        public const int AllowExtension_ImportExcel = 2;
        public const int AllowExtension_ImportWord = 3;
        public const int AllowExtension_ImportPDF = 4;
        public const int AllowExtension_ImportImage = 5;
        #endregion
    }
}
