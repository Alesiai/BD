using Poster.Entities;
using Poster.Views.AddNewItem;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Poster.Views.Tables
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        Order row;

        public Orders()
        {
            InitializeComponent();
            Seed.getListOfItem();
            Seed.getListOfUsers();
            DataGridXAML.ItemsSource = Seed.getListOfOrders(DateTime.Today, DateTime.Today.AddDays(1));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddOrder taskWindow = new AddOrder(new Order { Id = Seed.getIdForOrder() }, true);
            taskWindow.Show();
        }

        //Save Order's ID
        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                row = dg.SelectedItems[0] as Order;
            }
            catch { }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AddOrder taskWindow = new AddOrder(row, false);
                taskWindow.Show();
            }
            catch { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (FromDate.SelectedDate != null || ToDate.SelectedDate != null)
                DataGridXAML.ItemsSource = Seed.getListOfOrders(FromDate.SelectedDate, ToDate.SelectedDate);
            else
                DataGridXAML.ItemsSource = Seed.getListOfOrders(DateTime.Today, DateTime.Today.AddDays(1));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataGridXAML.ItemsSource = Seed.getListOfOrders(FromDate.SelectedDate, ToDate.SelectedDate);
        }
    }
}
