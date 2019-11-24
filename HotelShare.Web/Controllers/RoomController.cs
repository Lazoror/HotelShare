using System;
using System.Collections.Generic;
using AutoMapper;
using HotelShare.Domain;
using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels;
using HotelShare.Interfaces.Services;
using HotelShare.Web.Attributes;
using HotelShare.Web.ViewModels.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelShare.Web.Controllers
{
    [Route("room")]
    [StoreAuthorize(AuthorizePermission.Allow, RoleName.Manager)]
    public class RoomController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;

        public RoomController(IMapper mapper, IRoomService roomService)
        {
            _mapper = mapper;
            _roomService = roomService;
        }

        public ViewResult Index()
        {
            var rooms = _roomService.GetAllRooms();
            var roomsView = _mapper.Map<IEnumerable<RoomViewModel>>(rooms);

            return View(roomsView);
        }

        [HttpGet("{companyName}")]
        [AllowAnonymous]
        public ViewResult RoomDetails(Guid roomId)
        {
            var room = _roomService.Get(roomId);
            var roomView = _mapper.Map<RoomViewModel>(room);

            return View(roomView);
        }

        [HttpGet("new")]
        public IActionResult CreateRoom()
        {
            return View();
        }

        [HttpPost("new")]
        public IActionResult CreateRoom(RoomViewModel room)
        {
            if (ModelState.IsValid)
            {
                var roomModel = _mapper.Map<Room>(room);
                _roomService.CreateRoom(roomModel);
                var roomOrigin = _roomService.Get(room.RoomId);

                return RedirectToAction("Index", "Hotel");
            }

            return View(room);
        }

        [HttpGet("remove")]
        public IActionResult Delete(string companyName)
        {
            _roomService.Delete(companyName);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit")]
        public ViewResult Edit(Guid roomId)
        {
            var room = _roomService.Get(roomId);
            var roomView = _mapper.Map<RoomViewModel>(room);

            roomView.OldCompanyName = room.RoomName;

            return View(roomView);
        }

        [HttpPost("edit")]
        public IActionResult Edit(RoomViewModel room)
        {
            if (ModelState.IsValid)
            {
                var roomEntity = _roomService.Get(room.RoomId);
                roomEntity.RoomName = room.CompanyName;
                roomEntity.Description = room.Description;
                _roomService.EditRoom(roomEntity);

                return RedirectToAction(nameof(Index));
            }

            return View(room);
        }
    }
}