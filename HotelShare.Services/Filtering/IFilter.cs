using System;
using System.Linq.Expressions;
using HotelShare.Domain.Models.SqlModels.GameModels;

namespace HotelShare.Services.Filtering
{
    public interface IFilter<T>
    {
        T Execute(Expression<Func<Hotel, bool>> expression);
    }
}