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
        public bool AddCourse(CreateCourseDTO course)
        {
            return _repo.AddCourse(course);
        }

        public List<CourseDTO> GetAllCourses() => _repo.GetAllCourses();
        public CourseDTO GetCourseById(int id) => _repo.GetCourseById(id);
        public bool DeleteCourse(int id) => _repo.DeleteCourse(id);

        public bool UpdateCourse(int id, UpdateCourseDTO dto)
        {
            return _repo.UpdateCourse(id, dto);
        }


    }
}
