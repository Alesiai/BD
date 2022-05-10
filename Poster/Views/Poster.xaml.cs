using Poster.Entities;
using Poster.Views.Tables;
using System.Windows;

namespace Poster.Views
{
    /// <summary>
    /// Логика взаимодействия для Postaer.xaml
    /// </summary>
    public partial class Poster : Window
    {
        public Poster(User user)
        {
            InitializeComponent();
            if (user.Status != "admin")
            {
                Statistics.Visibility = Visibility.Collapsed;
                Users.Visibility = Visibility.Collapsed;
                Items.Visibility = Visibility.Collapsed;
            }
        }

        private void OpenOrders(object sender, RoutedEventArgs e)
        {
            MainBar.Content = new Orders();
        }
        private void OpenCustomers(object sender, RoutedEventArgs e)
        {
            MainBar.Content = new Users();
        }
        private void OpenStatistics(object sender, RoutedEventArgs e)
        {
            MainBar.Content = new Statistics.Statistics();
        }
        private void OpenPositions(object sender, RoutedEventArgs e)
        {
            MainBar.Content = new Positions();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
