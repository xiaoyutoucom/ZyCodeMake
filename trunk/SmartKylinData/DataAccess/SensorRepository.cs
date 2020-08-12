using Robin.NHibernate;
using Robin.NHibernate.Repositories;
using System.Collections.Generic;
using System.Linq;
namespace SiteAPI.Data
{
    /// <summary>
    /// ��Ա��Ϣ��ִ�
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
