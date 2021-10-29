﻿using API.DeskBookService.Core.Contracts;
using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.Domain;
using API.DeskBookService.Core.Services;
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

        /// <summary>
        /// Injects IDeskRepository
        /// </summary>
        /// <param name="deskService"></param>
        public DesksController(IDeskService deskService)
        {
            _deskService = deskService;
        }

        /// <summary>
        /// Get all desks
        /// </summary>
        /// <returns>Return List of Desk objects</returns>
        [Produces("application/json")]
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
        [HttpGet(APIRoutesV1.Desks.GetDeskAsync)]
        public async Task<IActionResult> GetDeskAsync([FromRoute] string id)
        {
            var desk = await _deskService.Get(id);
            if (desk==null)
                return BadRequest(new { result = "fail", message = $"Desk id:{id} not found" }); 

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
            var createdDesk = await _deskService.Save(desk);

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
        public async Task<IActionResult> UpdateDeskAsync([FromRoute] string id, [FromBody] DeskUpdateRequest deskIn)
        {
            var success = await _deskService.Update(id,deskIn);

            if (success)
                return Ok(new { result = "success", message = $"Desk id:{id} successfully updated" });
            else
                return BadRequest(new { result = "fail", message = $"Unable to update Desk id:{id}" });
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
            var success = await _deskService.Remove(id);

            if (success)
                return Ok(new { result = "success", message = $"Desk id:{id} successfully deleted" });
            else
                return BadRequest(new { result = "fail", message = $"Unable to delete Desk id:{id}" });
        }
    }
}
