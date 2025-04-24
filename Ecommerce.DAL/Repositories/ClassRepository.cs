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
        public List<ClassDTO> GetAllClass()
        {
            return _context.Classes
                .Select(clase => new ClassDTO
                {
                    Id = clase.Id,
                    Name = clase.Name,
                })
                .ToList();
        }

        public ClassDTO GetClassById(int id)
        {
            return _context.Classes
                .Where(clase => clase.Id == id)
                .Select(clase => new ClassDTO
                {
                    Id = clase.Id,
                    Name = clase.Name,
                }).FirstOrDefault();
        }
        public bool AddClass(CreateClassDTO clase)
        {
            var clas = new Classe
            {
                Name = clase.Name,


            };

            _context.Classes.Add(clas);
            _context.SaveChanges();

            return true;
        }
        public bool UpdateClass(int ID,UpdateClassDTO cl)
        {
            var clase = _context.Classes.Find(ID);
            if (clase == null) return false;

            clase.Name = cl.Name;
            _context.SaveChanges();
            return true;
        }
        public bool DeleteClass(int id)
        {
            var clase = _context.Classes.Find(id);
            if (clase == null) return false;

            _context.Classes.Remove(clase);
            _context.SaveChanges();
            return true;
        }

    }
}
