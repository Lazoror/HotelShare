using HotelShare.Domain.Models.SqlModels.HotelModels;
using System;
using System.Linq.Expressions;

namespace HotelShare.Services.Filtering
{
    public class GamePipeline : Pipeline<Expression<Func<Hotel, bool>>>
    {
        public override Expression<Func<Hotel, bool>> Process(Expression<Func<Hotel, bool>> expression)
        {
            foreach (var filter in Filters)
            {
                expression = filter.Execute(expression);
            }

            return expression;
        }
    }
}