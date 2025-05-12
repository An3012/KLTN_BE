using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucNoiBoDuAnDtcPhieuXuLy
{
    public string Id { get; set; } = null!;

    public string? IdThuTuc { get; set; }

    public int? Buoc1 { get; set; }

    public int? Buoc6 { get; set; }

    public int? Buoc2 { get; set; }

    public int? Buoc3 { get; set; }

    public int? Buoc4 { get; set; }

    public int? Buoc5 { get; set; }

    public string? TextBuoc1 { get; set; }

    public string? TextBuoc6 { get; set; }

    public string? TextBuoc2 { get; set; }

    public string? TextBuoc3 { get; set; }

    public string? TextBuoc4 { get; set; }

    public string? TextBuoc5 { get; set; }

    public int? ThoiGianThucHien { get; set; }

    public virtual QuanLyThuTucNoiBoDuAnDtc? IdThuTucNavigation { get; set; }
}
