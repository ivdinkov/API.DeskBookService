using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.DataInterfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Save(T t);
        Task<T> Get(string id);
        Task<IEnumerable<T>> Get();
        Task<bool> Update(string id, T t);
        Task<bool> Remove(string id);
    }
}