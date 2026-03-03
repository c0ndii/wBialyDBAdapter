
namespace wBialyDBAdapter.Database.ObjectRelational.Entities.Message
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string LatestModifyUsername { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual User.User User {  get; set; }
        public ICollection<User.User> CanModify { get; set; } = new List<User.User>();
    }
}
