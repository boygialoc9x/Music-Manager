using System;
using System.Collections.Generic;
using System.Collections;
using ConsoleTables;
using Persistence;
using System.Linq;
namespace ConsolePL.UTIL
{
    public class UIHelper
    {
        private static string line = "───────────────────────────────────────";
        private static string line2 = "└─────────────────────────────────────┘";
        private static string line3 = "┌─────────────────────────────────────┐";
        public static int lineLength = line.Length;
        public static void DrawStraightLine()
        {
            Console.WriteLine(line);
        }
        public static void DrawDownwardsLine()
        {
            Console.WriteLine(line3);
        }
        public static void DrawUpwardLine()
        {
            Console.WriteLine(line2);
        }
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
        public static void MenuTitle(string menuTitle)
        {
            bool networkStatus = GetIP.CheckConnect();
            Console.WriteLine("┌─────────────────────────────────────┐");
            Console.WriteLine("│         [Music Application]         │");
            //NetworkConnectionStatus(networkStatus);
            Console.WriteLine("│         Group 3 - PF14 v6.0         │");
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
                int maxRange = 10*pageIndex;// A page only has 10 element so this why the number 10 is here. And why maxRange ?
                                            // so, if songList only has 15 elements, u will have 2 page. Right? And now if u dont have
                                            // the maxRange, your "i" loop will run to 2*10 = 20, but u only have 15 elements. its
                                            // overange. And maxRange is the hero save your day. Read all code to understand
                //elementNum = elementNum - count;
                if (maxRange >= songList.Count)
                {
                    maxRange = songList.Count;
                }
                var table = new ConsoleTable("#", "TITLE", "ARTIST", "Album", "DURATION");
                for (int i = (pageIndex-1)*10; i< maxRange; i++)
                {
                    table.AddRow(i,songList[i].songName, RemoveCoincideSingerOrBand(songList[i]), songList[i].album, SongLengthConvert(songList[i].length));
                }
                table.Write();
            }
        }
        public static void DisplayArtistList(int pageIndex, List<Artists> artistList, Enum aType)
        {
            int maxRange = 10*pageIndex;
            if (maxRange >= artistList.Count) maxRange = artistList.Count;
            string displayBaseOn_aType = null;
            switch(aType)
            {
                case Persistence.Enum.ArtistType.Singer:
                displayBaseOn_aType = "Stage Name";
                var table1 = new ConsoleTable("#", $"{displayBaseOn_aType}", "Band Name", "Full Name", "Gender", "Birthday");
                for (int i = (pageIndex-1)*10; i< maxRange; i++)
                {
                   table1.AddRow(i, artistList[i].stageName, artistList[i].bandName, artistList[i].artistFirstName + " " + artistList[i].artistLastName, artistList[i].gender, ((Persistence.Enum.Month)artistList[i].born.Month).ToString() + " " + artistList[i].born.Day + ", " + artistList[i].born.Year);
                }
                table1.Write();
                break;

                case Persistence.Enum.ArtistType.Writer:
                displayBaseOn_aType = "Writer Alias";
                var table2 = new ConsoleTable("#", $"{displayBaseOn_aType}", "Full Name", "Gender", "Birthday");
                for (int i = (pageIndex-1)*10; i< maxRange; i++)
                {
                    table2.AddRow(i, artistList[i].songwriterAlias, artistList[i].artistFirstName + " " + artistList[i].artistLastName, artistList[i].gender, ((Persistence.Enum.Month)artistList[i].born.Month).ToString() + " " + artistList[i].born.Day + ", " + artistList[i].born.Year);
                }
                table2.Write();
                break;

                case Persistence.Enum.ArtistType.Producer:
                displayBaseOn_aType = "Producer Alias";
                var table3 = new ConsoleTable("#", $"{displayBaseOn_aType}", "Full Name", "Gender", "Birthday");
                for (int i = (pageIndex-1)*10; i< maxRange; i++)
                {
                    table3.AddRow(i, artistList[i].producerAlias, artistList[i].artistFirstName + " " + artistList[i].artistLastName, artistList[i].gender, ((Persistence.Enum.Month)artistList[i].born.Month).ToString() + " " + artistList[i].born.Day + ", " + artistList[i].born.Year);
                }
                table3.Write();
                break;
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

        public static string RemoveCoincideSingerOrBand(Song s)
        {
            List<string> artist = new List<string>();
            string theArtist = "";
            if (s.singer != null || s.band != null)
            {
                if(s.singer != null) foreach(var val in s.singer) artist.Add(val.stageName.ToString());
                
                if (s.band != null) foreach(var val in s.band) artist.Add(val.bandName.ToString());
                List<string> newCopA = artist.Distinct().ToList();

                foreach(var val in newCopA) theArtist += " ft. " + val;
            }
            if (theArtist.Length > 4) theArtist = theArtist.Remove(0,4).Trim();
            return theArtist;//Remove 4 first char in the string: " ft. "
        }
        private static Dictionary<int, string> GetNewBandList(List<Artists> bandList)
        {
            List<string> artist = new List<string>();
            Dictionary<int, string> newBandList = new Dictionary<int, string>();
            if (bandList!= null) 
            {
                foreach(var val in bandList) artist.Add(val.bandName.ToString());
                List<string> onlyBand = artist.Distinct().ToList();
                for(int i = 0; i< onlyBand.Count(); i++) newBandList.Add(i, onlyBand[i].ToString());    
            }
            return newBandList;
        }
        private static void DisplayBandList(Dictionary<int, string> bandList, int pageIndex)
        {

            int maxRange = 10*pageIndex;
            if (maxRange >= bandList.Count) maxRange = bandList.Count;

            var table = new ConsoleTable("#", "Band Name");
            for (int i = (pageIndex-1)*10; i< maxRange; i++)
            {
                table.AddRow(i, bandList[i]);
            }
            table.Write();
            
        }
        private static void DisplayArtistsListInTheBand(int pageIndex, List<Artists> artistList)
        {
            Console.Clear();
            UIHelper.MenuTitle("Displaying Band's Information");
            int maxRange = 10*pageIndex;
            if (maxRange >= artistList.Count) maxRange = artistList.Count;
            var table = new ConsoleTable("#", "Full Name", "Gender", "Band Name", "Stage Name", "Birthday");
            for (int i = (pageIndex-1)*10; i< maxRange; i++)
            {
                table.AddRow(i, artistList[i].artistFirstName + " " + artistList[i].artistLastName, artistList[i].gender, artistList[i].bandName, artistList[i].stageName, artistList[i].born.ToString("dd/MM/yyyy "));
            }
            table.Write();
        }
        //public static string RemoveC
        public static string SongLengthConvert(int length)
        {   
            int secondToMinute = 60; // 60s = 1m
            string songLength = (length / secondToMinute).ToString() + ":"+ (length % secondToMinute ).ToString(); 
            return songLength;
        }

        public static Artists DisplayArtistPages(Enum aType, List<Artists> artistsList, List<Artists> listArtistsChosenAlready)
        {
            bool endLoop = false;
            int pageIndex = 1;
            Artists chosenArtist = new Artists();

            int pageNum = UIHelper.GetNumberOfPages(artistsList.Count());
            if (listArtistsChosenAlready!= null) artistsList = RemoveArtistThatChosenBefore(artistsList, listArtistsChosenAlready);
            
            if (artistsList.Count <= 0) 
            {
                chosenArtist = null;
                Console.WriteLine($"Sorry! We dont have any valid {aType.ToString()} :(");
            }
            else
            {
                do
                {
                    int maxRange = pageIndex*10;
                    UIHelper.MenuTitle($" Choosing the {aType.ToString()} for the song");
                    UIHelper.DisplayArtistList(pageIndex, artistsList, aType);
                    Console.WriteLine("Choose a singer: Enter singer's ID | Next Page: N | Previous Page: P | Exit: E");
                    Console.WriteLine($"\nPlease choose the {aType.ToString()} for the song !");
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
                            Console.WriteLine();
                            UIHelper.DrawStraightLine();
                            chosenArtist = null;
                            endLoop = true;
                            break;
                        default:
                            if ( Int32.TryParse(choose, out int k) )
                            {
                                //Console.Clear();
                                if(maxRange > artistsList.Count) maxRange = artistsList.Count;

                                if ( k >= (pageIndex-1)*10 && k< (maxRange))
                                {
                                    DisplayArtist(artistsList[k]);
                                    //DrawDownwardsLine();
                                    DrawStraightLine();
                                    Console.WriteLine($"  Are you sure to choose this {aType.ToString()}\n");
                                    if(YNQuest.ask_YesOrNo()) 
                                    {
                                        chosenArtist = artistsList[k];
                                        endLoop = true;
                                        Console.WriteLine($"\n  Select {aType.ToString()} successfully !");
                                        //DrawUpwardLine();
                                        DrawStraightLine();
                                    }
                                    else 
                                    {
                                        Console.Clear();
                                        //Console.WriteLine($"Please choose the {aType.ToString()} for the song !");
                                        chosenArtist = null;
                                    }   
                                }                           
                                else 
                                {
                                    Console.Clear();
                                    Console.WriteLine("Invalid Input !!"); 
                                    //Console.WriteLine($"\nPlease choose the {aType.ToString()} for the song !");
                                }
                            } 
                            else 
                            {
                                Console.Clear();
                                Console.WriteLine("Invalid Input !!"); 
                                Console.WriteLine($"\nPlease choose the {aType.ToString()} for the song !");
                            }
                            break;
                    }

                    if (pageIndex <= 0)
                    {
                        Console.WriteLine("Sorry! We dont have a negative page :(");
                        Console.WriteLine("We'll get you back to the first page");
                        Console.WriteLine($"\nPlease choose the {aType.ToString()} for the song !");
                        pageIndex = 1; // first page number = 1
                    }

                    if (pageIndex > pageNum)
                    {
                        Console.WriteLine("Sorry! We dont have that page :(");
                        Console.WriteLine("We'll get you back to the last page");
                        Console.WriteLine($"\nPlease choose the {aType.ToString()} for the song !");
                        pageIndex = pageNum; // last page number = pageNu,
                    }
                    
                }
                while(!endLoop);
            }
            return chosenArtist;
        }
        public static List<Artists> DisplayBandPages(List<Artists> artistsList, List<Artists> listArtistsChosenAlready)
        {
            bool endLoop = false;
            int pageIndex = 1;
            List<Artists> chosenBand = new List<Artists>();

            artistsList = RemoveArtistThatChosenBefore(artistsList, listArtistsChosenAlready);
            Dictionary<int, string> bandList = GetNewBandList(artistsList);
            
            if (bandList.Count <= 0) Console.WriteLine($"Sorry! We dont have any valid band :(");
            else
            {
                int pageNum = bandList.Count;
                do
                {
                    int maxRange = pageIndex*10;
                    DisplayBandList(bandList, pageIndex);
                    Console.WriteLine("Choose a band: Enter band's ID | Next Page: N | Previous Page: P | Exit: E");
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
                            chosenBand = null;
                            endLoop = true;
                            break;
                        default:
                            if ( Int32.TryParse(choose, out int k) )
                            {
                                //Console.Clear();
                                if(maxRange > artistsList.Count) maxRange = artistsList.Count;

                                if ( k >= (pageIndex-1)*10 && k< (maxRange))
                                {
                                    BL.ArtistsBL aBL = new BL.ArtistsBL();
                                    chosenBand = aBL.GetArtistInforListByBandName(bandList[k]);
                                    DisplayArtistsListInTheBand(pageIndex, chosenBand);
                                    DrawDownwardsLine();
                                    Console.WriteLine($"  Are you sure to choose this band\n");
                                    if(YNQuest.ask_YesOrNo()) 
                                    {
                                        endLoop = true;
                                        Console.WriteLine($"\n  Select band successfully !");
                                        DrawUpwardLine();
                                    }
                                    else 
                                    {
                                        Console.Clear();
                                        Console.WriteLine($"Please choose the band for the song !");
                                        chosenBand = null;
                                    } 
                                }
                            }
                            break;
                    }

                    if (pageIndex <= 0)
                    {
                        Console.WriteLine("Sorry! We dont have a negative page :(");
                        Console.WriteLine("We'll get you back to the first page");
                        Console.WriteLine($"\nPlease choose the band for the song !");
                        pageIndex = 1; // first page number = 1
                    }

                    if (pageIndex > pageNum)
                    {
                        Console.WriteLine("Sorry! We dont have that page :(");
                        Console.WriteLine("We'll get you back to the last page");
                        Console.WriteLine($"\nPlease choose the band for the song !");
                        pageIndex = pageNum; // last page number = pageNu,
                    }
                    
                }
                while(!endLoop);
            }
            return chosenBand;
        }
        private static void DisplayArtist(Artists a)
        {
            Console.Clear();
            UIHelper.MenuTitle("Displaying Artist's Information");
            var table = new ConsoleTable("Id", $"{a.artistId}");
            table.AddRow("Full Name", $"{a.artistFirstName} {a.artistLastName}");
            if(a.isSinger) table.AddRow("Stage Name", $"{a.stageName}");
            if(a.isBand) table.AddRow("Band Name", $"{a.bandName}");
            if(a.isProducer) table.AddRow("Produced Alias", $"{a.producerAlias}");
            if(a.isWriter) table.AddRow("Writer Alias", $"{a.songwriterAlias}");
            table.AddRow("Birthday", $"{((Persistence.Enum.Month)a.born.Month).ToString()} {a.born.Day}, {a.born.Year}");
            table.Write();

        }
        
