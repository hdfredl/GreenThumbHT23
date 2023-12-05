namespace GreenThumbHT23.Model;

public class GardenConnection
{

    // Mellan Garden och plant 
    public int GardenId { get; set; }
    public GardenModel Garden { get; set; } = null!;

    public int PlantId { get; set; }
    public PlantModel Plant { get; set; } = null!;

    public int Quantity { get; set; } // Hade i tomteverkstad 


}
