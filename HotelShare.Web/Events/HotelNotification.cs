using HotelShare.Domain.Models.SqlModels.HotelModels;

namespace HotelShare.Web.Events
{
    public class HotelNotification
    {
        private Hotel _hotel;

        private delegate void AccountHandler(string message);

        private event AccountHandler Notify;

        public HotelNotification(Hotel hotel)
        {
            _hotel = hotel;
        }

        public void NotifyUsers()
        {
            if (_hotel.AvailableRooms > 0)
            {
                Notify?.Invoke($"Hotel {_hotel.Name} is available now");
            }

        }
    }
}