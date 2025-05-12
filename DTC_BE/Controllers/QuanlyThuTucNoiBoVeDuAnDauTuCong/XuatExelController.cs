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
using NPOI.SS.Formula.Functions;
using DTC_BE.Models.ThuTucNBKeHoachLuaChonNhaThau.BaoCaoTongHopGoiThauDuocDuyet;

namespace DTC_BE.Controllers.QuanlyThuTucNoiBoVeDuAnDauTuCong
{
    [Route("api/[controller]")]
    [ApiController]
    public class XuatExelController : BaseApiController
    {
        #region xuất exel
        private readonly IWebHostEnvironment _webHostEnvironment;
        public XuatExelController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("ExportExcelBieuTongHop")]
        [HttpPost]
        public IActionResult ExportExcelBieuTongHop()
        {
            var lstQuanLyThuTucNoiBo = context.QuanLyThuTucNoiBoDuAnDtcs;
            var lstMoiNhatTheoThuTuc = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.GroupBy(x => x.IdThuTuc).Select(g => g.OrderByDescending(x => x.NgayGiaiQuyet).First()).ToList();

            List<SelectListItem> lstLoaiThuTuc = new List<SelectListItem>();
            foreach (Enums.LoaiThuTuNoiBoDuAnDTC loaithutucnoibo in (Enums.LoaiThuTuNoiBoDuAnDTC[])Enum.GetValues(typeof(Enums.LoaiThuTuNoiBoDuAnDTC)))
            {
                lstLoaiThuTuc.Add(new SelectListItem { Text = loaithutucnoibo.GetDescription(), Value = loaithutucnoibo.GetHashCode().ToString() });
            }
            List<SelectListItem> listThongKeThuTucTheoLoai = new List<SelectListItem>();
            List<SelectListItem> listThongKeThuTucTheoTinhTrang = new List<SelectListItem>();

            foreach (var item in lstLoaiThuTuc)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = lstQuanLyThuTucNoiBo.Where(x => x.LoaiHoSo.ToString() == item.Value).Count().ToString()
                };
                listThongKeThuTucTheoLoai.Add(selectListItem);
            }
            List<SelectListItem> tinhtTrangHoSo = new List<SelectListItem>();


            foreach (Enums.TINH_TRANG_HO_SO loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.TINH_TRANG_HO_SO)))
            {
                tinhtTrangHoSo.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
            }

            foreach (var item in tinhtTrangHoSo)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = lstMoiNhatTheoThuTuc.Where(x => x.TrangThai.ToString() == item.Value).GroupBy(y => y.IdThuTuc).Count().ToString()
                };
                listThongKeThuTucTheoTinhTrang.Add(selectListItem);
            }
            var fileName = "BieuTongHopThuTuc" + DateTime.Now.ToString("dd_MM_yyyy_HHmm") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            MemoryStream stream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(stream))
            {
                excelPackage.Workbook.Properties.Author = "LapPhieuThuTuc";
                excelPackage.Workbook.Properties.Title = $"Biểu tổng hợp thủ tục";
                var worksheet = excelPackage.Workbook.Worksheets.Add("BieuTongHopThuTuc");

                worksheet.Cells.Style.WrapText = true;
                worksheet.DefaultRowHeight = 15;
                worksheet.Column(3).Width = 5;
                worksheet.Column(4).Width = 55;
                worksheet.Column(5).Width = 15;

                // ----- BẢNG 1: THEO LOẠI -----
                ExcelFunctions.SetCommonStyles(worksheet.Cells["C3:E4"], 30, "Times New Roman", 12, true, false, true);
                ExcelFunctions.MergeAndSetValue(worksheet, "C3:E3", $"TỔNG HỢP HỒ SƠ THEO LOẠI");
                worksheet.Cells["C4"].Value = "STT";
                worksheet.Cells["D4"].Value = "Phân loại hồ sơ";
                worksheet.Cells["E4"].Value = "Số lượng";

                int rowStartLoai = 5;
                for (int i = 0; i < listThongKeThuTucTheoLoai.Count; i++)
                {
                    worksheet.Cells[rowStartLoai + i, 3].Value = i + 1;
                    worksheet.Cells[rowStartLoai + i, 4].Value = listThongKeThuTucTheoLoai[i].Text;
                    worksheet.Cells[rowStartLoai + i, 5].Value = listThongKeThuTucTheoLoai[i].Value;
                }
                int rowEndLoai = rowStartLoai + listThongKeThuTucTheoLoai.Count - 1;

                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"C{rowStartLoai}:E{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"D{rowStartLoai}:D{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true, ExcelHorizontalAlignment.Left);

                // ----- BẢNG 2: THEO TÌNH TRẠNG -----
                int rowStartTinhTrang = rowEndLoai + 3; // cách bảng trên 2 dòng

                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"C{rowStartTinhTrang}:E{rowStartTinhTrang + 1}"], 30, "Times New Roman", 12, true, false, true);
                ExcelFunctions.MergeAndSetValue(worksheet, $"C{rowStartTinhTrang}:E{rowStartTinhTrang}", $"TỔNG HỢP HỒ SƠ THEO TÌNH TRẠNG");
                worksheet.Cells[$"C{rowStartTinhTrang + 1}"].Value = "STT";
                worksheet.Cells[$"D{rowStartTinhTrang + 1}"].Value = "Tình trạng hồ sơ";
                worksheet.Cells[$"E{rowStartTinhTrang + 1}"].Value = "Số lượng";

                int dataStartTinhTrang = rowStartTinhTrang + 2;
                for (int i = 0; i < listThongKeThuTucTheoTinhTrang.Count; i++)
                {
                    worksheet.Cells[dataStartTinhTrang + i, 3].Value = i + 1;
                    worksheet.Cells[dataStartTinhTrang + i, 4].Value = listThongKeThuTucTheoTinhTrang[i].Text;
                    worksheet.Cells[dataStartTinhTrang + i, 5].Value = listThongKeThuTucTheoTinhTrang[i].Value;
                }
                int rowEndTinhTrang = dataStartTinhTrang + listThongKeThuTucTheoTinhTrang.Count - 1;

                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"C{dataStartTinhTrang}:E{rowEndTinhTrang}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"D{dataStartTinhTrang}:D{rowEndTinhTrang}"], 30, "Times New Roman", 12, false, false, true, ExcelHorizontalAlignment.Left);

                stream.Position = 0;
                byte[] excelBytes = excelPackage.GetAsByteArray();

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [Route("ExportExelDanhMucChiTiet")]
        [HttpPost]
        public IActionResult ExportExelDanhMucChiTiet(TimKiemThongKeTheoLoai timKiemBaoCao)
        {
            List<SelectListItem> nhomDuAn = new List<SelectListItem>();
            foreach (Enums.NhomDuAn loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.NhomDuAn)))
            {
                nhomDuAn.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
            }
            var listCDT = context.DmChuDauTus;
            var lstNguoiDung = context.HtNguoiDungs;
            List<SelectListItem> lstLoaiHoSo = new List<SelectListItem>();

            foreach (Enums.LoaiThuTuNoiBoDuAnDTC loaiTrangThai in (Enums.LoaiThuTuNoiBoDuAnDTC[])Enum.GetValues(typeof(Enums.LoaiThuTuNoiBoDuAnDTC)))
            {
                lstLoaiHoSo.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
            }
            var ListTienDoXuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies;
            var Listkqth = context.QuanLyThuTucNoiBoDuAnDtcKqths;
            var listThongKeThuTucTheoLoai = context.QuanLyThuTucNoiBoDuAnDtcs.AsNoTracking().AsEnumerable().Where(s => timKiemBaoCao.loai == 0 || s.LoaiHoSo == timKiemBaoCao.loai)
                                                                                                  .OrderByDescending(s => s.NgayTao)
                                                                                                  .Skip(timKiemBaoCao.rowPerPage * (timKiemBaoCao.currentPage - 1))
                                                                                                  .Take(timKiemBaoCao.rowPerPage).Select(s => new XuatExxelTongQuan
                                                                                                  {
                                                                                                      Id = s.Id,
                                                                                                      TenHoSo = s.TenHoSo,
                                                                                                      MaHoSo = s.MaHoSo,
                                                                                                      NhomDuAn = nhomDuAn.FirstOrDefault(x => x.Value == s.NhomDuAn.ToString())?.Text ?? "",
                                                                                                      ChuDauTu = listCDT.FirstOrDefault(x => x.Id == s.IdDonViThucHienDuAn)?.TenChuDauTu ?? "",
                                                                                                      NgayNhanHoSo = s.NgayNhanHoSo?.ToString("dd/MM/yyyy") ?? "",
                                                                                                      HanGiaiQuyetHoSo = s.DuKienHoanThanh?.ToString("dd/MM/yyyy") ?? "",
                                                                                                      ThongKeHanXuLy = GetHanXuLy(s.Id, s.DuKienHoanThanh),
                                                                                                      TrangThaiLuuKho = s.LuuKho == 1 ? "Đã lưu kho" : "Chưa lưu kho",
                                                                                                      TinhTrangHoSo = GetTinhTrangThucHien(s.Id),
                                                                                                      ChuyenVienXuLy = lstNguoiDung.FirstOrDefault(x => x.Id == s.NguoiTao).HoTen,
                                                                                                      KquaThucHien1 = Listkqth.FirstOrDefault(x => x.IdThuTuc == s.Id)?.SoNgayVb1,
                                                                                                      KquaThucHien2 = Listkqth.FirstOrDefault(x => x.IdThuTuc == s.Id)?.SoNgayVb2,
                                                                                                      NgayKy1 = Listkqth.FirstOrDefault(x => x.IdThuTuc == s.Id)?.NgayKy1?.ToString("dd/MM/yyyy") ?? "",
                                                                                                      NgayKy2 = Listkqth.FirstOrDefault(x => x.IdThuTuc == s.Id)?.NgayKy2?.ToString("dd/MM/yyyy") ?? "",
                                                                                                      VanBanChuDauTu = s.VanBanChuDauTu,
                                                                                                      GhiChu = s.CacThongTinKhac,
                                                                                                      GhiChuTinhTrang = ListTienDoXuLy.Where(x => x.IdThuTuc == s.Id).OrderByDescending(x => x.NgayGiaiQuyet).First().GhiChuTinhTrang,
                                                                                                      QuyTrinhXuLy = lstLoaiHoSo.FirstOrDefault(x => x.Value == s.LoaiHoSo.ToString()).Text,
                                                                                                  }).ToList();


            string templatePath = _webHostEnvironment.ContentRootPath + @"/XuatExel/DanhMucHoSoChiTiet.xlsx";
            string filenameOutput = "NBXuatHS" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            FileInfo fileInfo = new FileInfo(templatePath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.Name = "DanhSachHoSo";

                int startRow = 4;
                int stt = 1;

                foreach (var item in listThongKeThuTucTheoLoai)
                {
                    ExcelFunctions.SetCommonStyles(worksheet.Cells[$"A{startRow}:R{startRow}"], 45, "Times New Roman", 9, false, false, true);
                    worksheet.Cells[startRow, 1].Value = stt;
                    worksheet.Cells[startRow, 2].Value = item.QuyTrinhXuLy;
                    worksheet.Cells[startRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 3].Value = item?.MaHoSo;
                    worksheet.Cells[startRow, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 4].Value = item?.TenHoSo;
                    worksheet.Cells[startRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 5].Value = item.NgayNhanHoSo;
                    worksheet.Cells[startRow, 6].Value = item.HanGiaiQuyetHoSo;
                    worksheet.Cells[startRow, 7].Value = item.ThongKeHanXuLy;
                    worksheet.Cells[startRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 8].Value = item.TinhTrangHoSo;
                    worksheet.Cells[startRow, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 9].Value = item.KquaThucHien1;
                    worksheet.Cells[startRow, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 10].Value = item.NgayKy1;
                    worksheet.Cells[startRow, 11].Value = item.KquaThucHien2;
                    worksheet.Cells[startRow, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[startRow, 12].Value = item.NgayKy2;
                    worksheet.Cells[startRow, 13].Value = item.ChuyenVienXuLy;
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

        [Route("ExportExelBaoCaoChiTiet/{id}")]
        [HttpPost]
        public IActionResult ExportExelBaoCaoChiTiet(string id)
        {
            var htNguoiDung = context.HtNguoiDungs;
            var baoCaoTongHop = context.QuanLyThuTucNoiBoBaoCaoTongHops.Where(baoCao => baoCao.Id == id)
                                                                       .ToList() // chuyển từ LINQ to Entities sang LINQ to Objects
                                                                       .Select(s => new LapBaoCaoTongHop
                                                                       {
                                                                           id = s.Id,
                                                                           Ten = s.Ten,
                                                                           Nam = s.Nam,
                                                                           NgayTao = s.NgayTao != null ? s.NgayTao.Value.ToString("dd/MM/yyyy") : "",
                                                                           NguoiTao = htNguoiDung.FirstOrDefault(x => x.Id == s.NguoiTao)?.HoTen ?? "",
                                                                       })
                                                                       .FirstOrDefault();


            List<SelectListItem> lstLoaiHoSo = new List<SelectListItem>();

            foreach (Enums.LoaiThuTuNoiBoDuAnDTC loaithutucnoibo in (Enums.LoaiThuTuNoiBoDuAnDTC[])Enum.GetValues(typeof(Enums.LoaiThuTuNoiBoDuAnDTC)))
            {
                lstLoaiHoSo.Add(new SelectListItem { Text = loaithutucnoibo.GetDescription(), Value = loaithutucnoibo.GetHashCode().ToString() });
            }

            var lstDuLieuBangBC = context.QuanLyThuTucNoiBoBaoCaoTongHopChiTiets.Where(x => x.IdBaoCao == id)
                                                                                .Select(x => new ListLapBaoCaoTongHopChiTiet
                                                                                {
                                                                                    loaiHoSo = x.LoaiHoSo.ToString(),
                                                                                    tongSLHSDaTiepNhan = x.TongSoHsTiepNhan,
                                                                                    tongSoLuongHSDaGiaiQuyet = x.TongSoHsDaGiaiQuyet,
                                                                                    soLuongHSDaGiaiQuyetTruocHan = x.SlHsDaGqTruocHan,
                                                                                    soLuongHSDaGiaiQuyetDungHan = x.SlHsDaGqDungHan,
                                                                                    soLuongHSDaGiaiQuyetQuaHan = x.SlHsDaGqQuaHan,
                                                                                    tongSLHSDangGiaiQuyet = x.TongSoHsDangGiaiQuyet,
                                                                                    soLuongHSDangGQTrongHan = x.SlHsDangGqTrongHan,
                                                                                    soLuongHSDangGQQuaHan = x.SlHsDangGqQuaHan,
                                                                                    tongSLHSHoanThanh = x.TongSoHsDaHoanThanh ?? 0,
                                                                                    slHsDaLuuKho = x.SlHsLuuKho ?? 0,
                                                                                    slHsChuaLuuKho = x.SlHsChuaLuuKho ?? 0,
                                                                                    ghiChu = x.GhiChu,
                                                                                }).OrderBy(x => x.loaiHoSo).ToList();

            var lstTongCong = new
            {
                tongSo3 = lstDuLieuBangBC.Sum(x => x.tongSLHSDaTiepNhan),
                tongSo4 = lstDuLieuBangBC.Sum(x => x.tongSoLuongHSDaGiaiQuyet),
                tongSo5 = lstDuLieuBangBC.Sum(x => x.soLuongHSDaGiaiQuyetTruocHan),
                tongSo6 = lstDuLieuBangBC.Sum(x => x.soLuongHSDaGiaiQuyetDungHan),
                tongSo7 = lstDuLieuBangBC.Sum(x => x.soLuongHSDaGiaiQuyetQuaHan),
                tongSo8 = lstDuLieuBangBC.Sum(x => x.tongSLHSDangGiaiQuyet),
                tongSo9 = lstDuLieuBangBC.Sum(x => x.soLuongHSDangGQTrongHan),
                tongSo10 = lstDuLieuBangBC.Sum(x => x.soLuongHSDangGQQuaHan),
                tongSo11 = lstDuLieuBangBC.Sum(x => x.tongSLHSHoanThanh),
                tongSo12 = lstDuLieuBangBC.Sum(x => x.slHsDaLuuKho),
                tongSo13 = lstDuLieuBangBC.Sum(x => x.slHsChuaLuuKho),
            };
            var fileName = "BieuTongHopThuTuc" + DateTime.Now.ToString("dd_MM_yyyy_HHmm") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            MemoryStream stream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(stream))
            {
                excelPackage.Workbook.Properties.Author = "LapPhieuThuTuc";
                excelPackage.Workbook.Properties.Title = $"Biểu tổng hợp thủ tục";
                var worksheet = excelPackage.Workbook.Worksheets.Add("BieuTongHopThuTuc");
                worksheet.Cells.Style.WrapText = true;
                worksheet.DefaultRowHeight = 15;

                // Set column widths (B to N)
                worksheet.Column(2).Width = 5;   // STT
                worksheet.Column(3).Width = 37;  // LOẠI HỒ SƠ
                for (int col = 4; col <= 14; col++) // D to N
                {
                    worksheet.Column(col).Width = 15;
                }
                worksheet.Column(15).Width = 50; // Ghi chú (O)

                // Style for headers
                ExcelFunctions.SetCommonStyles(worksheet.Cells["B2:O5"], 30, "Times New Roman", 12, true, false, true);

                // Merge main title
                ExcelFunctions.MergeAndSetValue(worksheet, "B2:O3", $"{baoCaoTongHop.Ten}");
                int rowStartHeader = 4;
                // Merge & set headers (rowStartHeader = 4, based on your image)
                worksheet.Cells[$"B{rowStartHeader}:B{rowStartHeader + 1}"].Merge = true;
                worksheet.Cells[$"C{rowStartHeader}:C{rowStartHeader + 1}"].Merge = true;
                worksheet.Cells[$"D{rowStartHeader}:D{rowStartHeader + 1}"].Merge = true;

                worksheet.Cells[$"E{rowStartHeader}:H{rowStartHeader}"].Merge = true;
                worksheet.Cells[$"I{rowStartHeader}:K{rowStartHeader}"].Merge = true;
                worksheet.Cells[$"L{rowStartHeader}:N{rowStartHeader}"].Merge = true;

                worksheet.Cells[$"O{rowStartHeader}:O{rowStartHeader + 1}"].Merge = true;

                // Header titles
                worksheet.Cells[$"B{rowStartHeader}"].Value = "STT";
                worksheet.Cells[$"C{rowStartHeader}"].Value = "LOẠI HỒ SƠ";
                worksheet.Cells[$"D{rowStartHeader}"].Value = "Số lượng hồ sơ mới";
                worksheet.Cells[$"E{rowStartHeader}"].Value = "Số lượng hồ sơ đã giải quyết";
                worksheet.Cells[$"I{rowStartHeader}"].Value = "Số lượng hồ sơ đang giải quyết";
                worksheet.Cells[$"L{rowStartHeader}"].Value = "Số lượng hồ sơ hoàn thành";
                worksheet.Cells[$"O{rowStartHeader}"].Value = "Ghi chú";

                // Sub-headers (rowStartHeader + 1)
                worksheet.Cells[$"E{rowStartHeader + 1}"].Value = "Tổng số";
                worksheet.Cells[$"F{rowStartHeader + 1}"].Value = "Trước hạn";
                worksheet.Cells[$"G{rowStartHeader + 1}"].Value = "Đúng hạn";
                worksheet.Cells[$"H{rowStartHeader + 1}"].Value = "Quá hạn";
                worksheet.Cells[$"I{rowStartHeader + 1}"].Value = "Tổng số";
                worksheet.Cells[$"J{rowStartHeader + 1}"].Value = "Trong hạn";
                worksheet.Cells[$"K{rowStartHeader + 1}"].Value = "Quá hạn";
                worksheet.Cells[$"L{rowStartHeader + 1}"].Value = "Tổng số";
                worksheet.Cells[$"M{rowStartHeader + 1}"].Value = "Đã lưu kho";
                worksheet.Cells[$"N{rowStartHeader + 1}"].Value = "Chưa lưu kho";

                // Số thứ tự cột (1 đến 15) ở hàng rowStartHeader + 2
                for (int i = 1; i < 15; i++)
                {
                    var cell = worksheet.Cells[rowStartHeader + 2, 1 + i];
                    cell.Value = i;
                    var border = cell.Style.Border;
                    border.Top.Style = ExcelBorderStyle.Thin;
                    border.Bottom.Style = ExcelBorderStyle.Thin;
                    border.Left.Style = ExcelBorderStyle.Thin;
                    border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                int rowStartLoai = 7;
                for (int i = 0; i < lstDuLieuBangBC.Count; i++)
                {
                    worksheet.Cells[rowStartLoai + i, 2].Value = i + 1;
                    worksheet.Cells[rowStartLoai + i, 3].Value = lstLoaiHoSo.Where(x => x.Value == lstDuLieuBangBC[i].loaiHoSo).FirstOrDefault().Text;
                    worksheet.Cells[rowStartLoai + i, 4].Value = lstDuLieuBangBC[i].tongSLHSDaTiepNhan;
                    worksheet.Cells[rowStartLoai + i, 5].Value = lstDuLieuBangBC[i].tongSoLuongHSDaGiaiQuyet;
                    worksheet.Cells[rowStartLoai + i, 6].Value = lstDuLieuBangBC[i].soLuongHSDaGiaiQuyetTruocHan;
                    worksheet.Cells[rowStartLoai + i, 7].Value = lstDuLieuBangBC[i].soLuongHSDaGiaiQuyetDungHan;
                    worksheet.Cells[rowStartLoai + i, 8].Value = lstDuLieuBangBC[i].soLuongHSDaGiaiQuyetQuaHan;
                    worksheet.Cells[rowStartLoai + i, 9].Value = lstDuLieuBangBC[i].tongSLHSDangGiaiQuyet;
                    worksheet.Cells[rowStartLoai + i, 10].Value = lstDuLieuBangBC[i].soLuongHSDangGQTrongHan;
                    worksheet.Cells[rowStartLoai + i, 11].Value = lstDuLieuBangBC[i].soLuongHSDangGQQuaHan;
                    worksheet.Cells[rowStartLoai + i, 12].Value = lstDuLieuBangBC[i].tongSLHSHoanThanh;
                    worksheet.Cells[rowStartLoai + i, 13].Value = lstDuLieuBangBC[i].slHsDaLuuKho;
                    worksheet.Cells[rowStartLoai + i, 14].Value = lstDuLieuBangBC[i].slHsChuaLuuKho;
                    worksheet.Cells[rowStartLoai + i, 15].Value = lstDuLieuBangBC[i].ghiChu;
                }
                int rowTong = rowStartLoai + lstDuLieuBangBC.Count;
                worksheet.Cells[rowTong, 3].Value = "TỔNG CỘNG";
                worksheet.Cells[rowTong, 3].Style.Font.Bold = true;
                worksheet.Cells[rowTong, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Các cột số từ Cột 4 → 15
                worksheet.Cells[rowTong, 4].Value = lstDuLieuBangBC.Sum(x => x.tongSLHSDaTiepNhan);
                worksheet.Cells[rowTong, 5].Value = lstDuLieuBangBC.Sum(x => x.tongSoLuongHSDaGiaiQuyet);
                worksheet.Cells[rowTong, 6].Value = lstDuLieuBangBC.Sum(x => x.soLuongHSDaGiaiQuyetTruocHan);
                worksheet.Cells[rowTong, 7].Value = lstDuLieuBangBC.Sum(x => x.soLuongHSDaGiaiQuyetDungHan);
                worksheet.Cells[rowTong, 8].Value = lstDuLieuBangBC.Sum(x => x.soLuongHSDaGiaiQuyetQuaHan);
                worksheet.Cells[rowTong, 9].Value = lstDuLieuBangBC.Sum(x => x.tongSLHSDangGiaiQuyet);
                worksheet.Cells[rowTong, 10].Value = lstDuLieuBangBC.Sum(x => x.soLuongHSDangGQTrongHan);
                worksheet.Cells[rowTong, 11].Value = lstDuLieuBangBC.Sum(x => x.soLuongHSDangGQQuaHan);
                worksheet.Cells[rowTong, 12].Value = lstDuLieuBangBC.Sum(x => x.tongSLHSHoanThanh);
                worksheet.Cells[rowTong, 13].Value = lstDuLieuBangBC.Sum(x => x.slHsDaLuuKho);
                worksheet.Cells[rowTong, 14].Value = lstDuLieuBangBC.Sum(x => x.slHsChuaLuuKho);
                // Rồi set style cuối cùng cho toàn dòng
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"B{rowTong}:O{rowTong}"], 30, "Times New Roman", 12, true, false, true);
                int rowEndLoai = rowTong - 1;
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"B{rowStartLoai}:B{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true, ExcelHorizontalAlignment.Left);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"C{rowStartLoai}:C{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true, ExcelHorizontalAlignment.Left);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"O{rowStartLoai}:O{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true, ExcelHorizontalAlignment.Left);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"B{rowStartLoai}:B{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"D{rowStartLoai}:D{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"E{rowStartLoai}:E{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"F{rowStartLoai}:F{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"G{rowStartLoai}:G{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"H{rowStartLoai}:H{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"I{rowStartLoai}:I{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"J{rowStartLoai}:J{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"K{rowStartLoai}:K{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"L{rowStartLoai}:L{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"M{rowStartLoai}:M{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);
                ExcelFunctions.SetCommonStyles(worksheet.Cells[$"N{rowStartLoai}:N{rowEndLoai}"], 30, "Times New Roman", 12, false, false, true);

                stream.Position = 0;
                byte[] excelBytes = excelPackage.GetAsByteArray();

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }


        [Route("ExportExelBaoCaoGoiThauChiTiet/{id}")]
        [HttpPost]
        public IActionResult ExportExelBaoCaoGoiThauChiTiet(string id)
        {
            var baoCaoTongHop = context.QuanLyThuTucNoiBoBaoCaoTongHops.Where(baoCao => baoCao.Id == id)
                                                                             .Select(s => new LapBaoCaoTongHop
                                                                             {
                                                                                 id = s.Id,
                                                                                 Ten = s.Ten,
                                                                                 Nam = s.Nam,
                                                                                 NgayTao = s.NgayTao != null ? s.NgayTao.Value.ToString("dd/MM/yyyy") : "",
                                                                             }).FirstOrDefault();


            var lstBaoCaoChiTiet = context.QuanLyThuTucBaoCaoTongHopVeGoiThaus.Where(baocao => baocao.IdBaoCao == id)
                                                                              .OrderBy(x => x.LinhVucVaHinhThuc)
                                                                              .ThenBy(x => x.HinhThucDauThau == 2 ? 0
                                                                                         : x.HinhThucDauThau == 1 ? 1
                                                                                         : 2)
                                                                              .Select(x => new TongHopKetQuaLuaChonNhaThau
                                                                              {
                                                                                  id = x.Id,
                                                                                  chenhLechDoQuocHoiChuTruongDauTu = string.Format("{0:#,##0}", x.ChenhLechDoQuocHoiChuTruongDauTu).Replace(",", "."),
                                                                                  chenhLechDuAnNhomA = string.Format("{0:#,##0}", x.ChenhLechDuAnNhomA).Replace(",", "."),
                                                                                  chenhLechDuAnNhomB = string.Format("{0:#,##0}", x.ChenhLechDuAnNhomB).Replace(",", "."),
                                                                                  chenhLechDuAnNhomC = string.Format("{0:#,##0}", x.ChenhLechDuAnNhomC).Replace(",", "."),
                                                                                  chenhLechTongCong = string.Format("{0:#,##0}", x.ChenhLechTongCong).Replace(",", "."),
                                                                                  hinhThucDauThau = x.HinhThucDauThau,
                                                                                  linhVucVaHinhThuc = x.LinhVucVaHinhThuc,
                                                                                  tongGiaGoiThauDoQuocHoiChuTruongDauTu = string.Format("{0:#,##0}", x.TongGiaGoiThauDoQuocHoiChuTruongDauTu).Replace(",", "."),
                                                                                  tongGiaGoiThauDuAnNhomA = string.Format("{0:#,##0}", x.TongGiaGoiThauDuAnNhomA).Replace(",", "."),
                                                                                  tongGiaGoiThauDuAnNhomB = string.Format("{0:#,##0}", x.TongGiaGoiThauDuAnNhomB).Replace(",", "."),
                                                                                  tongGiaGoiThauDuAnNhomC = string.Format("{0:#,##0}", x.TongGiaGoiThauDuAnNhomC).Replace(",", "."),
                                                                                  tongGiaGoiThauTongCong = string.Format("{0:#,##0}", x.TongGiaGoiThauTongCong).Replace(",", "."),
                                                                                  tongGiaTrungThauDoQuocHoiChuTruongDauTu = string.Format("{0:#,##0}", x.TongGiaTrungThauDoQuocHoiChuTruongDauTu).Replace(",", "."),
                                                                                  tongGiaTrungThauDuAnNhomA = string.Format("{0:#,##0}", x.TongGiaTrungThauDuAnNhomA).Replace(",", "."),
                                                                                  tongGiaTrungThauDuAnNhomB = string.Format("{0:#,##0}", x.TongGiaTrungThauDuAnNhomB).Replace(",", "."),
                                                                                  tongGiaTrungThauDuAnNhomC = string.Format("{0:#,##0}", x.TongGiaTrungThauDuAnNhomC).Replace(",", "."),
                                                                                  tongGiaTrungThauTongCong = string.Format("{0:#,##0}", x.TongGiaTrungThauTongCong).Replace(",", "."),
                                                                                  tongSoGoiThauDoQuocHoiChuTruongDauTu = string.Format("{0:#,##0}", x.TongSoGoiThauDoQuocHoiChuTruongDauTu).Replace(",", "."),
                                                                                  tongSoGoiThauDuAnNhomA = string.Format("{0:#,##0}", x.TongSoGoiThauDuAnNhomA).Replace(",", "."),
                                                                                  tongSoGoiThauDuAnNhomB = string.Format("{0:#,##0}", x.TongSoGoiThauDuAnNhomB).Replace(",", "."),
                                                                                  tongSoGoiThauDuAnNhomC = string.Format("{0:#,##0}", x.TongSoGoiThauDuAnNhomC).Replace(",", "."),
                                                                                  tongSoGoiThauTongCong = string.Format("{0:#,##0}", x.TongSoGoiThauTongCong).Replace(",", "."),
                                                                              }).ToList();
            string templatePath = _webHostEnvironment.ContentRootPath + @"/XuatExel/BieuTongHopGoiThau.xlsx";
            string filenameOutput = "XuatBieuTongHopGoiThau" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            FileInfo fileInfo = new FileInfo(templatePath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.Name = "DanhSachGoiThau";

                int startRow = 5;

                foreach (var item in lstBaoCaoChiTiet)
                {
                    
                        ExcelFunctions.SetCommonStyles(worksheet.Cells[$"C{startRow}:V{startRow}"], 18, "Times New Roman", 12, false, false, true);
                        worksheet.Cells[startRow, 3].Value = item?.tongSoGoiThauDoQuocHoiChuTruongDauTu;
                        worksheet.Cells[startRow, 4].Value = item?.tongGiaGoiThauDoQuocHoiChuTruongDauTu;
                        worksheet.Cells[startRow, 5].Value = item?.tongGiaTrungThauDoQuocHoiChuTruongDauTu;
                        worksheet.Cells[startRow, 6].Value = item?.chenhLechDoQuocHoiChuTruongDauTu;
                        worksheet.Cells[startRow, 7].Value = item?.tongSoGoiThauDuAnNhomA;
                        worksheet.Cells[startRow, 8].Value = item?.tongGiaGoiThauDuAnNhomA;
                        worksheet.Cells[startRow, 9].Value = item?.tongGiaTrungThauDuAnNhomA;
                        worksheet.Cells[startRow, 10].Value = item?.chenhLechDuAnNhomA;
                        worksheet.Cells[startRow, 11].Value = item?.tongSoGoiThauDuAnNhomB;
                        worksheet.Cells[startRow, 12].Value = item?.tongGiaGoiThauDuAnNhomB;
                        worksheet.Cells[startRow, 13].Value = item?.tongGiaTrungThauDuAnNhomB;
                        worksheet.Cells[startRow, 14].Value = item?.chenhLechDuAnNhomB;
                        worksheet.Cells[startRow, 15].Value = item?.tongSoGoiThauDuAnNhomC;
                        worksheet.Cells[startRow, 16].Value = item?.tongGiaGoiThauDuAnNhomC;
                        worksheet.Cells[startRow, 17].Value = item?.tongGiaTrungThauDuAnNhomC;
                        worksheet.Cells[startRow, 18].Value = item?.chenhLechDuAnNhomC;
                        worksheet.Cells[startRow, 19].Value = item?.tongSoGoiThauTongCong;
                        worksheet.Cells[startRow, 20].Value = item?.tongGiaGoiThauTongCong;
                        worksheet.Cells[startRow, 21].Value = item?.tongGiaTrungThauTongCong;
                        worksheet.Cells[startRow, 22].Value = item?.chenhLechTongCong;
                    if(startRow == 16)
                    {
                        startRow++;
                    }
                    startRow++;
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
    }
}
