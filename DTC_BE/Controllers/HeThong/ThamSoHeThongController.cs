using DTC_BE.Entities;
using DTC_BE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using DTC_BE.Models.HeThong.ThamSoHeThong;
using DTC_BE.CodeBase;

namespace DTC_BE.Controllers.HeThong
{
    [Route("api/HeThong/[controller]")]
    [ApiController]
    public class ThamSoHeThongController : BaseApiController
    {
        #region Lấy danh sách tham số hệ thống
        [Route("GetThamSoHeThong")]
        [HttpGet]
        public ResponseMessage GetThamSoHeThong()
        {
            try
            {
                HtThamSoHeThong? objThamSoHeThong = context.HtThamSoHeThongs.FirstOrDefault() ?? new HtThamSoHeThong();

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = objThamSoHeThong;
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

        #region Cập nhật tham số hệ thống
        [Route("CapNhatThamSoHeThong")]
        [HttpPost]
        public ResponseMessage CapNhatThamSoHeThong(ThamSoModel objThamSoHeThong)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                HtThamSoHeThong? thamSoHeThong = context.HtThamSoHeThongs.FirstOrDefault();
                try
                {
                    bool isNew = false;
                    if (thamSoHeThong == null)
                    {
                        thamSoHeThong = new()
                        {
                            Id = "1"
                        };
                        isNew = true;
                    }
                    thamSoHeThong.ApDungMapExcel = objThamSoHeThong?.ApDungMapExcel?.Trim();
                    thamSoHeThong.TypeDocument = objThamSoHeThong?.TypeDocument?.Trim();
                    thamSoHeThong.ImagePath = objThamSoHeThong?.ImagePath?.Trim();
                    thamSoHeThong.SmtpServer = objThamSoHeThong?.SmtpServer?.Trim();
                    thamSoHeThong.DinhKyTuan = objThamSoHeThong?.DinhKyTuan?.Trim();

                    if (isNew)
                        context.HtThamSoHeThongs.Add(thamSoHeThong);

                    message.IsError = false;
                    message.Title = "Cập nhật tham số hệ thống thành công";
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    ThemMoiNhatKy("Cập nhật tham số hệ thống", Enums.LoaiChucNang.CapNhat.GetDescription(),
                                                               Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                               Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                               HangSo.Admin);

                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = "Có lỗi xảy ra: " + ex.Message;
                    trans.Rollback();
                    ThemMoiNhatKy("Cập nhật tham số hệ thống", Enums.LoaiChucNang.CapNhat.GetDescription(),
                                                               Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                               Enums.NhatKyHeThong_TrangThai.KhongThanhCong.GetDescription(),
                                                               HangSo.Admin);
                }
                finally
                {
                    trans.Dispose();
                }
            }

            return message;
        }
        #endregion
    }
}
