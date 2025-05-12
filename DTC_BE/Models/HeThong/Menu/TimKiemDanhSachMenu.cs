namespace DTC_BE.Models.HeThong.Menu
{
    public class TimKiemDanhSachMenu
    {
        public string? TenMenu { get; set; }
        public string? MoTa { get; set; }
        public int CurrentPage { get; set; }
        public int RowPerPage { get; set; }
    }
}
