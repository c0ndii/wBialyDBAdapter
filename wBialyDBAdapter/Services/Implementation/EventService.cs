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
    public class EventService : IQueryService<UnifiedEventModel>
    {
        private readonly IEventMapper _mapper;

        // Główne repozytoria
        private readonly IBaseRepository<NoSql.Event> _mongoRepo;
        private readonly IRelationalRepository<Rel.Event> _relRepo;
        private readonly IObjectRelationalRepository<Obj.Event> _objRepo;

        // Repozytoria Tagów (do pobierania nazw po ID)
        private readonly IBaseRepository<NoSql.Tag> _mongoTagRepo;
        private readonly IRelationalRepository<Rel.Tag_Event> _relTagRepo;
        private readonly IObjectRelationalRepository<Obj.Tag_Event> _objTagRepo;

        public EventService(
            IEventMapper mapper,
            IBaseRepository<NoSql.Event> mongoRepo,
            IRelationalRepository<Rel.Event> relRepo,
            IObjectRelationalRepository<Obj.Event> objRepo,
            IBaseRepository<NoSql.Tag> mongoTagRepo,
            IRelationalRepository<Rel.Tag_Event> relTagRepo,
            IObjectRelationalRepository<Obj.Tag_Event> objTagRepo)
        {
            _mapper = mapper;
            _mongoRepo = mongoRepo;
            _relRepo = relRepo;
            _objRepo = objRepo;
            _mongoTagRepo = mongoTagRepo;
            _relTagRepo = relTagRepo;
            _objTagRepo = objTagRepo;
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

        // GET MANY
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

        // ADD
        public async Task<EndpointResponse<IReadOnlyList<UnifiedEventModel>>> AddAsync(PostRequest<UnifiedEventModel> request, CancellationToken cancellationToken = default)
        {
            if (request.Data == null)
                return new EndpointResponse<IReadOnlyList<UnifiedEventModel>> { Data = Array.Empty<UnifiedEventModel>() };

            var unifiedTags = request.Data.Tags ?? new List<UnifiedTagModel>();

            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    var noSqlEntity = _mapper.ToNoSql(request.Data);
                    noSqlEntity.Tags = unifiedTags.Select(t => new NoSql.Tag { Id = t.Id, Name = t.Name }).ToList();
                    await _mongoRepo.AddAsync(noSqlEntity);
                    break;

                case DatabaseType.Relational:
                    var relEntity = _mapper.ToRelational(request.Data);
                    relEntity.EventTags = unifiedTags.Select(t => new Rel.Tag_Event { Name = t.Name }).ToList();
                    await _relRepo.CreateAsync(relEntity);
                    break;

                case DatabaseType.ObjectRelational:
                    var objEntity = _mapper.ToObjectRelational(request.Data);
                    objEntity.EventTags = unifiedTags.Select(t => new Obj.Tag_Event { Name = t.Name }).ToList();
                    await _objRepo.CreateAsync(objEntity);
                    break;
            }

            return new EndpointResponse<IReadOnlyList<UnifiedEventModel>> { Data = new List<UnifiedEventModel> { request.Data } };
        }

        // UPDATE
        public async Task<EndpointResponse<IReadOnlyList<UnifiedEventModel>>> UpdateAsync(PostRequest<UnifiedEventModel> request, string id, CancellationToken cancellationToken = default)
        {
            if (request.Data == null)
                return new EndpointResponse<IReadOnlyList<UnifiedEventModel>> { Data = Array.Empty<UnifiedEventModel>() };

            var unifiedTags = request.Data.Tags ?? new List<UnifiedTagModel>();

            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    var noSqlEntity = _mapper.ToNoSql(request.Data);
                    noSqlEntity.Tags = unifiedTags.Select(t => new NoSql.Tag { Id = t.Id, Name = t.Name }).ToList();
                    await _mongoRepo.UpdateAsync(id, noSqlEntity);
                    break;

                case DatabaseType.Relational:
                    var relEntity = _mapper.ToRelational(request.Data);
                    relEntity.EventTags = unifiedTags.Select(t => new Rel.Tag_Event { Name = t.Name }).ToList();
                    await _relRepo.UpdateAsync(relEntity);
                    break;

                case DatabaseType.ObjectRelational:
                    var objEntity = _mapper.ToObjectRelational(request.Data);
                    objEntity.EventTags = unifiedTags.Select(t => new Obj.Tag_Event { Name = t.Name }).ToList();
                    await _objRepo.UpdateAsync(int.Parse(id), objEntity);
                    break;
            }

            return new EndpointResponse<IReadOnlyList<UnifiedEventModel>> { Data = new List<UnifiedEventModel> { request.Data } };
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