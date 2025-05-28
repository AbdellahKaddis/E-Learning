using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ecommerce.DAL.Repositories
{
    public class CourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CourseDTO>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.User)
                .Include(c => c.Level)
                .Select(course => new CourseDTO
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    Duration = course.Duration,
                    Level = course.Level != null ? course.Level.Name : "Non défini",
                    LevelId = course.Level != null ? course.Level.Id : 0,
                    ImageCourse = course.ImageCourse,
                    Category = course.Category != null ? course.Category.CategoryName : "Non défini",
                    CategoryId = course.Category != null ? course.Category.Id : 0,
                    Formateur = course.User != null ? course.User.LastName : "Non défini",
                    FourmateurId = course.User != null ? course.User.Id : 0,
                    Created = course.Created,
                    Updated = course.Updated
                }).ToListAsync();
        }


        public async Task<CourseDTO> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.User)
                .Include(c => c.Level)
                .Where(course => course.Id == id)
                .Select(course => new CourseDTO
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    Duration = course.Duration,
                    Level = course.Level.Name,
                    LevelId = course.Level.Id,
                    ImageCourse = course.ImageCourse,
                    Category = course.Category.CategoryName,
                    CategoryId = course.Category.Id,
                    Formateur = course.User.LastName,
                    FourmateurId = course.User.Id,
                    Created = course.Created,
                    Updated = course.Updated
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> AddCourseAsync(CreateCourseDTO course)
        {
            var cours = new Course
            {
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                Duration = course.Duration,
                LevelId = course.LevelId,
                ImageCourse = course.ImageCourse,
                CategoryId = course.CategoryId,
                UserId = course.UserId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
            };

            await _context.Courses.AddAsync(cours);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> UpdateCourseAsync(int id, UpdateCourseDTO course)
        {
            var cours = await _context.Courses.FindAsync(id);
            if (cours == null) return false;

            cours.CourseName = course.CourseName;
            cours.CourseDescription = course.CourseDescription;
            cours.Duration = course.Duration;
            cours.LevelId = course.LevelId;
            cours.ImageCourse = course.ImageCourse;
            cours.CategoryId = course.CategoryId;
            cours.UserId = course.UserId;
            cours.Updated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}