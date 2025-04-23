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
    public class CourseService
    {
        private readonly CourseRepository _repo;

        public CourseService(CourseRepository repo)
        {
            _repo = repo;
        }
        public Task<List<CourseDTO>> GetAllCoursesAsync() => _repo.GetAllCoursesAsync();
        public Task<CourseDTO> GetCourseByIdAsync(int id) => _repo.GetCourseByIdAsync(id);
        public Task<bool> AddCourseAsync(CreateCourseDTO course) => _repo.AddCourseAsync(course);
        public Task<bool> UpdateCourseAsync(int id, UpdateCourseDTO dto) => _repo.UpdateCourseAsync(id, dto);
        public Task<bool> DeleteCourseAsync(int id) => _repo.DeleteCourseAsync(id);



    }
}
