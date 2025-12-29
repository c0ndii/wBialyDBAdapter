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

        private readonly IBaseRepository<NoSql.Tag> _mongoTagRepo;
        private readonly IRelationalRepository<Rel.Tag_Gastro> _relTagRepo; 
        private readonly IObjectRelationalRepository<Obj.Tag_Gastro> _objTagRepo;

        public GastroService(
            IGastroMapper mapper,
            IBaseRepository<NoSql.Gastro> mongoRepo,
            IRelationalRepository<Rel.Gastro> relRepo,
            IObjectRelationalRepository<Obj.Gastro> objRepo,
            IBaseRepository<NoSql.Tag> mongoTagRepo,
            IRelationalRepository<Rel.Tag_Gastro> relTagRepo,
            IObjectRelationalRepository<Obj.Tag_Gastro> objTagRepo)
        {
            _mapper = mapper;
            _mongoRepo = mongoRepo;
            _relRepo = relRepo;
            _objRepo = objRepo;
            _mongoTagRepo = mongoTagRepo;
            _relTagRepo = relTagRepo;
            _objTagRepo = objTagRepo;
        }

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

        public async Task<EndpointResponse<IReadOnlyList<UnifiedGastroModel>>> AddAsync(PostRequest<UnifiedGastroModel> request, CancellationToken cancellationToken = default)
        {
            if (request.Data == null)
                return new EndpointResponse<IReadOnlyList<UnifiedGastroModel>> { Data = Array.Empty<UnifiedGastroModel>() };

            var unifiedTags = request.Data.Tags ?? new List<UnifiedTagModel>();

            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    var noSqlEntity = _mapper.ToNoSql(request.Data);
                    noSqlEntity.Tags = unifiedTags.Select(t => new NoSql.Tag { Id = t.Id, Name = t.Name }).ToList();
                    await _mongoRepo.AddAsync(noSqlEntity);
                    request.Data.Id = noSqlEntity.Id?.ToString();
                    break;

                case DatabaseType.Relational:
                    var relEntity = _mapper.ToRelational(request.Data);
                    relEntity.GastroTags = unifiedTags.Select(t => new Rel.Tag_Gastro { Name = t.Name }).ToList();
                    var postId = await _relRepo.CreateAsync(relEntity);
                    request.Data.Id = postId.ToString();
                    break;

                case DatabaseType.ObjectRelational:
                    var objEntity = _mapper.ToObjectRelational(request.Data);
                    objEntity.GastroTags = unifiedTags.Select(t => new Obj.Tag_Gastro { Name = t.Name }).ToList();
                    await _objRepo.CreateAsync(objEntity);
                    request.Data.Id = objEntity.PostId.ToString();
                    break;
            }

            return new EndpointResponse<IReadOnlyList<UnifiedGastroModel>> { Data = new List<UnifiedGastroModel> { request.Data } };
        }

        public async Task<EndpointResponse<IReadOnlyList<UnifiedGastroModel>>> UpdateAsync(PostRequest<UnifiedGastroModel> request, string id, CancellationToken cancellationToken = default)
        {
            if (request.Data == null)
                return new EndpointResponse<IReadOnlyList<UnifiedGastroModel>> { Data = Array.Empty<UnifiedGastroModel>() };

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
                    relEntity.GastroTags = unifiedTags.Select(t => new Rel.Tag_Gastro { Name = t.Name }).ToList();
                    await _relRepo.UpdateAsync(relEntity);
                    break;

                case DatabaseType.ObjectRelational:
                    var objEntity = _mapper.ToObjectRelational(request.Data);
                    objEntity.GastroTags = unifiedTags.Select(t => new Obj.Tag_Gastro { Name = t.Name }).ToList();
                    await _objRepo.UpdateAsync(int.Parse(id), objEntity);
                    break;
            }

            return new EndpointResponse<IReadOnlyList<UnifiedGastroModel>> { Data = new List<UnifiedGastroModel> { request.Data } };
        }

        // DELETE (Bez zmian)
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