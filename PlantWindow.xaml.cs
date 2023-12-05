using System.Windows;
using System.Windows.Controls;
using GreenThumbHT23.Database;
using GreenThumbHT23.Model;

namespace GreenThumbHT23;

/// <summary>
/// Interaction logic for PlantWindow.xaml
/// </summary>
public partial class PlantWindow : Window
{
    public PlantWindow()
    {
        InitializeComponent();


    }

    private void btnSignout_Click(object sender, RoutedEventArgs e) // Go back
    {
        MyGardenWindow myGardenWindow = new MyGardenWindow();
        myGardenWindow.Show();
        Close();
    }

    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
        string searchAndFind = txtSearchName.Text.ToLower();
        string Description = txtDesc.Text.ToLower();

        using (var unitOfWork = new GreenUOW(new GreenThumbDbContext()))
        {
            try
            {
                var searchResults = await unitOfWork.PlantRepository.SearchPlantsAsync(searchAndFind, Description); // lägg in variablen

                LoadaPlantsSearched(searchResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching plants: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

    private void LoadaPlantsSearched(List<PlantModel> searchResults)
    {
        lstItemList.Items.Clear();

        foreach (var plant in searchResults)
        {

            ListViewItem plantItem = new ListViewItem
            {
                Content = $"{plant.PlantName} - {plant.PlantDescription}"
            };

            lstItemList.Items.Add(plantItem);
        }
    }


    private void txtSearchName_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void btnToAddPlant_Click(object sender, RoutedEventArgs e)
    {



        AddPlantWindow addPlantWindow = new AddPlantWindow();
        addPlantWindow.Show();
        Close();

    }
}
