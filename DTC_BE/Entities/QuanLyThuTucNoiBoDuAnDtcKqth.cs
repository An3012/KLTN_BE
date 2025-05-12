using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucNoiBoDuAnDtcKqth
{
    public string Id { get; set; } = null!;

    public string? IdThuTuc { get; set; }

    public string? SoNgayVb1 { get; set; }

    public string? SoNgayVb2 { get; set; }

    public DateTime? NgayKy1 { get; set; }

    public DateTime? NgayKy2 { get; set; }

    public virtual QuanLyThuTucNoiBoDuAnDtc? IdThuTucNavigation { get; set; }
}
