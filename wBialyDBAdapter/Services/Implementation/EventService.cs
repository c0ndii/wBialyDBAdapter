using wBialyDBAdapter.Mapping;
using wBialyDBAdapter.Model;
using wBialyDBAdapter.Repository.NoSQL;
using wBialyDBAdapter.Repository.Relational;
using wBialyDBAdapter.Repository.ObjectRelational;
using Rel = wBialyDBAdapter.Database.Relational.Entities;
using Obj = wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Services.Implementation
{
    public class EventService : IQueryService<UnifiedEventModel>
    {
        private readonly IEventMapper _mapper;
        private readonly IBaseRepository<Database.NoSQL.Entities.Event> _mongoRepo;
        private readonly IRelationalRepository<Rel.Event> _relRepo;
        private readonly IObjectRelationalRepository<Obj.Event> _objRepo;

        public EventService(
            IEventMapper mapper,
            IBaseRepository<Database.NoSQL.Entities.Event> mongoRepo,
            IRelationalRepository<Rel.Event> relRepo,
            IObjectRelationalRepository<Obj.Event> objRepo)
        {
            _mapper = mapper;
            _mongoRepo = mongoRepo;
            _relRepo = relRepo;
            _objRepo = objRepo;
        }

        // GET BY ID
        public async Task<EndpointResponse<UnifiedEventModel?>> GetByKeyAsync(BaseRequest request, string id, CancellationToken cancellationToken = default)
        {
            UnifiedEventModel? data = request.DatabaseType switch
            {
                DatabaseType.NoSQL => _mapper.FromNoSql(await _mongoRepo.GetByKeyAsync(id)),
                DatabaseType.Relational => _mapper.FromRelational(await _relRepo.GetAsync(int.Parse(id))),
                DatabaseType.ObjectRelational => _mapper.FromObjectRelational(await _objRepo.GetAsync(int.Parse(id))),
                _ => null
            };

            return new EndpointResponse<UnifiedEventModel?> { Data = data };
        }

        public async Task<EndpointResponse<IReadOnlyList<UnifiedEventModel>>> GetManyAsync(EndpointRequest request, CancellationToken cancellationToken = default)
        {
            IReadOnlyList<UnifiedEventModel> data = request.DatabaseType switch
            {
                DatabaseType.NoSQL => (await _mongoRepo.GetManyAsync(request)).Select(_mapper.FromNoSql).ToList(),
                DatabaseType.Relational => (await _relRepo.GetAllAsync()).Select(_mapper.FromRelational).ToList(),
                DatabaseType.ObjectRelational => (await _objRepo.GetAllAsync()).Select(_mapper.FromObjectRelational).ToList(),
                _ => Array.Empty<UnifiedEventModel>()
            };

            return new EndpointResponse<IReadOnlyList<UnifiedEventModel>> { Data = data };
        }

        public async Task<EndpointResponse<IReadOnlyList<UnifiedEventModel>>> AddAsync(PostRequest<UnifiedEventModel> request, CancellationToken cancellationToken = default)
        {
            if (request.Data == null)
                return new EndpointResponse<IReadOnlyList<UnifiedEventModel>> { Data = Array.Empty<UnifiedEventModel>() };

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

            return new EndpointResponse<IReadOnlyList<UnifiedEventModel>> { Data = new List<UnifiedEventModel> { request.Data } };
        }

        public async Task<EndpointResponse<IReadOnlyList<UnifiedEventModel>>> UpdateAsync(PostRequest<UnifiedEventModel> request, string id, CancellationToken cancellationToken = default)
        {
            if (request.Data == null)
                return new EndpointResponse<IReadOnlyList<UnifiedEventModel>> { Data = Array.Empty<UnifiedEventModel>() };

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

            return new EndpointResponse<IReadOnlyList<UnifiedEventModel>> { Data = new List<UnifiedEventModel> { request.Data } };
        }

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
