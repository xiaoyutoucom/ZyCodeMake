using Robin.NHibernate;
using Robin.NHibernate.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SiteAPI.Data
{
    /// <summary>
    /// 人员信息表仓储
    /// </summary>
    public class DataRepository : NhRepositoryBase<MData, string>, IDataRepository
    {
        public ISessionProvider sessionProvider { get; set; }
        public DataRepository(ISessionProvider sessionProvider): base(sessionProvider)
        {
            this.sessionProvider = sessionProvider;

        }
        public MData GetTest(Expression<Func<MData, bool>> predicate)
        {
            var res = this.sessionProvider.Session.Query<MData>().Where(predicate).OrderByDescending(a=>a.COLL_TIME).FirstOrDefault();
            return res;
        }

    }
}
