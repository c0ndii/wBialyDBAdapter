namespace wBialyDBAdapter.Model.User
{
    public class UserSecuritySettingsDto
    {
        public bool IsLockoutEnabled { get; set; }
        public int MaxFailedLoginAttempts { get; set; }
        public bool IsPasswordManagerEnabled { get; set; }
    }
}
