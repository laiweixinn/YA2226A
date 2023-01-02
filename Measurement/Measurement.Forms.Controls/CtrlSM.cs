using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;
using System.Threading;
using DY.Core.Forms;
using LZ.CNC.Measurement.Core.Motions;

namespace LZ.CNC.Measurement.Forms.Controls
{

   
    public partial class CtrlSM : PageUC.BasePageUC
    {

        private DataTable _Table = null;

        private MeasurementData.RecipeDataItem _Data = null;

        private MeasurementData.SMDataItem _Positem = null;

        private bool _IsInitData = false;

        private bool _IsDelaySelect = false;

        private DateTime _DelaySelectTime = DateTime.Now;

        private int station_flag = 0; // 撕膜工位标志:  0 左撕膜,1中撕膜,2右撕膜
        private MeasurementAxis[] station_axise = null;

        public CtrlSM()
        {
            InitializeComponent();
            //Input_smstartX.ValueChanged += NumericInputBox_ValueChanged;
            //Input_smstartY.ValueChanged += NumericInputBox_ValueChanged;
            //Input_smstartZ.ValueChanged += NumericInputBox_ValueChanged;
            //Input_smEndX.ValueChanged += NumericInputBox_ValueChanged;
            //Input_smEndY.ValueChanged += NumericInputBox_ValueChanged;
            //Input_smEndZ.ValueChanged += NumericInputBox_ValueChanged;
            //Input_SMDist.ValueChanged += NumericInputBox_ValueChanged;
            //Chke_Enabled.CheckedChanged += NumericInputBox_ValueChanged;
            MeasurementContext.Worker.RecipeChanged += Worker_RecipeChanged;
            InitTable();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.ExStyle |= 0x02000000; 
                return parms;
            }
        }



        private void Worker_RecipeChanged(object sender, EventArgs e)
        {
            _Data = MeasurementContext.Data.CurrentRecipeData;
            RefreshTable();
        }




        public void Init(MeasurementData.RecipeDataItem data,MeasurementAxis[] axies,int flag)
        {

            _Data = data;
            station_flag = flag;
            station_axise = axies;
            if (_Data != null)
            {
                RefreshTable();
                InitData();             
            }
        }


