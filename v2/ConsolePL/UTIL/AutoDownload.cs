using System;
using System.Net;

namespace ConsolePL.UTIL
{
    public class AtomicDownload
    {
        public static void DownloadFile(string downloadLink, string fileName, string format)
        {
            using (null)
            {     
                try 
                {
                    Console.WriteLine("> Please Wait !!!!");

                    var DownFile = new WebClient();
                    DownFile.DownloadFile(downloadLink, fileName + format);

                    Console.WriteLine("> Download Complete");
                }
                catch
                {
                    Console.WriteLine("> Download Fail :(");
                    //Console.ForegroundColor = ConsoleColor.Red;
                }
            }
        }
    }
}