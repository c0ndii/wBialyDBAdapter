namespace wBialyBezdomnyEdition.Database.NoSQL.Entities
{
    public class Gastro : OnSite
    {
        public DateTime Day { get; set; }
        public List<Tag> Tags { get; set; } = new();
    }
}
