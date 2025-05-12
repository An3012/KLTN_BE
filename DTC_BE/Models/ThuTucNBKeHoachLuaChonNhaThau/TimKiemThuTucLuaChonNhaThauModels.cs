namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    public class TimKiemThuTucLuaChonNhaThauModels
    {
        public int id { get; set; }
        public string? tenDuAn { get; set; }
        public string? nhomDuAn { get; set; }
        public string? nguonVon { get; set; }
        public int currentPage { get; set; }
        public int rowPerPage { get; set; }
    }
}
