using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using wBialyBezdomnyEdition.Database.NoSQL.Entities;
using wBialyBezdomnyEdition.Repository.NoSQL;
using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Services.Implementation
{
    public class GastroService : IQueryService<Gastro>
    {
        private readonly IBaseRepository<Gastro> _NoSqlRepository;

        public GastroService(IBaseRepository<Gastro> noSqlRepository)
        {
            _NoSqlRepository = noSqlRepository;
        }

        public async Task<EndpointResponse<Gastro?>> GetByKeyAsync(
            BaseRequest request,
            string id, 
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    var result = await _NoSqlRepository.GetByKeyAsync(id);
                    return new EndpointResponse<Gastro?> { Data = result };

                default:
                    return new EndpointResponse<Gastro?> { Data = null };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Gastro>>> GetManyAsync(
            EndpointRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    var result = await _NoSqlRepository.GetManyAsync(request);
                    return new EndpointResponse<IReadOnlyList<Gastro>> { Data = result };

                default:
                    return new EndpointResponse<IReadOnlyList<Gastro>>
                    {
                        Data = Array.Empty<Gastro>()
                    };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Gastro>>> AddAsync(
            PostRequest<Gastro> request,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _NoSqlRepository.AddAsync(request.Data);
                    return new EndpointResponse<IReadOnlyList<Gastro>> { Data = null };

                default:
                    return new EndpointResponse<IReadOnlyList<Gastro>>
                    {
                        Data = Array.Empty<Gastro>()
                    };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Gastro>>> UpdateAsync(
            PostRequest<Gastro> request,
            string id,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _NoSqlRepository.UpdateAsync(id, request.Data);
                    return new EndpointResponse<IReadOnlyList<Gastro>> { Data = [request.Data] };

                default:
                    return new EndpointResponse<IReadOnlyList<Gastro>>
                    {
                        Data = Array.Empty<Gastro>()
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
                    await _NoSqlRepository.DeleteAsync(id);
                    return new EndpointResponse<bool> { Data = true };

                default:
                    return new EndpointResponse<bool> { Data = false };
            }
        }
    }
}
