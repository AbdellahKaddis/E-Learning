using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using System;
using System.Collections.Generic;
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
            string _domain = "bitbot.edu";

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
                UserId = createdUser.Id
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
            var logoUrl = "https://static.vecteezy.com/system/resources/thumbnails/008/998/006/small/url-logo-url-letter-url-letter-logo-design-initials-url-logo-linked-with-circle-and-uppercase-monogram-logo-url-typography-for-technology-business-and-real-estate-brand-vector.jpg";
  
            var body = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Student Credentials</title>
    </head>
    <body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Arial, sans-serif; color: #333;"">
        <div style=""max-width: 600px; margin: 20px auto; background-color: #f7f9fc; border-radius: 10px; padding: 30px;"">
            <div style=""text-align: center; margin-bottom: 30px;"">
                <img src=""{logoUrl}"" alt=""School Logo"" style=""max-width: 200px; height: auto;"">
            </div>

            <div style=""background: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);"">
                <h1 style=""color: #2c3e50; margin-top: 0;"">Welcome, {studentName}!</h1>
                <p style=""font-size: 16px;"">Your student account has been created. Here are your login credentials:</p>

                <div style=""background: #f8f9fa; padding: 20px; border-radius: 6px; margin: 25px 0;"">
                    <table>
                        <tr>
                            <td style=""padding: 8px 0; width: 100px;""><strong>Email:</strong></td>
                            <td style=""padding: 8px 0;""><code style=""background: #e9ecef; padding: 4px 8px; border-radius: 4px;"">{email}</code></td>
                        </tr>
                        <tr>
                            <td style=""padding: 8px 0;""><strong>Password:</strong></td>
                            <td style=""padding: 8px 0;""><code style=""background: #e9ecef; padding: 4px 8px; border-radius: 4px;"">{password}</code></td>
                        </tr>
                    </table>
                </div>

             

                <div style=""margin-top: 25px; padding-top: 25px; border-top: 1px solid #eee;"">
                    <h3 style=""color: #2c3e50; margin-top: 0;"">Security Notes</h3>
                    <ul style=""padding-left: 20px; color: #666;"">
                        <li>This is a temporary password - change it after first login</li>
                        <li>Never share your credentials with anyone</li>
                        <li>Contact {"bitbot@bitbot.com"} if you notice suspicious activity</li>
                    </ul>
                </div>
            </div>

            <div style=""text-align: center; margin-top: 30px; color: #666; font-size: 14px;"">
                <p>© {DateTime.Now.Year} {schoolName}. All rights reserved.</p>
                <p>{"Technopark"}</p>
            </div>
        </div>
    </body>
    </html>";

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

            await _userService.UpdateUserAsync(student.UserId, new UpdateUserDTO
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
                return await _userService.DeleteUserAsync(student.UserId);
            }

            return false;
        }
    }
}
