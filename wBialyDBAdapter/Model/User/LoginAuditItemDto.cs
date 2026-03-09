namespace wBialyDBAdapter.Model.User
{
    public class LoginAuditItemDto
    {
        public int Id { get; set; }
        public DateTime AttemptedAtUtc { get; set; }
        public int? UserId { get; set; }
        public string LoginIdentifier { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }
        public bool IsExistingUser { get; set; }
        public int AppliedDelaySeconds { get; set; }
        public DateTime? LockedUntilUtc { get; set; }
        public string? FailureCategory { get; set; }
        public string? IpAddress { get; set; }
    }
}
