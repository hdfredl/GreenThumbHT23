using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using GreenThumbHT23.Manager;
using GreenThumbHT23.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenThumbHT23.Database;

public class GreenThumbDbContext : DbContext
{
    private readonly IEncryptionProvider _provider;
    public GreenThumbDbContext()
    {
        _provider = new GenerateEncryptionProvider(KeyManager.GetEncryptionKey());
    }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<GardenModel> Garden { get; set; }
    public DbSet<PlantModel> Plants { get; set; }
    public DbSet<InstructionModel> Instructions { get; set; }
    public DbSet<GardenConnection> GardenConnections { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GreenThumbDB;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) // 
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseEncryption(_provider);

        modelBuilder.Entity<UserModel>()
           .HasOne(user => user.Garden)
           .WithOne(garden => garden.Users)
           .HasForeignKey<GardenModel>(g => g.GardenId);

        // KOnfiga m-t-m mellan Garden och Plant med en jointable
        modelBuilder.Entity<GardenModel>()
              .HasMany(g => g.Plants) // Med flera plants 
              .WithMany(p => p.Gardens) // Med flera Gardens
              .UsingEntity<GardenConnection>( // En kjoin table 
                  gardenconnection => gardenconnection.HasOne(garden => garden.Plant).WithMany().HasForeignKey(gardenconnection => gardenconnection.PlantId),
                  gardenconnection => gardenconnection.HasOne(garden => garden.Garden).WithMany().HasForeignKey(gardenconnection => gardenconnection.GardenId),
                  gardenconnection =>
                  {
                      gardenconnection.HasKey(gardenconnection => new { gardenconnection.GardenId, gardenconnection.PlantId });
                      gardenconnection.Property(gardenconnection => gardenconnection.Quantity).IsRequired(); // Måste ha antal Quantity, användes i Tomteverkstad 
                  });

        // Konfiga one-to-many mellan plantmodel o instruction
        modelBuilder.Entity<PlantModel>()
            .HasMany(plant => plant.Instructions)
            .WithOne(ins => ins.Plant)
            .HasForeignKey(ins => ins.PlantId)
            .OnDelete(DeleteBehavior.Cascade); // Om vi slänger en Planta så försvinner insturktioner till denna planta

        modelBuilder.Entity<InstructionModel>()
      .HasOne(ins => ins.Plant) // har en planta
      .WithMany(plants => plants.Instructions) // har många instruktioner 
      .HasForeignKey(inst => inst.PlantId);




        // SEEDA DATA - EXEMPEL. Jag har seedat 1-3 via SQL, 3-7 med AddPlantWindow, därav bara seedat 1 här som exempel. 
        modelBuilder.Entity<PlantModel>().HasData(new PlantModel()
        {
            PlantId = 8,
            PlantName = "Test Rose",
            PlantDescription = "Test flower",

        });


        modelBuilder.Entity<InstructionModel>().HasData(new InstructionModel()
        {
            InstructionId = 8,
            InstructionName = "pour water to plant",
            InstructionDescription = " Pour water every 2nd minute",
            PlantId = 8,
        });




    }

}
