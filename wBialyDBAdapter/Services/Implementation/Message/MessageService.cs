using wBialyDBAdapter.Model.Message;
using wBialyDBAdapter.Repository.ObjectRelational.Message;
using wBialyDBAdapter.Services.Message;

namespace wBialyDBAdapter.Services.Implementation.Message
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task CreateMessage(CreateMessageInput input, int userId)
        {
            await _messageRepository.CreateMessage(input, userId);
        }

        public async Task<bool> DeleteMessage(int messageId, int userId)
        {
            return await _messageRepository.DeleteMessage(messageId, userId);
        }

        public async Task<MessageGetListDto> GetMessages()
        {
            return await _messageRepository.GetMessages();
        }

        public async Task<bool> UpdateCanModifyBatch(UpdateMessageEditorsInput input, int userId)
        {
            return await _messageRepository.UpdateCanModifyBatch(input, userId);
        }

        public async Task<bool> UpdateMessage(UpdateMessageInput input, int userId)
        {
            return await _messageRepository.UpdateMessage(input, userId);
        }
    }
}
