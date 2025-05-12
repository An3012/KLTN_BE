namespace DTC_BE.Models
{
    public class ThongTinTepTin
    {
        public string? Id { get; set; }
        public string? TenHienThi { get; set; }
        public string? TenHeThong { get; set; }
        public bool IsNew { get; set; } = false;
        public bool IsDelete { get; set; } = false;
    }
}
