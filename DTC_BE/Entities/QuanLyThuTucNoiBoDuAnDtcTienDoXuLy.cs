using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucNoiBoDuAnDtcTienDoXuLy
{
    public string Id { get; set; } = null!;

    public string? IdThuTuc { get; set; }

    public string? SoNgayQuyetDinh { get; set; }

    public DateTime? NgayGiaiQuyet { get; set; }

    public int? TrangThai { get; set; }

    public string? GhiChuTinhTrang { get; set; }

    public string? IdChuyenVienThuLy { get; set; }

    public virtual HtNguoiDung? IdChuyenVienThuLyNavigation { get; set; }

    public virtual QuanLyThuTucNoiBoDuAnDtc? IdThuTucNavigation { get; set; }
}
