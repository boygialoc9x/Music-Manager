using System;
using System.Collections;
using System.Collections.Generic;
using Persistence;
using BL;
using ConsolePL.UI;
using ConsolePL.UTIL;
using ConsoleTables;

public class ArtistMenu
{
    private ArtistsBL aBL = new ArtistsBL();
    private static string menuTitle = MagicNumber.artistMenu_menuTitle;
    public Artists display_ArtistsMenu(Customer customer, Artists artists)
    {
        bool EndloginLoop = true;

        ArrayList mOption = MagicNumber.artistMenu_Remove_NonePermOption( customer.staff, customer.premium);
        int removingNumber = MagicNumber.artistMenu_Get_numberOfRemovingMenuOption(customer.staff, customer.premium);

        while (EndloginLoop)
        {
            this.DisplayArtist(artists, customer.staff);
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
                    Artists nArtist = InsertNewArtist(artists);
                    if (nArtist != artists)
                    {
                        this.UpdateArtist(nArtist, artists);
                    }
                    else
                    {
                        Console.WriteLine("Nothing change!");
                        Console.WriteLine("Cancel Update");
                    }
                    break;
                case "2":
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
        return artists;
    }

    private void DisplayArtist(Artists a, bool staff)
    {
        using (null)
        {
            var table = new ConsoleTable("", "");
            if (staff)
            {
                table.AddRow("Artist's Id", $"{a.artistId}");
            }
            table.AddRow("Artist's first name", $"{a.artistFirstName}");
            table.AddRow("Artist's last name", $"{a.artistLastName}");
            table.AddRow("Pseudonym/Band's name", $"{a.theArtist}");
            table.AddRow("Birthday", $"{a.born.ToString("MM/dd/yyyy")}");
            table.Write();
        }
    }

    private Artists InsertNewArtist(Artists oldArtist)
    {
        Artists nArtist = new Artists();

        nArtist.artistId = oldArtist.artistId;

        Console.WriteLine("Do you want to change this artist's first name ?");
        if (YNQuest.ask_YesOrNo())
        {
            nArtist.artistFirstName = StringUTil.GetNotEmptyString("Enter the artist's first name: ");
            nArtist.artistFirstName = StringUTil.UpperFirstLecter(nArtist.artistFirstName);
        }
        else
        {
            nArtist.artistFirstName = oldArtist.artistFirstName;
        }
        
        Console.WriteLine("Do you want to change this artist's last name ?");
        if (YNQuest.ask_YesOrNo())
        {
            nArtist.artistLastName = StringUTil.GetNotEmptyString("Enter the artist's last name: ");
            nArtist.artistLastName = StringUTil.UpperFirstLecter(nArtist.artistLastName);
        }
        else
        {
            nArtist.artistLastName = oldArtist.artistLastName;
        }

        Console.WriteLine("Do you want to change this artist's pseudonym / Band's name ?");
        if (YNQuest.ask_YesOrNo())
        {
            nArtist.theArtist = StringUTil.GetNotEmptyString("Enter the artist's pseudonym / Band's name :");
            nArtist.theArtist = StringUTil.UpperFirstLecter(nArtist.theArtist);
        }
        else
        {
            nArtist.theArtist = oldArtist.theArtist;
        }

        Console.WriteLine("Do you want to change this artist's birthday ?");
        if (YNQuest.ask_YesOrNo())
        {
            nArtist.born = StringUTil.GetTheTrueBithday();
        }
        else
        {
            nArtist.born = oldArtist.born;
        }

        nArtist.artistStatus = true;
                    
        return nArtist;
    }
    private Artists UpdateArtist(Artists newAt, Artists oldAt)
    {
        Artists a = oldAt;
        Console.Write("Save this Category? "); 
        if (YNQuest.ask_YesOrNo())
        {
            if (aBL.UpdateArtistInfor(newAt))
            {
                a = newAt;
                Console.WriteLine("Updated successfully!");
            }
            else
            {
                a = oldAt;
                Console.WriteLine("Some thing wrong! We couldn't update :( :(");
            }
        }
        else
        {
            a = oldAt;
            Console.WriteLine("Cancel update artist's information");
        }
        
        return a;
    }
    
}
