using DTC_BE.Entities;
using DTC_BE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DTC_BE.CodeBase;
using Microsoft.EntityFrameworkCore;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung;
using DTC_BE.Models.HeThong.DonVi;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.ThuTucThamDinhDTCBDT;
using Microsoft.IdentityModel.Tokens;
using static DTC_BE.CodeBase.Enums;

namespace DTC_BE.Controllers.QuanlyThuTucNoiBoVeDuAnDauTuCong
{

    [Route("api/QuanlyThuTucNoiBoVeDuAnDauTuCong/[controller]")]
    [ApiController]
    public class ThuTucThamDinhDuToanCBDTController : BaseApiController
    {
        #region ByListAll Thutucthamdinh
        [Route("getAllDanhMucThuTucThamDinhDuToanCBDT")]
        [HttpPost]
        public ResponseMessage getAllDanhMucThuTucThamDinhDuToanCBDT(TimKiemTTTDModels TimKiemThuTuc)
        {
            try
            {
                var listCDT = context.DmChuDauTus;
                List<SelectListItem> nhomDuAn = new List<SelectListItem>();
                foreach (Enums.NhomDuAn loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.NhomDuAn)))
                {
                    nhomDuAn.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }

                List<ThongTinTTTD> lstThuTuc = new List<ThongTinTTTD>();
                var lstChuyenThuLy = context.QuanLyThuTucNoiBoChuyenThuLies;
                var query = context.QuanLyThuTucNoiBoDuAnDtcs
                    .AsNoTracking()
                    .AsEnumerable()
                    .Where(s =>
                        (
                            (!string.IsNullOrEmpty(TimKiemThuTuc.TenHoSo) ? s.TenHoSo.ToLower().Trim().Contains(TimKiemThuTuc.TenHoSo.ToLower().Trim()) : true)
                            && (!string.IsNullOrEmpty(TimKiemThuTuc.MaHoSo) ? s.MaHoSo.ToLower().Trim().Contains(TimKiemThuTuc.MaHoSo.ToLower().Trim()) : true)
                            && (!string.IsNullOrEmpty(TimKiemThuTuc.DonViThucHienDuAn) ? s.IdDonViThucHienDuAn != null && s.IdDonViThucHienDuAn.ToLower().Trim().Contains(TimKiemThuTuc.DonViThucHienDuAn.ToLower().Trim()) : true)
                            && (TimKiemThuTuc.NhomDuAn != 0 ? s.NhomDuAn == TimKiemThuTuc.NhomDuAn : true)
                        )
                        && s.LoaiHoSo == (int)Enums.LoaiThuTuNoiBoDuAnDTC.ThuTucThamDinhDuToanChuanBiDauTu
                        && s.IsXoa != (int)Enums.IsXoa.DaXoa
                    ).ToList();

                // Bước 2: Lọc theo chuyên viên nếu có quyền
                var nguoiDung = context.HtNguoiDungs.FirstOrDefault(x => x.Id == TimKiemThuTuc.IdUser);
                var quyen = context.HtNhomQuyens.FirstOrDefault(x => x.Id == nguoiDung.HtNhomQuyenId).Ten;
                if (quyen == "Chuyên viên thụ lý")
                {
                    query = query.Where(s =>
                    {
                        var chuyenThuLyGanNhat = lstChuyenThuLy
                            .Where(x => x.IdThuTuc == s.Id)
                            .OrderByDescending(x => x.NgayChuyenThuLy)
                            .FirstOrDefault();

                        return chuyenThuLyGanNhat?.ChuyenVien == TimKiemThuTuc.IdUser;
                    }).ToList();
                }

