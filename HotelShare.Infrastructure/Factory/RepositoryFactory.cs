using Autofac;
using HotelShare.Interfaces.DAL.RepositorySql;

namespace HotelShare.Infrastructure.Factory
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ILifetimeScope _lifeTimeScope;

        public RepositoryFactory(ILifetimeScope lifeTimeScope)
        {
            _lifeTimeScope = lifeTimeScope;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return _lifeTimeScope.Resolve<IRepository<T>>();
        }
    }
}