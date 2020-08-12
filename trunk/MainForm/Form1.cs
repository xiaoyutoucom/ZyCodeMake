
using System;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft;
using System.Threading;
using SmartKylinApp.Module;
using SmartKylinApp.Common;
using Robin;
using System.Configuration;
using System.Linq;
using SiteAPI.Data;
using System.Timers;
using HslCommunication.LogNet;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using NPOI.SS.Util;

using IOTGateway.Core.Caching;
using IOTGateway.Core.MessageBus;

namespace SendMessage
{
    public partial class Form1 : Form
    {
        private Thread t;
        private static ManualResetEvent manual = new ManualResetEvent(false);
        private List<ENUM> listENUM = new List<ENUM>();
        private List<COORDINATE> listCOOR = new List<COORDINATE>();

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            loadData();
        }

       

        private void btn_SendMsg_Click(object sender, EventArgs e)
        {
            btn_SendMsg.Enabled = false;
            btn_StopSend.Enabled = true;
            StartSend();
        }


        private void StartSend()
        {
            var json = Environment.CurrentDirectory + "\\appsettings.json";
            var build = new ConfigurationBuilder()
                .AddJsonFile(json)
                .Build();
            var type = build["Application:SendTime"];
            timer = new System.Timers.Timer();
            //timer.AutoReset = true;
            if (String.IsNullOrEmpty(type))
            {
                type = "1";
            }
            timer.Interval = 60000*Convert.ToInt32(type);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

            timer.Start();


        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Send();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Send();
        }
        //初始化数据
        private void Send()
        {
            try
            {

                var sens = GlobalHandler.senresp.GetAllList();
                //var datas = GlobalHandler.dataresp.GetAllList();
                int count = sens.Count;
                int sendnum = 0;
                for (int i = 0; i < sens.Count; i++)
                {
                    var datas = GlobalHandler.dataresp.GetTest(a => a.SENSOR_CODE == sens[i].SENSOR_CODE);
                    try
                    {
                        if (datas != null)
                        {
                            var datadate = DateTime.Parse(datas.COLL_TIME).ToString("yyyy-MM-dd HH:mm:ss");
                            SendMsg(sens[i].SITE_ID.SITE_CODE.Replace("_", ""), sens[i].SENSOR_CODE, datas.COLL_VALUE, datadate);
                            sendnum++;
                        }
                        //break;

                    }
                    catch (Exception ex)
                    {
                        //XtraMessageBox.Show("redis发送失败！"+ex.ToString());
                    }
                    if (listBox1.Items.Count > 20)
                    {
                        listBox1.Items.Clear();
                    }


                }
                listBox1.Items.Add(DateTime.Now.ToString() + " 推送成功" + sendnum + "条数据！");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(@"推送失败！\n" + ex.Message);
                if (listBox1.Items.Count > 20)
                {
                    listBox1.Items.Clear();
                }
                listBox1.Items.Add(DateTime.Now.ToString() + " 推送失败！");

            }
        }


