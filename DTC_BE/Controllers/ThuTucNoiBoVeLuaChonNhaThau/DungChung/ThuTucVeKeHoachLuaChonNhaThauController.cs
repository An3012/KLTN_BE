//using DTC_BE.CodeBase;
//using DTC_BE.Entities;
//using DTC_BE.Models;
//using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using static DTC_BE.CodeBase.Enums;
//using static DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung.ThuTucLuaChonNhaThauModels;
//using static DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung.ThuTucModels;

//namespace DTC_BE.Controllers.ThuTucNoiBoVeLuaChonNhaThau.DungChung
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ThuTucVeKeHoachLuaChonNhaThauController : BaseApiController
//    {
//        #region Cấu hình đường dấu file
//        private string config_UrlSave = "Uploads/TTNoiBoVeLuaChonNhaThau/DungChung/QLTTNoiBoVeLuaChonNhaThau/" + GetFolderByDate();
//        private string config_UrlTemp = "Temps/TTNoiBoVeLuaChonNhaThau/DungChung/QLTTNoiBoVeLuaChonNhaThau/" + GetFolderByDate();
//        #endregion

//        [Route("GetSelected")]
//        [HttpGet]
//        public ResponseMessage GetSelected()
//        {
//            try
//            {
//                List<SelectListItem> lstDMNguonVon = new List<SelectListItem>();

//                lstDMNguonVon = context.DmNguonVons.Select(s => new SelectListItem
//                {
//                    Text = s.TenNguonVon,
//                    Value = s.Id,
//                }).ToList();

//                List<SelectListItem> lstChuDauTu = new List<SelectListItem>();

//                lstChuDauTu = context.DmChuDauTus.Select(s => new SelectListItem
//                {
//                    Text = s.TenChuDauTu,
//                    Value = s.Id,
//                }).ToList();

//                List<SelectListItem> lstNhomDuAn = new List<SelectListItem>();

//                lstNhomDuAn = context.DmNhomDuAns.Select(s => new SelectListItem
//                {
//                    Text = s.TenNhomDuAn,
//                    Value = s.Id,
//                }).ToList();

//                List<SelectListItem> lstLinhVuc = new List<SelectListItem>();

//                foreach (Enums.LinhVuc loaiTrangThai in (Enums.LinhVuc[])Enum.GetValues(typeof(Enums.LinhVuc)))
//                {
//                    lstLinhVuc.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
//                }

//                List<SelectListItem> lstHinhThucLuaChonNhaThau = new List<SelectListItem>();

//                foreach (Enums.HinhThucLuaChonNhaThau loaiTrangThai in (Enums.HinhThucLuaChonNhaThau[])Enum.GetValues(typeof(Enums.HinhThucLuaChonNhaThau)))
//                {
//                    lstHinhThucLuaChonNhaThau.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
//                }


//                List<SelectListItem> lstPhuongThucLuaChonNhaThau = new List<SelectListItem>();

//                foreach (Enums.PhuongThucLuachonNhaThau loaiTrangThai in (Enums.PhuongThucLuachonNhaThau[])Enum.GetValues(typeof(Enums.PhuongThucLuachonNhaThau)))
//                {
//                    lstPhuongThucLuaChonNhaThau.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
//                }

//                List<SelectListItem> lstHinhThucDauThau = new List<SelectListItem>();

//                foreach (Enums.HinhThucDauThau loaiTrangThai in (Enums.HinhThucDauThau[])Enum.GetValues(typeof(Enums.HinhThucDauThau)))
//                {
//                    lstHinhThucDauThau.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
//                }

//                List<SelectListItem> lstLoaiHopDong = new List<SelectListItem>();

//                foreach (Enums.LoaiHopDong loaiTrangThai in (Enums.LoaiHopDong[])Enum.GetValues(typeof(Enums.LoaiHopDong)))
//                {
//                    lstLoaiHopDong.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
//                }



//                message.IsError = false;
//                message.Code = HttpStatusCode.OK.GetHashCode();
//                message.ObjData = new { lstChuDauTu, lstNhomDuAn, lstLinhVuc, lstHinhThucLuaChonNhaThau, lstPhuongThucLuaChonNhaThau, lstLoaiHopDong, lstHinhThucDauThau, lstDMNguonVon };
//            }
//            catch (Exception ex)
//            {

//                message.IsError = true;
//                message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                message.Title = "Có lỗi xảy ra: " + ex.Message;
//            }
//            return message;
//        }

//        #region Thêm mới thủ tục
//        /// <summary>
//        /// Thêm mới thủ tục
//        /// </summary>
//        /// <param name=""></param>
//        /// <returns></returns>
//        [Route("ThemMoiThuTuc")]
//        [HttpPost]
//        //public ResponseMessage ThemMoiThuTuc(ThuTucLuaChonNhaThauModel objThuTuc)
//        //{
//        //    using (var trans = context.Database.BeginTransaction())
//        //    {
//        //        try
//        //        {
//        //            if (CheckExist(objThuTuc?.TenDuAn, objThuTuc?.Id))
//        //            {
//        //                throw new Exception("Dự án: " + objThuTuc?.TenDuAn?.Trim() + " đã tồn tại, Vui lòng kiểm tra lại");
//        //            }

//        //            string pathFile = Directory.GetCurrentDirectory() + objThuTuc?.FilePath;
//        //            FileInfo fileUpload = new FileInfo(pathFile);
//        //            string dt = DateTime.Now.ToString("ddMMyyhhmmss");
//        //            var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileName?.Trim()).Split(".")[0]));
//        //            string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileName?.Trim()) + extension;

//        //            var pathSaveFile = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

//        //            if (fileUpload.Exists)
//        //            {
//        //                Directory.CreateDirectory(pathSaveFile);

//        //                // Tạo đường dẫn đầy đủ cho file đích
//        //                string destinationFilePath = Path.Combine(pathSaveFile, SecurityService.EncryptPassword(objThuTuc?.FileName?.Trim()) + extension);
//        //                fileUpload.CopyTo(destinationFilePath, true);
//        //                string pathFileDelete = Directory.GetCurrentDirectory() + objThuTuc?.FilePath;
//        //                if (System.IO.File.Exists(pathFileDelete))
//        //                {
//        //                    System.IO.File.Delete(pathFileDelete);
//        //                }
//        //            }

//        //            QuanLyThuTucNoiBoKeHoachLuaChonNhaThau thuTuc = new()
//        //            {
//        //                Id = Guid.NewGuid().ToString(),
//        //                TenDuAn = objThuTuc?.TenDuAn?.Trim(),
//        //                NguonVonId = objThuTuc?.NguonVonId?.Trim(),
//        //                ChuDauTuId = objThuTuc?.ChuDauTuId,
//        //                TongMucDauTu = DoubleOrNull(objThuTuc?.TongMucDauTu),
//        //                ThoiGianThucHienDuAn = objThuTuc?.ThoiGianThucHienDuAn?.Trim(),
//        //                DiaDiemQuyMoDuAn = objThuTuc?.DiaDiemQuyMo?.Trim(),
//        //                CacThongTinKhac = objThuTuc?.CacThongTinKhac?.Trim(),
//        //                DuKienHoanThanh = DateTimeOrNull(objThuTuc?.HanGiaiQuyetHoSo?.Trim()),
//        //                FileDinhKem = objThuTuc?.FileName?.Trim(),
//        //                FilePath = tenHeThong,
//        //                LoaiThuTuc = Convert.ToInt32(objThuTuc?.LoaiThuTuc != null ? objThuTuc.LoaiThuTuc : "1"),
//        //                NgayTao = DateTime.Now,
//        //                NguoiTao = objThuTuc?.IdUser,
//        //                NhomDuAn = objThuTuc?.NhomDuAn?.Trim(),
//        //            };

//        //            context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Add(thuTuc);

//        //            if (objThuTuc?.lstCacGoiThau != null && objThuTuc?.lstCacGoiThau.Count > 0)
//        //            {
//        //                foreach (var thongThinCacGoiThau in objThuTuc.lstCacGoiThau)
//        //                {
//        //                    ThuTucNbLuaChonNhaThauPhanChiaGoiThau objThuTuc_CacGoiThau = new()
//        //                    {
//        //                        Id = Guid.NewGuid().ToString(),
//        //                        GiaGoiThau = thongThinCacGoiThau.GiaGoiThau,
//        //                        HinhThucDauThau = thongThinCacGoiThau.HinhThucDauThau,
//        //                        GiaTrungThau = thongThinCacGoiThau.GiaTrungThau,
//        //                        HinhThucLuaChonNhaThau = thongThinCacGoiThau.HinhThucLuaChonNhaThau,
//        //                        HinhThucLuaChonNhaThauTrongNgoaiNuoc = thongThinCacGoiThau.HinhThucLuaChonNhaThauTrongNgoaiNuoc,
//        //                        IdThuTuc = thuTuc.Id,
//        //                        LinhVuc = thongThinCacGoiThau.LinhVuc,
//        //                        LoaiHopDong = thongThinCacGoiThau.LoaiHopDong,
//        //                        NguonVon = thongThinCacGoiThau.NguonVon,
//        //                        PhuongThucLuaChonNhaThau = thongThinCacGoiThau.PhuongThucLuaChonNhaThau,
//        //                        TenGoiThau = thongThinCacGoiThau?.TenGoiThau?.Trim(),
//        //                        ThoiGianBdToChucLuaChonNhaThau = DateTimeOrNull(thongThinCacGoiThau?.ThoiGianBatDauToChucChonNhaThau?.Trim()),
//        //                        ThoiGianThucHienHopDong = thongThinCacGoiThau.ThoiGianThucHienHopDong,
//        //                    };
//        //                    context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Add(objThuTuc_CacGoiThau);
//        //                }
//        //            }
//        //            message.Title = "Thêm mới thành công";
//        //            message.IsError = false;
//        //            message.Code = HttpStatusCode.OK.GetHashCode();

