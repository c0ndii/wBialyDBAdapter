using wBialyDBAdapter.Model.Message;

namespace wBialyDBAdapter.Repository.ObjectRelational.Message
{
    public interface IMessageRepository
    {
        Task<MessageGetListDto> GetMessages();
        Task<bool> DeleteMessage(int messageId, int userId);
        Task CreateMessage(CreateMessageInput input, int userId);
        Task<bool> UpdateMessage(UpdateMessageInput input, int userId);
        Task<bool> UpdateCanModifyBatch(UpdateMessageEditorsInput input, int userId);
    }
}
