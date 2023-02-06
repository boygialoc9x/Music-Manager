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
                    this.ChangeArtistInformation(artists);
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
    private Artists ChangeArtistInformation(Artists artist)
    {
        bool endLoop = true;
        string menuTitle = MagicNumber.ChooseUpdatePartArtist_menuTitle;
        ArrayList mOption = MagicNumber.ChooseUpdatePartArtist_menuOption(artist);
        while(endLoop)
        {
            UIHelper.MenuTitle("Displaying song's information");
            this.DisplayArtist(artist, true);
            var chooseUpdatePart = UIHelper.ChooseOptionMenu(mOption, menuTitle);
            switch (chooseUpdatePart)
            {
                case "1":
                    artist.artistFirstName = this.ChangeFirstName(artist);
                    break;
                case "2":
                    artist.artistLastName = this.ChangeLastName(artist);
                    break;
                case "3":
                    artist.gender = this.ChangeGender(artist);
                    break;
                case "4":
                    artist.born = this.ChangeBirthday(artist);
                    break;
                case "5":
                    if (artist.isSinger) artist.stageName = this.ChangeStageName(artist);
                    else if (this.ChangingToSinger(artist)) 
                    {
                        artist.isSinger = true;
                        artist.stageName = this.ChangeStageName(artist);
                    }
                    break;
                case "6":
                    if (artist.isSinger) 
                    {
                        if (artist.isBand) artist.bandName = this.ChangeBandName(artist);
                        else if (this.ChangingToBand(artist))
                        {
                            artist.isBand = true;
                            artist.stageName = this.ChangeBandName(artist);
                        }
                    }
                    else Console.WriteLine("This artist must be a singer to join a band");
                    break;
                case "7":
                    if (artist.isWriter) artist.songwriterAlias = this.ChangeWriterName(artist);
                    else if (this.ChangingToWriter(artist))
                    {
                        artist.isWriter = true;
                        artist.songwriterAlias = this.ChangeWriterName(artist);
                    }
                    break;
                case "8":
                    if (artist.isProducer) artist.producerAlias = this.ChangeProducerName(artist);
                    else if (this.ChangingToProducer(artist))
                    {
                        artist.isProducer = true;
                        artist.producerAlias = this.ChangeProducerName(artist);
                    }
                    break;
                case "9":
                    endLoop = false;
                    break;
                default:
                    break;
            }
        }
        return artist;
    }
    private bool ChangingToSinger(Artists a)
    {
        Console.Clear();   
        UIHelper.MenuTitle(" Changing Artist Information");
        Console.WriteLine("Are you sure this artist is a singer?");
        if (YNQuest.ask_YesOrNo())
        {
            a.isSinger = true;

            if (aBL.UpdateArtistInfor(a))
            {
                Console.Clear();
                Console.WriteLine("Update succesffully");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Update fail :(");
            }
            
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.isSinger;
    }
    private string ChangeStageName(Artists a)
    {
        Console.Clear();   
        string name=null; 
        UIHelper.MenuTitle(" Changing The Artist's Stage Name");
        Console.WriteLine("Are you sure to change this artist's stage name");
        if (YNQuest.ask_YesOrNo())
        {
            name = StringUTil.GetValidName("Enter stage name <Necessitate>: ");
            if(name != null)
            {
                name = StringUTil.UpperFirstLecter(name);
                a.stageName = name;

                if (aBL.UpdateArtistInfor(a))
                {
                    Console.Clear();
                    Console.WriteLine("Update succesffully");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Update fail :(");
                }
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update first name");
        }
        return a.stageName;
    }
    private string ChangeBandName(Artists a)
    {
        Console.Clear();   
        string name=null; 
        UIHelper.MenuTitle(" Changing The Artist's Band Name");
        Console.WriteLine("Are you sure to change this artist's band name");
        if (YNQuest.ask_YesOrNo())
        {
            name = StringUTil.GetValidName("Enter band name <Necessitate>: ");
            if(name != null)
            {
                name = StringUTil.UpperFirstLecter(name);
                a.bandName = name;

                if (aBL.UpdateArtistInfor(a))
                {
                    Console.Clear();
                    Console.WriteLine("Update succesffully");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Update fail :(");
                }
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.bandName;
    }
    private bool ChangingToBand(Artists a)
    {
        Console.Clear();   
        UIHelper.MenuTitle(" Changing Artist Information");
        Console.WriteLine("Are you sure this artist is in a band?");
        if (YNQuest.ask_YesOrNo())
        {
            a.isBand = true;

            if (aBL.UpdateArtistInfor(a))
            {
                Console.Clear();
                Console.WriteLine("Update succesffully");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Update fail :(");
            }
            
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.isBand;
    }
    private string ChangeWriterName(Artists a)
    {
        Console.Clear();   
        string name=null; 
        UIHelper.MenuTitle("Changing The Artist's Writer Alias");
        Console.WriteLine("Are you sure to change this artist's writer alias");
        if (YNQuest.ask_YesOrNo())
        {
            name = StringUTil.GetValidName("Enter writer alias <Necessitate>: ");
            if(name != null)
            {
                name = StringUTil.UpperFirstLecter(name);
                a.songwriterAlias = name;

                if (aBL.UpdateArtistInfor(a))
                {
                    Console.Clear();
                    Console.WriteLine("Update succesffully");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Update fail :(");
                }
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.songwriterAlias;
    }
    private bool ChangingToWriter(Artists a)
    {
        Console.Clear();   
        UIHelper.MenuTitle(" Changing Artist Information");
        Console.WriteLine("Are you sure this artist is a composer?");
        if (YNQuest.ask_YesOrNo())
        {
            a.isWriter = true;

            if (aBL.UpdateArtistInfor(a))
            {
                Console.Clear();
                Console.WriteLine("Update succesffully");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Update fail :(");
            }
            
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.isWriter;
    }
    private string ChangeProducerName(Artists a)
    {
        Console.Clear();   
        string name=null; 
        UIHelper.MenuTitle(" Changing The Artist's Producer Alias");
        Console.WriteLine("Are you sure to change this artist's producer alias");
        if (YNQuest.ask_YesOrNo())
        {
            name = StringUTil.GetValidName("Enter producer alias <Necessitate>: ");
            if(name != null)
            {
                name = StringUTil.UpperFirstLecter(name);
                a.producerAlias = name;

                if (aBL.UpdateArtistInfor(a))
                {
                    Console.Clear();
                    Console.WriteLine("Update succesffully");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Update fail :(");
                }
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.producerAlias;
    }
    private bool ChangingToProducer(Artists a)
    {
        Console.Clear();   
        UIHelper.MenuTitle("Changing Artist Information");
        Console.WriteLine("Are you sure this artist is a producer?");
        if (YNQuest.ask_YesOrNo())
        {
            a.isProducer = true;

            if (aBL.UpdateArtistInfor(a))
            {
                Console.Clear();
                Console.WriteLine("Update succesffully");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Update fail :(");
            }
            
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.isProducer;
    }
    private DateTime ChangeBirthday(Artists a)
    {
        Console.Clear();    
        DateTime born = DateTime.MinValue;
        UIHelper.MenuTitle("Changing The Artist's Birthday");
        Console.WriteLine("Are you sure to change this artist's birthday?");
        if (YNQuest.ask_YesOrNo())
        {
            UIHelper.DrawStraightLine();
            born = StringUTil.GetTheTrueBithday();
            if(born != DateTime.MinValue)
            {
                a.born = born;
                if (aBL.UpdateArtistInfor(a))
                {
                    Console.Clear();
                    Console.WriteLine("Update succesffully");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Update fail :(");
                }
            }
        } 
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.born;
    }
    private Persistence.Enum.Gender ChangeGender(Artists a)
    {
        string gender = "";
        bool endLoop = true;
        Console.Clear();
        UIHelper.MenuTitle("Changing The Gender");
        Console.WriteLine("Are you sure to change this artist's gender?");
        if (YNQuest.ask_YesOrNo())
        {
            do 
            {
                Console.Clear();
                UIHelper.MenuTitle("Changing The Gender");
                Console.WriteLine("Choose this artist's gender");
                UIHelper.DrawDownwardsLine();
                Console.WriteLine("   0. Male");
                Console.WriteLine("   1. Female");
                Console.WriteLine("   2. LBGT");
                UIHelper.DrawUpwardLine();
                Console.Write("    #The gender: ");
                gender = Console.ReadLine();
                if (Int32.TryParse(gender, out int k))
                {
                    if (0 <= k && k <= 2)
                    {
                        endLoop = false;
                        a.gender = (Persistence.Enum.Gender)k;
                    }
                    else endLoop = true;
                } else endLoop = true;
                
            }
            while (endLoop);

            if (aBL.UpdateArtistInfor(a))
            {
                Console.Clear();
                Console.WriteLine("Update succesffully");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Update fail :(");
            }   
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.gender;
    }
    private string ChangeFirstName(Artists a)
    {
        string fname = this.ChangeName("first name");
        if (fname != null) 
        {
            a.artistFirstName = fname;
            if (aBL.UpdateArtistInfor(a))
            {
                Console.Clear();
                Console.WriteLine("Update succesffully");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Update fail :(");
            }
        }
        else
        {
           Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.artistFirstName;

    }
    private string ChangeLastName(Artists a)
    {
        string fname = this.ChangeName("last  name");
        if (fname != null) 
        {
            a.artistLastName = fname;
            if (aBL.UpdateArtistInfor(a))
            {
                Console.Clear();
                Console.WriteLine("Update succesffully");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Update fail :(");
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Cancel update");
        }
        return a.artistLastName;
    }
    private string ChangeName(string change)
    {
        Console.Clear();
        string name = null;
        UIHelper.MenuTitle($"Changing {StringUTil.UpperFirstLecter(change)}");
        Console.WriteLine($" Are you sure to change the {change} ?");
        if (YNQuest.ask_YesOrNo()) 
        {
            Console.Clear();
            UIHelper.MenuTitle($"Changing {StringUTil.UpperFirstLecter(change)}");
            name = StringUTil.GetValidName($"Enter {change} <Necessitate>: ");
            if (name != null) name = StringUTil.UpperFirstLecter(name);
        }
        return name;
    }
    private void DisplayArtist(Artists a, bool staff)
    {
        using (null)
        {
            var table = new ConsoleTable("Full name", $"{a.artistFirstName} {a.artistLastName}");
            table.AddRow("Gender", $"{a.gender.ToString()}");
            if (staff)
            {
                table.AddRow("Artist's Id", $"{a.artistId}");
            }
            if(a.isSinger) table.AddRow("Stage Name", $"{a.stageName}");
            if(a.isBand) table.AddRow("Band Name", $"{a.bandName}");
            if(a.isProducer) table.AddRow("Produced Alias", $"{a.producerAlias}");
            if(a.isWriter) table.AddRow("Writer Alias", $"{a.songwriterAlias}");
            table.AddRow("Birthday", $"{((Persistence.Enum.Month)a.born.Month).ToString()} {a.born.Day}, {a.born.Year}");
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
            nArtist.stageName = StringUTil.GetNotEmptyString("Enter the artist's pseudonym / Band's name :");
            nArtist.stageName = StringUTil.UpperFirstLecter(nArtist.stageName);
        }
        else
        {
            nArtist.stageName = oldArtist.stageName;
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
