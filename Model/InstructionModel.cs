using System.ComponentModel.DataAnnotations;

namespace GreenThumbHT23.Model;

public class InstructionModel
{
    [Key]
    public int InstructionId { get; set; }

    public string InstructionName { get; set; } = null!;

    public string InstructionDescription { get; set; } = null!;

    public int PlantId { get; set; }

    public PlantModel Plant { get; set; } // Flera instructions kan ha en plant 


    public override string ToString()
    {
        return $"{InstructionName} - {InstructionDescription}";
    }
}
