using DTC_BE.CodeBase;
using DTC_BE.Entities;
using DTC_BE.Models;
using DTC_BE.Models.HeThong.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Net;

namespace DTC_BE.Controllers.HeThong
{
    [Route("api/HeThong/[controller]")]
    [ApiController]
    public class MenuController : BaseApiController
    {
        #region Tìm kiếm menu
        [Route("GetDanhSach")]
        [HttpPost]
        public ResponseMessage GetDanhSachMenu(TimKiemDanhSachMenu timKiemDanhSach)
        {
            try
            {
                string GetCap(int? cap)
                {
                    string? result = "";
                    if (cap > 1)
                    {
                        result += "|";
                        for (int i = 0; i < cap.Value; i++)
                        {
                            result = result + "-";
                        }
                    }
                    return result + " ";
                }

                var lstMenu = context.HtMenus.OrderBy(menu => menu.PhanHe)
                                             .ThenBy(menu => menu.Code)
                                             .ThenBy(menu => menu.ThuTu)
                                             .Where(menu => !string.IsNullOrEmpty(timKiemDanhSach.TenMenu) ? menu.TenMenu.ToLower().Contains(timKiemDanhSach.TenMenu.ToLower().Trim()) : true)
                                             .AsEnumerable()
                                             .Select(menu => new
                                             {
                                                 menu.Id,
                                                 TenMenu = GetCap(menu.Cap) + menu.TenMenu,
                                                 menu.MoTa,
                                             })
                                             .Skip((timKiemDanhSach.CurrentPage - 1) * timKiemDanhSach.RowPerPage)
                                             .Take(timKiemDanhSach.RowPerPage)
                                             .ToList();
                int totalRecords = context.HtMenus.Where(menu => !string.IsNullOrEmpty(timKiemDanhSach.TenMenu) ? menu.TenMenu.ToLower().Contains(timKiemDanhSach.TenMenu.ToLower().Trim()) : true)
                                                  .Count();

                message.Title = "Lấy thông tin thành công";
                message.ObjData = new { lstMenu, totalRecords };
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception e)
            {
                message.Title = "Có lỗi xảy ra";
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
            }
            return message;
        }
        #endregion

        #region Other
        [Route("GetDanhMuc")]
        [HttpGet]
        public ResponseMessage GetDanhMuc()
        {
            try
            {
                List<SelectListItem> lstPhanHe = EnumHelper.GetListSelectItemByEnums(typeof(Enums.PhanHe));

                message.ObjData = lstPhanHe;
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception e)
            {
                message.Title = "Có lỗi xảy ra: " + e.Message;
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
            }
            return message;
        }
        [Route("GetMenuByPhanHe/{phanHe}")]
        [HttpGet]
        public ResponseMessage GetMenuByPhanHe(int? phanHe)
        {
            try
            {
                List<SelectListItem> lstMenu = context.HtMenus.OrderBy(menu => menu.ThuTu)
                                                              .ThenBy(menu => menu.ParentCode)
                                                              .ThenBy(menu => menu.Code)
                                                              .Where(menu => menu.PhanHe == phanHe && menu.Code.Length == 6)
                                                              .Select(menu => new SelectListItem
                                                              {
                                                                  Text = menu.TenMenu,
                                                                  Value = menu.Code
                                                              })
                                                              .ToList();

                List<SelectListItem> lstPhanHe = EnumHelper.GetListSelectItemByEnums(typeof(Enums.PhanHe));

                message.ObjData = lstMenu;
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception e)
            {
                message.Title = "Có lỗi xảy ra: " + e.Message;
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
            }
            return message;
        }
        [Route("GetMenuById/{id}")]
        [HttpGet]
        public ResponseMessage GetMenuById(string? id)
        {
            try
            {
                var menu = context.HtMenus.Select(menu => new
                {
                    menu.Id,
                    menu.TenMenu,
                    menu.Code,
                    menu.PhanHe,
                    menu.Icon,
                    menu.Cap,
                    menu.RouterLink,
                    menu.Link,
                    menu.MoTa,
                    menu.ParentCode,
                    menu.ThuTu,
                    Active = menu.IsActive == 0 ? false : true,
                }).FirstOrDefault(menu => menu.Id == id);

                message.ObjData = menu;
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception e)
            {
                message.Title = "Có lỗi xảy ra: " + e.Message;
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
            }
            return message;
        }

