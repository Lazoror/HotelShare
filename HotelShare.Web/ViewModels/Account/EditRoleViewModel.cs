using System;

namespace HotelShare.Web.ViewModels.Account
{
    public class EditRoleViewModel
    {
        public Guid RoleId { get; set; }

        public string Name { get; set; }

        public string NewRoleName { get; set; }
    }
}