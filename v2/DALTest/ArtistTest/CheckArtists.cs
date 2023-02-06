using Xunit;
using DAL;
using System.Collections.Generic;
using Persistence;
using System.Linq;
using System;
public class ArtistsTest
{
    ArtistsDAL aDAL = new ArtistsDAL();

    // [Theory]
    // [InlineData(null)]
    // [InlineData("")]
    // [InlineData("asfgasg")]
    // [InlineData("Joji")]
    // [InlineData("$uicideboy$")]
    // public void PassingGetArtistListbyTheArtist(string theArtist)
    // {
    //     List<Artists> aList = aDAL.GetArtistsInforListBytheArtist(theArtist);
    //     Assert.True(aList != null);
    //     Assert.True(aList.Count > 0);
    //     Assert.True( aList[0].theArtist.ToLower() == theArtist.ToLower());
    // }

    // [Theory]
    // [InlineData(0, true)] //non-exist id
    // [InlineData(22, true)]
    // [InlineData(22, false)]
    // [InlineData(1, false)] //have no non-active 
    // public void PassingGetArtistsOfSong(int songId, bool status)
    // {
    //     List<Artists> aList = aDAL.GetArtistsOfSong(songId, status);
    //     Assert.True(aList != null);
    //     Assert.True(aList.Count > 0);
    // }

    // [Theory]
    // [InlineData(null)]
    // [InlineData("")]
    // [InlineData("asf24")]
    // [InlineData("SZA")]
    // public void PassingCheckTheArtist(string theArtist)
    // {
    //     Assert.True(aDAL.ChecktheArtist(theArtist));
    // }  

    // [Fact]
    // public void PassingGetMaxId()
    // {
    //     Assert.True(aDAL.GetMaxIdInArtist() > 0);
    // }  

    // [Theory]
    // [InlineData("New artist", "last name", "The Test Unit", "1/1/2001", true)]
    // [InlineData("New artist II", "last name", "The Test Unit", "8/8/1988", false)]
    // public void PassingAddNewArtist(string firstName, string lastName, string theArtist ,string born, bool status)
    // {
    //     Artists a = new Artists();
    //     a.artistId = aDAL.GetMaxIdInArtist() + 1;
    //     a.artistFirstName = firstName;
    //     a.artistLastName = lastName;
    //     a.born = Convert.ToDateTime(born);
    //     a.theArtist = theArtist;
    //     a.artistStatus = status;
    //     Assert.True(aDAL.AddNewArtist(a));
    // }

    // [Theory]
    // [InlineData("The new artist", "new last name", "The Test Unit", "1/5/2001")]
    // public void PassingUpdateArtist(string firstName, string lastName, string theArtist ,string born)
    // {
    //     Artists a = new Artists();
    //     a.artistId = 21;
    //     a.artistFirstName = firstName;
    //     a.artistLastName = lastName;
    //     a.born = Convert.ToDateTime(born);
    //     a.theArtist = theArtist;
        
    //     Assert.True(aDAL.UpdateArtistInfor(a));
    // }

}