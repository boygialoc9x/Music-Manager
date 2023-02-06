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
    private CategoriesBL cateBL = new CategoriesBL();
    private SongBL sBL = new SongBL();
    private PlaylistBL plBL = new PlaylistBL();
    private ArtistsBL aBL = new ArtistsBL();
    //private static string[] menuOption = MagicNumber.songMenu_menuOption;
    private static string menuTitle = MagicNumber.songMenu_menuTitle;
    public Song display_SongMenu(Customer customer, Song song)
    {
        //Song copSong = song;
        bool EndloginLoop = true;

        ArrayList mOption = MagicNumber.songMenu_Remove_NonePermOption( customer.staff, customer.premium);
        int removingNumber = MagicNumber.songMenu_Get_numberOfRemovingMenuOption(customer.staff, customer.premium);

        while (EndloginLoop)
        {
            this.DisplaySong(song, customer.staff);
            //var ChonMenu = this.p_choose_optionMenu(mOption);
            //Console.ResetColor();
            var ChonMenu = UIHelper.ChooseOptionMenu(mOption, menuTitle);
            if ( Int32.TryParse(ChonMenu, out int k) )
            {
                if(k > mOption.Count - 1) 
                {                         
                k = k + removingNumber;   
                ChonMenu = k.ToString();  
                }                              
            }

            switch (ChonMenu)
            {
                case "1":
                    Console.Clear(); 
                        if ( song.lyric.Length > 1) //a song's lyric length always > 1 Right ?
                        {
                            Console.WriteLine(song.lyric);
                        }
                        else
                        {
                            Console.WriteLine("This song has no lyrics yet :(");
                        }             
                    break;
                case "2":
                    Console.Clear();
                    if (GetIP.CheckConnect())
                    {
                        this.p_download_song(song.downloadLink, song.songName);
                    }
                    else
                    {
                        Console.WriteLine("You are not connected to the internet !!");
                    }
                    break;
                case "3":
                    Console.Clear();
                    song.Artists = this.DisplayArtistMenu(customer, song);
                    break;
                case "4":
                    Console.Clear();
                    using (null)
                    {
                        Song s = InsertNewSong(song);
                        if (s!= null && s!=song)
                        {
                            song =  this.UpdateSong(s, song);
                            this.DisplaySong(song, customer.staff);
                        }
                        else
                        {
                            Console.WriteLine("Cancel update this song !!");
                        }
                    }
                    break;
                case "5":
                    Console.Clear();
                    using (null)
                    {
                        Console.WriteLine("Enter song's lyric: ");
                        Console.Write("> ");
                        string theLyric = Console.ReadLine();
                        Console.WriteLine("Save this change ? ");
                        if(YNQuest.ask_YesOrNo())
                        {
                            if (sBL.UpdateSongLyric(song.songId, theLyric)) 
                            {
                                Console.WriteLine("Update success !");
                                song.lyric = sBL.GetSongLyric(song.songId);
                            }
                            else Console.WriteLine("Update fail!");
                        }
                        else
                        {
                            Console.WriteLine("Cancel update :(");
                        }
                    }
                    break;
                case "6":
                    Console.Clear();
                    using (null)
                    {
                        Console.WriteLine("Enter song's download link: ");
                        Console.Write("> ");
                        string theDownloadLink = Console.ReadLine();
                        Console.WriteLine("Save this change ? ");
                        if(YNQuest.ask_YesOrNo())
                        {
                            if (sBL.UpdateSongDownloadLink(song.songId, theDownloadLink) ) 
                            {
                                Console.WriteLine("Update success !");
                                song.downloadLink = theDownloadLink;
                            }
                            else Console.WriteLine("Update fail!");
                        }
                        else
                        {
                            Console.WriteLine("Cancel update :(");
                        }
                    }
                    break;
                case "7":
                    Console.Clear();
                    using(null)
                    {
                        Console.Write("Active this song? ");
                        bool status = YNQuest.ask_YesOrNo();
                        Console.WriteLine("Save this change ? ");
                        if(YNQuest.ask_YesOrNo())
                        {
                            if (sBL.UpdateSongStatus(song.songId, status))
                            {
                                Console.WriteLine("Update Success !");

                                if (!status) if (plBL.AutoUpdateAllActiveSongBeforeToFalseAfterItsStatusChanged(song.songId)) 
                                {
                                    Console.WriteLine("Updated in all playlists success !");
                                }

                                song.songStatus = status;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Cancel update :(");
                        }
                    }
                    break;
                case "8":
                    Console.Clear();
                    Console.WriteLine("Exit");
                    EndloginLoop = false;
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
    private void DisplaySong(Song s, bool staff)
    {
        using (null)
        {
            //Convert List<Categories> to string to display
            //string lmao = s.Catogories.ToString();
            string categories = null;
            foreach(var val in s.Catogories)
            {
                if (val.categoryStatus)
                {
                    categories += val.categoryName + " ";
                }
            }
            //Console.WriteLine(categories);

            string theArtist = removeCoincideArtist(s);
            //Console.WriteLine(theArtist);

            var table = new ConsoleTable("", "");
            if (staff)
            {
                table.AddRow("Song's Id", $"{s.songId}");
            }
            table.AddRow("Song's Name", $"{s.songName}");
            table.AddRow("Song's Artist", $"{theArtist}");
            table.AddRow("Song's Length", $"{SongLengthConvert(s.length)}");
            table.AddRow("Song's Categoies", $"{categories}");
            //table.AddRow("Song's Categoies", $"{lmao}");
            if (staff)
            {
                table.AddRow("Song's Status", $"{s.songStatus}");
            }
            table.Write();
        }
    }
    public string removeCoincideArtist(Song s)
    {
        List<string> artist = new List<string>();
        string theArtist = "";
        if (s.Artists != null)
        {
            foreach(var val in s.Artists)
            {
                artist.Add(val.theArtist.ToString());
            }

            List<string> newCopA = artist.Distinct().ToList();
            foreach(var val in newCopA)
            {
                theArtist += " " + val;
            }
        }
        return theArtist.Trim();
    }
    private string SongLengthConvert(int length)
    {   
        int secondToMinute = 60; // 60s = 1m
        string songLength = (length / secondToMinute).ToString() + ":"+ (length % secondToMinute ).ToString(); 
        return songLength;
    }
    private List<Artists> DisplayArtistMenu(Customer cus, Song s)
    {
        using(null)
        {
            int pageNum = UIHelper.GetNumberOfPages(s.Artists.Count());

            return DisplayArtistPages(cus, s.Artists, pageNum );
        }
    }
    private void DisplayArtistsList(int pageIndex, List<Artists> ArtistsList)
    {
        using (null)
        {
            int maxRange = 10*pageIndex;// A page only has 10 element so this why the number 10 is here. And why maxRange ?
                                        // so, if songList only has 15 elements, u will have 2 page. Right? And now if u dont have
                                        // the maxRange, your "i" loop will run to 2*10 = 20, but u only have 15 elements. its
                                        // overange. And maxRange is the hero save your day. Read all code to understand
            //elementNum = elementNum - count;
            if (maxRange >= ArtistsList.Count)
            {
                maxRange = ArtistsList.Count;
            }
            //Console.WriteLine((pageIndex-1)*10 +1 + " - " + maxRange);
            var table = new ConsoleTable("ID", "Artist's Name", "Pseudonym/Band's name");
            for (int i = (pageIndex-1)*10; i< maxRange; i++)
            {
                //table.AddRow(i,songList[i].songName, removeCoincideArtist(songList[i]));
                table.AddRow(i,ArtistsList[i].artistFirstName +" "+ ArtistsList[i].artistLastName, ArtistsList[i].theArtist);
            }
            table.Write();
        }
    }
    // private int GetNumberOfArtistPages(List<Artists> artistsList)
    // {
    //     int numberOfelements = artistsList.Count;
    //     int numberOfelements_PerPage = 10;
        
    //     int numberOfpage = numberOfelements / numberOfelements_PerPage;

    //     if ( numberOfelements % numberOfelements_PerPage > 0)
    //     {
    //         numberOfpage ++;
    //     }
        
    //     return numberOfpage;
    // }
    private List<Artists> DisplayArtistPages(Customer customer, List<Artists> artistsList, int pageNum)
    {
        bool endLoop = false;
        int pageIndex = 1;

        using (null)
        {
            Console.Clear();
            if (pageNum == 0)
            {
                Console.WriteLine("Sorry! We dont have any artists :(");
            }
            else
            {
                do
                {
                    int maxRange = pageIndex*10;
                    DisplayArtistsList(pageIndex, artistsList);
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
                                if(maxRange > artistsList.Count)
                                {
                                    maxRange = artistsList.Count;
                                }
                                if ( k >= (pageIndex-1)*10 && k< (maxRange))
                                {
                                    //DisplayArtist(artistsList[k], customer.staff);
                                    using (null)
                                    {
                                        ArtistMenu aMenu = new ArtistMenu();
                                        artistsList[k] = aMenu.display_ArtistsMenu(customer, artistsList[k]);
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
            return artistsList;
        }
    }
    // private void DisplayArtist(Artists a, bool staff)
    // {
    //     using (null)
    //     {
    //         var table = new ConsoleTable("", "");
    //         if (staff)
    //         {
    //             table.AddRow("Artist's Id", $"{a.artistId}");
    //         }
    //         table.AddRow("Artist's first name", $"{a.artistFirstName}");
    //         table.AddRow("Artist's last name", $"{a.artistLastName}");
    //         table.AddRow("Pseudonym/Band's name", $"{a.theArtist}");
    //         table.AddRow("Birthday", $"{a.born.ToString("dd/MM/yyyy")}");
    //         table.Write();
    //     }
    // }
    private string p_get_songLyric_byDatabase(int songId)
    {
        return sBL.GetSongLyric(songId);
        //Console.WriteLine(lyric);
    }

    private string p_get_downloadLinkSong_byDatabase(int songId)
    {
        return sBL.GetSognDownloadLink(songId);
        //Console.WriteLine(downloadLink);
    }
    private void p_display_songLyric_bySongList(Song song)
    {
        Console.WriteLine(song.lyric);
    }

    private void p_display_songDownloadLink_bySongList(Song song)
    {
        Console.WriteLine(song.downloadLink);
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
    private Song UpdateSong(Song newSong, Song oldSong)
    {
        using (null)
        {    
            Song updatedSong = newSong;
            Console.Write("Save these change? ");
            if (YNQuest.ask_YesOrNo())
            {
                updatedSong = newSong;
                if (sBL.UpdateSongInfor(newSong) && sBL.UpdateSongDownloadLink(newSong.songId, newSong.downloadLink) && sBL.UpdateSongLyric(newSong.songId, newSong.lyric) && sBL.UpdateSongStatus(newSong.songId, newSong.songStatus))
                {
                    if (!newSong.songStatus) if ( plBL.AutoUpdateAllActiveSongBeforeToFalseAfterItsStatusChanged(newSong.songId) ) Console.WriteLine("Updated in all playlists success !");
/*  UPDATE CATEGORIES TO SONG */
                    if (newSong.Catogories != oldSong.Catogories)
                    {
                        sBL.UpdateAllCategoriesOfSongBeforeToFalse(newSong.songId);
                        List<Categories> noneActiveCate = cateBL.GetNoneActiveCategoryOfSong(oldSong.songId);
                        List<Categories> addRemoveCate = new List<Categories>();
                        if (noneActiveCate != null)
                        {                        
                            for (int i=0; i<noneActiveCate.Count; i++)
                            {
                                for(int j=0; j<newSong.Catogories.Count; j++)
                                {
                                    if (noneActiveCate[i].categoryId == newSong.Catogories[j].categoryId)
                                    {
                                        if (sBL.ReActiveCategoryOfSong(newSong.songId, newSong.Catogories[j].categoryId))
                                        {
                                            addRemoveCate.Add(newSong.Catogories[j]);
                                            newSong.Catogories.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (newSong.Catogories != null)
                        {
                            if (this.AddNewCategoriesToSong(newSong, newSong.Catogories))
                            {
                                Console.WriteLine("Updated song's categories");
                            }
                            else
                            {
                                Console.WriteLine("Couldn't update this song's category");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Updated song's categories");
                        }
                        if (addRemoveCate != null)
                        {
                            foreach(var val in addRemoveCate)
                            {
                                updatedSong.Catogories.Add(val);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Updated song's categories");
                    }
/*  UPDATE ARTISTS TO SONG */
                    if (newSong.Artists != oldSong.Artists)
                    {
                        sBL.UpdateAllArtitssOfSongBeforeToFalse(newSong.songId);
                        List<Artists> noneActiveArtist = aBL.GetNoneActiveArtistsOfSong(oldSong.songId);
                        List<Artists> addRemoveArtist = new List<Artists>();
                        if (noneActiveArtist != null)
                        {                        
                            for (int i=0; i<noneActiveArtist.Count; i++)
                            {
                                for(int j=0; j<newSong.Artists.Count; j++)
                                {
                                    if (noneActiveArtist[i].artistId == newSong.Artists[j].artistId)
                                    {
                                        if (sBL.ReActiveArtistOfSong(newSong.songId, newSong.Artists[j].artistId))
                                        {
                                            addRemoveArtist.Add(newSong.Artists[j]);
                                            newSong.Artists.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (newSong.Artists != null)
                        {
                            if (this.AddNewArtistToSong(newSong, newSong.Artists))
                            {
                                Console.WriteLine("Updated song's artists");
                            }
                            else
                            {
                                Console.WriteLine("Couldn't update this song's artists");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Updated song's artists");
                        }
                        if (addRemoveArtist != null)
                        {
                            foreach(var val in addRemoveArtist)
                            {
                                updatedSong.Artists.Add(val);
                            }
                        }
                    }
                    else Console.WriteLine("Updated song's artists");
                }
                else Console.WriteLine("Some thing wrong! Couldn't update this song :(");
            }
            else
            {
                Console.WriteLine("Cancel update!");
                updatedSong = oldSong;
            }
            return updatedSong;
        }
    }
    private Song RemoveCoincideCategory(Song s)
    {
        for (int i = 0; i < s.Catogories.Count; i++)
        {
            for(int j = i+1; j < s.Catogories.Count; j++)
            {
                if (s.Catogories[i].categoryId == s.Catogories[j].categoryId)
                {
                    s.Catogories.RemoveAt(j);
                }
            }
        }
        return s;
    }
    private Song InsertNewSong(Song song)
    {
        using (null)
        {
            Song nS = new Song();
            nS.songId = song.songId;
            
            Console.WriteLine("Do you want to change this song's name? ");
            if (YNQuest.ask_YesOrNo())
            {
                nS.songName = StringUTil.GetValidName("Enter song's name: ");
                if (nS.songName != null)
                {
                    nS.songName = StringUTil.UpperFirstLecter(nS.songName);
                }
            }
            else
            {
                nS.songName = song.songName;
            }
            
            if ( nS.songName != null )
            {
                Console.WriteLine("Do you want to change this song's artists? ");
                if (YNQuest.ask_YesOrNo())
                {
                    nS.Artists = this.InsertArtistToSong();
                    if (nS.Artists == null)
                    {
                        Console.WriteLine("Cancel change this song's artists");
                        nS.Artists = song.Artists;
                    }
                }
                else
                {
                    nS.Artists = song.Artists;
                }

                Console.WriteLine("Do you want to change this song's length? ");
                if (YNQuest.ask_YesOrNo())
                {
                    nS.length = StringUTil.GetValidNumber("Enter song's length: ");
                }
                else
                {
                    nS.length = song.length;
                }

                if (nS.length > 0)
                {
                    Console.WriteLine("Do you want to change this song's lyric? ");
                    if (YNQuest.ask_YesOrNo())
                    {
                        Console.Write("Enter song's lyric: ");
                        nS.lyric = Console.ReadLine();
                    }
                    else
                    {
                        nS.lyric = song.lyric;
                    }

                    Console.WriteLine("Do you want to change this song's download link? ");
                    if (YNQuest.ask_YesOrNo())
                    {
                        Console.Write("Enter song's download link: ");
                        nS.downloadLink = Console.ReadLine();
                    }
                    else
                    {
                        nS.downloadLink = song.downloadLink;
                    }

                    Console.Write("Do you want to change this song's category? ");
                    if (YNQuest.ask_YesOrNo())
                    {
                        nS.Catogories = this.InsertCategoriesToSong();
                    }
                    else
                    {
                        nS.Catogories = song.Catogories;
                    }

                    Console.Write("Active this song? ");
                    nS.songStatus = YNQuest.ask_YesOrNo();
                }
                else
                {
                    nS = null;
                }
            }
            else
            {
                nS = null;
            }

            if (nS != null)
            {
                nS = RemoveCoincideCategory(nS);
            }
            
            return nS;
        }      
    }
    private bool AddNewCategoriesToSong(Song s, List<Categories> cateList)
    {
        bool result = true;
        foreach(var val in cateList)
        {
            if(!sBL.AddCategoriesToSong(s.songId, val.categoryId))
            {
                Console.WriteLine("Fail to add these categories to this song");
                result = false;
            }
        }
        return result;
    }

    private bool AddNewArtistToSong(Song s, List<Artists> artistList)
    {
        bool result = true;
        foreach(var val in artistList)
        {
            if(!sBL.AddArtistsToSong(s.songId, val.artistId))
            {
                Console.WriteLine("Fail to add these artists to this song");
                result = false;
            }
        }
        return result;
    }

    private List<Artists> InsertArtistToSong()
    {
        using (null)
        {
            List<Artists> songArtist = new List<Artists>();
            do
            {
                string theArtist = StringUTil.GetValidName("Enter artist's pseudonym: ");

                if (theArtist != null)
                {
                    if (!aBL.ChecktheArtist(theArtist))
                    {
                        Console.WriteLine("These aritsts do not exist!\n");
                        Console.Write("Do you want to re-enter the artist's pseudonym? ");
                    }
                    else
                    {
                        List<Artists> artistsInaBand = aBL.GetArtistsInforListBytheArtist(theArtist);
                        foreach(var val in artistsInaBand)
                        {
                            songArtist.Add(val);
                        }
                        Console.Write("Continute? ");
                    }
                }
                else
                {
                    Console.Write("Do you want to re-enter the artist's pseudonym? ");
                }
            }
            while (YNQuest.ask_YesOrNo());

            return songArtist;
        }
    }
    private List<Categories> InsertCategoriesToSong()
    {
        using (null)
        {
            List<Categories> songCate = new List<Categories>();
            do
            {
                string categoryName = StringUTil.GetNotEmptyString("Enter category's name: ");
                if (categoryName != null)
                {
                    if (!cateBL.CheckCategoryName(categoryName))
                    {
                        Console.WriteLine("This category doesn't exist!\n");
                        Console.WriteLine("Do you want to add this new category to database ?: ");
                        if (YNQuest.ask_YesOrNo())
                        {
                            int newCateId = cateBL.GetMaxIdInCategories() + 1;
                            Categories nCate = (this.InsertNewCategory(newCateId, categoryName));

                            this.AddNewCategory(nCate);

                            songCate.Add(nCate);
                        }
                    }
                    else
                    {
                        Categories nCate = cateBL.GetCategoryInforByName(categoryName);
                        songCate.Add(nCate);
                    }
                    Console.Write("Continute? ");
                }
                else
                {
                    Console.WriteLine("Do you want to re-enter the song's category ?");
                }
            }
            while (YNQuest.ask_YesOrNo());

            return songCate;
        }
    }
    private Categories InsertNewCategory(int cateId, string categoryName)
    {
        using (null)
        {
            Categories nCate = new Categories();

            nCate.categoryId = cateId;

            if (categoryName != null)
            {
                string str = 
                nCate.categoryName = StringUTil.UpperFirstLecter(categoryName);
                nCate.categoryStatus = true;
            }
            else
            {
                nCate.categoryName = StringUTil.GetNotEmptyString("Enter category's name: ");
                if (nCate.categoryName != null)
                {
                    //Console.Write("Active this category? ");
                    //nCate.categoryStatus = YNQuest.ask_YesOrNo();
                    nCate.categoryStatus = true;
                }
                else
                {
                    nCate = null;
                }
            }

            return nCate;
        }
    }
    private void AddNewCategory(Categories nCate)
    {
        if (cateBL.AddNewCategory(nCate))
        {
            Console.WriteLine("This category added to database!");
        }
        else
        {
            Console.WriteLine("Some thing wrong! Couldn't add this category to database :(");
        }
    

    }
    
}
