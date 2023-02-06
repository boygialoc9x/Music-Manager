using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Persistence;

namespace DAL
{
    public class CategoriesDAL
    {
        private MySqlConnection connection = DBHelper.GetConnection();
        private MySqlDataReader reader;
        private MySqlCommand cmd;
        private string command = null;

        public Categories GetCategoryInfor(int categoryId, string categoryName)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            if (categoryName == null)
            {
                command = @"SELECT * FROM categories WHERE categoryId = " + "\"" + categoryId + "\";"; 
            }
            else
            {
                command = $"SELECT * FROM categories WHERE categoryName = '{categoryName}';"; 
            }
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            Categories cate = null;

            if (reader.Read())
            {
                cate = GetCategory(reader);
            }
            reader.Close();
            connection.Close();
        
            return cate;
        }
        private Categories GetCategory(MySqlDataReader reader)
        {
            Categories cate = new Categories();
            cate.categoryId = reader.GetInt32("categoryId");
            cate.categoryName = reader.GetString("categoryName");
            cate.categoryStatus = true;
            return cate;
        }
        public List<Categories> GetCategoriesList()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);
            command = $"SELECT categoryId, categoryName, categoryStatus FROM categories";
            query.CommandText = command;
            return GetCategories(query);
        }
        public List<Categories> GetCategoriesOfSong(int songId, bool status)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);
            command = $"SELECT c.categoryId, c.categoryName, c.categoryStatus FROM songs s INNER JOIN categories_song cs ON s.songId = cs.songId INNER JOIN categories c ON cs.categoryId = c.categoryId WHERE s.songId = {songId} AND cs.categorySongStatus = {status};";
            query.CommandText = command;
            return GetCategories(query);
        }
        private List<Categories> GetCategories(MySqlCommand command)
        {
            List<Categories> categoriesList = new List<Categories>();

            reader = command.ExecuteReader();
            while (reader.Read())
            {
                categoriesList.Add(GetCategory(reader));
            }
            
            reader.Close();
            connection.Close();
            return categoriesList;
        }
        public int GetCategoryIdByName(string categoryName)
        {
            int cateId = 0;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT categoryId FROM categories WHERE categoryName = '{categoryName}' ;";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            if (reader.Read())
            {
                cateId = reader.GetInt32("categoryId");
            }
            reader.Close();
            connection.Close();
        
            return cateId;
        }

        public int GetMaxIdInCategories()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command = @"SELECT MAX(categoryId) FROM categories;";

            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32("MAX(categoryId)");
            }
            reader.Close();
            connection.Close();
        
            return count;
        }

//    ADD    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool AddNewCategory(Categories cate)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            //command = $"INSERT INTO categories VALUES ({cate.categoryId}, '{cate.categoryName}');";
            command = $"INSERT INTO categories (categoryName) VALUES ('{cate.categoryName}');";
            try
            {
                cmd = new MySqlCommand(command, connection);
                int? re = null;
                if ( (re = cmd.ExecuteNonQuery()) > 0 )
                {
                    result = true;
                }
            }
            catch (Exception e){
                Console.WriteLine(e);
                result = false;
            }
            return result;
        }

//    CHECK    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool CheckCategoryName(string categoryName)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT categoryName FROM categories WHERE categoryName = '{categoryName}' ;";
            //Console.WriteLine($"{command}");

            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            bool result = reader.Read();
            reader.Close();
            connection.Close();
            
            return result;
        }
    }
}