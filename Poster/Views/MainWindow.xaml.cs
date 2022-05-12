using Poster.Entities;
using System.Windows;
using System.Windows.Controls;

namespace Poster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static User User { get; set; }
        public bool times = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = Seed.getUser(NameTextBox.Text);

            if (user != null)
            {
                if (user.Password == PasswordBox.Password.ToString())
                {
                    Views.Poster NewWindow = new Views.Poster(user);
                    User = user;
                    NewWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Пароль не верный");
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }
    }
}
