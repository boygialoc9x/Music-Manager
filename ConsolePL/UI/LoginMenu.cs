using System;
using System.Collections.Generic;
using System.Collections;
using Persistence;
using BL;
using ConsolePL.UTIL;


namespace ConsolePL.UI
{
    public class LoginMenu
    {
        private CustomerBL cBL = new CustomerBL();
        //private ChoosingUI UI = new ChoosingUI();
        static private string[] menuOption = MagicNumber.loginMenu_menuOption;
        static private string menuTitle = MagicNumber.loginMenu_menuTitle;
        public void option_loginMenu()
        {
            ArrayList mOption = new ArrayList();
            foreach(var val in menuOption)
            {
                mOption.Add(val);
            }

            using (null)
            {
                
                Customer customer = null;
                MainMenu mainMenu = new MainMenu();
                
                //Console.Clear();
                bool EndloginLoop = true;
                while (EndloginLoop)
                {
                    Console.Clear();
                    var ChonLoginMenu = UIHelper.ChooseOptionMenu(mOption, menuTitle);
                    switch (ChonLoginMenu)
                    {
                        case "1":
                            customer = this.AutoLogin();
                            bool autoLogin = false;
                            if (customer == null)
                            {
                                autoLogin = false;
                                customer = this.GetLogin();
                                if (customer != null)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Login Success !!!!");
                                    if (!customer.staff)
                                    {
                                        UIHelper.DrawStraightLine();
                                        Console.WriteLine("Do you want us to remember your account ?");
                                        Console.WriteLine("Then you can easy automatic login without typing ?");
                                        UIHelper.DrawStraightLine();
                                        if (YNQuest.ask_YesOrNo())
                                        {
                                            Console.Clear();
                                            this.SetUpAutoLogin(customer);
                                            Console.WriteLine("Remember your account successfully");
                                        }
                                        else Console.Clear();
                                    }
                                    mainMenu.option_mainMenu(customer, autoLogin);
                                }
                            }
                            else
                            {
                                autoLogin = true;
                                Console.Clear();
                                Console.WriteLine("\nLogin Success !!!!");
                                mainMenu.option_mainMenu(customer, autoLogin);
                            }
                            break;
                        case "2":
                            Console.Clear();
                            customer = this.Register();
                            if (customer != null)
                            {
                                if ( cBL.RegisterAccount(customer) )
                                {
                                    Console.Clear();
                                    Console.WriteLine("Register Success !!!!!");
                                    Console.WriteLine("Do you want us to send your account information to your email?");
                                    if (YNQuest.ask_YesOrNo())
                                    {
                                        SendingCode.SendSuccessRegister(customer);
                                    }
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Register fail !");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Cancel Register!");
                            }
                            break;
                        case "3":
                            Console.Clear();
                            Console.WriteLine("Goodbye !!");
                            EndloginLoop = false;
                            break;
                        default:
                            Console.Clear();

                            Console.WriteLine("Wrong Choice");
                            Console.WriteLine("Please TryAgain");

                            break;
                    }
                
                }
            }
        }

