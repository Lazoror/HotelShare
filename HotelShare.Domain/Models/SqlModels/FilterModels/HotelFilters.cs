using System.Collections.Generic;
using HotelShare.Domain.Models.SqlModels.HotelModels;

namespace HotelShare.Domain.Models.SqlModels.FilterModels
{
    public class HotelFilters
    {
        public IEnumerable<Hotel> Hotels { get; set; }

        public FilterDataModel Filters { get; set; }

        public FilterValues DefaultValues { get; set; }
    }
}