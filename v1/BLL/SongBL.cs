using System;
using System.Collections.Generic;
using Persistence;
using DAL;
//using chooseType;

namespace BL
{
    public class SongBL
    {
        private SongDAL songDAL = new SongDAL();

//////////////////////////////////////////////////////////////////////////////////////////////////
/*
        GET NUMBER OF PAGES ZONE
*/
        // public int get_NumberOfSongPages(int sType, string searchString)
        // {   
        //     int numberOfelements = this.get_NumberOfSongsElement(sType, searchString);
        //     int numberOfelements_PerPage = 10;

        //     int numberOfpage = numberOfelements / numberOfelements_PerPage;

        //     if ( numberOfelements % numberOfelements_PerPage > 0)
        //     {
        //         numberOfpage ++;
        //     }
            
        //     return numberOfpage;
        // }
        // public int get_NumberOfSongsElement(int sType, string searchStr)
        // {
        //     return songDAL.get_NumberOfSongsElement(sType, searchStr);
        // }

        public int GetMaxIdInSongs()
        {
            return songDAL.GetMaxIdInSongs();
        }
        
        // public int get_NumberOfSearchSongNameElement(string searchStr)
        // {
        //     return songDAL.get_NumberOfSongsElement(searchType.GET_BY_SONGNAME, searchStr);
        // }
        // public int get_NumberOfSearchSongArtistElement(string searchStr)
        // {
        //     return songDAL.get_NumberOfSongsElement(searchType.GET_BY_ARTISTNAME, searchStr);
        // }

        // public int get_NumberOfAllSongPages()
        // {
        //     string searchStr = null;
        //     return get_NumberOfSongPages(searchType.GET_ALL, searchStr);
        // }

        // public int get_NumberOfSearchSongByNamePages(string songName)
        // {
        //     return get_NumberOfSongPages(searchType.GET_BY_SONGNAME, songName);
        // }

        // public int get_NumberOfSearchSongBArtistPages(string artistName)
        // {
        //     return get_NumberOfSongPages(searchType.GET_BY_ARTISTNAME, artistName);
        // }

//////////////////////////////////////////////////////////////////////////////////////////////////
/*
        GET SONG INFORMATION ZONE
*/

        public Song GetSongInfor(int songId)
        {   
            return songDAL.GetSongInfor(songId);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
/*
        GET LIST OF SONG ZONE
*/

        public List<Song> GetAllSongsList()
        {
            string str = null; // this is an useless var. But we need it :>
            return songDAL.GetSongList(searchType.GET_ALL, str);
        }

        public List<Song> GetSearchSongByNameList(string songName)
        {
            return songDAL.GetSongList(searchType.GET_BY_SONGNAME, songName);
        }

        public List<Song> GetSearchSongByArtistList(string artistName)
        {
            return songDAL.GetSongList(searchType.GET_BY_ARTISTNAME, artistName);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
/*
        GET SOME SONG INFORMATION ( LYRIC OR DOWNLAOD ) LINK ZONE
*/
        public string GetSongLyric(int songId)
        {
            return songDAL.GetSomeSongInfor(getType.LYRIC,songId);
        }
        public string GetSognDownloadLink(int songId)
        {
            return songDAL.GetSomeSongInfor(getType.DOWNLOADLINK,songId);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
/*
        ADD NEW SONG ZONE
*/
        public bool AddNewSong(Song nS)
        {
            return songDAL.AddNewSong(nS);
        }
        public bool AddGenresToSong(int songId, int genreId)
        {
            return songDAL.AddGenresToSong(songId, genreId);
        }
        public bool AddBandToSong(int songId, int artistId)
        {
            return songDAL.AddArtistsToSong(songId, artistId, Persistence.Enum.ArtistType.Band);
        }
        public bool AddSingerToSong(int songId, int artistId)
        {
            return songDAL.AddArtistsToSong(songId, artistId, Persistence.Enum.ArtistType.Singer);
        }
        public bool AddWriterToSong(int songId, int artistId)
        {
            return songDAL.AddArtistsToSong(songId, artistId, Persistence.Enum.ArtistType.Writer);
        }
        public bool AddProducerToSong(int songId, int artistId)
        {
            return songDAL.AddArtistsToSong(songId, artistId, Persistence.Enum.ArtistType.Producer);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
/*
        UPDATE ZONE
*/
        public bool UpdateSongInfor(Song s)
        {
            return songDAL.UpdateSongInfor(s);
        }

        public bool UpdateSongLyric(int songId, string updateStr)
        {
            return songDAL.UpdateSomeSongInfor(getType.LYRIC, songId, updateStr);
        }
        public bool UpdateSongDownloadLink(int songId, string updateStr)
        {
            return songDAL.UpdateSomeSongInfor(getType.DOWNLOADLINK, songId, updateStr);
        }
        public bool UpdateSongStatus(int songId, bool updateStr)
        {
            return songDAL.UpdateSomeSongInfor(getType.STATUS, songId, updateStr.ToString());
        }
        public bool UpdateGenresToSong(int songId, int genreId)
        {
            return songDAL.UpdateGenresToSong(songId, genreId);
        }
        public void UpdateAllGenresOfSongBeforeToFalse(int songId)
        {
            songDAL.UpdateAllGenresOfSongBeforeToFalse(songId);
        }
        public void UpdateAllProducersOfSongBeforeToFalse(int songId)
        {
            songDAL.UpdateAllArtitssOfSongBeforeToFalse(songId, Persistence.Enum.ArtistType.Producer);
        }
        public void UpdateAllSingersOfSongBeforeToFalse(int songId)
        {
            songDAL.UpdateAllArtitssOfSongBeforeToFalse(songId, Persistence.Enum.ArtistType.Singer);
        }
        public void UpdateAllBandsOfSongBeforeToFalse(int songId)
        {
            songDAL.UpdateAllArtitssOfSongBeforeToFalse(songId, Persistence.Enum.ArtistType.Band);
        }
        public void UpdateAllWritersOfSongBeforeToFalse(int songId)
        {
            songDAL.UpdateAllArtitssOfSongBeforeToFalse(songId, Persistence.Enum.ArtistType.Writer);
        }
        public bool ReActiveGenreOfSong(int songId, int genreId)
        {
            return songDAL.ReActiveGenreOfSong(songId, genreId);
        }
        public bool ReActiveWriterOfSong(int songId, int artistId)
        {
            return songDAL.ReActiveArtistOfSong(songId, artistId, Persistence.Enum.ArtistType.Writer);
        }
        public bool ReActiveBandsOfSong(int songId, int artistId)
        {
            return songDAL.ReActiveArtistOfSong(songId, artistId, Persistence.Enum.ArtistType.Band);
        }
        public bool ReActiveSingerOfSong(int songId, int artistId)
        {
            return songDAL.ReActiveArtistOfSong(songId, artistId, Persistence.Enum.ArtistType.Singer);
        }
        public bool ReActiveProducerOfSong(int songId, int artistId)
        {
            return songDAL.ReActiveArtistOfSong(songId, artistId, Persistence.Enum.ArtistType.Producer);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////

    }
}