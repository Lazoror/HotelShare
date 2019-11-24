using System;
using System.ComponentModel.DataAnnotations;

namespace HotelShare.Web.ViewModels.Room
{
    public class RoomViewModel
    {
        public Guid RoomId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Description { get; set; }

        public string HomePage { get; set; }


        public string OldCompanyName { get; set; }
    }
}