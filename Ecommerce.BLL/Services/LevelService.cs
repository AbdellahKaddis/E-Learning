using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class LevelService
    {
        private readonly LevelRepository _repo;

        public LevelService(LevelRepository repo)
        {
            _repo = repo;
        }

        public Task<List<LevelDTO>> GetAllLevelsAsync() => _repo.GetAllLevelsAsync();
        public Task<LevelDTO> GetLevelByIdAsync(int id) => _repo.GetLevelByIdAsync(id);
        public Task<bool> AddLevelAsync(CreateLevelDTO levelDto) => _repo.AddLevelAsync(levelDto);
        public Task<bool> UpdateLevelAsync(int id, UpdateLevelDTO levelDto) => _repo.UpdateLevelAsync(id, levelDto);
        public Task<bool> DeleteLevelAsync(int id) => _repo.DeleteLevelAsync(id);
    }
}
