using System.ComponentModel;
using System.Reflection;

namespace DTC_BE.CodeBase
{
    public class Enums
    {
        #region Tỉnh, huyện, xã
        public enum TinhHuyenXa
        {
            [Description("Tỉnh")]
            Tinh = 1,
            [Description("Huyện")]
            Huyen = 2,
            [Description("Xã")]
            Xa = 3,
        }
        #endregion

        #region Nhật ký hệ thống
        public enum LoaiChucNang
        {
            [Description("Thêm mới")]
            ThemMoi = 1,
            [Description("Cập nhật")]
            CapNhat = 2,
            [Description("Xóa")]
            Xoa = 3,
            [Description("Gửi")]
            Gui = 4,
            [Description("Nhận")]
            Nhan = 8,
            [Description("Duyệt")]
            Duyet = 5,
            [Description("Từ chối")]
            TuChoi = 6,
            [Description("Thu hồi")]
            ThuHoi = 7,
        }

        public enum PhanHe
        {
            [Description("Quản lý kế hoạch đầu tư công trung hạn")]
            [DefaultValue("000")]
            QuanLyDauTuCongTrungHan = 1,
        
            [Description("Quản lý kế hoạch đầu tư công hằng năm")]
            [DefaultValue("001")]
            QuanLyDauTuCongHangNam = 2,
            
            [Description("Quản lý thủ tục nội bộ")]
            [DefaultValue("002")]
            QuanLyThuTucNoiBo = 3,
            
            [Description("Thủ tục nội bộ về kế hoạch lựa chọn nhà thầu")]
            [DefaultValue("003")]
            ThuTucNoiBoVeKeHoachLuaChonNhaThau = 4,
            
            [Description("Cở sở dữ liệu về NSNN")]
            [DefaultValue("004")]
            CoSoDuLieuVeNSNN = 5,
            
            [Description("Quản trị hệ thống")]
            [DefaultValue("005")]
            QuanTriHeThong = 6,
        }
        
        public enum NhatKyHeThong_TrangThai
        {
            [Description("Thành công")]
            ThanhCong = 1,
        
            [Description("Không thành công")]
            KhongThanhCong = 2,
        }

        #endregion


        public enum NhomQuyen
        {
            [Description("ab78aa3d-a03f-4052-8cf8-dce7f6eed9ea")]
            ChuyenVienXuLy = 1,
            [Description("dd96cd45-29d1-4b05-a2af-dd6d6c9bba01")]
            ChuyenVienThuLy = 2,
            [Description("cb8fecbb-0d0c-4728-861b-88ab8c3703a5")]
            LanhDaoSo = 3,
            [Description("e010355f-dd81-45de-96a9-f3a5e5de7bd6")]
            LanhDaoPhong = 4,
        }

        public enum NhomDuAnHangNam
        {
            [Description("a49b6e00-751a-4bdb-878f-5761f6a3dc45")]
            NhomA = 1,
            [Description("e256de93-955c-47fc-82a1-d4cdb33bd2b7")]
            NhomB = 2,
            [Description("42a82d62-440d-4921-ac4b-019d511c22ee")]
            NhomC = 3,
            [Description("cb526a06-e2bf-456c-a118-296d9dd6592f")]
            DuAnQuanTrongQuocGiaDoQuocHoiChuTruongDauTu = 4,
        }

        public enum LoaiChuDauTu
        {
            [Description("Chủ đầu tư")]
            ChuDauTu = 0,
            [Description("Huyện")]
            Huyen = 1,
        }

        public enum LoaiNguonVonTinh
        {
            [Description("NSTT")]
            NSTT = 1,
            [Description("XSKT")]
            XSKT = 2,
            [Description("Dat")]
            Dat = 3,
            [Description("Khac")]
            Khac = 4,
        }

        public enum LoaiNguonVonTW
        {
            [Description("Von_NSTW_MucTieu")]
            Von_NSTW_MucTieu = 5,
            [Description("Von_CTPH_KTXH")]
            Von_CTPH_KTXH = 6,
            [Description("Nguon_Du_Phong_NSTW")]
            Nguon_Du_Phong_NSTW = 7,
        }

