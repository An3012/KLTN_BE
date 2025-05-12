namespace DTC_BE.Models.HeThong.NhomQuyen
{
    public class TreeQuyen
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public bool? Checked { get; set; }
        public string? Code { get; set; }
        public int? Loai { get; set; }
        public List<TreeQuyen>? Children { get; set; }
    }
}
