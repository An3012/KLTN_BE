namespace DTC_BE.Models.HeThong.NhatKyHeThong
{
    public class TimKiemDanhSachNhatKyHeThong
    {
        public string? TenNguoiDung { get; set; }
        public string? IpNguoiDung { get; set; }
        public string? MoTa { get; set; }
        public string? TuNgay { get; set; }
        public string? DenNgay { get; set; }
        public int CurrentPage { get; set; }
        public int RowPerPage { get; set; }
    }
}
