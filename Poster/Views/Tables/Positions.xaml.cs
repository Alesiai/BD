using Poster.Entities;
using Poster.Views.AddNewItem;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Poster.Views.Tables
{
    public partial class Positions : Page
    {
        Item row;
        public Positions()
        {
            InitializeComponent();
            DataGridXAML.ItemsSource = Seed.getListOfItem();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPosition addPosition = new AddPosition(new int());
            addPosition.Show();
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                row = dg.SelectedItems[0] as Item;
            }
            catch { }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AddPosition addPosition = new AddPosition(row.Id);
                addPosition.Show();
            }
            catch { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataGridXAML.ItemsSource = Seed.getListOfItem();
        }
    }
}
