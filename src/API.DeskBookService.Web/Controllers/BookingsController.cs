using API.DeskBookService.Core.Conracts.Requests;
using API.DeskBookService.Core.Conracts.Responses;
using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.Contracts.Responses;
using API.DeskBookService.Core.Domain;
using API.DeskBookService.Core.Services;
using Microsoft.AspNetCore.Http;
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
        private IBookingService _bookingService;

        /// <summary>
        /// Inject IBookingRepository
        /// </summary>
        /// <param name="bookingRepository"></param>
        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Return list of bokings
        /// </summary>
        /// <returns>List of bokings</returns>
        [Produces("application/json")]
        [HttpGet(APIRoutesV1.Bookings.GetBokingsAsync)]
        public async Task<IActionResult> GetBookingsAsync()
        {
            var deskBookings = await _bookingService.GetAll();
            return Ok(deskBookings);
        }

        /// <summary>
        /// Get booking by ID
        /// </summary>
        /// <param name="id">The ID of the booking you want to get</param>
        /// <returns>Return the requested DeskBooking onject</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DeskBooking), 200)]
        [ProducesResponseType(typeof(Response), 404)]
        [HttpGet(APIRoutesV1.Bookings.GetBokingAsync)]
        public async Task<IActionResult> GetBookingAsync([FromRoute] string id)
        {
            var deskBooking = await _bookingService.Get(id);
            if (deskBooking != null)
                return Ok(deskBooking);

            return NotFound(new Response { Code = ResponseCode.Error.ToString(), Message = $"Booking id:{id} not found" });
        }

        /// <summary>
        /// Save new booking
        /// </summary>
        /// <param name="deskBookingRequest"></param>
        /// <returns>Return booking result</returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DeskBookingResult),201)]
        [ProducesResponseType(typeof(Response),400)]
        [HttpPost(APIRoutesV1.Bookings.BookAsync)]
        public async Task<IActionResult> BookAsync([FromBody] DeskBookingRequest deskBookingRequest)
        {
            var createdBooking = await _bookingService.BookDesk(deskBookingRequest);
            if (createdBooking.Code.ToString() == "Success")
            {
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                var locationUri = baseUrl + "/" + APIRoutesV1.Bookings.GetBokingAsync.Replace("{id}", createdBooking.DeskBookingId);

                return Created(locationUri, createdBooking);
            }

            return BadRequest(new Response { Code = ResponseCode.Error.ToString(), Message = createdBooking.Code.ToString() });
        }

        /// <summary>
        /// Update booking by ID
        /// </summary>
        /// <param name="id">The ID of the booking you want to update</param>
        /// <param name="deskBookingIn">Booking with new info</param>
        /// <returns>An ActionResult</returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPut(APIRoutesV1.Bookings.UpdateBokingAsync)]
        public async Task<IActionResult> UpdateBookingAsync([FromRoute] string id, [FromBody] DeskBookingUpdateRequest deskBookingIn)
        {
            var success = await _bookingService.Update(id, deskBookingIn);
            if (success)
                return Ok(new Response { Code = ResponseCode.Success.ToString(), Message = $"Booking id:{id} successfully updated" });

            return BadRequest(new Response { Code = ResponseCode.Error.ToString(), Message = $"Unable to update Booking id:{id}" });
        }

        /// <summary>
        /// Delete booking by ID
        /// </summary>
        /// <param name="id">The ID of the booking you want to delete</param>
        /// <returns>An ActionResult</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpDelete(APIRoutesV1.Bookings.DeleteBokingAsync)]
        public async Task<IActionResult> DeleteBookingkAsync([FromRoute] string id)
        {
            var success = await _bookingService.Remove(id);
            if (success)
                return Ok(new Response { Code = ResponseCode.Success.ToString(), Message = $"Booking id:{id} successfully deleted" });

            return BadRequest(new Response { Code = ResponseCode.Error.ToString(), Message = $"Unable to delete Booking id:{id}" });
        }
   }
}
