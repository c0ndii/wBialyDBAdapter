namespace wBialyDBAdapter.Database.ObjectRelational.Entities
{
    public class Tag_Event : Tag
    {
        public int EventID { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
