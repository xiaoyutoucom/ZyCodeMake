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
    public class MSensorMap : EntityMap<MSensor, string>
    {
        public MSensorMap()
            : base("sy_sensor_info")
        {
            Id(x => x.Id);
            Map(x => x.SENSOR_NAME);
            Map(x => x.SENSOR_CODE);
            Map(x => x.SENSOR_TYPE_CODE);
            //Map(x => x.SITE_ID);
            Map(x => x.REMARK);
            Map(x => x.SENSOR_UNIT);
            References(o => o.SITE_ID).NotFound.Ignore().LazyLoad().Column("SITE_ID");
        }
    }
    public class MSensor : Entity<string>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        //public virtual int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string SENSOR_NAME { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string SENSOR_CODE { get; set; }
        /// <summary>
        /// 传感器类型
        /// </summary>
        public virtual string SENSOR_TYPE_CODE { get; set; }
        /// <summary>
        /// 关联站点ID
        /// </summary>
        //public virtual string SITE_ID { get; set; }
        /// <summary>
        /// 关联站点ID
        /// </summary>
        public virtual MSite SITE_ID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string REMARK { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public virtual string SENSOR_UNIT { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public virtual string SENSOR_UNITNAME { get; set; }
        /// <summary>
        /// 传感器类型名称
        /// </summary>
        public virtual string SENSOR_TYPE_CODENAME { get; set; }
    }
}