        public enum LoaiNguonVonNuocNgoai
        {
            [Description("VỐN NƯỚC NGOÀI KHÔNG GIẢI NGÂN THEO CƠ CHẾ TÀI CHÍNH TRONG NƯỚC")]
            KhongGiaiNgan = 9,
            [Description("VỐN NƯỚC NGOÀI GIẢI NGÂN THEO CƠ CHẾ TÀI CHÍNH TRONG NƯỚC")]
            CoGiaiNgan = 8,

        }

        public enum LoaiNguonVonNN
        {
            [Description("Vốn ngân sách giải ngân theo tài chính trong nước")]
            Von_NSNN_GiaiNgan = 8,
            [Description("Vốn ngân sách không giải ngân theo tài chính trong nước")]
            Von_NSNN_KhongGiaiNgan = 9,
        }

        public enum LoaiNguonVonDangKyVonTrungHan
        {
            [Description("Trung ương")]
            TrungUong = 1,
            [Description("Tỉnh")]
            Tinh = 2,
            [Description("Nước ngoài")]
            NuocNgoai = 3,
        }
        /// <summary>
        /// Quản lý thủ tục nội bộ dự án đầu tư công
        /// Thông tin các loại thủ tục
        /// </summary>
        public enum LoaiThuTuNoiBoDuAnDTC
        {
            [Description("Thủ tục thẩm định dự toán chuẩn bị đầu tư")]
            ThuTucThamDinhDuToanChuanBiDauTu = 1,
            [Description("Thủ tục thẩm định chủ trương đầu tư")]
            ThuTucThamDinhchuTruongDauTu = 2,
            [Description("Thẩm định điều chỉnh chủ trương đầu tư")]
            ThamDinhDieuchinhChuTruongDauTu = 3,
            [Description("Thủ tục thẩm định dự án đầu tư có cấu phần xây dựng")]
            ThamDinhDuAnDauTuCoCauPhanXayDung = 4,
            [Description("Thủ tục thẩm định điều chỉnh dự án đầu tư có cấu phần xây dựng")]
            ThamDinhDieuChinhDuAnDauTuCoCauPhanXayDung = 5,
            [Description("Thủ tục thẩm định dự án đầu tư không có cấu phần xây dựng")]
            ThamDinhDuAnDauTuKhongCoCauPhanXayDung = 6,
            [Description("Thủ tục thẩm định điều chỉnh dự án đầu tư không có cấu phần xây dựng")]
            ThuTucThamDinhDieuChinhDuAnDauTuKhongCoCauPhanXayDung = 7,
            [Description("Thủ tục thẩm định dự toán dự án đầu tư không có cấu phần xây dựng")]
            ThuTucThamDinhDuToanDuAnDauTuKhongCoCauPhanXayDung = 8,
            [Description("Thủ tục thẩm định dự toán điều chỉnh dự án đầu tư không có cấu phần xây dựng")]
            ThuTucThamDinhDuToanDieuChinhDuAnDauTuKhongCoCauPhanXayDung = 9,
            [Description("Thủ tục thẩm định kế hoạch lựa chọn nhà thầu")]
            ThuTucThamDinhKeHoachLuaChonNhaThau = 10,
            [Description("Thủ tục thẩm định kế hoạch lựa chọn nhà thầu điều chỉnh")]
            ThuTucThamDinhKeHoachLuaChonNhaThauDieuChinh = 11,
        }

        public enum QUY_TRINH_XU_LY
        {
            [Description("Dự toán chuẩn bị đầu tư")]
            TTTD_DUTOAN_CHUANBI_DAUTU = 1,

            [Description("Chủ trương đầu tư")]
            TTTD_CHUTRUONG_DAUTU = 2,

            [Description("Điều chỉnh chủ trương đầu tư")]
            TTTD_DC_CHUTRUONG_DAUTU = 3,

            [Description("Dự án có cấu phần xây dựng")]
            TTTD_DA_DAUTU_CO_CPXD = 4,

            [Description("Điều chỉnh dự án có cấu phần xây dựng")]
            TTTD_DC_DA_DAUTU_CO_CPXD = 5,

            [Description("Dự án không có cấu phần xây dựng")]
            TTTD_DA_DAUTU_KC_CPXD = 6,

            [Description("Điều chỉnh dự án không có cấu phần xây dựng")]
            TTTD_DC_DA_DAUTU_KC_CPXD = 7,

