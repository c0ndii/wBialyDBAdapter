namespace wBialyDBAdapter.Model.Message
{
    public class UpdateMessageEditorsInput
    {
        public IEnumerable<int> UserIds { get; set; } = new List<int>();
        public int MessageId {  get; set; }
    }
}
