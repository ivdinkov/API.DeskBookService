using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Data.Repository
{
    public class DeskRepository : IDeskRepository
    {
        private IMongoCollection<Desk> _desks;

        public DeskRepository(IDeskBookerDataContext deskBookerContext)
        {
            _desks = deskBookerContext.Desks;
        }

        public async Task<Desk> Get(string id)
        {
            return await _desks.Find<Desk>(desk => desk.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Desk>> Get()
        {
            return await _desks.Find(_ => true).ToListAsync();
        }

        public async Task<bool> Remove(string id)
        {
            var result = await _desks.DeleteOneAsync(desk => desk.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Desk> Save(Desk desk)
        {
            await _desks.InsertOneAsync(desk);
            return desk;
        }

        public async Task<bool> Update(string id, Desk deskIn)
        {
            var result = await _desks.ReplaceOneAsync(desk => desk.Id == id, deskIn);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
