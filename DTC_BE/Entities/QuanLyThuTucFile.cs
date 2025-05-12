using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucFile
{
    public string Id { get; set; } = null!;

    public int? Loai { get; set; }

    public string? FileDinhKem { get; set; }

    public string? FilePath { get; set; }

    public string? IdThuTuc { get; set; }
}
