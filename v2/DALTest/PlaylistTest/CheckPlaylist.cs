using Xunit;
using DAL;
using Persistence;
using System.Collections.Generic;
public class PlaylistTest
{
    PlaylistDAL plDAL = new PlaylistDAL();
    // [Theory]
    // [InlineData("Test Playlist", 99)]//non-exist userId
    // [InlineData("Test Playlist 1", 2)]
    // [InlineData("Test Playlist 2", 2)]
    // public void PassingCreateANewPlaylist(string title, int userId)
    // {
    //     Playlist pl = new Playlist();
    //     pl.playlistTitle = title;
    //     pl.userId = userId;
    //     pl.createDate = System.DateTime.Today;
    //     Assert.True(plDAL.CreateANewPlaylist(pl));
    // }

    // [Fact]
    // public void PassingGetMaxIdInPlaylist()
    // {
    //     Assert.True(plDAL.GetMaxIdInPlaylist() != 0);
    // }

    // [Theory]
    // [InlineData(6, 99, false)]//Invalid songId
    // [InlineData(99, 1, false)]//Invalid playlistId
    // [InlineData(6, 1, false)]//valid playlist Id, song Id
    // [InlineData(6, 1, true)]
    // public void PassingUpdateSongInPlaylist(int plId, int songId, bool status)
    // {
    //     Assert.True(plDAL.UpdateSongInPlaylist(plId, songId, status));
    // }

    // [Theory]
    // [InlineData(99, "Update new playlist title", 2)]//Invalid plId
    // [InlineData(6, "Update new playlist title", 99)]//Invalid UserId
    // [InlineData(6, null, 2)]
    // [InlineData(6, "Update new playlist title", 2)]//Valid plId, userId
    // public void PassingUpdatePlaylistTitle(int plId, string title, int userId)
    // {
    //     Playlist pl = new Playlist();
    //     pl.playlistId = plId;
    //     pl.playlistTitle = title;
    //     pl.userId = userId;
    //     Assert.True(plDAL.UpdatePlaylistTitle(pl));
    // }

    // [Theory]
    // [InlineData(8,2)]//Valid plId, userId (plID can not be wrong => userId can not be wrong too)
    // public void PassingUpdateThisPlaylistStatusTo_False(int plId, int userId)
    // {
    //     Playlist pl = new Playlist();
    //     pl.playlistId = plId;
    //     pl.userId = userId;
    //     Assert.True(plDAL.UpdateThisPlaylistStatusTo_False(pl));
    // }

    // [Theory]
    // [InlineData(8, "Recycle playlist", 2)]//(plID can not be wrong => userId can not be wrong too,)
    // public void PassingRecycleRemovedPlaylist(int plId, string title, int userId)
    // {
    //     Playlist pl = new Playlist();
    //     pl.playlistId = plId;
    //     pl.playlistTitle = title;
    //     pl.userId = userId;
    //     pl.createDate = System.DateTime.Today;
    //     Assert.True(plDAL.RecycleRemovedPlaylist(pl));
    // }
    
    // [Theory]
    // [InlineData(99,1)]//Invalid plId
    // [InlineData(6,99)]//Invalid songId
    // [InlineData(6,1)]//Valid plId, songId
    // public void PassingAddSongToPlaylist(int plId, int songId)
    // {
    //     Assert.True(plDAL.AddSongToPlaylist(plId, songId));
    // }

    // [Theory]
    // [InlineData(99, true)]//Invalid userId
    // [InlineData(3, false)]
    // [InlineData(1, false)]//This user doen't not have any removed playlist
    // [InlineData(3, true)]
    // public void PassingGetPlaylistList(int userId, bool status)
    // {
    //     List<Playlist> playlistList = plDAL.GetPlaylistList(userId, status);
    //     Assert.True(playlistList != null);
    //     Assert.True(playlistList.Count > 0);
    // }

    // [Theory]
    // [InlineData(99, 1, 0)]
    // [InlineData(6, 2, 0)]
    // [InlineData(6, 2, 1)]
    // [InlineData(6, 1, 0)]//0: CHECK EXIST/
    // [InlineData(6, 1, 1)]//1: CHECK STATUS//This song's status now is false
    // public void PassingCheckSongExistOrItsStatusInPlaylist(int plId, int songId, int cType)
    // {
    //     Assert.True(plDAL.CheckSongExistOrItsStatusInPlaylist(plId, songId, cType));
    // }

    // [Theory]
    // [InlineData(99)]//invalid songId
    // [InlineData(3)]//false already
    // [InlineData(1)]
    // public void PassingAutoUpdateAllActiveSongBeforeToFalseAfterItsStatusChanged(int songId)
    // {
    //     Assert.True(plDAL.AutoUpdateAllActiveSongBeforeToFalseAfterItsStatusChanged(songId));
    // }
}