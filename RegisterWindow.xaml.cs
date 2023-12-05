using System.Windows;
using GreenThumbHT23.Database;
using GreenThumbHT23.Model;

namespace GreenThumbHT23
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            // Username: Testare
            // Password: Password

        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e) // Registera en ny användare 
        {
            string username = txtUserName.Text;
            string password = txtPassword.Password;
            string gardenName = txtGardenName.Text;

            using (var uow = new GreenUOW(new GreenThumbDbContext()))  // Ser om usernamne redan finns i databasen! 
            {
                UserModel? existingUser = await uow.UserRepository.GetByUsernameAsync(username);
                if (existingUser != null)
                {
                    MessageBox.Show("Användare finns redan, välj ett annat", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (username.Length < 4)
                {
                    MessageBox.Show("Minst 4 bokstäver för att kunna registrera användare.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (password.Length < 8)
                {
                    MessageBox.Show("Minst 8 bokstäver för att kunna registrera ett lösenord.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (gardenName.Length < 3)
                {
                    MessageBox.Show("Minst 3 bokstäver för att kunna tillhöra en garden.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                UserModel newUser = new UserModel
                {
                    Username = username,
                    Password = password
                };
                GardenModel newGarden = new GardenModel
                {
                    GardenName = gardenName
                };
                newUser.Garden = newGarden;
                await uow.UserRepository.AddCustomerAsync(newUser);
                uow.Complete();

                MessageBox.Show("Användare registrerad!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();

            }
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e) // Gå tillbaka till Mainmeny // Sign in window 
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