        private class MenuModel
        {
            public int? Id { get; set; }
            public string? IdMenu { get; set; }
            public string? Name { get; set; }
            public bool? IsOpen { get; set; }
            public string? Code { get; set; }
            public string? ParentCode { get; set; }
            public bool? Active { get; set; }
            public string? Icon { get; set; }
            public List<string>? RouterLink { get; set; }
            public string? Link { get; set; }
            public string? Description { get; set; }
            public List<MenuModel>? MenuChild { get; set; }
        }
        private class PhanHeModel
        {
            public string? Code { get; set; }
            public string? Text { get; set; }
            public int? Value { get; set; }
        }
        [Route("GetMenu")]
        [HttpGet]
        public ResponseMessage GetMenu()
        {
            try
            {
                List<MenuModel> lstMenuReturn = [];
                List<PhanHeModel> lstPhanHe = [];
                foreach (Enums.PhanHe phanHe in (Enums.PhanHe[])Enum.GetValues(typeof(Enums.PhanHe)))
                {
                    lstPhanHe.Add(new PhanHeModel { Code = phanHe.GetDefaultValue(), Text = phanHe.GetDescription(), Value = phanHe.GetHashCode() });
                }

                List<HtMenu> lstMenu = context.HtMenus.OrderBy(menu => menu.ThuTu).ToList();

                List<MenuModel> GetChildMenu(int? phanHe, string? code, int lengMaPhanCap)
                {
                    List<MenuModel> lstMenuChild = lstMenu.Where(menu => menu.Code != null
                                                                       && menu.PhanHe == phanHe
                                                                       && menu.Code.Length == lengMaPhanCap
                                                                       && (lengMaPhanCap > 6 ? menu.ParentCode == code : true))
                                                           .AsEnumerable()
                                                           .Select(menu => new MenuModel
                                                           {
                                                               Name = menu.TenMenu,
                                                               IsOpen = true,
                                                               Code = menu.Code,
                                                               ParentCode = menu.ParentCode,
                                                               Active = false,
                                                               Icon = menu.Icon,
                                                               MenuChild = GetChildMenu(phanHe, menu.Code, lengMaPhanCap + 3),
                                                               RouterLink = menu.RouterLink != null ? [menu.RouterLink] : null,
                                                               Link = menu.Link,
                                                               Description = menu.MoTa,
                                                               IdMenu = menu.Id,
                                                           })
                                                           .ToList();
                    return lstMenuChild;
                }
                int id = 1;
                foreach (var item in lstPhanHe)
                {
                    lstMenuReturn.Add(new MenuModel
                    {
                        Id = id,
                        Name = item.Text,
                        IsOpen = true,
                        Code = item.Code,
                        Active = false,
                        MenuChild = GetChildMenu(item.Value, "", 6),
                    });
                    id++;
                }
                message.ObjData = lstMenuReturn;
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception e)
            {
                message.Title = "Có lỗi xảy ra: " + e.Message;
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
            }
            return message;
        }
        #endregion

        #region CRUD menu
        [Route("ThemMoi")]
        [HttpPost]
        public ResponseMessage ThemMoi(HtMenu? objMenu)
        {
            try
            {
                objMenu.Id = Guid.NewGuid().ToString();
                int capCha = context.HtMenus.FirstOrDefault(menu => menu.Code == objMenu.ParentCode)?.Cap ?? 0;
                objMenu.Cap = capCha + 1;
                context.HtMenus.Add(objMenu);
                context.SaveChanges();

                message.Title = Notify.Insert("");
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception e)
            {
                message.Title = "Có lỗi xảy ra: " + e.Message;
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
            }
            return message;
        }

        [Route("CapNhat")]
        [HttpPost]
        public ResponseMessage CapNhat(HtMenu? objMenu)
        {
            try
            {
                HtMenu? menu = context.HtMenus.Find(objMenu.Id);
                if (menu != null)
                {
                    int capCha = context.HtMenus.FirstOrDefault(menu => menu.Code == objMenu.ParentCode)?.Cap ?? 0;
                    menu.TenMenu = objMenu.TenMenu;
                    menu.Cap = capCha + 1;
                    menu.MoTa = objMenu.MoTa;
                    menu.RouterLink = objMenu.RouterLink;
                    menu.Link = objMenu.Link;
                    menu.ThuTu = objMenu.ThuTu;
                    menu.Code = objMenu.Code;
                    menu.Icon = objMenu.Icon;
                    menu.IsActive = objMenu.IsActive;
                    menu.ParentCode = objMenu.ParentCode;
                    menu.PhanHe = objMenu.PhanHe;
                }
                context.SaveChanges();

                message.Title = Notify.Update("");
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception e)
            {
                message.Title = "Có lỗi xảy ra: " + e.Message;
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
            }
            return message;
        }

        [Route("Xoa")]
        [HttpDelete]
        public ResponseMessage Xoa(string? id, string? idUser)
        {
            HtMenu? menu = context.HtMenus.Find(id);
            try
            {
                if (menu != null)
                {
                    context.HtMenus.Remove(menu);
                    context.SaveChanges();
                }

                ThemMoiNhatKy($"Xóa trình đơn: {menu?.TenMenu}", Enums.LoaiChucNang.Xoa.GetDescription(),
                                                                 Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                 Enums.NhatKyHeThong_TrangThai.ThanhCong.GetDescription(),
                                                                 idUser);

                message.Title = Notify.Delete("");
                message.IsError = false;
                message.Code = HttpStatusCode.OK.GetHashCode();
            }
            catch (Exception e)
            {
                message.Title = "Có lỗi xảy ra: " + e.Message;
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                ThemMoiNhatKy($"Xóa trình đơn: {menu?.TenMenu}", Enums.LoaiChucNang.Xoa.GetDescription(),
                                                                 Enums.PhanHe.QuanTriHeThong.GetDescription(),
                                                                 Enums.NhatKyHeThong_TrangThai.KhongThanhCong.GetDescription(),
                                                                 idUser);
            }
            return message;
        }
        #endregion
    }
}
