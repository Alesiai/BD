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
                        Status = Convert.ToString(dr["STATUS"]) == "0"? "user" : "admin",
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

        public static List<Order> getListOfOrders(DateTime? FromDate, DateTime? ToDate)
        {

            List<Order> orders = new List<Order>();
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string from = FromDate != null ? ("\'" + FromDate.Value.ToShortDateString() + "\'") : "null";
                string to = ToDate != null ? ("\'" + ToDate.Value.ToShortDateString() + "\'") : "null";

                string sql = "SELECT * FROM TABLE(CaffeUser.OrderInCaffeNS.GetOrderInCaffeFromDate(null, " + from + ", " + to + "))";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    Order order = new Order
                    {
                        Id = Convert.ToInt32(dr["IDORDER"]),
                        Created = Convert.ToDateTime(dr["Created"]),
                        Discount = Convert.ToDouble(dr["Discount"]),
                        Cost = getCost(Convert.ToInt32(dr["IDORDER"])) - Convert.ToDouble(dr["Discount"]),
                        IsDeleted = Convert.ToBoolean(dr["IsDeleted"]),
                        User = GetUser(Convert.ToInt32(dr["IDUSER"])),
                    };
                    orders.Add(order);
                }
            }

            return orders;
        }

        public static List<Order> getListOfOrdersForRevenue(DateTime? FromDate, DateTime? ToDate)
        {

            List<Order> orders = new List<Order>();
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string from = FromDate != null ? ("\'" + FromDate.Value.ToShortDateString() + "\'") : "null";
                string to = ToDate != null ? ("\'" + ToDate.Value.ToShortDateString() + "\'") : "null";

                string sql = "SELECT * FROM TABLE(CaffeUser.OrderInCaffeNS.GetOrderInCaffeFromDate(null, " + from + ", " + to + "))";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    Order order = new Order
                    {
                        Id = Convert.ToInt32(dr["IDORDER"]),
                        Discount = Convert.ToDouble(dr["Discount"]),
                        Cost = getCost(Convert.ToInt32(dr["IDORDER"])) - Convert.ToDouble(dr["Discount"]),
                    };
                    orders.Add(order);
                }
            }

            return orders;
        }

        public static List<ItemsInOrder> getListOfItemsInOrderById(int orderId)
        {

            List<ItemsInOrder> itemsInOrder = new List<ItemsInOrder>();
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "SELECT * FROM TABLE(CaffeUser.ItemsInOrderNS.GetListOfItemsInOrderById(null, " + Convert.ToString(orderId) + "))";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ItemsInOrder order = new ItemsInOrder
                    {
                        Id = Convert.ToInt32(dr["ItemsInOrder"]),
                        OrderID = Convert.ToInt32(dr["IdOrder"]),
                        Count = Convert.ToInt32(dr["Count"]),
                        Item = GetItem(Convert.ToInt32(dr["IdItem"]))
                    };

                    itemsInOrder.Add(order);
                }
            }

            return itemsInOrder;
        }

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
                    user.Status = Convert.ToString(dr["STATUS"]) == "0" ? "user" : "admin";
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

                string sql = "SELECT * FROM TABLE(CaffeUser.UserInCaffeNS.GetUserInCaffeById(1," + userId + "))";

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
                    item.Cost = Math.Round(Convert.ToDouble(dr["Cost"]), 2);
                    item.IsDeleted = Convert.ToBoolean(dr["Isdeleted"]);
                }
            }

            return item;
        }

        internal static List<Order> orders = new List<Order>();
        public static string GetRevenue(DateTime? FromDate, DateTime? ToDate)
        {
            orders = getListOfOrdersForRevenue(FromDate, ToDate);
            return Convert.ToString(Math.Round(orders.Sum(x => x.Cost), 2));
        }

        //не сделан
        public static string GetCountOfOrdersPerDey()
        {
            return Convert.ToString(orders.Count());
        }

        public static void addUser(string name, string login, string password, DateTime? dateOfBirth, string phone, string status)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.UserInCaffeNS.InsertUserInCaffe";
            cmd.CommandType = CommandType.StoredProcedure;

            string created = DateTime.Now.ToShortDateString();

            DateTime dateTime = (DateTime)(dateOfBirth != null ? dateOfBirth : DateTime.Now);
            string birth = dateTime.ToShortDateString();

            cmd.Parameters.Add("Name", OracleDbType.Varchar2, 30).Value = name;
            cmd.Parameters.Add("Login", OracleDbType.Varchar2, 30).Value = login;
            cmd.Parameters.Add("Password", OracleDbType.Varchar2, 30).Value = password;
            cmd.Parameters.Add("Phone", OracleDbType.Varchar2, 30).Value = phone;
            cmd.Parameters.Add("Created", OracleDbType.Varchar2, 30).Value = created;
            cmd.Parameters.Add("DateOfBirth", OracleDbType.Varchar2, 30).Value = birth;
            cmd.Parameters.Add("Status", OracleDbType.Varchar2, 30).Value = status == "admin" ? "1" : "0";
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
            cmd.Parameters.Add("Created", OracleDbType.Varchar2, 30).Value = order.Created.ToShortDateString();
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

        public static void updateUser(int id, string name, string login, string password, string phone, DateTime? dateOfBirth, string status)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            DateTime dateTime = (DateTime)(dateOfBirth != null ? dateOfBirth : DateTime.Now);
            string birth = dateTime.ToShortDateString();
            string statusfortable = status == "admin" ? "1" : "0";

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.UserInCaffeNS.UpdateUserInCaffe";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("IdUser_t", OracleDbType.Int32, 30).Value = id;
            cmd.Parameters.Add("Name_t", OracleDbType.Varchar2, 30).Value = name;
            cmd.Parameters.Add("Login_t", OracleDbType.Varchar2, 30).Value = login;
            cmd.Parameters.Add("Password_t", OracleDbType.Varchar2, 30).Value = password;
            cmd.Parameters.Add("Phone_t", OracleDbType.Varchar2, 30).Value = phone;
            cmd.Parameters.Add("DateOfBirth_t", OracleDbType.Varchar2, 30).Value = birth;
            cmd.Parameters.Add("Status_t", OracleDbType.Varchar2, 30).Value = statusfortable;
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
            cmd.Parameters.Add("Discount_t", OracleDbType.Double, 30).Value = order.Discount;
            cmd.ExecuteNonQuery();
        }

        public static void updateItemInorder(int id, int count)
        {
            OracleConnection con = new OracleConnection();
            con.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "CaffeUser.ItemsInOrderNS.UpdateItemsInOrderOfCount";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("ItemsInOrder", OracleDbType.Int32, 30).Value = id;
            cmd.Parameters.Add("Count", OracleDbType.Int32, 30).Value = count;
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
            cmd.Parameters.Add("IDUSER", OracleDbType.Int32, 30).Value = userId;
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

        public static int getIdForOrder()
        {
            int count = 0;
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "select max(idorder) from orderincaffe";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    count = Convert.ToInt32(dr["max(idorder)"]);
                }
            }

            return count + 1;
        }

        public static int getIdForItemsInOrder()
        {
            int count = 0;
            using (OracleConnection oc = new OracleConnection())
            {
                oc.ConnectionString = "DATA SOURCE=localhost:1521/orcl;USER ID=CAFFEUSER;PASSWORD=secret";
                oc.Open();

                string sql = "select count(*) from itemsinorder";

                OracleDataAdapter oda = new OracleDataAdapter(sql, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    count = Convert.ToInt32(dr["count(*)"]);
                }
            }

            return count + 1;
        }

    }
}
