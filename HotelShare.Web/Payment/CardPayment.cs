using HotelShare.Domain.Models.SqlModels.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HotelShare.Web.Payment
{
    public class CardPayment : IPayment
    {
        public IActionResult Process(ProcessPaymentModel orderInfo)
        {
            var emptyModelMetaDataProvider = new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider();
            var modelStateDictionary = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();

            return new ViewResult
            {
                ViewData = new ViewDataDictionary(emptyModelMetaDataProvider, modelStateDictionary)
                {
                    Model = "Success paid"
                },
                ViewName = "CardSuccess"
            };
        }
    }
}