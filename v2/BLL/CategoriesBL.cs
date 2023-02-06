using System;
using System.Collections.Generic;
using Persistence;
using DAL;
namespace BL
{
    public class CategoriesBL
    {
        private CategoriesDAL cateDAL = new CategoriesDAL();

        public bool CheckCategoryName(string categoryName)
        {
            return cateDAL.CheckCategoryName(categoryName);
        }
        public int GetMaxIdInCategories()
        {
            return cateDAL.GetMaxIdInCategories();
        }
        public bool AddNewCategory(Categories nCate)
        {
            return cateDAL.AddNewCategory(nCate);
        }
        public Categories GetCategoryInforByName(string cateName)
        {
            int nullNumber = 0;
            return cateDAL.GetCategoryInfor(nullNumber, cateName);
        }

        public Categories GetCategoryInforById(int cateId)
        {
            string nullString = null;
            return cateDAL.GetCategoryInfor(cateId, nullString);
        }
        public int GetCategoryIdByName(string categoryName)
        {
            return cateDAL.GetCategoryIdByName(categoryName);
        }
        public List<Categories> GetNoneActiveCategoryOfSong(int songId)
        {
            List<Categories> cateList = cateDAL.GetCategoriesOfSong(songId, false);
            return cateList;
        }
    }
}