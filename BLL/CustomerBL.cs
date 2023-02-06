using System;
using System.Collections.Generic;
using Persistence;
using DAL;

namespace BL
{
    public class CustomerBL
    {
        private CustomerDAL cusDAL = new CustomerDAL();
        //private DatabaseConnection dtb = new DatabaseConnection();
        public Customer GetCustomer(string user_name)
        {
            return cusDAL.GetCustomer(user_name);
        }
        public Customer GetCustomerbyID(int userId)
        {
            return cusDAL.GetCustomerbyID(userId);
        }
        public bool CheckUserName(string user_name)
        {
            return cusDAL.CheckUserName(user_name);
        }

        public bool CheckPassword(string user_name, string password)
        {
            return cusDAL.CheckPassword(user_name, password);
        }
        // public void connect_Mysql_String(String connect)
        // {
        //     dtb.Write(connect);
        // }
        public bool CheckStaff(string user_name)
        {
            string staffPerm = "staff";
            return cusDAL.CheckPermission(user_name, staffPerm);
        }
        public bool CheckPremium(string user_name)
        {
            string premiumPerm = "premium";
            return cusDAL.CheckPermission(user_name, premiumPerm);
        }
        public bool UpgradePremium_Staff(string user_name)
        {
            bool status = true;
            return cusDAL.UpgradePremium_Staff(user_name, status);
        }
        public bool RemovePremium_Staff(string user_name)
        {
            bool status = false;
            return cusDAL.UpgradePremium_Staff(user_name, status);
        }
        public bool UpdateNewPassword(int userId, string newPass)
        {
            return cusDAL.UpdateNewPassword(userId, newPass);
        }
        
        public bool UpdateCustomerInformation(Customer customer)
        {
            return cusDAL.UpdateCustomerInformation(customer);
        }
        public bool CheckGmail(string gmail)
        {
            return cusDAL.CheckGmail(gmail);
        }
        public bool CheckGmailByUserName(string gmail, string user_name)
        {
            return cusDAL.CheckGmailByUserName(gmail, user_name);
        }
        public int GetMaxIdInCustomer()
        {
            return cusDAL.GetMaxIdInCustomer();
        }
        public bool RegisterAccount(Customer customer)
        {
            return cusDAL.RegisterAccount(customer);
        }
    }
}