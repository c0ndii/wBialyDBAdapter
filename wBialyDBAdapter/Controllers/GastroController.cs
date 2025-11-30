using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using wBialyBezdomnyEdition.Database.NoSQL.Entities;
using wBialyDBAdapter.Model;
using wBialyDBAdapter.Services;

namespace wBialyDBAdapter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GastroController : ControllerBase
    {
        private readonly IQueryService<Gastro> _gastroService;

        public GastroController(IQueryService<Gastro> gastroService)
        {
            _gastroService = gastroService;
        }

        [HttpPost("filter")]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<Gastro>>>> GetAll(
            [FromBody] EndpointRequest request,
            CancellationToken cancellationToken)
        {
            request.DatabaseType = DatabaseType.NoSQL;
            var response = await _gastroService.GetManyAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<Gastro?>>> GetById(
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
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<Gastro>>>> Create(
            [FromBody] PostRequest<Gastro> request,
            CancellationToken cancellationToken)
        {
            if (request.Data == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _gastroService.AddAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<Gastro>>>> Update(
            [FromRoute] string id,
            [FromBody] PostRequest<Gastro> request,
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
