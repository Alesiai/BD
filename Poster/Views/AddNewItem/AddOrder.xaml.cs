using Poster.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Poster.Views.AddNewItem
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        Order Order { get; set; }
        Item Item { get; set; }
        public static bool IsNew { get; set; } = false;

        public AddOrder(Order order, bool isNew)
        {
            Order = order;
            Order.User = MainWindow.User;
            IsNew = isNew;

            InitializeComponent();

            DataGridXAML.ItemsSource = Seed.getListOfItemsInOrderById(order.Id);
            ProductList.ItemsSource = Seed.getListOfItem();
            CostTextBlock.Text = Convert.ToString(Order.Cost);
            DiscountTextBlock.Text = Convert.ToString(Order.Discount);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Seed.deleteItemFromListOfItem(row.Id);
            DataGridXAML.ItemsSource = Seed.getListOfItemsInOrderById(Order.Id);

            Order.Cost = Seed.getCost(Order.Id) - Order.Discount;
            CostTextBlock.Text = Convert.ToString(Order.Cost);
        }

        ItemsInOrder row;

        private void DataGridXAML_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                row = dg.SelectedItems[0] as ItemsInOrder;

                Order.Cost = Seed.getCost(Order.Id) - Order.Discount;
                CostTextBlock.Text = Convert.ToString(Order.Cost);
            }
            catch { }
        }

        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox dg = sender as ListBox;
                Item = dg.SelectedItems[0] as Item;
            }
            catch { }
        }

        private void ProductList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var itemInOrder = new ItemsInOrder
            {
                Id = Seed.getIdForItemsInOrder(),
                Count = 1,
                Item = Item,
                OrderID = Order.Id,
            };

            Seed.addItemToItemsInOrderList(itemInOrder);

            DataGridXAML.ItemsSource = Seed.getListOfItemsInOrderById(Order.Id);

            Order.Cost = Seed.getCost(Order.Id) - Order.Discount;
            CostTextBlock.Text = Convert.ToString(Order.Cost);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Order.Cost = Seed.getCost(Order.Id);

            if (IsNew)
            {
                Seed.addOrder(Order);
            }
            else
            {
                Seed.updateOrder(Order);
            }
            Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton pressed = (RadioButton)sender;
                if (pressed.Content != null)
                {
                    try
                    {
                        switch (pressed.Content.ToString())
                        {
                            case "0%":
                                Order.Discount = 0 * Seed.getCost(Order.Id);
                                break;
                            case "10%":
                                Order.Discount = 0.1 * Seed.getCost(Order.Id);
                                break;
                            case "20%":
                                Order.Discount = 0.2 * Seed.getCost(Order.Id);
                                break;
                            case "50%":
                                Order.Discount = 0.5 * Seed.getCost(Order.Id);
                                break;
                            case "100%":
                                Order.Discount = 1 * Seed.getCost(Order.Id);
                                break;
                            default:
                                Order.Discount = 0;
                                break;
                        }

                        Order.Cost = Seed.getCost(Order.Id) - Order.Discount;
                        DiscountTextBlock.Text = Convert.ToString(Order.Discount);
                        CostTextBlock.Text = Convert.ToString(Order.Cost);
                    }
                    catch { }
                }

            }
            catch { }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Seed.deleteOrder(Order);
            Close();
        }
    }
}
