using System.Windows;
using GreenThumbHT23.Database;
using GreenThumbHT23.Model;

namespace GreenThumbHT23;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();


    }

    private async void btnLogIn_Click(object sender, RoutedEventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Password;

        using (var uow = new GreenUOW(new GreenThumbDbContext()))
        {
            UserModel? user = await uow.UserRepository.GetByUsernameAsync(username); // Hämtar från Usermodel/DB och checkar om username finns samt om lösenord stämmer

            if (user != null && user.Password == password)
            {
                OtherStatics.CurrentUser = user; // kollar den lokala userns inloggning 

                MessageBox.Show("Login Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                MyGardenWindow myGardenWindow = new MyGardenWindow();
                myGardenWindow.Show();
                Close();

            }
            else
            {
                MessageBox.Show("Fel lösenord eller användarnamn, försök igen.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtLoginWarning.Visibility = Visibility.Visible;
            }
        }
    }

    private void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        RegisterWindow registerWindow = new RegisterWindow();
        registerWindow.Show();
        Close();

    }
}