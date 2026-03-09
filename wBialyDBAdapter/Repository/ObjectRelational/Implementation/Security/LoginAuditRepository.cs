using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Repository.ObjectRelational.Security;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation.Security
{
    public class LoginAuditRepository : ILoginAuditRepository
    {
        private readonly ORDB _context;

        public LoginAuditRepository(ORDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoginAuditItemDto>> GetLoginAudits(int? userId, bool includeAll, int limit)
        {
            var normalizedLimit = Math.Clamp(limit, 1, 200);

            var query = _context.LoginAttemptAudits.AsNoTracking().AsQueryable();

            if (!includeAll)
            {
                query = query.Where(x => x.UserSecurityProfile != null && x.UserSecurityProfile.UserId == userId);
            }
            else if (userId.HasValue)
            {
                query = query.Where(x => x.UserSecurityProfile != null && x.UserSecurityProfile.UserId == userId.Value);
            }

            return await query
                .OrderByDescending(x => x.AttemptedAtUtc)
                .Take(normalizedLimit)
                .Select(x => new LoginAuditItemDto
                {
                    Id = x.LoginAttemptAuditId,
                    AttemptedAtUtc = x.AttemptedAtUtc,
                    IsSuccessful = x.IsSuccessful,
                    IsExistingUser = x.IsExistingUser,
                    AppliedDelaySeconds = x.AppliedDelaySeconds,
                    LockedUntilUtc = x.LockedUntilUtc,
                    FailureCategory = x.FailureCategory,
                    IpAddress = x.IpAddress,
                    LoginIdentifier = x.LoginIdentifier,
                    UserId = x.UserSecurityProfile != null ? x.UserSecurityProfile.UserId : null,
                })
                .ToListAsync();
        }
    }
}
