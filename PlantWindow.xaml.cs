using System.Windows;
using System.Windows.Controls;
using GreenThumbHT23.Database;
using GreenThumbHT23.Model;
using Microsoft.EntityFrameworkCore;

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

    private async void btnDeltePlant_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (lstItemList.SelectedItem != null)
            {
                ListViewItem selectedItem = (ListViewItem)lstItemList.SelectedItem;

                PlantModel selectedPlant = (PlantModel)selectedItem.Tag;

                using (var unitOfWork = new GreenUOW(new GreenThumbDbContext()))
                {
                    PlantModel? plantToDelete = await unitOfWork.PlantRepository.GetByIdAsync(selectedPlant.PlantId); // Hämta från db

                    if (plantToDelete != null)
                    {
                        await unitOfWork.PlantRepository.DeleteOrdersAsync(plantToDelete);

                        unitOfWork?.Complete();

                        MessageBox.Show($"Plantan {plantToDelete.PlantName} har tagits bort.", "Plantan Borttagen", MessageBoxButton.OK);

                        LoadList();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }


    private void btnDetailsAboutPlant_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (lstItemList.SelectedItem != null)
            {
                ListViewItem selectedItem = (ListViewItem)lstItemList.SelectedItem;

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


    private void LoadList()
    {
        using (GreenThumbDbContext context = new())
        {
            lstItemList.Items.Clear();
            var giftsInList = context.Plants.ToList();
            foreach (var gift in giftsInList)
            {
                ListViewItem giftsToList = new();

                giftsToList.Tag = gift;
                giftsToList.Content = $" {gift.PlantName},  {gift.PlantDescription} ";

                lstItemList.Items.Add(giftsToList);
            }
        }
    }
}
