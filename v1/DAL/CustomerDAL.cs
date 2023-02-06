using System;
using MySql.Data.MySqlClient;
using Persistence;
using Persistence.Enum;

namespace DAL
{
    public class CustomerDAL
    {
        private MySqlConnection connection = DBHelper.GetConnection();
        private MySqlDataReader reader;
        private MySqlCommand cmd;
        private string command = null;

        public bool CheckUserName(string user_name)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT user_name FROM customer WHERE user_name = '{user_name}';";

            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            bool result = reader.Read();
            reader.Close();
            connection.Close();
            
            return result;
        }
        public bool CheckPassword(string user_name, string password)
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }

            bool result = false;

            command = $"SELECT password FROM customer WHERE user_name = '{user_name}';";

            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            if (reader.Read())
            {
                string checkPassword = reader.GetString("password");
                if ( password == checkPassword )
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            reader.Close();
            connection.Close();
            return result;
        }
        public bool CheckPermission(string user_name, string perm)
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }
            command = $"SELECT {perm} FROM customer WHERE user_name = '{user_name}';";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            bool result = false;

            if (reader.Read())
            {
                result = reader.GetBoolean($"{perm}");
            }
            reader.Close();
            connection.Close();
            
            return result;
        }
        public Customer GetCustomer(string user_name)
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }
            command = $"SELECT * FROM customer WHERE user_name = '{user_name}';";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            
            Customer cus = null;

            if (reader.Read())
            {
                cus = p_GetCustomer(reader);
            }
            reader.Close();
            connection.Close();

            return cus;
        }
        public Customer GetCustomerbyID(int userId)
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }
            command = $"SELECT * FROM customer WHERE userId = '{userId}';";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            
            Customer cus = null;

            if (reader.Read())
            {
                cus = p_GetCustomer(reader);
            }
            reader.Close();
            connection.Close();

            return cus;
        }

        private Customer p_GetCustomer(MySqlDataReader reader)
        {
            Customer c = new Customer();
            try
            {   
                c.userId = reader.GetInt32("userId");
                c.user_name = reader.GetString("user_name");
                c.firstName = reader.GetString("firstName");
                c.lastName = reader.GetString("lastName");
                c.gmail = reader.GetString("gmail");
                c.password = reader.GetString("password");
                c.premium = reader.GetBoolean("premium");
                c.staff = reader.GetBoolean("staff");
                c.accountStatus = reader.GetBoolean("accountStatus");
                //c.gender = (Gender)Enum.Parse(typeof(Gender), reader.GetInt32("genderId").ToString()); 
                c.gender = (Gender)reader.GetInt32("genderId");
            }
            catch {
               c = null;
            }
            return c;
        }
        public bool UpgradePremium_Staff(string user_name, bool status)
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }
            bool result = false;
            command = $"UPDATE customer SET premium = {status} WHERE user_name = '{user_name}';";

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
        public bool UpdateCustomerInformation(Customer cus)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"UPDATE customer SET firstname = '{cus.firstName}', lastName = '{cus.lastName}', gmail = '{cus.gmail}' WHERE userId = {cus.userId};";
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
        public bool RegisterAccount(Customer customer)
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }
            bool result = false;
            //command = $"INSERT INTO customer VALUES ({customer.userId}, '{customer.user_name}' , '{customer.password}' ,{customer.premium} ,{customer.staff},'{customer.gmail}' ,{customer.accountStatus});";
            command = $"INSERT INTO customer (user_name, firstName, lastName, password, premium, staff, gmail) VALUES ('{customer.user_name}', '{customer.firstName}','{customer.lastName}','{customer.password}', {customer.premium}, {customer.staff}, '{customer.gmail}');";
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
        public bool CheckGmail(string gmail)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT gmail FROM customer WHERE gmail = '{gmail}';";
            //Console.WriteLine($"{command}");

            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            bool result = reader.Read();
            reader.Close();
            connection.Close();
            
            return result;
        }
        public bool CheckGmailByUserName(string gmail, string user_name)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT gmail FROM customer WHERE gmail = '{gmail}' AND user_name = '{user_name}';";
            //Console.WriteLine($"{command}");

            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            bool result = reader.Read();
            reader.Close();
            connection.Close();
            
            return result;
        }
        public bool UpdateNewPassword(int userId, string newPass)
        {
            //int userId = this.GetUserIdByUserName(user_name);
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            bool result = false;
            command = $"UPDATE customer SET password = '{newPass}' WHERE userId = {userId};";

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
        public int GetUserIdByUserName(string user_name)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT userId FROM customer WHERE user_name = '{user_name}';";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            int result = 0;

            if (reader.Read())
            {
                result = reader.GetInt32("userId");
            }
            reader.Close();
            connection.Close();
            
            return result;
        }
        public int GetMaxIdInCustomer()
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }
            command = @"SELECT MAX(userId) FROM customer;";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            
            int Maxid = 0;

            if (reader.Read())
            {
                Maxid = reader.GetInt32("MAX(userId)");
            }
            reader.Close();
            connection.Close();

            return Maxid;
        }
    }

}