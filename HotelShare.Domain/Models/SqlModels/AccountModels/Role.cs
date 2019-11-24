using System.Collections.Generic;

namespace HotelShare.Domain.Models.SqlModels.AccountModels
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<UserRole> Roles { get; set; }
    }
}