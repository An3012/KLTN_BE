using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucNoiBoBaoCaoTongHop
{
    public string Id { get; set; } = null!;

    public string? Ten { get; set; }

    /// <summary>
    /// 1: Báo cáo tổng hợp về thủ tục nội bộ - 2: báo cáo tổng hợp về gói thầu 
    /// </summary>
    public int? LoaiBaoCaoTongHop { get; set; }

    public string? Nam { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? NguoiTao { get; set; }

    public virtual ICollection<QuanLyThuTucBaoCaoTongHopVeGoiThau> QuanLyThuTucBaoCaoTongHopVeGoiThaus { get; set; } = new List<QuanLyThuTucBaoCaoTongHopVeGoiThau>();

    public virtual ICollection<QuanLyThuTucNoiBoBaoCaoTongHopChiTiet> QuanLyThuTucNoiBoBaoCaoTongHopChiTiets { get; set; } = new List<QuanLyThuTucNoiBoBaoCaoTongHopChiTiet>();
}
