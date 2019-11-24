using HotelShare.DAL.Data;
using HotelShare.Interfaces.DAL.RepositorySql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HotelShare.DAL.Repository.SqlRepositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly HotelContext _context;
        private readonly DbSet<TEntity> _set;

        public Repository(HotelContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            var count = _set.Count();

            if (filter != null)
            {
                count = _set.Count(filter);
            }

            return count;
        }

        public IEnumerable<TEntity> GetMany(
            int skip = 0,
            int take = Int32.MaxValue,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var set = _set.AsQueryable();

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                set = set.Include(include);
            }

            if (filter != null)
            {
                set = set.Where(filter);
            }

            if (orderBy != null)
            {
                set = set.OrderBy(orderBy);
            }

            return set.Skip(skip).Take(take).ToList();
        }

        public TEntity FirstOrDefault(
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var set = _set.AsQueryable();

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                set = set.Include(include);
            }

            return set.FirstOrDefault(filter);
        }

        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return _set.Any(filter);
        }

        public void Insert(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            _set.Remove(entity);
        }
    }
}