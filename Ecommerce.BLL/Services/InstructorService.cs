using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce.BLL.Services
{
    public class InstructorService
    {
        private readonly UserService _userService;
        private readonly InstructorRepository _instructorRepo;
        private readonly EmailService _emailService;


        public InstructorService(UserService userService, InstructorRepository repo, EmailService emailService)
        {
            _userService = userService;
            _instructorRepo = repo;
            _emailService = emailService;
        }
        public async Task<List<InstructorDTO>> GetAllInstructorsAsync() => await _instructorRepo.GetAllInstructorsAsync();
        public async Task<InstructorDTO?> GetInstructorByIdAsync(int id) => await _instructorRepo.GetInstructorByIdAsync(id);
        public async Task<InstructorDTO?> GetInstructorByUserIdAsync(int userId) => await _instructorRepo.GetInstructorByUserIdAsync(userId);
        public async Task<InstructorDTO> AddInstructorAsync(CreateInstructorDTO dto)
        {
            int insertedId = await _instructorRepo.AddInstructorAsync(dto);
            var instructor = await _instructorRepo.GetInstructorByIdAsync(insertedId);
            return new InstructorDTO
            {
                Id = insertedId,
                Address = instructor?.Address ?? string.Empty,
                Cin = instructor?.Cin ?? string.Empty,
                Specialite = instructor?.Specialite ?? string.Empty,
                Telephone = instructor?.Telephone ?? string.Empty,
                User = instructor?.User
            };
        }
        public async Task SendInstructorCredentialsEmailAsync(string instructorName, string password, string toEmail)
        {
            var schoolName = "BitBot";
            var subject = $"Your {schoolName} Instructor Account Credentials";
            var logoUrl = "https://i.imgur.com/luvAL8B.png";
            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Instructor Credentials</title>
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
                            <h1 style=""color: #ffffff; font-size: 24px; font-weight: 600; margin: 20px 0 10px;"">Welcome, {instructorName}!</h1>
                            <p style=""color: #dbeafe; font-size: 16px; margin: 0;"">Your instructor account is ready to go</p>
                        </td>
                    </tr>
                    <!-- Content -->
                    <tr>
                        <td style=""padding: 40px 20px;"">
                            <p style=""color: #4b5563; font-size: 16px; line-height: 1.6; margin: 0 0 20px;"">
                                We're excited to have you on board! Below is your password to access your instructor account.
                            </p>
                            <div style=""background: #f8fafc; padding: 20px; border-radius: 8px; margin: 25px 0; border: 1px solid #e5e7eb;"">
                                <table style=""width: 100%; border-collapse: collapse;"">
                                    <tr>
                                        <td style=""padding: 10px 0; color: #1f2937; font-weight: 500;"">Password:</td>
                                        <td style=""padding: 10px 0;""><code style=""background: #e5e7eb; padding: 4px 8px; border-radius: 4px; font-size: 14px; color: #1f2937;"">{password}</code></td>
                                    </tr>
                                </table>
                            </div>
                            <p style=""color: #4b5563; font-size: 16px; line-height: 1.6; margin: 0 0 20px;"">
                                Use your registered email address along with this password to log in.
                            </p>
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
        public async Task<InstructorDTO> CreateInstructorWithUserAsync(CreateUserWithInstructorDTO dto)
        {
            int instructorRoleId = 2;
            //if password is null we generate a password
            string? generatedPassword = null;
            if(dto.Password is null)
            {
                generatedPassword = StudentService.GenerateSecurePassword();
                dto.Password = generatedPassword;
            }
         
            var createdUser = await _userService.AddUserAsync(new CreateUserDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = instructorRoleId
            });


            var createdInstructor = await this.AddInstructorAsync(new CreateInstructorDTO
            {
                UserId = createdUser.Id,
                Address = dto.Address,
                Cin = dto.Cin,
                Telephone = dto.Telephone,
                Specialite = dto.Specialite
            });

            if (generatedPassword is null)
                await _emailService.SendEmailAsync(dto.Email, $"{dto.FirstName} {dto.LastName}", isBodyHtml: true);
            else
                await this.SendInstructorCredentialsEmailAsync($"{dto.FirstName} {dto.LastName}", generatedPassword, dto.Email);

            return new InstructorDTO
            {
                Id = createdInstructor.Id,
                Cin = createdInstructor.Cin,
                Address = createdInstructor.Address,
                Telephone = createdInstructor.Telephone,
                Specialite = createdInstructor.Specialite,
                User = new UserDTO
                {
                    Id = createdUser.Id,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    Email = createdUser.Email,
                    RoleName = createdUser.RoleName
                }
            };
        }
        public async Task<InstructorDTO?> UpdateInstructorAsync(int id, UpdateInstructorDTO dto)
        {
            var updated = await _instructorRepo.UpdateInstructorAsync(id, dto);
            if (!updated) return null;

            var updatedInstructor = await _instructorRepo.GetInstructorByIdAsync(id);
            return updatedInstructor;
        }

        public async Task<InstructorDTO?> UpdateParentWithUserAsync(int id, UpdateInstructorWithUserDTO dto)
        {
            var instructor = await this.GetInstructorByIdAsync(id);

            await _userService.UpdateUserAsync(instructor.User.Id, new UpdateUserDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
            });

            var updatedInstructor = await this.UpdateInstructorAsync(id, new UpdateInstructorDTO
            {
                Address = dto.Address,
                Cin = dto.Cin,
                Telephone = dto.Telephone,
                Specialite = dto.Specialite
            });
            return updatedInstructor;

        }
        public async Task<bool> DeleteInstructorAsync(int id)
        {
            var instructor = await this.GetInstructorByIdAsync(id);
            bool isInstructorDeleted = await _instructorRepo.DeleteInstructorAsync(id);
            if (isInstructorDeleted)
            {
                return await _userService.DeleteUserAsync(instructor.User.Id);
            }
            return false;
        }

    }
}
