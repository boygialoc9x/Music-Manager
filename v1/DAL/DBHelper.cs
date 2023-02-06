using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.IO;
using DAL.Encrypt;
using ConsolePL.UTIL;

namespace DAL
{
    // Using Singleton Design Pattern
    public class DBHelper
    {
        private static MySqlConnection connection;
        private static string fileName = "ConnectionString.txt";

        public static MySqlConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection
                {
                    ConnectionString = read_conString(fileName)
                };
            }
            return connection;
        }
        public static MySqlDataReader ExecQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteReader();
        }
        public static MySqlConnection OpenConnection()
        {
            if (connection == null)
            {
                GetConnection();
            }
            connection.Open();
            return connection;
        }
        public static void CloseConnection()
        {
            if (connection != null) connection.Close();
        }
        public static bool ExecTransaction(List<string> queries)
        {
            bool result = true;
            OpenConnection();
            MySqlCommand command = connection.CreateCommand();
            MySqlTransaction trans = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = trans;

            try
            {
                foreach (var query in queries)
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                    trans.Commit();
                }
                result = true;
            }
            catch
            {
                result = false;
                try
                {
                    trans.Rollback();
                }
                catch
                {
                }
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }
        private static string read_conString(string fileName)
        {
            string conString = null;
            string format = "";
            string conStringFileURL = "https://drive.google.com/uc?export=download&id=17zNH_zcSAobHXs4MSgMbIwxIYj3XpNYT";
            try
            {
                conString = JsonConvert.DeserializeObject<string>(File.ReadAllText(fileName));
                conString = EncryptTextTool.DecryptText(conString);
            }
            catch
            {
                AtomicDownload.DownloadFile(conStringFileURL, fileName, format);
                conString = read_conString(fileName);
            }
            return conString;
        }
    }
}
