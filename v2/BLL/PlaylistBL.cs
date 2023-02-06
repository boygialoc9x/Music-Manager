using System;
using System.Collections.Generic;
using Persistence;
using DAL;
namespace BL
{
    public class PlaylistBL
    {
        private PlaylistDAL plDAL = new PlaylistDAL();
        public bool CreateANewPlaylist(Playlist pl)
        {
            return plDAL.CreateANewPlaylist(pl);
        }
        public bool UpdateSongStatusInPlaylistTo_False(int plId, int songId)
        {
            bool status = false;
            return plDAL.UpdateSongInPlaylist(plId, songId, status);
        }
        public bool UpdateSongStatusInPlaylistTo_True(int plId, int songId)
        {
            bool status = true;
            return plDAL.UpdateSongInPlaylist(plId, songId, status);
        }
        public int GetMaxIdInPlaylist()
        {
            return plDAL.GetMaxIdInPlaylist();
        }
        public bool UpdatePlaylistTitle(Playlist pl)
        {
            return plDAL.UpdatePlaylistTitle(pl);
        }
        public bool UpdateThisPlaylistStatusTo_False(Playlist pl)
        {
            return plDAL.UpdateThisPlaylistStatusTo_False(pl);
        }
        public bool AddSongToPlaylist(int plId, int songId)
        {
            return plDAL.AddSongToPlaylist(plId, songId);
        }
        public List<Playlist> GetActivePlaylistList(int userId)
        {
            bool status = true;
            return plDAL.GetPlaylistList(userId, status);
        }
        public List<Playlist> GetRemovedPlaylistList(int userId)
        {
            bool status = false;
            return plDAL.GetPlaylistList(userId, status);
        }
        public bool RecycleRemovedPlaylist(Playlist pl)
        {
            return plDAL.RecycleRemovedPlaylist(pl);
        }
        public bool CheckSongExistInPlaylist(int plId, int songId) //Mean this song doen't exist in this playlist be4
        {
            int cType = 0; //Check Exist
            return plDAL.CheckSongExistOrItsStatusInPlaylist(plId, songId, cType);
        }
        public bool CheckSongStatusInPlaylist(int plId, int songId) //Check this song's playlistSongStatus in playlist
        {                                                           //True: This song is existing in playlist
            int cType = 1; //Check Status                           //False: This song existed in playlist but it's playlistSongStatus
            return plDAL.CheckSongExistOrItsStatusInPlaylist(plId, songId, cType); //is false. So we need to RE-ACTIVE (turn false to
        }                                                           // true) not add this song to playlist
        public bool AutoUpdateAllActiveSongBeforeToFalseAfterItsStatusChanged(int songId)
        {
            return plDAL.AutoUpdateAllActiveSongBeforeToFalseAfterItsStatusChanged(songId);
        }
    }
}