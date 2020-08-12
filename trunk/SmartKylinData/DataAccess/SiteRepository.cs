using Robin.NHibernate;
using Robin.NHibernate.Repositories;
namespace SiteAPI.Data
{
    /// <summary>
    /// 人员信息表仓储
    /// </summary>
    public class SiteRepository : NhRepositoryBase<MSite, string>, ISiteRepository
    {
        public ISessionProvider sessionProvider { get; set; }
        public SiteRepository(ISessionProvider sessionProvider): base(sessionProvider)
        {
            this.sessionProvider = sessionProvider;

        }

       
    }
}
