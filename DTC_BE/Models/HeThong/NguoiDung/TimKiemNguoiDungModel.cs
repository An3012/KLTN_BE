namespace DTC_BE.Models.HeThong.NguoiDung
{
    public class TimKiemNguoiDungModel
    {
        public int id { get; set; }
        public string tuKhoa { get; set; }
        public string donViId { get; set; }
        public int? loaiTaiKhoan { get; set; }
        public int currentPage { get; set; }
        public int rowPerPage { get; set; }
    }
}
