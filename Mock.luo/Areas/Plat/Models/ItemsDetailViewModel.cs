namespace Mock.luo.Areas.Plat.Models
{
    public class ItemsDetailViewModel
    {
        public int Id { get; set; }
        public int FId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int? SortCode { get; set; }
        public bool? IsEnableMark { get; set; }
        public string Remark { get; set; }
    }
}