        //初始化数据
        private void loadData()
        {
            var boot = RobinBootstrapper.Create<BootstrapMoudle>();
            boot.Initialize();
            GlobalHandler.Bootstrapper = boot;
        }
        private void SendMsg(string id, string tag, string tagval, string time)
        {
            string channel = ConfigurationManager.AppSettings["IOTGatewayMQAddress"].ToString();
            RedisConnectionProvider redisConnectionProvider = new RedisConnectionProvider();
            RedisMessageBusBroker redisMessageBusBroker = new RedisMessageBusBroker(redisConnectionProvider);
            redisMessageBusBroker.Publish(channel, id.Replace("_", "") + "&" + tag + "&" + time + "&" + tagval);
        }
        //private void SendMsg2(string id, string tag, string tagval, string time)
        //{
        //    LogNetDateTime pushLogNet;
        //    ///数据推送方法
        //    RedisMessageBusBroker _bus;
        //    //推送通道
        //    string Channel;
        //    //日志
        //    pushLogNet = new LogNetDateTime(AppDomain.CurrentDomain.BaseDirectory + "\\DataPushLogs", HslCommunication.LogNet.GenerateMode.ByEveryDay);
        //    ///数据推送方法
        //    _bus = new RedisMessageBusBroker(new RedisConnectionProvider(), pushLogNet);
        //    Channel = ConfigurationManager.AppSettings["IOTGatewayMQAddress"].ToString();
        //    //id = "EQGBSC";
        //    //tag = "5513";
        //    //XtraMessageBox.Show(id.Replace("_", "") + "&" + tag+time + "&" + tagval);
        //    _bus.Publish(Channel, id.Replace("_", "") + "&" + tag + "&" + time + "&" + tagval);
        //}
        private void btn_StopSend_Click(object sender, EventArgs e)
        {
           
            btn_StopSend.Enabled = false;
            btn_SendMsg.Enabled = true;
            timer.Stop();
            if (t != null)
            {
                manual.Reset();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            FileStream fs = null;
            try
            {
                var saveFileName = "传感器信息导入模板.xls";
                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                //(Optional) set the width of the columns
                sheet.SetColumnWidth(0, 20 * 256);
                sheet.SetColumnWidth(1, 40 * 256);
                sheet.SetColumnWidth(2, 30 * 256);
                sheet.SetColumnWidth(3, 30 * 256);
                sheet.SetColumnWidth(4, 30 * 256);
                sheet.SetColumnWidth(5, 30 * 256);
                sheet.SetColumnWidth(6, 30 * 256);
                sheet.SetColumnWidth(7, 30 * 256);


                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("设备名称");
                headerRow.CreateCell(1).SetCellValue("设备编码（必填）");
                headerRow.CreateCell(2).SetCellValue("传感器编码（必填）");
                headerRow.CreateCell(3).SetCellValue("传感器名称（必填）");
                headerRow.CreateCell(4).SetCellValue("设备类型");
                headerRow.CreateCell(5).SetCellValue("传感器类型");
                headerRow.CreateCell(6).SetCellValue("传感器类型名称");
                headerRow.CreateCell(7).SetCellValue("备注信息");
                headerRow.CreateCell(8).SetCellValue("监测点编码");
                headerRow.CreateCell(9).SetCellValue("监测项单位");
                headerRow.CreateCell(10).SetCellValue("监测项单位bm");
                sheet.CreateFreezePane(0, 1, 0, 1);
                //获取选中的监测点信息
                var Sensors = GlobalHandler.senresp.GetAllList().OrderBy(s=>s.SITE_ID.SITE_TYPE);
                //var sites = GlobalHandler.siteresp.GetAllList();
                if (listENUM.Count < 1)
                {
                    GetAllENUM();
                }
                if (listCOOR.Count < 1)
                {
                    GetAllCOORDINATE();
                }
                int i = 0;
                foreach (var Sensor in Sensors)
                {

                    ENUM en = listENUM.Find(a => a.code == Sensor.SENSOR_UNIT);
                    if (en != null)
                    {
                        Sensor.SENSOR_UNITNAME = en.name;
                    }
                    //传感器类型编码赋值
                    ENUM senserpen = listENUM.FirstOrDefault(a => a.code == "SENSOR_TYPE");
                    ENUM senseren = listENUM.Where(a => a.parent_id == senserpen.id).FirstOrDefault(a => a.code == Sensor.SENSOR_TYPE_CODE);
                    Sensor.SENSOR_TYPE_CODENAME = senseren.name;

                    var row = sheet.CreateRow(i + 1);


                    if (Sensor.SITE_ID != null)
                    {
                        row.CreateCell(0).SetCellValue(Sensor.SITE_ID.SITE_NAME);
                        row.CreateCell(1).SetCellValue(Sensor.SITE_ID.SITE_CODE.Replace("_", ""));
                        //传感器类型 传感器类型名称 备注信息
                        //1   压力  030304_003  030201_002
                        //2   累计流量    030304_004
                        //3   瞬时流量    030304_005
                        //4   浊度  030304_006
                        //5   余氯  030304_007
                        //6   PH  030304_008
                        //10  水位  030304_009
                        //27  电导率 030301_001
                        var a = Sensor.SENSOR_TYPE_CODE;
                        var b = Sensor.SITE_ID.SITE_TYPE;
                        if (Sensor.SITE_ID.SITE_TYPE == "1"|| Sensor.SITE_ID.SITE_TYPE == "030201")
                        {
                            Sensor.SITE_ID.SITE_TYPE = "030201";
                            if (Sensor.SENSOR_TYPE_CODE.Trim() == "1")
                            {
                                Sensor.SENSOR_TYPE_CODE = "030201_002";
                            }

                        }
                        else if (Sensor.SITE_ID.SITE_TYPE == "2" || Sensor.SITE_ID.SITE_TYPE == "030303" || Sensor.SITE_ID.SITE_TYPE == "030302")
                        {
                            var Ss = GlobalHandler.senresp.GetAllList(q => q.SITE_ID.Id == Sensor.SITE_ID.Id);
                            //site.SITE_TYPE = "030304";
                            var ty1 = Ss.Where(q => q.SENSOR_TYPE_CODE == "1" || q.SENSOR_TYPE_CODE == "10");
                            var ty2 = Ss.Where(q => q.SENSOR_TYPE_CODE=="2"|| q.SENSOR_TYPE_CODE == "3");
                            if (ty2.Count() > 0)
                            {
                                Sensor.SITE_ID.SITE_TYPE = "030303";
                                if (Sensor.SENSOR_TYPE_CODE.Trim() == "1")
                                {
                                    Sensor.SENSOR_TYPE_CODE = "030303_005";//压力
                                }
                                else if (Sensor.SENSOR_TYPE_CODE.Trim() == "10")
                                {
                                    Sensor.SENSOR_TYPE_CODE = "030303_004";//电压
                                }
                                else if (Sensor.SENSOR_TYPE_CODE.Trim() == "2")
                                {
                                    Sensor.SITE_ID.SITE_TYPE = "030303";
                                    Sensor.SENSOR_TYPE_CODE = "030303_002";//累计流量
                                }
                                else if (Sensor.SENSOR_TYPE_CODE.Trim() == "3")
                                {
                                    Sensor.SITE_ID.SITE_TYPE = "030303";
                                    Sensor.SENSOR_TYPE_CODE = "030303_003";//瞬时流量
                                }
                            }
                            else
                            {
                                Sensor.SITE_ID.SITE_TYPE = "030302";
                                if (Sensor.SENSOR_TYPE_CODE.Trim() == "1")
                                {
                                    Sensor.SENSOR_TYPE_CODE = "030302_005";//压力
                                }
                                else if (Sensor.SENSOR_TYPE_CODE.Trim() == "10")
                                {
                                    Sensor.SENSOR_TYPE_CODE = "030302_004";//电压
                                }
                            }
                        }
                        else if(Sensor.SITE_ID.SITE_TYPE == "7" || Sensor.SITE_ID.SITE_TYPE == "030301")
                        {
                            Sensor.SITE_ID.SITE_TYPE = "030301";
                            if (Sensor.SENSOR_TYPE_CODE.Trim() == "27")
                            {
                                Sensor.SENSOR_TYPE_CODE = "030301_001";
                            }
                            if (Sensor.SENSOR_TYPE_CODE.Trim() == "4")
                            {
                                Sensor.SENSOR_TYPE_CODE = "030301_002";
                            }
                            if (Sensor.SENSOR_TYPE_CODE.Trim() == "5")
                            {
                                Sensor.SENSOR_TYPE_CODE = "030301_003";
                            }
                            if (Sensor.SENSOR_TYPE_CODE.Trim() == "6")
                            {
                                Sensor.SENSOR_TYPE_CODE = "030301_004";
                            }
                            if (Sensor.SENSOR_TYPE_CODE.Trim() == "10")
                            {
                                Sensor.SENSOR_TYPE_CODE = "030301_005";
                            }
                        }
                        string bh = string.Empty;
                        var code = Sensor.SITE_ID.SITE_CODE.Replace("_", "");
                       
                        bh = "341602" + Sensor.SITE_ID.SITE_TYPE + code;
                        for (int j = 0; j < 20 - bh.Length; j++)
                        {
                            bh = bh + "X";
                        }
                        if (Sensor.SITE_ID.SITE_TYPE == "7")
                        {
                            bh = bh + "X";
                        }
                        row.CreateCell(4).SetCellValue(Sensor.SITE_ID.SITE_TYPE);
                        row.CreateCell(5).SetCellValue(Sensor.SENSOR_TYPE_CODE);
                        row.CreateCell(6).SetCellValue(Sensor.SENSOR_TYPE_CODENAME);
                        row.CreateCell(7).SetCellValue(a);
                        row.CreateCell(8).SetCellValue(bh);
                        row.CreateCell(9).SetCellValue(Sensor.SENSOR_UNITNAME);
                        row.CreateCell(10).SetCellValue(Sensor.SENSOR_UNIT);
                    }
                    row.CreateCell(2).SetCellValue(Sensor.SENSOR_CODE);
                    row.CreateCell(3).SetCellValue(Sensor.SENSOR_NAME);
                    i++;
                }
                var saveDialog = new SaveFileDialog
                {
                    DefaultExt = "xls",
                    Filter = @"Excel文件|*.xls;*.xlsx",
                    FileName = saveFileName
                };
                saveDialog.ShowDialog(); saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

                if (saveFileName == "") return;

                try
                {
                    fs = File.OpenWrite(saveDialog.FileName);
                    workbook.Write(fs);
                    MessageBox.Show(@"下载成功（第一条数据为示例数据，请手动删除）！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);

                }


            }
            catch (Exception exception)
            {
                fs?.Close();
                GC.SuppressFinalize(this);
                MessageBox.Show(@"下载失败！"+ exception.ToString());
               
            }
            finally
            {
                fs?.Close();

            }
            this.DialogResult = DialogResult.OK;
        }

        #region 获取json文件数据
        //读取枚举表
        public List<ENUM> GetAllENUM()
        {
            string jsonfile = System.AppDomain.CurrentDomain.BaseDirectory + "Data\\enum.json";
            listENUM = new List<ENUM>();
            string json = string.Empty;
            string Ru = GetJson(jsonfile);
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            listENUM = Serializer.Deserialize<List<ENUM>>(Ru);
            return listENUM;
        }
        //读取站点坐标表
        public List<COORDINATE> GetAllCOORDINATE()
        {
            string jsonfile = System.AppDomain.CurrentDomain.BaseDirectory + "Data\\coordinate.json";
            listCOOR = new List<COORDINATE>();
            string json = string.Empty;
            string Ru = GetJson(jsonfile);
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            listCOOR = Serializer.Deserialize<List<COORDINATE>>(Ru);
            return listCOOR;
        }
        //读取json转换为string
        public string GetJson(string jsonfile)
        {
            string json = string.Empty;
            string Ru = string.Empty;
            using (StreamReader sr = new StreamReader(jsonfile, Encoding.Default))
            {
                //JArray o = (JArray)JToken.ReadFrom(reader);
                json = sr.ReadToEnd().ToString();
                //json转换为list
                int IndexofA = json.IndexOf("[");
                int IndexofB = json.IndexOf("]");
                Ru = json.Substring(IndexofA, IndexofB - IndexofA + 1);
            }
            return Ru;
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //设备导入模板下载
            FileStream fs = null;
            try
            {
                var saveFileName = "设备信息导入模板.xls";

                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                //(Optional) set the width of the columns
                sheet.SetColumnWidth(0, 40 * 256);
                sheet.SetColumnWidth(1, 40 * 256);
                sheet.SetColumnWidth(2, 30 * 256);
                sheet.SetColumnWidth(3, 30 * 256);
                sheet.SetColumnWidth(4, 30 * 256);
                sheet.SetColumnWidth(5, 30 * 256);
                sheet.SetColumnWidth(6, 30 * 256);
                sheet.SetColumnWidth(7, 30 * 256);
                sheet.SetColumnWidth(8, 30 * 256);

                var headerRow = sheet.CreateRow(0);

                //Set the column names in the header row
                headerRow.CreateCell(0).SetCellValue("行业类型（必填）");
                headerRow.CreateCell(1).SetCellValue("设备名称（必填）");
                headerRow.CreateCell(2).SetCellValue("区划编号（必填）");
                headerRow.CreateCell(3).SetCellValue("区划名称");
                headerRow.CreateCell(4).SetCellValue("出厂编号（必填）");
                headerRow.CreateCell(5).SetCellValue("X");
                headerRow.CreateCell(6).SetCellValue("Y");
                headerRow.CreateCell(7).SetCellValue("行业类型");
                headerRow.CreateCell(8).SetCellValue("监测点编码");
                sheet.CreateFreezePane(0, 1, 0, 1);
                var rowNumber = 1;
                var sites = GlobalHandler.siteresp.GetAllList();
                if (listENUM.Count < 1)
                {
                    GetAllENUM();
                }
                if (listCOOR.Count < 1)
                {
                    GetAllCOORDINATE();
                }
                int i = 0;
                foreach (var site in sites)
                {
                    var type = "";
                    var row = sheet.CreateRow(i + 1);                  
                    if (site.SITE_TYPE == "1")
                    {
                        site.SITE_TYPE = "030201";
                        type = "水厂";
                    }
                    if (site.SITE_TYPE == "2")
                    {
                        var Sensors = GlobalHandler.senresp.GetAllList(a=>a.SITE_ID.Id == site.Id);
                        //site.SITE_TYPE = "030304";
                        type = "流量";
                        var ty1=Sensors.Where(a => a.SENSOR_TYPE_CODE=="1"|| a.SENSOR_TYPE_CODE == "10");
                        var ty2 = Sensors.Where(a => a.SENSOR_TYPE_CODE == "2" || a.SENSOR_TYPE_CODE == "3");
                        if (ty2.Count()>0)
                        {
                            site.SITE_TYPE = "030303";
                        }
                        else 
                        {
                            type = "压力";
                            site.SITE_TYPE = "030302";
                        }

                    }
                    if (site.SITE_TYPE == "7")
                    {
                        site.SITE_TYPE = "030301";
                        type = "水质";
                    }
                    string bh = string.Empty;
                    var code = site.SITE_CODE.Replace("_", "");
                    
                    bh = "341602" + site.SITE_TYPE + code;
                    for (int j = 0; j < 20 - bh.Length; j++)
                    {
                        bh = bh + "X";
                    }
                    if (site.SITE_TYPE=="7")
                    {
                        bh = bh + "X";
                    }

                    row.CreateCell(0).SetCellValue(site.SITE_TYPE);
                    row.CreateCell(1).SetCellValue(site.SITE_NAME);
                    row.CreateCell(2).SetCellValue("341602");
                    row.CreateCell(3).SetCellValue("谯城区");
                    row.CreateCell(4).SetCellValue(site.SITE_CODE);
                    COORDINATE coor = listCOOR.Find(g => g.SITE_CODE == site.SITE_CODE);
                    row.CreateCell(5).SetCellValue(coor.LATITUDE);
                    row.CreateCell(6).SetCellValue(coor.LONGOTUDE);
                    row.CreateCell(7).SetCellValue(type);
                    row.CreateCell(8).SetCellValue(bh);
                    i++;
                }
                var saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "xls"; saveDialog.Filter = @"Excel文件|*.xls;*.xlsx";
                saveDialog.FileName = saveFileName;
                saveDialog.ShowDialog(); saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

                if (saveFileName == "") return;

                try
                {
                    fs = File.OpenWrite(saveDialog.FileName);
                    workbook.Write(fs);
                    MessageBox.Show(@"下载成功（第一条数据为示例数据，请手动删除）！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }
            }
            catch (Exception exception)
            {
                //fs?.Close();
                GC.SuppressFinalize(this);
                MessageBox.Show(@"下载失败！\n" + exception.Message);
            }
            finally
            {
                fs?.Close();
            }
        }
        private bool windowCreate = true;
        private System.Timers.Timer timer;

        public string Day { get; private set; }

        private void button4_Click(object sender, EventArgs e)
        {
            if (windowCreate)
            {
                base.Visible = false;
                windowCreate = false;
            }
            this.Hide();
            base.OnActivated(e);   
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == true)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
            else
            {
                this.Visible = true;
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
                //this.Show();  
                this.BringToFront();
                windowCreate = true;
            }  
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             StartSend();
        }
    }
    public class COORDINATE
    {
        public string SITE_CODE { get; set; }

        public string SITE_NAME { get; set; }
        //经度
        public string LONGOTUDE { get; set; }
        //纬度
        public string LATITUDE { get; set; }

    }
    public class ENUM
    {
        public string id { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public string parent_id { get; set; }
    }
}
