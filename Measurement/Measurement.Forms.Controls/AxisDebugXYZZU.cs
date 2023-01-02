using System;
using System.Drawing;
using System.Windows.Forms;
using DY.CNC.Core;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Motions;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class AxisDebugXYZZU : UserControl
    {
        public AxisDebugXYZZU()
        {
            InitializeComponent();
            Refresh();
        }

        private int _MoveMode = 0;

        private int _SpeedMode = 1;

        private MeasurementAxis _Axis;

        private MeasurementAxis[] _Axises;

        public MeasurementAxis[] Axises
        {
            get
            {
                return _Axises;
            }
            set
            {
                _Axises = value;
            }
        }

        private void SelectAxis(Button button)
        {
            if (button.Text == "X-" || button.Text == "X+")
            {
                _Axis = _Axises[0];
            }
            if (button.Text == "Y-" || button.Text == "Y+")
            {
                _Axis = _Axises[1];
            }
            if (button.Text == "Z-" || button.Text == "Z+")
            {
                _Axis = _Axises[2];
            }
           
            if (button.Text == "U-" || button.Text == "U+")
            {
                _Axis = _Axises[3];
            }
            if (button.Text == "V-" || button.Text == "V+")
            {
                _Axis = _Axises[4];
            }
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
            if (_Axises != null)
            {
                SelectAxis(button);
            }
            if (_Axis != null)
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
                if (dist > 0)
                {
                    if (sender == btn_ydec || sender == btn_xdec || sender == btn_zdec || sender == btn_z2dec || sender == btn_udec)
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
            if (_Axises != null)
            {
                SelectAxis(button);
                if (_Axis != null)
                {
                    if (_MoveMode == 0)
                    {
                        _Axis.StopSlowly();
                    }
                }
            }
        }

        #endregion

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (_Axis != null)
            {
                _Axis.StopSlowly();
            }
        }

        private void btn_GoHome_Click(object sender, EventArgs e)
        {
            GoHomeForm goHomeForm = new GoHomeForm(_Axis);
            goHomeForm.ShowDialog();
        }

        private void cbo_speedmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SpeedMode = cbo_speedmode.SelectedIndex;
            cbo_speedmode.BackColor = _SpeedMode == 0 ? Color.Cyan : Color.Coral;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _MoveMode = cbo_movemode.SelectedIndex;
            cbo_movemode.BackColor = _MoveMode == 1 ? Color.Cyan : Color.Coral;
            numtxt_dist.Enabled = _MoveMode == 1 ? true : false;
        }

        private void AxisDebugXYZZU_Load(object sender, EventArgs e)
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
        }
    }
}
