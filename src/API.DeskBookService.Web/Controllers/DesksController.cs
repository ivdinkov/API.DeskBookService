using API.DeskBookService.Core.Contracts;
using API.DeskBookService.Core.Contracts.v1;
using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.DeskBookService.Web.Controllers
{
    /// <summary>
    /// Desks Controller
    /// </summary>
    [ApiController]
    public class DesksController : Controller
    {
        private IDeskRepository _deskRepository;

        /// <summary>
        /// Inject IDeskRepository
        /// </summary>
        /// <param name="deskRepository"></param>
        public DesksController(IDeskRepository deskRepository)
        {
            _deskRepository = deskRepository;
        }

        /// <summary>
        /// Get all desks
        /// </summary>
        /// <returns>Return List of Desk objects</returns>
        [Produces("application/json")]
        [HttpGet(APIRoutesV1.Desks.GetDesksAsync)]
        public async Task<IActionResult> GetDesksAsync()
        {
            var desks = await _deskRepository.Get();
            if (desks == null)
                return NotFound(new { result = "fail",message = "No Desks found" });

            return Ok(desks);
        }

        /// <summary>
        /// Get desk by ID
        /// </summary>
        /// <param name="id">The ID of the desk you want to get</param>
        /// <returns>Return the requested Desk object</returns>
        [Produces("application/json")]
        [HttpGet(APIRoutesV1.Desks.GetDeskAsync)]
        public async Task<IActionResult> GetDeskAsync([FromRoute] string id)
        {
            var desk = await _deskRepository.Get(id);
            if (desk == null)
                return NotFound(new { result = "fail",message = $"Desk id:{id} not found" });

            return Ok(desk);
        }

        /// <summary>
        /// Save new desk
        /// </summary>
        /// <param name="newDesk">New desk object you want to save</param>
        /// <returns>Return the new Desk</returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost(APIRoutesV1.Desks.SaveDeskAsync)]
        public async Task<IActionResult> SaveDeskAsync([FromBody] DeskSaveRequest newDesk)
        {
            Desk desk = new Desk { Name = newDesk.Name, Description = newDesk.Description };
            var createdDesk = await _deskRepository.Save(desk);
            return Ok(createdDesk);
        }

        /// <summary>
        /// Update desk by ID
        /// </summary>
        /// <param name="id">The ID of the desk you want to update</param>
        /// <param name="deskIn">Desk with new info</param>
        /// <returns>An ActionResult</returns>
        [Consumes("application/json")]
        [HttpPut(APIRoutesV1.Desks.UpdateDeskAsync)]
        public async Task<IActionResult> UpdateDeskAsync([FromRoute] string id, [FromBody] Desk deskIn)
        {
            var desk = await _deskRepository.Get(id);
            if (desk == null)
                return NotFound(new { result = "fail",message = $"Desk id:{id} not found" });

            var success = await _deskRepository.Update(id, deskIn);
            if (success)
                return Ok(deskIn);

            return NoContent();
        }

        /// <summary>
        /// Delete desk by ID
        /// </summary>
        /// <param name="id">The ID of the desk you want to delete</param>
        /// <returns>An ActionResult</returns>
        /// <remarks>
        /// Sample request (This request will delete Desk by Id) \
        /// DELETE /desks/id
        /// </remarks>
        [HttpDelete(APIRoutesV1.Desks.DeleteDeskAsync)]
        public async Task<IActionResult> DeleteDeskAsync([FromRoute] string id)
        {
            var desk = await _deskRepository.Get(id);
            if (desk == null)
                return NotFound(new { result = "fail",message = $"Desk id:{id} not found" });

            var success = await _deskRepository.Remove(desk.Id);
            if (success)
                return Ok(new { result="success",message = $"Desk id:{id} successfully deleted" });

            return NoContent();
        }
    }
}
