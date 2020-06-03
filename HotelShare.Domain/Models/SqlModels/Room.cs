
using System;
using HotelShare.Domain.Models.SqlModels.HotelModels;

namespace HotelShare.Domain.Models.SqlModels
{
    public class Room : BaseEntity
    {

        public string RoomName { get; set; }

        public string Description { get; set; }

        public string Facilities { get; set; }

        public bool IsDeleted { get; set; }


        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public Guid HotelId { get; set; }

        public Hotel Hotel { get; set; }
    }
}