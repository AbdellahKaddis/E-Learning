using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.BLL.Services;
using Ecommerce.DAL.Db;
using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ForgotPasswordRequest = Ecommerce.Models.DTOs.ForgotPasswordRequest;
using ResetPasswordRequest = Ecommerce.Models.DTOs.ResetPasswordRequest;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public UsersController(UserService service, EmailService emailService, IConfiguration configuration)
        {
            _service = service;
            _emailService = emailService;
            _configuration = configuration;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers() 
        {
            var users = await _service.GetAllUsersAsync(); 
            return users.Any() ? Ok(users) : NotFound("No Users Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetUserById(int id) 
        {
            var user = await _service.GetUserByIdAsync(id); 
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> AddUser(CreateUserDTOWithoutRoleId dto) 
        {

            try
            {
                var createdUser = await _service.AddUserAsync(new CreateUserDTO
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = dto.Password,
                    RoleId = 1 // 1 for admin
                });
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Login(LoginDTO loginDTO) 
        {
            if (!EmailService.IsValidEmail(loginDTO.Email))
                return BadRequest(new { error = "Invalid Email Format." });

            var user = await _service.GetUserByEmailAsync(loginDTO.Email); 
            if (user == null) return NotFound("Email not found");

            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
                return BadRequest("Incorrect password");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, UpdateUserDTO dto) 
        {
            var updated = await _service.UpdateUserAsync(id, dto); 
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id) 
        {
            var success = await _service.DeleteUserAsync(id); 
            return success ? Ok("Deleted successfully") : NotFound("No user found.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request.");

            // Always return generic response to prevent email enumeration
            var response = Ok("If your email is registered, you’ll receive an OTP shortly.");

            var user = await _service.GetUserByEmailAsync(request.Email);
            if (user == null) return response;

            // Generate and hash OTP
            string otp = OtpService.GenerateOtp();
            string hashedOtp = OtpService.HashOtp(otp);

            // Save OTP to user record
            user.OtpHash = hashedOtp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10); // 10-minute expiry
            user.OtpUsed = false;
            await _service.UpdateUserAsync(user);

            // Send OTP via email (use a background job in production)
            await _emailService.SendOtpEmailAsync(user.Email, otp);

            return response;
        }
        
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid request.");

            var user = await _service.GetUserByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("Invalid request."); 

            // Check OTP validity
            if ((bool)user.OtpUsed || user.OtpExpiry < DateTime.UtcNow)
                return BadRequest("OTP expired or already used.");

            // Verify OTP hash
            string hashedInputOtp = OtpService.HashOtp(request.Otp);
            if (user.OtpHash != hashedInputOtp)
                return BadRequest("Invalid OTP.");

            // Generate a secure reset token
            string resetToken = GenerateResetToken();

            // Store the reset token in the user record
            user.ResetToken = resetToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(10); // 10-minute expiry
            user.OtpUsed = true; // Invalidate OTP after successful verification
            await _service.UpdateUserAsync(user);

            return Ok(new { ResetToken = resetToken });
        }
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request.");

            // Find user by email
            var user = await _service.GetUserByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("Invalid request.");

            // Validate reset token
            if (user.ResetToken != request.ResetToken || user.ResetTokenExpiry < DateTime.UtcNow)
                return BadRequest("Invalid or expired reset token.");

            // Hash the new password
            string newPasswordHash = HashPassword(request.NewPassword);

            // Update the user's password and invalidate the reset token
            user.Password = newPasswordHash;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _service.UpdateUserAsync(user);

            return Ok("Password reset successfully.");
        }

        // Example password hashing with BCrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString();
        }
        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                     new Claim(ClaimTypes.Role, user.Role.Name),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}