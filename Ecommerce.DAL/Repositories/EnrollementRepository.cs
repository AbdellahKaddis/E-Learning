using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class EnrollementRepository
{
    private readonly AppDbContext _context;

    public EnrollementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<EnrollementDTO>> GetAllEnrollementsAsync()
    {
        return await _context.Enrollements
            .Include(e => e.User)
            .Include(e => e.Category)
            .Include(e => e.Classe)
            .Select(e => new EnrollementDTO
            {
                Id = e.Id,
                UserId = e.UserId,
                UserFullName = e.User.FirstName + " " + e.User.LastName,
                CategoryId = e.CategoryId,
                CategoryName = e.Category.CategoryName,
                ClasseId = e.ClasseId,
                ClasseName = e.Classe.Name,
                EnrollementDate = e.EnrollementDate
            })
            .ToListAsync();
    }

    public async Task<EnrollementDTO?> GetEnrollementByIdAsync(int id)
    {
        return await _context.Enrollements
            .Include(e => e.User)
            .Include(e => e.Category)
            .Include(e => e.Classe)
            .Where(e => e.Id == id)
            .Select(e => new EnrollementDTO
            {
                Id = e.Id,
                UserId = e.UserId,
                UserFullName = e.User.FirstName + " " + e.User.LastName,
                CategoryId = e.CategoryId,
                CategoryName = e.Category.CategoryName,
                ClasseId = e.ClasseId,
                ClasseName = e.Classe.Name,
                EnrollementDate = e.EnrollementDate
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> AddEnrollementAsync(CreateEnrollementDTO dto)
    {
        var enrollement = new Enrollement
        {
            UserId = dto.UserId,
            CategoryId = dto.CategoryId,
            ClasseId = dto.ClasseId,
            //EnrollementDate = DateTime.UtcNow
        };

        _context.Enrollements.Add(enrollement);
        await _context.SaveChangesAsync();
        return enrollement.Id;
    }

    public async Task<bool> UpdateEnrollementAsync(int id, UpdateEnrollementDTO dto)
    {
        var enrollement = await _context.Enrollements.FindAsync(id);
        if (enrollement == null) return false;

        if (dto.UserId.HasValue)
            enrollement.UserId = dto.UserId.Value;

        if (dto.CategoryId.HasValue)
            enrollement.CategoryId = dto.CategoryId.Value;

        if (dto.ClasseId.HasValue)
            enrollement.ClasseId = dto.ClasseId.Value;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEnrollementAsync(int id)
    {
        var enrollement = await _context.Enrollements.FindAsync(id);
        if (enrollement == null) return false;

        _context.Enrollements.Remove(enrollement);
        await _context.SaveChangesAsync();
        return true;
    }
}
