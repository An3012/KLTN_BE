
namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.ThuTucThamDinhDTCBDT
{
    public class HoSoModel
    {
        #region Nguồn vốn dự án
        public List<ThongTinVon>? ListNguonVonDuAn { get; set; }
        #endregion
        #region Đăng ký nhu cầu vốn
        public List<ThongTinVon>? ListDangKyNhuCauVon { get; set; }
        #endregion
    }

    public class ThongTinVon
    {
        public string? DmNguonVonId { get; set; }
        public string? SoVon { get; set; }
    }

    public class ThongTinThuTuc
    {
        public string Id{ get; set; }
        public string? MaHoSo{ get; set; }
        public string? TenHoSo{ get; set; }
        public string? TenDuAn{ get; set; }
        public string? idDonViThucHienDuAn { get; set; }
        public string? NgayTao{ get; set; }
    }
    public class ThongTinTTTD
    {
        public string Id{ get; set; }
        public string? MaHoSo{ get; set; }
        public string? TenHoSo{ get; set; }
        public string? NhomDuAn{ get; set; }
        public string? ChuDauTu { get; set; }
        public string? NgayNhanHoSo{ get; set; }
        public string? HanGiaiQuyetHoSo{ get; set; }
        public string? ThongKeHanXuLy{ get; set; }
        public int? TrangThaiLuuKho{ get; set; }
        public string? TinhTrangHoSo{ get; set; }
        public string? ChuyenVienThuLy{ get; set; }
    }
    public class ThongTinTongQuanTTTD
    {
        public string Id{ get; set; }
        public string? MaHoSo{ get; set; }
        public string? TenHoSo{ get; set; }
        public string? NhomDuAn{ get; set; }
        public string? ChuDauTu { get; set; }
        public string? NgayNhanHoSo{ get; set; }
        public string? HanGiaiQuyetHoSo{ get; set; }
        public string? ThongKeHanXuLy{ get; set; }
        public int? TrangThaiLuuKho{ get; set; }
        public string? TinhTrangHoSo{ get; set; }
        public string? ChuyenVienThuLy{ get; set; }
        public string? ChuyenVienXuLy{ get; set; }
    }
    public class XuatExxelTongQuan
    {
        public string Id{ get; set; }
        public string? MaHoSo{ get; set; }
        public string? TenHoSo{ get; set; }
        public string? NhomDuAn{ get; set; }
        public string? ChuDauTu { get; set; }
        public string? NgayNhanHoSo{ get; set; }
        public string? HanGiaiQuyetHoSo{ get; set; }
        public string? ThongKeHanXuLy{ get; set; }
        public string? TrangThaiLuuKho{ get; set; }
        public string? TinhTrangHoSo{ get; set; }
        public string? ChuyenVienXuLy{ get; set; }
        public string? KquaThucHien1{ get; set; }
        public string? KquaThucHien2 { get; set; }
        public string? NgayKy1{ get; set; }
        public string? NgayKy2 { get; set; }
        public string? VanBanChuDauTu{ get; set; }
        public string? GhiChu{ get; set; }
        public string? GhiChuTinhTrang{ get; set; }
        public string? QuyTrinhXuLy{ get; set; }
    }
}
