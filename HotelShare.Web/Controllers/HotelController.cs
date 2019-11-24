using AutoMapper;
using HotelShare.Domain;
using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.FilterModels;
using HotelShare.Domain.Models.SqlModels.GameModels;
using HotelShare.Interfaces.Services;
using HotelShare.Web.Attributes;
using HotelShare.Web.ViewModels.Comment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelShare.Domain.Models.SqlModels;
using HotelShare.Web.ViewModels.Hotel;

namespace HotelShare.Web.Controllers
{
    [Route("hotels")]
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;
        private IHostingEnvironment _hostingEnvironment;

        public HotelController(
            IHotelService hotelService,
            IMapper mapper,
            IRoomService roomService,
            ICommentService commentService,
            IAzureService azureService,
            IHostingEnvironment hostingEnvironment)
        {
            _hotelService = hotelService;
            _mapper = mapper;
            _roomService = roomService;
            _commentService = commentService;
            _azureService = azureService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("")]
        public ViewResult Index(FilterDataModel filters)
        {
            var valuesDto = GetFilterValues();
            var hotelsFiltersDto = _hotelService.ProcessFiltering(filters);
            hotelsFiltersDto.DefaultValues = valuesDto;

            return View(hotelsFiltersDto);
        }

        [HttpGet("image")]
        public async Task<IActionResult> GamePicture()
        {
            var picture = await _azureService.DownloadAndRead("2a329545-3e70-4974-a04c-79f82bb57dba.png");

            return View(picture);
        }

        [HttpPost("list")]
        public IActionResult GetGameList(FilterDataModel filters)
        {
            var valuesDto = GetFilterValues();
            var hotelsFiltersDto = _hotelService.ProcessFiltering(filters);
            hotelsFiltersDto.DefaultValues = valuesDto;

            return PartialView("_Games", hotelsFiltersDto);
        }

        [HttpGet("new")]
        [StoreAuthorize(AuthorizePermission.Allow, RoleName.Manager)]
        public ViewResult Add()
        {
            return View();
        }

        [HttpPost("new")]
        [StoreAuthorize(AuthorizePermission.Allow, RoleName.Manager)]
        public IActionResult Add(HotelViewModel entity)
        {
            if (String.IsNullOrEmpty(entity.Name))
            {
                ModelState.AddModelError(nameof(HotelViewModel.Name), "The Name field is required.");
            }

            if (String.IsNullOrEmpty(entity.Description))
            {
                ModelState.AddModelError(nameof(HotelViewModel.Description), "The Description field is required.");
            }

            if (ModelState.IsValid)
            {
                var hotel = _mapper.Map<HotelViewModel, Hotel>(entity);
                var hotelId = Guid.NewGuid();
                hotel.Id = hotelId;

                //if (!String.IsNullOrEmpty(entity.Room))
                //{
                //    var room = _roomService.GetRoomByName(entity.Room);
                //}

                //if (entity.Images.Any())
                //{
                //    AddPictureSync(entity.Images, entity.Key);
                //}

                _hotelService.Create(hotel);

                return RedirectToAction("Index");
            }

            //var rooms = _roomService.GetAllRoomCompanyNames().ToList();

            return View(entity);
        }

        [HttpGet("update")]
        [StoreAuthorize(AuthorizePermission.Allow, RoleName.Manager)]
        public IActionResult Edit(Guid hotelId)
        {
            var hotel = _hotelService.Get(hotelId);
            var hotelView = _mapper.Map<HotelViewModel>(hotel);

            return View(hotelView);
        }

        [HttpPost("update")]
        [StoreAuthorize(AuthorizePermission.Allow, RoleName.Manager)]
        public IActionResult Edit(HotelViewModel entity)
        {
            if (ModelState.IsValid)
            {
                var hotel = _mapper.Map<Hotel>(entity);

                _hotelService.Edit(hotel);

                return RedirectToAction(nameof(GetDetailsByKey), new { hotelId = entity.Id });
            }

            return View(entity);
        }

        [Route("{hotelId}")]
        [HttpGet]
        public async Task<ViewResult> GetDetailsByKey(Guid hotelId)
        {
            var hotel = _hotelService.Get(hotelId);

            var displayGameView = new DisplayGameDetailsByIdRequestModel();

            if (hotel != null)
            {
                var hotelComments = _commentService.GetAllCommentsByGameKey(hotelId);
                var commentViewModel = new DisplayCommentViewModel { HotelId = hotelId, Comments = hotelComments };
                var hotelViewModel = _mapper.Map<Hotel, HotelViewModel>(hotel);

                displayGameView.Id = hotelId;
                displayGameView.HotelViewModel = hotelViewModel;
                displayGameView.Comments = commentViewModel;
            }

            return View("GetDetailsByKey", displayGameView);
        }

        [Route("remove")]
        [HttpGet]
        [StoreAuthorize(AuthorizePermission.Allow, RoleName.Manager)]
        public IActionResult Delete(Guid hotelId)
        {
            _hotelService.Delete(hotelId);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet("notFound")]
        public ViewResult NotFound()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult ChangeRating([FromBody] HotelViewModel hotelViewModel)
        //{
        //    var hotel = _hotelService.Get(hotelViewModel.Id);

        //    if (hotel == null)
        //    {
        //        return PartialView("NotFound");
        //    }

        //    var oldRating = hotel.Rating * hotel.RatingQuantity + hotelViewModel.Rating;

        //    var newRating = oldRating / ++hotel.RatingQuantity;

        //    hotel.Rating = newRating;

        //    _hotelService.Edit(hotel);

        //    var hotelView = _mapper.Map<HotelViewModel>(hotel);

        //    return PartialView("_GameRating", hotelView);
        //}

        [HttpPost("add/picture")]
        public async Task AddPictureAsync(List<IFormFile> images, Guid hotelId)
        {
            var imageNames = new List<string>();

            foreach (var image in images)
            {
                if (image.Length > 0 && image.ContentType == "image/jpg" || image.ContentType == "image/png")
                {
                    var fileName = image.FileName;

                    var identifier = Guid.NewGuid();
                    var name = identifier + fileName;

                    await _azureService.UploadAsync(image.OpenReadStream(), name);

                    imageNames.Add(name);
                }
            }

            _hotelService.AddImage(hotelId, imageNames);
        }

        private FilterValues GetFilterValues()
        {
            var filterValuesDto = new FilterValues
            {
                Rooms = _roomService.GetAllRoomCompanyNames().ToList()
            };

            return filterValuesDto;
        }

        [HttpGet("add/room")]
        public IActionResult AddRoom(Guid hotelId)
        {
            var room = new Room {HotelId = hotelId};

            return View(room);
        }

        [HttpPost("add/room")]
        public IActionResult AddRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return View(room);
            }

            _roomService.CreateRoom(room);

            return RedirectToAction("GetDetailsByKey", new {hotelId = room.HotelId});
        }

    }
}