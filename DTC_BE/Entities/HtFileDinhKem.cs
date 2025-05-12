using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class HtFileDinhKem
{
    public string Id { get; set; } = null!;

    public string? ObjectId { get; set; }

    public int? Type { get; set; }

    public string? TenHienThi { get; set; }

    public string? TenHeThong { get; set; }
}
