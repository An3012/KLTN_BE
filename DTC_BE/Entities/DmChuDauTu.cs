using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class DmChuDauTu
{
    public string Id { get; set; } = null!;

    public string? TenChuDauTu { get; set; }

    public string? MaSoThue { get; set; }

    public string? DiaChi { get; set; }

    public string? Email { get; set; }

    public string? SoDienThoai { get; set; }

    /// <summary>
    /// Để phân biệt là ủy ban nhân dân huyện hoặc ko 0 là huyện 1; khác huyện
    /// </summary>
    public int? Loai { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? NguoiTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? NguoiCapNhat { get; set; }

    public string? TinhThanh { get; set; }

    public string? XaPhuong { get; set; }

    public string? QuanHuyen { get; set; }

    public string? NguoiDaiDien { get; set; }

    public DateTime? NgayHoatDong { get; set; }

    public virtual ICollection<DmDuAn> DmDuAns { get; set; } = new List<DmDuAn>();
}
