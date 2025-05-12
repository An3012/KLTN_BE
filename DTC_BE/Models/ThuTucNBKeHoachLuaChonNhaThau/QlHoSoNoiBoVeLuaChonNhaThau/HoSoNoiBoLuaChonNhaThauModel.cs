namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.QlHoSoNoiBo
{
    public class HoSoNoiBoLuaChonNhaThauModel
    {
        public string? Id { get; set; }
        public string? TenDuAn { get; set; }
        public int? LoaiHoSo { get; set; }
        public int? LuuKho { get; set; }
        public string? SoQuyetDinh { get; set; }
        public string? NgayKy { get; set; }
        public string? NguoiKy { get; set; }
        public string? FileNameDinhKemQuanLyHoSo { get; set; }
        public string? FilePathDinhKemQuanLyHoSo { get; set; }
        public bool IsNew { get; set; }
        public bool IsDelete { get; set; }
    }public class DMHoSoNoiBoLuaChonNhaThau
    {
        public string? Id { get; set; }
        public string? TenDuAn { get; set; }
        public string? LoaiHoSo { get; set; }
        public int? LuuKho { get; set; }
        public string? TTLuuKho { get; set; }
        public string? TenChuDauTu { get; set; }
        public string? SoQuyetDinh { get; set; }
        public string? NgayKy { get; set; }
        public string? NguoiKy { get; set; }
        public string? FileNameDinhKemQuanLyHoSo { get; set; }
        public string? FilePathDinhKemQuanLyHoSo { get; set; }
        public bool IsNew { get; set; }
        public bool IsDelete { get; set; }
    }
}
