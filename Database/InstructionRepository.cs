using GreenThumbHT23.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenThumbHT23.Database;

public class InstructionRepository
{
    private readonly GreenThumbDbContext _context;
    public InstructionRepository(GreenThumbDbContext context)
    {
        _context = context;
    }

    public async Task<List<InstructionModel>> GetAllAsync()
    {
        return await _context.Instructions.ToListAsync();
    }

    public async Task<InstructionModel?> GetByIdAsync(int id)
    {
        return await _context.Instructions.FirstOrDefaultAsync(s => s.InstructionId == id);
    }

    public async Task AddGardenAsync(InstructionModel instruction)
    {
        await _context.Instructions.AddAsync(instruction);
    }

    public async Task UpdateGardenAsync(int id, InstructionModel gifts)
    {
        InstructionModel? gardenName = await GetByIdAsync(id);

        if (gardenName != null)
        {
            gardenName.InstructionName = gifts.InstructionName;

        }
    }

    public async Task DeleteGardenAsync(int id)
    {
        InstructionModel? gardenToDelete = await GetByIdAsync(id);

        if (gardenToDelete != null)
        {
            _context.Instructions.Remove(gardenToDelete);
        }
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}
