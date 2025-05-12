namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.TongQuan
{
    public class TongQuanHoSo
    {
        public string? Id { get; set; }
        public string? tenHoSo { get; set; }
        public int? loaiHoSo { get; set; }
        public string? chuDauTu { get; set; }
        public string? trangThai { get; set; }
    }

    public class ThongTinChungThuTuc
    {
        public string? Id { get; set; }
        public string? tenHoSo { get; set; }
        public string? maHoSo { get; set; }
        public string? duAn { get; set; }
        public double? tongVonDauTu { get; set; }
        public string? diaDiemThucHien { get; set; }
        public string? dienTichSuDungDat { get; set; }
        public string? mucDichDauTu { get; set; }
        public string? thoiGianThucHien { get; set; }
        public string? coQuanDonViThucHien { get; set; }
        public string? tienDoThucHien { get; set; }
        public string? cacThongTinKhac { get; set; }
        public string? hanGiaiQuyet { get; set; }
        public string? fileDinhKem { get; set; }
        public string? filePath { get; set; }
        public string? soQuyetDinh { get; set; }
        public string? nguoiKy { get; set; }
        public string? ngayKy { get; set; }
        public int? luuKho { get; set; }
    }

    public class lstLoaiNguonVon
    {
        public string? loaiNguonVon { get; set; }
        public double? giaTriNguonVon { get; set; }

    }
}
