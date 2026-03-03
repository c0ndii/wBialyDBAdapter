using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Model.Message;
using wBialyDBAdapter.Repository.ObjectRelational.Message;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation.Message
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ORDB _context;
        public MessageRepository(ORDB context)
        {
            _context = context;
        }

        public async Task<MessageGetListDto> GetMessages()
        {
            var messages = await _context.Messages.Include(x => x.User).Include(x => x.CanModify).Select(x => new MessageDto
            {
                Id = x.Id,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                ModifiedAt = x.ModifiedAt,
                User = new Model.User.UserGetDto
                {
                    Id = x.User.UserId,
                    Username = x.User.Username
                },
                LatestModifyUsername = x.LatestModifyUsername,
                UserId = x.UserId,
                CanModify = x.CanModify.Select(y => new Model.User.UserGetDto
                {
                    Id = y.UserId,
                    Username = y.Username
                }).ToList()
            }).OrderByDescending(x => x.CreatedAt).ToListAsync();

            return new MessageGetListDto { Messages = messages };
        }

        public async Task<bool> DeleteMessage(int messageId, int userId)
        {
            var message = await _context.Messages.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == messageId);

            if (message == null || message.UserId != userId)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task CreateMessage(CreateMessageInput input, int userId)
        {
            await _context.Messages.AddAsync(new Database.ObjectRelational.Entities.Message.Message
            {
                Content = input.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMessage(UpdateMessageInput input, int userId)
        {
            var message = await _context.Messages.Include(x => x.User).Include(x => x.CanModify).FirstOrDefaultAsync(x => x.Id == input.MessageId);

            if (message == null || (message.UserId != userId && !message.CanModify.Any(x => x.UserId == userId)))
                return false;

            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            message.Content = input.Content;
            message.ModifiedAt = DateTime.UtcNow;
            message.LatestModifyUsername = currentUser.Username;

            _context.Update(message);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCanModifyBatch(UpdateMessageEditorsInput input, int userId)
        {
            var message = await _context.Messages.Include(x => x.User).Include(x => x.CanModify).FirstOrDefaultAsync(x => x.Id == input.MessageId);

            if (message == null || message.UserId != userId)
                return false;

            var users = await _context.Users.Where(x => input.UserIds.Any(y => y == x.UserId && y != userId)).ToListAsync();

            message.CanModify = users;
            _context.Update(message);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
