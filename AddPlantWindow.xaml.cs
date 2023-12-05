using System.Windows;
using System.Windows.Controls;
using GreenThumbHT23.Database;
using GreenThumbHT23.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenThumbHT23;

/// <summary>
/// Interaction logic for AddPlantWindow.xaml
/// </summary>
public partial class AddPlantWindow : Window
{
    public AddPlantWindow()
    {
        InitializeComponent();
        LoadList();

    }

    private async void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        string plantName = txtPlantName.Text;
        string descPlant = txtDesc.Text;
        string instructions = txtInstructions.Text;
        string instrctionsDesc = txtInstructionsDesc.Text;

        if (string.IsNullOrWhiteSpace(plantName) || string.IsNullOrWhiteSpace(descPlant))
        {
            MessageBox.Show("Lägg till ett namn och beskrivning! ");
            return;
        }


        if (await PlantexistsAsync(plantName)) // om plantan redan finns 
        {
            MessageBox.Show("Plantan finns redan, välj ett annat namn.");
            return;
        }

        PlantModel newPlant = new PlantModel
        {
            PlantName = plantName,
            PlantDescription = descPlant

        };

        InstructionModel newInstruction = new InstructionModel // ?? == eller.
        {
            InstructionName = instructions ?? "Bevattningsteknik ",
            InstructionDescription = instrctionsDesc ?? "Vattna varje 5e minut"
        };

        newPlant.Instructions.Add(newInstruction);

        using (var unitOfWork = new GreenUOW(new GreenThumbDbContext()))
        {
            try
            {
                //await unitOfWork.PlantRepository.AddPlantAsync(newPlant);
                await unitOfWork.PlantRepository.AddPlantAsync(newPlant);
                unitOfWork?.Complete();

                MessageBox.Show("Plantan har blivit registrerad");

                LoadList();
                txtPlantName.Text = "";
                txtDesc.Text = "";
                txtInstructions.Text = "";
                txtInstructionsDesc.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fel har inträffat: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

    private async Task<bool> PlantexistsAsync(string plantName)
    {

        using (var unitOfWork = new GreenUOW(new GreenThumbDbContext())) // Om det redan finns en planta
        {
            return await unitOfWork.PlantRepository.PlantexistsAsync(plantName);
        }
    }

    private void btnRead_Click(object sender, RoutedEventArgs e)
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
                        string plantDetailsInfo = $"Planta: {plantDetails.PlantName}\nDescription: {plantDetails.PlantDescription} - Instruktioner: "; // Displayar plant

                        foreach (var instruction in plantDetails.Instructions) // går igenom deras instructions
                        {
                            plantDetailsInfo += $"{instruction.InstructionName} - {instruction.InstructionDescription}";
                        }

                        MessageBox.Show(plantDetailsInfo, "Plant Details", MessageBoxButton.OK);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Under development ");
    }

    private async void btnNeutral_Click(object sender, RoutedEventArgs e) // Delete 
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

    private void btnGoBack_Click(object sender, RoutedEventArgs e)
    {
        MyGardenWindow myGardenWindow = new MyGardenWindow();
        myGardenWindow.Show();
        Close();
    }

    private async void btnAddtoGarden_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (OtherStatics.CurrentUser != null)
            {
                List<PlantModel> selectedPlants = lstItemsInCart.Items.Cast<ListViewItem>().Select(plantInList => (PlantModel)plantInList.Tag).ToList();

                using (var uow = new GreenUOW(new GreenThumbDbContext()))
                {
                    try
                    {
                        GardenModel? userGarden = await uow.GardenRepository.GetByIdAsync(OtherStatics.CurrentUser.UserId); // Hämtar användarens UserId och kollar vems garden som är inloggad

                        if (userGarden == null) // Om användaren inte har valt ett garden name vid register, så får den ett random namn
                        {
                            userGarden = new GardenModel
                            {
                                GardenName = $"Budget garden 1.0 - {OtherStatics.CurrentUser.UserId}"
                            };
                            await uow.GardenRepository.AddGardenAsync(userGarden); // SKapar en ny garden till den existerande usern.
                        }


                        foreach (var selectedPlant in selectedPlants) // lägger till den valda plant till userns garden. Från selectedPlants - lstInCart
                        {
                            GardenConnection connection = new GardenConnection
                            {
                                GardenId = userGarden.GardenId,
                                PlantId = selectedPlant.PlantId,
                                Quantity = 1 // Har bara lagt till en åt gången, kanske lägger till hur många man får möjlighet till att add. Hade denna i tomteverkstad
                            };

                            await uow.GardenConnectionRepository.AddGardenConAsync(connection);
                        }

                        uow.Complete();

                        MessageBox.Show("Plantan har lagts till i din gård.", "Success", MessageBoxButton.OK);

                        lstItemsInCart.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnAddToCart_Click(object sender, RoutedEventArgs e)
    {
        ListViewItem selectedItem = (ListViewItem)lstItemList.SelectedItem;

        if (selectedItem != null)
        {
            // Lägger till här i denna listan, endast till listan. Sparar ej den än till databasen för att hämtas ut senare. Gjorde samma i Tomteverkstad
            lstItemsInCart.Items.Add(selectedItem);
        }
    }
}
