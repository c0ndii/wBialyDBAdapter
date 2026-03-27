using System.Collections.ObjectModel;

namespace wBialyDBAdapter.Database.ObjectRelational.Entities.User
{
    public class UserSecurityProfile
    {
        public int UserSecurityProfileId { get; set; }
        public int UserId { get; set; }

        public int SuccessfulLoginCount { get; set; }
        public int FailedLoginCountTotal { get; set; }
        public int FailedLoginCountSinceLastSuccess { get; set; }
        public DateTime? LastFailedLoginAtUtc { get; set; }
        public DateTime? LastSuccessfulLoginAtUtc { get; set; }

        public bool IsLockoutEnabled { get; set; } = true;
        public int MaxFailedLoginAttempts { get; set; } = 5;
        public bool IsPasswordManagerEnabled { get; set; } = true;
        public DateTime? LockedUntilUtc { get; set; }
        public DateTime? NextAllowedLoginAtUtc { get; set; }

        public User User { get; set; } = null!;
        public ICollection<LoginAttemptAudit> LoginAttemptAudits { get; set; } = [];
        public ICollection<PartialPassword> PartialPasswords { get; set; } = new HashSet<PartialPassword>();
    }
}
