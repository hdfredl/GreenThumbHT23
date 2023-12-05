using GreenThumbHT23.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenThumbHT23.Database;

public class PlantRepository
{
    private readonly GreenThumbDbContext _context;
    public PlantRepository(GreenThumbDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlantModel>> GetAllAsync()
    {
        return await _context.Plants.ToListAsync();
    }

    public async Task<PlantModel?> GetByIdAsync(int id)
    {
        return await _context.Plants.FirstOrDefaultAsync(s => s.PlantId == id);
    }

    public async Task AddPlantAsync(PlantModel planets)
    {
        await _context.Plants.AddAsync(planets);
    }

    public async Task UpdateOrdersAsync(int id, PlantModel orders)
    {
        PlantModel? ordersToUpdate = await GetByIdAsync(id);

        if (ordersToUpdate != null)
        {
            ordersToUpdate.PlantName = orders.PlantName;

        }
    }

    public async Task DeleteOrdersAsync(PlantModel plantToDelete)
    {
        if (plantToDelete != null)
        {
            _context.Plants.Remove(plantToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> PlantexistsAsync(string plantName)
    {
        return await _context.Plants.AnyAsync(plant => plant.PlantName == plantName);
    }

    public async Task<List<PlantModel>> SearchPlantsAsync(string searchName, string searchDesc)
    {
        var searchResults = await _context.Plants.Where(plant => plant.PlantName.ToLower().Contains(searchName) && plant.PlantDescription.ToLower().Contains(searchDesc)).ToListAsync();

        return searchResults;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}
