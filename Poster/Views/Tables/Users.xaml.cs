using Poster.Entities;
using Poster.Views.AddNewItem;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Poster.Views.Tables
{
    /// <summary>
    /// Логика взаимодействия для Customers.xaml
    /// </summary>
    public partial class Users : Page
    {
        User row;
        public Users()
        {
            InitializeComponent();
            DataGridXAML.ItemsSource = Seed.getListOfUsers();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddUser addUser = new AddUser(new int());
            addUser.Show();
        }

        //Save Order's ID
        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                row = dg.SelectedItems[0] as User;
            }
            catch { }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AddUser taskWindow = new AddUser(row.Id);
                taskWindow.Show();
            }
            catch { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataGridXAML.ItemsSource = Seed.getListOfUsers();
        }
    }
}
