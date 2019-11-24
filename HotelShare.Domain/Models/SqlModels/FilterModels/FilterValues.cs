using System.Collections.Generic;

namespace HotelShare.Domain.Models.SqlModels.FilterModels
{
    public class FilterValues
    {
        public List<string> Genres { get; set; }

        public List<string> Rooms { get; set; }

        public List<string> Platforms { get; set; }
    }
}