        private static List<Artists> RemoveArtistThatChosenBefore(List<Artists> listValidArtists, List<Artists> listChosenArtitstBefore)
        { 
            if (listValidArtists != null)
            {
                int n = listValidArtists.Count;
                for(int i = 0; i < n; i++)
                {
                    foreach(var item in listChosenArtitstBefore)
                    {
                        //Console.WriteLine(">> " + i);
                        if (listValidArtists[i].artistId == item.artistId) 
                        {
                            listValidArtists.RemoveAt(i);
                            i--;
                            n--;
                            break;
                        }
                    }
                }
            }
            return listValidArtists;
        }
        public static Genres DisplayGenresPages(List<Genres> genresList, List<Genres> listGenreChosenAlready)
        {
            bool endLoop = false;
            int pageIndex = 1;
            Genres chosenGenre = new Genres();

            int pageNum = UIHelper.GetNumberOfPages(genresList.Count());

            genresList = RemoveGenreThatChosenBefore(genresList, listGenreChosenAlready);
            
            if (genresList.Count <= 0) Console.WriteLine($"Sorry! We dont have any valid genre :(");

            else
            {
                do
                {
                    UIHelper.MenuTitle($" Choosing the genres for the song");
                    int maxRange = pageIndex*10;
                    UIHelper.DisplayGenreList(pageIndex, genresList);
                    Console.WriteLine("Choose a genre: Enter genre's ID | Next Page: N | Previous Page: P | Exit: E");
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
                            chosenGenre = null;
                            endLoop = true;
                            break;
                        default:
                            if ( Int32.TryParse(choose, out int k) )
                            {
                                //Console.Clear();
                                if(maxRange > genresList.Count) maxRange = genresList.Count;

                                if ( k >= (pageIndex-1)*10 && k< (maxRange))
                                {
                                    DrawDownwardsLine();
                                    Console.WriteLine($"  Are you sure to choose '{genresList[k].genreTitle}' ?\n ");
                                    if(YNQuest.ask_YesOrNo()) 
                                    {
                                        chosenGenre = genresList[k];
                                        endLoop = true;
                                        Console.WriteLine($"\n  Select {genresList[k].genreTitle} successfully !");
                                        DrawUpwardLine();
                                    }
                                    else chosenGenre = null;
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
            return chosenGenre;
        }
        private static List<Genres> RemoveGenreThatChosenBefore(List<Genres> listValidGenres, List<Genres> listChosenGenreBefore)
        { 
            for(int i = 0; i < listValidGenres.Count(); i++)
            {
                foreach(var item in listChosenGenreBefore)
                {
                    if (listValidGenres[i].genreId == item.genreId) listValidGenres.RemoveAt(i);
                    
                }
            }
            return listValidGenres;
        }
        public static void DisplayGenreList(int pageIndex, List<Genres> genresList)
        {
            int maxRange = 10*pageIndex;
            if (maxRange >= genresList.Count) maxRange = genresList.Count;

            var table = new ConsoleTable("#", "Genre Title");
            for (int i = (pageIndex-1)*10; i< maxRange; i++)
            {
                table.AddRow(i, genresList[i].genreTitle);
            }
            table.Write();
            
        }
        public static void DisplayInsertingSongInformation(Song insertingSong)
        {
            Console.Clear();
            UIHelper.MenuTitle("Adding A New Song");
            var table = new ConsoleTable("Title", $"{insertingSong.songName}");
            if (insertingSong.singer != null || insertingSong.band != null) table.AddRow("Artists", $"{RemoveCoincideSingerOrBand(insertingSong)}");
            
            if (insertingSong.writer != null)
            {
                string writer = "";
                foreach (var val in insertingSong.writer) writer += val.songwriterAlias + ", ";
                if (writer.Length > 0) writer = writer.Trim().Remove(writer.Length - 2);
                table.AddRow("Written By", $"{writer}");
            }

            if (insertingSong.produced != null)
            {
                string producer = "";
                foreach (var val in insertingSong.produced) producer += val.producerAlias + ", ";
                if (producer.Length > 0) producer = producer.Trim().Remove(producer.Length - 2);
                table.AddRow("Produced by", $"{producer}");
            }

            if (insertingSong.album != null) table.AddRow("Album", $"{insertingSong.album}");
            if (insertingSong.copyright != null) table.AddRow("Coppyright", $"{insertingSong.copyright}");
            if (insertingSong.releaseDate != DateTime.MinValue) table.AddRow("Release Date", $"{((Persistence.Enum.Month)insertingSong.releaseDate.Month).ToString()} {insertingSong.releaseDate.Day}, {insertingSong.releaseDate.Year}");

            if (insertingSong.genres != null)
            {
                string genres = "";
                foreach (var val in insertingSong.genres) genres += val.genreTitle + ", ";
                if (genres.Length > 0) genres = genres.Trim().Remove(genres.Length - 2);
                table.AddRow("Produced by", $"{genres}");
            }
            table.Write();
            //if (insertingSong.songStatus.ToString() != null) Console.WriteLine($"Song's status: {insertingSong.songStatus.ToString()}");
        }
        public static void DisplayInsertingArtistInformation(Artists insertingArtist)
        {
            Console.Clear();
            UIHelper.MenuTitle("Adding A New Song");
            var table = new ConsoleTable("First Name", $"{insertingArtist.artistFirstName}");
            if (insertingArtist.artistLastName != null) table.AddRow("Last Name", $"{insertingArtist.artistLastName}");
            table.AddRow("Gender", $"{insertingArtist.gender.ToString()}");
            if (insertingArtist.born != DateTime.MinValue) table.AddRow("Birthday", $"{((Persistence.Enum.Month)insertingArtist.born.Month).ToString()} {insertingArtist.born.Day}, {insertingArtist.born.Year}");
            if (insertingArtist.isSinger) table.AddRow("Stage Name", $"{insertingArtist.stageName}");
            if (insertingArtist.isSinger) if (insertingArtist.isBand) table.AddRow("Band Name", $"{insertingArtist.bandName}");
            if (insertingArtist.isWriter) table.AddRow("Writer Alias", $"{insertingArtist.songwriterAlias}");
            if (insertingArtist.isWriter) table.AddRow("Producer Alias", $"{insertingArtist.producerAlias}");
            table.Write();
            //if (insertingSong.songStatus.ToString() != null) Console.WriteLine($"Song's status: {insertingSong.songStatus.ToString()}");
        }
        public static void DisplayInsertinAccountInformation(string user_name, string firstName, string lastName, Persistence.Enum.Gender gender, string gmail)
        {
            Console.Clear();
            UIHelper.MenuTitle("Adding A New Song");
            var table = new ConsoleTable("User Name", $"{user_name}");
            if (firstName != null) table.AddRow("First Name", $"{firstName}");
            if (lastName != null) table.AddRow("Last Name", $"{lastName}");
            table.AddRow("Gender", $"{gender.ToString()}");
            if (gmail != null) table.AddRow("Gmail", $"{gmail}");
            table.Write();
            //if (insertingSong.songStatus.ToString() != null) Console.WriteLine($"Song's status: {insertingSong.songStatus.ToString()}");
        }
    }
}