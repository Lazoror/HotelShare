﻿using System;

namespace HotelShare.Domain.Models.SqlModels.GameModels
{
    public class HotelImage
    {
        public Guid HotelId { get; set; }

        public Hotel Hotel { get; set; }

        public Guid ImageId { get; set; }

        public Image Image { get; set; }
    }
}