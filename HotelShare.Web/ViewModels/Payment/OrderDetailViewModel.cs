using System;
using System.Collections.Generic;

namespace HotelShare.Web.ViewModels.Payment
{
    public class OrderDetailViewModel
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public string RoomName { get; set; }

        public decimal Price { get; set; }

        public Int16 Quantity { get; set; }

        public Int16 Discount { get; set; }
    }
}