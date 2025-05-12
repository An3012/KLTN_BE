using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class HtQuyenNhomQuyen
{
    public string Id { get; set; } = null!;

    public string? HtQuyenId { get; set; }

    public string? HtNhomQuyenId { get; set; }

    public virtual HtNhomQuyen? HtNhomQuyen { get; set; }

    public virtual HtQuyen? HtQuyen { get; set; }
}
