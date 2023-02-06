using System;
using System.Collections.Generic;
using System.Collections;
using Persistence;
using BL;
using ConsolePL.UTIL;
using System.Linq;

using ConsoleTables;

public class MainMenu
{
    private static CustomerBL cBL = new CustomerBL();
    private static SongBL sBL = new SongBL();
    private static CategoriesBL cateBL = new CategoriesBL();
    private static ArtistsBL aBL = new ArtistsBL();
    private static PlaylistBL plBL = new PlaylistBL();
    private static string menuTitle = MagicNumber.mainMenu_menuTitle;
    private static List<Song> list_allSongs = sBL.GetAllSongsList();
    //Because we get all songs in database, get 1 time is enough.
    public void option_mainMenu(Customer customer, bool autoLogin)
    {

        bool EndloginLoop = true;

        ArrayList mOption = MagicNumber.mainMenu_Remove_NonePermOption(customer.staff, customer.premium);
        int removingNumber = MagicNumber.mainMenu_Get_numberOfRemovingMenuOption(customer.staff, customer.premium);
        PlaylistMenu pm = new PlaylistMenu();

        while (EndloginLoop)
        {
            //Console.Clear();
            var ChonMenu = UIHelper.ChooseOptionMenu(mOption, menuTitle);
            if ( Int32.TryParse(ChonMenu, out int k) )
            {
                if(k > mOption.Count - 4)
                {
                    k = k + removingNumber;
                    ChonMenu = k.ToString(); 
                }
            }                  

            switch (ChonMenu)
            {
                case "1":
                    Console.Clear();
                    this.DisplayAllSongMenu(customer);
                    break;
                case "2":
                    Console.Clear();
                    //Console.Write("Enter song's name: ");
                    string searchStr = StringUTil.GetNotEmptyString("Enter song's name: ");
                    if (searchStr != null)
                    {
                        this.DisplaySearchSongNameMenu(customer, searchStr);
                    }
                    else
                    {
                        Console.WriteLine("Cancel Search song by song's name");
                    }
                    break;
                case "3":
                    Console.Clear();
                    //Console.Write("Enter song's artist: ");
                    searchStr = StringUTil.GetNotEmptyString("Enter artist's name: ");
                    if (searchStr != null)
                    {
                        this.DisplaySearchSongArtistMenu(customer, searchStr);
                    }
                    else
                    {
                        Console.WriteLine("Cancel Search song by artist's name");
                    }
                    break;
                case "4":
                    //Console.Clear();
                    if (customer.staff)
                    {
                        string user_name = StringUTil.GetNotEmptyString("Enter user name: ");
                        if (user_name != null)
                        {
                            if (this.UpgradePremium_Staff(user_name))
                            {
                                Console.WriteLine("Success !!!!!!!!!!!!!!!!!!!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Cancel update!");
                        }

                    }
                    else
                    {
                        this.UpgradePremium_Customer(customer);
                    }
                    break;
                case "5":
                    Console.Clear();
                    using(null)
                    {
                        Song nS = this.InsertNewSong(sBL.GetMaxIdInSongs() + 1);
                        if (nS != null)
                        {
                            this.AddNewSong(nS);
                        }
                        else
                        {
                            Console.WriteLine("Cancel add a new song !!");
                        }
                    }
                    break;
                case "6":
                    Console.Clear();
                    using(null)
                    {
                        Categories cate = this.InsertNewCategory(sBL.GetMaxIdInSongs() + 1, null);
                        if(cate != null)
                        {
                            this.AddNewCategory(cate, false);
                        }
                        else
                        {
                            Console.WriteLine("Cancel add a new category !!");
                        }
                    }
                    break;
                case "7":
                    using (null)
                    {
                        Artists artist = this.InsertNewArtist(aBL.GetMaxIdInArtist()+1);
                        if(artist != null)
                        {
                            this.AddNewArtist(artist);
                        }
                        else
                        {
                            Console.WriteLine("Cancel add a new artist !!");
                        }
                    }
                    break;
                case "8":
                    Console.Clear();
                    pm.CreatePlaylist(customer, null); //input no clone playlist
                    break;
                case "9":
                    using (null)
                    {
                        if (customer.playlistCreated > 0)  
                        {
                            List<Playlist> list_playlist = plBL.GetActivePlaylistList(customer.userId);
                            Playlist pl = pm.ChoosePlaylistPages( customer, list_playlist, UIHelper.GetNumberOfPages( list_playlist.Count() ) );
                            customer.playlistCreated = cBL.GetCustomer(customer.user_name).playlistCreated;
                            if (pl != null)
                            {
                                List<Song> songListInPlayList = pm.display_PlaylistMenu( pl, UIHelper.RemoveNoneActiveSong(list_allSongs) );
                                list_allSongs = sBL.GetAllSongsList();
                                if (songListInPlayList.Count() > 0)
                                {
                                    this.DisplaySongPages(customer ,songListInPlayList, UIHelper.GetNumberOfPages(songListInPlayList.Count()));
                                } else Console.WriteLine("Sorry ! Your playlist have no song :(");
                                
                            }
                        } 
                        else 
                        {
                            Console.Clear();
                            Console.WriteLine("You have no playlist  !!");
                        }
                    }
                    break;
                case "10":
                    Console.Clear();
                    using(null)
                    {
                        ProfileMenu pMenu = new ProfileMenu();
                        customer = pMenu.display_ProfileMenu(customer);
                    }
                    break;
                case "11":
                    Console.WriteLine("Do you really want to log out ?");
                    if (YNQuest.ask_YesOrNo())
                    {
                        Console.Clear();
                        Console.WriteLine("Logged Out");
                        list_allSongs = sBL.GetAllSongsList();
                        customer = null;
                        EndloginLoop = false;

                        if (autoLogin)
                        {
                            Console.WriteLine("Do you want us to keep remember your account ?");
                            if (!YNQuest.ask_YesOrNo())
                            {
                                AutomaticLogin.RemoveCookie();
                            }
                        }
                    } else Console.Clear();
                    
                    break;
                default:

                    Console.Clear();

                    Console.WriteLine("Wrong Choice");
                    Console.WriteLine("Please TryAgain");

                    break;
            }
        
        }
    }
    private bool UpgradePremium_Staff(string user_name)
    {
        using (null)
        {
            bool result = false;

            if (cBL.CheckUserName(user_name))
            {
                Customer cus = cBL.GetCustomer(user_name);
                if (!cus.staff)
                {
                    if(!cus.premium)
                    {
                        Console.WriteLine("Are you sure about this? ");
                        if (YNQuest.ask_YesOrNo())
                        {
                            cBL.UpgradePremium_Staff(user_name);
                            result = true;
                        }
                        else
                        {
                            Console.WriteLine($"Cancel upgrade premium !");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{user_name} is already premium member");
                    }
                }
                else
                {
                    Console.WriteLine($"{user_name} is a staff member :))");
                }
            }
            else
            {
                Console.WriteLine($"{user_name} is not exist!!");
            }
            //Console.Clear();
            return result;
        }
    }
    private void UpgradePremium_Customer(Customer customer)
    {
        using (null)
        {
            Console.Clear();
            //bool result = false;
            if (customer.premium) Console.WriteLine("You are Premium Member already !");
            else
            {
                Console.WriteLine("Premium Member benefit: ");
                Console.WriteLine(" - You can create more playlist !");
                Console.WriteLine(" - View full song's lyric !");
                Console.WriteLine(" - Download completely a song !");
                Console.WriteLine(" - View artist's detail");
                Console.WriteLine(" - And lots of other features you can explore !");
                Console.WriteLine("All you need is: Contact to a Staff !");
                Console.WriteLine("Do you want us to send you a gmail about Premium member ?");
                if (YNQuest.ask_YesOrNo()) SendingCode.SendPremiumGmail(customer);
            }
            //return result;
        }
    }
    private Song InsertNewSong(int songId)
    {

        using (null){
            Song nS = new Song();
            nS.songId = songId;
            
            nS.songName = StringUTil.GetValidName("Enter song's name: ");
            if ( nS.songName != null )
            {
                nS.Artists = this.InsertArtistToSong();

                nS.length = StringUTil.GetValidNumber("Enter song's length: ");
                if (nS.length !=0)
                {
                    Console.WriteLine("Enter song's lyric: ");
                    nS.lyric = Console.ReadLine();
                    Console.WriteLine("Enter song's download link: ");
                    nS.downloadLink = Console.ReadLine();

                    nS.Catogories = this.InsertCategoriesToSong();

                    Console.Write("Active this song? ");
                    nS.songStatus = YNQuest.ask_YesOrNo();
                }
            }
            else
            {
                nS = null;
            }
            return nS;
        }      
    }
    private void AddNewSong(Song nS)
    {
        nS = this.RemoveCoincideCategory(nS);
        Console.Write("Save this song? ");
        if (YNQuest.ask_YesOrNo())
        {
            if (sBL.AddNewSong(nS) && this.AddCategoriesToSong(nS, nS.Catogories) && this.AddArtistToSong(nS, nS.Artists))
            {
                list_allSongs.Add(nS);
                Console.WriteLine("This song added to database!");
            }
            else
            {
                Console.WriteLine("Some thing wrong! Couldn't add this song to database :(");
            }
        }
        else
        {
            Console.WriteLine("Cancel add a new song !");
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
    private bool AddCategoriesToSong(Song s, List<Categories> cateList)
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

    private bool AddArtistToSong(Song s, List<Artists> artistList)
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
    private List<Categories> InsertCategoriesToSong()
    {
        using (null)
        {
            List<Categories> songCate = new List<Categories>();
            do
            {
                string categoryName = StringUTil.GetValidName("Enter category's name: ");
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

                            this.AddNewCategory(nCate, true);

                            songCate.Add(nCate);
                        }
                    }
                    else
                    {
                        Categories nCate = cateBL.GetCategoryInforByName(categoryName);
                        songCate.Add(nCate);
                    }
                }

                Console.Write("Continue? ");

            }
            while (YNQuest.ask_YesOrNo());

            return songCate;
        }
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
                    if (!aBL.ChecktheArtist(theArtist.ToLower()))
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
            }
            while (YNQuest.ask_YesOrNo());

            return songArtist;
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
                nCate.categoryName = StringUTil.UpperFirstLecter(categoryName);
                nCate.categoryStatus = true;
            }
            else
            {
                nCate.categoryName = StringUTil.GetValidName("Enter category's name: ");
                if (nCate.categoryName != null)
                {
                    nCate.categoryName = StringUTil.UpperFirstLecter(nCate.categoryName);
                    if (!cateBL.CheckCategoryName(nCate.categoryName))
                    {
                        //Console.Write("Active this category? ");
                        //nCate.categoryStatus = YNQuest.ask_YesOrNo();
                        nCate.categoryStatus = true;
                    }
                    else
                    {
                        Console.WriteLine($"This category's name \"{nCate.categoryName}\" is already in use");
                        Console.WriteLine("Do you want to retry?");
                        if ( YNQuest.ask_YesOrNo() )
                        {
                            nCate = InsertNewCategory(cateId, null);
                        }
                        else
                        {
                            nCate = null;
                        }
                    }    
                }
                else
                {
                    nCate = null;
                }
            }

