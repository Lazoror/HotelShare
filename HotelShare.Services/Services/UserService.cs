using HotelShare.Domain.Models.SqlModels.AccountModels;
using HotelShare.Interfaces.DAL.Data;
using HotelShare.Interfaces.DAL.RepositorySql;
using HotelShare.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

namespace HotelShare.Services.Services
{
    public class UserService : IUserService
    {
        public readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork, IRepository<Role> roleRepository, IRepository<User> userRepository)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public void Register(string email, string password)
        {
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Name = email,
                Email = email,
                Password = Crypto.HashPassword(password),
                Roles = new List<UserRole>()
            };

            _userRepository.Insert(user);
            _unitOfWork.Commit();

            AddUserRole(user.Email);
        }

        public void ChangeRole(string email, IEnumerable<string> roles)
        {
            var user = _userRepository.FirstOrDefault(u => u.Email == email, includes: u => u.Roles);

            if (roles == null)
            {
                return;
            }

            if (user == null)
            {
                return;
            }

            var newRoles = new List<UserRole>();

            foreach (string role in roles)
            {
                var roleEntity = _roleRepository.FirstOrDefault(r => r.Name == role);

                newRoles.Add(new UserRole { Role = roleEntity, User = user });
            }

            user.Roles = newRoles;

            _userRepository.Update(user);
            _unitOfWork.Commit();
        }

        public void CreateRole(string role)
        {
            if (!String.IsNullOrEmpty(role))
            {
                var roleEntity = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = role
                };

                _roleRepository.Insert(roleEntity);
                _unitOfWork.Commit();
            }
        }

        public void DeleteRole(string role)
        {
            var roleEntity = _roleRepository.FirstOrDefault(r => r.Name == role);

            if (roleEntity == null)
            {
                return;
            }

            _roleRepository.Delete(roleEntity);
            _unitOfWork.Commit();
        }

        public void DeleteUser(string email)
        {
            var user = _userRepository.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return;
            }

            user.IsDeleted = true;

            _userRepository.Update(user);
            _unitOfWork.Commit();
        }

        public void RestoreUser(string email)
        {
            var user = _userRepository.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return;
            }

            user.IsDeleted = false;

            _userRepository.Update(user);
            _unitOfWork.Commit();
        }

        public void EditRole(Guid roleId, string role)
        {
            var roleEntity = _roleRepository.FirstOrDefault(r => r.Id == roleId);

            if (roleEntity != null)
            {
                roleEntity.Name = role;

                _roleRepository.Update(roleEntity);
                _unitOfWork.Commit();
            }
        }

        public User GetUserByEmail(string email)
        {
            var user = _userRepository.FirstOrDefault(u => u.Email == email, u => u.Roles);

            if (user != null)
            {
                foreach (var userRole in user.Roles)
                {
                    var role = _roleRepository.FirstOrDefault(r => r.Id == userRole.RoleId);

                    userRole.Role = role;
                }
            }

            return user;
        }

        public User GetUserById(Guid userId)
        {
            var user = _userRepository.FirstOrDefault(u => u.Id == userId);

            return user;
        }

        public Role GetRoleByName(string role)
        {
            var roleEntity = _roleRepository.FirstOrDefault(r => r.Name == role);

            return roleEntity;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = _userRepository.GetMany(includes: u => u.Roles);

            foreach (var user in users)
            {
                foreach (var userRole in user.Roles)
                {
                    var role = _roleRepository.FirstOrDefault(r => r.Id == userRole.RoleId);

                    userRole.Role = role;
                }
            }

            return users;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            var roles = _roleRepository.GetMany();

            return roles;
        }

        public IEnumerable<string> GetRoleNames()
        {
            var roleNames = _roleRepository.GetMany().Select(r => r.Name);

            return roleNames;
        }

        private void AddUserRole(string email)
        {
            var user = _userRepository.FirstOrDefault(u => u.Email == email);
            var userRole = _roleRepository.FirstOrDefault(g => g.Name == "User");

            if (user != null)
            {
                user.Roles.Add(new UserRole { RoleId = userRole.Id, UserId = user.Id });

                _userRepository.Update(user);
            }

            _unitOfWork.Commit();
        }
    }
}