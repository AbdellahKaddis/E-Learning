using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using System.Collections.Generic;
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

        public async Task<List<RoleDTO>> GetAllRolesAsync() => await _repo.GetAllRolesAsync();

        public async Task<RoleDTO> GetRoleByIdAsync(int id) => await _repo.GetRoleByIdAsync(id);

        public async Task<RoleDTO> AddRoleAsync(RoleDTO newRole)
        {
            newRole.Id = await _repo.AddRoleAsync(newRole);
            return newRole;
        }

        public async Task<RoleDTO> UpdateRoleAsync(RoleDTO updatedRole)
        {
            var updated = await _repo.UpdateRoleAsync(updatedRole);
            return updated ? updatedRole : null;
        }

        public async Task<bool> DeleteRoleAsync(int id) => await _repo.DeleteRoleAsync(id);
    }
}
