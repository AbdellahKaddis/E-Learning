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
    public class ClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ClassDTO>> GetAllClassAsync()
        {
            return await _context.Classes
                .Select(clase => new ClassDTO
                {
                    Id = clase.Id,
                    Name = clase.Name,
                })
                .ToListAsync();
        }

        public async Task<ClassDTO> GetClassByIdAsync(int id)
        {
            return await _context.Classes
                .Where(clase => clase.Id == id)
                .Select(clase => new ClassDTO
                {
                    Id = clase.Id,
                    Name = clase.Name,
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> AddClassAsync(CreateClassDTO clase)
        {
            var clas = new Classe
            {
                Name = clase.Name,
            };

            await _context.Classes.AddAsync(clas);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateClassAsync(int ID, UpdateClassDTO cl)
        {
            var clase = await _context.Classes.FindAsync(ID);
            if (clase == null) return false;

            clase.Name = cl.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var clase = await _context.Classes.FindAsync(id);
            if (clase == null) return false;

            _context.Classes.Remove(clase);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
