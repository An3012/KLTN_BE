namespace DTC_BE.Models.QuanLyThuTucNoiBoVeDuAnDauTuCong.DungChung
{
    public class TienDoThucHienXuLyModel
    {
        #region Tiến độ thực hiện dự án
        public string Id { get; set; } = null!;

        public string? IdThuTuc { get; set; }

        public string? CanBoGiaiQuyet { get; set; }

        public string? NoiDungGiaiQuyet { get; set; }

        public string? NgayGiaiQuyet { get; set; }

        public string? GhiChu { get; set; }

        public string? TrangThai { get; set; }

        #endregion

    }
}
