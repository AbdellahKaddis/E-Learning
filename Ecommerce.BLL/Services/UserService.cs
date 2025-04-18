using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService(UserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserCreatedDTO>> GetAllUsersAsync() => await _repo.GetAllUsersAsync(); 

        public async Task<UserCreatedDTO> GetUserByIdAsync(int id) => await _repo.GetUserByIdAsync(id); 

        public async Task<User> GetUserByEmailAsync(string Email) => await _repo.GetUserByEmailAsync(Email); 

        public async Task<UserCreatedDTO> AddUserAsync(UserDTO dto) 
        {
            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            dto.Id = await _repo.AddUserAsync(dto); 
            var createdUser = await _repo.GetUserByEmailAsync(dto.Email); 
            return new UserCreatedDTO
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                RoleId = dto.RoleId,
                RoleName = createdUser.Role.RoleName
            };
        }

        public async Task<UserCreatedDTO> UpdateUserAsync(UserDTO dto) 
        {
            var updated = await _repo.UpdateUserAsync(dto); 
            if (!updated) return null;

            var updatedUser = await _repo.GetUserByEmailAsync(dto.Email); 
            return new UserCreatedDTO
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                RoleId = dto.RoleId,
                RoleName = updatedUser.Role.RoleName
            };
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