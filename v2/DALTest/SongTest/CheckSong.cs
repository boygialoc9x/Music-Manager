using Xunit;
using DAL;
using System.Collections.Generic;
using Persistence;
public class SongTest
{
    // GET_ALL = 0;
    // GET_BY_SONGNAME = 1;

    // LYRIC = 0;
    // DOWNLOADLINK = 1;
    // STATUS = 2;
    SongDAL sDAL = new SongDAL();
    // [Fact]
    // public void PassingGetMaxId()
    // {
    //     Assert.True(sDAL.GetMaxIdInSongs() != 0);
    // }

    // [Theory]
    // [InlineData(0)]//non-exist id
    // [InlineData(1)]
    // public void PassingGetSongInfor(int songId)
    // {
    //     Song song = sDAL.GetSongInfor(songId);
    //     Assert.True(song != null);
    //     Assert.Equal(songId, song.songId);
    // }

    // [Theory]
    // [InlineData(0, null)] //get all songs
    // [InlineData(1, "a")] //get all song that the song's name contain "a" char
    // [InlineData(1, null)]
    // [InlineData(1, "235sf")]
    // public void PassingGetSongList(int sType, string searchString)
    // {
    //     List<Song> songlist = sDAL.GetSongList(sType, searchString);
    //     Assert.True(songlist != null);
    //     Assert.True(songlist.Count > 0);
    // }

    // [Theory]
    // [InlineData("New Test Unit II", "testunit.com", "test unit lyric", 120, true)]
    // public void PassingAddANewSong(string songName, string downloadLink, string lyric, int length, bool status)
    // {
    //     Song song = new Song();
    //     song.songId = sDAL.GetMaxIdInSongs() + 1;
    //     song.songName = songName;
    //     song.downloadLink = downloadLink;
    //     song.lyric = lyric;
    //     song.length = length;
    //     song.songStatus = status;
    //     Assert.True(sDAL.AddNewSong(song));
    // }

    // [Theory]
    // [InlineData(0,0)] //non-exist songId and cateId
    // [InlineData(0, 1)] //non-exist songId
    // [InlineData(1, 0)] //non-exist cateId
    // [InlineData(26, 1)]
    // public void PassingAddCategoriesToSong(int songId, int cateId)
    // {
    //     Assert.True(sDAL.AddCategoriesToSong(songId, cateId));
    // }

    // [Theory]
    // [InlineData(0,0)] //non-exist songId and artistId
    // [InlineData(0,1)] //non-exist songId
    // [InlineData(1,0)] //non-exist songId
    // [InlineData(26,1)]
    // public void PassingAddArtistsToSong(int songId, int artistId)
    // {
    //     Assert.True(sDAL.AddArtistsToSong(songId, artistId));
    // }

    // [Theory]
    // [InlineData("", 0)]
    // [InlineData(null, 0)]
    // [InlineData("Update test unit", 120)]
    // public void PassingUpdateSongInfor(string songName,int length)
    // {
    //     int songId = 26;
    //     Song song = sDAL.GetSongInfor(songId);

    //     song.songName = songName;
    //     song.length = length;

    //     Assert.True(sDAL.UpdateSongInfor(song));
    // }

    // [Theory]
    // [InlineData(0, 26, null)]
    // [InlineData(0, 26, "this is updated lyric")] // LYRIC = 0
    // [InlineData(1, 26, null)]
    // [InlineData(1, 26, "updateddownloadlink.com")] // DOWNLAODLINK = 1
    // [InlineData(2, 26, null)] // STATUS = 2
    // [InlineData(2, 26, "false")]
    // [InlineData(2, 26, "true")]
    // public void PassingUpdateSomeSongInfor(int gType, int songId, string updateSTR)
    // {
    //     Assert.True(sDAL.UpdateSomeSongInfor(gType, songId, updateSTR));
    // }

}