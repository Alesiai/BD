using Poster.Entities;
using System.Windows;
using System.Windows.Controls;

namespace Poster.Views.Statistics
{
    /// <summary>
    /// Логика взаимодействия для Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        public Statistics()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MoneyTextBlock.Text = Seed.GetRevenue(FromDate.SelectedDate, ToDate.SelectedDate) + "руб";
        }
    }
}
