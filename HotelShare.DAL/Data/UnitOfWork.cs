using HotelShare.Interfaces.DAL.Data;
using HotelShare.Interfaces.DAL.RepositorySql;

namespace HotelShare.DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelContext _context;
        private readonly IRepositoryFactory _repositoryFactory;

        public UnitOfWork(HotelContext context, IRepositoryFactory repositoryFactory)
        {
            _context = context;
            _repositoryFactory = repositoryFactory;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IRepository<T> GetRepository<T>()
            where T : class
        {
            return _repositoryFactory.GetRepository<T>();
        }
    }
}