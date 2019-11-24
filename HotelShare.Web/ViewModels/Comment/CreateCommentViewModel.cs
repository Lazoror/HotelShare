using System;
using System.ComponentModel.DataAnnotations;

namespace HotelShare.Web.ViewModels.Comment
{
    public class CreateCommentViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Body { get; set; }

        public string Quote { get; set; }

        public Guid HotelId { get; set; }

        public Guid ParentCommentId { get; set; }
    }
}