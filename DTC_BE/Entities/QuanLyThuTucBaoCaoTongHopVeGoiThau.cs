using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucBaoCaoTongHopVeGoiThau
{
    public string Id { get; set; } = null!;

    public string? IdBaoCao { get; set; }

    public int? LinhVucVaHinhThuc { get; set; }

    public int? HinhThucDauThau { get; set; }

    public double? TongSoGoiThauDoQuocHoiChuTruongDauTu { get; set; }

    public double? TongGiaGoiThauDoQuocHoiChuTruongDauTu { get; set; }

    public double? TongGiaTrungThauDoQuocHoiChuTruongDauTu { get; set; }

    public double? ChenhLechDoQuocHoiChuTruongDauTu { get; set; }

    public double? TongSoGoiThauDuAnNhomA { get; set; }

    public double? TongGiaGoiThauDuAnNhomA { get; set; }

    public double? TongGiaTrungThauDuAnNhomA { get; set; }

    public double? ChenhLechDuAnNhomA { get; set; }

    public double? TongSoGoiThauDuAnNhomB { get; set; }

    public double? TongGiaGoiThauDuAnNhomB { get; set; }

    public double? TongGiaTrungThauDuAnNhomB { get; set; }

    public double? ChenhLechDuAnNhomB { get; set; }

    public double? TongSoGoiThauDuAnNhomC { get; set; }

    public double? TongGiaGoiThauDuAnNhomC { get; set; }

    public double? TongGiaTrungThauDuAnNhomC { get; set; }

    public double? ChenhLechDuAnNhomC { get; set; }

    public double? TongSoGoiThauTongCong { get; set; }

    public double? TongGiaGoiThauTongCong { get; set; }

    public double? TongGiaTrungThauTongCong { get; set; }

    public double? ChenhLechTongCong { get; set; }

    public int? NguonVon { get; set; }

    public virtual QuanLyThuTucNoiBoBaoCaoTongHop? IdBaoCaoNavigation { get; set; }
}
