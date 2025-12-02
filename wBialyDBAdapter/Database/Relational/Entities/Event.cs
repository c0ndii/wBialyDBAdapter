namespace wBialyDBAdapter.Database.Relational.Entities
{
    public class Event : OnSite
    {
        public DateTime EventDate;
        public IEnumerable<Tag_Event> EventTags {  get; set; } = new List<Tag_Event>();
    }
}
