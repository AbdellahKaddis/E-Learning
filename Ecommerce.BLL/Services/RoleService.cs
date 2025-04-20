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

        public async Task<RoleDTO?> GetRoleByIdAsync(int id) => await _repo.GetRoleByIdAsync(id);

        public async Task<RoleDTO> AddRoleAsync(CreateRoleDTO newRole)
        {
            int InsertedId = await _repo.AddRoleAsync(newRole);
            return new RoleDTO { Id = InsertedId, Name = newRole.Name};
        }

        public async Task<RoleDTO?> UpdateRoleAsync(UpdateRoleDTO dto)
        {
            var updated = await _repo.UpdateRoleAsync(dto);
            if (!updated) return null;

            var updatedRole = await _repo.GetRoleByIdAsync(dto.Id);
            return updated ? new RoleDTO { Id = updatedRole.Id, Name = updatedRole.Name } : null;
        }

        public async Task<bool> DeleteRoleAsync(int id) => await _repo.DeleteRoleAsync(id);
    }
}
