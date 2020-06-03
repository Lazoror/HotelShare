using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelShare.Domain.Models.SqlModels.AccountModels
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        [MaxLength(32), MinLength(6)]
        public string Password { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<UserRole> Roles { get; set; }
    }
}