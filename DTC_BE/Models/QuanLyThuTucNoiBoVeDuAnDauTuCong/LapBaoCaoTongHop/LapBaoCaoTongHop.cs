namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    public class LapBaoCaoTongHop
    {
        public string? id { get; set; }
        public string? Ten { get; set; }
        public string? NgayBatDau { get; set; }
        public string? NgayKetThuc { get; set; }
        public string? Nam { get; set; }
        public string? NgayTao { get; set; }
        public string? NguoiTao { get; set; }
        public int? LoaiBaoCaoTongHop { get; set; }
    }public class LapBaoCaoTongHopLCNT
    {
        public string? id { get; set; }
        public string? Ten { get; set; }
        public string? Nam { get; set; }
        public string? NguonVon { get; set; }
        public string? NgayTao { get; set; }
        public string? NguoiTao { get; set; }
        public int? LoaiBaoCaoTongHop { get; set; }
    }
    public class ThuTucThongKeTheoLoai
    {
        public string? id { get; set; }
        public string? maHoSo { get; set; }
        public string? tenHoSo { get; set; }
        public string? ngayNhanHoSo { get; set; }
        public string? hanTraKetQua { get; set; }
        public string? chuDauTu { get; set; }
        public string? nhomDuAn { get; set; }
    }

}
