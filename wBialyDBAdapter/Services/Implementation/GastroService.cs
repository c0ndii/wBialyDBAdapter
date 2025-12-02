using wBialyDBAdapter.Mapping;
using wBialyDBAdapter.Model;
using wBialyDBAdapter.Repository.NoSQL;
using wBialyDBAdapter.Repository.Relational;
using wBialyDBAdapter.Repository.ObjectRelational;
using Rel = wBialyDBAdapter.Database.Relational.Entities;
using Obj = wBialyDBAdapter.Database.ObjectRelational.Entities;
using NoSql = wBialyDBAdapter.Database.NoSQL.Entities;

namespace wBialyDBAdapter.Services.Implementation
{
    public class GastroService : IQueryService<UnifiedGastroModel>
    {
        private readonly IGastroMapper _mapper;
        private readonly IBaseRepository<NoSql.Gastro> _mongoRepo;
        private readonly IRelationalRepository<Rel.Gastro> _relRepo;
        private readonly IObjectRelationalRepository<Obj.Gastro> _objRepo;

        public GastroService(
            IGastroMapper mapper,
            IBaseRepository<NoSql.Gastro> mongoRepo,
            IRelationalRepository<Rel.Gastro> relRepo,
            IObjectRelationalRepository<Obj.Gastro> objRepo)
        {
            _mapper = mapper;
            _mongoRepo = mongoRepo;
            _relRepo = relRepo;
            _objRepo = objRepo;
        }

        // GET BY ID
        public async Task<EndpointResponse<UnifiedGastroModel?>> GetByKeyAsync(BaseRequest request, string id, CancellationToken cancellationToken = default)
        {
            UnifiedGastroModel? data = request.DatabaseType switch
            {
                DatabaseType.NoSQL => _mapper.FromNoSql(await _mongoRepo.GetByKeyAsync(id)),
                DatabaseType.Relational => _mapper.FromRelational(await _relRepo.GetAsync(int.Parse(id))),
                DatabaseType.ObjectRelational => _mapper.FromObjectRelational(await _objRepo.GetAsync(int.Parse(id))),
                _ => null
            };

            return new EndpointResponse<UnifiedGastroModel?> { Data = data };
        }

        // GET MANY
        public async Task<EndpointResponse<IReadOnlyList<UnifiedGastroModel>>> GetManyAsync(EndpointRequest request, CancellationToken cancellationToken = default)
        {
            IReadOnlyList<UnifiedGastroModel> data = request.DatabaseType switch
            {
                DatabaseType.NoSQL => (await _mongoRepo.GetManyAsync(request)).Select(_mapper.FromNoSql).ToList(),
                DatabaseType.Relational => (await _relRepo.GetAllAsync()).Select(_mapper.FromRelational).ToList(),
                DatabaseType.ObjectRelational => (await _objRepo.GetAllAsync()).Select(_mapper.FromObjectRelational).ToList(),
                _ => Array.Empty<UnifiedGastroModel>()
            };

            return new EndpointResponse<IReadOnlyList<UnifiedGastroModel>> { Data = data };
        }

        // ADD
        public async Task<EndpointResponse<IReadOnlyList<UnifiedGastroModel>>> AddAsync(PostRequest<UnifiedGastroModel> request, CancellationToken cancellationToken = default)
        {
            if (request.Data == null)
                return new EndpointResponse<IReadOnlyList<UnifiedGastroModel>> { Data = Array.Empty<UnifiedGastroModel>() };

            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _mongoRepo.AddAsync(_mapper.ToNoSql(request.Data));
                    break;
                case DatabaseType.Relational:
                    await _relRepo.CreateAsync(_mapper.ToRelational(request.Data));
                    break;
                case DatabaseType.ObjectRelational:
                    await _objRepo.CreateAsync(_mapper.ToObjectRelational(request.Data));
                    break;
            }

            return new EndpointResponse<IReadOnlyList<UnifiedGastroModel>> { Data = new List<UnifiedGastroModel> { request.Data } };
        }

        // UPDATE
        public async Task<EndpointResponse<IReadOnlyList<UnifiedGastroModel>>> UpdateAsync(PostRequest<UnifiedGastroModel> request, string id, CancellationToken cancellationToken = default)
        {
            if (request.Data == null)
                return new EndpointResponse<IReadOnlyList<UnifiedGastroModel>> { Data = Array.Empty<UnifiedGastroModel>() };

            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _mongoRepo.UpdateAsync(id, _mapper.ToNoSql(request.Data));
                    break;
                case DatabaseType.Relational:
                    await _relRepo.UpdateAsync(_mapper.ToRelational(request.Data));
                    break;
                case DatabaseType.ObjectRelational:
                    await _objRepo.UpdateAsync(int.Parse(id), _mapper.ToObjectRelational(request.Data));
                    break;
            }

            return new EndpointResponse<IReadOnlyList<UnifiedGastroModel>> { Data = new List<UnifiedGastroModel> { request.Data } };
        }

        // DELETE
        public async Task<EndpointResponse<bool>> DeleteAsync(BaseRequest request, string id, CancellationToken cancellationToken = default)
        {
            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    await _mongoRepo.DeleteAsync(id);
                    break;
                case DatabaseType.Relational:
                    await _relRepo.DeleteAsync(int.Parse(id));
                    break;
                case DatabaseType.ObjectRelational:
                    await _objRepo.DeleteAsync(int.Parse(id));
                    break;
                default:
                    return new EndpointResponse<bool> { Data = false };
            }

            return new EndpointResponse<bool> { Data = true };
        }
    }
}
