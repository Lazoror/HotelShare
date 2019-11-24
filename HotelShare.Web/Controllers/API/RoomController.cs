using System;
using AutoMapper;
using HotelShare.Domain.Models.SqlModels;
using HotelShare.Interfaces.Services;
using HotelShare.Web.ViewModels.Room;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HotelShare.Web.Controllers.API
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomController(IRoomService roomService,
            IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var rooms = _roomService.GetAllRooms();
            var roomsView = _mapper.Map<IEnumerable<RoomViewModel>>(rooms);

            if (roomsView.Any())
            {
                return Ok(roomsView);
            }

            return NoContent();
        }

        [HttpGet("{roomId}")]
        public IActionResult Get(Guid roomId)
        {
            var room = _roomService.Get(roomId);
            var roomView = _mapper.Map<RoomViewModel>(room);

            if (room != null)
            {
                return Ok(roomView);
            }

            return NoContent();
        }


        [HttpPost("delete/{companyName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public IActionResult Remove(string companyName)
        {
            _roomService.Delete(companyName);

            return Ok();
        }

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public IActionResult Create([FromForm] RoomViewModel room)
        {
            if (ModelState.IsValid)
            {
                var roomModel = _mapper.Map<Room>(room);
                _roomService.CreateRoom(roomModel);
                var roomOrigin = _roomService.Get(room.RoomId);

                return StatusCode(201);
            }

            return StatusCode(400);
        }

        [HttpPost("update/{companyName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public IActionResult Update([FromForm] RoomViewModel room)
        {
            if (ModelState.IsValid)
            {
                var roomEntity = _roomService.Get(room.RoomId);
                roomEntity.RoomName = room.CompanyName;
                roomEntity.Description = room.Description;
                _roomService.EditRoom(roomEntity);

                return Ok();
            }

            return StatusCode(304);
        }
    }
}