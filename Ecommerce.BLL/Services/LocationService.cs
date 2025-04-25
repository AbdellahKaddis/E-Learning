using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class LocationService
    {
        private readonly LocationRepository _repo;

        public LocationService(LocationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<LocationDTO>> GetAllLocationsAsync() => await _repo.GetAllLocationsAsync();

        public async Task<LocationDTO?> GetLocationByIdAsync(int id) => await _repo.GetLocationByIdAsync(id);

        public async Task<LocationDTO> AddLocationAsync(CreateLocationDTO newLocation)
        {
            int insertedId = await _repo.AddLocationAsync(newLocation);
            return new LocationDTO { Id = insertedId, Name = newLocation.Name };
        }

        public async Task<LocationDTO?> UpdateLocationAsync(UpdateLocationDTO dto)
        {
            var updated = await _repo.UpdateLocationAsync(dto);
            if (!updated) return null;

            var updatedLocation = await _repo.GetLocationByIdAsync(dto.Id);
            return updatedLocation is not null
                ? new LocationDTO { Id = updatedLocation.Id, Name = updatedLocation.Name }
                : null;
        }

        public async Task<bool> DeleteLocationAsync(int id) => await _repo.DeleteLocationAsync(id);
    }

}

