using AutoMapper;
using HotelShare.Domain.Models.SqlModels.FilterModels;
using HotelShare.Domain.Models.SqlModels.HotelModels;
using HotelShare.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using HotelShare.Web.ViewModels.Hotel;
using Newtonsoft.Json;

namespace HotelShare.Web.Controllers.API
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;

        public HotelController(IHotelService hotelService,
            IMapper mapper,
            IRoomService roomService,
            ICommentService commentService)
        {
            _hotelService = hotelService;
            _mapper = mapper;
            _roomService = roomService;
            _commentService = commentService;
        }

        [HttpGet("")]
        public IActionResult GetAll([FromForm] FilterDataModel filters)
        {
            var hotels = _hotelService.ProcessFiltering(filters);

            if (hotels.Hotels.Any())
            {
                var serializedHotels = JsonConvert.SerializeObject(hotels.Hotels);

                return Ok(serializedHotels);
            }

            return NoContent();
        }

        [HttpGet("{hotelId}")]
        public IActionResult Get(Guid hotelId)
        {
            var hotel = _hotelService.Get(hotelId);

            if (hotel != null)
            {
                var serializedHotel = JsonConvert.SerializeObject(hotel);

                return Ok(serializedHotel);
            }

            return NoContent();
        }

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public IActionResult Create([FromForm] HotelViewModel model)
        {
            if (String.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError(nameof(HotelViewModel.Name), "The Name field is required.");
            }

            if (String.IsNullOrEmpty(model.Description))
            {
                ModelState.AddModelError(nameof(HotelViewModel.Description), "The Description field is required.");
            }

            if (ModelState.IsValid)
            {
                var hotel = _mapper.Map<HotelViewModel, Hotel>(model);
                var hotelId = Guid.NewGuid();
                hotel.Id = hotelId;

                //if (!String.IsNullOrEmpty(model.Room))
                //{
                //    var room = _roomService.GetRoomByName(model.Room);
                //}

                _hotelService.Create(hotel);

                return StatusCode(201);
            }

            return StatusCode(400);
        }

        [HttpDelete("delete/{hotelId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public IActionResult Remove(Guid hotelId)
        {
            _hotelService.Delete(hotelId);

            return Ok();
        }

        [HttpPut("update/{hotelId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public IActionResult Update([FromForm] HotelViewModel entity)
        {
            if (ModelState.IsValid)
            {
                var hotel = _mapper.Map<Hotel>(entity);

                _hotelService.Edit(hotel);

                return Ok();
            }

            return StatusCode(304);
        }

        [HttpGet("{hotelId}/comments")]
        public IActionResult GetComments(Guid hotelId)
        {
            var hotelComments = _commentService.GetAllCommentsByGameKey(hotelId);

            return Ok(hotelComments);
        }

        [HttpGet("{hotelId}/comments/{commentId}")]
        public IActionResult GetComment(Guid hotelId, Guid commentId)
        {
            var hotelComment = _commentService.GetAllCommentsByGameKey(hotelId)
                .FirstOrDefault(c => c.CommentId == commentId && c.HotelId == hotelId);

            if (hotelComment != null)
            {
                return Ok(hotelComment);
            }

            return StatusCode(400);
        }

        //[HttpGet("{hotelId}/room")]
        //public IActionResult GetRoom(string hotelId)
        //{
        //    var hotel = _hotelService.Get(hotelId);

        //    if (hotel.Room != null)
        //    {
        //        return Ok(hotel.Room);
        //    }

        //    return StatusCode(204);
        //}
    }
}