using API.DeskBookService.Core.Domain;
using API.DeskBookService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.DeskBookService.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesksController : Controller
    {
        private IDeskRepository _deskRepository;

        public DesksController(IDeskRepository deskRepository)
        {
            _deskRepository = deskRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDesksAsync()
        {
            var desks = await _deskRepository.GetAll();

            return Ok(desks);
        }

        [HttpGet("{id:length(24)}", Name = "GetDesk")]
        public async Task<IActionResult> GetDeskAsync(string id)
        {
            var desk = await _deskRepository.Get(id);

            if (desk == null)
            {
                return NotFound();
            }

            return Ok(desk);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDeskAsync(Desk desk)
        {
            var created = await _deskRepository.Save(desk);

            return CreatedAtRoute("GetDesk", new { id = created.Id.ToString() }, desk);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateDeskAsync(string id, Desk deskIn)
        {
            var desk = await _deskRepository.Get(id);
            if (desk == null)
            {
                return NotFound();
            }

            var success = await _deskRepository.Update(id, deskIn);
            if (success)
            {
                return Ok();
            }

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteDeskAsync(string id)
        {
            var desk = await _deskRepository.Get(id);
            if (desk == null)
            {
                return NotFound();
            }

            var success = await _deskRepository.Remove(desk.Id);
            if (success)
            {
                return Ok();
            }

            return NoContent();
        }
    }
}
