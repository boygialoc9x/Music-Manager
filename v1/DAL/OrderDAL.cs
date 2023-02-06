using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class OrderDAL
    {
        private MySqlConnection connection = DBHelper.GetConnection();
        private MySqlDataReader reader;
        private MySqlCommand cmd;
        private string command = null;

        public bool CreateOrder(Order order)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"insert into orders(userId, orderDate,total) values ({order.orderCustomer.userId}, \"{order.orderDate.ToString("yyyy-MM-dd")}\", {order.total});";
            try
            {
                cmd = new MySqlCommand(command, connection);
                int? re = null;
                if ( (re = cmd.ExecuteNonQuery()) > 0 )
                {
                    result = true;
                }
            }
            catch {
                result = false;
            }
            return result;
        }
        public int GetMaxIdInOrder()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            
            string command = @"SELECT MAX(orderId) FROM orders;";
            MySqlDataReader reader = (new MySqlCommand(command, connection)).ExecuteReader();
            int count = 0;

            try
            {
                if (reader.Read())
                {
                    count = reader.GetInt32("MAX(orderId)");
                }
            }
            catch
            {
            }

            reader.Close();
            connection.Close();
        
            return count;
        }
        public Order GetOrderDetail(int userId)
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }
            command = $"SELECT * FROM orders WHERE userId = {userId};";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            
            Order order = null;
        
            if (reader.Read())
            {
                order = GetOrder(reader);
            }
            reader.Close();
            connection.Close();

            return order;
        }
        private Order GetOrder(MySqlDataReader reader)
        {
            Order order = new Order();
            try
            {   
                order.orderId = reader.GetInt32("orderId");
                order.orderDate= reader.GetDateTime("orderDate");
                order.total = reader.GetDecimal("total");
                order.orderStatus = reader.GetBoolean("orderStatus");
            }
            catch {
               order = null;
            }
            return order;
        }
    }
}