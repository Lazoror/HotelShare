using HotelShare.Domain.Models.SqlModels.AccountModels;
using HotelShare.Domain.Models.SqlModels.OrderModels;
using HotelShare.Interfaces.Services;
using HotelShare.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace HotelShare.Web.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public AccountController(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var userFind = _userService.GetUserByEmail(registerModel.Email);

                if (userFind == null)
                {
                    _userService.Register(registerModel.Email, registerModel.Password);

                    var user = _userService.GetUserByEmail(registerModel.Email);

                    if (Request.Cookies.ContainsKey("Order"))
                    {
                        var cachedOrderCookie = Request.Cookies["Order"];
                        var cachedOrderModified = JsonConvert.DeserializeObject<GuestBasketModel>(cachedOrderCookie);

                        var guestOrder = _orderService.GetOrderById(cachedOrderModified.OrderId);
                        guestOrder.CustomerId = user.Id;

                        _orderService.EditOrder(guestOrder);
                        Response.Cookies.Delete("Order");
                    }

                    var claims = GenerateClaims(user);
                    var principal = CreatePrincipal(claims);

                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Index", "Hotel");
                }

                ModelState.AddModelError("Email", "User with such email already exist");
            }

            return View(registerModel);
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            var loginModel = new LoginViewModel { ReturnUrl = returnUrl };

            return View(loginModel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var user = _userService.GetUserByEmail(loginModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(loginModel);
            }

            if (user.IsDeleted)
            {
                ModelState.AddModelError("", "Account is deleted");
            }

            if (ModelState.IsValid)
            {
                if (user != null && Crypto.VerifyHashedPassword(user.Password, loginModel.Password))
                {
                    if (Request.Cookies.ContainsKey("Order"))
                    {
                        var order = _orderService.GetAllCartOrder(loginModel.Email);
                        var cachedOrderCookie = Request.Cookies["Order"];
                        var cachedOrderModified = JsonConvert.DeserializeObject<GuestBasketModel>(cachedOrderCookie);

                        var guestOrder = _orderService.GetOrderById(cachedOrderModified.OrderId);

                        if (guestOrder != null && guestOrder.OrderDetails.Any())
                        {
                            foreach (var orderDetail in guestOrder.OrderDetails)
                            {
                                var orderDetailEntity =
                                    order.OrderDetails.FirstOrDefault(od => od.RoomId == orderDetail.RoomId);

                                if (orderDetailEntity != null)
                                {
                                    orderDetailEntity.Quantity += orderDetail.Quantity;
                                    orderDetailEntity.Price += orderDetail.Price;
                                }
                                else
                                {
                                    order.OrderDetails.Add(orderDetail);
                                }
                            }

                            _orderService.EditOrder(order);
                            Response.Cookies.Delete("Order");
                        }
                    }

                    var claims = GenerateClaims(user);
                    var principal = CreatePrincipal(claims);

                    await HttpContext.SignInAsync(principal);

                    if (!String.IsNullOrEmpty(loginModel.ReturnUrl))
                    {
                        return Redirect(loginModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Hotel");
                }

                ModelState.AddModelError(nameof(loginModel.Password), "Invalid login or password");
            }

            return View(loginModel);
        }

        [HttpGet("accessDenied")]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View(nameof(AccessDenied), returnUrl);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Hotel");

        }

        private ClaimsPrincipal CreatePrincipal(IEnumerable<Claim> claims)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        }

        private IEnumerable<Claim> GenerateClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Role.Name)));

            return claims;
        }
    }
}