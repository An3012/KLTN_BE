using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class HtMenu
{
    public string Id { get; set; } = null!;

    public string? TenMenu { get; set; }

    public string? Code { get; set; }

    public string? ParentCode { get; set; }

    public int? Cap { get; set; }

    public string? Icon { get; set; }

    public string? Link { get; set; }

    public string? RouterLink { get; set; }

    public int? IsActive { get; set; }

    public int? ThuTu { get; set; }

    public string? MoTa { get; set; }

    public int? PhanHe { get; set; }
}
