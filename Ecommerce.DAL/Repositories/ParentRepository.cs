using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repositories
{
    public class ParentRepository
    {
        private readonly AppDbContext _context;

        public ParentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ParentDTO>> GetAllParentsAsync()
        {
            return await _context.Parents.Include(p => p.User)
                .Select(p => new ParentDTO
                {
                    Id = p.Id,
                    Address = p.Address,
                    UserId = p.UserId,
                    User = new UserDTO
                    {
                        Id = p.User.Id,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        Email = p.User.Email,
                        RoleName = p.User.Role.Name
                    }
                }).ToListAsync();
        }

        public async Task<ParentDTO?> GetParentByIdAsync(int id)
        {
            return await _context.Parents
                .Include(p => p.User)
                .Where(p => p.Id == id)
                .Select(p => new ParentDTO
                {
                    Id = p.Id,
                    Address = p.Address,
                    UserId = p.UserId,
                    User = new UserDTO
                    {
                        Id = p.User.Id,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        Email = p.User.Email,
                        RoleName = p.User.Role.Name
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddParentAsync(CreateParentDTO dto)
        {
            var parent = new Parent
            {
                Address = dto.Address,
                UserId = dto.UserId
            };
            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();
            return parent.Id;
        }

        public async Task<bool> UpdateParentAsync(int id, UpdateParentDTO dto)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent is null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                parent.Address = dto.Address;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteParentAsync(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent is null) return false;

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StudentInfoDTO>> GetParentChildrenByIdAsync(int id)
        {
           return await _context.Students
                .Include(s=>s.User)
                .Where(s => s.ParentId == id)
                .Select(s=> new StudentInfoDTO
                {
                    Id = s.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName
                }).ToListAsync();
        }
    }
}
