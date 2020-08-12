using Robin.Domain.Repositories;
using Robin.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SiteAPI.Data
{
    /// <summary>
    /// 人员信息表仓储
    /// </summary>
    public interface IDataRepository : IRepository<MData, string>
    {
        MData GetTest(Expression<Func<MData, bool>> predicate);
    }
}
