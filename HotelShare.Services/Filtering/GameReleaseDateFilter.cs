using System;
using System.Linq.Expressions;
using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.HotelModels;

namespace HotelShare.Services.Filtering
{
    public class GameReleaseDateFilter : IFilter<Expression<Func<Hotel, bool>>>
    {
        private readonly ReleaseDate _releaseDate;

        public GameReleaseDateFilter(ReleaseDate releaseDate)
        {
            _releaseDate = releaseDate;
        }

        public Expression<Func<Hotel, bool>> Execute(Expression<Func<Hotel, bool>> expression)
        {
            Expression<Func<Hotel, bool>> newExp = _ => true; 

            //switch (_releaseDate)
            //{
            //    case ReleaseDate.LastWeek:
            //        var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
            //        newExp = g => g.PublishDate >= sevenDaysAgo;
            //        break;

            //    case ReleaseDate.LastMonth:
            //        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
            //        newExp = g => g.PublishDate >= oneMonthAgo;
            //        break;

            //    case ReleaseDate.LastYear:
            //        var oneYearAgo = DateTime.UtcNow.AddYears(-1);
            //        newExp = g => g.PublishDate >= oneYearAgo;
            //        break;

            //    case ReleaseDate.TwoYears:
            //        var twoYearsAgo = DateTime.UtcNow.AddYears(-2);
            //        newExp = g => g.PublishDate >= twoYearsAgo;
            //        break;

            //    case ReleaseDate.ThreeYears:
            //        var threeYearsAgo = DateTime.UtcNow.AddYears(-3);
            //        newExp = g => g.PublishDate >= threeYearsAgo;
            //        break;
            //}

            return expression.And(newExp);
        }
    }
}