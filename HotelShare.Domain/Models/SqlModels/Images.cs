using HotelShare.Domain.Models.SqlModels.GameModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HotelShare.Domain.Models.SqlModels
{
    public class Image : BaseEntity
    {
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<HotelImage> GameGenres { get; set; }
    }
}