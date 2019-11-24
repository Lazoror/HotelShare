using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HotelShare.Domain.Models.SqlModels.GameModels;

namespace HotelShare.Services.Filtering
{
    public class GameRoomFilter : IFilter<Expression<Func<Hotel, bool>>>
    {
        private readonly List<string> _rooms;

        public GameRoomFilter(List<string> rooms)
        {
            _rooms = rooms;
        }

        public Expression<Func<Hotel, bool>> Execute(Expression<Func<Hotel, bool>> expression)
        {
            //Expression<Func<Hotel, bool>> newExp = g => _rooms.Contains(g.Room.CompanyName);

            return expression.And(_ => true);
        }
    }
}