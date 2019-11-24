using HotelShare.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace HotelShare.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class StoreAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly AuthorizePermission _permission;
        private string _roles;

        public StoreAuthorizeAttribute(AuthorizePermission permission, string roles)
        {
            _permission = permission;
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var roleList = _roles.Split(", ").ToList();
            var IsUserInRole = false;

            // Checking if user in role and Authorize permission
            foreach (var role in roleList)
            {
                if (user.Identity.IsAuthenticated)
                {
                    if (user.IsInRole(role) && _permission == AuthorizePermission.Allow)
                    {
                        IsUserInRole = true;
                    }

                    if (!user.IsInRole(role) && _permission == AuthorizePermission.Disallow)
                    {
                        IsUserInRole = true;
                    }
                }
            }

            // Check if User is not authenticated and permission
            if (roleList.Contains("Guest") && _permission == AuthorizePermission.Allow && !user.Identity.IsAuthenticated)
            {
                IsUserInRole = true;
            }
            else if (roleList.Contains("Guest") && _permission == AuthorizePermission.Disallow && !user.Identity.IsAuthenticated)
            {
                IsUserInRole = false;
            }

            // Allow or disallow access source in order to below algorithm
            if (IsUserInRole)
            {
                return;
            }
            else
            {
                var returnUrl = context.HttpContext.Request.Headers["Referer"].ToString();

                var viewResult = new RedirectToActionResult("AccessDenied", "Account", new { returnUrl = returnUrl });

                context.Result = viewResult;
            }
        }
    }
}