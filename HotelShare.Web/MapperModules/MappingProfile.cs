using AutoMapper;
using HotelShare.Domain.Models.SqlModels;
using HotelShare.Domain.Models.SqlModels.AccountModels;
using HotelShare.Domain.Models.SqlModels.CommentModels;
using HotelShare.Domain.Models.SqlModels.HotelModels;
using HotelShare.Domain.Models.SqlModels.OrderModels;
using HotelShare.Web.ViewModels.Account;
using HotelShare.Web.ViewModels.Comment;
using HotelShare.Web.ViewModels.Hotel;
using HotelShare.Web.ViewModels.Payment;
using HotelShare.Web.ViewModels.Room;

namespace HotelShare.Web.MapperModules
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel, HotelViewModel>().ReverseMap();

            //CreateMap<GameViewModel, Hotel>().ForMember(a => a.Rooms,
            //    act => act.MapFrom(g => new Room {RoomName = g.Room}));

            //CreateMap<Hotel, GameViewModel>()
            //    .ForMember(g => g.Room, act => act.MapFrom(gm => gm.Room.CompanyName))
            //    .ForMember(g => g.Name, act => act.MapFrom(gm => gm.Name))
            //    .ForMember(g => g.Description, act => act.MapFrom(gm => gm.Description));


            CreateMap<Comment, DisplayCommentModel>().ForMember(dc => dc.HotelId, act => act.MapFrom(c => c.Hotel.Id))
                .ForMember(dc => dc.Quote, act => act.MapFrom(c => c.Quote))
                .ForMember(cm => cm.CommentId, act => act.MapFrom(c => c.Id));

            CreateMap<EditRoleViewModel, Role>()
                .ReverseMap()
                .ForMember(rm => rm.NewRoleName, act => act.MapFrom(r => r.Name))
                .ForMember(rm => rm.RoleId, act => act.MapFrom(r => r.Id));

            CreateMap<User, DeleteUserViewModel>().ReverseMap();
            CreateMap<CommentViewModel, Comment>().ReverseMap();
            CreateMap<CreateCommentViewModel, Comment>();
            CreateMap<RoomViewModel, Room>();
            CreateMap<OrderDetail, OrderDetailViewModel>();
            CreateMap<CreateCommentViewModel, Comment>();
        }
    }
}