            [Description("Dự toán dự án không có cấu phần xây dựng")]
            TTTD_DUTOAN_DA_DAUTU_KC_CPXD = 8,

            [Description("Điều chỉnh dự toán dự án không có cấu phần xây dựng")]
            TTTD_DUTOAN_DC_DA_DAUTU_KC_CPXD = 9,

            [Description("Kế hoạch lựa chọn nhà thầu")]
            TTTD_KH_LCNT = 10,

            [Description("Kế hoạch lựa chọn nhà thầu điều chỉnh")]
            TTTD_KH_LCNT_DC = 11,
        }
        public enum LoaiBaoCaoTongHopThuTucNoiBo
        {
            [Description("Báo cáo tổng hợp về thủ tục nội bộ")]
            BaoCaoTongHoVeThuTucNoiBo = 1,
            [Description("Báo cáo tổng hợp về gói thầu được duyệt")]
            BaoCaoTongHoVeGoiThauDuocDuyet = 2,
        }


        public enum IsXoa
        {
            [Description("Đã xóa")]
            DaXoa = 1,
        }
        #region Thủ Tục lựa chọn nhà thầu
        public enum LoaiThuTucNoiBoKeHoachLuaChonNhaThau
        {
            [Description("Thủ tục thẩm định kế hoạch lựa chọn nhà thầu")]
            ThuTucThamDinhKeHoachLuaChonNhaThau = 10,
            [Description("Thủ tục thẩm định kế hoạch lựa chọn nhà thầu điều chỉnh")]
            ThuTucThamDinhKeHoachLuaChonNhaThauDieuChinh = 11,
        }

        public enum LinhVuc
        {
            [Description("Phi tư vấn")]
            PhiTuVan = 1,
            [Description("Tư vấn")]
            TuVan = 2,
            [Description("Xây lắp")]
            XayLap = 3,
            [Description("Thiết bị")]
            ThietBi = 4,
            [Description("Hỗn hợp")]
            HonHop = 5,
        }

        public enum HinhThucLuaChonNhaThau
        {
            [Description("Đấu thầu rộng rãi")]
            DauThauRongRai = 1,

            [Description("Đấu thầu hạn chế ")]
            HanChe = 2,

            [Description("Chỉ định thầu")]
            ChiDinhThau = 3,

            [Description("Chào hàng cạnh tranh")]
            ChaoHangCanhTranh = 4,

            [Description("Mua sắm trực tiếp")]
            MuaSamTrucTiep = 5,

            [Description("Tự thực hiện")]
            TuThucHien = 6,

            [Description("Lựa chọn nhà thầu trong trường hợp đặc biệt")]
            LuaChonNhaThauTHDacBiet = 7,

            [Description("Tham gia thực hiện của cộng đồng")]
            ThamGiaThucHienCuaCongDong = 8,

            [Description("Đàm phán giá")]
            DamPhanGia = 9,

            [Description("Chào giá trực tuyến")]
            ChaoGiaTrucTuyen = 10,
        }


        public enum HinhThucDauThau
        {
            [Description("Qua mạng")]
            QuaMang = 1,
            [Description("Không qua mạng")]
            KhongQuaMang = 2,
        }
        public enum ThuTucNoiBo_TrangThai
        {
            [Description("Trước hạn")]
            TruocHan = 1,
            [Description("Đúng hạn")]
            DungHan = 2,
            [Description("Quá Hạn")]
            QuaHan = 3,
        }

