using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Persistence;

namespace DAL
{
    public class GenresDAL
    {
        private MySqlConnection connection = DBHelper.GetConnection();
        private MySqlDataReader reader;
        private MySqlCommand cmd;
        private string command = null;

        public Genres GetGenreInfor(int genreId, string genreTitle)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            if (genreTitle == null)
            {
                command = @"SELECT * FROM genres WHERE genreId = " + "\"" + genreId + "\";"; 
            }
            else
            {
                command = $"SELECT * FROM genres WHERE genreTitle = '{genreTitle}';"; 
            }
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            Genres genre = null;

            if (reader.Read())
            {
                genre = GetGenre(reader);
            }
            reader.Close();
            connection.Close();
        
            return genre;
        }
        private Genres GetGenre(MySqlDataReader reader)
        {
            Genres genre = new Genres();
            genre.genreId = reader.GetInt32("genreId");
            genre.genreTitle = reader.GetString("genreTitle");
            genre.genreStatus = reader.GetBoolean("genreStatus");
            return genre;
        }
        public List<Genres> GetGenresList(bool status)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);
            command = $"SELECT * FROM genres WHERE genreStatus = {status}";
            query.CommandText = command;
            return GetGenres(query);
        }
        public List<Genres> GetGenresOfSong(int songId, bool status)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);
            command = $"SELECT g.genreId, g.genreTitle, g.genreStatus FROM songs s INNER JOIN genres_song gs ON s.songId = gs.songId INNER JOIN genres g ON gs.genreId = g.genreId WHERE s.songId = {songId} AND gs.genreSongStatus = {status};";
            query.CommandText = command;
            return GetGenres(query);
        }
        private List<Genres> GetGenres(MySqlCommand command)
        {
            List<Genres> genresList = new List<Genres>();

            reader = command.ExecuteReader();
            while (reader.Read())
            {
                genresList.Add(GetGenre(reader));
            }
            
            reader.Close();
            connection.Close();
            return genresList;
        }
        public int GetGenreIdByTitle(string genreTitle)
        {
            int genreId = 0;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT genreId FROM genres WHERE genreTitle = \"{genreTitle}\" ;";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            if (reader.Read())
            {
                genreId = reader.GetInt32("genreId");
            }
            reader.Close();
            connection.Close();
        
            return genreId;
        }

        public int GetMaxIdInGenres()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command = @"SELECT MAX(genreId) FROM genres;";

            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32("MAX(genreId)");
            }
            reader.Close();
            connection.Close();
        
            return count;
        }

//    ADD    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool AddNewGenre(Genres genre)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command = $"INSERT INTO genres (genreTitle) VALUES (\"{genre.genreTitle}\");";
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

//    CHECK    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool CheckGenreTitle(string genreTitle)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT genreTitle FROM genres WHERE genreTitle = \"{genreTitle}\" ;";
            //Console.WriteLine($"{command}");

            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            bool result = reader.Read();
            reader.Close();
            connection.Close();
            
            return result;
        }
    }
}