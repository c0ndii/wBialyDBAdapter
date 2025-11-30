namespace wBialyDBAdapter.Database.ObjectRelational.Entities
{
    public class Event : OnSite
    {
        public DateTime EventDate;
        public ICollection<Tag_Event> EventTags {  get; set; } = new List<Tag_Event>();
    }
}
