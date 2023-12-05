using System.IO;
using System.Security.Cryptography;

namespace GreenThumbHT23.Manager;

public class KeyManager
{
    public static string GetEncryptionKey()
    {
        if (File.Exists("C:\\Users\\fhdlu\\Desktop\\Green.txt"))
        {
            return File.ReadAllText("C:\\Users\\fhdlu\\Desktop\\Green.txt"); // EN FIL SOM ÄR SKAPAD MÅSTE ANVÄNDAS OM IGEN, BYT EJ LÖSENORDED DÄR INNE. 
                                                                             // DET STÄLLER TILL DET NÄR MAN SKA HÄMTA EN USER..
        }
        else
        {
            string key = GenerateEncryptionKey(); // Hämtar denna method för att kunna skapa en ny nyckel. 
            File.WriteAllText("C:\\Users\\fhdlu\\Desktop\\Green.txt", key); // om den är tom, så skapar vi den.
            return key;
        }
    }

    public static string GenerateEncryptionKey()
    {
        var rng = new RNGCryptoServiceProvider();
        var byteArray = new byte[16]; // Genererar ett krypterat lösenord på 16 bokstäver. 
        rng.GetBytes(byteArray);
        return Convert.ToBase64String(byteArray);
    }
}
