using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class ScheduleService
    {
        private readonly ScheduleRepository _scheduleRepo;
        private readonly EmailService _emailService;

        public ScheduleService(ScheduleRepository scheduleRepo, EmailService emailService)
        {
            _scheduleRepo = scheduleRepo;
            _emailService = emailService;
        }

        public async Task<List<ScheduleDTO>> GetAllSchedulesAsync() => await _scheduleRepo.GetAllSchedulesAsync();

        public async Task<ScheduleDTO?> GetScheduleByIdAsync(int id) => await _scheduleRepo.GetScheduleByIdAsync(id);

        public async Task<List<StudentWithParentDTO>> GetStudentsWithTheirParentsByClassId(int id)
           => await _scheduleRepo.GetStudentsWithTheirParentsByClassId(id);
        public async Task AddSchedulesAsync(List<CreateScheduleDTO> newSchedules)
        {
            await _scheduleRepo.AddSchedulesAsync(newSchedules);

            //send schedule to parents
            int year = DateTime.Now.Year;
            int week = ISOWeek.GetWeekOfYear(DateTime.Today);
            //get this week schedule
      
            
            var classeIds = newSchedules
                .Select(s => s.ClasseId)
                .Distinct()
                .ToList();

            foreach (var classeId in classeIds)
            {
                var schedules = await this.GetSchedulesByYearAndWeekAsync(year, week,classeId);
                var parents = await this.GetStudentsWithTheirParentsByClassId(classeId);
                var uniqueParents = parents
                        .GroupBy(p => p.ParentEmail)
                        .Select(g => g.First())
                        .ToList();
                if (schedules.Any() && uniqueParents.Any())
                {
                    foreach (var parent in uniqueParents)
                    {
                        var pdf = SchedulePdfGenerator.Generate(schedules);
                        await _emailService.SendEmailAsync(parent.ParentEmail, parent.ParentFullName, pdf, schedules[0].Classe.Name,schedules.Count);
                    }
                }
            }
            
        }

        public async Task<ScheduleDTO?> UpdateScheduleAsync(int id, UpdateScheduleDTO dto)
        {
            var updated = await _scheduleRepo.UpdateScheduleAsync(id, dto);
            if (!updated) return null;
            return await _scheduleRepo.GetScheduleByIdAsync(id);
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            return await _scheduleRepo.DeleteScheduleAsync(id);
        }
        public async Task<List<ScheduleDTO>> GetStudentScheduleForThisWeekAsync(int studentId)
    => await _scheduleRepo.GetStudentScheduleForThisWeekAsync(studentId);

        public async Task<List<Schedule>> GetSchedulesByYearAndWeekAsync(int year, int week,int classeId)
        {
            return await _scheduleRepo.GetSchedulesByYearAndWeekAsync(year, week,classeId);
        }
    }
}
