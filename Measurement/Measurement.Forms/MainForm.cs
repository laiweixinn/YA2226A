using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using DY.Core.Util;
using DY.Core;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using DY.Core.Forms;
using DY.CNC.Core;
using LZ.CNC.UserLevel;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Motions;
using LZ.CNC.Measurement.Core.Events;
using LZ.CNC.Measurement.Forms.Controls;

namespace LZ.CNC.Measurement.Forms
{
    public partial class MainForm : Form
    {
        private DateTime _StartTime = DateTime.Now;

        private DateTime _TimeFlag = DateTime.Now;
        private DateTime _TimePre = DateTime.Now;
        private FrmIO _IoForm;

        private FrmSet _SetForm;

        private AlarmForm _AlarmForm;

        private FrmCapacity _frCapacity;

        private FrDebug _frDebug;

        private FileForm _FileForm;

        private CapacityForm _CapacityForm;

        private StatisticsForm _StatisticsForm;

        private TabForm[] _TabForms;

        private DateTime dateTime = DateTime.Now;

        private UserManagement _Management = null;

        private Thread thdTearSuck = null;
        private Thread AirAndVacumnCheckThread = null;

        public MeasurementWorker worker => MeasurementContext.Worker;
        public MainForm()
        {
            InitializeComponent();
            SplashScreen.Show(typeof(FrmSplashScreen));

            MeasurementContext.Init();




            _Management = MeasurementContext.UesrManage;
            _IoForm = new FrmIO();
            _AlarmForm = new AlarmForm();
            _frCapacity = new FrmCapacity();
            _SetForm = new FrmSet();
            _frDebug = new FrDebug();
            _FileForm = new FileForm();
            _CapacityForm = new CapacityForm();
            _StatisticsForm = new StatisticsForm();
            _TabForms = new TabForm[] { _IoForm, _FileForm, _SetForm, _frCapacity, _frDebug, _StatisticsForm, _AlarmForm };

            TabForm[] _tabForm = _TabForms;
            tabControlEx1_SelectedIndexChanged(tabControlEx1, null);
            for (int i = 0; i < _tabForm.Length; i++)
            {
                _tabForm[i].InvokeLoginChanged();
            }
            if (!AppUtil.AppChecked())
            {
                Environment.Exit(1);
            }


            RefreshUI();



        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            timer1.Start();
            timer2.Start();
            FileInfo fi = new FileInfo(GetType().Assembly.Location);
            DateTime dateTime = fi.LastWriteTime;
            label_version.Text = "版本:V" + dateTime.ToString("yyyyMMddHHmm");

            if (DateTime.Now >= worker.Recipe.CapacityTime && DateTime.Now < worker.Recipe.CapacityTime.AddHours(12))
            {
                _TimePre = _StartTime - worker.Capacity.DayFreeTime;
            }
            else
            {
                _TimePre = _StartTime - worker.Capacity.NightFreeTime;
            }
            MeasurementContext.MessageOutput += MeasurementContext_MessageOutput;
            InitSMTotalControl();
            worker.ConnectNet();

            worker.MessageShow += Worker_MessageShow;
            worker.WorkStatusChanged += Worker_WorkStatusChanged;
            worker.ExButtonDown += Worker_ExButtonDown;
            worker.RecipeChanged += Worker_RecipeChanged;
            worker.FeedMsgArrived += Worker_FeedMsgArrived;
            worker.Tear1MsgArrived += Worker_Tear1MsgArrived;
            worker.Tear2MsgArrived += Worker_Tear2MsgArrived;
            worker.Tear3MsgArrived += Worker_Tear3MsgArrived;
            worker.Bend1MsgArrived += Worker_Bend1MsgArrived;
            worker.Bend2MsgArrived += Worker_Bend2MsgArrived;
            worker.Bend3MsgArrived += Worker_Bend3MsgArrived;
            worker.Bend1DWArrived += Worker_Bend1DWArrived;
            worker.Bend2DWArrived += Worker_Bend2DWArrived;
            worker.Bend3DWArrived += Worker_Bend3DWArrived;
            worker.SMTotalUpdateChange += SMTotalUpdate;
            worker.TranferMsgArrived += Worker_TransferMsgArrived;
            worker.DischargeMsgArrived += Worker_DischargeMsgArrived;
            worker.ChartResetChange += ChartReset;
            worker.ChartUpdateChange += ChartUpdate;
            worker.TTArrived += Worker_TTArrived;
            worker.workstatus = WorkStatuses.Pending;
            worker.OnWorkStatusChanged();

            AxisPositionListen();
            Worker_RecipeChanged(this, null);
            LoginTypesChanged();

            worker.ConnectCard();

            thdTearSuck = new Thread(worker.TearSuckListen); //监控线程
            thdTearSuck.IsBackground = true;
            thdTearSuck.Start();


            AirAndVacumnCheckThread = new Thread(worker.AirAndVacumnCheck);//检测总气压负压
            AirAndVacumnCheckThread.IsBackground = true;
            AirAndVacumnCheckThread.Start();



            SplashScreen.Close();
            MeasurementContext.Worker.CanSetIOOut(MeasurementContext.Config.AllStepMotorServoOn_IOOutEx, true);
            MeasurementContext.Worker.CanSetIOOut(MeasurementContext.Config.Left_WServoOn_IOOutEx, true);
            MeasurementContext.Worker.CanSetIOOut(MeasurementContext.Config.Mid_WServoOn_IOOutEx, true);
            MeasurementContext.Worker.CanSetIOOut(MeasurementContext.Config.Right_WServoOn_IOOutEx, true);
            MeasurementContext.Worker.CanSetIOOut(MeasurementContext.Config.SM_AOIServoOn_IOOutEx, true);
            MeasurementContext.Worker.CanSetIOOut(MeasurementContext.Config.DischargZServoOn_IOOutEx, true);
            MeasurementContext.Worker.CanSetIOOut(MeasurementContext.Config.DischargZbrak_IOOutEx, true);
            MeasurementContext.Worker.SetIOOut(MeasurementContext.Config.CIMCOR_Cylinder, true);//打开离子风棒
            btn_ManualLoad.Visible = worker.Config.IsManualLoad;
            Btn_NGLine.Visible = worker.Config.IsUseNGLineButton;

            realChart1.Init();
            realChart2.Init();
            realChart3.Init();
            if (!worker.IsReset)
            {
                if (MessageBoxEx.ShowSystemQuestion("设备未复位，是否现在复位？"))
                {
                    Reset();
                }
            }


        }


