using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using DTC_BE.Models;
//using Newtonsoft.Json.Linq;
using System.Security.Principal;
using DTC_BE.CodeBase;
using DTC_BE.Entities;

namespace DTC_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        public class UserLogin
        {
            public string UserName { get; set; }
            public string PassWord { get; set; }
        }

        public MembershipController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("Login")]
        [HttpPost]
        public ResponseMessage Login([FromBody] UserLogin NguoiDung)
        {
            try
            {
                UserInfo mUserInfo = new UserInfo();

                if (NguoiDung is null)
                {
                    message.IsError = true;
                    message.Code = 400;
                    message.Title = "Yêu cầu kiểm tra lại thông tin nhập";
                    return message;
                }

                // Kiểm tra Content-Type
                if (!Request.Headers.ContainsKey("Content-Type") || !Request.Headers["Content-Type"].ToString().Equals("application/json"))
                {
                    throw new Exception("Invalid Content-Type.");
                }

                var objUser = context.HtNguoiDungs.Where(s => s.TenDangNhap == NguoiDung.UserName).FirstOrDefault();

                if (objUser == null)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.Unauthorized.GetHashCode();
                    message.Title = "Tài khoản không chính xác";
                    return message;
                }

                if (objUser?.TrangThai == 1)
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.Unauthorized.GetHashCode();
                    message.Title = "Tài khoản đã bị khóa";
                    return message;
                }

                //if (objUser != null && SecurityService.EncryptPassword_Sha256(NguoiDung.PassWord) == objUser.MatKhau)
                if (objUser != null && Security.EncryptPassword(NguoiDung.PassWord) == objUser.MatKhau)
                {
                    mUserInfo.IsAuthenticated = true;
                    mUserInfo.UserId = objUser.Id;
                    mUserInfo.UserName = objUser.TenDangNhap;
                    mUserInfo.FullName = objUser.HoTen;
                    string strCurrentSession = SecurityService.EncryptPassword(DateTime.Now.ToString());
                    mUserInfo.CurentSession = strCurrentSession;
                    mUserInfo.NhomQuyenId = objUser.HtNhomQuyenId;
                    mUserInfo.TenNhomQuyen = context.HtNhomQuyens.FirstOrDefault(nhomQuyen => nhomQuyen.Id == objUser.HtNhomQuyenId)?.Ten;
                    mUserInfo.ChuDauTuId = objUser.DmChuDauTuId;
                    var token = GenerateToken(objUser);
                    var genRefreshToken = GenerateRefreshToken();
                    var token_user = new Tokens { AccessToken = token, RefreshToken = genRefreshToken.refreshToken };
                    mUserInfo.Token = token;
                    mUserInfo.TokensUser = token_user;
                    //thêm refresh token cho user
                    objUser.Refreshtoken = genRefreshToken.refreshToken;
                    objUser.Expiresat = genRefreshToken.expiresAt;
                    objUser.Isrefreshtokenrevoked = false;

                    #region Đếm lượt truy cập
                    //ThongKeTruyCap? objThongKeTruyCap = context.ThongKeTruyCaps.Where(IsSameDate<ThongKeTruyCap>(o => o.Ngay.Value, DateTime.Now))
                    //                                                           .FirstOrDefault();
                    //if (objThongKeTruyCap == null)
                    //{
                    //    objThongKeTruyCap = new()
                    //    {
                    //        UseMultiId = mUserInfo.UserId.ToString(),
                    //        Ngay = DateTime.Now,
                    //    };
                    //    context.ThongKeTruyCaps.Add(objThongKeTruyCap);
                    //}
                    //else
                    //{
                    //    objThongKeTruyCap.UseMultiId += "," + objUser?.Id.ToString();
                    //}
                    #endregion
                    context.SaveChanges();

                    message.IsError = false;
                    message.Code = HttpStatusCode.OK.GetHashCode();
                    message.ObjData = new { token, mUserInfo };
                    return message;
                }
                else
                {
                    message.IsError = true;
                    message.Code = HttpStatusCode.Unauthorized.GetHashCode();
                    message.Title = "Mật khẩu không chính xác";
                    return message;
                }
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Code = HttpStatusCode.BadRequest.GetHashCode();
                message.Title = "Có lỗi xảy ra, vui lòng thử lại sau";
                if (ex.HResult == -2146232060)
                {
                    message.Title = "Lỗi kết nối, vui lòng liên hệ với quản trị";
                }
            }
            return message;
        }

        [Route("GetDataMenu")]
        [HttpGet]
        public ResponseMessage GetDataMenu(string idUser, string idNhomQuyen)
        {
            try
            {

                List<string>? lstCodeReturn = new();
                List<string> lstHtNhomQuyenId = new List<string>();
                lstHtNhomQuyenId = context.HtNguoiDungs.Where(s => s.Id == idUser).Select(s => s.HtNhomQuyenId).ToList();

                foreach (var item in lstHtNhomQuyenId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        List<string>? lstCode = new();
                        lstCode = GetListCodeByNhomQuyenId(item);
                        lstCodeReturn.AddRange(lstCode);
                    }
                }

                lstCodeReturn = lstCodeReturn.Distinct().ToList();

                message.IsError = false;
                message.ObjData = lstCodeReturn;
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Title = "Có lỗi xảy ra " + ex.Message.ToString();
            }
            return message;
        }

        [Route("RefreshToken")]
        [HttpPost]
        public ResponseMessage RefreshToken(RefreshTokenModel rfTokenModel)
        {
            try
            {
                bool refreshTokenInValid = true;
                string newToken = "";
                var objUser = context.HtNguoiDungs.FirstOrDefault(x => x.Id == rfTokenModel.UserId);
                if (objUser?.Isrefreshtokenrevoked == false && objUser?.Refreshtoken == rfTokenModel?.RefreshToken && objUser?.Expiresat > DateTime.UtcNow)
                {
                    refreshTokenInValid = false;
                    newToken = GenerateToken(objUser);
                }

                message.IsError = refreshTokenInValid;
                message.ObjData = newToken;
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Title = "Có lỗi xảy ra " + ex.Message.ToString();
            }
            return message;
        }

        /// <summary>
        /// thu hồi refresh token khi người dùng chủ động logout
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("RefreshTokenRevoke/{userId}")]
        [HttpGet]
        public ResponseMessage RefreshTokenRevoke(string? userId)
        {
            try
            {
                var objUser = context.HtNguoiDungs.FirstOrDefault(x => x.Id == userId);
                if (objUser != null)
                {
                    objUser.Isrefreshtokenrevoked = true;
                }
                message.IsError = false;
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.Title = "Có lỗi xảy ra " + ex.Message.ToString();
            }
            return message;
        }


        private List<string>? GetListCodeByNhomQuyenId(string nhomQuyenID)
        {
            return context.HtQuyenNhomQuyens.Where(s => s.HtNhomQuyenId == nhomQuyenID).Select(s => s.HtQuyen.Ma).ToList();
        }

        private string GenerateToken(HtNguoiDung user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user?.TenDangNhap),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// tạo refresh token và thời gian hết hạn refresh token
        /// </summary>
        /// <returns></returns>
        private (string refreshToken, DateTime expiresAt) GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                // Thời gian hết hạn: Ví dụ, 30 ngày từ thời điểm tạo
                var expiresAt = DateTime.UtcNow.Add(TimeSpan.FromDays(7));

                // Chuyển đổi thời gian hết hạn thành một chuỗi có thể lưu trữ, ví dụ: ticks
                var expiresAtTicks = expiresAt.Ticks.ToString();

                // Kết hợp refresh token và thời gian hết hạn để lưu vào cơ sở dữ liệu
                var refreshToken = Convert.ToBase64String(randomNumber) + "." + expiresAtTicks;

                return (refreshToken, expiresAt);
            }
        }

        public class RefreshTokenModel
        {
            public string? RefreshToken { get; set; }
            public string? UserId { get; set; }
        }

        public class UserInfo
        {
            public string? FullName { get; set; }
            public string? UserName { get; set; }
            public string? UserAdmin { get; set; }
            public bool? IsAuthenticated { get; set; }
            public string? LocalIP { get; set; }
            public string? UserId { get; set; }
            public string? ChucDanhID { get; set; }
            public string? ChuDauTuId { get; set; }
            public string? PhongBanId { get; set; }
            public string? TenNhomQuyen { get; set; }
            public string? Token { get; set; }
            public Tokens? TokensUser { get; set; }
            public string? CurentSession { get; set; }
            public List<string>? MenuCha { get; set; }
            public List<string>? MenuCon { get; set; }

            public string? NhomQuyenId { get; set; }
        }

        public class Tokens
        {
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
        }
    }
}
