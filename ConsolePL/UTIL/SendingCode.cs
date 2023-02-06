using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using Persistence;
using DAL.Encrypt;
using MySql.Data.MySqlClient;

namespace ConsolePL.UTIL
{
    public class SendingCode
    {
        //private static StreamManager helperGmailFile = new StreamManager();
        //private static string fileName = "Helper Gmail.txt";
        private static int timeInSec = 300;
        private static string encryptEmail = "7z6mSGXEaQbxsx1ahUPyYTBosLUwtHFIKX5I/hPI9J0=";
        private static string encryptPassword = "l7EJk8rP3+SA84lzJSzCqogyVzn0VNYq+GVqlGuc6RA=";
        private static string email = EncryptTextTool.DecryptText(encryptEmail);
        private static string emailPass = EncryptTextTool.DecryptText(encryptPassword);
        
        // private static string getHelperGmail()
        // {
        //     string encyptgmail = helperGmailFile.ReadHeperGmail(fileName)[0].ToString();
        //     string gmail = null;
        //     if (encyptgmail != null)
        //     {
        //         gmail = EncryptTextTool.DecryptText(encyptgmail);
        //     }
        //     else
        //     {
        //         getNewHelperFile();
        //         gmail = getHelperGmail();
        //     }
        //     return gmail;
        // }
        // private static void getNewHelperFile()
        // {
        //     string fileURL = "https://drive.google.com/uc?export=download&id=1n0I_Evgj9GaUypxFebtLnJDSau8hQZt6";
        //     AtomicDownload.DownloadFile(fileURL, fileName);
        // }
        public static bool enterCode(string receiveGmail, string user_name)
        {
            /// Generate Random Code
            Random r = new Random();
            int ranNum = r.Next(1000, 9999);
            bool enterRightcode = true;

            //Console.WriteLine(ranNum.ToString());
            
            string to = receiveGmail;

            string subject = "E-mail Authentication Notice";
            string message = $"Hi, {user_name} ! \nPlease enter the verification code below on our App. The code expires \nin 5 minutes, so be sure to enter within 5 minutes.[  {ranNum}  ]";

            if(!SendGmail(email, emailPass, to, subject, message))
            {
                Console.WriteLine("Cannot send email !!");
            }
            else
            {
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                Task test = new Task(
                    () => {
                        for (int i=timeInSec; i >= 0; i--)
                        {
                            if (token.IsCancellationRequested)
                            {
                                token.ThrowIfCancellationRequested();
                                return;
                            }
                            Thread.Sleep(1000);
                        }
                        enterRightcode = false;
                        Console.ResetColor();
                        Console.WriteLine("\nYour code is expired");
                        Console.Write("Press 'Enter' to continue");
                    }, token
                );

                test.Start();
                
                Console.WriteLine($"\nWe sent to '{StringUTil.HideGmail(receiveGmail)}' a code");
                Console.WriteLine("If you didn't get the email, check your junk folder or try again.");
                Console.WriteLine("Your code will be expried after 5 minutes");

                Console.ForegroundColor = ConsoleColor.Green;  
                Console.Write("Enter your Authentic Code here: ");
                string enterRanNum = Console.ReadLine().Trim();
                Console.ResetColor();

                if (enterRightcode)
                {
                    if (enterRanNum == ranNum.ToString())
                    {
                        tokenSource.Cancel();
                        enterRightcode = true;
                        Console.WriteLine("Your code is right");
                    }
                    else
                    {
                        tokenSource.Cancel();
                        enterRightcode = false;
                        Console.WriteLine("You entered wrong code :(");
                    }
                }
            }
            return enterRightcode;
        }
        private static string Mailmessage(string user_name, int code)
        {
            string m1 = $"Hi, {user_name}!";
            string m2 = "\nPlease enter the verification code below on our App. The code expires";
            string m3 = "\nin 3 minutes, so be sure to enter within 3 minutes.";
            string m4 = "\n┌────────────┐";
            string m5 = "\n│            │";
            string m6 = "\n│    {ranNum}    │";
            string m7 = "\n└────────────┘"; 
            string message = m1 + m2 + m3 + m4 +  m5 + m6 + m7;
            return message;
        }

        private static bool SendGmail(string email, string emailPassword, string address, string subject, string message)
        {
            try
            {
                var loginInfo = new NetworkCredential(email, emailPassword);
                var msg = new MailMessage();
                var stmpClient = new SmtpClient("smtp.gmail.com", 587);

                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(address));
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = true;

                stmpClient.UseDefaultCredentials = false;
                stmpClient.Credentials = loginInfo;
                stmpClient.EnableSsl = true;
                stmpClient.Send(msg);
            }
            catch
            {
                //Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public static void SendSuccessRegister(Customer cus)
        {
            string to = cus.gmail;

            string subject = "E-mail Authentication Notice";
            string message = $"Hi, {cus.firstName} {cus.lastName} ! This is your Account Information. User name: {cus.user_name}, Password: {cus.password}";
            
            if(!SendGmail(email, emailPass, to, subject, message))
            {
                Console.WriteLine("Cannot send email !!");
            }
        }
        public static void SendPremiumGmail(Order order)
        {
            string to = order.orderCustomer.gmail;

            string subject = "E-mail Authentication Notice";
            string message = $"Hi, {order.orderCustomer.firstName} {order.orderCustomer.lastName}!. This is your order detail: {((Persistence.Enum.Month)order.orderDate.Month).ToString()} {order.orderDate.Day}, {order.orderDate.Year} OrderId: #{order.orderId + order.orderCustomer.userId*1000}, ,Total: {order.total}vnd";
            Console.WriteLine("Please wait!!! This may take a few second !");
            if(!SendGmail(email, emailPass, to, subject, message))
            {
                Console.WriteLine("Cannot send email !!");
            }
            else Console.WriteLine("We sent you a mail. Please check it!");
        }

    }
}
