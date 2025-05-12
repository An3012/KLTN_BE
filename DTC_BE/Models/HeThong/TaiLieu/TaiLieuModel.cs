namespace DTC_BE.Models.HeThong.TaiLieu
{
    public class TaiLieuModel
    {
        public string? Id { get; set; }
        public string? TenTaiLieu { get; set; }
        public string? MaTaiLieu { get; set; }
        public string? GhiChu { get; set; }
        public string? TenHienThi { get; set; }
        public string? TenHeThong { get; set; }
        public string? UpdateBy { get; set; }
        public bool IsNew { get; set; }
        public bool IsDelete { get; set; }
    }
}
