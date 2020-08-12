using Robin.NHibernate;
using Robin.NHibernate.Repositories;
namespace SiteAPI.Data
{
    /// <summary>
    /// 人员信息表仓储
    /// </summary>
    public class DataHisRepository : NhRepositoryBase<MDataHis, string>,IDataHisRepository
    {
        public ISessionProvider sessionProvider { get; set; }
        public DataHisRepository(ISessionProvider sessionProvider): base(sessionProvider)
        {
            this.sessionProvider = sessionProvider;

        }

       
    }
}
