namespace wBialyDBAdapter.Model
{
    public class EndpointRequest
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 20;
        public int Skip => PageIndex * PageSize;
        public DatabaseType DatabaseType { get; set; } = DatabaseType.NoSQL;
    }
}
