using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmTrinhDon
{
    public string Id { get; set; } = null!;

    public string? Ten { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdateBy { get; set; }
}
