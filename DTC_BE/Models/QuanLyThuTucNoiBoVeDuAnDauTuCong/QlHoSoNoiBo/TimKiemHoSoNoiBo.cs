namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    public class TimKiemHoSoNoiBo
    {
        public int id { get; set; }
        public string? TenHoSo { get; set; }
        public string? MaHoSo { get; set; }
        public int NhomDuAn { get; set; }
        public int QuyTrinhXuLy { get; set; }
        public int? TrangThaiLuuKho { get; set; }
        public string? DonViThucHienDuAn { get; set; }
        public int currentPage { get; set; }
        public int rowPerPage { get; set; }
        public string IdUser { get; set; }
        public string Quyen { get; set; }
    }
    public class TimKiemDuAn
    {
        public string? TenDuAn { get; set; }
        public string? MaDuAn { get; set; }
        public int currentPage { get; set; }
        public int rowPerPage { get; set; }
    }
}
