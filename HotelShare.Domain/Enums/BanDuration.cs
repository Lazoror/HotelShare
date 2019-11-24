using System.ComponentModel.DataAnnotations;

namespace HotelShare.Domain.Enums
{
    public enum BanDuration
    {
        [Display(Name = "One Hour")]
        OneHour,

        [Display(Name = "One Day")]
        OneDay,

        [Display(Name = "One Week")]
        OneWeek,

        [Display(Name = "One Month")]
        OneMonth,

        [Display(Name = "Permanent")]
        Permanent
    }
}