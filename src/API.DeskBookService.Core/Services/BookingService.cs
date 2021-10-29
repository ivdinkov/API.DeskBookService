using API.DeskBookService.Core.Conracts.Requests;
using API.DeskBookService.Core.Conracts.Responses;
using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.Services
{
    /// <summary>
    /// Booking Service
    /// </summary>
    public class BookingService : IBookingService
    {
        private IBookingRepository _bookingRepo;

        /// <summary>
        /// Booking service constructor
        /// </summary>
        /// <param name="bookingRepository">Injects IBookingRepository</param>
        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepo = bookingRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deskBookingRequest"></param>
        /// <returns></returns>
        public async Task<DeskBookingResult> BookDesk(DeskBookingRequest deskBookingRequest)
        {
            return await _bookingRepo.BookDesk(deskBookingRequest);
        }

        /// <summary> 
        /// Get all Bookings
        /// </summary>
        /// <returns>Returns a list of all Bookings objects</returns>
        public async Task<IEnumerable<DeskBooking>> GetAll()
        {
            return await _bookingRepo.Get();
        }

        /// <summary>
        /// Get Booking by Id
        /// </summary>
        /// <param name="id">The Id of the booking</param>
        /// <returns>Returns DeskBooking object</returns>
        public async Task<DeskBooking> Get(string id)
        {
            return await _bookingRepo.Get(id);
        }

        /// <summary>
        /// Deletes Bookings object
        /// </summary>
        /// <param name="id">Booking Id</param>
        /// <returns>True or False result</returns>
        public async Task<bool> Remove(string id)
        {
            var result = await Get(id);
            if (result == null)
                return false;

            return await _bookingRepo.Remove(id);
        }

        /// <summary>
        /// Save new booking
        /// </summary>
        /// <param name="deskBooking">DeskBooking object</param>
        /// <returns>Returns DeskBooking object</returns>
        public async Task<DeskBooking> Save(DeskBooking deskBooking)
        {
            return await _bookingRepo.Save(deskBooking);
        }

        /// <summary>
        /// Update Booking by Id
        /// </summary>
        /// <param name="id">DeskBooking id</param>
        /// <param name="deskBookingIn">DeskBooking object</param>
        /// <returns>True or False result</returns>
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
