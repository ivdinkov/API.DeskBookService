using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Data.DataSettings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.DeskBookService.Data.Context
{
    public class DeskBookerDataContext : IDeskBookerDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }

        public DeskBookerDataContext(IOptions<DeskDatabaseSettings> settings)
        {
            _mongoClient = new MongoClient(settings.Value.ConnectionString);
            _db = _mongoClient.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return _db.GetCollection<T>(name);
        }
    }
}
