using AutoMapper;
using HotelShare.Domain.Models.SqlModels.OrderModels;
using HotelShare.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HotelShare.Web.Controllers.API
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService,
            IMapper mapper, IHotelService hotelService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _hotelService = hotelService;
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
    }
}