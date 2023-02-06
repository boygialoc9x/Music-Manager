using Xunit;
using DAL;
using System.Collections.Generic;
using Persistence;
public class CategoriesTest
{
    CategoriesDAL cateDAL = new CategoriesDAL();

    // [Theory]
    // [InlineData(3, null)] //Only get category infor by categoryId or categoryName
    // [InlineData(0, null)]
    // [InlineData(0, "edm")] //Set categoryName = null if you want get category by id. the same with categoryId = 0
    // [InlineData(0, "asgfafk")] 
    // public void PassingGetCategoryInfor(int categoryId, string categoryName)
    // {
    //     Categories cate = cateDAL.GetCategoryInfor(categoryId, categoryName);
    //     Assert.True(cate != null);
        
    //     if (categoryName != null) Assert.Equal(categoryName.ToLower(), cate.categoryName.ToLower());
    //     else Assert.Equal(categoryId, cate.categoryId );
    // }

    // [Theory]
    // [InlineData(22, true)]
    // [InlineData(22, false)]
    // [InlineData(50, true)] //Non-exist id
    // [InlineData(18, false)] //Have no non-active in list
    // public void PassingGetCategoriesOfSong(int songId, bool status)
    // {
    //     List<Categories> cateList = cateDAL.GetCategoriesOfSong(songId, status);
    //     Assert.True(cateList != null);
    //     Assert.True(cateList.Count > 0 );
    // }

    // [Fact]
    // public void PassingGetMaxId()
    // {
    //     Assert.True(cateDAL.GetMaxIdInCategories() > 0);
    // }

    // [Theory]
    // [InlineData("A New Categories", true)]
    // [InlineData("A new cate but false", false)]
    // public void PassingAddNewCategory(string categoryName, bool status)
    // {
    //     Categories cate = new Categories();
    //     cate.categoryId = cateDAL.GetMaxIdInCategories() + 1 ;
    //     cate.categoryName = categoryName;
    //     cate.categoryStatus = status;
    //     Assert.True(cateDAL.AddNewCategory(cate));
    // }

    // [Theory]
    // [InlineData(null)]
    // [InlineData("")]
    // [InlineData("asfkasf")]
    // [InlineData("edm")]
    // public void PassingCheckCategoryName(string categoryName)
    // {
    //     Assert.True(cateDAL.CheckCategoryName(categoryName));
    // }

}