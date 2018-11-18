namespace Mock.Data.AppModel
{
    public class DataGrid
    {
        public DataGrid()
        {
        }

        public DataGrid(int total, object rows)
        {
            Total = total;
            Rows = rows;
        }

        public int Total { get; set; }
        public object Rows { get; set; }
        public object Footer { get; set; }
    }
}
