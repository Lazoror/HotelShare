using HotelShare.Domain.Models.SqlModels.GameModels;
using System;
using System.Linq.Expressions;

namespace HotelShare.Services.Filtering
{
    public class GameSearchFilter : IFilter<Expression<Func<Hotel, bool>>>
    {
        private readonly string _searchString;

        public GameSearchFilter(string searchString)
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