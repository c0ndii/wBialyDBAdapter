using Microsoft.AspNetCore.Mvc;
using System;
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
    public class EventController : ControllerBase
    {
        private readonly IQueryService<Event> _eventService;

        public EventController(IQueryService<Event> eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<Event>>>> GetAll(CancellationToken cancellationToken)
        {
            var request = new EndpointRequest
            {
                DatabaseType = DatabaseType.NoSQL
            };

            var response = await _eventService.GetManyAsync(request, cancellationToken);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<Event?>>> GetById(string id, CancellationToken cancellationToken)
        {
            var request = new EndpointRequest
            {
                DatabaseType = DatabaseType.NoSQL
            };

            var response = await _eventService.GetByKeyAsync(request, id, cancellationToken);

            if (response.Data == null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<Event>>>> Create(
            [FromBody] Event model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = new EndpointRequest
            {
                DatabaseType = DatabaseType.NoSQL
            };

            var response = await _eventService.AddAsync(request, model, cancellationToken);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndpointResponse<IReadOnlyList<Event>>>> Update(
            string id,
            [FromBody] Event model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = new EndpointRequest
            {
                DatabaseType = DatabaseType.NoSQL
            };

            var response = await _eventService.UpdateAsync(request, id, model, cancellationToken);

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

            var response = await _eventService.DeleteAsync(request, id, cancellationToken);

            if (!response.Data)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
