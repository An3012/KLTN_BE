//using DTC_BE.Models.KeHoachDauTuCongTrungHan.QuanLyDuAn;

namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    public class ThuTucModels
    {
        public class ThuTucModel
        {
            #region Thông tin chung thủ tuch
            public string? Id { get; set; }
            public string? TenHoSo { get; set; }
            public string? MaHoSo { get; set; }
            public string? DuAnId { get; set; }
            public double? TongNguonVon { get; set; }
            public string? DiaDiem { get; set; }
            public string? DienTichSdDat { get; set; }
            public string? DonViDt { get; set; }
            public string? ThoiHanThucHienDuAn { get; set; }
            public string? IdDonViThucHienDuAn { get; set; }
            public string? TienDoThucHienDuAn { get; set; }
            public string? CacThongTinKhac { get; set; }
            public string? MucDichDauTu { get; set; }
            public string? LoaiHoSo { get; set; }
            public string? FileName { get; set; }
            public string? FilePath { get; set; }
            public string? IdUser { get; set; }
            public string? hanGiaiQuyetHoSo { get; set; }
            public bool IsNew { get; set; }
            public bool IsDelete { get; set; }

            #endregion

            #region Nguồn vốn dự án
            public List<ThongTinVonThuTuc>? ListNguonVonThuTuc { get; set; }
            #endregion
        }
        public class ThuTucTDModel
        {
            public string? Id { get; set; }
            public string? IdUser { get; set; }
            public string? TenHoSo { get; set; }
            public string? MaHoSo { get; set; }
            public string? NgayNhanHoSo { get; set; }
            public string? DuKienHoanThanh { get; set; }
            public string? ChuyenVienThuLy { get; set; }
            public int? TinhTrangHoSo { get; set; }
            public string? ChuDauTu { get; set; }
            public int? NhomDuAn { get; set; }
            public string? CacThongTinKhac { get; set; }
            public string? GhiChuTinhTrang { get; set; }
            public int? LoaiHoSo { get; set; }
            public string? FileDinhKem1 { get; set; }
            public string? FilePath1 { get; set; }
            public bool IsNew1 { get; set; }
            public bool IsDelete1 { get; set; }
            public string? VanBanChuDauTu { get; set; }
            public string? FileDinhKem2 { get; set; }
            public string? FilePath2 { get; set; }
            public bool IsNew2 { get; set; }
            public bool IsDelete2 { get; set; }
        }
        public class KetQuaThucHien
        {
            public string? Id { get; set; }
            public string? IdThuTuc { get; set; }
            public bool IsDeleteKqth1 { get; set; }
            public bool IsNewKqth1 { get; set; }
            public string? FileNameKqth1 { get; set; }
            public string? FilePathKqth1 { get; set; }
            public string? NgayKy1 { get; set; }
            public string? SoVanBanQuyetDinh1 { get; set; }
            public bool IsDeleteKqth2 { get; set; }
            public bool IsNewKqth2 { get; set; }
            public string? FileNameKqth2 { get; set; }
            public string? FilePathKqth2 { get; set; }
            public string? NgayKy2 { get; set; }
            public string? SoVanBanQuyetDinh2 { get; set; }
        }
        public class ThongTinVonThuTuc
        {
            public string? LoaiNguonVon { get; set; }
            public double? GiaTriNguonVon { get; set; }
        }

        public class ThongTinPhieuXuLy
        {
            public string? Id { get; set; }
            public string? IdThuTuc { get; set; }
            public string? TextBuoc1 { get; set; }
            public string? TextBuoc2 { get; set; }
            public string? TextBuoc3{ get; set; }
            public string? TextBuoc4 { get; set; }
            public string? TextBuoc5 { get; set; }
            public string? TextBuoc6 { get; set; }
            public int? Buoc1 { get; set; }
            public int? Buoc2 { get; set; }
            public int? Buoc3 { get; set; }
            public int? Buoc4 { get; set; }
            public int? Buoc5 { get; set; }
            public int? Buoc6 { get; set; }
            public int? ThoiGianThucHien { get; set; }
            public string? NgayBuoc1 { get; set; }
            public string? NgayBuoc2 { get; set; }
            public string? NgayBuoc3 { get; set; }
            public string? NgayBuoc4 { get; set; }
            public string? NgayBuoc5 { get; set; }
            public string? NgayBuoc6 { get; set; }
        }

    }
}
