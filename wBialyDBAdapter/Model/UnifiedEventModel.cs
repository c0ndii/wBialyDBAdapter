namespace wBialyDBAdapter.Model
{
    public class UnifiedEventModel
    {
        public string? Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public DateTime AddDate { get; set; }
        public string Place { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;

        public List<string> TagIds { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
    }
}
