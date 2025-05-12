using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class HtNhomQuyen
{
    public string Id { get; set; } = null!;

    public string? Ten { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<HtNguoiDung> HtNguoiDungs { get; set; } = new List<HtNguoiDung>();

    public virtual ICollection<HtQuyenNhomQuyen> HtQuyenNhomQuyens { get; set; } = new List<HtQuyenNhomQuyen>();
}
