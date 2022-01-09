using API.DeskBookService.Core.Conracts.Requests;
using API.DeskBookService.Core.Conracts.Responses;
using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DeskBookService.Data.Repository
{
    /// <summary>
    /// Booking repository
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private IMongoCollection<DeskBooking> _deskBookings;
        private IMongoCollection<Desk> _desks;

        /// <summary>
        /// Booking repository
        /// </summary>
        /// <param name="deskBookerContext">Injects IDeskBookerDataContext</param>
        public BookingRepository(IDeskBookerDBContext deskBookerDBContext)
        {
            _desks = deskBookerDBContext.GetCollection<Desk>(Collections.Desk.ToString());
            _deskBookings = deskBookerDBContext.GetCollection<DeskBooking>(Collections.DeskBooking.ToString());
        }

        /// <summary>
        /// Save new booking
        /// </summary>
        /// <param name="deskBooking">DeskBooking object</param>
        /// <returns>Returns DeskBooking object</returns>
        public async Task<DeskBooking> Save(DeskBooking deskBooking)
        {
            await _deskBookings.InsertOneAsync(deskBooking);
            return deskBooking;
        }

        /// <summary>
        /// Book a Desk
        /// </summary>
        /// <param name="deskBookingRequest">DeskBookingRequest object</param>
        /// <returns>Returns DeskBookingResult object</returns>
        public async Task<DeskBookingResult> BookDesk(DeskBookingRequest deskBookingRequest)
        {
            var result = Create<DeskBookingResult>(deskBookingRequest);
            result.Code = DeskBookingResultCode.NoDeskAvailable;

            try
            {
                var checkDeckId = _desks.Find(desk => desk.Id == deskBookingRequest.DeskId).Any();
                if (!checkDeckId)
                {
                    result.Code = DeskBookingResultCode.InvalidDeskId;
                    return result;
                }
            }
            catch (System.FormatException)
            {
                result.Code = DeskBookingResultCode.InvalidDeskId;
                return result;
            }

            var allBookings = await Get();
            var filter = allBookings
                .Where(s => s.Date.ToShortDateString() == deskBookingRequest.Date.ToShortDateString()
                    && s.DeskId.Contains(deskBookingRequest.DeskId))
                .Any();

            if (!filter && !(DateTime.Compare(deskBookingRequest.Date, DateTime.Now) < 0))
            {
                var deskBooking = Create<DeskBooking>(deskBookingRequest);

                await Save(deskBooking);

                result.DeskBookingId = deskBooking.Id;
                result.Code = DeskBookingResultCode.Success;

                return result;
            }

            return result;
        }

        /// <summary>
        /// Get Booking by Id
        /// </summary>
        /// <param name="id">The Id of the booking</param>
        /// <returns>Returns DeskBooking object</returns>
        public async Task<DeskBooking> Get(string id)
        {
            try
            {
                return await _deskBookings.Find(booking => booking.Id == id).FirstOrDefaultAsync();
            }
            catch (System.FormatException)
            {
                return null;
            }
        }

        /// <summary> 
        /// Get all Bookings
        /// </summary>
        /// <returns>Returns a list of all Bookings objects</returns>
        public async Task<IEnumerable<DeskBooking>> Get()
        {
            return await _deskBookings.Find(_ => true).ToListAsync();
        }

        /// <summary>
        /// Update Booking by Id
        /// </summary>
        /// <param name="id">DeskBooking id</param>
        /// <param name="deskBooking">DeskBooking object</param>
        /// <returns>True or False result</returns>
        public async Task<bool> Update(string id, DeskBooking deskBooking)
        {
            var result = await _deskBookings.ReplaceOneAsync(desk => deskBooking.Id == id, deskBooking);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes Bookings object
        /// </summary>
        /// <param name="id">Booking Id</param>
        /// <returns>True or False result</returns>
        public async Task<bool> Remove(string id)
        {
            var result = await _deskBookings.DeleteOneAsync(desk => desk.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        /// <summary>
        /// Generic class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private static T Create<T>(DeskBookingRequest request) where T : DeskBookingBase, new()
        {
            return new T
            {
                DeskId = request.DeskId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Message = request.Message,
                Date = request.Date
            };
        }
    }
}
