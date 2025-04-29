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
    public class InstructorRepository
    {
        private readonly AppDbContext _context;

        public InstructorRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<InstructorDTO>> GetAllInstructorsAsync()
        {
            return await _context.Instructors.Include(i => i.User)
                .Select(i => new InstructorDTO
                {
                    Id = i.Id,
                    Address = i.Address,
                    Cin = i.Cin,
                    Telephone = i.Telephone,
                    Specialite = i.Specialite,
                    User = new UserDTO
                    {
                        Id = i.User.Id,
                        FirstName = i.User.FirstName,
                        LastName = i.User.LastName,
                        Email = i.User.Email,
                        RoleName = i.User.Role.Name
                    }
                }).ToListAsync();
        }

        public async Task<InstructorDTO?> GetInstructorByIdAsync(int id)
        {
            return await _context.Instructors
                .Include(i => i.User)
                .Where(i => i.Id == id)
                .Select(i => new InstructorDTO
                {
                    Id = i.Id,
                    Address = i.Address,
                    Cin = i.Cin,
                    Telephone = i.Telephone,
                    Specialite = i.Specialite,
                    User = new UserDTO
                    {
                        Id = i.User.Id,
                        FirstName = i.User.FirstName,
                        LastName = i.User.LastName,
                        Email = i.User.Email,
                        RoleName = i.User.Role.Name
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddInstructorAsync(CreateInstructorDTO dto)
        {
            var instructor = new Instructor
            {
                Address = dto.Address,
                UserId = dto.UserId,
                Specialite = dto.Specialite,
                Telephone = dto.Telephone,
                Cin = dto.Cin
            };
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();
            return instructor.Id;
        }

        public async Task<bool> UpdateInstructorAsync(int id, UpdateInstructorDTO dto)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor is null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                instructor.Address = dto.Address;
            if (!string.IsNullOrWhiteSpace(dto.Cin))
                instructor.Cin = dto.Cin;
            if (!string.IsNullOrWhiteSpace(dto.Telephone))
                instructor.Telephone = dto.Telephone;
            if (!string.IsNullOrWhiteSpace(dto.Specialite))
                instructor.Specialite = dto.Specialite;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteInstructorAsync(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor is null) return false;

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}

