using System;
using System.Collections.Generic;
using System.Collections;
using Persistence;
using BL;
using ConsolePL.UTIL;


namespace ConsolePL.UTIL
{
    public class MagicNumber
    {
///  SONG MENU  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static string[] songMenu_menuOption = {"View song's lyric", "Download this song", "View artist's information","Upgrade song's information", "Upgrade song's lyric","Upgrade song's download link", "Change song status (Remove)","Exit"};
        /*
            NONE STAFF REMOVE OPTIONS: Upgrade song's information, Upgrade song's lyric, Upgrade song's download link, change song's status -> removingNumber_Staff = 4
            NONE PREMIUM REMOVE OPTIONS: View song's lyric, Download this song, View artist's information -> removingNumber_Premium = 2
        */
        public static string songMenu_menuTitle = " Welcome to Song Menu";
        private const int songMenu_RemovingNumber_Staff = 4;
        private const int songMenu_RemovingNumber_Premium = 3;

        public static ArrayList songMenu_Remove_NonePermOption(bool staff, bool premium)
        {
            ArrayList mOption = new ArrayList();

            foreach(var val in songMenu_menuOption)
            {
                mOption.Add(val);
            }

            if (!staff)
            {
                mOption.Remove("Upgrade song's information");
                mOption.Remove("Upgrade song's lyric");
                mOption.Remove("Upgrade song's download link");
                mOption.Remove("Change song status (Remove)");
                // => removingNumner = 4;

                if (!premium)
                {
                    mOption.Remove("View song's lyric");
                    mOption.Remove("Download this song");
                    mOption.Remove("View artist's information");
                    // => removingNumber += 3;
                }
            }

            return mOption;
        }
        public static int songMenu_Get_numberOfRemovingMenuOption(bool staff, bool premium)
        {
            int removingNumber = 0;
            if (!staff)
            {
                removingNumber += songMenu_RemovingNumber_Staff; 
                if (!premium)
                {
                    removingNumber += songMenu_RemovingNumber_Premium; 
                }
            }

            // Go to p_removeNonePermOption() function to see more details

            return removingNumber;
        }

///  MAIN MENU  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private static string[] mainMenu_menuOption = {"View all songs", "Search by Song's name", "Search by artist's name", "Upgrade Premium","Add a new Song", "Add a new Category", "Add a new artist","Create Playlist","My Playlist","My profile","Log Out"};
        /*
            NONE STAFF REMOVE OPTIONS: Add a new song, Add a new Category, Add a new Category -> removingNumber_Staff = 3
            NONE PREMIUM REMOVE OPTIONS: -> removingNumber_Premium = 0
        */
        public static string mainMenu_menuTitle = " Welcome to MainMenu ";
        private const int mainMenu_removingNumber_Staff = 3;
        private const int mainMenu_removingNumber_Premium = 0;
        public static ArrayList mainMenu_Remove_NonePermOption(bool staff, bool premium)
        {
            ArrayList mOption = new ArrayList();

            foreach(var val in mainMenu_menuOption)
            {
                mOption.Add(val);
            }
            if (!staff)
            {
                mOption.Remove("Add a new Song");
                mOption.Remove("Add a new Category");
                mOption.Remove("Add a new artist");
                // => removingNumner = 3;
                if (!premium)
                {

                    // => removingNumner = 0;
                }
            }

            return mOption;
        }
        public static int mainMenu_Get_numberOfRemovingMenuOption(bool staff, bool premium)
        {
            int removingNumber = 0;
            if (!staff)
            {
                removingNumber += mainMenu_removingNumber_Staff; 
                if (!premium)
                {
                    removingNumber += mainMenu_removingNumber_Premium; 
                }
            }

            return removingNumber;
        }
///  LOGIN MENU  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static public string[] loginMenu_menuOption = { "Login", "Register", "Forget Password","Exit" };
        static public string loginMenu_menuTitle = "Welcome to Login Menu";

