using API.DeskBookService.Core.Conracts.v1.Requests;
using API.DeskBookService.Core.Contracts.v1;
using API.DeskBookService.Core.Contracts.v1.Requests;
using API.DeskBookService.Core.DataInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.DeskBookService.Web.Controllers
{
    /// <summary>
    /// Bookings Controller
    /// </summary>
    [ApiController]
    public class BookingsController : Controller
    {
        private IBookingRepository _bookingRepository;

        /// <summary>
        /// Inject IBookingRepository
        /// </summary>
        /// <param name="bookingRepository"></param>
        public BookingsController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        /// <summary>
        /// Return list of bokings
        /// </summary>
        /// <returns>List of bokings</returns>
        [Produces("application/json")]
        [HttpGet(APIRoutesV1.Bookings.GetBokingsAsync)]
        public async Task<IActionResult> GetBookingsAsync()
        {
                var deskBookings = await _bookingRepository.Get();
                if (deskBookings == null)
                    return NotFound(new { result = "fail",message = "No Bookings found" });
                return Ok(deskBookings);
        }

        /// <summary>
        /// Get booking by ID
        /// </summary>
        /// <param name="id">The ID of the booking you want to get</param>
        /// <returns>Return the requested DeskBooking onject</returns>
        [Produces("application/json")]
        [HttpGet(APIRoutesV1.Bookings.GetBokingAsync)]
        public async Task<IActionResult> GetBookingAsync([FromRoute] string id)
        {
            var deskBooking = await _bookingRepository.Get(id);
            if (deskBooking == null)
                return NotFound(new { result = "fail",message = $"Booking id:{id} not found" });

            return Ok(deskBooking);
        }

        /// <summary>
        /// Save new booking
        /// </summary>
        /// <param name="deskBookingRequest"></param>
        /// <returns>Return booking result</returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost(APIRoutesV1.Bookings.BookAsync)]
        public async Task<IActionResult> BookAsync([FromBody] DeskBookingRequest deskBookingRequest)
        {
            var result = await _bookingRepository.BookDesk(deskBookingRequest);
            return Ok(new { message = result.Code.ToString() });
        }

        /// <summary>
        /// Update booking by ID
        /// </summary>
        /// <param name="id">The ID of the booking you want to update</param>
        /// <param name="deskBookingIn">Booking with new info</param>
        /// <returns>An ActionResult</returns>
        [Consumes("application/json")]
        [HttpPut(APIRoutesV1.Bookings.UpdateBokingAsync)]
        public async Task<IActionResult> UpdateBookingAsync([FromRoute] string id, [FromBody] DeskBookingUpdateRequest deskBookingIn)
        {
            var deskBooking = await _bookingRepository.Get(id);
            if (deskBooking == null)
                return NotFound(new { result = "fail",message = $"Booking id:{id} not found" });

            deskBooking.FirstName = deskBookingIn.FirstName;
            deskBooking.LastName = deskBookingIn.LastName;
            deskBooking.Email = deskBookingIn.Email;
            deskBooking.Message = deskBookingIn.Message;
            var success = await _bookingRepository.Update(id, deskBooking);
            if (success)
                return Ok(deskBookingIn);

            return NoContent();
        }

        /// <summary>
        /// Delete booking by ID
        /// </summary>
        /// <param name="id">The ID of the booking you want to delete</param>
        /// <returns>An ActionResult</returns>
        [HttpDelete(APIRoutesV1.Bookings.DeleteBokingAsync)]
        public async Task<IActionResult> DeleteBookingkAsync([FromRoute] string id)
        {
            var deskBookingIn = await _bookingRepository.Get(id);
            if (deskBookingIn == null)
                return NotFound(new { message = $"Booking id:{id} not found" });

            var success = await _bookingRepository.Remove(deskBookingIn.Id);
            if (success)
                return Ok(new { result = "success",message =$"Booking id:{id} successfully deleted"});

            return NoContent();
        }

   }
}
