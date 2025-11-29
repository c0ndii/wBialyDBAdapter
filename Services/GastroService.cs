using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using wBialyBezdomnyEdition.Database.NoSQL.Entities;
using wBialyBezdomnyEdition.Repository.NoSQL;
using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Services
{
    public class GastroService : IQueryService<Gastro>
    {
        private readonly IGastroRepository _NoSqlRepository;

        public GastroService(IGastroRepository noSqlRepository)
        {
            _NoSqlRepository = noSqlRepository;
        }

        public async Task<EndpointResponse<Gastro?>> GetByKeyAsync(
            EndpointRequest request,
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
                    var result = await _NoSqlRepository.GetManyAsync();
                    return new EndpointResponse<IReadOnlyList<Gastro>> { Data = result };

                default:
                    return new EndpointResponse<IReadOnlyList<Gastro>>
                    {
                        Data = Array.Empty<Gastro>()
                    };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Gastro>>> AddAsync(
            EndpointRequest request,
            Gastro data,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _NoSqlRepository.AddAsync(data);
                    var result = await _NoSqlRepository.GetManyAsync();
                    return new EndpointResponse<IReadOnlyList<Gastro>> { Data = result };

                default:
                    return new EndpointResponse<IReadOnlyList<Gastro>>
                    {
                        Data = Array.Empty<Gastro>()
                    };
            }
        }

        public async Task<EndpointResponse<IReadOnlyList<Gastro>>> UpdateAsync(
            EndpointRequest request,
            string id,
            Gastro data,
            CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _NoSqlRepository.UpdateAsync(id, data);
                    var result = await _NoSqlRepository.GetManyAsync();
                    return new EndpointResponse<IReadOnlyList<Gastro>> { Data = result };

                default:
                    return new EndpointResponse<IReadOnlyList<Gastro>>
                    {
                        Data = Array.Empty<Gastro>()
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
                    await _NoSqlRepository.DeleteAsync(id);
                    return new EndpointResponse<bool> { Data = true };

                default:
                    return new EndpointResponse<bool> { Data = false };
            }
        }
    }
}
