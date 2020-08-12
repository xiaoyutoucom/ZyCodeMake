using Robin.NHibernate;
using Robin.NHibernate.Repositories;
using System.Collections.Generic;
using System.Linq;
namespace SiteAPI.Data
{
    /// <summary>
    /// 人员信息表仓储
    /// </summary>
    public class SensorRepository : NhRepositoryBase<MSensor, string>, ISensorRepository
    {
        public ISessionProvider sessionProvider { get; set; }
        public SensorRepository(ISessionProvider sessionProvider): base(sessionProvider)
        {
            this.sessionProvider = sessionProvider;

        }

       
       
    }
}
