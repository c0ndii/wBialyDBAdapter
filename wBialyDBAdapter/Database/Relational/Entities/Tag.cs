namespace wBialyDBAdapter.Database.Relational.Entities
{
    public abstract class Tag
    {
        public int TagID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
