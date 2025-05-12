namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.TongQuan
{
    public class TimKiemTongQuanHoSo
    {
        public string? loaiHoSo { get; set; }
        public string? tenHoSo { get; set; }
        public string? chuDauTu { get; set; }
        public string? trangThai { get; set; }
        public string? maHoSo { get; set; }
        public string? nhomDuAn { get; set; }
        public string? trangThaiLuuKho { get; set; }
        public string? chuyenVienThuLy { get; set; }
        public string? chuyenVienXuLy { get; set; }
        public string? thongKeHanXuLy { get; set; }
        public string? ngayNhanHoSo { get; set; }
        public string? hanTraKetQua { get; set; }
        public string? idUser { get; set; }
        public int currentPage { get; set; }
        public int rowPerPage { get; set; }
    }
}
