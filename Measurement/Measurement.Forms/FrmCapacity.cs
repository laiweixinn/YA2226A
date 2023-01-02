using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;
using System.IO;
using DY.Core.Configs;

namespace LZ.CNC.Measurement.Forms
{
    public partial class FrmCapacity : TabForm
    {
        public FrmCapacity()
        {
            InitializeComponent();
        }

        private List<AOI1DataCollections.DetectDataItem> _DetectDataItems1 = null;
        private List<AOI2DataCollections.DetectDataItem> _DetectDataItems2 = null;
        private List<AOI3DataCollections.DetectDataItem> _DetectDataItems3 = null;
        private BindingList<AOI1DataCollections.DetectDataItem> _Bend1DataItems = null;
        private BindingList<AOI2DataCollections.DetectDataItem> _Bend2DataItems = null;
        private BindingList<AOI3DataCollections.DetectDataItem> _Bend3DataItems = null;

        AOI1DataCollections Bend1Data;
        AOI2DataCollections Bend2Data;
        AOI3DataCollections Bend3Data;
        MeasurementCapacity _capacity;
        MeasurementMonthCapacity _monthcapacity;

        private void RebdingBend1()
        {
            try
            {
                if (_DetectDataItems1 != null)
                {
                    _Bend1DataItems = new BindingList<AOI1DataCollections.DetectDataItem>(_DetectDataItems1);
                    dgv_Bend1AOI.DataSource = _Bend1DataItems;

                    dgv_Bend1AOI.Columns[0].HeaderText = "时间";
                    dgv_Bend1AOI.Columns[1].HeaderText = "Y1/mm";
                    dgv_Bend1AOI.Columns[2].HeaderText = "Y2/mm";
                    dgv_Bend1AOI.Columns[3].HeaderText = "X1/mm";
                    dgv_Bend1AOI.Columns[4].HeaderText = "X2/mm";
                    dgv_Bend1AOI.Columns[5].HeaderText = "结果";
                    dgv_Bend1AOI.Columns[6].HeaderText = "拉力/N";


                    dgv_Bend1AOI.Columns[0].FillWeight = 110;
                    dgv_Bend1AOI.Columns[1].FillWeight = 60;
                    dgv_Bend1AOI.Columns[2].FillWeight = 60;
                    dgv_Bend1AOI.Columns[3].FillWeight = 60;
                    dgv_Bend1AOI.Columns[4].FillWeight = 60;
                    dgv_Bend1AOI.Columns[5].FillWeight = 60;
                    dgv_Bend1AOI.Columns[6].FillWeight = 60;

                    //ZGH20220905增加弯折平台AOI统计显示
                    int Result_AOIOK = 0; int Result_AOING = 0;
                    for (int i = 0; i < dgv_Bend1AOI.RowCount; i++)
                    {
                        string ResultData = Convert.ToString(dgv_Bend1AOI.Rows[i].Cells[5].Value);
                        if (ResultData == "OK")
                        {
                            Result_AOIOK += 1;
                        }
                        else
                        {
                            if (ResultData == "NG")
                            {
                                Result_AOING += 1;
                            }
                        }
                    }
                    lbl_bend1AOIok.Text = Convert.ToString(Result_AOIOK);
                    lbl_bend1AOIng.Text = Convert.ToString(Result_AOING);
                    lbl_bend1AOIquantity.Text = Convert.ToString(dgv_Bend1AOI.RowCount);
                    if (Result_AOIOK > 0)
                    {
                        lbl_tear1AOIpercent.Text = Convert.ToString(Result_AOIOK / dgv_Bend1AOI.RowCount * 100);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            

        }

        private void RebdingBend2()
        {
            try
            {
                if (_DetectDataItems2 != null)
                {
                    _Bend2DataItems = new BindingList<AOI2DataCollections.DetectDataItem>(_DetectDataItems2);
                    dgv_Bend2AOI.DataSource = _Bend2DataItems;

                    dgv_Bend2AOI.Columns[0].HeaderText =  "时间";
                    dgv_Bend2AOI.Columns[1].HeaderText =  "Y1/mm";
                    dgv_Bend2AOI.Columns[2].HeaderText =  "Y2/mm";
                    dgv_Bend2AOI.Columns[3].HeaderText =  "X1/mm";
                    dgv_Bend2AOI.Columns[4].HeaderText =  "X2/mm";
                    dgv_Bend2AOI.Columns[5].HeaderText =  "结果";
                    dgv_Bend2AOI.Columns[6].HeaderText = "拉力/N";

                    dgv_Bend2AOI.Columns[0].FillWeight = 110;
                    dgv_Bend2AOI.Columns[1].FillWeight = 60;
                    dgv_Bend2AOI.Columns[2].FillWeight = 60;
                    dgv_Bend2AOI.Columns[3].FillWeight = 60;
                    dgv_Bend2AOI.Columns[4].FillWeight = 60;
                    dgv_Bend2AOI.Columns[5].FillWeight = 60;
                    dgv_Bend2AOI.Columns[6].FillWeight = 60;

                    //ZGH20220905增加弯折平台AOI统计显示
                    int Result_AOIOK = 0; int Result_AOING = 0;
                    for (int i = 0; i < dgv_Bend2AOI.RowCount; i++)
                    {
                        string ResultData = Convert.ToString(dgv_Bend2AOI.Rows[i].Cells[5].Value);
                        if (ResultData == "OK")
                        {
                            Result_AOIOK += 1;
                        }
                        else
                        {
                            if (ResultData == "NG")
                            {
                                Result_AOING += 1;
                            }
                        }
                    }
                    lbl_bend2AOIok.Text = Convert.ToString(Result_AOIOK);
                    lbl_bend2AOIng.Text = Convert.ToString(Result_AOING);
                    lbl_bend2AOIquantity.Text = Convert.ToString(dgv_Bend2AOI.RowCount);
                    if (Result_AOIOK > 0)
                    {
                        lbl_tear2AOIpercent.Text = Convert.ToString(Result_AOIOK / dgv_Bend2AOI.RowCount * 100);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void RebdingBend3()
        {
            try
            {
                if (_DetectDataItems3 != null)
                {
                    _Bend3DataItems = new BindingList<AOI3DataCollections.DetectDataItem>(_DetectDataItems3);
                    dgv_Bend3AOI.DataSource = _Bend3DataItems;

                    dgv_Bend3AOI.Columns[0].HeaderText = "时间";
                    dgv_Bend3AOI.Columns[1].HeaderText = "Y1/mm";
                    dgv_Bend3AOI.Columns[2].HeaderText = "Y2/mm";
                    dgv_Bend3AOI.Columns[3].HeaderText = "X1/mm";
                    dgv_Bend3AOI.Columns[4].HeaderText = "X2/mm";
                    dgv_Bend3AOI.Columns[5].HeaderText = "结果";
                    dgv_Bend3AOI.Columns[6].HeaderText = "拉力/N";

                    dgv_Bend3AOI.Columns[0].FillWeight = 110;
                    dgv_Bend3AOI.Columns[1].FillWeight = 60;
                    dgv_Bend3AOI.Columns[2].FillWeight = 60;
                    dgv_Bend3AOI.Columns[3].FillWeight = 60;
                    dgv_Bend3AOI.Columns[4].FillWeight = 60;
                    dgv_Bend3AOI.Columns[5].FillWeight = 60;
                    dgv_Bend3AOI.Columns[6].FillWeight = 60;

                    //ZGH20220905增加弯折平台AOI统计显示
                    int Result_AOIOK = 0; int Result_AOING = 0;
                    for (int i = 0; i < dgv_Bend3AOI.RowCount; i++)
                    {
                        string ResultData = Convert.ToString(dgv_Bend3AOI.Rows[i].Cells[5].Value);
                        
                        if (ResultData == "OK")
                        {
                            Result_AOIOK += 1;
                        }
                        else
                        {
                            if (ResultData == "NG")
                            {
                                Result_AOING += 1;
                            }
                        }
                    }
                    lbl_bend3AOIok.Text= Convert.ToString(Result_AOIOK);
                    lbl_bend3AOIng.Text = Convert.ToString(Result_AOING);
                    lbl_bend3AOIquantity.Text = Convert.ToString(dgv_Bend3AOI.RowCount);
                    if (Result_AOIOK > 0)
                    {
                        lbl_tear3AOIpercent.Text = Convert.ToString(Result_AOIOK/dgv_Bend3AOI.RowCount*100);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Init()
        {
            capacityUC1.Init(MeasurementContext.Capacity, MeasurementContext.MonthCapacity);

        }

        private void RefreshData()
        {


        }
        private void RefreshBendAoiData()
        {


        }
        

        private void DataForm_Load(object sender, EventArgs e)
        {

        }



        private void btn_query_Click(object sender, EventArgs e)
        {
            try
            {
                string selectname = "bend1_" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + ".dds";
                string path = Path.Combine(ConfigBase.GetApplicationPath("detectdatas"), selectname);
                Bend1Data = ConfigBase.Load(path) as AOI1DataCollections;
                _DetectDataItems1 = Bend1Data.DetectDatas;
                RebdingBend1();

                selectname = "bend2_" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + ".dds";
                path = Path.Combine(ConfigBase.GetApplicationPath("detectdatas"), selectname);
                Bend2Data = ConfigBase.Load(path) as AOI2DataCollections;
                _DetectDataItems2 = Bend2Data.DetectDatas;
                RebdingBend2();

                selectname = "bend3_" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + ".dds";
                path = Path.Combine(ConfigBase.GetApplicationPath("detectdatas"), selectname);
                Bend3Data = ConfigBase.Load(path) as AOI3DataCollections;
                _DetectDataItems3 = Bend3Data.DetectDatas;
                RebdingBend3();

                RefreshBendAoiData();
            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.Message);
            }
        }

        private void btn_check_Click(object sender, EventArgs e)
        {

            string selectname = dateTimePicker2.Value.Date.ToString("yyyyMMdd") + ".sta";
            string monthselectname = string.Format("{0}{1}month.sta", dateTimePicker2.Value.Date.Year, dateTimePicker2.Value.Date.Month);
            MeasurementContext.Capacity.Save(selectname);
            int iii = DateTime.Now.Month;
            string path = Path.Combine(ConfigBase.GetApplicationPath("statistics\\capacity"), selectname);
            string strpath = Path.Combine(ConfigBase.GetApplicationPath("statistics\\capacity"), monthselectname);

            _capacity = ConfigBase.Load(path) as MeasurementCapacity;
            _monthcapacity = ConfigBase.Load(strpath) as MeasurementMonthCapacity;

            if (!File.Exists(path))
            {
                MessageBox.Show(dateTimePicker2.Value.Date.ToShortDateString() + "无生产数据");
                return;
            }

            if (!File.Exists(strpath))
            {
                return;
            }


            if (dateTimePicker2.Value.Date.Day == DateTime.Now.Day && dateTimePicker2.Value.Date.Month == DateTime.Now.Month)
            {
                capacityUC1.Init(MeasurementContext.Capacity, MeasurementContext.MonthCapacity);
            }
            else
            {
                capacityUC1.Init(_capacity, _monthcapacity);
            }
        }



        private void FrmCapacity_Load(object sender, EventArgs e)
        {
            try
            {

                Init();
                Bend1Data = AOI1DataCollections.Load();
                if (Bend1Data == null)
                {
                    Bend1Data = new AOI1DataCollections();
                    Bend1Data.Save();
                    return;
                }
                _DetectDataItems1 = Bend1Data.DetectDatas;

                Bend2Data = AOI2DataCollections.Load();
                if (Bend2Data == null)
                {
                    Bend2Data = new AOI2DataCollections();
                    Bend2Data.Save();
                    return;
                }
                _DetectDataItems2 = Bend2Data.DetectDatas;


                Bend3Data = AOI3DataCollections.Load();
                if (Bend3Data == null)
                {
                    Bend3Data = new AOI3DataCollections();
                    Bend3Data.Save();
                    return;
                }
                _DetectDataItems3 = Bend3Data.DetectDatas;
                RebdingBend1();
                RebdingBend2();
                RebdingBend3();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_showdata_Click(object sender, EventArgs e)
        {
            DataForm frm = new DataForm();
            frm.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //  MeasurementContext.Worker.Capacity.AddPre(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.Capacity.AddPre(2, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.Capacity.AddPre(3, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.Capacity.AddPre(1, 1);
        }

        private void btn_PrintData_Click(object sender, EventArgs e)
        {

            try
            {
                DateTime time = dateTimePicker2.Value;
                string path = Path.Combine(MeasurementContext.Config.PrintPath, MeasurementContext.Worker.Recipe.Name, time.ToString("yyyyMMdd"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
             
                //打印产能数据
                MeasurementCapacity _capacity = MeasurementCapacity.Load(time);

                Task.Run(() =>
                {
                    string fullname = Path.Combine(path, time.ToString("yyyy年MM月dd日") + "--日产能统计.csv");
                    PrintCapacityDataOfOneDay(_capacity, fullname);

                    fullname = Path.Combine(path, time.ToString("yyyy年MM月dd日") + "--左折弯AOI数据.csv");
                    PrintCapacityDataOfAOI1(time, fullname);

                    fullname = Path.Combine(path, time.ToString("yyyy年MM月dd日") + "--中折弯AOI数据.csv");
                    PrintCapacityDataOfAOI2(time, fullname);

                    fullname = Path.Combine(path, time.ToString("yyyy年MM月dd日") + "--右折弯AOI数据.csv");
                    PrintCapacityDataOfAOI3(time, fullname);

                    MessageBox.Show("打印完成,请在指定文件夹中查看!");
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败,路径是否有效或已打开文件!");
            }
        }

        private void PrintCapacityDataOfOneDay(MeasurementCapacity capacity, string fullname)
        {
            try
            {
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }
                StringBuilder data = new StringBuilder();
                string hour;

                string str;
                if (MeasurementContext.Worker.Recipe.CapacityTime.Minute < 10)
                {
                    str = "0" + MeasurementContext.Worker.Recipe.CapacityTime.Minute.ToString();
                }
                else
                {
                    str = MeasurementContext.Worker.Recipe.CapacityTime.Minute.ToString();
                }


                File.AppendAllText(fullname, "时间段,折弯OK,折弯NG,撕膜NG,总数,生产时间,待料时间,折弯良率,综合良率,宕机时间,停/待机时间\r\n");
                for (int i = 0; i < 12; i++)
                {
                    hour = string.Format(" {0}:{2}--{1}:{3}", (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour).ToString().PadLeft(2, '0'), (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour + 1).ToString().PadLeft(2, '0'),
                         str, str);
                    data.Append(hour + ",");
                    data.Append(capacity.DayShiftProduct[i].NumOfDispenseOK.ToString() + ",");
                    data.Append(capacity.DayShiftProduct[i].NumOfDispenseNG.ToString() + ",");
                    data.Append(capacity.DayShiftProduct[i].NumOfIncomingNG.ToString() + ",");
                    data.Append(capacity.DayShiftProduct[i].TotalNumPerHour.ToString() + ",");
                    data.Append(capacity.DayShiftProduct[i].HourProductionTime.Minutes.ToString() + ",");
                    data.Append(capacity.DayShiftProduct[i].HourFreeTime.Minutes.ToString() + ",");
                    data.Append(capacity.DayShiftProduct[i].PercentDispenseOK.ToString() + ",");
                    data.Append(capacity.DayShiftProduct[i].PercentTotalOK.ToString() + ",");
                    data.Append(capacity.DayShiftProduct[i].HourAlarmTime.Minutes.ToString() + ",");
                    data.Append(capacity.DayShiftProduct[i].HourStopTime.ToString() + "\r\n");
                    File.AppendAllText(fullname, data.ToString());
                    data.Clear();
                }


                data.Append("【白班总计】,");
                data.Append(capacity.DayShiftTotalDisepenserOK.ToString() + ",");
                data.Append(capacity.DayShiftTotalDisepenserNG.ToString() + ",");
                data.Append(capacity.DayShiftTotalIncomingNG.ToString() + ",");
                data.Append(capacity.DayShiftTotal.ToString() + ",");
                data.Append(capacity.DayProductTime.ToString() + ",");
                data.Append(capacity.DayFreeTime.ToString() + ",");
                data.Append(capacity.DayShiftPercentDispenseOK.ToString() + ",");
                data.Append(capacity.DayShiftPercentTotalOK.ToString() + ",");
                data.Append(capacity.DayAlarmTime.ToString() + ",");
                data.Append(capacity.DayStopTime.ToString() + "\r\n");
                File.AppendAllText(fullname, data.ToString());
                File.AppendAllText(fullname, "\r\n");
                data.Clear();

                int int_flag = 12 - MeasurementContext.Worker.Recipe.CapacityTime.Hour;
                for (int i = 0; i < 12; i++)
                {
                    if (i < int_flag)
                    {
                        if (i == int_flag - 1)
                        {
                            hour = string.Format(" {0}:{2}-{1}:{3}", (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour + 12).ToString().PadLeft(2, '0'), (0).ToString().PadLeft(2, '0'), str, str);
                        }
                        else
                        {
                            hour = string.Format(" {0}:{2}-{1}:{3}", (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour + 12).ToString().PadLeft(2, '0'), (i + MeasurementContext.Worker.Recipe.CapacityTime.Hour + 13).ToString().PadLeft(2, '0'), str, str);
                        }
                    }
                    else
                    {
                        DateTime dt = DateTime.Now.AddDays(1);
                        hour = string.Format(" {0}:{2}--{1}:{3}", (i - (12 - MeasurementContext.Worker.Recipe.CapacityTime.Hour)).ToString().PadLeft(2, '0'), (i - (11 - MeasurementContext.Worker.Recipe.CapacityTime.Hour)).ToString().PadLeft(2, '0'), str, str);
                    }

                    data.Append(hour + ",");
                    data.Append(capacity.NightShiftProduct[i].NumOfDispenseOK.ToString() + ",");
                    data.Append(capacity.NightShiftProduct[i].NumOfDispenseNG.ToString() + ",");
                    data.Append(capacity.NightShiftProduct[i].NumOfIncomingNG.ToString() + ",");
                    data.Append(capacity.NightShiftProduct[i].TotalNumPerHour.ToString() + ",");
                    data.Append(capacity.NightShiftProduct[i].HourProductionTime.Minutes.ToString() + ",");
                    data.Append(capacity.NightShiftProduct[i].HourFreeTime.Minutes.ToString() + ",");
                    data.Append(capacity.NightShiftProduct[i].PercentDispenseOK.ToString() + ",");
                    data.Append(capacity.NightShiftProduct[i].PercentTotalOK.ToString() + ",");
                    data.Append(capacity.NightShiftProduct[i].HourAlarmTime.Minutes.ToString() + ",");
                    data.Append(capacity.NightShiftProduct[i].HourStopTime.ToString() + "\r\n");
                    File.AppendAllText(fullname, data.ToString());
                    data.Clear();
                }

                data.Append("【晚班总计】,");
                data.Append(capacity.NightShiftTotalDisepenserOK.ToString() + ",");
                data.Append(capacity.NightShiftTotalDisepenserNG.ToString() + ",");
                data.Append(capacity.NightShiftTotalIncomingNG.ToString() + ",");
                data.Append(capacity.NightShiftTotal.ToString() + ",");
                data.Append(capacity.NightProductTime.ToString() + ",");
                data.Append(capacity.NightFreeTime.ToString() + ",");
                data.Append(capacity.NightShiftPercentDispenseOK.ToString() + ",");
                data.Append(capacity.NightShiftPercentTotalOK.ToString() + ",");
                data.Append(capacity.NightAlarmTime.ToString() + ",");
                data.Append(capacity.NightStopTime.ToString() + "\r\n");
                File.AppendAllText(fullname, data.ToString());
                data.Clear();

            }
            catch (Exception ex)
            {
            }

        }

        private void PrintCapacityDataOfAOI1(DateTime tm, string fullname)
        {
            try
            {
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }

                string selectname = "bend1_" + tm.ToString("yyyy-MM-dd") + ".dds";
                string path = Path.Combine(ConfigBase.GetApplicationPath("detectdatas"), selectname);
                Bend1Data = ConfigBase.Load(path) as AOI1DataCollections;
                _DetectDataItems1 = Bend1Data.DetectDatas;

                StringBuilder strbd = new StringBuilder(128);
                strbd.Append("序号,时间,Y1/mm,Y2/mm,X1/mm,X2/mm,结果,拉力/N \r\n");
                File.AppendAllText(fullname, strbd.ToString());
                strbd.Clear();

                int num = 0;
                foreach (AOI1DataCollections.DetectDataItem item in _DetectDataItems1)
                {
                    strbd.Append(num + ",");
                    strbd.Append(item.PanelID + ",");                 
                    strbd.Append(item.AOIY1 + ",");
                    strbd.Append(item.AOIY2 + ",");
                    strbd.Append(item.AOIX1 + ",");
                    strbd.Append(item.AOIX2 + ",");               
                    strbd.Append(item.Result + ",");
                    strbd.Append(item.Weighval + ",");
                    strbd.Append("\r\n");
                    File.AppendAllText(fullname, strbd.ToString());
                    strbd.Clear();
                    num++;
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void PrintCapacityDataOfAOI2(DateTime tm, string fullname)
        {
            try
            {
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }

                string selectname = "bend2_" + tm.ToString("yyyy-MM-dd") + ".dds";
                string path = Path.Combine(ConfigBase.GetApplicationPath("detectdatas"), selectname);
                Bend2Data = ConfigBase.Load(path) as AOI2DataCollections;
                _DetectDataItems2 = Bend2Data.DetectDatas;

                StringBuilder strbd = new StringBuilder(128);
                strbd.Append("序号,时间,Y1/mm,Y2/mm,X1/mm,X2/mm,结果,拉力/N \r\n");
                File.AppendAllText(fullname, strbd.ToString());
                strbd.Clear();

                int num = 0;
                foreach (AOI2DataCollections.DetectDataItem item in _DetectDataItems2)
                {
                    strbd.Append(num + ",");
                    strbd.Append(item.PanelID + ",");                
                    strbd.Append(item.AOIY1 + ",");
                    strbd.Append(item.AOIY2 + ",");
                    strbd.Append(item.AOIX1 + ",");
                    strbd.Append(item.AOIX2 + ",");               
                    strbd.Append(item.Result + ",");
                    strbd.Append(item.Weighval + ",");
                    strbd.Append("\r\n");
                    File.AppendAllText(fullname, strbd.ToString());
                    strbd.Clear();
                    num++;
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void PrintCapacityDataOfAOI3(DateTime tm, string fullname)
        {

            try
            {
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }


                string selectname = "bend3_" + tm.ToString("yyyy-MM-dd") + ".dds";
                string path = Path.Combine(ConfigBase.GetApplicationPath("detectdatas"), selectname);
                Bend3Data = ConfigBase.Load(path) as AOI3DataCollections;
                _DetectDataItems3 = Bend3Data.DetectDatas;
                StringBuilder strbd = new StringBuilder(128);
                strbd.Append("序号,时间,Y1/mm,Y2/mm,X1/mm,X2/mm,结果,拉力/N \r\n");
                File.AppendAllText(fullname, strbd.ToString());
                strbd.Clear();

                int num = 0;
                foreach (AOI3DataCollections.DetectDataItem item in _DetectDataItems3)
                {
                    strbd.Append(num + ",");
                    strbd.Append(item.PanelID + ",");                
                    strbd.Append(item.AOIY1 + ",");
                    strbd.Append(item.AOIY2 + ",");
                    strbd.Append(item.AOIX1 + ",");
                    strbd.Append(item.AOIX2 + ",");               
                    strbd.Append(item.Result + ",");
                    strbd.Append(item.Weighval + ",");
                    strbd.Append("\r\n");
                    File.AppendAllText(fullname, strbd.ToString());
                    strbd.Clear();
                    num++;
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
