using System.ComponentModel.DataAnnotations;
using EntityFrameworkCore.EncryptColumn.Attribute;

namespace GreenThumbHT23.Model;

public class UserModel
{
    [Key]
    public int UserId { get; set; }

    public string Username { get; set; } = null!;
    [EncryptColumn]
    public string Password { get; set; } = null!;

    public GardenModel Garden { get; set; } // har en Garden 
}
