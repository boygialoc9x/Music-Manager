using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsolePL.UTIL
{
    public class YNQuest
    {
        public static bool ask_YesOrNo()
        {
            bool result = false;

            //Console.WriteLine("Try again?");
            //Console.Write("Yes/No: ");

            //string YN = Console.ReadLine().Trim().ToLower();
            string YN = null;
            do
            {
                Console.Write("Yes/No: ");
                YN = Console.ReadLine().Trim().ToLower();
            }
            while( !yesNoFormat(YN));

            if(YN == "y" || YN == "yes")
            {
                result = true;
            }
            else if(YN == "n" || YN == "no")
            {
                result = false;
            }
            return result;
        }

        private static bool yesNoFormat(string YN)
        {
            bool result = true;
            if( YN != "y" && YN != "yes" && YN != "n" && YN != "no")
            {
                Console.WriteLine("Invalid input! Please retry!");
                result = false;
            }
            return result;
        }
    }
}