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
    public class TagService : ITagService
    {
        private readonly ITagMapper _mapper;

        private readonly IBaseRepository<NoSql.Tag> _mongoRepo;

        private readonly IRelationalRepository<Rel.Tag_Event> _relEventTagRepo;
        private readonly IRelationalRepository<Rel.Tag_Gastro> _relGastroTagRepo;

        private readonly IObjectRelationalRepository<Obj.Tag_Event> _objEventTagRepo;
        private readonly IObjectRelationalRepository<Obj.Tag_Gastro> _objGastroTagRepo;

        public TagService(
            ITagMapper mapper,
            IBaseRepository<NoSql.Tag> mongoRepo,
            IRelationalRepository<Rel.Tag_Event> relEventTagRepo,
            IRelationalRepository<Rel.Tag_Gastro> relGastroTagRepo,
            IObjectRelationalRepository<Obj.Tag_Event> objEventTagRepo,
            IObjectRelationalRepository<Obj.Tag_Gastro> objGastroTagRepo)
        {
            _mapper = mapper;
            _mongoRepo = mongoRepo;
            _relEventTagRepo = relEventTagRepo;
            _relGastroTagRepo = relGastroTagRepo;
            _objEventTagRepo = objEventTagRepo;
            _objGastroTagRepo = objGastroTagRepo;
        }

        public async Task<EndpointResponse<IReadOnlyList<UnifiedTagModel>>> GetAllAsync(EndpointRequest request, CancellationToken cancellationToken = default)
        {
            List<UnifiedTagModel> allTags = new List<UnifiedTagModel>();

            switch (request.DatabaseType)
            {
                case DatabaseType.NoSQL:
                    var noSqlTags = await _mongoRepo.GetManyAsync(request);
                    allTags.AddRange(noSqlTags.Select(_mapper.FromNoSql));
                    break;

                case DatabaseType.Relational:
                    var relEventTags = await _relEventTagRepo.GetAllAsync();
                    var relGastroTags = await _relGastroTagRepo.GetAllAsync();

                    allTags.AddRange(relEventTags.Select(_mapper.FromRelationalEvent));
                    allTags.AddRange(relGastroTags.Select(_mapper.FromRelationalGastro));
                    break;

                case DatabaseType.ObjectRelational:
                    var objEventTags = await _objEventTagRepo.GetAllAsync();
                    var objGastroTags = await _objGastroTagRepo.GetAllAsync();

                    allTags.AddRange(objEventTags.Select(_mapper.FromObjectRelationalEvent));
                    allTags.AddRange(objGastroTags.Select(_mapper.FromObjectRelationalGastro));
                    break;
            }

            var uniqueTags = allTags
                .GroupBy(t => t.Name)
                .Select(g => g.First())
                .ToList();

            return new EndpointResponse<IReadOnlyList<UnifiedTagModel>> { Data = uniqueTags };
        }
    }
}