//        //            //ThemMoiNhatKy("Thêm mới dự án: " + objThuTuc?.TenDuAn?.Trim(), EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.ThemMoi),
//        //            //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//        //            //                                                                                 EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
//        //            //                                                                                 objThuTuc?.NguoiTao);
//        //            context.SaveChanges();
//        //            trans.Commit();
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            message.IsError = true;
//        //            message.Code = HttpStatusCode.BadRequest.GetHashCode();
//        //            message.Title = ex.Message;
//        //            trans.Rollback();
//        //            //ThemMoiNhatKy("Thêm mới dự án: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.ThemMoi),
//        //            //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//        //            //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
//        //            //                                                            objThuTuc?.NguoiTao);
//        //        }
//        //        finally
//        //        {
//        //            trans.Dispose();
//        //        }
//        //    }
//        //    return message;
//        //}
//        #endregion

//        #region Thêm mới thủ tục lựa chọn nhà thầu điều chỉnh
//        /// <summary>
//        /// Thêm mới thủ tục
//        /// </summary>
//        /// <param name=""></param>
//        /// <returns></returns>
//        [Route("ThemMoiThuTucLuaChonNhaThauDieuChinh")]
//        [HttpPost]
//        public ResponseMessage ThemMoiThuTucLuaChonNhaThauDieuChinh(ThuTucLuaChonNhaThauDieuChinhModel objThuTuc)
//        {
//            using (var trans = context.Database.BeginTransaction())
//            {
//                try
//                {
//                    if (CheckExist(objThuTuc?.TenDuAn, objThuTuc?.Id))
//                    {
//                        throw new Exception("Dự án: " + objThuTuc?.TenDuAn?.Trim() + " đã tồn tại, Vui lòng kiểm tra lại");
//                    }

//                    string pathFile = Directory.GetCurrentDirectory() + objThuTuc?.FilePath;
//                    FileInfo fileUpload = new FileInfo(pathFile);
//                    string dt = DateTime.Now.ToString("ddMMyyhhmmss");
//                    var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileName?.Trim()).Split(".")[0]));
//                    string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileName?.Trim()) + extension;

//                    var pathSaveFile = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

//                    if (fileUpload.Exists)
//                    {
//                        Directory.CreateDirectory(pathSaveFile);

//                        // Tạo đường dẫn đầy đủ cho file đích
//                        string destinationFilePath = Path.Combine(pathSaveFile, SecurityService.EncryptPassword(objThuTuc?.FileName?.Trim()) + extension);
//                        fileUpload.CopyTo(destinationFilePath, true);
//                        string pathFileDelete = Directory.GetCurrentDirectory() + objThuTuc?.FilePath;
//                        if (System.IO.File.Exists(pathFileDelete))
//                        {
//                            System.IO.File.Delete(pathFileDelete);
//                        }
//                    }

//                    QuanLyThuTucNoiBoKeHoachLuaChonNhaThau thuTuc = new()
//                    {
//                        Id = Guid.NewGuid().ToString(),
//                        TenDuAn = objThuTuc?.TenDuAn?.Trim(),
//                        NguonVonId = objThuTuc?.NguonVonId?.Trim(),
//                        ChuDauTuId = objThuTuc?.ChuDauTuId,
//                        TongMucDauTu = DoubleOrNull(objThuTuc?.TongMucDauTu),
//                        ThoiGianThucHienDuAn = objThuTuc?.ThoiGianThucHienDuAn?.Trim(),
//                        DiaDiemQuyMoDuAn = objThuTuc?.DiaDiemQuyMo?.Trim(),
//                        CacThongTinKhac = objThuTuc?.CacThongTinKhac?.Trim(),
//                        DuKienHoanThanh = DateTimeOrNull(objThuTuc?.HanGiaiQuyetHoSo?.Trim()),
//                        FileDinhKem = objThuTuc?.FileName?.Trim(),
//                        FilePath = tenHeThong,
//                        LoaiThuTuc = Convert.ToInt32(objThuTuc?.LoaiThuTuc != null ? objThuTuc.LoaiThuTuc : "1"),
//                        NgayTao = DateTime.Now,
//                        NguoiTao = objThuTuc?.IdUser,
//                        NhomDuAn = objThuTuc?.NhomDuAn?.Trim(),
//                        CacNoiDungDieuChinhKhac = objThuTuc?.CacNoiDungDieuChinhKhac?.Trim(),
//                    };

//                    context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Add(thuTuc);

//                    if (objThuTuc?.lstCacGoiThau != null && objThuTuc?.lstCacGoiThau.Count > 0)
//                    {
//                        foreach (var thongThinCacGoiThau in objThuTuc.lstCacGoiThau)
//                        {
//                            ThuTucNbLuaChonNhaThauPhanChiaGoiThau objThuTuc_CacGoiThau = new()
//                            {
//                                Id = Guid.NewGuid().ToString(),
//                                GiaGoiThau = thongThinCacGoiThau.GiaGoiThau,
//                                HinhThucDauThau = thongThinCacGoiThau.HinhThucDauThau,
//                                GiaTrungThau = thongThinCacGoiThau.GiaTrungThau,
//                                HinhThucLuaChonNhaThau = thongThinCacGoiThau.HinhThucLuaChonNhaThau,
//                                HinhThucLuaChonNhaThauTrongNgoaiNuoc = thongThinCacGoiThau.HinhThucLuaChonNhaThauTrongNgoaiNuoc,
//                                IdThuTuc = thuTuc.Id,
//                                LinhVuc = thongThinCacGoiThau.LinhVuc,
//                                LoaiHopDong = thongThinCacGoiThau.LoaiHopDong,
//                                NguonVon = thongThinCacGoiThau.NguonVon,
//                                PhuongThucLuaChonNhaThau = thongThinCacGoiThau.PhuongThucLuaChonNhaThau,
//                                TenGoiThau = thongThinCacGoiThau?.TenGoiThau?.Trim(),
//                                ThoiGianBdToChucLuaChonNhaThau = DateTimeOrNull(thongThinCacGoiThau?.ThoiGianBatDauToChucChonNhaThau?.Trim()),
//                                ThoiGianThucHienHopDong = thongThinCacGoiThau.ThoiGianThucHienHopDong,
//                            };
//                            context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Add(objThuTuc_CacGoiThau);
//                        }
//                    }
//                    if (objThuTuc?.lstNoiDungDieuChinh != null && objThuTuc?.lstNoiDungDieuChinh.Count > 0)
//                    {
//                        foreach (var thongTinNoiDungDieuChinh in objThuTuc.lstNoiDungDieuChinh)
//                        {
//                            ThuTucTdLuaChonNhaThauNoiDungDieuChinh objThuTuc_NoiDungDieuChinh = new()
//                            {
//                                Id = Guid.NewGuid().ToString(),
//                                TenGoiThau = thongTinNoiDungDieuChinh?.TenGoiThau?.Trim(),
//                                IdThuTuc = thuTuc.Id,
//                                ThoiGianBdToChucLuaChonNhaThau = thongTinNoiDungDieuChinh?.ThoiGianBdToChucLuaChonNhaThau?.Trim(),
//                                DeNghiDieuChinh = thongTinNoiDungDieuChinh.DeNghiDieuChinh,
//                            };
//                            context.ThuTucTdLuaChonNhaThauNoiDungDieuChinhs.Add(objThuTuc_NoiDungDieuChinh);
//                        }
//                    }
//                    message.Title = "Thêm mới thành công";
//                    message.IsError = false;
//                    message.Code = HttpStatusCode.OK.GetHashCode();

