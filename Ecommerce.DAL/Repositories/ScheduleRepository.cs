using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repositories
{
    public class ScheduleRepository
    {
        private readonly AppDbContext _context;

        public ScheduleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ScheduleDTO>> GetAllSchedulesAsync()
        {
            
            return await _context.Schedule
                .Include(s => s.Classe)
                .Include(s => s.Location)
                .Include(s => s.Course)
                    .ThenInclude(c => c.User)
                .Select(s => new ScheduleDTO
                {
                    Id = s.Id,
                    Year = s.Year,
                    Week = s.Week,
                    Day = s.Day,
                    ClasseName = s.Classe.Name,
                    LocationName = s.Location.Name,
                    CourseName = s.Course.CourseName,
                    FormateurName = s.Course.User.FirstName + " " + s.Course.User.LastName,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                }).ToListAsync();
        }

        public async Task<ScheduleDTO?> GetScheduleByIdAsync(int id)
        {
            return await _context.Schedule
                .Include(s => s.Classe)
                .Include(s => s.Location)
                .Include(s => s.Course)
                    .ThenInclude(c => c.User)
                .Where(s => s.Id == id)
                .Select(s => new ScheduleDTO
                {
                    Id = s.Id,
                    Year = s.Year,
                    Week = s.Week,
                    Day = s.Day,
                    ClasseName = s.Classe.Name,
                    LocationName = s.Location.Name,
                    CourseName = s.Course.CourseName,
                    FormateurName = s.Course.User.FirstName + " " + s.Course.User.LastName,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                }).FirstOrDefaultAsync();
        }

        public async Task AddSchedulesAsync(List<CreateScheduleDTO> schedules)
        {
            var scheduleEntities = schedules.Select(s => new Schedule
            {
                Year = s.Year,
                Week = s.Week,
                Day = s.Day,
                ClasseId = s.ClasseId,
                LocationId = s.LocationId,
                CourseId = s.CourseId,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            }).ToList(); 

            await _context.Schedule.AddRangeAsync(scheduleEntities);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> UpdateScheduleAsync(int id, UpdateScheduleDTO dto)
        {
            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null) return false;

            if (dto.Year.HasValue) schedule.Year = dto.Year.Value;
            if (dto.Week.HasValue) schedule.Week = dto.Week.Value;
            if (dto.Day.HasValue) schedule.Day = dto.Day.Value;
            if (dto.ClasseId.HasValue) schedule.ClasseId = dto.ClasseId.Value;
            if (dto.LocationId.HasValue) schedule.LocationId = dto.LocationId.Value;
            if (dto.CourseId.HasValue) schedule.CourseId = dto.CourseId.Value;
            if (dto.StartTime.HasValue) schedule.StartTime = dto.StartTime.Value;
            if (dto.EndTime.HasValue) schedule.EndTime = dto.EndTime.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null) return false;

            _context.Schedule.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ScheduleDTO>> GetStudentScheduleForThisWeekAsync(int studentId)
        {
            var today = DateTime.UtcNow;
            var calendar = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            int currentWeek = calendar.GetWeekOfYear(today, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int currentYear = today.Year;

            // Get student's class
            var student = await _context.Students
                .Include(s => s.Classe)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null) return new List<ScheduleDTO>();

            var classId = student.ClasseId;

            // Get schedule entries for that class, current year & week
            var schedules = await _context.Schedule
                .Include(s => s.Course).ThenInclude(c => c.User)
                .Include(s => s.Location)
                .Include(s => s.Classe)
                .Where(s => s.ClasseId == classId && s.Year == currentYear && s.Week == currentWeek)
                .ToListAsync();

            // Map to DTOs (assuming you have ScheduleDTO with properties)
            return schedules.Select(s => new ScheduleDTO
            {
                Id = s.Id,
                Year = s.Year,
                Week = s.Week,
                Day = s.Day,
                ClasseName = s.Classe.Name,
                LocationName = s.Location.Name,
                CourseName = s.Course.CourseName,
                FormateurName = $"{s.Course.User.FirstName} {s.Course.User.LastName}",
                StartTime = s.StartTime,
                EndTime = s.EndTime
            }).ToList();
        }
        public async Task<List<Schedule>> GetSchedulesByYearAndWeekAsync(int year, int week, int classeId)
        {
            return await _context.Schedule
                .Include(s => s.Classe)
                .Include(s => s.Location)
                .Include(s => s.Course)
                    .ThenInclude(c => c.User)
                .Where(s => s.Year == year && s.Week == week && s.ClasseId == classeId)
                .OrderBy(s => s.Day)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<StudentWithParentDTO>> GetStudentsWithTheirParentsByClassId(int id)
        {
            return await _context.Students
                    .Include(s => s.User)
                    .Include(s => s.Parent)
                    .ThenInclude(s => s.User)
                    .Where(s => s.ClasseId == id)
                    .Select(s => new StudentWithParentDTO
                    {
                        StudentFullName = $"{s.User.FirstName} {s.User.LastName}",
                        ParentFullName = $"{s.Parent.User.FirstName} {s.Parent.User.LastName}",
                        ParentEmail = s.Parent.User.Email
                    })
                    .ToListAsync();
        }

    }
}
