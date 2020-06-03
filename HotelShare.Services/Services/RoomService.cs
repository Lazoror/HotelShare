using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using HotelShare.Domain.Models.SqlModels;
using HotelShare.Interfaces.DAL.Data;
using HotelShare.Interfaces.DAL.RepositorySql;
using HotelShare.Interfaces.Services;

namespace HotelShare.Services.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Room> _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        private readonly string _language;

        public RoomService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHotelRepository hotelRepository)
        {
            _language = CultureInfo.CurrentCulture.Name;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _roomRepository = _unitOfWork.GetRepository<Room>();
        }

        public void CreateRoom(Room entity)
        {
            entity.Id = Guid.NewGuid();
            entity.IsAvailable = true;

            _roomRepository.Insert(entity);
            _unitOfWork.Commit();
        }

        public void Delete(string roomName)
        {
            var room = _roomRepository.FirstOrDefault(a => a.RoomName == roomName);

            if (room != null)
            {
                room.IsDeleted = true;
                _roomRepository.Update(room);
                _unitOfWork.Commit();
            }
        }

        public void EditRoom(Room room)
        {
            var roomEntity = _roomRepository.FirstOrDefault(p => p.Id == room.Id);

            roomEntity.Description = room.Description;

            _roomRepository.Update(roomEntity);
            _unitOfWork.Commit();
        }

        public IEnumerable<Room> GetAllRooms()
        {
            var rooms = _roomRepository.GetMany();
            var translatedRooms = new List<Room>();

            return rooms;
        }

        public IEnumerable<Room> GetHotelRooms(Guid hotelId)
        {
            var rooms = _roomRepository.GetMany(filter: x => x.HotelId == hotelId).ToList();

            return rooms;
        }

        public IEnumerable<string> GetAllRoomCompanyNames()
        {
            return _roomRepository.GetMany().Select(p => p.RoomName);
        }

        public Room Get(Guid roomId)
        {
            var room = _roomRepository.FirstOrDefault(a => a.Id == roomId);

            return room;
        }
    }
}