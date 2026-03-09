namespace wBialyDBAdapter.Model.User
{
    public class UserSecurityOverviewDto
    {
        public UserSecuritySettingsDto Settings { get; set; } = new();
        public UserSecurityStatsDto Stats { get; set; } = new();
    }
}
