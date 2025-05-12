using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class HtQuyen
{
    public string Id { get; set; } = null!;

    public string? TenQuyen { get; set; }

    public string? Ma { get; set; }

    public string? MaCha { get; set; }

    public virtual ICollection<HtQuyenNhomQuyen> HtQuyenNhomQuyens { get; set; } = new List<HtQuyenNhomQuyen>();
}
