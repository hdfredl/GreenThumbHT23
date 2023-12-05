using System.ComponentModel.DataAnnotations;

namespace GreenThumbHT23.Model;

public class GardenModel
{
    [Key]
    public int GardenId { get; set; }

    public string GardenName { get; set; } = null!;

    public UserModel Users { get; set; } // Har en User 

    public List<PlantModel> Plants { get; set; } = new(); // har en / flera Plants 


}
