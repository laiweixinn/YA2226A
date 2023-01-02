using LZ.CNC.Measurement.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms
{
    public partial class AlarmDisplayForm : Form
    {
        public static string ResultPath = @"alarms/" + DateTime.Now.ToString("yyyyMMdd") + ".alm";
        public static string Pathd = @"alarms/";
        public static string[] filename = null;
        public static string dataselect = null;
        public static string[] result = null;
        // private MeasurementAlarms.AlarmCollection _Alarms = null;
        public AlarmDisplayForm()
        {
            InitializeComponent();
            // _Alarms = MeasurementContext.Alarms.AlarmItems;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            ViewInitialze();
        }

        private void ViewInitialze()
        {
            dgv_Message.AllowUserToAddRows = true;//不显示出dataGridView1的最后一行空白
            dgv_Message.BackgroundColor = Color.Gray;
            dgv_Message.GridColor = Color.Black;//设置网格颜色
                                                //dgv_Message.Dock = DockStyle.Fill;
                                                //DataGridViewCheckBoxColumn dgcbc = new DataGridViewCheckBoxColumn();
                                                //dgv_Message.Columns.Add(dgcbc);
                                                //  dgv_Message.BorderStyle = BorderStyle.FixedSingle;



            dgv_Message.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//整行
            //显示格式
            //dgv_Message.Columns[1].DefaultCellStyle.Format = "c";

            //字体样式
            dgv_Message.Font = new Font("微软雅黑", 10, FontStyle.Regular);



            //330启用换行
            //dgv_Message.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgv_Message.Rows[3].Height = 80;

            //331禁止添加和删除行
            dgv_Message.AllowUserToDeleteRows = true;
            dgv_Message.AllowUserToAddRows = false;

            dgv_Message.MultiSelect = true;

            //dgv_Message.Columns[1].ReadOnly = true;
            //dgv_Message.Columns[2].ReadOnly = true;
            //  dgv_Message.ReadOnly = true;
            // dgv_Message.Columns[0].ReadOnly = false;

            dgv_Message.Columns.Add("", "序号");
            dgv_Message.Columns.Add("", "日期");
            dgv_Message.Columns.Add("", "内容");
            dgv_Message.Columns[0].Width = 100;
            dgv_Message.Columns[1].Width = 200;
            dgv_Message.Columns[2].Width = 400;

            //设置对齐方式
            dgv_Message.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv_Message.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv_Message.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private bool FileExists()
        {

            if (!File.Exists(Pathd + selectdate.ToString("yyyyMMdd") + ".alm"))
            {
                return false;
            }
            return true;
        }


        DateTime selectdate;
        private void OpenFile()
        {
            dgv_Message.Rows.Clear();
            selectdate = dateTimePicker1.Value;
            dgv_Message.Columns[0].HeaderText = "序号";
            dgv_Message.Columns[1].HeaderText = "时间";
            dgv_Message.Columns[2].HeaderText = "信息";
            dgv_Message.Columns[0].Width = 100;
            dgv_Message.Columns[1].Width = 200;
            dgv_Message.Columns[2].Width = 400;
            try
            {
                if (!Directory.Exists(Pathd))
                {
                    Directory.CreateDirectory(Pathd);
                }



                dgv_Message.Rows.Clear();
                int i = 0;
                if (radbtn_AlarmDaySelect.Checked)//白班
                {

                    foreach (MeasurementAlarms.AlarmItem item in GetDayAlarms(selectdate))
                    {
                        int index = dgv_Message.Rows.Add();
                        dgv_Message.Rows[index].Cells[0].Value = i;
                        dgv_Message.Rows[index].Cells[1].Value = item.Time;
                        dgv_Message.Rows[index].Cells[2].Value = item.AlarmInfo;

                        if (i % 2 == 0)
                        {
                            dgv_Message.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;
                        }
                        i++;
                    }
                }
                else
                {
                    foreach (MeasurementAlarms.AlarmItem item in GetNightAlarms(selectdate))
                    {
                        int index = dgv_Message.Rows.Add();
                        dgv_Message.Rows[index].Cells[0].Value = i;
                        dgv_Message.Rows[index].Cells[1].Value = item.Time;
                        dgv_Message.Rows[index].Cells[2].Value = item.AlarmInfo;

                        if (i % 2 == 0)
                        {
                            dgv_Message.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;
                        }
                        i++;

                    }
                }
            }

            catch (Exception ex)
            {
            }

        }


        private List<MeasurementAlarms.AlarmItem> GetDayAlarms(DateTime selecttime)
        {
            try
            {
                MeasurementAlarms.AlarmCollection _Alarms = null;
                List<MeasurementAlarms.AlarmItem> amlist = new List<MeasurementAlarms.AlarmItem>();
                MeasurementAlarms alarms = MeasurementAlarms.Load(selecttime);
                _Alarms = alarms.AlarmItems;


                foreach (MeasurementAlarms.AlarmItem item in _Alarms)
                {

                    if (item != null)
                    {

                        DateTime tm = new DateTime(selecttime.Year,
                            selecttime.Month,
                           selecttime.Day,
                             MeasurementContext.Worker.Recipe.CapacityTime.Hour,
                             MeasurementContext.Worker.Recipe.CapacityTime.Minute,
                             0);

                        if ((item.Time - tm).TotalHours <= 12 && (item.Time - tm).TotalHours >= 0)
                        {
                            amlist.Add(item);
                        }
                    }
                }

                return amlist;

            }
            catch (Exception ex)
            {
                return null;

            }
        }


        private List<MeasurementAlarms.AlarmItem> GetNightAlarms(DateTime selecttime)
        {
            try
            {
                MeasurementAlarms.AlarmCollection _Alarms = null;
                List<MeasurementAlarms.AlarmItem> amlist = new List<MeasurementAlarms.AlarmItem>();
                MeasurementAlarms alarms = MeasurementAlarms.Load(selecttime);
                _Alarms = alarms.AlarmItems;

                foreach (MeasurementAlarms.AlarmItem item in _Alarms)
                {

                    if (item != null)//20：00-0：00
                    {

                        DateTime tm = new DateTime(selecttime.Year,
                            selecttime.Month,
                           selecttime.Day,
                             MeasurementContext.Worker.Recipe.CapacityTime.Hour,
                             MeasurementContext.Worker.Recipe.CapacityTime.Minute,
                             0);

                        if ((item.Time - tm).TotalHours > 12)
                        {
                            amlist.Add(item);
                        }
                    }
                }
                alarms = MeasurementAlarms.Load(selectdate.AddDays(1));
                if (alarms == null) return amlist;

                _Alarms = alarms.AlarmItems;
              
                foreach (MeasurementAlarms.AlarmItem item in _Alarms)
                {
                    if (item != null)//00：00-8：00
                    {

                        DateTime tm = new DateTime(selecttime.Year,
                            selecttime.Month,
                           selecttime.AddDays(1).Day,
                             MeasurementContext.Worker.Recipe.CapacityTime.Hour,
                             MeasurementContext.Worker.Recipe.CapacityTime.Minute,
                             0);

                        if (((item.Time - tm).Hours < 0) ||
                            (((item.Time - tm).Hours == 0 && (item.Time - tm).Minutes <= 0)))
                        {
                            amlist.Add(item);
                        }
                    }
                }
                return amlist;
            }
            catch (Exception ex)
            {
                return null;
            }
        }





        private void button1_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {


                dgv_Message.Rows.Clear();
                if (!FileExists()) return;           
                Dictionary<string, int> infodic = new Dictionary<string, int>();
                if (radbtn_AlarmDaySelect.Checked)//白班
                {
                    foreach (MeasurementAlarms.AlarmItem item in GetDayAlarms(selectdate))
                    {
                        if (item == null) continue;
                        if (!infodic.ContainsKey(item.AlarmInfo))
                        {
                            infodic.Add(item.AlarmInfo, 1);
                        }
                        else
                        {
                            infodic[item.AlarmInfo] = infodic[item.AlarmInfo] + 1;
                        }
                    }
                }
                else
                {
                    foreach (MeasurementAlarms.AlarmItem item in GetNightAlarms(selectdate))//晚班
                    {
                        if (item == null) continue;
                        if (!infodic.ContainsKey(item.AlarmInfo))
                        {
                            infodic.Add(item.AlarmInfo, 1);
                        }
                        else
                        {
                            infodic[item.AlarmInfo] = infodic[item.AlarmInfo] + 1;
                        }
                    }

                }
             

                var collec = from k in infodic
                             orderby k.Value descending
                             select k;

                dgv_Message.Rows.Clear();
                dgv_Message.Columns[0].HeaderText = "序号";
                dgv_Message.Columns[1].HeaderText = "信息";
                dgv_Message.Columns[2].HeaderText = "次数";
                dgv_Message.Columns[0].Width = 100;
                dgv_Message.Columns[1].Width = 400;
                dgv_Message.Columns[2].Width = 100;
                int i = 0;
                foreach (var item in collec)
                {
                    int index = dgv_Message.Rows.Add();
                    dgv_Message.Rows[index].Cells[0].Value = i;
                    dgv_Message.Rows[index].Cells[1].Value = item.Key;
                    dgv_Message.Rows[index].Cells[2].Value = item.Value;
                    if (i % 2 == 0)
                    {
                        dgv_Message.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;
                    }
                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = ".alm";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            fileDialog.FileName = Pathd + ".txt";
            fileDialog.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
