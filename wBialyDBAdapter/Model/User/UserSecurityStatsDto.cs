namespace wBialyDBAdapter.Model.User
{
    public class UserSecurityStatsDto
    {
        public DateTime? LastFailedLoginAtUtc { get; set; }
        public DateTime? LastSuccessfulLoginAtUtc { get; set; }
        public int FailedLoginCountSinceLastSuccess { get; set; }
        public int FailedLoginCountTotal { get; set; }
        public int SuccessfulLoginCount { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedUntilUtc { get; set; }
        public DateTime? NextAllowedLoginAtUtc { get; set; }
    }
}
