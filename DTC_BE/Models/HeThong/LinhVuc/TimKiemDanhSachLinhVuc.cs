namespace DTC_BE.Models.HeThong.LinhVuc
{
    public class TimKiemDanhSachLinhVuc
    {
        public string? TenLinhVuc { get; set; }
        public string? MaLinhVuc { get; set; }
        public int CurrentPage { get; set; }
        public int RowPerPage { get; set; }
    }
}
