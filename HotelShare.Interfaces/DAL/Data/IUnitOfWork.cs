using HotelShare.Interfaces.DAL.RepositorySql;

namespace HotelShare.Interfaces.DAL.Data
{
    public interface IUnitOfWork
    {
        void Commit();

        IRepository<T> GetRepository<T>() where T : class;
    }
}