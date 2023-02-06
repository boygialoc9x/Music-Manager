using System;
using MySql.Data.MySqlClient;
using Persistence;
using System.Collections.Generic;
namespace DAL
{
    public static class checkType
    {
        public const int CHECK_EXIST = 0;
        public const int CHECK_STATUS = 1;
    };
    public class PlaylistDAL
    {
        private MySqlConnection connection = DBHelper.GetConnection();
        private MySqlDataReader reader;
        private MySqlCommand cmd;
        private string command = null;

        public bool CreateANewPlaylist(Playlist pl)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"CALL insert_playlist_and_update_playlistCreated('{pl.playlistTitle}', {pl.userId}, '{pl.createDate.ToString("yyyy-MM-dd")}');";
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
        public int GetMaxIdInPlaylist()
        {
            if(connection.State == System.Data.ConnectionState.Closed){
                connection.Open();
            }
            command = @"SELECT MAX(userId) FROM playlist;";
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
        public bool UpdateSongInPlaylist(int plId, int songId, bool status)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"UPDATE playlist_song SET playlistSongStatus = {status} WHERE playlistId = {plId} AND songId = {songId};";
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
        public bool UpdatePlaylistTitle(Playlist pl)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"UPDATE playlist SET playlistTitle = '{pl.playlistTitle}' WHERE playlistId = {pl.playlistId} AND userId = {pl.userId};";
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
        public bool UpdateThisPlaylistStatusTo_False(Playlist pl)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"CALL remove_allSongs_and_playlist_and_update_playlistCreated({pl.playlistId}, {pl.userId});";
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
        public bool RecycleRemovedPlaylist(Playlist pl)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"CALL recycle_removedPlaylist({pl.playlistId}, {pl.userId}, '{pl.playlistTitle}', '{pl.createDate.ToString("yyyy-MM-dd")}');";
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
        public bool AddSongToPlaylist(int plId, int songId)
        {
            bool result = false;
            
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command = $"INSERT INTO playlist_song VALUES({plId}, {songId}, {true});";

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
        public List<Playlist> GetPlaylistList(int userId, bool status)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);

            command = $"SELECT * FROM playlist WHERE userId = {userId} AND playlistStatus = {status};";
            query.CommandText = command;

            return GetplaylistList(query);
        }
        private List<Playlist> GetplaylistList(MySqlCommand command)
        {
            using (null)
            {
                List<Playlist> PlaylistList = new List<Playlist>();
                SongDAL sDAL = new SongDAL();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PlaylistList.Add(GetPlaylist(reader));
                }

                reader.Close();
                connection.Close();

                foreach(var val in PlaylistList)
                {
                    val.playlistSongs = sDAL.GetSongListInPlaylist(val.playlistId, true);
                }
                
                return PlaylistList;
            }
        }
        private Playlist GetPlaylist(MySqlDataReader reader)
        {
            Playlist pl = new Playlist();
            pl.playlistId = reader.GetInt32("playlistId");
            pl.playlistTitle = reader.GetString("playlistTitle");
            pl.playlistStatus = reader.GetBoolean("playlistStatus");
            pl.createDate = reader.GetDateTime("createDate");
            pl.userId = reader.GetInt32("userId");
            return pl;
        }
        public bool CheckSongExistOrItsStatusInPlaylist(int plId, int songId, int cType)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT playlistSongStatus FROM playlist_song WHERE playlistId = {plId} AND songId = {songId};";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            switch(cType)
            {
                case checkType.CHECK_EXIST:
                result = reader.Read();
                break;

                case checkType.CHECK_STATUS:
                if (reader.Read())
                result = reader.GetBoolean("playlistSongStatus");
                break;
            }
            //Console.WriteLine(result.ToString());
            reader.Close();
            connection.Close();
            
            return result;
        }
        public bool AutoUpdateAllActiveSongBeforeToFalseAfterItsStatusChanged(int songId)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command = $"UPDATE playlist_song SET playlistSongStatus = {false} WHERE songId = {songId};";

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