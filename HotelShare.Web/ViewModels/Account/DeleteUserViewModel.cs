using System;

namespace HotelShare.Web.ViewModels.Account
{
    public class DeleteUserViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsDeleted { get; set; }
    }
}