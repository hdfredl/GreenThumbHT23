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

    public void Complete()
    {
        _context.SaveChanges();
    }


}
