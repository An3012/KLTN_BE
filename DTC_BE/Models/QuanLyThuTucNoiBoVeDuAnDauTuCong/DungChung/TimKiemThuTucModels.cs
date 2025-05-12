namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    public class TimKiemTTTDModels
    {
        public int id { get; set; }
        public string? TenHoSo { get; set; }
        public string? MaHoSo { get; set; }
        public int NhomDuAn { get; set; }
        public string? DonViThucHienDuAn { get; set; }
        public int currentPage { get; set; }
        public int rowPerPage { get; set; }
        public string IdUser { get; set; }
        public string Quyen { get; set; }
    }
}
