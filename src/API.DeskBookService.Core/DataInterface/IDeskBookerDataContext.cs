using API.DeskBookService.Core.Domain;
using MongoDB.Driver;

namespace API.DeskBookService.Core.DataInterfaces
{
    public interface IDeskBookerDataContext
    {
        IMongoCollection<Desk> Desks { get; }
        IMongoCollection<DeskBooking> DesksBooking { get; }
    }
}