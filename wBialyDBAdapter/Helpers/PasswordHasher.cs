using System.Security.Cryptography;
using System.Text;

namespace wBialyDBAdapter.Helpers
{
    public static class PasswordHasher
    {
        public static string ComputeHash(char c, int position, string salt)
        {
            using var sha256 = SHA256.Create();

            var rawData = $"{c}{position}{salt}";
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            return Convert.ToBase64String(bytes);
        }

        public static (string hashes, string salt) HashMaskedPassword(string password)
        {
            var salt = Guid.NewGuid().ToString();
            var hashList = new List<string>();

            for (int i = 0; i < password.Length; i++)
            {
                var charHash = ComputeHash(password[i], i + 1, salt);
                hashList.Add(charHash);
            }

            return (string.Join(";", hashList), salt);
        }
    }
}