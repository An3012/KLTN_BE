using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class HtThamSoHeThong
{
    public string Id { get; set; } = null!;

    public string? ApDungMapExcel { get; set; }

    public string? TypeDocument { get; set; }

    public string? ImagePath { get; set; }

    public string? SmtpServer { get; set; }

    public string? DinhKyTuan { get; set; }
}
