using System;
using System.Collections.Generic;
using Persistence;
using DAL;
namespace BL
{
    public class OrderBL
    {
        private OrderDAL oDAL = new OrderDAL();
        public bool CreateOrder(Order order)
        {
            bool result = oDAL.CreateOrder(order);
            return result;
        }
        public int GetMaxIdInOrders()
        {
            return oDAL.GetMaxIdInOrder();
        }
        public Order GetOrderDetail(int userId)
        {
            return oDAL.GetOrderDetail(userId);
        }
    }
}