        /// <summary>
        /// Quản lý thủ tục nội bộ dự án đầu tư công
        /// Thông tin các loại thủ tục
        /// </summary>
        public enum LoaiCoSoDuLieu
        {
            [Description("Quản lý dữ liệu chủ trương đầu tư")]
            QuanLyDuLieuChuTruongDauTu = 1,
            [Description("Dữ liệu chủ trương đầu tư điều chỉnh")]
            DuLieuChuTruongDauTuDieuChinh = 2,
            [Description("Dữ liệu dự án")]
            DuLieuDuAn = 3,
            [Description("Dữ liệu dự án điều chỉnh")]
            DuLieuDuAnDieuChinh = 4,
            [Description("Dữ liệu dự toán chuẩn bị đầu tư")]
            DuLieuDuToanChuanBiDauTu = 5,
            [Description("Dữ liệu dự toán chuẩn bị đầu tư điều chỉnh")]
            DuLieuDuToanChuanBiDauTuDieuChinh = 6,
            [Description("Dữ liệu kế hoạch lựa chọn nhà thầu")]
            DuLieuKeHoachLuaChonNhaThau = 7,
            [Description("Dữ liệu kế hoạch lựa chọn nhà thầu điều chỉnh")]
            DuLieuKeHoachLuaChonNhaThauDieuChinh = 8,
        }
        public enum NGUON_VON
        {
            [Description("Nguồn vốn đầu tư công")]
            NguonVonDauTuCong = 1,
            [Description("Nguồn vốn khác")]
            NguonVonKhac = 2,
        }

        public enum LinhVucVaHinhThucBaoCaoTongHop
        {
            [Description("Phi tư vấn")]
            PhiTuVan = 1,
            [Description("Tư vấn")]
            TuVan = 2,
            [Description("Xây lắp")]
            XayLap = 3,
            [Description("Thiết bị")]
            ThietBi = 4,
            [Description("Hỗn hợp")]
            HonHop = 5,
            [Description("Tổng cộng 1")]
            TongCongQM_1 = 6,
            [Description("Tổng cộng Không qua mạng 1")]
            TongCongKQM_1 = 7,

            [Description("Đấu thầu rộng rãi")]
            DauThauRongRai = 11,

            [Description("Đấu thầu hạn chế ")]
            HanChe = 12,

            [Description("Chỉ định thầu")]
            ChiDinhThau = 13,

            [Description("Chào hàng cạnh tranh")]
            ChaoHangCanhTranh = 14,

            [Description("Mua sắm trực tiếp")]
            MuaSamTrucTiep = 15,

            [Description("Tự thực hiện")]
            TuThucHien = 16,

            [Description("Lựa chọn nhà thầu trong trường hợp đặc biệt")]
            LuaChonNhaThauTHDacBiet = 17,

            [Description("Tham gia thực hiện của cộng đồng")]
            ThamGiaThucHienCuaCongDong = 18,

            [Description("Đàm phán giá")]
            DamPhanGia = 19,

            [Description("Chào giá trực tuyến")]
            ChaoGiaTrucTuyen = 20,

            [Description("Tổng Cộng QM 2")]
            TongCongQM_2 = 21,
            [Description("Tổng Cộng KQM 2")]
            TongCongKQM_2 = 22,
        }

        public enum PhuongThucLuachonNhaThau
        {
            [Description("Một giai đoạn một túi hồ sơ")]
            MotGiaiDoanMotTuiHoSo = 1,
            [Description("Một giai đoạn hai túi hồ sơ")]
            MotGiaiDoanHaiTuiHoSo = 2,
            [Description("Hai giai đoạn một túi hồ sơ")]
            HaiGiaiDoanMotTuiHoSo = 3,
            [Description("Hai giai đoạn hai túi hồ sơ")]
            HaiGiaiDoanHaiTuiHoSo = 4,
        }

        public enum LoaiHopDong
        {
            [Description("Hợp đồng trọn gói")]
            HopDongTronGoi = 1,
            [Description("Hợp đồng theo đơn giá cố định")]
            HopDongTheoDongGiaCoDinh = 2,
            [Description("Hợp đồng theo đơn giá điều chỉnh")]
            HopDongTheoDonGiaDieuChinh = 3,
            [Description("Hợp đồng theo thời gian")]
            HopDongTheoThoiGian = 4,
        }
        #endregion
        public enum KetQuaTienDoThucHienXuLyThuTuc
        {
            [Description("Nhận hồ sơ")]
            NhanHoSo = 1,
            [Description("Xác minh hồ sơ")]
            XacMinhHoSo = 2,
            [Description("Trình ký")]
            TrinhKy = 3,
            [Description("Trả kết quả")]
            TraKetQua = 4,
        }
        public enum KetQuaThucHienThuTuc
        {
            [Description("Chấp thuận")]
            ChapThuan = 1,
            [Description("Không chấp thuận")]
            KhongChapThuan = 0,
        }
        public enum NhomDuAn
        {
            [Description("Dự án nhóm A")]
            DuAnNhomA = 1,
            [Description("Dự án nhóm B")]
            DuAnNhomB = 2,
            [Description("Dự án nhóm C")]
            DuAnNhomC = 3,
            [Description("Dự án quan trọng Quốc gia")]
            DuAnQuanTrongQuocGiaDoQuocHoiChuTruongDauTu = 4,
        }
        
