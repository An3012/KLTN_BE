using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmTaiLieu
{
    public string Id { get; set; } = null!;

    public string? TenTaiLieu { get; set; }

    public string? MaTaiLieu { get; set; }

    public string? TenHienThi { get; set; }

    public string? TenHeThong { get; set; }

    public string? GhiChu { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdateBy { get; set; }
}
