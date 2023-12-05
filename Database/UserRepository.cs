using GreenThumbHT23.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenThumbHT23.Database;

public class UserRepository
{
    private readonly GreenThumbDbContext _context;

    public UserRepository(GreenThumbDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserModel>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<UserModel?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(s => s.UserId == id);
    }

    public async Task AddCustomerAsync(UserModel planets)
    {
        await _context.Users.AddAsync(planets);
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<UserModel?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(c => c.Username == username);
    }
}
