using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ConsolePL.UTIL
{ 
    public class StringUTil
    {
        public static bool CheckEmpty(string str)
        {
            bool TheCheck = true;
            if (string.IsNullOrWhiteSpace(str))
            {
                Console.WriteLine("You are not allowed to leave this blank");
                Console.Write("Please try again: ");
                TheCheck = false;
            }
            return TheCheck;
        }
        public static string insertPassword()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if(key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if(!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            return pass;
        }
        public static bool CheckValidPassword(string password)
        {
            bool Check = true;
            bool isPassword = Regex.IsMatch(password, @"(?=^.{8,50}$)(?=.*\d)(?=.*[a-zA-Z])(?!.*\s)[0-9a-zA-Z*$+?_&=!%{}/@#.^]*$", RegexOptions.IgnoreCase);
            
            if ( !isPassword )
            {
                Console.WriteLine("\nYour password is not strong enough !!\n");
                Console.WriteLine("Your password must be: ");
                Console.WriteLine(" + At least one alphabetic");
                Console.WriteLine(" + At least one numeric character");
                Console.WriteLine(" + May include some special characters.");
                Console.WriteLine(" + Password length must be between 8 and 50 characters\n");
                Check = false;
            }
            return Check;
        }
        public static string GetNotEmptyString(string title)
        {
            string str = null;
            bool endloop = false;
            do
            {
                Console.Write(title);
                str = Console.ReadLine();
                if (!CheckEmpty(str))
                {
                    Console.Write("Retry? ");
                    if (!YNQuest.ask_YesOrNo())
                    {
                        str = null;
                        endloop = true;
                    }
                }
                else
                {
                    endloop = true;
                }
            }
            while (!endloop);
            return str;
        }
        public static int GetValidNumber(string title)
        {
            bool endloop = false;
            int theNum = -1;
            do
            {
                Console.Write(title);
                try
                {
                    theNum = int.Parse(Console.ReadLine());
                    endloop = true;
                }
                catch
                {
                    Console.WriteLine("Invalid Number!!");
                    Console.Write("Retry? ");
                    if (!YNQuest.ask_YesOrNo())
                    {
                        theNum = -1;
                        endloop = true;
                    }
                }
            }
            while (!endloop);
            return theNum;
        }
        public static bool CheckValidEmail(string str)
        {
            bool Check = true;
            try
            {
                bool isEmail = Regex.IsMatch(str, @"\A(?:[a-z0-9!#$%&*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                
                if ( !isEmail )
                {
                    Console.WriteLine("Invalid email !!");
                    Check = false;
                }
            }
            catch
            {
                Check = false;
            }
            return Check;
        }
        public static bool CheckNoQuoteCharecterInString(string str)
        {
            bool Check = true;
            try
            {
                bool isEmail = Regex.IsMatch(str, @"^[a-zA-Z0-9%$#@!&^\s-]+$", RegexOptions.IgnoreCase);
                
                if ( !isEmail )
                {
                    Console.WriteLine("Invalid special character!!");
                    Check = false;
                }
            }
            catch
            {
                Check = false;
            }
            return Check;  
        }
        public static string GetValidName(string title)
        {
            string str = null;
            bool endloop = false;
            do
            {
                str = GetNotEmptyString(title);
                if (!CheckNoQuoteCharecterInString(str))
                {
                    Console.Write("Retry? ");
                    if (!YNQuest.ask_YesOrNo())
                    {
                        str = null;
                        endloop = true;
                    }
                }
                else endloop = true;
            }
            while (!endloop);
            return str;
        }
        public static string GetValidUserName(string title)
        {
            string str = null;
            bool endloop = false;
            do
            {
                Console.Write(title);
                str = Console.ReadLine();
                if (!CheckEmpty(str))
                {
                    Console.Write("Retry? ");
                    if (!YNQuest.ask_YesOrNo())
                    {
                        str = null;
                        endloop = true;
                    }
                }
                else
                {
                    if (!CheckValidUserName(str))
                    {
                        Console.Write("Retry? ");
                        if (!YNQuest.ask_YesOrNo())
                        {
                            str = null;
                            endloop = true;
                        }
                    }
                    else
                    {
                        endloop = true;
                    }
                }
            }
            while (!endloop);
            return str;
        }
        private static bool CheckValidUserName(string str)
        {
            //
            bool result = true;
            try
            {
                bool isEmail = Regex.IsMatch(str, @"^([a-zA-Z])[a-zA-Z_-]*[\w_-]*[\S]$|^([a-zA-Z])[0-9_-]*[\S]$|^[a-zA-Z]*[\S]$", RegexOptions.IgnoreCase);
                
                if ( !isEmail )
                {
                    Console.WriteLine("Invalid User Name !\n");
                    Console.WriteLine("Your user name must be like: ");
                    Console.WriteLine(" + Is alphanumeric");
                    Console.WriteLine(" + Starts with an alphabet and contains no special characters other than underscore or dash\n");
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public static string GetFixedEmail()
        {
            string str = GetNotEmptyString("Enter gmail: ");

            if (str != null)
            {
                str = str.Trim();
                if (!CheckValidEmail(str))
                {
                    Console.WriteLine("Do you want to retry?");
                    if (YNQuest.ask_YesOrNo())
                    {
                        str = GetFixedEmail();
                    }
                    else
                    {
                        str = null;
                    }
                }
            }
            else
            {
                str = null;
            }
            return str;
        }

        private static bool check_ValidPhoneNumber(string str)
        {
            bool Check = true;
            bool isPhone = Regex.IsMatch(str, @"((09|03|07|08|05)+([0-9]{8})\b)", RegexOptions.IgnoreCase);
            
            if ( !isPhone )
            {
                Check = false;
            }
            return Check;
        }

        public static string fix_PhoneNumber()
        {
            string str = Console.ReadLine().Trim();

            while(!check_ValidPhoneNumber(str))
            {
                str = Console.ReadLine().Trim();
            }

            return str;
        } 
        public static string UpperFirstLecter(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower().Trim());  
        }
        public static string HideGmail(string gmail)
        {
            string hideMail = null;
            for (int i = 0; i < 5; i++)//Get 5 first char in gmail
            {
                hideMail += gmail[i];
            }
            int atIndex = gmail.Length;
            for (int i = 5; i< gmail.Length; i++)
            {
                if (gmail[i] == '@')
                {
                    atIndex = i;
                    break;
                }
                else
                {
                hideMail += '*';
                }
            }
            for (int i = atIndex; i< gmail.Length; i++)
            {
                hideMail += gmail[i];
            }
            return hideMail;
        }

        private static bool CaculateAge(DateTime theDate)
        {
            
            bool theCheck = true;

            var today = DateTime.Today;

            var age = today.Year - theDate.Year;

            if (age <= 0)
            {
                Console.WriteLine("Is this person has not been born yet?");
                theCheck = false;
            }

            if (age <= 3 && 1<=age)
            {
                Console.WriteLine("This person cannot speak well yet :(");
                theCheck = false;
            }
            if (age <= 6 && 5<=age)
            {
                Console.WriteLine("Wow! What a genius!");
                theCheck = true;
            }
            
            return theCheck;
        }

        public static DateTime GetTheTrueBithday()
        {
            DateTime birthdate = DateTime.MinValue;
            bool endloop = false;
            do
            {
                birthdate = ReadBirthDate();
                if (birthdate != DateTime.MinValue)
                {
                    if (!CaculateAge(birthdate))
                    {
                        Console.WriteLine("Do you want to retry?");
                        if (YNQuest.ask_YesOrNo())
                        {
                            endloop = false;
                        }
                        else
                        {
                            Console.WriteLine("Cancel enter the birthday date!");
                            endloop = true;
                        }
                    }
                    else
                    {
                        endloop = true;
                    }
                } else endloop = true;
            }
            while(!endloop);

            return birthdate;
        }

        private static DateTime ReadBirthDate()
        {
            DateTime birthdate;
            bool endloop = false;
            do
            {
                var tempBirthDay = GetNotEmptyString("Enter the birthday date: ");
                if (DateTime.TryParse(tempBirthDay, out DateTime birthDay))
                {
                    birthdate = birthDay;
                    endloop = true;
                }
                else
                {
                    birthdate = DateTime.MinValue;
                    Console.WriteLine("Error format !!");
                    Console.WriteLine("You must enter in the format: 'mm/dd/yyyy' ");

                    Console.WriteLine("Do you want to retry?");
                    if (YNQuest.ask_YesOrNo())
                    {
                        endloop = false;
                    }
                    else
                    {
                        Console.WriteLine("Cancel enter the birthday date!");
                        birthdate = DateTime.MinValue;
                        endloop = true;
                    }
                }
            }
            while (!endloop);

            return birthdate;
        }
    }
}
