using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.FilterModels;
using HotelShare.Domain.Models.SqlModels.HotelModels;
using System;
using System.Linq.Expressions;

namespace HotelShare.Services.Filtering
{
    public class GamePipelineBuilder
    {
        private Pipeline<Expression<Func<Hotel, bool>>> _hotelPipeline;
        private readonly FilterDataModel _filtersGameData;

        public GamePipelineBuilder(FilterDataModel hotelFilters)
        {
            _filtersGameData = hotelFilters;
            _hotelPipeline = new GamePipeline();
        }

        public GamePipelineBuilder WithSearchFilter()
        {
            if (!String.IsNullOrWhiteSpace(_filtersGameData.SearchString) && _filtersGameData.SearchString.Length >= 3)
            {
                _hotelPipeline.Filters.Add(new HotelSearchFilter(_filtersGameData.SearchString));
            }

            return this;
        }

        public GamePipelineBuilder WithGamePriceFilter()
        {
            if (_filtersGameData.PriceFrom >= 0 && _filtersGameData.PriceFrom <= _filtersGameData.PriceTo && _filtersGameData.PriceFrom + _filtersGameData.PriceTo != 0)
            {
                _hotelPipeline.Filters.Add(new GamePriceFilter(_filtersGameData.PriceFrom, _filtersGameData.PriceTo));
            }

            return this;
        }

        public GamePipelineBuilder WithGameReleaseDateFilter()
        {
            if (_filtersGameData.ReleaseDate != ReleaseDate.None)
            {
                _hotelPipeline.Filters.Add(new GameReleaseDateFilter(_filtersGameData.ReleaseDate));
            }

            return this;
        }

        public GamePipelineBuilder WithGameRoomFilter()
        {
            if (_filtersGameData.Rooms != null)
            {
                _hotelPipeline.Filters.Add(new GameRoomFilter(_filtersGameData.Rooms));
            }

            return this;
        }
        public Expression<Func<Hotel, bool>> Build()
        {
            var result = _hotelPipeline.Process(g => g.Name != "");
            _hotelPipeline = new GamePipeline();

            return result;
        }
    }
}