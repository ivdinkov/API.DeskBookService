using API.DeskBookService.Core.Conracts.Requests;
using API.DeskBookService.Core.Conracts.Responses;
using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.Services
{
    public interface IBookingService
    {
        Task<DeskBookingResult> BookDesk(DeskBookingRequest deskBookingRequest);
        Task<IEnumerable<DeskBooking>> GetAll();
        Task<DeskBooking> Get(string id);
        Task<bool> Remove(string id);
        Task<DeskBooking> Save(DeskBooking deskBooking);
        Task<bool> Update(string id, DeskBookingUpdateRequest deskBooking);
    }
}