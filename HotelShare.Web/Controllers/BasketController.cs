using AutoMapper;
using HotelShare.Domain.Models.SqlModels.OrderModels;
using HotelShare.Interfaces.Services;
using HotelShare.Web.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;

namespace HotelShare.Web.Controllers
{
    [Route("basket")]
    public class BasketController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;

        public BasketController(IOrderService orderService, IMapper mapper, IHotelService hotelService, IRoomService roomService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _hotelService = hotelService;
            _roomService = roomService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var customerEmail = User.Identity.Name;
            var order = _orderService.GetAllCartOrder(customerEmail);

            if (order == null || !EnumerableExtensions.Any(order.OrderDetails))
            {
                return View("EmptyBasket");
            }
            else
            {
                var ordersView = GetBasketItems();

                return View(ordersView);

            }
        }

        private IEnumerable<OrderDetailViewModel> GetBasketItems()
        {
            var customerEmail = User.Identity.Name;
            var order = _orderService.GetAllCartOrder(customerEmail);

            var ordersView =
                _mapper.Map<IEnumerable<OrderDetailViewModel>>(order.OrderDetails);

            foreach (var orderView in ordersView)
            {
                var room = _roomService.Get(orderView.RoomId);
                orderView.RoomName = room.RoomName;
            }

            return ordersView;
        }

        [HttpPost("item/increment")]
        public IActionResult ChangeQuantityBasketItem([FromBody] OrderDetailViewModel order)
        {
            var orderDetail = _orderService.GetOrderDetail(order.Id);
            var room = _roomService.Get(orderDetail.RoomId);

            orderDetail.Quantity += order.Quantity;
            orderDetail.Price = orderDetail.Quantity * room.Price;

            _orderService.EditOrderDetail(orderDetail);

            var ordersView = GetBasketItems();

            return PartialView("_BasketItems", ordersView);
        }

        [HttpPost("item/remove")]
        public IActionResult RemoveBasketItem([FromBody] OrderDetailViewModel order)
        {
            //if (String.IsNullOrEmpty(order.HotelId))
            //{
            //    return PartialView("_NotFound");
            //}

            _orderService.DeleteOrderDetail(order.RoomId);

            var ordersView = GetBasketItems();

            return PartialView("_BasketItems", ordersView);
        }

        [HttpGet("/hotel/{hotelId}/buy")]
        public IActionResult AddToBasket(Guid roomId)
        {
            var room = _roomService.Get(roomId);

            if (!room.IsDeleted)
            {
                var orderDetail = new OrderDetail { RoomId = roomId, Discount = 0, Quantity = 1, Price = room.Price};
                var customerEmail = User.Identity.Name;

                _orderService.CreateOrder(orderDetail, customerEmail);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFound", "Hotel");
        }

        [HttpGet("remove")]
        public IActionResult Delete(Guid hotelId)
        {
            _orderService.DeleteOrderDetail(hotelId);

            return RedirectToAction(nameof(Index));
        }
    }
}