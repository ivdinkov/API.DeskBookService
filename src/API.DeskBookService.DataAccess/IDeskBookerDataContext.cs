using API.DeskBookService.Core.Domain;
using MongoDB.Driver;

namespace API.DeskBookService.DataAccess
{
    public interface IDeskBookerDataContext
    {
        IMongoCollection<Desk> Desks { get; }
        IMongoCollection<DeskBooking> DesksBooking { get; }
    }
}