        public enum PhongBan
        {
            [Description("Văn phòng Sở")]
            VanPhongSO = 1,
            [Description("Phòng Tổng hợp – Quy hoạch")]
            PhongTongHopQUyHoach = 2,
            [Description("Phòng Quản lý ngành")]
            PhongQlyNganh = 3,
            [Description("Phòng Đấu thầu, Thẩm định và Giám sát đầu tư")]
            PhongDauThauThamDinh = 4,
            [Description("Phòng Kinh tế đối ngoại")]
            PhongKinhTeDoiNgoai = 5,
            [Description("Phòng Đăng ký kinh doanh")]
            PhongDkyKinhDoan = 6,
            [Description("Thanh tra Sở")]
            ThanhTraSo = 7,
        }
        public enum TINH_TRANG_HO_SO
        {
            [Description("Hồ sơ hoàn thành")]
            HoanThanh = 1,

            [Description("Hồ sơ trình UBND tỉnh")]
            TrinhUBT = 2,

            [Description("CTĐT chờ họp Hội đồng thẩm định")]
            ChoHoiDongThamDinh = 3,

            [Description("Hồ sơ chờ bổ sung của chủ đầu tư")]
            HoSoBoSungCuaChuDauTu = 4,

            [Description("Hồ sơ chờ ý kiến các ngành")]
            HoSoChoYKienCacNganh = 5,

            [Description("Hồ sơ đang xử lý")]
            HoSoDangXuLy = 6,

