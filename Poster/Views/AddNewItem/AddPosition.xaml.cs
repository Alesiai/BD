using Poster.Entities;
using System;
using System.Windows;

namespace Poster.Views.AddNewItem
{
    /// <summary>
    /// Логика взаимодействия для AppPosition.xaml
    /// </summary>
    public partial class AddPosition : Window
    {
        public static bool isNew;
        public static int Id;
        public AddPosition(int itemId)
        {
            InitializeComponent();
            if (itemId == 0)
            {
                isNew = true;
                DelateButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                Id = itemId;
                Item item = Seed.GetItem(itemId);
                NameTextBox.Text = item.Name;
                CostTextBox.Text = Convert.ToString(Math.Round(item.Cost, 2));
                CostTextBox.IsReadOnly = true;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isNew)
            {
                Seed.addItem(NameTextBox.Text, Convert.ToDouble(CostTextBox.Text.Replace(".", ",")));
            }
            else
            {
                Seed.updateItem(Id, NameTextBox.Text);
            }
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Seed.deleteItem(Id);
            Close();
        }
    }
}
