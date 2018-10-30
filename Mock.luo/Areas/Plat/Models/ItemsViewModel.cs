namespace Mock.luo.Areas.Plat.Models
{
    public class ItemsViewModel
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public bool? Open { get; set; }
        public int? SortCode { get; set; }
        public bool? IsEnableMark { get; set; }
        public string Remark { get; set; }
    }
}