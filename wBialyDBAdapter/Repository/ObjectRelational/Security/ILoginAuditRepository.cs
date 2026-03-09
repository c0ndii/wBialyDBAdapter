using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Repository.ObjectRelational.Security
{
    public interface ILoginAuditRepository
    {
        Task<IEnumerable<LoginAuditItemDto>> GetLoginAudits(int? userId, bool includeAll, int limit);
    }
}
