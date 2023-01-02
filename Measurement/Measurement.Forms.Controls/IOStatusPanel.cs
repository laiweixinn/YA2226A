using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Motions;
using DY.CNC.Core;
using System.Threading;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class IOStatePanel : UserControl
    {
        private bool _CurrentStatus=false;

        public IOStatePanel()
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
                    if (_IO.CardID == CardIDs.D&& _IO.IO == 16)
                    {

                    }

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
                    if (_IO.CardID == CardIDs.D && _IO.IO == 16)
                    {

                    }
                    if (_IO.CardID == CardIDs.D && _IO.IO == 17)
                    {

                    }
                    MotionIOListener.IOStatusChangedEventArgs oStatusChangedEventArg = e as MotionIOListener.IOStatusChangedEventArgs;
                    if (_IO.CardID == CardIDs.D && _IO.IO == 0)
                    {
                        //MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IO.CardID) as MeasurementMotion;
                        //bool[] ioins = motion.IOListener.IOInStatus;
                        //string s = "";
                        //for (int i = 0; i < ioins.Length; i++)
                        //{
                        //    if(i!=0 && i !=16 && i != 17 && i != 1 && i != 2)
                        //    { continue; }
                        //    s += "IN" + i + (ioins[i] ? 1 : 0) + ",";
                        //}
                        //MeasurementContext.OutputError(s);
                        //Thread.Sleep(3000);
                    }
                    if (oStatusChangedEventArg.Index==_IO.IO)
                    {
                        try
                        {
                            Invoke(new MethodInvoker(()=> SetIOStatus(oStatusChangedEventArg.NewStatus)));
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
                if (_IO!=null)
                {
                    lbl_iostate.Text = _IO.Name;
                    MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IO.CardID) as MeasurementMotion;
                    string IOIndexFullName = IO.CardID.ToString();
                    if (motion!=null)
                    {
                        if (_IO.IsIOEx)
                        {
                            motion.IOListener.IOInStatusExChanged += IOListener_IOInStatusExChanged;
                            motion.IOListener.IOOutStatusExChanged += IOListener_IOOutStatusExChanged;
                            IOIndexFullName = IOIndexFullName + _IO.IO.ToString() + "e";
                        }
                        else
                        {
                            motion.IOListener.IOInStatusChanged += IOListener_IOInStatusChanged;
                            motion.IOListener.IOOutStatusChanged += IOListener_IOOutStatusChanged;
                            IOIndexFullName = IOIndexFullName + _IO.IO.ToString();
                        }
                        lb_IOIndexDisp.Text = IOIndexFullName;
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
            if(this.lbl_iostate.Text == "中折弯压合气缸下感应")
            {

            }
            if (!(_IO==null?false:_IO.IsValid))
            {
                lbl_iostate.BackColor = Color.Green;
            }
            else if (status!=_IO.Status)
            {
                lbl_iostate.BackColor = Color.Green;
            }
            else
            {
                lbl_iostate.BackColor = Color.Red;
            }
        }

        private void lbl_iostate_Click(object sender, EventArgs e)
        {
            if (_IsOutPut && MeasurementContext.Worker!=null && _IO!=null)
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

        private void IOStatePanel_Load(object sender, EventArgs e)
        {
            if (MeasurementContext.Worker==null?false:_IO!=null)
            {
                MeasurementMotion motion = MeasurementContext.Worker.GetMotion(_IO.CardID) as MeasurementMotion;
                string IOIndexFullName = IO.CardID.ToString();
                if (IO.Name== "左撕膜平台真空吸")
                {

                }
                if (this.Name== "IO_Tear1Suck")
                {

                }
                if (this.Name== "IO_Tear2Suck")
                {

                }

                if ( _IO.IsValid)
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
                        IOIndexFullName = IOIndexFullName + _IO.IO.ToString() + "e";
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
                        IOIndexFullName = IOIndexFullName + _IO.IO.ToString();
                    }
                    lb_IOIndexDisp.Text = IOIndexFullName;
                }
            }
        }
    }
}
