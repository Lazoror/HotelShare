using HotelShare.Domain.Models.SqlModels.OrderModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace HotelShare.Interfaces.Services
{
    public interface IOrderService
    {
        void CreateOrder(OrderDetail orderDetails, string email);

        Order GetAllCartOrder(string email);

        Order GetOrderById(Guid orderId);

        OrderDetail GetOrderDetail(Guid orderDetailId);

        IEnumerable<Order> GetHistoryOrder(DateTime fromDate, DateTime toDate);

        void DeleteOrderDetail(Guid hotelId);

        void DeleteOrder(Guid userId);

        void EditOrderDetail(OrderDetail entity);

        void EditOrder(Order order);

        MemoryStream GenerateInvoiceFile(ProcessPaymentModel orderInfo);
    }
}