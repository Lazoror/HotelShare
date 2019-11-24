using System;
using System.Collections.Generic;
using HotelShare.Domain.Models.SqlModels;

namespace HotelShare.Interfaces.Services
{
    public interface IRoomService
    {
        IEnumerable<Room> GetAllRooms();

        IEnumerable<string> GetAllRoomCompanyNames();

        Room Get(Guid roomId);

        void Delete(string companyName);

        void EditRoom(Room room);

        void CreateRoom(Room entity);
    }
}