using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobFileSystem.Shared.Interfaces
{
    public interface IClient<T>
    {
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken token=default);
        Task<T> GetAsync(string parentId, string childId, CancellationToken token = default);
        Task<T> CreateAsync(T dto, CancellationToken token = default);
        Task<T> UpdateAsync(T dto, CancellationToken token = default);
        Task<bool> DeleteAsync(string parentId, string childId, CancellationToken token = default);

        Task<int> CountAsync(CancellationToken token = default);
    }
}
