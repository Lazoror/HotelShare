using HotelShare.Domain.Models.SqlModels.AccountModels;
using System;
using System.Collections.Generic;

namespace HotelShare.Interfaces.Services
{
    public interface IUserService
    {
        void Register(string email, string password);

        void ChangeRole(string email, IEnumerable<string> roles);

        void CreateRole(string role);

        void DeleteRole(string role);

        void DeleteUser(string email);

        void RestoreUser(string email);

        void EditRole(Guid roleId, string role);

        User GetUserByEmail(string email);

        User GetUserById(Guid userId);

        Role GetRoleByName(string role);

        IEnumerable<User> GetAllUsers();

        IEnumerable<Role> GetAllRoles();

        IEnumerable<string> GetRoleNames();
    }
}