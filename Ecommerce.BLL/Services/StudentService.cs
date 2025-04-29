using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class StudentService
    {
        private readonly UserService _userService;
        private readonly StudentRepository _studentRepo;
        private readonly EmailService _emailService;
        private readonly ParentService _parentService;

        public StudentService(UserService userService, StudentRepository studentRepo, EmailService emailService, ParentService parentService)
        {
            _userService = userService;
            _studentRepo = studentRepo;
            _emailService = emailService;
            _parentService = parentService;
        }

        public async Task<List<StudentDTO>> GetAllStudentsAsync() => await _studentRepo.GetAllStudentsAsync();

        public async Task<StudentDTO?> GetStudentByIdAsync(int id) => await _studentRepo.GetStudentByIdAsync(id);

        public async Task<StudentDTO> AddStudentAsync(CreateStudentDTO dto)
        {
            int studentId = await _studentRepo.AddStudentAsync(dto);
            return await GetStudentByIdAsync(studentId)
                ?? throw new Exception("Failed to retrieve newly added student.");
        }
        private static string GenerateBaseUsername(string firstName, string lastName)
        {
            var cleanFirstName = CleanName(firstName);
            var cleanLastName = CleanName(lastName);
            return $"{cleanFirstName}.{cleanLastName}";
        }

        private static string CleanName(string name)
        {
            return name.ToLowerInvariant().Replace(" ", "").Replace("'", "");
        }

        private static string GenerateUniqueSuffix()
        {
            // Use first 6 characters of a cryptographic hash for better security
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Guid.NewGuid().ToByteArray());
            return BitConverter.ToString(hash)[..6].Replace("-", "");
        }

        private static string GenerateSecurePassword()
        {
            const int _passwordLength = 12;
            const string validChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@#$%&*?";
            var data = new byte[_passwordLength];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            var chars = new char[_passwordLength];
            for (int i = 0; i < _passwordLength; i++)
            {
                chars[i] = validChars[data[i] % validChars.Length];
            }

            return new string(chars);
        }
        public async Task<StudentDTO> CreateStudentWithUserAsync(CreateUserWithStudentDTO dto)
        {
            int studentRoleId = 4;
            string _domain = "bitbot.ma";

            var baseUsername = GenerateBaseUsername(dto.FirstName, dto.LastName);
            var uniqueSuffix = GenerateUniqueSuffix();

            var generatedEmail = $"{baseUsername}.{uniqueSuffix}@{_domain}";
            var generatedPassword = GenerateSecurePassword();

            var createdUser = await _userService.AddUserAsync(new CreateUserDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = generatedEmail,
                Password = generatedPassword,
                RoleId = studentRoleId
            });

            var studentId = await _studentRepo.AddStudentAsync(new CreateStudentDTO
            {
                DateOfBirth = dto.DateOfBirth,
                ParentId = dto.ParentId,
                LevelId = dto.LevelId,
                UserId = createdUser.Id,
                ClasseId = dto.ClasseId
            });

            var parent = await _parentService.GetParentByIdAsync(dto.ParentId);

            await this.SendStudentCredentialsEmailAsync($"{dto.FirstName} {dto.LastName}", generatedEmail, generatedPassword, parent.User.Email);
            
            return await GetStudentByIdAsync(studentId)
                ?? throw new Exception("Failed to retrieve newly added student.");
        }
        public async Task SendStudentCredentialsEmailAsync(string studentName, string email, string password, string toEmail)
        {
            var schoolName = "BitBot";
            var subject = $"Your {schoolName} Student Account Credentials";
            var logoUrl = "https://i.imgur.com/luvAL8B.png";
  
            var body = $@"
                <!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Student Credentials</title>
</head>
<body style=""margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; color: #1f2937; background-color: #f4f4f4;"">
  <table role=""presentation"" style=""width: 100%; border-collapse: collapse; background-color: #f4f4f4; padding: 0; margin: 0;"">
    <tr>
      <td align=""center"" style=""padding: 40px 10px;"">
        <table role=""presentation"" style=""width: 100%; max-width: 600px; border-collapse: collapse; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.1);"">
          <!-- Header -->
          <tr>
            <td style=""text-align: center; padding: 30px 20px; background: linear-gradient(135deg, #3b82f6, #1e3a8a);"">
              <img src='{logoUrl}' alt=""School Logo"" style=""max-width: 180px; height: auto; display: block; margin: 0 auto;"">
              <h1 style=""color: #ffffff; font-size: 24px; font-weight: 600; margin: 20px 0 10px;"">Welcome, {studentName}!</h1>
              <p style=""color: #dbeafe; font-size: 16px; margin: 0;"">Your student account is ready to go</p>
            </td>
          </tr>
          <!-- Content -->
          <tr>
            <td style=""padding: 40px 20px;"">
              <p style=""color: #4b5563; font-size: 16px; line-height: 1.6; margin: 0 0 20px;"">
                We're excited to have you on board! Below are your login credentials to access your student account.
              </p>
              <div style=""background: #f8fafc; padding: 20px; border-radius: 8px; margin: 25px 0; border: 1px solid #e5e7eb;"">
                <table style=""width: 100%; border-collapse: collapse;"">
                  <tr>
                    <td style=""padding: 10px 0; width: 100px; color: #1f2937; font-weight: 500;"">Email:</td>
                    <td style=""padding: 10px 0;""><code style=""background: #e5e7eb; padding: 4px 8px; border-radius: 4px; font-size: 14px; color: #1f2937;"">{email}</code></td>
                  </tr>
                  <tr>
                    <td style=""padding: 10px 0; color: #1f2937; font-weight: 500;"">Password:</td>
                    <td style=""padding: 10px 0;""><code style=""background: #e5e7eb; padding: 4px 8px; border-radius: 4px; font-size: 14px; color: #1f2937;"">{password}</code></td>
                  </tr>
                </table>
              </div>
              <a href=""#"" style=""display: inline-block; background-color: #3b82f6; color: #ffffff; padding: 12px 24px; border-radius: 8px; text-decoration: none; font-size: 16px; font-weight: 500; text-align: center; transition: background-color 0.2s;"" onmouseover=""this.style.backgroundColor='#2563eb'"" onmouseout=""this.style.backgroundColor='#3b82f6'"">Log In Now</a>
              <!-- Security Notes -->
              <div style=""margin-top: 30px; padding-top: 20px; border-top: 1px solid #e5e7eb;"">
                <h3 style=""color: #1f2937; font-size: 18px; font-weight: 600; margin: 0 0 15px;"">Security Notes</h3>
                <ul style=""padding-left: 20px; color: #4b5563; font-size: 14px; line-height: 1.6; margin: 0;"">
                  <li style=""margin-bottom: 10px;"">This is a temporary password - please change it after your first login.</li>
                  <li style=""margin-bottom: 10px;"">Never share your credentials with anyone.</li>
                  <li>Contact <a href=""mailto:bitbot@bitbot.com"" style=""color: #3b82f6; text-decoration: none;"">bitbot@bitbot.com</a> if you notice any suspicious activity.</li>
                </ul>
              </div>
            </td>
          </tr>
          <!-- Footer -->
          <tr>
            <td style=""background-color: #f8fafc; padding: 20px; text-align: center; border-top: 1px solid #e5e7eb;"">
              <p style=""color: #6b7280; font-size: 14px; margin: 0 0 10px;"">© {DateTime.Now.Year} {schoolName}. All rights reserved.</p>
              <p style=""color: #6b7280; font-size: 14px; margin: 0;"">Technopark</p>
              <p style=""color: #6b7280; font-size: 14px; margin: 10px 0 0;"">
                <a href=""#"" style=""color: #3b82f6; text-decoration: none; margin: 0 10px;"">Privacy Policy</a> |
                <a href=""#"" style=""color: #3b82f6; text-decoration: none; margin: 0 10px;"">Unsubscribe</a>
              </p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>
            ";

            await _emailService.SendEmailAsync(toEmail, subject, body, true);
        }
        public async Task<StudentDTO?> UpdateStudentAsync(int id, UpdateStudentDTO dto)
        {
            var success = await _studentRepo.UpdateStudentAsync(id, dto);
            return success ? await GetStudentByIdAsync(id) : null;
        }

        public async Task<StudentDTO?> UpdateStudentWithUserAsync(int id, UpdateStudentWithUserDTO dto)
        {
            var student = await GetStudentByIdAsync(id);
            if (student == null) return null;

            await _userService.UpdateUserAsync(student.User.Id, new UpdateUserDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = null,// the student can't change his email becuase we generated to him a unique email
                Password = dto.Password,
            });

            var updated = await UpdateStudentAsync(id, new UpdateStudentDTO
            {
                DateOfBirth = dto.DateOfBirth,
                ParentId = dto.ParentId,
                LevelId = dto.LevelId,
                ClasseId = dto.ClassId

            });

            return updated;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await GetStudentByIdAsync(id);
            if (student == null) return false;

            bool isDeleted = await _studentRepo.DeleteStudentAsync(id);
            if (isDeleted)
            {
                return await _userService.DeleteUserAsync(student.User.Id);
            }

            return false;
        }
    }
}
