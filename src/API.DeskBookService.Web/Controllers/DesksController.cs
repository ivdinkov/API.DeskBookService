using API.DeskBookService.Core.Contracts;
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
            try
            {
                var desk = await _deskRepository.Get(id);
                if (desk==null)
                    return BadRequest(new { result = "fail", message = $"Desk id:{id} not found" }); 

                return Ok(desk);
            }
            catch (System.Exception)
            { 
                return BadRequest(new { result = "fail", message = "Invalid deskId!" });
            }        
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

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + APIRoutesV1.Desks.GetDeskAsync.Replace("{id:length(24)}", createdDesk.Id);

            return Created(locationUri, createdDesk);
        }

        /// <summary>
        /// Update desk by ID
        /// </summary>
        /// <param name="id">The ID of the desk you want to update</param>
        /// <param name="deskIn">Desk with new info</param>
        /// <returns>An ActionResult</returns>
        [Consumes("application/json")]
        [HttpPut(APIRoutesV1.Desks.UpdateDeskAsync)]
        public async Task<IActionResult> UpdateDeskAsync([FromRoute] string id, [FromBody] DeskBase deskIn)
        {
            try
            {
                var desk = await _deskRepository.Get(id);
                if (desk == null)
                    return BadRequest(new { result = "fail", message = $"Desk id:{id} not found" });

                desk.Description = deskIn.Description;
                desk.Name = deskIn.Name;
                var success = await _deskRepository.Update(id, desk);
                if (success)
                    return Ok(desk);
                else
                    return BadRequest(new { result = "fail", message = $"Unable to update Desk id:{id}" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { result = "fail", message = "Invalid deskId!" });
            }
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
            try
            {
                var desk = await _deskRepository.Get(id);
                if (desk == null)
                    return NotFound(new { result = "fail", message = $"Desk id:{id} not found" });

                var success = await _deskRepository.Remove(desk.Id);
                if (success)
                    return Ok(new { result = "success", message = $"Desk id:{id} successfully deleted" });
                else
                    return BadRequest(new { result = "fail", message = $"Booking exist. Unable to delete Desk id:{id}" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { result = "fail", message = "Invalid deskId!" });
            }
        }
    }
}