        private void InitSMTotalControl()//tfx 20210916
        {
            sts_SMTotal1.MaxValue = MeasurementContext.Config.SMUseMax;
            sts_SMTotal1.Hour = "左";
            sts_SMTotal1.value = MeasurementContext.Config.LeftSMUseCount;
            lbl_LeftReplaceTime.Text = "更换记录: " + MeasurementContext.Config.LeftSMReplaceTime.ToString("MM-dd  HH:mm");


            sts_SMTotal2.MaxValue = MeasurementContext.Config.SMUseMax;
            sts_SMTotal2.Hour = "中";
            sts_SMTotal2.value = MeasurementContext.Config.MidSMUseCount;
            lbl_MidReplaceTime.Text = "更换记录: " + MeasurementContext.Config.MidSMReplaceTime.ToString("MM-dd  HH:mm");

            sts_SMTotal3.MaxValue = MeasurementContext.Config.SMUseMax;
            sts_SMTotal3.Hour = "右";
            sts_SMTotal3.value = MeasurementContext.Config.RightSMUseCount;
            lbl_RightReplaceTime.Text = "更换记录: " + MeasurementContext.Config.RightSMReplaceTime.ToString("MM-dd  HH:mm");

        }


        #region 事件响应程序

        #region  Free Event


        /// <summary>
        /// 刷新撕膜耗材统计    tfx 20210916
        /// </summary>
        /// <param name="staton">0 1 2 代表左中右工位</param>
        /// <param name="value"></param>
        private void SMTotalUpdate(int staton, int value)
        {
            switch (staton)
            {
                case 0:
                    sts_SMTotal1.value = value;
                    break;

                case 1:
                    sts_SMTotal2.value = value;
                    break;

                case 2:
                    sts_SMTotal3.value = value;
                    break;


            }





        }

        private void ChartReset(int station)
        {
            switch (station)
            {
                case 0:
                    realChart1.Reset();
                    break;
                case 1:
                    realChart2.Reset();
                    break;
                case 2:
                    realChart3.Reset();
                    break;

            }

        }

        private void ChartUpdate(double value1, double value2, int station)
        {
            switch (station)
            {
                case 0:
                    realChart1.Add(value1, value2);
                    break;
                case 1:
                    realChart2.Add(value1, value2);
                    break;
                case 2:
                    realChart3.Add(value1, value2);
                    break;
            }

        }

        private void Worker_NinePointBegin(object sender, EventArgs e)
        {
            tabControlEx1.SelectedTab = tabPage1;
        }

        private void Worker_PointChanged(object sender, EventArgs e)
        {
            PointChangedEventArgs args = e as PointChangedEventArgs;
            //lbl_camx.Text = args.CamX.ToString();
            //lbl_camy.Text = args.CamY.ToString();
            //lbl_camresult.Text = args.CamOK ? "OK" : "NG";
            //lbl_camresult.BackColor = args.CamOK ? Color.GreenYellow : Color.Red;

            if (!args.CamOK)
            {
                if (new VisionFailProc(args.PointNum).ShowDialog() != DialogResult.OK)
                {
                    // Worker.PhotoHander = PhotoFailHander.Cancel;
                    MeasurementContext.OutputMessage("手动对位取消！");
                }
                else
                {
                    // Worker.PhotoHander = PhotoFailHander.Conti;
                    MeasurementContext.OutputMessage("手动对位执行，继续生产！");
                }
            }
        }

        private void Worker_ExButtonDown(object sender, EventArgs e)
        {
            ButtonUpDownEventArgs args = e as ButtonUpDownEventArgs;
            if (args.ButtonType == ButtonTypes.RESET)
            {
                Reset();
            }
        }

        private void Laser_MessageOutPut(object sender, EventArgs e)
        {
            MessageOutputEventArgs me = e as MessageOutputEventArgs;
            string[] datas = me.Message.Split(',');
        }

        private void Worker_PointDataArrived(object sender, EventArgs e)
        {
            PointDataArrivedEventArgs args = e as PointDataArrivedEventArgs;

        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            RefreshUI();
        }

        private void Laser_ConnectStateChanged(object sender, EventArgs e)
        {

        }
        #endregion 

        private void Worker_RecipeChanged(object sender, EventArgs e)
        {
            stsinfo_sysfile.Text = string.Format("【{0}】", MeasurementContext.Data.CurrentRecipeData.Name);
            lbl_workmodel.Text = string.Format("【{0}】", MeasurementContext.Data.CurrentRecipeData.WorkModel == 0 ?
                "XY方式" : (MeasurementContext.Data.CurrentRecipeData.WorkModel == 1 ? "YX方式" : "YY方式"));


        }

