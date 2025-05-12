namespace DTC_BE.Models.HeThong.NguoiDung
{
    public class ThemMoiNguoiDungModel
    {
        public string Id { get; set; }
        public string HoTen { get; set; }
        public string MatKhau { get; set; }
        public string TenDangNhap { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string HtNhomQuyenId { get; set; }
        public string NguoiTao { get; set; }
        public int? TrangThai { get; set; }
        public int? PhongBan { get; set; }
    }
    public class NguoiDungModel
    {
        public string Id { get; set; }
        public string HoTen { get; set; }
        public string MatKhau { get; set; }
        public string TenDangNhap { get; set; }
        public string Email { get; set; }
        public string NhomQuyen { get; set; }
        public int? TrangThai { get; set; }
        public string PhongBan { get; set; }
    }
}
