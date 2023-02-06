using System;
using System.Collections;
using System.Collections.Generic;
using Persistence;
using BL;
using ConsolePL.UI;
using System.Linq;
using ConsolePL.UTIL;
using ConsoleTables;

public class SongMenu
{
    private GenresBL genreBL = new GenresBL();
    private SongBL sBL = new SongBL();
    private ArtistsBL aBL = new ArtistsBL();
    //private static string[] menuOption = MagicNumber.songMenu_menuOption;
    private static string menuTitle = MagicNumber.songMenu_menuTitle;
    public Song display_SongMenu(Customer customer, Song song)
    {
        //Song copSong = song;
        bool endLoop = true;

        ArrayList mOption = MagicNumber.songMenu_Remove_NonePermOption( customer.staff, customer.premium);
        int removingNumber = MagicNumber.songMenu_Get_numberOfRemovingMenuOption(customer.staff, customer.premium);

        while (endLoop)
        {
            //Console.Clear();
            UIHelper.MenuTitle("Displaying song's information");
            this.DisplaySong(song, customer.staff);
            var chooseMenuOpTion = UIHelper.ChooseOptionMenu(mOption, menuTitle);
            if ( Int32.TryParse(chooseMenuOpTion, out int k) )
            {
                if(k > mOption.Count - 1) 
                {                         
                    k = k + removingNumber;   
                    chooseMenuOpTion = k.ToString();  
                }                              
            }

            switch (chooseMenuOpTion)
            {
                case "1":
                    Console.Clear(); 
                        if ( song.lyric.Length > 1) //a song's lyric length always > 1 Right ?
                        {
                            UIHelper.MenuTitle($"{song.songName}'s Lyric");
                            Console.WriteLine();
                            Console.WriteLine(song.lyric);
                            Console.WriteLine();
                            UIHelper.DrawStraightLine();
                            Console.Write("Press any key to continute... ");
                            Console.ReadKey();
                            Console.Clear();
                        }  
                        else Console.WriteLine("This song has no lyrics yet :(");            
                    break;
                case "2":
                    Console.Clear();
                    if (GetIP.CheckConnect()) this.p_download_song(song.downloadLink, song.songName);
                    else Console.WriteLine("You are not connected to the internet !!");
                    break;
                case "3":
                    Console.Clear();
                    Song decoy = this.ChooseArtistsFromArtistsPages(customer, song);
                    List<Artists> copSingerList = new List<Artists>();
                    foreach(var val in decoy.singer) copSingerList.Add(val);
                    List<Artists> isBand = DetectSingerIfTheyCanBeABand(copSingerList);
                    if (isBand != null) 
                    { 
                        decoy.band = isBand;

                        foreach(var val in isBand) decoy.singer.RemoveAt(decoy.singer.FindIndex(x=>x.artistId.Equals(val.artistId)));   
                    }
                    break;
                case "4":
                    Console.Clear();
                    this.ChangeSongInformation(song);
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine("Exit");
                    endLoop = false;
                    break;
                default:
                    Console.Clear();

                    Console.WriteLine("Wrong Choice");
                    Console.WriteLine("Please TryAgain");
                    break;
            }
        } 
        return song;
    }
    private Song autoDetectSingerAndBand(Song decoy)
    {
        if (decoy.singer != null)
        {
            List<Artists> copSingerList = new List<Artists>();
            foreach(var val in decoy.singer) copSingerList.Add(val);
            List<Artists> isBand = DetectSingerIfTheyCanBeABand(copSingerList);
            if (isBand != null) 
            { 
                decoy.band = isBand;

                foreach(var val in isBand) decoy.singer.RemoveAt(decoy.singer.FindIndex(x=>x.artistId.Equals(val.artistId)));   
            }
        }
        return decoy;
    }
    private Song ChangeSongInformation(Song song)
    {
        bool endLoop = true;
        //Song updatedSong = new Song();
        string menuTitle = MagicNumber.ChooseUpdatePartSong_menuTitle;
        ArrayList mOption = MagicNumber.ChooseUpdatePartSong_menuOption();
        while(endLoop)
        {
            UIHelper.MenuTitle("Displaying song's information");
            this.DisplaySong(song, true);
            var chooseUpdatePart = UIHelper.ChooseOptionMenu(mOption, menuTitle);
            switch (chooseUpdatePart)
            {
                case "1":
                    song.songName = this.ChangeSongTitle(song);
                    break;
                case "2":
                    Song decoySong = this.ChangeSongSinger(song);
                    song.band = decoySong.band;
                    song.singer = decoySong.singer;
                    break;
                case "3":
                    song.writer = this.ChangeSongComposer(song);
                    break;
                case "4":
                    song.produced = this.ChangeSongProducer(song);
                    break;
                case "5":
                    song.length = this.ChangeSongDuration(song);
                    break;
                case "6":
                    song.lyric = this.ChangeSongLyric(song);
                    break;
                case "7":
                    song.downloadLink = this.ChangeSongDownloadLink(song);
                    break;
                case "8":
                    song.album = this.ChangeAlbumTitle(song);
                    break;
                case "9":
                    song.copyright = this.ChangeCoppyright(song);
                    break;
                case "10":
                    song.releaseDate = this.ChangeSongRelease(song);
                    break;
                case "11":
                    song.genres = this.ChangeSongGenres(song);
                    break;
                case "12":
                    song.songStatus = this.ChangeSongStatus(song);
                    break;
                case "13":
                    endLoop = false;
                    break;
                default:
                    break;
            }
        }
        return song;
    }
    private string EnterSomeSongInformation(string menuTitle, string askMess)
    {
        Console.Clear();
        UIHelper.MenuTitle($"{menuTitle}");
        Console.WriteLine($"{askMess}");
        string title = null;
        if (YNQuest.ask_YesOrNo())
        {
           title = StringUTil.GetValidName("Enter Title: "); 
        }  
        return title;
    }
    private string ChangeSongTitle(Song song)
    {
        string title = this.EnterSomeSongInformation("Changing Song's Title", " Are you sure to change this song's title ?");
        if ( title != null)
        {
            song.songName = title;

            if (sBL.UpdateSongInfor(song)) { Console.Clear(); Console.WriteLine("Update successfully !"); }
            else { Console.Clear(); Console.WriteLine("Update fail :("); }
        } 
        else{ Console.Clear(); Console.WriteLine("Cancel update song's title!"); }
        
        return song.songName;
    }
    private string ChangeAlbumTitle(Song song)
    {
        string title = this.EnterSomeSongInformation("Changing Song's Album Title", " Are you sure to change this song's album Title ?");
        if ( title != null)
        {
            song.album = title;

            if (sBL.UpdateSongInfor(song)) { Console.Clear(); Console.WriteLine("Update successfully !"); }
            else  { Console.Clear(); Console.WriteLine("Update fail :("); }
        } 
        else { Console.Clear(); Console.WriteLine("Cancel update song's album!"); }
        
        return song.album;
    }
    private string ChangeCoppyright(Song song)
    {
        string title = this.EnterSomeSongInformation("Changing Song's Coppyright", " Are you sure to change this song's coppyright ?");
        if ( title != null)
        {
            song.copyright = title;

            if (sBL.UpdateSongInfor(song)) { Console.Clear(); Console.WriteLine("Update successfully !"); }
            else { Console.Clear(); Console.WriteLine("Update fail :("); }
        } 
        else { Console.Clear(); Console.WriteLine("Cancel update song's coppyright!"); }
        
        return song.copyright;
    }
    private int ChangeSongDuration(Song song)
    {
        Console.Clear();
        int duration = 0;
        UIHelper.MenuTitle($"Chaging Song's Duration");
        Console.WriteLine($" Are you sure to change this song's title ?");
        if (YNQuest.ask_YesOrNo()) duration = StringUTil.GetValidNumber("Enter Duration: ");
         
        if (duration > 0) 
        {
            song.length = duration;

            if (sBL.UpdateSongInfor(song)) { Console.Clear(); Console.WriteLine("Update successfully !"); }
            else { Console.Clear(); Console.WriteLine("Update fail :("); }
        }
        else { Console.Clear(); Console.WriteLine("Cancel update song's title!"); }
        return song.length;
    }
    private string ChangeSongLyric(Song song)
    {
        Console.Clear();
        UIHelper.MenuTitle($"Changing Song's Lyric");
        Console.Write("Enter lyric: ");
        string theLyric = Console.ReadLine();
        Console.WriteLine("Save this change ? ");
        if(YNQuest.ask_YesOrNo())
        {
            if (sBL.UpdateSongLyric(song.songId, theLyric)) 
            {
                Console.Clear(); Console.WriteLine("Update success !");
                song.lyric = sBL.GetSongLyric(song.songId);
            }
            else { Console.Clear(); Console.WriteLine("Update fail!"); }
        }
        else { Console.Clear(); Console.WriteLine("Cancel update :("); }
        return song.lyric;
    }
    private string ChangeSongDownloadLink(Song song)
    {
        Console.Clear();
        UIHelper.MenuTitle($"Changing Song's Download Link");
        Console.Write("Enter download link: ");
        string theDownloadLink = Console.ReadLine();
        Console.WriteLine("Save this change ? ");
        if(YNQuest.ask_YesOrNo())
        {
            if (sBL.UpdateSongDownloadLink(song.songId, theDownloadLink) ) 
            {
                Console.Clear(); Console.WriteLine("Update success !");
                song.downloadLink = theDownloadLink;
            }
            else { Console.Clear(); Console.WriteLine("Update fail!");}
        }
        else { Console.Clear(); Console.WriteLine("Cancel update :("); }
        return song.downloadLink;
    }
    private bool ChangeSongStatus(Song song)
    {
        Console.Clear();
        UIHelper.MenuTitle($" Changing Song's Status");
        Console.Write("Active this song?");
        song.songStatus = YNQuest.ask_YesOrNo();
        sBL.UpdateSongStatus(song.songId, song.songStatus);
        return song.songStatus;
    }
    private DateTime ChangeSongRelease(Song song)
    {
        Console.Clear();
        DateTime releaseDate = song.releaseDate;
        UIHelper.MenuTitle($" Changing Song's Release Date");
        Console.WriteLine(" Are you sure to change this song's release date?");
        if(YNQuest.ask_YesOrNo()) releaseDate = StringUTil.GetReleaseDate();
            
        if (releaseDate != DateTime.MinValue)
        {
            song.releaseDate = releaseDate;
            if (sBL.UpdateSongInfor(song)) { Console.Clear(); Console.WriteLine("Update successfully !"); }
            else { Console.Clear(); Console.WriteLine("Update fail :("); }
        }
        return song.releaseDate;
    }
    private List<Artists> ChangeSongComposer(Song song)
    {
        Console.Clear();
        UIHelper.MenuTitle($" Changing Song's Composer");
        Console.WriteLine(" Are you sure to change this song's composers?");
        List<Artists> listComposer = new List<Artists>();
        if (YNQuest.ask_YesOrNo()) 
        {
            listComposer = this.InsertWriterToSong();
        } else listComposer = null;
        if (listComposer == null) listComposer = song.writer;
        else
        {
            if (listComposer != song.writer)
            {
                sBL.UpdateAllWritersOfSongBeforeToFalse(song.songId);

                if (this.AddWriterToSong(song.songId, listComposer)) { Console.Clear(); Console.WriteLine("Updated song's artists"); }
                else { Console.Clear(); Console.WriteLine("Couldn't update this song's artists"); }
            }
            else { Console.Clear(); Console.WriteLine("Nothing change to this song's composer !"); }
        }
        return listComposer;
    }
    private List<Artists> ChangeSongProducer(Song song)
    {
        Console.Clear();
        UIHelper.MenuTitle($" Changing Song's Producer");
        Console.WriteLine(" Are you sure to change this song's producer?");
        List<Artists> producer = new List<Artists>();
        if (YNQuest.ask_YesOrNo()) 
        {
            producer = this.InsertProducerToSong();
        } else producer = null;
        if (producer == null) producer = song.produced;
        else
        {
            if (producer != song.produced)
            {
                sBL.UpdateAllProducersOfSongBeforeToFalse(song.songId);

                if (this.AddProducerToSong(song.songId, producer)) 
                {
                    Console.Clear();
                    Console.WriteLine("Updated song's artists");
                }
                else 
                {
                    Console.Clear();
                    Console.WriteLine("Couldn't update this song's artists");
                }
            }
            else 
            {
                Console.Clear();
                Console.WriteLine("Nothing change to this song's producer !");
            }
        }
        return producer;
    }
    private Song ChangeSongSinger(Song song)
    {
        Console.Clear();
        UIHelper.MenuTitle($" Changing Song's Artist");
        Console.WriteLine(" Are you sure to change this song's artist?");
        Song decoySong = new Song();
        if (YNQuest.ask_YesOrNo()) 
        {
            decoySong = this.InsertBandOrSingerToSong();
        }
        else decoySong = null;

        if (decoySong == null) decoySong = song;
        else
        {
            if (decoySong.band != null)
            {
                if (decoySong.band != song.band)
                {
                    sBL.UpdateAllBandsOfSongBeforeToFalse(song.songId);

                    if (this.AddBandToSong(song.songId, decoySong.band)) 
                    {
                        Console.Clear();
                        Console.WriteLine("Updated song's band");
                    }
                    else 
                    {
                        Console.Clear();
                        Console.WriteLine("Couldn't update this song's band");
                    }
                }
                else 
                {
                    Console.Clear();
                    Console.WriteLine("Nothing change to this song's band !");
                }
            }

            if (decoySong.singer != null)
            {
                if (decoySong.singer != song.singer)
                {
                    sBL.UpdateAllSingersOfSongBeforeToFalse(song.songId);

                    if (this.AddSingerToSong(song.songId, decoySong.singer)) 
                    {
                        Console.Clear();
                        Console.WriteLine("Updated song's singers");
                    }
                    else 
                    {
                        Console.Clear();
                        Console.WriteLine("Couldn't update this song's singers");
                    }
                }
                else 
                {
                    Console.Clear();
                    Console.WriteLine("Nothing change to this song's singer !");
                }
            }
        }
        return decoySong;
    }
    private List<Genres> ChangeSongGenres(Song song)
    {
        Console.Clear();
        UIHelper.MenuTitle($" Changing The Song's Genres");
        Console.WriteLine(" Are you sure to change this song's genres?");
        List<Genres> listGenres = new List<Genres>();
        if (YNQuest.ask_YesOrNo()) 
        {
            listGenres = this.InsertGenresToSong();
        } else listGenres = null;
        if (listGenres == null) listGenres = song.genres;
        else
        {
            if (listGenres != song.genres)
            {
                sBL.UpdateAllGenresOfSongBeforeToFalse(song.songId);

                if (this.AddGenresToSong(song.songId, listGenres)) {Console.Clear(); Console.WriteLine("Updated song's genres");} 
                else {Console.Clear(); Console.WriteLine("Couldn't update this song's genres"); }
            }
            else { Console.Clear(); Console.WriteLine("Nothing change to this song's genres !"); }
        }
        return listGenres;
    }
    private void DisplaySong(Song s, bool staff)
    {
        using (null)
        {
            //Convert List<Genres> to string to display
            string genres = this.ConvertListGenreToString(s.genres);

            string singerAndBand = UIHelper.RemoveCoincideSingerOrBand(s);
            string writer = this.ConvertWriterToString(s.writer);
            string producer = this.ConvertProducerToString(s.produced);

            var table = new ConsoleTable("Title", $"{s.songName}");
            if (staff)
            {
                table.AddRow("Id", $"{s.songId}");
            }
            table.AddRow("Artist", $"{singerAndBand}");
            table.AddRow("Duration", $"{UIHelper.SongLengthConvert(s.length)}");
            table.AddRow("Genres", $"{genres}");
            table.AddRow("Written By", $"{writer}");
            table.AddRow("Produced By", $"{producer}");
            table.AddRow("Album: ", $"{s.album}");
            table.AddRow("Copyright: ", $"{s.copyright}");
            table.AddRow("Release Date: ", $"{((Persistence.Enum.Month)s.releaseDate.Month).ToString()} {s.releaseDate.Day}, {s.releaseDate.Year}");
            if (staff)
            {
                table.AddRow("Status", $"{s.songStatus}");
            }
            table.Write();
        }
    }
    private string ConvertListGenreToString(List<Genres> genres)
    {
        string genresString = "";
        if (genres !=null) foreach(var val in genres) if (val.genreStatus) genresString += ", " + val.genreTitle ;
        if (genresString.Length > 0) genresString = genresString.Remove(0,2);
        return genresString.Trim();
    }
    private string ConvertWriterToString(List<Artists> listArtist)
    {
        string theString = "";
        if (listArtist != null) foreach(var val in listArtist) theString += ", " + val.songwriterAlias;
        if (theString.Length > 0) theString = theString.Remove(0,2);
        return theString.Trim();
    }
    private string ConvertProducerToString(List<Artists> listArtist)
    {
        string theString = "";
        if (listArtist != null) foreach(var val in listArtist) theString += ", " + val.producerAlias;
        if (theString.Length > 0) theString = theString.Remove(0,2);
        return theString.Trim();
    }
    private Song ChooseArtistsFromArtistsPages(Customer customer, Song song)
    {
        bool endLoop = false;
        int pageIndex = 1;
        Song decoy = new Song();
        decoy.singer = song.singer.Union(song.band).ToList();
        
        int pageNum = UIHelper.GetNumberOfPages(decoy.singer.Count());

        Console.Clear();
        if (pageNum == 0)
        {
            Console.WriteLine("Sorry! We dont have any artists :(");
        }
        else
        {
            do
            {
                Console.Clear();
                UIHelper.MenuTitle(" Displaying The Artist List");
                int maxRange = pageIndex*10;
                DisplayArtistsList(pageIndex, decoy.singer);
                Console.WriteLine("View an Artist Information: ID, Next Page: N, Previous Page: P, Exit: E");
                Console.Write("# YOUR CHOICE: ");
                string choose = Console.ReadLine();

                switch (choose)
                {
                    case "n":
                        Console.Clear();
                        pageIndex++;
                        break;
                    case "p":
                        Console.Clear();
                        pageIndex--;
                        break;
                    case "e":
                        Console.Clear();
                        endLoop = true;
                        break;
                    default:
                        if ( Int32.TryParse(choose, out int k) )
                        {
                            Console.Clear();
                            if(maxRange > decoy.singer.Count)
                            {
                                maxRange = decoy.singer.Count;
                            }
                            if ( k >= (pageIndex-1)*10 && k< (maxRange))
                            {
                                //DisplayArtist(artistsList[k], customer.staff);
                                using (null)
                                {
                                    ArtistMenu aMenu = new ArtistMenu();
                                    decoy.singer[k] = aMenu.display_ArtistsMenu(customer, decoy.singer[k]);
                                }
                            }
                        }
                        break;
                }

                if (pageIndex <= 0)
                {
                    Console.WriteLine("Sorry! We dont have a negative page :(");
                    Console.WriteLine("We'll get you back to the first page");
                    pageIndex = 1; // first page number = 1
                }

                if (pageIndex > pageNum)
                {
                    Console.WriteLine("Sorry! We dont have that page :(");
                    Console.WriteLine("We'll get you back to the last page");
                    pageIndex = pageNum; // last page number = pageNu,
                }
                
            }
            while(!endLoop);

        }
        return decoy;
    
    }
    private void DisplayArtistsList(int pageIndex, List<Artists> artistList)
    {
        using (null)
        {
            int maxRange = 10*pageIndex;// A page only has 10 element so this why the number 10 is here. And why maxRange ?
                                        // so, if songList only has 15 elements, u will have 2 page. Right? And now if u dont have
                                        // the maxRange, your "i" loop will run to 2*10 = 20, but u only have 15 elements. its
                                        // overange. And maxRange is the hero save your day. Read all code to understand
            //elementNum = elementNum - count;
            if (maxRange >= artistList.Count)
            {
                maxRange = artistList.Count;
            }
            var table = new ConsoleTable("#","Stage Name", "Band Name" , "Full Name", "Gender","Birthday");
            for (int i = (pageIndex-1)*10; i< maxRange; i++)
            {
                table.AddRow(i, artistList[i].stageName, artistList[i].bandName, artistList[i].artistFirstName + " " + artistList[i].artistLastName, artistList[i].gender, artistList[i].born.ToString("dd/MM/yyyy "));
            }
            table.Write();
        }
    }
    private void p_download_song(string link, string songName)
    {
        using (null)
        {
            string format = ".mp3";
            //AtomicDownload auto = new AtomicDownload();
            AtomicDownload.DownloadFile(link, songName, format);
        }
    }
    private bool AddGenresToSong(int songId, List<Genres> genreList)
    {
        bool result = true;
        foreach(var val in genreList)
        {
            if(!sBL.AddGenresToSong(songId, val.genreId))
            {
                Console.WriteLine("Fail to add these genres to this song");
                result = false;
            }
        }
        return result;
    }

