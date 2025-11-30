namespace wBialyDBAdapter.Database.ObjectRelational.Entities
{
    public class Gastro : OnSite
    {
        public DateTime Day;
        public IEnumerable<Tag_Gastro> GastroTags {  get; set; } = new List<Tag_Gastro>();
    }
}
