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
    public class LevelRepository
    {
        private readonly AppDbContext _context;

        public LevelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LevelDTO>> GetAllLevelsAsync()
        {
            return await _context.Levels
                .Select(level => new LevelDTO
                {
                    Id = level.Id,
                    Name = level.Name,
                })
                .ToListAsync();
        }

        public async Task<LevelDTO> GetLevelByIdAsync(int id)
        {
            return await _context.Levels
                .Where(level => level.Id == id)
                .Select(level => new LevelDTO
                {
                    Id = level.Id,
                    Name = level.Name,
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> AddLevelAsync(CreateLevelDTO levelDto)
        {
            var level = new Level
            {
                Name = levelDto.Name,
            };

            await _context.Levels.AddAsync(level);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateLevelAsync(int id, UpdateLevelDTO levelDto)
        {
            var level = await _context.Levels.FindAsync(id);
            if (level == null) return false;

            level.Name = levelDto.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLevelAsync(int id)
        {
            var level = await _context.Levels.FindAsync(id);
            if (level == null) return false;

            _context.Levels.Remove(level);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
