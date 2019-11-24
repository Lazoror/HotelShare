using System;
using System.ComponentModel.DataAnnotations;

namespace HotelShare.Web.ViewModels.Comment
{
    public class CommentViewModel
    {
        public Guid CommentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Body { get; set; }
        
        public Guid HotelId { get; set; }
    }
}