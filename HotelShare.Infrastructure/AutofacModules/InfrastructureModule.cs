using Autofac;
using HotelShare.Infrastructure.Factory;
using HotelShare.Interfaces.DAL.RepositorySql;

namespace HotelShare.Infrastructure.AutofacModules
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>();
        }
    }
}