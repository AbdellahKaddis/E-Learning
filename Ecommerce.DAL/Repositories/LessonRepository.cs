using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;

namespace Ecommerce.DAL.Repositories
{
    public class LessonRepository
    {
        private readonly AppDbContext _context;

        public LessonRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Lesson> GetAllLessons()
        {
            return _context.Lesson.ToList();
        }
        public Lesson GetLessonId(int id)
        {
            return _context.Lesson.FirstOrDefault(u => u.LessonId == id);
        }
        public Lesson AddLesson(Lesson l)
        {
            var lesson = new Lesson
            {
                URL = l.URL,
                Duration = l.Duration,
                titre = l.titre,
                course = l.course,
                courseId = l.courseId
            };
            _context.Lesson.Add(lesson);
            _context.SaveChanges();
            return lesson;
        }
        public Lesson UpdateLesson(int id, Lesson l)
        {
            var lesson = _context.Lesson.FirstOrDefault(les => les.LessonId == id);
            if (lesson == null)
            {
                return null;
            }

            lesson.titre = l.titre;
            lesson.URL = l.URL;
            lesson.Duration = l.Duration;
            lesson.course = l.course;
            lesson.courseId = l.courseId;

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
