using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using MongoDB.Driver;

namespace API.DeskBookService.Data.Context
{
    public class DeskBookerDataContext : IDeskBookerDataContext
    {
        public DeskBookerDataContext(IDeskDatabaseSettings settings)
        {
            var _mongoClient = new MongoClient(settings.ConnectionString);
            var _db = _mongoClient.GetDatabase(settings.DatabaseName);

            Desks = _db.GetCollection<Desk>("Desk");
            DesksBooking = _db.GetCollection<DeskBooking>("DeskBooking");
        }

        public IMongoCollection<Desk> Desks { get; private set; }
        public IMongoCollection<DeskBooking> DesksBooking { get; private set; }
    }
}
