using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Motions;
using DY.CNC.Core;
using System;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class AxisSetPanel : UserControl
    {
        private DataTable _Table = null;

        private MeasurementMotion[] _Motions = null;

        private MeasurementAxisSet _AxisSet = null;

        private List<MeasurementAxisSet> list = new List<MeasurementAxisSet>();

        private List<NumericInputBox> _numboxlist = new List<NumericInputBox>();

        private List<string> _ColumnsName = new List<string>()
        {
            "AxisName",
            "PulseNum",
            "Lead",
            "StartSpeed",
            "ACC",
            "RunSpeed",
            "HomeSpeed",
            "LowSpeed",
            "NomalSpeed",
            "HighSpeed",
            "SoftMax",
            "SoftMin",
            "SoftEnabled",
            "HomeDir"
        };

        private bool _IsInitData = false;

        private bool _IsDelaySelect = false;

        private DateTime _DelaySelectTime = DateTime.Now;

        public AxisSetPanel()
        {
            InitializeComponent();
            cbo_homedir.Items.Clear();
            cbo_homedir.Items.Add("正方向");
            cbo_homedir.Items.Add("负方向");
            _numboxlist.Add(numtxt_pulsnum);
            _numboxlist.Add(numtxt_leaddist);
            _numboxlist.Add(numtxt_startspeed);
            _numboxlist.Add(numtxt_accspeed);
            _numboxlist.Add(numtxt_runspeed);
            _numboxlist.Add(numtxt_homespeed);
            _numboxlist.Add(numtxt_handlow);
            _numboxlist.Add(numtxt_handnomal);
            _numboxlist.Add(numtxt_handhih);
            _numboxlist.Add(numtxt_softmax);
            _numboxlist.Add(numtxt_softmin);
            for (int i = 0; i < _numboxlist.Count; i++)
            {
                _numboxlist[i].ValueChanged += AxisSetPanel_ValueChanged;
            }
            chk_softenabled.CheckedChanged += AxisSetPanel_ValueChanged;
            cbo_homedir.SelectedIndexChanged += AxisSetPanel_ValueChanged;
            InitTable();
        }

        private void AxisSetPanel_ValueChanged(object sender, EventArgs e)
        {
            if (!_IsInitData)
            {
                SaveData();
            }
        }

        private void InitTable()
        {
            _Table = new DataTable();
            for (int i = 0; i < _ColumnsName.Count; i++)
            {
                _Table.Columns.Add(_ColumnsName[i]);
            }
            _Table.Columns.Add("Obj", typeof(object));

            for (int i = 0; i < _ColumnsName.Count; i++)
            {
                dataGridView1.Columns[i].DataPropertyName = _ColumnsName[i];
            }

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _Table;
        }

        private void RefreshTable()
        {
            if (_Motions != null)
            {
                int i = list.Count - _Table.Rows.Count;
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

                for (int j = 0; j < list.Count; j++)
                {
                    MeasurementAxisSet axisSet = list[j];
                    DataRow dr = _Table.Rows[j];
                    dr["AxisName"] = axisSet.AxisName;
                    dr["PulseNum"] = axisSet.PulsePerLap;
                    dr["Lead"] = axisSet.UnitPerLap;
                    dr["StartSpeed"] = axisSet.SpeedStart;
                    dr["ACC"] = axisSet.SpeedAcc;
                    dr["RunSpeed"] = axisSet.MoveSpeed;
                    dr["HomeSpeed"] = axisSet.HomeSpeed;
                    dr["LowSpeed"] = axisSet.ManualSpeedLow;
                    dr["NomalSpeed"] = axisSet.ManualSpeedNormal;
                    dr["HighSpeed"] = axisSet.ManualSpeedHigh;
                    dr["SoftMax"] = axisSet.SoftLimitMax;
                    dr["SoftMin"] = axisSet.SoftLimitMin;
                    dr["SoftEnabled"] = axisSet.SoftLimitEnabled ? "启用" : "停用";
                    dr["HomeDir"] = axisSet.HomeDir == MoveDirections.Negative ? "负方向" : "正方向";
                    dr["Obj"] = axisSet;
                }
            }
        }

        public void Save()
        {
            if (_Motions != null)
            {
                SaveData();
            }
        }

        private void SaveData()
        {
            if (_Motions != null && _AxisSet != null)
            {
                _AxisSet.PulsePerLap = (int)_numboxlist[0].Value;
                _AxisSet.UnitPerLap = _numboxlist[1].Value;
                _AxisSet.SpeedStart = _numboxlist[2].Value;
                _AxisSet.SpeedAcc = _numboxlist[3].Value;
                _AxisSet.MoveSpeed = _numboxlist[4].Value;
                int val = GetAxisSpeedMaxVal(_AxisSet.AxisName);
                _numboxlist[4].Value = _numboxlist[4].Value > val ? val : _numboxlist[4].Value;
                _AxisSet.HomeSpeed = _numboxlist[5].Value;
                _AxisSet.ManualSpeedLow = _numboxlist[6].Value;
                _AxisSet.ManualSpeedNormal = _numboxlist[7].Value;
                _AxisSet.ManualSpeedHigh = _numboxlist[8].Value;
                _AxisSet.SoftLimitMax = _numboxlist[9].Value;
                _AxisSet.SoftLimitMin = _numboxlist[10].Value;
                _AxisSet.HomeDir = (cbo_homedir.SelectedIndex == 0 ? MoveDirections.Positive : MoveDirections.Negative);
                _AxisSet.SoftLimitEnabled = chk_softenabled.Checked;
                RefreshTable();
                MeasurementContext.Worker.MotionA.MotionSet.SaveMotionSet("0");
                MeasurementContext.Worker.MotionB.MotionSet.SaveMotionSet("1");
                MeasurementContext.Worker.MotionC.MotionSet.SaveMotionSet("2");
                MeasurementContext.Worker.MotionD.MotionSet.SaveMotionSet("3");
            }
        }

        private void InitData()
        {
            if (_Motions != null)
            {

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow dvr = dataGridView1.SelectedRows[0];
                    DataRowView drv = dvr.DataBoundItem as DataRowView;
                    DataRow dr = drv.Row;
                    MeasurementAxisSet axisSet = dr["Obj"] as MeasurementAxisSet;
                    _AxisSet = axisSet;
                    _IsInitData = true;
                    grp_axisparm.Text = _AxisSet.AxisName;
                    _numboxlist[0].Value = _AxisSet.PulsePerLap;
                    _numboxlist[1].Value = _AxisSet.UnitPerLap;
                    _numboxlist[2].Value = _AxisSet.SpeedStart;
                    _numboxlist[3].Value = _AxisSet.SpeedAcc;
                    _numboxlist[4].Value = _AxisSet.MoveSpeed;
                    _numboxlist[4].MaxValue = GetAxisSpeedMaxVal(_AxisSet.AxisName);
                    _numboxlist[5].Value = _AxisSet.HomeSpeed;
                    _numboxlist[6].Value = _AxisSet.ManualSpeedLow;
                    _numboxlist[7].Value = _AxisSet.ManualSpeedNormal;
                    _numboxlist[8].Value = _AxisSet.ManualSpeedHigh;
                    _numboxlist[9].Value = _AxisSet.SoftLimitMax;
                    _numboxlist[10].Value = _AxisSet.SoftLimitMin;
                    cbo_homedir.SelectedIndex = (int)_AxisSet.HomeDir - 1;
                    chk_softenabled.Checked = _AxisSet.SoftLimitEnabled;
                    _IsInitData = false;
                }
            }
        }

        Dictionary<string, int> dicSpeedMax = new Dictionary<string, int>();
        private int GetAxisSpeedMaxVal(string axisName)
        {
            try
            {
                if (dicSpeedMax.Count <= 0)
                {
                    dicSpeedMax.Add("来料平台Y轴", 200);
                    dicSpeedMax.Add("上料工位X轴", 1200);
                    dicSpeedMax.Add("左撕膜Y轴", 800);
                    dicSpeedMax.Add("中撕膜Y轴", 800);
                    dicSpeedMax.Add("右撕膜Y轴", 800);

                    dicSpeedMax.Add("左撕膜X轴", 300);
                    dicSpeedMax.Add("中撕膜X轴", 300);
                    dicSpeedMax.Add("右撕膜X轴", 300);

                    dicSpeedMax.Add("左撕膜Z轴", 300);
                    dicSpeedMax.Add("中撕膜Z轴", 300);
                    dicSpeedMax.Add("右撕膜Z轴", 300);

                    dicSpeedMax.Add("左撕膜W轴", 300);
                    dicSpeedMax.Add("中撕膜W轴", 300);
                    dicSpeedMax.Add("右撕膜W轴", 300);

                    dicSpeedMax.Add("中转工位X轴", 2000);
                    dicSpeedMax.Add("撕膜相机X轴", 800);

                    dicSpeedMax.Add("左折弯对位X轴", 400);
                    dicSpeedMax.Add("左折弯对位Y轴", 400);
                    dicSpeedMax.Add("左折弯对位R轴", 400);
                    dicSpeedMax.Add("左折弯对位W轴", 200);
                    dicSpeedMax.Add("左折弯平台Y轴", 1500);
                    dicSpeedMax.Add("左折弯相机X轴", 300);

                    dicSpeedMax.Add("中折弯对位X轴", 400);
                    dicSpeedMax.Add("中折弯对位Y轴", 400);
                    dicSpeedMax.Add("中折弯对位R轴", 400);
                    dicSpeedMax.Add("中折弯对位W轴", 200);
                    dicSpeedMax.Add("中折弯平台Y轴", 1500);
                    dicSpeedMax.Add("中折弯相机X轴", 300);

                    dicSpeedMax.Add("右折弯对位X轴", 400);
                    dicSpeedMax.Add("右折弯对位Y轴", 400);
                    dicSpeedMax.Add("右折弯对位R轴", 400);
                    dicSpeedMax.Add("右折弯对位W轴", 200);
                    dicSpeedMax.Add("右折弯平台Y轴", 1500);
                    dicSpeedMax.Add("右折弯相机X轴", 300);

                    dicSpeedMax.Add("出料工位X轴", 1000);
                    dicSpeedMax.Add("出料工位Z轴", 600);
                }

                if (dicSpeedMax.ContainsKey(axisName))
                {
                    return dicSpeedMax[axisName];
                }
                else
                {
                    return 1500;
                }
            }
            catch (Exception ex)
            {
                return 1500;
            }

        }


        public void Init(MeasurementMotion[] motions)
        {
            _Motions = motions;
            if (motions != null)
            {
                list.Clear();
                foreach (MeasurementMotion item in _Motions)
                {
                    for (int i = 0; i < item.AxisCount; i++)
                    {
                        list.Add(item.Axises[i].AxisSet as MeasurementAxisSet);
                    }
                }
                RefreshTable();
                InitData();
            }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            _DelaySelectTime = DateTime.Now;
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
                            SaveData();
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
    }
}
