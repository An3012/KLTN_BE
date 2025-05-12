using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class ThuTucNbLuaChonNhaThauPhanChiaGoiThau
{
    public string Id { get; set; } = null!;

    public string? IdThuTuc { get; set; }

    public string? TenGoiThau { get; set; }

    public double? GiaGoiThau { get; set; }

    public double? GiaTrungThau { get; set; }

    public int? NguonVon { get; set; }

    public int? LinhVuc { get; set; }

    public int? HinhThucDauThau { get; set; }

    public int? HinhThucLuaChonNhaThau { get; set; }

    public virtual QuanLyThuTucNoiBoDuAnDtc? IdThuTucNavigation { get; set; }
}
