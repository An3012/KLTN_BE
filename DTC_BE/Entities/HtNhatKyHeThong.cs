using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class HtNhatKyHeThong
{
    public string Id { get; set; } = null!;

    public string? TenNguoiDung { get; set; }

    public string? IpNguoiDung { get; set; }

    public string? MoTa { get; set; }

    public string? LoaiChucNang { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayCapNhat { get; set; }
}
