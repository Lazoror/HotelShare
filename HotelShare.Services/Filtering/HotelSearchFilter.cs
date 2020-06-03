using System;
using System.Linq.Expressions;
using HotelShare.Domain.Models.SqlModels.HotelModels;

namespace HotelShare.Services.Filtering
{
    public class HotelSearchFilter : IFilter<Expression<Func<Hotel, bool>>>
    {
        private readonly string _searchString;

        public HotelSearchFilter(string searchString)
        {
            _searchString = searchString;
        }

        public Expression<Func<Hotel, bool>> Execute(Expression<Func<Hotel, bool>> expression)
        {
            Expression<Func<Hotel, bool>> searchExpression = g => g.Name.Contains(_searchString);

            return expression.And(searchExpression);
        }
    }
}