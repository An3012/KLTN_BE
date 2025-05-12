using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucNoiBoDuAnDtcNguonVon
{
    public string Id { get; set; } = null!;

    public string? IdThuTuc { get; set; }

    public string? LoaiNguonVon { get; set; }

    public double? GiaTriNguonVon { get; set; }

    public virtual QuanLyThuTucNoiBoDuAnDtc? IdThuTucNavigation { get; set; }
}
