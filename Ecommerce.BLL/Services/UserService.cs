using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;
        private readonly EmailService _emailService;

        public UserService(UserRepository repo, EmailService emailService)
        {
            _repo = repo;
            _emailService = emailService;
        }

        public async Task<List<UserDTO>> GetAllUsersAsync() => await _repo.GetAllUsersAsync(); 

        public async Task<UserDTO?> GetUserByIdAsync(int id) => await _repo.GetUserByIdAsync(id); 

        public async Task<User?> GetUserByEmailAsync(string Email) => await _repo.GetUserByEmailAsync(Email); 

        public async Task<UserDTO> AddUserAsync(CreateUserDTO dto) 
        {

            if (!EmailService.IsValidEmail(dto.Email))
                throw new ArgumentException("Invalid Email Format.");

            if (await _repo.IsEmailExistsAsync(dto.Email))
                throw new ArgumentException("Email already exists.");

            if (!await _repo.IsRoleExistsAsync(dto.RoleId))
                throw new ArgumentException($"RoleId {dto.RoleId} does not exist.");

            if(dto.RoleId != 4)
                await _emailService.SendEmailAsync(dto.Email, $"{dto.FirstName} {dto.LastName}", isBodyHtml: true);

            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            int InsertedId = await _repo.AddUserAsync(dto);
            RoleDTO? role = await _repo.GetRoleById(dto.RoleId);
            return new UserDTO
            {
                Id = InsertedId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                RoleName = role?.Name
            };
        }

        public async Task<UserDTO?> UpdateUserAsync(int id, UpdateUserDTO dto) 
        {
            var updated = await _repo.UpdateUserAsync(id, dto); 
            if (!updated) return null;

            var updatedUser = await _repo.GetUserByIdAsync(id);
            return updatedUser;
        }

        public async Task<bool> DeleteUserAsync(int id) => await _repo.DeleteUserAsync(id); 

        public async Task<bool> IsEmailExistsAsync(string email) => await _repo.IsEmailExistsAsync(email); 

        public static bool IsValidDate(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }

        public async Task<bool> IsRoleExistsAsync(int id) => await _repo.IsRoleExistsAsync(id); 
    }
}