using AutoMapper;
using HotelShare.Domain;
using HotelShare.Domain.Enums;
using HotelShare.Interfaces.Services;
using HotelShare.Web.Attributes;
using HotelShare.Web.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HotelShare.Web.Controllers
{
    [Route("manage")]
    [StoreAuthorize(AuthorizePermission.Allow, RoleName.Admin)]
    public class AdministrationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AdministrationController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("users")]
        public IActionResult ManageUsers()
        {
            var users = _userService.GetAllUsers();

            return View(users);
        }

        [HttpGet("roles")]
        public IActionResult ManageRoles()
        {
            var roles = _userService.GetAllRoles();

            return View(roles);
        }

        [HttpPost("role/change")]
        public IActionResult ChangeRole(ChangeRoleUserViewModel changeRoleModel)
        {
            if (User.Identity.Name != changeRoleModel.UserEmail)
            {
                _userService.ChangeRole(changeRoleModel.UserEmail, changeRoleModel.Roles);
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpGet("role/change")]
        public IActionResult ChangeRole(string userEmail)
        {
            var roleNames = _userService.GetRoleNames();
            var changeRoleViewModel = new ChangeRoleUserViewModel
            {
                Roles = roleNames.ToList(),
                UserEmail = userEmail
            };

            return View(changeRoleViewModel);
        }

        [HttpGet("role/create")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost("role/create")]
        public IActionResult CreateRole(string role)
        {
            if (ModelState.IsValid)
            {
                _userService.CreateRole(role);

                return RedirectToAction(nameof(ManageRoles));
            }

            return View();
        }

        [HttpGet("role/edit")]
        public IActionResult EditRole(string role)
        {
            var roleEntity = _userService.GetRoleByName(role);

            var editRoleViewModel = _mapper.Map<EditRoleViewModel>(roleEntity);

            return View(editRoleViewModel);
        }

        [HttpPost("role/edit")]
        public IActionResult EditRole(EditRoleViewModel editRoleViewModel)
        {
            if (editRoleViewModel.Name == RoleName.User)
            {
                ModelState.AddModelError("", "User is immutable");
            }

            if (ModelState.IsValid)
            {
                _userService.EditRole(editRoleViewModel.RoleId, editRoleViewModel.NewRoleName);

                return RedirectToAction(nameof(ManageRoles));
            }

            return View(editRoleViewModel);
        }

        [HttpPost("user/delete")]
        public IActionResult DeleteUser(DeleteUserViewModel model, string manage)
        {
            var user = _userService.GetUserByEmail(model.Email);
            var userViewModel = _mapper.Map<DeleteUserViewModel>(user);

            if (user == null)
            {
                ModelState.AddModelError("", "There is no user with such email");

                return View(userViewModel);
            }

            if (user.IsDeleted)
            {
                if (manage == "Delete")
                {
                    ModelState.AddModelError("UserDeleted", "User is already deleted");
                }
            }

            if (user.Email == User.Identity.Name)
            {
                ModelState.AddModelError("", "You cannot delete yourself");
            }

            if (ModelState.IsValid)
            {
                if (manage == "Restore")
                {
                    _userService.RestoreUser(user.Email);
                }
                else
                {
                    _userService.DeleteUser(user.Email);
                }

                return RedirectToAction(nameof(ManageUsers));
            }

            return View(userViewModel);
        }

        [HttpGet("user/delete")]
        public IActionResult DeleteUser(string userEmail)
        {
            var user = _userService.GetUserByEmail(userEmail);
            var userViewModel = _mapper.Map<DeleteUserViewModel>(user);

            return View(userViewModel);
        }

        [HttpGet("role/delete")]
        public IActionResult DeleteRole(string role)
        {
            if (!User.IsInRole(role) && role != RoleName.User)
            {
                _userService.DeleteRole(role);
            }

            return RedirectToAction(nameof(ManageRoles));
        }
    }
}