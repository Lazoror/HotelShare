using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HotelShare.Domain.Models.SqlModels.HotelModels;

namespace HotelShare.Services.Filtering
{
    public abstract class Pipeline<T>
    {
        internal readonly List<IFilter<T>> Filters = new List<IFilter<T>>();

        public abstract T Process(Expression<Func<Hotel, bool>> expression);
    }
}