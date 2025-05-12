using DTC_BE.CodeBase;
using DTC_BE.Entities;
using DTC_BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.UserModel;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using static DTC_BE.CodeBase.Enums;

namespace DTC_BE.Controllers
{
    public class BaseApiController : ControllerBase
    {
        public CultureInfo cultureInfo = new("vi-VN");
        public ResponseMessage message { get; set; }
        public DauTuCongContext context { get; set; }
        public System.Globalization.CultureInfo objCultureInfo = new("vi-VN");
        public BaseApiController()
        {
            context ??= new DauTuCongContext();

            message ??= new ResponseMessage();
        }

        public static string IntToRoman(int num)
        {
            if (num < 1 || num > 3999)
                throw new ArgumentOutOfRangeException("Input must be between 1 and 3999");

            StringBuilder result = new StringBuilder();

            // Mảng chứa các giá trị số nguyên và số La Mã tương ứng
            int[] values = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] symbols = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

            for (int i = 0; i < values.Length && num > 0; i++)
            {
                while (num >= values[i])
                {
                    num -= values[i];
                    result.Append(symbols[i]);
                }
            }

            return result.ToString();
        }

        public static void MoveOrCopyRow(ISheet sheet, int sourceRowIndex, int targetRowIndex, bool move)
        {
            // Get the source row
            IRow sourceRow = sheet.GetRow(sourceRowIndex);
            if (sourceRow == null)
            {
                throw new ArgumentException($"Source row {sourceRowIndex} does not exist.");
            }

            // Create or get the target row
            IRow targetRow = sheet.GetRow(targetRowIndex) ?? sheet.CreateRow(targetRowIndex);

            // Copy row height
            targetRow.Height = sourceRow.Height;

            // Copy cells from source row to target row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                ICell sourceCell = sourceRow.GetCell(i);
                if (sourceCell != null)
                {
                    ICell targetCell = targetRow.GetCell(i) ?? targetRow.CreateCell(i);
                    CopyCell(sourceCell, targetCell, sheet.Workbook);
                }
            }

