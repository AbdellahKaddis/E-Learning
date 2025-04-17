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
    public class CategoryService
    {
        private readonly CategoryRepositories   _repo;

        public CategoryService(CategoryRepositories repo)
        {
            _repo = repo;
        }
        public Category AddCategory(CategoryDTO category)
        {
            return _repo.AddCategory(category);
        }
        public List<CategoryDTO> GetAllCategories()=>_repo.GetAllCategories();
        public CategoryDTO GetCategoryById(int id) => _repo.GetCategoryById(id);
        public bool DeleteCategory(int id) => _repo.DeleteCategory(id);
        public bool UpdateCategory(CategoryDTO category) => _repo.UpdateCategory(category);

        //public bool UpdateCategory(Category existingCategory)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
