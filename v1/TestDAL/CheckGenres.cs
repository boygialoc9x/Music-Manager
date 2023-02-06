using Xunit;
using DAL;
using System.Collections.Generic;
using Persistence;
public class GenresTest
{
    GenresDAL genreDAL = new GenresDAL();

    [Theory]
    [InlineData(3, null)] //Only get category infor by categoryId or categoryName
    [InlineData(0, null)]
    [InlineData(0, "edm")] //Set categoryName = null if you want get category by id. the same with categoryId = 0
    [InlineData(0, "asgfafk")] 
    public void PassingGetGenreInfor(int genreId, string genreTitle)
    {
        Genres genre = genreDAL.GetGenreInfor(genreId, genreTitle);
        Assert.True(genre != null);
        
        if (genreTitle != null) Assert.Equal(genreTitle.ToLower(), genre.genreTitle.ToLower());
        else Assert.Equal(genreId, genre.genreId );
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(1, false)]//Have no non-active in list
    [InlineData(50, true)] //Non-exist id
    public void PassingGetGenresOfSong(int songId, bool status)
    {
        List<Genres> cateList = genreDAL.GetGenresOfSong(songId, status);
        Assert.True(cateList != null);
        Assert.True(cateList.Count > 0 );
    }

    [Fact]
    public void PassingGetMaxId()
    {
        Assert.True(genreDAL.GetMaxIdInGenres() > 0);
    }

    [Theory]
    [InlineData("Rock'n'roll", true)]
    [InlineData("Dance", false)]
    public void PassingAddNewGenre(string genreTitle, bool status)
    {
        Genres genre = new Genres();
        genre.genreId = genreDAL.GetMaxIdInGenres() + 1 ;
        genre.genreTitle = genreTitle;
        genre.genreStatus = status;
        Assert.True(genreDAL.AddNewGenre(genre));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("asfkasf")]
    [InlineData("edm")]
    public void PassingCheckGenreTitle(string genreTitle)
    {
        Assert.True(genreDAL.CheckGenreTitle(genreTitle));
    }

}