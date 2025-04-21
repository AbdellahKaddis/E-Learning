using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DAL.Repositories
{
    public class LessonRepository
    {
        private readonly AppDbContext _context;

        public LessonRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<LessonDto> GetAllLessons()
        {
     
            return _context.Lesson
                .Include(c => c.Course)
                .Select(lesson => new LessonDto
                {
                    LessonId = lesson.LessonId,
                    titre = lesson.titre,
                    URL = lesson.URL,
                    Duration = lesson.Duration,
                    CourseName = lesson.Course.CourseName,
                  
                })
                .ToList();
        }
        
        public Lesson GetLessonId(int id)
        {
            return _context.Lesson.FirstOrDefault(u => u.LessonId == id);
        }

        public Lesson AddLesson(createLessonDto dto)
        {
            var lesson = new Lesson
            {
                URL = dto.URL,
                Duration = dto.Duration,
                titre = dto.titre,
                CourseId = dto.CourseId
            };

            _context.Lesson.Add(lesson);
            _context.SaveChanges();
            return lesson;
        }

        public Lesson UpdateLesson(int id, updateLessonDto l)
        {
            var lesson = _context.Lesson.FirstOrDefault(les => les.LessonId == id);
            if (lesson == null)
            {
                return null;
            }

            lesson.titre = l.titre;
            lesson.URL = l.URL;
            lesson.Duration = l.Duration;
            lesson.CourseId = l.CourseId;

            _context.SaveChanges();
            return lesson;
        }
        public bool DeleteLesson(int id)
        {
            var lesson = _context.Lesson.Find(id);
            if (lesson != null)
            {
                _context.Lesson.Remove(lesson);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
