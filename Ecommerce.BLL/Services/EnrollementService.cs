using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class EnrollementService
    {
        private readonly EnrollementRepository _repo;

        public EnrollementService(EnrollementRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<EnrollementDTO>> GetAllEnrollementsAsync()
            => await _repo.GetAllEnrollementsAsync();

        public async Task<EnrollementDTO?> GetEnrollementByIdAsync(int id)
            => await _repo.GetEnrollementByIdAsync(id);

        public async Task<EnrollementDTO?> AddEnrollementAsync(CreateEnrollementDTO dto)
        {
            int insertedId = await _repo.AddEnrollementAsync(dto);
            return await _repo.GetEnrollementByIdAsync(insertedId);
        }

        public async Task<EnrollementDTO?> UpdateEnrollementAsync(int id, UpdateEnrollementDTO dto)
        {
            bool updated = await _repo.UpdateEnrollementAsync(id, dto);
            if (!updated) return null;

            return await _repo.GetEnrollementByIdAsync(id);
        }

        public async Task<bool> DeleteEnrollementAsync(int id)
            => await _repo.DeleteEnrollementAsync(id);
    }
}
