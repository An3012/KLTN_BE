using DTC_BE.Entities;
using DTC_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung;
using DTC_BE.CodeBase;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.QlHoSoNoiBo;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.ThuTucTDDADTCKhongCoCPXD;
using static DTC_BE.CodeBase.Enums;
using NPOI.SS.Formula.Functions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace DTC_BE.Controllers.QuanlyThuTucNoiBoVeDuAnDauTuCong
{
    [Route("api/[controller]")]
    [ApiController]
    public class QlHoSoNoiBoController : BaseApiController
    {
        #region Tìm kiếm hồ sơ
        [Route("GetDanhSachHoSo")]
        [HttpPost]
        public ResponseMessage GetDanhSachHoSo(TimKiemHoSoNoiBo timKiemDanhSach)
        {
            try
            {
                var listCDT = context.DmChuDauTus;
                List<SelectListItem> nhomDuAn = new List<SelectListItem>();
                foreach (Enums.NhomDuAn loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.NhomDuAn)))
                {
                    nhomDuAn.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                var kqths = context.QuanLyThuTucNoiBoDuAnDtcKqths
                                    .Select(x => x.IdThuTuc)
                                    .ToHashSet();
                var tienDoXuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.ToList();
                var chuyenThuLy = context.QuanLyThuTucNoiBoChuyenThuLies.ToList();
                List<HoSoNoiBo> lstHoSoNoiBo = new List<HoSoNoiBo>();
                lstHoSoNoiBo = context.QuanLyThuTucNoiBoDuAnDtcs.AsNoTracking()
                                                                           .AsEnumerable()
                                                                           .Where(s =>
                                                                                (
                                                                                    (!string.IsNullOrWhiteSpace(timKiemDanhSach.TenHoSo) ? s.TenHoSo.ToLower().Trim().Contains(timKiemDanhSach.TenHoSo.ToLower().Trim()) : true)
                                                                                    && (!string.IsNullOrWhiteSpace(timKiemDanhSach.MaHoSo) ? s.MaHoSo.ToLower().Trim().Contains(timKiemDanhSach.MaHoSo.ToLower().Trim()) : true)
                                                                                    && (!string.IsNullOrWhiteSpace(timKiemDanhSach.DonViThucHienDuAn) ? s.IdDonViThucHienDuAn != null && s.IdDonViThucHienDuAn.ToLower().Trim().Contains(timKiemDanhSach.DonViThucHienDuAn.ToLower().Trim()) : true)
                                                                                    && (timKiemDanhSach.NhomDuAn != 0 ? s.NhomDuAn == timKiemDanhSach.NhomDuAn : true)
                                                                                    && (
                                                                                        timKiemDanhSach.TrangThaiLuuKho == 1 ? s.LuuKho == 1 :
                                                                                        timKiemDanhSach.TrangThaiLuuKho == 0 ? (s.LuuKho == 0 || s.LuuKho == null) : true
                                                                                    )
                                                                                    && (timKiemDanhSach.QuyTrinhXuLy != 0 ? s.LoaiHoSo == timKiemDanhSach.QuyTrinhXuLy : true)
                                                                                )
                                                                                && (s.LoaiHoSo != (int)Enums.LoaiThuTuNoiBoDuAnDTC.ThuTucThamDinhKeHoachLuaChonNhaThau)
                                                                                && (s.LoaiHoSo != (int)Enums.LoaiThuTuNoiBoDuAnDTC.ThuTucThamDinhKeHoachLuaChonNhaThauDieuChinh)
                                                                                && (s.IsXoa != (int)Enums.IsXoa.DaXoa)
                                                                                && (tienDoXuLy.Where(td => td.IdThuTuc == s.Id)
                                                                                     .OrderByDescending(tdxl => tdxl.NgayGiaiQuyet)
                                                                                     .FirstOrDefault()?.TrangThai == 1)
                                                                            )
                                                       .OrderByDescending(s => s.NgayTao)
                                                       .Skip(timKiemDanhSach.rowPerPage * (timKiemDanhSach.currentPage - 1))
                                                       .Take(timKiemDanhSach.rowPerPage).Select(s => new HoSoNoiBo
                                                       {
                                                           Id = s.Id,
                                                           TenHoSo = s.TenHoSo,
                                                           MaHoSo = s.MaHoSo,
                                                           LuuKho = s.LuuKho,
                                                           NhomDuAn = nhomDuAn.FirstOrDefault(x => x.Value == s.NhomDuAn.ToString())?.Text ?? "",
                                                           ChuDauTu = listCDT.FirstOrDefault(x => x.Id == s.IdDonViThucHienDuAn)?.TenChuDauTu ?? "",
                                                           NgayNhanHoSo = s.NgayTao?.ToString("dd/MM/yyyy") ?? "",
                                                           HanGiaiQuyetHoSo = s.DuKienHoanThanh?.ToString("dd/MM/yyyy") ?? "",
                                                           ThongKeHanXuLy = GetHanXuLy(s.Id, s.DuKienHoanThanh),
                                                           TinhTrangHoSo = GetTinhTrangThucHien(s.Id),
                                                           ChuyenVienThuLy = GetTenChuyenVien(s.Id),
                                                           LoaiHoSo = s.LoaiHoSo,
                                                           NguoiTao = s.NguoiTao,
                                                       }).ToList();
                int totalRecords = context.QuanLyThuTucNoiBoDuAnDtcs.AsEnumerable().Where(s =>
                                                                                        (
                                                                                            (!string.IsNullOrWhiteSpace(timKiemDanhSach.TenHoSo) ? s.TenHoSo.ToLower().Trim().Contains(timKiemDanhSach.TenHoSo.ToLower().Trim()) : true)
                                                                                            && (!string.IsNullOrWhiteSpace(timKiemDanhSach.MaHoSo) ? s.MaHoSo.ToLower().Trim().Contains(timKiemDanhSach.MaHoSo.ToLower().Trim()) : true)
                                                                                            && (!string.IsNullOrWhiteSpace(timKiemDanhSach.DonViThucHienDuAn) ? s.IdDonViThucHienDuAn != null && s.IdDonViThucHienDuAn.ToLower().Trim().Contains(timKiemDanhSach.DonViThucHienDuAn.ToLower().Trim()) : true)
                                                                                            && (timKiemDanhSach.NhomDuAn != 0 ? s.NhomDuAn == timKiemDanhSach.NhomDuAn : true)
                                                                                            && (
                                                                                                timKiemDanhSach.TrangThaiLuuKho == 1 ? s.LuuKho == 1 :
                                                                                                timKiemDanhSach.TrangThaiLuuKho == 0 ? (s.LuuKho == 0 || s.LuuKho == null) : true
                                                                                            )
                                                                                            && (timKiemDanhSach.QuyTrinhXuLy != 0 ? s.LoaiHoSo == timKiemDanhSach.QuyTrinhXuLy : true)
                                                                                        )
                                                                                        && (s.LoaiHoSo != (int)Enums.LoaiThuTuNoiBoDuAnDTC.ThuTucThamDinhKeHoachLuaChonNhaThau)
                                                                                        && (s.LoaiHoSo != (int)Enums.LoaiThuTuNoiBoDuAnDTC.ThuTucThamDinhKeHoachLuaChonNhaThauDieuChinh)
                                                                                        && (s.IsXoa != (int)Enums.IsXoa.DaXoa)
                                                                                        && (tienDoXuLy.Where(td => td.IdThuTuc == s.Id)
                                                                                             .OrderByDescending(tdxl => tdxl.NgayGiaiQuyet)
                                                                                             .FirstOrDefault()?.TrangThai == 1)
                                                                                    ).Count();
                message.IsError = false;
                message.ObjData = new { lstHoSoNoiBo, totalRecords };
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

        [Route("GetDanhSachHoSoLcnt")]
        [HttpPost]
        public ResponseMessage GetDanhSachHoSoLcnt(TimKiemHoSoNoiBo timKiemDanhSach)
        {
            try
            {
                var listCDT = context.DmChuDauTus;
                List<SelectListItem> nhomDuAn = new List<SelectListItem>();
                foreach (Enums.NhomDuAn loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.NhomDuAn)))
                {
                    nhomDuAn.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                var kqths = context.QuanLyThuTucNoiBoDuAnDtcKqths
                .Select(x => x.IdThuTuc)
                                    .ToHashSet();
                var tienDoXuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.ToList();
                var chuyenThuLy = context.QuanLyThuTucNoiBoChuyenThuLies.ToList();
                List<HoSoNoiBo> lstHoSoNoiBo = new List<HoSoNoiBo>();
                lstHoSoNoiBo = context.QuanLyThuTucNoiBoDuAnDtcs.AsNoTracking()
                                                                           .AsEnumerable()
                                                                           .Where(s =>
                                                                                (
                                                                                    (!string.IsNullOrWhiteSpace(timKiemDanhSach.TenHoSo) ? s.TenHoSo.ToLower().Trim().Contains(timKiemDanhSach.TenHoSo.ToLower().Trim()) : true)
                                                                                    && (!string.IsNullOrWhiteSpace(timKiemDanhSach.MaHoSo) ? s.MaHoSo.ToLower().Trim().Contains(timKiemDanhSach.MaHoSo.ToLower().Trim()) : true)
                                                                                    && (!string.IsNullOrWhiteSpace(timKiemDanhSach.DonViThucHienDuAn) ? s.IdDonViThucHienDuAn != null && s.IdDonViThucHienDuAn.ToLower().Trim().Contains(timKiemDanhSach.DonViThucHienDuAn.ToLower().Trim()) : true)
                                                                                    && (timKiemDanhSach.NhomDuAn != 0 ? s.NhomDuAn == timKiemDanhSach.NhomDuAn : true)
                                                                                    && (
                                                                                        timKiemDanhSach.TrangThaiLuuKho == 1 ? s.LuuKho == 1 :
                                                                                        timKiemDanhSach.TrangThaiLuuKho == 0 ? (s.LuuKho == 0 || s.LuuKho == null) : true
                                                                                    )
                                                                                    && (timKiemDanhSach.QuyTrinhXuLy != 0 ? s.LoaiHoSo == timKiemDanhSach.QuyTrinhXuLy : true)
                                                                                )
                                                                                && (s.LoaiHoSo == (int)Enums.LoaiThuTuNoiBoDuAnDTC.ThuTucThamDinhKeHoachLuaChonNhaThau ||
          s.LoaiHoSo == (int)Enums.LoaiThuTuNoiBoDuAnDTC.ThuTucThamDinhKeHoachLuaChonNhaThauDieuChinh)

                                                                                && (s.IsXoa != (int)Enums.IsXoa.DaXoa)
                                                                                && (tienDoXuLy.Where(td => td.IdThuTuc == s.Id)
                                                                                     .OrderByDescending(tdxl => tdxl.NgayGiaiQuyet)
                                                                                     .FirstOrDefault()?.TrangThai == 1)
                                                                            )
                                                       .OrderByDescending(s => s.NgayTao)
                                                       .Skip(timKiemDanhSach.rowPerPage * (timKiemDanhSach.currentPage - 1))
                                                       .Take(timKiemDanhSach.rowPerPage).Select(s => new HoSoNoiBo
                                                       {
                                                           Id = s.Id,
                                                           TenHoSo = s.TenHoSo,
                                                           MaHoSo = s.MaHoSo,
                                                           LuuKho = s.LuuKho,
                                                           NhomDuAn = nhomDuAn.FirstOrDefault(x => x.Value == s.NhomDuAn.ToString())?.Text ?? "",
                                                           ChuDauTu = listCDT.FirstOrDefault(x => x.Id == s.IdDonViThucHienDuAn)?.TenChuDauTu ?? "",
                                                           NgayNhanHoSo = s.NgayTao?.ToString("dd/MM/yyyy") ?? "",
                                                           HanGiaiQuyetHoSo = s.DuKienHoanThanh?.ToString("dd/MM/yyyy") ?? "",
                                                           ThongKeHanXuLy = GetHanXuLy(s.Id, s.DuKienHoanThanh),
                                                           TinhTrangHoSo = GetTinhTrangThucHien(s.Id),
                                                           ChuyenVienThuLy = GetTenChuyenVien(s.Id),
                                                           LoaiHoSo = s.LoaiHoSo,
                                                           NguoiTao = s.NguoiTao,
                                                       }).ToList();
                int totalRecords = context.QuanLyThuTucNoiBoDuAnDtcs.AsEnumerable().Where(s =>
                                                                                        (
                                                                                            (!string.IsNullOrWhiteSpace(timKiemDanhSach.TenHoSo) ? s.TenHoSo.ToLower().Trim().Contains(timKiemDanhSach.TenHoSo.ToLower().Trim()) : true)
                                                                                            && (!string.IsNullOrWhiteSpace(timKiemDanhSach.MaHoSo) ? s.MaHoSo.ToLower().Trim().Contains(timKiemDanhSach.MaHoSo.ToLower().Trim()) : true)
                                                                                            && (!string.IsNullOrWhiteSpace(timKiemDanhSach.DonViThucHienDuAn) ? s.IdDonViThucHienDuAn != null && s.IdDonViThucHienDuAn.ToLower().Trim().Contains(timKiemDanhSach.DonViThucHienDuAn.ToLower().Trim()) : true)
                                                                                            && (timKiemDanhSach.NhomDuAn != 0 ? s.NhomDuAn == timKiemDanhSach.NhomDuAn : true)
                                                                                            && (
                                                                                                timKiemDanhSach.TrangThaiLuuKho == 1 ? s.LuuKho == 1 :
                                                                                                timKiemDanhSach.TrangThaiLuuKho == 0 ? (s.LuuKho == 0 || s.LuuKho == null) : true
                                                                                            )
                                                                                            && (timKiemDanhSach.QuyTrinhXuLy != 0 ? s.LoaiHoSo == timKiemDanhSach.QuyTrinhXuLy : true)
                                                                                        )
                                                                                        && (s.LoaiHoSo == (int)Enums.LoaiThuTuNoiBoDuAnDTC.ThuTucThamDinhKeHoachLuaChonNhaThau ||
          s.LoaiHoSo == (int)Enums.LoaiThuTuNoiBoDuAnDTC.ThuTucThamDinhKeHoachLuaChonNhaThauDieuChinh)
                                                                                        && (s.IsXoa != (int)Enums.IsXoa.DaXoa)
                                                                                        && (tienDoXuLy.Where(td => td.IdThuTuc == s.Id)
                                                                                             .OrderByDescending(tdxl => tdxl.NgayGiaiQuyet)
                                                                                             .FirstOrDefault()?.TrangThai == 1)
                                                                                    ).Count();
                message.IsError = false;
                message.ObjData = new { lstHoSoNoiBo, totalRecords };
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

        [Route("GetListsDuAn")]
        [HttpPost]
        public ResponseMessage GetListsDuAn(TimKiemDuAn timKiemDanhSach)
        {
            try
            {
                var listCDT = context.DmChuDauTus;
                List<DM_DuAnmodel> DmDuAn = new List<DM_DuAnmodel>();
                DmDuAn = context.DmDuAns.AsNoTracking()
                                        .AsEnumerable()
                                        .Where(s =>
                                             (
                                                 (!string.IsNullOrWhiteSpace(timKiemDanhSach.TenDuAn) ? s.TenDuAn.ToLower().Trim().Contains(timKiemDanhSach.TenDuAn.ToLower().Trim()) : true)
                                                 && (!string.IsNullOrWhiteSpace(timKiemDanhSach.MaDuAn) ? s.MaDuAn.ToLower().Trim().Contains(timKiemDanhSach.MaDuAn.ToLower().Trim()) : true)
                                             )
                                         )
                                                       .OrderByDescending(s => s.CreateAt)
                                                       .Skip(timKiemDanhSach.rowPerPage * (timKiemDanhSach.currentPage - 1))
                                                       .Take(timKiemDanhSach.rowPerPage).Select(s => new DM_DuAnmodel
                                                       {
                                                           Id = s.Id,
                                                           TenDuAn = s.TenDuAn,
                                                           MaDuAn = s.MaDuAn,

                                                       }).ToList();
                int totalRecords = context.DmDuAns.AsEnumerable().Where(s =>
                                             (
                                                 (!string.IsNullOrWhiteSpace(timKiemDanhSach.TenDuAn) ? s.TenDuAn.ToLower().Trim().Contains(timKiemDanhSach.TenDuAn.ToLower().Trim()) : true)
                                                 && (!string.IsNullOrWhiteSpace(timKiemDanhSach.MaDuAn) ? s.MaDuAn.ToLower().Trim().Contains(timKiemDanhSach.MaDuAn.ToLower().Trim()) : true)
                                             )
                                            ).ToList().Count();
                message.IsError = false;
                message.ObjData = new { DmDuAn, totalRecords };
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


        [Route("GetDuAn/{id}")]
        [HttpPost]
        public ResponseMessage GetDuAn(string? id)
        {
            try
            {
                var listCDT = context.DmChuDauTus;
                var DmDuAn = context.DmDuAns.FirstOrDefault(x => x.Id == id);
                var TenDuAn = DmDuAn?.TenDuAn?.Trim();
                var MaDuAn = DmDuAn?.MaDuAn?.Trim();
                message.IsError = false;
                message.ObjData = new { TenDuAn, MaDuAn };
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
        [Route("ThemMoiDuAn")]
        [NonAction]
        public ResponseMessage ThemMoiDuAn(DM_DuAnmodel objDuAn)
        {
            try
            {
                DmDuAn tienDoXuLy = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    MaDuAn = objDuAn?.MaDuAn?.Trim(),
                    TenDuAn = objDuAn?.TenDuAn?.Trim(),
                };

                context.DmDuAns.Add(tienDoXuLy);
                message.Title = "Thêm mới dự án thành công";
                message.IsError = false;
                message.Data = tienDoXuLy.Id;
                message.Code = HttpStatusCode.OK.GetHashCode();


                context.SaveChanges();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = ex.Message;
            }

            return message;
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
        #region get thủ tục lựa chọn nhà thầu
        [Route("GetSelectedListItemLcnt")]
        [HttpGet]
        public ResponseMessage GetSelectedListItemLcnt()
        {
            try
            {
                List<SelectListItem> lstLinhVuc = new List<SelectListItem>();

                foreach (Enums.LinhVuc loaiTrangThai in (Enums.LinhVuc[])Enum.GetValues(typeof(Enums.LinhVuc)))
                {
                    lstLinhVuc.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }

                List<SelectListItem> lstHinhThucLuaChonNhaThau = new List<SelectListItem>();

                foreach (Enums.HinhThucLuaChonNhaThau loaiTrangThai in (Enums.HinhThucLuaChonNhaThau[])Enum.GetValues(typeof(Enums.HinhThucLuaChonNhaThau)))
                {
                    lstHinhThucLuaChonNhaThau.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                List<SelectListItem> lstHinhThucDauThau = new List<SelectListItem>();

                foreach (Enums.HinhThucDauThau loaiTrangThai in (Enums.HinhThucDauThau[])Enum.GetValues(typeof(Enums.HinhThucDauThau)))
                {
                    lstHinhThucDauThau.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = new { lstLinhVuc, lstHinhThucLuaChonNhaThau, lstHinhThucDauThau };
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;
        }

        [Route("GetPhanChiaDuAnThanhCacGoiThauById")]
        [HttpGet]
        public ResponseMessage GetThuTucLuaChonNhaThauById(string? id)
        {
            try
            {
                List<PhanChiaDuAnThanhCacGoiThau> lstPhanChiaGoiThau = context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Where(nguonVon => nguonVon.IdThuTuc == id)
                                                                        .Select(thuTuc => new PhanChiaDuAnThanhCacGoiThau
                                                                        {
                                                                            TenGoiThau = thuTuc.TenGoiThau,
                                                                            GiaGoiThau = thuTuc.GiaGoiThau,
                                                                            GiaTrungThau = thuTuc.GiaTrungThau,
                                                                            NguonVon = thuTuc.NguonVon,
                                                                            LinhVuc = thuTuc.LinhVuc,
                                                                            HinhThucDauThau = thuTuc.HinhThucDauThau,
                                                                            HinhThucLuaChonNhaThau = thuTuc.HinhThucLuaChonNhaThau,
                                                                        })
                                                                        .ToList();
                if (!lstPhanChiaGoiThau.Any())
                {
                    lstPhanChiaGoiThau.Add(new PhanChiaDuAnThanhCacGoiThau
                    {
                        TenGoiThau = string.Empty,
                        GiaGoiThau = 0,
                        GiaTrungThau = 0,
                        NguonVon = 0, // hoặc giá trị mặc định nếu bạn muốn
                        LinhVuc = 0,
                        HinhThucDauThau = 0,
                        HinhThucLuaChonNhaThau = 0
                    });
                }
                message.ObjData = new { lstPhanChiaGoiThau };
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = ex.Message;
            }
            return message;
        }

        #endregion

        [Route("GetHoSoNoiBoById/{id}")]
        [HttpGet]
        public ResponseMessage GetHoSoNoiBoById(string? id)
        {
            try
            {
                var HoSoNoiBo = context.QuanLyThuTucNoiBoDuAnDtcs.FirstOrDefault(x => x.Id == id);
                var ThuTucCNKQ = context.QuanLyThuTucCnkqs.FirstOrDefault(x => x.IdThuTuc == id);

                if (HoSoNoiBo == null)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = "Không tìm thấy hồ sơ nội bộ!";
                }

                if (ThuTucCNKQ == null)
                {
                    message.ObjData = CreateModelWhenThuTucNull(HoSoNoiBo);
                }
                else
                {
                    message.ObjData = CreateModelWhenThuTucExist(HoSoNoiBo, ThuTucCNKQ);
                }

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = ex.Message;
            }
            return message;
        }
        private object CreateModelWhenThuTucNull(QuanLyThuTucNoiBoDuAnDtc hoSo)
        {
            switch (hoSo.LoaiHoSo)
            {
                case 1:
                case 8:
                    return new DuToanCbDauTuCnkqModel
                    {
                        Id = "",
                        IdThuTuc = hoSo.Id,
                        ChuDauTu = hoSo.IdDonViThucHienDuAn,
                        DuAnId = "",
                        SoNgayQuyetDinh = "",
                        NamPheDuyet = "",
                        DutoanCpCbdautu = 0,
                        CoCauNguonVon = ""
                    };
                case 2:
                    return new ChuTruongDauTuCnkqModel
                    {
                        Id = "",
                        IdThuTuc = hoSo.Id,
                        ChuDauTu = hoSo.IdDonViThucHienDuAn,
                        NhomDuAn = hoSo.NhomDuAn,
                        DuAnId = "",
                        SoNgayQuyetDinh = "",
                        NamPheDuyet = "",
                        NangLucThietKe = "",
                        DiaDiemDauTu = "",
                        CoCauNguonVon = "",
                        TongMucDauTu = 0,
                        CpXayDung = 0,
                        CpThietbi = 0,
                        CpBoithuong = 0,
                        CpChung = 0,
                        CpDuphong = 0,
                        ThoigianThuchien = "",
                        TiendoThuchien = ""
                    };
                case 3:
                    return new DcChuTruongDauTuCnkqModel
                    {
                        Id = "",
                        IdThuTuc = hoSo.Id,
                        ChuDauTu = hoSo.IdDonViThucHienDuAn,
                        DuAnId = "",
                        SoNgayQuyetDinh = "",
                        SoNgayQuyetDinhBiDc = "",
                        NamPheDuyet = "",
                        CoCauNguonVonDieuChinh = "",
                        GhiChu = "",
                        TongMucDauTu = 0,
                        CpXayDung = 0,
                        CpThietbi = 0,
                        CpBoithuong = 0,
                        CpChung = 0,
                        CpDuphong = 0
                    };
                case 4:
                case 6:
                    return new DuAnDauTuCnkqModel
                    {
                        Id = "",
                        IdThuTuc = hoSo.Id,
                        ChuDauTu = hoSo.IdDonViThucHienDuAn,
                        NhomDuAn = hoSo.NhomDuAn,
                        DuAnId = "",
                        SoNgayQuyetDinh = "",
                        NamPheDuyet = "",
                        NangLucThietKe = "",
                        DiaDiemDauTu = "",
                        CoCauNguonVon = "",
                        TongMucDauTu = 0,
                        CpXayDung = 0,
                        CpThietbi = 0,
                        CpBoithuong = 0,
                        CpChung = 0,
                        CpDuphong = 0,
                        ThoigianThuchien = "",
                        SoBuocThietKe = "",
                        HinhThucQuanly = ""
                    };
                case 5:
                case 7:
                    return new DCDuAnDauTuCnkqModel
                    {
                        Id = "",
                        IdThuTuc = hoSo.Id,
                        ChuDauTu = hoSo.IdDonViThucHienDuAn,
                        DuAnId = "",
                        SoNgayQuyetDinh = "",
                        SoNgayQuyetDinhBiDc = "",
                        NamPheDuyet = "",
                        CoCauNguonVonDieuChinh = "",
                        GhiChu = "",
                        ThoigianThuchienDc = ""
                    };
                case 9:
                    return new DtDCDaKoCoCpxdCnkqModel
                    {
                        Id = "",
                        IdThuTuc = hoSo.Id,
                        ChuDauTu = hoSo.IdDonViThucHienDuAn,
                        DuAnId = "",
                        SoNgayQuyetDinh = "",
                        SoNgayQuyetDinhBiDc = "",
                        NamPheDuyet = "",
                        DutoanCpCbdautuDieuChinh = 0,
                        CoCauNguonVonDieuChinh = "",
                        GhiChu = ""
                    };
                case 10:
                case 11:
                    return new LCNTCnkqModel
                    {
                        Id = "",
                        IdThuTuc = hoSo.Id,
                        ChuDauTu = hoSo.IdDonViThucHienDuAn,
                        NhomDuAn = hoSo.NhomDuAn,
                        SoNgayQuyetDinh = "",
                        SoNgayQuyetDinhBiDc = "",
                        NamPheDuyet = "",
                        TenKeHoach = hoSo.TenHoSo,
                    };

                default:
                    return null;
            }
        }

        private object CreateModelWhenThuTucExist(QuanLyThuTucNoiBoDuAnDtc hoSo, QuanLyThuTucCnkq thuTuc)
        {
            switch (hoSo.LoaiHoSo)
            {
                case 1:
                case 8:
                    return new DuToanCbDauTuCnkqModel
                    {
                        Id = thuTuc.Id,
                        IdThuTuc = hoSo.Id,
                        ChuDauTu = thuTuc.ChuDauTu,
                        DuAnId = thuTuc.DuAnId,
                        SoNgayQuyetDinh = thuTuc.SoNgayQuyetDinh,
                        NamPheDuyet = thuTuc.NamPheDuyet,
                        DutoanCpCbdautu = thuTuc.DutoanCpCbdautu,
                        CoCauNguonVon = thuTuc.CoCauNguonVon
                    };
                case 2:
                    return new ChuTruongDauTuCnkqModel
                    {
                        Id = thuTuc.Id ?? "",
                        IdThuTuc = thuTuc.IdThuTuc ?? "",
                        ChuDauTu = thuTuc.ChuDauTu ?? "",
                        NhomDuAn = thuTuc.NhomDuAn ?? 0,
                        DuAnId = thuTuc.DuAnId ?? "",
                        SoNgayQuyetDinh = thuTuc.SoNgayQuyetDinh ?? "",
                        NamPheDuyet = thuTuc.NamPheDuyet ?? "",
                        NangLucThietKe = thuTuc.NangLucThietKe ?? "",
                        DiaDiemDauTu = thuTuc.DiaDiemDauTu ?? "",
                        CoCauNguonVon = thuTuc.CoCauNguonVon ?? "",
                        TongMucDauTu = thuTuc.TongMucDauTu ?? 0,
                        CpXayDung = thuTuc.CpXayDung ?? 0,
                        CpThietbi = thuTuc.CpThietbi ?? 0,
                        CpBoithuong = thuTuc.CpBoithuong ?? 0,
                        CpChung = thuTuc.CpChung ?? 0,
                        CpDuphong = thuTuc.CpDuphong ?? 0,
                        ThoigianThuchien = thuTuc.ThoigianThuchien ?? "",
                        TiendoThuchien = thuTuc.TiendoThuchien ?? ""
                    };
                case 3:
                    return new DcChuTruongDauTuCnkqModel
                    {
                        Id = thuTuc.Id ?? "",
                        IdThuTuc = thuTuc.IdThuTuc ?? "",
                        ChuDauTu = thuTuc.ChuDauTu ?? "",
                        DuAnId = thuTuc.DuAnId ?? "",
                        SoNgayQuyetDinh = thuTuc.SoNgayQuyetDinh ?? "",
                        SoNgayQuyetDinhBiDc = thuTuc.SoNgayQuyetDinhBiDc ?? "",
                        NamPheDuyet = thuTuc.NamPheDuyet ?? "",
                        CoCauNguonVonDieuChinh = thuTuc.CoCauNguonVonDc ?? "",
                        GhiChu = thuTuc.GhiChu ?? "",
                        TongMucDauTu = thuTuc.TongMucDauTu ?? 0,
                        CpXayDung = thuTuc.CpXayDung ?? 0,
                        CpThietbi = thuTuc.CpThietbi ?? 0,
                        CpBoithuong = thuTuc.CpBoithuong ?? 0,
                        CpChung = thuTuc.CpChung ?? 0,
                        CpDuphong = thuTuc.CpDuphong ?? 0
                    };
                case 4:
                case 6:
                    return new DuAnDauTuCnkqModel
                    {
                        Id = thuTuc.Id ?? "",
                        IdThuTuc = thuTuc.IdThuTuc ?? "",
                        ChuDauTu = thuTuc.ChuDauTu ?? "",
                        NhomDuAn = thuTuc.NhomDuAn ?? 0,
                        DuAnId = thuTuc.DuAnId ?? "",
                        SoNgayQuyetDinh = thuTuc.SoNgayQuyetDinh ?? "",
                        NamPheDuyet = thuTuc.NamPheDuyet ?? "",
                        NangLucThietKe = thuTuc.NangLucThietKe ?? "",
                        DiaDiemDauTu = thuTuc.DiaDiemDauTu ?? "",
                        CoCauNguonVon = thuTuc.CoCauNguonVon ?? "",
                        TongMucDauTu = thuTuc.TongMucDauTu ?? 0,
                        CpXayDung = thuTuc.CpXayDung ?? 0,
                        CpThietbi = thuTuc.CpThietbi ?? 0,
                        CpBoithuong = thuTuc.CpBoithuong ?? 0,
                        CpChung = thuTuc.CpChung ?? 0,
                        CpDuphong = thuTuc.CpDuphong ?? 0,
                        ThoigianThuchien = thuTuc.ThoigianThuchien ?? "",
                        SoBuocThietKe = thuTuc.SoBuocThietKe ?? "",
                        HinhThucQuanly = thuTuc.HinhThucQuanly ?? ""
                    };
                case 5:
                case 7:
                    return new DCDuAnDauTuCnkqModel
                    {
                        Id = thuTuc.Id ?? "",
                        IdThuTuc = thuTuc.IdThuTuc ?? "",
                        ChuDauTu = thuTuc.ChuDauTu ?? "",
                        DuAnId = thuTuc.DuAnId ?? "",
                        SoNgayQuyetDinh = thuTuc.SoNgayQuyetDinh ?? "",
                        SoNgayQuyetDinhBiDc = thuTuc.SoNgayQuyetDinhBiDc ?? "",
                        NamPheDuyet = thuTuc.NamPheDuyet ?? "",
                        CoCauNguonVonDieuChinh = thuTuc.CoCauNguonVonDc ?? "",
                        GhiChu = thuTuc.GhiChu ?? "",
                        ThoigianThuchienDc = thuTuc.ThoigianThuchienDc ?? ""
                    };
                case 9:
                    return new DtDCDaKoCoCpxdCnkqModel
                    {
                        Id = thuTuc.Id ?? "",
                        IdThuTuc = thuTuc.IdThuTuc ?? "",
                        ChuDauTu = thuTuc.ChuDauTu ?? "",
                        DuAnId = thuTuc.DuAnId ?? "",
                        SoNgayQuyetDinh = thuTuc.SoNgayQuyetDinh ?? "",
                        SoNgayQuyetDinhBiDc = thuTuc.SoNgayQuyetDinhBiDc ?? "",
                        NamPheDuyet = thuTuc.NamPheDuyet ?? "",
                        DutoanCpCbdautuDieuChinh = thuTuc.DutoanCpCbdautuDc ?? 0,
                        CoCauNguonVonDieuChinh = thuTuc.CoCauNguonVonDc ?? ""
                    };
                case 10:
                case 11:
                    return new LCNTCnkqModel
                    {
                        Id = thuTuc.Id ?? "",
                        IdThuTuc = thuTuc.IdThuTuc ?? "",
                        ChuDauTu = thuTuc.ChuDauTu ?? "",
                        NhomDuAn = thuTuc.NhomDuAn ?? 0,
                        SoNgayQuyetDinh = thuTuc.SoNgayQuyetDinh ?? "",
                        SoNgayQuyetDinhBiDc = thuTuc.SoNgayQuyetDinhBiDc ?? "",
                        NamPheDuyet = thuTuc.NamPheDuyet ?? "",
                        TenKeHoach = thuTuc.TenKeHoach ?? "",
                    };
                default:
                    return null;
            }
        }
        [Route("GetDanhSachLoaiHoSo")]
        [HttpGet]
        public ResponseMessage GetDanhSachLoaiHoSo()
        {
            try
            {
                List<SelectListItem> lstLoaiHoSo = new List<SelectListItem>();

                foreach (Enums.LoaiThuTuNoiBoDuAnDTC loaiTrangThai in (Enums.LoaiThuTuNoiBoDuAnDTC[])Enum.GetValues(typeof(Enums.LoaiThuTuNoiBoDuAnDTC)))
                {
                    lstLoaiHoSo.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }

                message.ObjData = lstLoaiHoSo;

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = ex.Message;
            }
            return message;
        }

        [Route("getLoaiHoSoById/{id}")]
        [HttpGet]
        public ResponseMessage GetLoaiHoSoById(string id)
        {
            try
            {
                List<SelectListItem> lstLoaiHoSo = new List<SelectListItem>();

                foreach (Enums.LoaiThuTuNoiBoDuAnDTC loaiTrangThai in (Enums.LoaiThuTuNoiBoDuAnDTC[])Enum.GetValues(typeof(Enums.LoaiThuTuNoiBoDuAnDTC)))
                {
                    lstLoaiHoSo.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }

                var LoaiHoso = lstLoaiHoSo.FirstOrDefault(LoaiHoso => LoaiHoso.Value == id);

                message.ObjData = LoaiHoso;

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = ex.Message;
            }
            return message;
        }

        [Route("LuuKhoHoSoNoiBo")]
        [HttpPost]
        public ResponseMessage LuuKhoHoSoNoiBo(string? id)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    QuanLyThuTucNoiBoDuAnDtc? hoSoNoiBo = context.QuanLyThuTucNoiBoDuAnDtcs.FirstOrDefault(hoSo => hoSo.Id == id);
                    if (hoSoNoiBo != null)
                    {
                        hoSoNoiBo.LuuKho = 1;
                    }

                    message.Title = "Lưu kho hồ sơ thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("HuyLuuKhoHoSoNoiBo")]
        [HttpPost]
        public ResponseMessage HuyLuuKhoHoSoNoiBo(string? id)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    QuanLyThuTucNoiBoDuAnDtc? hoSoNoiBo = context.QuanLyThuTucNoiBoDuAnDtcs.FirstOrDefault(hoSo => hoSo.Id == id);
                    if (hoSoNoiBo != null)
                    {
                        hoSoNoiBo.LuuKho = 0;
                    }

                    message.Title = "Hủy lưu kho hồ sơ thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();

                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("CapNhatKQHSDuToanCbDauTu")]
        [HttpPost]
        public ResponseMessage CapNhatKQHSDuToanCbDauTu(DuToanCbDauTuCnkqModel model)
        {
            try
            {
                string id1 = model.Id.ToString();
                var quanLyThuTucCNKQ1 = context.QuanLyThuTucCnkqs.FirstOrDefault(cnkq => cnkq.Id == id1);

                if (quanLyThuTucCNKQ1 == null)
                {
                    quanLyThuTucCNKQ1 = new QuanLyThuTucCnkq();
                    quanLyThuTucCNKQ1.Id = Guid.NewGuid().ToString();
                    context.QuanLyThuTucCnkqs.Add(quanLyThuTucCNKQ1);
                }

                // Gán giá trị thuộc tính dùng chung
                quanLyThuTucCNKQ1.IdThuTuc = model.IdThuTuc.Trim();
                quanLyThuTucCNKQ1.SoNgayQuyetDinh = model.SoNgayQuyetDinh.Trim();
                quanLyThuTucCNKQ1.NamPheDuyet = model.NamPheDuyet.Trim();
                quanLyThuTucCNKQ1.ChuDauTu = model.ChuDauTu.Trim();
                quanLyThuTucCNKQ1.DuAnId = model.DuAnId.Trim();
                quanLyThuTucCNKQ1.DutoanCpCbdautu = model.DutoanCpCbdautu;
                quanLyThuTucCNKQ1.CoCauNguonVon = model.CoCauNguonVon.Trim();

                context.SaveChanges(); // Lưu tất cả thay đổi ở đây
                message.Title = "Cập nhật kết quả hồ sơ dự toán đầu tư thành công";
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = ex.Message;
            }
            return message;
        }

        [Route("CapNhatKQHSChuTruongDauTu")]
        [HttpPost]
        public ResponseMessage CapNhatKQHSChuTruongDauTu(ChuTruongDauTuCnkqModel model2)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {

                    string id2 = model2.Id.ToString();
                    var quanLyThuTucCNKQ2 = context.QuanLyThuTucCnkqs.FirstOrDefault(cnkq => cnkq.Id == id2);

                    if (quanLyThuTucCNKQ2 == null)
                    {
                        quanLyThuTucCNKQ2 = new QuanLyThuTucCnkq();
                        quanLyThuTucCNKQ2.Id = Guid.NewGuid().ToString();
                        context.QuanLyThuTucCnkqs.Add(quanLyThuTucCNKQ2);
                    }

                    // Cập nhật hoặc gán các thuộc tính
                    quanLyThuTucCNKQ2.IdThuTuc = model2.IdThuTuc.Trim();
                    quanLyThuTucCNKQ2.SoNgayQuyetDinh = model2.SoNgayQuyetDinh.Trim();
                    quanLyThuTucCNKQ2.NamPheDuyet = model2.NamPheDuyet.Trim();
                    quanLyThuTucCNKQ2.ChuDauTu = model2.ChuDauTu.Trim();
                    quanLyThuTucCNKQ2.DuAnId = model2.DuAnId.Trim();
                    quanLyThuTucCNKQ2.NhomDuAn = model2.NhomDuAn;
                    quanLyThuTucCNKQ2.NangLucThietKe = model2.NangLucThietKe.Trim();
                    quanLyThuTucCNKQ2.DiaDiemDauTu = model2.DiaDiemDauTu.Trim();
                    quanLyThuTucCNKQ2.CoCauNguonVon = model2.CoCauNguonVon.Trim();
                    quanLyThuTucCNKQ2.TongMucDauTu = model2.TongMucDauTu;
                    quanLyThuTucCNKQ2.CpXayDung = model2.CpXayDung;
                    quanLyThuTucCNKQ2.CpThietbi = model2.CpThietbi;
                    quanLyThuTucCNKQ2.CpBoithuong = model2.CpBoithuong;
                    quanLyThuTucCNKQ2.CpChung = model2.CpChung;
                    quanLyThuTucCNKQ2.CpDuphong = model2.CpDuphong;
                    quanLyThuTucCNKQ2.ThoigianThuchien = model2.ThoigianThuchien.Trim();
                    quanLyThuTucCNKQ2.TiendoThuchien = model2.TiendoThuchien.Trim();


                    context.SaveChanges(); // Lưu tất cả thay đổi ở đây
                    trans.Commit();

                    message.Title = "Cập nhật kết quả hồ sơ chủ trương đầu tư thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        [Route("CapNhatKQHSDcChuTruongDauTu")]
        [HttpPost]
        public ResponseMessage CapNhatKQHSDcChuTruongDauTu(DcChuTruongDauTuCnkqModel model3)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string id3 = model3.Id?.ToString();
                    var quanLyThuTucCNKQ3 = context.QuanLyThuTucCnkqs.FirstOrDefault(cnkq => cnkq.Id == id3);

                    if (quanLyThuTucCNKQ3 == null)
                    {
                        quanLyThuTucCNKQ3 = new QuanLyThuTucCnkq();
                        quanLyThuTucCNKQ3.Id = Guid.NewGuid().ToString();
                        context.QuanLyThuTucCnkqs.Add(quanLyThuTucCNKQ3);
                    }

                    // Gán dữ liệu từ model vào entity
                    quanLyThuTucCNKQ3.IdThuTuc = model3.IdThuTuc.Trim();
                    quanLyThuTucCNKQ3.SoNgayQuyetDinhBiDc = model3.SoNgayQuyetDinhBiDc.Trim();
                    quanLyThuTucCNKQ3.SoNgayQuyetDinh = model3.SoNgayQuyetDinh.Trim();
                    quanLyThuTucCNKQ3.NamPheDuyet = model3.NamPheDuyet.Trim();
                    quanLyThuTucCNKQ3.ChuDauTu = model3.ChuDauTu.Trim();
                    quanLyThuTucCNKQ3.DuAnId = model3.DuAnId.Trim();
                    quanLyThuTucCNKQ3.CoCauNguonVonDc = model3.CoCauNguonVonDieuChinh.Trim();
                    quanLyThuTucCNKQ3.GhiChu = model3.GhiChu.Trim();
                    quanLyThuTucCNKQ3.TongMucDauTu = model3.TongMucDauTu;
                    quanLyThuTucCNKQ3.CpXayDung = model3.CpXayDung;
                    quanLyThuTucCNKQ3.CpThietbi = model3.CpThietbi;
                    quanLyThuTucCNKQ3.CpBoithuong = model3.CpBoithuong;
                    quanLyThuTucCNKQ3.CpChung = model3.CpChung;
                    quanLyThuTucCNKQ3.CpDuphong = model3.CpDuphong;

                    context.SaveChanges(); // Lưu tất cả thay đổi ở đây
                    trans.Commit();

                    message.Title = "Cập nhật kết quả hồ sơ điều chỉnh chủ trương đầu tư thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        [Route("CapNhatKQHSDuAnDauTu")]
        [HttpPost]
        public ResponseMessage CapNhatKQHSDuAnDauTu(DuAnDauTuCnkqModel model4)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string id4 = model4.Id?.ToString();

                    var quanLyThuTucCNKQ4 = context.QuanLyThuTucCnkqs.FirstOrDefault(cnkq => cnkq.Id == id4);
                    if (quanLyThuTucCNKQ4 == null)
                    {
                        quanLyThuTucCNKQ4 = new QuanLyThuTucCnkq
                        {
                            Id = Guid.NewGuid().ToString()
                        };
                        context.QuanLyThuTucCnkqs.Add(quanLyThuTucCNKQ4);
                    }

                    // Gán giá trị từ model4 sang quanLyThuTucCNKQ4
                    quanLyThuTucCNKQ4.IdThuTuc = model4.IdThuTuc.Trim();
                    quanLyThuTucCNKQ4.SoNgayQuyetDinh = model4.SoNgayQuyetDinh.Trim();
                    quanLyThuTucCNKQ4.NamPheDuyet = model4.NamPheDuyet.Trim();
                    quanLyThuTucCNKQ4.ChuDauTu = model4.ChuDauTu.Trim();
                    quanLyThuTucCNKQ4.DuAnId = model4.DuAnId.Trim();
                    quanLyThuTucCNKQ4.NhomDuAn = model4.NhomDuAn;
                    quanLyThuTucCNKQ4.NangLucThietKe = model4.NangLucThietKe.Trim();
                    quanLyThuTucCNKQ4.DiaDiemDauTu = model4.DiaDiemDauTu.Trim();
                    quanLyThuTucCNKQ4.CoCauNguonVon = model4.CoCauNguonVon.Trim();
                    quanLyThuTucCNKQ4.TongMucDauTu = model4.TongMucDauTu;
                    quanLyThuTucCNKQ4.CpXayDung = model4.CpXayDung;
                    quanLyThuTucCNKQ4.CpThietbi = model4.CpThietbi;
                    quanLyThuTucCNKQ4.CpBoithuong = model4.CpBoithuong;
                    quanLyThuTucCNKQ4.CpChung = model4.CpChung;
                    quanLyThuTucCNKQ4.CpDuphong = model4.CpDuphong;
                    quanLyThuTucCNKQ4.ThoigianThuchien = model4.ThoigianThuchien.Trim();
                    quanLyThuTucCNKQ4.SoBuocThietKe = model4.SoBuocThietKe.Trim();
                    quanLyThuTucCNKQ4.HinhThucQuanly = model4.HinhThucQuanly.Trim();

                    context.SaveChanges(); // Lưu tất cả thay đổi ở đây
                    trans.Commit();

                    message.Title = "Cập nhật kết quả hồ sơ dự án đầu tư thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        [Route("CapNhatKQHSDCDuAnDauTu")]
        [HttpPost]
        public ResponseMessage CapNhatKQHSDCDuAnDauTu(DCDuAnDauTuCnkqModel model5)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string id = model5.Id.ToString();
                    var quanLyThuTucCNKQ5 = context.QuanLyThuTucCnkqs.FirstOrDefault(cnkq => cnkq.Id == id);

                    if (quanLyThuTucCNKQ5 == null)
                    {
                        quanLyThuTucCNKQ5 = new QuanLyThuTucCnkq();
                        quanLyThuTucCNKQ5.Id = Guid.NewGuid().ToString();
                        context.QuanLyThuTucCnkqs.Add(quanLyThuTucCNKQ5);
                    }

                    // Gán các thuộc tính từ model5 vào quanLyThuTucCNKQ5
                    quanLyThuTucCNKQ5.IdThuTuc = model5.IdThuTuc.Trim();
                    quanLyThuTucCNKQ5.SoNgayQuyetDinhBiDc = model5.SoNgayQuyetDinhBiDc.Trim();
                    quanLyThuTucCNKQ5.SoNgayQuyetDinh = model5.SoNgayQuyetDinh.Trim();
                    quanLyThuTucCNKQ5.NamPheDuyet = model5.NamPheDuyet.Trim();
                    quanLyThuTucCNKQ5.ChuDauTu = model5.ChuDauTu.Trim();
                    quanLyThuTucCNKQ5.DuAnId = model5.DuAnId.Trim();
                    quanLyThuTucCNKQ5.TongMucDauTu = model5.TongMucDauTu;
                    quanLyThuTucCNKQ5.CpXayDung = model5.CpXayDung;
                    quanLyThuTucCNKQ5.CpThietbi = model5.CpThietbi;
                    quanLyThuTucCNKQ5.CpBoithuong = model5.CpBoithuong;
                    quanLyThuTucCNKQ5.CpChung = model5.CpChung;
                    quanLyThuTucCNKQ5.CpDuphong = model5.CpDuphong;
                    quanLyThuTucCNKQ5.CoCauNguonVonDc = model5.CoCauNguonVonDieuChinh.Trim();
                    quanLyThuTucCNKQ5.ThoigianThuchienDc = model5.ThoigianThuchienDc.Trim();
                    quanLyThuTucCNKQ5.GhiChu = model5.GhiChu.Trim();
                    context.SaveChanges(); // Lưu tất cả thay đổi ở đây
                    trans.Commit();

                    message.Title = "Cập nhật kết quả hồ sơ điều chỉnh dự án đầu tư thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        [Route("CapNhatKQHSDieuChinhDuToan")]
        [HttpPost]
        public ResponseMessage CapNhatKQHSDieuChinhDuToan(DtDCDaKoCoCpxdCnkqModel model9)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {

                    string id6 = model9.Id?.ToString();
                    var quanLyThuTucCNKQ6 = context.QuanLyThuTucCnkqs.FirstOrDefault(cnkq => cnkq.Id == id6);

                    if (quanLyThuTucCNKQ6 == null)
                    {
                        quanLyThuTucCNKQ6 = new QuanLyThuTucCnkq
                        {
                            Id = Guid.NewGuid().ToString()
                        };
                        context.QuanLyThuTucCnkqs.Add(quanLyThuTucCNKQ6);
                    }

                    quanLyThuTucCNKQ6.IdThuTuc = model9.IdThuTuc.Trim();
                    quanLyThuTucCNKQ6.SoNgayQuyetDinhBiDc = model9.SoNgayQuyetDinhBiDc.Trim();
                    quanLyThuTucCNKQ6.SoNgayQuyetDinh = model9.SoNgayQuyetDinh.Trim();
                    quanLyThuTucCNKQ6.NamPheDuyet = model9.NamPheDuyet.Trim();
                    quanLyThuTucCNKQ6.ChuDauTu = model9.ChuDauTu.Trim();
                    quanLyThuTucCNKQ6.DuAnId = model9.DuAnId.Trim();
                    quanLyThuTucCNKQ6.DutoanCpCbdautuDc = model9.DutoanCpCbdautuDieuChinh;
                    quanLyThuTucCNKQ6.CoCauNguonVonDc = model9.CoCauNguonVonDieuChinh.Trim();
                    quanLyThuTucCNKQ6.GhiChu = model9.GhiChu.Trim();

                    context.SaveChanges(); // Lưu tất cả thay đổi ở đây
                    trans.Commit();

                    message.Title = "Cập nhật kết quả hồ sơ điều chỉnh dự toán đầu tư thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        [Route("CapNhatKQHSLCNT")]
        [HttpPost]
        public ResponseMessage CapNhatKQHSLCNT(LCNTCnkqModel model10)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {

                    string id7 = model10.Id.ToString();
                    var quanLyThuTucCNKQ7 = context.QuanLyThuTucCnkqs.FirstOrDefault(cnkq => cnkq.Id == id7);
                    if (quanLyThuTucCNKQ7 == null)
                    {
                        quanLyThuTucCNKQ7 = new QuanLyThuTucCnkq
                        {
                            Id = Guid.NewGuid().ToString()
                        };
                        context.QuanLyThuTucCnkqs.Add(quanLyThuTucCNKQ7);
                    }
                    quanLyThuTucCNKQ7.IdThuTuc = model10.IdThuTuc.Trim();
                    quanLyThuTucCNKQ7.SoNgayQuyetDinh = model10.SoNgayQuyetDinh.Trim();
                    quanLyThuTucCNKQ7.NamPheDuyet = model10.NamPheDuyet.Trim();
                    quanLyThuTucCNKQ7.ChuDauTu = model10.ChuDauTu.Trim();
                    quanLyThuTucCNKQ7.NhomDuAn = model10.NhomDuAn;
                    quanLyThuTucCNKQ7.TenKeHoach = model10.TenKeHoach.Trim();
                    #region Xóa danh sách dự án kèm theo cũ
                    List<ThuTucNbLuaChonNhaThauPhanChiaGoiThau> lstCacGoiThau = context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Where(nguonVon => nguonVon.IdThuTuc == model10.IdThuTuc).ToList();

                    if (lstCacGoiThau != null && lstCacGoiThau.Count > 0)
                    {
                        context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.RemoveRange(lstCacGoiThau);
                    }

                    #endregion

                    #region Thêm mới danh sách dự án kèm theo mới
                    if (model10?.lstCacGoiThau != null && model10?.lstCacGoiThau.Count > 0)
                    {
                        foreach (PhanChiaDuAnThanhCacGoiThau thongTinNguonVon in model10.lstCacGoiThau)
                        {
                            ThuTucNbLuaChonNhaThauPhanChiaGoiThau objThuTuc_CacGoiThauThuTuc = new()
                            {
                                Id = Guid.NewGuid().ToString(),
                                IdThuTuc = model10.IdThuTuc.Trim(),
                                TenGoiThau = thongTinNguonVon?.TenGoiThau.Trim(),
                                GiaGoiThau = DoubleOrNull(thongTinNguonVon?.GiaGoiThau),
                                GiaTrungThau = DoubleOrNull(thongTinNguonVon?.GiaTrungThau),
                                NguonVon = thongTinNguonVon?.NguonVon,
                                LinhVuc = thongTinNguonVon?.LinhVuc,
                                HinhThucDauThau = thongTinNguonVon?.HinhThucDauThau,
                                HinhThucLuaChonNhaThau = thongTinNguonVon?.HinhThucLuaChonNhaThau,
                            };
                            context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Add(objThuTuc_CacGoiThauThuTuc);
                        }
                    }
                    #endregion
                    context.SaveChanges(); // Lưu tất cả thay đổi ở đây
                    trans.Commit();

                    message.Title = "Cập nhật kết quả hồ sơ kế hoạch lựa chọn nhà thầu thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        [Route("CapNhatKQHSLCNTDieuChinh")]
        [HttpPost]
        public ResponseMessage CapNhatKQHSLCNTDieuChinh(LCNTCnkqModel model10)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {

                    string id7 = model10.Id.ToString();
                    var quanLyThuTucCNKQ11 = context.QuanLyThuTucCnkqs.FirstOrDefault(cnkq => cnkq.Id == id7);
                    if (quanLyThuTucCNKQ11 == null)
                    {
                        quanLyThuTucCNKQ11 = new QuanLyThuTucCnkq
                        {
                            Id = Guid.NewGuid().ToString()
                        };
                        context.QuanLyThuTucCnkqs.Add(quanLyThuTucCNKQ11);
                    }
                    quanLyThuTucCNKQ11.IdThuTuc = model10.IdThuTuc.Trim();
                    quanLyThuTucCNKQ11.SoNgayQuyetDinhBiDc = model10.SoNgayQuyetDinhBiDc.Trim();
                    quanLyThuTucCNKQ11.SoNgayQuyetDinh = model10.SoNgayQuyetDinh.Trim();
                    quanLyThuTucCNKQ11.NamPheDuyet = model10.NamPheDuyet.Trim();
                    quanLyThuTucCNKQ11.ChuDauTu = model10.ChuDauTu.Trim();
                    quanLyThuTucCNKQ11.NhomDuAn = model10.NhomDuAn;
                    quanLyThuTucCNKQ11.TenKeHoach = model10.TenKeHoach.Trim();
                    #region Xóa danh sách dự án kèm theo cũ
                    List<ThuTucNbLuaChonNhaThauPhanChiaGoiThau> lstCacGoiThau = context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Where(nguonVon => nguonVon.IdThuTuc == model10.IdThuTuc).ToList();

                    if (lstCacGoiThau != null && lstCacGoiThau.Count > 0)
                    {
                        context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.RemoveRange(lstCacGoiThau);
                    }

                    #endregion

                    #region Thêm mới danh sách dự án kèm theo mới
                    if (model10?.lstCacGoiThau != null && model10?.lstCacGoiThau.Count > 0)
                    {
                        foreach (PhanChiaDuAnThanhCacGoiThau thongTinNguonVon in model10.lstCacGoiThau)
                        {
                            ThuTucNbLuaChonNhaThauPhanChiaGoiThau objThuTuc_CacGoiThauThuTuc = new()
                            {
                                Id = Guid.NewGuid().ToString(),
                                IdThuTuc = model10.IdThuTuc.Trim(),
                                TenGoiThau = thongTinNguonVon?.TenGoiThau.Trim(),
                                GiaGoiThau = DoubleOrNull(thongTinNguonVon?.GiaGoiThau),
                                GiaTrungThau = DoubleOrNull(thongTinNguonVon?.GiaTrungThau),
                                NguonVon = thongTinNguonVon?.NguonVon,
                                LinhVuc = thongTinNguonVon?.LinhVuc,
                                HinhThucDauThau = thongTinNguonVon?.HinhThucDauThau,
                                HinhThucLuaChonNhaThau = thongTinNguonVon?.HinhThucLuaChonNhaThau,
                            };
                            context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Add(objThuTuc_CacGoiThauThuTuc);
                        }
                    }
                    #endregion
                    context.SaveChanges(); // Lưu tất cả thay đổi ở đây
                    trans.Commit();

                    message.Title = "Cập nhật kết quả hồ sơ điều chỉnh kế hoạch lựa chọn nhà thầu thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

    }
}
