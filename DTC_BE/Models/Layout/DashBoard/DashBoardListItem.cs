namespace DTC_BE.Models.Layout.DashBoard
{
    public class DashBoardListItem
    {
        public DashBoard_DuAnModel? DashBoard_DuAnModel { get; set; }
        public DashBoard_TongMucDauTuModel? DashBoard_TongMucDauTuModel { get; set; }
        public DashBoard_TyLeGiaiNganModel? DashBoard_TyLeGiaiNganModel { get; set; }
        public List<DashBoard_VanBanHuongDan>? DashBoard_VanBanHuongDan { get; set; }
    }

    public class ThongKeChuDauTu
    {
        public string? TenChuDauTu { get; set; }
        public string? Count_DuAn { get; set; }
        public string? Count_DuAn_KhoiCongMoi { get; set; }
        public string? Count_DuAn_ChuyenTiep { get; set; }
        public string? Count_DuAn_HoanThanh { get; set; }
    }

    public class DashBoard_DuAnModel
    {
        public string? Count_DuAn { get;set; }
        public string? Count_DuAn_KhoiCong { get;set; }
        public string? Count_DuAn_ChuyenTiep { get;set; }
    }

    public class DashBoard_TongMucDauTuModel
    {
        public string? Count_TongKeHoachVon { get; set; }
        public string? Count_TongGiaTriThucHien { get; set; }
        public string? Count_TongGiaTriGiaiNgan { get; set; }
    }

    public class DashBoard_TyLeGiaiNganModel
    {
        public string? Count_DuAnCoTyLeGiaiNganThap { get; set; }
        public string? Count_DuAnTreTienDo { get; set; }
        public string? Count_DuAnChuaDuocPheDuyet { get; set; }
    }

    public class DashBoard_VanBanHuongDan
    {
        public string? Value { get; set; }
        public string? Text { get; set; }
    }
}
