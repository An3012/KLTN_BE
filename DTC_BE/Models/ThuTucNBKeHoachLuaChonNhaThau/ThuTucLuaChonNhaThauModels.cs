namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    public class ThuTucLuaChonNhaThauModels
    {
        public class ThuTucLuaChonNhaThauModel
        {
            #region Thông tin chung thủ tuc
            public string? Id { get; set; }
            public string? TenDuAn { get; set; }
            public string? TongMucDauTu { get; set; }
            public string? ChuDauTuId { get; set; }
            public string? ThoiGianThucHienDuAn { get; set; }
            public string? NguonVonId { get; set; }
            public string? NhomDuAn { get; set; }
            public string? DiaDiemQuyMo { get; set; }
            public string? CacThongTinKhac { get; set; }
            public int? LoaiThuTuc { get; set; }
            public string? FileName { get; set; }
            public string? FilePath { get; set; }
            public string? IdUser { get; set; }
            public string? HanGiaiQuyetHoSo { get; set; }
            public bool IsNew { get; set; }
            public bool IsDelete { get; set; }

            #region Kết quả thực hiện

            public string? TrangThaiKetQua { get; set; }
            public string? SoQuyetDinhKetQua { get; set; }
            public string? NguoiKyKetQua { get; set; }
            public string? NgayKyKetQua { get; set; }
            public bool IsDeleteKqth { get; set; }
            public bool IsNewKqth { get; set; }
            public string? FileNameKqth { get; set; }
            public string? FilePathKqth { get; set; }
            #endregion
            #endregion

            #region Các gói thầu nhỏ
            public List<PhanChiaDuAnThanhCacGoiThau>? lstCacGoiThau { get; set; }
            #endregion
        }

        public class ThuTucLuaChonNhaThauDieuChinhModel
        {
            #region Thông tin chung thủ tuc
            public string? Id { get; set; }
            public string? TenDuAn { get; set; }
            public string? TongMucDauTu { get; set; }
            public string? ChuDauTuId { get; set; }
            public string? ThoiGianThucHienDuAn { get; set; }
            public string? NguonVonId { get; set; }
            public string? NhomDuAn { get; set; }
            public string? DiaDiemQuyMo { get; set; }
            public string? CacThongTinKhac { get; set; }
            public string? LoaiThuTuc { get; set; }
            public string? FileName { get; set; }
            public string? FilePath { get; set; }
            public string? IdUser { get; set; }
            public string? HanGiaiQuyetHoSo { get; set; }
            public bool IsNew { get; set; }
            public bool IsDelete { get; set; }

            #region Kết quả thực hiện

            public string? TrangThaiKetQua { get; set; }
            public string? SoQuyetDinhKetQua { get; set; }
            public string? NguoiKyKetQua { get; set; }
            public string? NgayKyKetQua { get; set; }
            public bool IsDeleteKqth { get; set; }
            public bool IsNewKqth { get; set; }
            public string? FileNameKqth { get; set; }
            public string? FilePathKqth { get; set; }
            public string? CacNoiDungDieuChinhKhac { get; set; }
            #endregion
            #endregion

            #region CacGoiThauNho
            public List<PhanChiaDuAnThanhCacGoiThau>? lstCacGoiThau { get; set; }
            #endregion
            #region NoiDungDieuChinh
            public List<NoiDungDieuChinh>? lstNoiDungDieuChinh { get; set; }
            #endregion
        }

        public class NoiDungDieuChinh
        {
            public string? TenGoiThau { get; set; }
            public string? ThoiGianBdToChucLuaChonNhaThau { get; set; }
            public string? DeNghiDieuChinh { get; set; }
        }

        public class PhanChiaDuAnThanhCacGoiThau
        {
            public string? TenGoiThau { get; set; }
            public double? GiaGoiThau { get; set; }
            public double? GiaTrungThau { get; set; }
            public double? NguonVon { get; set; }
            public int? LinhVuc { get; set; }
            public int? HinhThucLuaChonNhaThauTrongNgoaiNuoc { get; set; }
            public int? HinhThucLuaChonNhaThau { get; set; }
            public int? HinhThucDauThau { get; set; }
            public int? PhuongThucLuaChonNhaThau { get; set; }
            public int? LoaiHopDong { get; set; }
            public string? ThoiGianBatDauToChucChonNhaThau { get; set; }
            public string? ThoiGianThucHienHopDong { get; set; }
        }

    }
}
