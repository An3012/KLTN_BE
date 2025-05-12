using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.QlHoSoNoiBo;
using DTC_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.TongQuan;
using DTC_BE.CodeBase;
using DTC_BE.Entities;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.LapBaoCaoTongHop;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.ThuTucThamDinhDTCBDT;
using static DTC_BE.CodeBase.Enums;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.ThuTucTDDADTCKhongCoCPXD;
using DTC_BE.Models.HeThong.NhatKyHeThong;
using NPOI.OpenXmlFormats.Spreadsheet;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml.Style;

namespace DTC_BE.Controllers.QuanlyThuTucNoiBoVeDuAnDauTuCong
{
    [Route("api/[controller]")]
    [ApiController]
    public class TongQuanController : BaseApiController
    {

        #region Tìm kiếm hồ sơ
        [Route("GetDanhSachHoSo")]
        [HttpPost]
        public ResponseMessage GetDanhSachHoSo(TimKiemTongQuanHoSo timKiemDanhSach)
        {
            try
            {
                var listCDT = context.DmChuDauTus;
                List<SelectListItem> nhomDuAn = new List<SelectListItem>();
                foreach (Enums.NhomDuAn loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.NhomDuAn)))
                {
                    nhomDuAn.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                var nguoiDung = context.HtNguoiDungs.FirstOrDefault(x => x.Id == timKiemDanhSach.idUser);
                var lstChuyenThuLy = context.QuanLyThuTucNoiBoChuyenThuLies;
                var quyen = context.HtNhomQuyens.FirstOrDefault(x => x.Id == nguoiDung.HtNhomQuyenId).Ten;
                List<ThongTinTongQuanTTTD> lstThuTuc = new List<ThongTinTongQuanTTTD>();
                var query = context.QuanLyThuTucNoiBoDuAnDtcs
                   .AsNoTracking()
                   .AsEnumerable()
                   .Where(s => s.IsXoa != (int)Enums.IsXoa.DaXoa);

                if (!string.IsNullOrEmpty(timKiemDanhSach.tenHoSo))
                {
                    query = query.Where(s => s.TenHoSo?.ToLower().Trim().Contains(timKiemDanhSach.tenHoSo.ToLower().Trim()) == true);
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.maHoSo))
                {
                    query = query.Where(s => s.MaHoSo?.ToLower().Trim().Contains(timKiemDanhSach.maHoSo.ToLower().Trim()) == true);
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.chuDauTu))
                {
                    query = query.Where(s => s.IdDonViThucHienDuAn != null && s.IdDonViThucHienDuAn.ToLower().Trim().Contains(timKiemDanhSach.chuDauTu.ToLower().Trim()));
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.nhomDuAn))
                {
                    if (int.TryParse(timKiemDanhSach.nhomDuAn, out int nhomDuAnInt))
                    {
                        query = query.Where(s => s.NhomDuAn == nhomDuAnInt);
                    }
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.loaiHoSo))
                {
                    if (int.TryParse(timKiemDanhSach.loaiHoSo, out int loaiHoSoInt))
                    {
                        query = query.Where(s => s.LoaiHoSo == loaiHoSoInt);
                    }
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.trangThaiLuuKho))
                {
                    if (int.TryParse(timKiemDanhSach.trangThaiLuuKho, out int trangThaiLuuKhoInt))
                    {
                        query = query.Where(s => s.LuuKho == trangThaiLuuKhoInt);
                    }
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.ngayNhanHoSo))
                {
                    var ngayNhan = DateTimeOrNull(timKiemDanhSach.ngayNhanHoSo);
                    if (ngayNhan.HasValue)
                    {
                        query = query.Where(s => s.NgayNhanHoSo == ngayNhan);
                    }
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.hanTraKetQua))
                {
                    var hanTra = DateTimeOrNull(timKiemDanhSach.hanTraKetQua);
                    if (hanTra.HasValue)
                    {
                        query = query.Where(s => s.DuKienHoanThanh == hanTra);
                    }
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.chuyenVienXuLy))
                {
                    query = query.Where(s => s.NguoiTao == timKiemDanhSach.chuyenVienXuLy);
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.chuyenVienThuLy))
                {
                    var tempList = new List<QuanLyThuTucNoiBoDuAnDtc>();

                    foreach (var s in query.ToList())
                    {
                        var chuyenThuLyGanNhat = lstChuyenThuLy
                            .Where(x => x.IdThuTuc == s.Id)
                            .OrderByDescending(x => x.NgayChuyenThuLy)
                            .FirstOrDefault();

                        if (chuyenThuLyGanNhat?.ChuyenVien == timKiemDanhSach.chuyenVienThuLy)
                        {
                            tempList.Add(s);
                        }
                    }

                    query = tempList;
                }

                if (!string.IsNullOrEmpty(timKiemDanhSach.trangThai))
                {
                    if (int.TryParse(timKiemDanhSach.trangThai, out int trangThaiInt))
                    {
                        var tempList = new List<QuanLyThuTucNoiBoDuAnDtc>();

                        foreach (var s in query.ToList())
                        {
                            var tienDoGanNhat = s.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies
                                .OrderByDescending(x => x.NgayGiaiQuyet)
                                .FirstOrDefault();

                            if (tienDoGanNhat?.TrangThai == trangThaiInt)
                            {
                                tempList.Add(s);
                            }
                        }

                        query = tempList;
                    }
                }

                if (quyen == "Chuyên viên thụ lý")
                {
                    var tempList = new List<QuanLyThuTucNoiBoDuAnDtc>();

                    foreach (var s in query.ToList())
                    {
                        var chuyenThuLyGanNhat = lstChuyenThuLy
                            .Where(x => x.IdThuTuc == s.Id)
                            .OrderByDescending(x => x.NgayChuyenThuLy)
                            .FirstOrDefault();

                        if (chuyenThuLyGanNhat?.ChuyenVien == timKiemDanhSach.idUser)
                        {
                            tempList.Add(s);
                        }
                    }

                    query = tempList;
                }

                lstThuTuc = query
                    .OrderByDescending(s => s.NgayTao)
                    .Skip(timKiemDanhSach.rowPerPage * (timKiemDanhSach.currentPage - 1))
                    .Take(timKiemDanhSach.rowPerPage)
                    .Select(s => new ThongTinTongQuanTTTD
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
                        ChuyenVienXuLy = GetTenChuyenVienXuLy(s.Id),
                    })
                    .ToList();
                if (!string.IsNullOrEmpty(timKiemDanhSach.thongKeHanXuLy))
                {
                    if (int.Parse(timKiemDanhSach.thongKeHanXuLy) == 0)
                    {
                        lstThuTuc = lstThuTuc.Where(x => x.ThongKeHanXuLy == "Quá hạn xử lý").ToList();
                    }
                    if (int.Parse(timKiemDanhSach.thongKeHanXuLy) == 1)
                    {
                        lstThuTuc = lstThuTuc.Where(x => x.ThongKeHanXuLy == "Trong hạn xử lý").ToList();
                    }
                }
                var totalRecords = lstThuTuc.Count();
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
        [Route("GetTenChuyenVienXuLy")]
        [NonAction]
        public string GetTenChuyenVienXuLy(string Id)
        {
            var listItemsChuyenVienThuLy = context.HtNguoiDungs;
            string TenChuyenVien = "";
            if (!string.IsNullOrEmpty(Id))
            {
                var x = context.QuanLyThuTucNoiBoDuAnDtcs
                               .Where(ct => ct.Id == Id)
                               .FirstOrDefault()?.NguoiTao ?? "";

                if (string.IsNullOrEmpty(x))
                {
                    TenChuyenVien = "";
                }
                else
                {
                    TenChuyenVien = listItemsChuyenVienThuLy
                        .FirstOrDefault(cv => cv.Id == x)?.HoTen ?? "";
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
        [Route("GetTenChuDauTu")]
        [NonAction]
        public string GetTenChuDauTu(string Id)
        {
            string TenChuDauTu = "";
            if (!string.IsNullOrEmpty(Id))
            {
                TenChuDauTu = context.QuanLyThuTucNoiBoDuAnDtcs.Where(s => s.Id == Id).Select(s => s.IdDonViThucHienDuAn).FirstOrDefault();
            }
            else
            {
                TenChuDauTu = "";
            }
            return TenChuDauTu;
        }

        [Route("GetTenHoSo")]
        [NonAction]
        public string GetTenHoSo(string Id)
        {
            string TenHoSo = "";
            if (!string.IsNullOrEmpty(Id))
            {
                TenHoSo = context.QuanLyThuTucNoiBoDuAnDtcs.Where(s => s.Id == Id).Select(s => s.TenHoSo).FirstOrDefault();
            }
            else
            {
                TenHoSo = "";
            }
            return TenHoSo;
        }

        [Route("GetLoaiHoSo")]
        [NonAction]
        public string GetLoaiHoSo(int? id)
        {
            List<SelectListItem> lstLoaiThuTuc = new List<SelectListItem>();

            foreach (Enums.LoaiThuTuNoiBoDuAnDTC loaithutucnoibo in (Enums.LoaiThuTuNoiBoDuAnDTC[])Enum.GetValues(typeof(Enums.LoaiThuTuNoiBoDuAnDTC)))
            {
                lstLoaiThuTuc.Add(new SelectListItem { Text = loaithutucnoibo.GetDescription(), Value = loaithutucnoibo.GetHashCode().ToString() });
            }
            string loaiHS = "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                loaiHS = lstLoaiThuTuc.Where(s => s.Value == id.ToString()).Select(s => s.Text).FirstOrDefault();
            }
            else
            {
                loaiHS = "";
            }
            return loaiHS;
        }


        [Route("GetTrangThaiLoaiHoSo")]
        [HttpGet]
        public ResponseMessage GetTrangThaiLoaiHoSo()
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
        #endregion

        #region xuất exel
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TongQuanController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("ExportExcel")]
        [HttpPost]
        public IActionResult ExportExcel(TimKiemTongQuanHoSo timKiemDanhSach)
        {
            var listCDT = context.DmChuDauTus;
            List<SelectListItem> nhomDuAn = new List<SelectListItem>();
            foreach (Enums.NhomDuAn loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.NhomDuAn)))
            {
                nhomDuAn.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
            }
            var nguoiDung = context.HtNguoiDungs.FirstOrDefault(x => x.Id == timKiemDanhSach.idUser);
            var lstChuyenThuLy = context.QuanLyThuTucNoiBoChuyenThuLies;
            var quyen = context.HtNhomQuyens.FirstOrDefault(x => x.Id == nguoiDung.HtNhomQuyenId).Ten;
            List<XuatExxelTongQuan> lstThuTuc = new List<XuatExxelTongQuan>();
            var query = context.QuanLyThuTucNoiBoDuAnDtcs
               .AsNoTracking()
               .AsEnumerable()
               .Where(s => s.IsXoa != (int)Enums.IsXoa.DaXoa);

            if (!string.IsNullOrEmpty(timKiemDanhSach.tenHoSo))
            {
                query = query.Where(s => s.TenHoSo?.ToLower().Trim().Contains(timKiemDanhSach.tenHoSo.ToLower().Trim()) == true);
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.maHoSo))
            {
                query = query.Where(s => s.MaHoSo?.ToLower().Trim().Contains(timKiemDanhSach.maHoSo.ToLower().Trim()) == true);
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.chuDauTu))
            {
                query = query.Where(s => s.IdDonViThucHienDuAn != null && s.IdDonViThucHienDuAn.ToLower().Trim().Contains(timKiemDanhSach.chuDauTu.ToLower().Trim()));
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.nhomDuAn))
            {
                if (int.TryParse(timKiemDanhSach.nhomDuAn, out int nhomDuAnInt))
                {
                    query = query.Where(s => s.NhomDuAn == nhomDuAnInt);
                }
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.loaiHoSo))
            {
                if (int.TryParse(timKiemDanhSach.loaiHoSo, out int loaiHoSoInt))
                {
                    query = query.Where(s => s.LoaiHoSo == loaiHoSoInt);
                }
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.trangThaiLuuKho))
            {
                if (int.TryParse(timKiemDanhSach.trangThaiLuuKho, out int trangThaiLuuKhoInt))
                {
                    query = query.Where(s => s.LuuKho == trangThaiLuuKhoInt);
                }
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.ngayNhanHoSo))
            {
                var ngayNhan = DateTimeOrNull(timKiemDanhSach.ngayNhanHoSo);
                if (ngayNhan.HasValue)
                {
                    query = query.Where(s => s.NgayNhanHoSo == ngayNhan);
                }
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.hanTraKetQua))
            {
                var hanTra = DateTimeOrNull(timKiemDanhSach.hanTraKetQua);
                if (hanTra.HasValue)
                {
                    query = query.Where(s => s.DuKienHoanThanh == hanTra);
                }
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.chuyenVienXuLy))
            {
                query = query.Where(s => s.NguoiTao == timKiemDanhSach.chuyenVienXuLy);
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.chuyenVienThuLy))
            {
                var tempList = new List<QuanLyThuTucNoiBoDuAnDtc>();

                foreach (var s in query.ToList())
                {
                    var chuyenThuLyGanNhat = lstChuyenThuLy
                        .Where(x => x.IdThuTuc == s.Id)
                        .OrderByDescending(x => x.NgayChuyenThuLy)
                        .FirstOrDefault();

                    if (chuyenThuLyGanNhat?.ChuyenVien == timKiemDanhSach.chuyenVienThuLy)
                    {
                        tempList.Add(s);
                    }
                }

                query = tempList;
            }

            if (!string.IsNullOrEmpty(timKiemDanhSach.trangThai))
            {
                if (int.TryParse(timKiemDanhSach.trangThai, out int trangThaiInt))
                {
                    var tempList = new List<QuanLyThuTucNoiBoDuAnDtc>();

                    foreach (var s in query.ToList())
                    {
                        var tienDoGanNhat = s.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies
                            .OrderByDescending(x => x.NgayGiaiQuyet)
                            .FirstOrDefault();

                        if (tienDoGanNhat?.TrangThai == trangThaiInt)
                        {
                            tempList.Add(s);
                        }
                    }

                    query = tempList;
                }
            }

            if (quyen == "Chuyên viên thụ lý")
            {
                var tempList = new List<QuanLyThuTucNoiBoDuAnDtc>();

                foreach (var s in query.ToList())
                {
                    var chuyenThuLyGanNhat = lstChuyenThuLy
                        .Where(x => x.IdThuTuc == s.Id)
                        .OrderByDescending(x => x.NgayChuyenThuLy)
                        .FirstOrDefault();

                    if (chuyenThuLyGanNhat?.ChuyenVien == timKiemDanhSach.idUser)
                    {
                        tempList.Add(s);
                    }
                }

                query = tempList;
            }
            var Listkqth = context.QuanLyThuTucNoiBoDuAnDtcKqths;
            var ListTienDoXuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies;
            List<SelectListItem> LstLoaihoSo = new List<SelectListItem>();
            foreach (Enums.QUY_TRINH_XU_LY loaiTrangThai in (Enums.QUY_TRINH_XU_LY[])Enum.GetValues(typeof(Enums.QUY_TRINH_XU_LY)))
            {
                 LstLoaihoSo.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
            }
            lstThuTuc = query
                .OrderByDescending(s => s.NgayTao)
                .Skip(timKiemDanhSach.rowPerPage * (timKiemDanhSach.currentPage - 1))
                .Take(timKiemDanhSach.rowPerPage)
                .Select(s => new XuatExxelTongQuan
                {
                    Id = s.Id,
                    TenHoSo = s.TenHoSo,
                    MaHoSo = s.MaHoSo,
                    NhomDuAn = nhomDuAn.FirstOrDefault(x => x.Value == s.NhomDuAn.ToString())?.Text ?? "",
                    ChuDauTu = listCDT.FirstOrDefault(x => x.Id == s.IdDonViThucHienDuAn)?.TenChuDauTu ?? "",
                    NgayNhanHoSo = s.NgayNhanHoSo?.ToString("dd/MM/yyyy") ?? "",
                    HanGiaiQuyetHoSo = s.DuKienHoanThanh?.ToString("dd/MM/yyyy") ?? "",
                    ThongKeHanXuLy = GetHanXuLy(s.Id, s.DuKienHoanThanh),
                    TrangThaiLuuKho = s.LuuKho == 1? "Đã lưu kho" : "Chưa lưu kho",
                    TinhTrangHoSo = GetTinhTrangThucHien(s.Id),
                    ChuyenVienXuLy = GetTenChuyenVienXuLy(s.Id),
                    KquaThucHien1 = Listkqth.FirstOrDefault(x => x.IdThuTuc == s.Id)?.SoNgayVb1,
                    KquaThucHien2 = Listkqth.FirstOrDefault(x => x.IdThuTuc == s.Id)?.SoNgayVb2,
                    NgayKy1 = Listkqth.FirstOrDefault(x => x.IdThuTuc == s.Id)?.NgayKy1?.ToString("dd/MM/yyyy") ?? "",
                    NgayKy2 = Listkqth.FirstOrDefault(x => x.IdThuTuc == s.Id)?.NgayKy2?.ToString("dd/MM/yyyy") ?? "",
                    VanBanChuDauTu = s.VanBanChuDauTu,
                    GhiChu =s.CacThongTinKhac,
                    GhiChuTinhTrang = ListTienDoXuLy.Where(x => x.IdThuTuc == s.Id).OrderByDescending(x=> x.NgayGiaiQuyet).First().GhiChuTinhTrang,
                    QuyTrinhXuLy = LstLoaihoSo.FirstOrDefault(x => x.Value == s.LoaiHoSo.ToString()).Text,
                })
                .ToList();
            if (!string.IsNullOrEmpty(timKiemDanhSach.thongKeHanXuLy))
            {
                if (int.Parse(timKiemDanhSach.thongKeHanXuLy) == 0)
                {
                    lstThuTuc = lstThuTuc.Where(x => x.ThongKeHanXuLy == "Quá hạn xử lý").ToList();
                }
                if (int.Parse(timKiemDanhSach.thongKeHanXuLy) == 1)
                {
                    lstThuTuc = lstThuTuc.Where(x => x.ThongKeHanXuLy == "Trong hạn xử lý").ToList();
                }
            }

            string templatePath = _webHostEnvironment.ContentRootPath + @"/XuatExel/TongQuan.xlsx";
            string filenameOutput = "NBXuatHS" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            FileInfo fileInfo = new FileInfo(templatePath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.Name = "DanhSachHoSo";

                int startRow = 4;
                int stt = 1;

                foreach (var item in lstThuTuc)
                {
                    ExcelFunctions.SetCommonStyles(worksheet.Cells[$"A{startRow}:R{startRow}"], 45, "Times New Roman", 9, false, false, true);
                    worksheet.Cells[startRow, 1].Value = stt;
                    worksheet.Cells[startRow, 2].Value = item?.MaHoSo;
                    worksheet.Cells[startRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 3].Value = item?.TenHoSo;
                    worksheet.Cells[startRow, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 4].Value = item.NgayNhanHoSo;
                    worksheet.Cells[startRow, 5].Value = item.HanGiaiQuyetHoSo;
                    worksheet.Cells[startRow, 6].Value = item.ThongKeHanXuLy;
                    worksheet.Cells[startRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 7].Value = item.TinhTrangHoSo;
                    worksheet.Cells[startRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 8].Value = item.KquaThucHien1;
                    worksheet.Cells[startRow, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 9].Value = item.NgayKy1;
                    worksheet.Cells[startRow, 10].Value = item.KquaThucHien2;
                    worksheet.Cells[startRow, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 11].Value = item.NgayKy2;
                    worksheet.Cells[startRow, 12].Value = item.ChuyenVienXuLy;
                    worksheet.Cells[startRow, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 13].Value = item.QuyTrinhXuLy;
                    worksheet.Cells[startRow, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 14].Value = item.NhomDuAn;
                    worksheet.Cells[startRow, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 15].Value = item.ChuDauTu;
                    worksheet.Cells[startRow, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 16].Value = item.VanBanChuDauTu;
                    worksheet.Cells[startRow, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 17].Value = item.GhiChu;
                    worksheet.Cells[startRow, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 18].Value = item.GhiChuTinhTrang;
                    worksheet.Cells[startRow, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    startRow++;
                    stt++;
                }

                // Xuất file ra MemoryStream
                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    stream.Position = 0;
                    byte[] excelBytes = package.GetAsByteArray();
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Excel.xlsx");
                }
            }
        }

        #endregion
    }
}
