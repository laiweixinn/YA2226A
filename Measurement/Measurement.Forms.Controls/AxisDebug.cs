using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DY.CNC.Core;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Motions;


namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class AxisDebug : UserControl
    {
        public AxisDebug()
        {
            InitializeComponent();
            Refresh();
        }

        private int _MoveMode = 0;

        private int _SpeedMode = 1;

       

        private MeasurementAxis _Axises;

        public MeasurementAxis Axises
        {
            get
            {
                return _Axises;
            }
            set
            {
                _Axises = value;
                if (_Axises != null)
                {
                    lbl_axisname.Text = string.Format("{0}:", _Axises.AxisSet.AxisName);
                    MeasurementMotion motion = _Axises.Motion as MeasurementMotion;
                    motion.PositionListener.PositionDevChanged += PositionListener_PositionDevChanged;
                }
            }
        }

       

        private void PositionListener_PositionDevChanged(object sender, EventArgs e)
        {
            MotionPositionListener.PositionChangedEventArgs pe = e as MotionPositionListener.PositionChangedEventArgs;
            if (_Axises == null ? false : pe.AxisType == _Axises.AxisType)
            {
                try
                {
                    Invoke(new MethodInvoker(() => SetPosition(_Axises.Motion.PositionListener.PositionDev[_Axises.AxisType])));
                }
                catch (Exception)
                {
                }
            }
        }

        private void SetPosition(double posdev)
        {
            lbl_position.Text = posdev.ToString("0.000");
        }

        private void SelectAxis(Button button)
        {
              
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
            button.BackColor = Color.LightGray;
        }

        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.BackColor = Color.Green;
            if (_Axises != null)
            {
                SelectAxis(button);
            }
            if (_Axises != null)
            {

                MeasurementAxisSet axisSet = _Axises.AxisSet as MeasurementAxisSet;
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
                            dist =(double) num_dist.Value;
                            break;
                        }

                }
                if (dist > 0)
                {
                    if (sender == btn_negative )
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
                    if (speed > 0)
                    {
                        _Axises.Move(dist, speed);
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
            if (_Axises != null)
            {
                SelectAxis(button);
                if (_Axises != null)
                {
                    if (_MoveMode == 0)
                    {
                        _Axises.StopSlowly();
                    }
                }
            }
        }

        #endregion

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (_Axises != null)
            {
                _Axises.StopSlowly();
            }
        }

        private void btn_GoHome_Click(object sender, EventArgs e)
        {
            GoHomeForm goHomeForm = new GoHomeForm(_Axises);
            goHomeForm.ShowDialog();
        }

            

        private void btn_movemode_Click(object sender, EventArgs e)
        {
            if (btn_movemode.Text == "连续")
            {
                btn_movemode.Text = "距离";
                btn_movemode.BackColor = Color.Coral;
                _MoveMode = 1;
            }
            else
            {
                btn_movemode.Text = "连续";
                btn_movemode.BackColor = Color.LightGray;
                _MoveMode = 0;
            }
        }

        private void AxisDebug_Load(object sender, EventArgs e)
        {
            cbo_speedmode.Items.Clear();
            cbo_speedmode.Items.Add("低速");
            cbo_speedmode.Items.Add("常速");
            cbo_speedmode.Items.Add("高速");
            cbo_speedmode.SelectedIndex = 1;
            if (_Axises != null)
            {
                MeasurementPositionListener lis = _Axises.Motion.PositionListener as MeasurementPositionListener;
                SetPosition(lis.PositionDev[_Axises.AxisType]);
            }
        }

        private void cbo_speedmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SpeedMode = cbo_speedmode.SelectedIndex;
            cbo_speedmode.BackColor = _SpeedMode == 0 ? Color.Cyan : Color.Coral;
        }

    }
}
