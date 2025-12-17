using Microsoft.AspNetCore.Mvc;
using wBialyDBAdapter.Model;
using wBialyDBAdapter.Services;

namespace wBialyDBAdapter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost("filter")]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<UnifiedTagModel>>>> GetAll(
            [FromBody] EndpointRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _tagService.GetAllAsync(request, cancellationToken);
            return Ok(response);
        }
    }
}