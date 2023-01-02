using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LZ.CNC.Measurement.Core;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using LZ.CNC.Measurement.Forms.Controls;

namespace LZ.CNC.Measurement.Forms.PageUC
{
    public partial class CapacityUC : BasePageUC
    {
        private MeasurementCapacity _Capacity;

        private MeasurementMonthCapacity _MonthCapacity;
        List<SimpleStatisticalUnit> lisdaysmpnits = new List<SimpleStatisticalUnit>();
        List<SimpleStatisticalUnit> lisnightsmpnits = new List<SimpleStatisticalUnit>();

        List<StatisticalUnit> lisdaystatis = new List<StatisticalUnit>();
        List<StatisticalUnit> lisnightstatis = new List<StatisticalUnit>();
        public CapacityUC()
        {
            InitializeComponent();
        }

        public void Init(MeasurementCapacity capacity, MeasurementMonthCapacity monthcapacity)
        {
            InitStatisControls();//容器内找统计控件并排序初始化
            _Capacity = capacity;
            _MonthCapacity = monthcapacity;
            if (MeasurementContext.Worker.Recipe != null)
            {
                Var_StartHour.Value = MeasurementContext.Worker.Recipe.CapacityTime.Hour;
                Var_StartMinute.Value = MeasurementContext.Worker.Recipe.CapacityTime.Minute;

                valb_HourTarget.Value = MeasurementContext.Worker.Recipe.HourTargetCapacity;
                valb_ShiftTarget.Value = MeasurementContext.Worker.Recipe.DayTargetCapacity;

            }
            if (_Capacity != null)
            {
                //capacity.CapacityChanged += Capacity_CapacityChanged;
                RefreshUI();
            }
            Load_Chart();
            ChartDisplay();
        }
        /// <summary>
        /// 容器内找统计控件并排序初始化
        /// </summary>
        private void InitStatisControls()
        {
            //日产能表格的上部分控件
            lisdaysmpnits = FindControlByType<SimpleStatisticalUnit>(pel_DaySimple);
            lisdaysmpnits = SimoleControlsSort(lisdaysmpnits);
            //日产能表格的下部分控件
            lisnightsmpnits = FindControlByType<SimpleStatisticalUnit>(pel_NightSimple);
            lisnightsmpnits = SimoleControlsSort(lisnightsmpnits);
            //日线左部分控件
            lisdaystatis = FindControlByType<StatisticalUnit>(pel_DayStatis);
            lisdaystatis = ControlsSort(lisdaystatis);
            //日线右部分控件
            lisnightstatis = FindControlByType<StatisticalUnit>(pel_NightStatis);
            lisnightstatis = ControlsSort(lisnightstatis);

        }

