using System;
using Persistence;
using System.Collections.Generic;
using ConsoleTables;
using System.Collections;
using ConsolePL.UTIL;
using System.Linq;
using BL;
public static class TypeInPLMenu
{
    public const int ADD = 0;
    public const int REMOVE = 1;
};
public class PlaylistMenu
{
    private static string menuTitle = MagicNumber.PlaylistMenu_menuTitle;
    private PlaylistBL plBL = new PlaylistBL();
    public List<Song> display_PlaylistMenu(Playlist pl, List<Song> list_allActiveSongs)
    {
        bool EndLoop = true;
        ArrayList mOption = MagicNumber.PlaylistMenu_Remove_NonePermOption();
        List<Song> songListInPlaylist = new List<Song>();
        while (EndLoop)
        {
            //var ChonMenu = this.p_choose_optionMenu(mOption);
            var ChonMenu = UIHelper.ChooseOptionMenu(mOption, menuTitle);

            switch (ChonMenu)
            {
                case "1":
                    Console.Clear();   
                    Playlist plx = this.display_ManagePlaylistMenu(pl, list_allActiveSongs);
                    if (plx != null) pl = plx;
                    else EndLoop = false;
                    break;
                case "2":
                    Console.Clear();
                    songListInPlaylist = pl.playlistSongs;
                    EndLoop = false;
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Exit");
                    EndLoop = false;
                    break;
                default:
                    Console.Clear();

                    Console.WriteLine("Wrong Choice");
                    Console.WriteLine("Please TryAgain");
                    break;
            }
        } 
        return songListInPlaylist;
    }

