using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmDuAnNguonVon
{
    public string Id { get; set; } = null!;

    public string? DmDuAnId { get; set; }

    public string? DmNguonVonId { get; set; }

    public double? SoVon { get; set; }

    public virtual DmDuAn? DmDuAn { get; set; }

    public virtual DmNguonVon? DmNguonVon { get; set; }
}
