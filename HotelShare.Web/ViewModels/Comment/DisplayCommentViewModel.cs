using System;
using System.Collections.Generic;
using HotelShare.Domain.Models.SqlModels.CommentModels;

namespace HotelShare.Web.ViewModels.Comment
{
    public class DisplayCommentViewModel
    {
        public Guid HotelId { get; set; }

        public List<DisplayCommentModel> Comments { get; set; }
    }
}