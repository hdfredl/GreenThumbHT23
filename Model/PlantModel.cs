using System.ComponentModel.DataAnnotations;

namespace GreenThumbHT23.Model;

public class PlantModel
{
    [Key]
    public int PlantId { get; set; }

    public string PlantName { get; set; } = null!;
    public string PlantDescription { get; set; } = null!;

    public List<GardenModel> Gardens { get; set; } = new(); // mant to many relation med Gardens. 

    public List<InstructionModel> Instructions { get; set; } = new(); // Har flera instructions till en plant. 

}
