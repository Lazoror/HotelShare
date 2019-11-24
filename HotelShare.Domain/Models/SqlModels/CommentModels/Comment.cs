using System;
using HotelShare.Domain.Models.SqlModels.GameModels;
using Newtonsoft.Json;

namespace HotelShare.Domain.Models.SqlModels.CommentModels
{
    public class Comment : BaseEntity
    {
        public string Name { get; set; }

        public string Body { get; set; }

        public bool IsDeleted { get; set; }

        public string Quote { get; set; }

        public Guid? ParentCommentId { get; set; }
        
        public Guid HotelId { get; set; }

        [JsonIgnore]
        public Comment ParentComment { get; set; }

        [JsonIgnore]
        public Hotel Hotel { get; set; }
    }
}