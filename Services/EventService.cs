using wBialyBezdomnyEdition.Database.NoSQL.Entities;
using wBialyBezdomnyEdition.Repository.NoSQL;
using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Services
{
    public class EventService : IQueryService<Event>
    {
        private readonly IEventRepository _noSqlRepository;

        public EventService(IEventRepository noSqlRepository)
        {
            _noSqlRepository = noSqlRepository;
        }

        public async Task<EndpointResponse<Event?>> GetByKeyAsync(
            EndpointRequest request,
            string id,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    var result = await _noSqlRepository.GetByKeyAsync(id.ToString());
                    return new EndpointResponse<Event?> { Data = result };

                default:
                    return new EndpointResponse<Event?> { Data = null };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Event>>> GetManyAsync(
            EndpointRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    var result = await _noSqlRepository.GetManyAsync();
                    return new EndpointResponse<IReadOnlyList<Event>> { Data = result };

                default:
                    return new EndpointResponse<IReadOnlyList<Event>>
                    {
                        Data = Array.Empty<Event>()
                    };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Event>>> AddAsync(
            EndpointRequest request,
            Event data,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _noSqlRepository.AddAsync(data);
                    var result = await _noSqlRepository.GetManyAsync();
                    return new EndpointResponse<IReadOnlyList<Event>> { Data = result };

                default:
                    return new EndpointResponse<IReadOnlyList<Event>>
                    {
                        Data = Array.Empty<Event>()
                    };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Event>>> UpdateAsync(
            EndpointRequest request,
            string id,
            Event data,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _noSqlRepository.UpdateAsync(id.ToString(), data);
                    var result = await _noSqlRepository.GetManyAsync();
                    return new EndpointResponse<IReadOnlyList<Event>> { Data = result };

                default:
                    return new EndpointResponse<IReadOnlyList<Event>>
                    {
                        Data = Array.Empty<Event>()
                    };
            }
        }

        public async Task<EndpointResponse<bool>> DeleteAsync(
            EndpointRequest request,
            string id,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _noSqlRepository.DeleteAsync(id.ToString());
                    return new EndpointResponse<bool> { Data = true };

                default:
                    return new EndpointResponse<bool> { Data = false };
            }
        }
    }
}
