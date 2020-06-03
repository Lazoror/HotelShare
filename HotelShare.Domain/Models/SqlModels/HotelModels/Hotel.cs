using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotelShare.Domain.Models.SqlModels.CommentModels;
using Newtonsoft.Json;

namespace HotelShare.Domain.Models.SqlModels.HotelModels
{
    public class Hotel : BaseEntity
    {
        [Required]
        [DataType(DataType.Text)]
        [MinLength(10)]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MinLength(10)]
        [MaxLength(256)]
        public string Description { get; set; }


        public short AvailableRooms { get; set; }

        public bool IsDeleted { get; set; }

        public bool Discontinued { get; set; }

        public decimal Rating { get; set; }

        [JsonIgnore]
        public ICollection<Room> Rooms { get; set; }

        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }

        [JsonIgnore]
        public ICollection<HotelImage> HotelImages { get; set; }
    }
}