//                    //ThemMoiNhatKy("Thêm mới dự án: " + objThuTuc?.TenDuAn?.Trim(), EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.ThemMoi),
//                    //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                    //                                                                                 EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
//                    //                                                                                 objThuTuc?.NguoiTao);
//                    context.SaveChanges();
//                    trans.Commit();
//                }
//                catch (Exception ex)
//                {
//                    message.IsError = true;
//                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                    message.Title = ex.Message;
//                    trans.Rollback();
//                    //ThemMoiNhatKy("Thêm mới dự án: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.ThemMoi),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
//                    //                                                            objThuTuc?.NguoiTao);
//                }
//                finally
//                {
//                    trans.Dispose();
//                }
//            }
//            return message;
//        }
//        #endregion

//        #region Xóa thủ tục
//        [Route("XoaThuTuc")]
//        [HttpGet]
//        public ResponseMessage XoaThuTuc(string? id, string? idUser)
//        {
//            using (var trans = context.Database.BeginTransaction())
//            {
//                try
//                {
//                    QuanLyThuTucNoiBoKeHoachLuaChonNhaThau? thuTuc = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.FirstOrDefault(thuTuc => thuTuc.Id == id);
//                    if (thuTuc != null)
//                    {
//                        thuTuc.IsXoa = (int)Enums.IsXoa.DaXoa;
//                    }

//                    message.Title = "Xóa thành công";
//                    message.IsError = false;
//                    message.Code = HttpStatusCode.OK.GetHashCode();

//                    //ThemMoiNhatKy("Xóa dự án: " + duAn?.TenDuAn, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.Xoa),
//                    //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                    //                                                                 EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
//                    //                                                                 idUser);
//                    context.SaveChanges();
//                    trans.Commit();
//                }
//                catch (Exception ex)
//                {
//                    message.IsError = true;
//                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                    message.Title = ex.Message;
//                    trans.Rollback();
//                    //ThemMoiNhatKy("Xóa dự án: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.Xoa),
//                    //                                                       EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                    //                                                       EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
//                    //                                                       idUser);
//                }
//                finally
//                {
//                    trans.Dispose();
//                }
//            }
//            return message;
//        }
//        #endregion

//        #region Xuất word
//        private readonly IWebHostEnvironment _webHostEnvironment;

//        public ThuTucVeKeHoachLuaChonNhaThauController(IWebHostEnvironment webHostEnvironment)
//        {
//            _webHostEnvironment = webHostEnvironment;
//        }
//        [Route("ExportWord")]
//        [HttpGet]
//        public IActionResult ExportWord(string? id)
//        {
//            try
//            {
//                QuanLyThuTucNoiBoKeHoachLuaChonNhaThau? thuTuc = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.FirstOrDefault(thuTuc => thuTuc.Id == id);
//                string fileName = $"CanBo_{thuTuc.TenDuAn}_{DateTime.Now:ddMMyyyyhhmmss}.doc";
//                string contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; // Định nghĩa content type cho file Word

//                // Đường dẫn tới file XML
//                string strTemp = _webHostEnvironment.ContentRootPath + @"/XML/ThuTucNoiBoDuAnDauTuCong/BieuMauSo5ThuTuc.xml";
//                string strTemplateFull = ReadContentDataFromFile(strTemp);

//                #region  Load danh sách Phiếu xử lý
//                string strRowsDaoTao = _webHostEnvironment.ContentRootPath + @"/XML/ThuTucNoiBoDuAnDauTuCong/PhieuXuLy.xml";
//                string strTemplateRowsDaoTao = ReadContentDataFromFile(strRowsDaoTao);

//                List<QuanLyThuTucNoiBoDuAnDtcPhieuXuLy> lstPhieuXuLy = context.QuanLyThuTucNoiBoDuAnDtcPhieuXuLies.Where(s => s.IdThuTuc == id).ToList();
//                string strRowsContentdt = string.Empty;

//                if (lstPhieuXuLy != null && lstPhieuXuLy.Count > 0)
//                {
//                    foreach (var item in lstPhieuXuLy)
//                    {
//                        string Ngay = item.NgayGiao?.ToString("dd") ?? string.Empty;
//                        string Thang = item.NgayGiao?.ToString("MM") ?? string.Empty;
//                        string Nam = item.NgayGiao?.ToString("yyyy") ?? string.Empty;
//                        string Gio = item.NgayGiao?.ToString("HH") ?? string.Empty;
//                        string Phut = item.NgayGiao?.ToString("mm") ?? string.Empty;
//                        string TrangThai = GetTenTrangThai(item.TrangThai);

//                        strRowsContentdt += strTemplateRowsDaoTao.Replace("@BoPhanGiao", CheckStringOrEmpty(item.BoPhanGiao))
//                                                                  .Replace("@BoPhanNhan", CheckStringOrEmpty(item.BoPhanNhan))
//                                                                  .Replace("@Gio", CheckStringOrEmpty(Gio))
//                                                                  .Replace("@Phut", CheckStringOrEmpty(Phut))
//                                                                  .Replace("@Ngay", CheckStringOrEmpty(Ngay))
//                                                                  .Replace("@Thang", CheckStringOrEmpty(Thang))
//                                                                  .Replace("@Nam", CheckStringOrEmpty(Nam))
//                                                                  .Replace("@NguoiGiao", CheckStringOrEmpty(item.NguoiGiao))
//                                                                  .Replace("@NguoiNhan", CheckStringOrEmpty(item.NguoiNhan))
//                                                                  .Replace("@KetQua", CheckStringOrEmpty(TrangThai))
//                                                                  .Replace("@GhiChu", CheckStringOrEmpty(item.GhiChu));
//                    }
//                }

//                string NgayTao = DateTime.Now.ToString("dd");
//                string ThangTao = DateTime.Now.ToString("MM");
//                string NamTao = DateTime.Now.ToString("yyyy");

//                string strContent = strTemplateFull.Replace("@NgayT", CheckStringOrEmpty(NgayTao))
//                                                   .Replace("@ThangT", CheckStringOrEmpty(ThangTao))
//                                                   .Replace("@NamT", CheckStringOrEmpty(NamTao))
//                                                   .Replace("@SoTao", CheckStringOrEmpty(thuTuc.SoTaoPhieuXuLy))
//                                                   //.Replace("@MaSoHoSo", CheckStringOrEmpty(thuTuc.MaHoSo))
//                                                   .Replace("@CoQuanGiaiQuyet", CheckStringOrEmpty(thuTuc.CoQuanGiaiQuyetHoSo))
//                                                   .Replace("@CoQuanPhoiHop", CheckStringOrEmpty(thuTuc.CoQuanPhoiHop))
//                                                   .Replace("@PhieuXuLyRow", strRowsContentdt);
//                #endregion
//                // Khởi tạo MemoryStream và ghi nội dung vào đó
//                var stream = new MemoryStream();
//                var writer = new StreamWriter(stream);
//                writer.Write(strContent);
//                writer.Flush();
//                stream.Position = 0; // Đặt vị trí của stream về đầu
//                                     // Trả về file dưới dạng FileContentResult
//                return File(stream.ToArray(), contentType, fileName);
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        [Route("GetTenTrangThai")]
//        [NonAction]
//        public string GetTenTrangThai(int? TrangThai)
//        {
//            string result = string.Empty;
//            if (TrangThai != null && TrangThai == 1)
//            {
//                result = "Trước hạn";
//            }
//            else if (TrangThai != null && TrangThai == 2)
//            {
//                result = "Đúng hạn";
//            }
//            else if (TrangThai != null && TrangThai == 3)
//            {
//                result = "Quá hạn";
//            }
//            else
//                result = "";

//            return result;
//        }

//        [Route("CheckStringOrEmpty")]
//        [NonAction]
//        public string CheckStringOrEmpty(string input)
//        {
//            string result = string.Empty;
//            if (!string.IsNullOrEmpty(input))
//                result = input;
//            else
//                result = "";

//            return result;
//        }

//        [Route("ReadContentDataFromFile")]
//        [NonAction]
//        public static string ReadContentDataFromFile(string _fileName)
//        {
//            FileStream objFileStream = null;
//            try
//            {
//                objFileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
//                StreamReader objReader = new StreamReader(objFileStream);
//                string s = objReader.ReadToEnd();
//                return s;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                if (objFileStream != null)
//                    objFileStream.Close();
//            }
//        }
//        #endregion

//        #region get thủ tục lựa chọn nhà thầu

//        [Route("GetThuTucLuaChonNhaThauById")]
//        [HttpGet]
//        public ResponseMessage GetThuTucLuaChonNhaThauById(string? id)
//        {
//            try
//            {
//                var thuTuc = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Where(duAn => duAn.Id == id)
//                                          .Select(duAn => new
//                                          {
//                                              duAn.Id,
//                                              duAn.TenDuAn,
//                                              duAn.TongMucDauTu,
//                                              duAn.NhomDuAn,
//                                              duAn.ChuDauTuId,
//                                              duAn.NguonVonId,
//                                              duAn.ThoiGianThucHienDuAn,
//                                              duAn.DiaDiemQuyMoDuAn,
//                                              duAn.CacThongTinKhac,
//                                              duAn.FileDinhKem,
//                                              duAn.FilePath,
//                                              duAn.DuKienHoanThanh,
//                                              duAn.LoaiThuTuc,
//                                          })
//                                          .FirstOrDefault();

