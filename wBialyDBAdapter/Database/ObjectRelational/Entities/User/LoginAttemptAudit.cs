namespace wBialyDBAdapter.Database.ObjectRelational.Entities.User
{
    public class LoginAttemptAudit
    {
        public int LoginAttemptAuditId { get; set; }
        public DateTime AttemptedAtUtc { get; set; }
        public string LoginIdentifier { get; set; } = string.Empty;
        public bool IsExistingUser { get; set; }
        public bool IsSuccessful { get; set; }
        public string? FailureCategory { get; set; }
        public int AppliedDelaySeconds { get; set; }
        public DateTime? LockedUntilUtc { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        public int? UserSecurityProfileId { get; set; }
        public UserSecurityProfile? UserSecurityProfile { get; set; }
    }
}
