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
    public class RoleService
    {
        private readonly RoleRepository _repo;

        public RoleService(RoleRepository repo)
        {
            _repo = repo;
        }

        public List<RoleDTO> GetAllRoles() => _repo.GetAllRoles();

        public RoleDTO GetRoleById(int id) => _repo.GetRoleById(id);

        public RoleDTO AddRole(RoleDTO newRole)
        {
            newRole.Id = _repo.AddRole(newRole);
            return newRole;
        }

        public RoleDTO UpdateRole(RoleDTO updatedRole)
        {
            var updated = _repo.UpdateRole(updatedRole);
            return updated ? updatedRole : null;
        }

        public bool DeleteRole(int id) => _repo.DeleteRole(id);

        

    }
}
