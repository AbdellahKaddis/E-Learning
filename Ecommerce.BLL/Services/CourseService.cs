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
        private readonly EmailService _emailService;
        private readonly UserService _userService;

        public CourseService(CourseRepository repo, EmailService emailService, UserService userService)
        {
            _repo = repo;
            _emailService = emailService;
            _userService = userService;
        }

        public Task<List<CourseDTO>> GetAllCoursesAsync() => _repo.GetAllCoursesAsync();
        public Task<CourseDTO> GetCourseByIdAsync(int id) => _repo.GetCourseByIdAsync(id);

        public async Task<bool> AddCourseAsync(CreateCourseDTO course)
        {
            bool result = await _repo.AddCourseAsync(course);

            if (result)
            {
                var users = await _userService.GetAllUsersAsync();

                foreach (var user in users)
                {
                    if (user.RoleName == "parent")
                    {

                    await _emailService.SendCourseNotificationAsync(
                        user.Email,
                        user.FirstName,
                        course.CourseName,
                        course.CourseDescription);
                    }
                }
            }

            return result;
        }

        public Task<bool> UpdateCourseAsync(int id, UpdateCourseDTO dto) => _repo.UpdateCourseAsync(id, dto);
        public Task<bool> DeleteCourseAsync(int id) => _repo.DeleteCourseAsync(id);
    }
}