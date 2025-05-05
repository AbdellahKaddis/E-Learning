using System.Security.Cryptography;
using System.Text;

namespace Ecommerce.BLL.Services
{
    public static class OtpService
    {
        public static string GenerateOtp()
        {
            // Generate 6 random bytes (48 bits of entropy)
            byte[] randomBytes = new byte[6];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            // Convert to a 6-digit number (000000 to 999999)
            int otpValue = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % 1000000;
            return otpValue.ToString("000000");
        }

        public static string HashOtp(string otp)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otp));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
