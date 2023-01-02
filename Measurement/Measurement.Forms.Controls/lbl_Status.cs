using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Motions;
using DY.CNC.Core;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class lbl_status : UserControl
    {

        private bool _CurrentStatus = false;
        public lbl_status()
        {
            InitializeComponent();
        }


        private void IOListener_IOInStatusExChanged(object sender, EventArgs e)
        {
            if (!_IsOutPut)
            {
                if (_IO == null ? false : _IO.IsValid)
                {
                    MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                    if (oStatusChangedEventArg.Index == _IO.IO)
                    {
                        try
                        {
                            Invoke(new MethodInvoker(() => SetIOStatus(oStatusChangedEventArg.NewStatus)));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private void IOListener_IOOutStatusExChanged(object sender, EventArgs e)
        {
            if (IsOutPut)
            {
                if (_IO == null ? false : _IO.IsValid)
                {
                    MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                    if (oStatusChangedEventArg.Index == _IO.IO)
                    {
                        try
                        {
                            Invoke(new MethodInvoker(() => SetIOStatus(oStatusChangedEventArg.NewStatus)));
                            _CurrentStatus = oStatusChangedEventArg.NewStatus;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private void IOListener_IOOutStatusChanged(object sender, EventArgs e)
        {
            if (_IsOutPut)
            {
                if (_IO == null ? false : _IO.IsValid)
                {
                    MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                    if (oStatusChangedEventArg.Index == _IO.IO)
                    {
                        try
                        {
                            Invoke(new MethodInvoker(() => SetIOStatus(oStatusChangedEventArg.NewStatus)));
                            _CurrentStatus = oStatusChangedEventArg.NewStatus;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private void IOListener_IOInStatusChanged(object sender, EventArgs e)
        {
            if (!_IsOutPut)
            {
                if (_IO == null ? false : _IO.IsValid)
                {
                    MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                    if (oStatusChangedEventArg.Index == _IO.IO)
                    {
                        try
                        {
                            Invoke(new MethodInvoker(() => SetIOStatus(oStatusChangedEventArg.NewStatus)));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private MeasurementConfig.ConfigIO _IO = null;

        public MeasurementConfig.ConfigIO IO
        {
            get
            {
                return _IO;
            }
            set
            {
                _IO = value;
                if (_IO != null)
                {

                    MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IO.CardID) as MeasurementMotion;
                    if (motion != null)
                    {
                        if (_IO.IsIOEx)
                        {
                            motion.IOListener.IOInStatusExChanged += IOListener_IOInStatusExChanged;
                            motion.IOListener.IOOutStatusExChanged += IOListener_IOOutStatusExChanged;
                        }
                        else
                        {
                            motion.IOListener.IOInStatusChanged += IOListener_IOInStatusChanged;
                            motion.IOListener.IOOutStatusChanged += IOListener_IOOutStatusChanged;
                        }
                    }
                }
            }
        }

        private bool _IsOutPut = false;

        public bool IsOutPut
        {
            get
            {
                return _IsOutPut;
            }
            set
            {
                _IsOutPut = value;
                if (_IsOutPut)
                {
                    Cursor = Cursors.Hand;
                    lbl_iostate.Cursor = Cursors.Hand;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public void SetIOStatus(bool status)
        {
            if (!(_IO == null ? false : _IO.IsValid))
            {
                lbl_iostate.BackColor = Color.Gray;
            }
            else if (status != _IO.Status)
            {
                lbl_iostate.BackColor = Color.Green;
            }
            else
            {
                lbl_iostate.BackColor = Color.Red;
            }
        }





        private void lbl_iostate_Click_1(object sender, EventArgs e)
        {
            if (_IsOutPut && MeasurementContext.Worker != null && _IO != null)
            {
                if (!_IO.IsIOEx)
                {
                    MeasurementContext.Worker.SetIOOutReverse(_IO);
                }
                else
                {
                    MeasurementContext.Worker.SetIOOutReverse(_IO);
                }
                //MeasurementContext.OutputMessage(string.Format("{0}{1}[H]" ,_IO.Name , (_CurrentStatus ? "开" : "关")));
            }
        }

        private void lbl_status_Load(object sender, EventArgs e)
        {
            if (MeasurementContext.Worker == null ? false : _IO != null)
            {
                MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IO.CardID) as MeasurementMotion;
                if (_IO.IsValid)
                {
                    MeasurementIOListener motionIOListener = motion.IOListener;
                    if (_IO.IsIOEx)
                    {
                        if (!_IsOutPut)
                        {
                            SetIOStatus(motionIOListener.IoInStatusEx[_IO.IO]);
                        }
                        else
                        {
                            SetIOStatus(motionIOListener.IoOutStatusEx[_IO.IO]);
                        }
                    }
                    else
                    {
                        if (!_IsOutPut)
                        {
                            SetIOStatus(motionIOListener.IOInStatus[_IO.IO]);
                        }
                        else
                        {
                            SetIOStatus(motionIOListener.IOOutStatus[_IO.IO]);
                            _CurrentStatus = motionIOListener.IOOutStatus[_IO.IO];
                        }
                    }
                }
            }
        }
    }




}
