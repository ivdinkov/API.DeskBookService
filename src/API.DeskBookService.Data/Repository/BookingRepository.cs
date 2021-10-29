using API.DeskBookService.Core.Conracts.Requests;
using API.DeskBookService.Core.Conracts.Responses;
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

            if (!filter)
            {
                var deskBooking = Create<DeskBooking>(deskBookingRequest);

                await Save(deskBooking);

                result.DeskBookingId = deskBooking.Id;
                result.Code = DeskBookingResultCode.Success;

                return result;
            }

            return result;
        }

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
