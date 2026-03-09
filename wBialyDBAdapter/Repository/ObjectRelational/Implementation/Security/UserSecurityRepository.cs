using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using wBialyDBAdapter.Config;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Database.ObjectRelational.Entities.User;
using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Repository.ObjectRelational.Security;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation.Security
{
    public class UserSecurityRepository : IUserSecurityRepository
    {
        private readonly ORDB _context;
        private readonly UserSecurityOptions _securityOptions;

        public UserSecurityRepository(ORDB context, IOptions<UserSecurityOptions> securityOptions)
        {
            _context = context;
            _securityOptions = securityOptions.Value;
        }

        public async Task<LoginAttemptResultDto> Login(UserLoginInput input, string? ipAddress, string? userAgent)
        {
            var now = DateTime.UtcNow;
            var loginIdentifier = input.Login.Trim();

            var user = await _context.Users
                .Include(x => x.SecurityProfile)
                .FirstOrDefaultAsync(x => x.Login == loginIdentifier);

            if (user == null)
            {
                var delaySeconds = await CalculateGlobalDelaySeconds();

                await _context.LoginAttemptAudits.AddAsync(new LoginAttemptAudit
                {
                    AttemptedAtUtc = now,
                    LoginIdentifier = loginIdentifier,
                    IsExistingUser = false,
                    IsSuccessful = false,
                    FailureCategory = "InvalidCredentials",
                    AppliedDelaySeconds = delaySeconds,
                    LockedUntilUtc = now.AddSeconds(delaySeconds),
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                });

                await _context.SaveChangesAsync();

                return new LoginAttemptResultDto
                {
                    IsSuccess = false,
                    RetryAfterSeconds = delaySeconds,
                };
            }

            var profile = user.SecurityProfile ?? await CreateDefaultProfile(user.UserId);

            if (profile.NextAllowedLoginAtUtc.HasValue && profile.NextAllowedLoginAtUtc.Value > now)
            {
                var retryAfter = Math.Max(1, (int)Math.Ceiling((profile.NextAllowedLoginAtUtc.Value - now).TotalSeconds));

                await SaveAudit(profile.UserSecurityProfileId, loginIdentifier, now, false, "DelayWindow", retryAfter, profile.NextAllowedLoginAtUtc, ipAddress, userAgent);

                return new LoginAttemptResultDto
                {
                    IsSuccess = false,
                    RetryAfterSeconds = retryAfter,
                };
            }

            var isLocked = profile.IsLockoutEnabled && profile.LockedUntilUtc.HasValue && profile.LockedUntilUtc.Value > now;
            if (isLocked)
            {
                var retryAfter = Math.Max(1, (int)Math.Ceiling((profile.LockedUntilUtc!.Value - now).TotalSeconds));

                await SaveAudit(profile.UserSecurityProfileId, loginIdentifier, now, false, "AccountLocked", retryAfter, profile.LockedUntilUtc, ipAddress, userAgent);

                return new LoginAttemptResultDto
                {
                    IsSuccess = false,
                    RetryAfterSeconds = retryAfter,
                };
            }

            if (user.Password != input.Password)
            {
                profile.FailedLoginCountTotal += 1;
                profile.FailedLoginCountSinceLastSuccess += 1;
                profile.LastFailedLoginAtUtc = now;

                var delaySeconds = await CalculateGlobalDelaySeconds();
                profile.NextAllowedLoginAtUtc = now.AddSeconds(delaySeconds);

                if (profile.IsLockoutEnabled && profile.FailedLoginCountSinceLastSuccess >= profile.MaxFailedLoginAttempts)
                {
                    profile.LockedUntilUtc = profile.NextAllowedLoginAtUtc;
                }
                else
                {
                    profile.LockedUntilUtc = null;
                }

                await SaveAudit(profile.UserSecurityProfileId, loginIdentifier, now, false, "InvalidCredentials", delaySeconds, profile.LockedUntilUtc, ipAddress, userAgent);

                var retryAfter = delaySeconds;
                if (profile.LockedUntilUtc.HasValue && profile.LockedUntilUtc.Value > now)
                {
                    retryAfter = Math.Max(retryAfter, (int)Math.Ceiling((profile.LockedUntilUtc.Value - now).TotalSeconds));
                }

                return new LoginAttemptResultDto
                {
                    IsSuccess = false,
                    RetryAfterSeconds = retryAfter,
                };
            }

            profile.SuccessfulLoginCount += 1;
            profile.LastSuccessfulLoginAtUtc = now;
            profile.FailedLoginCountSinceLastSuccess = 0;
            profile.LockedUntilUtc = null;
            profile.NextAllowedLoginAtUtc = null;

            await SaveAudit(profile.UserSecurityProfileId, loginIdentifier, now, true, null, 0, null, ipAddress, userAgent);

            return new LoginAttemptResultDto
            {
                IsSuccess = true,
                User = new UserGetDto
                {
                    Id = user.UserId,
                    Username = user.Username,
                },
            };
        }

        public async Task<UserSecurityOverviewDto> GetSecurityOverview(int userId)
        {
            var profile = await GetProfileOrThrow(userId);
            var now = DateTime.UtcNow;

            return new UserSecurityOverviewDto
            {
                Settings = new UserSecuritySettingsDto
                {
                    IsLockoutEnabled = profile.IsLockoutEnabled,
                    MaxFailedLoginAttempts = profile.MaxFailedLoginAttempts,
                    IsPasswordManagerEnabled = profile.IsPasswordManagerEnabled,
                },
                Stats = new UserSecurityStatsDto
                {
                    LastFailedLoginAtUtc = profile.LastFailedLoginAtUtc,
                    LastSuccessfulLoginAtUtc = profile.LastSuccessfulLoginAtUtc,
                    FailedLoginCountSinceLastSuccess = profile.FailedLoginCountSinceLastSuccess,
                    FailedLoginCountTotal = profile.FailedLoginCountTotal,
                    SuccessfulLoginCount = profile.SuccessfulLoginCount,
                    IsLocked = profile.IsLockoutEnabled && profile.LockedUntilUtc.HasValue && profile.LockedUntilUtc.Value > now,
                    LockedUntilUtc = profile.LockedUntilUtc,
                    NextAllowedLoginAtUtc = profile.NextAllowedLoginAtUtc,
                }
            };
        }

        public async Task<UserSecuritySettingsDto> UpdateSecuritySettings(int userId, UserSecuritySettingsDto input)
        {
            var profile = await GetProfileOrThrow(userId);

            profile.IsLockoutEnabled = input.IsLockoutEnabled;
            profile.MaxFailedLoginAttempts = Math.Clamp(input.MaxFailedLoginAttempts, 1, 20);
            profile.IsPasswordManagerEnabled = input.IsPasswordManagerEnabled;

            await _context.SaveChangesAsync();

            return new UserSecuritySettingsDto
            {
                IsLockoutEnabled = profile.IsLockoutEnabled,
                MaxFailedLoginAttempts = profile.MaxFailedLoginAttempts,
                IsPasswordManagerEnabled = profile.IsPasswordManagerEnabled,
            };
        }

        private async Task SaveAudit(
            int? userSecurityProfileId,
            string loginIdentifier,
            DateTime attemptedAtUtc,
            bool isSuccessful,
            string? failureCategory,
            int delaySeconds,
            DateTime? lockedUntilUtc,
            string? ipAddress,
            string? userAgent)
        {
            await _context.LoginAttemptAudits.AddAsync(new LoginAttemptAudit
            {
                AttemptedAtUtc = attemptedAtUtc,
                LoginIdentifier = loginIdentifier,
                IsExistingUser = userSecurityProfileId.HasValue,
                IsSuccessful = isSuccessful,
                FailureCategory = failureCategory,
                AppliedDelaySeconds = delaySeconds,
                LockedUntilUtc = lockedUntilUtc,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                UserSecurityProfileId = userSecurityProfileId,
            });

            await _context.SaveChangesAsync();
        }

        private async Task<UserSecurityProfile> GetProfileOrThrow(int userId)
        {
            return await _context.UserSecurityProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId)
                ?? throw new InvalidOperationException("User security profile not found.");
        }

        private async Task<UserSecurityProfile> CreateDefaultProfile(int userId)
        {
            var profile = new UserSecurityProfile
            {
                UserId = userId,
                IsLockoutEnabled = true,
                MaxFailedLoginAttempts = 5,
                IsPasswordManagerEnabled = true,
            };

            await _context.UserSecurityProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();

            return profile;
        }

        private async Task<int> GetGlobalConsecutiveFailures()
        {
            var recentAttempts = await _context.LoginAttemptAudits
                .AsNoTracking()
                .OrderByDescending(x => x.AttemptedAtUtc)
            .Take(1000)
                .Select(x => new { x.IsSuccessful })
                .ToListAsync();

            var failures = 0;
            foreach (var attempt in recentAttempts)
            {
                if (attempt.IsSuccessful)
                {
                    break;
                }

                failures += 1;
            }

            return failures;
        }

        private async Task<int> CalculateGlobalDelaySeconds()
        {
            var consecutiveFailures = await GetGlobalConsecutiveFailures() + 1;

            if (consecutiveFailures <= 0)
            {
                return 0;
            }

            var baseDelay = Math.Max(1, _securityOptions.BaseDelaySeconds);
            var stepDelay = Math.Max(1, _securityOptions.DelayStepSeconds);
            var maxDelay = Math.Max(baseDelay, _securityOptions.MaxDelaySeconds);

            var delay = baseDelay + ((consecutiveFailures - 1) * stepDelay);
            return Math.Min(delay, maxDelay);
        }
    }
}
