namespace wBialyDBAdapter.Database.ObjectRelational.Entities
{
    public class Tag_Gastro : Tag
    {
        public int GastroID { get; set; }
        public ICollection<Gastro> Gastros { get; set; } = new List<Gastro>();
    }
}
