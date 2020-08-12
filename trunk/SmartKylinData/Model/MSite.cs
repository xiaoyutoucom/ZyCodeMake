/*********************************************
* 命名空间:Smart.Core.Model
* 功 能： Led 实体
* 类 名： LedModel
* 作 者:  东腾
* 时 间： 2018/6/22 19:00:24 
**********************************************
*/using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SiteAPI.Data
{
    public class MSiteMap : EntityMap<MSite, string>
    {
        public MSiteMap():base("sy_site_info")
        {
            Id(x => x.Id);
            Map(x => x.SITE_NAME);
            Map(x => x.SITE_CODE);
            Map(x => x.SITE_TYPE);
            Map(x => x.NOTE);
        }
    }
    public class MSite : Entity<string>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        //public virtual int Id { get; set; }j
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string SITE_NAME { get; set; }
        /// <summary>
        /// 站点类型
        /// </summary>
        public virtual string SITE_TYPE { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string SITE_CODE { get; set; }
   
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string NOTE { get; set; }
        //站点类型名称
        //public virtual string SITE_TYPENAME { get; set; }
        ////经度
        //public virtual string LONGOTUDE { get; set; }
        //////纬度
        //public virtual string LATITUDE { get; set; }

    }
}
