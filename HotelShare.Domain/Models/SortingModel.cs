using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.HotelModels;
using System;
using System.Linq.Expressions;

namespace HotelShare.Domain.Models
{
    public class SortingModel
    {
        public SortDirection SortDirection { get; set; }

        public Expression<Func<Hotel, object>> OrderExpression { get; set; }
    }
}