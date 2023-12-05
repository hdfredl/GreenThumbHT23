using GreenThumbHT23.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenThumbHT23.Database;

public class GardenRepository
{
    private readonly GreenThumbDbContext _context;
    public GardenRepository(GreenThumbDbContext context)
    {
        _context = context;
    }

    public async Task<List<GardenModel>> GetAllAsync()
    {
        return await _context.Garden.ToListAsync();
    }

    public async Task<GardenModel?> GetByIdAsync(int id)
    {
        return await _context.Garden.FirstOrDefaultAsync(s => s.GardenId == id);
    }

    public async Task AddGardenAsync(GardenModel planets)
    {
        await _context.Garden.AddAsync(planets);
    }

    public async Task UpdateGardenAsync(int id, GardenModel gifts)
    {
        GardenModel? gardenName = await GetByIdAsync(id);

        if (gardenName != null)
        {
            gardenName.GardenName = gifts.GardenName;

        }
    }

    public async Task DeleteGardenAsync(int id)
    {
        GardenModel? gardenToDelete = await GetByIdAsync(id);

        if (gardenToDelete != null)
        {
            _context.Garden.Remove(gardenToDelete);
        }
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}
