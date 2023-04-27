using Ardalis.Specification;

namespace JobFileSystem.Shared.Interfaces
{

    public interface IRepository<T> : IRepositoryBase<T> where T : class
    {
    }
}