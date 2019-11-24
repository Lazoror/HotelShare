using HotelShare.Domain.Enums;
using HotelShare.Domain.Models;

namespace HotelShare.Services.Filtering
{
    public class GameSortingBuilder
    {
        private SortingModel _sortingModel;

        public GameSortingBuilder()
        {
            _sortingModel = new SortingModel();
        }

        public GameSortingBuilder ResolveSorting(SortType sortType)
        {
            switch (sortType)
            {
                //case SortType.PriceDesc:
                //    _sortingModel = new SortingModel { SortDirection = SortDirection.Descending, OrderExpression = g => g.Price };
                //    break;
                //case SortType.PriceAsc:
                //    _sortingModel = new SortingModel { SortDirection = SortDirection.Ascending, OrderExpression = g => g.Price };
                //    break;
                case SortType.MostCommented:
                    _sortingModel = new SortingModel { SortDirection = SortDirection.Descending, OrderExpression = g => g.Comments.Count };
                    break;
                case SortType.MostPopular:
                    _sortingModel = new SortingModel { SortDirection = SortDirection.Descending, OrderExpression = g => g.ViewCount };
                    break;
                case SortType.New:
                    _sortingModel = new SortingModel { SortDirection = SortDirection.Descending, OrderExpression = g => g.AddDate };
                    break;
            }

            return this;
        }

        public SortingModel Build()
        {
            var result = _sortingModel;
            _sortingModel = new SortingModel();

            return result;
        }
    }
}