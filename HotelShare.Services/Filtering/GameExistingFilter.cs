using HotelShare.Domain.Models.SqlModels.GameModels;
using System;
using System.Linq.Expressions;

namespace HotelShare.Services.Filtering
{
    public class GameExistingFilter : IFilter<Expression<Func<Hotel, bool>>>
    {
        public Expression<Func<Hotel, bool>> Execute(Expression<Func<Hotel, bool>> expression)
        {
            Expression<Func<Hotel, bool>> newExp = g => g.IsDeleted == false;

            return expression.And(newExp);
        }
    }
}