namespace wBialyDBAdapter.Model.User
{
    public class UserGetDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