            return nCate;
        }
    }
    private void AddNewCategory(Categories nCate, bool autoSave)
    {
        bool theAutoSave = false;

        if (autoSave) 
        {
            theAutoSave = autoSave;
        }
        else 
        {
            Console.Write("Save this Category? "); 
            theAutoSave = YNQuest.ask_YesOrNo();
        }

        if (theAutoSave)
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
        else
        {
            Console.WriteLine("Cancel Add a new category !");
        }
    }
    private Artists InsertNewArtist(int artistId)
    {
        Artists nArtist = new Artists();
        nArtist.artistId = artistId;

        nArtist.artistFirstName = StringUTil.GetValidName("Enter the artist's first name: ");
        if (nArtist.artistFirstName != null)
        {
            nArtist.artistFirstName = StringUTil.UpperFirstLecter(nArtist.artistFirstName);

            nArtist.artistLastName = StringUTil.GetValidName("Enter the artist's last name: ");
            if(nArtist.artistLastName != null)
            {
                nArtist.artistLastName = StringUTil.UpperFirstLecter(nArtist.artistLastName);

                nArtist.theArtist = StringUTil.GetValidName("Enter the artist's pseudonym / Band's name: ");
                if(nArtist.theArtist != null)
                {
                    nArtist.born = StringUTil.GetTheTrueBithday();
                    //Console.WriteLine(nArtist.born);
                    if (nArtist.born != DateTime.MinValue)
                    {
                        nArtist.artistStatus = true;
                    }
                    else
                    {
                        nArtist = null;
                    }
                }
                else
                {
                    nArtist = null;
                }
            }
            else
            {
                nArtist = null;
            }
        }
        else
        {
            nArtist = null;
        }
        return nArtist;
    }
    private void AddNewArtist(Artists a)
    {
        Console.Write("Save this Artist? "); 
        if (YNQuest.ask_YesOrNo())
        {
            if (aBL.AddNewArtist(a))
            {
                Console.WriteLine("This artist added to database!");
            }
            else
            {
                Console.WriteLine("Some thing wrong! Couldn't add this artist to database :(");
            }
        }
        else
        {
            Console.WriteLine("Cancel add a new artist");
        }
    }
    private void DisplayAllSongMenu(Customer cus)
    {
        using(null)
        {
            int pageNum = UIHelper.GetNumberOfPages(list_allSongs.Count());

            DisplaySongPages(cus, list_allSongs, pageNum );
        }
    }
    private void DisplaySearchSongNameMenu(Customer cus, string searchStr)
    {
        using(null)
        {

            //List<Song> searchSongNameList = new List<Song>();
            
            // foreach(var val in list_allSongs)
            // {
            //     if(val.songName.ToLower().Contains(searchStr.ToLower()))
            //     {
            //         searchSongNameList.Add(val);
            //     }
            // }

            // int pageNum = this.GetNumberOfSongPages(searchSongNameList);

            var songList = (from ss in list_allSongs where ss.songName.ToLower().Contains(searchStr.ToLower()) select ss).ToList();
            int pageNum = UIHelper.GetNumberOfPages(songList.Count());
            DisplaySongPages(cus, songList, pageNum);
        }
    }
    private void DisplaySearchSongArtistMenu(Customer cus, string searchStr)
    {
        using(null)
        {
            List<Song> searchSongArtistList = new List<Song>();

            foreach(var val in list_allSongs)
            {
                foreach(var item in val.Artists)
                {
                    if(item.theArtist.ToLower().Contains(searchStr.ToLower()))
                    {
                        searchSongArtistList.Add(val);
                        break;
                    }
                }
            }

            // var songList = list_allSongs.Where(
            //     (ss) => {
            //         return ss.Artists.Select(
            //             (aa) => {
            //                 return aa.theArtist;
            //             }
            //         ).Contains(searchStr.ToLower());
            //     }
            // ).ToList();

            int pageNum = UIHelper.GetNumberOfPages(searchSongArtistList.Count());

            DisplaySongPages(cus, searchSongArtistList, pageNum);
        }
        //Need to flush after search because we search many time n we dont have RAM for this.
    }

    // private int GetNumberOfPages(int numberOfelements)
    // {
    //     int numberOfelements_PerPage = 10;
        
    //     int numberOfpage = numberOfelements / numberOfelements_PerPage;

    //     if ( numberOfelements % numberOfelements_PerPage > 0)
    //     {
    //         numberOfpage ++;
    //     }
        
    //     return numberOfpage;
    // }
    private void DisplaySongPages(Customer customer, List<Song> songList, int pageNum)
    {
        bool endLoop = false;
        int pageIndex = 1;
        
        if (!customer.staff) songList = UIHelper.RemoveNoneActiveSong(songList);

        using (null)
        {
            Console.Clear();
            if (songList.Count <= 0)
            {
                Console.WriteLine("Sorry! We dont have any song :(");
            }
            else
            {
                do
                {
                    int maxRange = pageIndex*10;
                    UIHelper.DisplaySongList(pageIndex, songList);
                    Console.WriteLine("View a Song Information: ID, Next Page: N, Previous Page: P, Exit: E");
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
                                if(maxRange > songList.Count)
                                {
                                    maxRange = songList.Count;
                                }
                                if ( k >= (pageIndex-1)*10 && k< (maxRange))
                                {
                                    //this.DisplaySong(songList[k], customer.staff);
                                    using (null)
                                    {
                                        SongMenu sMenu = new SongMenu();
                                        Song songClone = sMenu.display_SongMenu(customer, songList[k]);
                                        list_allSongs[list_allSongs.FindIndex(x => x.songId == (songList[k].songId) )] = songClone;
                                        songList[k] = songClone;
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
        }
    }

    // private void DisplaySongList(int pageIndex, List<Song> songList)
    // {
    //     using (null)
    //     {
    //         SongMenu sMenu = new SongMenu();
    //         int maxRange = 10*pageIndex;// A page only has 10 element so this why the number 10 is here. And why maxRange ?
    //                                     // so, if songList only has 15 elements, u will have 2 page. Right? And now if u dont have
    //                                     // the maxRange, your "i" loop will run to 2*10 = 20, but u only have 15 elements. its
    //                                     // overange. And maxRange is the hero save your day. Read all code to understand
    //         //elementNum = elementNum - count;
    //         if (maxRange >= songList.Count)
    //         {
    //             maxRange = songList.Count;
    //         }
    //         //Console.WriteLine((pageIndex-1)*10 +1 + " - " + maxRange);
    //         var table = new ConsoleTable("ID", "Song's Name", "Artist");
    //         for (int i = (pageIndex-1)*10; i< maxRange; i++)
    //         {
    //             table.AddRow(i,songList[i].songName, sMenu.removeCoincideArtist(songList[i]));
    //         }
    //         table.Write();
    //     }
    // }
    
    // private List<Song> RemoveNoneActiveSong(List<Song> songList)
    // {
    //     for (int i=0; i<songList.Count; i++)
    //     {
    //         if (!songList[i].songStatus)
    //         {
    //             songList.RemoveAt(i);
    //             i--;
    //         }
    //     }
    //     return songList;
    // }

    // public void DisplaySong(Song s, bool staff)
    // {
    //     using (null)
    //     {
    //         //Convert List<Categories> to string to display
    //         //string lmao = s.Catogories.ToString();
    //         string categories = null;
    //         foreach(var val in s.Catogories)
    //         {
    //             if (val.categoryStatus)
    //             {
    //                 categories += val.categoryName + " ";
    //             }
    //         }
    //         //Console.WriteLine(categories);

    //         string theArtist = removeCoincideArtist(s);
    //         //Console.WriteLine(theArtist);

    //         var table = new ConsoleTable("", "");
    //         if (staff)
    //         {
    //             table.AddRow("Song's Id", $"{s.songId}");
    //         }
    //         table.AddRow("Song's Name", $"{s.songName}");
    //         table.AddRow("Song's Artist", $"{theArtist}");
    //         table.AddRow("Song's Length", $"{SongLengthConvert(s.length)}");
    //         table.AddRow("Song's Categoies", $"{categories}");
    //         //table.AddRow("Song's Categoies", $"{lmao}");
    //         if (staff)
    //         {
    //             table.AddRow("Song's Status", $"{s.songStatus}");
    //         }
    //         table.Write();
    //     }
    // }
    // private string removeCoincideArtist(Song s)
    // {
    //     List<string> artist = new List<string>();
    //     string theArtist = "";
    //     if (s.Artists != null)
    //     {
    //         foreach(var val in s.Artists)
    //         {
    //             artist.Add(val.theArtist.ToString());
    //         }

    //         List<string> newCopA = artist.Distinct().ToList();
    //         foreach(var val in newCopA)
    //         {
    //             theArtist += " " + val;
    //         }
    //     }
    //     return theArtist.Trim();
    // }
    // private string SongLengthConvert(int length)
    // {   
    //     int secondToMinute = 60; // 60s = 1m
    //     string songLength = (length / secondToMinute).ToString() + ":"+ (length % secondToMinute ).ToString(); 
    //     return songLength;
    // }
}