        private void Capacity_CapacityChanged(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private new void RefreshUI()
        {
            try
            {
                if (_Capacity != null)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(() => _RefreshUI()));
                    }
                    else
                    {
                        _RefreshUI();
                    }
                }
            }
            catch (Exception EX)
            {

            }
        }


        private void _RefreshUI()
        {
            if (_Capacity != null)
            {


                lstviewex_capacity.BeginUpdate();
                lstviewex_month_capacity.BeginUpdate();
                lstviewex_month_capacity.Items.Clear();
                lstviewex_capacity.Items.Clear();
                ListViewItem listView = null;
                ListViewItem listViewMonth = null;

                for (int i = 0; i < 31; i++)
                {
                    listViewMonth = new ListViewItem
                    {
                        Text = string.Format("{0}号产能", i + 1)
                    };

                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].NumOfDayBendOk.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].NumOfDayBendNG.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].NumOfDayTearNG.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].DayTotalNum.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].DayPercentBendOK.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].DayPercentTotalOK.ToString());
                    listViewMonth.SubItems.Add(" ");
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].NumOfNightBendOk.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].NumOfNightBendNG.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].NumOfNightTearNG.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].NightTotalNum.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].NightPercentBendOK.ToString());
                    listViewMonth.SubItems.Add(_MonthCapacity.MonthProductStatic[i].NightPercentTotalOK.ToString());

                    lstviewex_month_capacity.Items.Add(listViewMonth);

                }

                string str;
                if (MeasurementContext.Worker.Recipe.CapacityTime.Minute < 10)
                {
                    str = "0" + MeasurementContext.Worker.Recipe.CapacityTime.Minute.ToString();
                }
                else
                {
                    str = MeasurementContext.Worker.Recipe.CapacityTime.Minute.ToString();
                }
                string hour;
                for (int i = 0; i < 12; i++)
                {
                    hour = string.Format(" {0}:{2}--{1}:{3}", (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour).ToString().PadLeft(2, '0'), (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour + 1).ToString().PadLeft(2, '0'),
                      str, str);
                    listView = new ListViewItem
                    {
                        Text = hour
                    };
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].NumOfDispenseOK.ToString());
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].NumOfDispenseNG.ToString());
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].NumOfIncomingNG.ToString());
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].TotalNumPerHour.ToString());
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].HourProductionTime.Minutes.ToString());
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].HourFreeTime.Minutes.ToString());
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].PercentDispenseOK.ToString());
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].PercentTotalOK.ToString());
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].HourAlarmTime.Minutes.ToString());
                    listView.SubItems.Add(_Capacity.DayShiftProduct[i].HourStopTime.ToString());

                    lisdaystatis[i].Hour = lisdaysmpnits[i].Hour = hour;
                    lisdaystatis[i].value = lisdaysmpnits[i].value = _Capacity.DayShiftProduct[i].TotalNumPerHour;

                    if (i % 2 == 0)
                    {
                        listView.BackColor = Color.FromArgb(204, 204, 204);
                    }


                    if (CurrentTimeIndexInDayOrNight()<=11)
                    {
                        if (i== CurrentTimeIndexInDayOrNight())
                        {
                            listView.BackColor = Color.Green;
                        }
                    }

               
                    lstviewex_capacity.Items.Add(listView);
                }

                listView = new ListViewItem
                {
                    Text = string.Format("【白班】")
                };
                listView.SubItems.Add(_Capacity.DayShiftTotalDisepenserOK.ToString());
                listView.SubItems.Add(_Capacity.DayShiftTotalDisepenserNG.ToString());
                listView.SubItems.Add(_Capacity.DayShiftTotalIncomingNG.ToString());
                listView.SubItems.Add(_Capacity.DayShiftTotal.ToString());
                listView.SubItems.Add(_Capacity.DayProductTime.ToString());
                listView.SubItems.Add(_Capacity.DayFreeTime.ToString());
                listView.SubItems.Add(_Capacity.DayShiftPercentDispenseOK.ToString());
                listView.SubItems.Add(_Capacity.DayShiftPercentTotalOK.ToString());
                listView.SubItems.Add(_Capacity.DayAlarmTime.ToString());
                listView.SubItems.Add(_Capacity.DayStopTime.ToString());
                lstviewex_capacity.Items.Add(listView);
                listView = new ListViewItem();
                listView.BackColor = Color.FromArgb(204, 204, 204);
                lstviewex_capacity.Items.Add(listView);
                int int_flag = 12 - MeasurementContext.Worker.Recipe.CapacityTime.Hour;
                lisdaystatis[12].Hour = lisdaysmpnits[12].Hour = "总计";
                lisdaystatis[12].value = lisdaysmpnits[12].value = _Capacity.DayShiftTotal;
               

                for (int i = 0; i < 12; i++)
                {
                    listView = new ListViewItem();
                    if (i < int_flag)
                    {
                        if (i == int_flag - 1)
                        {

                            hour = string.Format(" {0}:{2}-{1}:{3}", (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour + 12).ToString().PadLeft(2, '0'), (0).ToString().PadLeft(2, '0'), str, str);
                            listView.Text = hour;
                        }
                        else
                        {
                            hour = string.Format(" {0}:{2}-{1}:{3}", (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour + 12).ToString().PadLeft(2, '0'), (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour + 13).ToString().PadLeft(2, '0'), str, str);
                            listView.Text = hour;
                        }
                    }
                    else
                    {
                        DateTime dt = DateTime.Now.AddDays(1);
                        hour = string.Format(" {0}:{2}--{1}:{3}", (i - (12 - MeasurementContext.Worker.Recipe.CapacityTime.Hour)).ToString().PadLeft(2, '0'), (i - (11 - MeasurementContext.Worker.Recipe.CapacityTime.Hour)).ToString().PadLeft(2, '0'), str, str);
                        listView.Text = hour;
                    }


                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].NumOfDispenseOK.ToString());
                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].NumOfDispenseNG.ToString());
                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].NumOfIncomingNG.ToString());
                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].TotalNumPerHour.ToString());
                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].HourProductionTime.Minutes.ToString());
                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].HourFreeTime.Minutes.ToString());
                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].PercentDispenseOK.ToString());
                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].PercentTotalOK.ToString());
                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].HourAlarmTime.Minutes.ToString());
                    listView.SubItems.Add(_Capacity.NightShiftProduct[i].HourStopTime.ToString());

                    lisnightstatis[i].Hour = lisnightsmpnits[i].Hour = hour;
                    lisnightstatis[i].value = lisnightsmpnits[i].value = _Capacity.NightShiftProduct[i].TotalNumPerHour;

                    if (i%2==0)
                    {
                        listView.BackColor = Color.FromArgb(204, 204, 204); 
                    }

                    if (CurrentTimeIndexInDayOrNight() >= 12)
                    {
                        if (i == CurrentTimeIndexInDayOrNight()-12)
                        {
                            listView.BackColor = Color.Green;
                        }
                    }


                    lstviewex_capacity.Items.Add(listView);
                }




                listView = new ListViewItem
                {
                    Text = string.Format("【晚班】")
                };
                listView.SubItems.Add(_Capacity.NightShiftTotalDisepenserOK.ToString());
                listView.SubItems.Add(_Capacity.NightShiftTotalDisepenserNG.ToString());
                listView.SubItems.Add(_Capacity.NightShiftTotalIncomingNG.ToString());
                listView.SubItems.Add(_Capacity.NightShiftTotal.ToString());
                listView.SubItems.Add(_Capacity.NightProductTime.ToString());
                listView.SubItems.Add(_Capacity.NightFreeTime.ToString());
                listView.SubItems.Add(_Capacity.NightShiftPercentDispenseOK.ToString());
                listView.SubItems.Add(_Capacity.NightShiftPercentTotalOK.ToString());
                listView.SubItems.Add(_Capacity.NightAlarmTime.ToString());
                listView.SubItems.Add(_Capacity.NightStopTime.ToString());

                listView.BackColor = Color.FromArgb(204, 204, 204);
                lisnightstatis[12].Hour = lisnightsmpnits[12].Hour = "总计";
                lisnightstatis[12].value = lisnightsmpnits[12].value = _Capacity.NightShiftTotal;

                lstviewex_capacity.Items.Add(listView);
                lstviewex_capacity.EndUpdate();
                lstviewex_month_capacity.EndUpdate();

                lbl_tear1ok.Text = _Capacity.tear1ok.ToString();
                lbl_tear1ng.Text = _Capacity.tear1ng.ToString();
                lbl_tear1percent.Text = _Capacity.tear1okpercent;
                lbl_tear2ok.Text = _Capacity.tear2ok.ToString();
                lbl_tear2ng.Text = _Capacity.tear2ng.ToString();
                lbl_tear2percent.Text = _Capacity.tear2okpercent;
                lbl_tear3ok.Text = _Capacity.tear3ok.ToString();
                lbl_tear3ng.Text = _Capacity.tear3ng.ToString();
                lbl_tear3percent.Text = _Capacity.tear3okpercent;

                lbl_bend1ok.Text = _Capacity.bend1ok.ToString();
                lbl_bend1ng.Text = _Capacity.bend1ng.ToString();
                lbl_bend1percent.Text = _Capacity.bend1okpercent;
                lbl_bend2ok.Text = _Capacity.bend2ok.ToString();
                lbl_bend2ng.Text = _Capacity.bend2ng.ToString();
                lbl_bend2percent.Text = _Capacity.bend2okpercent;
                lbl_bend3ok.Text = _Capacity.bend3ok.ToString();
                lbl_bend3ng.Text = _Capacity.bend3ng.ToString();
                lbl_bend3percent.Text = _Capacity.bend3okpercent;
                lbl_bendgapng.Text = _Capacity.bendGetGapNG.ToString();
                lbl_bendaoing.Text = _Capacity.bendAOING.ToString();
                // lbl_dayfreetime.Text = _Capacity.FreeTime.Hours.ToString() + "小时" + _Capacity.FreeTime.Minutes.ToString() + "分" + _Capacity.FreeTime.Seconds + "秒";
            }

        }

        private List<T> FindControlByType<T>(Panel panel)
        {
            List<T> controls = new List<T>();
            foreach (var item in panel.Controls)
            {
                if (item is T)
                {
                    controls.Add((T)item);
                }
            }
            return controls;
        }

        private List<SimpleStatisticalUnit> SimoleControlsSort(List<SimpleStatisticalUnit> cons)
        {
            List<SimpleStatisticalUnit> coll = new List<SimpleStatisticalUnit>();
            var a = from k in cons
                    orderby k.Index ascending
                    select k;

            foreach (SimpleStatisticalUnit item in a)
            {
                if (item.Index != 12)
                {
                    item.MaxValue = MeasurementContext.Worker.Recipe.HourTargetCapacity;
                }
                else
                {
                    item.MaxValue = MeasurementContext.Worker.Recipe.DayTargetCapacity;
                }
                coll.Add(item);
            }

            return coll;
        }

        private List<StatisticalUnit> ControlsSort(List<StatisticalUnit> cons)
        {
            List<StatisticalUnit> coll = new List<StatisticalUnit>();
            var a = from k in cons
                    orderby k.Index ascending
                    select k;

            foreach (StatisticalUnit item in a)
            {
                if (item.Index != 12)
                {
                    item.MaxValue = MeasurementContext.Worker.Recipe.HourTargetCapacity;
                }
                else
                {
                    item.MaxValue = MeasurementContext.Worker.Recipe.DayTargetCapacity;
                }
                coll.Add(item);
            }
            return coll;
        }



        private void btn_Save_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.Recipe.CapacityTime = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:00", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, (int)Var_StartHour.Value, (int)Var_StartMinute.Value));
            MeasurementContext.Worker.Recipe.HourTargetCapacity = (int)valb_HourTarget.Value;
            MeasurementContext.Worker.Recipe.DayTargetCapacity = (int)valb_ShiftTarget.Value;
            MeasurementContext.Data.Save();
            RefreshUI();
        }

        private void btn_rstdata_Click(object sender, EventArgs e)
        {

            try
            {
                _Capacity = new MeasurementCapacity();
                _Capacity.Save();
                RefreshUI();
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                MeasurementContext.Capacity.AddPre(1, 1);
            }

            MeasurementContext.MonthCapacity.AddPre(1);
        }

        #region 月线显示

        List<string> day = new List<string>();
        List<int> dayproductnum = new List<int>();
        List<int> nightproductnum = new List<int>();
        private void Load_Chart()//图初始化
        {
            try
            {
                chart1.ChartAreas[0].AxisX.Interval = 2;
                chart1.ChartAreas[0].AxisY.Interval = 1000;

                chart1.Series[0].BorderWidth = 3;
                chart1.Series[0].LabelBorderWidth = 3;
                chart1.Series[1].BorderWidth = 3;

                //chart1.Series["白班"]["PointWidth"] = "0.6";
                //chart1.Series["晚班"]["PointWidth"] = "0.6";

                chart1.Legends[0].Docking = Docking.Top;              //'调整图例的位置      
                day.Clear();
                dayproductnum.Clear();
                nightproductnum.Clear();
                for (int i = 0; i < 31; i++)
                {
                    day.Add(string.Format("{0}号", i + 1));
                    dayproductnum.Add(MeasurementContext.MonthCapacity.MonthProductStatic[i].DayTotalNum);
                    nightproductnum.Add(MeasurementContext.MonthCapacity.MonthProductStatic[i].NightTotalNum);
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void ChartDisplay()//条形图数据显示
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[0].IsValueShownAsLabel = true;  // '柱头显示
            chart1.Series[1].IsValueShownAsLabel = true;

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;//X轴网格线
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;//Y轴网格线
                                                           
            //chart1.Series[0].LabelBackColor = Color.Black;  //'柱状图显示的颜色
            //chart1.Series[1].LabelForeColor = Color.Brown;


            //  chart1.Series[1].LabelBackColor = Color.Coral;

            chart1.Series[0].Points.DataBindXY(day, dayproductnum);
            chart1.Series[1].Points.DataBindXY(day, nightproductnum);
        }





        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private bool DateTimeCompare(DateTime t1, DateTime t2)
        {
            bool res = true;
            if (t1.Hour > t2.Hour) return res;
            if (t1.Hour == t2.Hour && t1.Minute > t2.Minute) return res;
            if (t1.Hour == t2.Hour && t1.Minute == t2.Minute && t1.Second > t2.Second) return res;
            if (t1.Hour == t2.Hour && t1.Minute == t2.Minute && t1.Second == t2.Second) return res;
            return false;
        }


        private int CurrentTimeIndexInDayOrNight()
        {
            int index;
            if (DateTimeCompare(DateTime.Now, MeasurementContext.Worker.Recipe.CapacityTime) && !DateTimeCompare(DateTime.Now, MeasurementContext.Worker.Recipe.CapacityTime.AddHours(12)))//白班
            {
                TimeSpan tm = DateTime.Now - MeasurementContext.Worker.Recipe.CapacityTime;
                index = tm.Hours;
            }
            else if (!DateTimeCompare(DateTime.Now, MeasurementContext.Worker.Recipe.CapacityTime) && DateTimeCompare(DateTime.Now, MeasurementContext.Worker.Recipe.CapacityTime.AddHours(-MeasurementContext.Worker.Recipe.CapacityTime.Hour)))//0：30-7：30
            {
                TimeSpan tm = DateTime.Now - MeasurementContext.Worker.Recipe.CapacityTime.AddHours(-MeasurementContext.Worker.Recipe.CapacityTime.Hour);
                index = tm.Hours + 12+5;
            }
            else
            {
                TimeSpan tm = DateTime.Now - MeasurementContext.Worker.Recipe.CapacityTime.AddHours(12);
                index = tm.Hours + 12;
            }
            return index;
        }
    }
}
