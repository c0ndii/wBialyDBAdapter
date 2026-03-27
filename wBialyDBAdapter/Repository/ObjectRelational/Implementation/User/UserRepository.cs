using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Database.ObjectRelational.Entities.User;
using wBialyDBAdapter.Helpers;
using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Repository.ObjectRelational.User;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ORDB _context;
        public UserRepository(ORDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserGetDto>> GetAll(int excludedUserId)
        {
            return await _context.Users
                .Where(x => x.UserId != excludedUserId)
                .Select(x => new UserGetDto { Id = x.UserId, Username = x.Username })
                .ToListAsync();
        }

        public async Task<UserGetDto?> GetAsync(int userId)
        {
            return await _context.Users
                .Where(x => x.UserId == userId)
                .Select(x => new UserGetDto { Id = x.UserId, Username = x.Username })
                .FirstOrDefaultAsync();
        }

        public async Task Register(UserRegisterInput input)
        {
            if (input.Password.Length < 12 || input.Password.Length > 18)
            {
                throw new Exception("Hasło musi mieć od 12 do 18 znaków.");
            }

            var user = new Database.ObjectRelational.Entities.User.User
            {
                Login = input.Login,
                Password = input.Password,
                Username = input.Username,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(); 

            var securityProfile = new UserSecurityProfile
            {
                UserId = user.UserId,
                IsLockoutEnabled = true,
                MaxFailedLoginAttempts = 5,
                IsPasswordManagerEnabled = true,
            };

            for (int i = 1; i <= 10; i++)
            {
                var challenge = CreateChallengeSet(input.Password, i);
                securityProfile.PartialPasswords.Add(challenge);
            }

            await _context.UserSecurityProfiles.AddAsync(securityProfile);
            await _context.SaveChangesAsync();
        }

        private PartialPassword CreateChallengeSet(string password, int slotNumber)
        {
            var random = new Random();
            int len = password.Length;

            int maxToAsk = Math.Max(6, len / 2);
            int countToAsk = random.Next(6, maxToAsk + 1);

            var positions = Enumerable.Range(1, len)
                .OrderBy(_ => Guid.NewGuid())
                .Take(countToAsk)
                .OrderBy(x => x)
                .ToList();

            var salt = Guid.NewGuid().ToString();

            var hashes = positions.Select(p => PasswordHasher.ComputeHash(password[p - 1], p, salt));

            return new PartialPassword
            {
                SlotNumber = slotNumber,
                PasswordLength = len,
                RequiredPositions = string.Join(",", positions),
                CharacterHashes = string.Join(";", hashes),
                Salt = salt
            };
        }
    }
}