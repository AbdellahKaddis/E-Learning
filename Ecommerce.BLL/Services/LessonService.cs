using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;

namespace Ecommerce.BLL.Services
{
    public class LessonService
    {

        private readonly LessonRepository _repo;
        private UserRepository repo;

        public LessonService(LessonRepository repo)
        {
            _repo = repo;
        }

        public LessonService(UserRepository repo)
        {
            this.repo = repo;
        }

        public List<Lesson> GetAllLessons() => _repo.GetAllLessons();
        public Lesson GetLessonId(int id) => _repo.GetLessonId(id);
    public Lesson AddLesson(Lesson l) => _repo.AddLesson(l);
        public bool DeleteLesson(int id) => _repo.DeleteLesson(id);
        public Lesson UpdateLesson(int id, Lesson l) => _repo.UpdateLesson(id, l);
    }
}
