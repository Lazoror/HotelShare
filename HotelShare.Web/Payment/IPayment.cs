using HotelShare.Domain.Models.SqlModels.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelShare.Web.Payment
{
    public interface IPayment
    {
        IActionResult Process(ProcessPaymentModel orderInfo);
    }
}