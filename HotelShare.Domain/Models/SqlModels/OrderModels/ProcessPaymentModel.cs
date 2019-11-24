using System;

namespace HotelShare.Domain.Models.SqlModels.OrderModels
{
    public class ProcessPaymentModel
    {
        public Guid CustomerId { get; set; }

        public Guid OrderId { get; set; }

        public decimal OrderSum { get; set; }

    }
}