                // Bước 3: Phân trang và ánh xạ dữ liệu
                lstThuTuc = query
                    .OrderByDescending(s => s.NgayTao)
                    .Skip(TimKiemThuTuc.rowPerPage * (TimKiemThuTuc.currentPage - 1))
                    .Take(TimKiemThuTuc.rowPerPage)
                    .Select(s => new ThongTinTTTD
                    {
                        Id = s.Id,
                        TenHoSo = s.TenHoSo,
                        MaHoSo = s.MaHoSo,
                        NhomDuAn = nhomDuAn.FirstOrDefault(x => x.Value == s.NhomDuAn.ToString())?.Text ?? "",
                        ChuDauTu = listCDT.FirstOrDefault(x => x.Id == s.IdDonViThucHienDuAn)?.TenChuDauTu ?? "",
                        NgayNhanHoSo = s.NgayNhanHoSo?.ToString("dd/MM/yyyy") ?? "",
                        HanGiaiQuyetHoSo = s.DuKienHoanThanh?.ToString("dd/MM/yyyy") ?? "",
                        ThongKeHanXuLy = GetHanXuLy(s.Id, s.DuKienHoanThanh),
                        TrangThaiLuuKho = s.LuuKho,
                        TinhTrangHoSo = GetTinhTrangThucHien(s.Id),
                        ChuyenVienThuLy = GetTenChuyenVien(s.Id),
                    }).ToList();

                int totalRecords = lstThuTuc.Count();
                message.IsError = false;
                message.ObjData = new { lstThuTuc, totalRecords };
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;
        }
        [Route("GetTenChuyenVien")]
        [NonAction]
        public string GetTenChuyenVien(string Id)
        {
            List<SelectListItem> listItemsChuyenVienThuLy = context.HtNguoiDungs
                    .Where(x => x.HtNhomQuyenId == EnumAttributesHelper.GetDescription(NhomQuyen.ChuyenVienThuLy))
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id,
                        Text = x.HoTen
                    })
                    .ToList();
            string TenChuyenVien = "";
            if (!string.IsNullOrEmpty(Id))
            {
                var x = context.QuanLyThuTucNoiBoChuyenThuLies
                               .Where(ct => ct.IdThuTuc == Id)
                               .OrderByDescending(ct => ct.NgayChuyenThuLy)
                               .FirstOrDefault()?.ChuyenVien ?? "";

                if (string.IsNullOrEmpty(x))
                {
                    TenChuyenVien = "";
                }
                else
                {
                    TenChuyenVien = listItemsChuyenVienThuLy
                        .FirstOrDefault(cv => cv.Value == x)?.Text ?? "";
                }
            }
            else
            {
                TenChuyenVien = "";
            }
            return TenChuyenVien;
        }

        [Route("GetTinhTrangThucHien")]
        [NonAction]
        public string GetTinhTrangThucHien(string Id)
        {
            string tinhTrang = "";
            if (!string.IsNullOrEmpty(Id))
            {
                var x = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.Where(x => x.IdThuTuc == Id).OrderByDescending(x => x.NgayGiaiQuyet).FirstOrDefault() != null ? context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.Where(x => x.IdThuTuc == Id).OrderByDescending(x => x.NgayGiaiQuyet).FirstOrDefault().TrangThai : 0;
                if (x == 0)
                {
                    tinhTrang = "Chưa giải quyết";
                }
                else
                {
                    List<SelectListItem> lstTrangThai = EnumHelper.GetListSelectItemByEnums(typeof(Enums.TINH_TRANG_HO_SO));
                    foreach (var item in lstTrangThai)
                    {
                        if (x.ToString() == item.Value)
                        {
                            tinhTrang = item.Text;
                        }
                    }
                }
            }
            else
            {
                tinhTrang = "";
            }
            return tinhTrang;
        }

        [Route("GetHanXuLy")]
        [NonAction]
        public string GetHanXuLy(string id, DateTime? hanTraKetQua)
        {
            var x = context.QuanLyThuTucNoiBoDuAnDtcKqths.FirstOrDefault(t => t.IdThuTuc == id);
            if (x == null)
                return DateTime.Now <= hanTraKetQua ? "Trong hạn xử lý" : "Quá hạn xử lý";

            if (x.NgayKy1 != null)
            {
                return x.NgayKy1 <= hanTraKetQua ? "Trong hạn xử lý" : "Quá hạn xử lý";
            }
            else if (x.NgayKy2 != null)
            {
                return x.NgayKy2 <= hanTraKetQua ? "Trong hạn xử lý" : "Quá hạn xử lý";
            }
            else
            {
                return DateTime.Now <= hanTraKetQua ? "Trong hạn xử lý" : "Quá hạn xử lý";
            }
        }

        #endregion
    }
}
