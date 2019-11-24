namespace HotelShare.Interfaces.DAL.RepositorySql
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : class;
    }
}