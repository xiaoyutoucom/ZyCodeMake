using Robin.Domain.Repositories;
using Robin.NHibernate;

namespace SiteAPI.Data
{
    /// <summary>
    /// 人员信息表仓储
    /// </summary>
    public interface IDataHisRepository : IRepository<MDataHis, string>
    {
          
    }
}
