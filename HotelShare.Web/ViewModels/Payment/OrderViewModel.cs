using System.Collections.Generic;
using HotelShare.Domain.Models.SqlModels.OrderModels;

namespace HotelShare.Web.ViewModels.Payment
{
    public class OrderViewModel
    {
        public Order Order { get; set; }

        public List<string> Shippers { get; set; }
    }
}