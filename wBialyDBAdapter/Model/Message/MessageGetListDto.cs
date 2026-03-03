namespace wBialyDBAdapter.Model.Message
{
    public class MessageGetListDto
    {
        public IEnumerable<MessageDto> Messages { get; set; } = new List<MessageDto>();
    }
}
