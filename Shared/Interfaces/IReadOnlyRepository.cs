using Ardalis.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobFileSystem.Shared.Interfaces
{
    public interface IReadOnlyRepository 
    {
        Task<T> GetByIdAsync<T>(string id) where T : BaseEntity;
        Task<IReadOnlyList<T>> ListAllAsync<T>() where T : BaseEntity;
        Task<IReadOnlyList<T>> ListAsync<T>(ISpecification<T> spec) where T : BaseEntity;
        Task<int> CountAsync<T>(ISpecification<T> spec) where T : BaseEntity;
        Task<T> FirstAsync<T>(ISpecification<T> spec) where T : BaseEntity;
        Task<T> FirstOrDefaultAsync<T>(ISpecification<T> spec) where T : BaseEntity;
    }
}