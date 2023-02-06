using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Persistence;

namespace DAL
{
    public class ArtistsDAL
    {
        private MySqlConnection connection = DBHelper.GetConnection();
        private MySqlDataReader reader;
        private MySqlCommand cmd;
        private string command = null;

        public Artists GetArtistInforById(int artistId)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command = $"SELECT * FROM artists WHERE artistId = {artistId};"; 
            
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            Artists artist = null;

            if (reader.Read())
            {
                artist = GetArtist(reader);
            }
            reader.Close();
            connection.Close();
        
            return artist;
        }
        public List<Artists> GetArtistsInforListBytheArtist(string theArtist)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);
            command = $"SELECT * FROM artists WHERE theArtist = '{theArtist}';"; 
            query.CommandText = command;

            return GetArtistsList(query);
        }
        private Artists GetArtist(MySqlDataReader reader)
        {
            Artists artist = new Artists();
            
            artist.artistId = reader.GetInt32("artistId");
            artist.artistFirstName = reader.GetString("artistFirstName");
            artist.artistLastName = reader.GetString("artistLastName");
            artist.theArtist = reader.GetString("theArtist");
            artist.born = reader.GetDateTime("born");
            artist.artistStatus = reader.GetBoolean("artistStatus");

            return artist;
        }
        public List<Artists> GetArtistsOfSong(int songId, bool status)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);
            command = $"SELECT a.artistId, a.theArtist, a.artistFirstName, a.artistLastName, a.born, a.artistStatus FROM songs s INNER JOIN artists_song ats ON s.songId = ats.songId INNER JOIN artists a ON a.artistId = ats.artistId WHERE s.songId = {songId} AND ats.artistSongStatus = {status};";
            query.CommandText = command;

            return GetArtistsList(query);
        }
        private List<Artists> GetArtistsList(MySqlCommand command)
        {
            List<Artists> artistsList = new List<Artists>();

            reader = command.ExecuteReader();
            while (reader.Read())
            {
                artistsList.Add(GetArtist(reader));
            }
            reader.Close();
            connection.Close();

            return artistsList;
        }
        public bool ChecktheArtist(string theArtist)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT theArtist FROM artists WHERE theArtist = '{theArtist}';";
            //Console.WriteLine($"{command}");

            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            bool result = reader.Read();
            reader.Close();
            connection.Close();
            
            return result;
        }

        public int GetMaxIdInArtist()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command = $"SELECT MAX(artistId) FROM artists;";

            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32("MAX(artistId)");
            }
            reader.Close();
            connection.Close();
        
            return count;
        }

        public bool AddNewArtist(Artists a)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"INSERT INTO artists (artistFirstName, artistLastName, theArtist, born) VALUES ('{a.artistFirstName}', '{a.artistLastName}', '{a.theArtist}', '{a.born.ToString("yyyy-MM-dd")}');";
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

        public bool UpdateArtistInfor(Artists a)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"UPDATE artists SET artistFirstName = '{a.artistFirstName}', artistLastName = '{a.artistLastName}', theArtist = '{a.theArtist}', born = '{a.born.ToString("yyyy-MM-dd")}' WHERE artistId = {a.artistId};";
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

    }
}