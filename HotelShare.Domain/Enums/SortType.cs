using System.ComponentModel.DataAnnotations;

namespace HotelShare.Domain.Enums
{
    public enum SortType
    {
        [Display(Name = "None")]
        None,

        [Display(Name = "Most Popular")]
        MostPopular,

        [Display(Name = "Most Commented")]
        MostCommented,

        [Display(Name = "Price Asc")]
        PriceAsc,

        [Display(Name = "Price Desc")]
        PriceDesc,

        [Display(Name = "New")]
        New
    }
}