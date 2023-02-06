using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Persistence;
using Persistence.Enum;

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
        public List<Artists> GetArtistsInforListByBandName(string bandName)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);
            command = $"SELECT * FROM artists WHERE bandName = '{bandName}';"; 
            query.CommandText = command;

            return GetArtistsList(query);
        }
        private Artists GetArtist(MySqlDataReader reader)
        {
            Artists artist = new Artists();
            
            artist.artistId = reader.GetInt32("artistId");
            artist.artistFirstName = reader.GetString("artistFirstName");
            artist.artistLastName = reader.GetString("artistLastName");
            artist.born = reader.GetDateTime("born");
            artist.gender = (Gender)reader.GetInt32("genderId");

            artist.bandName = reader.GetString("bandName");
            artist.stageName = reader.GetString("stageName");
            artist.producerAlias = reader.GetString("producerAlias");
            artist. songwriterAlias = reader.GetString("songwriterAlias");
            
            artist.isBand = reader.GetBoolean("isBand");
            artist.isProducer = reader.GetBoolean("isProducer");
            artist.isSinger = reader.GetBoolean("isSinger");
            artist.isWriter = reader.GetBoolean("isWriter");

            artist.artistStatus = reader.GetBoolean("artistStatus");

            return artist;
        }
        public List<Artists> GetArtistOfSong(int songId, bool status, Enum aType)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);
            command = $"SELECT a.artistId ,a.artistFirstName, a.artistLastName, a.born, a.genderId, a.isBand, a.bandName, a.stageName, a.producerAlias, a.songwriterAlias, a.isSinger, a.isWriter, a.isProducer, a.artistStatus FROM songs s INNER JOIN artists_song ats ON s.songId = ats.songId INNER JOIN artists a ON a.artistId = ats.artistId WHERE s.songId = {songId} ";
            switch (aType)
            {
                case Persistence.Enum.ArtistType.Band:
                    command += $"AND ats.bandSongStatus = {status};";
                    break;
                case Persistence.Enum.ArtistType.Singer:
                    command += $"AND ats.singerSongStatus = {status};";
                    break;
                case Persistence.Enum.ArtistType.Writer:
                    command += $"AND ats.writerSongStatus = {status};";
                    break;
                case Persistence.Enum.ArtistType.Producer:
                    command += $"AND ats.producedSongStatus = {status};";
                    break;
            }
            
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
            command = $"INSERT INTO artists (artistFirstName, artistLastName, genderId, born, isBand, bandName, isSinger, stageName, isProducer, producerAlias, isWriter, songwriterAlias) VALUES (\"{a.artistFirstName}\", \"{a.artistLastName}\", {((int)a.gender).ToString()}, \"{a.born.ToString("yyyy-MM-dd")}\", {a.isBand}, \"{a.bandName}\", {a.isSinger}, \"{a.stageName}\", {a.isProducer}, \"{a.producerAlias}\", {a.isWriter}, \"{a.songwriterAlias}\");";
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
                Console.Write(e);
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
            command = $"UPDATE artists SET artistFirstName = \"{a.artistFirstName}\", artistLastName = \"{a.artistLastName}\", isSinger = {a.isSinger}, stageName = \"{a.stageName}\",bandName = \"{a.bandName}\", isBand = {a.isBand}, isProducer = {a.isProducer}, producerAlias = \"{a.producerAlias}\", isWriter = {a.isWriter}, songwriterAlias = \"{a.songwriterAlias}\", genderId = {(int)a.gender} ,born = \"{a.born.ToString("yyyy-MM-dd")}\" WHERE artistId = {a.artistId};";
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
                Console.Write(e);
                result = false;
            }
            return result;
        }

        public List<Artists> GetListOfArtists(Enum aType, bool status) //*
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);
            command = $"SELECT * FROM artists ";
            switch (aType)
            {
                case Persistence.Enum.ArtistType.Band:
                    command += $"WHERE isBand = {status} AND artistStatus = {true};";
                    break;
                case Persistence.Enum.ArtistType.Singer:
                    command += $"WHERE isSinger = {status} AND artistStatus = {true};";
                    break;
                case Persistence.Enum.ArtistType.Writer:
                    command += $"WHERE isWriter = {status} AND artistStatus = {true};";
                    break;
                case Persistence.Enum.ArtistType.Producer:
                    command += $"WHERE isProducer = {status} AND artistStatus = {true};";
                    break;
            }
            query.CommandText = command;
            return this.GetArtistsList(query);
        }
    }
}