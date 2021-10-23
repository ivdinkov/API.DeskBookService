using API.DeskBookService.Core.Domain;
using MongoDB.Driver;

namespace API.DeskBookService.DataAccess
{
    public class DeskBookerDataContext : IDeskBookerDataContext
    {
        public DeskBookerDataContext(IDeskDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Desks = database.GetCollection<Desk>(settings.DeskCollectionName);
            DesksBooking = database.GetCollection<DeskBooking>(settings.DeskCollectionName);
        }
        public IMongoCollection<Desk> Desks { get; private set; }
        public IMongoCollection<DeskBooking> DesksBooking { get; private set; }
    }
}
