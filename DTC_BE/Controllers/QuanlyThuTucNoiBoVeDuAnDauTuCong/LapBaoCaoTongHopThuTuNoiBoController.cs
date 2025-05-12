using DTC_BE.CodeBase;
using DTC_BE.Entities;
using DTC_BE.Models;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.LapBaoCaoTongHop;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.QlHoSoNoiBo;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.ThuTucTDDADTCKhongCoCPXD;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.ThuTucThamDinhDTCBDT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Net;
using static DTC_BE.CodeBase.Enums;
using static DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung.ThuTucModels;

namespace DTC_BE.Controllers.QuanlyThuTucNoiBoVeDuAnDauTuCong
{
    [Route("api/[controller]")]
    [ApiController]
    public class LapBaoCaoTongHopThuTuNoiBoController : BaseApiController
    {
        [Route("GetDanhSachBaoCao")]
        [HttpPost]
        public ResponseMessage GetDanhSachBaoCao(TimKiemBaoCao timKiemBaoCao)
        {
            try
            {
                var htNguoiDung = context.HtNguoiDungs;
                var lstBaoCaoTongHop = context.QuanLyThuTucNoiBoBaoCaoTongHops.AsNoTracking().AsEnumerable().Where(s => ((!string.IsNullOrEmpty(timKiemBaoCao.tenBaoCao) ?
                                                                           s.Ten.ToLower()
                                                                           .Trim().Contains(timKiemBaoCao.tenBaoCao.ToLower().Trim()) : true)
                                                                           &&
                                                                           (!string.IsNullOrEmpty(timKiemBaoCao.ngayTao) ?
                                                                           s.NgayTao.Value.ToString("dd/MM/yyyy")
                                                                           .Trim().Contains(timKiemBaoCao.ngayTao.ToLower().Trim()) : true)
                                                                           && (s.LoaiBaoCaoTongHop == (int)Enums.LoaiBaoCaoTongHopThuTucNoiBo.BaoCaoTongHoVeThuTucNoiBo)
                                                                           ))
                                                       .OrderByDescending(s => s.NgayTao)
                                                       .Skip(timKiemBaoCao.rowPerPage * (timKiemBaoCao.currentPage - 1))
                                                       .Take(timKiemBaoCao.rowPerPage).Select(s => new LapBaoCaoTongHop
                                                       {
                                                           id = s.Id,
                                                           Ten = s.Ten,
                                                           Nam = s.Nam,
                                                           NgayTao = s.NgayTao != null ? s.NgayTao.Value.ToString("dd/MM/yyyy") : "",
                                                           NguoiTao = htNguoiDung.FirstOrDefault(x => x.Id == s.NguoiTao)?.HoTen ?? "",
                                                       }).ToList();
                int totalRecords = context.QuanLyThuTucNoiBoBaoCaoTongHops.AsEnumerable().Where(s => ((!string.IsNullOrEmpty(timKiemBaoCao.tenBaoCao) ?
                                                                           s.Ten.ToLower()
                                                                           .Trim().Contains(timKiemBaoCao.tenBaoCao.ToLower().Trim()) : true)
                                                                           && (!string.IsNullOrEmpty(timKiemBaoCao.ngayTao) ?
                                                                           s.NgayTao.Value.ToString("dd/MM/yyyy")
                                                                           .Trim().Contains(timKiemBaoCao.ngayTao.ToLower().Trim()) : true)
                                                                           && (s.LoaiBaoCaoTongHop == (int)Enums.LoaiBaoCaoTongHopThuTucNoiBo.BaoCaoTongHoVeThuTucNoiBo)
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
        [Route("GetListThongKeThuTucTheoLoai")]
        [HttpPost]
        public ResponseMessage GetListThongKeThuTucTheoLoai(TimKiemThongKeTheoLoai timKiemBaoCao)
        {
            try
            {
                List<SelectListItem> nhomDuAn = new List<SelectListItem>();
                foreach (Enums.NhomDuAn loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.NhomDuAn)))
                {
                    nhomDuAn.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                var htNguoiDung = context.DmChuDauTus;
                var listThongKeThuTucTheoLoai = context.QuanLyThuTucNoiBoDuAnDtcs.AsNoTracking().AsEnumerable().Where(s => timKiemBaoCao.loai == 0 || s.LoaiHoSo == timKiemBaoCao.loai)
                                                                                                      .OrderByDescending(s => s.NgayTao)
                                                                                                      .Skip(timKiemBaoCao.rowPerPage * (timKiemBaoCao.currentPage - 1))
                                                                                                      .Take(timKiemBaoCao.rowPerPage).Select(s => new ThuTucThongKeTheoLoai
                                                                                                      {
                                                                                                          id = s.Id,
                                                                                                          maHoSo = s.MaHoSo,
                                                                                                          tenHoSo = s.TenHoSo,
                                                                                                          ngayNhanHoSo = s.NgayNhanHoSo != null ? s.NgayNhanHoSo.Value.ToString("dd/MM/yyyy") : "",
                                                                                                          hanTraKetQua = s.DuKienHoanThanh != null ? s.DuKienHoanThanh.Value.ToString("dd/MM/yyyy") : "",
                                                                                                          chuDauTu = htNguoiDung.FirstOrDefault(x => x.Id == s.IdDonViThucHienDuAn)?.TenChuDauTu ?? "",
                                                                                                          nhomDuAn = nhomDuAn.FirstOrDefault(x => x.Value == s.NhomDuAn.ToString())?.Text ?? "",
                                                                                                      }).ToList();
                int totalRecords = context.QuanLyThuTucNoiBoDuAnDtcs.AsEnumerable().Where(s => timKiemBaoCao.loai == 0 || s.LoaiHoSo == timKiemBaoCao.loai).Count();
                message.IsError = false;
                message.ObjData = new { listThongKeThuTucTheoLoai, totalRecords };
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
        [Route("GetListThongKeThuTuc")]
        [HttpPost]
        public ResponseMessage GetListThongKeThuTuc()
        {
            try
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

                message.IsError = false;
                message.ObjData = new { listThongKeThuTucTheoLoai, listThongKeThuTucTheoTinhTrang };
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

        [Route("GetBaoCaoTongHopThuTucById/{id}")]
        [HttpPost]
        public ResponseMessage GetBaoCaoTongHopThuTucById(string id)
        {
            try
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


                List<SelectListItem> lstLoaiThuTuc = new List<SelectListItem>();

                foreach (Enums.LoaiThuTuNoiBoDuAnDTC loaithutucnoibo in (Enums.LoaiThuTuNoiBoDuAnDTC[])Enum.GetValues(typeof(Enums.LoaiThuTuNoiBoDuAnDTC)))
                {
                    lstLoaiThuTuc.Add(new SelectListItem { Text = loaithutucnoibo.GetDescription(), Value = loaithutucnoibo.GetHashCode().ToString() });
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
                message.IsError = false;
                message.ObjData = new { lstDuLieuBangBC, baoCaoTongHop, lstLoaiThuTuc, lstTongCong };
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

        [Route("ThemMoiLapBaoCao")]
        [HttpPost]
        public ResponseMessage ThemMoiLapBaoCao(LapBaoCaoTongHop objBaoCaoTongHop)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
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

                    var NgayBDBaoCao = DateTimeOrNull(objBaoCaoTongHop.NgayBatDau);
                    var NgayKTBaoCao = DateTimeOrNull(objBaoCaoTongHop.NgayKetThuc);

                    List<SelectListItem> lstLoaiThuTuc = new List<SelectListItem>();

                    int ngayBatDauBaoCao = int.Parse(NgayBDBaoCao != null ? NgayBDBaoCao.Value.ToString("yyyyMMdd") : "");

                    int ngayKetThucBaoCao = int.Parse(NgayKTBaoCao != null ? NgayKTBaoCao.Value.ToString("yyyyMMdd") : "");

                    foreach (Enums.LoaiThuTuNoiBoDuAnDTC loaithutucnoibo in (Enums.LoaiThuTuNoiBoDuAnDTC[])Enum.GetValues(typeof(Enums.LoaiThuTuNoiBoDuAnDTC)))
                    {
                        lstLoaiThuTuc.Add(new SelectListItem { Text = loaithutucnoibo.GetDescription(), Value = loaithutucnoibo.GetHashCode().ToString() });
                    }

                    List<TienDoXuLyThuTucModel> lstHoSoTiepNhan = new List<TienDoXuLyThuTucModel>();
                    List<KetQuaThucHienModel> lstHoSoDaGiaiQuyet = new List<KetQuaThucHienModel>();
                    List<KetQuaThucHienModel> lstHoSoDangGiaiQuyet = new List<KetQuaThucHienModel>();
                    // Fetch and process all records in one go
                    var allThuTucThamDinh = context.QuanLyThuTucNoiBoDuAnDtcs.AsNoTracking().ToList();
                    var allTienDoXuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.AsNoTracking().ToList();
                    var allKQTHThuTuc = context.QuanLyThuTucNoiBoDuAnDtcKqths.AsNoTracking().ToList();
                    foreach (var item in lstLoaiThuTuc)
                    {
                        var loaiHoSoValue = Convert.ToInt32(item.Value);

                        // Filter ThuTucThamDinh
                        var thuTucThamDinhItems = allThuTucThamDinh
                            .Where(x => x.LoaiHoSo == loaiHoSoValue && x.IsXoa != 1)
                            .Select(s => new ThuTucThamDinh
                            {
                                id = s.Id,
                                NgayTao = s.NgayTao.HasValue ? int.Parse(s.NgayTao.Value.ToString("yyyyMMdd")) : 0,
                                HanGiaiQuyetHoSo = s.DuKienHoanThanh.HasValue ? int.Parse(s.DuKienHoanThanh.Value.ToString("yyyyMMdd")) : 0,
                                idLoai = loaiHoSoValue,
                                LuuKho = s.LuuKho,
                            })
                            .ToList();



                        foreach (var thuTuc in thuTucThamDinhItems)
                        {
                            var tienDoItems = allTienDoXuLy
                                .Where(x => x.IdThuTuc == thuTuc.id)
                                .ToList();
                            var ketQuaThucHienThuTucItems = allKQTHThuTuc
                                .Where(x => x.IdThuTuc == thuTuc.id)
                                .ToList();

                            #region Hồ sơ đã tiếp nhận
                            var DaTiepNhan = tienDoItems.Where(x => x.NgayGiaiQuyet >= NgayBDBaoCao && x.NgayGiaiQuyet <= NgayKTBaoCao)
                                                        .GroupBy(x => x.IdThuTuc)
                                                        .Select(g => g.OrderByDescending(x => x.NgayGiaiQuyet).FirstOrDefault()) // lấy ngày mới nhất
                                                        .Select(x => new TienDoXuLyThuTucModel
                                                        {
                                                            id = x.Id,
                                                            idThuTuc = x.IdThuTuc,
                                                            NgayTao = thuTuc.NgayTao,
                                                            HanGiaiQuyetHoSo = thuTuc.HanGiaiQuyetHoSo,
                                                            idLoai = thuTuc.idLoai,
                                                            NgayGiaiQuyet = x.NgayGiaiQuyet.HasValue
                                                                ? int.Parse(x.NgayGiaiQuyet.Value.ToString("yyyyMMdd"))
                                                                : 0,
                                                        })
                                                        .ToList();

                            lstHoSoTiepNhan.AddRange(DaTiepNhan);
                            #endregion

                            #region Hồ sơ đang giải quyết
                            var TienDodangGiaiQuyet = tienDoItems
                                .GroupBy(x => x.IdThuTuc)
                                .SelectMany(g =>
                                {
                                    if (g.Any(x => x.TrangThai == 1))
                                        return new List<QuanLyThuTucNoiBoDuAnDtcTienDoXuLy>(); // không lấy nếu đã giải quyết
                                    else
                                        return g.Where(x => x.TrangThai >= 2 && x.TrangThai <= 6)
                                                .OrderByDescending(x => x.NgayGiaiQuyet)
                                                .ToList(); // lấy tất cả
                                })
                                .ToList();

                            var dangGiaiQuyet = TienDodangGiaiQuyet.Select(td => new KetQuaThucHienModel
                            {
                                id = td?.Id ?? "", // nếu không có Id trong TienDo thì để 0 hoặc cần sửa
                                idThuTuc = td.IdThuTuc,
                                NgayTao = thuTuc.NgayTao,
                                HanGiaiQuyetHoSo = thuTuc.HanGiaiQuyetHoSo,
                                idLoai = thuTuc.idLoai,
                                NgayKy1 = 0,
                                NgayKy2 = 0,
                                NgayGiaiQuyet = td.NgayGiaiQuyet.HasValue ? int.Parse(td.NgayGiaiQuyet.Value.ToString("yyyyMMdd")) : 0
                            }).ToList();

                            lstHoSoDangGiaiQuyet.AddRange(dangGiaiQuyet);
                            #endregion

                            #region Hồ sơ đã giải quyết
                            var TienDoDaGiaiQuyet = tienDoItems
                                .Where(x => x.TrangThai == 1)
                                .GroupBy(x => x.IdThuTuc)
                                .Select(g => g.OrderByDescending(x => x.NgayGiaiQuyet).FirstOrDefault())
                                .Where(x => x != null)
                                .ToList();

                            var DaGiaiQuyet = TienDoDaGiaiQuyet.Select(td =>
                            {
                                var ketQua = ketQuaThucHienThuTucItems.FirstOrDefault(x => x.IdThuTuc == td.IdThuTuc);

                                return new KetQuaThucHienModel
                                {
                                    id = ketQua?.Id ?? "",
                                    idThuTuc = td.IdThuTuc,
                                    NgayTao = thuTuc.NgayTao,
                                    HanGiaiQuyetHoSo = thuTuc.HanGiaiQuyetHoSo,
                                    idLoai = thuTuc.idLoai,
                                    NgayKy1 = ketQua?.NgayKy1.HasValue == true ? int.Parse(ketQua.NgayKy1.Value.ToString("yyyyMMdd")) : 0,
                                    NgayKy2 = ketQua?.NgayKy2.HasValue == true ? int.Parse(ketQua.NgayKy2.Value.ToString("yyyyMMdd")) : 0,
                                    LuuKho = thuTuc.LuuKho,
                                    NgayGiaiQuyet = td.NgayGiaiQuyet.HasValue ? int.Parse(td.NgayGiaiQuyet.Value.ToString("yyyyMMdd")) : 0
                                };
                            }).ToList();

                            lstHoSoDaGiaiQuyet.AddRange(DaGiaiQuyet);
                            #endregion
                        }
                    }

                    foreach (var item in lstLoaiThuTuc)
                    {
                        var loaiHoSoValue = Convert.ToInt32(item.Value);
                        var lstTPDL = new QuanLyThuTucNoiBoBaoCaoTongHopChiTiet
                        {
                            Id = Guid.NewGuid().ToString(),
                            LoaiHoSo = loaiHoSoValue,
                            IdBaoCao = BaoCaoTongHop.Id,

                            TongSoHsTiepNhan = lstHoSoTiepNhan.Where(x => x.idLoai == loaiHoSoValue).Count(),


                            SlHsDaGqDungHan = lstHoSoDaGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue && (x.NgayKy1 == x.HanGiaiQuyetHoSo || x.NgayKy2 == x.HanGiaiQuyetHoSo || (x.NgayKy1 == 0 && x.NgayKy2 == 0 && x.NgayGiaiQuyet.HasValue && x.NgayGiaiQuyet == x.HanGiaiQuyetHoSo))).Count(),
                            SlHsDaGqTruocHan = lstHoSoDaGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue && ((x.NgayKy1 < x.HanGiaiQuyetHoSo && x.NgayKy2 < x.HanGiaiQuyetHoSo) || (x.NgayKy1 == 0 && x.NgayKy2 == 0 && x.NgayGiaiQuyet.HasValue && x.NgayGiaiQuyet < x.HanGiaiQuyetHoSo))).Count(),
                            SlHsDaGqQuaHan = lstHoSoDaGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue && ((x.NgayKy1 > x.HanGiaiQuyetHoSo || x.NgayKy2 > x.HanGiaiQuyetHoSo) || (x.NgayKy1 == 0 && x.NgayKy2 == 0 && x.NgayGiaiQuyet.HasValue && x.NgayGiaiQuyet > x.HanGiaiQuyetHoSo))).Count(),
                            TongSoHsDaGiaiQuyet = lstHoSoDaGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue).Count(),

                            TongSoHsDangGiaiQuyet = lstHoSoDangGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue).Count(),

                            SlHsDangGqQuaHan = lstHoSoDangGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue && ((x.NgayKy1 != 0 && x.NgayKy1 > x.HanGiaiQuyetHoSo) || (x.NgayKy2 != 0 && x.NgayKy2 > x.HanGiaiQuyetHoSo) || (x.NgayKy1 == 0 && x.NgayKy2 == 0 && int.Parse(DateTime.Now.ToString("yyyyMMdd")) > x.HanGiaiQuyetHoSo))).Count(),

                            SlHsDangGqTrongHan = lstHoSoDangGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue && ((x.NgayKy1 != 0 && x.NgayKy1 <= x.HanGiaiQuyetHoSo && (x.NgayKy2 == 0 || x.NgayKy2 <= x.HanGiaiQuyetHoSo)) || (x.NgayKy2 != 0 && x.NgayKy2 <= x.HanGiaiQuyetHoSo && (x.NgayKy1 == 0 || x.NgayKy1 <= x.HanGiaiQuyetHoSo)) || (x.NgayKy1 == 0 && x.NgayKy2 == 0 && int.Parse(DateTime.Now.ToString("yyyyMMdd")) <= x.HanGiaiQuyetHoSo))).Count(),

                            TongSoHsDaHoanThanh = lstHoSoDaGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue).Count(),
                            SlHsLuuKho = lstHoSoDaGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue && x.LuuKho == 1).Count(),
                            SlHsChuaLuuKho = lstHoSoDaGiaiQuyet.Where(x => x.idLoai == loaiHoSoValue && x.LuuKho != 1).Count(),

                            GhiChu = "",
                        };
                        context.QuanLyThuTucNoiBoBaoCaoTongHopChiTiets.Add(lstTPDL);
                    }

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
                    //trans.Rollback();
                    //ThemMoiNhatKy("Thêm mới cấp công trình: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.ThemMoi),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                                            objGiaiDoan?.NguoiTao);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
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
        [Route("CapNhatBaoCao")]
        [HttpPost]
        public ResponseMessage CapNhatBaoCao(LapbaoCaoTongHopChiTiet objBaoCaoTongHopChiTiet)
        {
            try
            {

                #region Xóa danh sách chi tiết đi kèm cũ
                List<QuanLyThuTucNoiBoBaoCaoTongHopChiTiet> listItemBaoCaoTongHopChiTiet = context.QuanLyThuTucNoiBoBaoCaoTongHopChiTiets.Where(s => s.IdBaoCao == objBaoCaoTongHopChiTiet.idBaoCao).ToList();

                if (listItemBaoCaoTongHopChiTiet != null && listItemBaoCaoTongHopChiTiet.Count > 0)
                {
                    context.QuanLyThuTucNoiBoBaoCaoTongHopChiTiets.RemoveRange(listItemBaoCaoTongHopChiTiet);
                }

                #endregion

                #region Thêm mới danh sách chi tiết đi kèm mới
                if (objBaoCaoTongHopChiTiet?.listLapBaoCaoTongHopChiTiets != null && objBaoCaoTongHopChiTiet?.listLapBaoCaoTongHopChiTiets.Count > 0)
                {
                    foreach (ListLapBaoCaoTongHopChiTiet thongTinChiTietBaocao in objBaoCaoTongHopChiTiet.listLapBaoCaoTongHopChiTiets)
                    {
                        QuanLyThuTucNoiBoBaoCaoTongHopChiTiet objChiTietBaoCaoTongHop = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            LoaiHoSo = Convert.ToInt32(thongTinChiTietBaocao.loaiHoSo),
                            IdBaoCao = objBaoCaoTongHopChiTiet.idBaoCao,
                            TongSoHsTiepNhan = thongTinChiTietBaocao.tongSLHSDaTiepNhan,
                            SlHsDaGqTruocHan = thongTinChiTietBaocao.soLuongHSDaGiaiQuyetDungHan,
                            SlHsDaGqDungHan = thongTinChiTietBaocao.soLuongHSDaGiaiQuyetTruocHan,
                            SlHsDaGqQuaHan = thongTinChiTietBaocao.soLuongHSDaGiaiQuyetQuaHan,
                            TongSoHsDaGiaiQuyet = thongTinChiTietBaocao.tongSoLuongHSDaGiaiQuyet,
                            SlHsDangGqTrongHan = thongTinChiTietBaocao.soLuongHSDangGQTrongHan,
                            SlHsDangGqQuaHan = thongTinChiTietBaocao.soLuongHSDangGQQuaHan,
                            TongSoHsDangGiaiQuyet = thongTinChiTietBaocao.tongSLHSDangGiaiQuyet,
                            TongSoHsDaHoanThanh = thongTinChiTietBaocao.tongSLHSHoanThanh,
                            SlHsLuuKho = thongTinChiTietBaocao.slHsDaLuuKho,
                            SlHsChuaLuuKho = thongTinChiTietBaocao.slHsChuaLuuKho,
                            GhiChu = thongTinChiTietBaocao.ghiChu,
                        };
                        context.QuanLyThuTucNoiBoBaoCaoTongHopChiTiets.Add(objChiTietBaoCaoTongHop);
                    }
                }
                #endregion
                context.SaveChanges();
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
                    List<QuanLyThuTucNoiBoBaoCaoTongHopChiTiet> listItemBaoCaoTongHopChiTiet = context.QuanLyThuTucNoiBoBaoCaoTongHopChiTiets.Where(s => s.IdBaoCao == baoCaoTongHop.Id).ToList();

                    if (listItemBaoCaoTongHopChiTiet != null && listItemBaoCaoTongHopChiTiet.Count > 0)
                    {
                        context.QuanLyThuTucNoiBoBaoCaoTongHopChiTiets.RemoveRange(listItemBaoCaoTongHopChiTiet);
                    }
                    context.QuanLyThuTucNoiBoBaoCaoTongHops.Remove(baoCaoTongHop);
                }

                message.Title = "Xóa thành công";
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();

                //ThemMoiNhatKy("Xóa dự án: " + duAn?.TenDuAn, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.Xoa),
                //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                //                                                                 EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
                //                                                                 idUser);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = ex.Message;
                //ThemMoiNhatKy("Xóa dự án: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.Xoa),
                //                                                       EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                //                                                       EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                //                                                       idUser);
            }
            return message;
        }

    }
}
