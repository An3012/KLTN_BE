namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    public class ChuyenVienThuLyModels
    {
        public string? Id { get; set; }

        public string? ChuyenVienThuLy { get; set; }

        public string? PhongBanThuLy { get; set; }
    }
    public class ChuyenVienThuLy
    {
        public string? Id { get; set; }

        public string? ChuyenVien { get; set; }

        public string? PhongBan { get; set; }
        public string? NgayTao { get; set; }
    }
}
