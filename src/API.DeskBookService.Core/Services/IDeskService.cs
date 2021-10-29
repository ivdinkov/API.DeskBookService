using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.Services
{
    /// <summary>
    /// Desk service interface
    /// </summary>
    public interface IDeskService
    {
        /// <summary>
        /// Save new Desk object
        /// </summary>
        /// <param name="desk">Desk object</param>
        /// <returns>Returns the new created Desk object</returns>
        Task<Desk> Save(Desk desk);

        /// <summary>
        /// Get object
        /// </summary>
        /// <param name="id">Mongodb Id of the Desk</param>
        /// <returns>Desk object</returns>
        Task<Desk> Get(string id);

        /// <summary>
        /// Get list of Desks
        /// </summary>
        /// <returns>IEnumerable collection of Desks</returns>
        Task<IEnumerable<Desk>> GetAll();

        /// <summary>
        /// Update object
        /// </summary>
        /// <param name="id">Mongodb Id of the Desk</param>
        /// <param name="deskIn">Desk object</param>
        /// <returns>True or False result</returns>
        Task<bool> Update(string id, DeskUpdateRequest deskIn);

        /// <summary>
        /// Delete object
        /// </summary>
        /// <param name="id">Mongodb Id of the Desk</param>
        /// <returns>true or False result</returns>
        Task<bool> Remove(string id);
    }
}
