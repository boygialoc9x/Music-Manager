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
    private static OrderBL orderBL = new OrderBL();
    private static GenresBL gBL = new GenresBL();
    private static ArtistsBL aBL = new ArtistsBL();
    private static string menuTitle = MagicNumber.mainMenu_menuTitle;
    private static List<Song> list_allSongs = sBL.GetAllSongsList();
    //Because we get all songs in database, get 1 time is enough.
    public void option_mainMenu(Customer customer, bool autoLogin)
    {

        bool EndloginLoop = true;

        ArrayList mOption = MagicNumber.mainMenu_Remove_NonePermOption(customer.staff, customer.premium);
        int removingNumber = MagicNumber.mainMenu_Get_numberOfRemovingMenuOption(customer.staff, customer.premium);

        while (EndloginLoop)
        {
            //Console.Clear();
            var ChonMenu = UIHelper.ChooseOptionMenu(mOption, menuTitle);
            if ( Int32.TryParse(ChonMenu, out int k) )
            {
                if(k > mOption.Count - 2)
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
                    this.DisplaySearchSongNameMenu(customer);
                    break;
                case "3":
                    Console.Clear();
                    this.DisplaySearchSongArtistMenu(customer);
                    break;
                case "4":
                    Console.Clear();
                    customer.premium = this.UpgradePremium_Customer(customer);
                    break;
                case "5":
                    Console.Clear();
                    
                    Song nS = this.InsertNewSong();
                    if (nS != null) this.AddNewSong(nS);
                    else 
                    {
                        Console.Clear();
                        Console.WriteLine("Cancel add a new song !!");
                    }
                    break;
                case "6":
                    Console.Clear();
                    Genres genre = this.InsertNewGenre();
                    if(genre != null) this.AddNewGenre(genre);
                    else Console.WriteLine("Cancel add a new genre !!"); 
                    break;
                case "7":
                    Console.Clear();
                    Artists artist = this.InsertNewArtist();
                    if(artist != null) this.AddNewArtist(artist);
                    else 
                    {
                        Console.Clear();
                        Console.WriteLine("Cancel add a new artist !!");
                    }
                    break;
                case "8":
                    ProfileMenu pMenu = new ProfileMenu();
                    pMenu.display_ProfileMenu(customer);
                    break;
                case "9":
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
    private bool UpgradePremium_Customer(Customer customer)
    {
        if (customer.staff) Console.WriteLine("You are a staff. No need to upgrade premium !");
        else if (customer.premium) Console.WriteLine("You are premium member already !");
        else
        {
            UIHelper.MenuTitle("Purchasing Premium MemberShip");
            UIHelper.DrawDownwardsLine();
            Console.WriteLine("│ Premium MemberShip                  │");
            Console.WriteLine("│ Starting Today                      │");
            Console.WriteLine("│      One time payment of 89.000 vnd │");
            Console.WriteLine("│                        for lifetime │");
            UIHelper.DrawUpwardLine();
            
            Console.WriteLine(" #Enter your choice: ");
            Console.Write("  ");
            if (YNQuest.ask_YesOrNo())
            {
                Order order = new Order();
                order.orderId = orderBL.GetMaxIdInOrders() + 1;
                order.orderCustomer = customer;
                order.total = 89000;
                order.orderDate = DateTime.Today;
                customer.premium = true;
                Console.WriteLine("Purchasing ....\n");
                if (cBL.UpgradePremium_Staff(customer.user_name))
                {
                    if (orderBL.CreateOrder(order)) 
                    {
                        Console.WriteLine();
                        SendingCode.SendPremiumGmail(order);
                        Console.Clear();
                        UIHelper.MenuTitle("Purchasing Successfully");
                        UIHelper.DrawDownwardsLine();
                        Console.WriteLine("│   You are Premium Membership now    │");
                        Console.WriteLine("│ We sent you a receipt for the order │");
                        Console.WriteLine("│ to your gmail.                      │");
                        UIHelper.DrawUpwardLine(); 
                        Console.Write("Press any key to continute! ");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else 
                    {
                        cBL.RemovePremium_Staff(customer.user_name);
                    }
                }
            }  
        }
        return customer.premium;
    }
    private Song InsertNewSong()
    {
        UIHelper.MenuTitle("Adding A New Song");
        Song nS = new Song();
        nS.songId = sBL.GetMaxIdInSongs() + 1;
        
        nS.songName = StringUTil.GetValidName("Enter Title <Necessitate>: ");
        if ( nS.songName != null )
        {
            Song decoySong = InsertBandOrSingerToSong(nS);
            nS.band = decoySong.band;
            nS.singer = decoySong.singer;

            nS.writer = this.InsertWriterToSong(nS);

            nS.produced = this.InsertProducerToSong(nS);

            UIHelper.DisplayInsertingSongInformation(nS); //re display the song information
            nS.length = StringUTil.GetValidNumber("Enter Duration (in second) <Necessitate>: ");
            if (nS.length > 0)
            {
                Console.Write("Enter Lyric: ");
                nS.lyric = Console.ReadLine();
                Console.Write("Enter Download link: ");
                nS.downloadLink = Console.ReadLine();
                Console.Write("Enter Album title: ");
                nS.album = Console.ReadLine();
                Console.Write("Enter song's coppyright: ");
                nS.copyright = Console.ReadLine();
                nS.releaseDate = StringUTil.GetReleaseDate();

                nS.genres = this.InsertGenresToSong(nS);

                UIHelper.DisplayInsertingSongInformation(nS); //re display the song information
                Console.Write("Active this song? ");
                nS.songStatus = YNQuest.ask_YesOrNo();
                UIHelper.DisplayInsertingSongInformation(nS); //display the song's information after insert
                Console.WriteLine($"> Song's Status {nS.songStatus.ToString()}");
            }
            else nS = null;
        }
        else nS = null;
        return nS;     
    }
    private void AddNewSong(Song nS)
    {
        nS = this.RemoveCoincideGenre(nS);
        Console.Write("Save this song? ");
        if (YNQuest.ask_YesOrNo())
        {
            if (sBL.AddNewSong(nS) && this.AddGenresToSong(nS, nS.genres) && this.AddBandToSong(nS, nS.band) && this.AddSingerToSong(nS, nS.singer) && this.AddWriterToSong(nS, nS.writer) && this.AddProducerToSong(nS, nS.produced))
            {
                list_allSongs.Add(nS);
                Console.Clear();
                Console.WriteLine("This song is added to database successfully!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Some thing wrong! Couldn't add this song to database :(");
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel add a new song !");
        }
    }
    private Song RemoveCoincideGenre(Song s)
    {
        for (int i = 0; i < s.genres.Count; i++)
        {
            for(int j = i+1; j < s.genres.Count; j++)
            {
                if (s.genres[i].genreId == s.genres[j].genreId)
                {
                    s.genres.RemoveAt(j);
                }
            }
        }
        return s;
    }
    private bool AddGenresToSong(Song s, List<Genres> genreList)
    {
        bool result = true;
        foreach(var val in genreList)
        {
            if(!sBL.AddGenresToSong(s.songId, val.genreId))
            {
                Console.WriteLine("Fail to add these genres to this song");
                result = false;
            }
        }
        return result;
    }
    private bool AddBandToSong(Song s, List<Artists> artistList)
    {
        bool result = true;
        foreach(var val in artistList)
        {
            if(!sBL.AddBandToSong(s.songId, val.artistId))
            {
                Console.WriteLine("Fail to add these bands to this song");
                result = false;
            }
        }
        return result;
    }

    private bool AddSingerToSong(Song s, List<Artists> artistList)
    {
        bool result = true;
        foreach(var val in artistList)
        {
            if(!sBL.AddSingerToSong(s.songId, val.artistId))
            {
                Console.WriteLine("Fail to add these singers to this song");
                result = false;
            }
        }
        return result;
    }
    private bool AddWriterToSong(Song s, List<Artists> artistList)
    {
        bool result = true;
        foreach(var val in artistList)
        {
            if(!sBL.AddWriterToSong(s.songId, val.artistId))
            {
                Console.WriteLine("Fail to add these writers to this song");
                result = false;
            }
        }
        return result;
    }
    private bool AddProducerToSong(Song s, List<Artists> artistList)
    {
        bool result = true;
        foreach(var val in artistList)
        {
            if(!sBL.AddProducerToSong(s.songId, val.artistId))
            {
                Console.WriteLine("Fail to add these producers to this song");
                result = false;
            }
        }
        return result;
    }
    private List<Genres> InsertGenresToSong(Song insertingSong)
    {
        List<Genres> songGenre = new List<Genres>();
        do
        {
            UIHelper.DisplayInsertingSongInformation(insertingSong);
            Console.Write("\n> Genres: ");
            foreach (var item in songGenre) Console.Write($"{item.genreTitle}, ");
            Console.WriteLine();

            Console.WriteLine("\nPlease choose the genre for the song !");
            Genres chosenGenre = this.ChooseGenreToAddToSong(songGenre);
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
    // private List<Artists> InsertBandToSong(Song insertingSong)
    // {
    //     List<Artists> chosenArtist = new List<Artists>();
    //     List<Artists> songArtist = new List<Artists>();
    //     //if (insertingSong.band != null)
    //     do
    //     {
    //         //* Display the song information *//
    //         UIHelper.DisplayInsertingSongInformation(insertingSong);
    //         Console.Write("Artists: ");
    //         foreach (var item in songArtist) Console.Write($"{item.bandName} ".Trim());
    //         Console.WriteLine();

    //         //* Choose the singer for the songs *//
    //         Console.WriteLine("\nPlease choose the band for the song !");
    //         if (insertingSong.band != null) 
    //         {
    //             songArtist = insertingSong.band;
    //             chosenArtist = ChooseBandToAddToSong(songArtist.Union(insertingSong.band).ToList());
    //         }
    //         else chosenArtist = ChooseBandToAddToSong(songArtist);
    //         if (chosenArtist != null) 
    //         {
    //             //UIHelper.DrawALine();
    //             songArtist.AddRange(chosenArtist);
    //             Console.Write("Continute? ");
    //         }
    //         else Console.WriteLine("Do you want to retry? ");
    //     }
    //     while (YNQuest.ask_YesOrNo());

    //     return songArtist;
        
    // }
    private Song InsertBandOrSingerToSong(Song insertingSong)
    {
        //bool endLoop = true;
        Song decoySong = insertingSong;
        // bool enableChooseBand = true;
        // bool enableChoosSinger = true;
        // ArrayList mOption = MagicNumber.ChooseBandOrSinger_menuOption(enableChooseBand, enableChoosSinger);
        // string menuTitle = MagicNumber.ChooseBandOrSinger_menuTitle;
        
        //while(endLoop)
        //{
            //mOption = MagicNumber.ChooseBandOrSinger_menuOption(enableChooseBand, enableChoosSinger);
            //var choose = UIHelper.ChooseOptionMenu(mOption, menuTitle);
            //if (!enableChooseBand && enableChoosSinger) choose = "2";
            //else if (enableChooseBand && !enableChoosSinger) choose = "1";
            //else if (!enableChooseBand && !enableChoosSinger) choose = "3";
            // switch (choose)
            // {
            //     case "1":
            //     if (enableChooseBand)
            //     {
            //         decoySong.band = (this.InsertBandToSong(decoySong));
            //         enableChooseBand = false;
            //         if(decoySong.singer!= null)
            //         {
            //             if (decoySong.band != null) 
            //             {
            //                 this.DetectSingerIfTheyCanBeApartOfABand(decoySong.singer, decoySong.band);
            //             }
            //         }
            //     }
            //         break;
            //     case "2":
            //    if (enableChoosSinger)
            //    {
        decoySong.singer = (this.InsertSingerToSong(decoySong));
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
            //      enableChoosSinger = false;
            //    }
            //         break;
            //     case "3":
            //         endLoop = false;
            //         break;  
            //     default:
            //         Console.Clear();

            //         Console.WriteLine("Wrong Choice");
            //         Console.WriteLine("Please TryAgain");
            //         break;
            // }
        //} 
        return decoySong;
    }

    // private List<Artists> DetectSingerIfTheyCanBeApartOfABand(List<Artists> artistList, List<Artists> bandList)
    // {
    //     for(int i = 0; i<artistList.Count(); i++) //Remove non-band singer
    //     {
    //         if (!artistList[i].isBand)  artistList.RemoveAt(i);
    //     } 
    //     while (artistList.Count() > 1) //a band need atlease 2 persons right?
    //     {
    //         var theSingerList = (from ss in artistList where ss.bandName.ToLower().Contains(artistList[0].bandName.ToLower()) select ss).ToList();
    //         var theBandList = (from bb in bandList where bb.bandName.ToLower().Contains(artistList[0].bandName.ToLower()) select bb).ToList();
    //         if (theBandList.Count() != theSingerList.Count())
    //         {
    //             foreach(var val in theSingerList) artistList.Remove(val);
    //         } 
    //         else foreach(var val in theSingerList) artistList.Remove(val);
    //     } 
    //     return artistList;
    // }
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
    private List<Artists> InsertSingerToSong(Song insertingSong)
    {
        Artists chosenArtist = new Artists();
        List<Artists> songArtist = new List<Artists>();
        do
        {
            UIHelper.DisplayInsertingSongInformation(insertingSong);
            Console.Write("\n> Artists: ");
            foreach (var item in songArtist) Console.Write($"{item.stageName}, ");
            Console.WriteLine();
            
            //* Choose the singer for the songs *//
            //Console.WriteLine("\nPlease choose the singer for the song !");
            //if (insertingSong.band != null) chosenArtist = ChooseArtistToAddToSong(Persistence.Enum.ArtistType.Singer, songArtist.Union(insertingSong.band).ToList());
            //else 
            chosenArtist = ChooseArtistToAddToSong(Persistence.Enum.ArtistType.Singer, songArtist);
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
    private List<Artists> InsertWriterToSong(Song insertingSong)
    {
        List<Artists> songArtist = new List<Artists>();
        do
        {
            UIHelper.DisplayInsertingSongInformation(insertingSong);
            Console.Write("\n> Written By: ");
            foreach (var item in songArtist) Console.Write($"{item.songwriterAlias}, ");
            Console.WriteLine();

            //Console.WriteLine("\nPlease choose the writer for the song !");
            Artists chosenArtist =  ChooseArtistToAddToSong(Persistence.Enum.ArtistType.Writer, songArtist);
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
    private List<Artists> InsertProducerToSong(Song insertingSong)
    {
        List<Artists> songArtist = new List<Artists>();
        do
        {
            UIHelper.DisplayInsertingSongInformation(insertingSong);
            Console.Write("\n> Produced By: ");
            foreach (var item in songArtist) Console.Write($"{item.producerAlias}, ");
            Console.WriteLine();

            //Console.WriteLine("\nPlease choose the producer for the song !");
            Artists chosenArtist =  ChooseArtistToAddToSong(Persistence.Enum.ArtistType.Producer, songArtist);
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
    private Genres InsertNewGenre()
    {
        Genres nGenre = new Genres();

        nGenre.genreId = sBL.GetMaxIdInSongs() + 1;
        Console.Clear();
        UIHelper.MenuTitle("Adding New Genre");
        nGenre.genreTitle = StringUTil.GetValidName("Enter genre's title: ");
        if (nGenre.genreTitle != null)
        {
            nGenre.genreTitle = StringUTil.UpperFirstLecter(nGenre.genreTitle);
            if (!gBL.CheckGenreTitle(nGenre.genreTitle))
            {
                nGenre.genreStatus = true;
            }
            else
            {
                Console.WriteLine($"This genre's title \"{nGenre.genreTitle}\" is already in use");
                Console.WriteLine("Do you want to retry?");
                if ( YNQuest.ask_YesOrNo() )
                {
                    nGenre = InsertNewGenre();
                }
                else
                {
                    nGenre = null;
                }
            }    
        }
        else
        {
            nGenre = null;
        }
        return nGenre;

    }
    private void AddNewGenre(Genres nGenre)
    {
        Console.WriteLine("Do you want to save this genre ?");
        if (YNQuest.ask_YesOrNo())
        {
            if (gBL.AddNewGenre(nGenre))
            {
                Console.Clear();
                Console.WriteLine("This genre added to database!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Some thing wrong! Couldn't add this genre to database :(");
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel Add a new genre !");
        }
    }
    private Artists InsertNewArtist()
    {
        Artists nA = new Artists();
        nA.artistId = aBL.GetMaxIdInArtist() + 1;
        Console.Clear();
        bool next = true;
        UIHelper.MenuTitle(" Adding New Genre");
        nA.artistFirstName = StringUTil.GetValidName("Enter first name <Necessitate>: ");
        if (nA.artistFirstName != null)
        {
            nA.artistFirstName = StringUTil.UpperFirstLecter(nA.artistFirstName);
            nA.artistLastName = StringUTil.GetValidName("Enter last name <Necessitate>: ");
            if (nA.artistLastName != null)
            {
                nA.artistLastName = StringUTil.UpperFirstLecter(nA.artistLastName);
                bool endLoop = true;
                do 
                {
                    Console.Clear();
                    Console.WriteLine("Choose this artist's gender");
                    UIHelper.DrawDownwardsLine();
                    Console.WriteLine("   0. Male");
                    Console.WriteLine("   1. Female");
                    Console.WriteLine("   2. LBGT");
                    UIHelper.DrawUpwardLine();
                    Console.Write("    #The gender: ");
                    string gender = Console.ReadLine();
                    if (Int32.TryParse(gender, out int k))
                    {
                        if (0 <= k && k <= 2)
                        {
                            endLoop = false;
                            nA.gender = (Persistence.Enum.Gender)k;
                        }
                        else endLoop = true;
                    } else endLoop = true;
                    
                }
                while (endLoop);
                UIHelper.DisplayInsertingArtistInformation(nA);

                nA.born = StringUTil.GetTheTrueBithday();
                UIHelper.DisplayInsertingArtistInformation(nA);
                
                Console.Write("Is this artist a singer? ");
                nA.isSinger = YNQuest.ask_YesOrNo();
                if (nA.isSinger) 
                {
                    nA.stageName = StringUTil.GetValidName("Enter stage name <Necessitate>: ");

                    if (nA.stageName != null)  
                    {
                        nA.stageName = StringUTil.UpperFirstLecter(nA.stageName); 
                        UIHelper.DisplayInsertingArtistInformation(nA);
                    }
                    else next = false;

                    if (next && nA.isSinger) 
                    {
                        Console.Write("Is this singer a band's memeber ");
                        nA.isBand = YNQuest.ask_YesOrNo();
                        if (nA.isBand) 
                        {
                            nA.bandName = StringUTil.GetValidName("Enter band name <Necessitate>: ");
                            if (nA.bandName != null)
                            {
                                nA.bandName = StringUTil.UpperFirstLecter(nA.bandName);
                                UIHelper.DisplayInsertingArtistInformation(nA);
                            } 
                            else next = false;
                        }
                    }
                }

                if (next)
                {
                    Console.Write("Is this artist a composer? ");
                    nA.isWriter = YNQuest.ask_YesOrNo();
                    if (nA.isWriter) 
                    {
                        nA.songwriterAlias = StringUTil.GetValidName("Enter the alias (Writer) <Necessitate>: ");
                        if (nA.songwriterAlias != null)
                        {
                            nA.songwriterAlias = StringUTil.UpperFirstLecter(nA.songwriterAlias);
                            UIHelper.DisplayInsertingArtistInformation(nA);
                        }
                        else next = false;
                    }
                }

                if (next)
                {
                    Console.Write("Is this artist a producer? ");
                    nA.isProducer = YNQuest.ask_YesOrNo();
                    if (nA.isProducer) 
                    {
                        nA.producerAlias = StringUTil.GetValidName("Enter the alias (Producer) <Necessitate>: ");
                        if (nA.producerAlias != null)
                        {
                            nA.producerAlias = StringUTil.UpperFirstLecter(nA.producerAlias);
                            UIHelper.DisplayInsertingArtistInformation(nA);
                        }
                        else next = false;
                    }
                }
            } else nA = null;
        } else nA = null;
        if (!nA.isSinger && !nA.isProducer && !nA.isWriter) nA = null;
        if (!next) nA = null;
        return nA;
    }
    private void AddNewArtist(Artists a)
    {
        Console.Write("Save this Artist? "); 
        if (YNQuest.ask_YesOrNo())
        {
            if (aBL.AddNewArtist(a))
            {
                Console.Clear();
                Console.WriteLine("This artist added to database!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Some thing wrong! Couldn't add this artist to database :(");
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel add a new artist");
        }
    }
    private void DisplayAllSongMenu(Customer cus)
    {
        this.DisplaySongPages(cus, list_allSongs);
    }
    private void DisplaySearchSongNameMenu(Customer cus)
    {
        UIHelper.MenuTitle("Search A Song By Name");
        string searchStr = StringUTil.GetNotEmptyString("Enter artist's name: ");
        if (searchStr != null)
        {
            var songList = (from ss in list_allSongs where ss.songName.ToLower().Contains(searchStr.ToLower()) select ss).ToList();
            this.DisplaySongPages(cus, songList);
        }
        else Console.WriteLine("Cancel Search song by song's name");
    }
    private void DisplaySearchSongArtistMenu(Customer cus)
    {
        UIHelper.MenuTitle("Search A Song By Artist");
        List<Song> searchSongArtistList = new List<Song>();
        string searchStr = StringUTil.GetNotEmptyString("Enter artist's name: ");
        if (searchStr != null)
        {
            searchSongArtistList = new List<Song>();

            foreach(var val in list_allSongs)
            {
                foreach(var item in val.singer)
                {
                    if(item.stageName.ToLower().Contains(searchStr.ToLower()))
                    {
                        searchSongArtistList.Add(val);
                        break;
                    }
                }
            }
        }
        else
        {
            //searchSongArtistList = null;
            Console.WriteLine("Cancel Search song by artist's name");
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

        this.DisplaySongPages(cus, searchSongArtistList);
    }

    private void DisplaySongPages(Customer customer, List<Song> songList)
    {
        bool endLoop = false;
        int pageIndex = 1;

        if (!customer.staff) songList = UIHelper.RemoveNoneActiveSong(songList);

        int pageNum = UIHelper.GetNumberOfPages(songList.Count());
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
                    UIHelper.MenuTitle(" Displaying The Song List");
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
    public Artists ChooseArtistToAddToSong(Enum aType, List<Artists> listArtistsChosenAlready)
    {
        List<Artists> artistsList = new List<Artists>();
        switch(aType)
        {
            case Persistence.Enum.ArtistType.Singer:
            artistsList = aBL.GetListOfActiveSinger();
            break;
            case Persistence.Enum.ArtistType.Writer:
            artistsList = aBL.GetListOfActiveWriter();
            break;
            case Persistence.Enum.ArtistType.Producer:
            artistsList = aBL.GetListOfActiveProducer();
            break;
        }
        return UIHelper.DisplayArtistPages(aType, artistsList, listArtistsChosenAlready);
    }
    private List<Artists> ChooseBandToAddToSong(List<Artists> listArtistsChosenAlready)
    {
        List<Artists> artistsList = aBL.GetListOfActiveBand();
        return UIHelper.DisplayBandPages(artistsList, listArtistsChosenAlready);
    }
    public Genres ChooseGenreToAddToSong(List<Genres> listGenresChosenAlready)
    {
        List<Genres> genresList = new List<Genres>();
        genresList = gBL.GetListOfActiveGenre();
        return UIHelper.DisplayGenresPages(genresList, listGenresChosenAlready);
    }
}