//                List<PhanChiaDuAnThanhCacGoiThau> lstPhanChiaGoiThau = context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Where(nguonVon => nguonVon.IdThuTuc == id)
//                                                                        .Select(thuTuc => new PhanChiaDuAnThanhCacGoiThau
//                                                                        {
//                                                                            TenGoiThau = thuTuc.TenGoiThau,
//                                                                            GiaGoiThau = thuTuc.GiaGoiThau,
//                                                                            GiaTrungThau = thuTuc.GiaTrungThau,
//                                                                            NguonVon = thuTuc.NguonVon,
//                                                                            LinhVuc = thuTuc.LinhVuc,
//                                                                            HinhThucDauThau = thuTuc.HinhThucDauThau,
//                                                                            HinhThucLuaChonNhaThau = thuTuc.HinhThucLuaChonNhaThau,
//                                                                            HinhThucLuaChonNhaThauTrongNgoaiNuoc = thuTuc.HinhThucLuaChonNhaThauTrongNgoaiNuoc,
//                                                                            PhuongThucLuaChonNhaThau = thuTuc.PhuongThucLuaChonNhaThau,
//                                                                            LoaiHopDong = thuTuc.LoaiHopDong,
//                                                                            ThoiGianBatDauToChucChonNhaThau = thuTuc.ThoiGianBdToChucLuaChonNhaThau != null ? thuTuc.ThoiGianBdToChucLuaChonNhaThau.Value.ToString("dd/MM/yyyy") : "",
//                                                                            ThoiGianThucHienHopDong = thuTuc.ThoiGianThucHienHopDong,
//                                                                        })
//                                                                        .ToList();

//                var ketQua = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Where(duAn => duAn.Id == id)
//                                          .Select(thuTuc => new
//                                          {
//                                              thuTuc.TrangThaiKetQua,
//                                              thuTuc.NguoiKyKetQua,
//                                              thuTuc.NgayKyKetQua,
//                                              thuTuc.SoQuyetDinhKetQua,
//                                              thuTuc.FileDinhKemKetQua,
//                                              thuTuc.FilePathDinhKemKetQua,
//                                          })
//                                          .FirstOrDefault();
//                message.ObjData = new
//                {
//                    thuTuc,
//                    lstPhanChiaGoiThau,
//                    ketQua,
//                };
//                message.IsError = false;
//                message.Code = HttpStatusCode.OK.GetHashCode();
//            }
//            catch (Exception ex)
//            {
//                message.IsError = true;
//                message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                message.Title = ex.Message;
//            }
//            return message;
//        }

//        #endregion

//        #region get thủ tục lựa chọn nhà thầu điều chỉnh

//        [Route("GetTTLCNhaThauDieuChinhById")]
//        [HttpGet]
//        public ResponseMessage GetTTLCNhaThauDieuChinhById(string? id)
//        {
//            try
//            {
//                var thuTuc = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Where(duAn => duAn.Id == id)
//                                          .Select(duAn => new
//                                          {
//                                              duAn.Id,
//                                              duAn.TenDuAn,
//                                              duAn.TongMucDauTu,
//                                              duAn.NhomDuAn,
//                                              duAn.ChuDauTuId,
//                                              duAn.NguonVonId,
//                                              duAn.ThoiGianThucHienDuAn,
//                                              duAn.DiaDiemQuyMoDuAn,
//                                              duAn.CacThongTinKhac,
//                                              duAn.FileDinhKem,
//                                              duAn.FilePath,
//                                              duAn.DuKienHoanThanh,
//                                              duAn.LoaiThuTuc,
//                                              duAn.CacNoiDungDieuChinhKhac,
//                                          })
//                                          .FirstOrDefault();

//                List<ThuTucNbLuaChonNhaThauPhanChiaGoiThau> lstPhanChiaGoiThau = context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Where(x => x.IdThuTuc == id).ToList();
//                List<ThuTucTdLuaChonNhaThauNoiDungDieuChinh> lstNoiDungDieuChinh = context.ThuTucTdLuaChonNhaThauNoiDungDieuChinhs.Where(s => s.IdThuTuc == id).ToList();

//                var ketQua = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Where(duAn => duAn.Id == id)
//                                          .Select(thuTuc => new
//                                          {
//                                              thuTuc.TrangThaiKetQua,
//                                              thuTuc.NguoiKyKetQua,
//                                              thuTuc.NgayKyKetQua,
//                                              thuTuc.SoQuyetDinhKetQua,
//                                              thuTuc.FileDinhKemKetQua,
//                                              thuTuc.FilePathDinhKemKetQua,
//                                          })
//                                          .FirstOrDefault();
//                message.ObjData = new
//                {
//                    thuTuc,
//                    lstPhanChiaGoiThau,
//                    lstNoiDungDieuChinh,
//                    ketQua,
//                };
//                message.IsError = false;
//                message.Code = HttpStatusCode.OK.GetHashCode();
//            }
//            catch (Exception ex)
//            {
//                message.IsError = true;
//                message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                message.Title = ex.Message;
//            }
//            return message;
//        }

//        #endregion

//        #region Cập nhật thủ tục
//        [Route("CapNhatThuTuc")]
//        [HttpPost]
//        public ResponseMessage CapNhatThuTuc(ThuTucLuaChonNhaThauModel objThuTuc)
//        {
//            using (var trans = context.Database.BeginTransaction())
//            {
//                try
//                {
//                    //if (CheckExist(objThuTuc?.TenHoSo, objThuTuc?.Id))
//                    //{
//                    //    throw new Exception("Tên hồ sơ: " + objThuTuc?.TenHoSo?.Trim() + " đã tồn tại, Vui lòng kiểm tra lại");
//                    //}

//                    QuanLyThuTucNoiBoKeHoachLuaChonNhaThau? thuTuc = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.FirstOrDefault(thuTuc => thuTuc.Id == objThuTuc.Id);
//                    if (thuTuc != null)
//                    {
//                        #region Cập nhật thông tin chung của dự án
//                        thuTuc.TenDuAn = objThuTuc?.TenDuAn?.Trim();
//                        thuTuc.ThoiGianThucHienDuAn = objThuTuc?.ThoiGianThucHienDuAn?.Trim();
//                        thuTuc.ChuDauTuId = objThuTuc?.ChuDauTuId?.Trim();
//                        thuTuc.TongMucDauTu = DoubleOrNull(objThuTuc?.TongMucDauTu);
//                        thuTuc.NguonVonId = objThuTuc?.NguonVonId?.Trim();
//                        thuTuc.NhomDuAn = objThuTuc?.NhomDuAn?.Trim();
//                        thuTuc.DiaDiemQuyMoDuAn = objThuTuc?.DiaDiemQuyMo?.Trim();
//                        thuTuc.LoaiThuTuc = Convert.ToInt32(objThuTuc?.LoaiThuTuc);
//                        thuTuc.DuKienHoanThanh = DateTimeOrNull(objThuTuc?.HanGiaiQuyetHoSo?.Trim());
//                        thuTuc.CacThongTinKhac = objThuTuc?.CacThongTinKhac?.Trim();
//                        thuTuc.NgayCapNhat = DateTime.Now;

//                        if (objThuTuc?.IsDelete == true)
//                        {
//                            if (!string.IsNullOrEmpty(thuTuc.FilePath))
//                            {
//                                FileInfo fileUpload = new FileInfo(thuTuc.FilePath);
//                                if (System.IO.File.Exists(Directory.GetCurrentDirectory() + thuTuc?.FilePath))
//                                {
//                                    System.IO.File.Delete(Directory.GetCurrentDirectory() + thuTuc?.FilePath);
//                                }
//                            }

//                            thuTuc.FileDinhKem = null;
//                            thuTuc.FilePath = null;
//                        }

//                        if (objThuTuc?.IsNew == true)
//                        {
//                            string pathFile = Directory.GetCurrentDirectory() + objThuTuc?.FilePath;
//                            FileInfo fileUpload = new FileInfo(pathFile);
//                            string dt = DateTime.Now.ToString("ddMMyyhhmmss");
//                            var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileName?.Trim()).Split(".")[0]));

//                            string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileName?.Trim() + dt) + extension;

//                            var pathSaveFile = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

//                            if (fileUpload.Exists)
//                            {
//                                Directory.CreateDirectory(pathSaveFile);

//                                // Tạo đường dẫn đầy đủ cho file đích
//                                string destinationFilePath = Path.Combine(pathSaveFile, SecurityService.EncryptPassword(objThuTuc?.FileName?.Trim() + dt) + extension);
//                                fileUpload.CopyTo(destinationFilePath, true);
//                                if (System.IO.File.Exists(pathFile))
//                                {
//                                    System.IO.File.Delete(pathFile);
//                                }
//                                thuTuc.FilePath = tenHeThong;
//                                thuTuc.FileDinhKem = objThuTuc?.FileName?.Trim();
//                            }
//                        }
//                        #endregion

