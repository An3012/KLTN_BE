using DTC_BE.CodeBase;
using DTC_BE.Entities;
using DTC_BE.Models;
using DTC_BE.Models.HeThong.ChuDauTu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;

namespace DTC_BE.Controllers.HeThong
{
    [Route("api/HeThong/[controller]")]
    [ApiController]
    public class ChuDauTuController : BaseApiController
    {
        private string config_UrlSave = "Uploads/HeThong/ChuDauTu/";
        private string config_UrlTemp = "Temps/HeThong/ChuDauTu/";

        #region Tìm kiếm chủ đầu tư
        [Route("GetDanhSachChuDauTu")]
        [HttpPost]
        public ResponseMessage GetDanhSachChuDauTu(TimKiemDanhSachChuDauTu timKiemDanhSach)
        {
            try
            {
                List<DmChuDauTu> lstChuDauTu = new();
                lstChuDauTu = context.DmChuDauTus.Where(ChuDauTu => (!string.IsNullOrEmpty(timKiemDanhSach.TenChuDauTu) ? ChuDauTu.TenChuDauTu.ToLower().Trim().Contains(timKiemDanhSach.TenChuDauTu.ToLower().Trim()) : true)
                                                                    && (!string.IsNullOrEmpty(timKiemDanhSach.MaSoThue) ? ChuDauTu.MaSoThue.ToLower().Trim().Contains(timKiemDanhSach.MaSoThue.ToLower().Trim()) : true))
                                                 .OrderByDescending(ChuDauTu => ChuDauTu.NgayTao)
                                                 .Skip(timKiemDanhSach.RowPerPage * (timKiemDanhSach.CurrentPage - 1))
                                                 .Take(timKiemDanhSach.RowPerPage)
                                                 .ToList();
                int totalRecords = context.DmChuDauTus.Where(ChuDauTu => (!string.IsNullOrEmpty(timKiemDanhSach.TenChuDauTu) ? ChuDauTu.TenChuDauTu.ToLower().Trim().Contains(timKiemDanhSach.TenChuDauTu.ToLower().Trim()) : true)
                                                                         && (!string.IsNullOrEmpty(timKiemDanhSach.MaSoThue) ? ChuDauTu.MaSoThue.ToLower().Trim().Contains(timKiemDanhSach.MaSoThue.ToLower().Trim()) : true))
                                                      .Count();
                message.IsError = false;
                message.ObjData = new { lstChuDauTu, totalRecords };
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

        #region CRUD chủ đầu tư
        [Route("ThemMoiChuDauTu")]
        [HttpPost]
        public ResponseMessage ThemMoiChuDauTu(ChuDauTuModel objChuDauTu)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (CheckExist(objChuDauTu?.TenChuDauTu, objChuDauTu?.Id))
                    {
                        throw new Exception("Tên chủ đầu tư: " + objChuDauTu?.TenChuDauTu?.Trim() + " đã tồn tại, Vui lòng kiểm tra lại");
                    }

                    string dt = DateTime.Now.ToString("ddMMyyhhmmss");

                    DmChuDauTu ChuDauTu = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TenChuDauTu = objChuDauTu?.TenChuDauTu?.Trim(),
                        MaSoThue = objChuDauTu?.MaSoThue?.Trim(),
                        DiaChi = objChuDauTu?.DiaChi?.Trim(),
                        SoDienThoai = objChuDauTu?.SoDienThoai?.Trim(),
                        Email = objChuDauTu?.Email?.Trim(),
                        Loai = objChuDauTu?.Loai,
                        NgayTao = DateTime.Now,
                        NguoiTao = objChuDauTu?.NguoiTao?.Trim(),
                        XaPhuong = objChuDauTu?.XaPhuong?.Trim(),
                        QuanHuyen = objChuDauTu?.QuanHuyen?.Trim(),
                        TinhThanh = objChuDauTu?.TinhThanh?.Trim(),
                        NguoiDaiDien = objChuDauTu?.NguoiDaiDien?.Trim(),
                        NgayHoatDong = DateTimeOrNull(objChuDauTu?.NgayHoatDong),
                    };

                    context.DmChuDauTus.Add(ChuDauTu);

                    message.Title = "Thêm mới thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    ThemMoiNhatKy($"Thêm mới chủ đầu tư: {objChuDauTu?.TenChuDauTu}", Enums.LoaiChucNang.ThemMoi.GetDescription(),
                                                                                      Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                                      Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                                      objChuDauTu?.NguoiTao);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    ThemMoiNhatKy($"Thêm mới chủ đầu tư: {objChuDauTu?.TenChuDauTu}", Enums.LoaiChucNang.ThemMoi.GetDescription(),
                                                                                      Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                                      Enums.NhatKyHeThong_TrangThai.KhongThanhCong.GetDescription(),
                                                                                      objChuDauTu?.NguoiTao);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("CapNhatChuDauTu")]
        [HttpPost]
        public ResponseMessage CapNhatChuDauTu(ChuDauTuModel objChuDauTu)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (CheckExist(objChuDauTu?.TenChuDauTu, objChuDauTu?.Id))
                    {
                        throw new Exception("Tên chủ đầu tư: " + objChuDauTu?.TenChuDauTu?.Trim() + " đã tồn tại, Vui lòng kiểm tra lại");
                    }

