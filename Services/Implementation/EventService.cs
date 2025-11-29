using wBialyBezdomnyEdition.Database.NoSQL.Entities;
using wBialyBezdomnyEdition.Repository.NoSQL;
using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Services.Implementation
{
    public class EventService : IQueryService<Event>
    {
        private readonly IBaseRepository<Event> _noSqlRepository;

        public EventService(IBaseRepository<Event> noSqlRepository)
        {
            _noSqlRepository = noSqlRepository;
        }

        public async Task<EndpointResponse<Event?>> GetByKeyAsync(
            BaseRequest request,
            string id,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    var result = await _noSqlRepository.GetByKeyAsync(id);
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
                    var result = await _noSqlRepository.GetManyAsync(request);
                    return new EndpointResponse<IReadOnlyList<Event>> { Data = result };

                default:
                    return new EndpointResponse<IReadOnlyList<Event>>
                    {
                        Data = Array.Empty<Event>()
                    };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Event>>> AddAsync(
            PostRequest<Event> request,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    if (request.Data != null)
                        await _noSqlRepository.AddAsync(request.Data);

                    return new EndpointResponse<IReadOnlyList<Event>> { Data = null };

                default:
                    return new EndpointResponse<IReadOnlyList<Event>>
                    {
                        Data = Array.Empty<Event>()
                    };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Event>>> UpdateAsync(
            PostRequest<Event> request,
            string id,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    if (request.Data != null)
                        await _noSqlRepository.UpdateAsync(id, request.Data);

                    return new EndpointResponse<IReadOnlyList<Event>> { Data = new List<Event> { request.Data } };

                default:
                    return new EndpointResponse<IReadOnlyList<Event>>
                    {
                        Data = Array.Empty<Event>()
                    };
            }
        }

        public async Task<EndpointResponse<bool>> DeleteAsync(
            BaseRequest request,
            string id,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _noSqlRepository.DeleteAsync(id);
                    return new EndpointResponse<bool> { Data = true };

                default:
                    return new EndpointResponse<bool> { Data = false };
            }
        }
    }
}
