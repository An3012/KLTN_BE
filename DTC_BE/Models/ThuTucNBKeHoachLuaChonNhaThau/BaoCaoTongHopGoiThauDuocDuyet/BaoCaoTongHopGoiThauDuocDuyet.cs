namespace DTC_BE.Models.ThuTucNBKeHoachLuaChonNhaThau.BaoCaoTongHopGoiThauDuocDuyet
{
    public class ChiTietGoiThauDuocDuyet
    {
        public string? Id { get; set; }
        public string? IdThuTuc { get; set; }
        public int? HinhThucDauThau { get; set; }
        public int? NguonVon { get; set; }
        public int? LinhVuc { get; set; }
        public int? HinhThucLuaChonNhaThau { get; set; }
        public double? GiaGoiThau { get; set; }
        public double? GiaTrungThau { get; set; }
        public string? NhomDuAn { get; set; }
    }

    public class TongHopKetQuaLuaChonNhaThau
    {
        public string? id { get; set; }
        public string? chenhLechDoQuocHoiChuTruongDauTu { get; set; }
        public string? chenhLechDuAnNhomA { get; set; }
        public string? chenhLechDuAnNhomB { get; set; }
        public string? chenhLechDuAnNhomC { get; set; }
        public string? chenhLechTongCong { get; set; }
        public int? hinhThucDauThau { get; set; }
        public int? linhVucVaHinhThuc { get; set; }
        public string? tongGiaGoiThauDoQuocHoiChuTruongDauTu { get; set; }
        public string? tongGiaGoiThauDuAnNhomA { get; set; }
        public string? tongGiaGoiThauDuAnNhomB { get; set; }
        public string? tongGiaGoiThauDuAnNhomC { get; set; }
        public string? tongGiaGoiThauTongCong { get; set; }
        public string? tongGiaTrungThauDoQuocHoiChuTruongDauTu { get; set; }
        public string? tongGiaTrungThauDuAnNhomA { get; set; }
        public string? tongGiaTrungThauDuAnNhomB { get; set; }
        public string? tongGiaTrungThauDuAnNhomC { get; set; }
        public string? tongGiaTrungThauTongCong { get; set; }
        public string? tongSoGoiThauDoQuocHoiChuTruongDauTu { get; set; }
        public string? tongSoGoiThauDuAnNhomA { get; set; }
        public string? tongSoGoiThauDuAnNhomB { get; set; }
        public string? tongSoGoiThauDuAnNhomC { get; set; }
        public string? tongSoGoiThauTongCong { get; set; }
    }
}
