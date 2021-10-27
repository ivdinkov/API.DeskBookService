using API.DeskBookService.Core.Conracts.Requests;
using API.DeskBookService.Core.Conracts.Responses;
using API.DeskBookService.Core.Domain;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.DataInterfaces
{
    /// <summary>
    /// IBookingRepository
    /// </summary>
    public interface IBookingRepository : IBaseRepository<DeskBooking>
    {
        /// <summary>
        /// User request to book a desk
        /// </summary>
        /// <param name="deskBookingRequest">The user booking request</param>
        /// <returns>DeskBookingResult Task</returns>
        Task<DeskBookingResult> BookDesk(DeskBookingRequest deskBookingRequest);
    }
}
