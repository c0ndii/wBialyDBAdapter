namespace wBialyDBAdapter.Model.Dto
{
    public class OnSiteDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime AddDate { get; set; } = DateTime.UtcNow;
        public string Place { get; set; }
        public string Author { get; set; }
        public string Link { get; set; }
    }
}
