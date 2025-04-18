using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserCreatedDTO>> GetAllUsersAsync() // Async change
        {
            return await _context.Users.Include(u => u.Role)
                .Select(u => new UserCreatedDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    RoleName = u.Role.RoleName,
                    RoleId = u.RoleId
                }).ToListAsync(); // Async change
        }

        public async Task<UserCreatedDTO> GetUserByIdAsync(int id) // Async change
        {
            return await _context.Users.Include(u => u.Role)
                .Where(u => u.Id == id)
                .Select(u => new UserCreatedDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    RoleName = u.Role.RoleName,
                    RoleId = u.RoleId
                }).FirstOrDefaultAsync(); // Async change
        }

        public async Task<User> GetUserByEmailAsync(string Email) // Async change
        {
            return await _context.Users.Include(u => u.Role)
                .Where(u => u.Email == Email)
                .Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    Password = u.Password,
                    RoleId = u.RoleId,
                    Role = u.Role
                }).FirstOrDefaultAsync(); 
        }

        public async Task<int> AddUserAsync(UserDTO dto) 
        {
            var user = new User { FirstName = dto.FirstName, LastName = dto.LastName, DateOfBirth = dto.DateOfBirth, Email = dto.Email, Password = dto.Password, RoleId = dto.RoleId };
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); 
            return user.Id;
        }

        public async Task<bool> UpdateUserAsync(UserDTO dto) 
        {
            var user = await _context.Users.FindAsync(dto.Id); 
            if (user == null) return false;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.DateOfBirth = dto.DateOfBirth;
            user.Email = dto.Email;
            user.Password = dto.Password;
            user.RoleId = dto.RoleId;
            await _context.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id) 
        {
            var user = await _context.Users.FindAsync(id); 
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> IsEmailExistsAsync(string email) 
        {
            return await _context.Users.AnyAsync(u => u.Email == email); 
        }

        public async Task<bool> IsRoleExistsAsync(int id) 
        {
            return await _context.Roles.AnyAsync(r => r.Id == id); 
        }
    }
}