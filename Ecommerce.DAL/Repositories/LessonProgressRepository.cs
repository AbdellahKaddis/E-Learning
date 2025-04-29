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
                .Select(lesPro => new LessonProgressDTO
                {
                    Id = lesPro.Id,
                    StudentId = lesPro.StudentId,
                    LessonId = lesPro.LessonId,
                    StudentName = lesPro.Student.User.FirstName,
                    LastSecond = lesPro.LastSecond,
                    LessonName = lesPro.Lesson.titre,
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
                    LessonId = lesPro.LessonId,
                    StudentName = lesPro.Student.User.FirstName,
                    LastSecond = lesPro.LastSecond,
                    LessonName = lesPro.Lesson.titre,
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

                await _context.SaveChangesAsync(); 
                return false; 
            }
            else
            {
                var newLessonProgress = new LessonProgress
                {
                    StudentId = les.StudentId,
                    LessonId = les.LessonId,
                    LastSecond = les.LastSecond,
                    UpdatedAt = DateTime.UtcNow
                };

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
    }
}
