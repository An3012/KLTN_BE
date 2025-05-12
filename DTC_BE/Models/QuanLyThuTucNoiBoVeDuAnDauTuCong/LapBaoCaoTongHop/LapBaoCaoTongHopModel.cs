namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.LapBaoCaoTongHop
{
    public class ListLapBaoCaoTongHopChiTiet
    {
        public string loaiHoSo {  get; set; }
        public int? tongSLHSDaTiepNhan {  get; set; }
        public int? soLuongHSDaGiaiQuyetDungHan {  get; set; }
        public int? soLuongHSDaGiaiQuyetTruocHan {  get; set; }
        public int? soLuongHSDaGiaiQuyetQuaHan { get; set; }
        public int? tongSoLuongHSDaGiaiQuyet {  get; set; }
        public int? soLuongHSDangGQTrongHan {  get; set; }
        public int? soLuongHSDangGQQuaHan { get; set; }
        public int? tongSLHSDangGiaiQuyet {  get; set; }
        public int? tongSLHSHoanThanh {  get; set; }
        public int? slHsDaLuuKho {  get; set; }
        public int? slHsChuaLuuKho {  get; set; }
        public string ghiChu { get; set; }
    }

    public class LapbaoCaoTongHopChiTiet
    {
        public string idBaoCao { get; set; }
        public List<ListLapBaoCaoTongHopChiTiet> listLapBaoCaoTongHopChiTiets { get; set; }
    }

    public class TienDoXuLyThuTucModel
    {
        public string? id { get; set; }
        public string? idThuTuc { get; set; }
        public int? idLoai { get; set; }
        public int? trangThai { get; set; }
        public int? NgayTao { get; set; }
        public int? NgayGiaiQuyet { get; set; }
        public int? HanGiaiQuyetHoSo { get; set; }
        public int? TrangThai { get; set; }
    }
    public class KetQuaThucHienModel
    {
        public string? id { get; set; }
        public string? idThuTuc { get; set; }
        public int? idLoai { get; set; }
        public int? trangThai { get; set; }
        public int? NgayTao { get; set; }
        public int? NgayKy1 { get; set; }
        public int? NgayKy2 { get; set; }
        public int? NgayGiaiQuyet { get; set; }
        public int? HanGiaiQuyetHoSo { get; set; }
        public int? LuuKho { get; set; }
    }
    public class ThuTucThamDinh
    {
        public string? id { get; set; }
        public int? idLoai { get; set; }
        public int? NgayTao { get; set; }
        public int? HanGiaiQuyetHoSo { get; set; }
        public int? LuuKho { get; set; }
    }
    public class ThuTucThamDinhKQTH
    {
        public string? id { get; set; }
        public int? idLoai { get; set; }
    }



}
