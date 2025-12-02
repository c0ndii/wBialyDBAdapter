using Microsoft.AspNetCore.Mvc;
using wBialyDBAdapter.Model;
using wBialyDBAdapter.Services;

namespace wBialyDBAdapter.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GastroController : ControllerBase
	{
		private readonly IQueryService<UnifiedGastroModel> _gastroService;

		public GastroController(IQueryService<UnifiedGastroModel> gastroService)
		{
			_gastroService = gastroService;
		}

		[HttpPost("filter")]
		public async Task<ActionResult<EndpointResponse<IReadOnlyList<UnifiedGastroModel>>>> GetAll(
			[FromBody] EndpointRequest request,
			CancellationToken cancellationToken)
		{
			var response = await _gastroService.GetManyAsync(request, cancellationToken);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<EndpointResponse<UnifiedGastroModel?>>> GetById(
			[FromRoute] string id,
			[FromQuery] BaseRequest request,
			CancellationToken cancellationToken)
		{
			var response = await _gastroService.GetByKeyAsync(request, id, cancellationToken);

			if (response.Data == null)
				return NotFound(response);

			return Ok(response);
		}

		[HttpPost]
		public async Task<ActionResult<EndpointResponse<IReadOnlyList<UnifiedGastroModel>>>> Create(
			[FromBody] PostRequest<UnifiedGastroModel> request,
			CancellationToken cancellationToken)
		{
			if (request.Data == null || !ModelState.IsValid)
				return BadRequest(ModelState);

			var response = await _gastroService.AddAsync(request, cancellationToken);
			return Ok(response);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<EndpointResponse<IReadOnlyList<UnifiedGastroModel>>>> Update(
			[FromRoute] string id,
			[FromBody] PostRequest<UnifiedGastroModel> request,
			CancellationToken cancellationToken)
		{
			if (request.Data == null || !ModelState.IsValid)
				return BadRequest(ModelState);

			var response = await _gastroService.UpdateAsync(request, id, cancellationToken);
			return Ok(response);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<EndpointResponse<bool>>> Delete(
			[FromRoute] string id,
			[FromBody] BaseRequest request,
			CancellationToken cancellationToken)
		{
			var response = await _gastroService.DeleteAsync(request, id, cancellationToken);

			if (!response.Data)
				return BadRequest(response);

			return Ok(response);
		}
	}

}