//                        #region Xóa danh sách dự án kèm theo cũ
//                        List<ThuTucNbLuaChonNhaThauPhanChiaGoiThau> lstCacGoiThau = context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Where(nguonVon => nguonVon.IdThuTuc == thuTuc.Id).ToList();

//                        if (lstCacGoiThau != null && lstCacGoiThau.Count > 0)
//                        {
//                            context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.RemoveRange(lstCacGoiThau);
//                        }

//                        #endregion

//                        #region Thêm mới danh sách dự án kèm theo mới
//                        if (objThuTuc?.lstCacGoiThau != null && objThuTuc?.lstCacGoiThau.Count > 0)
//                        {
//                            foreach (PhanChiaDuAnThanhCacGoiThau thongTinNguonVon in objThuTuc.lstCacGoiThau)
//                            {
//                                ThuTucNbLuaChonNhaThauPhanChiaGoiThau objThuTuc_CacGoiThauThuTuc = new()
//                                {
//                                    Id = Guid.NewGuid().ToString(),
//                                    IdThuTuc = thuTuc.Id,
//                                    TenGoiThau = thongTinNguonVon?.TenGoiThau,
//                                    GiaGoiThau = DoubleOrNull(thongTinNguonVon?.GiaGoiThau),
//                                    GiaTrungThau = DoubleOrNull(thongTinNguonVon?.GiaTrungThau),
//                                    NguonVon = DoubleOrNull(thongTinNguonVon?.NguonVon),
//                                    LinhVuc = thongTinNguonVon?.LinhVuc,
//                                    HinhThucLuaChonNhaThauTrongNgoaiNuoc = thongTinNguonVon?.HinhThucLuaChonNhaThauTrongNgoaiNuoc,
//                                    HinhThucLuaChonNhaThau = thongTinNguonVon?.HinhThucLuaChonNhaThau,
//                                    HinhThucDauThau = thongTinNguonVon?.HinhThucDauThau,
//                                    PhuongThucLuaChonNhaThau = thongTinNguonVon?.PhuongThucLuaChonNhaThau,
//                                    LoaiHopDong = thongTinNguonVon?.LoaiHopDong,
//                                    ThoiGianBdToChucLuaChonNhaThau = DateTimeOrNull(thongTinNguonVon?.ThoiGianBatDauToChucChonNhaThau),
//                                    ThoiGianThucHienHopDong = thongTinNguonVon?.ThoiGianThucHienHopDong,
//                                };
//                                context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Add(objThuTuc_CacGoiThauThuTuc);
//                            }
//                        }
//                        #endregion

//                        #region cập Nhật Kết quả thực hiện
//                        QuanLyThuTucNoiBoKeHoachLuaChonNhaThau? kqth = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.FirstOrDefault(thuTuc => thuTuc.Id == objThuTuc.Id);
//                        if (kqth != null)
//                        {
//                            kqth.TrangThaiKetQua = IntOrNull(objThuTuc?.TrangThaiKetQua);
//                            if (objThuTuc?.IsDeleteKqth == true)
//                            {
//                                if (!string.IsNullOrEmpty(kqth.FilePathDinhKemKetQua))
//                                {
//                                    FileInfo fileUpload = new FileInfo(kqth.FilePath);
//                                    if (System.IO.File.Exists(Directory.GetCurrentDirectory() + kqth?.FilePathDinhKemKetQua))
//                                    {
//                                        System.IO.File.Delete(Directory.GetCurrentDirectory() + kqth?.FilePathDinhKemKetQua);
//                                    }
//                                }

//                                kqth.FileDinhKemKetQua = null;
//                                kqth.FilePathDinhKemKetQua = null;
//                            }

//                            if (objThuTuc?.IsNewKqth == true)
//                            {
//                                string pathFile = Directory.GetCurrentDirectory() + objThuTuc?.FilePathKqth;
//                                FileInfo fileUpload = new FileInfo(pathFile);
//                                string dt = DateTime.Now.ToString("ddMMyyhhmmss");
//                                var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileNameKqth?.Trim()).Split(".")[0]));

//                                string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileNameKqth?.Trim() + dt) + extension;

//                                var pathSaveFile = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

//                                if (fileUpload.Exists)
//                                {
//                                    Directory.CreateDirectory(pathSaveFile);

//                                    // Tạo đường dẫn đầy đủ cho file đích
//                                    string destinationFilePath = Path.Combine(pathSaveFile, SecurityService.EncryptPassword(objThuTuc?.FileNameKqth?.Trim() + dt) + extension);
//                                    fileUpload.CopyTo(destinationFilePath, true);
//                                    if (System.IO.File.Exists(pathFile))
//                                    {
//                                        System.IO.File.Delete(pathFile);
//                                    }
//                                    kqth.FilePathDinhKemKetQua = tenHeThong;
//                                    kqth.FileDinhKemKetQua = objThuTuc?.FileNameKqth?.Trim();
//                                }
//                            }
//                            context.SaveChanges();
//                        }
//                        else
//                        {
//                            string pathFile = Directory.GetCurrentDirectory() + objThuTuc?.FilePathKqth;
//                            FileInfo fileUpload = new FileInfo(pathFile);
//                            string dt = DateTime.Now.ToString("ddMMyyhhmmss");
//                            var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileNameKqth?.Trim()).Split(".")[0]));
//                            string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileNameKqth?.Trim()) + extension;

//                            var pathSaveFile = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

//                            if (fileUpload.Exists)
//                            {
//                                Directory.CreateDirectory(pathSaveFile);

//                                // Tạo đường dẫn đầy đủ cho file đích
//                                string destinationFilePath = Path.Combine(pathSaveFile, SecurityService.EncryptPassword(objThuTuc?.FileNameKqth?.Trim()) + extension);
//                                fileUpload.CopyTo(destinationFilePath, true);
//                                string pathFileDelete = Directory.GetCurrentDirectory() + objThuTuc?.FilePathKqth;
//                                if (System.IO.File.Exists(pathFileDelete))
//                                {
//                                    System.IO.File.Delete(pathFileDelete);
//                                }
//                            }

//                            QuanLyThuTucNoiBoKeHoachLuaChonNhaThau kqthThemMoi = new()
//                            {
//                                TrangThaiKetQua = IntOrNull(objThuTuc?.TrangThaiKetQua),
//                                SoQuyetDinhKetQua = objThuTuc.SoQuyetDinhKetQua,
//                                NguoiKyKetQua = objThuTuc.NguoiKyKetQua,
//                                NgayKyKetQua = DateTimeOrNull(objThuTuc.NgayKyKetQua),
//                                FileDinhKemKetQua = objThuTuc?.FileNameKqth?.Trim(),
//                                FilePathDinhKemKetQua = tenHeThong,
//                            };

//                            context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Add(kqthThemMoi);
//                        }
//                        #endregion

//                        message.Title = "Cập nhật thành công";
//                        message.IsError = false;
//                        message.Code = HttpStatusCode.OK.GetHashCode();
//                        //ThemMoiNhatKy("Cập nhật dự án: " + objThuTuc?.TenDuAn?.Trim(), EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
//                        //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                        //EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
//                        //                                                                                 objThuTuc?.NguoiCapNhat);

//                        context.SaveChanges();
//                        trans.Commit();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    message.IsError = true;
//                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                    message.Title = ex.Message;
//                    trans.Rollback();
//                    //ThemMoiNhatKy("Cập nhật dự án: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
//                    //                                                            objThuTuc?.NguoiCapNhat);
//                }
//                finally
//                {
//                    trans.Dispose();
//                }
//            }
//            return message;
//        }
//        #endregion

//        #region Cập nhật thủ tục Lựa chọn nhà thầu điều chỉnh
//        [Route("CapNhatThuTucLuaChonNhaThauDieuChinh")]
//        [HttpPost]
//        public ResponseMessage CapNhatThuTucLuaChonNhaThauDieuChinh(ThuTucLuaChonNhaThauDieuChinhModel objThuTuc)
//        {
//            using (var trans = context.Database.BeginTransaction())
//            {
//                try
//                {
//                    //if (CheckExist(objThuTuc?.TenHoSo, objThuTuc?.Id))
//                    //{
//                    //    throw new Exception("Tên hồ sơ: " + objThuTuc?.TenHoSo?.Trim() + " đã tồn tại, Vui lòng kiểm tra lại");
//                    //}

