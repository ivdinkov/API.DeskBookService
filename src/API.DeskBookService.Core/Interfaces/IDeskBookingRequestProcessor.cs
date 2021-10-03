using API.DeskBookService.Core.Domain;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.Processor.Interfaces
{
    public interface IDeskBookingRequestProcessor
    {
        Task<DeskBookingResult> BookDesk(DeskBookingRequest request);
    }
}