    private bool AddWriterToSong(int songId, List<Artists> artistList)
    {
        bool result = true;
        foreach(var val in artistList)
        {
            if(!sBL.AddWriterToSong(songId, val.artistId))
            {
                Console.WriteLine("Fail to add these artists to this song");
                result = false;
            }
        }
        return result;
    }
    private bool AddProducerToSong(int songId, List<Artists> artistList)
    {
        bool result = true;
        foreach(var val in artistList)
        {
            if(!sBL.AddProducerToSong(songId, val.artistId))
            {
                Console.WriteLine("Fail to add these artists to this song");
                result = false;
            }
        }
        return result;
    }
    private bool AddBandToSong(int songId, List<Artists> artistList)
    {
        bool result = true;
        foreach(var val in artistList)
        {
            if(!sBL.AddBandToSong(songId, val.artistId))
            {
                Console.WriteLine("Fail to add these band to this song");
                result = false;
            }
        }
        return result;
    }
    private bool AddSingerToSong(int songId, List<Artists> artistList)
    {
        bool result = true;
        foreach(var val in artistList)
        {
            if(!sBL.AddSingerToSong(songId, val.artistId))
            {
                Console.WriteLine("Fail to add these singer to this song");
                result = false;
            }
        }
        return result;
    }
    private List<Genres> InsertGenresToSong()
    {
        List<Genres> songGenre = new List<Genres>();
        do
        {
            Console.Clear();
            UIHelper.MenuTitle($" Changing The Song's Genres");
            Console.Write("\n> Genres: ");
            foreach (var item in songGenre) Console.Write($"{item.genreTitle}, ");
            Console.WriteLine();

            Console.WriteLine("\nPlease choose the genre for the song !");
            MainMenu mm = new MainMenu();
            Genres chosenGenre = mm.ChooseGenreToAddToSong(songGenre);
            if(chosenGenre != null)
            {
                //UIHelper.DrawALine();
                songGenre.Add(chosenGenre);
                Console.WriteLine(" #Continute? ");
            }
            else Console.WriteLine("Do you want to retry? ");

        }
        while (YNQuest.ask_YesOrNo());

        return songGenre;
    }
    private Song InsertBandOrSingerToSong()
    {
        Song decoySong = new Song();
        decoySong.singer = this.InsertSingerToSong();
        if(decoySong.singer != null)
        {
            List<Artists> copSingerList = new List<Artists>();
            foreach(var val in decoySong.singer) copSingerList.Add(val);
            List<Artists> isBand = DetectSingerIfTheyCanBeABand(copSingerList);
            if (isBand != null) 
            { 
                if (decoySong.band != null) decoySong.band = decoySong.band.Union(isBand).ToList();
                else decoySong.band = isBand;

                foreach(var val in isBand) decoySong.singer.RemoveAt(decoySong.singer.FindIndex(x=>x.artistId.Equals(val.artistId)));
            }
        }
        return decoySong;
    }
    private List<Artists> DetectSingerIfTheyCanBeABand(List<Artists> artistList)
    {
        List<Artists> isBand = new List<Artists>();
        List<Artists> bandList = aBL.GetListOfActiveBand();
        for(int i = 0; i<artistList.Count(); i++) //Remove non-band singer
        {
            if (!artistList[i].isBand)  artistList.RemoveAt(i);
        } 
        while (artistList.Count() > 1) //a band need atlease 2 persons right?
        {
            var theSingerList = (from ss in artistList where ss.bandName.ToLower().Contains(artistList[0].bandName.ToLower()) select ss).ToList();
            var theBandList = (from bb in bandList where bb.bandName.ToLower().Contains(artistList[0].bandName.ToLower()) select bb).ToList();
            if (theBandList.Count() == theSingerList.Count())
            {
                isBand = isBand.Union(theBandList).ToList();
                foreach(var val in theSingerList) artistList.Remove(val);
            } 
            else foreach(var val in theSingerList) artistList.Remove(val);
        } 
        return isBand;
    }
    private List<Artists> InsertSingerToSong()
    {
        Artists chosenArtist = new Artists();
        List<Artists> songArtist = new List<Artists>();
        do
        {
            Console.Clear();
            UIHelper.MenuTitle($" Changing Song's Artist");
            Console.Write("\n> Artists: ");
            foreach (var item in songArtist) Console.Write($"{item.stageName}, ");
            Console.WriteLine("\n");
            MainMenu mm = new MainMenu();
            chosenArtist = mm.ChooseArtistToAddToSong(Persistence.Enum.ArtistType.Singer, songArtist);
            if (chosenArtist != null) 
            {
                songArtist.Add(chosenArtist);
                Console.Write("Continute? ");
            }
            else Console.WriteLine("Do you want to retry? ");
        }
        while (YNQuest.ask_YesOrNo());

        return songArtist;
    
    }
    private List<Artists> InsertWriterToSong()
    {
        List<Artists> songArtist = new List<Artists>();
        do
        {
            Console.Clear();
            UIHelper.MenuTitle($" Changing Song's Composer");
            Console.Write("\n> Written By: ");
            foreach (var item in songArtist) Console.Write($"{item.songwriterAlias}, ");
            Console.WriteLine("\n");
            MainMenu mm = new MainMenu();
            Artists chosenArtist =  mm.ChooseArtistToAddToSong(Persistence.Enum.ArtistType.Writer, songArtist);
            if (chosenArtist != null) 
            {
                //UIHelper.DrawALine();
                songArtist.Add(chosenArtist);
                Console.Write("Continute? ");
            }
            else Console.WriteLine("Do you want to retry? ");
        }
        while (YNQuest.ask_YesOrNo());

        return songArtist;
    
    }
    private List<Artists> InsertProducerToSong()
    {
        List<Artists> songArtist = new List<Artists>();
        do
        {
            Console.Clear();
            UIHelper.MenuTitle($" Changing Song's Producer");
            Console.Write("\n> Produced By: ");
            foreach (var item in songArtist) Console.Write($"{item.producerAlias}, ");
            Console.WriteLine("\n");

            MainMenu mm = new MainMenu();
            Artists chosenArtist = mm.ChooseArtistToAddToSong(Persistence.Enum.ArtistType.Producer, songArtist);
            if (chosenArtist != null) 
            {
                songArtist.Add(chosenArtist);
                Console.Write("Continute? ");
            }
            else Console.WriteLine("Do you want to retry? ");
        }
        while (YNQuest.ask_YesOrNo());

        return songArtist;
}
}
