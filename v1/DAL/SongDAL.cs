using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Persistence;
using System.Linq;

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
            s.songId = reader.GetInt32("songId");
            s.songName = reader.GetString("songName");
            s.length = reader.GetInt32("length");
            s.album = reader.GetString("album");
            s.copyright = reader.GetString("coppyright");
            s.releaseDate = reader.GetDateTime("releaseDate");
            try{
                s.lyric = reader.GetString("lyric");
            }
            catch{
                s.lyric = null;
            }
            try{
                s.downloadLink = reader.GetString("downloadLink");
            }
            catch{
                s.downloadLink = null;
            }
            s.songStatus = reader.GetBoolean("songStatus");
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
                    command = $"SELECT * FROM songs;";
                    break;
                case searchType.GET_BY_SONGNAME:
                    command = $"SELECT * FROM songs WHERE songName LIKE \"%{searchString}%\";";
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

            command = $"SELECT s.songName, s.length, s.songId, s.lyric, s.downloadLink, s.songStatus, s.releaseDate, s.album, s.coppyright FROM playlist pl INNER JOIN playlist_song pls ON pl.playlistId = pls.playlistId INNER JOIN songs s ON pls.songId = s.songId WHERE pl.playlistId = {plId} AND pls.playlistSongStatus = {status};";
            query.CommandText = command;

            return this.GetSongList(query);
        }
        private List<Song> GetSongList(MySqlCommand command)
        {
            using (null)
            {
                List<Song> songList = new List<Song>();
                GenresDAL genreDAL = new GenresDAL();
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
                    val.genres = genreDAL.GetGenresOfSong(val.songId, true);
                }
                foreach(var val in songList)
                {
                    val.singer = aDAL.GetArtistOfSong(val.songId, true, Persistence.Enum.ArtistType.Singer); //Get Singer
                    val.band = aDAL.GetArtistOfSong(val.songId, true, Persistence.Enum.ArtistType.Band); //Get Band
                    val.writer = aDAL.GetArtistOfSong(val.songId, true, Persistence.Enum.ArtistType.Writer);
                    val.produced = aDAL.GetArtistOfSong(val.songId, true, Persistence.Enum.ArtistType.Producer);
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
            command = $"INSERT INTO songs (songName, length, lyric, downloadLink, songStatus, releaseDate, album, coppyright) VALUES (\"{nS.songName}\", {nS.length}, \"{nS.lyric}\", \"{nS.downloadLink}\",{nS.songStatus}, \"{nS.releaseDate.ToString("yyyy-MM-dd")}\", \"{nS.album}\", \"{nS.copyright}\");";
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
        public bool AddGenresToSong(int songId, int genId)
        {
            bool checkGenreExistedInSong = this.CheckIfThisGenreWereInTheSongAlready(genId, songId);
            bool result = false;

            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            if (checkGenreExistedInSong) command = $"UPDATE genres_song SET genreSongStatus = {true} WHERE genreId = {genId} AND songId = {songId};";
            else command = $"INSERT INTO genres_song VALUES({genId}, {songId}, {true});";

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

        public bool AddArtistsToSong(int songId, int artistId, Enum aType) //*
        {
            bool checkArtistExistedInsong = this.CheckIfThisArtistWereInTheSongAlready(artistId, songId);
            //Console.WriteLine(checkArtistExistedInsong.ToString());
            bool result = false;
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            switch (aType)
            {
                case Persistence.Enum.ArtistType.Band:
                    if (checkArtistExistedInsong) command = $"UPDATE artists_song SET bandSongStatus = {true} WHERE artistId = {artistId} AND songId = {songId};";
                    else command = $"INSERT INTO artists_song(artistId, songId, bandSongStatus) VALUES({artistId}, {songId}, {true});";
                    break;
                case Persistence.Enum.ArtistType.Singer:
                    if (checkArtistExistedInsong) command = $"UPDATE artists_song SET singerSongStatus = {true} WHERE artistId = {artistId} AND songId = {songId};";
                    else command = $"INSERT INTO artists_song(artistId, songId, singerSongStatus) VALUES({artistId}, {songId}, {true});";
                    break;
                case Persistence.Enum.ArtistType.Writer:
                    if (checkArtistExistedInsong) command = $"UPDATE artists_song SET writerSongStatus = {true} WHERE artistId = {artistId} AND songId = {songId};";
                    else command = $"INSERT INTO artists_song(artistId, songId, writerSongStatus) VALUES({artistId}, {songId}, {true});";
                    break;
                case Persistence.Enum.ArtistType.Producer:
                if (checkArtistExistedInsong) command = $"UPDATE artists_song SET producedSongStatus = {true} WHERE artistId = {artistId} AND songId = {songId};";
                    else command = $"INSERT INTO artists_song(artistId, songId, producedSongStatus) VALUES({artistId}, {songId}, {true});";
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
                result = false;
                Console.WriteLine(e);
            }
            return result;
        }
//     CHECK //
        public bool CheckIfThisArtistWereInTheSongAlready(int artistId, int songId) //*
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT * FROM artists_song WHERE artistId = {artistId} AND songId = {songId};";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            bool result = reader.Read();
            //Console.WriteLine(result.ToString());
            reader.Close();
            connection.Close();
            
            return result;
        }
        public bool CheckIfThisGenreWereInTheSongAlready(int genreId, int songId) //*
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"SELECT * FROM genres_song WHERE genreId = {genreId} AND songId = {songId};";
            reader = (new MySqlCommand(command, connection)).ExecuteReader();

            bool result = reader.Read();
            //Console.WriteLine(result.ToString());
            reader.Close();
            connection.Close();
            
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
            command = $"UPDATE songs SET songName = \"{s.songName}\", length = {s.length}, releaseDate = \"{s.releaseDate.ToString("yyyy-MM-dd")}\", album = \"{s.album}\", coppyright = \"{s.copyright}\" WHERE songId = {s.songId};";
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
        public bool UpdateGenresToSong(int songId, int genreId) //not use
        {
            bool result = false;
            
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            command = $"INSERT INTO genres_song (genreId ,songId , genreSongStatus) VALUES({genreId}, {songId}, {true});";

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
        public void UpdateAllGenresOfSongBeforeToFalse(int songId) // no need to test
        {
            
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command= $"Update genres_song SET genreSongStatus = {false} WHERE songId = {songId}";

            cmd = new MySqlCommand(command, connection);
            cmd.ExecuteNonQuery();
        }

        public void UpdateAllArtitssOfSongBeforeToFalse(int songId, Enum aType) // no need to test
        {
            
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            switch (aType)
            {
                case Persistence.Enum.ArtistType.Band:
                    command = $"Update artists_song SET bandSongStatus = {false} WHERE songId = {songId}";
                    break;
                case Persistence.Enum.ArtistType.Singer:
                    command = $"Update artists_song SET singerSongStatus = {false} WHERE songId = {songId}";
                    break;
                case Persistence.Enum.ArtistType.Writer:
                    command = $"Update artists_song SET writerSongStatus = {false} WHERE songId = {songId}";
                    break;
                case Persistence.Enum.ArtistType.Producer:
                    command = $"Update artists_song SET producedSongStatus = {false} WHERE songId = {songId}";
                    break;
            }

            cmd = new MySqlCommand(command, connection);
            cmd.ExecuteNonQuery();
        }

        public bool ReActiveGenreOfSong(int songId, int genreId) // no need to test
        {
            bool result = false;

            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            command= $"Update genres_song SET genreSongStatus = {true} WHERE songId = {songId} AND genreId = {genreId}";

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

        public bool ReActiveArtistOfSong(int songId, int artistId, Enum aType) // no need to test
        {
            bool result = false;

            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            switch (aType)
            {
                case Persistence.Enum.ArtistType.Band:
                    command= $"Update artists_song SET bandSongStatus = {true} WHERE songId = {songId} AND artistId = {artistId}";
                    break;
                case Persistence.Enum.ArtistType.Singer:
                    command= $"Update artists_song SET singerSongStatus = {true} WHERE songId = {songId} AND artistId = {artistId}";
                    break;
                case Persistence.Enum.ArtistType.Writer:
                    command= $"Update artists_song SET writerSongStatus = {true} WHERE songId = {songId} AND artistId = {artistId}";
                    break;
                case Persistence.Enum.ArtistType.Producer:
                    command= $"Update artists_song SET producedSongStatus = {true} WHERE songId = {songId} AND artistId = {artistId}";
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
            catch {
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
            catch {
                result = false;
            }
            return result;
        }
//       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }

}