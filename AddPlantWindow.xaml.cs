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
        //string instructions = txtInstructions.Text;
        //string instrctionsDesc = txtInstructionsDesc.Text;

        if (string.IsNullOrWhiteSpace(plantName) || string.IsNullOrWhiteSpace(descPlant))
        {
            MessageBox.Show("Lägg till ett namn och beskrivning! ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        //else if (instructions.Length < 3 && instrctionsDesc.Length < 3)
        //{
        //    MessageBox.Show("Lägg till instruktioner och beskrivning av plantan ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    return;
        //}

        if (await PlantexistsAsync(plantName)) // om plantan redan finns 
        {
            MessageBox.Show("Plantan finns redan, välj ett annat namn.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        PlantModel newPlant = new PlantModel // lägger till en planta
        {
            PlantName = plantName,
            PlantDescription = descPlant

        };

        //InstructionModel newInstruction = new InstructionModel // ?? == eller. // lägger till en instructions
        //{
        //    InstructionName = instructions,
        //    InstructionDescription = instrctionsDesc
        //};

        //newPlant.Instructions.Add(newInstruction);

        using (var unitOfWork = new GreenUOW(new GreenThumbDbContext()))
        {
            try
            {

                foreach (var item in lstInstructionList.Items)
                {
                    if (item is InstructionModel instruction)
                    {
                        newPlant.Instructions.Add(instruction);
                    }
                }

                await unitOfWork.PlantRepository.AddPlantAsync(newPlant);
                unitOfWork?.Complete();

                MessageBox.Show("Plantan har blivit registrerad");

                LoadList();
                txtPlantName.Text = "";
                txtDesc.Text = "";
                txtInstructions.Text = "";
                txtInstructionsDesc.Text = "";
                lstInstructionList.Items.Clear();
                txtPlantQuantity.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fel har inträffat: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }

    private void btnAddInstructions_Click(object sender, RoutedEventArgs e)
    {
        // Gör logik så att man kan addera flera instructions till en planta.
        string instructions = txtInstructions.Text;
        string instrctionsDesc = txtInstructionsDesc.Text;

        if (instructions.Length < 3 && instrctionsDesc.Length < 3)
        {
            MessageBox.Show("Lägg till instruktioner och beskrivning av plantan ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        InstructionModel newInstruction = new InstructionModel // ?? == eller. // lägger till en instructions
        {
            InstructionName = instructions,
            InstructionDescription = instrctionsDesc
        };

        lstInstructionList.Items.Add(newInstruction);


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
                        string plantDetailsInfo = $"Planta: {plantDetails.PlantName}\n" + $"Description: {plantDetails.PlantDescription} -\n" + "Instruktioner: "; // Displayar plant med brytning

                        foreach (var instruction in plantDetails.Instructions) // går igenom deras instructions
                        {
                            plantDetailsInfo += $"{instruction.InstructionName} - {instruction.InstructionDescription}\n";
                        }

                        MessageBox.Show(plantDetailsInfo, "Plant Details", MessageBoxButton.OK); // Printar ut plantDetailsInfo som sparats
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
        if (lstItemList.SelectedItem != null)
        {
            PlantModel updatingPlant = (PlantModel)((ListViewItem)lstItemList.SelectedItem).Tag;

            string updatePlantName = txtPlantName.Text;
            string updatePlantDesc = txtDesc.Text;
            string updatePlantInstructions = txtInstructions.Text;
            string updatePlantDescInstrcDesc = txtInstructionsDesc.Text;

            using (var uow = new GreenUOW(new GreenThumbDbContext()))
            {
                try
                {
                    PlantModel? existingPlant = await uow.PlantRepository.GetByIdAsync(updatingPlant.PlantId); // Hämta 

                    if (existingPlant != null)
                    {
                        existingPlant.PlantName = updatePlantName;
                        existingPlant.PlantDescription = updatePlantDesc;

                        InstructionModel? existingInstructions = existingPlant.Instructions.FirstOrDefault(); // kopplar in Instruction klassen

                        if (existingInstructions != null)
                        {
                            existingInstructions.InstructionName = updatePlantInstructions; // uppdatererar
                            existingInstructions.InstructionDescription = updatePlantDescInstrcDesc;
                        }

                        uow.Complete();
                        MessageBox.Show("Plantan har blivit uppdaterad");
                        LoadList();
                    }
                    else
                    {
                        MessageBox.Show("Ingen planta har blivit uppdaterd.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errorn: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

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

                if (lstItemsInCart.Items.Count == 0)
                {
                    MessageBox.Show("Din cart är tom, addera en planta först.", "Tom vagn", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }


                using (var uow = new GreenUOW(new GreenThumbDbContext()))
                {
                    try
                    {
                        GardenModel? userGarden = await uow.GardenRepository.GetByIdAsync(OtherStatics.CurrentUser.UserId); // Hämtar användarens UserId och kollar vems garden som är inloggad
                        int quantity;

                        if (int.TryParse(txtPlantQuantity.Text, out quantity))
                        {
                            MessageBox.Show("Ange antal plants du vill ha till garden", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        foreach (var selectedPlant in selectedPlants) // lägger till den valda plant till userns garden. Från selectedPlants - lstInCart
                        {
                            GardenConnection connection = new GardenConnection
                            {
                                GardenId = userGarden!.GardenId,
                                PlantId = selectedPlant.PlantId, // Hämtar från listviewen, lstItemsInCart. Vet ej hur man kan lägga till direkt till garden, så fick bli en mellanhand här. 
                                Quantity = quantity //  Hade denna i tomteverkstad
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
            lstItemList.Items.Remove(selectedItem);

            // Lägger till här i denna listan, endast till listan. Sparar ej den än till databasen för att hämtas ut senare. Gjorde samma i Tomteverkstad
            lstItemsInCart.Items.Add(selectedItem);
        }
    }

    private void btnDeleteFromCart_Click(object sender, RoutedEventArgs e)
    {

        ListViewItem selectedItem = (ListViewItem)lstItemsInCart.SelectedItem;
        if (selectedItem != null)
        {
            lstItemsInCart.Items.Remove(selectedItem);
        }

    }

    private void lstInstructionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        lstInstructionList.Items.Clear();
        // lstItemList
        ListViewItem selectedItem = (ListViewItem)lstItemList.SelectedItem;

        if (selectedItem != null)
        {
            PlantModel selectedPlant = (PlantModel)selectedItem.Tag;

            List<InstructionModel> instructions = selectedPlant.Instructions; // instruktioner

            foreach (var instruction in instructions)
            {

                lstInstructionList.Items.Add(instruction);

            }
        }
    }

    private void lstItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}

