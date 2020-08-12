/*********************************************
* 命名空间:SmartKylinApp.Common
* 功 能： 全局帮助类
* 类 名： GlobalHandler
* 作 者:  东腾
* 时 间： 2018-08-08 22:15:28 
**********************************************
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Robin;
using SiteAPI.Data;

namespace SmartKylinApp.Common
{
    public class GlobalHandler : ConfigHelp
    {

        public static RobinBootstrapper Bootstrapper;

        //字典信息
        public static IDataRepository dataresp => Bootstrapper.IocManager.Resolve<IDataRepository>();
        //字典信息
        public static ISensorRepository senresp => Bootstrapper.IocManager.Resolve<ISensorRepository>();
        //字典信息
        public static ISiteRepository siteresp => Bootstrapper.IocManager.Resolve<ISiteRepository>();

    }
}