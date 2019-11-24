using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HotelShare.Interfaces.DAL.RepositorySql
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        int Count(Expression<Func<TEntity, bool>> filter = null);

        IEnumerable<TEntity> GetMany(
            int skip = 0,
            int take = Int32.MaxValue,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        TEntity FirstOrDefault(
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includes);

        bool Any(Expression<Func<TEntity, bool>> filter);
    }
}