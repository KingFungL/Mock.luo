

namespace Mock.Code.Web.TreeGrid
{
    public class TreeGridModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Text { get; set; }
        public bool IsLeaf { get; set; }
        public bool Expanded { get; set; }
        public bool Loaded { get; set; }
        public string EntityJson { get; set; }
    }
}
