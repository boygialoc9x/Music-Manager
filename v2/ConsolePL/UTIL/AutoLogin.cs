using System;
using DAL.Encrypt;
using Persistence;

namespace ConsolePL.UTIL
{
    public class AutomaticLogin
    {
        private static string fileName = "Cookie.txt";
        private static StreamManager fileManager = new StreamManager();
        private static void EncryptCustomerData2Layer()
        {
            if (fileManager.Check(fileName))
            {
                string encryptString = fileManager.ReadEncryptStringAtLayer2(fileName);
            
                if (encryptString != null)
                {
                    RemoveCookie();
                    fileManager.WriteEncryptStringAtLayer2_Js(EncryptTextTool.EncryptText(encryptString), fileName);
                }
            }
        }
        public static void EncryptCustomerData(Customer customer)
        {
            Customer autloLoginCustomer = new Customer();

            autloLoginCustomer.user_name = EncryptTextTool.EncryptText(customer.user_name);
            autloLoginCustomer.password = EncryptTextTool.EncryptText(customer.password);

            fileManager.WriteEncryptData_Js(autloLoginCustomer, fileName);

            EncryptCustomerData2Layer();
        }
        private static bool DecryptDataLayer2()
        {
            bool result = false;
            if (fileManager.Check(fileName))
            {
                try
                {
                    string encryptString = fileManager.ReadEncryptStringAtLayer2_Js(fileName);
                    if (encryptString != null)
                    {
                        RemoveCookie();
                        fileManager.WriteEncryptStringAtLayer2(EncryptTextTool.DecryptText(encryptString), fileName);
                        result = true;
                    }
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }
        public static Customer GetSomeCustomerData()
        {
            Customer customer = new Customer();
            if (DecryptDataLayer2())
            {
                if (fileManager.Check(fileName))
                {
                    try
                    {
                        customer = fileManager.ReadEncryptData_Js(fileName);
                        customer.user_name = EncryptTextTool.DecryptText(customer.user_name);
                        customer.password = EncryptTextTool.DecryptText(customer.password);

                        EncryptCustomerData2Layer();
                    }
                    catch
                    {
                        customer = null;
                    }      
                }
                else
                {
                    customer = null;
                }
            }
            else
            {
                customer = null;
                RemoveCookie();
            }
            return customer;
        }
        public static void RemoveCookie()
        {
            if (fileManager.Check(fileName))
            {
                try
                {
                    fileManager.ClearAllData(fileName);
                }
                catch
                {
                    //Console.WriteLine("There is no cookie to remove!");
                }
            }
            else
            {
                //Console.WriteLine("There is no cookie to remove!!");
            }
        }

    }
}