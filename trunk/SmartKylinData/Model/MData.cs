/*********************************************
* 命名空间:Smart.Core.Model
* 功 能： Led 实体
* 类 名： LedModel
* 作 者:  东腾
* 时 间： 2018/6/22 19:00:24 
**********************************************
*/using System;
using System.Linq;
using System.Text.RegularExpressions;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SiteAPI.Data
{
    public class MDataMap : EntityMap<MData, string>
    {
        public MDataMap()
            : base("sersor_coll_data")
        {
            Id(x => x.Id);
            //References(o => o.SENSOR_CODE, "SENSOR_CODE").NotFound.Ignore().LazyLoad().Column("SENSOR_CODE");
            Map(x => x.SENSOR_CODE);
            Map(x => x.COLL_TAG_NAME);
            Map(x => x.COLL_TIME);
            Map(x => x.COLL_VALUE);
        }
    }
    public class MData : Entity<string>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        //public virtual int Id { get; set; }
        /// <summary>
        /// 传感器编号
        /// </summary>
        public virtual string SENSOR_CODE { get; set; }
        /// <summary>
        /// 数据名称
        /// </summary>
        public virtual string COLL_TAG_NAME { get; set; }
        /// <summary>
        /// 数据时间
        /// </summary>
        public virtual string COLL_TIME { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public virtual string COLL_VALUE { get; set; }
    }
}