        private Customer GetLogin()
        {
            Console.Clear();
            UIHelper.MenuTitle("Logging In To The App");
            Customer cus = null;
            //string user_name = InsertUserName();
            string user_name = StringUTil.GetNotEmptyString(" [User Name] ");
            if (user_name != null)
            {
                if ( !cBL.CheckUserName(user_name))
                {
                    UIHelper.DrawStraightLine();
                    Console.WriteLine($"\nThis username \"{user_name}\" could not be found ");
                    Console.WriteLine("Do you want to retry ?");
                    if ( YNQuest.ask_YesOrNo() )
                    {
                        cus = this.GetLogin();
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.Write(" [Password]  ");
                    string password = StringUTil.insertPassword();
                    if ( !cBL.CheckPassword(user_name, password) )
                    {
                        Console.WriteLine("\nIncorrect password");
                        Console.WriteLine("Do you want to retry ?");
                        if ( YNQuest.ask_YesOrNo() )
                        {
                            cus = this.GetLogin();
                        }
                    }
                    else
                    {
                        cus = cBL.GetCustomer(user_name);
                    }
                }
            }
            else
            {
                Console.WriteLine("Cancel Login !");
            }
            return cus;
        }
  
        private Customer Register()
        {
            using (null)
            {
                Console.Clear();
                UIHelper.MenuTitle("Register An Account");
                Customer cus = null;
                Persistence.Enum.Gender theGender = 0;
                string user_name = StringUTil.GetValidUserName("\nUser Name: ");
                UIHelper.DisplayInsertinAccountInformation(user_name, null, null, theGender, null);
                if (user_name != null)
                {
                    if ( cBL.CheckUserName(user_name))
                    {
                        Console.WriteLine($"This username \"{user_name}\" is already in use");
                        Console.WriteLine("Do you want to retry?");
                        if ( YNQuest.ask_YesOrNo() )
                        {
                            cus = this.Register();
                        }
                    }  
                    else
                    {
                        string firstName = StringUTil.GetNotEmptyString("\nFirst Name: ");
                        if (firstName != null)
                        {
                            firstName = StringUTil.UpperFirstLecter(firstName);
                            UIHelper.DisplayInsertinAccountInformation(user_name, firstName, null, theGender, null);
                            string lastName = StringUTil.GetNotEmptyString("\nLast Name: ");
                            if (lastName != null)
                            {
                                lastName = StringUTil.UpperFirstLecter(lastName);
                                UIHelper.DisplayInsertinAccountInformation(user_name, firstName, lastName, theGender, null);
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
                                            theGender = (Persistence.Enum.Gender)k;
                                        }
                                        else endLoop = true;
                                    } else endLoop = true;
                                    
                                }
                                while (endLoop);
                                UIHelper.DisplayInsertinAccountInformation(user_name, firstName, lastName, theGender, null);
                                Console.Write("\nPassword: ");
                                string password = StringUTil.insertPassword();
                                if (StringUTil.CheckValidPassword(password))
                                {
                                    Console.Write("\nConfirm Password: ");
                                    string rePassword = StringUTil.insertPassword();
                                    if (password == rePassword)
                                    {
                                        Console.WriteLine("\n");
                                        string gmail = StringUTil.GetFixedEmail().ToLower();
                                        if (gmail!= null)
                                        {
                                            if (cBL.CheckGmail(gmail))
                                            {
                                                Console.WriteLine("\nThis gmail is already in use");
                                                Console.WriteLine("Do you want to retry?");
                                                if ( YNQuest.ask_YesOrNo() )
                                                {
                                                    cus = this.Register();
                                                }
                                            }
                                            else
                                            {
                                                if (SendingCode.enterCode(gmail, user_name))
                                                {
                                                    UIHelper.DisplayInsertinAccountInformation(user_name, firstName, lastName, theGender, gmail);
                                                    cus = this.GetCustomer(cBL.GetMaxIdInCustomer() + 1, user_name.ToLower(), password, gmail, firstName, lastName, theGender);
                                                }
                                                else
                                                {
                                                    Console.WriteLine("We'll send you back to login menu");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Do you want to retry ?");
                                            if ( YNQuest.ask_YesOrNo() )
                                            {
                                                cus = this.Register();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nPasswords did not match. Please enter the same password in both fields.");
                                        Console.WriteLine("Do you want to retry? ");
                                        if ( YNQuest.ask_YesOrNo() )
                                        {
                                            cus = this.Register();
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Do you want to retry? ");
                                    if ( YNQuest.ask_YesOrNo() )
                                    {
                                        cus = this.Register();
                                    }
                                }
                            }
                        }
                    }
                }
                return cus;
            }
        }
        
        private Customer GetCustomer(int userId, string user_name, string password, string gmail, string firstName, string lastName, Persistence.Enum.Gender gender)
        {
            Customer cus = new Customer();
            cus.userId = userId;
            cus.firstName = StringUTil.UpperFirstLecter(firstName);
            cus.lastName = StringUTil.UpperFirstLecter(lastName);
            cus.gmail = gmail;
            cus.password = password;
            cus.user_name = user_name;
            cus.accountStatus = true;
            cus.staff = false;
            cus.premium = false;
            cus.gender = gender;
            return cus;
        }
        private Customer AutoLogin()
        {
            Customer autoLogCustomer = AutomaticLogin.GetSomeCustomerData();
            if (autoLogCustomer != null)
            {
                if (cBL.CheckPassword(autoLogCustomer.user_name, autoLogCustomer.password))
                {
                    autoLogCustomer = cBL.GetCustomer(autoLogCustomer.user_name);
                }
                else
                {
                    autoLogCustomer = null;
                    Console.WriteLine("Auto login fail");
                    Console.WriteLine("We'll remove all cookie's data");
                    AutomaticLogin.RemoveCookie();
                }
            }
            return autoLogCustomer;
        }
        private void SetUpAutoLogin(Customer customer)
        {
            try
            {
            AutomaticLogin.RemoveCookie();
            AutomaticLogin.EncryptCustomerData(customer);
            Console.WriteLine("Setting success !!");
            }
            catch
            {
                Console.WriteLine("Setting Auto Login faill !!");
            }
        }
    }
}