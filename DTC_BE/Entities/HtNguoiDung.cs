using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class HtNguoiDung
{
    public string Id { get; set; } = null!;

    public string? TenDangNhap { get; set; }

    public string? HoTen { get; set; }

    public string? MatKhau { get; set; }

    public int? TrangThai { get; set; }

    public string? DmDonViId { get; set; }

    public string? HtNhomQuyenId { get; set; }

    public string? Email { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public string? DmChuDauTuId { get; set; }

    /// <summary>
    /// 1: Chủ đầu tư 2: UBND huyện
    /// </summary>
    public string? LoaiTaiKhoan { get; set; }

    /// <summary>
    /// 0: nữ 1: nam
    /// </summary>
    public string? GioiTinh { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? NguoiTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? NguoiCapNhat { get; set; }

    public string? Refreshtoken { get; set; }

    public DateTime? Expiresat { get; set; }

    public bool? Isrefreshtokenrevoked { get; set; }

    public int? PhongBan { get; set; }

    public virtual HtNhomQuyen? HtNhomQuyen { get; set; }

    public virtual ICollection<QuanLyThuTucNoiBoDuAnDtcTienDoXuLy> QuanLyThuTucNoiBoDuAnDtcTienDoXuLies { get; set; } = new List<QuanLyThuTucNoiBoDuAnDtcTienDoXuLy>();
}
