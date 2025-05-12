using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung;
using DTC_BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using DTC_BE.Models.Layout.TopBar;
using DTC_BE.CodeBase;
using DTC_BE.Entities;

namespace DTC_BE.Controllers.Layout
{
    [Route("api/Layout/[controller]")]
    [ApiController]
    public class TopBarController : BaseApiController
    {
        #region Tìm kiếm thông báo chủ đầu tư
        [Route("GetDanhSachThongBao")]
        [HttpPost]
        public ResponseMessage GetDanhSachThongBao(TimKiemDanhSachThongBao timKiemDanhSach)
        {
            try
            {
                var listHtnguoiDung = context.HtNguoiDungs;
                var quanlyThuTuc = context.QuanLyThuTucNoiBoDuAnDtcs;
                var thuTucDict = quanlyThuTuc.GroupBy(x => x.TenHoSo.Trim())
                                             .ToDictionary(g => g.Key, g => g.First().Id);
               List<ThongBaoListItem> lstThongBao = context.HtNhatKyHeThongs
                                             .Where(thongBao =>
                                                 thongBao.TenNguoiDung == timKiemDanhSach.IdUser &&
                                                 (thongBao.LoaiChucNang == Enums.PhanHe.ThuTucNoiBoVeKeHoachLuaChonNhaThau.GetDescription()
                                                  || thongBao.LoaiChucNang == Enums.PhanHe.QuanLyThuTucNoiBo.GetDescription()))
                                             .OrderBy(thongBao => thongBao.TrangThai)
                                             .AsEnumerable() // Lúc này dữ liệu đã tải về, có thể xử lý với Dictionary
                                             .Select(thongBao =>
                                             {
                                                 string prefix = "Cập nhật thụ lý ";
                                                 string thongBaoId;

                                                 if (thongBao.MoTa.StartsWith(prefix))
                                                 {
                                                     string tenHoSo = thongBao.MoTa.Substring(prefix.Length).Trim();
                                                     thongBaoId = thuTucDict.ContainsKey(tenHoSo)
                                                         ? thuTucDict[tenHoSo].ToString()
                                                         : thongBao.MoTa;
                                                 }
                                                 else
                                                 {
                                                     thongBaoId = thongBao.MoTa;
                                                 }

                                                 return new ThongBaoListItem
                                                 {
                                                     Id = thongBao.Id,
                                                     ThongBaoId = thongBaoId,
                                                     TieuDe = thongBao.MoTa,
                                                     TrangThai = int.Parse(thongBao.TrangThai),
                                                     NgayGui = thongBao.NgayCapNhat.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy hh:mm tt"),
                                                 };
                                             })
                                             .Skip(timKiemDanhSach.RowPerPage * (timKiemDanhSach.CurrentPage - 1))
                                             .Take(timKiemDanhSach.RowPerPage)
                                             .ToList();

                int totalRecords = lstThongBao.Count();

                int countThongBaoChuaDoc = lstThongBao.Where(thongBao => thongBao.TrangThai == (int)Enums.LoaiChucNang.Nhan)
                                                                     .Count();

                message.IsError = false;
                message.ObjData = new { lstThongBao, totalRecords, countThongBaoChuaDoc };
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

        [Route("GetThongTinNguoiDung")]
        [HttpPost]
        public ResponseMessage GetThongTinNguoiDung([FromBody] string iduser)
        {
            try
            {
                var listHtnguoiDung = context.HtNguoiDungs;
                var nguoiDung = listHtnguoiDung.FirstOrDefault(x => x.Id == iduser);

                if (nguoiDung == null)
                {
                    message.IsError = true;
                    message.Title = "Không tìm thấy người dùng";
                    message.Code = HttpStatusCode.NotFound.GetHashCode();
                    return message;
                }
                var htnhomquyen = context.HtNhomQuyens;
                var TenTaiKhoan = nguoiDung.HoTen;
                var nhomQuyen = htnhomquyen.FirstOrDefault(x =>x.Id == nguoiDung.HtNhomQuyenId)?.Ten;

                message.IsError = false;
                message.ObjData = new { TenTaiKhoan, nhomQuyen };
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

        #endregion

        #region Action other
        [Route("XemThongBao")]
        [HttpGet]
        public ResponseMessage XemThongBao(string? idThongBaoChiTiet)
        {
            try
            {
                HtNhatKyHeThong? thongBaoView = context.HtNhatKyHeThongs.FirstOrDefault(thongBao => thongBao.Id == idThongBaoChiTiet);
                if (thongBaoView != null)
                {
                    thongBaoView.NgayCapNhat = DateTime.Now;
                    thongBaoView.TrangThai = ((int)Enums.LoaiChucNang.Nhan).ToString();

                    context.SaveChanges();
                }

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = ex.Message;
            }
            finally
            {
            }
            return message;
        }
        #endregion
    }
}
