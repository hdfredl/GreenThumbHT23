using GreenThumbHT23.Model;

namespace GreenThumbHT23.Database;

public class GreenUOW : IDisposable
{
    private readonly GreenThumbDbContext _context;

    public GardenRepository GardenRepository { get; }

    public InstructionRepository InstructionRepository { get; }

    public PlantRepository PlantRepository { get; }

    public UserRepository UserRepository { get; }

    public GardenConnectionRepository GardenConnectionRepository { get; }


    public void Dispose()
    {

    }

    public GreenUOW(GreenThumbDbContext context)
    {
        _context = context;
        GardenRepository = new(context);
        InstructionRepository = new(context);
        PlantRepository = new(context);
        UserRepository = new(context);
        GardenConnectionRepository = new(context);
    }

    public List<PlantModel> UserPlantsInDB(int userId)
    {
        return _context.GardenConnections.Where(gc => gc.Garden.Users.UserId == userId).Select(gc => gc.Plant).ToList();
        //   Kollar i GardenConnections. Kollar om det finns någon förbindelse mellan Garden och om den garden har en user.     Sen selectar vi alla Plants.      
    }


    public void Complete()
    {
        _context.SaveChanges();
    }


}
