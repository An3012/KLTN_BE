using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmDonVi
{
    public string Id { get; set; } = null!;

    public string? TenDonVi { get; set; }

    public string? MaDonVi { get; set; }

    public string? IdCha { get; set; }

    public string? DiaChi { get; set; }

    public string? GhiChu { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? NguoiTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? NguoiCapNhat { get; set; }
}
