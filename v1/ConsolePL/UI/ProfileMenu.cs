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
    private OrderBL oBL = new OrderBL();
    private static string menuTitle = MagicNumber.profileMenu_menuTitle;
    public void display_ProfileMenu(Customer customer)
    {
        bool EndLoop = true;
        this.DisplayCustomerInformation(customer);
        ArrayList mOption = MagicNumber.profileMenu_Remove_NonePermOption( customer.staff, customer.premium);
        int removingNumber = MagicNumber.profileMenu_Get_numberOfRemovingMenuOption(customer.staff, customer.premium);

        while (EndLoop)
        {
            var ChonMenu = UIHelper.ChooseOptionMenu(mOption, menuTitle);

            switch (ChonMenu)
            {
                case "1":
                    Console.Clear();
                    this.DisplayOrderDetail(customer);
                    EndLoop = false;
                    break;
                case "2":
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
    }
    private void DisplayOrderDetail(Customer customer)
    {
        Order order = oBL.GetOrderDetail(customer.userId);
        if (order != null)
        {
            UIHelper.MenuTitle("Your Order Detail");
            UIHelper.DrawDownwardsLine();
            this.DisplayTheOrderDetail("Name",customer.firstName + " " + customer.lastName);
            this.DisplayTheOrderDetail("Date", ((Persistence.Enum.Month)order.orderDate.Month).ToString() + " " +  order.orderDate.Day + ", " + order.orderDate.Year);
            this.DisplayTheOrderDetail("Number","#" + (order.orderId + customer.userId*1000).ToString());
            this.DisplayTheOrderDetail("Product", "Premium Membership");
            this.DisplayTheOrderDetail("Total", order.total.ToString() + "vnd");     
            UIHelper.DrawUpwardLine();
            Console.Write("Press any key to continute! ");
            Console.ReadKey();
            Console.Clear();
        } else Console.WriteLine("You dont have any order !");
    }
    private void DisplayTheOrderDetail(string title, string thestr)
    {
        string str = $"│ {title}:";
        string prestr = $"{thestr}";
        int n = prestr.Length;
        for (int j = str.Length; j<UIHelper.lineLength-1-n; j++)
        {
            prestr = ' ' + prestr;
        }
        Console.WriteLine($"{str+prestr}│");
    }
    private void DisplayCustomerInformation(Customer cus)
    {
        Console.Clear();
        UIHelper.MenuTitle("Displaying Your Profile");
        var table = new ConsoleTable("User Name: ", $"{cus.user_name}");
        table.AddRow("Full Name: ", $"{cus.firstName} {cus.lastName}");
        table.AddRow("Gender: ", $"{cus.gender.ToString()}");
        if (cus.staff)
        {
            table.AddRow("Permission: ", "STAFF");
        }
        else
        {
            if (cus.premium)
            {
                table.AddRow("Premium Member: ", "Yes");
            }
            else
            {
                table.AddRow("Premium Member: ", "No");
            }
            table.AddRow("Gmail: ", $"{StringUTil.HideGmail(cus.gmail)}");
        }
        table.Write();
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
    
}
