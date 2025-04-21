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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<ActionResult<UserDTO>> AddUser(CreateUserDTO dto) 
        {
            //if (string.IsNullOrWhiteSpace(dto.DateOfBirth.ToString()))
            //    return BadRequest(new { error = "DateOfBirth is required." });

            //if (!UserService.IsValidDate(dto.DateOfBirth.ToString()))
            //    return BadRequest(new { error = "Invalid Date Format." });

            if (dto.RoleId != 1 && dto.RoleId != 2)
                return BadRequest(new { error = "Invalid RoleId." });
            try
            {
                var createdUser = await _service.AddUserAsync(dto);
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