                    DmChuDauTu? ChuDauTu = context.DmChuDauTus.FirstOrDefault(ChuDauTu => ChuDauTu.Id == objChuDauTu.Id);
                    if (ChuDauTu != null)
                    {
                        ChuDauTu.TenChuDauTu = objChuDauTu?.TenChuDauTu?.Trim();
                        ChuDauTu.MaSoThue = objChuDauTu?.MaSoThue?.Trim();
                        ChuDauTu.DiaChi = objChuDauTu?.DiaChi?.Trim();
                        ChuDauTu.SoDienThoai = objChuDauTu?.SoDienThoai?.Trim();
                        ChuDauTu.Email = objChuDauTu?.Email?.Trim();
                        ChuDauTu.Loai = objChuDauTu?.Loai;
                        ChuDauTu.NgayCapNhat = DateTime.Now;
                        ChuDauTu.NguoiCapNhat = objChuDauTu?.NguoiCapNhat?.Trim();
                        ChuDauTu.XaPhuong = objChuDauTu?.XaPhuong?.Trim();
                        ChuDauTu.QuanHuyen = objChuDauTu?.QuanHuyen?.Trim();
                        ChuDauTu.TinhThanh = objChuDauTu?.TinhThanh?.Trim();
                        ChuDauTu.NguoiDaiDien = objChuDauTu?.NguoiDaiDien?.Trim();
                        ChuDauTu.NgayHoatDong = DateTimeOrNull(objChuDauTu?.NgayHoatDong);
                    }

