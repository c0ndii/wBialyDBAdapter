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

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<Gastro>>>> GetAll(
            CancellationToken cancellationToken)
        {
            var request = new EndpointRequest
            {
                DatabaseType = DatabaseType.NoSQL
            };

            var response = await _gastroService.GetManyAsync(request, cancellationToken);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<Gastro?>>> GetById(
            string id,
            CancellationToken cancellationToken)
        {
            var request = new EndpointRequest
            {
                DatabaseType = DatabaseType.NoSQL
            };

            var response = await _gastroService.GetByKeyAsync(request, id, cancellationToken);

            if (response.Data == null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<Gastro>>>> Create(
            [FromBody] Gastro model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = new EndpointRequest
            {
                DatabaseType = DatabaseType.NoSQL
            };

            var response = await _gastroService.AddAsync(request, model, cancellationToken);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<Gastro>>>> Update(
            string id,
            [FromBody] Gastro model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = new EndpointRequest
            {
                DatabaseType = DatabaseType.NoSQL
            };

            var response = await _gastroService.UpdateAsync(request, id, model, cancellationToken);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse<bool>>> Delete(
            string id,
            CancellationToken cancellationToken)
        {
            var request = new EndpointRequest
            {
                DatabaseType = DatabaseType.NoSQL
            };

            var response = await _gastroService.DeleteAsync(request, id, cancellationToken);

            if (!response.Data)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
