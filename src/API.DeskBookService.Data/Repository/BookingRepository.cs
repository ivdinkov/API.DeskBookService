using API.DeskBookService.Core.Conracts.v1.Requests;
using API.DeskBookService.Core.Conracts.v1.Responses;
using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DeskBookService.Data.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private IMongoCollection<DeskBooking> _deskBookings;
        private IMongoCollection<Desk> _desks;

        public BookingRepository(IDeskBookerDataContext deskBookerContext)
        {
            _deskBookings = deskBookerContext.DesksBooking;
            _desks = deskBookerContext.Desks;
        }

        public async Task<DeskBooking> Save(DeskBooking deskBooking)
        {
            await _deskBookings.InsertOneAsync(deskBooking);
            return deskBooking;
        }

        public async Task<DeskBookingResult> BookDesk(DeskBookingRequest deskBookingRequest)
        {
            var bookings = await _deskBookings.Find(d => d.DeskId.Equals(deskBookingRequest.DeskId)).ToListAsync();
            var isBooked = bookings.Where(s => s.Date.ToShortDateString() == deskBookingRequest.Date.ToShortDateString()).Any();

            var result = Create<DeskBookingResult>(deskBookingRequest);
            if (!isBooked)
            {
                var deskBooking = Create<DeskBooking>(deskBookingRequest);

                await Save(deskBooking);

                result.DeskBookingId = deskBooking.Id;
                result.Code = DeskBookingResultCode.Success;
            }
            else
            {
                result.Code = DeskBookingResultCode.NoDeskAvailable;
            }
            return result;
        }

        public async Task<DeskBooking> Get(string id)
        {
            return await _deskBookings.Find<DeskBooking>(booking => booking.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DeskBooking>> Get()
        {
            return await _deskBookings.Find(_ => true).ToListAsync();
        }

        public async Task<bool> Update(string id, DeskBooking deskBooking)
        {
            var result = await _deskBookings.ReplaceOneAsync(desk => deskBooking.Id == id, deskBooking);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }   
        
        public async Task<bool> Remove(string id)
        {
            var result = await _deskBookings.DeleteOneAsync(desk => desk.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        private static T Create<T>(DeskBookingRequest request) where T : DeskBookingBase, new()
        {
            return new T
            {
                DeskId = request.DeskId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date,
            };
        }
    }
}
