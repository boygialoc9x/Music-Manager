using System;
using System.Collections.Generic;

namespace Persistence
{
    public class Order
    {
        public int orderId { set; get; }
        public DateTime orderDate { set; get; }
        public Customer orderCustomer { set; get; }
        public decimal total {get;set;}
        public bool orderStatus {get;set;}
    }
}