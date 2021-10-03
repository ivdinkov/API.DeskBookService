using API.DeskBookService.Core.Domain;
using API.DeskBookService.Core.Interfaces;
using API.DeskBookService.Core.Processor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.DeskBookService.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : Controller
    {
        private IDeskBookingRepository _deskBookingkRepository;
        private IDeskBookingRequestProcessor _processor;

        public BookingsController(IDeskBookingRepository deskBookingRepository, IDeskBookingRequestProcessor processor)
        {
            _deskBookingkRepository = deskBookingRepository;
            _processor = processor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var deskBookings = await _deskBookingkRepository.GetAll();

            return Ok(deskBookings);
        }

        [HttpGet("{id:length(24)}", Name = "GetBooking")]
        public async Task<IActionResult> GetBookingAsync(string id)
        {
            var deskBooking = await _deskBookingkRepository.Get(id);

            if (deskBooking == null)
            {
                return NotFound();
            }

            return Ok(deskBooking);
        }

        [HttpPost]
        public async Task<IActionResult> BookAsync(DeskBookingRequest deskBookingRequest)
        {
            var result = await _processor.BookDesk(deskBookingRequest);
            return Ok(result.Code);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateBookingAsync(string id, DeskBooking deskBookingIn)
        {
            var deskBooking = await _deskBookingkRepository.Get(id);

            if (deskBooking == null)
            {
                return NotFound();
            }

            var success = await _deskBookingkRepository.Update(id, deskBookingIn);

            if (success)
            {
                return Ok();
            }

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteBookingkAsync(string id)
        {
            var deskBookingIn = await _deskBookingkRepository.Get(id);

            if (deskBookingIn == null)
            {
                return NotFound();
            }

            var success = await _deskBookingkRepository.Remove(deskBookingIn.Id);

            if (success)
            {
                return Ok();
            }

            return NoContent();
        }
    }
}
