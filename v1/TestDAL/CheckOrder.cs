using Xunit;
using DAL;
using System.Collections.Generic;
using System;
using Persistence;
public class OrderTest
{
    OrderDAL oDAL = new OrderDAL();
    [Theory]
    [InlineData(1, "10/27/2021", 89000, true)]
    public void PassingCreateOrder(int userId, string orderDate, decimal total, bool status)
    {
        Order order = new Order();
        CustomerDAL cDAL = new CustomerDAL();
        order.orderCustomer = cDAL.GetCustomerbyID(userId);
        order.orderDate = Convert.ToDateTime(orderDate);
        order.total = total;
        order.orderStatus = status;
        Assert.True(oDAL.CreateOrder(order));
    }
    [Theory]
    [InlineData(2)]
    [InlineData(3)] //Have no order
    public void PassingGetOrder(int userId)
    {
        Assert.True(oDAL.GetOrderDetail(userId) != null);
    }



}