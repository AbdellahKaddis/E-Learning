using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public List<RoleDTO> GetAllRoles()
        {
            return _context.Roles.Select(r => new RoleDTO { Id=r.Id, RoleName = r.RoleName}).ToList();
        }

        public RoleDTO GetRoleById(int id)
        {
            return _context.Roles.Select(r => new RoleDTO { Id = r.Id, RoleName = r.RoleName }).FirstOrDefault(r => r.Id == id);
        }

        public int AddRole(RoleDTO newRole)
        {
            var role = new Role { RoleName = newRole.RoleName};
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.Id;
        }

        public bool UpdateRole(RoleDTO updatedRole)
        {
            var role = _context.Roles.Find(updatedRole.Id);
            if (role == null) return false;

            role.RoleName = updatedRole.RoleName;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteRole(int id)
        {
            var role = _context.Roles.Find(id);
            if (role is null) return false;

            _context.Roles.Remove(role);
            _context.SaveChanges();
            return true;
        }


    }
}
