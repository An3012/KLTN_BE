using DTC_BE.CodeBase;
using DTC_BE.Models;
using DTC_BE.Models.Layout.DashBoard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DTC_BE.Controllers.Layout
{
    [Route("api/Layout/[controller]")]
    [ApiController]
    public class DashBoardController : BaseApiController
    {
        #region Tìm kiếm thông báo chủ đầu tư
        //[Route("GetThongTin")]
        //[HttpPost]
        //public ResponseMessage GetThongTin(TimKiemDashBoard timKiemDanhSach)
        //{
        //    try
        //    {
        //        int currentYear = DateTime.Now.Year;
        //        DashBoardListItem dashBoardListItem = new()
        //        {
        //            DashBoard_DuAnModel = new()
        //            {
        //                Count_DuAn = CurrencyFormat(context.DmDuAns.Where(duAn => duAn.TrangThai > 0).Count()),
        //                Count_DuAn_KhoiCong = CurrencyFormat(context.DmDuAns.Where(duAn => duAn.TrangThai == Enums.TienDoDuAn.ChuanBiDauTu.GetHashCode()).Count()),
        //                Count_DuAn_ChuyenTiep = CurrencyFormat(context.DmDuAns.Where(duAn => duAn.TrangThai == Enums.TienDoDuAn.ChuyenTiep.GetHashCode()).Count()),
        //            },
        //            DashBoard_TongMucDauTuModel = new()
        //            {
        //                Count_TongKeHoachVon = CurrencyFormat(context.DmDuAnNguonVons.Where(duAn => duAn.DmDuAn.TrangThai > 0
        //                                                                                         && duAn.DmDuAn.QddtNamBd != null
        //                                                                                         && duAn.DmDuAn.QddtNamBd.Value < currentYear
        //                                                                                         && duAn.DmDuAn.QddtNamKt != null
        //                                                                                         && duAn.DmDuAn.QddtNamKt.Value > currentYear
        //                                                                                         ).Sum(duAn => duAn.SoVon)),
        //                Count_TongGiaTriThucHien = "0",
        //                Count_TongGiaTriGiaiNgan = "0"
        //            },
        //            DashBoard_TyLeGiaiNganModel = new()
        //            {
        //                Count_DuAnCoTyLeGiaiNganThap = "0",
        //                Count_DuAnTreTienDo = "0",
        //                Count_DuAnChuaDuocPheDuyet = "0",
        //            },
        //            DashBoard_VanBanHuongDan = context.VbHuongDanLapKeHoachViews.Include(tb => tb.VbHuongDanLapKeHoach)
        //                                                                        .OrderByDescending(tb => tb.CreateAt)
        //                                                                        .AsEnumerable()
        //                                                                        .Where(tb => tb.DmChuDauTuId == timKiemDanhSach.IdDmChuDauTu && tb.CreateAt.GetValueOrDefault(DateTime.Now).Year == currentYear)
        //                                                                        .Select(tb => new DashBoard_VanBanHuongDan
        //                                                                        {
        //                                                                            Value = tb.Id,
        //                                                                            Text = tb.VbHuongDanLapKeHoach.TieuDe,
        //                                                                        })
        //                                                                        .Take(3)
        //                                                                        .ToList()
        //        };

        //        List<ThongKeChuDauTu> lstThongKeChuDauTu = context.DmChuDauTus.OrderBy(tb => tb.TenChuDauTu).Select(tb => new ThongKeChuDauTu
        //        {
        //            TenChuDauTu = tb.TenChuDauTu,
        //            Count_DuAn = CurrencyFormat(tb.DmDuAns.Where(da => da.TrangThai > 0).Count()),
        //            Count_DuAn_KhoiCongMoi = CurrencyFormat(tb.DmDuAns.Where(da => da.TrangThai  == Enums.TienDoDuAn.KhoiCongMoi.GetHashCode()).Count()),
        //            Count_DuAn_ChuyenTiep = CurrencyFormat(tb.DmDuAns.Where(da => da.TrangThai  == Enums.TienDoDuAn.ChuyenTiep.GetHashCode()).Count()),
        //            Count_DuAn_HoanThanh = CurrencyFormat(tb.DmDuAns.Where(da => da.TrangThai  == Enums.TienDoDuAn.HoanThanh.GetHashCode()).Count()),
        //        }).ToList();

        //        message.IsError = false;
        //        message.ObjData = new { dashBoardListItem, lstThongKeChuDauTu };
        //        message.Code = HttpStatusCode.OK.GetHashCode();
        //    }
        //    catch (Exception ex)
        //    {
        //        message.IsError = true;
        //        message.Code = HttpStatusCode.BadRequest.GetHashCode();
        //        message.Title = "Có lỗi xảy ra: " + ex.Message;
        //    }
        //    return message;
        //}
        #endregion
    }
}
