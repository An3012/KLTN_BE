namespace DTC_BE.Models.HeThong.ChuDauTu
{
    public class ChuDauTuModel
    {
        public string Id { get; set; } = null!;

        public string? TenChuDauTu { get; set; }

        public string? MaSoThue { get; set; }

        public string? DiaChi { get; set; }

        public string? Email { get; set; }

        public string? SoDienThoai { get; set; }

        public int? Loai { get; set; }

        public string? NguoiTao { get; set; }

        public string? NguoiCapNhat { get; set; }

        public string? TinhThanh { get; set; }

        public string? XaPhuong { get; set; }

        public string? QuanHuyen { get; set; }

        public string? NguoiDaiDien { get; set; }

        public string? NgayHoatDong { get; set; }
    }
}
