using Microsoft.AspNetCore.Mvc;
using Sea.Application.Requests.Requests;
using Sea.Application.Responses;
using Sea.Application.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace Sea.Api.Controllers;

[ApiController]
[Route("api/request")]
[ApiExplorerSettings(GroupName = "main")]
//[Authorize]
public class RequestController(IRequestService _service) : Controller
{
    [HttpGet("get-by-id")]
    [SwaggerOperation(Summary = "Fetches the task overview of a student.")]
    public async ValueTask<ActionResult<RequestResponse>> GetById(Guid id) => Ok(await _service.GetUserById(id));

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Fetches the task overview of a student.")]
    public async ValueTask<IActionResult> Add(RequestCreateRequest request) => Ok(await _service.Add(request));

    [HttpGet("get-list")]
    [SwaggerOperation(Summary = "Fetches the task overview of a student.")]
    public async ValueTask<ActionResult<RequestResponse>> GetRequestList(int page, int pageSize) => Ok(await _service.GetRequestList(page, pageSize));
}
