using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucNoiBoChuyenThuLy
{
    public string Id { get; set; } = null!;

    public string? IdThuTuc { get; set; }

    public string? PhongBan { get; set; }

    public string? ChuyenVien { get; set; }

    public DateTime? NgayChuyenThuLy { get; set; }

    public virtual QuanLyThuTucNoiBoDuAnDtc? IdThuTucNavigation { get; set; }
}
