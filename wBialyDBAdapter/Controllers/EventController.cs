using Microsoft.AspNetCore.Mvc;
using wBialyDBAdapter.Model;
using wBialyDBAdapter.Services;

namespace wBialyDBAdapter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IQueryService<UnifiedEventModel> _eventService;

        public EventController(IQueryService<UnifiedEventModel> eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("filter")]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<UnifiedEventModel>>>> GetAll(
            [FromBody] EndpointRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _eventService.GetManyAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<UnifiedEventModel?>>> GetById(
            [FromRoute] string id,
            [FromQuery] BaseRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _eventService.GetByKeyAsync(request, id, cancellationToken);

            if (response.Data == null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<UnifiedEventModel>>>> Create(
            [FromBody] PostRequest<UnifiedEventModel> request,
            CancellationToken cancellationToken)
        {
            if (request.Data == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _eventService.AddAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<UnifiedEventModel>>>> Update(
            [FromRoute] string id,
            [FromBody] PostRequest<UnifiedEventModel> request,
            CancellationToken cancellationToken)
        {
            if (request.Data == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _eventService.UpdateAsync(request, id, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse<bool>>> Delete(
            [FromRoute] string id,
            [FromBody] BaseRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _eventService.DeleteAsync(request, id, cancellationToken);

            if (!response.Data)
                return BadRequest(response);

            return Ok(response);
        }
    }

}
