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
                    Console.WriteLine("> Please Wait!!! This may take a few second... ");

                    var DownFile = new WebClient();
                    DownFile.DownloadFile(downloadLink, fileName + format);
                    Console.WriteLine();
                    Console.WriteLine(">>> Download Complete");
                }
                catch
                {
                    Console.WriteLine();
                    Console.WriteLine(">>> Download Fail :(");
                    //Console.ForegroundColor = ConsoleColor.Red;
                }
            }
        }
    }
}