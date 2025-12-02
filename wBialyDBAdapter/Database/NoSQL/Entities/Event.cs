namespace wBialyDBAdapter.Database.NoSQL.Entities
{
    public class Event : OnSite
    {
        public DateTime EventDate { get; set; }
        public List<Tag> Tags { get; set; } = new();
    }
}
