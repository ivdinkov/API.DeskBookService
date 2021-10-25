using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.DataInterfaces
{
    /// <summary>
    /// Mongodb generic repository 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Save object
        /// </summary>
        /// <param name="t">The type of the obect</param>
        /// <returns></returns>
        Task<T> Save(T t);

        /// <summary>
        /// Get object
        /// </summary>
        /// <param name="id">Mongodb Id of the object</param>
        /// <returns></returns>
        Task<T> Get(string id);

        /// <summary>
        /// Get list of objects
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> Get();

        /// <summary>
        /// Update object
        /// </summary>
        /// <param name="id">Mongodb Id of the object</param>
        /// <param name="t">T type of the object</param>
        /// <returns></returns>
        Task<bool> Update(string id, T t);

        /// <summary>
        /// Delete object
        /// </summary>
        /// <param name="id">Mongodb Id of the object</param>
        /// <returns></returns>
        Task<bool> Remove(string id);
    }
}