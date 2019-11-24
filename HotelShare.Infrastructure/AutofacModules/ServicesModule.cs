using Autofac;
using HotelShare.Interfaces.Services;
using HotelShare.Services.Services;

namespace HotelShare.Infrastructure.AutofacModules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommentService>().As<ICommentService>();
            builder.RegisterType<HotelService>().As<IHotelService>();
            builder.RegisterType<RoomService>().As<IRoomService>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<AzureService>().As<IAzureService>();
        }
    }
}