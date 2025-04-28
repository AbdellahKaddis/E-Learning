using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class ClasseService
    {
        private readonly ClassRepository _repo;

        public ClasseService(ClassRepository repo)
        {
            _repo = repo;
        }


        public Task<List<ClassDTO>> GetAllClassAsync() => _repo.GetAllClassAsync();
        public Task<ClassDTO> GetClassByIdAsync(int id) => _repo.GetClassByIdAsync(id);
        public Task<bool> AddClassAsync(CreateClassDTO clase) => _repo.AddClassAsync(clase);
        public Task<bool> UpdateClassAsync(int ID, UpdateClassDTO clase) => _repo.UpdateClassAsync(ID, clase);
        public Task<bool> DeleteClassAsync(int id) => _repo.DeleteClassAsync(id);

  }
}
