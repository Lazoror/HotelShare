using HotelShare.Domain.Models.SqlModels.FilterModels;
using HotelShare.Domain.Models.SqlModels.HotelModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HotelShare.Domain.Enums;

namespace HotelShare.Interfaces.Services
{
    public interface IHotelService
    {
        Hotel Get(Guid hotelId);

        HotelFilters ProcessFiltering(FilterDataModel filters);

        IEnumerable<Hotel> GetAllHotels(SortType sortType, string lang, Expression<Func<Hotel, bool>> filter = null, int skip = 0, int take = Int32.MaxValue);

        int CountAllHotels();

        void Delete(Guid hotelId);

        void Create(Hotel entity);

        void Edit(Hotel entity);

        void AddImage(Guid hotelId, List<string> imageNames);
    }
}