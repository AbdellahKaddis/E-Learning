using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repositories
{
    public class CategoryRepositories
    {
        private readonly AppDbContext _context;
        
        public CategoryRepositories(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(ca => new CategoryDTO
                {
                    Id = ca.Id,
                    CategoryName = ca.CategoryName,
                    ImageCategory = ca.ImageCategory,
                })
                .ToListAsync();
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Where(ca => ca.Id == id)
                .Select(ca => new CategoryDTO
                {
                    Id = ca.Id,
                    CategoryName = ca.CategoryName,
                    ImageCategory = ca.ImageCategory,
                }).FirstOrDefaultAsync();
        }

        public async Task<Category> AddCategoryAsync(CategoryDTO cat)
        {
            var category = new Category
            {
                CategoryName = cat.CategoryName,
                ImageCategory = cat.ImageCategory
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryDTO cat)
        {
            var category = await _context.Categories.FindAsync(cat.Id);
            if (category == null) return false;

            category.CategoryName = cat.CategoryName;
            category.ImageCategory = cat.ImageCategory;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
