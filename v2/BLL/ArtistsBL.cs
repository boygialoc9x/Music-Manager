using System;
using System.Collections.Generic;
using Persistence;
using DAL;
namespace BL
{
    public class ArtistsBL
    {
        private ArtistsDAL atDAL = new ArtistsDAL();
        public Artists GetArtistInforById(int artistId)
        {
            return atDAL.GetArtistInforById(artistId);
        }
        public List<Artists> GetArtistsInforListBytheArtist(string theArtist )
        {
            return atDAL.GetArtistsInforListBytheArtist(theArtist);
        }
        public bool ChecktheArtist(string theArtist)
        {
            return atDAL.ChecktheArtist(theArtist);
        }

        public List<Artists> GetNoneActiveArtistsOfSong(int songId)
        {
            List<Artists> artistsList = atDAL.GetArtistsOfSong(songId, false);
            return artistsList;
        }
        public int GetMaxIdInArtist()
        {
            return atDAL.GetMaxIdInArtist();
        }
        public bool AddNewArtist(Artists a)
        {
            return atDAL.AddNewArtist(a);
        }
        public bool UpdateArtistInfor(Artists a)
        {
            return atDAL.UpdateArtistInfor(a);
        }

    }
}
