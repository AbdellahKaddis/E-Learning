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
        public bool AddClass(CreateClassDTO clase)
        {
            return _repo.AddClass(clase);
        }
        public List<ClassDTO> GetAllclass() => _repo.GetAllClass();
        public ClassDTO GetClassById(int id) => _repo.GetClassById(id);
        public bool DeleteClass(int id) => _repo.DeleteClass(id);
        public bool UpdateClass(int ID,UpdateClassDTO clase) => _repo.UpdateClass(ID,clase);
    }
}
