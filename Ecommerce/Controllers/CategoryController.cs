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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();
            return categories.Any() ? Ok(categories) : NotFound("No categories Found.");
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
                return BadRequest("Category is invalid");

            var newCategory = await _service.AddCategoryAsync(category);
            return Ok(new { message = "Category created successfully" });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            var category = await _service.GetCategoryByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var success = await _service.DeleteCategoryAsync(id);
            return success ? Ok(new { message = "Category deleted successfully" }) : NotFound("No categories Found.");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO cat)
        {
            var existingCategory = await _service.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound("Category not found.");
            }

            existingCategory.CategoryName = cat.CategoryName;
            existingCategory.ImageCategory = cat.ImageCategory;

            var updated = await _service.UpdateCategoryAsync(existingCategory);
            if (!updated)
            {
                return StatusCode(500, "Failed to update category.");
            }

            return Ok(existingCategory);
        }



    }
}
