using Xunit;
using DAL;
using System.Collections.Generic;
using Persistence;
using System.Linq;
using System;
public class ArtistsTest
{
    ArtistsDAL aDAL = new ArtistsDAL();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("asfgasg")]
    [InlineData("$uicideboy$")]
    public void PassingGetArtistListbybandName(string bandName)
    {
        List<Artists> aList = aDAL.GetArtistsInforListByBandName(bandName);
        Assert.True(aList != null);
        Assert.True(aList.Count > 0);
        Assert.True( aList[0].bandName.ToLower() == bandName.ToLower());
    }

    [Theory]
    [InlineData(0, true, Persistence.Enum.ArtistType.Singer)] //non-exist id
    [InlineData(2, true, Persistence.Enum.ArtistType.Writer)]
    [InlineData(2, false, Persistence.Enum.ArtistType.Producer)]
    [InlineData(2, false, Persistence.Enum.ArtistType.Band)]
    public void PassingGetArtistsOfSong(int songId, bool status, Enum aType)
    {
        List<Artists> aList = aDAL.GetArtistOfSong(songId, status, aType);
        Assert.True(aList != null);
        Assert.True(aList.Count > 0);
    }


    [Fact]
    public void PassingGetMaxId()
    {
        Assert.True(aDAL.GetMaxIdInArtist() > 0);
    }  

    [Theory]
    [InlineData("Aubrey", "Drake Graham ", false, "",true, "Drake", true, "Drake", true, "Drake" ,"10/24/1986",true, Persistence.Enum.Gender.Male)]
    public void PassingAddNewArtist(string firstName, string lastName, bool isBand,string banName, bool isSinger, string stageName, bool isWriter, string songwriterAlias, bool isProducer, string producerAlias ,string born, bool status, Persistence.Enum.Gender gender)
    {
        Artists a = new Artists();
        a.artistId = aDAL.GetMaxIdInArtist() + 1;
        a.artistFirstName = firstName;
        a.artistLastName = lastName;
        a.born = Convert.ToDateTime(born);
        a.isBand = isBand;
        a.bandName = banName;
        a.isProducer = isProducer;
        a.producerAlias = producerAlias;
        a.isSinger = isSinger;
        a.stageName = stageName;
        a.isWriter = isWriter;
        a.songwriterAlias = songwriterAlias;
        a.gender = gender;
        a.artistStatus = status;
        Assert.True(aDAL.AddNewArtist(a));
    }

    [Theory]
    [InlineData("Aubrey", "Drake Graham ", false, "",true, "Drake", true, "Drake", true, "Drake" ,"10/24/1986",true, Persistence.Enum.Gender.Male)]
    public void PassingUpdateArtist(string firstName, string lastName, bool isBand,string banName, bool isSinger, string stageName, bool isWriter, string songwriterAlias, bool isProducer, string producerAlias ,string born, bool status, Persistence.Enum.Gender gender)
    {
        Artists a = new Artists();
        a.artistId = aDAL.GetMaxIdInArtist();
        a.artistFirstName = firstName;
        a.artistLastName = lastName;
        a.born = Convert.ToDateTime(born);
        a.isBand = isBand;
        a.bandName = banName;
        a.isProducer = isProducer;
        a.producerAlias = producerAlias;
        a.isSinger = isSinger;
        a.stageName = stageName;
        a.isWriter = isWriter;
        a.songwriterAlias = songwriterAlias;
        a.gender = gender;
        a.artistStatus = status;
        
        Assert.True(aDAL.UpdateArtistInfor(a));
    }

    [Theory]
    [InlineData (Persistence.Enum.ArtistType.Singer, true)]
    [InlineData (Persistence.Enum.ArtistType.Producer, false)]
    public void PassingGetListOfArtists(Enum aTYpe, bool status)
    {
        Assert.True(aDAL.GetListOfArtists(aTYpe, status).Count > 0);
    }

}