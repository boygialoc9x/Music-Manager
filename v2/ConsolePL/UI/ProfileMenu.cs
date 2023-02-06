using System;
using System.Collections;
using System.Collections.Generic;
using Persistence;
using BL;
using ConsolePL.UI;
using ConsolePL.UTIL;
using ConsoleTables;

public class ProfileMenu
{
    private CustomerBL cBL = new CustomerBL();
    private static string menuTitle = MagicNumber.songMenu_menuTitle;
    public Customer display_ProfileMenu(Customer customer)
    {
        bool EndLoop = true;
        this.DisplayCustomerInformation(customer);
        ArrayList mOption = MagicNumber.profileMenu_Remove_NonePermOption( customer.staff, customer.premium);
        int removingNumber = MagicNumber.profileMenu_Get_numberOfRemovingMenuOption(customer.staff, customer.premium);

        while (EndLoop)
        {
            //var ChonMenu = this.p_choose_optionMenu(mOption);
            var ChonMenu = UIHelper.ChooseOptionMenu(mOption, menuTitle);
            if ( Int32.TryParse(ChonMenu, out int k) )
            {
                if(k > mOption.Count - 0) 
                {                         
                k = k + removingNumber;   
                ChonMenu = k.ToString();  
                }                              
            }

            switch (ChonMenu)
            {
                case "1":
                    Console.Clear();   
                    customer = this.ChangeMyInformation(customer);     
                    break;
                case "2":
                    Console.Clear();
                    using (null)
                    {
                        Customer copCus = this.ChangeMyPassword(customer);
                        if (copCus != customer)
                        {
                            customer = copCus;
                        }
                        else
                        {
                            Console.WriteLine("Cancel change password !");
                        }
                    }
                    
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
        return customer;
    }
    private void DisplayCustomerInformation(Customer cus)
    {
        int maxPlaylist = MagicNumber.maxPlayList_Member;
        var table = new ConsoleTable("User Name: ", $"{cus.user_name}");
        table.AddRow("First Name: ", $"{cus.firstName}");
        table.AddRow("Last Name: ", $"{cus.lastName}");
        if (cus.staff)
        {
            maxPlaylist = 999999;
            table.AddRow("Permission: ", "STAFF");
        }
        else
        {
            if (cus.premium)
            {
                maxPlaylist = MagicNumber.maxPlayList_Premium;
                table.AddRow("Premium Member: ", "Yes");
            }
            else
            {
                table.AddRow("Premium Member: ", "No");
            }
            table.AddRow("Gmail: ", $"{StringUTil.HideGmail(cus.gmail)}");
        }
        table.AddRow("Number of playlist: ",$"{cus.playlistCreated}/{maxPlaylist}");
        table.Write();
    }

    private Customer ChangeMyInformation(Customer customer)
    {
        using(null)
        {
            Customer newCus = new Customer();
            newCus.premium = customer.premium;
            newCus.user_name = customer.user_name;
            newCus.accountStatus = customer.accountStatus;
            newCus.userId = customer.userId;
            newCus.staff = customer.staff;
            newCus.password = customer.password;
            if (customer.staff)
            {
                Console.WriteLine("This is a staff account !!");
                Console.WriteLine("Contact to owner to change your information");
                newCus = customer;
            }
            else
            {
                if(SendingCode.enterCode(customer.gmail, customer.user_name))
                {
                    Console.WriteLine("Do you want to change your first name ? ");
                    if(YNQuest.ask_YesOrNo())
                    {
                        newCus.firstName = StringUTil.GetNotEmptyString("First Name: ");
                        if (newCus.firstName != null)
                        {
                            newCus.firstName = StringUTil.UpperFirstLecter(newCus.firstName);
                        }
                        else
                        {
                            Console.WriteLine("We'll reuse your old first name!");
                            newCus.firstName = customer.firstName;
                        }
                    }
                    else
                    {
                        newCus.firstName = customer.firstName;
                    }

                    Console.WriteLine("Do you want to change your last name ? ");
                    if(YNQuest.ask_YesOrNo())
                    {
                        newCus.lastName = StringUTil.GetNotEmptyString("Last Name: ");
                        if (newCus.lastName != null)
                        {
                            newCus.lastName = StringUTil.UpperFirstLecter(newCus.lastName);
                        }
                        else
                        {
                            Console.WriteLine("We'll reuse your old last name!");
                            newCus.lastName = customer.lastName;
                        }
                    }
                    else
                    {
                        newCus.lastName = customer.lastName;
                    }

                    Console.WriteLine("Do you want to change your gmail? ");
                    if(YNQuest.ask_YesOrNo())
                    {
                        newCus.gmail = this.getNewGmail(newCus.user_name, customer.gmail);
                    }
                    else
                    {
                        newCus.gmail = customer.gmail;
                    }
                }
                else
                {
                    Console.WriteLine("Cancel change your information !");
                    newCus = customer;
                }
            }
            if (newCus != customer)
            {
                if (cBL.UpdateCustomerInformation(newCus))
                {
                    Console.WriteLine("Change your information successfully !");
                }
                else
                {
                    Console.WriteLine("Some thing wrong !!");
                    Console.WriteLine("We cannot change your information :(");
                }
            }
            return newCus;
        }
    }

    private string getNewGmail(string user_name, string oldGmail)
    {
        string gmail = StringUTil.GetFixedEmail();
        if (gmail!= null)
        {
            if (gmail != oldGmail)
            {
                if (cBL.CheckGmail(gmail))
                {
                    Console.WriteLine("This gmail is already in use");
                    Console.WriteLine("Do you want to retry?");
                    if ( YNQuest.ask_YesOrNo() )
                    {
                        gmail = getNewGmail(user_name, oldGmail);
                    }
                    else
                    {
                        Console.WriteLine("There nothing change with your gmail");
                        Console.WriteLine("We'll reuse your old gmail");
                        gmail = oldGmail;
                    }
                }
                else
                {
                    if (!SendingCode.enterCode(gmail, user_name))
                    {
                        Console.WriteLine("There nothing change with your gmail");
                        Console.WriteLine("We'll reuse your old gmail");
                        gmail = oldGmail;
                    }
                }
            }
            else
            {
                Console.WriteLine("This is your old gmail");
                Console.WriteLine("We'll keep it !");
                gmail = oldGmail;
            }
        }
        else
        {
            Console.WriteLine("Do you want to retry ?");
            if ( YNQuest.ask_YesOrNo() )
            {
                gmail = getNewGmail(user_name, oldGmail);
            }
            else gmail = oldGmail;
        }
        return gmail;
    }
    private Customer ChangeMyPassword(Customer cus)
    {
        using(null)
        {
            Customer newCus = cus;
            if (cus.staff)
            {
                Console.WriteLine("This is a staff account !!");
                Console.WriteLine("Contact to owner to change your password");
            }
            else
            {
                Console.WriteLine("Are you sure to change your password ? ");
                if (YNQuest.ask_YesOrNo())
                {
                    // if(SendingCode.enterCode(cus.gmail, cus.user_name))
                    // {
                    Console.Write("Enter your password: ");
                    string oldpassword = StringUTil.insertPassword();
                    if (oldpassword == cus.password)
                    {
                        Console.Write("\nEnter your new Password: ");
                        string password = StringUTil.insertPassword();
                        if (StringUTil.CheckValidPassword(password))
                        {
                            Console.Write("\nConfirm your Password: ");
                            string rePassword = StringUTil.insertPassword();
                            if (password == rePassword)
                            {
                                if (password != cus.password)
                                {
                                    if(SendingCode.enterCode(cus.gmail, cus.user_name))
                                    {
                                        if (cBL.UpdateNewPassword(cus.userId, password))
                                        {
                                            newCus.password = password;
                                            Console.WriteLine("\nChange password successfully");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nCannot change your password :(");
                                            Console.WriteLine("Some thing wrong :(");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nWe'll send you back to profile menu");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("\n\nYour password cannot be the same as your last passwords !");
                                    Console.WriteLine("Please enter a new password");
                                    Console.WriteLine("We'll send you back to profile menu");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\n\nPasswords did not match. Please enter the same password in both fields.");
                                Console.WriteLine("We'll send you back to profile menu");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nWe'll send you back to profile menu");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nYou entered wrong password");
                        Console.WriteLine("We'll send you back to profile menu");
                    }
                }
                else
                {
                    Console.WriteLine("\nCancel change your password !");
                }
            }
            return newCus;
        }
    }
    
}
