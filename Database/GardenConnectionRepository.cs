using GreenThumbHT23.Model;

namespace GreenThumbHT23.Database;

public class GardenConnectionRepository
{
    private readonly GreenThumbDbContext _context;
    public GardenConnectionRepository(GreenThumbDbContext context)
    {
        _context = context;
    }

    public async Task AddGardenConAsync(GardenConnection connection)
    {
        await _context.GardenConnections.AddAsync(connection);
    }

}