        private void InitTable()
        {
            _Table = new DataTable();
            _Table.Columns.Add("Index");
            _Table.Columns.Add("startX");
            _Table.Columns.Add("startY");
            _Table.Columns.Add("startZ");
            _Table.Columns.Add("endX");
            _Table.Columns.Add("endY");
            _Table.Columns.Add("endZ");
            _Table.Columns.Add("smdist");
            _Table.Columns.Add("smEnable");
            _Table.Columns.Add("gluedist");
            _Table.Columns.Add("gluestickdist");
            _Table.Columns.Add("gluestickspd");
            _Table.Columns.Add("tearrllcldEnable");
            _Table.Columns.Add("Obj", typeof(object));

            Column1.DataPropertyName = "Index";
            Column2.DataPropertyName = "startX";
            Column3.DataPropertyName = "startY";
            Column4.DataPropertyName = "startZ";
            Column5.DataPropertyName = "endX";
            Column6.DataPropertyName = "endY";
            Column7.DataPropertyName = "endZ";
            Column8.DataPropertyName = "smdist";
            Column9.DataPropertyName = "smEnable";
            Column10.DataPropertyName = "gluedist";
            Column11.DataPropertyName = "gluestickdist";
            Column12.DataPropertyName = "gluestickspd";
            Column13.DataPropertyName = "tearrllcldEnable";
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _Table;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void InitData()
        {
            if (_Data != null)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow dvr = dataGridView1.SelectedRows[0];
                    DataRowView drv = dvr.DataBoundItem as DataRowView;
                    DataRow dr = drv.Row;
                    MeasurementData.SMDataItem item = dr["Obj"] as MeasurementData.SMDataItem;
                    _Positem = item;
                    _IsInitData = true;
                    int index = _Data.SMdatas.IndexOf(item);
                    Input_smstartX.Value = _Positem.SMStartX;
                    Input_smstartY.Value = _Positem.SMStartY;
                    Input_smstartZ.Value = _Positem.SMStartZ;
                    Input_smEndX.Value = _Positem.SMEndX;
                    Input_smEndY.Value = _Positem.SMEndY;
                    Input_smEndZ.Value = _Positem.SMEndZ;
                    Input_SMDist.Value = _Positem.SMDist;
                    Input_GlueDist.Value = _Positem.GlueDist;
                    Input_GlueStickDist.Value = _Positem.GlueStickDist;
                    Input_GlueStickSPD.Value = _Positem.GlueStickSpd;
                    Chke_Enabled.IsCkecked = _Positem.SMEnabled;
                    Chke_RllCLDEnabled.IsCkecked = _Positem.TearRllCLDEnabled;
                   

                
                    groupBox1.Text = string.Format("撕膜位置{0}", (char)(index + 65-station_flag*6));
                    _IsInitData = false;
                }
            }
        }

        private void RefreshTable()
        {
            if (_Data != null)
            {
                MeasurementData.SMDataItemCollection data = null;
                data = _Data.SMdatas;
                int i = 6 - _Table.Rows.Count;

                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        _Table.Rows.Add(_Table.NewRow());
                    }
                }
                else if (i < 0)
                {
                    i = -i;
                    for (int j = 0; j < i; j++)
                    {
                        _Table.Rows.RemoveAt(_Table.Rows.Count - 1);
                    }
                }
                for (int j = 0; j < 6; j++)
                {
                    MeasurementData.SMDataItem item = data[j+station_flag*6];
                    DataRow dr = _Table.Rows[j];                   
                    dr["Index"] = string.Format("撕膜位置{0}", (char)(j + 65));
                    dr["startX"] = item.SMStartX.ToString("0.000");
                    dr["startY"] = item.SMStartY.ToString("0.000");
                    dr["startZ"] = item.SMStartZ.ToString("0.000");
                    dr["endX"] = item.SMEndX.ToString("0.000");
                    dr["endY"] = item.SMEndY.ToString("0.000");
                    dr["endZ"] = item.SMEndZ.ToString("0.000");
                    dr["smdist"] = item.SMDist.ToString("0.000");
                    dr["smEnable"] = item.SMEnabled?"开启":"未开启";
                    dr["gluedist"] = item.GlueDist.ToString("0.000");
                    dr["gluestickdist"] = item.GlueStickDist.ToString("0.000");
                    dr["gluestickspd"] = item.GlueStickSpd.ToString("0.000");
                    dr["tearrllcldEnable"] = item.TearRllCLDEnabled ? "开启" : "未开启";
                    dr["Obj"] = item;
                }
            }
        }

        public void NumericInputBox_ValueChanged(object sender, EventArgs e)
        {
            if (!_IsInitData)
            {
                SaveData();
            }
        }




        public override void Save()
        {
            if (_Data != null)
            {
                SaveData();
            }
        }

        private void SaveData()
        {
            if (_Data != null && _Positem != null)
            {
                _Positem.SMStartX = Input_smstartX.Value;
                _Positem.SMStartY = Input_smstartY.Value;
                _Positem.SMStartZ = Input_smstartZ.Value;
                _Positem.SMEndX = Input_smEndX.Value;
                _Positem.SMEndY = Input_smEndY.Value;
                _Positem.SMEndZ = Input_smEndZ.Value;
                _Positem.SMDist = Input_SMDist.Value;
                _Positem.SMEnabled = Chke_Enabled.IsCkecked;
                _Positem.TearRllCLDEnabled = Chke_RllCLDEnabled.IsCkecked;                
                _Positem.GlueDist = Input_GlueDist.Value;
                _Positem.GlueStickSpd = Input_GlueStickSPD.Value;
                _Positem.GlueStickDist = Input_GlueStickDist.Value;
                RefreshTable();
                MeasurementContext.Data.Save();
            }
        }

        
        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (!_IsDelaySelect)
            {
                _IsDelaySelect = true;
                ThreadPool.QueueUserWorkItem(delegate
                {
                    while ((DateTime.Now - _DelaySelectTime).TotalMilliseconds < 200.0)
                    {
                    }
                    try
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                           // SaveData();
                            InitData();                           
                        }));
                    }
                    catch (Exception)
                    {
                    }
                    _IsDelaySelect = false;
                });
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录未撕膜起始位置?"))
            {
                MeasurementData.SMDataItemCollection data = null;
                data = _Data.SMdatas;
                int index = data.IndexOf(_Positem);

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_LeftSM_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_LeftSM_Y;
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_LeftSM_Z;
                if((index>5&index<12))
                {
                     axisx = MeasurementContext.Worker.Axis_MidSM_X;
                     axisy = MeasurementContext.Worker.Axis_MidSM_Y;
                     axisz = MeasurementContext.Worker.Axis_MidSM_Z;
                }
                else if(index>11)
                {
                    axisx = MeasurementContext.Worker.Axis_RightSM_X;
                    axisy = MeasurementContext.Worker.Axis_RightSM_Y;
                    axisz = MeasurementContext.Worker.Axis_RightSM_Z;
                } 


                Input_smstartX.Value = axisx.PositionDev;
                Input_smstartY.Value = axisy.PositionDev;
                Input_smstartZ.Value = axisz.PositionDev;
             
                if (_Data != null && _Positem != null)
                {
                    _Positem.SMStartX = Input_smstartX.Value;
                    _Positem.SMStartY = Input_smstartY.Value;
                    _Positem.SMStartZ = Input_smstartZ.Value;                   
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位撕膜起始位置?"))
            {
                MeasurementData.SMDataItemCollection data = null;
                 
                data = _Data.SMdatas;
                int index = data.IndexOf(_Positem);
                LocateSmStartPos(index,station_axise);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录撕膜结束位置?"))
            {
                MeasurementData.SMDataItemCollection data = null;
                data = _Data.SMdatas;
                int index = data.IndexOf(_Positem);

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_LeftSM_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_LeftSM_Y;
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_LeftSM_Z;

                if ((index > 5 & index < 11))
                {
                    axisx = MeasurementContext.Worker.Axis_MidSM_X;
                    axisy = MeasurementContext.Worker.Axis_MidSM_Y;
                    axisz = MeasurementContext.Worker.Axis_MidSM_Z;
                }
                else if (index > 10)
                {
                    axisx = MeasurementContext.Worker.Axis_RightSM_X;
                    axisy = MeasurementContext.Worker.Axis_RightSM_Y;
                    axisz = MeasurementContext.Worker.Axis_RightSM_Z;
                }
                Input_smEndX .Value = axisx.PositionDev;
                Input_smEndY.Value = axisy.PositionDev;
                Input_smEndZ.Value = axisz.PositionDev;

                if (_Data != null && _Positem != null)
                {
                    _Positem.SMEndX = Input_smstartX.Value;
                    _Positem.SMEndY = Input_smstartY.Value;
                    _Positem.SMEndZ = Input_smstartZ.Value;
                }
            }
        }


        private void LocateSmStartPos(int index,MeasurementAxis[] axises)
        {
            WaitForm.Show(string.Format("定位撕膜起始位置[{0}]...", (char)(index + 65)), (IAsyncResult argument0) =>
            {
                if (!MeasurementContext.Worker.LocateSMPt(index,false,axises))
                {
                    WaitForm.ShowErrorMessage(string.Format("定位撕膜起始位置[{0}]失败...", (char)(index + 65)));
                }
            }, (IAsyncResult argument1) =>
            {
                MeasurementContext.Worker.EndMotion();
                MeasurementContext.Worker.StopSlowly();
            });
        }


        private void LocateSMEndPos(int index, MeasurementAxis[] axises)
        {
            WaitForm.Show(string.Format("定位撕膜结束位置[{0}]...", (char)(index + 65)), (IAsyncResult argument0) =>
            {
                if (!MeasurementContext.Worker.LocateSMPt(index, true, axises))
                {
                    WaitForm.ShowErrorMessage(string.Format("定位撕膜结束位置[{0}]...", (char)(index + 65)));
                }
            }, (IAsyncResult argument1) =>
            {
                MeasurementContext.Worker.EndMotion();
                MeasurementContext.Worker.StopSlowly();
            });
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位撕膜结束位置?"))
            {
                MeasurementData.SMDataItemCollection data = null;
                              
                data = _Data.SMdatas;
                int index = data.IndexOf(_Positem);
                LocateSMEndPos(index, station_axise);
            }
        }
    }
}
