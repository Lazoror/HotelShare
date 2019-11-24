using System.Collections.Generic;

namespace HotelShare.Web.ViewModels.Account
{
    public class ChangeRoleUserViewModel
    {
        public List<string> Roles { get; set; }

        public string UserEmail { get; set; }
    }
}