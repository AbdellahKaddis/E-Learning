using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repositories
{
    public class RoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Select(r => new RoleDTO { Id = r.Id, Name = r.Name })
                .ToListAsync();
        }

        public async Task<RoleDTO?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles
                .Select(r => new RoleDTO { Id = r.Id, Name = r.Name })
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<int> AddRoleAsync(CreateRoleDTO newRole)
        {
            var role = new Role { Name = newRole.Name };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role.Id;
        }

        public async Task<bool> UpdateRoleAsync(UpdateRoleDTO updatedRole)
        {
            var role = await _context.Roles.FindAsync(updatedRole.Id);
            if (role == null) return false;

            if (!string.IsNullOrWhiteSpace(updatedRole.Name))
                role.Name = updatedRole.Name;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
