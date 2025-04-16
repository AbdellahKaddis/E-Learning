using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService(UserRepository repo)
        {
            _repo = repo;
        }

        public List<UserDTO> GetAllUsers() => _repo.GetAllUsers();

        public UserDTO GetUserById(int id) => _repo.GetUserById(id);

        public UserDTO GetUserByEmail(string Email) => _repo.GetUserByEmail(Email);


        public UserDTO AddUser(UserDTO dto)
        {
            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            dto.Id = _repo.AddUser(dto);
            return dto;
        }

        //public AuthorDTO UpdateAuthor(AuthorDTO dto)
        //{
        //    var updated = _repo.UpdateAuthor(dto);
        //    return updated ? dto : null;
        //}

        public bool DeleteUser(int id) => _repo.DeleteUser(id);

        public bool IsEmailExists(string email) => _repo.IsEmailExists(email);

        public static bool IsValidDate(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }

        public bool IsRoleExists(int id) => _repo.IsRoleExists(id);
    }
}
