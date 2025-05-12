using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmGiaiDoan
{
    public string Id { get; set; } = null!;

    public string? TenGiaiDoan { get; set; }

    public int? NamBd { get; set; }

    public int? NamKt { get; set; }
}
