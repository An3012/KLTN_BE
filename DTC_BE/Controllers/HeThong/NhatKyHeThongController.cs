using DTC_BE.Entities;
using DTC_BE.Models.HeThong.NhatKyHeThong;
using DTC_BE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DTC_BE.CodeBase;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace DTC_BE.Controllers.HeThong
{
    [Route("api/HeThong/[controller]")]
    [ApiController]
    public class NhatKyHeThongController : BaseApiController
    {
        #region Tìm kiếm nhật ký hệ thống
        [Route("GetDanhSachNhatKyHeThong")]
        [HttpPost]
        public ResponseMessage GetDanhSachNhatKyHeThong(TimKiemDanhSachNhatKyHeThong timKiemDanhSach)
        {
            try
            {
                DateTime? tuNgay = !string.IsNullOrEmpty(timKiemDanhSach?.TuNgay) ? Convert.ToDateTime(timKiemDanhSach?.TuNgay, objCultureInfo).Date : null;
                DateTime? denNgay = !string.IsNullOrEmpty(timKiemDanhSach?.DenNgay) ? Convert.ToDateTime(timKiemDanhSach?.DenNgay, objCultureInfo).Date : null;

                List<HtNhatKyHeThong> lstNhatKyHeThong = context.HtNhatKyHeThongs.Where(x => (!string.IsNullOrEmpty(timKiemDanhSach.TenNguoiDung) ? x.TenNguoiDung.ToLower().Trim().Contains(timKiemDanhSach.TenNguoiDung.ToLower().Trim()) : true)
                                                                                          && (!string.IsNullOrEmpty(timKiemDanhSach.IpNguoiDung) ? x.IpNguoiDung.ToLower().Trim().Contains(timKiemDanhSach.IpNguoiDung.ToLower().Trim()) : true)
                                                                                          && (!string.IsNullOrEmpty(timKiemDanhSach.MoTa) ? x.MoTa.ToLower().Trim().Contains(timKiemDanhSach.MoTa.ToLower().Trim()) : true)
                                                                                          && (!string.IsNullOrEmpty(timKiemDanhSach.TuNgay) ? (x.NgayCapNhat != null && x.NgayCapNhat.Value.Date >= tuNgay) : true)
                                                                                          && (!string.IsNullOrEmpty(timKiemDanhSach.DenNgay) ? (x.NgayCapNhat != null && x.NgayCapNhat.Value.Date <= denNgay) : true))
                                                                                 .OrderByDescending(s => s.NgayCapNhat)
                                                                                 .Skip(timKiemDanhSach.RowPerPage * (timKiemDanhSach.CurrentPage - 1))
                                                                                 .Take(timKiemDanhSach.RowPerPage)
                                                                                 .ToList();

                int totalRecords = context.HtNhatKyHeThongs.Where(x => (!string.IsNullOrEmpty(timKiemDanhSach.TenNguoiDung) ? x.TenNguoiDung.ToLower().Trim().Contains(timKiemDanhSach.TenNguoiDung.ToLower().Trim()) : true)
                                                                                          && (!string.IsNullOrEmpty(timKiemDanhSach.IpNguoiDung) ? x.IpNguoiDung.ToLower().Trim().Contains(timKiemDanhSach.IpNguoiDung.ToLower().Trim()) : true)
                                                                                          && (!string.IsNullOrEmpty(timKiemDanhSach.MoTa) ? x.MoTa.ToLower().Trim().Contains(timKiemDanhSach.MoTa.ToLower().Trim()) : true)
                                                                                          && (!string.IsNullOrEmpty(timKiemDanhSach.TuNgay) ? (x.NgayCapNhat != null && x.NgayCapNhat.Value.Date >= tuNgay) : true)
                                                                                          && (!string.IsNullOrEmpty(timKiemDanhSach.DenNgay) ? (x.NgayCapNhat != null && x.NgayCapNhat.Value.Date <= denNgay) : true))
                                                           .Count();

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = new { lstNhatKyHeThong, totalRecords };
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;
        }
        #endregion

        #region CRUD nhật ký hệ thống
        [Route("XoaNhatKyHeThong/{id}")]
        [HttpGet]
        public ResponseMessage XoaNhatKyHeThong(string? id)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    HtNhatKyHeThong? nhatKyHeThong = context.HtNhatKyHeThongs.FirstOrDefault(nhatKyHeThong => nhatKyHeThong.Id == id);
                    if (nhatKyHeThong != null)
                    {
                        context.HtNhatKyHeThongs.Remove(nhatKyHeThong);
                    }

                    message.Title = "Xóa thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    //ThemMoiNhatKy("Xóa nhật ký hệ thống: " + nhatKyHeThong?.TenNhatKyHeThong, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.Xoa),
                    //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                                                 EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
                    //                                                                 idUser);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    //ThemMoiNhatKy("Xóa nhật ký hệ thống: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.Xoa),
                    //                                                       EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                                       EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                                       idUser);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        #endregion

        #region Other
        [Route("ExportExcel")]
        [HttpPost]
        public IActionResult ExportExcel(TimKiemDanhSachNhatKyHeThong timKiemDanhSach)
        {
            try
            {
                List<HtNhatKyHeThong> GetDataExport()
                {
                    DateTime? tuNgay = !string.IsNullOrEmpty(timKiemDanhSach?.TuNgay) ? Convert.ToDateTime(timKiemDanhSach?.TuNgay, objCultureInfo).Date : null;
                    DateTime? denNgay = !string.IsNullOrEmpty(timKiemDanhSach?.DenNgay) ? Convert.ToDateTime(timKiemDanhSach?.DenNgay, objCultureInfo).Date : null;

                    List<HtNhatKyHeThong> lstNhatKyHeThong = context.HtNhatKyHeThongs.Where(x => (!string.IsNullOrEmpty(timKiemDanhSach.TenNguoiDung) ? x.TenNguoiDung.ToLower().Trim().Contains(timKiemDanhSach.TenNguoiDung.ToLower().Trim()) : true)
                                                                                              && (!string.IsNullOrEmpty(timKiemDanhSach.IpNguoiDung) ? x.IpNguoiDung.ToLower().Trim().Contains(timKiemDanhSach.IpNguoiDung.ToLower().Trim()) : true)
                                                                                              && (!string.IsNullOrEmpty(timKiemDanhSach.MoTa) ? x.MoTa.ToLower().Trim().Contains(timKiemDanhSach.MoTa.ToLower().Trim()) : true)
                                                                                              && (!string.IsNullOrEmpty(timKiemDanhSach.TuNgay) ? (x.NgayCapNhat != null && x.NgayCapNhat.Value.Date >= tuNgay) : true)
                                                                                              && (!string.IsNullOrEmpty(timKiemDanhSach.DenNgay) ? (x.NgayCapNhat != null && x.NgayCapNhat.Value.Date <= denNgay) : true))
                                                                                     .OrderByDescending(s => s.NgayCapNhat)
                                                                                     .Skip(timKiemDanhSach.RowPerPage * (timKiemDanhSach.CurrentPage - 1))
                                                                                     .Take(timKiemDanhSach.RowPerPage)
                                                                                     .ToList();
                    return lstNhatKyHeThong;
                }

                List<HtNhatKyHeThong> listXuatExcel = GetDataExport();
                var fileName = "DanhSachNhatKyHeThong" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + DateTime.Now.ToString("HH:mm") + ".xlsx";
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                MemoryStream stream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(stream))
                {
                    excelPackage.Workbook.Properties.Author = "SFB";
                    excelPackage.Workbook.Properties.Title = $"Danh sách nhật ký hệ thống";
                    var worksheet = excelPackage.Workbook.Worksheets.Add("DanhSachNhatKyHeThong");
                    //if (listXuatExcel.Count > 0)
                    //{
                    // Setting default styles and column widths
                    worksheet.Cells.Style.WrapText = true;
                    worksheet.DefaultRowHeight = 15;
                    worksheet.Column(1).Width = 5;
                    worksheet.Column(2).Width = 25;
                    worksheet.Column(3).Width = 20;
                    worksheet.Column(4).Width = 35;
                    worksheet.Column(5).Width = 85;
                    worksheet.Column(6).Width = 24;
                    worksheet.Column(7).Width = 20;
                    ExcelFunctions.SetCommonStyles(worksheet.Cells["A1:G3"], 30, "Times New Roman", 12, true, false, true);
                    // Setting header values and styles
                    ExcelFunctions.MergeAndSetValue(worksheet, "A1:G2", $"DANH SÁCH NHẬT KÝ HỆ THỐNG");
                    ExcelFunctions.MergeAndSetValue(worksheet, "A3", $"STT");
                    ExcelFunctions.MergeAndSetValue(worksheet, "B3", $"Tên người dùng");
                    ExcelFunctions.MergeAndSetValue(worksheet, "C3", $"IP người dùng");
                    ExcelFunctions.MergeAndSetValue(worksheet, "D3", $"Loại chức năng");
                    ExcelFunctions.MergeAndSetValue(worksheet, "E3", $"Mô tả");
                    ExcelFunctions.MergeAndSetValue(worksheet, "F3", $"Ngày cập nhật");
                    ExcelFunctions.MergeAndSetValue(worksheet, "G3", $"Trạng thái");

                    int rowStart = 4;
                    int rowEnd = 0;
                    for (int i = 0; i < listXuatExcel.Count; i++)
                    {
                        worksheet.Cells[rowStart + i, 1].Value = i + 1;
                        worksheet.Cells[rowStart + i, 2].Value = listXuatExcel[i].TenNguoiDung;
                        worksheet.Cells[rowStart + i, 3].Value = listXuatExcel[i].IpNguoiDung;
                        worksheet.Cells[rowStart + i, 4].Value = listXuatExcel[i].LoaiChucNang;
                        worksheet.Cells[rowStart + i, 5].Value = listXuatExcel[i].MoTa;
                        worksheet.Cells[rowStart + i, 6].Value = listXuatExcel[i].NgayCapNhat.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt");
                        worksheet.Cells[rowStart + i, 7].Value = listXuatExcel[i].TrangThai;
                        rowEnd = rowStart + i;
                    }

                    rowEnd = rowEnd > rowStart ? rowEnd : rowStart;

                    ExcelFunctions.SetCommonStyles(worksheet.Cells[$"A{rowStart}:A{rowEnd}"], 30, "Times New Roman", 12, false, false, true);
                    ExcelFunctions.SetCommonStyles(worksheet.Cells[$"B{rowStart}:G{rowEnd}"], 30, "Times New Roman", 12, false, false, true, ExcelHorizontalAlignment.Left);
                    stream.Position = 0; // Đặt lại vị trí stream để đọc từ đầu
                    byte[] excelBytes = excelPackage.GetAsByteArray();
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Excel.xlsx");
                    // Ghi workbook ra memory stream
                }
            }
            //}
            catch (Exception ex)
            {
            }
            return null;
        }
        #endregion
    }
}
