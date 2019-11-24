using System.Collections.Generic;
using HotelShare.Domain.Enums;

namespace HotelShare.Domain.Models.SqlModels.FilterModels
{
    public class FilterDataModel
    {
        public SortType SortType { get; set; }

        public decimal PriceFrom { get; set; }

        public decimal PriceTo{ get; set; }

        public string SearchString { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int ItemsPerPage { get; set; } = 10;

        public List<string> Rooms { get; set; }

        public ReleaseDate ReleaseDate { get; set; }

        public List<string> Genres { get; set; }

        public List<string> Platforms { get; set; }
    }
}