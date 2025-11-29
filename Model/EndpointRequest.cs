namespace wBialyDBAdapter.Model
{
    public class EndpointRequest : BaseRequest
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 20;
        public int Skip => PageIndex * PageSize;
    }
}
