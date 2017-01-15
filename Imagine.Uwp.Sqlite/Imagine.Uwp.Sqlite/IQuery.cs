using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zinio.SDK.Data
{
    public interface IQuery<T>
    {
        Task<IEnumerable<T>> SelectAsync(string query);

        Task<IEnumerable<T>> QueryAsync(string query);

        Task<IEnumerable<T>> SelectAsync(string format, params object[] parametters);

        Task<IEnumerable<T>> SelectAsync(int page, int size, string query = "");

        Task<T> FirstOrDefault(string query);
        
        Task<int> CommitAsync(T input);

        Task<int> InsertAsync(T input);

        Task<int> InsertAllAsync(IEnumerable<T> input);

        Task<int> UpdateAsync(T input);

        Task<int> DeleteAsync(T input);

        Task<int> Clear();
    }
}
