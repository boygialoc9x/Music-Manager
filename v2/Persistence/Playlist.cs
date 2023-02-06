using System;
using System.Collections.Generic;
namespace Persistence
{
    public class Playlist
    {
        public int playlistId {get;set;}
        public string playlistTitle {get;set;}
        public bool playlistStatus {get;set;}
        public DateTime createDate {get;set;}
        public int userId {get;set;}
        public List<Song> playlistSongs {get;set;}
    }
}