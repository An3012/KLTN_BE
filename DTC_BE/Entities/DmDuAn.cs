using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmDuAn
{
    public string Id { get; set; } = null!;

    public string? DmChuDauTuId { get; set; }

    public string? TenDuAn { get; set; }

    public string? MaDuAn { get; set; }

    public DateTime? CreateAt { get; set; }

    public int? IsXoa { get; set; }

    public int? NhomDuAn { get; set; }

    public double? TongMucDauTu { get; set; }

    public string? CoCauNguonVon { get; set; }

    public string? ThoiGianThucHien { get; set; }

    public string? TienDoThucHien { get; set; }

    public string? NangLucThietKe { get; set; }

    public string? DiaDiemDauTu { get; set; }

    public string? GhiChu { get; set; }

    public string? HinhThucQuanLy { get; set; }

    public string? SoBuocThietKe { get; set; }

    public virtual DmChuDauTu? DmChuDauTu { get; set; }

    public virtual ICollection<QuanLyThuTucCnkq> QuanLyThuTucCnkqs { get; set; } = new List<QuanLyThuTucCnkq>();
}
