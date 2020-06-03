using System;
using System.Collections.Generic;
using AutoMapper;
using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels;
using HotelShare.Domain.Models.SqlModels.OrderModels;
using HotelShare.Interfaces.Services;
using HotelShare.Web.ViewModels.Payment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelShare.Web.Controllers.API
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService,
            IMapper mapper, IHotelService hotelService,
            IRoomService roomService, IUserService userService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _hotelService = hotelService;
            _roomService = roomService;
            _userService = userService;
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get(Guid orderId)
        {
            var order = _orderService.GetOrderById(orderId);

            if (order != null)
            {
                return Ok(order);
            }

            return StatusCode(204);
        }

        [HttpGet("current")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCurrent()
        {
            var order = _orderService.GetAllCartOrder(User.Identity.Name);

            if (order != null)
            {
                return Ok(order);
            }

            return StatusCode(204);
        }

        [HttpGet("pay")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Pay()
        {
            var order = _orderService.GetAllCartOrder(User.Identity.Name);
            order.OrderStatus = OrderStatus.Paid;

            _orderService.EditOrder(order);

            return Ok();
        }

        [HttpGet("current/details")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCurrentDetails()
        {
            var order = GetBasketItems();

            if (order != null)
            {
                var orderDetails = JsonConvert.SerializeObject(order);

                return Ok(orderDetails);
            }

            return StatusCode(204);
        }

        [HttpGet("completed/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetOrdered(string email)
        {
            var orders = GetOrderItems(email);

            if (orders != null)
            {
                var orderDetails = JsonConvert.SerializeObject(orders);

                return Ok(orderDetails);
            }

            return StatusCode(204);
        }

        [HttpGet("{roomId}/AddBasket/{email}")]
        public IActionResult AddToBasket(Guid roomId, string email)
        {
            var room = _roomService.Get(roomId);

            if (!room.IsDeleted)
            {
                AddToBasketUser(roomId, room, email);

                return Ok();
            }

            return NotFound();
        }

        private void AddToBasketUser(Guid roomId, Room room, string email)
        {
            var orderDetail = new OrderDetail { RoomId = roomId, Discount = 0, Quantity = 1, Price = room.Price };

            var customerEmail = User.Identity.Name;

            if (!string.IsNullOrEmpty(email))
            {
                customerEmail = email;
            }

            _orderService.CreateOrder(orderDetail, customerEmail);
        }

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create(Guid hotelId)
        {
            var hotel = _hotelService.Get(hotelId);

            if (!hotel.IsDeleted)
            {
                var orderDetail = new OrderDetail { RoomId = hotelId, Discount = 0, Quantity = 1 };
                var customerEmail = User.Identity.Name;

                _orderService.CreateOrder(orderDetail, customerEmail);

                return Ok(orderDetail);
            }

            return StatusCode(400);
        }

        //[HttpPost("remove")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public IActionResult Remove(Guid hotelId)
        //{
        //    if (!String.IsNullOrEmpty(hotelId))
        //    {
        //        _orderService.DeleteOrderDetail(hotelId);

        //        return Ok();
        //    }

        //    return StatusCode(304);
        //}

        [HttpPost("update/{hotelId}/{quantity}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateQuantity(Guid hotelId, int quantity)
        {
            var order = _orderService.GetAllCartOrder(User.Identity.Name);

            if (order != null)
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    if (orderDetail.RoomId == hotelId)
                    {
                        orderDetail.Quantity = (short)quantity;
                    }
                }

                _orderService.EditOrder(order);

                return Ok(order);
            }

            return StatusCode(304);
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

        private IEnumerable<OrderDetailViewModel> GetOrderItems(string email)
        {
            var user = _userService.GetUserByEmail(email);
            var orders = _orderService.GetOrderedRooms(user.Id);
            var allOrdered = new List<OrderDetailViewModel>();

            foreach (var order in orders)
            {
                var ordersView =
                    _mapper.Map<IEnumerable<OrderDetailViewModel>>(order.OrderDetails);

                foreach (var orderView in ordersView)
                {
                    var room = _roomService.Get(orderView.RoomId);
                    orderView.RoomName = room.RoomName;
                }

                allOrdered.AddRange(ordersView);
            }

            return allOrdered;
        }
    }
}