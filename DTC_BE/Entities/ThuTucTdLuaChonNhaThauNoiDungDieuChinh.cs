using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class ThuTucTdLuaChonNhaThauNoiDungDieuChinh
{
    public string Id { get; set; } = null!;

    public string? IdThuTuc { get; set; }

    public string? TenGoiThau { get; set; }

    public string? ThoiGianBdToChucLuaChonNhaThau { get; set; }

    public string? DeNghiDieuChinh { get; set; }
}
