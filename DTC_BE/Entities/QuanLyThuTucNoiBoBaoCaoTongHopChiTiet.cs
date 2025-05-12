using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucNoiBoBaoCaoTongHopChiTiet
{
    public string Id { get; set; } = null!;

    public string? IdBaoCao { get; set; }

    public int? LoaiHoSo { get; set; }

    public int? TongSoHsTiepNhan { get; set; }

    public int? TongSoHsDaGiaiQuyet { get; set; }

    public int? SlHsDaGqTruocHan { get; set; }

    public int? SlHsDaGqDungHan { get; set; }

    public int? SlHsDaGqQuaHan { get; set; }

    public int? TongSoHsDangGiaiQuyet { get; set; }

    public int? SlHsDangGqTrongHan { get; set; }

    public int? SlHsDangGqQuaHan { get; set; }

    public string? GhiChu { get; set; }

    public int? TongSoHsDaHoanThanh { get; set; }

    public int? SlHsLuuKho { get; set; }

    public int? SlHsChuaLuuKho { get; set; }

    public virtual QuanLyThuTucNoiBoBaoCaoTongHop? IdBaoCaoNavigation { get; set; }
}
