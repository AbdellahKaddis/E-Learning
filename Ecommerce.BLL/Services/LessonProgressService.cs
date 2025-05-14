using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using System;

namespace Ecommerce.BLL.Services
{
    public class LessonProgressService
    {
        private readonly LessonProgressRepository _repo;

        public LessonProgressService(LessonProgressRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> AddOrUpdateCourseAsync(CreateLessonProgressDTO les)
        {
            return await _repo.AddOrUpdateLessonProgressAsync(les);
        }

        public async Task<List<LessonProgressDTO>> GetAllLessonProgressAsync()
        {
            return await _repo.GetAllLessonProgressAsync();
        }

        public async Task<List<LessonProgressDTO>> GetLessonProgressByStudentIdAsync(int studentId)
        {
            return await _repo.GetLessonProgressByStudentIdAsync(studentId);
        }

        public async Task<List<LessonProgressDTO>> GetLessonProgressByStudentIdAndCourseIdAsync(int studentId, int courseId)
            => await _repo.GetLessonProgressByStudentIdAndCourseIdAsync(studentId,courseId);

        public async Task<bool> UpdateLessonProgressAsync(int studentId, int lessonId, UpdateLessonProgressDTO les)
        {
            return await _repo.UpdateLessonProgressAsync(studentId, lessonId, les);
        }

        public async Task<bool> DeleteLessonProgressAsync(int id)
        {
            return await _repo.DeleteLessonProgressAsync(id);
        }
        public async Task<CourseProgress> GetCourseProgressAsync(int studentId, int courseId)
        {
            return await _repo.GetCourseProgressAsync(studentId, courseId);
        }
        public async Task<List<CourseProgress>> GetCoursesProgressByStudentLevelAsync(int studentId)
        {
            return await _repo.GetCoursesProgressByStudentLevelAsync(studentId);
        }
        public async Task<List<CourseProgress>> GetCoursesProgressByStudentAndLevelAsync(int studentId,int LevelId)
        {
            return await _repo.GetCoursesProgressByStudentAndLevelAsync(studentId,LevelId);
        }





    }
}
