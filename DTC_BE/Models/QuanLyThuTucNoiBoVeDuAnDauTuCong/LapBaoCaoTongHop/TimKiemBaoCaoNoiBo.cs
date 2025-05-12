namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    public class TimKiemBaoCao
    {
        public string? ngayTao { get; set; }
        public string? tenBaoCao { get; set; }
        public int currentPage { get; set; }
        public int rowPerPage { get; set; }
    }
    public class TimKiemThongKeTheoLoai
    {
        public int? loai { get; set; }
        public int currentPage { get; set; }
        public int rowPerPage { get; set; }
    }
}
