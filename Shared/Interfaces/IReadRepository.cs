using Ardalis.Specification;

namespace JobFileSystem.Shared.Interfaces
{
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}