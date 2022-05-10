using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Poster.Entities
{
    internal class Seed
    {
        internal static ObservableCollection<User> listOfUsers = new ObservableCollection<User>();
        internal static ObservableCollection<Item> listOfItems = new ObservableCollection<Item>();
        internal static ObservableCollection<Order> listOfOrders = new ObservableCollection<Order>();
        internal static ObservableCollection<ItemsInOrder> listOfItemsInOrder = new ObservableCollection<ItemsInOrder>();

        public static void SeedAllObjects()
        {
            listOfUsers.Add(new User
            {
                Id = 1,
                Name = "Катя",
                Login = "user",
                Password = "1111",
                DateOfBirth = Convert.ToDateTime("12.02.2002")
            });

            listOfUsers.Add(new User
            {
                Id = 2,
                Name = "Admin",
                Login = "admin",
                Password = "1111",
                Status = "admin",
                DateOfBirth = Convert.ToDateTime("10.04.1998")
            });

            listOfItems.Add(
                new Item
                {
                    Name = "Кофе",
                    Cost = 1.35,
                    Id = 1,
                });
            listOfItems.Add(
                new Item
                {
                    Name = "Чай",
                    Cost = 1.35,
                    Id = 2,
                });
            listOfItems.Add(
                new Item
                {
                    Name = "Булочка DELETED",
                    Cost = 1.35,
                    Id = 3,
                    IsDeleted = true
                });

            listOfItemsInOrder.Add(new ItemsInOrder
            {
                Id = 1,
                Item = listOfItems[0],
                OrderID = 1,
                Count = 2,
            });

            listOfItemsInOrder.Add(new ItemsInOrder
            {
                Id = 2,
                Item = listOfItems[1],
                OrderID = 1,
                Count = 1,
            });

            listOfItemsInOrder.Add(new ItemsInOrder
            {
                Id = 3,
                Item = listOfItems[2],
                OrderID = 2,
                Count = 2,
            }); ;

            var orderFirst = new Order
            {
                Cost = getCost(1),
                Id = 1,
                Created = DateTime.Now,
                User = listOfUsers.Where(x => x.Id == 1).First(),
            };
            listOfOrders.Add(orderFirst);

            var orderSecond = new Order
            {
                Cost = getCost(2),
                Id = 2,
                Created = DateTime.Now,
                User = listOfUsers.Where(x => x.Id == 1).First(),
            };
            listOfOrders.Add(orderSecond);

            var third = new Order
            {
                Cost = getCost(2),
                Id = 3,
                Created = DateTime.Now.AddDays(1),
                User = listOfUsers.Where(x => x.Id == 1).First(),
            };
            listOfOrders.Add(third);
        }

        public static List<User> getListOfUsers()
        {
            ObservableCollection<User> list = new ObservableCollection<User>();

            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "SELECT * FROM TABLE(CaffeUser.UserInCaffeNS.GetUserInCaffe(1000))";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    User user = new User
                    {
                        Id = Convert.ToInt32(dr["IDUSER"]),
                        Name = Convert.ToString(dr["Name"]),
                        Login = Convert.ToString(dr["LOGIN"]),
                        Password = Convert.ToString(dr["Password"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Created = Convert.ToDateTime(dr["Created"]),
                        DateOfBirth = Convert.ToDateTime(dr["DATEOFBIRTH"]),
                        Status = Convert.ToString(dr["STATUS"]),
                    };
                    list.Add(user);
                }
            }

            return list.ToList();
        }

        public static List<Item> getListOfItem()
        {
            ObservableCollection<Item> list = new ObservableCollection<Item>();

            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "SELECT * FROM TABLE(CaffeUser.ItemNS.GetItem(1000))";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    Item item = new Item
                    {
                        Id = Convert.ToInt32(dr["IDITEM"]),
                        Name = Convert.ToString(dr["Name"]),
                        Cost = Convert.ToDouble(dr["Cost"])
                    };
                    list.Add(item);
                }
            }

            return list.ToList();
        }


        //здесь началась ебала
        public static List<Order> getListOfOrders(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime dateTime = (DateTime)(ToDate != null ? ToDate : DateTime.Now);
            return listOfOrders.Where(x => x.IsDeleted == false && x.Created >= FromDate && x.Created <= dateTime.AddDays(1)).ToList();
        }
        public static List<Order> getListOfOrders(DateTime FromDate)
        {
            return listOfOrders.Where(x => x.IsDeleted == false && x.Created >= FromDate && x.Created <= FromDate.AddDays(1)).ToList();
        }
        public static List<Order> getListOfOrders()
        {
            return listOfOrders.Where(x => x.IsDeleted == false).ToList();
        }

        public static List<ItemsInOrder> getListOfItemsInOrderById(int orderId)
        {
            return listOfItemsInOrder.Where(x => x.OrderID == orderId).ToList();
        }

        //тут она закончилась


        public static User getUser(string login)
        {
            User user = new User();
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "SELECT * FROM TABLE(CaffeUser.UserInCaffeNS.GetUserInCaffeByLogin(1, '" + login + "'))";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                
                foreach (DataRow dr in dt.Rows)
                {

                    user.Id = Convert.ToInt32(dr["IDUSER"]);
                    user.Name = Convert.ToString(dr["Name"]);
                    user.Login = Convert.ToString(dr["LOGIN"]);
                    user.Password = Convert.ToString(dr["Password"]);
                    user.Phone = Convert.ToString(dr["Phone"]);
                    user.Created = Convert.ToDateTime(dr["Created"]);
                    user.DateOfBirth = Convert.ToDateTime(dr["DATEOFBIRTH"]);
                    user.Status = Convert.ToString(dr["STATUS"]);
                }
            }

            return user;
        }

        public static User GetUser(int userId)
        {            
            User user = new User();
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "SELECT * FROM TABLE(CaffeUser.UserInCaffeNS.GetUserInCaffeById(1,"+ userId + "))";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                
                foreach (DataRow dr in dt.Rows)
                {
                    user.Id = Convert.ToInt32(dr["IDUSER"]);
                    user.Name = Convert.ToString(dr["Name"]);
                    user.Login = Convert.ToString(dr["LOGIN"]);
                    user.Password = Convert.ToString(dr["Password"]);
                    user.Phone = Convert.ToString(dr["Phone"]);
                    user.Created = Convert.ToDateTime(dr["Created"]);
                    user.DateOfBirth = Convert.ToDateTime(dr["DATEOFBIRTH"]);
                    user.Status = Convert.ToString(dr["STATUS"]) == "0" ? "user" : "admin";
                }
            }

            return user;
        }

        public static Item GetItem(int itemId)
        {

            Item item = new Item();
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "SELECT * FROM TABLE(CaffeUser.ItemNS.GetItemById(1," + itemId + "))";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {

                    item.Id = Convert.ToInt32(dr["IDitem"]);
                    item.Name = Convert.ToString(dr["Name"]);
                    item.Cost = Math.Round(Convert.ToDouble(dr["Cost"]),2);
                    item.IsDeleted = Convert.ToBoolean(dr["Isdeleted"]);
                }
            }

            return item;
        }

        //не сделан
        public static string GetRevenue(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime dateTime = (DateTime)(ToDate != null ? ToDate : DateTime.Now);
            return "245";
        }
        
        //не сделан
        public static List<int> GetCountOfOrdersPerDey(DateTime fromDate, DateTime toDate)
        {
            return new List<int>();
        }

        //проблема в хуй знает чем
        public static void addUser(string name, string login, string password, DateTime? dateOfBirth, string phone, string status)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.UserInCaffeNS.InsertUserInCaffe";
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.Add("Name", OracleDbType.Varchar2, 30).Value = name;
            cmd.Parameters.Add("Login", OracleDbType.Varchar2, 30).Value = login;
            cmd.Parameters.Add("Password", OracleDbType.Varchar2, 30).Value = password;
            cmd.Parameters.Add("Phone", OracleDbType.Varchar2, 30).Value = phone;
            cmd.Parameters.Add("Created", OracleDbType.Varchar2, 30).Value = DateTime.Now;
            cmd.Parameters.Add("DateOfBirth", OracleDbType.Varchar2, 30).Value = dateOfBirth;
            cmd.Parameters.Add("Status", OracleDbType.Varchar2, 30).Value = status == "admin" ? "1" : "0";
            cmd.Parameters.Add("IsDeleted", OracleDbType.Decimal, 30).Value = 0;
            cmd.ExecuteNonQuery();
        }

        public static void addItem(string name, double cost)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.ItemNS.InsertItem";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("Name", OracleDbType.Varchar2, 30).Value = name;
            cmd.Parameters.Add("Cost", OracleDbType.Decimal, 30).Value = cost;
            cmd.Parameters.Add("IsDeleted", OracleDbType.Decimal, 30).Value = 0;
            cmd.ExecuteNonQuery();
        }

        public static void addItemToItemsInOrderList(ItemsInOrder itemsInOrder)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.ItemsInOrderNS.InsertItemsInOrder";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("IdOrder", OracleDbType.Varchar2, 30).Value = itemsInOrder.OrderID;
            cmd.Parameters.Add("IdItem", OracleDbType.Decimal, 30).Value = itemsInOrder.Item.Id;
            cmd.Parameters.Add("Count", OracleDbType.Decimal, 30).Value = itemsInOrder.Count;
            cmd.ExecuteNonQuery();
        }
      
        //проблема в хуй знает чем
        public static void addOrder(Order order)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.OrderInCaffeNS.InsertOrderInCaffe";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("IdUser", OracleDbType.Int32, 30).Value = order.User.Id;
            cmd.Parameters.Add("Created", OracleDbType.Varchar2, 30).Value = order.Created;
            cmd.Parameters.Add("Discount", OracleDbType.Decimal, 30).Value = order.Discount;
            cmd.Parameters.Add("IsDeleted", OracleDbType.Int32, 30).Value = 0;
            cmd.ExecuteNonQuery();
        }

        public static void updateItem(int itemId, string name)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.ItemNS.UpdateItem";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("IdItem_t", OracleDbType.Int32, 30).Value = itemId;
            cmd.Parameters.Add("Name", OracleDbType.Varchar2, 30).Value = name;
            cmd.ExecuteNonQuery();
        }
        //удалить два поя
        public static void updateUser(int id, string name, string login, string password, string phone, DateTime? dateOfBirth, string status)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.UserInCaffeNS.UpdateUserInCaffe";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("IdUser_t", OracleDbType.Int32, 30).Value = id;
            cmd.Parameters.Add("Name_t", OracleDbType.Varchar2, 30).Value = name;
            cmd.Parameters.Add("Login_t", OracleDbType.Varchar2, 30).Value = login;
            cmd.Parameters.Add("Password_t", OracleDbType.Varchar2, 30).Value = password;
            cmd.Parameters.Add("Phone_t", OracleDbType.Varchar2, 30).Value = phone;
            cmd.Parameters.Add("DateOfBirth_t", OracleDbType.Varchar2, 30).Value = dateOfBirth;
            cmd.Parameters.Add("Status_t", OracleDbType.Varchar2, 30).Value = status == "admin" ? "1" : "0";
            cmd.ExecuteNonQuery();
        }
        //удалить два поя
        public static void updateOrder(Order order)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.OrderInCaffeNS.UpdateOrderInCaffe";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("IdOrder_t", OracleDbType.Int32, 30).Value = order.Id;
            cmd.Parameters.Add("IdUser_t", OracleDbType.Int32, 30).Value = order.User.Id;
            cmd.Parameters.Add("Discount_t", OracleDbType.Varchar2, 30).Value = order.Discount;
            cmd.ExecuteNonQuery();
        }

        public static void deleteItem(int itemId)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.ItemNS.DeleteItemGeniusVersion";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("IdItem", OracleDbType.Int32, 30).Value = itemId;
            cmd.ExecuteNonQuery();
        }

        public static void deleteUser(int userId)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.UserInCaffeNS.DeleteUserInCaffeGeniusVersion";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("IdUser", OracleDbType.Int32, 30).Value = userId;
            cmd.ExecuteNonQuery();
        }

        public static void deleteItemFromListOfItem(int? id)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.ItemsInOrderNS.DeleteItemsInOrder";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("ItemsInOrder", OracleDbType.Int32, 30).Value = id;
            cmd.ExecuteNonQuery();

        }

        public static void deleteOrder(Order order)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.OrderInCaffeNS.DeleteOrderInCaffeGeniusVersion";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("IdOrder_t", OracleDbType.Int32, 30).Value = order.Id;
            cmd.ExecuteNonQuery();
        }

        public static double getCost(int id)
        {
            double cost = Math.Round(getListOfItemsInOrderById(id).Sum(x => x.Item.Cost * x.Count), 2);
            return cost;
        }

        public static int getIdForItemsInOrder()
        {
            int count = 0;
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "select CaffeUser.OrderInCaffeNS.GetCountOfAllOrder from dual";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    count = Convert.ToInt32(dr["GetCountOfAllOrder"]);
                }
            }

            return count + 1;
        }

    }
}
 