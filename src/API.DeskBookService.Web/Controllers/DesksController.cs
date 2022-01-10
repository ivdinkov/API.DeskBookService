using API.DeskBookService.Core.Contracts;
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
    /// Desks Controller
    /// </summary>
    [ApiController]
    public class DesksController : Controller
    {
        private IDeskService _deskService;
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Injects IDeskRepository
        /// </summary>
        /// <param name="deskService"></param>
        public DesksController(IDeskService deskService, IHttpContextAccessor httpContextAccessor)
        {
            _deskService = deskService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all desks
        /// </summary>
        /// <returns>Return List of Desk objects</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Desk), 200)]
        [HttpGet(APIRoutesV1.Desks.GetDesksAsync)]
        public async Task<IActionResult> GetDesksAsync()
        {
            var desks = await _deskService.GetAll();
            return Ok(desks);
        }

        /// <summary>
        /// Get desk by ID
        /// </summary>
        /// <param name="id">The ID of the desk you want to get</param>
        /// <returns>Return the requested Desk object</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Desk), 200)]
        [ProducesResponseType(typeof(Response), 404)]
        [HttpGet(APIRoutesV1.Desks.GetDeskAsync)]
        public async Task<IActionResult> GetDeskAsync([FromRoute] string id)
        {
            var desk = await _deskService.Get(id);
            if (desk != null)
                return Ok(desk);

            return NotFound(new Response { Code = ResponseCode.Error.ToString(), Message = $"Desk id:{id} not found" });
        }

        /// <summary>
        /// Save new desk
        /// </summary>
        /// <param name="newDesk">New desk object you want to save</param>
        /// <returns>Return the new Desk</returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost(APIRoutesV1.Desks.SaveDeskAsync)]
        public async Task<IActionResult> SaveDeskAsync([FromBody] DeskSaveRequest newDesk)
        {
            Desk desk = new Desk { Name = newDesk.Name, Description = newDesk.Description };
            var createdDesk = await _deskService.Save(desk);

            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + APIRoutesV1.Desks.GetDeskAsync.Replace("{id}", createdDesk.Id);

            return Created(locationUri, createdDesk);
        }

        /// <summary>
        /// Update desk by ID
        /// </summary>
        /// <param name="id">The ID of the desk you want to update</param>
        /// <param name="deskIn">Desk with new info</param>
        /// <returns>An ActionResult</returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPut(APIRoutesV1.Desks.UpdateDeskAsync)]
        public async Task<IActionResult> UpdateDeskAsync([FromRoute] string id, [FromBody] DeskUpdateRequest deskIn)
        {
            var success = await _deskService.Update(id,deskIn);
            if (success)
                return Ok(new Response { Code = ResponseCode.Success.ToString(), Message = $"Desk id:{id} successfully updated" });
                
            return BadRequest(new Response { Code = ResponseCode.Error.ToString(), Message = $"Unable to update Desk id:{id}" });
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
        [Produces("application/json")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> DeleteDeskAsync([FromRoute] string id)
        {
            var success = await _deskService.Remove(id);
            if (success)
                return Ok(new Response { Code = ResponseCode.Success.ToString(), Message = $"Desk id:{id} successfully deleted" });
               
            return BadRequest(new Response { Code = ResponseCode.Error.ToString(), Message = $"Unable to delete Desk id:{id}" });
        }
    }
}
