using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmXaPhuong
{
    public string Id { get; set; } = null!;

    public string? TenXaPhuong { get; set; }

    public string? DmQuanHuyenId { get; set; }

    public string? MaXaPhuong { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdateBy { get; set; }
}
