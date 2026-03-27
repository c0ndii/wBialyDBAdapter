namespace wBialyDBAdapter.Model.User
{
    public class PartialLoginInput
    {
        public string Login { get; set; } = string.Empty;
        public int PartialPasswordId { get; set; }
        public Dictionary<int, char> ProvidedCharacters { get; set; } = new();
    }
}
