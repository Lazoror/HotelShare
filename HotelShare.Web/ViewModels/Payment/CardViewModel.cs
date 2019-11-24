using System.ComponentModel.DataAnnotations;

namespace HotelShare.Web.ViewModels.Payment
{
    public class CardViewModel
    {
        [Required]
        public string CartHolderName { get; set; }

        [Required]
        public long CardNumber { get; set; }

        [Required]
        public short MonthExpiry { get; set; }

        [Required]
        public short YearExpiry { get; set; }

        [Required]
        public short SecurityCode { get; set; }
    }
}