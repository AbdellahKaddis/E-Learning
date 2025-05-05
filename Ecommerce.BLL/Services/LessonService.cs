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


        public LessonService(LessonRepository repo)
        {
            _repo = repo;
        }

        public List<LessonDto> GetAllLessons() => _repo.GetAllLessons();
        public Lesson GetLessonId(int id) => _repo.GetLessonId(id);
        public Lesson AddLesson(createLessonDto dto) => _repo.AddLesson(dto);

        public bool DeleteLesson(int id) => _repo.DeleteLesson(id);
        public Lesson UpdateLesson(int id, updateLessonDto l) => _repo.UpdateLesson(id, l);
        public async Task<List<LessonDto>> getCourseLessonsByCourseId(int courseId)
            => await _repo.getCourseLessonsByCourseId(courseId);
    }
}
