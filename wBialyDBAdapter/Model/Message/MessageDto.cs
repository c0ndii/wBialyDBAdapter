using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Model.Message
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string LatestModifyUsername { get; set; } = string.Empty;
        public int UserId { get; set; }
        public UserGetDto User { get; set; }
        public ICollection<UserGetDto> CanModify { get; set; } = new List<UserGetDto>();
    }
}
