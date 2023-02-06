using System;
using System.Collections.Generic;
using Persistence;
using DAL;
namespace BL
{
    public class GenresBL
    {
        private GenresDAL genreDAL = new GenresDAL();

        public bool CheckGenreTitle(string genreTitle)
        {
            return genreDAL.CheckGenreTitle(genreTitle);
        }
        public int GetMaxIdInGenres()
        {
            return genreDAL.GetMaxIdInGenres();
        }
        public bool AddNewGenre(Genres nGenre)
        {
            return genreDAL.AddNewGenre(nGenre);
        }
        public Genres GetGenreInforByTitle(string genTitle)
        {
            int nullNumber = 0;
            return genreDAL.GetGenreInfor(nullNumber, genTitle);
        }

        public Genres GetGenreInforById(int genreId)
        {
            string nullString = null;
            return genreDAL.GetGenreInfor(genreId, nullString);
        }
        public int GetGenreIdByTitle(string genreTitle)
        {
            return genreDAL.GetGenreIdByTitle(genreTitle);
        }
        public List<Genres> GetNoneActiveGenreOfSong(int songId)
        {
            List<Genres> genreList = genreDAL.GetGenresOfSong(songId, false);
            return genreList;
        }
        public List<Genres> GetListOfActiveGenre()
        {
            bool status = true;
            return genreDAL.GetGenresList(status);
        }
    }
}