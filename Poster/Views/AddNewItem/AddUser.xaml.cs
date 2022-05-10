using Poster.Entities;
using System;
using System.Windows;

namespace Poster.Views.AddNewItem
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public static bool isNew = false;
        public static int Id;
        public AddUser(int userId)
        {
            InitializeComponent();
            if (userId == 0)
            {
                isNew = true;
                DelateButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                Id = userId;
                User user = Seed.GetUser(userId);
                NameTextBox.Text = user.Name;
                LoginTextBox.Text = user.Login;
                PasswordTextBox.Text = user.Password;
                DateOfBirthText.SelectedDate = user.DateOfBirth;
                PhoneTextBox.Text = user.Phone;
                StatusText.Text = user.Status;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isNew)
            {
                Seed.addUser(NameTextBox.Text, LoginTextBox.Text, PasswordTextBox.Text,
                    DateOfBirthText.SelectedDate, PhoneTextBox.Text, StatusText.Text);
            }
            else
            {
                Seed.updateUser(Id, NameTextBox.Text, LoginTextBox.Text, PasswordTextBox.Text, PhoneTextBox.Text,
                    DateOfBirthText.SelectedDate, StatusText.Text);
            }
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Seed.deleteUser(Id);
            Close();
        }
    }
}
