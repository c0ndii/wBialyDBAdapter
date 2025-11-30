namespace wBialyDBAdapter.Model
{
    public class EndpointResponse<T>
    {
        public T Data { get; set; }
        public int TotalCount { get; set; }
        public double DbOperationTime { get; set; }

        public static EndpointResponse<T> SuccessResponse(T data, double timeMs, int totalCount)
            => new()
            {
                Data = data,
                DbOperationTime = timeMs,
                TotalCount = totalCount,
            };
    }
}
