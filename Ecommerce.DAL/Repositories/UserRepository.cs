using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Macs;
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

        public async Task<List<UserDTO>> GetAllUsersAsync() 
        {
            return await _context.Users.Include(u => u.Role)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    RoleName = u.Role.Name,
                }).ToListAsync(); 
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Id == id)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    RoleName = u.Role.Name
                })
                .FirstOrDefaultAsync();
        }


        public async Task<User?> GetUserByEmailAsync(string Email)
        {
            return await _context.Users.Include(u => u.Role)
                .Where(u => u.Email == Email)
                .Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Password = u.Password,
                    RoleId = u.RoleId,
                    Role = u.Role,
                    OtpHash = u.OtpHash,
                    OtpExpiry = u.OtpExpiry,
                    OtpUsed = u.OtpUsed,
                    ResetToken = u.ResetToken,
                    ResetTokenExpiry = u.ResetTokenExpiry
                }).FirstOrDefaultAsync(); 
        }

        public async Task<int> AddUserAsync(CreateUserDTO dto) 
        {
            var user = new User { FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = dto.Password, RoleId = dto.RoleId };
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); 
            return user.Id;
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDTO dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return false;

            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!string.IsNullOrWhiteSpace(dto.LastName))
                user.LastName = dto.LastName;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password); ;
            }

            if (dto.RoleId.HasValue)
                user.RoleId = dto.RoleId.Value;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteUserAsync(int id) 
        {
            var user = await _context.Users.FindAsync(id); 
            if (user is null) return false;

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
        public async Task<RoleDTO?> GetRoleById(int id)
        {
            return await _context.Roles.
                Select(r=> new RoleDTO {
                    Id = r.Id,
                    Name = r.Name,
                }).FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}