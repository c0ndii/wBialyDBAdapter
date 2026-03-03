namespace wBialyDBAdapter.Model.Message
{
    public class UpdateMessageInput
    {
        public int MessageId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
