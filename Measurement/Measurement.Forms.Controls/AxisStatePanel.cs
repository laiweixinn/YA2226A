using System;
using System.Drawing;
using System.Windows.Forms;
using DY.CNC.Core;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Motions;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class  AxisStatePanel : UserControl
    {
        private ToolTip Tips = new ToolTip();

        private int _MoveMode = 0;

        private int _SpeedMode = 1;

        private MeasurementAxis _Axis;

        public MeasurementAxis Axis
        {
            get
            {
                return _Axis;
            }
            set
            {
                _Axis = value;
                if (_Axis!=null)
                {
                    lbl_axisname.Text = string.Format("{0}:", _Axis.AxisSet.AxisName);
                    MeasurementMotion motion = _Axis.Motion as MeasurementMotion;
                    motion.IOListener.AxisIOStatusChanged += IOListener_AxisIOStatusChanged;
                    motion.PositionListener.PositionDevChanged += PositionListener_PositionDevChanged;
                    Tips.SetToolTip(lbl_eln, string.Format("[{0}]负限位信号",_Axis.AxisSet.AxisName));
                    Tips.SetToolTip(lbl_elp, string.Format("[{0}]正限位信号", _Axis.AxisSet.AxisName));
                    Tips.SetToolTip(lbl_org, string.Format("[{0}]原点信号", _Axis.AxisSet.AxisName));
                    Tips.SetToolTip(lbl_alm, string.Format("[{0}]报警信号", _Axis.AxisSet.AxisName));
                }
            }
        }

        private void PositionListener_PositionDevChanged(object sender, EventArgs e)
        {
            MotionPositionListener.PositionChangedEventArgs pe = e as MotionPositionListener.PositionChangedEventArgs;
            if (_Axis==null?false:pe.AxisType==_Axis.AxisType)
            {
                try
                {
                    Invoke(new MethodInvoker(()=>SetPosition(_Axis.Motion.PositionListener.PositionDev[_Axis.AxisType])));
                }
                catch (Exception)
                {
                }
            }
        }

        private void IOListener_AxisIOStatusChanged(object sender, EventArgs e)
        {
            AxisIOStatusEventArgs ae = e as AxisIOStatusEventArgs;
            if (_Axis==null?false:ae.AxisType==_Axis.AxisType)
            {
                try
                {
                    Invoke(new MethodInvoker(() => {
                        MeasurementIOListener listener = _Axis.Motion.IOListener as MeasurementIOListener ;
                        int index = _Axis.AxisIndex - 1;
                        SetAxisStatus(listener.ELN[index], listener.ELP[index], listener.ORG[index], listener.ALM[index]);
                    }));
                }
                catch (Exception)
                {
                }
            }
        }

        public AxisStatePanel()
        {
            InitializeComponent();
            Refresh();      
        }

        #region 外观

        private void Mouse_Enter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.BackColor = Color.LightGray;
        }

        private void Mouse_Leave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.BackColor = Color.Gray;
        }

        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.BackColor = Color.Green;
            if (_Axis!=null)
            {
                MeasurementAxisSet axisSet = _Axis.AxisSet as MeasurementAxisSet;
                double dist = 0;
                switch (_MoveMode)
                {
                    case 0:
                        {
                            dist = axisSet.StrokeLength;
                            break;
                        }
                    
                    case 1:
                        {
                            dist = numtxt_dist.Value;
                            break;
                        }
                      
                }
                if (dist>0)
                {
                    if (sender==btn_AxisJogSub)
                    {
                        dist = -dist;
                    }
                    double speed = 0;
                    switch (_SpeedMode)
                    {
                        case 0:
                            {
                                speed = axisSet.ManualSpeedLow;
                                break;
                            }
                        case 1:
                            {
                                speed = axisSet.ManualSpeedNormal;
                                break;
                            }

                        case 2:
                            {
                                speed = axisSet.ManualSpeedHigh;
                                break;
                            }
                    }
                    if (speed>0)
                    {
                        _Axis.Move(dist, speed);
                    }
                    else
                    {
                        MessageBox.Show("手动速度必须大于零");
                    }
                }
                else
                {
                    MessageBox.Show("请输入距离");
                }
            }
        }

        private void Mouse_Up(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.BackColor = Color.Gray;
            if (_Axis!=null)
            {
                if (_MoveMode==0)
                {
                    _Axis.StopSlowly();
                }
            }
        }

        #endregion

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (_Axis!= null)
            {
                _Axis.StopSlowly();
            }
        }

        private void SetAxisStatus(bool eln, bool elp,bool org,bool alm)
        {
            lbl_eln.BackColor = (eln ? Color.Red : Color.Green);
            lbl_elp.BackColor = (elp ? Color.Red : Color.Green);
            lbl_org.BackColor = (org ? Color.Red : Color.Green);
            lbl_alm.BackColor = (alm ? Color.Red : Color.Green);
        }

        private void SetPosition(double posdev)
        {
            lbl_position.Text = posdev.ToString("0.000");
        }

        private void btn_GoHome_Click(object sender, EventArgs e)
        {
            GoHomeForm goHomeForm = new GoHomeForm(_Axis);
            goHomeForm.ShowDialog();
        }

        private void AxisStatePanel_Load(object sender, EventArgs e)
        {
            cbo_speedmode.Items.Clear();
            cbo_speedmode.Items.Add("低速");
            cbo_speedmode.Items.Add("常速");
            cbo_speedmode.Items.Add("高速");
            cbo_movemode.Items.Clear();
            cbo_movemode.Items.Add("连续");
            cbo_movemode.Items.Add("距离");
            cbo_speedmode.SelectedIndex = 1;
            cbo_movemode.SelectedIndex = 0;

            if (_Axis!=null)
            {
                MeasurementPositionListener lis = _Axis.Motion.PositionListener as MeasurementPositionListener;
                SetPosition(lis.PositionDev[_Axis.AxisType]);
            }

            if (_Axis!=null)
            {
                MeasurementIOListener listener = _Axis.Motion.IOListener as MeasurementIOListener;
                int index = _Axis.AxisIndex - 1;
                SetAxisStatus(listener.ELN[index], listener.ELP[index], listener.ORG[index], listener.ALM[index]);
            }
        }

        private void cbo_speedmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SpeedMode = cbo_speedmode.SelectedIndex;
            cbo_speedmode.BackColor = _SpeedMode == 0 ? Color.Cyan : Color.Coral;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _MoveMode = cbo_movemode.SelectedIndex;
            cbo_movemode.BackColor=_MoveMode==1? Color.Cyan : Color.Coral;
            numtxt_dist.Enabled = _MoveMode == 1 ? true : false;
        }
    }
}
