using System.Threading.Tasks;
using HotelShare.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelShare.Infrastructure.Components
{
    public class GameCount : ViewComponent
    {
        private readonly IHotelService _hotelService;

        public GameCount(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hotelCount = _hotelService.CountAllHotels();

            return await Task.FromResult((IViewComponentResult)View("TotalGames", hotelCount));
        }
    }
}