                    message.Title = "Cập nhật thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                    ThemMoiNhatKy($"Cập nhật chủ đầu tư: {objChuDauTu?.TenChuDauTu}", Enums.LoaiChucNang.CapNhat.GetDescription(),
                                                                                      Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                                      Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                                      objChuDauTu?.NguoiCapNhat);

                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    ThemMoiNhatKy($"Cập nhật chủ đầu tư: {objChuDauTu?.TenChuDauTu}", Enums.LoaiChucNang.CapNhat.GetDescription(),
                                                                                      Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                                      Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                                      objChuDauTu?.NguoiCapNhat);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("XoaChuDauTu")]
        [HttpGet]
        public ResponseMessage XoaChuDauTu(string? id, string? idUser)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                DmChuDauTu? ChuDauTu = context.DmChuDauTus.FirstOrDefault(ChuDauTu => ChuDauTu.Id == id);
                try
                {
                    if (ChuDauTu != null)
                    {
                        context.DmChuDauTus.Remove(ChuDauTu);
                    }

                    message.Title = "Xóa thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    ThemMoiNhatKy($"Xóa chủ đầu tư: {ChuDauTu?.TenChuDauTu}", Enums.LoaiChucNang.CapNhat.GetDescription(),
                                                                              Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                              Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                              idUser);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)  // Error code 547 là lỗi FK constraint
                    {
                        message.Title = "Bản ghi đang sử dụng, không thể xóa";
                    }
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    ThemMoiNhatKy($"Xóa chủ đầu tư: {ChuDauTu?.TenChuDauTu}", Enums.LoaiChucNang.CapNhat.GetDescription(),
                                                                              Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                              Enums.NhatKyHeThong_TrangThai.KhongThanhCong.GetDescription(),
                                                                              idUser);
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
        [Route("GetChuDauTuById/{id}")]
        [HttpGet]
        public ResponseMessage GetChuDauTuById(string? id)
        {
            try
            {
                var ChuDauTu = context.DmChuDauTus.Where(ChuDauTu => ChuDauTu.Id == id)
                                                  .Select(ChuDauTu => new
                                                  {
                                                      ChuDauTu.Id,
                                                      ChuDauTu.TenChuDauTu,
                                                      ChuDauTu.MaSoThue,
                                                      ChuDauTu.DiaChi,
                                                      ChuDauTu.Loai,
                                                      ChuDauTu.Email,
                                                      ChuDauTu.SoDienThoai,
                                                      ChuDauTu.XaPhuong,
                                                      ChuDauTu.TinhThanh,
                                                      ChuDauTu.QuanHuyen,
                                                      ChuDauTu.NguoiDaiDien,
                                                      NgayHoatDong = ChuDauTu.NgayHoatDong != null ? ChuDauTu.NgayHoatDong.Value.ToString("dd/MM/yyyy") : "",
                                                  })
                                                  .FirstOrDefault();

                message.ObjData = ChuDauTu;
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

        private bool CheckExist(string? ten, string? id)
        {
            int count = context.DmChuDauTus.Where(ChuDauTu => ChuDauTu.TenChuDauTu.ToLower().Trim() == ten.ToLower().Trim() && ChuDauTu.Id != id).Count();
            return count > 0;
        }
        #endregion

        #region Get tỉnh huyện xã
        [Route("GetDanhSachTinhThanh")]
        [HttpGet]
        public ResponseMessage GetDanhSachTinhThanh()
        {
            try
            {
                List<DmTinhThanh> lstTinhThanh = context.DmTinhThanhs.ToList();
                message.IsError = false;
                message.ObjData = lstTinhThanh;
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

        [Route("GetDanhSachQuanHuyen/{idTinhThanh}")]
        [HttpGet]
        public ResponseMessage GetDanhSachQuanHuyen(string idTinhThanh)
        {
            try
            {
                string? tenTinhThanh = context.DmTinhThanhs.Where(tinhthanh => tinhthanh.Id == idTinhThanh).Select(tinhthanh => tinhthanh.TenTinhThanh).FirstOrDefault();
                var dmQuanHuyens = context.DmQuanHuyens.Where(quanhuyen => quanhuyen.DmTinhThanhId == idTinhThanh)
                                                    .Select(quanhuyen => new
                                                    {
                                                        quanhuyen.Id,
                                                        quanhuyen.TenQuanHuyen,
                                                    })
                                                    .ToList();
                message.IsError = false;
                message.ObjData = new { dmQuanHuyens, tenTinhThanh };
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

        [Route("GetDanhSachXaPhuong/{idQuanHuyen}")]
        [HttpGet]
        public ResponseMessage GetDanhSachXaPhuong(string idQuanHuyen)
        {
            try
            {
                string? tenQuanHuyen = context.DmQuanHuyens.Where(quanhuyen => quanhuyen.Id == idQuanHuyen).Select(quanhuyen => quanhuyen.TenQuanHuyen).FirstOrDefault();
                var dmXaPhuongs = context.DmXaPhuongs.Where(quanhuyen => quanhuyen.DmQuanHuyenId == idQuanHuyen)
                                                    .Select(quanhuyen => new
                                                    {
                                                        quanhuyen.Id,
                                                        quanhuyen.TenXaPhuong,
                                                    })
                                                    .ToList();
                message.IsError = false;
                message.ObjData = new { dmXaPhuongs, tenQuanHuyen };
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
        [Route("GetXaPhuong/{id}")]
        [HttpGet]
        public ResponseMessage GetXaPhuong(string id)
        {
            try
            {
                string? tenXaPhuong = context.DmXaPhuongs.Where(xaphuong => xaphuong.Id == id).Select(quanhuyen => quanhuyen.TenXaPhuong).FirstOrDefault();

                message.IsError = false;
                message.ObjData = tenXaPhuong;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra " + ex.Message;
            }
            return message;
        }
        #endregion

    }
}
