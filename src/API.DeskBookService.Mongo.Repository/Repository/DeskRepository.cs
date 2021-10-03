using API.DeskBookService.Core.Domain;
using API.DeskBookService.Core.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.DataAccess.Repository
{
    public class DeskRepository : IDeskRepository
    {
        private IMongoCollection<Desk> _desks;

        public DeskRepository(IDeskBookerDataContext deskBookerContext)
        {
            _desks = deskBookerContext.Desks;
        }
        public async Task<List<Desk>> GetAvailableDesks(DateTime date)
        {
            //var bookedDeskIds = _deskBookerContext.DeskBooking.
            //  Where(x => x.Date == date)
            //  .Select(b => b.DeskId)
            //  .ToList();

            //return _deskBookerContext.Desk
            //  .Where(x => !bookedDeskIds.Contains(x.Id))
            //  .ToList();

            throw new NotImplementedException();
        }
        public async Task<List<Desk>> GetAll()
        {
            return await _desks.Find(desk => true).ToListAsync();
        }

        public async Task<Desk> Get(string id)
        {
            return await _desks.Find<Desk>(desk => desk.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Desk> Save(Desk desk)
        {
            await _desks.InsertOneAsync(desk);
            return desk;
        }

        public async Task<bool> Update(string id, Desk deskIn)
        {
            var result = await _desks.ReplaceOneAsync(desk => desk.Id == id, deskIn);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> Remove(string id)
        {
            var result = await _desks.DeleteOneAsync(desk => desk.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
