using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmQuanHuyen
{
    public string Id { get; set; } = null!;

    public string? TenQuanHuyen { get; set; }

    public string? DmTinhThanhId { get; set; }

    public string? MaQuanHuyen { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdateBy { get; set; }
}
