using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucCnkq
{
    public string Id { get; set; } = null!;

    public string? IdThuTuc { get; set; }

    public string? SoNgayQuyetDinh { get; set; }

    public string? SoNgayQuyetDinhBiDc { get; set; }

    public string? NamPheDuyet { get; set; }

    public string? ChuDauTu { get; set; }

    public string? DuAnId { get; set; }

    public int? NhomDuAn { get; set; }

    public double? DutoanCpCbdautu { get; set; }

    public double? DutoanCpCbdautuDc { get; set; }

    public string? CoCauNguonVon { get; set; }

    public string? CoCauNguonVonDc { get; set; }

    public string? NangLucThietKe { get; set; }

    public string? DiaDiemDauTu { get; set; }

    public double? TongMucDauTu { get; set; }

    public double? CpXayDung { get; set; }

    public double? CpThietbi { get; set; }

    public double? CpBoithuong { get; set; }

    public double? CpChung { get; set; }

    public double? CpDuphong { get; set; }

    public string? ThoigianThuchien { get; set; }

    public string? ThoigianThuchienDc { get; set; }

    public string? TiendoThuchien { get; set; }

    public string? GhiChu { get; set; }

    public string? SoBuocThietKe { get; set; }

    public string? HinhThucQuanly { get; set; }

    public string? TenKeHoach { get; set; }

    public virtual DmDuAn? DuAn { get; set; }

    public virtual QuanLyThuTucNoiBoDuAnDtc? IdThuTucNavigation { get; set; }
}