        private void Worker_FeedMsgArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_feedmsg.InvokeRequired)
            {
                lbl_feedmsg.BeginInvoke((MethodInvoker)delegate
                    {
                        lbl_feedmsg.Text = msg;
                    });
            }
            else
            {
                lbl_feedmsg.Text = msg;
            }

        }

        private void Worker_Tear1MsgArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_tear1msg.InvokeRequired)
            {
                lbl_tear1msg.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_tear1msg.Text = msg;
                });
            }
            else
                lbl_tear1msg.Text = msg;
        }

        private void Worker_Tear2MsgArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_tear2msg.InvokeRequired)
            {
                lbl_tear2msg.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_tear2msg.Text = msg;
                });
            }
            else
                lbl_tear2msg.Text = msg;
        }


        private void Worker_Tear3MsgArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_tear3msg.InvokeRequired)
            {
                lbl_tear3msg.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_tear3msg.Text = msg;
                });
            }
            else
                lbl_tear3msg.Text = msg;
        }


        private void Worker_Bend1MsgArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_bend1msg.InvokeRequired)
            {
                lbl_bend1msg.BeginInvoke((MethodInvoker)delegate
               {
                   lbl_bend1msg.Text = msg;
               });
            }
            else
                lbl_bend1msg.Text = msg;
        }


        private void Worker_Bend2MsgArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_bend2msg.InvokeRequired)
            {
                lbl_bend2msg.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_bend2msg.Text = msg;
                });
            }
            else
                lbl_bend1msg.Text = msg;
        }


        private void Worker_Bend3MsgArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_bend3msg.InvokeRequired)
            {
                lbl_bend3msg.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_bend3msg.Text = msg;
                });
            }
            else
                lbl_bend3msg.Text = msg;
        }

        private void Worker_Bend1DWArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_bend1num.InvokeRequired)
            {
                lbl_bend1num.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_bend1num.Text = msg;
                });
            }
            else
                lbl_bend1num.Text = msg;
        }

        private void Worker_Bend2DWArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_bend2num.InvokeRequired)
            {
                lbl_bend2num.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_bend2num.Text = msg;
                });
            }
            else
                lbl_bend2num.Text = msg;
        }


        private void Worker_Bend3DWArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_bend3num.InvokeRequired)
            {
                lbl_bend3num.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_bend3num.Text = msg;
                });
            }
            else
                lbl_bend3num.Text = msg;
        }

        private void Worker_DischargeMsgArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_dischargemsg.InvokeRequired)
            {
                lbl_dischargemsg.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_dischargemsg.Text = msg;
                });
            }
            else
            {
                lbl_dischargemsg.Text = msg;
            }
        }


        private void Worker_TTArrived(object sender, double[] e)
        {
            double[] TT = e;
            string CTT = TT[0].ToString("0.00");
            string TTT = TT[1].ToString("0.00");
            if (this.InvokeRequired)
            {
                lblCT.BeginInvoke((MethodInvoker)delegate
                {
                    lblCT.Text = CTT + "S";
                });
                lblTT.BeginInvoke((MethodInvoker)delegate
                {
                    lblTT.Text = TTT + "S";
                });
            }
            else
            {
                lblCT.Text = CTT + "S";
                lblTT.Text = TTT + "S";
            }
        }

        private void Worker_TransferMsgArrived(object sender, string e)
        {
            string msg = e;
            if (lbl_transfermsg.InvokeRequired)
            {
                lbl_transfermsg.BeginInvoke((MethodInvoker)delegate
                {
                    lbl_transfermsg.Text = msg;
                });
            }
            else
            {
                lbl_dischargemsg.Text = msg;
            }
        }



        private void Worker_WorkStatusChanged(object sender, EventArgs e)
        {
            //  btn_gounloadpos.Enabled = (Worker.WorkStatus != WorkStatuses.Running);

            //if (Worker.WorkStatus == WorkStatuses.Running)
            //{
            //    tabControlEx1.SelectedTab = tabPage1;
            //}
            RefreshUI();
        }

        #endregion

        private void Worker_MessageShow(object sender, EventArgs e)
        {
            MessageShowEventArgs messageShowEventArg = e as MessageShowEventArgs;
            try
            {
                BeginInvoke(new MethodInvoker(() => MessageBoxEx.ShowSystemMessage(messageShowEventArg.Message)));
            }
            catch (Exception)
            {
            }
        }

        private void MeasurementContext_MessageOutput(object sender, EventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(() =>
                {
                    MessageOutputEventArgs me = e as MessageOutputEventArgs;
                    if (string.IsNullOrEmpty(me.Message))
                    {
                    }
                    else
                    {
                        WriteLine(me.Message, me.IsError);
                    }
                }));
            }
            catch (Exception)//MethodInvoker
            {
            }
        }

        private void WriteLine(string msg, bool iserror)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                int i = txt_info.TextLength;
                if (i > 1024 * 10)
                {
                    txt_info.Clear();
                }
                i = txt_err.TextLength;
                if (i > 1024 * 10)
                {
                    txt_err.Clear();
                }
                if (iserror)
                {
                    i = txt_err.TextLength;
                    msg = string.Format("[{0}]: {1}\r\n", DateTime.Now.ToString("HH:mm:ss fff"), msg);
                    txt_err.AppendText(msg);
                    txt_err.Select(i, txt_err.TextLength - 1);
                    txt_err.SelectionStart = txt_err.TextLength;
                    txt_err.SelectionLength = 0;
                    txt_err.ScrollToCaret();
                }
                else
                {
                    i = txt_info.TextLength;
                    msg = string.Format("[{0}]: {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), msg);
                    txt_info.AppendText(msg);
                    txt_info.Select(i, txt_info.TextLength - 1);
                    //  txt_info.SelectionColor = (iserror ? Color.Red : Color.Black);
                    txt_info.SelectionStart = txt_info.TextLength;
                    txt_info.SelectionLength = 0;
                    txt_info.ScrollToCaret();
                }




            }
        }

        public void RefreshUI()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => _RefreshUI()));
            }
            else
            {
                _RefreshUI();
            }
        }

        public void _RefreshUI()
        {
            label1.BackColor = (worker.WorkStatus == WorkStatuses.Running ? Color.Green : worker.WorkStatus == WorkStatuses.Error ? Color.Red : Color.Yellow);
            label1.Text = string.Format("【{0}】", worker.WorkStatusString);
            if (worker.workstatus == WorkStatuses.Pausing)
            {
                worker.OpenYellowLight();
                worker.CloseGreenLight();
                worker.CloseRedLight();
            }

            if (worker.workstatus == WorkStatuses.Stopped || worker.workstatus == WorkStatuses.Idle || worker.workstatus == WorkStatuses.Homing || worker.workstatus == WorkStatuses.Pending)
            {
                worker.OpenYellowLight();
                worker.CloseGreenLight();
                worker.CloseRedLight();
            }

            if (worker.workstatus == WorkStatuses.Error)
            {
                worker.CloseYellowLight();
                worker.CloseGreenLight();
                worker.OpenRedLight();
            }

            if (worker.workstatus != WorkStatuses.Running)
            {
                btn_pause.Enabled = false;
                worker.CloseBlueStartLight();
            }
            else
            {
                btn_pause.Enabled = true;
                worker.OpenGreenLight();
                worker.CloseYellowLight();
                worker.CloseRedLight();
                worker.OpenBlueStartLight();
            }


            if (worker.workstatus != WorkStatuses.Homing)
            {
                worker.CloseYellowResetLight();
            }

            if (worker.workstatus == WorkStatuses.Running || worker.workstatus == WorkStatuses.Pausing || worker.workstatus == WorkStatuses.Stoping)
            {
                //rbt_debug.Enabled = false;
                //rbt_io.Enabled = false;
            }
            else
            {
                //rbt_debug.Enabled = true;
                //rbt_io.Enabled = true;
                worker.b_timespan_flag = true;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan timeSpan = DateTime.Now - _StartTime;
            label_date.Text = DateTime.Today.ToString("yyyy-MM-dd ") + DateTime.Now.ToString("HH:mm:ss");
            TimeSpan ts = DateTime.Now - _StartTime;
            label_runtime.Text = string.Format("运行时间 {0} 天 {1} 时 {2} 分 {3} 秒", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);

            Task.Run(() =>
            {
                worker.Capacity.CurrentTime = DateTime.Now;
                if (DateTime.Now.Day == 1) MeasurementContext.MonthCapacity.OnCurrentMonthChange();
                if ((DateTime.Now.Hour == worker.Recipe.CapacityTime.Hour && DateTime.Now.Minute < worker.Recipe.CapacityTime.Minute)
                    || DateTime.Now.Hour < worker.Recipe.CapacityTime.Hour)
                {
                    MeasurementContext.Worker.Recipe.CapacityTime = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:00", DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, worker.Recipe.CapacityTime.Hour, worker.Recipe.CapacityTime.Minute));
                }
                else
                {
                    MeasurementContext.Worker.Recipe.CapacityTime = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:00", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, worker.Recipe.CapacityTime.Hour, worker.Recipe.CapacityTime.Minute));
                }

            });
        }


        /// <summary>
        /// 控制来料皮带线
        /// </summary>
        private void CtrlInputProductLine()
        {
            if (worker.GetIOInStatus(worker.Config.UpstreamBeltOpticalIOINEx)
                && worker.GetIOInStatus(worker.Config.BeltOpticalIOIN)
                && worker.GetIOInStatus(worker.Config.BeltStartOpticalIOINEx))//三个光纤都有信号
            {
                worker.SetIOOut(MeasurementContext.Worker.Config.UpstreamSuplyBeltIOOut, true);//停止
            }
            else if (MeasurementContext.Worker.GetIOInStatus(MeasurementContext.Worker.Config.UpstreamBeltOpticalIOINEx)
                && MeasurementContext.Worker.GetIOInStatus(MeasurementContext.Worker.Config.BeltStartOpticalIOINEx)
                && !MeasurementContext.Worker.GetIOOutStatus(MeasurementContext.Worker.Config.SuplyBeltIOOut))//两个光纤有信号 本机流水线停止状态
            {
                MeasurementContext.Worker.SetIOOut(MeasurementContext.Worker.Config.UpstreamSuplyBeltIOOut, true);//停止
            }
            else
            {
                MeasurementContext.Worker.SetIOOut(MeasurementContext.Worker.Config.UpstreamSuplyBeltIOOut, false);//运行
            }

        }

        public void AxisPositionListen()
        {
            axisPositionListener1.Init(worker.Axis_LeftSM_X);
            axisPositionListener2.Init(worker.Axis_LeftSM_Y);
            axisPositionListener3.Init(worker.Axis_LeftSM_Z);

            axisPositionListener4.Init(worker.Axis_MidSM_X);
            axisPositionListener5.Init(worker.Axis_MidSM_Y);
            axisPositionListener6.Init(worker.Axis_MidSM_Z);

            axisPositionListener7.Init(worker.Axis_RightSM_X);
            axisPositionListener8.Init(worker.Axis_RightSM_Y);
            axisPositionListener9.Init(worker.Axis_RightSM_Z);

            axisPositionListener10.Init(worker.Axis_Load_X);
            axisPositionListener11.Init(worker.Axis_Transfer_X);
            axisPositionListener12.Init(worker.Axis_Discharge_X);

            axisPositionListener13.Init(worker.Axis_LeftBend_CCDX);
            axisPositionListener14.Init(worker.Axis_LeftBend_stgY);
            axisPositionListener15.Init(worker.Axis_LeftBend_DWX);
            axisPositionListener16.Init(worker.Axis_LeftBend_DWY);
            axisPositionListener17.Init(worker.Axis_LeftBend_DWR);
            axisPositionListener18.Init(worker.Axis_LeftBend_DWW);

            axisPositionListener24.Init(worker.Axis_MidBend_CCDX);
            axisPositionListener23.Init(worker.Axis_MidBend_stgY);
            axisPositionListener22.Init(worker.Axis_MidBend_DWX);
            axisPositionListener21.Init(worker.Axis_MidBend_DWY);
            axisPositionListener20.Init(worker.Axis_MidBend_DWR);
            axisPositionListener19.Init(worker.Axis_MidBend_DWW);

            axisPositionListener30.Init(worker.Axis_RightBend_CCDX);
            axisPositionListener29.Init(worker.Axis_RightBend_stgY);
            axisPositionListener28.Init(worker.Axis_RightBend_DWX);
            axisPositionListener27.Init(worker.Axis_RightBend_DWY);
            axisPositionListener26.Init(worker.Axis_RightBend_DWR);
            axisPositionListener25.Init(worker.Axis_RightBend_DWW);

            axisPositionListener31.Init(worker.Axis_LeftSM_W);
            axisPositionListener32.Init(worker.Axis_MidSM_W);
            axisPositionListener33.Init(worker.Axis_RightSM_W);
            axisPositionListener34.Init(worker.Axis_SMCCD_X);
            if (MeasurementContext.Config.IsLoadYAxisEnable)
            {
                axisPositionListener35.Init(worker.Axis_Load_Y);
            }
            if (!MeasurementContext.Config.IsLoadZCylinder)
            {
                axisPositionListener36.Init(worker.Axis_Load_Z);
            }
            if (!MeasurementContext.Config.IsTransferZCylinder)
            {
                axisPositionListener37.Init(worker.Axis_Transfer_Z);
            }

            axisPositionListener38.Init(worker.Axis_Discharge_Z);
        }


        #region 登录



        private void statusInfoPanel1_LinkClicked(object sender, EventArgs e)
        {
            _Management.LoginIn(false, _Management);
        }

        private void LoginTypesChanged()
        {
            lbl_Login.Text = (MeasurementContext.UesrManage.LoginType == LoginTypes.None) ? "登录" : "登出";
            label13.Text = _Management.LoginTypeToString;
            // stsinfo_user.IsLinked = (MeasurementContext.UesrManage.LoginType != LoginTypes.None);
            TabForm[] _forms = _TabForms;
            for (int i = 0; i < _forms.Length; i++)
            {
                _forms[i].InvokeLoginChanged();
            }
        }

        #endregion

        #region Button交互
        private void btn_showall_Click(object sender, EventArgs e)
        {
            new StatisticForm().ShowDialog();
        }
        private void btn_exit_Click(object sender, EventArgs e)   //退出
        {
            Close();
        }

        private void btn_paused_Click(object sender, EventArgs e)  //暂停
        {
            //  Worker.Pause();
        }

        private void btn_continuing_Click(object sender, EventArgs e)  //继续
        {
            //  Worker.Continuing();
        }

        private void btn_rst_Click(object sender, EventArgs e)  //复位
        {
            Reset();
        }

        private void btn_stop_Click(object sender, EventArgs e)  //停止
        {

            if (worker.WorkStatus == WorkStatuses.Running ? false : true)
            {
                worker.GetMotion(CardIDs.A).EndMotion();
                worker.GetMotion(CardIDs.A).StopSlowly();
            }
            else
            {
                worker.Stop();
            }
        }

        private void btnex_start_Click(object sender, EventArgs e)   //开始
        {
            //   Worker.Run();
        }



        private void btn_EMG_Click(object sender, EventArgs e)  //急停
        {
            worker.EmgStop();
        }



        private void btn_closebuzzer_Click(object sender, EventArgs e)  //蜂鸣器关闭
        {
            worker.CloseBuzzer();
        }

        private void stsinfo_errorclear_LinkClicked(object sender, EventArgs e)
        {
            worker.ErrorClear();
        }



        #endregion

        private void Reset()
        {
            bool flag = true;
            if ((worker.workstatus == WorkStatuses.Running || worker.workstatus == WorkStatuses.Pausing || worker.workstatus == WorkStatuses.Stoping))
            {
                MeasurementContext.OutputError("当前状态无法复位!");
                return;
            }

            foreach (MeasurementAxis axis in new MeasurementAxis[] {
                worker.MotionA.AxisX,
                worker.MotionA.AxisY,
                worker.MotionA.AxisZ,
                worker.MotionA.AxisU,
                worker.MotionA.AxisV,
                worker.MotionA.AxisW,
                worker.MotionA.AxisA,
                worker.MotionA.AxisB
            })
            {
                if (axis.IsALM)
                {
                    MeasurementContext.OutputError(axis.AxisSet.AxisName.ToString() + "伺服报警");
                    flag = false;
                }
            }

            foreach (MeasurementAxis axis in new MeasurementAxis[] {
                worker.MotionB.AxisX,
                worker.MotionB.AxisY,
                worker.MotionB.AxisZ,
                worker.MotionB.AxisU,
                worker.MotionB.AxisV,
                worker.MotionB.AxisW,
                worker.MotionB.AxisA,
                worker.MotionB.AxisB,
                worker.MotionB.AxisC,
                worker.MotionB.AxisD
            })
            {
                if (axis.IsALM)
                {
                    MeasurementContext.OutputError(axis.AxisSet.AxisName.ToString() + "伺服报警");
                    flag = false;
                }
            }

            foreach (MeasurementAxis axis in new MeasurementAxis[] {
                worker.MotionC.AxisX,
                worker.MotionC.AxisY,
                worker.MotionC.AxisZ,
                worker.MotionC.AxisU,
               worker.MotionC.AxisV,
                worker.MotionC.AxisW,
               worker.MotionC.AxisA,
               worker.MotionC.AxisB
            })
            {
                if (axis.IsALM)
                {
                    MeasurementContext.OutputError(axis.AxisSet.AxisName.ToString() + "伺服报警");
                    flag = false;
                }
            }

            foreach (MeasurementAxis axis in new MeasurementAxis[] {
                worker.MotionD.AxisX,
                worker.MotionD.AxisY,
                worker.MotionD.AxisZ,
                worker.MotionD.AxisU,
                worker.MotionD.AxisV,
                worker.MotionD.AxisW,
                worker.MotionD.AxisA,
                worker.MotionD.AxisB
            })
            {
                if (axis.IsALM)
                {

                    MeasurementContext.OutputError(axis.AxisSet.AxisName.ToString() + "伺服报警");
                    flag = false;
                }
            }
            if (!flag)
            {
                return;
            }
            GoHomeForm goHomeForm = new GoHomeForm();
            goHomeForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TabForm[] _forms = _TabForms;
            for (int i = 0; i < _forms.Length; i++)
            {
                _forms[i].InvokeTabClose();
            }
            Thread.Sleep(500);
            if ((worker.WorkStatus == WorkStatuses.Running || worker.WorkStatus == WorkStatuses.Stoping || worker.WorkStatus == WorkStatuses.Homing))
            {
                MessageBoxEx.ShowErrorMessage("设备运行中，请等待.");
                e.Cancel = true;
                return;
            }
            worker.CloseNetEvent();
            Thread.Sleep(300);
            worker.CloseNet();
            MeasurementContext.Data.Save();
            MeasurementContext.Config.Save();
        }




        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            worker.CanSetIOOut(worker.Config.AllStepMotorServoOn_IOOutEx, false);
            worker.CanSetIOOut(worker.Config.DischargZServoOn_IOOutEx, false);
            worker.CanSetIOOut(worker.Config.DischargZbrak_IOOutEx, false);
            worker.SetIOOut(MeasurementContext.Config.CIMCOR_Cylinder, false);//关闭离子风棒
            MeasurementContext.Worker.MotionA.MotionSet.SaveMotionSet("0");
            MeasurementContext.Worker.MotionB.MotionSet.SaveMotionSet("1");
            MeasurementContext.Worker.MotionC.MotionSet.SaveMotionSet("2");
            MeasurementContext.Worker.MotionD.MotionSet.SaveMotionSet("3");
            worker.CloseBeltMotor();
            worker.CloseDischargeBeltMotor();
            MeasurementContext.Worker.DisConnect();
        }

        private void rbt_io_CheckedChanged(object sender, EventArgs e)
        {

            switch (((RadioButton)sender).Name)
            {
                case "rbtn_product":
                    if (rbtn_product.Checked)
                    {
                        tabControlEx1.SelectedTab = tabPage1;
                    }
                    break;
                case "rbt_io":
                    if (rbt_io.Checked)
                    {
                        tabControlEx1.SelectedTab = tabPage2;
                    }
                    break;
                case "rbt_debug":
                    if (rbt_debug.Checked)
                        tabControlEx1.SelectedTab = tabPage4;

                    break;
                case "rbt_set":
                    if (rbt_set.Checked)
                        tabControlEx1.SelectedTab = tabPage3;
                    break;
                case "rbt_file":
                    if (rbt_file.Checked)
                        tabControlEx1.SelectedTab = tabPage5;
                    break;
                case "rbt_vision":
                    if (rbt_vision.Checked)
                        tabControlEx1.SelectedTab = tabPage6;
                    break;
                case "rbt_alarm":
                    if (rbt_alarm.Checked)
                        tabControlEx1.SelectedTab = tabPage7;
                    break;
                default:
                    break;
            }

        }


        private void tabControlEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = tabControlEx1.SelectedIndex;
            TabPage tabPage = tabControlEx1.SelectedTab;
            TabForm[] forms = _TabForms;

            for (int i = 0; i < forms.Length; i++)
            {
                forms[i].IsSelected = (index == i);
                forms[i].InvokeTabSelectChanged();
            }

            if (tabPage.Controls.Count == 0)
            {
                switch (index)
                {
                    case 0:
                        // FormUtil.AddFormToControl(tabPage, _AutoForm);
                        break;
                    case 1:
                        FormUtil.AddFormToControl(tabPage, _IoForm);
                        break;
                    case 2:
                        FormUtil.AddFormToControl(tabPage, _SetForm);
                        break;
                    case 3:
                        FormUtil.AddFormToControl(tabPage, _frDebug);
                        break;
                    case 4:
                        FormUtil.AddFormToControl(tabPage, _FileForm);
                        break;
                    case 5:
                        FormUtil.AddFormToControl(tabPage, _frCapacity);
                        break;
                    case 6:
                        FormUtil.AddFormToControl(tabPage, _AlarmForm);
                        break;
                    case 7:
                        FormUtil.AddFormToControl(tabPage, _AlarmForm);
                        break;
                    default:
                        break;
                }
            }
        }


        private void btn_reset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btn_stop_Click_1(object sender, EventArgs e)
        {
            worker.StopMachine();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {

            worker.RunHander();
            WriteLog("启动");
        }


        bool _isFrPauseShowing = false;
        private void btn_pause_Click(object sender, EventArgs e)
        {
            if (!_isFrPauseShowing)
            {
                _isFrPauseShowing = true;
                FrPause fr = new FrPause();               
                fr.Show();
                _isFrPauseShowing = false;
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            worker.ConnectNet();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            DataForm ddf = new DataForm();
            ddf.ShowDialog();
        }

        private void btn_Estop_Click(object sender, EventArgs e)
        {
            worker.EstopMachine();
        }

        int NGLineCylinderReachTime, NGLineCylinderRetracte;

        TimeSpan tms;

        private void timer2_Tick(object sender, EventArgs e)
        {



            if ((!worker.IsAutoRun) && (worker.workstatus == WorkStatuses.Pausing))
            {
                worker.OpenBlueStartLight();
                worker.OpenYellowLight();
                Thread.Sleep(500);
                worker.CloseBlueStartLight();
                worker.CloseYellowLight();
            }

            if ((!worker.IsManualRun) && (worker.workstatus == WorkStatuses.Idle))
            {
                worker.OpenBlueStartLight();
                worker.OpenYellowLight();
                Thread.Sleep(500);
                worker.CloseBlueStartLight();
                worker.CloseYellowLight();
            }



            Task.Run(() =>
            {

                #region 离子风棒报警

                ////撕膜




                if (!worker.CanGetIOInStatus(worker.Config.SMION_AlarmIOInEx))
                {
                    worker.OutputError(worker.Config.SMION_AlarmIOInEx.Name);
                }


                ////折弯     


                if (!worker.CanGetIOInStatus(worker.Config.BendION_AlarmIOInEx))
                {
                    worker.OutputError(worker.Config.BendION_AlarmIOInEx.Name);
                }
                #endregion


                #region NG气缸
                //if (worker.GetIOOutStatus(worker.Config.NGLinePushCylinder) && !worker.GetIOInStatus(worker.Config.NGLineCylinderDynamic))
                //{
                //    if (NGLineCylinderReachTime == 0)
                //    {
                //        NGLineCylinderReachTime = Environment.TickCount;
                //    }
                //    else if (Environment.TickCount - NGLineCylinderReachTime > 4500)
                //    {
                //        worker.OutputError(worker.Config.NGLineCylinderDynamic.Name + "报警", true);
                //    }

                //}
                //else if (!worker.GetIOOutStatus(worker.Config.NGLinePushCylinder) || worker.GetIOInStatus(worker.Config.NGLineCylinderDynamic))
                //{
                //    NGLineCylinderReachTime = 0;
                //}




                //if (!worker.GetIOOutStatus(worker.Config.NGLinePushCylinder) && !worker.GetIOInStatus(worker.Config.NGLineCylinderStatic))
                //{
                //    if (NGLineCylinderRetracte == 0)
                //    {
                //        NGLineCylinderRetracte = Environment.TickCount;
                //    }
                //    else if (Environment.TickCount - NGLineCylinderRetracte > 4500)
                //    {
                //        worker.OutputError(worker.Config.NGLineCylinderStatic.Name + "报警", true);
                //    }
                //}
                //else if (worker.GetIOOutStatus(worker.Config.NGLinePushCylinder) || worker.GetIOInStatus(worker.Config.NGLineCylinderStatic))
                //{
                //    NGLineCylinderRetracte = 0;
                //}




                #endregion


            });
            // bool flg =   worker.Config.IsLeftSMDisabled;

        }

        private bool DateTimeCompare(DateTime t1, DateTime t2)
        {
            bool res = true;
            if (t1.Hour > t2.Hour) return res;
            if (t1.Hour == t2.Hour && t1.Minute > t2.Minute) return res;
            if (t1.Hour == t2.Hour && t1.Minute == t2.Minute && t1.Second > t2.Second) return res;
            if (t1.Hour == t2.Hour && t1.Minute == t2.Minute && t1.Second == t2.Second) return res;
            return false;
        }





        int howlongnothaveproduct;//记录多长时间没有来料
        /// <summary>
        /// 刷新统计的标志位
        /// </summary>     
        private void RefStatisticsFlags()//TimeSpan timespan
        {
            if (worker.workstatus == WorkStatuses.Running)
            {
                worker.b_timespanstop = false;
                worker.b_timespanproduct = false;
                if (!worker.b_AlarmFlag || worker.FlagAlarm)//报警时间
                {
                    worker.b_timespanalarm = true;
                    return;
                }
                else
                {
                    worker.b_timespanalarm = false;
                }

                if (!worker.GetIOInStatus(worker.Config.BeltOpticalIOIN))//来料光纤无感应  待料时间
                {
                    howlongnothaveproduct++;
                    if (howlongnothaveproduct > 5)//5秒后还没来料 则待料计时
                    {
                        worker.b_timespanfree = true;
                        return;
                    }
                }
                else
                {
                    howlongnothaveproduct = 0;
                    worker.b_timespanfree = false;
                }
                worker.b_timespanproduct = true;
            }
            else //停机时间
            {
                // worker.b_timespanalarm = true;
                worker.b_timespanproduct = false;
                worker.b_timespanstop = true;
                worker.b_timespanfree = false;
            }
        }




       private void btn_lockdoor_Click(object sender, EventArgs e)
        {
            if (btn_lockdoor.Text == "门锁")
            {
                worker.LockSafeDoor();
                btn_lockdoor.Text = "解除门锁";
            }
            else if((btn_lockdoor.Text == "解除门锁"))
            {
                worker.UnlockSafeDoor();
                btn_lockdoor.Text = "门锁";
            }
        }


        private void btn_clearalarm_Click(object sender, EventArgs e)
        {
            worker.ErrorClear();
            //FrPause fr = new FrPause();
            //fr.Show();
            //worker.IsStop = false;
            //double ffff = 0;
            //worker.LoadCellWork(ref ffff, worker.LoadCell3Net);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            worker.CloseBuzzer();
        }

        private void btn_stop_Click_2(object sender, EventArgs e)
        {
            if (worker.workstatus != WorkStatuses.Running)
            {
                MeasurementContext.OutputError("当前状态无法点停止");
            }
            else
            {
                //worker.IsStop = true;
                worker.FeedStop = true;
                worker.Tear1Stop = true;
                worker.Tear2Stop = true;
                worker.Tear3Stop = true;
                worker.Bend1Stop = true;
                worker.Bend2Stop = true;
                worker.Bend3Stop = true;
                worker.TransferStop = true;
                worker.DischargeStop = true;
                worker.ManualStop = true;               
                worker.StopworkStausChanged();
            }
        }
        int sec;
        TimeSpan interval = new TimeSpan(0, 0, 1);
        private void CapacityTime_Tick(object sender, EventArgs e)
        {



            if (sec != DateTime.Now.Second)//防止长时间统计时间有误差 
            {
                sec = DateTime.Now.Second;
            }
            else
            {
                return;
            }

            RefStatisticsFlags();
            int hour = DateTime.Now.Hour;
            if (worker.b_timespan_flag)
            {
                if (DateTime.Now > worker.Recipe.CapacityTime.AddHours(12))
                {
                    //worker.Capacity.NightFreeTime.Add(interval);
                    //TimeSpan tst = worker.Capacity.NightfreeTime.Add(interval);
                    //worker.Capacity.NightfreeTime = tst;
                    lbl_nightfreetime.Text = string.Format("{0} 时 {1} 分 {2} 秒", worker.Capacity.NightFreeTime.Hours, worker.Capacity.NightFreeTime.Minutes, worker.Capacity.NightFreeTime.Seconds);
                }
                else
                {
                    //worker.Capacity.DayFreeTime.Add(interval);
                    //TimeSpan tst = worker.Capacity.DayFreeTime.Add(interval);
                    //worker.Capacity.DayFreeTime = tst;
                    lbl_dayfreetime.Text = string.Format("{0} 时 {1} 分 {2} 秒", worker.Capacity.DayFreeTime.Hours, worker.Capacity.DayFreeTime.Minutes, worker.Capacity.DayFreeTime.Seconds);
                }
            }

            Task.Run(() =>
            {
                if (DateTimeCompare(DateTime.Now, worker.Recipe.CapacityTime) && !DateTimeCompare(DateTime.Now, worker.Recipe.CapacityTime.AddHours(12)))//白班
                {
                    TimeSpan tm = DateTime.Now - worker.Recipe.CapacityTime;
                    hour = tm.Hours;
                    if (worker.b_timespanfree)
                    {
                        tms = worker.Capacity.DayShiftProduct[hour].HourFreeTime.Add(interval);
                        worker.Capacity.DayShiftProduct[hour].HourFreeTime = tms;
                    }
                    if (worker.b_timespanalarm)
                    {
                        tms = worker.Capacity.DayShiftProduct[hour].HourAlarmTime.Add(interval);
                        worker.Capacity.DayShiftProduct[hour].HourAlarmTime = tms;
                    }
                    if (worker.b_timespanproduct)
                    {
                        tms = worker.Capacity.DayShiftProduct[hour].HourProductionTime.Add(interval);
                        worker.Capacity.DayShiftProduct[hour].HourProductionTime = tms;
                    }
                }
                else if (!DateTimeCompare(DateTime.Now, worker.Recipe.CapacityTime) && DateTimeCompare(DateTime.Now, worker.Recipe.CapacityTime.AddHours(-worker.Recipe.CapacityTime.Hour)))//0：30-7：30
                {
                    TimeSpan tm = DateTime.Now - worker.Recipe.CapacityTime.AddHours(-worker.Recipe.CapacityTime.Hour);
                    hour = tm.Hours;
                    if (worker.b_timespanfree)
                    {
                        tms = worker.Capacity.NightShiftProduct[hour + 5].HourFreeTime.Add(interval);
                        worker.Capacity.NightShiftProduct[hour + 5].HourFreeTime = tms;
                    }
                    if (worker.b_timespanalarm)
                    {
                        tms = worker.Capacity.NightShiftProduct[hour + 5].HourAlarmTime.Add(interval);
                        worker.Capacity.NightShiftProduct[hour + 5].HourAlarmTime = tms;
                    }
                    if (worker.b_timespanproduct)
                    {
                        tms = worker.Capacity.NightShiftProduct[hour + 5].HourProductionTime.Add(interval);
                        worker.Capacity.NightShiftProduct[hour + 5].HourProductionTime = tms;
                    }
                }
                else
                {
                    TimeSpan tm = DateTime.Now - worker.Recipe.CapacityTime.AddHours(12);
                    hour = tm.Hours;

                    if (worker.b_timespanfree)
                    {
                        tms = worker.Capacity.NightShiftProduct[hour].HourFreeTime.Add(interval);
                        worker.Capacity.NightShiftProduct[hour].HourFreeTime = tms;
                    }
                    if (worker.b_timespanalarm)
                    {
                        tms = worker.Capacity.NightShiftProduct[hour].HourAlarmTime.Add(interval);
                        worker.Capacity.NightShiftProduct[hour].HourAlarmTime = tms;
                    }
                    if (worker.b_timespanproduct)
                    {
                        tms = worker.Capacity.NightShiftProduct[hour].HourProductionTime.Add(interval);
                        worker.Capacity.NightShiftProduct[hour].HourProductionTime = tms;
                    }
                }

            });
        }

        private void btn_DeviceInspection_Click(object sender, EventArgs e)
        {
            FrDeviceInspection devinspection = new FrDeviceInspection();
            devinspection.ShowDialog();
        }

        private void axisPositionListener10_Load(object sender, EventArgs e)
        {

        }

        private void axisPositionListener11_Load(object sender, EventArgs e)
        {

        }

        private void axisPositionListener12_Load(object sender, EventArgs e)
        {

        }



        void WriteLog(string info)
        {
            try
            {

                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\LoadWorkerStatus.csv", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + info + "\r\n");
            }
            catch (Exception ex)
            {
            }

        }


        int count = 0;
        private void timer3_Tick(object sender, EventArgs e)
        {
          
            if (MeasurementContext.Config.IsControlUpStreamEnable)
            {
               // CtrlInputProductLine();
            }
            else
            {
                MeasurementContext.Worker.SetIOOut(MeasurementContext.Worker.Config.UpstreamSuplyBeltIOOut, false);//运行
            }


            if (MeasurementContext.Worker.workstatus == WorkStatuses.Running &&
                MeasurementContext.Worker.CanGetIOInStatus(MeasurementContext.Config.ResetbtnIOInEx))//暂停
            {
                count++;
                if (count==40)
                {
                    btn_pause_Click(null, null);
                    count = 0;
                }              
            }
            else
            {
                count = 0;
            }          
        }

        private void timer4_Tick(object sender, EventArgs e)
        {


            //Random d1 = new Random();

            //sts_SMTotal1.value = d1.Next(0, 2000);
            //sts_SMTotal2.value = d1.Next(0, 2000);
            //sts_SMTotal3.value = d1.Next(0, 2000);
        }



        private void axisPositionListener38_Load(object sender, EventArgs e)
        {

        }

        private void pic_Min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Btn_NGLine_Click(object sender, EventArgs e)
        {
            if (worker.GetIOOutStatus(worker.Config.NGLinePushCylinder))
            {
                worker.SetIOOut(worker.Config.NGLinePushCylinder, false);
            }
            else
            {
                worker.SetIOOut(worker.Config.NGLinePushCylinder, true);
            }
        }

        private void btn_ManualLoad_Click(object sender, EventArgs e)
        {
            worker._IsBeltMotorFirst = false;
        }



        private void lbl_Login_Click(object sender, EventArgs e)
        {
            if (_Management.LoginType == LoginTypes.None)
            {
                _Management.LoginIn(true, _Management);
            }
            else
            {
                _Management.LoginOut();
            }
            LoginTypesChanged();
        }

        private void pic_Close_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否退出系统?"))
            {
                this.Close();
            }
        }


        private void SMTotalReset_Click(object sender, EventArgs e)
        {

            string Name = ((Control)sender).Name;

            string index = Name.Substring(Name.Length - 1, 1);
            switch (index)
            {
                case "1":
                    MeasurementContext.Config.LeftSMReplaceTime = DateTime.Now;
                    lbl_LeftReplaceTime.Text = "更换记录: " + DateTime.Now.ToString("MM-dd  HH:mm");
                    sts_SMTotal1.value = MeasurementContext.Config.LeftSMUseCount = 0;

                    break;

                case "2":
                    MeasurementContext.Config.MidSMReplaceTime = DateTime.Now;
                    lbl_MidReplaceTime.Text = "更换记录: " + DateTime.Now.ToString("MM-dd  HH:mm");
                    sts_SMTotal2.value = MeasurementContext.Config.MidSMUseCount = 0;
                    break;

                case "3":
                    MeasurementContext.Config.RightSMReplaceTime = DateTime.Now;
                    lbl_RightReplaceTime.Text = "更换记录: " + DateTime.Now.ToString("MM-dd  HH:mm");
                    sts_SMTotal3.value = MeasurementContext.Config.RightSMUseCount = 0;
                    break;


            }
        }

        private void btn_SMTotalReset1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            worker.CloseNet();
        }
    }
}

