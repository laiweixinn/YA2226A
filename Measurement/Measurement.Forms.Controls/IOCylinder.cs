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
using LZ.CNC.Measurement.Core.Motions;
using DY.CNC.Core;


namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class IOCylinder : UserControl
    {
        public IOCylinder()
        {
            InitializeComponent();
        }

        private bool _CurrentStatus = false;



        private void IOListener1_IOInStatusExChanged(object sender, EventArgs e)
        {
            if (_IO1 == null ? false : _IO1.IsValid)
            {
                MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                if (oStatusChangedEventArg.Index == _IO1.IO)
                {
                    try
                    {
                        Invoke(new MethodInvoker(() => SetIOStatus1(oStatusChangedEventArg.NewStatus)));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void IOListener1_IOInStatusChanged(object sender, EventArgs e)
        {
            if (_IO1 == null ? false : _IO1.IsValid)
            {
                MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                if (oStatusChangedEventArg.Index == _IO1.IO)
                {
                    try
                    {
                        Invoke(new MethodInvoker(() => SetIOStatus1(oStatusChangedEventArg.NewStatus)));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }



        private void IOListener2_IOInStatusExChanged(object sender, EventArgs e)
        {
            if (_IO2 == null ? false : _IO2.IsValid)
            {
                MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                if (oStatusChangedEventArg.Index == _IO2.IO)
                {
                    try
                    {
                        Invoke(new MethodInvoker(() => SetIOStatus2(oStatusChangedEventArg.NewStatus)));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void IOListener2_IOInStatusChanged(object sender, EventArgs e)
        {
            if (_IO2 == null ? false : _IO2.IsValid)
            {
                MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                if (oStatusChangedEventArg.Index == _IO2.IO)
                {
                    try
                    {
                        Invoke(new MethodInvoker(() => SetIOStatus2(oStatusChangedEventArg.NewStatus)));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
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
                    btn_m.Text = _IO.Name;
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



        private MeasurementConfig.ConfigIO _IO1 = null;

        public MeasurementConfig.ConfigIO IO1
        {
            get
            {
                return _IO1;
            }
            set
            {
                _IO1 = value;
                if (_IO1 != null)
                {
                   
                    MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IO1.CardID) as MeasurementMotion;
                    if (motion != null)
                    {
                        if (_IO1.IsIOEx)
                        {
                            motion.IOListener.IOInStatusExChanged += IOListener1_IOInStatusExChanged;
                           // motion.IOListener.IOOutStatusExChanged += IOListener_IOOutStatusExChanged;
                        }
                        else
                        {
                            motion.IOListener.IOInStatusChanged += IOListener1_IOInStatusChanged;
                           // motion.IOListener.IOOutStatusChanged += IOListener_IOOutStatusChanged;
                        }
                    }
                }
            }
        }


        private MeasurementConfig.ConfigIO _IO2 = null;

        public MeasurementConfig.ConfigIO IO2
        {
            get
            {
                return _IO2;
            }
            set
            {
                _IO2 = value;
                if (_IO2 != null)
                {

                    MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IO2.CardID) as MeasurementMotion;
                    if (motion != null)
                    {
                        if (_IO2.IsIOEx)
                        {
                            motion.IOListener.IOInStatusExChanged += IOListener2_IOInStatusExChanged;                          
                        }
                        else
                        {
                            motion.IOListener.IOInStatusChanged += IOListener2_IOInStatusChanged;                          
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
                    btn_m.Cursor = Cursors.Hand;
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
                btn_m.BackColor = Color.Gray;
            }
            else if (status != _IO.Status)
            {
                btn_m.BackColor = Color.Green;
            }
            else
            {
                btn_m.BackColor = Color.Red;
            }
        }


        public void SetIOStatus1(bool status)
        {
            if (!(_IO == null ? false : _IO1.IsValid))
            {
                lbl_sen_up.BackColor = Color.Gray;
               
            }
            else if (status != _IO1.Status)
            {
                lbl_sen_up.BackColor = Color.Green;
            }
            else
            {
                lbl_sen_up.BackColor = Color.Red;
            }
        }

        public void SetIOStatus2(bool status)
        {
            if (!(_IO == null ? false : _IO2.IsValid))
            {
                lbl_sen_down.BackColor = Color.Gray;
            }
            else if (status != _IO2.Status)
            {
                lbl_sen_down.BackColor = Color.Green;
            }
            else
            {
                lbl_sen_down.BackColor = Color.Red;
            }
        }



        private void btn_m_Click(object sender, EventArgs e)
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
            }
        }

        private void IOCylinder_Load(object sender, EventArgs e)
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

            if (MeasurementContext.Worker == null ? false : _IO1 != null)
            {
                MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IO.CardID) as MeasurementMotion;
                if (_IO1.IsValid)
                {
                    MeasurementIOListener motionIOListener = motion.IOListener;
                    if (_IO1.IsIOEx)
                    {
                        SetIOStatus1(motionIOListener.IoInStatusEx[_IO1.IO]);

                    }
                    else
                    {
                        SetIOStatus1(motionIOListener.IOInStatus[_IO1.IO]);
                    }
                }
            }

            if (MeasurementContext.Worker == null ? false : _IO1 != null)
            {
                MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IO.CardID) as MeasurementMotion;
                if (_IO2.IsValid)
                {
                    MeasurementIOListener motionIOListener = motion.IOListener;
                    if (_IO2.IsIOEx)
                    {
                        SetIOStatus2(motionIOListener.IoInStatusEx[_IO2.IO]);
                    }
                    else
                    {
                        SetIOStatus2(motionIOListener.IOInStatus[_IO2.IO]);
                    }
                }
            }


        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
