using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Data.Repository
{
    /// <summary>
    /// Desk Repository
    /// </summary>
    public class DeskRepository : IDeskRepository
    {
        private IMongoCollection<Desk> _desks;
        private IMongoCollection<DeskBooking> _deskBookings;

        /// <summary>
        /// Desk repo constructor
        /// </summary>
        /// <param name="deskBookerContext">Injects IDeskBookerDataContext</param>
        public DeskRepository(IDeskBookerDBContext deskBookerDBContext)
        {
            _desks = deskBookerDBContext.GetCollection<Desk>(typeof(Desk).Name);
            _deskBookings = deskBookerDBContext.GetCollection<DeskBooking>(typeof(DeskBooking).Name);
        }

        /// <summary>
        /// Save new Desk object
        /// </summary>
        /// <param name="desk">The Desk object</param>
        /// <returns>Returns the new Desk object</returns>
        public async Task<Desk> Save(Desk desk)
        {
            await _desks.InsertOneAsync(desk);
            return desk;
        }

        /// <summary>
        /// Get Desk by Id
        /// </summary>
        /// <param name="id">Requested Desk Id</param>
        /// <returns>Returns the Desk object</returns>
        public async Task<Desk> Get(string id)
        {
            try
            {
                return await _desks.Find<Desk>(desk => desk.Id == id).FirstOrDefaultAsync();
            }
            catch (System.FormatException)
            {
                return null;
            }            
        }

        /// <summary>
        /// Get all desks
        /// </summary>
        /// <returns>Retuns a list of all Desks</returns>
        public async Task<IEnumerable<Desk>> Get()
        {
            return await _desks.Find(_ => true).ToListAsync();
        }

        /// <summary>
        /// Remove Desk by Id
        /// </summary>
        /// <param name="id">The Id of the Desk to be removed</param>
        /// <returns>True or False result</returns>
        public async Task<bool> Remove(string id)
        {
            var booking = await _deskBookings.Find(b => b.DeskId == id).AnyAsync();
            if (booking)
                return false;

            var result = await _desks.DeleteOneAsync(desk => desk.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        /// <summary>
        /// Updates the Desk by Id
        /// </summary>
        /// <param name="id">The Id of the Desk</param>
        /// <param name="deskIn">The Desk object to be updated</param>
        /// <returns>True or False result</returns>
        public async Task<bool> Update(string id, Desk deskIn)
        {
            var result = await _desks.ReplaceOneAsync(d => d.Id == id, deskIn);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
