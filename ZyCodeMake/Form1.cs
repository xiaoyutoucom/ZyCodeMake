using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace ZyCodeMake
{
    public partial class FormMain : Form
    {
        string filePath = System.IO.Directory.GetCurrentDirectory() + "\\OutFile";

        public FormMain()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;//设置边框为不可调节
            this.MaximizeBox = false;//取消最大化按键
            this.MinimizeBox = false;//取消最小化按键
        }

        //读取连接字符串
        private void btnRead_Click(object sender, EventArgs e)
        {
            string str_DbConnect = this.txtDbConnect.Text;
            if (string.IsNullOrEmpty(str_DbConnect))
            {
                MessageBox.Show("请填写连接字符串！");
                return;
            }

            //先清空
            this.clb_Tables.Items.Clear();

            NpgsqlConnection conn;
            try
            {
                conn = new NpgsqlConnection(str_DbConnect);
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败！ex：" + ex.Message.ToString());
                return;
            }
            using (conn)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("连接失败！ex：" + ex.Message.ToString());
                    return;
                }
                

                string sql_tables = @"
select relname 表名,cast(obj_description(relfilenode,'pg_class') as varchar) 名称 
  from pg_class
 where relname in (select tablename from pg_tables where schemaname='public' and position('_2' in tablename)=0)
 order by relname asc
";


                using (var cmd = new NpgsqlCommand(sql_tables, conn))
                using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    this.clb_Tables.Items.Add(reader["表名"].ToString() + "  |  " + reader["名称"].ToString());
            }

            //全选判断
            chkAllTablesFun();
        }

        //生成代码
        private void btnCodeMakeAll_Click(object sender, EventArgs e)
        {
            //判断
            if (this.clb_Tables.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择表！");
                return;
            }
            if (string.IsNullOrEmpty(this.txtNameSpace.Text.ToString().Trim()))
            {
                MessageBox.Show("请填写命名空间！");
                return;
            }

            //连接字符串
            string str_DbConnect = this.txtDbConnect.Text;
            //命名空间
            string strNameSpace = this.txtNameSpace.Text.ToString().Trim();
            //选择的表
            string[] arrTables = new string[this.clb_Tables.CheckedItems.Count];
            this.clb_Tables.CheckedItems.CopyTo(arrTables, 0);
            
            //sql语句
            string sql_select = @"
SELECT col_description(a.attrelid,a.attnum) as ColComment,
				format_type(a.atttypid,a.atttypmod) as ColType,
				a.attname as ColName, 
				a.attnotnull as ColNotNull   
	FROM pg_class as c,pg_attribute as a 
 WHERE c.relname = '{0}' 
   and a.attrelid = c.oid 
   and a.attnum>0
   and a.attisdropped = false
";
            string sql_PK_select = @"
select pg_attribute.attname as colname,pg_type.typname as typename,pg_constraint.conname as pk_name from 
pg_constraint  inner join pg_class 
on pg_constraint.conrelid = pg_class.oid 
inner join pg_attribute on pg_attribute.attrelid = pg_class.oid 
and  pg_attribute.attnum = pg_constraint.conkey[1]
inner join pg_type on pg_type.oid = pg_attribute.atttypid
where pg_class.relname = '{0}' 
and pg_constraint.contype='p'
";

            //最终表信息
            List<TableInfo> list = new List<TableInfo>();

            for (int i = 0; i < arrTables.Length; i++)
            {
                TableInfo model = new TableInfo()
                {
                    TableName = arrTables[i].Split('|')[0].Trim(),
                    TableComment = arrTables[i].Split('|')[1].Trim(),
                    List_TableFieldInfo = new List<TableFieldInfo>()
                };
                string sql = string.Format(sql_select, model.TableName);
                string sql_PK = string.Format(sql_PK_select, model.TableName);


                using (var conn = new NpgsqlConnection(str_DbConnect))
                {
                    conn.Open();

                    //读出所有字段
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    using (var dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        TableFieldInfo fieldModel = new TableFieldInfo();
                        fieldModel.ColComment = ObjToString(dr["ColComment"]);
                        fieldModel.ColType = ObjToString(dr["ColType"]);
                        fieldModel.ColName = ObjToString(dr["ColName"]);
                        fieldModel.ColNotNull = ObjToString(dr["ColNotNull"]);

                        model.List_TableFieldInfo.Add(fieldModel);
                    }
                    //找到主键
                    using (var cmd = new NpgsqlCommand(sql_PK, conn))
                    using (var dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        model.TablePK = ObjToString(dr["colname"]);
                    }
                }
                list.Add(model);
            }

            //代码生成方法
            CodeMakeFun(list, strNameSpace);

            MessageBox.Show("生成成功！");
        }

        //代码生成方法
        private void CodeMakeFun(List<TableInfo> list, string strNameSpace)
        {
            foreach (TableInfo item in list)
            {
                //1.生成Model
                if (this.chkModel.Checked)
                    Model_Make(item, strNameSpace);


                //2.生成Repository
                if (this.chkRepository.Checked)
                    Repository_Make(item, strNameSpace);

                //3.生成Interface
                if (this.chkInterface.Checked)
                    InterFace_Make(item, strNameSpace);
            }
        }

        private void Model_Make(TableInfo model, string strNameSpace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(@"/******************************************
* 模块名称：Model-{0}
* 当前版本：1.0
* 生成时间：{1}
* 版本历史：此代码由正元地理信息代码生成工具自动生成。
* 
******************************************/
using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;", 
ToUppperFirst(model.TableName), DateTime.Now.ToString("yyyy/MM/dd")));

            sb.AppendLine("namespace " + strNameSpace);
            sb.AppendLine("{");
            sb.AppendLine("\tpublic class "+ ToUppperFirst(model.TableName) + "Map : EntityMap<"+ ToUppperFirst(model.TableName) + ", string>");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tpublic " + ToUppperFirst(model.TableName) + "Map() : base(\""+ model.TableName + "\")");
            sb.AppendLine("\t\t{");
            foreach (TableFieldInfo item in model.List_TableFieldInfo)
            {
                //主键
                if (item.ColName.ToLower() == ObjToString(model.TablePK).ToLower())
                {
                    sb.AppendLine("\t\t\tId(x => x."+ item.ColName.ToUpper() + ");");
                }
                else
                {
                    sb.AppendLine("\t\t\tMap(x => x." + item.ColName.ToUpper() + ");");
                }
            }
            
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            sb.AppendLine("\tpublic class "+ ToUppperFirst(model.TableName) + " : Entity<string>");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\t#region 公共属性");
            foreach (TableFieldInfo item in model.List_TableFieldInfo)
            {
                //主键
                if (item.ColName.ToLower() == ObjToString(model.TablePK).ToLower())
                {
                    continue;
                }

                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine("\t\t/// " + item.ColComment.Replace("\r\n", "") + "");
                sb.AppendLine("\t\t/// </summary>");
                if (item.ColType.ToLower().Contains("character"))
                {
                    sb.AppendLine("\t\tpublic virtual string " + item.ColName.ToUpper() + " { get; set; }");
                }
                else if (item.ColType.ToLower().Contains("numeric"))
                {
                    sb.AppendLine("\t\tpublic virtual Nullable<decimal> " + item.ColName.ToUpper() + " { get; set; }");
                }
                else
                {
                    sb.AppendLine("\t\tpublic virtual string " + item.ColName.ToUpper() + " { get; set; }");
                }
            }
            sb.AppendLine("\t\t#endregion");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
           
            FileCreate(ToUppperFirst(model.TableName) + ".cs", sb.ToString());
        }

        private void Repository_Make(TableInfo model, string strNameSpace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(@"/******************************************
* 模块名称：DataAccess-{0}
* 当前版本：1.0
* 生成时间：{1}
* 版本历史：此代码由正元地理信息代码生成工具自动生成。
* 
******************************************/
using NHibernate;
using Robin.NHibernate;
using Robin.NHibernate.Repositories;",
ToUppperFirst(model.TableName)+ "Repository", DateTime.Now.ToString("yyyy/MM/dd")));

            sb.AppendLine("namespace " + strNameSpace);
            sb.AppendLine("{");
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// " + (string.IsNullOrEmpty(model.TableComment) ? model.TableName : model.TableComment) + "仓储");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine("\tpublic class " + ToUppperFirst(model.TableName) + "Repository : NhRepositoryBase<" + ToUppperFirst(model.TableName) + ", string>, I" + ToUppperFirst(model.TableName) + "Repository");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tpublic "+ ToUppperFirst(model.TableName) + "Repository(ISessionProvider sessionProvider): base(sessionProvider)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            FileCreate(ToUppperFirst(model.TableName) + "Repository.cs", sb.ToString());
        }

        private void InterFace_Make(TableInfo model, string strNameSpace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(@"/******************************************
* 模块名称：Core-{0}
* 当前版本：1.0
* 生成时间：{1}
* 版本历史：此代码由正元地理信息代码生成工具自动生成。
* 
******************************************/
using Robin.Domain.Repositories;",
"I" + ToUppperFirst(model.TableName) + "Repository", DateTime.Now.ToString("yyyy/MM/dd")));

            sb.AppendLine("namespace " + strNameSpace);
            sb.AppendLine("{");
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// " + (string.IsNullOrEmpty(model.TableComment) ? model.TableName : model.TableComment) + "仓储");
            sb.AppendLine("\t/// </summary>");

            sb.AppendLine("\tpublic interface I"+ ToUppperFirst(model.TableName) + "Repository : IRepository<"+ ToUppperFirst(model.TableName) + ", string>");
            sb.AppendLine("\t{");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            FileCreate("I" + ToUppperFirst(model.TableName) + "Repository.cs", sb.ToString());
        }

        //首字母大写
        private string ToUppperFirst(string s)
        {
            if (string.IsNullOrEmpty(s) || s.Length < 2)
                return s;
            
            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }

        //对象转string
        private string ObjToString(object obj)
        {
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }

        //创建文件
        private void FileCreate(string FileName, string strContent)
        {
            if (System.IO.Directory.Exists(filePath) == false)
            {
                System.IO.Directory.CreateDirectory(filePath);
            }

            File.WriteAllText(filePath + "\\" + FileName, strContent);
        }

        //打开文件夹
        private void btnOpenPath_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", filePath);
        }

        //设置全选操作
        private void chkAllTables_CheckedChanged(object sender, EventArgs e)
        {
            chkAllTablesFun();
        }
        //设置全选方法
        private void chkAllTablesFun()
        {
            bool checkFlag = false;
            if (this.chkAllTables.Checked)
                checkFlag = true;
            else
                checkFlag = false;

            for (int j = 0; j < this.clb_Tables.Items.Count; j++)
                this.clb_Tables.SetItemChecked(j, checkFlag);
        }
    }

    public class TableInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表名备注
        /// </summary>
        public string TableComment { get; set; }

        /// <summary>
        /// 表主键
        /// </summary>
        public string TablePK { get; set; }

        /// <summary>
        /// 每列
        /// </summary>
        public List<TableFieldInfo> List_TableFieldInfo { get; set; }
    }


    public class TableFieldInfo
    {
        /// <summary>
        /// 字段备注
        /// </summary>
        public string ColComment { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string ColType { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// 字段是否为null
        /// </summary>
        public string ColNotNull { get; set; }
    }
}
