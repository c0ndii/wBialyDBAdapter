namespace wBialyDBAdapter.Model
{
    public class PostRequest<T> : BaseRequest
    {
        public T? Data { get; set; }
    }
}
