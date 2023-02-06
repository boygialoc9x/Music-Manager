using Xunit;
using DAL;
using System.Collections.Generic;
using System;
using Persistence;
public class SongTest
{
    // GET_ALL = 0;
    // GET_BY_SONGNAME = 1;

    // LYRIC = 0;
    // DOWNLOADLINK = 1;
    // STATUS = 2;
    SongDAL sDAL = new SongDAL();
    [Fact]
    public void PassingGetMaxId()
    {
        Assert.True(sDAL.GetMaxIdInSongs() != 0);
    }

    [Theory]
    [InlineData(0, null)] //get all songs
    public void PassingGetSongList(int sType, string searchString)
    {
        List<Song> songlist = sDAL.GetSongList(sType, searchString);
        Assert.True(songlist != null);
        Assert.True(songlist.Count > 0);
    }

    [Theory]
    [InlineData("Rockstar", "", "", 218, true, "9/15/2017", "beerbongs & bentleys", "")]
    public void PassingAddANewSong(string songName, string downloadLink, string lyric, int length, bool status, string releaseDate, string album, string coppyright)
    {
        Song song = new Song();
        song.songId = sDAL.GetMaxIdInSongs() + 1;
        song.songName = songName;
        song.downloadLink = downloadLink;
        song.lyric = lyric;
        song.length = length;
        song.songStatus = status;
        song.releaseDate = Convert.ToDateTime(releaseDate);
        song.album = album;
        song.copyright = coppyright;
        Assert.True(sDAL.AddNewSong(song));
    }

    [Theory]
    [InlineData(0,0)] //non-exist songId and cateId
    [InlineData(0, 1)] //non-exist songId
    [InlineData(1, 0)] //non-exist cateId
    [InlineData(8, 3)]
    public void PassingAddGenresToSong(int songId, int cateId)
    {
        Assert.True(sDAL.AddGenresToSong(songId, cateId));
    }

    [Theory]
    [InlineData(0,0, Persistence.Enum.ArtistType.Singer)] //non-exist songId and artistId
    [InlineData(0,1, Persistence.Enum.ArtistType.Singer)] //non-exist songId
    [InlineData(1,0, Persistence.Enum.ArtistType.Singer)] //non-exist songId
    [InlineData(8,3, Persistence.Enum.ArtistType.Singer)]
    [InlineData(8,3, Persistence.Enum.ArtistType.Writer)]
    public void PassingAddArtistsToSong(int songId, int artistId, Enum aType)
    {
        Assert.True(sDAL.AddArtistsToSong(songId, artistId, aType));
    }

    [Theory]
    [InlineData("Rockstar", "", "", 218, true, "9/15/2017", "beerbongs & bentleys", "Republic Records")]
    public void PassingUpdateSongInfor(string songName, string downloadLink, string lyric, int length, bool status, string releaseDate, string album, string coppyright)
    {
        int songId = 8;
        Song song = sDAL.GetSongInfor(songId);

        song.songName = songName;
        song.length = length;
        song.downloadLink = downloadLink;
        song.lyric = lyric;
        song.songStatus = status;
        song.releaseDate = Convert.ToDateTime(releaseDate);
        song.album = album;
        song.copyright = coppyright;

        Assert.True(sDAL.UpdateSongInfor(song));
    }

    [Theory]
    [InlineData(0, 8, null)]
    [InlineData(0, 8, "this is updated lyric")] // LYRIC = 0
    [InlineData(1, 8, null)]
    [InlineData(1, 8, "updateddownloadlink.com")] // DOWNLAODLINK = 1
    [InlineData(2, 8, null)] // STATUS = 2
    [InlineData(2, 8, "false")]
    [InlineData(2, 8, "true")]
    public void PassingUpdateSomeSongInfor(int gType, int songId, string updateSTR)
    {
        Assert.True(sDAL.UpdateSomeSongInfor(gType, songId, updateSTR));
    }

}