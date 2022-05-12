using Oracle.ManagedDataAccess.Client;
using Poster.Entities;
using Poster.Views.Tables;
using System;
using System.Data;
using System.IO;
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
                SaveToXml.Visibility = Visibility.Collapsed;
            }
            UserName.Text += user.Name + "\tСтатус: " + user.Status;
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

        private void SaveToXml_Click(object sender, RoutedEventArgs e)
        {
            string xml="";
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "select * from CaffeUser.XML_VIEW";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    xml = Convert.ToString(dr["faculties"]);
                }

                File.WriteAllText("D:\\XML.xml", xml);
            }
        }
    }
}
