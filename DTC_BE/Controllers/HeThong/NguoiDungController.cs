using DTC_BE.CodeBase;
using DTC_BE.Entities;
using DTC_BE.Models;
using DTC_BE.Models.HeThong.NguoiDung;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using static DTC_BE.CodeBase.Enums;

namespace DTC_BE.Controllers.HeThong
{
    [Route("api/HeThong/[controller]")]
    [ApiController]
    public class NguoiDungController : BaseApiController
    {
        /// <summary>
        /// Lấy danh sách người dùng
        /// </summary>
        /// <param name="modelTimKiem"></param>
        /// <returns></returns>
        [Route("GetDanhSachNguoiDung")]
        [HttpPost]
        public ResponseMessage GetDanhSachNguoiDung(TimKiemNguoiDungModel modelTimKiem)
        {
            try
            {
                List<NguoiDungModel> lstNguoiDung = new List<NguoiDungModel>();
                List<SelectListItem> LstPhongban = new List<SelectListItem>();
                var lstNhomQuyen = context.HtNhomQuyens.AsNoTracking().ToList();

                foreach (Enums.PhongBan loaiTrangThai in (Enums.PhongBan[])Enum.GetValues(typeof(Enums.PhongBan)))
                {
                    LstPhongban.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                lstNguoiDung = context.HtNguoiDungs.AsNoTracking().AsEnumerable().Where(s => (string.IsNullOrEmpty(modelTimKiem.tuKhoa) || (!string.IsNullOrEmpty(modelTimKiem.tuKhoa) && (s.HoTen.ToLower().Trim().Contains(modelTimKiem.tuKhoa.ToLower().Trim()) || s.TenDangNhap.ToLower().Trim().Contains(modelTimKiem.tuKhoa.ToLower().Trim()))))
                ).Skip(modelTimKiem.rowPerPage * (modelTimKiem.currentPage - 1)).Take(modelTimKiem.rowPerPage).Select(s => new NguoiDungModel
                {
                    Id = s.Id,
                    HoTen = s.HoTen != null ? s.HoTen : "",
                    TenDangNhap = s.TenDangNhap != null ? s.TenDangNhap : "",
                    TrangThai = s.TrangThai,
                    NhomQuyen = s.HtNhomQuyenId != null ? (lstNhomQuyen.FirstOrDefault(x => x.Id == s.HtNhomQuyenId)?.Ten ?? "") : "",
                    PhongBan = s.PhongBan !=null? LstPhongban.FirstOrDefault(x => x.Value == s.PhongBan.ToString()).Text: "",
                    Email = s.Email,
                }).ToList();

                int TotalRecord = context.HtNguoiDungs.Where(s => (string.IsNullOrEmpty(modelTimKiem.tuKhoa) || (!string.IsNullOrEmpty(modelTimKiem.tuKhoa) && (s.HoTen.ToLower().Trim().Contains(modelTimKiem.tuKhoa.ToLower().Trim()) || s.TenDangNhap.ToLower().Trim().Contains(modelTimKiem.tuKhoa.ToLower().Trim()))))
                ).Count();

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = new { lstUser = lstNguoiDung, TotalReCord = TotalRecord };
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }


        /// <summary>
        /// Thêm mới tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ThemMoiTaiKhoan")]
        [HttpPost]
        public ResponseMessage ThemMoiTaiKhoan(ThemMoiNguoiDungModel model)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                if (string.IsNullOrEmpty(model.GioiTinh))
                {
                    model.GioiTinh = "M";
                }
                HtNguoiDung objNguoiDung = new HtNguoiDung();
                try
                {
                    objNguoiDung.Id = Guid.NewGuid().ToString();
                    objNguoiDung.HoTen = model?.HoTen;
                    objNguoiDung.MatKhau = Security.EncryptPassword(model?.MatKhau);
                    objNguoiDung.TenDangNhap = model?.TenDangNhap?.Trim();
                    objNguoiDung.GioiTinh = model?.GioiTinh;
                    objNguoiDung.DiaChi = model?.DiaChi?.Trim();
                    objNguoiDung.Email = model?.Email?.Trim();
                    objNguoiDung.SoDienThoai = model?.SoDienThoai?.Trim();
                    objNguoiDung.HtNhomQuyenId = model?.HtNhomQuyenId;
                    objNguoiDung.TrangThai = model?.TrangThai;
                    objNguoiDung.PhongBan = model?.PhongBan;
                    objNguoiDung.NgayTao = DateTime.Now;
                    objNguoiDung.NguoiTao = model?.NguoiTao;

                    if (CheckExist(objNguoiDung.TenDangNhap, null))
                        throw new Exception("Tên đăng nhập đã tồn tại");

                    context.HtNguoiDungs.Add(objNguoiDung);
                    context.SaveChanges();

                    message.Title = "Thêm mới tài khoản thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    ThemMoiNhatKy($"Thêm mới tài khoản: {model?.HoTen}", Enums.LoaiChucNang.ThemMoi.GetDescription(),
                                                                         Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                         Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                         model?.NguoiTao);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = "Có lỗi xảy ra: " + ex.Message;
                    ThemMoiNhatKy($"Thêm mới tài khoản: {model?.HoTen}", Enums.LoaiChucNang.ThemMoi.GetDescription(),
                                                                         Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                         Enums.NhatKyHeThong_TrangThai.KhongThanhCong.GetDescription(),
                                                                         model?.NguoiTao);
                }
            }
            return message;
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CapNhatTaiKhoan")]
        [HttpPost]
        public ResponseMessage CapNhatTaiKhoan(ThemMoiNguoiDungModel model)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    HtNguoiDung objNguoiDung = new HtNguoiDung();
                    objNguoiDung = context.HtNguoiDungs.Where(s => s.Id == model.Id).FirstOrDefault();
                    objNguoiDung.HoTen = model.HoTen;
                    objNguoiDung.TenDangNhap = model.TenDangNhap?.Trim();
                    objNguoiDung.GioiTinh = model.GioiTinh;
                    objNguoiDung.DiaChi = model.DiaChi?.Trim();
                    objNguoiDung.Email = model.Email?.Trim();
                    objNguoiDung.SoDienThoai = model.SoDienThoai?.Trim();
                    objNguoiDung.TrangThai = model.TrangThai;
                    objNguoiDung.NgayCapNhat = DateTime.Now;
                    objNguoiDung.MatKhau = objNguoiDung.MatKhau;
                    objNguoiDung.NguoiCapNhat = objNguoiDung.NguoiTao;
                    objNguoiDung.PhongBan = model?.PhongBan;
                    if (CheckExist(objNguoiDung.TenDangNhap, model.Id))
                        throw new Exception("Tên đăng nhập đã tồn tại");

