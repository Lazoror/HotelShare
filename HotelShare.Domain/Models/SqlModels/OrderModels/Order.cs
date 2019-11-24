using HotelShare.Domain.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HotelShare.Domain.Models.SqlModels.OrderModels
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string Shipper { get; set; }

        [JsonIgnore]
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}