using API.DeskBookService.Core.Domain;
using API.DeskBookService.Core.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.DataAccess.Repository
{
    public class DeskBookingRepository : IDeskBookingRepository
    {
        private IMongoCollection<DeskBooking> _deskBooking;

        public DeskBookingRepository(IDeskBookerDataContext deskBookerContext)
        {
            _deskBooking = deskBookerContext.DesksBooking;
        }

        public async Task<DeskBooking> Get(string id)
        {
            return await _deskBooking.Find<DeskBooking>(booking => booking.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<DeskBooking>> GetAll()
        {
            return await _deskBooking.Find(booking => true).ToListAsync();
        }

        public async Task<DeskBooking> Save(DeskBooking deskBooking)
        {
            await _deskBooking.InsertOneAsync(deskBooking);
            return deskBooking;
        }

        public async Task<bool> Update(string id, DeskBooking deskIn)
        {
            var success = await _deskBooking.ReplaceOneAsync(desk => deskIn.Id == id, deskIn);
            return success.ModifiedCount > 0;
        }

        public async Task<bool> Remove(string id)
        {
            var success = await _deskBooking.DeleteOneAsync(desk => desk.Id == id);
            return success.DeletedCount > 0;
        }

    }
}