//                    QuanLyThuTucNoiBoKeHoachLuaChonNhaThau? thuTuc = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.FirstOrDefault(thuTuc => thuTuc.Id == objThuTuc.Id);
//                    if (thuTuc != null)
//                    {
//                        #region Cập nhật thông tin chung của dự án
//                        thuTuc.TenDuAn = objThuTuc?.TenDuAn?.Trim();
//                        thuTuc.ThoiGianThucHienDuAn = objThuTuc?.ThoiGianThucHienDuAn?.Trim();
//                        thuTuc.ChuDauTuId = objThuTuc?.ChuDauTuId?.Trim();
//                        thuTuc.TongMucDauTu = DoubleOrNull(objThuTuc?.TongMucDauTu);
//                        thuTuc.NguonVonId = objThuTuc?.NguonVonId?.Trim();
//                        thuTuc.NhomDuAn = objThuTuc?.NhomDuAn?.Trim();
//                        thuTuc.DiaDiemQuyMoDuAn = objThuTuc?.DiaDiemQuyMo?.Trim();
//                        thuTuc.LoaiThuTuc = Convert.ToInt32(objThuTuc?.LoaiThuTuc);
//                        thuTuc.DuKienHoanThanh = DateTimeOrNull(objThuTuc?.HanGiaiQuyetHoSo?.Trim());
//                        thuTuc.CacThongTinKhac = objThuTuc?.CacThongTinKhac?.Trim();
//                        thuTuc.CacNoiDungDieuChinhKhac = objThuTuc?.CacNoiDungDieuChinhKhac?.Trim();
//                        thuTuc.NgayCapNhat = DateTime.Now;

//                        if (objThuTuc?.IsDelete == true)
//                        {
//                            if (!string.IsNullOrEmpty(thuTuc.FilePath))
//                            {
//                                FileInfo fileUpload = new FileInfo(thuTuc.FilePath);
//                                if (System.IO.File.Exists(Directory.GetCurrentDirectory() + thuTuc?.FilePath))
//                                {
//                                    System.IO.File.Delete(Directory.GetCurrentDirectory() + thuTuc?.FilePath);
//                                }
//                            }

//                            thuTuc.FileDinhKem = null;
//                            thuTuc.FilePath = null;
//                        }

//                        if (objThuTuc?.IsNew == true)
//                        {
//                            string pathFile = Directory.GetCurrentDirectory() + objThuTuc?.FilePath;
//                            FileInfo fileUpload = new FileInfo(pathFile);
//                            string dt = DateTime.Now.ToString("ddMMyyhhmmss");
//                            var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileName?.Trim()).Split(".")[0]));

//                            string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileName?.Trim() + dt) + extension;

//                            var pathSaveFile = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

//                            if (fileUpload.Exists)
//                            {
//                                Directory.CreateDirectory(pathSaveFile);

//                                // Tạo đường dẫn đầy đủ cho file đích
//                                string destinationFilePath = Path.Combine(pathSaveFile, SecurityService.EncryptPassword(objThuTuc?.FileName?.Trim() + dt) + extension);
//                                fileUpload.CopyTo(destinationFilePath, true);
//                                if (System.IO.File.Exists(pathFile))
//                                {
//                                    System.IO.File.Delete(pathFile);
//                                }
//                                thuTuc.FilePath = tenHeThong;
//                                thuTuc.FileDinhKem = objThuTuc?.FileName?.Trim();
//                            }
//                        }
//                        #endregion

//                        #region Xóa danh sách dự án kèm theo cũ
//                        List<ThuTucNbLuaChonNhaThauPhanChiaGoiThau> lstCacGoiThau = context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Where(nguonVon => nguonVon.IdThuTuc == thuTuc.Id).ToList();

//                        if (lstCacGoiThau != null && lstCacGoiThau.Count > 0)
//                        {
//                            context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.RemoveRange(lstCacGoiThau);
//                        }

//                        #endregion

//                        #region Thêm mới danh sách dự án kèm theo mới
//                        if (objThuTuc?.lstCacGoiThau != null && objThuTuc?.lstCacGoiThau.Count > 0)
//                        {
//                            foreach (PhanChiaDuAnThanhCacGoiThau thongTinNguonVon in objThuTuc.lstCacGoiThau)
//                            {
//                                ThuTucNbLuaChonNhaThauPhanChiaGoiThau objThuTuc_CacGoiThauThuTuc = new()
//                                {
//                                    Id = Guid.NewGuid().ToString(),
//                                    IdThuTuc = thuTuc.Id,
//                                    TenGoiThau = thongTinNguonVon?.TenGoiThau,
//                                    GiaGoiThau = DoubleOrNull(thongTinNguonVon?.GiaGoiThau),
//                                    GiaTrungThau = DoubleOrNull(thongTinNguonVon?.GiaTrungThau),
//                                    NguonVon = DoubleOrNull(thongTinNguonVon?.NguonVon),
//                                    LinhVuc = thongTinNguonVon?.LinhVuc,
//                                    HinhThucLuaChonNhaThauTrongNgoaiNuoc = thongTinNguonVon?.HinhThucLuaChonNhaThauTrongNgoaiNuoc,
//                                    HinhThucLuaChonNhaThau = thongTinNguonVon?.HinhThucLuaChonNhaThau,
//                                    HinhThucDauThau = thongTinNguonVon?.HinhThucDauThau,
//                                    PhuongThucLuaChonNhaThau = thongTinNguonVon?.PhuongThucLuaChonNhaThau,
//                                    LoaiHopDong = thongTinNguonVon?.LoaiHopDong,
//                                    ThoiGianBdToChucLuaChonNhaThau = DateTimeOrNull(thongTinNguonVon?.ThoiGianBatDauToChucChonNhaThau),
//                                    ThoiGianThucHienHopDong = thongTinNguonVon?.ThoiGianThucHienHopDong,
//                                };
//                                context.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus.Add(objThuTuc_CacGoiThauThuTuc);
//                            }
//                        }
//                        #endregion

//                        #region Xóa danh sách Nội dung điều chỉnh kèm theo cũ
//                        List<ThuTucTdLuaChonNhaThauNoiDungDieuChinh> lstNoiDungDieuChinh = context.ThuTucTdLuaChonNhaThauNoiDungDieuChinhs.Where(nguonVon => nguonVon.IdThuTuc == thuTuc.Id).ToList();

//                        if (lstNoiDungDieuChinh != null && lstNoiDungDieuChinh.Count > 0)
//                        {
//                            context.ThuTucTdLuaChonNhaThauNoiDungDieuChinhs.RemoveRange(lstNoiDungDieuChinh);
//                        }

//                        #endregion

//                        #region Thêm mới danh sách Nội dung điều chỉnh kèm theo mới
//                        if (objThuTuc?.lstNoiDungDieuChinh != null && objThuTuc?.lstNoiDungDieuChinh.Count > 0)
//                        {
//                            foreach (NoiDungDieuChinh thongTinNguonVon in objThuTuc.lstNoiDungDieuChinh)
//                            {
//                                ThuTucTdLuaChonNhaThauNoiDungDieuChinh objThuTuc_NoiDungDieuChinh = new()
//                                {
//                                    Id = Guid.NewGuid().ToString(),
//                                    IdThuTuc = thuTuc.Id,
//                                    TenGoiThau = thongTinNguonVon?.TenGoiThau,
//                                    ThoiGianBdToChucLuaChonNhaThau = thongTinNguonVon?.ThoiGianBdToChucLuaChonNhaThau,
//                                    DeNghiDieuChinh = thongTinNguonVon?.DeNghiDieuChinh,
//                                };
//                                context.ThuTucTdLuaChonNhaThauNoiDungDieuChinhs.Add(objThuTuc_NoiDungDieuChinh);
//                            }
//                        }
//                        #endregion

//                        #region cập Nhật Kết quả thực hiện
//                        QuanLyThuTucNoiBoKeHoachLuaChonNhaThau? kqth = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.FirstOrDefault(thuTuc => thuTuc.Id == objThuTuc.Id);
//                        if (kqth != null)
//                        {
//                            kqth.TrangThaiKetQua = IntOrNull(objThuTuc?.TrangThaiKetQua);
//                            if (objThuTuc?.IsDeleteKqth == true)
//                            {
//                                if (!string.IsNullOrEmpty(kqth.FilePathDinhKemKetQua))
//                                {
//                                    FileInfo fileUpload = new FileInfo(kqth.FilePath);
//                                    if (System.IO.File.Exists(Directory.GetCurrentDirectory() + kqth?.FilePathDinhKemKetQua))
//                                    {
//                                        System.IO.File.Delete(Directory.GetCurrentDirectory() + kqth?.FilePathDinhKemKetQua);
//                                    }
//                                }

//                                kqth.FileDinhKemKetQua = null;
//                                kqth.FilePathDinhKemKetQua = null;
//                            }

//                            if (objThuTuc?.IsNewKqth == true)
//                            {
//                                string pathFile = Directory.GetCurrentDirectory() + objThuTuc?.FilePathKqth;
//                                FileInfo fileUpload = new FileInfo(pathFile);
//                                string dt = DateTime.Now.ToString("ddMMyyhhmmss");
//                                var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileNameKqth?.Trim()).Split(".")[0]));

//                                string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileNameKqth?.Trim() + dt) + extension;

