using DTC_BE.CodeBase;
using DTC_BE.Entities;
using DTC_BE.Models;
using DTC_BE.Models.HeThong.NhomQuyen;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DTC_BE.Controllers.HeThong
{
    [Route("api/HeThong/[controller]")]
    [ApiController]
    public class NhomQuyenController : BaseApiController
    {
        #region Tìm kiếm nhóm quyền
        [Route("GetDanhSachNhomQuyen")]
        [HttpPost]
        public ResponseMessage GetDanhSachNhomQuyen(TimKiemDanhSachNhomQuyen timKiemDanhSach)
        {
            try
            {
                List<HtNhomQuyen> lstNhomQuyen = new();
                lstNhomQuyen = context.HtNhomQuyens.Where(nhomQuyen => (!string.IsNullOrEmpty(timKiemDanhSach.TenNhomQuyen) ?
                                                                           nhomQuyen.Ten.ToLower().Trim().Contains(timKiemDanhSach.TenNhomQuyen.ToLower().Trim()) : true)
                                                                           && (!string.IsNullOrEmpty(timKiemDanhSach.MoTa) ?
                                                                           nhomQuyen.MoTa.ToLower().Trim().Contains(timKiemDanhSach.MoTa.ToLower().Trim()) : true))
                                                       .OrderBy(nhomQuyen => nhomQuyen.Ten)
                                                       .Skip(timKiemDanhSach.RowPerPage * (timKiemDanhSach.CurrentPage - 1))
                                                       .Take(timKiemDanhSach.RowPerPage)
                                                       .ToList();
                int totalRecords = context.HtNhomQuyens.Where(nhomQuyen => (!string.IsNullOrEmpty(timKiemDanhSach.TenNhomQuyen) ?
                                                                           nhomQuyen.Ten.ToLower().Trim().Contains(timKiemDanhSach.TenNhomQuyen.ToLower().Trim()) : true)
                                                                           && (!string.IsNullOrEmpty(timKiemDanhSach.MoTa) ?
                                                                           nhomQuyen.MoTa.ToLower().Trim().Contains(timKiemDanhSach.MoTa.ToLower().Trim()) : true)).Count();
                message.IsError = false;
                message.ObjData = new { lstNhomQuyen, totalRecords };
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

        #region CRUD nhóm quyền
        [Route("ThemMoiNhomQuyen")]
        [HttpPost]
        public ResponseMessage ThemMoiNhomQuyen(HtNhomQuyen objNhomQuyen)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (CheckExist(objNhomQuyen?.Ten, objNhomQuyen?.Id))
                    {
                        throw new Exception("Tên nhóm quyền: " + objNhomQuyen?.Ten?.Trim() + " đã tồn tại, Vui lòng kiểm tra lại");
                    }

                    HtNhomQuyen nhomQuyen = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Ten = objNhomQuyen?.Ten?.Trim(),
                        MoTa = objNhomQuyen?.MoTa?.Trim(),
                    };

                    context.HtNhomQuyens.Add(nhomQuyen);

                    message.Title = "Thêm mới thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    //ThemMoiNhatKy("Thêm mới nhóm quyền: " + objNhomQuyen?.TenNhomQuyen?.Trim(), EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.ThemMoi),
                    //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                                                                 EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
                    //                                                                                 objNhomQuyen?.NguoiTao);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    //ThemMoiNhatKy("Thêm mới nhóm quyền: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.ThemMoi),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                                            objNhomQuyen?.NguoiTao);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("CapNhatNhomQuyen")]
        [HttpPost]
        public ResponseMessage CapNhatNhomQuyen(HtNhomQuyen objNhomQuyen)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (CheckExist(objNhomQuyen?.Ten, objNhomQuyen?.Id))
                    {
                        throw new Exception("Tên nhóm quyền: " + objNhomQuyen?.Ten?.Trim() + " đã tồn tại, Vui lòng kiểm tra lại");
                    }

                    HtNhomQuyen? nhomQuyen = context.HtNhomQuyens.FirstOrDefault(nhomQuyen => nhomQuyen.Id == objNhomQuyen.Id);
                    if (nhomQuyen != null)
                    {
                        nhomQuyen.Ten = objNhomQuyen?.Ten?.Trim();
                        nhomQuyen.MoTa = objNhomQuyen?.MoTa?.Trim();

                    }

                    message.Title = "Cập nhật thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                    //ThemMoiNhatKy("Cập nhật nhóm quyền: " + objNhomQuyen?.TenNhomQuyen?.Trim(), EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
                    //                                                                                 objNhomQuyen?.NguoiCapNhat);

                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    //ThemMoiNhatKy("Cập nhật nhóm quyền: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                                            objNhomQuyen?.NguoiCapNhat);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("XoaNhomQuyen")]
        [HttpGet]
        public ResponseMessage XoaNhomQuyen(string? id, string? idUser)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    HtNhomQuyen? nhomQuyen = context.HtNhomQuyens.FirstOrDefault(nhomQuyen => nhomQuyen.Id == id);
                    List<HtNguoiDung> htNguoiDung = context.HtNguoiDungs.Where(x => x.HtNhomQuyenId == id).ToList();
                    List<HtQuyenNhomQuyen> HtQuyenNhomQuyen = context.HtQuyenNhomQuyens.Where(x => x.HtNhomQuyenId == id).ToList();
                    if (nhomQuyen != null)
                    {
                        context.HtQuyenNhomQuyens.RemoveRange(HtQuyenNhomQuyen);
                        context.HtNguoiDungs.RemoveRange(htNguoiDung);
                        context.HtNhomQuyens.Remove(nhomQuyen);
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
                    //ThemMoiNhatKy("Xóa nhóm quyền: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.Xoa),
                    //                                                       EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                                       EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                                       idUser);
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
        [Route("GetNhomQuyenById/{id}")]
        [HttpGet]
        public ResponseMessage GetNhomQuyenById(string? id)
        {
            try
            {
                var nhomQuyen = context.HtNhomQuyens.Where(nhomQuyen => nhomQuyen.Id == id)
                                                    .Select(nhomQuyen => new
                                                    {
                                                        nhomQuyen.Id,
                                                        nhomQuyen.Ten,
                                                        nhomQuyen.MoTa
                                                    })
                                                    .FirstOrDefault();

                message.ObjData = nhomQuyen;
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
            int count = 0;
            if (!string.IsNullOrEmpty(id))
            {
                count = context.HtNhomQuyens.Where(nhomQuyen => nhomQuyen.Ten.ToLower().Trim() == ten.ToLower().Trim() && nhomQuyen.Id != id).Count();
            }
            return count > 0;
        }
        #endregion

        #region Lấy danh sách quyền
        [Route("GetDanhSachQuyenDuocCap/{idNhomQuyen}")]
        [HttpGet]
        public ResponseMessage GetDanhSachQuyenDuocCap(string? idNhomQuyen)
        {
            try
            {
                List<string> lstMaPhanCapTmp = new List<string>();
                var lstAllQuyen = context.HtQuyens.ToList();//lấy toàn bộ danh sách quyền
                var lstQuyenNhomQuyen = context.HtQuyenNhomQuyens.Where(nhomQuyen => nhomQuyen.HtNhomQuyenId == idNhomQuyen).ToList(); //lấy ra toàn bộ quyền từng nút trong nhóm
                List<string?> lstQuyenId = lstQuyenNhomQuyen.Select(p => p.HtQuyenId)
                                                         .Distinct()
                                                         .ToList(); // lọc ra quyền

                var lstQuyen = lstAllQuyen.Where(quyen => lstQuyenId.Contains(quyen.Id)).ToList(); // lọc ra những quyền đang chưa thêm
                var lstMaPhanCap = lstQuyen.Select(quyen => quyen.Ma)
                                                    .Distinct()
                                                    .ToList(); // lấy mã phân cấp cha con

                if (lstMaPhanCap != null && lstMaPhanCap.Count > 0)
                {
                    foreach (var maPhanCap in lstMaPhanCap)
                    {
                        if (maPhanCap != null)
                        {
                            string strMaPhanCap = maPhanCap;
                            if (!lstMaPhanCapTmp.Contains(strMaPhanCap))
                                lstMaPhanCapTmp.Add(strMaPhanCap); // chưa có thì add vào list
                            while (strMaPhanCap.Length > 3)
                            {
                                strMaPhanCap = strMaPhanCap.Substring(0, strMaPhanCap.Length - 3);
                                if (!lstMaPhanCapTmp.Contains(strMaPhanCap))
                                    lstMaPhanCapTmp.Add(strMaPhanCap); // lấy ra quyền cha
                            }
                        }
                    }
                }
                //lấy danh sách menu thuộc mã phân cấp
                List<HtQuyen> lstQuyenCha = (from quyen in lstAllQuyen // lấy ra toàn bộ quyền cha
                                             where lstMaPhanCapTmp.Contains(quyen.Ma)
                                             select quyen)
                                            .ToList();
                //cộng thêm mã phân cấp 

                if (lstQuyenCha != null && lstQuyenCha.Count > 0)
                {
                    foreach (var quyen in lstQuyenCha)
                    {
                        lstQuyen.Add(quyen);
                    }

                    lstQuyen = lstQuyen.GroupBy(quyen => quyen.Id)
                                       .Select(quyen => quyen.FirstOrDefault())
                                       .ToList();
                }

                List<QuyenCustom> lstQuyenDuocCap = lstQuyen.OrderBy(p => p.Ma)
                                                            .Distinct()
                                                            .Select(quyen => new QuyenCustom
                                                            {
                                                                Id = quyen.Id,
                                                                Title = quyen.TenQuyen,
                                                                Code = quyen.Ma,
                                                            })
                                                            .ToList() ?? new List<QuyenCustom>();

                var treeData = new List<TreeQuyen>(); // Tạo một danh sách đối tượng TreeNode

                List<QuyenCustom> lstQuyenParent = lstQuyenDuocCap.Where(quyen => quyen.Code != null && quyen.Code.Length == 3)
                                                             .Select(quyen => new QuyenCustom
                                                             {
                                                                 Id = quyen.Id,
                                                                 Title = quyen.Title,
                                                                 Code = quyen.Code
                                                             })
                                                             .ToList();

                if (lstQuyenParent != null && lstQuyenParent.Count > 0)
                {
                    foreach (var quyen in lstQuyenParent)
                    {
                        var treeNode = new TreeQuyen
                        {
                            Id = quyen.Id,
                            Title = quyen.Title,
                            Checked = false,
                            Code = quyen.Code,
                            Loai = HangSo.QuyenDuocCap,
                            Children = CreateNode(lstQuyenDuocCap, quyen.Code, HangSo.QuyenDuocCap)
                        };
                        treeData.Add(treeNode);
                    }
                }

                message.ObjData = treeData;
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


        [Route("GetDanhSachQuyenChuaCap/{idNhomQuyen}")]
        [HttpGet]
        public ResponseMessage GetDanhSachQuyenChuaCap(string? idNhomQuyen)
        {
            try
            {
                var lstAllQuyen = context.HtQuyens.ToList();//lấy toàn bộ danh sách quyền
                var lstQuyenNhomQuyen = context.HtQuyenNhomQuyens.Where(nhomQuyen => nhomQuyen.HtNhomQuyenId == idNhomQuyen).ToList(); //lấy ra toàn bộ quyền từng nút trong nhóm
                List<string?> lstQuyenId = lstQuyenNhomQuyen.Select(p => p.HtQuyenId)
                                                         .Distinct()
                                                         .ToList(); // lọc ra quyền

                var lstQuyen = lstAllQuyen.Where(s => !lstQuyenId.Contains(s.Id)).ToList(); // lọc ra những quyền đang chưa thêm
                var lstMaPhanCap = lstQuyen.Select(quyen => quyen.Ma)
                                                    .Distinct()
                                                    .ToList() ?? new List<string?>(); // lấy mã phân cấp cha con
                List<string> lstMaPhanCapTmp = new List<string>();
                foreach (var maPhanCap in lstMaPhanCap)
                {
                    if (maPhanCap != null)
                    {
                        string strMaPhanCap = maPhanCap;
                        if (!lstMaPhanCapTmp.Contains(strMaPhanCap))
                            lstMaPhanCapTmp.Add(strMaPhanCap); // chưa có thì add vào list
                        while (strMaPhanCap.Length > 3)
                        {
                            strMaPhanCap = strMaPhanCap.Substring(0, strMaPhanCap.Length - 3);
                            if (!lstMaPhanCapTmp.Contains(strMaPhanCap))
                                lstMaPhanCapTmp.Add(strMaPhanCap); // lấy ra quyền cha
                        }
                    }
                }
                //lấy danh sách menu thuộc mã phân cấp
                List<HtQuyen> lstQuyenCha = (from quyen in lstAllQuyen // lấy ra toàn bộ quyền cha
                                             where lstMaPhanCapTmp.Contains(quyen.Ma)
                                             select quyen).ToList();
                //cộng thêm mã phân cấp 

                if (lstQuyenCha != null && lstQuyenCha.Count > 0)
                {
                    foreach (var item in lstQuyenCha)
                    {
                        lstQuyen.Add(item);
                    }

                    lstQuyen = lstQuyen.GroupBy(quyen => quyen.Id)
                                       .Select(quyen => quyen.FirstOrDefault())
                                       .ToList();
                }

                List<QuyenCustom> lstQuyenChuaCap = lstQuyen.OrderBy(quyen => quyen.Ma)
                                                            .Distinct()
                                                            .Select(quyen => new QuyenCustom
                                                            {
                                                                Id = quyen.Id,
                                                                Title = quyen.TenQuyen,
                                                                Code = quyen.Ma,
                                                            })
                                                            .ToList() ?? new List<QuyenCustom>();

                var treeData = new List<TreeQuyen>(); // Tạo một danh sách đối tượng TreeNode

                List<QuyenCustom> lstQuyenParent = lstQuyenChuaCap.Where(quyen => quyen.Code != null && quyen.Code.Length == 3)
                                                             .Select(quyen => new QuyenCustom
                                                             {
                                                                 Id = quyen.Id,
                                                                 Title = quyen.Title,
                                                                 Code = quyen.Code
                                                             })
                                                             .ToList();

                if (lstQuyenParent != null && lstQuyenParent.Count > 0)
                {
                    foreach (var quyen in lstQuyenParent)
                    {
                        var treeNode = new TreeQuyen
                        {
                            Id = quyen.Id,
                            Title = quyen.Title,
                            Checked = false,
                            Code = quyen.Code,
                            Loai = HangSo.QuyenChuaCap,
                            Children = CreateNode(lstQuyenChuaCap, quyen.Code, HangSo.QuyenChuaCap)
                        };
                        treeData.Add(treeNode);
                    }
                }

                message.ObjData = treeData;
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


        private List<TreeQuyen> CreateNode(List<QuyenCustom> lstQuyen, string? maPhanCap, int? loai)
        {
            var result = new List<TreeQuyen>();

            List<QuyenCustom> lstChildQuyen = lstQuyen.Where(p => p.Code != null && p.Code.StartsWith(maPhanCap) && p.Code.Length == maPhanCap.Length + 3)
                                                      .Select(quyen => new QuyenCustom
                                                      {
                                                          Id = quyen.Id,
                                                          Title = quyen.Title,
                                                          Code = quyen.Code
                                                      })
                                                      .ToList();
            if (lstChildQuyen != null && lstChildQuyen.Count > 0)
            {

                foreach (var quyen in lstChildQuyen)
                {
                    var treeNode = new TreeQuyen
                    {
                        Id = quyen.Id,
                        Title = quyen.Title,
                        Checked = false,
                        Code = quyen.Code,
                        Loai = loai,
                        Children = CreateNode(lstQuyen, quyen.Code, loai)
                    };

                    result.Add(treeNode);
                }
            }

            return result;
        }
        #endregion

        #region Thêm, xóa quyền
        [Route("XoaQuyen")]
        [HttpPost]
        public ResponseMessage XoaQuyen(QuyenModel model)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string? tenNhomQuyen = context.HtNhomQuyens.FirstOrDefault(nhomQuyen => nhomQuyen.Id == model.NhomQuyenId)?.Ten;

                    if (model?.LstQuyenId != null && model?.LstQuyenId.Count > 0)
                    {
                        var lstQuyenNhomQuyen = context.HtQuyenNhomQuyens.Where(nhomQuyen => nhomQuyen.HtNhomQuyenId == model.NhomQuyenId
                                                                                             && model.LstQuyenId.Contains(nhomQuyen.HtQuyenId))
                                                                         .ToList();
                        if (lstQuyenNhomQuyen != null)
                        {
                            context.HtQuyenNhomQuyens.RemoveRange(lstQuyenNhomQuyen);
                        }
                    }
                    context.SaveChanges();

                    message.Title = "Xóa quyền thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    //ThemMoiNhatKy("Xóa quyền: " + tenNhomQuyen, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
                    //                                            HangSo.Admin.ToString());
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    //ThemMoiNhatKy("Xóa quyền: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                             EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                             EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                             HangSo.Admin);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("ThemQuyen")]
        [HttpPost]
        public ResponseMessage ThemQuyen(QuyenModel model)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string? tenNhomQuyen = context.HtNhomQuyens.FirstOrDefault(nhomQuyen => nhomQuyen.Id == model.NhomQuyenId)?.Ten;

                    if (model?.LstQuyenId != null && model?.LstQuyenId.Count > 0)
                    {
                        foreach (var idQuyen in model.LstQuyenId)
                        {
                            HtQuyenNhomQuyen objQuyenNhomQuyen = new()
                            {
                                Id = Guid.NewGuid().ToString(),
                                HtNhomQuyenId = model.NhomQuyenId,
                                HtQuyenId = idQuyen,
                            };
                            context.HtQuyenNhomQuyens.Add(objQuyenNhomQuyen);
                        }
                        context.SaveChanges();
                    }

                    message.Title = "Thêm quyền thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    //ThemMoiNhatKy("Thêm quyền: " + tenNhomQuyen, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                             EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                             EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
                    //                                             HangSo.Admin.ToString());
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    //ThemMoiNhatKy("Thêm quyền: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                              EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                              EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                              HangSo.Admin);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("ThemToanQuyen/{idNhomQuyen}")]
        [HttpGet]
        public ResponseMessage ThemToanQuyen(string? idNhomQuyen)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string? tenNhomQuyen = context.HtNhomQuyens.FirstOrDefault(nhomQuyen => nhomQuyen.Id == idNhomQuyen)?.Ten;

                    var lstQuyenXoa = context.HtQuyenNhomQuyens.Where(x => x.HtNhomQuyenId == idNhomQuyen).ToList();
                    if (lstQuyenXoa != null)
                    {
                        context.HtQuyenNhomQuyens.RemoveRange(lstQuyenXoa);
                        context.SaveChanges();
                    }

                    var lstQuyenId = context.HtQuyens.Select(x => x.Id).ToList();
                    if (lstQuyenId != null && lstQuyenId.Count > 0)
                    {

                        foreach (var idQuyen in lstQuyenId)
                        {
                            HtQuyenNhomQuyen objQuyenNhomQuyen = new()
                            {
                                Id = Guid.NewGuid().ToString(),
                                HtNhomQuyenId = idNhomQuyen,
                                HtQuyenId = idQuyen,
                            };
                            context.HtQuyenNhomQuyens.Add(objQuyenNhomQuyen);
                        }
                        context.SaveChanges();
                    }

                    message.Title = "Thêm quyền thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    //ThemMoiNhatKy("Thêm quyền: " + tenNhomQuyen, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                             EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                             EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
                    //                                             HangSo.Admin.ToString());
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    //ThemMoiNhatKy("Thêm quyền: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                              EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                              EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                              HangSo.Admin);
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return message;
        }

        [Route("XoaToanQuyen/{idNhomQuyen}")]
        [HttpGet]
        public ResponseMessage XoaToanQuyen(string? idNhomQuyen)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string? tenNhomQuyen = context.HtNhomQuyens.FirstOrDefault(nhomQuyen => nhomQuyen.Id == idNhomQuyen)?.Ten;

                    var lstQuyenXoa = context.HtQuyenNhomQuyens.Where(x => x.HtNhomQuyenId == idNhomQuyen).ToList();
                    if (lstQuyenXoa != null)
                    {
                        context.HtQuyenNhomQuyens.RemoveRange(lstQuyenXoa);
                        context.SaveChanges();
                    }

                    message.Title = "Xóa quyền thành công";
                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();

                    //ThemMoiNhatKy("Xóa quyền: " + tenNhomQuyen, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                            EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                            EnumAttributesHelper.GetDescription(Enums.TrangThai.ThanhCong),
                    //                                            HangSo.Admin.ToString());
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.BadRequest.GetHashCode();
                    message.Title = ex.Message;
                    trans.Rollback();
                    //ThemMoiNhatKy("Xóa quyền: " + message.Title, EnumAttributesHelper.GetDescription(Enums.LoaiChucNang.CapNhat),
                    //                                             EnumAttributesHelper.GetDescription(Enums.PhanHe.HeThong),
                    //                                             EnumAttributesHelper.GetDescription(Enums.TrangThai.KhongThanhCong),
                    //                                             HangSo.Admin);
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
