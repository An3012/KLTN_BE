namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.QlHoSoNoiBo
{
    public class HoSoNoiBo
    {
        public string Id { get; set; }
        public string? MaHoSo { get; set; }
        public string? TenHoSo { get; set; }
        public string? NhomDuAn { get; set; }
        public string? ChuDauTu { get; set; }
        public string? NgayNhanHoSo { get; set; }
        public string? HanGiaiQuyetHoSo { get; set; }
        public string? ThongKeHanXuLy { get; set; }
        public int? LuuKho { get; set; }
        public string? TinhTrangHoSo { get; set; }
        public string? ChuyenVienThuLy { get; set; }
        public string? NguoiTao { get; set; }
        public int? LoaiHoSo { get; set; }
    }
    public class DM_DuAnmodel
    {
        public string Id { get; set; }
        public string? MaDuAn { get; set; }
        public string? TenDuAn { get; set; }
    }
    public class HoSoNoiBoModel
    {
        public string? Id { get; set; }
        public string? TenHoSo { get; set; }
        public string? MaHoSo { get; set; }
        public double? LoaiHoSo { get; set; }
        public int? LuuKho { get; set; }
        public string? SoQuyetDinh { get; set; }
    }
    public class DuToanCbDauTuCnkqModel
    {
        public string? Id { get; set; }
        public string? IdThuTuc { get; set; }
        public string? SoNgayQuyetDinh { get; set; }
        public string? NamPheDuyet { get; set; }
        public string? ChuDauTu { get; set; }
        public string? DuAnId { get; set; }
        public double? DutoanCpCbdautu { get; set; }
        public string? CoCauNguonVon { get; set; }
    }

    public class ChuTruongDauTuCnkqModel
    {
        public string? Id { get; set; }
        public string? IdThuTuc { get; set; }
        public string? SoNgayQuyetDinh { get; set; }
        public string? NamPheDuyet { get; set; }
        public string? ChuDauTu { get; set; }
        public string? DuAnId { get; set; }
        public int? NhomDuAn { get; set; }
        public string? NangLucThietKe { get; set; }
        public string? DiaDiemDauTu { get; set; }
        public string? CoCauNguonVon { get; set; }
        public double? TongMucDauTu { get; set; }
        public double? CpXayDung { get; set; }
        public double? CpThietbi { get; set; }
        public double? CpBoithuong { get; set; }
        public double? CpChung { get; set; }
        public double? CpDuphong { get; set; }
        public string? ThoigianThuchien { get; set; }
        public string? TiendoThuchien { get; set; }
    }

    public class DcChuTruongDauTuCnkqModel
    {
        public string? Id { get; set; }
        public string? IdThuTuc { get; set; }
        public string? SoNgayQuyetDinhBiDc { get; set; }
        public string? SoNgayQuyetDinh { get; set; }
        public string? NamPheDuyet { get; set; }
        public string? ChuDauTu { get; set; }
        public string? DuAnId { get; set; }
        public string? CoCauNguonVonDieuChinh { get; set; }
        public string? GhiChu { get; set; }
        public double? TongMucDauTu { get; set; }
        public double? CpXayDung { get; set; }
        public double? CpThietbi { get; set; }
        public double? CpBoithuong { get; set; }
        public double? CpChung { get; set; }
        public double? CpDuphong { get; set; }
    }

    public class DuAnDauTuCnkqModel
    {
        public string? Id { get; set; }
        public string? IdThuTuc { get; set; }
        public string? SoNgayQuyetDinh { get; set; }
        public string? NamPheDuyet { get; set; }
        public string? ChuDauTu { get; set; }
        public string? DuAnId { get; set; }
        public int? NhomDuAn { get; set; }
        public string? NangLucThietKe { get; set; }
        public string? DiaDiemDauTu { get; set; }
        public string? CoCauNguonVon { get; set; }
        public double? TongMucDauTu { get; set; }
        public double? CpXayDung { get; set; }
        public double? CpThietbi { get; set; }
        public double? CpBoithuong { get; set; }
        public double? CpChung { get; set; }
        public double? CpDuphong { get; set; }
        public string? ThoigianThuchien { get; set; }
        public string? SoBuocThietKe { get; set; }
        public string? HinhThucQuanly { get; set; }
    }

    public class DCDuAnDauTuCnkqModel
    {
        public string? Id { get; set; }
        public string? IdThuTuc { get; set; }
        public string? SoNgayQuyetDinhBiDc { get; set; }
        public string? SoNgayQuyetDinh { get; set; }
        public string? NamPheDuyet { get; set; }
        public string? ChuDauTu { get; set; }
        public double? TongMucDauTu { get; set; }
        public double? CpXayDung { get; set; }
        public double? CpThietbi { get; set; }
        public double? CpBoithuong { get; set; }
        public double? CpChung { get; set; }
        public double? CpDuphong { get; set; }
        public string? DuAnId { get; set; }
        public string? CoCauNguonVonDieuChinh { get; set; }
        public string? GhiChu { get; set; }
        public string? ThoigianThuchienDc { get; set; }
    }

    public class DtDCDaKoCoCpxdCnkqModel
    {
        public string? Id { get; set; }
        public string? IdThuTuc { get; set; }
        public string? SoNgayQuyetDinhBiDc { get; set; }
        public string? SoNgayQuyetDinh { get; set; }
        public string? NamPheDuyet { get; set; }
        public string? ChuDauTu { get; set; }
        public string? DuAnId { get; set; }
        public double? DutoanCpCbdautuDieuChinh { get; set; }
        public string? CoCauNguonVonDieuChinh { get; set; }
        public string? GhiChu { get; set; }
    }

    public class LCNTCnkqModel
    {
        public string? Id { get; set; }
        public string? IdThuTuc { get; set; }
        public string? SoNgayQuyetDinhBiDc { get; set; }
        public string? SoNgayQuyetDinh { get; set; }
        public string? NamPheDuyet { get; set; }
        public string? TenKeHoach { get; set; }
        public string? ChuDauTu { get; set; }
        public int? NhomDuAn { get; set; }

        public List<PhanChiaDuAnThanhCacGoiThau>? lstCacGoiThau { get; set; }
    }

    public class PhanChiaDuAnThanhCacGoiThau
    {
        public string? TenGoiThau { get; set; }
        public double? GiaGoiThau { get; set; }
        public double? GiaTrungThau { get; set; }
        public int? NguonVon { get; set; }
        public int? LinhVuc { get; set; }
        public int? HinhThucDauThau { get; set; }
        public int? HinhThucLuaChonNhaThau { get; set; }
    }


}
