﻿using Ecommerce.DAL.Db;
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
        public List<CourseDTO> GetAllCourses()
        {
            return _context.Courses
                .Include(c => c.Category)
                .Include(c => c.User)
                .Select(course => new CourseDTO
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    Duration = course.Duration,
                    Level = course.Level,
                    ImageCourse = course.ImageCourse,
                    Category = course.Category.CategoryName,
                    Formateur = course.User.FirstName,
                    Created = course.Created,
                    Updated = course.Updated
                })
                .ToList();
        }

        public CourseDTO GetCourseById(int id)
        {
            return _context.Courses
                .Include(c => c.Category)
                .Include(c => c.User)
                .Where(course => course.Id == id)
                .Select(course => new CourseDTO
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    Duration = course.Duration,
                    Level = course.Level,
                    ImageCourse = course.ImageCourse,
                    Category = course.Category.CategoryName,
                    Formateur = course.User.FirstName,
                    Created = course.Created,
                    Updated = course.Updated
                }).FirstOrDefault();
        }
        public bool AddCourse(CreateCourseDTO course)
        {
            var cours = new Course
            {
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                Duration = course.Duration,
                Level = course.Level,
                ImageCourse = course.ImageCourse,
                CategoryId = course.CategoryId,
                UserId = course.UserId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                
            };

            _context.Courses.Add(cours);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateCourse(int id, UpdateCourseDTO course)
        {
            var cours = _context.Courses.Find(id);
            if (cours == null) return false;

            cours.CourseName = course.CourseName;
            cours.CourseDescription = course.CourseDescription;
            cours.Duration = course.Duration;
            cours.Level = course.Level;
            cours.ImageCourse = course.ImageCourse;
            cours.CategoryId = course.CategoryId;
            cours.UserId = course.UserId;
            cours.Updated = DateTime.UtcNow;
            _context.SaveChanges();
            return true;
        }

    }
}