        public const int loginMenu_removingNumber_Staff = 0;
        public const int loginMenu_removingNumber_Premium = 0;

///  PROFILE MENU  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static string[] profileMenu_menuOption = {"Change my information", "Change my password","Exit"};
        /*
            NONE STAFF REMOVE OPTIONS: Upgrade song's information, Upgrade song's lyric, Upgrade song's download link, change song's status -> removingNumber_Staff = 4
            NONE PREMIUM REMOVE OPTIONS: View song's lyric, Download this song -> removingNumber_Premium = 2
        */
        public static string profileMenu_menuTitle = "Welcome to Profile Menu";
        private const int profileMenu_RemovingNumber_Staff = 0;
        private const int profileMenu_RemovingNumber_Premium = 0;

        public static ArrayList profileMenu_Remove_NonePermOption(bool staff, bool premium)
        {
            ArrayList mOption = new ArrayList();

            foreach(var val in profileMenu_menuOption)
            {
                mOption.Add(val);
            }

            if (!staff)
            {
                // => removingNumner = 0;

                if (!premium)
                {
                    // => removingNumber += 0;
                }
            }

            return mOption;
        }
        public static int profileMenu_Get_numberOfRemovingMenuOption(bool staff, bool premium)
        {
            int removingNumber = 0;
            if (!staff)
            {
                removingNumber += profileMenu_RemovingNumber_Staff; 
                if (!premium)
                {
                    removingNumber += profileMenu_RemovingNumber_Premium; 
                }
            }

            // Go to p_removeNonePermOption() function to see more details

            return removingNumber;
        }

///  ARTIST MENU  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static string[] artistMenu_menuOption = {"Upgrade Artist Information","Exit"};
        /*
            NONE STAFF REMOVE OPTIONS: Update Artist Information -> removingNumber_Staff = 1
            NONE PREMIUM REMOVE OPTIONS:  -> removingNumber_Premium = 0
        */
        public static string artistMenu_menuTitle = " Welcome to Artist Menu";
        private const int artistMenu_RemovingNumber_Staff = 1;
        private const int artistMenu_RemovingNumber_Premium = 0;

        public static ArrayList artistMenu_Remove_NonePermOption(bool staff, bool premium)
        {
            ArrayList mOption = new ArrayList();

            foreach(var val in artistMenu_menuOption)
            {
                mOption.Add(val);
            }

            if (!staff)
            {
                mOption.Remove("Upgrade Artist Information");
                // => removingNumner = 1;

                if (!premium)
                {
                    // => removingNumber += 0;
                }
            }

            return mOption;
        }
        public static int artistMenu_Get_numberOfRemovingMenuOption(bool staff, bool premium)
        {
            int removingNumber = 0;
            if (!staff)
            {
                removingNumber += artistMenu_RemovingNumber_Staff; 
                if (!premium)
                {
                    removingNumber += artistMenu_RemovingNumber_Premium; 
                }
            }

            // Go to p_removeNonePermOption() function to see more details

            return removingNumber;
        }
///  PLAY LIST  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public const int maxPlayList_Member = 3;
        public const int maxPlayList_Premium = 7;

        private static string[] PlaylistMenu_menuOption = {"Manage My Playlist", "Display song list","Exit"};
        /*
            NONE STAFF REMOVE OPTIONS: Upgrade song's information, Upgrade song's lyric, Upgrade song's download link, change song's status -> removingNumber_Staff = 4
            NONE PREMIUM REMOVE OPTIONS: View song's lyric, Download this song -> removingNumber_Premium = 2
        */
        private static string[] managePlaylistMenu_menuOption = {"Add a song to this Playlist", "Remove a song from this Playlist","Rename this Playlist","Remove this Playlist","Clone this playlist","Exit"};
        public static string PlaylistMenu_menuTitle = " Welcome to Playlist Menu";
        private const int PlaylistMenu_RemovingNumber_Staff = 0;
        private const int PlaylistMenu_RemovingNumber_Premium = 0;

        public static ArrayList PlaylistMenu_Remove_NonePermOption()
        {
            ArrayList mOption = new ArrayList();

            foreach(var val in PlaylistMenu_menuOption)
            {
                mOption.Add(val);
            }

            return mOption;
        }

        public static ArrayList managePlaylistMenu_Remove_NonePermOption()
        {
            ArrayList mOption = new ArrayList();

            foreach(var val in managePlaylistMenu_menuOption)
            {
                mOption.Add(val);
            }

            return mOption;
        }
    }


}