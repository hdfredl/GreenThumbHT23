using System.Windows;
using System.Windows.Controls;
using GreenThumbHT23.Database;
using GreenThumbHT23.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenThumbHT23;

/// <summary>
/// Interaction logic for MyGardenWindow.xaml
/// </summary>
public partial class MyGardenWindow : Window
{

    public MyGardenWindow()
    {
        InitializeComponent();
        LoadUserlants();
        // Displaya alla plants, garden o all info som behöves och meny till övriga delar i appen.
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void btnGoBack_Click(object sender, RoutedEventArgs e)
    {




        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void btnPlantWindow_Click(object sender, RoutedEventArgs e)
    {
        PlantWindow plantWindow = new PlantWindow();
        plantWindow.Show();
        Close();
    }

    private void btnAddPlantWindow_Click(object sender, RoutedEventArgs e)
    {
        AddPlantWindow addPlantWindow = new AddPlantWindow();
        addPlantWindow.Show();
        Close();

    }

    private void btnDetailsWindow_Click(object sender, RoutedEventArgs e) // Nice to have, avvakta
    {
        PlantDetailsWindow plantDetailsWindow = new PlantDetailsWindow();
        plantDetailsWindow.Show();
        Close();

    }

    private void LoadUserlants()
    {
        using (var uow = new GreenUOW(new GreenThumbDbContext()))
        {

            if (OtherStatics.CurrentUser != null)// hämtar från UOW klassen, anvädes i Tomteverkstad
            {
                List<PlantModel> userPlants = uow.UserPlantsInDB(OtherStatics.CurrentUser.UserId); // lägger in user id som är en Int

                lstPurchasedList.Items.Clear();

                foreach (var plant in userPlants)
                {
                    ListViewItem plantToList = new();
                    plantToList.Tag = plant;
                    plantToList.Content = $"{plant.PlantName} och {plant.PlantDescription} ";

                    lstPurchasedList.Items.Add(plantToList);
                }
            }
        }
    }

    private void btnDetailaboutPlant_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (lstPurchasedList.SelectedItem != null)
            {
                ListViewItem selectedItem = (ListViewItem)lstPurchasedList.SelectedItem;

                PlantModel selectedPlant = (PlantModel)selectedItem.Tag; // Tagar plant

                using (GreenThumbDbContext context = new()) // Databasen
                {
                    var plantDetails = context.Plants.Include(plant => plant.Instructions).FirstOrDefault(p => p.PlantId == selectedPlant.PlantId);

                    if (plantDetails != null)
                    {
                        string plantDetailsInfo = $"Plantan kallas: {plantDetails.PlantName} \nBeskrivning av plantan: {plantDetails.PlantDescription} - Instruktioner om hur man tar hand om plantan:  "; // Displayar plant

                        foreach (var instruction in plantDetails.Instructions) // går igenom deras instructions
                        {
                            plantDetailsInfo += $"{instruction.InstructionName} - {instruction.InstructionDescription}"; // displayar instrucktionen och beskrivning av hur man gör
                        }

                        MessageBox.Show(plantDetailsInfo, "Plant Details", MessageBoxButton.OK); // Printar hela stringen.
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }


    }
}
