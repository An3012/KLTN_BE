using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.LapBaoCaoTongHop;
using DTC_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static DTC_BE.CodeBase.Enums;
using DTC_BE.CodeBase;
using DTC_BE.Entities;
using DTC_BE.Models.ThuTucNBKeHoachLuaChonNhaThau.BaoCaoTongHopGoiThauDuocDuyet;
using NPOI.SS.Formula.Functions;

namespace DTC_BE.Controllers.ThuTucNoiBoVeLuaChonNhaThau
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaoCaoTHVeGoiThauDuocDuyetController : BaseApiController
    {
        [Route("GetDanhSachBaoCao")]
        [HttpPost]
        public ResponseMessage GetDanhSachBaoCao(TimKiemBaoCao timKiemBaoCao)
        {
            try
            {
                var lstBaoCaoTongHop = context.QuanLyThuTucNoiBoBaoCaoTongHops.AsNoTracking().AsEnumerable().Where(s => ((!string.IsNullOrEmpty(timKiemBaoCao.tenBaoCao) ?
                                                                           s.Ten.ToLower()
                                                                           .Trim().Contains(timKiemBaoCao.tenBaoCao.ToLower().Trim()) : true)
                                                                           &&
                                                                           (!string.IsNullOrEmpty(timKiemBaoCao.ngayTao) ?
                                                                           s.NgayTao.Value.ToString("dd/MM/yyyy")
                                                                           .Trim().Contains(timKiemBaoCao.ngayTao.ToLower().Trim()) : true)
                                                                           && (s.LoaiBaoCaoTongHop == (int)Enums.LoaiBaoCaoTongHopThuTucNoiBo.BaoCaoTongHoVeGoiThauDuocDuyet)
                                                                           ))
                                                       .OrderByDescending(s => s.NgayTao)
                                                       .Skip(timKiemBaoCao.rowPerPage * (timKiemBaoCao.currentPage - 1))
                                                       .Take(timKiemBaoCao.rowPerPage).Select(s => new LapBaoCaoTongHop
                                                       {
                                                           id = s.Id,
                                                           Ten = s.Ten,
                                                           Nam = s.Nam,
                                                           NgayTao = s.NgayTao != null ? s.NgayTao.Value.ToString("dd/MM/yyyy") : "",
                                                       }).ToList();
                int totalRecords = context.QuanLyThuTucNoiBoBaoCaoTongHops.AsEnumerable().Where(s => ((!string.IsNullOrEmpty(timKiemBaoCao.tenBaoCao) ?
                                                                           s.Ten.ToLower()
                                                                           .Trim().Contains(timKiemBaoCao.tenBaoCao.ToLower().Trim()) : true)
                                                                           && (!string.IsNullOrEmpty(timKiemBaoCao.ngayTao) ?
                                                                           s.NgayTao.Value.ToString("dd/MM/yyyy")
                                                                           .Trim().Contains(timKiemBaoCao.ngayTao.ToLower().Trim()) : true)
                                                                           && (s.LoaiBaoCaoTongHop == (int)Enums.LoaiBaoCaoTongHopThuTucNoiBo.BaoCaoTongHoVeGoiThauDuocDuyet)
                                                                           )).Count();
                message.IsError = false;
                message.ObjData = new { lstBaoCaoTongHop, totalRecords };
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

        [Route("ThemMoiLapBaoCaoVeGoiThau")]
        [HttpPost]
        public ResponseMessage ThemMoiLapBaoCaoVeGoiThau(LapBaoCaoTongHopLCNT objBaoCaoTongHop)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    int nguonVon = int.Parse(objBaoCaoTongHop.NguonVon);
                    string dt = DateTime.Now.ToString("ddMMyyhhmmss");
                    QuanLyThuTucNoiBoBaoCaoTongHop BaoCaoTongHop = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Ten = objBaoCaoTongHop?.Ten?.Trim(),
                        LoaiBaoCaoTongHop = objBaoCaoTongHop?.LoaiBaoCaoTongHop,
                        Nam = objBaoCaoTongHop?.Nam,
                        NgayTao = DateTime.Now,
                        NguoiTao = objBaoCaoTongHop?.NguoiTao,
                    };
                    string originalName = BaoCaoTongHop.Ten;
                    string newName = originalName;
                    int version = 1;
                    while (context.QuanLyThuTucNoiBoBaoCaoTongHops.Any(x => x.Ten.ToLower() == newName.ToLower()))
                    {
                        newName = $"{originalName}_v{version.ToString("D2")}";
                        version++;
                    }

                    BaoCaoTongHop.Ten = newName;
                    context.QuanLyThuTucNoiBoBaoCaoTongHops.Add(BaoCaoTongHop);

                    List<SelectListItem> lstLinhVucVaHinhThuc = new List<SelectListItem>();

                    foreach (Enums.LinhVucVaHinhThucBaoCaoTongHop loaithutucnoibo in (Enums.LinhVucVaHinhThucBaoCaoTongHop[])Enum.GetValues(typeof(Enums.LinhVucVaHinhThucBaoCaoTongHop)))
                    {
                        lstLinhVucVaHinhThuc.Add(new SelectListItem { Text = loaithutucnoibo.GetDescription(), Value = loaithutucnoibo.GetHashCode().ToString() });
                    }
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
                    var lstThuTucLuaChonNhaThau = context.QuanLyThuTucNoiBoDuAnDtcs.Where(x => x.LoaiHoSo == 11 || x.LoaiHoSo == 10).ToList();
                    var QuanLyThuTucCnkqs = context.QuanLyThuTucCnkqs.ToList();
                    List<QuanLyThuTucNoiBoDuAnDtc> lstThuTuc = new List<QuanLyThuTucNoiBoDuAnDtc>();
                    lstThuTucLuaChonNhaThau.Where(item => item.QuanLyThuTucCnkqs
                                            .Any(x => x.IdThuTuc.Trim() == item.Id.Trim() && x.NamPheDuyet.Trim() == BaoCaoTongHop.Nam.Trim()))
                                        .ToList()
                                        .ForEach(item => lstThuTuc.Add(item));

                    List<ChiTietGoiThauDuocDuyet> lstAllGoiThauDuocDuyet = new List<ChiTietGoiThauDuocDuyet>();
                    foreach (var item in lstThuTuc)
                    {
                        var lstGoiThauDuocDuyet = context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Where(x => x.IdThuTuc == item.Id && x.NguonVon == nguonVon)
                                                      .Select(s => new ChiTietGoiThauDuocDuyet
                                                      {
                                                          Id = s.Id,
                                                          GiaGoiThau = s.GiaGoiThau,
                                                          GiaTrungThau = s.GiaTrungThau,
                                                          HinhThucDauThau = s.HinhThucDauThau,
                                                          HinhThucLuaChonNhaThau = s.HinhThucLuaChonNhaThau,
                                                          LinhVuc = s.LinhVuc,
                                                          NhomDuAn = item.NhomDuAn.ToString(),
                                                          NguonVon = s.NguonVon,
                                                      }).ToList();
                        lstAllGoiThauDuocDuyet.AddRange(lstGoiThauDuocDuyet);
                    }
                    List<QuanLyThuTucBaoCaoTongHopVeGoiThau> lstChiTietBaoCaoGoiThauTheoLinhVuc = new List<QuanLyThuTucBaoCaoTongHopVeGoiThau>();
                    List<QuanLyThuTucBaoCaoTongHopVeGoiThau> baoCaoTongHopVeGoiThaus = new List<QuanLyThuTucBaoCaoTongHopVeGoiThau>();

                    int i = 0;

                    foreach (var item in lstLinhVuc)
                    {
                        double TongSoGoiThauDoQuocHoiChuTruongDauTu = 0.0;
                        double TongGiaGoiThauDoQuocHoiChuTruongDauTu = 0.0;
                        double TongGiaTrungThauDoQuocHoiChuTruongDauTu = 0.0;
                        double TongSoGoiThauDuAnNhomA = 0.0;
                        double TongGiaGoiThauDuAnNhomA = 0.0;
                        double TongGiaTrungThauDuAnNhomA = 0.0;
                        double TongSoGoiThauDuAnNhomB = 0.0;
                        double TongGiaGoiThauDuAnNhomB = 0.0;
                        double TongGiaTrungThauDuAnNhomB = 0.0;
                        double TongSoGoiThauDuAnNhomC = 0.0;
                        double TongGiaGoiThauDuAnNhomC = 0.0;
                        double TongGiaTrungThauDuAnNhomC = 0.0;
                        double TongSoGoiThauDoQuocHoiChuTruongDauTuKQM = 0.0;
                        double TongGiaGoiThauDoQuocHoiChuTruongDauTuKQM = 0.0;
                        double TongGiaTrungThauDoQuocHoiChuTruongDauTuKQM = 0.0;
                        double TongSoGoiThauDuAnNhomAKQM = 0.0;
                        double TongGiaGoiThauDuAnNhomAKQM = 0.0;
                        double TongGiaTrungThauDuAnNhomAKQM = 0.0;
                        double TongSoGoiThauDuAnNhomBKQM = 0.0;
                        double TongGiaGoiThauDuAnNhomBKQM = 0.0;
                        double TongGiaTrungThauDuAnNhomBKQM = 0.0;
                        double TongSoGoiThauDuAnNhomCKQM = 0.0;
                        double TongGiaGoiThauDuAnNhomCKQM = 0.0;
                        double TongGiaTrungThauDuAnNhomCKQM = 0.0;

                        foreach (var item1 in lstAllGoiThauDuocDuyet)
                        {
                            if (item1.LinhVuc.ToString() == item.Value)
                            {
                                switch (int.Parse(item1.NhomDuAn))
                                {
                                    case var x when x == (int)Enums.NhomDuAn.DuAnQuanTrongQuocGiaDoQuocHoiChuTruongDauTu:
                                        if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang)
                                        {
                                            TongSoGoiThauDoQuocHoiChuTruongDauTuKQM += 1;
                                            TongGiaGoiThauDoQuocHoiChuTruongDauTuKQM += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDoQuocHoiChuTruongDauTuKQM += item1.GiaTrungThau ?? 0.0;
                                        }
                                        else if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang)
                                        {
                                            TongSoGoiThauDoQuocHoiChuTruongDauTu += 1;
                                            TongGiaGoiThauDoQuocHoiChuTruongDauTu += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDoQuocHoiChuTruongDauTu += item1.GiaTrungThau ?? 0.0;
                                        }
                                        break;

                                    case var x when x == (int)Enums.NhomDuAn.DuAnNhomA:
                                        if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomAKQM += 1;
                                            TongGiaGoiThauDuAnNhomAKQM += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomAKQM += item1.GiaTrungThau ?? 0.0;
                                        }
                                        else if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomA += 1;
                                            TongGiaGoiThauDuAnNhomA += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomA += item1.GiaTrungThau ?? 0.0;
                                        }
                                        break;

                                    case var x when x == (int)Enums.NhomDuAn.DuAnNhomB:
                                        if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomBKQM += 1;
                                            TongGiaGoiThauDuAnNhomBKQM += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomBKQM += item1.GiaTrungThau ?? 0.0;
                                        }
                                        else if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomB += 1;
                                            TongGiaGoiThauDuAnNhomB += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomB += item1.GiaTrungThau ?? 0.0;
                                        }
                                        break;

                                    case var x when x == (int)Enums.NhomDuAn.DuAnNhomC:
                                        if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomCKQM += 1;
                                            TongGiaGoiThauDuAnNhomCKQM += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomCKQM += item1.GiaTrungThau ?? 0.0;
                                        }
                                        else if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomC += 1;
                                            TongGiaGoiThauDuAnNhomC += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomC += item1.GiaTrungThau ?? 0.0;
                                        }
                                        break;
                                }

                            }
                        }


                        lstChiTietBaoCaoGoiThauTheoLinhVuc.Add(new QuanLyThuTucBaoCaoTongHopVeGoiThau
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdBaoCao = BaoCaoTongHop.Id,
                            LinhVucVaHinhThuc = i + 1,
                            NguonVon = nguonVon,
                            HinhThucDauThau = (int)Enums.HinhThucDauThau.KhongQuaMang,
                            TongSoGoiThauDoQuocHoiChuTruongDauTu = TongSoGoiThauDoQuocHoiChuTruongDauTuKQM,
                            TongGiaGoiThauDoQuocHoiChuTruongDauTu = TongGiaGoiThauDoQuocHoiChuTruongDauTuKQM,
                            TongGiaTrungThauDoQuocHoiChuTruongDauTu = TongGiaTrungThauDoQuocHoiChuTruongDauTuKQM,
                            ChenhLechDoQuocHoiChuTruongDauTu = TongGiaTrungThauDoQuocHoiChuTruongDauTuKQM - TongGiaGoiThauDoQuocHoiChuTruongDauTuKQM,
                            TongSoGoiThauDuAnNhomA = TongSoGoiThauDuAnNhomAKQM,
                            TongGiaGoiThauDuAnNhomA = TongGiaGoiThauDuAnNhomAKQM,
                            TongGiaTrungThauDuAnNhomA = TongGiaTrungThauDuAnNhomAKQM,
                            ChenhLechDuAnNhomA = TongGiaTrungThauDuAnNhomAKQM - TongGiaGoiThauDuAnNhomAKQM,
                            TongSoGoiThauDuAnNhomB = TongSoGoiThauDuAnNhomBKQM,
                            TongGiaGoiThauDuAnNhomB = TongGiaGoiThauDuAnNhomBKQM,
                            TongGiaTrungThauDuAnNhomB = TongGiaTrungThauDuAnNhomBKQM,
                            ChenhLechDuAnNhomB = TongGiaTrungThauDuAnNhomBKQM - TongGiaGoiThauDuAnNhomBKQM,
                            TongSoGoiThauDuAnNhomC = TongSoGoiThauDuAnNhomCKQM,
                            TongGiaGoiThauDuAnNhomC = TongGiaGoiThauDuAnNhomCKQM,
                            TongGiaTrungThauDuAnNhomC = TongGiaTrungThauDuAnNhomCKQM,
                            ChenhLechDuAnNhomC = TongGiaTrungThauDuAnNhomCKQM - TongGiaGoiThauDuAnNhomCKQM,
                            TongSoGoiThauTongCong = TongSoGoiThauDoQuocHoiChuTruongDauTuKQM + TongSoGoiThauDuAnNhomAKQM + TongSoGoiThauDuAnNhomBKQM + TongSoGoiThauDuAnNhomCKQM,
                            TongGiaGoiThauTongCong = TongGiaGoiThauDoQuocHoiChuTruongDauTuKQM + TongGiaGoiThauDuAnNhomAKQM + TongGiaGoiThauDuAnNhomBKQM + TongGiaGoiThauDuAnNhomCKQM,
                            TongGiaTrungThauTongCong = TongGiaTrungThauDoQuocHoiChuTruongDauTuKQM + TongGiaTrungThauDuAnNhomAKQM + TongGiaTrungThauDuAnNhomBKQM + TongGiaTrungThauDuAnNhomCKQM,
                            ChenhLechTongCong = (TongGiaTrungThauDoQuocHoiChuTruongDauTuKQM + TongGiaTrungThauDuAnNhomAKQM + TongGiaTrungThauDuAnNhomBKQM + TongGiaTrungThauDuAnNhomCKQM) - (TongGiaGoiThauDoQuocHoiChuTruongDauTuKQM + TongGiaGoiThauDuAnNhomAKQM + TongGiaGoiThauDuAnNhomBKQM + TongGiaGoiThauDuAnNhomCKQM),
                        });
                        lstChiTietBaoCaoGoiThauTheoLinhVuc.Add(new QuanLyThuTucBaoCaoTongHopVeGoiThau
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdBaoCao = BaoCaoTongHop.Id,
                            LinhVucVaHinhThuc = i + 1,
                            NguonVon = nguonVon,
                            HinhThucDauThau = (int)Enums.HinhThucDauThau.QuaMang,
                            TongSoGoiThauDoQuocHoiChuTruongDauTu = TongSoGoiThauDoQuocHoiChuTruongDauTu,
                            TongGiaGoiThauDoQuocHoiChuTruongDauTu = TongGiaGoiThauDoQuocHoiChuTruongDauTu,
                            TongGiaTrungThauDoQuocHoiChuTruongDauTu = TongGiaTrungThauDoQuocHoiChuTruongDauTu,
                            ChenhLechDoQuocHoiChuTruongDauTu = TongGiaTrungThauDoQuocHoiChuTruongDauTu - TongGiaGoiThauDoQuocHoiChuTruongDauTu,
                            TongSoGoiThauDuAnNhomA = TongSoGoiThauDuAnNhomA,
                            TongGiaGoiThauDuAnNhomA = TongGiaGoiThauDuAnNhomA,
                            TongGiaTrungThauDuAnNhomA = TongGiaTrungThauDuAnNhomA,
                            ChenhLechDuAnNhomA = TongGiaTrungThauDuAnNhomA - TongGiaGoiThauDuAnNhomA,
                            TongSoGoiThauDuAnNhomB = TongSoGoiThauDuAnNhomB,
                            TongGiaGoiThauDuAnNhomB = TongGiaGoiThauDuAnNhomB,
                            TongGiaTrungThauDuAnNhomB = TongGiaTrungThauDuAnNhomB,
                            ChenhLechDuAnNhomB = TongGiaTrungThauDuAnNhomB - TongGiaGoiThauDuAnNhomB,
                            TongSoGoiThauDuAnNhomC = TongSoGoiThauDuAnNhomC,
                            TongGiaGoiThauDuAnNhomC = TongGiaGoiThauDuAnNhomC,
                            TongGiaTrungThauDuAnNhomC = TongGiaTrungThauDuAnNhomC,
                            ChenhLechDuAnNhomC = TongGiaTrungThauDuAnNhomC - TongGiaGoiThauDuAnNhomC,
                            TongSoGoiThauTongCong = TongSoGoiThauDoQuocHoiChuTruongDauTu + TongSoGoiThauDuAnNhomA + TongSoGoiThauDuAnNhomB + TongSoGoiThauDuAnNhomC,
                            TongGiaGoiThauTongCong = TongGiaGoiThauDoQuocHoiChuTruongDauTu + TongGiaGoiThauDuAnNhomA + TongGiaGoiThauDuAnNhomB + TongGiaGoiThauDuAnNhomC,
                            TongGiaTrungThauTongCong = TongGiaTrungThauDoQuocHoiChuTruongDauTu + TongGiaTrungThauDuAnNhomA + TongGiaTrungThauDuAnNhomB + TongGiaTrungThauDuAnNhomC,
                            ChenhLechTongCong = (TongGiaTrungThauDoQuocHoiChuTruongDauTu + TongGiaTrungThauDuAnNhomA + TongGiaTrungThauDuAnNhomB + TongGiaTrungThauDuAnNhomC) - (TongGiaGoiThauDoQuocHoiChuTruongDauTu + TongGiaGoiThauDuAnNhomA + TongGiaGoiThauDuAnNhomB + TongGiaGoiThauDuAnNhomC),
                        });
                        i++;
                    }

                    var BaoCaoTongHopVeGoiThauVeLinhVucTongCongKQM = new QuanLyThuTucBaoCaoTongHopVeGoiThau
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdBaoCao = BaoCaoTongHop.Id,
                        LinhVucVaHinhThuc = (int)Enums.LinhVucVaHinhThucBaoCaoTongHop.TongCongKQM_1,
                        NguonVon = nguonVon,
                        HinhThucDauThau = 0,
                        TongSoGoiThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauDoQuocHoiChuTruongDauTu)),
                        TongGiaGoiThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauDoQuocHoiChuTruongDauTu)),
                        TongGiaTrungThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauDoQuocHoiChuTruongDauTu)),
                        ChenhLechDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechDoQuocHoiChuTruongDauTu)),
                        TongSoGoiThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauDuAnNhomA)),
                        TongGiaGoiThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauDuAnNhomA)),
                        TongGiaTrungThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauDuAnNhomA)),
                        ChenhLechDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechDuAnNhomA)),
                        TongSoGoiThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauDuAnNhomB)),
                        TongGiaGoiThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauDuAnNhomB)),
                        TongGiaTrungThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauDuAnNhomB)),
                        ChenhLechDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechDuAnNhomB)),
                        TongSoGoiThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauDuAnNhomC)),
                        TongGiaGoiThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauDuAnNhomC)),
                        TongGiaTrungThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauDuAnNhomC)),
                        ChenhLechDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechDuAnNhomC)),
                        TongSoGoiThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauTongCong)),
                        TongGiaGoiThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauTongCong)),
                        TongGiaTrungThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauTongCong)),
                        ChenhLechTongCong = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechTongCong)),
                    };

                    var BaoCaoTongHopVeGoiThauVeLinhVucTongCongQM = new QuanLyThuTucBaoCaoTongHopVeGoiThau
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdBaoCao = BaoCaoTongHop.Id,
                        LinhVucVaHinhThuc = (int)Enums.LinhVucVaHinhThucBaoCaoTongHop.TongCongQM_1,
                        NguonVon = nguonVon,
                        HinhThucDauThau = 0,
                        TongSoGoiThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauDoQuocHoiChuTruongDauTu)),
                        TongGiaGoiThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauDoQuocHoiChuTruongDauTu)),
                        TongGiaTrungThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauDoQuocHoiChuTruongDauTu)),
                        ChenhLechDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechDoQuocHoiChuTruongDauTu)),
                        TongSoGoiThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauDuAnNhomA)),
                        TongGiaGoiThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauDuAnNhomA)),
                        TongGiaTrungThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauDuAnNhomA)),
                        ChenhLechDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechDuAnNhomA)),
                        TongSoGoiThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauDuAnNhomB)),
                        TongGiaGoiThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauDuAnNhomB)),
                        TongGiaTrungThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauDuAnNhomB)),
                        ChenhLechDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechDuAnNhomB)),
                        TongSoGoiThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauDuAnNhomC)),
                        TongGiaGoiThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauDuAnNhomC)),
                        TongGiaTrungThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauDuAnNhomC)),
                        ChenhLechDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechDuAnNhomC)),
                        TongSoGoiThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauTongCong)),
                        TongGiaGoiThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauTongCong)),
                        TongGiaTrungThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauTongCong)),
                        ChenhLechTongCong = Sum(lstChiTietBaoCaoGoiThauTheoLinhVuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechTongCong)),
                    };

                    baoCaoTongHopVeGoiThaus.Add(BaoCaoTongHopVeGoiThauVeLinhVucTongCongQM);
                    baoCaoTongHopVeGoiThaus.Add(BaoCaoTongHopVeGoiThauVeLinhVucTongCongKQM);
                    baoCaoTongHopVeGoiThaus.AddRange(lstChiTietBaoCaoGoiThauTheoLinhVuc);

                    int EnumLvHT = 10;

                    List<QuanLyThuTucBaoCaoTongHopVeGoiThau> lstChiTietBaoCaoGoiThauTheoHinhThuc = new List<QuanLyThuTucBaoCaoTongHopVeGoiThau>();

                    foreach (var item in lstHinhThucLuaChonNhaThau)
                    {


                        double TongSoGoiThauDoQuocHoiChuTruongDauTu_Kqm = 0.0;
                        double TongGiaGoiThauDoQuocHoiChuTruongDauTu_Kqm = 0.0;
                        double TongGiaTrungThauDoQuocHoiChuTruongDauTu_Kqm = 0.0;
                        double TongSoGoiThauDuAnNhomA_Kqm = 0.0;
                        double TongGiaGoiThauDuAnNhomA_Kqm = 0.0;
                        double TongGiaTrungThauDuAnNhomA_Kqm = 0.0;
                        double TongSoGoiThauDuAnNhomB_Kqm = 0.0;
                        double TongGiaGoiThauDuAnNhomB_Kqm = 0.0;
                        double TongGiaTrungThauDuAnNhomB_Kqm = 0.0;
                        double TongSoGoiThauDuAnNhomC_Kqm = 0.0;
                        double TongGiaGoiThauDuAnNhomC_Kqm = 0.0;
                        double TongGiaTrungThauDuAnNhomC_Kqm = 0.0;


                        double TongSoGoiThauDoQuocHoiChuTruongDauTu_Qm = 0.0;
                        double TongGiaGoiThauDoQuocHoiChuTruongDauTu_Qm = 0.0;
                        double TongGiaTrungThauDoQuocHoiChuTruongDauTu_Qm = 0.0;
                        double TongSoGoiThauDuAnNhomA_Qm = 0.0;
                        double TongGiaGoiThauDuAnNhomA_Qm = 0.0;
                        double TongGiaTrungThauDuAnNhomA_Qm = 0.0;
                        double TongSoGoiThauDuAnNhomB_Qm = 0.0;
                        double TongGiaGoiThauDuAnNhomB_Qm = 0.0;
                        double TongGiaTrungThauDuAnNhomB_Qm = 0.0;
                        double TongSoGoiThauDuAnNhomC_Qm = 0.0;
                        double TongGiaGoiThauDuAnNhomC_Qm = 0.0;
                        double TongGiaTrungThauDuAnNhomC_Qm = 0.0;

                        foreach (var item1 in lstAllGoiThauDuocDuyet)
                        {
                            if (item1.HinhThucLuaChonNhaThau.ToString() == item.Value)
                            {
                                switch (int.Parse(item1.NhomDuAn))
                                {
                                    case var x when x == (int)Enums.NhomDuAnHangNam.DuAnQuanTrongQuocGiaDoQuocHoiChuTruongDauTu:

                                        if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang)
                                        {
                                            TongSoGoiThauDoQuocHoiChuTruongDauTu_Kqm += 1;
                                            TongGiaGoiThauDoQuocHoiChuTruongDauTu_Kqm += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDoQuocHoiChuTruongDauTu_Kqm += item1.GiaTrungThau ?? 0.0;
                                        }
                                        else if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang)
                                        {
                                            TongSoGoiThauDoQuocHoiChuTruongDauTu_Qm += 1;
                                            TongGiaGoiThauDoQuocHoiChuTruongDauTu_Qm += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDoQuocHoiChuTruongDauTu_Qm += item1.GiaTrungThau ?? 0.0;
                                        }
                                        break;
                                    case var x when x == (int)Enums.NhomDuAnHangNam.NhomA:

                                        if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomA_Kqm += 1;
                                            TongGiaGoiThauDuAnNhomA_Kqm += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomA_Kqm += item1.GiaTrungThau ?? 0.0;
                                        }
                                        else if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomA_Qm += 1;
                                            TongGiaGoiThauDuAnNhomA_Qm += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomA_Qm += item1.GiaTrungThau ?? 0.0;
                                        }
                                        break;

                                    case var x when x == (int)Enums.NhomDuAnHangNam.NhomB:

                                        if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomB_Kqm += 1;
                                            TongGiaGoiThauDuAnNhomB_Kqm += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomB_Kqm += item1.GiaTrungThau ?? 0.0;
                                        }
                                        else if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomB_Qm += 1;
                                            TongGiaGoiThauDuAnNhomB_Qm += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomB_Qm += item1.GiaTrungThau ?? 0.0;
                                        }
                                        break;

                                    case var x when x == (int)Enums.NhomDuAnHangNam.NhomC:

                                        if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomC_Kqm += 1;
                                            TongGiaGoiThauDuAnNhomC_Kqm += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomC_Kqm += item1.GiaTrungThau ?? 0.0;
                                        }
                                        else if (item1.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang)
                                        {
                                            TongSoGoiThauDuAnNhomC_Qm += 1;
                                            TongGiaGoiThauDuAnNhomC_Qm += item1.GiaGoiThau ?? 0.0;
                                            TongGiaTrungThauDuAnNhomC_Qm += item1.GiaTrungThau ?? 0.0;
                                        }
                                        break;
                                }
                            }
                            
                        }
                        lstChiTietBaoCaoGoiThauTheoHinhThuc.Add(new QuanLyThuTucBaoCaoTongHopVeGoiThau
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdBaoCao = BaoCaoTongHop.Id,
                            LinhVucVaHinhThuc = EnumLvHT + 1,
                            HinhThucDauThau = (int)Enums.HinhThucDauThau.QuaMang,
                            NguonVon = nguonVon,
                            TongSoGoiThauDoQuocHoiChuTruongDauTu = TongSoGoiThauDoQuocHoiChuTruongDauTu_Kqm,
                            TongGiaGoiThauDoQuocHoiChuTruongDauTu = TongGiaGoiThauDoQuocHoiChuTruongDauTu_Kqm,
                            TongGiaTrungThauDoQuocHoiChuTruongDauTu = TongGiaTrungThauDoQuocHoiChuTruongDauTu_Kqm,
                            ChenhLechDoQuocHoiChuTruongDauTu = TongGiaTrungThauDoQuocHoiChuTruongDauTu_Kqm - TongGiaGoiThauDoQuocHoiChuTruongDauTu_Kqm,
                            TongSoGoiThauDuAnNhomA = TongSoGoiThauDuAnNhomA_Kqm,
                            TongGiaGoiThauDuAnNhomA = TongGiaGoiThauDuAnNhomA_Kqm,
                            TongGiaTrungThauDuAnNhomA = TongGiaTrungThauDuAnNhomA_Kqm,
                            ChenhLechDuAnNhomA = TongGiaTrungThauDuAnNhomA_Kqm - TongGiaGoiThauDuAnNhomA_Kqm,
                            TongSoGoiThauDuAnNhomB = TongSoGoiThauDuAnNhomB_Kqm,
                            TongGiaGoiThauDuAnNhomB = TongGiaGoiThauDuAnNhomB_Kqm,
                            TongGiaTrungThauDuAnNhomB = TongGiaTrungThauDuAnNhomB_Kqm,
                            ChenhLechDuAnNhomB = TongGiaTrungThauDuAnNhomB_Kqm - TongGiaGoiThauDuAnNhomB_Kqm,
                            TongSoGoiThauDuAnNhomC = TongSoGoiThauDuAnNhomC_Kqm,
                            TongGiaGoiThauDuAnNhomC = TongGiaGoiThauDuAnNhomC_Kqm,
                            TongGiaTrungThauDuAnNhomC = TongGiaTrungThauDuAnNhomC_Kqm,
                            ChenhLechDuAnNhomC = TongGiaTrungThauDuAnNhomC_Kqm - TongGiaGoiThauDuAnNhomC_Kqm,
                            TongSoGoiThauTongCong = TongSoGoiThauDoQuocHoiChuTruongDauTu_Kqm + TongSoGoiThauDuAnNhomA_Kqm + TongSoGoiThauDuAnNhomB_Kqm + TongSoGoiThauDuAnNhomC_Kqm,
                            TongGiaGoiThauTongCong = TongGiaGoiThauDoQuocHoiChuTruongDauTu_Kqm + TongGiaGoiThauDuAnNhomA_Kqm + TongGiaGoiThauDuAnNhomB_Kqm + TongGiaGoiThauDuAnNhomC_Kqm,
                            TongGiaTrungThauTongCong = TongGiaTrungThauDoQuocHoiChuTruongDauTu_Kqm + TongGiaTrungThauDuAnNhomA_Kqm + TongGiaTrungThauDuAnNhomB_Kqm + TongGiaTrungThauDuAnNhomC_Kqm,
                            ChenhLechTongCong = (TongGiaTrungThauDoQuocHoiChuTruongDauTu_Kqm + TongGiaTrungThauDuAnNhomA_Kqm + TongGiaTrungThauDuAnNhomB_Kqm + TongGiaTrungThauDuAnNhomC_Kqm) - (TongGiaGoiThauDoQuocHoiChuTruongDauTu_Kqm + TongGiaGoiThauDuAnNhomA_Kqm + TongGiaGoiThauDuAnNhomB_Kqm + TongGiaGoiThauDuAnNhomC_Kqm),
                        });
                        lstChiTietBaoCaoGoiThauTheoHinhThuc.Add(new QuanLyThuTucBaoCaoTongHopVeGoiThau
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdBaoCao = BaoCaoTongHop.Id,
                            LinhVucVaHinhThuc = EnumLvHT + 1,
                            HinhThucDauThau = (int)Enums.HinhThucDauThau.KhongQuaMang,
                            NguonVon = nguonVon,
                            TongSoGoiThauDoQuocHoiChuTruongDauTu = TongSoGoiThauDoQuocHoiChuTruongDauTu_Qm,
                            TongGiaGoiThauDoQuocHoiChuTruongDauTu = TongGiaGoiThauDoQuocHoiChuTruongDauTu_Qm,
                            TongGiaTrungThauDoQuocHoiChuTruongDauTu = TongGiaTrungThauDoQuocHoiChuTruongDauTu_Qm,
                            ChenhLechDoQuocHoiChuTruongDauTu = TongGiaTrungThauDoQuocHoiChuTruongDauTu_Qm - TongGiaGoiThauDoQuocHoiChuTruongDauTu_Qm,
                            TongSoGoiThauDuAnNhomA = TongSoGoiThauDuAnNhomA_Qm,
                            TongGiaGoiThauDuAnNhomA = TongGiaGoiThauDuAnNhomA_Qm,
                            TongGiaTrungThauDuAnNhomA = TongGiaTrungThauDuAnNhomA_Qm,
                            ChenhLechDuAnNhomA = TongGiaTrungThauDuAnNhomA_Qm - TongGiaGoiThauDuAnNhomA_Qm,
                            TongSoGoiThauDuAnNhomB = TongSoGoiThauDuAnNhomB_Qm,
                            TongGiaGoiThauDuAnNhomB = TongGiaGoiThauDuAnNhomB_Qm,
                            TongGiaTrungThauDuAnNhomB = TongGiaTrungThauDuAnNhomB_Qm,
                            ChenhLechDuAnNhomB = TongGiaTrungThauDuAnNhomB_Qm - TongGiaGoiThauDuAnNhomB_Qm,
                            TongSoGoiThauDuAnNhomC = TongSoGoiThauDuAnNhomC_Qm,
                            TongGiaGoiThauDuAnNhomC = TongGiaGoiThauDuAnNhomC_Qm,
                            TongGiaTrungThauDuAnNhomC = TongGiaTrungThauDuAnNhomC_Qm,
                            ChenhLechDuAnNhomC = TongGiaTrungThauDuAnNhomC_Qm - TongGiaGoiThauDuAnNhomC_Qm,
                            TongSoGoiThauTongCong = TongSoGoiThauDoQuocHoiChuTruongDauTu_Qm + TongSoGoiThauDuAnNhomA_Qm + TongSoGoiThauDuAnNhomB_Qm + TongSoGoiThauDuAnNhomC_Qm,
                            TongGiaGoiThauTongCong = TongGiaGoiThauDoQuocHoiChuTruongDauTu_Qm + TongGiaGoiThauDuAnNhomA_Qm + TongGiaGoiThauDuAnNhomB_Qm + TongGiaGoiThauDuAnNhomC_Qm,
                            TongGiaTrungThauTongCong = TongGiaTrungThauDoQuocHoiChuTruongDauTu_Qm + TongGiaTrungThauDuAnNhomA_Qm + TongGiaTrungThauDuAnNhomB_Qm + TongGiaTrungThauDuAnNhomC_Qm,
                            ChenhLechTongCong = (TongGiaTrungThauDoQuocHoiChuTruongDauTu_Qm + TongGiaTrungThauDuAnNhomA_Qm + TongGiaTrungThauDuAnNhomB_Qm + TongGiaTrungThauDuAnNhomC_Qm) - (TongGiaGoiThauDoQuocHoiChuTruongDauTu_Qm + TongGiaGoiThauDuAnNhomA_Qm + TongGiaGoiThauDuAnNhomB_Qm + TongGiaGoiThauDuAnNhomC_Qm),
                        });
                        EnumLvHT++;

                    }

                    baoCaoTongHopVeGoiThaus.AddRange(lstChiTietBaoCaoGoiThauTheoHinhThuc);

                    var BaoCaoTongHopVeGoiThauVeHinhThucTongCongKQM = new QuanLyThuTucBaoCaoTongHopVeGoiThau
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdBaoCao = BaoCaoTongHop.Id,
                        LinhVucVaHinhThuc = (int)Enums.LinhVucVaHinhThucBaoCaoTongHop.TongCongKQM_2,
                        NguonVon = nguonVon,
                        HinhThucDauThau = 0,
                        TongSoGoiThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauDoQuocHoiChuTruongDauTu)),
                        TongGiaGoiThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauDoQuocHoiChuTruongDauTu)),
                        TongGiaTrungThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauDoQuocHoiChuTruongDauTu)),
                        ChenhLechDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechDoQuocHoiChuTruongDauTu)),
                        TongSoGoiThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauDuAnNhomA)),
                        TongGiaGoiThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauDuAnNhomA)),
                        TongGiaTrungThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauDuAnNhomA)),
                        ChenhLechDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechDuAnNhomA)),
                        TongSoGoiThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauDuAnNhomB)),
                        TongGiaGoiThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauDuAnNhomB)),
                        TongGiaTrungThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauDuAnNhomB)),
                        ChenhLechDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechDuAnNhomB)),
                        TongSoGoiThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauDuAnNhomC)),
                        TongGiaGoiThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauDuAnNhomC)),
                        TongGiaTrungThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauDuAnNhomC)),
                        ChenhLechDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechDuAnNhomC)),
                        TongSoGoiThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongSoGoiThauTongCong)),
                        TongGiaGoiThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaGoiThauTongCong)),
                        TongGiaTrungThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.TongGiaTrungThauTongCong)),
                        ChenhLechTongCong = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.KhongQuaMang).Select(x => x.ChenhLechTongCong)),
                    };

                    var BaoCaoTongHopVeGoiThauVeHinhThucTongCongQM = new QuanLyThuTucBaoCaoTongHopVeGoiThau
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdBaoCao = BaoCaoTongHop.Id,
                        LinhVucVaHinhThuc = (int)Enums.LinhVucVaHinhThucBaoCaoTongHop.TongCongQM_2,
                        NguonVon = nguonVon,
                        HinhThucDauThau = 0,
                        TongSoGoiThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauDoQuocHoiChuTruongDauTu)),
                        TongGiaGoiThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauDoQuocHoiChuTruongDauTu)),
                        TongGiaTrungThauDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauDoQuocHoiChuTruongDauTu)),
                        ChenhLechDoQuocHoiChuTruongDauTu = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechDoQuocHoiChuTruongDauTu)),
                        TongSoGoiThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauDuAnNhomA)),
                        TongGiaGoiThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauDuAnNhomA)),
                        TongGiaTrungThauDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauDuAnNhomA)),
                        ChenhLechDuAnNhomA = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechDuAnNhomA)),
                        TongSoGoiThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauDuAnNhomB)),
                        TongGiaGoiThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauDuAnNhomB)),
                        TongGiaTrungThauDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauDuAnNhomB)),
                        ChenhLechDuAnNhomB = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechDuAnNhomB)),
                        TongSoGoiThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauDuAnNhomC)),
                        TongGiaGoiThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauDuAnNhomC)),
                        TongGiaTrungThauDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauDuAnNhomC)),
                        ChenhLechDuAnNhomC = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechDuAnNhomC)),
                        TongSoGoiThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongSoGoiThauTongCong)),
                        TongGiaGoiThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaGoiThauTongCong)),
                        TongGiaTrungThauTongCong = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.TongGiaTrungThauTongCong)),
                        ChenhLechTongCong = Sum(lstChiTietBaoCaoGoiThauTheoHinhThuc.Where(x => x.HinhThucDauThau == (int)Enums.HinhThucDauThau.QuaMang).Select(x => x.ChenhLechTongCong)),
                    };

                    baoCaoTongHopVeGoiThaus.Add(BaoCaoTongHopVeGoiThauVeHinhThucTongCongQM);
                    baoCaoTongHopVeGoiThaus.Add(BaoCaoTongHopVeGoiThauVeHinhThucTongCongKQM);

                    context.QuanLyThuTucBaoCaoTongHopVeGoiThaus.AddRange(baoCaoTongHopVeGoiThaus);

                    context.SaveChanges();

                    message.Title = "Thêm mới thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    trans.Commit();

                }
                catch (Exception ex)
                {
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
        private double Sum(IEnumerable<double?> values)
        {
            return values.Sum(v => v ?? 0.0);
        }


        [Route("GetBaoCaoTongHopVeGoiThauById/{id}")]
        [HttpPost]
        public ResponseMessage GetBaoCaoTongHopVeGoiThauById(string id)
        {
            try
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
                                                                                  .ThenBy(x => x.HinhThucDauThau)
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
                message.IsError = false;
                message.ObjData = new { baoCaoTongHop, lstBaoCaoChiTiet };
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

        [Route("XoaBaoCaoById")]
        [HttpGet]
        public ResponseMessage XoaBaoCaoById(string? id, string? idUser)
        {
            try
            {
                QuanLyThuTucNoiBoBaoCaoTongHop? baoCaoTongHop = context.QuanLyThuTucNoiBoBaoCaoTongHops.FirstOrDefault(baoCaoTongHop => baoCaoTongHop.Id == id);
                if (baoCaoTongHop != null)
                {
                    List<QuanLyThuTucBaoCaoTongHopVeGoiThau> listItemBaoCaoTongHopChiTiet = context.QuanLyThuTucBaoCaoTongHopVeGoiThaus.Where(s => s.IdBaoCao == baoCaoTongHop.Id).ToList();

                    if (listItemBaoCaoTongHopChiTiet != null && listItemBaoCaoTongHopChiTiet.Count > 0)
                    {
                        context.QuanLyThuTucBaoCaoTongHopVeGoiThaus.RemoveRange(listItemBaoCaoTongHopChiTiet);
                    }
                    context.QuanLyThuTucNoiBoBaoCaoTongHops.Remove(baoCaoTongHop);
                }

                message.Title = "Xóa thành công";
                message.IsError = false;
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
    }
}
