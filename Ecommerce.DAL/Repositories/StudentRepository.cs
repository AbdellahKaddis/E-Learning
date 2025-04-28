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
    public class StudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentDTO>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.Level)
                .Include(s => s.User)
                .Include(s => s.Parent)
                    .ThenInclude(p => p.User)
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    DateOfBirth = s.DateOfBirth,
                    UserId = s.UserId,
                    ParentId = s.ParentId,
                    User = new UserDTO
                    {
                        Id = s.User.Id,
                        FirstName = s.User.FirstName,
                        LastName = s.User.LastName,
                        Email = s.User.Email,
                        RoleName = s.User.Role.Name
                    },
                    Parent = new ParentDTO
                    {
                        Id = s.Parent.Id,
                        Address = s.Parent.Address,
                        UserId = s.Parent.UserId,
                        User = new UserDTO
                        {
                            Id = s.Parent.User.Id,
                            FirstName = s.Parent.User.FirstName,
                            LastName = s.Parent.User.LastName,
                            Email = s.Parent.User.Email,
                            RoleName = s.Parent.User.Role.Name
                        }
                    },
                    Level = new LevelDTO
                    {
                        Id = s.Level.Id,
                        Name = s.Level.Name
                    }
                }).ToListAsync();
        }

        public async Task<StudentDTO?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.Level)
                .Include(s => s.Parent)
                    .ThenInclude(p => p.User)
                .Where(s => s.Id == id)
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    DateOfBirth = s.DateOfBirth,
                    UserId = s.UserId,
                    ParentId = s.ParentId,
                    User = new UserDTO
                    {
                        Id = s.User.Id,
                        FirstName = s.User.FirstName,
                        LastName = s.User.LastName,
                        Email = s.User.Email,
                        RoleName = s.User.Role.Name
                    },
                    Parent = new ParentDTO
                    {
                        Id = s.Parent.Id,
                        Address = s.Parent.Address,
                        UserId = s.Parent.UserId,
                        User = new UserDTO
                        {
                            Id = s.Parent.User.Id,
                            FirstName = s.Parent.User.FirstName,
                            LastName = s.Parent.User.LastName,
                            Email = s.Parent.User.Email,
                            RoleName = s.Parent.User.Role.Name
                        }
                    },
                    Level = new LevelDTO
                    {
                        Id = s.Level.Id,
                        Name = s.Level.Name
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task<int> AddStudentAsync(CreateStudentDTO dto)
        {
            var student = new Student
            {
                DateOfBirth = dto.DateOfBirth,
                LevelId = dto.LevelId,
                UserId = dto.UserId,
                ParentId = dto.ParentId,
                ClasseId = dto.ClasseId,
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> UpdateStudentAsync(int id, UpdateStudentDTO dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            if (dto.DateOfBirth.HasValue)
                student.DateOfBirth = dto.DateOfBirth.Value;
            if (dto.LevelId.HasValue)
                student.LevelId = dto.LevelId.Value;

            if (dto.ParentId.HasValue)
                student.ParentId = dto.ParentId.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
        
    }

}
