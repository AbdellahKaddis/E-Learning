using Ecommerce.BLL.Services;
using Ecommerce.DAL.Db;
using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly UserService _service;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        public UsersController(AppDbContext context, EmailService emailService, IConfiguration configuration)
        {
            var repo = new UserRepository(context);
            _service = new UserService(repo);
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _service.GetAllUsers();
            return users.Any() ? Ok(users) : NotFound("No Users Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUserById(int id)
        {
            var user = _service.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> AddUser(UserDTO dto)
        {
            if (dto == null)
                return BadRequest(new { error = "Invalid user data." });

            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return BadRequest(new { error = "FirstName is required." });

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return BadRequest(new { error = "LastName is required." });

            if (string.IsNullOrWhiteSpace(dto.DateOfBirth.ToString()))
                return BadRequest(new { error = "DateOfBirth is required." });

            if (!UserService.IsValidDate(dto.DateOfBirth.ToString()))
                return BadRequest(new { error = "Invalid Date Format." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { error = "Email is required." });

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { error = "Password is required." });

            if (string.IsNullOrWhiteSpace(dto.RoleId.ToString()))
                return BadRequest(new { error = "RoleId is required." });

            if(!EmailService.IsValidEmail(dto.Email))
                return BadRequest(new { error = "Invalid Email Format." });

            if (_service.IsEmailExists(dto.Email))
                return BadRequest(new { error = "Email already exists." });

            string subject = "Welcome to BitBot Enterprise!";
            string logoUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.designcrowd.com%2Fdesign%2F1904994&psig=AOvVaw2arplkXLNI3xkcZL__l-5D&ust=1744631978889000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCKCktuL61IwDFQAAAAAdAAAAABAE"; // Replace with your actual logo URL

            string body = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Welcome Email</title>
                        </head>
                        <body style='margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; background-color: #f4f4f4;'>
                            <table role='presentation' width='100%' cellspacing='0' cellpadding='0' style='max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 8px rgba(0,0,0,0.1);'>
                                <!-- Header -->
                                <tr>
                                    <td style='background-color: #007BFF; padding: 20px; text-align: center;'>
                                        <img src='{logoUrl}' alt='BitBot Enterprise Logo' style='max-width: 150px; height: auto; display: block; margin: 0 auto;'>
                                    </td>
                                </tr>
                                <!-- Content -->
                                <tr>
                                    <td style='padding: 40px 30px; text-align: left;'>
                                        <h1 style='color: #333333; font-size: 24px; margin: 0 0 20px; font-weight: normal;'>Welcome, {dto.FirstName} {dto.LastName}!</h1>
                                        <p style='color: #555555; font-size: 16px; line-height: 1.6; margin: 0 0 20px;'>Thank you for registering with BitBot Enterprise! We're excited to have you on board. Our platform is designed to help you streamline your workflows and achieve your goals efficiently.</p>
                                        <p style='color: #555555; font-size: 16px; line-height: 1.6; margin: 0 0 20px;'>Get started by exploring our features or contacting our support team if you have any questions.</p>
                                        <a href='https://yourdomain.com/get-started' style='display: inline-block; padding: 12px 24px; background-color: #007BFF; color: #ffffff; text-decoration: none; border-radius: 4px; font-size: 16px; font-weight: bold;'>Get Started Now</a>
                                    </td>
                                </tr>
                                <!-- Footer -->
                                <tr>
                                    <td style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                                        <p style='color: #777777; font-size: 14px; margin: 0 0 10px;'>BitBot Enterprise &copy; {DateTime.Now.Year}. All rights reserved.</p>
                                        <p style='color: #777777; font-size: 14px; margin: 0;'>
                                            <a href='https://yourdomain.com/privacy' style='color: #007BFF; text-decoration: none;'>Privacy Policy</a> | 
                                            <a href='https://yourdomain.com/contact' style='color: #007BFF; text-decoration: none;'>Contact Us</a>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </body>
                        </html>";

            // No need for Replace since we're directly embedding the name and logoUrl
            _emailService.SendEmail(dto.Email, subject, body, isBodyHtml: true);

            var created = _service.AddUser(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = created.Id }, created);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<string> Login(string Email,string Password)
        {
            if (string.IsNullOrWhiteSpace(Email))
                return BadRequest(new { error = "Email is required." });

            if (!EmailService.IsValidEmail(Email))
                return BadRequest(new { error = "Invalid Email Format." });

            if (string.IsNullOrWhiteSpace(Password))
                return BadRequest(new { error = "Password is required." });

            var user = _service.GetUserByEmail(Email);
            if (user == null)
            {
                return NotFound("Email not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                return BadRequest("Incorrect password");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(UserDTO user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        //[HttpPut("{id}")]
        //public ActionResult<BookDTO> UpdateBook(int id, BookDTO dto)
        //{
        //    if (id != dto.Id) return BadRequest("ID mismatch");

        //    var updated = _service.UpdateBook(dto);
        //    return updated == null ? NotFound() : Ok(updated);
        //}

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUser(int id)
        {
            var success = _service.DeleteUser(id);
            return success ? Ok() : NotFound();
        }
    }
}
