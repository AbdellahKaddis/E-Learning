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
    public class LocationRepository
    {
        private readonly AppDbContext _context;

        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LocationDTO>> GetAllLocationsAsync()
        {
            return await _context.Locations
                .Select(l => new LocationDTO { Id = l.Id, Name = l.Name })
                .ToListAsync();
        }

        public async Task<LocationDTO?> GetLocationByIdAsync(int id)
        {
            return await _context.Locations
                .Select(l => new LocationDTO { Id = l.Id, Name = l.Name })
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<int> AddLocationAsync(CreateLocationDTO newLocation)
        {
            var location = new Location { Name = newLocation.Name };
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return location.Id;
        }

        public async Task<bool> UpdateLocationAsync(UpdateLocationDTO updatedLocation)
        {
            var location = await _context.Locations.FindAsync(updatedLocation.Id);
            if (location == null) return false;

            if (!string.IsNullOrWhiteSpace(updatedLocation.Name))
                location.Name = updatedLocation.Name;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return false;

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

