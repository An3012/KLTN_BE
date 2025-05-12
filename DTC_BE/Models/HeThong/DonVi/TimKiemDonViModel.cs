namespace DTC_BE.Models.HeThong.DonVi
{
    public class TimKiemDonViModel
    {
        public int id { get; set; }
        public string tuKhoa { get; set; }
        public string maDonVi { get; set; }
        public int currentPage { get; set; }
        public int rowPerPage { get; set; }
    }
}
