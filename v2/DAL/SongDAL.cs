using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Persistence;

namespace DAL
{
    public static class searchType
    {
        public const int GET_ALL = 0;
        public const int GET_BY_SONGNAME = 1;
        public const int GET_BY_ARTISTNAME = 2;
    };
    public static class getType
    {
        public const int LYRIC = 0;
        public const int DOWNLOADLINK = 1;
        public const int STATUS = 2;
    };
    public class SongDAL
    {
        private MySqlConnection connection = DBHelper.GetConnection();
        private MySqlDataReader reader;
        private MySqlCommand cmd;
        private string command = null;

//    GET    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int GetMaxIdInSongs()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            
            command = @"SELECT MAX(songId) FROM songs;";

            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32("MAX(songId)");
            }
            reader.Close();
            connection.Close();
        
            return count;
        }
        public Song GetSongInfor(int songId)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT * FROM songs WHERE songId = {songId};"; 
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            Song song = null;

            if (reader.Read())
            {
                song = GetSong(reader);
            }
            reader.Close();
            connection.Close();
        
            return song;
        }
        private Song GetSong(MySqlDataReader reader)
        {
            Song s = new Song();
            //CategoriesDAL cateDAL = new CategoriesDAL();
            
            s.songId = reader.GetInt32("songId");
            s.songName = reader.GetString("songName");
            s.length = reader.GetInt32("length");
            //s.lyric = reader.GetString("lyric");
            try{
                s.lyric = reader.GetString("lyric");
            }
            catch{
                s.lyric = null;
            }
            //s.aritst = reader.GetString("artist");
            try{
                s.downloadLink = reader.GetString("downloadLink");
            }
            catch{
                s.downloadLink = null;
            }
            //s.downloadLink = null;
            s.songStatus = reader.GetBoolean("songStatus");
            
            //s.Catogories = ;
            return s;
        }
        
        public string GetSomeSongInfor(int gType, int songId)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            string type = null;
            switch(gType)
            {
                case getType.LYRIC:
                    command = $"SELECT lyric FROM songs WHERE songId = {songId};"; 
                    type = "lyric";
                    break;
                case getType.DOWNLOADLINK:
                    command = $"SELECT downloadLink FROM songs WHERE songId = {songId};"; 
                    type = "downloadLink";
                    break;
            }
            reader = (new MySqlCommand(command, connection)).ExecuteReader();
            string getString = null;
            if (reader.Read())
            {
                getString = reader.GetString(type);
            }
            reader.Close();
            connection.Close();
            return getString;
        }
        public List<Song> GetSongList(int sType, string searchString)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);

            switch(sType)
            {
                case searchType.GET_ALL:
                    command = $"SELECT songId, songName, length, lyric, downloadLink, songStatus FROM songs;";
                    break;
                case searchType.GET_BY_SONGNAME:
                    command = $"SELECT songId, songName, length, lyric, downloadLink, songStatus FROM songs WHERE songName LIKE \'%{searchString}%\';";
                    break;
            }
            query.CommandText = command;
            return GetSongList(query);
        }
        public List<Song> GetSongListInPlaylist(int plId, bool status)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand query = new MySqlCommand("", connection);

            command = $"SELECT s.songName, s.length, s.songId, s.lyric, s.downloadLink, s.songStatus FROM playlist pl INNER JOIN playlist_song pls ON pl.playlistId = pls.playlistId INNER JOIN songs s ON pls.songId = s.songId WHERE pl.playlistId = {plId} AND pls.playlistSongStatus = {status};";
            query.CommandText = command;

            return this.GetSongList(query);
        }
        private List<Song> GetSongList(MySqlCommand command)
        {
            using (null)
            {
                List<Song> songList = new List<Song>();
                CategoriesDAL cateDAL = new CategoriesDAL();
                ArtistsDAL aDAL = new ArtistsDAL();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    songList.Add(GetSong(reader));
                }
                reader.Close();
                connection.Close();

                foreach(var val in songList)
                {
                    val.Catogories = cateDAL.GetCategoriesOfSong(val.songId, true);
                }
                foreach(var val in songList)
                {
                    val.Artists = aDAL.GetArtistsOfSong(val.songId, true);
                }
                // true mean active song
                
                return songList;
            }
        }
//    ADD    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool AddNewSong(Song nS)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

           // command = $"INSERT INTO songs VALUES ({nS.songId}, '{nS.songName}', {nS.length}, \"{nS.lyric}\", '{nS.aritst}', \"{nS.downloadLink}\", {nS.songStatus});";
            command = $"INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('{nS.songName}', {nS.length}, \"{nS.lyric}\", \"{nS.downloadLink}\",{nS.songStatus});";
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
        public bool AddCategoriesToSong(int songId, int cateId)
        {
            bool result = false;

            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command = $"INSERT INTO categories_song VALUES({cateId}, {songId}, {true});";

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

        public bool AddArtistsToSong(int songId, int artistId)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command = $"INSERT INTO artists_song VALUES({artistId}, {songId}, {true});";

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

//    UPDATE    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool UpdateSongInfor(Song s)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            //command = $"UPDATE songs SET songName = \'{s.songName}\', length = {s.length}, artist = \'{s.aritst}\' WHERE songId = {s.songId}";
            command = $"UPDATE songs SET songName = \'{s.songName}\', length = {s.length} WHERE songId = {s.songId};";
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
        public bool UpdateCategoriesToSong(int songId, int cateId) //not use
        {
            bool result = false;
            
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"INSERT INTO categories_song (categoryId ,songId , categorySongStatus) VALUES({cateId}, {songId}, {true});";

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
        public void UpdateAllCategoriesOfSongBeforeToFalse(int songId) // no need to test
        {
            
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command= $"Update categories_song SET categorySongStatus = {false} WHERE songId = {songId}";

            cmd = new MySqlCommand(command, connection);
            cmd.ExecuteNonQuery();
        }

        public void UpdateAllArtitssOfSongBeforeToFalse(int songId) // no need to test
        {
            
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command= $"Update artists_song SET artistSongStatus = {false} WHERE songId = {songId}";

            cmd = new MySqlCommand(command, connection);
            cmd.ExecuteNonQuery();
        }

        public bool ReActiveCategoryOfSong(int songId, int cateId) // no need to test
        {
            //CategoriesDAL cateDAL = new CategoriesDAL();
            //int cateId = cateDAL.GetCategoryIdByName(cateName);
            bool result = false;

            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command= $"Update categories_song SET categorySongStatus = {true} WHERE songId = {songId} AND categoryId = {cateId}";

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

        public bool ReActiveArtistOfSong(int songId, int artistId) // no need to test
        {
            bool result = false;

            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command= $"Update artists_song SET artistSongStatus = {true} WHERE songId = {songId} AND artistId = {artistId}";

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
        public bool UpdateSomeSongInfor(int gType, int songId, string updateStr)
        {
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            switch(gType)
            {
                case getType.LYRIC:
                    command = $"UPDATE songs SET lyric = \"{updateStr}\" WHERE songId = {songId}";
                    break;
                case getType.DOWNLOADLINK:
                    command = $"UPDATE songs SET downloadLink = \"{updateStr}\" WHERE songId = {songId}";
                    break;
                case getType.STATUS:
                    command = $"UPDATE songs SET songStatus = {bool.Parse(updateStr)} WHERE songId = {songId}";
                    break;
            }
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
//       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }

}