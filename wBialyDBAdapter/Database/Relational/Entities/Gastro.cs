namespace wBialyBezdomnyEdition.Database.Relational.Entities
{
    public class Gastro : OnSite
    {
        public DateTime Day;
        public IEnumerable<Tag_Gastro> GastroTags {  get; set; } = new List<Tag_Gastro>();
    }
}
