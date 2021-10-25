using API.DeskBookService.Core.Domain;
using MongoDB.Driver;

namespace API.DeskBookService.Core.DataInterfaces
{
    /// <summary>
    /// DataContext
    /// </summary>
    public interface IDeskBookerDataContext
    {
        /// <summary>
        /// Desk collection
        /// </summary>
        IMongoCollection<Desk> Desks { get; }

        /// <summary>
        /// Bookings collection
        /// </summary>
        IMongoCollection<DeskBooking> DesksBooking { get; }
    }
}