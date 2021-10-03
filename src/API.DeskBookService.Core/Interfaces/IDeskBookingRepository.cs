using API.DeskBookService.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.Interfaces
{
    public interface IDeskBookingRepository
    {
        Task<DeskBooking> Get(string id);
        Task<List<DeskBooking>> GetAll();
        Task<DeskBooking> Save(DeskBooking deskBooking);
        Task<bool> Update(string id, DeskBooking deskIn);
        Task<bool> Remove(string id);

    }
}
