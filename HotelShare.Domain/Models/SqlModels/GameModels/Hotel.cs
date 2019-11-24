using System;
using System.Collections.Generic;
using HotelShare.Domain.Models.SqlModels.CommentModels;

namespace HotelShare.Domain.Models.SqlModels.GameModels
{
    public class Hotel : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public short AvailableRooms { get; set; }

        public bool IsDeleted { get; set; }

        public bool Discontinued { get; set; }

        public decimal Rating { get; set; }

        public int RatingQuantity { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public DateTime AddDate { get; set; }

        public int ViewCount { get; set; }

        public string Location { get; set; }

        public ICollection<Room> Rooms { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<HotelImage> GameImages { get; set; }
    }
}