            // Remove the source row if moving
            if (move)
            {
                sheet.RemoveRow(sourceRow);
            }
        }

        public static void CopyCell(ICell sourceCell, ICell targetCell, IWorkbook workbook)
        {
            // Copy cell value
            switch (sourceCell.CellType)
            {
                case CellType.Blank:
                    targetCell.SetCellValue(sourceCell.StringCellValue);
                    break;
                case CellType.Boolean:
                    targetCell.SetCellValue(sourceCell.BooleanCellValue);
                    break;
                case CellType.Error:
                    targetCell.SetCellErrorValue(sourceCell.ErrorCellValue);
                    break;
                case CellType.Formula:
                    targetCell.CellFormula = sourceCell.CellFormula;
                    break;
                case CellType.Numeric:
                    targetCell.SetCellValue(sourceCell.NumericCellValue);
                    break;
                case CellType.String:
                    targetCell.SetCellValue(sourceCell.RichStringCellValue);
                    break;
            }

            // Copy cell style
            if (sourceCell.CellStyle != null)
            {
                // Avoid creating a new style if possible
                ICellStyle existingStyle = workbook.GetCellStyleAt(sourceCell.CellStyle.Index);
                if (existingStyle == null)
                {
                    existingStyle = workbook.CreateCellStyle();
                    existingStyle.CloneStyleFrom(sourceCell.CellStyle);
                }
                targetCell.CellStyle = existingStyle;
            }

            // Copy cell comment if exists
            if (sourceCell.CellComment != null)
            {
                targetCell.CellComment = sourceCell.CellComment;
            }

            // Copy hyperlink if exists
            if (sourceCell.Hyperlink != null)
            {
                targetCell.Hyperlink = sourceCell.Hyperlink;
            }
        }

        public static string ConvertDoubleToString(double? value)
        {
            return value.HasValue ? value.Value.ToString() : "";
        }

        public static class Notify
        {
            private static string FormatMessage(string action, string? value)
            {
                value = !string.IsNullOrEmpty(value) ? $" {value} " : " ";
                return $"{action}{value}thành công";
            }

            public static string Insert(string? value)
            {
                return FormatMessage("Thêm mới", value);
            }

            public static string Update(string? value)
            {
                return FormatMessage("Cập nhật", value);
            }

            public static string Delete(string? value)
            {
                return FormatMessage("Xóa", value);
            }

            public static string Send(string? value)
            {
                return FormatMessage("Gửi", value);
            }
        }

        public static string[] getAllowedFileExsTypes()
        {
            return new[] { ".doc", ".docx", ".pdf", ".xlsx", ".xls" };
        }

        public static string[] getAllowedFileExsTypesWithParam(int allowType)
        {
            if (allowType == 1)
                return new[] { ".doc", ".docx", ".pdf", ".xlsx", ".xls" };
            if (allowType == 2)
                return new[] { ".xlsx" };
            if (allowType == 3)
                return new[] { ".docx" };
            if (allowType == 4)
                return new[] { ".pdf" };
            if (allowType == 4)
                return new[] { ".png", ".jpg", ".gif", ".jpge" };
            return null;
        }

        public static string[] getAllowedTienIchFileTypes()
        {
            return new[] { ".doc", ".docx", ".pdf", ".xlsx", ".xls", ".png", ".jpg" };
        }

        public static string[] getAllowedImageFileTypes()
        {
            return new[] { ".png", ".jpg", ".gif", ".jpeg", ".bmp", ".tif" };
        }
        public static string[] getAllowedImportTypes()
        {
            return new[] { ".xlsx" };
        }

        public static string[] getAllowedVanBanFileTypes()
        {
            return new[] { ".doc", ".docx", ".pdf", ".xlsx", ".xls", ".csv" };
        }

        public static string GetFolderByDate()
        {
            return $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/";
        }

        public static string ToRoman(int num)
        {
            if (num < 1 || num > 3999)
                return "I";

            string[] thousands = { "", "M", "MM", "MMM" };
            string[] hundreds = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
            string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
            string[] units = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

            return thousands[num / 1000] +
                   hundreds[(num % 1000) / 100] +
                   tens[(num % 100) / 10] +
                   units[num % 10];
        }

        public static string? CurrencyFormat(object? value)
        {
            string valueReturn = "";
            try
            {
                valueReturn = string.Format("{0:#,##0}", value).Replace(",", ".");
            }
            catch (Exception)
            {
                return "";
            }
            return valueReturn;
        }

        public static async Task<ResponseMessage> UploadFileService(IFormFile file, string config_UrlTemp, int allowExt = HangSo.AllowExtension_ImportFile)
        {
            var message = new ResponseMessage();

            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new Exception("File không tồn tại");
                }

                var allowedFileTypes = getAllowedFileExsTypesWithParam(allowExt);
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedFileTypes.Contains(fileExtension))
                {
                    throw new Exception("Định dạng file không được cho phép (cho phép: " + string.Join(",", allowedFileTypes) + " )");
                }

                string dt = DateTime.Now.ToString("ddMMyyhhmmss");
                string extens = Path.GetExtension(file.FileName);

                var fileNameSplit = ChuyenTiengVietKhongDau(Reverse(Reverse(file.FileName).Split(".")[1])).Replace(" ", "") + "_" + dt + extens;
                var urlSave = Path.Combine(Directory.GetCurrentDirectory(), config_UrlTemp);

                if (!Directory.Exists(urlSave))
                {
                    Directory.CreateDirectory(urlSave);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), config_UrlTemp, fileNameSplit);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                string filePath = "/" + config_UrlTemp + fileNameSplit;

                message.ObjData = new { filePath, fileName = file.FileName };
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.Title = "Tải tệp lên thành công";
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }

            return message;
        }

        public static IActionResult DownloadFileService(string filePath, string fileName)
        {

            string urlFile = Directory.GetCurrentDirectory() + filePath;

            // Kiểm tra xem filepath có hợp lệ không
            if (string.IsNullOrEmpty(urlFile) || !System.IO.File.Exists(urlFile))
            {
                return new NotFoundObjectResult("Không tìm thấy đường dẫn file");
            }

            // Thiết lập loại nội dung và trả về dữ liệu tệp
            return new FileStreamResult(System.IO.File.OpenRead(urlFile), "application/octet-stream")
            {
                FileDownloadName = fileName
            };
        }


        public static double? DoubleOrNull(object? value)
        {
            double? result = null;
            try
            {
                if (!string.IsNullOrEmpty(value?.ToString()))
                {
                    result = Convert.ToDouble(value);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public static int? IntOrNull(object? value)
        {
            int? result = null;
            try
            {
                if (!string.IsNullOrEmpty(value?.ToString()))
                {
                    result = Convert.ToInt32(value);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public static DateTime? DateTimeOrNull(object? value)
        {
            DateTime? result = null;
            try
            {
                if (!string.IsNullOrEmpty(value?.ToString()))
                {
                    result = Convert.ToDateTime(value, new CultureInfo("vi-VN"));
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public static void RemoveFile(string? pathFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(pathFile))
                {
                    FileInfo fileUpload = new FileInfo(pathFile);
                    if (System.IO.File.Exists(Directory.GetCurrentDirectory() + pathFile))
                    {
                        System.IO.File.Delete(Directory.GetCurrentDirectory() + pathFile);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string CopyAndEncryptFile(string? pathFile, string? fileName, string? config_UrlSave)
        {
            string filePathReturn = "";
            try
            {
                string fullPathFile = Directory.GetCurrentDirectory() + pathFile;
                FileInfo fileUpload = new FileInfo(fullPathFile);
                string dtn = DateTime.Now.ToString("ddMMyyhhmmss");
                var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(fileName?.Trim()).Split(".")[0]));
                filePathReturn = "/" + config_UrlSave + SecurityService.EncryptPassword(fileName?.Trim() + dtn) + extension;
                var pathSaveFile = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

                if (fileUpload.Exists)
                {
                    Directory.CreateDirectory(pathSaveFile);
                    // Tạo đường dẫn đầy đủ cho file đích
                    string destinationFilePath = Path.Combine(pathSaveFile, SecurityService.EncryptPassword(fileName?.Trim() + dtn) + extension);
                    #region copy file to new path
                    fileUpload.CopyTo(destinationFilePath, true);
                    #endregion

                    #region remove old file
                    if (System.IO.File.Exists(fullPathFile))
                    {
                        System.IO.File.Delete(fullPathFile);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return filePathReturn;
        }

        public Expression<Func<TElement, bool>> IsSameDate<TElement>(Expression<Func<TElement, DateTime>> valueSelector, DateTime value)
        {
            ParameterExpression p = valueSelector.Parameters.Single();

            var antes = Expression.GreaterThanOrEqual(valueSelector.Body, Expression.Constant(value.Date, typeof(DateTime)));

            var despues = Expression.LessThan(valueSelector.Body, Expression.Constant(value.AddDays(1).Date, typeof(DateTime)));

            Expression body = Expression.And(antes, despues);

            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }


        public static DateTime ParseDate(string dateStr, CultureInfo cultureInfo)
        {
            DateTime parsedDate;
            string[] dateFormats = { "dd/MM/yyyy", "MM/dd/yyyy" };

            if (DateTime.TryParseExact(dateStr, dateFormats, cultureInfo, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                throw new FormatException("Invalid date format");
            }
        }

        public static string ChuyenTiengVietKhongDau(string strVietNamese)
        {
            const string TextToFind = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string TextToReplace = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            while ((index = strVietNamese.IndexOfAny(TextToFind.ToCharArray())) != -1)
            {
                int index2 = TextToFind.IndexOf(strVietNamese[index]);
                strVietNamese = strVietNamese.Replace(strVietNamese[index], TextToReplace[index2])
                                             .Replace(" ", "");
            }
            // strVietNamese = strVietNamese.Replace(" ", "-").Replace(".", "");
            return strVietNamese;

        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static int? NullOrInt32(object? value)
        {
            return string.IsNullOrEmpty(value?.ToString()) ? null : Convert.ToInt32(value);
        }

        public static int NullOrInt32_Return0(object? value)
        {
            return string.IsNullOrEmpty(value?.ToString()) ? 0 : Convert.ToInt32(value);
        }

        public static DateTime? NullOrDateTime(object? value)
        {
            return string.IsNullOrEmpty(value?.ToString()) ? null : Convert.ToDateTime(value, new CultureInfo("vi-vn"));
        }

        public static string GetUserRole(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "your-issuer", // Cần khớp với cấu hình khi tạo token
                ValidateAudience = true,
                ValidAudience = "your-audience", // Cần khớp với cấu hình khi tạo token
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key")) // Cần khớp với cấu hình khi tạo token
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken validatedToken);

                // Lấy danh sách các Claims từ token
                var claims = claimsPrincipal.Claims;

                // Tìm và trả về vai trò của người dùng (nếu có)
                var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                if (roleClaim != null)
                {
                    return roleClaim.Value;
                }
                else
                {
                    return "No Role Found"; // Nếu không tìm thấy vai trò
                }
            }
            catch (Exception ex)
            {
                return "Invalid Token"; // Xử lý lỗi, token không hợp lệ
            }
        }

        [NonAction]
        public void ThemMoiNhatKy(string moTa, string loaiChucNang, string phanHe, string trangThai, string? idNguoiDung)
        {
            try
            {
                string? tenNguoiDung = context.HtNguoiDungs.Where(s => s.Id == idNguoiDung).FirstOrDefault()?.HoTen;

                HtNhatKyHeThong objNhatky = new HtNhatKyHeThong();
                objNhatky.Id = Guid.NewGuid().ToString();
                objNhatky.IpNguoiDung = HttpContext.Connection.RemoteIpAddress?.ToString();
                objNhatky.MoTa = moTa;
                objNhatky.TenNguoiDung = tenNguoiDung;
                objNhatky.LoaiChucNang = loaiChucNang;
                objNhatky.TrangThai = trangThai;
                objNhatky.NgayCapNhat = DateTime.Now;
                context.HtNhatKyHeThongs.Add(objNhatky);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [NonAction]
        public void ThemMoiChuyenThuLy(string moTa, string loaiChucNang, string phanHe, string trangThai, string? idNguoiDung, string? tenNguoiDung)
        {
            try
            {

                HtNhatKyHeThong objNhatky = new HtNhatKyHeThong();
                objNhatky.Id = Guid.NewGuid().ToString();
                objNhatky.IpNguoiDung = HttpContext.Connection.RemoteIpAddress?.ToString();
                objNhatky.MoTa = moTa;
                objNhatky.TenNguoiDung = tenNguoiDung;
                objNhatky.LoaiChucNang = loaiChucNang;
                objNhatky.TrangThai = trangThai;
                objNhatky.NgayCapNhat = DateTime.Now;
                context.HtNhatKyHeThongs.Add(objNhatky);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public static class EntityExtensions
    {
        public static string GetCombinedString<TEntity>(this TEntity entity, params Expression<Func<TEntity, string>>[] propertyExpressions)
        {
            var combinedString = string.Empty;

            foreach (var expression in propertyExpressions)
            {
                var propertyValue = expression.Compile().Invoke(entity);
                combinedString += propertyValue ?? string.Empty;
            }

            return combinedString;
        }
    }


}
