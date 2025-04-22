using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class ScheduleService
    {
        private readonly ScheduleRepository _scheduleRepo;


        public ScheduleService(ScheduleRepository scheduleRepo)
        {
            _scheduleRepo = scheduleRepo;
        }

        public async Task<List<ScheduleDTO>> GetAllSchedulesAsync() => await _scheduleRepo.GetAllSchedulesAsync();

        public async Task<ScheduleDTO?> GetScheduleByIdAsync(int id) => await _scheduleRepo.GetScheduleByIdAsync(id);

        public async Task<ScheduleDTO> AddScheduleAsync(CreateScheduleDTO dto)
        {
            int insertedId = await _scheduleRepo.AddScheduleAsync(dto);
            var schedule = await _scheduleRepo.GetScheduleByIdAsync(insertedId);
            return schedule!;
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
    }
}
