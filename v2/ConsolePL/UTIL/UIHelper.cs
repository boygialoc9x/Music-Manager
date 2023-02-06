using System;
using System.Collections.Generic;
using System.Collections;
using ConsoleTables;
using Persistence;
namespace ConsolePL.UTIL
{
    public class UIHelper
    {
        private static string line = "───────────────────────────────────────";
        private static string line2 = "└─────────────────────────────────────┘";
        private static string line3 = "┌─────────────────────────────────────┐";
        private static int lineLength = line.Length;
        private static ArrayList ThemOption(string[] menuOption)
        {
            ArrayList mOption = new ArrayList();
            foreach(var val in menuOption)
            {
                mOption.Add(val);
            }
            return mOption;
        }
        public static string ChooseOptionMenu(ArrayList mOption, string menuTitle)
        {
            //ArrayList mOption = ThemOption(menuOption);
            MenuTitle(menuTitle);
            //Console.WriteLine(lineLength);
            Console.WriteLine(line3);
            for(int i = 0; i < mOption.Count; i++)
            {
                string str = $"│ {i+1}. {mOption[i]}";
                for (int j = str.Length; j<lineLength-1; j++)
                {
                    str = str + ' ';
                }
                Console.WriteLine($"{str}│");
            }

            Console.WriteLine($"{line2}");
            Console.Write(" #YOUR CHOICE: ");
            string choice = Console.ReadLine();
            return choice;

        }
        private static void MenuTitle(string menuTitle)
        {
            bool networkStatus = GetIP.CheckConnect();
            Console.WriteLine("┌─────────────────────────────────────┐");
            Console.Write("│         [Music Application]         │");
            NetworkConnectionStatus(networkStatus);
            Console.WriteLine("│         Group 3 - PF14 v1.0         │");
            string str = menuTitle;
            for (int i = 0; i<(lineLength - menuTitle.Length)/2-2; i++ )
            {
                str = ' ' + str + ' ';
            }
            Console.WriteLine($"│ {str} │");
            Console.WriteLine("└─────────────────────────────────────┘");
      
        }
        private static void NetworkConnectionStatus(bool networkStatus)
        {
            string netStatus = null;
            if (networkStatus)
            {
                Console.ForegroundColor = ConsoleColor.Green;  
                netStatus = "Online";
                Console.WriteLine("\t\t*" + netStatus);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;  
                netStatus = "Offline";
                Console.WriteLine("\t\t*" + netStatus + "\n");
                Console.ResetColor();
            }
        }
        public static void DisplaySongList(int pageIndex, List<Song> songList)
        {
            using (null)
            {
                SongMenu sMenu = new SongMenu();
                int maxRange = 10*pageIndex;// A page only has 10 element so this why the number 10 is here. And why maxRange ?
                                            // so, if songList only has 15 elements, u will have 2 page. Right? And now if u dont have
                                            // the maxRange, your "i" loop will run to 2*10 = 20, but u only have 15 elements. its
                                            // overange. And maxRange is the hero save your day. Read all code to understand
                //elementNum = elementNum - count;
                if (maxRange >= songList.Count)
                {
                    maxRange = songList.Count;
                }
                //Console.WriteLine((pageIndex-1)*10 +1 + " - " + maxRange);
                var table = new ConsoleTable("ID", "Song's Name", "Artist");
                for (int i = (pageIndex-1)*10; i< maxRange; i++)
                {
                    table.AddRow(i,songList[i].songName, sMenu.removeCoincideArtist(songList[i]));
                }
                table.Write();
            }
        }

        public static int GetNumberOfPages(int numberOfelements)
        {
            int numberOfelements_PerPage = 10;
            
            int numberOfpage = numberOfelements / numberOfelements_PerPage;

            if ( numberOfelements % numberOfelements_PerPage > 0)
            {
                numberOfpage ++;
            }
            
            return numberOfpage;
        }
        public static List<Song> RemoveNoneActiveSong(List<Song> songList)
        {
            for (int i=0; i<songList.Count; i++)
            {
                if (!songList[i].songStatus)
                {
                    songList.RemoveAt(i);
                    i--;
                }
            }
            return songList;
        }
    }
}