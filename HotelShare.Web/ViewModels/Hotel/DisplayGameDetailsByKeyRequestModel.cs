using System;
using System.Collections.Generic;
using HotelShare.Web.ViewModels.Comment;

namespace HotelShare.Web.ViewModels.Hotel
{
    public class DisplayGameDetailsByIdRequestModel
    {
        public Guid Id { get; set; }

        public HotelViewModel HotelViewModel { get; set; }

        public DisplayCommentViewModel Comments { get; set; }

        public List<byte[]> HotelImages { get; set; }
    }
}