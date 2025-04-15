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
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<UserDTO> GetAllUsers()
        {
            return _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    Password = u.Password,
                    RoleId = u.RoleId
                }).ToList();
        }

        public UserDTO GetUserById(int id)
        {
            return _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    Password = u.Password,
                    RoleId = u.RoleId
                }).FirstOrDefault();
        }

        public UserDTO GetUserByEmail(string Email)
        {
            return _context.Users
                .Where(u => u.Email == Email)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    Password = u.Password,
                    RoleId = u.RoleId
                }).FirstOrDefault();
        }

        public int AddUser(UserDTO dto)
        {
            var user = new User { FirstName = dto.FirstName, LastName = dto.LastName, DateOfBirth = dto.DateOfBirth, Email = dto.Email, Password = dto.Password, RoleId = dto.RoleId };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }

        //public bool UpdateAuthor(AuthorDTO dto)
        //{
        //    var author = _context.Authors.Find(dto.Id);
        //    if (author == null) return false;

        //    author.FullName = dto.FullName;
        //    _context.SaveChanges();
        //    return true;
        //}

        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public bool IsEmailExists(string email)
        {
            var isExist = _context.Users.Any(u => u.Email == email);

            return isExist;
        }
    }
}