                    context.SaveChanges();

                    /////Xóa nhóm quyền hiện tại
                    //var lstNhomQuyenHienTai = context.HtNguoiDungNhomQuyens.Where(s => s.HtNguoiDungId == objNguoiDung.Id).ToList();
                    //context.HtNguoiDungNhomQuyens.RemoveRange(lstNhomQuyenHienTai);
                    //context.SaveChanges();

                    /////Thêm nhiều quyền vào bảng người dùng nhóm quyền
                    //foreach (var item in model.HtNhomQuyenId)
                    //{
                    //    HtNguoiDungNhomQuyen objNguoiDungNhomQuyen = new HtNguoiDungNhomQuyen();
                    //    objNguoiDungNhomQuyen.Id = Guid.NewGuid().ToString();
                    //    objNguoiDungNhomQuyen.HtNguoiDungId = objNguoiDung.Id;
                    //    objNguoiDungNhomQuyen.HtNhomQuyenId = item;
                    //    context.HtNguoiDungNhomQuyens.Add(objNguoiDungNhomQuyen);
                    //}
                    context.SaveChanges();

                    message.Title = "Cập nhật tài khoản thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    ThemMoiNhatKy($"Cập nhật tài khoản: {model?.HoTen}", Enums.LoaiChucNang.CapNhat.GetDescription(),
                                                                         Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                         Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                         model?.NguoiTao);
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = "Có lỗi xảy ra: " + ex.Message;
                    ThemMoiNhatKy($"Cập nhật tài khoản: {model?.HoTen}", Enums.LoaiChucNang.CapNhat.GetDescription(),
                                                                         Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                         Enums.NhatKyHeThong_TrangThai.KhongThanhCong.GetDescription(),
                                                                         model?.NguoiTao);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        /// <summary>
        /// Lấy bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetTaiKhoanById")]
        [HttpGet]
        public ResponseMessage GetTaiKhoanById(string id)
        {
            try
            {
                var objNguoiDung = context.HtNguoiDungs.Where(s => s.Id == id).Select(s => new
                {
                    s.Id,
                    s.TenDangNhap,
                    s.HoTen,
                    s.TrangThai,
                    s.DiaChi,
                    s.GioiTinh,
                    s.SoDienThoai,
                    s.Email,
                    s.HtNhomQuyenId,
                    s.DmChuDauTuId,
                    s.DmDonViId,
                    s.MatKhau,
                    s.LoaiTaiKhoan
                }).FirstOrDefault();
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = objNguoiDung;
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;
        }

        [Route("XoaTaiKhoan")]
        [HttpPost]
        public ResponseMessage XoaTaiKhoan(string? idTaiKhoan)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                HtNguoiDung? linhVuc = context.HtNguoiDungs.FirstOrDefault(linhVuc => linhVuc.Id == idTaiKhoan);

                try
                {
                    if (linhVuc != null)
                    {
                        context.HtNguoiDungs.Remove(linhVuc);
                    }

                    message.Title = "Xóa thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    ThemMoiNhatKy($"Xóa tài khoản: {linhVuc?.HoTen}", Enums.LoaiChucNang.Xoa.GetDescription(),
                                                                      Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                      Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                      idTaiKhoan);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    ThemMoiNhatKy($"Xóa tài khoản: {linhVuc?.HoTen}", Enums.LoaiChucNang.Xoa.GetDescription(),
                                                                      Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                      Enums.NhatKyHeThong_TrangThai.KhongThanhCong.GetDescription(),
                                                                      idTaiKhoan);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        /// <summary>
        /// Lấy bản ghi trong Phong ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetChuDauTu")]
        [HttpGet]
        public ResponseMessage GetChuDauTu()
        {
            try
            {
                List<DmChuDauTu> lstChuDauTu = new List<DmChuDauTu>();

                lstChuDauTu = context.DmChuDauTus.ToList();
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = lstChuDauTu;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }

        /// <summary>
        /// Lấy bản ghi trong Phong ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetNhomQuyen")]
        [HttpGet]
        public ResponseMessage GetNhomQuyen()
        {
            try
            {
                List<HtNhomQuyen> lstNhomQuyen = new List<HtNhomQuyen>();

                lstNhomQuyen = context.HtNhomQuyens.ToList();
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = lstNhomQuyen;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }

        /// <summary>
        /// Check tồn tại khi thêm mới hoặc cập nhật bản ghi
        /// </summary>
        /// <param name="TenDangNhap"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        private bool CheckExist(string TenDangNhap, string Id)
        {
            if (context.HtNguoiDungs.Where(p => p.TenDangNhap.ToLower().Trim() == TenDangNhap.ToLower().Trim() && p.Id != Id).Count() > 0)
                return true;
            return false;
        }

        
    }
}
