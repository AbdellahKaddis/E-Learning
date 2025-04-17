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
        public List<CategoryDTO> GetAllCategories()
        {
            return _context.Categories
                .Select(ca => new CategoryDTO
                {
                    Id = ca.Id,
                    CategoryName =  ca.CategoryName,
                    ImageCategory = ca.ImageCategory,

                }).ToList();
        }
        public CategoryDTO GetCategoryById(int id)
        {
            return _context.Categories
                .Where(ca => ca.Id == id)
                .Select(ca => new CategoryDTO
                {
                    Id = ca.Id,
                    CategoryName = ca.CategoryName,
                    ImageCategory = ca.ImageCategory,
                }).FirstOrDefault();
        }
        public Category AddCategory(CategoryDTO cat)
        {
            var category = new Category
            {
                CategoryName = cat.CategoryName,
                ImageCategory = cat.ImageCategory
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category;
        }

        public bool UpdateCategory(CategoryDTO cat)
        {
            var category = _context.Categories.Find(cat.Id);
            if (category == null) return false;

            category.CategoryName = cat.CategoryName;
            category.ImageCategory = cat.ImageCategory;
            _context.SaveChanges();
            return true;
        }
        public bool DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }


    }
}
