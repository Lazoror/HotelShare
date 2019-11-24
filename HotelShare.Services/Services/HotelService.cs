using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels;
using HotelShare.Domain.Models.SqlModels.FilterModels;
using HotelShare.Domain.Models.SqlModels.GameModels;
using HotelShare.Interfaces.DAL.Data;
using HotelShare.Interfaces.DAL.RepositorySql;
using HotelShare.Interfaces.Services;
using HotelShare.Services.Filtering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace HotelShare.Services.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelRepository _hotelRepository;
        private readonly string _language;

        public HotelService(IUnitOfWork unitOfWork,
            IHotelRepository hotelRepository)
        {
            _language = CultureInfo.CurrentCulture.Name;
            _unitOfWork = unitOfWork;
            _hotelRepository = hotelRepository;
        }

        public void Create(Hotel entity)
        {
            entity.AddDate = DateTime.UtcNow;
            entity.Id = Guid.NewGuid();

            _hotelRepository.Insert(entity);
            _unitOfWork.Commit();
        }

        public void Edit(Hotel entity)
        {
            var oldGame = JsonConvert.SerializeObject(Get(entity.Id));
            var hotel = Get(entity.Id);

            hotel.Name = entity.Name;
            hotel.Description = entity.Description;
            hotel.Discontinued = entity.Discontinued;
            hotel.AvailableRooms = entity.AvailableRooms;

            _hotelRepository.Update(hotel);

            //FillGamePlatform(ref hotel, platforms);


            //if (entity.Room != null && !string.IsNullOrEmpty(entity.Room.CompanyName))
            //{
            //    var room = _unitOfWork.GetRepository<Room>().FirstOrDefault(p => p.CompanyName == entity.Room.CompanyName);

            //    hotel.Room = room;
            //}

            _unitOfWork.Commit();
        }


        public void AddImage(Guid hotelId, List<string> imageNames)
        {
            var hotel = Get(hotelId);

            if (hotel == null)
            {
                return;
            }

            hotel.GameImages = new List<HotelImage>();

            foreach (var imageName in imageNames)
            {
                var image = _unitOfWork.GetRepository<Image>().FirstOrDefault(i => i.Name == imageName);

                if (image != null)
                {
                    var hotelImage = new HotelImage
                    {
                        ImageId = image.Id,
                        HotelId = hotel.Id
                    };

                    hotel.GameImages.Add(hotelImage);
                }
            }

            _hotelRepository.Update(hotel);
            _unitOfWork.Commit();
        }

        //public IEnumerable<string> GetAllGameGenres(string hotelId)
        //{
        //    var hotel = _hotelRepository.FirstOrDefault(a => a.Key == hotelId);
        //    var genres = new List<string>();

        //    foreach (GameGenre genre in hotel.GameGenres)
        //    {
        //        var genreEntity = _genreRepository.FirstOrDefault(a => a.Id == genre.GenreId);
        //        genres.Add(genreEntity.Name);
        //    }

        //    return genres;
        //}

        public HotelFilters ProcessFiltering(FilterDataModel filters)
        {
            var hotelsFilters = new HotelFilters { Filters = filters };
            var hotelPipelineExpression = new GamePipelineBuilder(filters).WithSearchFilter()
                .WithGamePriceFilter().WithGameRoomFilter().WithGameReleaseDateFilter().Build();
            var hotels = GetAllHotels(filters.SortType, _language, filter: hotelPipelineExpression, skip: (filters.CurrentPage - 1) * filters.ItemsPerPage, take: filters.ItemsPerPage);

            filters.TotalPages = GetTotalPages(_hotelRepository.Count(hotelPipelineExpression), filters.ItemsPerPage);
            hotelsFilters.Hotels = hotels;

            return hotelsFilters;
        }

        //public IEnumerable<string> GetGamePlatforms(string hotelId)
        //{
        //    var hotel = _hotelRepository.FirstOrDefault(a => a.Key == hotelId);

        //    foreach (var hotelPlatform in hotel.GamePlatforms)
        //    {
        //        if (hotelPlatform.PlatformType == null)
        //        {
        //            hotelPlatform.PlatformType = _platformRepository.FirstOrDefault(p => p.Id == hotelPlatform.PlatformTypeId);
        //        }
        //    }

        //    return hotel.GamePlatforms.Select(p => p.PlatformType.Name);
        //}

        public int CountAllHotels()
        {
            return _hotelRepository.Count();
        }

        public void Delete(Guid hotelId)
        {
            var hotel = _hotelRepository.FirstOrDefault(a => a.Id == hotelId);

            if (hotel != null)
            {
                hotel.IsDeleted = true;
                _hotelRepository.Update(hotel);
                _unitOfWork.Commit();
            }
        }

        public Hotel Get(Guid hotelId)
        {
            var hotel = _hotelRepository.FirstOrDefault(a => a.Id == hotelId);

            return hotel;
        }

        public IEnumerable<Hotel> GetAllHotels(SortType sortType, string lang, Expression<Func<Hotel, bool>> filter = null, int skip = 0, int take = Int32.MaxValue)
        {
            var sortModel = new GameSortingBuilder().ResolveSorting(sortType).Build();

            var hotels = _hotelRepository.GetMany(skip, take: take, filter: filter, sortModel.OrderExpression, sortModel.SortDirection).ToList();

            return hotels;
        }

        private int GetTotalPages(int allHotelsCount, int itemsPerPage)
        {
            decimal totalPages = (decimal)allHotelsCount / itemsPerPage;
            int result = allHotelsCount / itemsPerPage;

            if (totalPages % 2 != 0 && allHotelsCount - itemsPerPage != 0)
            {
                result++;
            }

            return result;
        }

        private void FillHotelRooms(ref Hotel hotel, List<string> platforms)
        {
            if (platforms != null && platforms.Any())
            {
                hotel.Rooms = new List<Room>();

                foreach (var platform in platforms)
                {
                    //var platformEntity = _platformRepository.FirstOrDefault(p => p.Name == platform);

                    var hotelPlatform = new Room();
                    {
                        //GameId = hotel.Id,
                        //Game = hotel,
                        //PlatformType = platformEntity,
                        //PlatformTypeId = platformEntity.Id
                    };

                    hotel.Rooms.Add(hotelPlatform);
                }
            }
        }
    }
}