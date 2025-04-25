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
        public Task<List<CategoryDTO>> GetAllCategoriesAsync() => _repo.GetAllCategoriesAsync();
        public Task<Category> AddCategoryAsync(CategoryDTO category)
        {
            return _repo.AddCategoryAsync(category);
        }

        public Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            return _repo.GetCategoryByIdAsync(id);
        }

        public Task<bool> DeleteCategoryAsync(int id)
        {
            return _repo.DeleteCategoryAsync(id);
        }

        public Task<bool> UpdateCategoryAsync(CategoryDTO category)
        {
            return _repo.UpdateCategoryAsync(category);
        }



    }
}
