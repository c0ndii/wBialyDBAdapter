namespace wBialyDBAdapter.Database.ObjectRelational.Entities
{
    public abstract class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime AddDate { get; set; }
        public string Place { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}
