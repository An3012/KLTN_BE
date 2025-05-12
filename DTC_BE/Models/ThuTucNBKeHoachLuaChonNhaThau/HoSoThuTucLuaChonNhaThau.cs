namespace DTC_BE.Models.ThuTucNBKeHoachLuaChonNhaThau
{
    public class HoSoThuTucLuaChonNhaThau
    {
        public class ThongTinThuTucLuaChonNhaThau
        {
            public string Id { get; set; }
            public string? TenDuAn { get; set; }
            public string? NhomDuAn { get; set; }
            public string? NguonVon { get; set; }
            public string? TenChuDauTu { get; set; }
            public DateTime? NgayTao { get; set; }
        }

    }
}
