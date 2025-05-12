using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmNguonVon
{
    public string Id { get; set; } = null!;

    public string? TenNguonVon { get; set; }

    /// <summary>
    /// 2: Tỉnh 1: Trung ương 3: nước ngoài
    /// </summary>
    public int? LoaiNguonVon { get; set; }

    public string? MoTa { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdateBy { get; set; }

    public virtual ICollection<DmDuAnNguonVon> DmDuAnNguonVons { get; set; } = new List<DmDuAnNguonVon>();
}