//                                var pathSaveFile = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

//                                if (fileUpload.Exists)
//                                {
//                                    Directory.CreateDirectory(pathSaveFile);

//                                    // Tạo đường dẫn đầy đủ cho file đích
//                                    string destinationFilePath = Path.Combine(pathSaveFile, SecurityService.EncryptPassword(objThuTuc?.FileNameKqth?.Trim() + dt) + extension);
//                                    fileUpload.CopyTo(destinationFilePath, true);
//                                    if (System.IO.File.Exists(pathFile))
//                                    {
//                                        System.IO.File.Delete(pathFile);
//                                    }
//                                    kqth.FilePathDinhKemKetQua = tenHeThong;
//                                    kqth.FileDinhKemKetQua = objThuTuc?.FileNameKqth?.Trim();
//                                }
//                            }
//                            context.SaveChanges();
//                        }
//                        else
//                        {
//                            string pathFile = Directory.GetCurrentDirectory() + objThuTuc?.FilePathKqth;
//                            FileInfo fileUpload = new FileInfo(pathFile);
//                            string dt = DateTime.Now.ToString("ddMMyyhhmmss");
//                            var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileNameKqth?.Trim()).Split(".")[0]));
//                            string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileNameKqth?.Trim()) + extension;

//                            var pathSaveFile = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

//                            if (fileUpload.Exists)
//                            {
//                                Directory.CreateDirectory(pathSaveFile);

//                                // Tạo đường dẫn đầy đủ cho file đích
//                                string destinationFilePath = Path.Combine(pathSaveFile, SecurityService.EncryptPassword(objThuTuc?.FileNameKqth?.Trim()) + extension);
//                                fileUpload.CopyTo(destinationFilePath, true);
//                                string pathFileDelete = Directory.GetCurrentDirectory() + objThuTuc?.FilePathKqth;
//                                if (System.IO.File.Exists(pathFileDelete))
//                                {
//                                    System.IO.File.Delete(pathFileDelete);
//                                }
//                            }

//                            QuanLyThuTucNoiBoKeHoachLuaChonNhaThau kqthThemMoi = new()
//                            {
//                                TrangThaiKetQua = IntOrNull(objThuTuc?.TrangThaiKetQua),
//                                SoQuyetDinhKetQua = objThuTuc.SoQuyetDinhKetQua,
//                                NguoiKyKetQua = objThuTuc.NguoiKyKetQua,
//                                NgayKyKetQua = DateTimeOrNull(objThuTuc.NgayKyKetQua),
//                                FileDinhKemKetQua = objThuTuc?.FileNameKqth?.Trim(),
//                                FilePathDinhKemKetQua = tenHeThong,
//                            };

//                            context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Add(kqthThemMoi);
//                        }
//                        #endregion

//                        message.Title = "Cập nhật thành công";
//                        message.IsError = false;
//                        message.Code = HttpStatusCode.OK.GetHashCode();
//                        //ThemMoiNhatKy("Cập nhật dự án: " + objThuTuc?.TenDuAn?.Trim(), EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
//                        //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                        //EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
//                        //                                                                                 objThuTuc?.NguoiCapNhat);

//                        context.SaveChanges();
//                        trans.Commit();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    message.IsError = true;
//                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                    message.Title = ex.Message;
//                    trans.Rollback();
//                    //ThemMoiNhatKy("Cập nhật dự án: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
//                    //                                                            objThuTuc?.NguoiCapNhat);
//                }
//                finally
//                {
//                    trans.Dispose();
//                }
//            }
//            return message;
//        }
//        #endregion

//        #region Thủ tục nội bộ dự án ĐTC Phiếu xử lý 

//        #region Lấy dữ liệu phiếu xử lý
//        [Route("getPhieuXuLyById")]
//        [HttpGet]
//        public ResponseMessage getPhieuXuLyById(string? id)
//        {
//            try
//            {
//                var thuTuc = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Where(duAn => duAn.Id == id)
//                                          .Select(duAn => new
//                                          {
//                                              duAn.Id,
//                                              duAn.CoQuanGiaiQuyetHoSo,
//                                              duAn.CoQuanPhoiHop,
//                                              duAn.SoTaoPhieuXuLy,
//                                          })
//                                          .FirstOrDefault();

//                List<QuanLyThuTucNoiBoDuAnDtcPhieuXuLy> lstNguonVonPhieuXuLy = context.QuanLyThuTucNoiBoDuAnDtcPhieuXuLies.Where(nguonVon => nguonVon.IdThuTuc == id).ToList();

//                if (lstNguonVonPhieuXuLy.Count == 0)
//                {
//                    var abc = new QuanLyThuTucNoiBoDuAnDtcPhieuXuLy
//                    {
//                        Id = "1",
//                        IdThuTuc = thuTuc.Id,
//                        BoPhanGiao = "",
//                        BoPhanNhan = "",
//                        NgayGiao = null,
//                        NguoiGiao = "",
//                        NguoiNhan = "",
//                        TrangThai = null,
//                        NgayTao = null,
//                        NguoiTao = ""
//                    };

//                    lstNguonVonPhieuXuLy.Add(abc);
//                }

//                message.ObjData = new
//                {
//                    thuTuc,
//                    lstNguonVonPhieuXuLy,
//                };
//                message.IsError = false;
//                message.Code = HttpStatusCode.OK.GetHashCode();
//            }
//            catch (Exception ex)
//            {
//                message.IsError = true;
//                message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                message.Title = ex.Message;
//            }
//            return message;
//        }
//        #endregion

//        #region Cập Nhật Phiếu xử lý
//        [Route("CapNhatPhieuXuLy")]
//        [HttpPost]
//        public ResponseMessage CapNhatPhieuXuLy(ThuTucLuaChonNhaThauPhieuXuLyModel objThuTuc)
//        {
//            using (var trans = context.Database.BeginTransaction())
//            {
//                try
//                {
//                    QuanLyThuTucNoiBoKeHoachLuaChonNhaThau? thuTuc = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.FirstOrDefault(thuTuc => thuTuc.Id == objThuTuc.Id);
//                    if (thuTuc != null)
//                    {
//                        #region Cập nhật thông tin chung của dự án
//                        thuTuc.CoQuanGiaiQuyetHoSo = objThuTuc?.CoQuanGiaiQuyetHoSo?.Trim();
//                        thuTuc.CoQuanPhoiHop = objThuTuc?.CoQuanPhoiHop?.Trim();
//                        thuTuc.SoTaoPhieuXuLy = objThuTuc?.SoTaoPhieuXuLy?.Trim();
//                        #endregion

//                        #region Xóa danh sách dự án kèm theo cũ
//                        List<QuanLyThuTucNoiBoDuAnDtcPhieuXuLy> lstNguonVon = context.QuanLyThuTucNoiBoDuAnDtcPhieuXuLies.Where(nguonVon => nguonVon.IdThuTuc == thuTuc.Id).ToList();

//                        if (lstNguonVon != null && lstNguonVon.Count > 0)
//                        {
//                            context.QuanLyThuTucNoiBoDuAnDtcPhieuXuLies.RemoveRange(lstNguonVon);
//                        }

//                        #endregion

//                        #region Thêm mới danh sách dự án kèm theo mới
//                        if (objThuTuc?.LstPhieuXuLyThuTuc != null && objThuTuc?.LstPhieuXuLyThuTuc.Count > 0)
//                        {
//                            foreach (ThongTinLuaChonNhaThauPhieuXuLy thongTinNguonVon in objThuTuc.LstPhieuXuLyThuTuc)
//                            {
//                                QuanLyThuTucNoiBoDuAnDtcPhieuXuLy objThuTuc_NguonVonThuTuc = new()
//                                {
//                                    Id = Guid.NewGuid().ToString(),
//                                    IdThuTuc = thuTuc.Id,
//                                    BoPhanGiao = thongTinNguonVon?.BoPhanGiao,
//                                    BoPhanNhan = thongTinNguonVon?.BoPhanNhan,
//                                    NguoiGiao = thongTinNguonVon?.NguoiGiao,
//                                    NguoiNhan = thongTinNguonVon?.NguoiNhan,
//                                    GhiChu = thongTinNguonVon?.GhiChu,
//                                    NgayGiao = DateTimeOrNull(thongTinNguonVon?.NgayGiao),
//                                    TrangThai = IntOrNull(thongTinNguonVon?.TrangThai),
//                                    NgayTao = DateTime.Now,
//                                };
//                                context.QuanLyThuTucNoiBoDuAnDtcPhieuXuLies.Add(objThuTuc_NguonVonThuTuc);
//                            }
//                        }
//                        #endregion

//                        message.Title = "Cập nhật thành công";
//                        message.IsError = false;
//                        message.Code = HttpStatusCode.OK.GetHashCode();
//                        //ThemMoiNhatKy("Cập nhật dự án: " + objThuTuc?.TenDuAn?.Trim(), EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
//                        //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                        //EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
//                        //                                                                                 objThuTuc?.NguoiCapNhat);

