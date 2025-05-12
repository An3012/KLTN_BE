using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmCapCongTrinh
{
    public string Id { get; set; } = null!;

    public string? TenCapCongTrinh { get; set; }

    public string? MoTa { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdateBy { get; set; }

    public virtual ICollection<DmDuAn> DmDuAns { get; set; } = new List<DmDuAn>();
}
