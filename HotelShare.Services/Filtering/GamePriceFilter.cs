using System;
using System.Linq.Expressions;
using HotelShare.Domain.Models.SqlModels.GameModels;

namespace HotelShare.Services.Filtering
{
    public class GamePriceFilter : IFilter<Expression<Func<Hotel, bool>>>
    {
        private readonly decimal _priceFrom;
        private readonly decimal _priceTo;

        public GamePriceFilter(decimal priceFrom, decimal priceTo)
        {
            _priceFrom = priceFrom;
            _priceTo = priceTo;
        }

        public Expression<Func<Hotel, bool>> Execute(Expression<Func<Hotel, bool>> expression)
        {
            //Expression<Func<Hotel, bool>> newExp = g => g.Price >= _priceFrom && g.Price <= _priceTo;

            return expression.And(_ => true);
        }
    }
}