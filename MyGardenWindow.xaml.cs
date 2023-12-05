using System.Windows;
using GreenThumbHT23.Database;
using GreenThumbHT23.Model;

namespace GreenThumbHT23;

/// <summary>
/// Interaction logic for MyGardenWindow.xaml
/// </summary>
public partial class MyGardenWindow : Window
{

    public MyGardenWindow()
    {
        InitializeComponent();

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
            List<PlantModel> userPlants;

            lstPurchasedList.Items.Clear();



        }
    }


}
