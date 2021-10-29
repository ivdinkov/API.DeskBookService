using API.DeskBookService.Core.Conracts.Requests;
using API.DeskBookService.Core.Conracts.Responses;
using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.Services
{
    public class BookingService : IBookingService
    {
        private IBookingRepository _bookingRepo;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepo = bookingRepository;
        }
        public async Task<DeskBookingResult> BookDesk(DeskBookingRequest deskBookingRequest)
        {
            return await _bookingRepo.BookDesk(deskBookingRequest);
        }

        public async Task<IEnumerable<DeskBooking>> GetAll()
        {
            return await _bookingRepo.Get();
        }

        public async Task<DeskBooking> Get(string id)
        {
            return await _bookingRepo.Get(id);
        }

        public async Task<bool> Remove(string id)
        {
            var result = await Get(id);
            if (result == null)
                return false;

            return await _bookingRepo.Remove(id);
        }

        public async Task<DeskBooking> Save(DeskBooking deskBooking)
        {
            return await _bookingRepo.Save(deskBooking);
        }

        public async Task<bool> Update(string id, DeskBookingUpdateRequest deskBookingIn)
        {
            var deskBooking = await Get(id);
            if (deskBooking == null)
                return false;

            deskBooking.FirstName = deskBookingIn.FirstName;
            deskBooking.LastName = deskBookingIn.LastName;
            deskBooking.Email = deskBookingIn.Email;
            deskBooking.Message = deskBookingIn.Message;

            return await _bookingRepo.Update(id, deskBooking);
        }
    }
}