//                        context.SaveChanges();
//                        trans.Commit();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    message.IsError = true;
//                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                    message.Title = ex.Message;
//                    trans.Rollback();
//                    //ThemMoiNhatKy("Cập nhật dự án: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
//                    //                                                            objThuTuc?.NguoiCapNhat);
//                }
//                finally
//                {
//                    trans.Dispose();
//                }
//            }
//            return message;
//        }
//        #endregion


//        #endregion

//        #region oder
//        [Route("GetDSTienDoThucHienXuLyByThuTuc/{id}")]
//        [HttpGet]
//        public ResponseMessage GetDSTienDoThucHienXuLyByThuTuc(string? id)
//        {
//            try
//            {
//                var lstTienDoThucHienXuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.Where(tienDoXuLy => tienDoXuLy.IdThuTuc == id)
//                                          .Select(tienDoXuLy => new TienDoThucHienXuLyModel
//                                          {
//                                              Id = tienDoXuLy.Id,
//                                              IdThuTuc = tienDoXuLy.IdThuTuc,
//                                              CanBoGiaiQuyet = tienDoXuLy.CanBoGiaiQuyet,
//                                              NgayGiaiQuyet = tienDoXuLy.NgayGiaiQuyet != null ? tienDoXuLy.NgayGiaiQuyet.Value.ToString("dd/MM/yyyy") : "",
//                                              NoiDungGiaiQuyet = tienDoXuLy.NoiDungGiaiQuyet,
//                                              GhiChu = tienDoXuLy.GhiChu,
//                                              TrangThai = Enums.GetEnumDescription((Enums.KetQuaTienDoThucHienXuLyThuTuc)tienDoXuLy.TrangThai.GetValueOrDefault(1)),
//                                              FileName = tienDoXuLy.FileDinhKem,
//                                              FilePath = tienDoXuLy.FilePath,
//                                          })
//                                          .ToList();

//                message.ObjData = lstTienDoThucHienXuLy;

//                message.IsError = false;
//                message.Code = HttpStatusCode.OK.GetHashCode();
//            }
//            catch (Exception ex)
//            {
//                message.IsError = true;
//                message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                message.Title = ex.Message;
//            }
//            return message;
//        }

//        [Route("GetTienDoThucHienXuLyById/{id}")]
//        [HttpGet]
//        public ResponseMessage GetTienDoThucHienXuLyById(string? id)
//        {
//            try
//            {
//                var TienDoThucHienXuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.Where(tienDoXuLy => tienDoXuLy.Id == id)
//                                          .Select(tienDoXuLy => new TienDoThucHienXuLyModel
//                                          {
//                                              Id = tienDoXuLy.Id,
//                                              IdThuTuc = tienDoXuLy.IdThuTuc,
//                                              CanBoGiaiQuyet = tienDoXuLy.CanBoGiaiQuyet,
//                                              NgayGiaiQuyet = tienDoXuLy.NgayGiaiQuyet != null ? tienDoXuLy.NgayGiaiQuyet.Value.ToString("dd/MM/yyyy") : "",
//                                              NoiDungGiaiQuyet = tienDoXuLy.NoiDungGiaiQuyet,
//                                              GhiChu = tienDoXuLy.GhiChu,
//                                              TrangThai = tienDoXuLy.TrangThai.ToString(),
//                                              FileName = tienDoXuLy.FileDinhKem,
//                                              FilePath = tienDoXuLy.FilePath,
//                                          })
//                                          .FirstOrDefault();

//                message.ObjData = TienDoThucHienXuLy;

//                message.IsError = false;
//                message.Code = HttpStatusCode.OK.GetHashCode();
//            }
//            catch (Exception ex)
//            {
//                message.IsError = true;
//                message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                message.Title = ex.Message;
//            }
//            return message;
//        }

//        [Route("GetKetQuaTrangThaiTienDoXuLyByTrangThai/{id}")]
//        [HttpGet]
//        public ResponseMessage GetKetQuaTrangThaiTienDoXuLyByTrangThai(string id)
//        {
//            try
//            {
//                List<SelectListItem> lstTrangThaiTienDoXuLy = new List<SelectListItem>();

//                foreach (Enums.KetQuaTienDoThucHienXuLyThuTuc loaiTrangThai in (Enums.KetQuaTienDoThucHienXuLyThuTuc[])Enum.GetValues(typeof(Enums.KetQuaTienDoThucHienXuLyThuTuc)))
//                {
//                    lstTrangThaiTienDoXuLy.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
//                }

//                message.ObjData = lstTrangThaiTienDoXuLy;

//                message.IsError = false;
//                message.Code = HttpStatusCode.OK.GetHashCode();
//            }
//            catch (Exception ex)
//            {
//                message.IsError = true;
//                message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                message.Title = ex.Message;
//            }
//            return message;
//        }

//        [Route("GetTrangThaiTienDoXuLy")]
//        [HttpGet]
//        public ResponseMessage GetTrangThaiTienDoXuLy()
//        {
//            try
//            {
//                List<SelectListItem> lstTrangThaiTienDoXuLy = new List<SelectListItem>();

//                foreach (Enums.KetQuaTienDoThucHienXuLyThuTuc loaiTrangThai in (Enums.KetQuaTienDoThucHienXuLyThuTuc[])Enum.GetValues(typeof(Enums.KetQuaTienDoThucHienXuLyThuTuc)))
//                {
//                    lstTrangThaiTienDoXuLy.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
//                }

//                message.ObjData = lstTrangThaiTienDoXuLy;

//                message.IsError = false;
//                message.Code = HttpStatusCode.OK.GetHashCode();
//            }
//            catch (Exception ex)
//            {
//                message.IsError = true;
//                message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                message.Title = ex.Message;
//            }
//            return message;
//        }
//        #endregion

//        #region Chuyên viên thụ lý 
//        [Route("CapNhatChuyenVienThuLy")]
//        [HttpPost]
//        public ResponseMessage CapNhatChuyenVienThuLy(ChuyenVienThuLyModels objChuyenVienThuLyModel)
//        {
//            using (var trans = context.Database.BeginTransaction())
//            {
//                try
//                {
//                    QuanLyThuTucNoiBoKeHoachLuaChonNhaThau? thuTuc = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.FirstOrDefault(thuTuc => thuTuc.Id == objChuyenVienThuLyModel.Id);
//                    if (thuTuc != null)
//                    {
//                        thuTuc.ChuyenVienThuLy = objChuyenVienThuLyModel?.ChuyenVienThuLy?.Trim();
//                        thuTuc.PhongBanThuLy = objChuyenVienThuLyModel?.PhongBanThuLy?.Trim();
//                    }

//                    message.Title = "Chuyển thụ lý thành công";
//                    message.IsError = false;
//                    message.Code = HttpStatusCode.OK.GetHashCode();
//                    //ThemMoiNhatKy("Cập nhật tài liệu: " + objTaiLieu?.TenTaiLieu?.Trim(), EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
//                    //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                    //EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
//                    //                                                                                 objTaiLieu?.NguoiCapNhat);

//                    context.SaveChanges();
//                    trans.Commit();
//                }
//                catch (Exception ex)
//                {
//                    message.IsError = true;
//                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                    message.Title = ex.Message;
//                    trans.Rollback();
//                    //ThemMoiNhatKy("Cập nhật tài liệu: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
//                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
//                    //                                                            objTaiLieu?.NguoiCapNhat);
//                }
//                finally
//                {
//                    trans.Dispose();
//                }
//            }
//            return message;
//        }
//        [Route("GetChuyenVienThuLyById/{id}")]
//        [HttpGet]
//        public ResponseMessage GetChuyenVienThuLyById(string id)
//        {
//            try
//            {
//                var chuyenVienThuLy = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Where(cv => cv.Id == id)
//                                          .Select(cv => new ChuyenVienThuLyModels
//                                          {
//                                              Id = cv.Id,
//                                              ChuyenVienThuLy = cv.ChuyenVienThuLy,
//                                              PhongBanThuLy = cv.PhongBanThuLy,
//                                          })
//                                          .FirstOrDefault();

//                message.ObjData = chuyenVienThuLy;

//                message.IsError = false;
//                message.Code = HttpStatusCode.OK.GetHashCode();
//            }
//            catch (Exception ex)
//            {
//                message.IsError = true;
//                message.Code = HttpStatusCode.BadRequest.GetHashCode();
//                message.Title = ex.Message;
//            }
//            return message;
//        }

//        #endregion

//        private bool CheckExist(string? ten, string? id)
//        {
//            int count = context.QuanLyThuTucNoiBoKeHoachLuaChonNhaThaus.Where(hoSo => hoSo.TenDuAn.ToLower().Trim() == ten.ToLower().Trim() && hoSo.Id != id).Count();
//            return count > 0;
//        }
//    }
//}
