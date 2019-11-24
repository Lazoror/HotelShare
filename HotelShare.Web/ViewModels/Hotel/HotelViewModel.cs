using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HotelShare.Web.ViewModels.Hotel
{
    public class HotelViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public short Room { get; set; }

        public bool Discontinued { get; set; }

        public decimal Rating { get; set; }

        public bool IsDeleted { get; set; }

        public List<Domain.Models.SqlModels.Room> Rooms { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}