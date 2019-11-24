using HotelShare.DAL.Data;
using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.GameModels;
using HotelShare.Interfaces.DAL.RepositorySql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HotelShare.DAL.Repository.SqlRepositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelContext _context;
        private readonly DbSet<Hotel> _set;

        public HotelRepository(HotelContext context)
        {
            _context = context;
            _set = context.Set<Hotel>();
        }

        public void Insert(Hotel entity)
        {
            _context.Entry(entity).State = EntityState.Added;
        }

        public void Update(Hotel entity)
        {
            _context.Update(entity).State = EntityState.Modified;
        }

        public void Delete(Hotel entity)
        {
            _set.Remove(entity);
        }

        public int Count(Expression<Func<Hotel, bool>> filter = null)
        {
            var count = _set.Count();

            if (filter != null)
            {
                count = _set.Count(filter);
            }

            return count;
        }

        public IEnumerable<Hotel> GetMany(int skip = 0,
            int take = Int32.MaxValue,
            Expression<Func<Hotel, bool>> filter = null,
            Expression<Func<Hotel, object>> orderBy = null,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            var set = _set.AsQueryable();

            set = set.Include(g => g.Rooms).Include(g => g.Comments).Include(g => g.GameImages);

            if (filter != null)
            {
                set = set.Where(filter);
            }

            if (orderBy != null)
            {
                switch (sortDirection)
                {
                    case SortDirection.Ascending:
                        set = set.OrderBy(orderBy);
                        break;
                    case SortDirection.Descending:
                        set = set.OrderByDescending(orderBy);
                        break;
                }
            }

            return set.Skip(skip).Take(take).ToList();
        }

        public Hotel FirstOrDefault(Expression<Func<Hotel, bool>> filter)
        {
            var set = _set.AsQueryable();

            set = set.Include(g => g.Rooms).Include(g => g.Comments).Include(g => g.GameImages);

            return set.FirstOrDefault(filter);
        }

        public bool Any(Expression<Func<Hotel, bool>> filter)
        {
            return _set.Any(filter);
        }
    }
}