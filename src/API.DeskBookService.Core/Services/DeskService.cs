using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.Services
{
    /// <summary>
    /// Desk Service 
    /// </summary>
    public class DeskService : IDeskService
    {
        private readonly IDeskRepository _deskRepo;
        private readonly IBookingRepository _bookingRepo;

        /// <summary>
        /// Desk service constructor
        /// </summary>
        /// <param name="deskRepository">Injects IDeskRepository</param>
        /// <param name="bookingRepository">Injects IBookingRepository</param>
        public DeskService(IDeskRepository deskRepository, IBookingRepository bookingRepository)
        {
            _deskRepo = deskRepository;
            _bookingRepo = bookingRepository;
        }

        /// <summary>
        /// Get Desk by Id
        /// </summary>
        /// <param name="id">Desk Id</param>
        /// <returns>Returns Desk object</returns>
        public async Task<Desk> Get(string id)
        {
            return await _deskRepo.Get(id);
        }

        /// <summary>
        /// Get all Desks
        /// </summary>
        /// <returns>Returns a list of all Desks</returns>
        public async Task<IEnumerable<Desk>> GetAll()
        {
            return await _deskRepo.Get();
        }

        /// <summary>
        /// Remove Desk by Id
        /// </summary>
        /// <param name="id">The Id of the Desk to be removed</param>
        /// <returns>True or False result</returns>
        public async Task<bool> Remove(string id)
        {
            var result = await Get(id);
            if (result == null)
                return false;

            return await _deskRepo.Remove(id);
        }

        /// <summary>
        /// Save new Desk object
        /// </summary>
        /// <param name="desk">The Desk object</param>
        /// <returns>Returns the new Desk object</returns>
        public async Task<Desk> Save(Desk desk)
        {
            return await _deskRepo.Save(desk);
        }

        /// <summary>
        /// Updates the Desk by Id
        /// </summary>
        /// <param name="id">The Id of the Desk</param>
        /// <param name="deskIn">The Desk object to be updated</param>
        /// <returns>True or False result</returns>
        public async Task<bool> Update(string id, DeskUpdateRequest deskIn)
        {
            var desk = await Get(id);
            if (desk == null)
                return false;

            desk.Description = deskIn.Description;
            desk.Name = deskIn.Name;

            return await _deskRepo.Update(id,desk);
        }
    }
}
