using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.GameModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HotelShare.Interfaces.DAL.RepositorySql
{
    public interface IHotelRepository
    {
        void Insert(Hotel entity);

        void Update(Hotel entity);

        void Delete(Hotel entity);

        int Count(Expression<Func<Hotel, bool>> filter = null);

        IEnumerable<Hotel> GetMany(
            int skip = 0,
            int take = Int32.MaxValue,
            Expression<Func<Hotel, bool>> filter = null,
            Expression<Func<Hotel, object>> orderBy = null,
            SortDirection sortingDirection = SortDirection.Ascending);

        Hotel FirstOrDefault(Expression<Func<Hotel, bool>> filter);

        bool Any(Expression<Func<Hotel, bool>> filter);
    }
}