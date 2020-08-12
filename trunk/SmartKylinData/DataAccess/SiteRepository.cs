using Robin.NHibernate;
using Robin.NHibernate.Repositories;
namespace SiteAPI.Data
{
    /// <summary>
    /// ��Ա��Ϣ��ִ�
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
