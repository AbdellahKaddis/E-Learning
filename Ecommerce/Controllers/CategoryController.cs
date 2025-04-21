using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult AddCategory([FromBody] CategoryDTO category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
                return BadRequest("Category is invalid");

            var newCategory = _service.AddCategory(category);
            return Ok(new { message = "Category created successfully" });
        }
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categories = _service.GetAllCategories();
            return categories.Any() ? Ok(categories) : NotFound("No categories Found.");
        }
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategoryById(int id)
        {
            var category = _service.GetCategoryById(id);
            return category == null ? NotFound() : Ok(category);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCategory(int id)
        {
            var success = _service.DeleteCategory(id);
            return success ? Ok(new { message = "Category deleted successfully" }) : NotFound("No categories Found.");
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateCategory(int id, CategoryDTO cat)
        {
            var existingCategory = _service.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound("Category not found.");
            }

            existingCategory.CategoryName = cat.CategoryName;
            existingCategory.ImageCategory = cat.ImageCategory;

            var updated = _service.UpdateCategory(existingCategory);
            if (!updated)
            {
                return StatusCode(500, "Failed to update category.");
            }

            return Ok(existingCategory);
        }


    }
}
