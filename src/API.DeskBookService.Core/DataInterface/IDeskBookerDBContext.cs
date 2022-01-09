using MongoDB.Driver;

namespace API.DeskBookService.Core.DataInterfaces
{
    public interface IDeskBookerDBContext
    {
        IClientSessionHandle Session { get; set; }

        IMongoCollection<T> GetCollection<T>(string name);
    }
}