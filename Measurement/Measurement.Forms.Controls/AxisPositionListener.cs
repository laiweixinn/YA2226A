using System;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core.Motions;
using DY.CNC.Core;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class AxisPositionListener : UserControl
    {
        private MeasurementAxis _Axis;

        private void PositionListener_PositionDevChanged(object sender, EventArgs e)
        {
            MotionPositionListener.PositionChangedEventArgs pe = e as MotionPositionListener.PositionChangedEventArgs;
            if (_Axis == null ? false : pe.AxisType == _Axis.AxisType)
            {
                try
                {
                    Invoke(new MethodInvoker(() => SetPosition(_Axis.Motion.PositionListener.PositionDev[_Axis.AxisType])));
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

        public AxisPositionListener()
        {
            InitializeComponent();
        }

        public void Init(MeasurementAxis axis)
        {
            _Axis = axis;
            if (_Axis != null)
            {
                lbl_name.Text = string.Format("{0}:", _Axis.AxisSet.AxisName);
                MeasurementMotion motion = _Axis.Motion as MeasurementMotion;
                motion.PositionListener.PositionDevChanged += PositionListener_PositionDevChanged;
            }
            if (_Axis != null)
            {
                MeasurementPositionListener lis = _Axis.Motion.PositionListener as MeasurementPositionListener;
                SetPosition(lis.PositionDev[_Axis.AxisType]);
            }
        }

        private void lbl_position_Click(object sender, EventArgs e)
        {

        }
    }
}
