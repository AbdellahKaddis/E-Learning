using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ecommerce.DAL.Repositories
{
    public class LessonProgressRepository
    {
        private readonly AppDbContext _context;

        public LessonProgressRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<LessonProgressDTO>> GetAllLessonProgressAsync()
        {
            return await _context.lessonProgresses
                .Include(s => s.Student)
                .Include(l => l.Lesson)
                .Include(c => c.Course)
                .Select(lesPro => new LessonProgressDTO
                {
                    Id = lesPro.Id,
                    StudentId = lesPro.StudentId,
                    StudentName = lesPro.Student.User.FirstName,
                    LessonId = lesPro.LessonId,
                    LessonName = lesPro.Lesson.titre,
                    CourseId = lesPro.CourseId,
                    CourseName = lesPro.Course.CourseName,
                    IsCompleted = lesPro.IsCompleted,
                    LastSecond = lesPro.LastSecond,
                    UpdatedAt = lesPro.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<List<LessonProgressDTO>> GetLessonProgressByStudentIdAsync(int studentId)
        {
            return await _context.lessonProgresses
                .Include(s => s.Student)
                .Include(l => l.Lesson)
                .Where(lesPro => lesPro.StudentId == studentId)
                .Select(lesPro => new LessonProgressDTO
                {
                    Id = lesPro.Id,
                    StudentId = lesPro.StudentId,
                    StudentName = lesPro.Student.User.FirstName,
                    LessonId = lesPro.LessonId,
                    LessonName = lesPro.Lesson.titre,
                    CourseId = lesPro.CourseId,
                    CourseName = lesPro.Course.CourseName,
                    IsCompleted = lesPro.IsCompleted,
                    LastSecond = lesPro.LastSecond,
                    UpdatedAt = lesPro.UpdatedAt
                })
                .ToListAsync();
        }
        public async Task<List<LessonProgressDTO>> GetLessonProgressByStudentIdAndCourseIdAsync(int studentId,int courseId)
        {
            return await _context.lessonProgresses
                .Include(s => s.Student)
                .Include(l => l.Lesson)
                .Where(lesPro => lesPro.StudentId == studentId && lesPro.CourseId == courseId)
                .Select(lesPro => new LessonProgressDTO
                {
                    Id = lesPro.Id,
                    StudentId = lesPro.StudentId,
                    StudentName = lesPro.Student.User.FirstName,
                    LessonId = lesPro.LessonId,
                    LessonName = lesPro.Lesson.titre,
                    CourseId = lesPro.CourseId,
                    CourseName = lesPro.Course.CourseName,
                    IsCompleted = lesPro.IsCompleted,
                    LastSecond = lesPro.LastSecond,
                    UpdatedAt = lesPro.UpdatedAt
                })
                .ToListAsync();
        }


        public async Task<bool> AddOrUpdateLessonProgressAsync(CreateLessonProgressDTO les)
        {
            var existingProgress = await _context.lessonProgresses
                .FirstOrDefaultAsync(p => p.StudentId == les.StudentId && p.LessonId == les.LessonId);

            if (existingProgress != null)
            {
                existingProgress.LastSecond = les.LastSecond;
                existingProgress.UpdatedAt = DateTime.UtcNow;

                var lesson = await _context.Lesson.FirstOrDefaultAsync(l => l.LessonId == les.LessonId);
                if (lesson != null && existingProgress.LastSecond >= lesson.Duration.TotalSeconds)
                {
                    existingProgress.IsCompleted = true;
                }

                await _context.SaveChangesAsync();
                return false;
            }
            else
            {
                var newLessonProgress = new LessonProgress
                {
                    StudentId = les.StudentId,
                    LessonId = les.LessonId,
                    CourseId = les.CourseId,
                    LastSecond = les.LastSecond,
                    UpdatedAt = DateTime.UtcNow,
                    IsCompleted = false
                };

                var lesson = await _context.Lesson.FirstOrDefaultAsync(l => l.LessonId == les.LessonId);
                if (lesson != null && newLessonProgress.LastSecond >= lesson.Duration.TotalSeconds)
                {
                    newLessonProgress.IsCompleted = true;
                }

                await _context.lessonProgresses.AddAsync(newLessonProgress);
                await _context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> UpdateLessonProgressAsync(int studentId, int lessonId, UpdateLessonProgressDTO les)
        {
            var existingProgress = await _context.lessonProgresses
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.LessonId == lessonId);

            if (existingProgress == null) return false;

            existingProgress.LastSecond = les.LastSecond;
            existingProgress.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> DeleteLessonProgressAsync(int id)
        {
            var lesPro = await _context.lessonProgresses.FindAsync(id);
            if (lesPro == null) return false;
            _context.lessonProgresses.Remove(lesPro);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CourseProgress> GetCourseProgressAsync(int studentId, int courseId)
        {
            var totalLessons = await _context.Lesson
                .Where(l => l.CourseId == courseId)
                .CountAsync();

            var completedLessons = await _context.lessonProgresses
                .Where(lp => lp.StudentId == studentId && lp.CourseId == courseId && lp.IsCompleted)
                .CountAsync();

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            double percentageCompleted = totalLessons > 0 ? (completedLessons * 100.0) / totalLessons : 0;

            return new CourseProgress
            {
                CourseId = courseId,
                CourseName = course?.CourseName ?? "",
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                PercentageCompleted = Math.Round(percentageCompleted, 2),
                IsCourseCompleted = totalLessons > 0 && completedLessons == totalLessons
            };
        }
        public async Task<List<CourseProgress>> GetCoursesProgressByStudentLevelAsync(int studentId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null || student.LevelId == null)
            {
                return new List<CourseProgress>();
            }

            int studentLevelId = student.LevelId.Value;

            var courses = await _context.Courses
                .Where(c => c.LevelId == studentLevelId)
                .ToListAsync();

            var result = new List<CourseProgress>();

            foreach (var course in courses)
            {
                var totalLessons = await _context.Lesson
                    .Where(l => l.CourseId == course.Id)
                    .CountAsync();

                var completedLessons = await _context.lessonProgresses
                    .Where(lp => lp.StudentId == studentId && lp.CourseId == course.Id && lp.IsCompleted)
                    .CountAsync();

                double percentageCompleted = totalLessons > 0 ? (completedLessons * 100.0) / totalLessons : 0;

                result.Add(new CourseProgress
                {
                    CourseId = course.Id,
                    CourseName = course.CourseName,
                    TotalLessons = totalLessons,
                    CompletedLessons = completedLessons,
                    PercentageCompleted = Math.Round(percentageCompleted, 2),
                    IsCourseCompleted = totalLessons > 0 && completedLessons == totalLessons
                });
            }

            return result;
        }
        public async Task<List<CourseProgress>> GetCoursesProgressByStudentAndLevelAsync(int studentId, int levelId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
            {
                return new List<CourseProgress>();
            }

            // Get all courses of the level (no change here)
            var courses = await _context.Courses
                .Where(c => c.LevelId == levelId)
                .ToListAsync();

            var courseIds = courses.Select(c => c.Id).ToList();

            // Get lesson counts for each course (1 query)
            var lessonsCounts = await _context.Lesson
                .Where(l => courseIds.Contains(l.CourseId))
                .GroupBy(l => l.CourseId)
                .Select(g => new { CourseId = g.Key, TotalLessons = g.Count() })
                .ToListAsync();

            // Get completed lessons for each course by student (1 query)
            var completedCounts = await _context.lessonProgresses
                .Where(lp => lp.StudentId == studentId && courseIds.Contains(lp.CourseId) && lp.IsCompleted)
                .GroupBy(lp => lp.CourseId)
                .Select(g => new { CourseId = g.Key, CompletedLessons = g.Count() })
                .ToListAsync();

            var result = new List<CourseProgress>();

            foreach (var course in courses)
            {
                var totalLessons = lessonsCounts.FirstOrDefault(l => l.CourseId == course.Id)?.TotalLessons ?? 0;
                var completedLessons = completedCounts.FirstOrDefault(c => c.CourseId == course.Id)?.CompletedLessons ?? 0;

                double percentageCompleted = totalLessons > 0 ? (completedLessons * 100.0) / totalLessons : 0;

                result.Add(new CourseProgress
                {
                    CourseId = course.Id,
                    CourseName = course.CourseName,
                    TotalLessons = totalLessons,
                    CompletedLessons = completedLessons,
                    PercentageCompleted = Math.Round(percentageCompleted, 2),
                    IsCourseCompleted = totalLessons > 0 && completedLessons == totalLessons
                });
            }

            return result;
        }
    }
}
