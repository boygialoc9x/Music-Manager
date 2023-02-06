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
        public List<Artists> GetArtistInforListByBandName(string bandName )
        {
            return atDAL.GetArtistsInforListByBandName(bandName);
        }
        public bool ChecktheArtist(string theArtist)
        {
            return atDAL.ChecktheArtist(theArtist);
        }

        public List<Artists> GetNoneActiveSingerOfSong(int songId)
        {
            bool status = false;
            return atDAL.GetArtistOfSong(songId, status, Persistence.Enum.ArtistType.Singer);
        }
        public List<Artists> GetNoneActiveBandOfSong(int songId)
        {
            bool status = false;
            return atDAL.GetArtistOfSong(songId, status, Persistence.Enum.ArtistType.Band);
        }
        public List<Artists> GetNoneActiveProducerOfSong(int songId)
        {
            bool status = false;
            return atDAL.GetArtistOfSong(songId, status, Persistence.Enum.ArtistType.Producer);
        }
        public List<Artists> GetNoneActiveWriterOfSong(int songId)
        {
            bool status = false;
            return atDAL.GetArtistOfSong(songId, status, Persistence.Enum.ArtistType.Writer);
        }
        public List<Artists> GetBandListOfSong(int songId)
        {
            bool status = true;
            return atDAL.GetArtistOfSong(songId, status, Persistence.Enum.ArtistType.Band );
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
        public List<Artists> GetListOfActiveSinger()
        {
            bool status = true;
            return atDAL.GetListOfArtists(Persistence.Enum.ArtistType.Singer, status);
        }
        public List<Artists> GetListOfActiveBand()
        {
            bool status = true;
            return atDAL.GetListOfArtists(Persistence.Enum.ArtistType.Band, status);
        }
        public List<Artists> GetListOfActiveWriter()
        {
            bool status = true;
            return atDAL.GetListOfArtists(Persistence.Enum.ArtistType.Writer, status);
        }
        public List<Artists> GetListOfActiveProducer()
        {
            bool status = true;
            return atDAL.GetListOfArtists(Persistence.Enum.ArtistType.Producer, status);
        }
    }
}
