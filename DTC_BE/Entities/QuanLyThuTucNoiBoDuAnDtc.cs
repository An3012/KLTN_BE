using System;
using System.Collections.Generic;

namespace DTC_BE.Entities;

public partial class QuanLyThuTucNoiBoDuAnDtc
{
    public string Id { get; set; } = null!;

    public string? TenHoSo { get; set; }

    public string? MaHoSo { get; set; }

    public string? IdDonViThucHienDuAn { get; set; }

    public string? CacThongTinKhac { get; set; }

    /// <summary>
    /// 1 - Thủ tục thẩm định dự toán chuẩn bị đầu tư/ 
    /// 2 - Thủ tục thẩm định chủ trương đầu tư/ 
    /// 3 - thẩm định điều chỉnh chủ trương đầu tư/ 
    /// 4 - thủ tục thẩm đỉnh dự án đầu tư có cấu phần xây dưng/ 
    /// 5 - thủ tục thẩm định điều chỉnh dự án đầu tư có cấu phần xây dựng/ 
    /// 6 - thủ tục thẩm định dự dự án đầu tư không có cấu phần xây dựng/ 
    /// 7 - thủ tục thẩm định điều chỉnh dự án đầu tư không có cấu phần xây dựng/ 
    /// 8 - thủ tục thẩm định dự toán dự án đầu tư không có cấu phần xây dựng/ 
    /// 9 - thủ tục thẩm định dự toán điều chỉnh dự án đầu tư không có cấu phần xây dựng/ 
    /// </summary>
    public int? LoaiHoSo { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? NguoiTao { get; set; }

    public int? LuuKho { get; set; }

    /// <summary>
    /// 1:  Đã xóa
    /// </summary>
    public int? IsXoa { get; set; }

    public DateTime? DuKienHoanThanh { get; set; }

    public string? VanBanChuDauTu { get; set; }

    public int? NhomDuAn { get; set; }

    public DateTime? NgayNhanHoSo { get; set; }

    public virtual ICollection<QuanLyThuTucCnkq> QuanLyThuTucCnkqs { get; set; } = new List<QuanLyThuTucCnkq>();

    public virtual ICollection<QuanLyThuTucNoiBoChuyenThuLy> QuanLyThuTucNoiBoChuyenThuLies { get; set; } = new List<QuanLyThuTucNoiBoChuyenThuLy>();

    public virtual ICollection<QuanLyThuTucNoiBoDuAnDtcKqth> QuanLyThuTucNoiBoDuAnDtcKqths { get; set; } = new List<QuanLyThuTucNoiBoDuAnDtcKqth>();

    public virtual ICollection<QuanLyThuTucNoiBoDuAnDtcNguonVon> QuanLyThuTucNoiBoDuAnDtcNguonVons { get; set; } = new List<QuanLyThuTucNoiBoDuAnDtcNguonVon>();

    public virtual ICollection<QuanLyThuTucNoiBoDuAnDtcPhieuXuLy> QuanLyThuTucNoiBoDuAnDtcPhieuXuLies { get; set; } = new List<QuanLyThuTucNoiBoDuAnDtcPhieuXuLy>();

    public virtual ICollection<QuanLyThuTucNoiBoDuAnDtcTienDoXuLy> QuanLyThuTucNoiBoDuAnDtcTienDoXuLies { get; set; } = new List<QuanLyThuTucNoiBoDuAnDtcTienDoXuLy>();

    public virtual ICollection<ThuTucNbLuaChonNhaThauPhanChiaGoiThau> ThuTucNbLuaChonNhaThauPhanChiaGoiThaus { get; set; } = new List<ThuTucNbLuaChonNhaThauPhanChiaGoiThau>();
}
