using DTC_BE.CodeBase;
using DTC_BE.Entities;
using DTC_BE.Models;
using DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Net;
using System.Text;
using System.Web;
using static DTC_BE.CodeBase.Enums;
using static DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung.ThuTucModels;

namespace DTC_BE.Controllers.QuanlyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyThuTucNoiBoDuAnDauTuCongController : BaseApiController
    {

        #region Cấu hình đường dấu file
        private string config_UrlSave = "Uploads/QuanlyThuTucNoiBoVeDuAnDauTuCong/DungChung/QLThuTucNoiBoDuAnDTC/" + GetFolderByDate();
        private string config_UrlTemp = "Temps/QuanlyThuTucNoiBoVeDuAnDauTuCong/DungChung/QLThuTucNoiBoDuAnDTC/" + GetFolderByDate();
        #endregion

        #region other
        /// <summary>
        /// Lấy danh sách nguồn vốn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <summary>
        /// Lấy bản ghi trong Phong ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetDuAn")]
        [HttpGet]
        public ResponseMessage GetDuAn()
        {
            try
            {
                List<DmDuAn> lstDuAn = new List<DmDuAn>();

                lstDuAn = context.DmDuAns.ToList();
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = lstDuAn;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }
        [Route("GetChuDauTu")]
        [HttpGet]
        public ResponseMessage GetChuDauTu()
        {
            try
            {
                List<SelectListItem> listItems = context.DmChuDauTus
                                    .Select(x => new SelectListItem
                                    {
                                        Value = x.Id,
                                        Text = x.TenChuDauTu
                                    })
                                    .ToList();
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = listItems;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }
        [Route("GetAllNhomDuAn")]
        [HttpGet]
        public ResponseMessage GetAllNhomDuAn()
        {
            try
            {
                List<SelectListItem> nhomDuAn = new List<SelectListItem>();
                foreach (Enums.NhomDuAn loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.NhomDuAn)))
                {
                    nhomDuAn.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = nhomDuAn;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }
        [Route("GetAllTinhTrang")]
        [HttpGet]
        public ResponseMessage GetAllTinhTrang()
        {
            try
            {
                List<SelectListItem> tinhtTrangHoSo = new List<SelectListItem>();


                foreach (Enums.TINH_TRANG_HO_SO loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.TINH_TRANG_HO_SO)))
                {
                    tinhtTrangHoSo.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = tinhtTrangHoSo;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }
        [Route("GetAllChuyenVienByPhongBan")]
        [HttpGet]
        public ResponseMessage GetAllChuyenVienByPhongBan(int phongban)
        {
            try
            {
                List<SelectListItem> listItems = context.HtNguoiDungs.Where(x => x.HtNhomQuyenId == EnumAttributesHelper.GetDescription(NhomQuyen.ChuyenVienThuLy) && x.PhongBan == phongban).Select(x => new SelectListItem
                {
                    Value = x.Id,
                    Text = x.HoTen
                })
                    .ToList();
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = listItems;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }
        [Route("GetAllChuyenVien")]
        [HttpGet]
        public ResponseMessage GetAllChuyenVien()
        {
            try
            {

                List<SelectListItem> listItems = context.HtNguoiDungs
                    .Where(x => x.HtNhomQuyenId == EnumAttributesHelper.GetDescription(NhomQuyen.ChuyenVienThuLy))
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id,
                        Text = x.HoTen
                    })
                    .ToList();
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = listItems;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }
        [Route("GetAllChuyenVienXuLy")]
        [HttpGet]
        public ResponseMessage GetAllChuyenVienXuLy()
        {
            try
            {

                List<SelectListItem> listItems = context.HtNguoiDungs
                    .Where(x => x.HtNhomQuyenId == EnumAttributesHelper.GetDescription(NhomQuyen.ChuyenVienXuLy))
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id,
                        Text = x.HoTen
                    })
                    .ToList();
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = listItems;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }

        [Route("GetKetQuaThucHien")]
        [HttpGet]
        public ResponseMessage GetKetQuaThucHien()
        {
            try
            {
                List<SelectListItem> tinhTrangKqTh = new List<SelectListItem>();


                foreach (Enums.KetQuaThucHienThuTuc loaiTrangThai in (Enums.KetQuaThucHienThuTuc[])Enum.GetValues(typeof(Enums.KetQuaThucHienThuTuc)))
                {
                    tinhTrangKqTh.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = tinhTrangKqTh;
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
        [Route("GetDanhMuc")]
        [HttpGet]
        public ResponseMessage GetDanhMuc()
        {
            try
            {
                List<SelectListItem> lstTrangThai = EnumHelper.GetListSelectItemByEnums(typeof(Enums.ThuTucNoiBo_TrangThai));

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = lstTrangThai;
            }
            catch (Exception ex)
            {

                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra: " + ex.Message;
            }
            return message;

        }
        [Route("GetPhongBan")]
        [HttpGet]
        public ResponseMessage GetPhongBan()
        {
            try
            {
                List<SelectListItem> LstphongBan = EnumHelper.GetListSelectItemByEnums(typeof(Enums.PhongBan));

                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
                message.ObjData = LstphongBan;
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

        #region Upload file
        [Route("UploadFile")]
        [HttpPost]
        public async Task<ResponseMessage> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new Exception("File không tồn tại");
                }

                var allowedFileTypes = getAllowedFileExsTypes();
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedFileTypes.Contains(fileExtension))
                {
                    // Trả về thông báo lỗi nếu tệp tin không thuộc các loại được cho phép
                    throw new Exception("Định dạng file không được cho phép (cho phép: " + string.Join(",", allowedFileTypes) + " )");
                }

                string dt = DateTime.Now.ToString("ddMMyyhhmmss");
                string extens = Path.GetExtension(file.FileName);

                var fileNameSplit = ChuyenTiengVietKhongDau(Reverse(Reverse(file.FileName).Split(".")[1])).Replace(" ", "") + "_" + dt + extens;
                var urlSave = Path.Combine(Directory.GetCurrentDirectory(), config_UrlTemp);
                if (!Directory.Exists(urlSave))
                {
                    // Nếu đường dẫn không tồn tại, tạo thư mục mới
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

        [Route("GetFile")]
        [HttpGet]
        public IActionResult GetFile(string Id, int Loai)
        {
            var file = context.QuanLyThuTucFiles.FirstOrDefault(f => f.IdThuTuc == Id && f.Loai == Loai);
            if (file == null)
            {
                return NotFound("File không tồn tại");
            }
            string filePath = Directory.GetCurrentDirectory() + file.FilePath.Replace("/", "\\");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Không tìm thấy đường dẫn file"); // Trả về lỗi nếu tệp tin không tồn tại
            }

            // Đọc tệp tin và trả về dữ liệu như là một phản hồi file
            return File(System.IO.File.OpenRead(filePath), "application/octet-stream", file.FileDinhKem);
        }

        [Route("DeleteFile/{id}")]
        [HttpGet]
        public ResponseMessage DeleteFile(string pathFile)
        {
            try
            {
                string fileTemp = Directory.GetCurrentDirectory() + pathFile;
                if (System.IO.File.Exists(fileTemp))
                {
                    System.IO.File.Delete(fileTemp);
                }
                message.Title = "Xóa thành công";
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception)
            {
                message.Title = "Có lỗi xảy ra";
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
            }
            // Đọc tệp tin và trả về dữ liệu như là một phản hồi file
            return message;
        }
        #endregion

        #region CRUD thủ tục

        #region Thêm mới thủ tục
        /// <summary>
        /// Thêm mới thủ tục
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("ThemMoiThuTuc")]
        [HttpPost]
        public ResponseMessage ThemMoiThuTuc(ThuTucTDModel objThuTuc)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string pathFile1 = Directory.GetCurrentDirectory() + objThuTuc?.FilePath1;
                    FileInfo fileUpload1 = new FileInfo(pathFile1);
                    string dt = DateTime.Now.ToString("ddMMyyhhmmss");
                    var extension1 = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileDinhKem1?.Trim()).Split(".")[0]));
                    string tenHeThong1 = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileDinhKem1?.Trim()) + extension1;

                    var pathSaveFile1 = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

                    if (fileUpload1.Exists)
                    {
                        Directory.CreateDirectory(pathSaveFile1);

                        // Tạo đường dẫn đầy đủ cho file đích
                        string destinationFilePath1 = Path.Combine(pathSaveFile1, SecurityService.EncryptPassword(objThuTuc?.FileDinhKem1?.Trim()) + extension1);
                        fileUpload1.CopyTo(destinationFilePath1, true);
                        string pathFileDelete1 = Directory.GetCurrentDirectory() + objThuTuc?.FilePath1;
                        if (System.IO.File.Exists(pathFileDelete1))
                        {
                            System.IO.File.Delete(pathFileDelete1);
                        }
                    }
                    string pathFile2 = Directory.GetCurrentDirectory() + objThuTuc?.FilePath2;
                    FileInfo fileUpload2 = new FileInfo(pathFile2);
                    var extension2 = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(objThuTuc?.FileDinhKem2?.Trim()).Split(".")[0]));
                    string tenHeThong2 = "/" + config_UrlSave + SecurityService.EncryptPassword(objThuTuc?.FileDinhKem2?.Trim()) + extension2;

                    var pathSaveFile2 = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

                    if (fileUpload2.Exists)
                    {
                        Directory.CreateDirectory(pathSaveFile2);

                        // Tạo đường dẫn đầy đủ cho file đích
                        string destinationFilePath2 = Path.Combine(pathSaveFile2, SecurityService.EncryptPassword(objThuTuc?.FileDinhKem2?.Trim()) + extension2);
                        fileUpload2.CopyTo(destinationFilePath2, true);
                        string pathFileDelete2 = Directory.GetCurrentDirectory() + objThuTuc?.FilePath2;
                        if (System.IO.File.Exists(pathFileDelete2))
                        {
                            System.IO.File.Delete(pathFileDelete2);
                        }
                    }

                    QuanLyThuTucNoiBoDuAnDtc thuTuc = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TenHoSo = objThuTuc?.TenHoSo?.Trim(),
                        MaHoSo = objThuTuc?.MaHoSo?.Trim(),
                        NhomDuAn = objThuTuc?.NhomDuAn,
                        IdDonViThucHienDuAn = objThuTuc?.ChuDauTu,
                        DuKienHoanThanh = DateTimeOrNull(objThuTuc?.DuKienHoanThanh?.Trim()),
                        NgayTao = DateTimeOrNull(DateTime.Now),
                        NgayNhanHoSo = DateTimeOrNull(objThuTuc?.NgayNhanHoSo?.Trim()),
                        NguoiTao = objThuTuc?.IdUser,
                        CacThongTinKhac = objThuTuc?.CacThongTinKhac,
                        LoaiHoSo = Convert.ToInt32(objThuTuc?.LoaiHoSo != null ? objThuTuc.LoaiHoSo : "1"),
                    };

                    context.QuanLyThuTucNoiBoDuAnDtcs.Add(thuTuc);
                    if (!String.IsNullOrEmpty(objThuTuc?.FileDinhKem1?.Trim()))
                    {
                        QuanLyThuTucFile quanLyThuTucFile = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdThuTuc = thuTuc.Id,
                            Loai = 1,
                            FileDinhKem = objThuTuc?.FileDinhKem1?.Trim(),
                            FilePath = tenHeThong1,
                        };
                        context.QuanLyThuTucFiles.Add(quanLyThuTucFile);
                    }
                    if (!String.IsNullOrEmpty(objThuTuc?.FileDinhKem2?.Trim()))
                    {
                        QuanLyThuTucFile quanLyThuTucFile = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdThuTuc = thuTuc.Id,
                            Loai = 2,
                            FileDinhKem = objThuTuc?.FileDinhKem2?.Trim(),
                            FilePath = tenHeThong2,
                        };
                        context.QuanLyThuTucFiles.Add(quanLyThuTucFile);
                    }
                    QuanLyThuTucNoiBoDuAnDtcTienDoXuLy quanLyThuTucNoiBoDuAnDtcTienDoXuLy = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdThuTuc = thuTuc.Id,
                        NgayGiaiQuyet = thuTuc.NgayNhanHoSo,
                        TrangThai = (objThuTuc?.TinhTrangHoSo != null && objThuTuc?.TinhTrangHoSo != 0)
                        ? objThuTuc.TinhTrangHoSo
                        : 7,
                        GhiChuTinhTrang = !string.IsNullOrWhiteSpace(objThuTuc?.GhiChuTinhTrang)
                              ? objThuTuc.GhiChuTinhTrang.Trim()
                              : null,
                        IdChuyenVienThuLy = objThuTuc?.IdUser.Trim(),
                    };
                    context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.Add(quanLyThuTucNoiBoDuAnDtcTienDoXuLy);

                    if (!String.IsNullOrEmpty(objThuTuc?.ChuyenVienThuLy?.Trim()))
                    {
                        QuanLyThuTucNoiBoChuyenThuLy quanLyThuTucNoiBoChuyenThuLy = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdThuTuc = thuTuc.Id,
                            ChuyenVien = objThuTuc?.ChuyenVienThuLy?.Trim(),
                        };
                        string? tenNguoiDung = context.HtNguoiDungs.Where(s => s.Id == quanLyThuTucNoiBoChuyenThuLy.ChuyenVien).FirstOrDefault()?.HoTen;
                        context.QuanLyThuTucNoiBoChuyenThuLies.Add(quanLyThuTucNoiBoChuyenThuLy);
                        if (thuTuc.LoaiHoSo == 10 || thuTuc.LoaiHoSo == 11)
                        {
                            ThemMoiChuyenThuLy("Cập nhật thụ lý "+ thuTuc.TenHoSo, ((int)Enums.LoaiChucNang.Gui).ToString(),
                                                                   Enums.PhanHe.ThuTucNoiBoVeKeHoachLuaChonNhaThau.GetDescription(),
                                                                   Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                   thuTuc.NguoiTao.Trim(), quanLyThuTucNoiBoChuyenThuLy.ChuyenVien);
                        }
                        ThemMoiChuyenThuLy("Cập nhật thụ lý " + thuTuc.TenHoSo, ((int)Enums.LoaiChucNang.Gui).ToString(),
                                                                   Enums.PhanHe.QuanLyThuTucNoiBo.GetDescription(),
                                                                   Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                   thuTuc.NguoiTao.Trim(), quanLyThuTucNoiBoChuyenThuLy.ChuyenVien);
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
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        #endregion

        #region Get dữ liệu thủ tục
        [Route("GetThuTucById")]
        [HttpGet]
        public ResponseMessage GetThuTucById(string? id)
        {
            try
            {
                var file1 = context.QuanLyThuTucFiles.FirstOrDefault(x => x.IdThuTuc == id && x.Loai == 1);
                var file2 = context.QuanLyThuTucFiles.FirstOrDefault(x => x.IdThuTuc == id && x.Loai == 2);

                var chuyenVienThuLy = context.QuanLyThuTucNoiBoChuyenThuLies
                                             .Where(cv => cv.IdThuTuc == id)
                                             .OrderByDescending(cv => cv.NgayChuyenThuLy) 
                                             .FirstOrDefault();
                var xuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies
                    .Where(x => x.IdThuTuc == id)
                    .OrderByDescending(x => x.NgayGiaiQuyet)
                    .FirstOrDefault();

                var thuTuc = context.QuanLyThuTucNoiBoDuAnDtcs
                    .Where(x => x.Id == id)
                    .Select(x => new ThuTucTDModel
                    {
                        Id = x.Id,
                        TenHoSo = x.TenHoSo,
                        MaHoSo = x.MaHoSo,
                        NgayNhanHoSo = x.NgayNhanHoSo.HasValue ? x.NgayNhanHoSo.Value.ToString("dd/MM/yyyy") : "",
                        DuKienHoanThanh = x.DuKienHoanThanh.HasValue ? x.DuKienHoanThanh.Value.ToString("dd/MM/yyyy") : "",
                        ChuyenVienThuLy = chuyenVienThuLy != null ? chuyenVienThuLy.ChuyenVien : "",
                        NhomDuAn = x.NhomDuAn ?? 0,
                        TinhTrangHoSo = xuLy != null ? xuLy.TrangThai : 0,
                        GhiChuTinhTrang = xuLy != null ? xuLy.GhiChuTinhTrang : "",
                        CacThongTinKhac = x.CacThongTinKhac,
                        ChuDauTu = x.IdDonViThucHienDuAn,
                        VanBanChuDauTu = x.VanBanChuDauTu,
                        FileDinhKem1 = file1 != null ? file1.FileDinhKem : null,
                        FilePath1 = file1 != null ? file1.FilePath : null,
                        FileDinhKem2 = file2 != null ? file2.FileDinhKem : null,
                        FilePath2 = file2 != null ? file2.FilePath : null,
                        LoaiHoSo = x.LoaiHoSo,
                    })
                    .FirstOrDefault();
                var luuKho = context.QuanLyThuTucNoiBoDuAnDtcs
                    .Where(x => x.Id == id).FirstOrDefault().LuuKho;
                var chuyenVienXuLy = context.QuanLyThuTucNoiBoDuAnDtcs
                    .Where(x => x.Id == id).FirstOrDefault().NguoiTao;
                // Lấy kết quả thực hiện
                var fileKqth1 = context.QuanLyThuTucFiles.FirstOrDefault(x => x.IdThuTuc == id && x.Loai == 3);
                var fileKqth2 = context.QuanLyThuTucFiles.FirstOrDefault(x => x.IdThuTuc == id && x.Loai == 4);

                var ketQua = context.QuanLyThuTucNoiBoDuAnDtcKqths
                    .Where(x => x.IdThuTuc == id)
                    .Select(x => new KetQuaThucHien
                    {
                        Id = x.Id,
                        IdThuTuc = x.IdThuTuc,
                        SoVanBanQuyetDinh1 = x.SoNgayVb1,
                        NgayKy1 = x.NgayKy1.HasValue ? x.NgayKy1.Value.ToString("dd/MM/yyyy") : "",
                        FileNameKqth1 = fileKqth1 != null ? fileKqth1.FileDinhKem : null,
                        FilePathKqth1 = fileKqth1 != null ? fileKqth1.FilePath : null,
                        SoVanBanQuyetDinh2 = x.SoNgayVb2,
                        NgayKy2 = x.NgayKy2.HasValue ? x.NgayKy2.Value.ToString("dd/MM/yyyy") : "",
                        FileNameKqth2 = fileKqth2 != null ? fileKqth2.FileDinhKem : null,
                        FilePathKqth2 = fileKqth2 != null ? fileKqth2.FilePath : null,
                    })
                    .FirstOrDefault();

                message.ObjData = new
                {
                    thuTuc,
                    ketQua,
                    luuKho,
                    chuyenVienXuLy
                };
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

        #endregion

        #region Cập nhật thủ tục
        [Route("CapNhatThuTuc")]
        [HttpPost]
        public ResponseMessage CapNhatThuTuc(ThuTucTDModel objThuTuc)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    QuanLyThuTucNoiBoDuAnDtc? thuTuc = context.QuanLyThuTucNoiBoDuAnDtcs.FirstOrDefault(thuTuc => thuTuc.Id == objThuTuc.Id);
                    if (thuTuc != null)
                    {
                        #region Cập nhật thông tin chung của dự án
                        thuTuc.TenHoSo = objThuTuc?.TenHoSo?.Trim();
                        thuTuc.NhomDuAn = objThuTuc?.NhomDuAn;
                        thuTuc.IdDonViThucHienDuAn = objThuTuc?.ChuDauTu;
                        thuTuc.DuKienHoanThanh = DateTimeOrNull(objThuTuc?.DuKienHoanThanh?.Trim());
                        thuTuc.NgayNhanHoSo = DateTimeOrNull(objThuTuc?.NgayNhanHoSo?.Trim());
                        thuTuc.CacThongTinKhac = objThuTuc?.CacThongTinKhac;


                        QuanLyThuTucFile? thuTucFile1 = context.QuanLyThuTucFiles.FirstOrDefault(x => x.IdThuTuc == thuTuc.Id && x.Loai == 1);
                        XuLyFileThuTuc(
                            thuTucFile1,
                            objThuTuc?.IsDelete1 == true,
                            objThuTuc?.IsNew1 == true,
                            objThuTuc?.FilePath1,
                            objThuTuc?.FileDinhKem1,
                            1,
                            thuTuc.Id
                        );

                        QuanLyThuTucFile? thuTucFile2 = context.QuanLyThuTucFiles.FirstOrDefault(x => x.IdThuTuc == thuTuc.Id && x.Loai == 2);
                        XuLyFileThuTuc(
                            thuTucFile2,
                            objThuTuc?.IsDelete2 == true,
                            objThuTuc?.IsNew2 == true,
                            objThuTuc?.FilePath2,
                            objThuTuc?.FileDinhKem2,
                            2,
                            thuTuc.Id
                        );
                        #endregion

                        message.Title = "Cập nhật thành công";
                        message.IsError = false;
                        message.Code = HttpStatusCode.OK.GetHashCode();
                        context.SaveChanges();
                        trans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        #endregion
        void XuLyFileThuTuc(QuanLyThuTucFile? thuTucFile, bool isDelete, bool isNew, string? filePathTam, string? fileNameGoc, int loai, string idThuTuc)
        {
            if (thuTucFile != null)
            {
                if (isDelete)
                {
                    if (!string.IsNullOrEmpty(thuTucFile.FilePath))
                    {
                        string path = Directory.GetCurrentDirectory() + thuTucFile.FilePath;
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    thuTucFile.FileDinhKem = null;
                    thuTucFile.FilePath = null;
                }

                if (isNew)
                {
                    string pathFile = Directory.GetCurrentDirectory() + filePathTam;
                    FileInfo fileUpload = new(pathFile);
                    string dt = DateTime.Now.ToString("ddMMyyhhmmss");
                    var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(fileNameGoc?.Trim()).Split(".")[0]));
                    string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(fileNameGoc?.Trim() + dt) + extension;
                    var pathSave = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

                    if (fileUpload.Exists)
                    {
                        Directory.CreateDirectory(pathSave);
                        string destination = Path.Combine(pathSave, SecurityService.EncryptPassword(fileNameGoc?.Trim() + dt) + extension);
                        fileUpload.CopyTo(destination, true);
                        System.IO.File.Delete(pathFile);

                        thuTucFile.FilePath = tenHeThong;
                        thuTucFile.FileDinhKem = fileNameGoc?.Trim();
                    }
                }
            }
            else if (isNew)
            {
                string pathFile = Directory.GetCurrentDirectory() + filePathTam;
                FileInfo fileUpload = new(pathFile);
                string dt = DateTime.Now.ToString("ddMMyyhhmmss");
                var extension = "." + ChuyenTiengVietKhongDau(Reverse(Reverse(fileNameGoc?.Trim()).Split(".")[0]));
                string tenHeThong = "/" + config_UrlSave + SecurityService.EncryptPassword(fileNameGoc?.Trim() + dt) + extension;
                var pathSave = Directory.GetCurrentDirectory() + "/" + config_UrlSave;

                if (fileUpload.Exists)
                {
                    Directory.CreateDirectory(pathSave);
                    string destination = Path.Combine(pathSave, SecurityService.EncryptPassword(fileNameGoc?.Trim() + dt) + extension);
                    fileUpload.CopyTo(destination, true);
                    System.IO.File.Delete(pathFile);
                }

                QuanLyThuTucFile fileMoi = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    IdThuTuc = idThuTuc,
                    Loai = loai,
                    FileDinhKem = fileNameGoc?.Trim(),
                    FilePath = tenHeThong,
                };

                context.QuanLyThuTucFiles.Add(fileMoi);
            }
        }

        #region Cập nhật kết quả thực hiện thủ tục
        [Route("CapNhatKqth")]
        [HttpPost]
        public ResponseMessage CapNhatKqth(KetQuaThucHien objKQTHThuTuc)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    #region cập Nhật Kết quả thực hiện
                    QuanLyThuTucNoiBoDuAnDtcKqth? kqth = context.QuanLyThuTucNoiBoDuAnDtcKqths.FirstOrDefault(thuTuc => thuTuc.Id == objKQTHThuTuc.Id);
                    if (kqth != null)
                    {
                        kqth.NgayKy1 = DateTimeOrNull(objKQTHThuTuc?.NgayKy1?.Trim());
                        kqth.NgayKy2 = DateTimeOrNull(objKQTHThuTuc?.NgayKy2?.Trim());
                        kqth.SoNgayVb1 = objKQTHThuTuc?.SoVanBanQuyetDinh1?.Trim();
                        kqth.SoNgayVb2 = objKQTHThuTuc?.SoVanBanQuyetDinh2?.Trim();

                    }
                    else
                    {
                        QuanLyThuTucNoiBoDuAnDtcKqth kqthThemMoi = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdThuTuc = objKQTHThuTuc.IdThuTuc,
                            NgayKy1 = DateTimeOrNull(objKQTHThuTuc?.NgayKy1?.Trim()),
                            NgayKy2 = DateTimeOrNull(objKQTHThuTuc?.NgayKy2?.Trim()),
                            SoNgayVb1 = objKQTHThuTuc?.SoVanBanQuyetDinh1?.Trim(),
                            SoNgayVb2 = objKQTHThuTuc?.SoVanBanQuyetDinh2?.Trim(),
                        };
                        context.QuanLyThuTucNoiBoDuAnDtcKqths.Add(kqthThemMoi);
                    }
                    QuanLyThuTucFile? thuTucFile1 = context.QuanLyThuTucFiles.FirstOrDefault(x => x.IdThuTuc == objKQTHThuTuc.IdThuTuc && x.Loai == 3);
                    XuLyFileThuTuc(
                        thuTucFile1,
                        objKQTHThuTuc?.IsDeleteKqth1 == true,
                        objKQTHThuTuc?.IsNewKqth1 == true,
                        objKQTHThuTuc?.FilePathKqth1,
                        objKQTHThuTuc?.FileNameKqth1,
                        3,
                        objKQTHThuTuc.IdThuTuc
                    );
                    QuanLyThuTucFile? thuTucFile2 = context.QuanLyThuTucFiles.FirstOrDefault(x => x.IdThuTuc == objKQTHThuTuc.IdThuTuc && x.Loai == 4);
                    XuLyFileThuTuc(
                        thuTucFile2,
                        objKQTHThuTuc?.IsDeleteKqth2 == true,
                        objKQTHThuTuc?.IsNewKqth2 == true,
                        objKQTHThuTuc?.FilePathKqth2,
                        objKQTHThuTuc?.FileNameKqth2,
                        4,
                        objKQTHThuTuc.IdThuTuc
                    );
                    context.SaveChanges();
                    #endregion
                    message.Title = "Cập nhật thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                    context.SaveChanges();
                    trans.Commit();
                }

                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        #endregion




        #region Xóa thủ tục
        [Route("XoaThuTuc")]
        [HttpGet]
        public ResponseMessage XoaThuTuc(string? id, string? idUser)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    QuanLyThuTucNoiBoDuAnDtc? thuTuc = context.QuanLyThuTucNoiBoDuAnDtcs.FirstOrDefault(thuTuc => thuTuc.Id == id);
                    if (thuTuc != null)
                    {
                        thuTuc.IsXoa = (int)Enums.IsXoa.DaXoa;
                    }

                    message.Title = "Xóa thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }
        #endregion
        private bool CheckExist(string? ten, string? id)
        {
            int count = context.QuanLyThuTucNoiBoDuAnDtcs.Where(hoSo => hoSo.TenHoSo.ToLower().Trim() == ten.ToLower().Trim() && hoSo.Id != id).Count();
            return count > 0;
        }
        #endregion

        #region Thủ tục nội bộ dự án ĐTC Phiếu xử lý 

        #region Lấy dữ liệu phiếu xử lý
        [Route("getPhieuXuLyById")]
        [HttpGet]
        public ResponseMessage getPhieuXuLyById(string? id)
        {
            try
            {
                var thuTuc = context.QuanLyThuTucNoiBoDuAnDtcs.Where(duAn => duAn.Id == id)
                                          .Select(duAn => new ThuTucTDModel
                                          {
                                              Id = duAn.Id,
                                              NhomDuAn = duAn.NhomDuAn,
                                              TenHoSo = duAn.TenHoSo,
                                              MaHoSo = duAn.MaHoSo,
                                              LoaiHoSo = duAn.LoaiHoSo,
                                              NgayNhanHoSo = duAn.NgayNhanHoSo.HasValue ? duAn.NgayNhanHoSo.Value.ToString("dd/MM/yyyy") : "",
                                              DuKienHoanThanh = duAn.DuKienHoanThanh.HasValue ? duAn.DuKienHoanThanh.Value.ToString("dd/MM/yyyy") : "",
                                          })
                                          .FirstOrDefault();

                QuanLyThuTucNoiBoDuAnDtcPhieuXuLy phieuXuLy = context.QuanLyThuTucNoiBoDuAnDtcPhieuXuLies.FirstOrDefault(nguonVon => nguonVon.IdThuTuc == id);

                if (phieuXuLy == null)
                {
                    var abc = new QuanLyThuTucNoiBoDuAnDtcPhieuXuLy();
                    bool isLoaiHoSo_1_8_9 = thuTuc.LoaiHoSo == 1 || thuTuc.LoaiHoSo == 8 || thuTuc.LoaiHoSo == 9;
                    bool isLoaiHoSo_4_5 = thuTuc.LoaiHoSo == 4 || thuTuc.LoaiHoSo == 5;
                    bool isLoaiHoSo_6_7 = thuTuc.LoaiHoSo == 6 || thuTuc.LoaiHoSo == 7;
                    bool isLoaiHoSo_2_3 = thuTuc.LoaiHoSo == 2 || thuTuc.LoaiHoSo == 3;
                    bool isLoaiHoSo_10_11 = thuTuc.LoaiHoSo == 10 || thuTuc.LoaiHoSo == 11;

                    if ((thuTuc.NhomDuAn == 1 || thuTuc.NhomDuAn == 4) && isLoaiHoSo_1_8_9)
                    {
                        GanGiaTriABC(abc, thuTuc, 40, 1, 27, 6, 3, 2, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 25 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, ký Báo cáo thẩm định/ Văn bản";
                        abc.TextBuoc6 = "Trả kết quả";
                    }
                    else if (thuTuc.NhomDuAn == 2 && isLoaiHoSo_1_8_9)
                    {
                        GanGiaTriABC(abc, thuTuc, 30, 1, 17, 6, 3, 2, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 15 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, ký Báo cáo thẩm định/ Văn bản";
                        abc.TextBuoc6 = "Trả kết quả";
                    }
                    else if (thuTuc.NhomDuAn == 3 && isLoaiHoSo_1_8_9)
                    {
                        GanGiaTriABC(abc, thuTuc, 20, 1, 7, 6, 3, 2, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 5 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, ký Báo cáo thẩm định/ Văn bản";
                        abc.TextBuoc6 = "Trả kết quả";
                    }
                    else if ((thuTuc.NhomDuAn == 1 || thuTuc.NhomDuAn == 4) && isLoaiHoSo_4_5)
                    {
                        GanGiaTriABC(abc, thuTuc, 50, 1, 32, 9, 4, 3, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 30 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, ký Tờ trình thẩm định/ Văn bản";
                        abc.TextBuoc6 = "Trả kết quả và gửi UBT";
                    }
                    else if (thuTuc.NhomDuAn == 2 && isLoaiHoSo_4_5)
                    {
                        GanGiaTriABC(abc, thuTuc, 40, 1, 27, 6, 3, 2, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 25 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, ký Tờ trình thẩm định/ Văn bản";
                        abc.TextBuoc6 = "Trả kết quả và gửi UBT";
                    }
                    else if (thuTuc.NhomDuAn == 3 && isLoaiHoSo_4_5)
                    {
                        GanGiaTriABC(abc, thuTuc, 30, 1, 17, 6, 3, 2, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 15 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, ký Tờ trình thẩm định/ Văn bản";
                        abc.TextBuoc6 = "Trả kết quả và gửi UBT";
                    }
                    else if ((thuTuc.NhomDuAn == 1 || thuTuc.NhomDuAn == 4) && isLoaiHoSo_2_3)
                    {
                        GanGiaTriABC(abc, thuTuc, 40, 1, 27, 6, 3, 2, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 25 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, cho ý kiến";
                        abc.TextBuoc6 = "Đưa dự án vào danh mục chờ họp HĐTĐ";
                    }
                    else if ((thuTuc.NhomDuAn == 2 || thuTuc.NhomDuAn == 3) && isLoaiHoSo_2_3)
                    {
                        GanGiaTriABC(abc, thuTuc, 25, 1, 12, 6, 3, 2, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 10 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, cho ý kiến";
                        abc.TextBuoc6 = "Đưa dự án vào danh mục chờ họp HĐTĐ";
                    }
                    else if ((thuTuc.NhomDuAn == 1 || thuTuc.NhomDuAn == 4) && isLoaiHoSo_6_7)
                    {
                        GanGiaTriABC(abc, thuTuc, 45, 1, 27, 9, 4, 3, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 25 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, ký Tờ trình thẩm định/ Văn bản";
                        abc.TextBuoc6 = "Trả kết quả và gửi UBT";
                    }
                    else if ((thuTuc.NhomDuAn == 2 || thuTuc.NhomDuAn == 3) && isLoaiHoSo_6_7)
                    {
                        GanGiaTriABC(abc, thuTuc, 30, 1, 17, 6, 3, 2, 1);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Gửi lấy ý kiến các đơn vị(Gửi hồ sơ: 1 ngày; Gửi hồ sơ giấy 1 ngày; Chờ ý kiến: 15 ngày)";
                        abc.TextBuoc3 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc4 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc5 = "Xem xét kiến nghị của Phòng, ký Tờ trình thẩm định/ Văn bản";
                        abc.TextBuoc6 = "Trả kết quả và gửi UBT";
                    }
                    else if ((thuTuc.NhomDuAn == 1 || thuTuc.NhomDuAn == 4 || thuTuc.NhomDuAn == 2 || thuTuc.NhomDuAn == 3) && isLoaiHoSo_10_11)
                    {
                        GanGiaTriABC(abc, thuTuc, 20, 1, 13, 3, 2, 1, 0);
                        abc.TextBuoc1 = "Nhận hồ sơ";
                        abc.TextBuoc2 = "Thẩm định hồ sơ, trình LĐP";
                        abc.TextBuoc3 = "Kiểm tra nội dung thẩm định, trình BGĐ";
                        abc.TextBuoc4 = "Xem xét kiến nghị của Phòng, ký Quyết định/ Văn bản";
                        abc.TextBuoc5 = "Trả kết quả";
                        abc.TextBuoc6 = "";
                    }
                    phieuXuLy = abc;
                }
                else
                {
                    phieuXuLy = phieuXuLy;
                }

                message.ObjData = new
                {
                    thuTuc,
                    phieuXuLy,
                };
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
        private void GanGiaTriABC(QuanLyThuTucNoiBoDuAnDtcPhieuXuLy abc, ThuTucTDModel thuTuc, int thoiGianThucHien, int buoc1, int buoc2, int buoc3, int buoc4, int buoc5, int buoc6)
        {
            abc.Id = "1";
            abc.IdThuTuc = thuTuc.Id;
            abc.ThoiGianThucHien = thoiGianThucHien;
            abc.Buoc1 = buoc1;
            abc.Buoc2 = buoc2;
            abc.Buoc3 = buoc3;
            abc.Buoc4 = buoc4;
            abc.Buoc5 = buoc5;
            abc.Buoc6 = buoc6;

        }
        #endregion

        #region Cập Nhật Phiếu xử lý
        [Route("CapNhatPhieuXuLy")]
        [HttpPost]
        public ResponseMessage CapNhatPhieuXuLy(QuanLyThuTucNoiBoDuAnDtcPhieuXuLy objThuTucPhieuXuLy)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    var obj = context.QuanLyThuTucNoiBoDuAnDtcPhieuXuLies.FirstOrDefault(x => x.Id == objThuTucPhieuXuLy.Id);

                    if (obj != null)
                    {
                        #region Cập nhật thông tin chung của dự án
                        obj.TextBuoc1 = objThuTucPhieuXuLy?.TextBuoc1?.Trim();
                        obj.TextBuoc2 = objThuTucPhieuXuLy?.TextBuoc2?.Trim();
                        obj.TextBuoc3 = objThuTucPhieuXuLy?.TextBuoc3?.Trim();
                        obj.TextBuoc4 = objThuTucPhieuXuLy?.TextBuoc4?.Trim();
                        obj.TextBuoc5 = objThuTucPhieuXuLy?.TextBuoc5?.Trim();
                        obj.TextBuoc6 = objThuTucPhieuXuLy?.TextBuoc6?.Trim();
                        obj.Buoc1 = objThuTucPhieuXuLy?.Buoc1;
                        obj.Buoc2 = objThuTucPhieuXuLy?.Buoc2;
                        obj.Buoc3 = objThuTucPhieuXuLy?.Buoc3;
                        obj.Buoc4 = objThuTucPhieuXuLy?.Buoc4;
                        obj.Buoc5 = objThuTucPhieuXuLy?.Buoc5;
                        obj.Buoc6 = objThuTucPhieuXuLy?.Buoc6;
                        obj.ThoiGianThucHien = objThuTucPhieuXuLy?.ThoiGianThucHien;
                        #endregion
                    }
                    else
                    {
                        #region Thêm mới phiếu xử lý
                        var objnew = new QuanLyThuTucNoiBoDuAnDtcPhieuXuLy
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdThuTuc = objThuTucPhieuXuLy?.IdThuTuc?.Trim(),
                            TextBuoc1 = objThuTucPhieuXuLy?.TextBuoc1?.Trim(),
                            TextBuoc2 = objThuTucPhieuXuLy?.TextBuoc2?.Trim(),
                            TextBuoc3 = objThuTucPhieuXuLy?.TextBuoc3?.Trim(),
                            TextBuoc4 = objThuTucPhieuXuLy?.TextBuoc4?.Trim(),
                            TextBuoc5 = objThuTucPhieuXuLy?.TextBuoc5?.Trim(),
                            TextBuoc6 = objThuTucPhieuXuLy?.TextBuoc6?.Trim(),
                            Buoc1 = objThuTucPhieuXuLy?.Buoc1,
                            Buoc2 = objThuTucPhieuXuLy?.Buoc2,
                            Buoc3 = objThuTucPhieuXuLy?.Buoc3,
                            Buoc4 = objThuTucPhieuXuLy?.Buoc4,
                            Buoc5 = objThuTucPhieuXuLy?.Buoc5,
                            Buoc6 = objThuTucPhieuXuLy?.Buoc6,
                            ThoiGianThucHien = objThuTucPhieuXuLy?.ThoiGianThucHien
                        };
                        context.QuanLyThuTucNoiBoDuAnDtcPhieuXuLies.Add(objnew);
                        #endregion
                    }

                    context.SaveChanges(); // Lưu tất cả thay đổi ở đây
                    trans.Commit();

                    message.Title = "Lập phiếu xử lý thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
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
        #endregion
        #region Xuất word
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuanLyThuTucNoiBoDuAnDauTuCongController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("ExportWordLapPhieuXuLy")]
        [HttpPost] 
        public IActionResult ExportWordLapPhieuXuLy([FromBody] ThongTinPhieuXuLy objThuTucPhieuXuLy)
        {
            try
            {
                var thuTuc = context.QuanLyThuTucNoiBoDuAnDtcs.Where(duAn => duAn.Id == objThuTucPhieuXuLy.IdThuTuc)
                                          .Select(duAn => new ThuTucTDModel
                                          {
                                              Id = duAn.Id,
                                              NhomDuAn = duAn.NhomDuAn,
                                              TenHoSo = duAn.TenHoSo,
                                              MaHoSo = duAn.MaHoSo,
                                              LoaiHoSo = duAn.LoaiHoSo,
                                              NgayNhanHoSo = duAn.NgayNhanHoSo.HasValue ? duAn.NgayNhanHoSo.Value.ToString("dd/MM/yyyy") : "",
                                              DuKienHoanThanh = duAn.DuKienHoanThanh.HasValue ? duAn.DuKienHoanThanh.Value.ToString("dd/MM/yyyy") : "",
                                          })
                                          .FirstOrDefault();
                var objnew = new ThongTinPhieuXuLy
                {
                    IdThuTuc = objThuTucPhieuXuLy?.IdThuTuc,
                    TextBuoc1 = objThuTucPhieuXuLy?.TextBuoc1,
                    TextBuoc2 = objThuTucPhieuXuLy?.TextBuoc2,
                    TextBuoc3 = objThuTucPhieuXuLy?.TextBuoc3,
                    TextBuoc4 = objThuTucPhieuXuLy?.TextBuoc4,
                    TextBuoc5 = objThuTucPhieuXuLy?.TextBuoc5,
                    TextBuoc6 = objThuTucPhieuXuLy?.TextBuoc6,
                    Buoc1 = objThuTucPhieuXuLy?.Buoc1,
                    Buoc2 = objThuTucPhieuXuLy?.Buoc2,
                    Buoc3 = objThuTucPhieuXuLy?.Buoc3,
                    Buoc4 = objThuTucPhieuXuLy?.Buoc4,
                    Buoc5 = objThuTucPhieuXuLy?.Buoc5,
                    Buoc6 = objThuTucPhieuXuLy?.Buoc6,
                    NgayBuoc1 = objThuTucPhieuXuLy?.NgayBuoc1,
                    NgayBuoc2 = objThuTucPhieuXuLy?.NgayBuoc2,
                    NgayBuoc3 = objThuTucPhieuXuLy?.NgayBuoc3,
                    NgayBuoc4 = objThuTucPhieuXuLy?.NgayBuoc4,
                    NgayBuoc5 = objThuTucPhieuXuLy?.NgayBuoc5,
                    NgayBuoc6 = objThuTucPhieuXuLy?.NgayBuoc6,
                    ThoiGianThucHien = objThuTucPhieuXuLy?.ThoiGianThucHien
                };
                List<SelectListItem> LstnhomDuAn = new List<SelectListItem>();


                foreach (Enums.NhomDuAn loaiTrangThai in (Enums.NhomDuAn[])Enum.GetValues(typeof(Enums.NhomDuAn)))
                {
                    LstnhomDuAn.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                List<SelectListItem> LstLoaihoSo = new List<SelectListItem>();


                foreach (Enums.LoaiThuTuNoiBoDuAnDTC loaiTrangThai in (Enums.LoaiThuTuNoiBoDuAnDTC[])Enum.GetValues(typeof(Enums.LoaiThuTuNoiBoDuAnDTC)))
                {
                    LstLoaihoSo.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                string TenTatThuTuc = "";

                if(thuTuc.LoaiHoSo == 1)
                {
                    TenTatThuTuc = "DTCBDT_";
                }if(thuTuc.LoaiHoSo == 2)
                {
                    TenTatThuTuc = "CTDT_";
                }if(thuTuc.LoaiHoSo == 3)
                {
                    TenTatThuTuc = "CTDT_DC_";
                }if(thuTuc.LoaiHoSo == 4)
                {
                    TenTatThuTuc = "DA_CPXD_";
                }if(thuTuc.LoaiHoSo == 5)
                {
                    TenTatThuTuc = "DC_DA_CPXD_";
                }if(thuTuc.LoaiHoSo == 6)
                {
                    TenTatThuTuc = "DA_KCCPXD_";
                }if(thuTuc.LoaiHoSo == 7)
                {
                    TenTatThuTuc = "DC_DA_KCCPXD_";
                }if(thuTuc.LoaiHoSo == 8)
                {
                    TenTatThuTuc = "DT_DA_KCCPXD_";
                }if(thuTuc.LoaiHoSo == 9)
                {
                    TenTatThuTuc = "DT_DCDA_KCCPXD_";
                }if(thuTuc.LoaiHoSo == 10)
                {
                    TenTatThuTuc = "KH_LCNT_";
                }if(thuTuc.LoaiHoSo == 11)
                {
                    TenTatThuTuc = "KH_LCNT_DC_";
                }
                string strContent = "";
                string contentType = "";
                string filenameOutput = "";
                if (thuTuc.LoaiHoSo == 10 || thuTuc.LoaiHoSo == 11)
                {
                    string fileName = $"PhieuXL_TTTD_{thuTuc.TenHoSo}_{DateTime.Now:ddMMyyyyhhmmss}.doc";
                    filenameOutput = "PhieuXL_TTTD_" + TenTatThuTuc + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".doc";
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; // Định nghĩa content type cho file Word

                    // Đường dẫn tới file XML
                    string strTemp = _webHostEnvironment.ContentRootPath + @"/XML/ThuTucNoiBoDuAnDauTuCong/PhieuXuLyLCNT.xml";
                    string strTemplateFull = ReadContentDataFromFile(strTemp);

                    String tenNhomDuAn = (LstLoaihoSo.FirstOrDefault(x => x.Value == thuTuc.LoaiHoSo.ToString())?.Text.ToUpper()) + " (" + (LstnhomDuAn.FirstOrDefault(x => x.Value == thuTuc.NhomDuAn.ToString())?.Text.ToUpper() ?? "") + ")";

                    strContent = strTemplateFull.Replace("{TEN_THU_TUC}", HttpUtility.HtmlEncode(tenNhomDuAn))
                                                       .Replace("{MA_HO_SO}", HttpUtility.HtmlEncode(thuTuc.MaHoSo))
                                                       .Replace("{TEN_HO_SO}", HttpUtility.HtmlEncode(thuTuc.TenHoSo))
                                                       .Replace("{TGTH}", objnew.ThoiGianThucHien.ToString())
                                                       .Replace("{Buoc_1}", objnew.Buoc1.ToString())
                                                       .Replace("{Buoc_2}", objnew.Buoc2.ToString())
                                                       .Replace("{Buoc_3}", objnew.Buoc3.ToString())
                                                       .Replace("{Buoc_4}", objnew.Buoc4.ToString())
                                                       .Replace("{Buoc_5}", objnew.Buoc5.ToString())
                                                       .Replace("{Text_1}", objnew.TextBuoc1.ToString() ?? "")
                                                       .Replace("{Text_2}", objnew.TextBuoc2.ToString() ?? "")
                                                       .Replace("{Text_3}", objnew.TextBuoc3.ToString() ?? "")
                                                       .Replace("{Text_4}", objnew.TextBuoc4.ToString() ?? "")
                                                       .Replace("{Text_5}", objnew.TextBuoc5.ToString() ?? "")
                                                       .Replace("{Ngay_Buoc_1}", objnew.NgayBuoc1 ?? "")
                                                       .Replace("{Ngay_Buoc_2}", objnew.NgayBuoc2 ?? "")
                                                       .Replace("{Ngay_Buoc_3}", objnew.NgayBuoc3 ?? "")
                                                       .Replace("{Ngay_Buoc_4}", objnew.NgayBuoc4 ?? "")
                                                       .Replace("{Ngay_Buoc_5}", objnew.NgayBuoc5 ?? "")
                                                       .Replace("{choykien}", (Convert.ToInt32(objnew.Buoc2) - 2).ToString());
                }
                else {    
                string fileName = $"PhieuXL_TTTD_{thuTuc.TenHoSo}_{DateTime.Now:ddMMyyyyhhmmss}.doc";
                filenameOutput = "PhieuXL_TTTD_"+ TenTatThuTuc + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".doc";
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; // Định nghĩa content type cho file Word

                // Đường dẫn tới file XML
                string strTemp = _webHostEnvironment.ContentRootPath + @"/XML/ThuTucNoiBoDuAnDauTuCong/PhieuXuLy.xml";
                string strTemplateFull = ReadContentDataFromFile(strTemp);

                String tenNhomDuAn = (LstLoaihoSo.FirstOrDefault(x => x.Value == thuTuc.LoaiHoSo.ToString())?.Text.ToUpper()) + " (" + (LstnhomDuAn.FirstOrDefault(x => x.Value == thuTuc.NhomDuAn.ToString())?.Text.ToUpper() ?? "") + ")";

                strContent = strTemplateFull.Replace("{TEN_THU_TUC}", HttpUtility.HtmlEncode(tenNhomDuAn))
                                                   .Replace("{MA_HO_SO}", HttpUtility.HtmlEncode(thuTuc.MaHoSo))
                                                   .Replace("{TEN_HO_SO}", HttpUtility.HtmlEncode(thuTuc.TenHoSo))
                                                   .Replace("{TGTH}", objnew.ThoiGianThucHien.ToString())
                                                   .Replace("{Buoc_1}", objnew.Buoc1.ToString())
                                                   .Replace("{Buoc_2}", objnew.Buoc2.ToString())
                                                   .Replace("{Buoc_3}", objnew.Buoc3.ToString())
                                                   .Replace("{Buoc_4}", objnew.Buoc4.ToString())
                                                   .Replace("{Buoc_5}", objnew.Buoc5.ToString())
                                                   .Replace("{Buoc_6}", objnew.Buoc6.ToString())
                                                   .Replace("{Text_1}", objnew.TextBuoc1.ToString() ?? "")
                                                   .Replace("{Text_2}", objnew.TextBuoc2.ToString() ?? "")
                                                   .Replace("{Text_3}", objnew.TextBuoc3.ToString() ?? "")
                                                   .Replace("{Text_4}", objnew.TextBuoc4.ToString() ?? "")
                                                   .Replace("{Text_5}", objnew.TextBuoc5.ToString() ?? "")
                                                   .Replace("{Text_6}", objnew.TextBuoc6.ToString() ?? "")
                                                   .Replace("{Ngay_Buoc_1}", objnew.NgayBuoc1 ?? "")
                                                   .Replace("{Ngay_Buoc_2}", objnew.NgayBuoc2 ?? "")
                                                   .Replace("{Ngay_Buoc_3}", objnew.NgayBuoc3 ?? "")
                                                   .Replace("{Ngay_Buoc_4}", objnew.NgayBuoc4 ?? "")
                                                   .Replace("{Ngay_Buoc_5}", objnew.NgayBuoc5 ?? "")
                                                   .Replace("{Ngay_Buoc_6}", objnew.NgayBuoc6 ?? "")
                                                   .Replace("{choykien}", (Convert.ToInt32(objnew.Buoc2) - 2).ToString());
                }

                #endregion
                // Khởi tạo MemoryStream và ghi nội dung vào đó
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(strContent);
                writer.Flush();
                stream.Position = 0; // Đặt vị trí của stream về đầu
                                     // Trả về file dưới dạng FileContentResult
                return File(stream.ToArray(), contentType, filenameOutput);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        [Route("GetTenTrangThai")]
        [NonAction]
        public string GetTenTrangThai(int? TrangThai)
        {
            string result = string.Empty;
            if (TrangThai != null && TrangThai == 1)
            {
                result = "Trước hạn";
            }
            else if (TrangThai != null && TrangThai == 2)
            {
                result = "Đúng hạn";
            }
            else if (TrangThai != null && TrangThai == 3)
            {
                result = "Quá hạn";
            }
            else
                result = "";

            return result;
        }

        [Route("CheckStringOrEmpty")]
        [NonAction]
        public string CheckStringOrEmpty(string input)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(input))
                result = input;
            else
                result = "";

            return result;
        }

        [Route("ReadContentDataFromFile")]
        [NonAction]
        public static string ReadContentDataFromFile(string _fileName)
        {
            FileStream objFileStream = null;
            try
            {
                objFileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
                StreamReader objReader = new StreamReader(objFileStream);
                string s = objReader.ReadToEnd();
                return s;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objFileStream != null)
                    objFileStream.Close();
            }
        }
        #endregion


        #region CRUD QLThuTucTienDoXuLy
        [Route("ThemMoiTienDoXuLy")]
        [HttpPost]
        public ResponseMessage ThemMoiTienDoXuLy(TienDoThucHienXuLyModel objTienDoXuLy)
        {
            try
            {
                QuanLyThuTucNoiBoDuAnDtcTienDoXuLy tienDoXuLy = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    IdThuTuc = objTienDoXuLy?.IdThuTuc?.Trim(),
                    GhiChuTinhTrang = objTienDoXuLy?.GhiChu?.Trim(),
                    TrangThai = Convert.ToInt32(objTienDoXuLy?.TrangThai),
                    NgayGiaiQuyet = DateTimeOrNull(objTienDoXuLy?.NgayGiaiQuyet?.Trim()),
                    IdChuyenVienThuLy = objTienDoXuLy?.CanBoGiaiQuyet?.Trim(),
                    SoNgayQuyetDinh = objTienDoXuLy?.NoiDungGiaiQuyet?.Trim(),
                };

                context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.Add(tienDoXuLy);
                message.Title = "Cập nhật tiến độ xử lý thành công";
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();


                context.SaveChanges();
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = ex.Message;
                //ThemMoiNhatKy("Thêm mới tài liệu: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.ThemMoi),
                //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                //                                                            objTaiLieu?.NguoiTao);
            }

            return message;
        }
        #endregion

        #region oder
        [Route("GetDSTienDoThucHienXuLyByThuTuc/{id}")]
        [HttpGet]
        public ResponseMessage GetDSTienDoThucHienXuLyByThuTuc(string? id)
        {
            try
            {
                var lstTienDoThucHienXuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.Where(tienDoXuLy => tienDoXuLy.IdThuTuc == id)
                                          .Select(tienDoXuLy => new TienDoThucHienXuLyModel
                                          {
                                              Id = tienDoXuLy.Id,
                                              IdThuTuc = tienDoXuLy.IdThuTuc,
                                              NgayGiaiQuyet = tienDoXuLy.NgayGiaiQuyet != null ? tienDoXuLy.NgayGiaiQuyet.Value.ToString("dd/MM/yyyy") : "",
                                              GhiChu = tienDoXuLy.GhiChuTinhTrang,
                                              TrangThai = tienDoXuLy.TrangThai > 0 ? Enums.GetEnumDescription((Enums.TINH_TRANG_HO_SO)tienDoXuLy.TrangThai) : "Chưa cập nhật tiến độ",
                                              CanBoGiaiQuyet = context.HtNguoiDungs.FirstOrDefault(x => x.Id == tienDoXuLy.IdChuyenVienThuLy).HoTen,
                                              NoiDungGiaiQuyet = tienDoXuLy.SoNgayQuyetDinh,
                                          })
                                          .ToList();

                message.ObjData = lstTienDoThucHienXuLy;

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

        [Route("GetTienDoThucHienXuLyById/{id}")]
        [HttpGet]
        public ResponseMessage GetTienDoThucHienXuLyById(string? id)
        {
            try
            {
                var TienDoThucHienXuLy = context.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies.Where(tienDoXuLy => tienDoXuLy.Id == id)
                                          .Select(tienDoXuLy => new TienDoThucHienXuLyModel
                                          {
                                              Id = tienDoXuLy.Id,
                                              IdThuTuc = tienDoXuLy.IdThuTuc,
                                              NgayGiaiQuyet = tienDoXuLy.NgayGiaiQuyet != null ? tienDoXuLy.NgayGiaiQuyet.Value.ToString("dd/MM/yyyy") : "",
                                              GhiChu = tienDoXuLy.GhiChuTinhTrang,
                                              TrangThai = tienDoXuLy.TrangThai.ToString(),
                                          })
                                          .FirstOrDefault();

                message.ObjData = TienDoThucHienXuLy;

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

        [Route("GetKetQuaTrangThaiTienDoXuLyByTrangThai/{id}")]
        [HttpGet]
        public ResponseMessage GetKetQuaTrangThaiTienDoXuLyByTrangThai(string id)
        {
            try
            {
                List<SelectListItem> lstTrangThaiTienDoXuLy = new List<SelectListItem>();

                foreach (Enums.TINH_TRANG_HO_SO loaiTrangThai in (Enums.TINH_TRANG_HO_SO[])Enum.GetValues(typeof(Enums.TINH_TRANG_HO_SO)))
                {
                    lstTrangThaiTienDoXuLy.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }

                message.ObjData = lstTrangThaiTienDoXuLy;

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

        [Route("GetTrangThaiTienDoXuLy")]
        [HttpGet]
        public ResponseMessage GetTrangThaiTienDoXuLy()
        {
            try
            {
                List<SelectListItem> lstTrangThaiTienDoXuLy = new List<SelectListItem>();

                foreach (Enums.TINH_TRANG_HO_SO loaiTrangThai in (Enums.TINH_TRANG_HO_SO[])Enum.GetValues(typeof(Enums.TINH_TRANG_HO_SO)))
                {
                    lstTrangThaiTienDoXuLy.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }

                message.ObjData = lstTrangThaiTienDoXuLy;

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
        #endregion

        #region Chuyên viên thụ lý
        [Route("GetChuyenVienThuLy/{id}")]
        [HttpGet]
        public ResponseMessage GetChuyenVienThuLy(string id)
        {
            try
            {
                List<HtNguoiDung> lstNguoiDung = context.HtNguoiDungs.ToList();
                List<SelectListItem> LstPhongban = new List<SelectListItem>();
                foreach (Enums.PhongBan loaiTrangThai in (Enums.PhongBan[])Enum.GetValues(typeof(Enums.PhongBan)))
                {
                    LstPhongban.Add(new SelectListItem { Text = loaiTrangThai.GetDescription(), Value = loaiTrangThai.GetHashCode().ToString() });
                }
                var lstchuyenVienThuLy = context.QuanLyThuTucNoiBoChuyenThuLies
                                                .Where(cv => cv.IdThuTuc == id)
                                                .OrderByDescending(cv => cv.NgayChuyenThuLy)
                                                .AsEnumerable()
                                                .Select(cv => new ChuyenVienThuLy
                                                {
                                                    Id = cv.Id,
                                                    ChuyenVien = lstNguoiDung.FirstOrDefault(x => x.Id == cv.ChuyenVien)?.HoTen ?? "", // Nếu không tìm thấy tên chuyên viên, đặt giá trị là rỗng
                                                    PhongBan = LstPhongban.FirstOrDefault(x => x.Value == cv.PhongBan?.ToString())?.Text ?? "", // Nếu không tìm thấy phòng ban, đặt giá trị là rỗng
                                                    NgayTao = cv.NgayChuyenThuLy != null ? cv.NgayChuyenThuLy.Value.ToString("dd/MM/yyyy") : "",
                                                })
                                                .Where(c => !string.IsNullOrEmpty(c.ChuyenVien) && !string.IsNullOrEmpty(c.PhongBan)) // Loại bỏ các mục có giá trị rỗng cho ChuyenVien hoặc PhongBan
                                                .ToList();

                message.ObjData = lstchuyenVienThuLy;

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

        [Route("CapNhatChuyenVienThuLy")]
        [HttpPost]
        public ResponseMessage CapNhatChuyenVienThuLy(ChuyenVienThuLyModels objChuyenVienThuLyModel)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    QuanLyThuTucNoiBoChuyenThuLy thuTuc = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdThuTuc = objChuyenVienThuLyModel?.Id?.Trim(),
                        ChuyenVien = objChuyenVienThuLyModel?.ChuyenVienThuLy?.Trim(),
                        PhongBan = objChuyenVienThuLyModel?.PhongBanThuLy?.Trim(),
                        NgayChuyenThuLy = DateTime.Now,
                    };
                    var QuanLyThuTucNoiBoDuAnDtc = context.QuanLyThuTucNoiBoDuAnDtcs.FirstOrDefault(x => x.Id == objChuyenVienThuLyModel.Id.Trim());
                    string? tenNguoiDung = context.HtNguoiDungs.Where(s => s.Id == thuTuc.ChuyenVien).FirstOrDefault()?.HoTen;
                    context.QuanLyThuTucNoiBoChuyenThuLies.Add(thuTuc);
                    context.SaveChanges();

                    message.Title = "Chuyển thụ lý thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                    if(QuanLyThuTucNoiBoDuAnDtc.LoaiHoSo == 10 || QuanLyThuTucNoiBoDuAnDtc.LoaiHoSo == 11)
                    {
                        ThemMoiChuyenThuLy("Cập nhật thụ lý "+ QuanLyThuTucNoiBoDuAnDtc.TenHoSo, ((int)Enums.LoaiChucNang.Gui).ToString(),
                                                               Enums.PhanHe.ThuTucNoiBoVeKeHoachLuaChonNhaThau.GetDescription(),
                                                               Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                               QuanLyThuTucNoiBoDuAnDtc.NguoiTao.Trim(), thuTuc.ChuyenVien);
                    }
                    ThemMoiChuyenThuLy("Cập nhật thụ lý " + QuanLyThuTucNoiBoDuAnDtc.TenHoSo, ((int)Enums.LoaiChucNang.Gui).ToString(),
                                                               Enums.PhanHe.QuanLyThuTucNoiBo.GetDescription(),
                                                               Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                               QuanLyThuTucNoiBoDuAnDtc.NguoiTao.Trim(), thuTuc.ChuyenVien);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    //ThemMoiNhatKy("Cập nhật tài liệu: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                                            objTaiLieu?.NguoiCapNhat);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("CapNhatChuyenVienThuLyById")]
        [HttpPost]
        public ResponseMessage CapNhatChuyenVienThuLyById(ChuyenVienThuLyModels objChuyenVienThuLyModel)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    QuanLyThuTucNoiBoChuyenThuLy? thuTuc = context.QuanLyThuTucNoiBoChuyenThuLies.FirstOrDefault(thuTuc => thuTuc.Id == objChuyenVienThuLyModel.Id);
                    if (thuTuc != null)
                    {
                        thuTuc.ChuyenVien = objChuyenVienThuLyModel?.ChuyenVienThuLy?.Trim();
                        thuTuc.PhongBan = objChuyenVienThuLyModel?.PhongBanThuLy?.Trim();
                        thuTuc.NgayChuyenThuLy = DateTime.Now;
                    }

                    message.Title = "Chuyển thụ lý thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                    //ThemMoiNhatKy("Cập nhật tài liệu: " + objTaiLieu?.TenTaiLieu?.Trim(), EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
                    //                                                                                 objTaiLieu?.NguoiCapNhat);

                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    //ThemMoiNhatKy("Cập nhật tài liệu: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                                            objTaiLieu?.NguoiCapNhat);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("GetChuyenVienThuLyById/{id}")]
        [HttpGet]
        public ResponseMessage GetChuyenVienThuLyById(string id)
        {
            try
            {
                var chuyenVienThuLy = context.QuanLyThuTucNoiBoChuyenThuLies.Where(cv => cv.Id == id)
                                          .Select(cv => new ChuyenVienThuLyModels
                                          {
                                              Id = cv.Id,
                                              ChuyenVienThuLy = cv.ChuyenVien,
                                              PhongBanThuLy = cv.PhongBan,
                                          })
                                          .FirstOrDefault();

                message.ObjData = chuyenVienThuLy;

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

        #endregion

    }
}
