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
    public partial class IOInIn : UserControl
    {
        private bool _CurrentStatus = false;
        public IOInIn()
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



        #region 
        private void IOListenerIN_IOInStatusExChanged(object sender, EventArgs e)
        {
            
                if (_IOIn == null ? false : _IOIn.IsValid)
                {
                    MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                    if (oStatusChangedEventArg.Index == _IOIn.IO)
                    {
                        try
                        {
                            Invoke(new MethodInvoker(() => SetIOStausIn(oStatusChangedEventArg.NewStatus)));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
           
        }


        private void IOListenerIN_IOInStatusChanged(object sender, EventArgs e)
        {
          
                if (_IOIn == null ? false : _IOIn.IsValid)
                {
                    MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                    if (oStatusChangedEventArg.Index == _IOIn.IO)
                    {
                        try
                        {
                            Invoke(new MethodInvoker(() => SetIOStausIn(oStatusChangedEventArg.NewStatus)));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }          
        }
        #endregion


        #region 
        private void IOListenerIN2_IOInStatusExChanged(object sender, EventArgs e)
        {
            
                if (_IOIn2 == null ? false : _IOIn2.IsValid)
                {
                    MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                    if (oStatusChangedEventArg.Index == _IOIn2.IO)
                    {
                        try
                        {
                            Invoke(new MethodInvoker(() => SetIOStausIn2(oStatusChangedEventArg.NewStatus)));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
           
        }


        private void IOListenerIN2_IOInStatusChanged(object sender, EventArgs e)
        {

            if (_IOIn2 == null ? false : _IOIn2.IsValid)
            {
                MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                if (oStatusChangedEventArg.Index == _IOIn2.IO)
                {
                    try
                    {
                        Invoke(new MethodInvoker(() => SetIOStausIn2(oStatusChangedEventArg.NewStatus)));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        #endregion

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
                    lbl_iostate.Text = _IO.Name;
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


        private MeasurementConfig.ConfigIO _IOIn = null;

        public MeasurementConfig.ConfigIO IOIn
        {
            get
            {
                return _IOIn;
            }
            set
            {
                _IOIn = value;
                if (_IOIn != null)
                {

                    MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IOIn.CardID) as MeasurementMotion;
                    if (motion != null)
                    {
                        if (_IOIn.IsIOEx)
                        {
                            motion.IOListener.IOInStatusExChanged += IOListenerIN_IOInStatusExChanged;

                        }
                        else
                        {
                            motion.IOListener.IOInStatusChanged += IOListenerIN_IOInStatusChanged;
                        }
                    }
                }
            }



        }


        private MeasurementConfig.ConfigIO _IOIn2 = null;

        public MeasurementConfig.ConfigIO IOIn2
        {
            get
            {
                return _IOIn2;
            }
            set
            {
                _IOIn2 = value;
                if (_IOIn2 != null)
                {

                    MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IOIn2.CardID) as MeasurementMotion;
                    if (motion != null)
                    {
                        if (_IOIn2.IsIOEx)
                        {
                            motion.IOListener.IOInStatusExChanged += IOListenerIN2_IOInStatusExChanged;

                        }
                        else
                        {
                            motion.IOListener.IOInStatusChanged += IOListenerIN2_IOInStatusChanged;
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


        public void SetIOStausIn(bool status)
        {
            if (!(_IOIn == null ? false : _IOIn.IsValid))
            {
                InstatusUP.BackColor = Color.Gray;
            }
            else if (status != _IOIn.Status)
            {
                InstatusUP.BackColor = Color.Green;
            }
            else
            {
                InstatusUP.BackColor = Color.Red;
            }

        }

        public void SetIOStausIn2(bool status)
        {
            if (!(_IOIn2 == null ? false : _IOIn2.IsValid))
            {
                InstatusDown.BackColor = Color.Gray;
            }
            else if (status != _IOIn2.Status)
            {
                InstatusDown.BackColor = Color.Green;
            }
            else
            {
                InstatusDown.BackColor = Color.Red;
            }

        }

           
        private void lbl_iostate_Click(object sender, EventArgs e)
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

        private void IOInIn_Load(object sender, EventArgs e)
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
                            lbl_iostate.Enabled = false;
                            SetIOStatus(motionIOListener.IoInStatusEx[_IO.IO]);
                        }
                        else
                        {
                            lbl_iostate.Enabled = true;
                            SetIOStatus(motionIOListener.IoOutStatusEx[_IO.IO]);
                        }
                    }
                    else
                    {
                        if (!_IsOutPut)
                        {
                            lbl_iostate.Enabled = false;
                            SetIOStatus(motionIOListener.IOInStatus[_IO.IO]);
                        }
                        else
                        {
                            lbl_iostate.Enabled = true;
                            SetIOStatus(motionIOListener.IOOutStatus[_IO.IO]);
                            _CurrentStatus = motionIOListener.IOOutStatus[_IO.IO];
                        }
                    }
                }
            }

            if (MeasurementContext.Worker == null ? false : _IOIn != null)
            {
                MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IOIn.CardID) as MeasurementMotion;
                if (_IOIn.IsValid)
                {
                    MeasurementIOListener motionIOListener = motion.IOListener;
                    if (_IO.IsIOEx)
                    {

                        SetIOStausIn(motionIOListener.IoInStatusEx[_IOIn.IO]);

                    }
                    else
                    {
                        SetIOStausIn(motionIOListener.IOInStatus[_IOIn.IO]);
                    }
                }
            }

            if (MeasurementContext.Worker == null ? false : _IOIn2 != null)
            {
                MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IOIn2.CardID) as MeasurementMotion;
                if (_IOIn2.IsValid)
                {
                    MeasurementIOListener motionIOListener = motion.IOListener;
                    if (_IOIn2.IsIOEx)
                    {

                        SetIOStausIn2(motionIOListener.IoInStatusEx[_IOIn.IO]);

                    }
                    else
                    {
                        SetIOStausIn2(motionIOListener.IOInStatus[_IOIn.IO]);
                    }
                }
            }
        }
    }
}
