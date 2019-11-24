using System;

namespace HotelShare.Domain.Models.SqlModels.OrderModels
{
    public class GuestBasketModel
    {
        public Guid UserId { get; set; }

        public  Guid OrderId { get; set; }
    }
}