using Autofac;
using HotelShare.DAL.Data;
using HotelShare.DAL.Repository.SqlRepositories;
using HotelShare.Interfaces.DAL.Data;
using HotelShare.Interfaces.DAL.RepositorySql;


namespace HotelShare.Infrastructure.AutofacModules
{
    public class DALModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<HotelContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterType<HotelRepository>().As<IHotelRepository>();


            //builder.RegisterGeneric(typeof(Repository<>)).Keyed(RepositoryType.SQL, typeof(IRepository<>));

            //builder.RegisterType<GameRepository>().Keyed<IGameRepository>(RepositoryType.SQL);

            //builder.RegisterType<Repository<Platform>>().As<IRepository<Platform>>();
            //builder.RegisterType<Repository<Comment>>().As<IRepository<Comment>>();
            //builder.RegisterType<Repository<Language>>().As<IRepository<Language>>();
            //builder.RegisterType<Repository<RoomTranslation>>().As<IRepository<RoomTranslation>>();
            //builder.RegisterType<Repository<PlatformTranslation>>().As<IRepository<PlatformTranslation>>();
            //builder.RegisterType<Repository<GenreTranslation>>().As<IRepository<GenreTranslation>>();
            //builder.RegisterType<Repository<GameTranslation>>().As<IRepository<GameTranslation>>();
            //builder.RegisterType<Repository<GameState>>().As<IRepository<GameState>>();
            //builder.RegisterType<Repository<Genre>>().As<IRepository<Genre>>();
            //builder.RegisterType<Repository<Room>>().As<IRepository<Room>>();
            //builder.RegisterType<Repository<Image>>().As<IRepository<Image>>();
            //builder.RegisterType<Repository<User>>().As<IRepository<User>>();
            //builder.RegisterType<Repository<Role>>().As<IRepository<Role>>();
        }
    }
}