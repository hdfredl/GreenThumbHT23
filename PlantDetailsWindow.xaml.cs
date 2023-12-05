using System.Windows;

namespace GreenThumbHT23
{
    /// <summary>
    /// Interaction logic for PlantDetailsWindow.xaml
    /// </summary>
    public partial class PlantDetailsWindow : Window
    {
        public PlantDetailsWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyGardenWindow myGardenWindow = new MyGardenWindow();
            myGardenWindow.Show();
            Close();
        }
    }
}
