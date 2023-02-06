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
                    var ChonLoginMenu = UIHelper.ChooseOptionMenu(mOption, menuTitle);
                    switch (ChonLoginMenu)
                    {
                        case "1":
                            //Console.Clear();
                            customer = this.AutoLogin();
                            bool autoLogin = false;
                            if (customer == null)
                            {
                                autoLogin = false;
                                customer = this.GetLogin();
                                if (customer != null)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\nLogin Success !!!!");
                                    if (!customer.staff)
                                    {
                                        Console.WriteLine("\nDo you want us to remember your account ?");
                                        Console.WriteLine("Then you can easy automatic login without typing ?");
                                        if (YNQuest.ask_YesOrNo())
                                        {
                                            this.SetUpAutoLogin(customer);
                                        }
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
                            this.ForgotPassword();
                            break;
                        case "4":
                            Console.Clear();

                            Console.WriteLine("Goodbye !!");
                            EndloginLoop = false;
                            //manager.Write(students.dictionary, fileName);

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
            //Console.Write("User Name: ");
            Customer cus = null;
            //string user_name = InsertUserName();
            string user_name = StringUTil.GetNotEmptyString("User Name: ");
            if (user_name != null)
            {
                if ( !cBL.CheckUserName(user_name))
                {
                    Console.WriteLine($"This username \"{user_name}\" could not be found ");
                    Console.WriteLine("Do you want to retry ?");
                    if ( YNQuest.ask_YesOrNo() )
                    {
                        cus = this.GetLogin();
                    }
                }
                else
                {
                    Console.Write("Password: ");
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
  
        private string InsertUserName()
        {
            string user_name = Console.ReadLine();
            while ( !StringUTil.CheckEmpty(user_name) )
            {
                user_name = Console.ReadLine();
            }
            return user_name;
        }
        private Customer Register()
        {
            using (null)
            {
                //Console.Write("User Name: ");
                Customer cus = null;
                string user_name = StringUTil.GetValidUserName("User Name: ");
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
                        string firstName = StringUTil.GetNotEmptyString("First Name: ");
                        if (firstName != null)
                        {
                            firstName = StringUTil.UpperFirstLecter(firstName);
                            string lastName = StringUTil.GetNotEmptyString("Last Name: ");
                            if (lastName != null)
                            {
                                lastName = StringUTil.UpperFirstLecter(lastName);
                                Console.Write("Password: ");
                                string password = StringUTil.insertPassword();
                                if (StringUTil.CheckValidPassword(password))
                                {
                                    Console.Write("\nConfirm Password: ");
                                    string rePassword = StringUTil.insertPassword();
                                    if (password == rePassword)
                                    {
                                        Console.WriteLine();
                                        string gmail = StringUTil.GetFixedEmail().ToLower();
                                        if (gmail!= null)
                                        {
                                            if (cBL.CheckGmail(gmail))
                                            {
                                                Console.WriteLine("This gmail is already in use");
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
                                                    cus = this.GetCustomer(cBL.GetMaxIdInCustomer() + 1, user_name.ToLower(), password, gmail, firstName, lastName);
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
                Console.Clear();
                return cus;
            }
        }
        
        private Customer GetCustomer(int userId, string user_name, string password, string gmail, string firstName, string lastName)
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
            return cus;
        }
        private void ForgotPassword()
        {
            using(null)
            {
                //bool result = false;
                //Console.Write("Enter your user Name: ");
                //Customer cus = null;
                //string user_name = InsertUserName();
                string user_name = StringUTil.GetNotEmptyString("Enter your User Name: ");
                if (user_name != null)
                {
                    //if ( !cBL.CheckUserName(user_name))
                    //{
                        //Console.WriteLine($"This username \"{user_name}\" could not be found ");
                        //Console.WriteLine("We'll send you back to Login menu");
                    //}
                    //else
                    //{
                    Customer cus = cBL.GetCustomer(user_name);
                    if (cus!= null)
                    {
                        //Console.WriteLine();
                        if (cus.staff)
                        {
                            Console.WriteLine("This is a staff account !!");
                            Console.WriteLine("Contact to owner to get back your password");
                        }
                        else
                        {
                            if(SendingCode.enterCode(cus.gmail, user_name))
                            {
                                Console.Write("Enter your new Password: ");
                                string password = StringUTil.insertPassword();
                                if (StringUTil.CheckValidPassword(password))
                                {
                                    Console.Write("\nConfirm your Password: ");
                                    string rePassword = StringUTil.insertPassword();
                                    if (password == rePassword)
                                    {
                                        if (cBL.UpdateNewPassword(cus.userId, password))
                                        {
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
                                        Console.WriteLine("\nPasswords did not match. Please enter the same password in both fields.");
                                        Console.WriteLine("We'll send you back to Login menu");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("\nSome thing wrong :(");
                                    Console.WriteLine("We'll send you back to Login menu");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"This username \"{user_name}\" could not be found ");
                        Console.WriteLine("We'll send you back to Login menu");
                    }
                }
                else
                {
                    Console.WriteLine("Cancel forgot password !");
                }
            }
            Console.Clear();
            //return result;
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