            [Description("Hồ sơ mới nhận trong tuần")]
            HoSoMoiNhanTrongTuan = 7,
        }
        public static string GetEnumDescription(Enum? value)
        {
            try
            {
                if (value == null)
                    return "";
                FieldInfo? fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    public static class EnumAttributesHelper
    {
        public static Dictionary<int, string> BieuTongHopVonTinh()
        {
            Dictionary<int, string> BaoCaoTongHopTinh = new Dictionary<int, string>();
            BaoCaoTongHopTinh.Add(1, " Đầu tư từ nguồn thu sử dụng đất");
            BaoCaoTongHopTinh.Add(2, "Trong đó: ");
            BaoCaoTongHopTinh.Add(3, "+ Phân bổ vốn theo dự án");
            BaoCaoTongHopTinh.Add(4, "+ Vốn điều lệ quỹ hỗ trợ phát triển sử dụng đất");
            BaoCaoTongHopTinh.Add(5, "- Xổ số kiến thiết");
            BaoCaoTongHopTinh.Add(6, "- Bội chi ngân sách địa phương");
            BaoCaoTongHopTinh.Add(7, "- Khác");
            return BaoCaoTongHopTinh;
        }

        public static Dictionary<int, string> BieuTongHopVonTW()
        {
            Dictionary<int, string> BaoCaoTongHopTinh = new Dictionary<int, string>();
            BaoCaoTongHopTinh.Add(1, "- Vốn NSTW hỗ trợ có mục tiêu");
            BaoCaoTongHopTinh.Add(2, "- Vốn CTPH & PT KTXH");
            BaoCaoTongHopTinh.Add(3, "- Nguồn dự phòng NSTW");
            return BaoCaoTongHopTinh;
        }

        public static Dictionary<int, string> BieuTongHopVonTinh_HangNam()
        {
            Dictionary<int, string> BaoCaoTongHopTinh = new Dictionary<int, string>();
            BaoCaoTongHopTinh.Add(1, "- Ngân sách tập trung");
            BaoCaoTongHopTinh.Add(2, "- Xổ số kiến thiết");
            BaoCaoTongHopTinh.Add(3, "- Đất");
            BaoCaoTongHopTinh.Add(4, "- Khác");
            return BaoCaoTongHopTinh;
        }

        public static Dictionary<int, string> BieuTongHopKQTHVonTinh_HangNam()
        {
            Dictionary<int, string> BaoCaoTongHopKQTHTinh = new Dictionary<int, string>();
            BaoCaoTongHopKQTHTinh.Add(1, "- Chuẩn bị đầu tư");
            BaoCaoTongHopKQTHTinh.Add(2, "- Thực hiện dự án");
            BaoCaoTongHopKQTHTinh.Add(3, "Ngân sách tập trung");
            BaoCaoTongHopKQTHTinh.Add(4, "Xổ số kiến thiết");
            BaoCaoTongHopKQTHTinh.Add(5, "Đất");
            BaoCaoTongHopKQTHTinh.Add(6, "Khác");
            return BaoCaoTongHopKQTHTinh;
        }

        public static Dictionary<int, string> BieuTongHopKQTHVonTW_HangNam()
        {
            Dictionary<int, string> BaoCaoTongHopKQTHTW = new Dictionary<int, string>();
            BaoCaoTongHopKQTHTW.Add(1, "- Chuẩn bị đầu tư");
            BaoCaoTongHopKQTHTW.Add(2, "- Thực hiện dự án");
            BaoCaoTongHopKQTHTW.Add(3, "Vốn NSTW hỗ trợ có mục tiêu");
            BaoCaoTongHopKQTHTW.Add(4, "Vốn CTPH & PT KTXH");
            BaoCaoTongHopKQTHTW.Add(5, "Nguồn dự phòng NSTW");
            return BaoCaoTongHopKQTHTW;
        }


        public static Dictionary<int, string> BieuTongHopVonTW_Hangnam()
        {
            Dictionary<int, string> BaoCaoTongHopTW = new Dictionary<int, string>();
            BaoCaoTongHopTW.Add(1, "- Vốn NSTW hỗ trợ có mục tiêu");
            BaoCaoTongHopTW.Add(2, "- Vốn CTPH & PT KTXH");
            BaoCaoTongHopTW.Add(3, "- Nguồn dự phòng NSTW");
            return BaoCaoTongHopTW;
        }

        public static Dictionary<int, string> BieuMau29_Tinh()
        {
            Dictionary<int, string> BaoCaoTongHopTinh = new Dictionary<int, string>();
            BaoCaoTongHopTinh.Add(1, "- Bội chi ngân sách địa phương");
            BaoCaoTongHopTinh.Add(2, "- Xổ số kiến thiết");
            BaoCaoTongHopTinh.Add(3, "- Đầu tư từ nguồn thu sử dụng đất");
            BaoCaoTongHopTinh.Add(4, "- Khác");
            return BaoCaoTongHopTinh;
        }

        public static Dictionary<int, string> BieuMau29_TW()
        {
            Dictionary<int, string> BaoCaoTongHopTinh = new Dictionary<int, string>();
            BaoCaoTongHopTinh.Add(1, "- Vốn NSTW hỗ trợ có mục tiêu");
            BaoCaoTongHopTinh.Add(2, "- Vốn CTPH & PT KTXH");
            BaoCaoTongHopTinh.Add(3, "- Nguồn dự phòng NSTW");
            return BaoCaoTongHopTinh;
        }

        public static string GetDescription(this Enum value)
        {
            try
            {
                var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return da.Length > 0 ? da[0].Description : value.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetDefaultValue(this Enum value)
        {
            try
            {
                var da = (DefaultValueAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DefaultValueAttribute), false);
                return da.Length > 0 ? da[0]?.Value?.ToString() : value.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public static class EnumHelper
    {
        public static List<DTC_BE.Models.SelectListItem> GetListSelectItemByEnums(Type enumType)
        {
            List<DTC_BE.Models.SelectListItem> listReturn = [];

            try
            {

                if (!enumType.IsEnum)
                    return [];

                foreach (var enumValue in Enum.GetValues(enumType))
                {
                    var enumField = enumType.GetField(enumValue.ToString());
                    DescriptionAttribute? descriptionAttribute = enumField.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                    string description = descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();
                    string value = ((int)enumValue).ToString();

                    listReturn.Add(new DTC_BE.Models.SelectListItem { Text = description, Value = value });
                }
            }
            catch (Exception)
            {
                return [];
            }
            return listReturn;
        }
    }
}