    private Playlist display_ManagePlaylistMenu(Playlist pl, List<Song> list_allActiveSongs)
    {
        bool EndLoop = true;
        ArrayList mOption = MagicNumber.managePlaylistMenu_Remove_NonePermOption();
        List<Song> list_allValidSong = this.RemoveSongThatExistedBeforeInPlaylist(list_allActiveSongs, pl.playlistSongs);
        Song chosenSong = new Song();

        while (EndLoop)
        {
            //var ChonMenu = this.p_choose_optionMenu(mOption);
            var ChonMenu = UIHelper.ChooseOptionMenu(mOption, menuTitle);

            switch (ChonMenu)
            {
                case "1":
                    Console.Clear();  
                    chosenSong = this.DisplayTheSongList(pl, list_allValidSong, TypeInPLMenu.ADD); //Exit session return null
                    if (chosenSong != null) 
                    {
                        pl.playlistSongs.Add(chosenSong);
                        list_allValidSong.Remove(chosenSong);
                    }
                    break;
                case "2":
                    Console.Clear();
                    if (pl.playlistSongs.Count() > 0)
                    {
                        chosenSong = this.DisplayTheSongList(pl, pl.playlistSongs, TypeInPLMenu.REMOVE);
                        if (chosenSong != null)
                        {
                            pl.playlistSongs.Remove(chosenSong);
                            list_allValidSong.Add(chosenSong);
                        }
                    }
                    break;
                case "3":
                    Console.Clear();
                    pl.playlistTitle = this.UpdatePlaylistTitle(pl).playlistTitle;
                    break;
                case "4":
                    if (this.UpdateThisPlaylistStatusTo_False(pl))
                    {
                        EndLoop = false;
                        pl = null;
                    }
                    break;
                case "5":
                    CustomerBL cBL = new CustomerBL();
                    Console.WriteLine("Are you sure to clone this playlist ?");
                    if (YNQuest.ask_YesOrNo()) 
                    {
                        Playlist clonePL = this.CreatePlaylist(cBL.GetCustomerbyID(pl.userId), this.ClonePlaylist(pl));
                        if ( clonePL != null ) 
                        {
                            this.CloneSongPlaylist(pl.playlistSongs, clonePL);
                            EndLoop = false;
                            pl = null;
                        }
                    }
                    else Console.WriteLine("Cancel clone !");
                    break;
                case "6":
                    Console.Clear();
                    Console.WriteLine("Exit");
                    EndLoop = false;
                    break;
                default:
                    Console.Clear();

                    Console.WriteLine("Wrong Choice");
                    Console.WriteLine("Please TryAgain");
                    break;
            }
        } 
        return pl;
    }
    private Playlist ClonePlaylist(Playlist pl)
    {
        string title = pl.playlistTitle + " - clone";
        Playlist newpl = pl;
        newpl.playlistTitle = title;
        return newpl;
    }
    private void CloneSongPlaylist(List<Song> listsongActive, Playlist clonePL)
    {
        bool passingYNQuest = true;
        List<Song> copListSong = new List<Song>();
        foreach(var val in listsongActive)
        {
           copListSong.Add(this.ChooseASongToAddToPlaylist(val, clonePL, passingYNQuest));
        }
        if (copListSong.ToString().Equals(listsongActive.ToString())) Console.WriteLine("Clone all songs from this Playlist to the clone-playlist successfully !");
        else Console.WriteLine("Clone all songs false :(");
    }
    private Song DisplayTheSongList(Playlist pl, List<Song> list_allActiveSongs, int Type)
    {
        bool endLoop = false;
        bool passingYNQuest = false;
        int pageIndex = 1;
        int pageNum = UIHelper.GetNumberOfPages(list_allActiveSongs.Count());
        //bool result = false;
        Song chosenSong = new Song();

        Console.Clear();
        if (list_allActiveSongs.Count() <= 0)
        {
            Console.WriteLine("Sorry! We have no song :(");
        }
        else
        {
            do
            {
                int maxRange = pageIndex*10;
                UIHelper.DisplaySongList(pageIndex, list_allActiveSongs);
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
                        chosenSong = null;
                        break;
                    default:
                        if ( Int32.TryParse(choose, out int k) )
                        {
                            Console.Clear();
                            if(maxRange > list_allActiveSongs.Count)
                            {
                                maxRange = list_allActiveSongs.Count;
                            }
                            if ( k >= (pageIndex-1)*10 && k< (maxRange))
                            {
                                switch(Type)
                                {
                                    case TypeInPLMenu.ADD:
                                        chosenSong = this.ChooseASongToAddToPlaylist(list_allActiveSongs[k], pl, passingYNQuest); //no pasing YNquest
                                        if (chosenSong != null) endLoop = true;
                                        break;
                                    case TypeInPLMenu.REMOVE:
                                        chosenSong = this.RemoveASongFromPlaylist(list_allActiveSongs[k], pl);
                                        if (chosenSong != null) endLoop = true;
                                        break;
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
        return chosenSong;
    }
    private Song ChooseASongToAddToPlaylist(Song chosenSong, Playlist pl, bool passingYNQuest)
    {
        Song repChosenSong = new Song();
        if (passingYNQuest == false) 
        {
            Console.WriteLine($"Are you sure to add: '{chosenSong.songName}'");
            Console.WriteLine($"To your playlist ?");
            passingYNQuest = YNQuest.ask_YesOrNo();
        }

        if(passingYNQuest)
        {
            if (!plBL.CheckSongExistInPlaylist(pl.playlistId, chosenSong.songId))
            {
                if (plBL.AddSongToPlaylist(pl.playlistId, chosenSong.songId))
                { 
                    Console.WriteLine("Added song to your playlist successfully!!");;
                    repChosenSong = chosenSong;
                } 
                else 
                {
                    Console.WriteLine("Can not add this song to your playlist !!\nPlease try again :(");
                    repChosenSong = null;
                }
            }
            else if (!plBL.CheckSongStatusInPlaylist(pl.playlistId, chosenSong.songId))
            {
                if (plBL.UpdateSongStatusInPlaylistTo_True(pl.playlistId, chosenSong.songId)) 
                {
                    Console.WriteLine("Added song to your playlist successfully!!");;
                    repChosenSong = chosenSong;
                } 
                else 
                {
                    Console.WriteLine("Can not add this song to your playlist !!\nPlease try again :(");
                    repChosenSong = null;
                }
            }
            else
            {
                Console.WriteLine("Can not add this song to your playlist !!");
                Console.WriteLine("Please try again :(");
                repChosenSong = null;
            }
        }
        else repChosenSong = null;
        return repChosenSong;
    }
    private Song RemoveASongFromPlaylist(Song chosenSong, Playlist pl)
    {
        Song repChosenSong = new Song();
        Console.WriteLine($"Are you sure to remove: '{chosenSong.songName}'");
        Console.WriteLine($"From your playlist ?");
        if(YNQuest.ask_YesOrNo())
        {
            if (plBL.CheckSongExistInPlaylist(pl.playlistId, chosenSong.songId))
            {
                if (plBL.UpdateSongStatusInPlaylistTo_False(pl.playlistId, chosenSong.songId))
                {
                    Console.WriteLine("This song has been removed from your playlist !!");
                    repChosenSong = chosenSong;
                }
                else
                {
                    Console.WriteLine("Can not remove this song from your playlist !");
                    Console.WriteLine("Please try again :(");
                    repChosenSong = null;
                }
            }
            else
            {
                Console.WriteLine("Your playlist doesn't contain this song !");
                Console.WriteLine("Please try again :(");
                repChosenSong = null;
            }
        }
        else repChosenSong = null;
        return repChosenSong;
    }
    private Playlist UpdatePlaylistTitle(Playlist pl)
    {
        Playlist newpl = new Playlist();
        newpl.userId = pl.userId;
        newpl.playlistId = pl.playlistId;
        Console.WriteLine("Are you sure to change this playlist's title ?");
        if (YNQuest.ask_YesOrNo())
        {
            newpl.playlistTitle = StringUTil.GetNotEmptyString("Enter your new playlist's title: ");
            if (newpl.playlistTitle != null && newpl.playlistTitle != pl.playlistTitle) 
            {
                newpl.playlistTitle = StringUTil.UpperFirstLecter(newpl.playlistTitle);
                if (plBL.UpdatePlaylistTitle(newpl)) Console.WriteLine("Update successfully !!");
                else 
                {
                    newpl.playlistTitle = pl.playlistTitle;
                    Console.WriteLine("Update fail :(");
                }
            }
            else 
            {
                newpl.playlistTitle = pl.playlistTitle;
                Console.WriteLine("Nothing changed !!");
            }
        }
        else 
        {
            newpl = pl;
            Console.WriteLine("Cancel change this playlist's title");
        }
        return newpl;
    }
    private bool UpdateThisPlaylistStatusTo_False(Playlist pl)
    {
        bool result = false;
        Console.Clear();
        Console.WriteLine("Are you sure to delete this playlist ? ");
        if (YNQuest.ask_YesOrNo())
        {
            if (plBL.UpdateThisPlaylistStatusTo_False(pl)) 
            {
                result = true;
                Console.WriteLine("Update successfully !");
            }
            else Console.WriteLine("Update fail :(");
        }
        else
        {
            Console.WriteLine("Cancel !!");
        }
        return result;
    }
    public Playlist CreatePlaylist(Customer customer, Playlist clonePL)
    {
        Playlist pl = new Playlist();
        string title;
        if (this.CheckIfCustomerCanCreateANewPlaylist(customer))
        {
            if (clonePL == null) title = StringUTil.GetNotEmptyString("Enter your playlist's title: ");
            else title = clonePL.playlistTitle;
            pl = this.RecycleRemovedPlaylist(customer.userId);
            if (title != null)
            {
                title = StringUTil.UpperFirstLecter(title);
                if (pl == null) 
                {
                    pl = new Playlist();
                    pl.playlistTitle = title;
                    pl.createDate = DateTime.Now;
                    pl.playlistId = plBL.GetMaxIdInPlaylist() + 1;
                    pl.userId = customer.userId;
                    if (plBL.CreateANewPlaylist(pl)) Console.WriteLine("Successfully !");
                    else Console.WriteLine("Fail :((");
                }
                else 
                {
                    pl.playlistTitle = title;
                    pl.createDate = DateTime.Now;
                    if (plBL.RecycleRemovedPlaylist(pl)) Console.WriteLine("Successfully !");
                    else Console.WriteLine("Fail :((");
                }
            }
            else Console.WriteLine("Cancel Create a new playlist !");
        }
        else pl = null;

        return pl;
    }
    private bool CheckIfCustomerCanCreateANewPlaylist(Customer customer)
    {
        bool result = false;
        int maxPLCreated = MagicNumber.maxPlayList_Member;

        if (customer.staff) 
        {
            maxPLCreated = 9999;
        }
        else if (customer.premium) 
        {
            maxPLCreated = MagicNumber.maxPlayList_Premium;
        }

        if (customer.playlistCreated < maxPLCreated)
        {
            result = true;
        }
        else if (customer.premium) 
        {
            Console.WriteLine("You reached the limit of playlist !");
        }
        else
        {
            Console.WriteLine("You reached the limit of playlist !");
            Console.WriteLine("Please upgrade to premium member to create more playlists !");
        }
        return result;
    }
    private Playlist RecycleRemovedPlaylist(int userId)
    {
        List<Playlist> removedPlaylistlist = plBL.GetRemovedPlaylistList(userId);
        Playlist pl = new Playlist();
        if (removedPlaylistlist.Count() > 0) pl = removedPlaylistlist[0]; //get the first playlist in the list
        else pl = null;
        return pl;
    }
    public Playlist ChoosePlaylistPages(Customer customer, List<Playlist> plList, int pageNum)
    {
        bool endLoop = false;
        int pageIndex = 1;
        Playlist listPL = new Playlist();

        Console.Clear();
        if (pageNum == 0)
        {
            Console.WriteLine("Sorry! You have no playlist :(");
            listPL = null;
        }
        else
        {
            do
            {
                int maxRange = pageIndex*10;
                DisplayPlaylistList(pageIndex, plList);
                if (customer.staff) Console.WriteLine("View a Playlist Information: ID, Next Page: N, Previous Page: P, Exit: E");
                else Console.WriteLine("View a Playlist Information: ID, Exit: E");
                Console.Write("# YOUR CHOICE: ");
                string choose = Console.ReadLine();

                switch (choose)
                {
                    case "n":
                        Console.Clear();
                        if (customer.staff) pageIndex++;
                        else Console.WriteLine("WRONG CHOICE !! PLEASE TRY AGAIN!");
                        break;
                    case "p":
                        Console.Clear();
                        if (customer.staff) pageIndex--;
                        else Console.WriteLine("WRONG CHOICE !! PLEASE TRY AGAIN!");
                        break;
                    case "e":
                        Console.Clear();
                        listPL = null;
                        endLoop = true;
                        break;
                    default:
                        if ( Int32.TryParse(choose, out int k) )
                        {
                            Console.Clear();
                            if(maxRange > plList.Count)
                            {
                                maxRange = plList.Count;
                            }
                            if ( k >= (pageIndex-1)*10 && k< (maxRange))
                            {
                                //this.DisplaySong(songList[k], customer.staff);
                                using (null)
                                {
                                    listPL = plList[k];
                                    endLoop = true;
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
        return listPL;
    }
    
    private void DisplayPlaylistList(int pageIndex, List<Playlist> plList)
    {
        using (null)
        {
            int maxRange = 10*pageIndex;
            if (maxRange >= plList.Count) maxRange = plList.Count;
            var table = new ConsoleTable("ID", "Playlist Title", "Created Date");
            for (int i = (pageIndex-1)*10; i< maxRange; i++) table.AddRow(i,plList[i].playlistTitle, plList[i].createDate.ToString("MM/dd/yyyy"));
            table.Write();
        }
    }
    private List<Song> RemoveSongThatExistedBeforeInPlaylist(List<Song> list_allActiveSongs, List<Song> list_allActiveSongsExistedInPLB4)
    { 
        for(int i = 0; i < list_allActiveSongs.Count(); i++)
        {
            foreach(var item in list_allActiveSongsExistedInPLB4)
            {
                if (list_allActiveSongs[i].songId == item.songId) list_allActiveSongs.RemoveAt(i);
                
            }
        }
        return list_allActiveSongs;
    }
}