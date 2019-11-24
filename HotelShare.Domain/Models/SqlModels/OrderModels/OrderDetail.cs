using Newtonsoft.Json;
using System;

namespace HotelShare.Domain.Models.SqlModels.OrderModels
{
    public class OrderDetail : BaseEntity
    {
        public Guid RoomId { get; set; }

        public decimal Price { get; set; }

        public Int16 Quantity { get; set; }

        public Int16 Discount { get; set; }

        public Guid OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }
    }
}