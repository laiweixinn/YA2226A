using System;
using System.Diagnostics;
using System.Collections.Generic;
using NetWork;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LZ.CNC.Measurement.Core.Motions;
using LZ.CNC.Measurement.Core.Events;
using DY.CNC.Core;
using SciSmtCamLib;
using QSACTIVEXLib;
using DY.Core.Forms;
using System.Windows.Forms;
using System.IO;

namespace LZ.CNC.Measurement.Core
{
    /// <summary>
    /// 复位主界面chart控件
    /// </summary>
    /// <param name="station">0 1 2 代表折弯左中右</param>
    public delegate void ChartResetDel(int station);
    /// <summary>
    /// 刷新主界面chart控件值
    /// </summary>
    /// <param name="value1">补偿值1</param>
    /// <param name="vlue2">补偿值2</param>
    /// <param name="station">0 1 2 代表折弯左中右</param>
    public delegate void ChartUpdateDel(double value1, double vlue2, int station);


    /// <summary>
    /// 刷新主界面撕膜统计控件 tfx 20210916
    /// </summary>
    /// <param name="station"></param>
    /// <param name="value"></param>
    public delegate void SMTotalUpdateDel(int station, int value);




    public enum EProductAtt
    {
        OK,
        NG_NOTBEND,//NG但未反折
        NG,
    }

    public class MeasurementWorker : MeasurementWorkerBase
    {
        private static object Obj_BendLock = new object();
        private static long NetTimeOut = 5000;
        static object Obj_CCDlock = new object();
        private bool[] SMAOIResult = { false, false, false };
        private string str_LoadCellSend = "#0101\r";// = '\u0002' + "011RWT01\r\n"; //称重传感器重力查询字符
        private string str_ResetCellValSend = "%01@@2302+00000\r";//'\u0002' + "011OCZ84\r\n";//称重清零字符
        private string str_QRCodeSend = "RUN";
        /// <summary>
        /// true表示不报警？
        /// </summary>
        public bool b_AlarmFlag = true;

        #region 交互列队
        private int[] SMAOIReslut = { 3, 3, 3 };
        Queue<int> QueueSM = new Queue<int>();
        Queue<int> QueueBend = new Queue<int>();
        Queue<int> QueueTransferOut = new Queue<int>();
        Queue<int> QueueTransferIn = new Queue<int>();
        Queue<int> QueueBendOut = new Queue<int>();
        Queue<int> QueueTearResult = new Queue<int>();
        #endregion

        #region 小时统计标志
        public bool b_timespan_flag = true; //计算待机时间标志
        public bool b_timenow_flag = true;

        public bool b_timespanalarm = false;//报警时间
        public bool b_timespanfree = false;//待料时间
        public bool b_timespanstop = false;//停机时间
        public bool b_timespanproduct = false;//生产时间
        #endregion

        #region  重复撕膜标志
        /// <summary>
        /// 需要重新撕膜膜编号 是从Socket接收到数据时解析出来的
        /// </summary>
        StringBuilder LeftSocketReciveSMRecheckItems = new StringBuilder();
        StringBuilder MidSocketReciveSMRecheckItems = new StringBuilder();
        StringBuilder RightSocketReciveSMRecheckItems = new StringBuilder();

        /// <summary>
        /// 当前产品已撕膜次数 撕膜OK或重新撕膜次数到上限置零
        /// </summary>
        int LeftSMReckeckCurrentCount = 0;
        int MidSMReckeckCurrentCount = 0;
        int RightSMReckeckCurrentCount = 0;

        /// <summary>
        /// 当前产品需要进行重新撕膜 撕膜OK或重新撕膜次数到上限重置
        /// </summary>
        bool IsLeftRecheck = false;
        bool IsMidRecheck = false;
        bool IsRightRecheck = false;

        #endregion

        #region 撕膜运行标志
        bool _IsLeftSMDone = false;
        bool _IsMidSMDone = false;
        bool _IsRightSMDone = false;

        bool _IsLeftSMOut = false;
        bool _IsMidSMOut = false;
        bool _IsRightSMOut = false;

        bool _IsLeftsmloadDone = false;
        bool _IsMidsmloadDone = false;
        bool _IsRightsmloadDone = false;

        private bool _IsleftsmWorking = true;
        private bool _IsmidsmWorking = true;
        private bool _IsrightsmWorking = true;

        int Step_LeftSM = 0;
        int Step_RightSM = 0;
        int Step_MidSM = 0;

        public string str_leftsm;
        public string str_midsm;
        public string str_rightsm;

        #endregion

        #region 弯折标志
        bool b_bend1flag = false; //判断折弯工位是否将折弯上料信号压入队列
        bool b_bend2flag = false;
        bool b_bend3flag = false;

        bool b_bend1EnableLoop = false;
        bool b_bend2EnableLoop = false;
        bool b_bend3EnableLoop = false;

        bool _IsLeftBendReady = false;
        bool _IsMidBendReady = false;
        bool _IsRightBendReady = false;

        bool _IsLeftBendUp = false;
        bool _IsMidBendUp = false;
        bool _IsRightBendUp = false;

        bool _IsLeftBendOutReady = false;
        bool _IsMidBendOutReady = false;
        bool _IsRightBendOutReady = false;

        private bool _IsleftbendWorking = true;
        private bool _IsmidbendWorking = true;
        private bool _IsrightbendWorking = true;

        int Step_LeftBend = 0;
        int Step_MidBend = 0;
        int Step_RightBend = 0;
        int Step_Transfer = 0;

        public string str_midbend_y;
        public string str_leftbend_y;
        public string str_rightbend_y;
        public string str_leftbend_degree;
        public string str_midbend_degree;
        public string str_rightbend_degree;

        #endregion

        #region 下料中转标志

        bool _IsLeftDischargeReady = false;
        bool _IsMidDischargeReady = false;
        bool _IsRightDischargeReady = false;
        private bool _IsTransferWorking = true;

        /// <summary>
        /// 下料准备就绪 未就绪表示有产品
        /// </summary>
        bool _IsDischargeRotateReady;

        Thread thdTakeOutBoard;
        #endregion

        #region 称重
        /// <summary>
        /// 左称重值 如果称重失败 默认返回上一次的值
        /// </summary>
        double left_WeigtVal = 0;
        double mid_WeigtVal = 0;
        double right_WeigtVal = 0;

        #endregion


        bool IsLeftReachloadPos = false;
        bool IsMidReachloadPos = false;
        bool IsRightReachloadPos = false;
        private bool _flagAlarm = false;

        public bool ManualStop = false; //停止记忆标签
        public bool FeedStop = false;  //每个工位一个停止标签，避免不同线程延时情况
        public bool Tear1Stop = false;
        public bool Tear2Stop = false;
        public bool Tear3Stop = false;
        public bool Bend1Stop = false;
        public bool Bend2Stop = false;
        public bool Bend3Stop = false;
        public bool TransferStop = false;
        public bool DischargeStop = false;
        /// <summary>
        /// 弹窗的报警
        /// </summary>
        public bool FlagAlarm
        {
            get
            {
                return _flagAlarm;
            }
            set
            {
                _flagAlarm = value;
            }
        }

        public string PathCode
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath + "\\CodeLog\\Code" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            }
        }
        public MeasurementWorker()
        {
            _MotionA.AxisX.AxisSet.AxisName = "左撕膜X轴";
            _MotionA.AxisY.AxisSet.AxisName = "左撕膜Y轴";
            _MotionA.AxisZ.AxisSet.AxisName = "左撕膜Z轴";
            _MotionA.AxisU.AxisSet.AxisName = "中撕膜X轴";
            _MotionA.AxisV.AxisSet.AxisName = "中撕膜Y轴";
            _MotionA.AxisW.AxisSet.AxisName = "中撕膜Z轴";
            _MotionA.AxisA.AxisSet.AxisName = "右撕膜X轴";
            _MotionA.AxisB.AxisSet.AxisName = "右撕膜Y轴";
            _MotionA.AxisC.AxisSet.AxisName = "左撕膜W轴";
            _MotionA.AxisD.AxisSet.AxisName = "中撕膜W轴";
            _MotionA.AxisE.AxisSet.AxisName = "右撕膜W轴";
            _MotionA.AxisF.AxisSet.AxisName = "来料皮带Y轴";

            _MotionB.AxisX.AxisSet.AxisName = "右撕膜Z轴";
            _MotionB.AxisY.AxisSet.AxisName = "上料工位X轴";
            _MotionB.AxisZ.AxisSet.AxisName = "上料工位Z轴";
            _MotionB.AxisU.AxisSet.AxisName = "中转工位X轴";
            _MotionB.AxisV.AxisSet.AxisName = "中转工位Z轴";
            _MotionB.AxisW.AxisSet.AxisName = "左折弯相机X轴";
            _MotionB.AxisA.AxisSet.AxisName = "左折弯对位X轴";
            _MotionB.AxisB.AxisSet.AxisName = "左折弯对位Y轴";
            _MotionB.AxisC.AxisSet.AxisName = "出料工位Z轴";
            _MotionB.AxisD.AxisSet.AxisName = "撕膜相机X轴";
            _MotionB.AxisE.AxisSet.AxisName = "####";
            _MotionB.AxisF.AxisSet.AxisName = "####";

            _MotionC.AxisX.AxisSet.AxisName = "左折弯对位R轴";
            _MotionC.AxisY.AxisSet.AxisName = "左折弯对位W轴";
            _MotionC.AxisZ.AxisSet.AxisName = "左折弯平台Y轴";
            _MotionC.AxisU.AxisSet.AxisName = "中折弯相机X轴";
            _MotionC.AxisV.AxisSet.AxisName = "中折弯对位X轴";
            _MotionC.AxisW.AxisSet.AxisName = "中折弯对位Y轴";
            _MotionC.AxisA.AxisSet.AxisName = "中折弯对位R轴";
            _MotionC.AxisB.AxisSet.AxisName = "中折弯对位W轴";
            _MotionC.AxisC.AxisSet.AxisName = "####";
            _MotionC.AxisD.AxisSet.AxisName = "####";
            _MotionC.AxisE.AxisSet.AxisName = "####";
            _MotionC.AxisF.AxisSet.AxisName = "####";

            _MotionD.AxisX.AxisSet.AxisName = "中折弯平台Y轴";
            _MotionD.AxisY.AxisSet.AxisName = "右折弯相机X轴";
            _MotionD.AxisZ.AxisSet.AxisName = "右折弯对位X轴";
            _MotionD.AxisU.AxisSet.AxisName = "右折弯对位Y轴";
            _MotionD.AxisV.AxisSet.AxisName = "右折弯对位R轴";
            _MotionD.AxisW.AxisSet.AxisName = "右折弯对位W轴";
            _MotionD.AxisA.AxisSet.AxisName = "右折弯平台Y轴";
            _MotionD.AxisB.AxisSet.AxisName = "出料工位X轴";
            _MotionD.AxisC.AxisSet.AxisName = "####";
            _MotionD.AxisD.AxisSet.AxisName = "####";
            _MotionD.AxisE.AxisSet.AxisName = "####";
            _MotionD.AxisF.AxisSet.AxisName = "####";

            _MotionA.AxisW.AxisSet.AxisMode = AxisModes.Linear;
            _MotionA.AxisA.AxisSet.AxisMode = AxisModes.Linear;
            _MotionA.AxisB.AxisSet.AxisMode = AxisModes.Linear;
            _MotionC.AxisY.AxisSet.AxisMode = AxisModes.Linear;
            _MotionC.AxisB.AxisSet.AxisMode = AxisModes.Linear;
            _MotionD.AxisW.AxisSet.AxisMode = AxisModes.Linear;

            _MotionA.IOListener.IOInStatusChanged += IOListenerA_IOInStatusChanged;
            _MotionA.IOListener.AxisIOStatusChanged += IOListener_AxisIOStatusChanged;
            _MotionA.IOListener.IOInStatusExChanged += IOListenerA_IOInStatusExChanged;


            _MotionB.IOListener.IOInStatusChanged += IOListenerB_IOInStatusChanged;
            _MotionB.IOListener.IOInStatusExChanged += IOListenerB_IOInStatusExChanged;
            _MotionB.IOListener.AxisIOStatusChanged += IOListenerB_AxisIOStatusChanged;

            _MotionC.IOListener.IOInStatusChanged += IOListener_IOInStatusChanged;
            MotionC.IOListener.IOInStatusExChanged += IOListenerC_IOInStatusExChanged;
            _MotionC.IOListener.AxisIOStatusChanged += IOListenerC_AxisIOStatusChanged;

            _MotionD.IOListener.IOInStatusChanged += IOListenerD_IOInStatusChanged;
            _MotionD.IOListener.AxisIOStatusChanged += IOListenerD_AxisIOStatusChanged;


            BendCCDNet.OnStateInfo += Bend1StateInfo;
            BendCCDNet.OnReceviceByte += Bend1ReciveceMsg;
            BendCCDNet.OnErrorMsg += Bend1ErrRecevice;

            Bend2CCDNet.OnStateInfo += Bend2StateInfo;
            Bend2CCDNet.OnReceviceByte += Bend2ReciveceMsg;
            Bend2CCDNet.OnErrorMsg += Bend2ErrRecevice;

            Bend3CCDNet.OnStateInfo += Bend3StateInfo;
            Bend3CCDNet.OnReceviceByte += Bend3ReciveceMsg;
            Bend3CCDNet.OnErrorMsg += Bend3ErrRecevice;

            LoadCell1Net.OnStateInfo += LoadCell1StateInfo;
            LoadCell1Net.OnReceviceByte += LoadCell1ReciveceMsg;
            LoadCell1Net.OnErrorMsg += LoadCell1ErrRecevice;

            LoadCell2Net.OnStateInfo += LoadCell2StateInfo;
            LoadCell2Net.OnReceviceByte += LoadCell2ReciveceMsg;
            LoadCell2Net.OnErrorMsg += LoadCell2ErrRecevice;

            LoadCell3Net.OnStateInfo += LoadCell3StateInfo;
            LoadCell3Net.OnReceviceByte += LoadCell3ReciveceMsg;
            LoadCell3Net.OnErrorMsg += LoadCell3ErrRecevice;


            QRCodeNet.OnStateInfo += QrCodeStateInfo;
            QRCodeNet.OnReceviceByte += QrCodeReciveceMsg;
            QRCodeNet.OnErrorMsg += QrCodeErrRecevice;

            TearCCDNet.OnStateInfo += TearStateInfo;
            TearCCDNet.OnReceviceByte += TearReciveceMsg;
            TearCCDNet.OnErrorMsg += TearErrRecevice;

        }

        #region
        public string str_TearRev = "";
        public string str_Bend1Rev = "";
        public string str_Bend2Rev = "";
        public string str_Bend3Rev = "";
        public string str_Cell1Rev = "";
        public string str_Cell2Rev = "";
        public string str_Cell3Rev = "";
        public string str_QRCodeRev = "";
        private void TearStateInfo(string tmpstr, TcpClientHelper.SocketState tmp)
        {
            OutputMessage("撕膜相机:" + tmpstr);
        }
        private void TearErrRecevice(string tmpstr)
        {
            OutputError("撕膜相机:" + tmpstr);
        }
        private void TearReciveceMsg(byte[] data)
        {

            string tmpstr = System.Text.Encoding.Default.GetString(data);
            str_TearRev = tmpstr;
            tmpstr = "撕膜相机收到消息: " + tmpstr;
            OutputMessage(tmpstr);
        }

        private void Bend1StateInfo(string tmpstr, TcpClientHelper.SocketState tmp)
        {
            //OutputMessage("左折弯相机:" + tmpstr);
        }
        private void Bend1ErrRecevice(string tmpstr)
        {
            OutputError("左折弯相机:" + tmpstr);
        }
        private void Bend1ReciveceMsg(byte[] data)
        {
            string tmpstr = System.Text.Encoding.Default.GetString(data);
            str_Bend1Rev = tmpstr;
            tmpstr = "左折弯相机收到消息: " + tmpstr;
            OutputMessage(tmpstr);
        }

        private void Bend2StateInfo(string tmpstr, TcpClientHelper.SocketState tmp)
        {
            //   OutputMessage("中折弯相机:" + tmpstr);
        }
        private void Bend2ErrRecevice(string tmpstr)
        {
            OutputError("中折弯相机:" + tmpstr);
        }
        private void Bend2ReciveceMsg(byte[] data)
        {
            string tmpstr = System.Text.Encoding.Default.GetString(data);
            str_Bend2Rev = tmpstr;
            tmpstr = "中折弯相机收到消息: " + tmpstr;
            OutputMessage(tmpstr);
        }


        private void Bend3StateInfo(string tmpstr, TcpClientHelper.SocketState tmp)
        {
            //  OutputMessage("右折弯相机:" + tmpstr);
        }
        private void Bend3ErrRecevice(string tmpstr)
        {
            OutputError("右折弯相机:" + tmpstr);
        }
        private void Bend3ReciveceMsg(byte[] data)
        {
            string tmpstr = System.Text.Encoding.Default.GetString(data);
            str_Bend3Rev = tmpstr;
            tmpstr = "右折弯相机收到消息: " + tmpstr;
            OutputMessage(tmpstr);
        }

        private void LoadCell1StateInfo(string tmpstr, TcpClientHelper.SocketState tmp)
        {
            OutputMessage("称重1:" + tmpstr);
        }
        private void LoadCell1ErrRecevice(string tmpstr)
        {
            OutputError("称重1:" + tmpstr);
        }
        private void LoadCell1ReciveceMsg(byte[] data)
        {
            try
            {
                string tmpstr = System.Text.Encoding.Default.GetString(data);
                str_Cell1Rev = tmpstr;
                tmpstr = "称重1: " + tmpstr;
                //string[] tmp = str_Cell1Rev.Split('@');

                //double val = double.Parse(tmp[1].Substring(1, 8)) / 10000;

                OutputMessage(tmpstr);
            }
            catch (Exception)
            {


            }

        }

        private void LoadCell2StateInfo(string tmpstr, TcpClientHelper.SocketState tmp)
        {
            OutputMessage("称重2:" + tmpstr);
        }
        private void LoadCell2ErrRecevice(string tmpstr)
        {
            OutputError("称重2:" + tmpstr);
        }
        private void LoadCell2ReciveceMsg(byte[] data)
        {
            string tmpstr = System.Text.Encoding.Default.GetString(data);
            str_Cell2Rev = tmpstr;
            tmpstr = "称重2: " + tmpstr;
            OutputMessage(tmpstr);
        }

        private void LoadCell3StateInfo(string tmpstr, TcpClientHelper.SocketState tmp)
        {
            OutputMessage("称重3:" + tmpstr);
        }
        private void LoadCell3ErrRecevice(string tmpstr)
        {
            OutputError("称重3:" + tmpstr);
        }
        private void LoadCell3ReciveceMsg(byte[] data)
        {
            string tmpstr = System.Text.Encoding.Default.GetString(data);
            str_Cell3Rev = tmpstr;
            tmpstr = "称重3: " + tmpstr;
            OutputMessage(tmpstr);
        }

        private void QrCodeStateInfo(string tmpstr, TcpClientHelper.SocketState tmp)
        {
            OutputMessage("扫码:" + tmpstr);
        }
        private void QrCodeErrRecevice(string tmpstr)
        {
            OutputError("扫码:" + tmpstr);
        }
        private void QrCodeReciveceMsg(byte[] data)
        {
            string tmpstr = System.Text.Encoding.Default.GetString(data);
            str_QRCodeRev = tmpstr;
            tmpstr = "扫码: " + tmpstr;
            OutputMessage(tmpstr);
        }
        #endregion
        private bool b_tear1loop = false;
        private bool b_tear2loop = false;
        private bool b_tear3loop = false;
        private bool b_tear1stop = false;
        private bool b_tear2stop = false;
        private bool b_tear3stop = false;

        /// <summary>
        /// 撕膜平台真空检测监控
        /// </summary>
        public void TearSuckListen()
        {
            while (true)
            {
                Thread.Sleep(20);
                if (CanGetIOOutStatus(Config.AllStepMotorServoOn_IOOutEx))
                {
                    CanSetIOOut(Config.DischargZbrak_IOOutEx, true);
                }
                else
                {
                    CanSetIOOut(Config.DischargZbrak_IOOutEx, false);
                }

                if (b_tear1loop && (!b_tear1stop))
                {
                    if (GetIOInStatus(Config.LeFTSMVacuumIOIn) != true)
                    {
                        Thread.Sleep(50);//等待50MS后再次检测
                        if (GetIOInStatus(Config.LeFTSMVacuumIOIn) != true)
                        {
                            b_tear1stop = true;
                        }
                    }
                }

                if (b_tear2loop && (!b_tear2stop))
                {
                    if (CanGetIOInStatus(Config.MidSMVacuumIOInEx) != true)
                    {
                        Thread.Sleep(50);//等待50MS后再次检测
                        if (CanGetIOInStatus(Config.MidSMVacuumIOInEx) != true)
                        {
                            b_tear2stop = true;
                        }
                    }
                }

                if (b_tear3loop && (!b_tear3stop))
                {
                    if (GetIOInStatus(Config.RightSMVacuumIOIn) != true)
                    {
                        Thread.Sleep(50);//等待50MS后再次检测
                        if (GetIOInStatus(Config.RightSMVacuumIOIn) != true)
                        {
                            b_tear3stop = true;
                        }
                    }
                }
            }
        }

        public void AirAndVacumnCheck()
        {
            while (true)
            {
                Thread.Sleep(10000);
                if (!CanGetIOInStatus(Config.InputAir_IOInEx))
                {
                    DialogResult DRet = ShowMsgChoiceBox("气源正压异常,请检查总气压!", false, false);
                }

                if (!CanGetIOInStatus(Config.InputVacumn_IOInEx))
                {
                    DialogResult DRet = ShowMsgChoiceBox("气源负压异常,请检查总负压!", false, false);
                }


                //耗材使用达到上限报警  tfx 20210916
                if (workstatus == WorkStatuses.Running)
                {
                    bool flg = false;
                    if (MeasurementContext.Config.LeftSMUseCount >= MeasurementContext.Config.SMUseMax)
                    {
                        flg = true;
                        MeasurementContext.OutputError("左撕膜耗材已达到使用上限,请进行更换,完成更换后请点击重置按钮");
                    }

                    if (MeasurementContext.Config.MidSMUseCount >= MeasurementContext.Config.SMUseMax)
                    {
                        flg = true;
                        MeasurementContext.OutputError("中撕膜耗材已达到使用上限,请进行更换,完成更换后请点击重置按钮");
                    }

                    if (MeasurementContext.Config.RightSMUseCount >= MeasurementContext.Config.SMUseMax)
                    {
                        flg = true;
                        MeasurementContext.OutputError("右撕膜耗材已达到使用上限,请进行更换,完成更换后请点击重置按钮");
                    }

                    if (flg)
                    {
                        flg = false;
                        for (int i = 0; i < 3; i++)
                        {
                            OpenBuzzer();
                            OpenRedLight();
                            Thread.Sleep(1200);
                            CloseRedLight();
                            CloseBuzzer();
                            Thread.Sleep(300);
                        }
                    }

                }



            }
        }

        /// <summary>
        /// 撕膜轴运动过程中轴停止真空检测
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool CheckTear1AxisDone(MeasurementAxis axis)
        {
            bool flag = true;

            while ((DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_check_done((ushort)(axis.Motion.Id), (ushort)(axis.AxisIndex - 1)) == 0))
            {
                if (b_tear1stop)
                {
                    IsAutoRun = false;
                    flag = false;
                    break;
                }
                Thread.Sleep(5);
            }
            if (!flag)
            {
                axis.StopSlowly();
            }
            return flag;
        }

        public bool CheckTear2AxisDone(MeasurementAxis axis)
        {
            bool flag = true;

            while ((DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_check_done((ushort)(axis.Motion.Id), (ushort)(axis.AxisIndex - 1)) == 0))
            {
                if (b_tear2stop)
                {
                    IsAutoRun = false;
                    flag = false;
                    break;
                }
                Thread.Sleep(5);
            }
            if (!flag)
            {
                axis.StopSlowly();
            }
            return flag;
        }
        public bool CheckTear3AxisDone(MeasurementAxis axis)
        {
            bool flag = true;

            while ((DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_check_done((ushort)(axis.Motion.Id), (ushort)(axis.AxisIndex - 1)) == 0))
            {
                if (b_tear3stop)
                {
                    IsAutoRun = false;
                    flag = false;
                    break;
                }
                Thread.Sleep(5);
            }
            if (!flag)
            {
                axis.StopSlowly();
            }
            return flag;
        }

        private static Object Obj_ConfirmLock = new Object();
        private bool _FlagConfirm = false;
        public bool FlagConfirm
        {
            get
            {
                return _FlagConfirm;
            }
            set
            {
                _FlagConfirm = value;
            }
        }

        public DialogResult ShowMsgChoiceBox(string str = "", bool isCancel = false, bool isAbort = false)
        {
            DialogResult result;
            lock (Obj_ConfirmLock)
            {
                while (FlagConfirm)
                {
                    Thread.Sleep(20);
                }
                frmConfirm frm = new frmConfirm(str, isCancel, isAbort);
                frm.ShowDialog();
                result = frm.DialogResult;
            }
            return result;
        }

        private static Object Obj_AlarmDoorLock = new Object();

        public DialogResult ShowMsgDoorAlarm(string info)
        {
            DialogResult result;

            lock (Obj_AlarmDoorLock)
            {
                while (FlagAlarm)
                {
                    Thread.Sleep(5);
                }
                FrmConfirmAlarm frm = new FrmConfirmAlarm(info);
                frm.ShowDialog();
                result = frm.DialogResult;
            }
            return result;
        }

        /// <summary>
        /// 添加产品生产详情 弯折Y1 Y2
        /// </summary>
        /// <param name="item"></param>
        private void SaveRecord(DatasCollections.DetectDataItem item)
        {
            lock (this)
            {
                DatasCollections detect = DatasCollections.Load();
                if (detect == null)
                {
                    detect = new DatasCollections();
                }
                detect.DetectDatas.Insert(0, item);
                detect.Save();
            }
        }
        /// <summary>
        /// 添加弯折AOI检测四个数据
        /// </summary>
        /// <param name="item"></param>
        private void SaveBend1Record(AOI1DataCollections.DetectDataItem item)
        {
            AOI1DataCollections detect = AOI1DataCollections.Load();
            if (detect == null)
            {
                detect = new AOI1DataCollections();
            }
            detect.DetectDatas.Insert(0, item);
            detect.Save();
        }
        private void SaveBend2Record(AOI2DataCollections.DetectDataItem item)
        {
            AOI2DataCollections detect = AOI2DataCollections.Load();
            if (detect == null)
            {
                detect = new AOI2DataCollections();
            }
            detect.DetectDatas.Insert(0, item);
            detect.Save();
        }
        private void SaveBend3Record(AOI3DataCollections.DetectDataItem item)
        {
            AOI3DataCollections detect = AOI3DataCollections.Load();
            if (detect == null)
            {
                detect = new AOI3DataCollections();
            }
            detect.DetectDatas.Insert(0, item);
            detect.Save();
        }
        public bool ConnectCard()
        {
            bool result;
            bool res = false;
            int err = 0;
            ushort node = 0;
            bool connect = false;
            res = _MotionA.Connect();
            if (!res)
            {
                err++;
                OutputError(string.Format("无法连接至控制卡A"), false);
            }

            res = _MotionB.Connect();
            if (!res)
            {
                err++;
                OutputError(string.Format("无法连接至控制卡B"), false);
            }

            res = _MotionC.Connect();
            if (!res)
            {
                err++;
                OutputError(string.Format("无法连接至控制卡C"), false);
            }

            res = _MotionD.Connect();
            if (!res)
            {
                err++;
                OutputError(string.Format("无法连接至控制卡D"), false);
            }

            _MotionA.CANSetState(1, 1);
            //_MotionA.CANGetState(ref node, ref connect);

            //if (!connect)
            //{
            //    err++;
            //    OutputError(string.Format("无法连接至扩展模块A"), false);
            //}

            _MotionB.CANSetState(1, 1);
            //_MotionB.CANGetState(ref node, ref connect);
            //if (!connect)
            //{
            //    err++;
            //    OutputError(string.Format("无法连接至扩展模块B"), false);
            //}

            _MotionC.CANSetState(1, 1);
            //_MotionC.CANGetState(ref node, ref connect);
            //if (!connect)
            //{
            //    err++;
            //    OutputError(string.Format("无法连接至扩展模块C"), false);
            //}

            _MotionD.CANSetState(1, 1);
            //_MotionD.CANGetState(ref node, ref connect);
            //if (!connect)
            //{
            //    err++;
            //    OutputError(string.Format("无法连接至模拟量模块A"), false);
            //}

            _MotionD.CANSetState(2, 1);
            //_MotionD.CANGetState(ref node, ref connect);
            //if (!connect)
            //{
            //    err++;
            //    OutputError(string.Format("无法连接至模拟量模块B"), false);
            //}

            string path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "set\\card_0.ini");
            if (!(DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_download_configfile(0, path) == 0 ? true : false))
            {
                OutputError("无法写入A卡参数!", false);
            }

            path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "set\\card_1.ini");
            if (!(DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_download_configfile(1, path) == 0 ? true : false))
            {
                OutputError("无法写入B卡参数!", false);
            }

            path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "set\\card_2.ini");
            if (!(DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_download_configfile(2, path) == 0 ? true : false))
            {
                OutputError("无法写入C卡参数!", false);
            }

            path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "set\\card_3.ini");
            if (!(DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_download_configfile(3, path) == 0 ? true : false))
            {
                OutputError("无法写入D卡参数!", false);
            }


            if (err != 0)
            {
                result = false;
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
            }
            else
            {
                result = true;
                _MotionA.SetSevON();
                _MotionB.SetSevON();
                _MotionC.SetSevON();
                _MotionD.SetSevON();

                CanSetIOOut(Config.Left_WServoOn_IOOutEx, true);
                CanSetIOOut(Config.Mid_WServoOn_IOOutEx, true);
                CanSetIOOut(Config.Right_WServoOn_IOOutEx, true);
                CanSetIOOut(Config.SM_AOIServoOn_IOOutEx, true);
                CanSetIOOut(Config.DischargZServoOn_IOOutEx, true);
                CanSetIOOut(Config.DischargZbrak_IOOutEx, true);

                _MotionA.PositionListener.StartListen();
                _MotionA.IOListener.StartListen();
                _MotionB.PositionListener.StartListen();
                _MotionB.IOListener.StartListen();
                _MotionC.PositionListener.StartListen();
                _MotionC.IOListener.StartListen();
                _MotionD.PositionListener.StartListen();
                _MotionD.IOListener.StartListen();
                CloseBuzzer();
            }
            return result;
        }
        public bool DisConnect()
        {

            CloseGreenLight();
            CloseRedLight();
            CloseYellowLight();

            _MotionA.StopSlowly();
            _MotionB.StopSlowly();
            _MotionC.StopSlowly();
            _MotionD.StopSlowly();

            _MotionA.EndMotion();
            _MotionB.EndMotion();
            _MotionC.EndMotion();
            _MotionD.EndMotion();

            _MotionA.EndGoHome();
            _MotionB.EndGoHome();
            _MotionC.EndGoHome();
            _MotionD.EndGoHome();

            _MotionA.IOListener.IOInStatusChanged -= IOListener_IOInStatusChanged;
            _MotionA.IOListener.AxisIOStatusChanged -= IOListener_AxisIOStatusChanged;

            _MotionB.IOListener.IOInStatusChanged -= IOListener_IOInStatusChanged;
            _MotionB.IOListener.IOInStatusExChanged -= IOListenerB_IOInStatusExChanged;
            _MotionB.IOListener.AxisIOStatusChanged -= IOListenerB_AxisIOStatusChanged;

            _MotionC.IOListener.IOInStatusChanged -= IOListener_IOInStatusChanged;
            _MotionC.IOListener.AxisIOStatusChanged -= IOListenerC_AxisIOStatusChanged;

            _MotionD.IOListener.IOInStatusChanged -= IOListener_IOInStatusChanged;
            _MotionD.IOListener.AxisIOStatusChanged -= IOListenerD_AxisIOStatusChanged;

            _MotionA.PositionListener.StopListen();
            _MotionB.PositionListener.StopListen();
            _MotionC.PositionListener.StopListen();
            _MotionD.PositionListener.StopListen();

            _MotionA.CANSetState(1, 0);
            _MotionB.CANSetState(1, 0);
            _MotionC.CANSetState(1, 0);
            _MotionD.CANSetState(1, 0);
            _MotionD.CANSetState(2, 0);
            DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_board_close();
            return true;
        }
        public void BeginMotion()
        {
            _MotionA.BeginMotion();
            _MotionB.BeginMotion();
            _MotionC.BeginMotion();
            _MotionD.BeginMotion();
        }
        public void EndMotion()
        {
            _MotionA.EndMotion();
            _MotionB.EndMotion();
            _MotionC.EndMotion();
            _MotionD.EndMotion();
        }
        public void StopSlowly()
        {
            _MotionA.StopSlowly();
            _MotionB.StopSlowly();
            _MotionC.StopSlowly();
            _MotionD.StopSlowly();
        }
        public bool CheckRun()
        {
            bool result = false;
            _IsStop = false;
            if (IsEmgButtonDown)
            {
                ShowMessage("急停开关已按下,请松开后操作!");
                result = false;
            }
            else if (_WorkStatus == WorkStatuses.Emg)
            {
                ShowMessage("设备急停状态,请先复位后操作!");
                result = false;
            }
            else if (_WorkStatus == WorkStatuses.Homing)
            {
                ShowMessage("设备回原中!");
                result = false;
            }
            else if (_WorkStatus == WorkStatuses.Running || _WorkStatus == WorkStatuses.Pausing || _WorkStatus == WorkStatuses.Stoping)
            {
                OutputError("设备处于运行状态!", false);
                result = false;
            }
            else if (!_IsReset)
            {
                ShowMessage("设备未复位,不允许操作!");
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
        public bool Reset()
        {
            bool flag = true;
            _WorkStatus = WorkStatuses.Homing;
            OnWorkStatusChanged();

            if (IsEmgButtonDown)
            {
                OutputError("急停开关已按下，请松开后操作！", false);
                flag = false;
            }
            else if (!IsGateSafe())
            {
                OutputError("安全门未关闭！", false);
                flag = false;
            }
            else if ((_WorkStatus == WorkStatuses.Running))
            {
                OutputError("当前状态无法复位!", false);
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                flag = false;
            }

            if (!flag)
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }


            //if (IsLoadSTSuck || IsTransferSuck || IsDischargeSuck)
            //{
            //    MessageBoxEx.ShowErrorMessage("搬运机械手真空吸报警，请检查是否有物料!");
            //    _WorkStatus = WorkStatuses.Error;
            //    OnWorkStatusChanged();
            //    return false;
            //}

            //if (IsLeftSMStgSuck || IsMidSMStgSuck || IsRightSMStgSuck)
            //{
            //    MessageBoxEx.ShowErrorMessage("撕膜平台真空吸报警，请检查是否有物料!");
            //    _WorkStatus = WorkStatuses.Error;
            //    OnWorkStatusChanged();
            //    return false;
            //}

            //if (IsLeftBendStgSuck || IsMidBendStgSuck || IsRightBendStgSuck)
            //{
            //    MessageBoxEx.ShowErrorMessage("折弯平台真空吸报警，请检查是否有物料!");
            //    _WorkStatus = WorkStatuses.Error;
            //    OnWorkStatusChanged();
            //    return false;
            //}

            b_bend1flag = false;
            b_bend2flag = false;
            b_bend3flag = false;

            ManualStop = false;
            Tear1Stop = false;
            Tear2Stop = false;
            Tear3Stop = false;
            Bend1Stop = false;
            Bend2Stop = false;
            Bend3Stop = false;
            FeedStop = false;
            TransferStop = false;
            DischargeStop = false;
            _IsStop = false;

            if (Config.IsLoadZCylinder)
            {
                FeedUDCylinderUp();
                if (!WaitIOExMSec(Config.Feed_UpDownCylinder_UpIOInEx))
                {
                    _WorkStatus = WorkStatuses.Error;
                    OnWorkStatusChanged();
                    return false;
                }
            }


            if (Config.IsTransferZCylinder)
            {
                TransferUDCylinderUp();
                if (!WaitIOMSec_TransferUDCylinderUP(3000))
                {
                    _WorkStatus = WorkStatuses.Error;
                    OnWorkStatusChanged();
                    return false;
                }
            }


            CloseLeftSMFBCylinder();
            //if (!WaitIOMSec(Config.LeftSM_FB_CylinderBackIOIn))
            //{
            //    _WorkStatus = WorkStatuses.Error;
            //    OnWorkStatusChanged();
            //    return false;
            //}
            CloseLeftSMLRCylinder();
            //if (!WaitIOMSec(Config.LeftSM_LR_CylinderRightIOIn))
            //{
            //    _WorkStatus = WorkStatuses.Error;
            //    OnWorkStatusChanged();
            //    return false;
            //}

            CloseLeftSMRollerUDCyliner();
            if (!WaitIOExMSec(Config.LeftSM_RollerUD_CylinderUpIOInEx))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }

            CloseLeftSMUDCylinder();
            if (!WaitIOMSec(Config.LeftSM_UD_CylinderUPIOIn))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }

            CloseLeftSMGlueUDCylinder();
            if (!WaitIOExMSec(Config.LeftSM_RollerUD_CylinderUpIOInEx))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }



            CloseMidSMFBCylinder();
            //if (!WaitIOExMSec(Config.MidSM_FB_CylinderBackIOInEx))
            //{
            //    _WorkStatus = WorkStatuses.Error;
            //    OnWorkStatusChanged();
            //    return false;
            //}
            CloseMidSMLRCylinder();
            //if (!WaitIOMSec(Config.MidSM_LR_CylinderRightIOInEx))
            //{
            //    _WorkStatus = WorkStatuses.Error;
            //    OnWorkStatusChanged();
            //    return false;
            //}
            CloseMidSMRollerUDCyliner();
            if (!WaitIOExMSec(Config.MidSM_RollerUD_CylinderUPIOInEx))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }
            CloseMidSMUDCylinder();
            if (!WaitIOExMSec(Config.MidSM_UD_CylinderUPIOInEx))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }

            CloseMidSMGlueUDCylinder();
            if (!WaitIOExMSec(Config.MidSM_GlueUD_CylinderUPIOInEx))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }

            CloseRightSMFBCylinder();
            //if (!WaitIOMSec(Config.RightSM_FB_CylinderBackIOIn))
            //{
            //    _WorkStatus = WorkStatuses.Error;
            //    OnWorkStatusChanged();
            //    return false;
            //}
            CloseRightSMLRCylinder();
            //if (!WaitIOMSec(Config.RightSM_LR_CylinderRightIOIn))
            //{
            //    _WorkStatus = WorkStatuses.Error;
            //    OnWorkStatusChanged();
            //    return false;
            //}

            CloseRightSMRollerUDCyliner();
            if (!WaitIOExMSec(Config.RightSM_RollerUD_CylinderUPIOInEx))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }
            CloseRightSMUDCylinder();
            if (!WaitIOMSec(Config.RightSM_UD_CylinderUPIOIn))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }
            CloseRightSMGlueUDCylinder();
            if (!WaitIOMSec(Config.RightSM_GlueUDCylinderUPIOIn))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }


            CloseBeltMotor();
            CloseDischargeBeltMotor();
            CloseNGBeltMotor();

            CloseLeftBend_PressCylinder();
            if (!WaitIOMSec(Config.LeftBend_PressCylinder_UPIOIn))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }
            CloseMidBend_PressCylinder();
            if (!WaitIOMSec(Config.MidBend_PressCylinder_UPIOIn))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }

            CloseRightBend_PressCylinder();
            if (!WaitIOMSec(Config.RightBend_PressCylinder_UPIOIn))
            {
                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
                return false;
            }


            Thread.Sleep(300);
            SetPressure(0, MeasurementContext.Config.LeftBendPressure * 1000);
            SetPressure(1, MeasurementContext.Config.MidBendPressure * 1000);
            SetPressure(2, MeasurementContext.Config.RightBendPressure * 1000);

            CloseLoadSTBlow();
            CloseTransferBlow();
            CloseDischargeBlow();

            OpenYellowResetLight();
            OpenBlueStartLight();

            CloseBuzzer();
            CloseYellowLight();

            CloseLeftSMFPCSuck();
            CloseLeftSMBlow();
            CloseLeftSMReduce();

            CloseRightSMReduce();
            CloseRightSMFPCSuck();
            CloseRightSMBlow();

            CloseMidSMFPCSuck();
            CloseMidSMBlow();
            CloseMidSMReduce();

            CloseLoadSTBlow();
            CloseDischargeBlow();
            CloseTransferBlow();

            CloseMidBendBlow();
            CloseLeftBendBlow();
            CloseRightBendBlow();
            OpenLeftSMSuck();
            Thread.Sleep(150);
            if (!IsLeftSMStgSuck)
            {
                CloseLeftSMSuck();
            }

            OpenMidSMSuck();
            Thread.Sleep(150);
            if (!IsMidSMStgSuck)
            {
                CloseMidSMSuck();
            }

            OpenRightSMSuck();
            Thread.Sleep(150);
            if (!IsRightSMStgSuck)
            {
                CloseRightSMSuck();
            }

            OpenLeftBendSuck();
            Thread.Sleep(150);
            if (!IsLeftBendStgSuck)
            {
                CloseLeftBendSuck();
            }

            OpenMidBendSuck();
            Thread.Sleep(150);
            if (!IsMidBendStgSuck)
            {
                CloseMidBendSuck();
            }

            OpenRightBendSuck();
            Thread.Sleep(150);
            if (!IsRightBendStgSuck)
            {
                CloseRightBendSuck();
            }

            CloseLeftBendClawCylinder();
            CloseMidBendClawCylinder();
            CloseRightBendClawCylinder();

            CloseYellowLight();
            CloseRedLight();
            OpenYellowLight();
            SetMotionSevOn();
            Thread.Sleep(200);

            if (Config.DischargeAxiaZCylinderEnable)
            {
                OpenDischargeAxisZCylinder();
            }

            if (ResetAxis())
            {
                _IsStop = false;
                _IsReset = true;
                OutputMessage("复位完成!");
                _WorkStatus = WorkStatuses.Idle;
                OnWorkStatusChanged();
            }
            else
            {
                CloseMidSMReduce();
                CloseMidSMFPCSuck();
                CloseMidSMBlow();

                _WorkStatus = WorkStatuses.Error;
                OnWorkStatusChanged();
            }

            if (Config.IsFeedCylinderEnable)
            {
                OpenFeedRotateUPCylinder();
                Thread.Sleep(300);
                OpenFeedRotateCylinder();

                if (!WaitIOMSec(Config.Feed_RotateUpDownCylinder_UpIOIn) || !WaitIOMSec(Config.Feed_RotateCylinder_UpIOIn))
                {
                    return false;
                }
            }

            if (Config.IsDischargeCylinderEnable)
            {
                OpenDischargeUPCylinder();
                Thread.Sleep(300);
                OpenDischargeRotateCylinder();
            }
            #region //ZGH20220912增加撕膜X轴安全位
            if (!IsOnPosition(_Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))
            {
                if (!AxisMoveTo(_Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))
                {
                    _Axis_LeftSM_Z.StopSlowly();
                    return false;
                }
            }
            if (!IsOnPosition(_Axis_LeftSM_X, Recipe.LeftSM_XSafePos))
            {
                if (!AxisMoveTo(_Axis_LeftSM_X, Recipe.LeftSM_XSafePos))
                {
                    _Axis_LeftSM_X.StopSlowly();
                    return false;
                }
            }
            if (!IsOnPosition(_Axis_MidSM_Z, Recipe.SMPosition[1].Lsm_WaitZ))
            {
                if (!AxisMoveTo(_Axis_MidSM_Z, Recipe.SMPosition[1].Lsm_WaitZ))
                {
                    _Axis_MidSM_Z.StopSlowly();
                    return false;
                }
            }
            if (!IsOnPosition(_Axis_MidSM_X, Recipe.MidSM_XSafePos))
            {
                if (!AxisMoveTo(_Axis_MidSM_X, Recipe.MidSM_XSafePos))
                {
                    _Axis_MidSM_X.StopSlowly();
                    return false;
                }
            }
            if (!IsOnPosition(_Axis_RightSM_Z, Recipe.SMPosition[2].Lsm_WaitZ))
            {
                if (!AxisMoveTo(_Axis_RightSM_Z, Recipe.SMPosition[2].Lsm_WaitZ))
                {
                    _Axis_RightSM_Z.StopSlowly();
                    return false;
                }
            }
            if (!IsOnPosition(_Axis_RightSM_X, Recipe.RightSM_XSafePos))
            {
                if (!AxisMoveTo(_Axis_RightSM_X, Recipe.RightSM_XSafePos))
                {
                    _Axis_RightSM_X.StopSlowly();
                    return false;
                }
            }
            #endregion
            return flag;
        }

        public void SetMotionSevOn()
        {
            _MotionA.SetSevON();
            _MotionB.SetSevON();
            _MotionC.SetSevON();
            _MotionD.SetSevON();

            CanSetIOOut(Config.AllStepMotorServoOn_IOOutEx, true);
            CanSetIOOut(Config.Left_WServoOn_IOOutEx, true);
            CanSetIOOut(Config.Mid_WServoOn_IOOutEx, true);
            CanSetIOOut(Config.Right_WServoOn_IOOutEx, true);
            CanSetIOOut(Config.SM_AOIServoOn_IOOutEx, true);
            CanSetIOOut(Config.DischargZServoOn_IOOutEx, true);
            CanSetIOOut(Config.DischargZbrak_IOOutEx, true);
        }
        public void SetMotionSevOff()
        {
            _MotionA.SetSevOFF();
            _MotionB.SetSevOFF();
            _MotionC.SetSevOFF();
            _MotionD.SetSevOFF();

            CanSetIOOut(Config.AllStepMotorServoOn_IOOutEx, false);
            CanSetIOOut(Config.Left_WServoOn_IOOutEx, false);
            CanSetIOOut(Config.Mid_WServoOn_IOOutEx, false);
            CanSetIOOut(Config.Right_WServoOn_IOOutEx, false);
            CanSetIOOut(Config.SM_AOIServoOn_IOOutEx, false);
            CanSetIOOut(Config.DischargZServoOn_IOOutEx, false);
            CanSetIOOut(Config.DischargZbrak_IOOutEx, false);
        }
        private bool ResetAxis()
        {
            bool result = true;
            bool res1 = true;
            bool res2 = true;
            bool res3 = true;
            bool res4 = true;

            bool stopa = false;
            bool stopb = false;
            bool stopc = false;
            bool stopd = false;

            AxisBase[] Axise1 = new AxisBase[] { _Axis_LeftSM_Z, _Axis_MidSM_Z };
            AxisBase[] Axise2;

            List<AxisBase> axiseZTemp = new List<AxisBase>();
            axiseZTemp.Add(_Axis_RightSM_Z);
            axiseZTemp.Add(_Axis_Discharge_Z);

            if (!Config.IsLoadZCylinder)
            {
                axiseZTemp.Add(Axis_Load_Z);
            }

            if (!Config.IsTransferZCylinder)
            {
                axiseZTemp.Add(_Axis_Transfer_Z);
            }
            Axise2 = axiseZTemp.ToArray();

            AxisBase[] Axise5;
            if (Config.IsManualLoad)
            {
                Axise5 = new AxisBase[] { _Axis_LeftSM_X, _Axis_LeftSM_Y, _Axis_MidSM_X, _Axis_MidSM_Y, _Axis_RightSM_X, _Axis_RightSM_Y };//, _Axis_Load_Y
            }
            else
            {
                Axise5 = new AxisBase[] { _Axis_LeftSM_X, _Axis_LeftSM_Y, _Axis_MidSM_X, _Axis_MidSM_Y, _Axis_RightSM_X, _Axis_RightSM_Y, _Axis_Load_Y };//, _Axis_Load_Y
            }
            AxisBase[] Axise6 = new AxisBase[] { _Axis_Load_X, _Axis_Transfer_X, _Axis_LeftBend_CCDX, _Axis_LeftBend_DWX, _Axis_LeftBend_DWY, _Axis_SMCCD_X };
            AxisBase[] Axise7 = new AxisBase[] { _Axis_LeftBend_stgY, _Axis_MidBend_DWY, _Axis_MidBend_CCDX, _Axis_MidBend_DWX };
            AxisBase[] Axise8 = new AxisBase[] { _Axis_MidBend_stgY, _Axis_RightBend_CCDX, _Axis_RightBend_DWX, _Axis_RightBend_DWY, _Axis_RightBend_stgY, _Axis_Discharge_X };


            AxisBase[] Axise3 = new AxisBase[] { /*_Axis_LeftBend_DWR, _Axis_MidBend_DWR,*/ _Axis_LeftBend_DWW, _Axis_MidBend_DWW };
            AxisBase[] Axise4 = new AxisBase[] { /*_Axis_RightBend_DWR, */_Axis_RightBend_DWW };

            AxisBase[] Axise9 = new AxisBase[] { _Axis_LeftBend_DWR, _Axis_MidBend_DWR };
            AxisBase[] Axise10 = new AxisBase[] { _Axis_RightBend_DWR };

            OutputMessage("Z轴开始复位!");
            ThreadPool.QueueUserWorkItem(delegate
            {
                res1 = _MotionA.GoHome(Axise1);
                stopa = true;
            });

            ThreadPool.QueueUserWorkItem(delegate
            {
                res2 = _MotionB.GoHome(Axise2);
                stopb = true;
            });



            while (!stopa || !stopb)
            {
                Thread.Sleep(50);
            }

            if (!(res1 & res2))
            {
                OutputError("Z轴复位失败!");
                return false;
            }
            else
            {


                int i = 0;
                while (i < Axise1.Length)
                {
                    MeasurementAxis axis = Axise1[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        result = false;
                        return result;
                    }
                }

                i = 0;
                while (i < Axise2.Length)
                {
                    MeasurementAxis axis = Axise2[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        result = false;
                        return result;
                    }
                }


            }


            stopa = false;
            stopb = false;
            stopc = false;
            stopd = false;

            if (!AxisMove(_Axis_Discharge_Z, Config.DischargeZGoHomeOffset))
            {
                return false;
            }


            OutputMessage("平台XY轴开始复位!");
            ThreadPool.QueueUserWorkItem(delegate
            {
                res1 = _MotionA.GoHome(Axise5);
                stopa = true;
            });

            ThreadPool.QueueUserWorkItem(delegate
            {
                res2 = _MotionB.GoHome(Axise6);
                stopb = true;
            });

            while (!stopa || !stopb)
            {
                Thread.Sleep(50);
            }

            ThreadPool.QueueUserWorkItem(delegate
            {
                res3 = _MotionC.GoHome(Axise7);
                stopc = true;
            });

            ThreadPool.QueueUserWorkItem(delegate
            {
                res4 = _MotionD.GoHome(Axise8);
                stopd = true;
            });

            while (!stopc || !stopd)
            {
                Thread.Sleep(50);
            }

            if (!(res1 & res2 & res3 & res4))
            {
                OutputError("平台XY轴复位失败!!");
                return false;
            }
            else
            {

                int i = 0;
                while (i < Axise5.Length)
                {
                    MeasurementAxis axis = Axise5[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        return false;
                    }
                }
                i = 0;
                while (i < Axise6.Length)
                {
                    MeasurementAxis axis = Axise6[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        return false;
                    }
                }

                i = 0;
                while (i < Axise7.Length)
                {
                    MeasurementAxis axis = Axise7[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        return false;
                    }
                }

                i = 0;
                while (i < Axise8.Length)
                {
                    MeasurementAxis axis = Axise8[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            stopa = false;
            stopb = false;
            stopc = false;
            stopd = false;
            if (!AxisMove(_Axis_Load_X, Config.LoadXGoHomeOffset))
            {
                return false;
            }
            OutputMessage("反折R轴开始复位!");
            ThreadPool.QueueUserWorkItem(delegate
            {
                res1 = _MotionC.GoHome(Axise9);
                stopa = true;
            });

            ThreadPool.QueueUserWorkItem(delegate
            {
                res2 = _MotionD.GoHome(Axise10);
                stopb = true;
            });










            while (!stopa || !stopb)
            {
                Thread.Sleep(50);
            }

            if (!(res1 & res2))
            {
                OutputError("反折R轴复位失败!");
                return false;
            }
            else
            {
                int i = 0;
                while (i < Axise9.Length)
                {
                    MeasurementAxis axis = Axise9[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        return false;
                    }
                }
                i = 0;
                while (i < Axise10.Length)
                {
                    MeasurementAxis axis = Axise10[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        return false;
                    }
                }
            }



            ThreadPool.QueueUserWorkItem(delegate
            {
                res3 = _MotionC.GoHome(Axise3);
                stopc = true;
            });

            ThreadPool.QueueUserWorkItem(delegate
            {
                res4 = _MotionD.GoHome(Axise4);
                stopd = true;
            });


            while (!stopc || !stopd)
            {
                Thread.Sleep(50);
            }

            if (!(res3 & res4))
            {
                OutputError("反折W轴复位失败!");
                return false;
            }
            else
            {

                int i = 0;
                while (i < Axise3.Length)
                {
                    MeasurementAxis axis = Axise3[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        return false;
                    }
                }

                i = 0;
                while (i < Axise4.Length)
                {
                    MeasurementAxis axis = Axise4[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        return false;
                    }
                }
            }



            if ((!Axis_LeftSM_W.Move(40)) || (!Axis_MidSM_W.Move(40)) || (!Axis_RightSM_W.Move(40)))
            {
                _Axis_LeftSM_W.StopSlowly();
                _Axis_MidSM_W.StopSlowly();
                _Axis_RightSM_W.StopSlowly();
                return false;
            }

            if (!CheckAxisDone(_Axis_LeftSM_W))
            {
                return false;
            }

            if (!CheckAxisDone(_Axis_MidSM_W))
            {
                return false;
            }

            if (!CheckAxisDone(_Axis_RightSM_W))
            {
                return false;
            }

            _Axis_LeftSM_W.PositionDev = 0;
            _Axis_LeftSM_W.PositionCode = 0;
            _Axis_MidSM_W.PositionDev = 0;
            _Axis_MidSM_W.PositionCode = 0;
            _Axis_RightSM_W.PositionDev = 0;
            _Axis_RightSM_W.PositionCode = 0;
            return result;

        }
        public void ConnectNet()
        {
            BendCCDNet.Ip = "192.160.0.1";
            BendCCDNet.Port = 6666;
            BendCCDNet.StartConnection();


            Bend2CCDNet.Ip = "192.160.0.1";
            Bend2CCDNet.Port = 7777;
            Bend2CCDNet.StartConnection();


            Bend3CCDNet.Ip = "192.160.0.1";
            Bend3CCDNet.Port = 8888;
            Bend3CCDNet.StartConnection();

            TearCCDNet.Ip = "192.160.0.1";
            TearCCDNet.Port = 9999;
            TearCCDNet.StartConnection();

            LoadCell1Net.Ip = "192.168.2.200";
            LoadCell1Net.Port = 502;
            LoadCell1Net.StartConnection();

            LoadCell2Net.Ip = "192.168.2.201";
            LoadCell2Net.Port = 502;
            LoadCell2Net.StartConnection();

            LoadCell3Net.Ip = "192.168.2.202";
            LoadCell3Net.Port = 502;
            LoadCell3Net.StartConnection();

            //QRCodeNet.Ip = "192.168.2.203";
            //QRCodeNet.Port = 502;
            //QRCodeNet.StartConnection();

        }
        public void CloseNet()
        {
            BendCCDNet.StopConnection();
            BendCCDNet.Dispose();

            Bend2CCDNet.StopConnection();
            Bend2CCDNet.Dispose();

            Bend3CCDNet.StopConnection();
            Bend3CCDNet.Dispose();

            TearCCDNet.StopConnection();
            TearCCDNet.Dispose();

            LoadCell1Net.StopConnection();
            LoadCell1Net.Dispose();

            LoadCell2Net.StopConnection();
            LoadCell2Net.Dispose();

            LoadCell3Net.StopConnection();
            LoadCell3Net.Dispose();

            //QRCodeNet.StopConnection();
            //QRCodeNet.Dispose();
        }
        public void CloseNetEvent()
        {
            BendCCDNet.OnStateInfo -= Bend1StateInfo;
            BendCCDNet.OnReceviceByte -= Bend1ReciveceMsg;
            BendCCDNet.OnErrorMsg -= Bend1ErrRecevice;

            Bend2CCDNet.OnStateInfo -= Bend2StateInfo;
            Bend2CCDNet.OnReceviceByte -= Bend2ReciveceMsg;
            Bend2CCDNet.OnErrorMsg -= Bend2ErrRecevice;

            Bend3CCDNet.OnStateInfo -= Bend3StateInfo;
            Bend3CCDNet.OnReceviceByte -= Bend3ReciveceMsg;
            Bend3CCDNet.OnErrorMsg -= Bend3ErrRecevice;

            LoadCell1Net.OnStateInfo -= LoadCell1StateInfo;
            LoadCell1Net.OnReceviceByte -= LoadCell1ReciveceMsg;
            LoadCell1Net.OnErrorMsg -= LoadCell1ErrRecevice;

            LoadCell2Net.OnStateInfo -= LoadCell2StateInfo;
            LoadCell2Net.OnReceviceByte -= LoadCell2ReciveceMsg;
            LoadCell2Net.OnErrorMsg -= LoadCell2ErrRecevice;

            LoadCell3Net.OnStateInfo -= LoadCell3StateInfo;
            LoadCell3Net.OnReceviceByte -= LoadCell3ReciveceMsg;
            LoadCell3Net.OnErrorMsg -= LoadCell3ErrRecevice;


            QRCodeNet.OnStateInfo -= QrCodeStateInfo;
            QRCodeNet.OnReceviceByte -= QrCodeReciveceMsg;
            QRCodeNet.OnErrorMsg -= QrCodeErrRecevice;

            TearCCDNet.OnStateInfo -= TearStateInfo;
            TearCCDNet.OnReceviceByte -= TearReciveceMsg;
            TearCCDNet.OnErrorMsg -= TearErrRecevice;
        }
        public bool SendMsg(string msg)
        {

            bool result = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();

            if (!BendCCDNet.IsConnected)
            {
                BendCCDNet.StartConnection();
            }
            else
            {
                while (!BendCCDNet.IsConnected)
                {
                    if (stw.ElapsedMilliseconds > 5000)
                    {
                        return false;
                    }
                }
                result = BendCCDNet.SendCommand(msg);
            }
            return result;
        }
        public bool Bend2SendMsg(string msg)
        {
            bool result = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();

            if (!Bend2CCDNet.IsConnected)
            {
                Bend2CCDNet.StartConnection();
            }
            else
            {
                while (!Bend2CCDNet.IsConnected)
                {
                    if (stw.ElapsedMilliseconds > 5000)
                    {
                        return false;
                    }
                }
                result = Bend2CCDNet.SendCommand(msg);
            }
            return result;
        }
        public bool Bend3SendMsg(string msg)
        {
            bool result = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();

            if (!Bend3CCDNet.IsConnected)
            {
                Bend3CCDNet.StartConnection();
            }
            else
            {
                while (!Bend3CCDNet.IsConnected)
                {
                    if (stw.ElapsedMilliseconds > 5000)
                    {
                        return false;
                    }
                }
                result = Bend3CCDNet.SendCommand(msg);
            }
            return result;
        }
        public bool Bend1CCDGetGap(ref double[] pos)
        {
            bool result = true;
            double[] poses = new double[] { -100, -100 };
            str_Bend1Rev = "";
            if (!SendMsg("A,PZS"))//PZS"
            {
                OutputError("折弯相机1发送拍照数据失败");
                pos = poses;
                return false;
            }
            else
            {
                Stopwatch stt = new Stopwatch();
                stt.Restart();
                //  OutputMessage("等待折弯相机1返回数据!");
                //   WriteLog("折弯1  PZS等待返回数据!");
                while (str_Bend1Rev.Length < 2)
                {
                    if (stt.ElapsedMilliseconds > NetTimeOut)
                    {
                        pos = poses;
                        BendCCDNet.StopConnection();
                        BendCCDNet.Dispose();
                        Thread.Sleep(200);
                        BendCCDNet.StartConnection();
                        OutputError("折弯相机1接收信息超时报警!");
                        WriteLog("折弯1PZS数据等待超时!");
                        return false;
                    }
                    if (_IsStop)
                    {
                        pos = poses;
                        return false;
                    }
                    Thread.Sleep(20);
                }
                WriteLog(string.Format("折弯1   PZS数据返回{0}!", str_Bend1Rev));
                pos = SplitYString(str_Bend1Rev);
            }
            return result;
        }
        public bool Bend2CCDGetGap(ref double[] pos)
        {
            bool result = true;
            double[] poses = new double[] { -100, -100 };
            str_Bend2Rev = "";
            if (!Bend2SendMsg("B,PZS"))
            {
                OutputError("折弯相机2发送拍照数据失败");
                pos = poses;
                return false;
            }
            else
            {
                Stopwatch stt = new Stopwatch();
                stt.Restart();
                //OutputMessage("等待折弯相机2返回数据!");
                //WriteLog("折弯2  PZS等待返回数据!");
                while (str_Bend2Rev.Length < 2)
                {
                    if (stt.ElapsedMilliseconds > NetTimeOut)
                    {



                        //Bend2CCDNet.StopConnection();
                        //Bend2CCDNet.Dispose();
                        //Thread.Sleep(200);
                        //Bend2CCDNet.StartConnection();
                        OutputError("折弯相机2接收信息超时报警!");
                        WriteLog("折弯2 PZS数据等待超时!");
                        pos = poses;
                        return false;
                    }
                    if (_IsStop)
                    {
                        pos = poses;
                        return false;
                    }
                    Thread.Sleep(20);
                }
                WriteLog(string.Format("折弯2   PZS数据返回{0}!", str_Bend2Rev));
                pos = Split2YString(str_Bend2Rev);
            }
            return result;
        }
        public bool Bend3CCDGetGap(ref double[] pos)
        {
            bool result = true;
            double[] poses = new double[] { -100, -100 };
            str_Bend3Rev = "";
            if (!Bend3SendMsg("C,PZS"))
            {
                OutputError("折弯相机3发送拍照数据失败");
                pos = poses;
                return false;
            }
            else
            {
                Stopwatch stt = new Stopwatch();
                stt.Restart();
                //OutputMessage("等待折弯相机3返回数据!");
                //WriteLog("折弯3  PZS等待返回数据!");
                while (str_Bend3Rev.Length < 2)
                {
                    if (stt.ElapsedMilliseconds > NetTimeOut)
                    {
                        Bend3CCDNet.StopConnection();
                        Bend3CCDNet.Dispose();
                        Thread.Sleep(200);
                        Bend3CCDNet.StartConnection();
                        OutputError("折弯相机3接收信息超时报警!");
                        WriteLog("折弯3PZS数据等待超时!");
                        pos = poses;
                        return false;
                    }
                    if (_IsStop)
                    {
                        pos = poses;
                        return false;
                    }
                    Thread.Sleep(20);
                }
                WriteLog(string.Format("折弯3   PZS数据返回{0}!", str_Bend3Rev));
                pos = Split3YString(str_Bend3Rev);
            }
            return result;
        }
        public bool Bend1GetAOIResult(ref double[] pos)
        {
            bool result = true;
            double[] poses = new double[] { -100, -100, -100, -100, -100 };
            str_Bend1Rev = "";
            if (!SendMsg("A,AOI"))
            {
                pos = poses;
                OutputError("折弯相机1发送拍照数据失败");
                return false;
            }
            else
            {
                Stopwatch stt = new Stopwatch();
                stt.Restart();
                //OutputMessage("等待折弯相机1返回数据!");
                //WriteLog("折弯1  AOI等待返回数据!");
                while (str_Bend1Rev.Length < 2)
                {
                    if (stt.ElapsedMilliseconds > NetTimeOut)
                    {
                        BendCCDNet.StopConnection();
                        BendCCDNet.Dispose();
                        Thread.Sleep(200);
                        BendCCDNet.StartConnection();
                        OutputError("折弯相机1接收信息超时报警!");
                        WriteLog("折弯1 AOI数据等待超时!");
                        pos = poses;
                        return false;
                    }
                    if (_IsStop)
                    {
                        pos = poses;
                        return false;
                    }
                    Thread.Sleep(20);
                }
                WriteLog(string.Format("折弯1   AOI数据返回{0}!", str_Bend1Rev));
                pos = SplitAOIString(str_Bend1Rev);
            }
            return result;
        }
        public bool Bend2GetAOIResult(ref double[] pos)
        {
            bool result = true;
            double[] poses = new double[] { -100, -100, -100, -100, -100 };
            str_Bend2Rev = "";
            if (!Bend2SendMsg("B,AOI"))
            {
                pos = poses;
                OutputError("折弯相机2发送拍照数据失败");
                return false;
            }
            else
            {
                Stopwatch stt = new Stopwatch();
                stt.Restart();
                //OutputMessage("等待折弯相机2返回数据!");
                //WriteLog("折弯2  AOI等待返回数据!");
                while (str_Bend2Rev.Length < 2)
                {
                    if (stt.ElapsedMilliseconds > NetTimeOut)
                    {
                        Bend2CCDNet.StopConnection();
                        Bend2CCDNet.Dispose();
                        Thread.Sleep(200);
                        Bend2CCDNet.StartConnection();
                        OutputError("折弯相机2接收信息超时报警!");
                        WriteLog("折弯2 AOI数据等待超时!");
                        pos = poses;
                        return false;
                    }
                    if (_IsStop)
                    {
                        pos = poses;
                        return false;
                    }
                    Thread.Sleep(20);
                }
                WriteLog(string.Format("折弯2   AOI数据返回{0}!", str_Bend2Rev));
                pos = SplitAOIString(str_Bend2Rev);
            }
            return result;
        }
        public bool Bend3GetAOIResult(ref double[] pos)
        {
            bool result = true;
            double[] poses = new double[] { -100, -100, -100, -100, -100 };
            str_Bend3Rev = "";
            if (!Bend3SendMsg("C,AOI"))
            {
                pos = poses;
                OutputError("折弯相机3发送拍照数据失败");
                return false;
            }
            else
            {
                Stopwatch stt = new Stopwatch();
                stt.Restart();
                //OutputMessage("等待折弯相机3返回数据!");
                //WriteLog("折弯3  AOI等待返回数据!");
                while (str_Bend3Rev.Length < 2)
                {
                    if (stt.ElapsedMilliseconds > NetTimeOut)
                    {
                        Bend3CCDNet.StopConnection();
                        Bend3CCDNet.Dispose();
                        Thread.Sleep(200);
                        Bend3CCDNet.StartConnection();
                        OutputError("折弯相机3接收信息超时报警!");
                        WriteLog("折弯3 AOI数据等待超时!");
                        pos = poses;
                        return false;
                    }
                    if (_IsStop)
                    {
                        pos = poses;
                        return false;
                    }
                    Thread.Sleep(20);
                }
                WriteLog(string.Format("折弯3   AOI数据返回{0}!", str_Bend3Rev));
                pos = SplitAOIString(str_Bend3Rev);

            }
            return result;
        }
        public bool TearSendMsg(string msg)
        {
            bool result = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();
            if (!TearCCDNet.IsConnected)
            {
                TearCCDNet.StartConnection();
            }
            else
            {
                while (!TearCCDNet.IsConnected)
                {
                    if (stw.ElapsedMilliseconds > 5000)
                    {
                        return false;
                    }
                }
                result = TearCCDNet.SendCommand(msg);
            }
            return result;
        }
        public bool LeftMoveDegree()
        {

            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                str_Bend1Rev = "";
                if (!SendMsg("A,PZF"))
                {
                    MessageBox.Show("折弯相机1发送拍照数据失败");
                    return false;
                }
                else
                {
                    while (str_Bend1Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            MessageBox.Show("手动停止!");
                            return false;
                        }
                        Thread.Sleep(20);
                    }
                }

                double pos = SplitDegreeString(str_Bend1Rev);

                if (Math.Abs(pos) > 10)
                {
                    ShowMessage("角度返回数据过大，或失败");
                    return false;
                }
                if (!AxisMove(_Axis_LeftBend_DWW, -Recipe.LeftBend_DWW_BasePos + pos))
                {
                    _Axis_LeftBend_DWW.StopSlowly();
                    return false;
                }

                return result;
            }
        }
        public bool MidMoveDegree()
        {

            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {

                str_Bend2Rev = "";
                if (!Bend2SendMsg("B,PZF"))
                {
                    MessageBox.Show("折弯相机2发送拍照数据失败");
                    return false;
                }
                else
                {
                    while (str_Bend2Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            MessageBox.Show("手动停止!");
                            return false;
                        }
                        Thread.Sleep(20);
                    }
                }
                double pos = SplitDegreeString(str_Bend2Rev);

                if (Math.Abs(pos) > 10)
                {
                    ShowMessage("角度返回数据过大，或失败");
                    return false;
                }
                if (!AxisMove(_Axis_MidBend_DWW, -Recipe.MidBend_DWW_BasePos + pos))
                {
                    _Axis_MidBend_DWW.StopSlowly();
                    return false;
                }
                return result;
            }
        }
        public bool RightMoveDegree()
        {

            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {

                str_Bend3Rev = "";
                if (!Bend3SendMsg("C,PZF"))
                {
                    MessageBox.Show("折弯相机3发送拍照数据失败");
                    return false;
                }
                else
                {
                    while (str_Bend3Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            MessageBox.Show("手动停止!");
                            return false;
                        }
                        Thread.Sleep(20);
                    }
                }


                double pos = SplitDegreeString(str_Bend3Rev);


                if (Math.Abs(pos) > 10)
                {
                    ShowMessage("角度返回数据过大，或失败");
                    return false;
                }

                if (!AxisMove(_Axis_RightBend_DWW, -Recipe.RightBend_DWW_BasePos + pos))
                {
                    _Axis_RightBend_DWW.StopSlowly();
                    return false;
                }
                return result;
            }
        }
        public bool LeftMoveY()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                str_Bend1Rev = "";
                if (!SendMsg("A,PZS"))//PZS
                {
                    MessageBox.Show("折弯相机1发送拍照数据失败");
                    return false;
                }
                else
                {
                    while (str_Bend1Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            MessageBox.Show("手动停止!");
                            return false;
                        }
                        Thread.Sleep(20);
                    }
                }
                double[] poo = SplitYString(str_Bend1Rev);


                if (Math.Abs(poo[0] - Recipe.BendPara[0].BaseRate) > Recipe.BendPara[0].BaseRate * 0.5
                    || Math.Abs(poo[1] - Recipe.BendPara[1].BaseRate) > Recipe.BendPara[1].BaseRate * 0.5)
                {
                    AlarmWork();
                    OnMessageShow("视觉处理返回数据过大");
                    return false;
                }

                if (Math.Abs(poo[0] - Recipe.BendPara[0].BaseRate) > Recipe.BendPara[0].ErrAnd || Math.Abs(poo[1] - Recipe.BendPara[1].BaseRate) > Recipe.BendPara[0].ErrAnd)
                {
                    double[] poses = Calculate_DeltXY(poo, (int)StationType.Left);
                    if (!AxisMove(_Axis_LeftBend_DWX, poses[0] * Recipe.BendPara[0].DirValue))
                    {
                        _Axis_LeftBend_DWX.StopSlowly();
                        return false;
                    }

                    if (!AxisMove(_Axis_LeftBend_DWY, poses[1] * Recipe.BendPara[1].DirValue))
                    {
                        _Axis_LeftBend_DWY.StopSlowly();
                        return false;
                    }
                }
                else
                {
                    OnMessageShow("反折校正到位");
                }
                return result;
            }
        }
        public bool MidMoveY()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {

                str_Bend2Rev = "";
                if (!Bend2SendMsg("B,PZS"))
                {
                    MessageBox.Show("折弯相机2发送拍照数据失败");
                    return false;
                }
                else
                {
                    while (str_Bend2Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            MessageBox.Show("手动停止!");
                            return false;
                        }
                        Thread.Sleep(20);
                    }
                }

                double[] poo = Split2YString(str_Bend2Rev);

                if (Math.Abs(poo[0] - Recipe.BendPara[2].BaseRate) > Recipe.BendPara[2].BaseRate * 0.5
                    || Math.Abs(poo[1] - Recipe.BendPara[3].BaseRate) > Recipe.BendPara[3].BaseRate * 0.5)
                {
                    AlarmWork();
                    OnMessageShow("视觉处理返回数据过大");
                    return false;
                }

                if (Math.Abs(poo[0] - Recipe.BendPara[2].BaseRate) > Recipe.BendPara[2].ErrAnd || Math.Abs(poo[1] - Recipe.BendPara[3].BaseRate) > Recipe.BendPara[3].ErrAnd)
                {
                    double[] poses = Calculate_DeltXY(poo, (int)StationType.Mid);
                    if (!AxisMove(_Axis_MidBend_DWX, poses[0] * Recipe.BendPara[2].DirValue))
                    {
                        _Axis_MidBend_DWX.StopSlowly();
                        return false;
                    }
                    if (!AxisMove(_Axis_MidBend_DWY, poses[1] * Recipe.BendPara[3].DirValue))
                    {
                        _Axis_MidBend_DWY.StopSlowly();
                        return false;
                    }
                }
                else
                {
                    OnMessageShow("右反折校正到位");
                }
            }
            return result;
        }

        public bool RightMoveY()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                str_Bend3Rev = "";
                if (!Bend3SendMsg("C,PZS"))//PZS
                {
                    MessageBox.Show("折弯相机3发送拍照数据失败");
                    return false;
                }
                else
                {
                    while (str_Bend3Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            MessageBox.Show("手动停止!");
                            return false;
                        }
                        Thread.Sleep(20);
                    }
                }

                double[] poo = Split3YString(str_Bend3Rev);
                if (Math.Abs(poo[0] - Recipe.BendPara[4].BaseRate) > Recipe.BendPara[4].BaseRate * 0.5
                    || Math.Abs(poo[1] - Recipe.BendPara[5].BaseRate) > Recipe.BendPara[5].BaseRate * 0.5)
                {
                    AlarmWork();
                    OnMessageShow("视觉处理返回数据过大");
                    return false;
                }

                if (Math.Abs(poo[0] - Recipe.BendPara[4].BaseRate) > Recipe.BendPara[4].ErrAnd || Math.Abs(poo[1] - Recipe.BendPara[5].BaseRate) > Recipe.BendPara[5].ErrAnd)
                {
                    double[] poses = Calculate_DeltXY(poo, (int)StationType.Right);
                    if (!AxisMove(_Axis_RightBend_DWX, poses[0] * Recipe.BendPara[4].DirValue))
                    {
                        _Axis_RightBend_DWX.StopSlowly();
                        return false;
                    }
                    if (!AxisMove(_Axis_RightBend_DWY, poses[1] * Recipe.BendPara[5].DirValue))
                    {
                        _Axis_RightBend_DWY.StopSlowly();
                        return false;
                    }
                }
                else
                {
                    OnMessageShow("右反折校正到位");
                }
            }
            return result;
        }

        /// <summary>
        /// 进料Z动作 可能Z是气缸 可能是伺服
        /// </summary>
        /// <param name="updown"> 气缸上升0 下降1</param>
        /// <param name="pos">轴点位</param>
        /// <param name="ismanual">是否手动模式</param>
        /// <returns></returns>
        public bool FeedZWork(int updown, double pos, bool ismanual = false)
        {
            bool result = true;
            if (ismanual && !CheckRun())//手动模式下检测
            {
                return false;
            }

            if (Config.IsLoadZCylinder)
            {
                if (updown == 0)
                {
                    FeedUDCylinderUp();
                    while (!WaitIOExMSec(Config.Feed_UpDownCylinder_UpIOInEx, 3000, true))
                    {
                        if (IsStop || ismanual)
                        {
                            return false;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "进料上下气缸动作失败";
                        fra.ShowDialog();
                    }
                }
                else
                {
                    FeedUDCylinderDown();
                    while (!WaitIOExMSec(Config.Feed_UpDownCylinder_DownIOInEx, 3000, true))
                    {
                        if (IsStop || ismanual)
                        {
                            return false;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "进料上下气缸动作失败";
                        fra.ShowDialog();
                    }
                }
                return result;
            }

            if (!AxisMoveTo(_Axis_Load_Z, pos))
            {
                result = false;
                _Axis_Load_Z.StopSlowly();
            }
            return result;
        }



        /// <summary>
        /// 中转Z动作 可能Z是气缸 可能是伺服
        /// </summary>
        /// <param name="updown"> 气缸上升0 下降1</param>
        /// <param name="pos">轴点位</param>
        /// <param name="ismanual">是否手动模式</param>
        /// <returns></returns>
        public bool TransferZWork(int updown, double pos, bool ismanual = false)
        {
            bool result = true;
            if (ismanual && !CheckRun())//手动模式下检测
            {
                return false;
            }

            if (Config.IsTransferZCylinder)
            {
                if (updown == 0)
                {
                    TransferUDCylinderUp();
                    while (!WaitIOMSec_TransferUDCylinderUP(3000))
                    {
                        if (IsStop || ismanual)
                        { return false; }

                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "中转上下气缸动作失败";
                        fra.ShowDialog();
                    }
                }
                else
                {
                    TransferUDCylinderDown();
                    while (!WaitIOMSec_TransferUDCylinderDown(3000))
                    {
                        if (IsStop || ismanual)
                        {
                            return false;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "中转上下气缸动作失败";
                        fra.ShowDialog();

                    }
                }
                return result;
            }

            if (!AxisMoveTo(_Axis_Transfer_Z, pos))
            {
                result = false;
                _Axis_Transfer_Z.StopSlowly();
            }
            return result;
        }

        public bool DischargeZGoPos(double pos)
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                if (!AxisMoveTo(_Axis_Discharge_Z, pos))
                {
                    result = false;
                    _Axis_Discharge_Z.StopSlowly();
                }
            }
            return result;
        }

        public bool AxisFeedXGoPos(double pos)
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {

                if (!AxisMoveTo(_Axis_Load_X, pos))
                {
                    _Axis_Load_X.StopSlowly();
                    return false;
                }
            }
            return result;
        }
        public bool AxisTransferXGoPos(double pos)
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }

            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                return false;
            }

            if (!AxisMoveTo(_Axis_Transfer_X, pos))
            {
                _Axis_Transfer_X.StopSlowly();
                return false;
            }

            return result;

        }
        public bool AxisTransferXYMoveTo(MeasurementAxis axisY, double[] poses)
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                if (!TransferZWork(0, Recipe.TransferZSafePos, true))
                {
                    return false;
                }


                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_Transfer_X, axisY }, poses))
                {
                    axisY.StopSlowly();
                    _Axis_Transfer_X.StopSlowly();
                    return false;
                }
            }
            return result;
        }
        public bool DischargeXGoPos(double pos)
        {
            bool result = true;

            if (Config.DischargeAxiaZCylinderEnable && !IsDischargeAxisZCylinder_UP)
            {
                if (DischargeAxisZCylinderUp() == -2) return false;
            }


            if (!CheckRun())
            {
                return false;
            }
            else
            {
                if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                {
                    OutputError("下料Z轴运动失败!");
                    _Axis_Discharge_Z.StopSlowly();
                    return false;
                }

                if (_Axis_Load_X.PositionDev > Recipe.SMPosition[1].Lsm_loadX)
                {

                    if (!FeedZWork(0, Recipe.LoadZWaitPos, true))
                    {
                        return false;
                    }

                    if (!AxisMoveTo(_Axis_Load_X, Recipe.SMPosition[1].Lsm_loadX))
                    {
                        _Axis_Load_X.StopSlowly();
                        return false;
                    }

                }


                if (!AxisMoveTo(_Axis_Discharge_X, pos))
                {
                    _Axis_Discharge_X.StopSlowly();
                    return false;
                }
            }
            return result;
        }

        public bool DischargeXYMoveTo(MeasurementAxis axisY, double[] poses)
        {
            bool result = true;
            if (Config.DischargeAxiaZCylinderEnable && !IsDischargeAxisZCylinder_UP)
            {
                if (DischargeAxisZCylinderUp() == -2) return false;
            }
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                {
                    OutputError("下料Z轴运动失败!");
                    _Axis_Discharge_Z.StopSlowly();
                    return false;
                }

                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_Discharge_X, axisY }, poses))
                {
                    axisY.StopSlowly();
                    _Axis_Discharge_X.StopSlowly();
                    return false;
                }
            }
            return result;


        }

        private double[] Analysis_Rate(double[] array_para, int Stat_ID)
        {
            double[] temp = new double[2] { 0, 0 };
            if (Stat_ID == 0)
            {
                if (Math.Abs(array_para[0] - Recipe.BendPara[0].BaseRate) > Recipe.BendPara[0].Zone1Low
                    && Math.Abs(array_para[0] - Recipe.BendPara[0].BaseRate) <= Recipe.BendPara[0].Zone1Up)
                {
                    temp[0] = Recipe.BendPara[0].Rate1;
                }
                else if (Math.Abs(array_para[0] - Recipe.BendPara[0].BaseRate) > Recipe.BendPara[0].Zone2Low
                    && Math.Abs(array_para[0] - Recipe.BendPara[0].BaseRate) <= Recipe.BendPara[0].Zone2Up)
                {
                    temp[0] = Recipe.BendPara[0].Rate2;
                }
                else
                {
                    temp[0] = Recipe.BendPara[0].Rate3;
                }

                if (Math.Abs(array_para[1] - Recipe.BendPara[1].BaseRate) > Recipe.BendPara[1].Zone1Low
                    && Math.Abs(array_para[1] - Recipe.BendPara[1].BaseRate) <= Recipe.BendPara[1].Zone1Up)
                {
                    temp[1] = Recipe.BendPara[1].Rate1;
                }
                else if (Math.Abs(array_para[1] - Recipe.BendPara[1].BaseRate) > Recipe.BendPara[1].Zone2Low
                    && Math.Abs(array_para[1] - Recipe.BendPara[1].BaseRate) <= Recipe.BendPara[1].Zone2Up)
                {
                    temp[1] = Recipe.BendPara[1].Rate2;
                }
                else
                {
                    temp[1] = Recipe.BendPara[1].Rate3;
                }
            }
            else
            {
                int index = (int)Math.Pow(2, Stat_ID);

                if (Math.Abs(array_para[0] - Recipe.BendPara[index].BaseRate) > Recipe.BendPara[index].Zone1Low
                    && Math.Abs(array_para[0] - Recipe.BendPara[index].BaseRate) <= Recipe.BendPara[index].Zone1Up)
                {
                    temp[0] = Recipe.BendPara[index].Rate1;
                }
                else if (Math.Abs(array_para[0] - Recipe.BendPara[index].BaseRate) > Recipe.BendPara[index].Zone2Low
                    && Math.Abs(array_para[0] - Recipe.BendPara[index].BaseRate) <= Recipe.BendPara[index].Zone2Up)
                {
                    temp[0] = Recipe.BendPara[index].Rate2;
                }
                else
                {
                    temp[0] = Recipe.BendPara[index].Rate3;
                }
                index = index + 1;
                if (Math.Abs(array_para[1] - Recipe.BendPara[index].BaseRate) > Recipe.BendPara[index].Zone1Low
                    && Math.Abs(array_para[1] - Recipe.BendPara[index].BaseRate) <= Recipe.BendPara[index].Zone1Up)
                {
                    temp[1] = Recipe.BendPara[index].Rate1;
                }
                else if (Math.Abs(array_para[1] - Recipe.BendPara[index].BaseRate) > Recipe.BendPara[index].Zone2Low
                    && Math.Abs(array_para[1] - Recipe.BendPara[index].BaseRate) <= Recipe.BendPara[index].Zone2Up)
                {
                    temp[1] = Recipe.BendPara[index].Rate2;
                }
                else
                {
                    temp[1] = Recipe.BendPara[index].Rate3;
                }
            }
            return temp;
        }

        private double[] Calculate_DeltXY(double[] array_para, int Stat_ID)
        {
            double[] posrate = Analysis_Rate(array_para, Stat_ID);
            double[] poses = new double[2] { 0, 0 };


            switch (Recipe.WorkModel)
            {
                case 0://xy
                    if (Stat_ID == 0)
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[0].BaseRate) * posrate[0];
                        poses[1] = (array_para[1] - Recipe.BendPara[1].BaseRate) * posrate[1];
                    }
                    else if (Stat_ID == 1)
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[2].BaseRate) * posrate[0];
                        poses[1] = (array_para[1] - Recipe.BendPara[3].BaseRate) * posrate[1];
                    }
                    else
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[4].BaseRate) * posrate[0];
                        poses[1] = (array_para[1] - Recipe.BendPara[5].BaseRate) * posrate[1];
                    }
                    break;

                case 1://yx
                    if (Stat_ID == 0)
                    {
                        poses[1] = 0 - (array_para[0] - Recipe.BendPara[0].BaseRate) * posrate[0];
                        poses[0] = (array_para[1] - Recipe.BendPara[1].BaseRate) * posrate[1];
                    }
                    else if (Stat_ID == 1)
                    {
                        poses[1] = 0 - (array_para[0] - Recipe.BendPara[2].BaseRate) * posrate[0];
                        poses[0] = (array_para[1] - Recipe.BendPara[3].BaseRate) * posrate[1];
                    }
                    else
                    {
                        poses[1] = 0 - (array_para[0] - Recipe.BendPara[4].BaseRate) * posrate[0];
                        poses[0] = (array_para[1] - Recipe.BendPara[5].BaseRate) * posrate[1];
                    }
                    break;

                case 2://yy
                    if (Stat_ID == 0)
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[0].BaseRate - array_para[1] + Recipe.BendPara[1].BaseRate) * posrate[0];
                        poses[1] = (array_para[0] - Recipe.BendPara[0].BaseRate + array_para[1] - Recipe.BendPara[1].BaseRate) * posrate[1] * 0.5;
                    }
                    else if (Stat_ID == 1)
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[2].BaseRate - array_para[1] + Recipe.BendPara[3].BaseRate) * posrate[0];
                        poses[1] = (array_para[0] - Recipe.BendPara[2].BaseRate + array_para[1] - Recipe.BendPara[3].BaseRate) * posrate[1] * 0.5;
                    }
                    else
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[4].BaseRate - array_para[1] + Recipe.BendPara[5].BaseRate) * posrate[0];
                        poses[1] = (array_para[0] - Recipe.BendPara[4].BaseRate + array_para[1] - Recipe.BendPara[5].BaseRate) * posrate[1] * 0.5;
                    }
                    break;

                default:
                    poses[0] = 0;
                    poses[1] = 0;
                    break;
            }
            return poses;
        }

        /// <summary>
        /// 没有乘比例系数 补偿值只用来进行显示
        /// </summary>
        /// <param name="array_para"></param>
        /// <param name="Stat_ID"></param>
        /// <returns></returns>
        private double[] Calculate_DeltXYPure(double[] array_para, int Stat_ID)
        {

            double[] poses = new double[2] { 0, 0 };
            switch (Recipe.WorkModel)
            {
                case 0://xy
                    if (Stat_ID == 0)
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[0].BaseRate);
                        poses[1] = (array_para[1] - Recipe.BendPara[1].BaseRate);
                    }
                    else if (Stat_ID == 1)
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[2].BaseRate);
                        poses[1] = (array_para[1] - Recipe.BendPara[3].BaseRate);
                    }
                    else
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[4].BaseRate);
                        poses[1] = (array_para[1] - Recipe.BendPara[5].BaseRate);
                    }
                    break;

                case 1://yx
                    if (Stat_ID == 0)
                    {
                        poses[1] = 0 - (array_para[0] - Recipe.BendPara[0].BaseRate);
                        poses[0] = (array_para[1] - Recipe.BendPara[1].BaseRate);
                    }
                    else if (Stat_ID == 1)
                    {
                        poses[1] = 0 - (array_para[0] - Recipe.BendPara[2].BaseRate);
                        poses[0] = (array_para[1] - Recipe.BendPara[3].BaseRate);
                    }
                    else
                    {
                        poses[1] = 0 - (array_para[0] - Recipe.BendPara[4].BaseRate);
                        poses[0] = (array_para[1] - Recipe.BendPara[5].BaseRate);
                    }
                    break;

                case 2://yy
                    if (Stat_ID == 0)
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[0].BaseRate - array_para[1] + Recipe.BendPara[1].BaseRate);
                        poses[1] = (array_para[0] - Recipe.BendPara[0].BaseRate + array_para[1] - Recipe.BendPara[1].BaseRate);
                    }
                    else if (Stat_ID == 1)
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[2].BaseRate - array_para[1] + Recipe.BendPara[3].BaseRate);
                        poses[1] = (array_para[0] - Recipe.BendPara[2].BaseRate + array_para[1] - Recipe.BendPara[3].BaseRate);
                    }
                    else
                    {
                        poses[0] = (array_para[0] - Recipe.BendPara[4].BaseRate - array_para[1] + Recipe.BendPara[5].BaseRate);
                        poses[1] = (array_para[0] - Recipe.BendPara[4].BaseRate + array_para[1] - Recipe.BendPara[5].BaseRate);
                    }
                    break;

                default:
                    poses[0] = 0;
                    poses[1] = 0;
                    break;
            }
            return poses;
        }


        private double SplitDegreeString(string str_para)
        {

            double m_dgree;
            lock (Obj_BendLock)
            {
                string[] tmp = str_para.Split(',');
                m_dgree = double.Parse(tmp[2].Trim());
            }
            return m_dgree;
        }
        private double[] SplitYString(string str_para)
        {
            string[] tmp = str_para.Split(',');
            double[] Y_array = new double[] { double.Parse(tmp[4].Trim()), double.Parse(tmp[8].Trim()) };
            return Y_array;
        }
        private double[] Split2YString(string str_para)
        {
            string[] tmp = str_para.Split(',');
            double[] Y_array = new double[] { double.Parse(tmp[4].Trim()), double.Parse(tmp[8].Trim()) };
            return Y_array;
        }
        private double[] Split3YString(string str_para)
        {
            string[] tmp = str_para.Split(',');
            double[] Y_array = new double[] { double.Parse(tmp[4].Trim()), double.Parse(tmp[8].Trim()) };
            return Y_array;
        }

        private string SplitTearString(string str)
        {
            string str_rec = str;
            string[] msg = str_rec.Split(',');
            return msg[1];
        }

        private string SplitTearString(string str, ref StringBuilder items)
        {
            string str_rec = str;
            string[] msg = str_rec.Split(',');
            items.Clear();
            if (msg.Length > 2)
            {
                //Array.ConstrainedCopy(msg, 2, indexs, 0, msg.Length - 2);
                for (int i = 2; i < msg.Length; i++)
                {
                    items.Append(msg[i] + ",");
                }
            }
            return msg[1];
        }

        private double[] SplitAOIString(string str_para)
        {
            string[] tmp = str_para.Split(',');
            double[] AOI_array = new double[] { double.Parse(tmp[2].Trim()), double.Parse(tmp[4].Trim()), double.Parse(tmp[6].Trim()), double.Parse(tmp[8].Trim()), double.Parse(tmp[10].Trim()) };
            return AOI_array;
        }
        private double[] SplitAdjustXYString(string str_para)
        {
            string[] tmp = str_para.Split(',');
            double[] XY_array = new double[] { double.Parse(tmp[2].Trim()), double.Parse(tmp[4].Trim()) };
            return XY_array;
        }

        #region Locate SM Station
        public bool LocateSMPt(int index, bool isend, MeasurementAxis[] axises)
        {

            bool result = false;

            if (!CheckRun())
            {
                return false;
            }

            double X = Recipe.SMdatas[index].SMStartX;
            double Y = Recipe.SMdatas[index].SMStartY;
            double Z = Recipe.SMdatas[index].SMStartZ;

            if (isend)
            {
                X = Recipe.SMdatas[index].SMEndX;
                Y = Recipe.SMdatas[index].SMEndY;
                Z = Recipe.SMdatas[index].SMEndZ;
            }

            if (!IsOnPosition(axises[2], 0))
            {
                if (!axises[2].MoveTo(0))
                {
                    return false;
                }
                if (!CheckAxisDone(axises[2]))
                {
                    return false;
                }
            }

            if (!axises[0].MoveTo(X) || !axises[1].MoveTo(Y))
            {
                return false;
            }
            if ((!CheckAxisDone(axises[0])))
            {
                return false;
            }

            if (!CheckAxisDone(axises[1]))
            {
                return false;
            }

            if (!axises[2].MoveTo(Z))
            {
                return false;
            }
            if (!CheckAxisDone(axises[2]))
            {
                return false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public bool LocateSMWaitPt(int flag)
        {

            bool result = false;
            if (!CheckRun())
            {
                return false;
            }

            MeasurementAxis axisx = null;
            MeasurementAxis axisy = null;
            MeasurementAxis axisz = null;
            double[] poses = new double[]
             {
                 Recipe.SMPosition[flag].Lsm_WaitX,
                 Recipe.SMPosition[flag].Lsm_WaitY,
                 Recipe.SMPosition[flag].Lsm_WaitZ
             };

            if (flag == 0)
            {
                axisx = Axis_LeftSM_X;
                axisy = Axis_LeftSM_Y;
                axisz = Axis_LeftSM_Z;
            }
            else if (flag == 1)
            {
                axisx = Axis_MidSM_X;
                axisy = Axis_MidSM_Y;
                axisz = Axis_MidSM_Z;
            }
            else if (flag == 2)
            {
                axisx = Axis_RightSM_X;
                axisy = Axis_RightSM_Y;
                axisz = Axis_RightSM_Z;
            }

            if (!AxisMoveTo(new MeasurementAxis[] { axisx, axisy, axisz }, poses))
            {
                return false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public bool LocateSMCCDPt(int flag)
        {
            bool result = false; ;
            if (!CheckRun())
            {
                return false;
            }

            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                return false;
            }


            MeasurementAxis[] axise = null;
            double[] poses = new double[] { Recipe.SMPosition[flag].Lsm_CCDX, Recipe.SMPosition[flag].Lsm_CCDY };

            if (flag == 0)
            {
                axise = new MeasurementAxis[] { Axis_SMCCD_X, Axis_LeftSM_Y };
            }
            else if (flag == 1)
            {
                axise = new MeasurementAxis[] { Axis_SMCCD_X, Axis_MidSM_Y };
            }
            else
            {
                axise = new MeasurementAxis[] { Axis_SMCCD_X, Axis_RightSM_Y };
            }

            if (!AxisMoveTo(axise, poses))
            {
                return false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public bool LocateSMLoadPt(int flag)
        {
            bool result = false;
            if (!CheckRun())
            {
                return false;
            }
            MeasurementAxis[] axises = null;
            double[] poses = new double[]
            {
                Recipe.SMPosition[flag].Lsm_loadX,
                Recipe.SMPosition[flag].Lsm_loadY,
            };

            if (flag == 0)
            {
                axises = new MeasurementAxis[]
                {
                    Axis_Load_X,
                    Axis_LeftSM_Y
                };
            }
            else if (flag == 1)
            {
                axises = new MeasurementAxis[]
                {
                    Axis_Load_X,
                    Axis_MidSM_Y
                };
            }
            else
            {
                axises = new MeasurementAxis[]
                {
                    Axis_Load_X,
                    Axis_RightSM_Y
                };
            }

            //ZGH20220905取消出料手臂移动位置（已确认机构无干涉）
            //if (flag == 2)
            //{
            //    if (_Axis_Discharge_X.PositionDev < Recipe.LeftBend_Discharge_x)
            //    {
            //        if (!AxisMoveTo(_Axis_Discharge_Z, 0))
            //        {
            //            _Axis_Discharge_Z.StopSlowly();
            //            return false;
            //        }

            //        if (!AxisMoveTo(_Axis_Discharge_X, Recipe.LeftBend_Discharge_x))
            //        {
            //            _Axis_Discharge_X.StopSlowly();
            //            return false;
            //        }
            //    }
            //}

            if (!AxisMoveTo(axises, poses))
            {
                return false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public bool LocateQrCodePt()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            MeasurementAxis axises = _Axis_Load_X;
            double poses = Recipe.QrCodePos;



            if (!AxisMoveTo(axises, poses))
            {
                _Axis_Load_X.StopSlowly();
                return false;
            }
            return result;
        }



        public bool LocateSMDischargePt(int flag)
        {
            bool result = false;
            if (!CheckRun())
            {
                return false;
            }
            MeasurementAxis[] axises = null;
            double[] poses;

            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                return false;
            }



            if (flag == 0)
            {
                axises = new MeasurementAxis[] { Axis_Transfer_X, Axis_LeftSM_Y };
            }
            else if (flag == 1)
            {
                axises = new MeasurementAxis[] { Axis_Transfer_X, Axis_MidSM_Y };
            }
            else
            {
                axises = new MeasurementAxis[] { Axis_Transfer_X, Axis_RightSM_Y };
            }

            poses = new double[] { Recipe.SMPosition[flag].Lsm_DischargeX, Recipe.SMPosition[flag].Lsm_DischargeY };

            if (!AxisMoveTo(axises, poses))
            {
                return false;
            }
            else
            {
                result = true;
            }


            return result;
        }



        #endregion

        #region others
        public bool IsOnPosition(AxisBase axis, double pos)
        {
            bool result;
            if (axis == null)
            {
                result = false;
            }
            else
            {
                double m_pos = axis.PositionDev;
                result = (Math.Abs(m_pos - pos) <= 0.01 && !axis.IsMoving());
            }
            return result;
        }




        public bool IsOnPosition(AxisBase[] axises, double[] poses)
        {
            bool result = true;
            for (int i = 0; i < axises.Length; i++)
            {
                AxisBase axis = axises[i];
                double pos = poses[i];
                double m_pos = axis.PositionDev;
                result = (Math.Abs(m_pos - pos) <= 0.01 && !axis.IsMoving());
                if (!result)
                {
                    // OutputMessage("轴" + axis.AxisSet.AxisName.ToString() + "没有到位");
                    result = false;
                    break;
                }
            }
            return result;
        }

        public bool AxisMove(AxisBase axis, double pos)
        {
            bool result;
            if (!axis.Move(pos))
            {
                AlarmWork();
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = axis.AxisSet.AxisName.ToString() + "点位运动失败,请检查点位或轴参数是否合理,轴是否过载或运动到极限位置!\r\n" + $" AxisIsMoving:{axis.IsMoving().ToString()}";
                fra.ShowDialog();

                OutputError(string.Format("{0}点位运动失败！", axis.AxisSet.AxisName));
                axis.StopSlowly();
                return false;
            }
            else
            {
                while ((DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_check_done((ushort)(axis.Motion.Id), (ushort)(axis.AxisIndex - 1)) == 0))
                {
                    Thread.Sleep(1);
                }
                result = true;
            }
            return result;

        }

        public bool AxisMoveTo(AxisBase axis, double pos)
        {

            bool result;
            if (!IsOnPosition(axis, pos))
            {
                if (!axis.MoveTo(pos))// 
                {
                    axis.StopSlowly();
                    //while (axis.IsMoving())
                    //{
                    //    Thread.Sleep(50);
                    //}
                }
                else
                {
                    while ((DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_check_done((ushort)(axis.Motion.Id), (ushort)(axis.AxisIndex - 1)) == 0))
                    {
                        Thread.Sleep(1);
                    }
                }
            }

            if (!IsOnPosition(axis, pos))
            {
                AlarmWork();
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = axis.AxisSet.AxisName.ToString() + "点位运动失败,设备运行停止,请检查点位或轴参数是否合理,轴是否过载或运动到极限位置！";
                fra.ShowDialog();
                OutputError(string.Format("{0}点位运动失败！", axis.AxisSet.AxisName));
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }


        public bool AxisMoveTo(AxisBase axis, double pos, double speed)
        {
            bool result;
            if (speed == 0)
            {
                speed = 5;
            }

            if (!IsOnPosition(axis, pos))
            {
                if (!axis.MoveTo(pos, speed))
                {
                    axis.StopSlowly();
                }
                else
                {
                    while ((DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_check_done((ushort)(axis.Motion.Id), (ushort)(axis.AxisIndex - 1)) == 0))
                    {
                        Thread.Sleep(1);
                    }
                }
            }

            if (!IsOnPosition(axis, pos))
            {
                AlarmWork();
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = axis.AxisSet.AxisName.ToString() + "点位运动失败,设备运行停止,请检查点位或轴参数是否合理,轴是否过载或运动到极限位置！";
                fra.ShowDialog();

                OutputError(string.Format("{0}点位运动失败！", axis.AxisSet.AxisName));
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public bool AxisMoveTo(AxisBase[] axises, double[] poses)
        {
            bool result;
            for (int i = 0; i < axises.Length; i++)
            {
                AxisBase axis = axises[i];
                double pos = poses[i];
                if (!IsOnPosition(axis, pos))
                {
                    if (!axis.MoveTo(pos))
                    {
                        AlarmWork();
                        axis.StopSlowly();
                        IsStop = true;
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = axis.AxisSet.AxisName.ToString() + "点位运动失败,设备运行停止,请检查点位或轴参数是否合理,轴是否过载或运动到极限位置！";
                        fra.ShowDialog();
                        OutputError(axis.AxisSet.AxisName.ToString() + "点位运动失败！");
                        return false;
                    }
                }
            }

            for (int i = 0; i < axises.Length; i++)
            {
                AxisBase axis = axises[i];
                while ((DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_check_done((ushort)(axis.Motion.Id), (ushort)(axis.AxisIndex - 1)) == 0))
                {
                    Thread.Sleep(7);
                }

                //if (!axis.WaitStop())
                //{
                //    OutputError(axis.AxisSet.AxisName.ToString() + "等待停止失败！");
                //    return false;
                //}
            }

            if (IsOnPosition(axises, poses))
            {
                result = true;
            }
            else
            {
                IsStop = true;
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = axises[0].AxisSet.AxisName.ToString() + "点位运动失败,设备运行停止,请检查点位或轴参数是否合理,轴是否过载或运动到极限位置！";
                fra.ShowDialog();
                AlarmWork();
                OutputError(axises[0].AxisSet.AxisName.ToString() + "等多轴点位运动失败！");
                result = false;
            }
            return result;
        }

        public bool CheckAxisDone(AxisBase axis)
        {
            bool flag = true;
            while ((DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.dmc_check_done((ushort)(axis.Motion.Id), (ushort)(axis.AxisIndex - 1)) == 0))
            {
                if (_IsStop)
                {
                    flag = false;
                    break;
                }
                Thread.Sleep(5);
            }
            return flag;
        }
        protected bool Wait(int time)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            if (time > 0)
            {
                while (true)
                {
                    Thread.Sleep(15);
                    if (stopwatch.ElapsedMilliseconds > time)
                    {
                        break;
                    }
                }
            }
            stopwatch.Stop();
            return true;
        }

        #endregion

        public bool SetPressure(int index, double pressure)
        {
            double v = pressure / 90.0;
            v = Math.Round(v, 2);
            if (index == 0)
            {
                v = pressure / 90.0;
                v = Math.Round(v, 2);
                DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.nmc_set_da_output(3, 1, 0, v);
            }
            else if (index == 1)
            {
                v = pressure / 90.0;
                v = Math.Round(v, 2);
                DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.nmc_set_da_output(3, 1, 1, v);
            }
            else
            {
                v = pressure / 90.0;
                v = Math.Round(v, 2);
                DY.CNC.LeadShine.LTDMC.Base.ApiInvoke.nmc_set_da_output(3, 2, 0, v);
            }
            return true;
        }

        #region SM Motion
        public string ScanCode()
        {
            str_QRCodeRev = "";
            if (!QRCodeNet.SendCommand(str_QRCodeSend))
            {
                return "NG";
            }
            else
            {
                Stopwatch stt = new Stopwatch();
                stt.Restart();
                while (str_QRCodeRev.Length < 2)
                {
                    if (stt.ElapsedMilliseconds > 4000)
                    {
                        QRCodeNet.StopConnection();
                        QRCodeNet.Dispose();
                        Thread.Sleep(200);
                        QRCodeNet.StartConnection();
                        OutputError("扫码接收信息超时报警!");
                        break;
                    }
                    if (_IsStop)
                    {
                        break;
                    }
                    Thread.Sleep(20);
                }

                if (str_QRCodeRev.Length > 2)
                {
                    return str_QRCodeRev;
                }
                else
                {
                    AlarmWork();
                    Thread.Sleep(100);
                    ClearAlarm();
                    return "NG";
                }
            }
        }

        public string RunQRCodeWork()
        {




            Thread.Sleep(100);
            string qrcode = ScanCode();
            OutputMessage(string.Format("扫码返回:{0}", qrcode.Trim()));
            try
            {
                File.AppendAllText(PathCode, DateTime.Now.ToLocalTime().ToString() + ": " + qrcode + "\r");
            }
            catch (Exception ex)
            {
                AlarmWork();
                OutputError("扫码数据保存NG,请确认是否已打开保存文档，并关闭！");
            }
            return qrcode;
        }

        public bool RunloadStation_TakeMatrial()
        {
            bool result = false;
            if (!CheckRun())
            {
                return false;
            }


            if (!FeedZWork(0, Recipe.LoadZWaitPos, true))
            {
                return false;
            }


            if (!AxisMoveTo(_Axis_Load_X, Recipe.LoadXWaitPos))
            {
                return false;
            }

            if (!Config.IsRunNull)
            {

                if (!CheckAxisDone(_Axis_Load_Y))
                {
                    return false;
                }

                if (!IsLoadBeltHaveSth)
                {
                    OnMessageShow("上料流水线没有物料");
                    return false;
                }
            }

            #region 上料气缸
            if (Config.IsFeedCylinderEnable && !_IsAutoRun)
            {
                if (FeedRotateFatch() != 0) return false;
            }
            #endregion

            if (!AxisMoveTo(_Axis_Load_X, Recipe.LoadXpos))
            {
                return false;
            }

            if (!FeedZWork(1, Recipe.LoadZpos, true))
            {
                return false;
            }



            if (!OpenIOOut(Config.LoadVacuumIOOut))
            {
                return false;
            }

            if (!OpenIOOut(Config.LoadFPCVacuumIOOut))
            {
                return false;
            }

            if (Config.IsFeedCylinderEnable)
            {
                Thread.Sleep(Config.RobotFetchDelay);
                CloseFeedRotateFPCSuck();
                CloseFeedRotateSuck();
            }
            Thread.Sleep(200);
            if (!IsLoadSTSuck)
            {
                AlarmWork();
                if (!MessageBoxEx.ShowSystemQuestion("上料平台真空吸报警，是否继续运行!"))
                {
                    ClearAlarm();
                    FeedZWork(0, Recipe.LoadZWaitPos, true);
                    return false;
                }
                Thread.Sleep(100);
                ClearAlarm();
            }

            if (!IsLoadFPCSuck)
            {
                AlarmWork();
                if (!MessageBoxEx.ShowSystemQuestion("上料平台FPC真空吸报警，是否继续运行!"))
                {
                    ClearAlarm();
                    FeedZWork(0, Recipe.LoadZWaitPos, true);
                    return false;
                }
                Thread.Sleep(100);
                ClearAlarm();
            }

            if (!FeedZWork(0, Recipe.LoadZWaitPos, true))
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public bool RunloadStation_PullMatrial(int flag) //
        {
            bool ret = false;

            MeasurementAxis[] axises = null;
            MeasurementAxis[] dischargaxises = null;
            if (!CheckRun())
            {
                return false;
            }

            if (flag == 0)
            {
                axises = new MeasurementAxis[]
                {
                    _Axis_Load_X,
                    _Axis_LeftSM_Y
                   // _Axis_Load_Z
                };



            }
            else if (flag == 1)
            {
                axises = new MeasurementAxis[]
                {
                    _Axis_Load_X,
                    _Axis_MidSM_Y
                   // _Axis_Load_Z
                };

            }
            else
            {
                axises = new MeasurementAxis[]
                {
                    _Axis_Load_X,
                    _Axis_RightSM_Y
                    //_Axis_Load_Z
               };

                dischargaxises = new MeasurementAxis[]
                    {
                        _Axis_Discharge_X,
                        _Axis_Discharge_Z
                     };

            }

            if (dischargaxises != null)
            {

                if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                {
                    _Axis_Discharge_Z.StopSlowly();
                    return false;
                }

                if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeX_OK_PullPos))
                {
                    _Axis_Discharge_X.StopSlowly();
                    return false;
                }
            }


            double[] poses = new double[] { Recipe.SMPosition[flag].Lsm_loadX, Recipe.SMPosition[flag].Lsm_loadY };

            if (!AxisMoveTo(axises, poses))
            {
                return false;
            }

            Thread.Sleep(10);


            if (!FeedZWork(1, Recipe.SMPosition[flag].Lsm_LoadZ, true))
            {
                return false;
            }




            Thread.Sleep(50);


            if (flag == 0)
            {
                if (!OpenIOOut(Config.LeftSM_StgVacuum_IOOut))
                {
                    return false;
                }





            }
            else if (flag == 2)
            {
                if (!OpenIOOutEx(Config.RightSM_StgVacuum_IOOutEx))
                {
                    return false;
                }


                //if (!GetIOInStatus(Config.RightSM_UD_CylinderDownIOIn))
                //{
                //    return false;
                //}

                //if (!GetIOInStatus(Config.RightSMVacuumIOIn))
                //{
                //    return false;
                //}
            }
            else if (flag == 1)
            {
                if (!OpenIOOut(Config.MidSM_StgVacuum_IOOut))
                {
                    return false;
                }
            }

            if (!CloseIOOut(Config.LoadVacuumIOOut))
            {
                return false;
            }

            if (!CloseIOOut(Config.LoadFPCVacuumIOOut))
            {
                return false;
            }
            OpenLoadSTBlow();
            Thread.Sleep(100);
            CloseLoadSTBlow();


            if (!FeedZWork(0, Recipe.LoadZWaitPos, true))
            {
                return false;
            }


            if (!AxisMoveTo(_Axis_Load_X, Recipe.LoadXWaitPos))
            {
                _Axis_Load_X.StopSlowly();
                return false;
            }
            ret = true;
            return ret;
        }

        public bool RunSMStation_SMMotion(int flag)
        {
            bool ret = false;
            if (!CheckRun())
            {
                return false;
            }
            if (flag == 0)
            {
                if (Config.IsTearFilmCloseVacCalib)//临时关闭平台真空 部分产品对位时开真空会对刮花产品
                {
                    CloseLeftSMSuck();
                    CloseLeftSMFPCSuck();
                }
                else
                {
                    OpenLeftSMSuck();
                    OpenLeftSMFPCSuck();
                    OpenLeftSMReduce();
                }
                Thread.Sleep(200);

                if (!OpenIOOut(Config.LeftSM_FBCylinder_IOOut))
                {
                    return false;
                }

                if (!OpenIOOut(Config.LeftSM_LRCylinder_IOOut))
                {
                    return false;
                }
                Thread.Sleep(200);
                if (!WaitIOMSec(Config.LeftSM_FB_CylinderFrontIOIn, 400, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "左撕膜前后气缸前感应位报警！";
                    fra.ShowDialog();
                }

                if (!WaitIOMSec(Config.LeftSM_LR_CylinderLeftIOIn, 400, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "左撕膜左右气缸左感应位报警！";
                    fra.ShowDialog();
                }

                if (Config.IsTearFilmCloseVacCalib)
                {
                    OpenLeftSMSuck();
                    OpenLeftSMFPCSuck();
                }
                else
                {
                    CloseLeftSMReduce();
                }
                CloseLeftSMFBCylinder();
                CloseLeftSMLRCylinder();
                if (!LeftSMLoop())
                {
                    OutputError("左撕膜失败", true);
                    ErrSMLoop(StationType.Left);
                }
            }
            else if (flag == 1)
            {
                if (Config.IsTearFilmCloseVacCalib)//临时关闭平台真空 部分产品对位时开真空会对刮花产品
                {
                    CloseMidSMSuck();
                    CloseMidSMFPCSuck();
                }
                else
                {
                    OpenMidSMSuck();
                    OpenMidSMFPCSuck();
                    OpenMidSMReduce();
                }
                Thread.Sleep(50);
                if (!OpenIOOutEx(Config.MidSM_FBCylinder_IOOutEx))
                {
                    return false;
                }
                Thread.Sleep(20);
                if (!OpenIOOutEx(Config.MidSM_LRCylinder_IOOutEx))
                {
                    return false;
                }


                Thread.Sleep(200);
                if (!WaitIOExMSec(Config.MidSM_FB_CylinderFrontIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "中撕膜前后气缸前感应位报警！";
                    fra.ShowDialog();
                }

                if (!WaitIOExMSec(Config.MidSM_LR_CylinerLeftIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "中撕膜左右气缸左感应位报警！";
                    fra.ShowDialog();
                }
                Thread.Sleep(50);
                if (Config.IsTearFilmCloseVacCalib)
                {
                    OpenMidSMSuck();
                    OpenMidSMFPCSuck();
                }
                else
                {
                    CloseMidSMReduce();
                }

                CloseMidSMLRCylinder();
                CloseMidSMFBCylinder();


                if (!MidSMLoop())
                {
                    OutputError("中撕膜失败!", true);
                    ErrSMLoop(StationType.Mid);
                }
            }
            else
            {
                if (Config.IsTearFilmCloseVacCalib)//临时关闭平台真空 部分产品对位时开真空会对刮花产品
                {
                    CloseRightSMSuck();
                    CloseRightSMFPCSuck();
                }
                else
                {
                    OpenRightSMSuck();
                    OpenRightSMFPCSuck();
                    OpenRightSMReduce();
                }
                Thread.Sleep(50);

                if (!OpenIOOutEx(Config.RightSM_FBCylinder_IOOutEx))
                {
                    return false;
                }

                if (!OpenIOOutEx(Config.RightSM_LRCylinder_IOOutEx))
                {
                    return false;
                }


                Thread.Sleep(200);
                if (!WaitIOMSec(Config.RightSM_FB_CylinderFrontIOIn, 400, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "右撕膜前后气缸前感应位报警！";
                    fra.ShowDialog();
                }

                if (!WaitIOMSec(Config.RightSM_LR_CylinerLeftIOIn, 400, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "右撕膜左右气缸左感应位报警！";
                    fra.ShowDialog();
                }

                if (Config.IsTearFilmCloseVacCalib)
                {
                    OpenRightSMSuck();
                    OpenRightSMFPCSuck();
                }
                else
                {
                    CloseRightSMReduce();
                }
                CloseRightSMFBCylinder();
                CloseRightSMLRCylinder();


                if (!RightSMLoop())
                {
                    OutputError("右撕膜失败!", true);
                    ErrSMLoop(StationType.Right);
                }
            }
            return ret;
        }

        public bool RunSMStation_SMAOIMotion(int flag)
        {
            bool ret = true;
            MeasurementAxis[] axises = null;
            if (!CheckRun())
            {
                return false;
            }

            double[] poses;
            lock (Obj_CCDlock)
            {
                if (flag == 0)
                {
                    LeftSocketReciveSMRecheckItems.Clear();
                    axises = new MeasurementAxis[] { _Axis_LeftSM_Y, _Axis_SMCCD_X };
                    poses = new double[] { Recipe.SMPosition[0].Lsm_CCDY, Recipe.SMPosition[0].Lsm_CCDX };
                    OpenSMlightController();
                    if (Config.TearAOI_Blow_Enable)
                    {
                        CloseLeftSMUDCylinder();
                        Thread.Sleep(100);
                    }
                    if (!AxisMoveTo(axises, poses))
                    {
                        return false;
                    }
                    if (Config.TearAOI_Blow_Enable)
                    {
                        OpenTearFPCBlow();
                        OpenTear1FPCSuck();
                    }
                    str_leftsm = " ";
                    if (Config.IsRunNull)
                    {
                        str_leftsm = "OK";
                        SMAOIReslut[0] = 1;
                        return true;
                    }
                    Thread.Sleep(50);
                    str_TearRev = "";
                    if (!TearSendMsg("S1,PZ"))
                    {
                        OutputError("撕膜相机发送数据S1,PZ失败!");
                        str_leftsm = "NG";
                    }
                    else
                    {

                        Stopwatch stt = new Stopwatch();
                        stt.Restart();
                        while (str_TearRev.Length < 2)
                        {
                            if (stt.ElapsedMilliseconds > NetTimeOut)
                            {
                                TearCCDNet.StopConnection();
                                TearCCDNet.Dispose();
                                Thread.Sleep(200);
                                TearCCDNet.StartConnection();
                                OutputError("左撕膜相机接收信息超时报警!");
                                break;
                            }
                            if (_IsStop)
                            {
                                break;
                            }
                            Thread.Sleep(20);
                        }
                    }
                    if (str_TearRev.Length > 2)
                    {

                        str_leftsm = SplitTearString(str_TearRev);
                    }
                    else
                    {
                        AlarmWork();
                        Thread.Sleep(100);
                        str_leftsm = "NG";
                        ClearAlarm();
                    }

                    if (str_leftsm == "OK")
                    {
                        MeasurementContext.Capacity.AddPre(4, 1);
                        SMAOIReslut[0] = 1;
                    }
                    else if (str_leftsm == "NG")
                    {
                        SMAOIReslut[0] = 0;
                        MeasurementContext.Capacity.AddPre(3, 1);
                        DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "左撕膜", 0, 0, "NG", "左撕膜NG", 0);
                        SaveRecord(item);
                    }
                    else
                    {
                        SMAOIReslut[0] = 3;
                    }

                    str_leftsm = " ";
                    CloseLeftSMFPCSuck();
                    CloseSMlightController();
                }
                else if (flag == 1)
                {
                    MidSocketReciveSMRecheckItems.Clear();

                    axises = new MeasurementAxis[] { _Axis_MidSM_Y, _Axis_SMCCD_X };
                    poses = new double[] { Recipe.SMPosition[1].Lsm_CCDY, Recipe.SMPosition[1].Lsm_CCDX };
                    OpenSMlightController();

                    if (Config.TearAOI_Blow_Enable)
                    {
                        CloseMidSMUDCylinder();
                        Thread.Sleep(100);

                    }

                    if (!AxisMoveTo(axises, poses))
                    {
                        return false;
                    }
                    if (Config.TearAOI_Blow_Enable)
                    {
                        OpenTearFPCBlow();
                        OpenTear2FPCSuck();
                    }
                    str_midsm = "";
                    if (Config.IsRunNull)
                    {
                        str_midsm = "OK";
                        SMAOIReslut[1] = 1;
                        return true;
                    }
                    Thread.Sleep(50);

                    str_TearRev = "";
                    if (!TearSendMsg("S2,PZ"))
                    {
                        OutputError("撕膜相机发送数据S2,PZ失败!");
                        str_midsm = "NG";
                    }
                    else
                    {

                        Stopwatch stt = new Stopwatch();
                        stt.Restart();
                        while (str_TearRev.Length < 2)
                        {
                            if (stt.ElapsedMilliseconds > NetTimeOut)
                            {
                                TearCCDNet.StopConnection();
                                TearCCDNet.Dispose();
                                Thread.Sleep(200);
                                TearCCDNet.StartConnection();
                                OutputError("中撕膜相机接收信息超时报警!");
                                break;
                            }
                            if (_IsStop)
                            {
                                break;
                            }
                            Thread.Sleep(20);
                        }

                        if (str_TearRev.Length > 2)
                        {
                            str_midsm = SplitTearString(str_TearRev);
                        }
                        else
                        {
                            AlarmWork();
                            Thread.Sleep(100);
                            str_midsm = "NG";
                            ClearAlarm();
                        }
                    }

                    if (str_midsm == "OK")
                    {
                        SMAOIReslut[1] = 1;
                        MeasurementContext.Capacity.AddPre(4, 2);
                    }
                    else if (str_midsm == "NG")
                    {
                        // QueueTearResult.Enqueue(0);
                        SMAOIReslut[1] = 0;
                        MeasurementContext.Capacity.AddPre(3, 2);
                        DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "中撕膜", 0, 0, "NG", "中撕膜NG", 0);
                        SaveRecord(item);
                    }
                    else
                    {
                        SMAOIReslut[1] = 3;
                    }

                    str_midsm = "";
                    CloseMidSMFPCSuck();
                    CloseSMlightController();
                }
                else
                {
                    RightSocketReciveSMRecheckItems.Clear();
                    axises = new MeasurementAxis[] { _Axis_RightSM_Y, _Axis_SMCCD_X };
                    poses = new double[] { Recipe.SMPosition[2].Lsm_CCDY, Recipe.SMPosition[2].Lsm_CCDX };
                    OpenSMlightController();
                    if (Config.TearAOI_Blow_Enable)
                    {
                        CloseRightSMUDCylinder();
                        Thread.Sleep(100);
                    }

                    if (!AxisMoveTo(axises, poses))
                    {
                        return false;
                    }
                    str_rightsm = "";
                    if (Config.TearAOI_Blow_Enable)
                    {
                        OpenTearFPCBlow();
                        OpenTear3FPCSuck();
                    }
                    if (Config.IsRunNull)
                    {
                        str_rightsm = "OK";
                        SMAOIReslut[2] = 1;
                        return true;
                    }

                    Thread.Sleep(50);
                    str_TearRev = "";
                    if (!TearSendMsg("S3,PZ"))
                    {
                        OutputError("撕膜相机发送数据S3,PZ失败!");
                        str_midsm = "NG";
                    }
                    else
                    {
                        Stopwatch stt = new Stopwatch();
                        stt.Restart();
                        while (str_TearRev.Length < 2)
                        {
                            if (stt.ElapsedMilliseconds > NetTimeOut)
                            {
                                TearCCDNet.StopConnection();
                                TearCCDNet.Dispose();
                                Thread.Sleep(200);
                                TearCCDNet.StartConnection();
                                OutputError("右撕膜相机接收信息超时报警!");
                                break;
                            }
                            if (_IsStop)
                            {
                                break;
                            }
                            Thread.Sleep(20);
                        }

                        if (str_TearRev.Length > 2)
                        {
                            str_rightsm = SplitTearString(str_TearRev);
                        }
                        else
                        {
                            AlarmWork();
                            Thread.Sleep(100);
                            str_rightsm = "NG";
                            ClearAlarm();
                        }
                    }

                    if (str_rightsm == "OK")
                    {

                        SMAOIReslut[2] = 1;
                        MeasurementContext.Capacity.AddPre(4, 3);
                    }
                    else if (str_rightsm == "NG")
                    {
                        SMAOIReslut[2] = 0;
                        MeasurementContext.Capacity.AddPre(3, 3);
                        DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "右撕膜", 0, 0, "NG", "右撕膜NG", 0);
                        SaveRecord(item);
                    }
                    else
                    {
                        SMAOIReslut[2] = 3;
                    }
                    str_rightsm = "";
                    CloseRightSMFPCSuck();
                    CloseSMlightController();
                }
            }
            CloseTearFPCBlow();

            SMAOIReturnValue(flag);
            return ret;
        }







        #region 2020619
        /// <summary>
        /// 撕膜检测并输出当前产品是否需要重新撕膜
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="IsResm">是否需要重新撕膜</param>
        /// <returns></returns>
        public bool RunSMStation_SMAOIMotion(int flag, out bool IsResm)
        {
            lock (Obj_CCDlock)
            {
                bool ret = true;
                MeasurementAxis[] axises = null;

                double[] poses;
                IsResm = false;

                if (flag == 0)
                {

                    LeftSocketReciveSMRecheckItems.Clear();
                    axises = new MeasurementAxis[] { _Axis_LeftSM_Y, _Axis_SMCCD_X };
                    poses = new double[] { Recipe.SMPosition[0].Lsm_CCDY, Recipe.SMPosition[0].Lsm_CCDX };
                    OpenSMlightController();
                    if (Config.TearAOI_Blow_Enable)
                    {
                        CloseLeftSMUDCylinder();
                        Thread.Sleep(100);
                    }
                    if (!AxisMoveTo(axises, poses))
                    {

                        return false;
                    }
                    if (Config.TearAOI_Blow_Enable)
                    {
                        OpenTearFPCBlow();
                        OpenTear1FPCSuck();
                    }
                    str_leftsm = " ";
                    if (Config.IsRunNull)
                    {
                        str_leftsm = "OK";
                        SMAOIReslut[0] = 1;
                        return true;
                    }
                    Thread.Sleep(50);
                    str_TearRev = "";
                    if (!TearSendMsg("S1,PZ"))
                    {
                        OutputError("撕膜相机发送数据S1,PZ失败!");
                        str_leftsm = "NG";
                    }
                    else
                    {

                        Stopwatch stt = new Stopwatch();
                        stt.Restart();
                        while (str_TearRev.Length < 2)
                        {
                            if (stt.ElapsedMilliseconds > NetTimeOut)
                            {
                                TearCCDNet.StopConnection();
                                TearCCDNet.Dispose();
                                Thread.Sleep(200);
                                TearCCDNet.StartConnection();
                                OutputError("左撕膜相机接收信息超时报警!");
                                break;
                            }
                            if (_IsStop)
                            {
                                break;
                            }
                            Thread.Sleep(20);
                        }
                    }
                    if (str_TearRev.Length > 2)
                    {

                        str_leftsm = SplitTearString(str_TearRev, ref LeftSocketReciveSMRecheckItems);
                    }
                    else
                    {
                        AlarmWork();
                        Thread.Sleep(100);
                        str_leftsm = "NG";
                        ClearAlarm();
                    }

                    if (str_leftsm == "OK")
                    {
                        MeasurementContext.Capacity.AddPre(4, 1);
                        SMAOIReslut[0] = 1;

                    }
                    else if (str_leftsm == "NG" && string.IsNullOrEmpty(LeftSocketReciveSMRecheckItems.ToString()))//Items为空的NG 表示没找模板NG 
                    {
                        SMAOIReslut[0] = 0;
                        MeasurementContext.Capacity.AddPre(3, 1);
                        DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "左撕膜", 0, 0, "NG", "左撕膜NG", 0);
                        SaveRecord(item);
                    }
                    else if (str_leftsm == "NG" && !string.IsNullOrEmpty(LeftSocketReciveSMRecheckItems.ToString()))//Items不为空 表示撕膜NG并有NG编号 
                    {
                        if (!MeasurementContext.Config.TearRecheckEnabled || (MeasurementContext.Config.TearRecheckEnabled && LeftSMReckeckCurrentCount >= MeasurementContext.Config.TearRecheckCountParam))//重复撕膜到达次数
                        {
                            SMAOIReslut[0] = 0;
                            MeasurementContext.Capacity.AddPre(3, 1);
                            DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "左撕膜", 0, 0, "NG", "左撕膜NG", 0);
                            SaveRecord(item);

                            LeftSMReckeckCurrentCount = 0;
                            IsResm = false;
                        }
                        else if (MeasurementContext.Config.TearRecheckEnabled && LeftSMReckeckCurrentCount < MeasurementContext.Config.TearRecheckCountParam)//开始重新撕膜
                        {
                            LeftSMReckeckCurrentCount++;
                            IsResm = true;
                        }
                    }

                    else
                    {
                        SMAOIReslut[0] = 3;
                    }

                    str_leftsm = " ";
                    CloseLeftSMFPCSuck();
                    CloseSMlightController();
                }
                else if (flag == 1)
                {
                    axises = new MeasurementAxis[] { _Axis_MidSM_Y, _Axis_SMCCD_X };
                    poses = new double[] { Recipe.SMPosition[1].Lsm_CCDY, Recipe.SMPosition[1].Lsm_CCDX };
                    OpenSMlightController();

                    if (Config.TearAOI_Blow_Enable)
                    {
                        CloseMidSMUDCylinder();
                        Thread.Sleep(100);

                    }

                    if (!AxisMoveTo(axises, poses))
                    {
                        return false;
                    }
                    if (Config.TearAOI_Blow_Enable)
                    {
                        OpenTearFPCBlow();
                        OpenTear2FPCSuck();
                    }
                    str_midsm = "";
                    if (Config.IsRunNull)
                    {
                        str_midsm = "OK";
                        SMAOIReslut[1] = 1;
                        return true;
                    }
                    Thread.Sleep(50);

                    str_TearRev = "";
                    if (!TearSendMsg("S2,PZ"))
                    {
                        OutputError("撕膜相机发送数据S2,PZ失败!");
                        str_midsm = "NG";
                    }
                    else
                    {

                        Stopwatch stt = new Stopwatch();
                        stt.Restart();
                        while (str_TearRev.Length < 2)
                        {
                            if (stt.ElapsedMilliseconds > NetTimeOut)
                            {
                                TearCCDNet.StopConnection();
                                TearCCDNet.Dispose();
                                Thread.Sleep(200);
                                TearCCDNet.StartConnection();
                                OutputError("中撕膜相机接收信息超时报警!");
                                break;
                            }
                            if (_IsStop)
                            {
                                break;
                            }
                            Thread.Sleep(20);
                        }

                        if (str_TearRev.Length > 2)
                        {
                            str_midsm = SplitTearString(str_TearRev, ref MidSocketReciveSMRecheckItems);
                        }
                        else
                        {
                            AlarmWork();
                            Thread.Sleep(100);
                            str_midsm = "NG";
                            ClearAlarm();
                        }
                    }

                    if (str_midsm == "OK")
                    {
                        SMAOIReslut[1] = 1;
                        MeasurementContext.Capacity.AddPre(4, 2);

                    }
                    else if (str_midsm == "NG" && string.IsNullOrEmpty(MidSocketReciveSMRecheckItems.ToString()))
                    {
                        // QueueTearResult.Enqueue(0);
                        SMAOIReslut[1] = 0;
                        MeasurementContext.Capacity.AddPre(3, 2);
                        DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "中撕膜", 0, 0, "NG", "中撕膜NG", 0);
                        SaveRecord(item);
                    }
                    else if (str_midsm == "NG" && !string.IsNullOrEmpty(MidSocketReciveSMRecheckItems.ToString()))
                    {
                        if (!MeasurementContext.Config.TearRecheckEnabled || (MeasurementContext.Config.TearRecheckEnabled && MidSMReckeckCurrentCount >= MeasurementContext.Config.TearRecheckCountParam))//重复撕膜超次数
                        {
                            SMAOIReslut[1] = 0;
                            MeasurementContext.Capacity.AddPre(3, 2);
                            DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "中撕膜", 0, 0, "NG", "中撕膜NG", 0);
                            SaveRecord(item);

                            IsResm = false;
                            MidSMReckeckCurrentCount = 0;
                        }
                        else if (MeasurementContext.Config.TearRecheckEnabled && MidSMReckeckCurrentCount < MeasurementContext.Config.TearRecheckCountParam)
                        {
                            MidSMReckeckCurrentCount++;
                            IsResm = true;
                        }

                    }
                    else
                    {
                        SMAOIReslut[1] = 3;
                    }

                    str_midsm = "";
                    CloseMidSMFPCSuck();
                    CloseSMlightController();
                }
                else
                {
                    axises = new MeasurementAxis[] { _Axis_RightSM_Y, _Axis_SMCCD_X };
                    poses = new double[] { Recipe.SMPosition[2].Lsm_CCDY, Recipe.SMPosition[2].Lsm_CCDX };
                    OpenSMlightController();
                    if (Config.TearAOI_Blow_Enable)
                    {
                        CloseRightSMUDCylinder();
                        Thread.Sleep(100);

                    }

                    if (!AxisMoveTo(axises, poses))
                    {
                        return false;
                    }
                    str_rightsm = "";
                    if (Config.TearAOI_Blow_Enable)
                    {
                        OpenTearFPCBlow();
                        OpenTear3FPCSuck();
                    }
                    if (Config.IsRunNull)
                    {
                        str_rightsm = "OK";
                        SMAOIReslut[2] = 1;
                        return true;
                    }

                    Thread.Sleep(50);
                    str_TearRev = "";
                    if (!TearSendMsg("S3,PZ"))
                    {
                        OutputError("撕膜相机发送数据S3,PZ失败!");
                        str_midsm = "NG";
                    }
                    else
                    {
                        Stopwatch stt = new Stopwatch();
                        stt.Restart();
                        while (str_TearRev.Length < 2)
                        {
                            if (stt.ElapsedMilliseconds > NetTimeOut)
                            {
                                TearCCDNet.StopConnection();
                                TearCCDNet.Dispose();
                                Thread.Sleep(200);
                                TearCCDNet.StartConnection();
                                OutputError("右撕膜相机接收信息超时报警!");
                                break;
                            }
                            if (_IsStop)
                            {
                                break;
                            }
                            Thread.Sleep(20);
                        }

                        if (str_TearRev.Length > 2)
                        {
                            str_rightsm = SplitTearString(str_TearRev, ref RightSocketReciveSMRecheckItems);
                        }
                        else
                        {
                            AlarmWork();
                            Thread.Sleep(100);
                            str_rightsm = "NG";
                            ClearAlarm();
                        }
                    }

                    if (str_rightsm == "OK")
                    {

                        SMAOIReslut[2] = 1;
                        MeasurementContext.Capacity.AddPre(4, 3);

                    }
                    else if (str_rightsm == "NG" && string.IsNullOrEmpty(RightSocketReciveSMRecheckItems.ToString()))
                    {
                        SMAOIReslut[2] = 0;
                        MeasurementContext.Capacity.AddPre(3, 3);
                        DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "右撕膜", 0, 0, "NG", "右撕膜NG", 0);
                        SaveRecord(item);
                    }
                    else if (str_rightsm == "NG" && !string.IsNullOrEmpty(RightSocketReciveSMRecheckItems.ToString()))
                    {
                        if (!MeasurementContext.Config.TearRecheckEnabled || (MeasurementContext.Config.TearRecheckEnabled && RightSMReckeckCurrentCount >= MeasurementContext.Config.TearRecheckCountParam))//不允许再重复撕膜
                        {
                            SMAOIReslut[2] = 0;
                            MeasurementContext.Capacity.AddPre(3, 3);
                            DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "右撕膜", 0, 0, "NG", "右撕膜NG", 0);
                            SaveRecord(item);
                            RightSMReckeckCurrentCount = 0;
                        }
                        else if (MeasurementContext.Config.TearRecheckEnabled && RightSMReckeckCurrentCount < MeasurementContext.Config.TearRecheckCountParam)
                        {
                            IsResm = true;
                            RightSMReckeckCurrentCount++;
                        }
                    }
                    else
                    {
                        SMAOIReslut[2] = 3;
                    }
                    str_rightsm = "";
                    CloseRightSMFPCSuck();
                    CloseSMlightController();
                }

                CloseTearFPCBlow();

                SMAOIReturnValue(flag);
                return ret;
            }
        }
        #endregion 


        public bool RunSMStation_GetOutoffStg(int flag)
        {
            bool ret = false;
            MeasurementAxis[] axises = null;
            double[] poses;
            if (!CheckRun())
            {
                return false;
            }

            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                return false;
            }


            if (flag == 0)
            {
                axises = new MeasurementAxis[] { _Axis_Transfer_X, _Axis_LeftSM_Y };
                poses = new double[] { Recipe.SMPosition[0].Lsm_DischargeX, Recipe.SMPosition[0].Lsm_DischargeY };
                if (!AxisMoveTo(axises, poses))
                {
                    return false;
                }
                CloseLeftSMUDCylinder();
                Thread.Sleep(2000);

                if (!TransferZWork(1, Recipe.SMPosition[0].Lsm_DischargeZ, true))
                {
                    return false;
                }

                if (!OpenIOOut(Config.Transfer_Suckvacuum_IOOut))
                {
                    return false;
                }

                if (!OpenIOOut(Config.Transfer_FPCSuckvacuum_IOOut))
                {
                    return false;
                }

                if (!CloseIOOut(Config.LeftSM_StgVacuum_IOOut))
                {
                    return false;
                }

                if (!CloseIOOut(Config.LeftSM_StgFPCVacuum_IOOut))
                {
                    return false;
                }

                OpenLeftSMBlow();

                Thread.Sleep(Config.StageBlowDelay);


                if (!TransferZWork(0, Recipe.TransferZSafePos, true))
                {
                    return false;
                }

                CloseLeftSMBlow();

            }
            else if (flag == 1)
            {
                axises = new MeasurementAxis[] { _Axis_Transfer_X, _Axis_MidSM_Y };
                poses = new double[] { Recipe.SMPosition[1].Lsm_DischargeX, Recipe.SMPosition[1].Lsm_DischargeY };
                if (!AxisMoveTo(axises, poses))
                {
                    return false;
                }

                CloseMidSMUDCylinder();
                Thread.Sleep(2000);

                if (!TransferZWork(1, Recipe.SMPosition[1].Lsm_DischargeZ, true))
                {
                    return false;
                }

                if (!OpenIOOut(Config.Transfer_Suckvacuum_IOOut))
                {
                    return false;
                }

                if (!OpenIOOut(Config.Transfer_FPCSuckvacuum_IOOut))
                {
                    return false;
                }

                if (!CloseIOOut(Config.MidSM_StgVacuum_IOOut))
                {
                    return false;
                }

                if (!CloseIOOut(Config.MidSM_StgFPCVacuum_IOOut))
                {
                    return false;
                }

                if (!OpenIOOutEx(Config.MidSM_StgBlowVacuum_IOOutEx))
                {
                    return false;
                }

                Thread.Sleep(Config.StageBlowDelay);

                if (!TransferZWork(0, Recipe.TransferZSafePos, true))
                {
                    return false;
                }

                if (!CloseIOOutEx(Config.MidSM_StgBlowVacuum_IOOutEx))
                {
                    return false;
                }
            }
            else
            {
                axises = new MeasurementAxis[] { _Axis_Transfer_X, _Axis_RightSM_Y };
                poses = new double[] { Recipe.SMPosition[2].Lsm_DischargeX, Recipe.SMPosition[2].Lsm_DischargeY };
                if (!AxisMoveTo(axises, poses))
                {
                    return false;
                }

                CloseRightSMUDCylinder();
                Thread.Sleep(2000);

                if (!TransferZWork(1, Recipe.SMPosition[2].Lsm_DischargeZ, true))
                {
                    return false;
                }

                if (!OpenIOOut(Config.Transfer_Suckvacuum_IOOut))
                {
                    return false;
                }

                if (!OpenIOOut(Config.Transfer_FPCSuckvacuum_IOOut))
                {
                    return false;
                }

                if (!CloseIOOutEx(Config.RightSM_StgVacuum_IOOutEx))
                {
                    return false;
                }

                if (!CloseIOOutEx(Config.RightSM_StgFPCVacuum_IOOutEx))
                {
                    return false;
                }

                if (!OpenIOOutEx(Config.RightSM_StgBlowVacuum_IOOutEx))
                {
                    return false;
                }

                Thread.Sleep(Config.StageBlowDelay);
                if (!TransferZWork(0, Recipe.TransferZSafePos, true))
                {
                    return false;
                }

                if (!CloseIOOutEx(Config.RightSM_StgBlowVacuum_IOOutEx))
                {
                    return false;
                }
            }
            ret = true;
            return ret;
        }

        public bool RunSMStaton_GoOutPos(StationType station)
        {

            MeasurementAxis[] Axises;
            double[] poses = new double[] { 0 };
            MeasurementAxis axisY;
            double posY;

            if (!CheckRun())
            {
                return false;
            }
            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                return false;
            }

            switch (station)
            {
                case StationType.Left:
                    Axises = new MeasurementAxis[] { Axis_LeftSM_Z };
                    axisY = Axis_LeftSM_Y;
                    posY = Recipe.SMPosition[0].Lsm_DischargeY;
                    break;
                case StationType.Mid:
                    Axises = new MeasurementAxis[] { Axis_MidSM_Z };
                    axisY = Axis_MidSM_Y;
                    posY = Recipe.SMPosition[1].Lsm_DischargeY;
                    break;
                case StationType.Right:
                    Axises = new MeasurementAxis[] { Axis_RightSM_Z };
                    axisY = Axis_RightSM_Y;
                    posY = Recipe.SMPosition[2].Lsm_DischargeY;
                    break;
                default:
                    return false;

            }

            if (!AxisMoveTo(Axises, poses) || !AxisMoveTo(axisY, posY))
            {
                return false;
            }
            return true;
        }



        public bool RunSMStaion_CylinderWork(int flag)
        {
            bool result = true;
            if (flag == 0)
            {

                if (Config.IsTearFilmCloseVacCalib)//临时关闭平台真空 部分产品对位时开真空会对刮花产品
                {
                    CloseLeftSMSuck();
                    CloseLeftSMFPCSuck();
                }
                else
                {
                    OpenLeftSMSuck();
                    OpenLeftSMFPCSuck();
                    OpenLeftSMReduce();
                }


                Thread.Sleep(200);
                if (!OpenIOOut(Config.LeftSM_FBCylinder_IOOut))
                {
                    return false;
                }
                Thread.Sleep(20);

                if (!OpenIOOut(Config.LeftSM_LRCylinder_IOOut))
                {
                    return false;
                }
                Thread.Sleep(200);
                if (!WaitIOMSec(Config.LeftSM_FB_CylinderFrontIOIn, 400, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "左撕膜前后气缸前感应位报警！";
                    fra.ShowDialog();
                }

                if (!WaitIOMSec(Config.LeftSM_LR_CylinderLeftIOIn, 400, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "左撕膜左右气缸左感应位报警！";
                    fra.ShowDialog();
                }
                Thread.Sleep(100);
                Thread.Sleep(Config.LeftSMCylinderAligningDelay);//ZGH20220913增加撕膜气缸定位延时
                if (Config.IsTearFilmCloseVacCalib)
                {
                    OpenLeftSMSuck();
                    OpenLeftSMFPCSuck();
                }
                else
                {
                    CloseLeftSMReduce();
                }

                CloseLeftSMFBCylinder();
                CloseLeftSMLRCylinder();
            }
            else if (flag == 1)
            {
                if (Config.IsTearFilmCloseVacCalib)//临时关闭平台真空 部分产品对位时开真空会对刮花产品
                {
                    CloseMidSMSuck();
                    CloseMidSMFPCSuck();
                }
                else
                {
                    OpenMidSMSuck();
                    OpenMidSMFPCSuck();
                    OpenMidSMReduce();
                }

                Thread.Sleep(100);

                if (!OpenIOOutEx(Config.MidSM_FBCylinder_IOOutEx))
                {
                    return false;
                }
                Thread.Sleep(20);
                if (!OpenIOOutEx(Config.MidSM_LRCylinder_IOOutEx))
                {
                    return false;
                }


                Thread.Sleep(200);
                if (!WaitIOExMSec(Config.MidSM_FB_CylinderFrontIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "中撕膜前后气缸前感应位报警！";
                    fra.ShowDialog();
                }

                if (!WaitIOExMSec(Config.MidSM_LR_CylinerLeftIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "中撕膜左右气缸左感应位报警！";
                    fra.ShowDialog();
                }
                Thread.Sleep(50);
                Thread.Sleep(Config.MidSMCylinderAligningDelay);//ZGH20220913增加撕膜气缸定位延时
                if (Config.IsTearFilmCloseVacCalib)
                {
                    OpenMidSMSuck();
                    OpenMidSMFPCSuck();
                }
                else
                {
                    CloseMidSMReduce();
                }
                CloseMidSMLRCylinder();
                CloseMidSMFBCylinder();
            }
            else
            {
                if (Config.IsTearFilmCloseVacCalib)//临时关闭平台真空 部分产品对位时开真空会对刮花产品
                {
                    CloseRightSMSuck();
                    CloseRightSMFPCSuck();
                }
                else
                {
                    OpenRightSMSuck();
                    OpenRightSMFPCSuck();
                    OpenRightSMReduce();
                }
                Thread.Sleep(100);

                if (!OpenIOOutEx(Config.RightSM_FBCylinder_IOOutEx))
                {
                    return false;
                }
                Thread.Sleep(20);
                if (!OpenIOOutEx(Config.RightSM_LRCylinder_IOOutEx))
                {
                    return false;
                }

                Thread.Sleep(200);
                if (!WaitIOMSec(Config.RightSM_FB_CylinderFrontIOIn, 400, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "右撕膜前后气缸前感应位报警！";
                    fra.ShowDialog();
                }

                if (!WaitIOMSec(Config.RightSM_LR_CylinerLeftIOIn, 400, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "右撕膜左右气缸左感应位报警！";
                    fra.ShowDialog();
                }
                Thread.Sleep(50);
                Thread.Sleep(Config.RightSMCylinderAligningDelay);//ZGH20220913增加撕膜气缸定位延时
                if (Config.IsTearFilmCloseVacCalib)
                {
                    OpenRightSMSuck();
                    OpenRightSMFPCSuck();
                }
                else
                {
                    CloseRightSMReduce();
                }
                CloseRightSMFBCylinder();
                CloseRightSMLRCylinder();
            }

            return result;
        }



        private int SMAOIReturnValue(int flag)
        {
            return SMAOIReslut[flag];
        }

        private bool LeftSMLoop()
        {
            bool ret = false;
            _IsStop = false;
            MeasurementAxis[] axise = new MeasurementAxis[] { _Axis_LeftSM_X, _Axis_LeftSM_Y, _Axis_LeftSM_Z };
            double[] startposes;
            double[] endposes;
            double[] smspeed = new double[] { Recipe.SMPosition[0].SM_XSpeed, Recipe.SMPosition[0].SM_YSpeed, Recipe.SMPosition[0].SM_ZSpeed };
            double paste_Speed = Recipe.SMPosition[0].Paste_Speed;
            double[] posxyz = new double[] { 0, 0, 0 };
            for (int j = 0; j < 6; j++)
            {
                if (Recipe.SMdatas[j].SMEnabled)
                {
                    posxyz[0] = Recipe.SMdatas[j].SMStartX;
                    posxyz[1] = Recipe.SMdatas[j].SMStartY;
                    posxyz[2] = Recipe.SMdatas[j].SMStartZ;
                    break;
                }
            }

            if (MeasurementContext.Config.IsLeftSMUDCylinderEnable && MeasurementContext.Config.IsPreTearFilmPress)
            {
                OpenLeftSMUDCylinder();
                Thread.Sleep(50);
                if (!WaitIOExMSec(Config.LeftSM_UD_CylinderDownIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "左撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            if (!IsOnPosition(_Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))//ZGH20220913增加
            {
                if (!AxisMoveTo(_Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))
                {
                    _Axis_LeftSM_Z.StopSlowly();
                    return false;
                }
            }

            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftSM_X, _Axis_LeftSM_Y },
                       new double[] { posxyz[0], posxyz[1] }))
            {
                _Axis_LeftSM_X.StopSlowly();
                _Axis_LeftSM_Y.StopSlowly();
                return false;
            }

            OpenLeftSMGlueUDCylinder();
            if (!WaitIOExMSec(Config.LeftSM_GlueUD_CylinderDownIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "左撕膜胶带上下气缸下感应位报警！";
                fra.ShowDialog();
            }

            if (!IsOnPosition(_Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))//ZGH20220912增加
            {
                if (!AxisMoveTo(_Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))
                {
                    _Axis_LeftSM_Z.StopSlowly();
                    return false;
                }
            }

            if (!AxisMoveTo(_Axis_LeftSM_Z, posxyz[2]))
            {
                _Axis_LeftSM_Z.StopSlowly();
                return false;
            }

            Thread.Sleep(50);
            b_tear1loop = true;
            b_tear1stop = false;

            OpenLeftSMFPCSuck();
            WaitMilliSec(30);

            if (MeasurementContext.Config.IsLeftSMUDCylinderEnable)
            {
                OpenLeftSMUDCylinder();
                Thread.Sleep(50);
                if (!WaitIOExMSec(Config.LeftSM_UD_CylinderDownIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "左撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            CloseLeftSMRollerUDCyliner();
            if (!WaitIOExMSec(Config.LeftSM_RollerUD_CylinderUpIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "左撕膜滚轮气缸上感应位报警！";
                fra.ShowDialog();
            }

            Thread.Sleep(10);
            for (int i = 0; i < 6; i++)
            {

                if (Recipe.SMdatas[i].SMEnabled)
                {
                    Thread.Sleep(10);

                    startposes = new double[] { Recipe.SMdatas[i].SMStartX, Recipe.SMdatas[i].SMStartY };//, Recipe.SMdatas[i].SMStartZ };
                    endposes = new double[] { Recipe.SMdatas[i].SMEndX, Recipe.SMdatas[i].SMEndY, Recipe.SMdatas[i].SMEndZ };

                    if (!AxisMoveTo(new MeasurementAxis[] { axise[0], axise[1] }, startposes))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!axise[2].MoveTo(Recipe.SMdatas[i].SMStartZ, 150))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }
                    Thread.Sleep(200);

                    if (!Axis_LeftSM_Y.Move(Recipe.SMdatas[i].GlueDist, Recipe.SMdatas[i].GlueStickSpd)
                        || !Axis_LeftSM_W.Move(Recipe.SMdatas[i].GlueStickDist, Recipe.SMdatas[i].GlueStickSpd))
                    {
                        Axis_LeftSM_Y.StopSlowly();
                        Axis_LeftSM_W.StopSlowly();
                        return false;
                    }


                    if (!CheckTear1AxisDone(_Axis_LeftSM_Y))
                    {
                        return false;
                    }

                    if (!CheckTear1AxisDone(_Axis_LeftSM_W))
                    {
                        return false;
                    }

                    if (Recipe.SMdatas[i].TearRllCLDEnabled)
                    {
                        OpenLeftSMRollerUDCylinder();
                        Thread.Sleep(100);

                    }

                    Thread.Sleep(100);//ZGH20220912增加
                    if (!axise[2].MoveTo(endposes[2], smspeed[2]))//ZGH20220912增加
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!Axis_LeftSM_Z.Move(-Recipe.SMPosition[0].SM_ZDist))
                    {
                        return false;
                    }
                    //if (!axise[2].MoveTo(endposes[2], smspeed[2]))
                    //{
                    //    axise[2].StopSlowly();
                    //    return false;
                    //}

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }
                    //while (axise[2].PositionDev > (Recipe.SMdatas[i].SMStartZ - Recipe.SMPosition[0].SM_ZDist))
                    //{
                    //    OutputError("左撕膜起始位Z高度低于结束位高度!");
                    //    OutputMessage($"ZPos{axise[2].PositionDev} ZStart{Recipe.SMdatas[i].SMStartZ}");
                    //    return false;
                    //}

                    if (!axise[0].MoveTo(endposes[0], smspeed[0]) || !axise[1].MoveTo(endposes[1], smspeed[1]))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!_Axis_LeftSM_W.Move(Recipe.SMdatas[i].SMDist))
                    {
                        _Axis_LeftSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear1AxisDone(axise[0]))
                    {
                        return false;
                    }

                    if (!CheckTear1AxisDone(axise[1]))
                    {
                        return false;
                    }

                    if (!CheckTear1AxisDone(axise[2]))
                    {
                        return false;
                    }

                    if (!CheckTear1AxisDone(_Axis_LeftSM_W))
                    {
                        return false;
                    }

                    CloseLeftSMRollerUDCyliner();
                    if (!IsLeftSM_MPHave)
                    {
                        DialogResult DRet = ShowMsgChoiceBox("左撕膜膜片检测NG", false, false);
                    }
                }
            }
            if (!AxisMoveTo(Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))
            {
                _Axis_LeftSM_Z.StopSlowly();
                return false;
            }

            //ZGH20220912增加撕膜X轴安全位
            if (!IsOnPosition(_Axis_LeftSM_X, Recipe.LeftSM_XSafePos))
            {
                if (!AxisMoveTo(_Axis_LeftSM_X, Recipe.LeftSM_XSafePos))
                {
                    _Axis_LeftSM_X.StopSlowly();
                    return false;
                }
            }
            b_tear1loop = false;
            Thread.Sleep(20);
            ret = true;
            return ret;
        }

        private bool LeftSMLoop(params int[] indexs)
        {

            bool ret = false;
            _IsStop = false;
            MeasurementAxis[] axise = new MeasurementAxis[] { _Axis_LeftSM_X, _Axis_LeftSM_Y, _Axis_LeftSM_Z };
            double[] startposes;
            double[] endposes;
            double[] smspeed = new double[] { Recipe.SMPosition[0].SM_XSpeed, Recipe.SMPosition[0].SM_YSpeed, Recipe.SMPosition[0].SM_ZSpeed };
            double paste_Speed = Recipe.SMPosition[0].Paste_Speed;
            double[] posxyz = new double[] { 0, 0, 0 };

            if (MeasurementContext.Config.IsLeftSMUDCylinderEnable && MeasurementContext.Config.IsPreTearFilmPress)
            {
                OpenLeftSMUDCylinder();
                Thread.Sleep(50);
                if (!WaitIOExMSec(Config.LeftSM_UD_CylinderDownIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "左撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }


            if (!IsOnPosition(_Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))
            {
                if (!AxisMoveTo(_Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))
                {
                    _Axis_LeftSM_Z.StopSlowly();
                    return false;
                }
            }

            foreach (int j in indexs)
            {
                if (j >= 6 || j < 0) continue;
                if (Recipe.SMdatas[j].SMEnabled)
                {
                    posxyz[0] = Recipe.SMdatas[j].SMStartX;
                    posxyz[1] = Recipe.SMdatas[j].SMStartY;
                    posxyz[2] = Recipe.SMdatas[j].SMStartZ;
                    break;
                }
            }

            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftSM_X, _Axis_LeftSM_Y },
                       new double[] { posxyz[0], posxyz[1] }))
            {
                _Axis_LeftSM_X.StopSlowly();
                _Axis_LeftSM_Y.StopSlowly();
                return false;
            }
            OpenLeftSMGlueUDCylinder();
            if (!WaitIOExMSec(Config.LeftSM_GlueUD_CylinderDownIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "左撕膜胶带上下气缸下感应位报警！";
                fra.ShowDialog();
            }

            if (!AxisMoveTo(_Axis_LeftSM_Z, posxyz[2]))
            {
                _Axis_LeftSM_Z.StopSlowly();
                return false;
            }

            Thread.Sleep(50);
            b_tear1loop = true;
            b_tear1stop = false;

            OpenLeftSMFPCSuck();
            WaitMilliSec(30);

            if (MeasurementContext.Config.IsLeftSMUDCylinderEnable)
            {
                OpenLeftSMUDCylinder();
                Thread.Sleep(50);
                if (!WaitIOExMSec(Config.LeftSM_UD_CylinderDownIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "左撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            CloseLeftSMRollerUDCyliner();
            if (!WaitIOExMSec(Config.LeftSM_RollerUD_CylinderUpIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "左撕膜滚轮气缸上感应位报警！";
                fra.ShowDialog();
            }

            Thread.Sleep(10);
            foreach (int i in indexs)
            {
                if (i >= 6 || i < 0)
                {
                    continue;
                }
                if (Recipe.SMdatas[i].SMEnabled)
                {
                    Thread.Sleep(10);
                    startposes = new double[] { Recipe.SMdatas[i].SMStartX, Recipe.SMdatas[i].SMStartY };//, Recipe.SMdatas[i].SMStartZ };
                    endposes = new double[] { Recipe.SMdatas[i].SMEndX, Recipe.SMdatas[i].SMEndY, Recipe.SMdatas[i].SMEndZ };

                    if (!AxisMoveTo(new MeasurementAxis[] { axise[0], axise[1] }, startposes))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!axise[2].MoveTo(Recipe.SMdatas[i].SMStartZ, 150))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }
                    Thread.Sleep(200);

                    if (!Axis_LeftSM_Y.Move(Recipe.SMdatas[i].GlueDist, Recipe.SMdatas[i].GlueStickSpd)
                        || !Axis_LeftSM_W.Move(Recipe.SMdatas[i].GlueStickDist, Recipe.SMdatas[i].GlueStickSpd))
                    {
                        Axis_LeftSM_Y.StopSlowly();
                        Axis_LeftSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear1AxisDone(_Axis_LeftSM_Y))
                    {
                        return false;
                    }

                    if (!CheckTear1AxisDone(_Axis_LeftSM_W))
                    {
                        return false;
                    }

                    if (Recipe.SMdatas[i].TearRllCLDEnabled)
                    {
                        OpenLeftSMRollerUDCylinder();
                        Thread.Sleep(100);
                    }

                    Thread.Sleep(100);//ZGH20220912增加
                    if (!axise[2].MoveTo(endposes[2], smspeed[2]))//ZGH20220912增加
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!Axis_LeftSM_Z.Move(-Recipe.SMPosition[0].SM_ZDist))
                    {
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }
                    //if (!axise[2].MoveTo(endposes[2], smspeed[2]))
                    //{
                    //    axise[2].StopSlowly();
                    //    return false;
                    //}
                    //axise[2].Wait();
                    //while (axise[2].PositionDev > (Recipe.SMdatas[i].SMStartZ - Recipe.SMPosition[0].SM_ZDist))
                    //{
                    //    OutputError("左撕膜起始位Z高度低于结束位高度!");
                    //    return false;
                    //}
                    if (!axise[0].MoveTo(endposes[0], smspeed[0]) || !axise[1].MoveTo(endposes[1], smspeed[1]))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!_Axis_LeftSM_W.Move(Recipe.SMdatas[i].SMDist))
                    {
                        _Axis_LeftSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear1AxisDone(axise[0]))
                    {
                        return false;
                    }

                    if (!CheckTear1AxisDone(axise[1]))
                    {
                        return false;
                    }

                    if (!CheckTear1AxisDone(axise[2]))
                    {
                        return false;
                    }

                    if (!CheckTear1AxisDone(_Axis_LeftSM_W))
                    {
                        return false;
                    }

                    CloseLeftSMRollerUDCyliner();
                    if (!IsLeftSM_MPHave)
                    {
                        DialogResult DRet = ShowMsgChoiceBox("左撕膜膜片检测NG", false, false);
                    }
                }
            }
            if (!AxisMoveTo(Axis_LeftSM_Z, Recipe.SMPosition[0].Lsm_WaitZ))
            {
                _Axis_LeftSM_Z.StopSlowly();
                return false;
            }
            //ZGH20220912增加撕膜X轴安全位
            if (!IsOnPosition(_Axis_LeftSM_X, Recipe.LeftSM_XSafePos))
            {
                if (!AxisMoveTo(_Axis_LeftSM_X, Recipe.LeftSM_XSafePos))
                {
                    _Axis_LeftSM_X.StopSlowly();
                    return false;
                }
            }
            b_tear1loop = false;
            Thread.Sleep(20);
            ret = true;
            return ret;
        }



        private bool MidSMLoop()
        {
            bool ret = true;
            MeasurementAxis[] axise = new MeasurementAxis[] { _Axis_MidSM_X, _Axis_MidSM_Y, _Axis_MidSM_Z };
            double[] startposes;
            double[] endposes;

            double[] smspeed = new double[] { Recipe.SMPosition[1].SM_XSpeed, Recipe.SMPosition[1].SM_YSpeed, Recipe.SMPosition[1].SM_ZSpeed };
            double paste_Speed = Recipe.SMPosition[1].Paste_Speed;
            double[] posxyz = new double[] { 0, 0, 0 };


            if (MeasurementContext.Config.IsMidSMUDCylinderEnable && MeasurementContext.Config.IsPreTearFilmPress)
            {
                OpenMidSMUDCylinder();
                Thread.Sleep(50);
                if (!WaitIOExMSec(Config.MidSM_UD_CylinderDownIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "中撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            if (!IsOnPosition(_Axis_MidSM_Z, Recipe.SMPosition[1].Lsm_WaitZ))
            {
                if (!AxisMoveTo(_Axis_MidSM_Z, Recipe.SMPosition[1].Lsm_WaitZ))
                {
                    _Axis_MidSM_Z.StopSlowly();
                    return false;
                }
            }

            for (int j = 6; j < 12; j++)
            {
                if (Recipe.SMdatas[j].SMEnabled)
                {
                    posxyz[0] = Recipe.SMdatas[j].SMStartX;
                    posxyz[1] = Recipe.SMdatas[j].SMStartY;
                    posxyz[2] = Recipe.SMdatas[j].SMStartZ;
                    break;
                }

            }


            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidSM_X, _Axis_MidSM_Y },
                      new double[] { posxyz[0], posxyz[1] }))
            {
                _Axis_MidSM_X.StopSlowly();
                _Axis_MidSM_Y.StopSlowly();
                return false;
            }
            OpenMidSMGlueUDCylinder();
            if (!WaitIOExMSec(Config.MidSM_GlueUD_CylinderDownIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "中撕膜胶带上下气缸下感应位报警！";
                fra.ShowDialog();
            }

            if (!AxisMoveTo(_Axis_MidSM_Z, posxyz[2]))
            {
                _Axis_MidSM_Z.StopSlowly();
                return false;
            }
            OpenMidSMFPCSuck();
            Thread.Sleep(20);

            if (MeasurementContext.Config.IsMidSMUDCylinderEnable)
            {
                OpenMidSMUDCylinder();
                Thread.Sleep(200);
                if (!WaitIOExMSec(Config.MidSM_UD_CylinderDownIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "中撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            CloseMidSMRollerUDCyliner();
            if (!WaitIOExMSec(Config.MidSM_RollerUD_CylinderUPIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "中撕膜滚轮气缸上感应位报警！";
                fra.ShowDialog();
            }

            b_tear2loop = true;
            b_tear2stop = false;
            Thread.Sleep(10);
            for (int i = 6; i < 12; i++)
            {
                if (Recipe.SMdatas[i].SMEnabled)
                {
                    Thread.Sleep(10);
                    //while (!_IsAutoRun)
                    //{
                    //    Thread.Sleep(50);
                    //    if (_IsStop)
                    //    {
                    //        return false;
                    //    }
                    //}

                    startposes = new double[] { Recipe.SMdatas[i].SMStartX, Recipe.SMdatas[i].SMStartY };// Recipe.SMdatas[i].SMStartZ };
                    endposes = new double[] { Recipe.SMdatas[i].SMEndX, Recipe.SMdatas[i].SMEndY, Recipe.SMdatas[i].SMEndZ };
                    if (!AxisMoveTo(new MeasurementAxis[] { axise[0], axise[1] }, startposes))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!axise[2].MoveTo(Recipe.SMdatas[i].SMStartZ, 150))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }

                    Thread.Sleep(100);
                    if (!Axis_MidSM_Y.Move(Recipe.SMdatas[i].GlueDist, Recipe.SMdatas[i].GlueStickSpd) ||
                        !Axis_MidSM_W.Move(Recipe.SMdatas[i].GlueStickDist, Recipe.SMdatas[i].GlueStickSpd))
                    {
                        Axis_MidSM_Y.StopSlowly();
                        Axis_MidSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear2AxisDone(_Axis_MidSM_Y))
                    {
                        return false;
                    }

                    if (!CheckTear2AxisDone(_Axis_MidSM_W))
                    {
                        return false;
                    }
                    if (Recipe.SMdatas[i].TearRllCLDEnabled)
                    {
                        OpenMidSMRollerUDCylinder();
                        Thread.Sleep(100);
                    }
                    Thread.Sleep(100);

                    if (!axise[2].MoveTo(endposes[2], smspeed[2]))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!Axis_MidSM_Z.Move(-Recipe.SMPosition[1].SM_ZDist))
                    {
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }

                    //while (axise[2].PositionDev > (Recipe.SMdatas[i].SMStartZ - Recipe.SMPosition[1].SM_ZDist))
                    //{
                    //    OutputError("中撕膜起始位Z高度低于结束位高度!");
                    //    return false;
                    //}



                    if (!axise[0].MoveTo(endposes[0], smspeed[0]) || !axise[1].MoveTo(endposes[1], smspeed[1]))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!_Axis_MidSM_W.Move(Recipe.SMdatas[i].SMDist))
                    {
                        _Axis_MidSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear2AxisDone(axise[0]))
                    {
                        return false;
                    }

                    if (!CheckTear2AxisDone(axise[1]))
                    {
                        return false;
                    }

                    if (!CheckTear2AxisDone(axise[2]))
                    {
                        return false;
                    }

                    if (!CheckTear2AxisDone(_Axis_MidSM_W))
                    {
                        return false;
                    }
                    CloseMidSMRollerUDCyliner();
                    if (!IsMidSM_MPHave)
                    {
                        DialogResult DRet = ShowMsgChoiceBox("中撕膜膜片检测NG，点确定继续运行", false, false);
                    }
                }
            }
            if (!AxisMoveTo(Axis_MidSM_Z, Recipe.SMPosition[1].Lsm_WaitZ))
            {
                return false;
            }

            //ZGH20220912增加撕膜X轴安全位
            if (!IsOnPosition(_Axis_MidSM_X, Recipe.MidSM_XSafePos))
            {
                if (!AxisMoveTo(_Axis_MidSM_X, Recipe.MidSM_XSafePos))
                {
                    _Axis_MidSM_X.StopSlowly();
                    return false;
                }
            }
            b_tear2loop = false;
            Thread.Sleep(20);
            return ret;
        }

        #region 2020619
        /// <summary>
        /// 撕指定编号的膜
        /// </summary>
        /// <param name="indexs">膜编号 0 1 2</param>
        /// <returns></returns>
        private bool MidSMLoop(params int[] indexs)
        {
            bool ret = true;
            MeasurementAxis[] axise = new MeasurementAxis[] { _Axis_MidSM_X, _Axis_MidSM_Y, _Axis_MidSM_Z };
            double[] startposes;
            double[] endposes;

            double[] smspeed = new double[] { Recipe.SMPosition[1].SM_XSpeed, Recipe.SMPosition[1].SM_YSpeed, Recipe.SMPosition[1].SM_ZSpeed };
            double paste_Speed = Recipe.SMPosition[1].Paste_Speed;
            double[] posxyz = new double[] { 0, 0, 0 };
            if (MeasurementContext.Config.IsMidSMUDCylinderEnable && MeasurementContext.Config.IsPreTearFilmPress)
            {
                OpenMidSMUDCylinder();
                Thread.Sleep(50);
                if (!WaitIOExMSec(Config.MidSM_UD_CylinderDownIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "中撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            if (!IsOnPosition(_Axis_MidSM_Z, Recipe.SMPosition[1].Lsm_WaitZ))
            {
                if (!AxisMoveTo(_Axis_MidSM_Z, Recipe.SMPosition[1].Lsm_WaitZ))
                {
                    _Axis_MidSM_Z.StopSlowly();
                    return false;
                }
            }
            foreach (int j in indexs)
            {

                if (j >= 6 || j < 0) continue;
                if (Recipe.SMdatas[j + 6].SMEnabled)
                {
                    posxyz[0] = Recipe.SMdatas[j + 6].SMStartX;
                    posxyz[1] = Recipe.SMdatas[j + 6].SMStartY;
                    posxyz[2] = Recipe.SMdatas[j + 6].SMStartZ;
                    break;
                }

            }


            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidSM_X, _Axis_MidSM_Y },
                      new double[] { posxyz[0], posxyz[1] }))
            {
                _Axis_MidSM_X.StopSlowly();
                _Axis_MidSM_Y.StopSlowly();
                return false;
            }
            OpenMidSMGlueUDCylinder();
            if (!WaitIOExMSec(Config.MidSM_GlueUD_CylinderDownIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "中撕膜胶带上下气缸下感应位报警！";
                fra.ShowDialog();
            }



            if (!AxisMoveTo(_Axis_MidSM_Z, posxyz[2]))
            {
                _Axis_MidSM_Z.StopSlowly();
                return false;
            }



            OpenMidSMFPCSuck();
            Thread.Sleep(20);

            if (MeasurementContext.Config.IsMidSMUDCylinderEnable)
            {
                OpenMidSMUDCylinder();
                Thread.Sleep(200);
                if (!WaitIOExMSec(Config.MidSM_UD_CylinderDownIOInEx))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "中撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            CloseMidSMRollerUDCyliner();
            if (!WaitIOExMSec(Config.MidSM_RollerUD_CylinderUPIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "中撕膜滚轮气缸上感应位报警！";
                fra.ShowDialog();
            }

            b_tear2loop = true;
            b_tear2stop = false;
            Thread.Sleep(10);
            foreach (int n in indexs)
            {
                if (n >= 6 || n < 0) continue;
                int i = 0;
                i = n + 6;

                if (Recipe.SMdatas[i].SMEnabled)
                {
                    Thread.Sleep(10);
                    //while (!_IsAutoRun)
                    //{
                    //    Thread.Sleep(50);
                    //    if (_IsStop)
                    //    {
                    //        return false;
                    //    }
                    //}

                    startposes = new double[] { Recipe.SMdatas[i].SMStartX, Recipe.SMdatas[i].SMStartY };// Recipe.SMdatas[i].SMStartZ };
                    endposes = new double[] { Recipe.SMdatas[i].SMEndX, Recipe.SMdatas[i].SMEndY, Recipe.SMdatas[i].SMEndZ };
                    if (!AxisMoveTo(new MeasurementAxis[] { axise[0], axise[1] }, startposes))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!axise[2].MoveTo(Recipe.SMdatas[i].SMStartZ, 150))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }

                    Thread.Sleep(100);
                    if (!Axis_MidSM_Y.Move(Recipe.SMdatas[i].GlueDist, Recipe.SMdatas[i].GlueStickSpd) ||
                        !Axis_MidSM_W.Move(Recipe.SMdatas[i].GlueStickDist, Recipe.SMdatas[i].GlueStickSpd))
                    {
                        Axis_MidSM_Y.StopSlowly();
                        Axis_MidSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear2AxisDone(_Axis_MidSM_Y))
                    {
                        return false;
                    }

                    if (!CheckTear2AxisDone(_Axis_MidSM_W))
                    {
                        return false;
                    }
                    if (Recipe.SMdatas[i].TearRllCLDEnabled)
                    {
                        OpenMidSMRollerUDCylinder();
                        Thread.Sleep(100);
                    }
                    Thread.Sleep(100);

                    if (!axise[2].MoveTo(endposes[2], smspeed[2]))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!Axis_MidSM_Z.Move(-Recipe.SMPosition[1].SM_ZDist))
                    {
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }

                    //axise[2].Wait();
                    //while (axise[2].PositionDev > (Recipe.SMdatas[i].SMStartZ - Recipe.SMPosition[1].SM_ZDist))
                    //{
                    //    OutputError("中撕膜起始位Z高度低于结束位高度!");
                    //    return false;
                    //}



                    if (!axise[0].MoveTo(endposes[0], smspeed[0]) || !axise[1].MoveTo(endposes[1], smspeed[1]))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!_Axis_MidSM_W.Move(Recipe.SMdatas[i].SMDist))
                    {
                        _Axis_MidSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear2AxisDone(axise[0]))
                    {
                        return false;
                    }

                    if (!CheckTear2AxisDone(axise[1]))
                    {
                        return false;
                    }

                    if (!CheckTear2AxisDone(axise[2]))
                    {
                        return false;
                    }

                    if (!CheckTear2AxisDone(_Axis_MidSM_W))
                    {
                        return false;
                    }
                    CloseMidSMRollerUDCyliner();
                    if (!IsMidSM_MPHave)
                    {
                        DialogResult DRet = ShowMsgChoiceBox("中撕膜膜片检测NG，点确定继续运行", false, false);
                    }
                }
            }
            if (!AxisMoveTo(Axis_MidSM_Z, Recipe.SMPosition[1].Lsm_WaitZ))
            {
                return false;
            }
            //ZGH20220912增加撕膜X轴安全位
            if (!IsOnPosition(_Axis_MidSM_X, Recipe.MidSM_XSafePos))
            {
                if (!AxisMoveTo(_Axis_MidSM_X, Recipe.MidSM_XSafePos))
                {
                    _Axis_MidSM_X.StopSlowly();
                    return false;
                }
            }
            b_tear2loop = false;
            Thread.Sleep(20);
            return ret;
        }

        #endregion 

        private bool RightSMLoop()
        {
            bool ret = false;
            MeasurementAxis[] axise = new MeasurementAxis[] { _Axis_RightSM_X, _Axis_RightSM_Y, _Axis_RightSM_Z };
            double[] startposes;
            double[] endposes;
            double[] smspeed = new double[] { Recipe.SMPosition[2].SM_XSpeed, Recipe.SMPosition[2].SM_YSpeed, Recipe.SMPosition[2].SM_ZSpeed };
            double paste_Speed = Recipe.SMPosition[2].Paste_Speed;
            double[] posxyz = new double[] { 0, 0, 0 };

            if (MeasurementContext.Config.IsRightUDCylinderEnable && MeasurementContext.Config.IsPreTearFilmPress)
            {
                OpenRightSMUDCylinder();
                Thread.Sleep(200);
                if (!WaitIOMSec(Config.RightSM_UD_CylinderDownIOIn, 1000, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "右撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            if (!IsOnPosition(_Axis_RightSM_Z, Recipe.SMPosition[2].Lsm_WaitZ))
            {
                if (!AxisMoveTo(_Axis_RightSM_Z, Recipe.SMPosition[2].Lsm_WaitZ))
                {
                    _Axis_RightSM_Z.StopSlowly();
                    return false;
                }
            }
            for (int j = 12; j < 18; j++)
            {
                if (Recipe.SMdatas[j].SMEnabled)
                {
                    posxyz[0] = Recipe.SMdatas[j].SMStartX;
                    posxyz[1] = Recipe.SMdatas[j].SMStartY;
                    posxyz[2] = Recipe.SMdatas[j].SMStartZ;
                    break;
                }
            }


            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightSM_X, _Axis_RightSM_Y },
                new double[] { posxyz[0], posxyz[1] }))
            {
                _Axis_RightSM_X.StopSlowly();
                _Axis_RightSM_Y.StopSlowly();
                return false;
            }

            OpenRightSMGlueUDCylinder();
            if (!WaitIOExMSec(Config.RightSM_GlueUD_CylinderDownIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "右撕膜胶带上下气缸下感应位报警！";
                fra.ShowDialog();
            }


            if (!AxisMoveTo(_Axis_RightSM_Z, posxyz[2]))
            {
                _Axis_RightSM_Z.StopSlowly();
                return false;
            }

            OpenRightSMFPCSuck();
            Thread.Sleep(30);

            if (MeasurementContext.Config.IsRightUDCylinderEnable)
            {
                OpenRightSMUDCylinder();
                Thread.Sleep(200);
                if (!WaitIOMSec(Config.RightSM_UD_CylinderDownIOIn, 1000, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "右撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            CloseRightSMRollerUDCyliner();
            if (!WaitIOExMSec(Config.RightSM_RollerUD_CylinderUPIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "右撕膜滚轮气缸上感应位报警！";
                fra.ShowDialog();
            }

            b_tear3loop = true;
            b_tear3stop = false;
            for (int i = 12; i < 18; i++)
            {

                if (Recipe.SMdatas[i].SMEnabled)
                {
                    Thread.Sleep(10);

                    //while (!_IsAutoRun)
                    //{
                    //    Thread.Sleep(50);
                    //    if (_IsStop)
                    //    {
                    //        return false;
                    //    }
                    //}

                    startposes = new double[] { Recipe.SMdatas[i].SMStartX, Recipe.SMdatas[i].SMStartY };//Recipe.SMdatas[i].SMStartZ };
                    endposes = new double[] { Recipe.SMdatas[i].SMEndX, Recipe.SMdatas[i].SMEndY, Recipe.SMdatas[i].SMEndZ };
                    if (!AxisMoveTo(new MeasurementAxis[] { axise[0], axise[1] }, startposes))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!axise[2].MoveTo(Recipe.SMdatas[i].SMStartZ, 100))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }
                    Thread.Sleep(100);

                    if (!Axis_RightSM_Y.Move(Recipe.SMdatas[i].GlueDist, Recipe.SMdatas[i].GlueStickSpd)
                        || !Axis_RightSM_W.Move(Recipe.SMdatas[i].GlueStickDist, Recipe.SMdatas[i].GlueStickSpd))
                    {
                        Axis_RightSM_Y.StopSlowly();
                        Axis_RightSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear3AxisDone(_Axis_RightSM_Y))
                    {
                        return false;
                    }

                    if (!CheckTear3AxisDone(_Axis_RightSM_W))
                    {
                        return false;
                    }

                    Thread.Sleep(100);

                    if (Recipe.SMdatas[i].TearRllCLDEnabled)
                    {
                        OpenRightRollerUDCylinder();
                        Thread.Sleep(100);
                    }


                    if (!axise[2].MoveTo(endposes[2], smspeed[2]))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!Axis_RightSM_Z.Move(-Recipe.SMPosition[2].SM_ZDist))
                    {
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }


                    //axise[2].Wait();
                    //while (axise[2].PositionDev > (Recipe.SMdatas[i].SMStartZ - Recipe.SMPosition[2].SM_ZDist))
                    //{
                    //    OutputError("右撕膜起始位Z高度低于结束位高度!");
                    //    return false;
                    //}


                    if (!axise[0].MoveTo(endposes[0], smspeed[0]) || !axise[1].MoveTo(endposes[1], smspeed[1]))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!_Axis_RightSM_W.Move(Recipe.SMdatas[i].SMDist))
                    {
                        _Axis_RightSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear3AxisDone(axise[0]))
                    {
                        return false;
                    }

                    if (!CheckTear3AxisDone(axise[1]))
                    {
                        return false;
                    }

                    if (!CheckTear3AxisDone(axise[2]))
                    {
                        return false;
                    }

                    if (!CheckTear3AxisDone(_Axis_RightSM_W))
                    {
                        return false;
                    }

                    CloseRightSMRollerUDCyliner();

                    if (!IsRightSM_MPHave)
                    {
                        DialogResult DRet = ShowMsgChoiceBox("右撕膜膜片检测NG，点确定继续运行", false, false);
                    }


                }
            }

            if (!AxisMoveTo(Axis_RightSM_Z, Recipe.SMPosition[2].Lsm_WaitZ))
            {
                return false;
            }
            //ZGH20220912增加撕膜X轴安全位
            if (!IsOnPosition(_Axis_RightSM_X, Recipe.RightSM_XSafePos))
            {
                if (!AxisMoveTo(_Axis_RightSM_X, Recipe.RightSM_XSafePos))
                {
                    _Axis_RightSM_X.StopSlowly();
                    return false;
                }
            }
            b_tear3loop = false;
            Thread.Sleep(20);

            ret = true;
            return ret;
        }

        private bool RightSMLoop(params int[] indexs)
        {
            bool ret = false;
            MeasurementAxis[] axise = new MeasurementAxis[] { _Axis_RightSM_X, _Axis_RightSM_Y, _Axis_RightSM_Z };
            double[] startposes;
            double[] endposes;
            double[] smspeed = new double[] { Recipe.SMPosition[2].SM_XSpeed, Recipe.SMPosition[2].SM_YSpeed, Recipe.SMPosition[2].SM_ZSpeed };
            double paste_Speed = Recipe.SMPosition[2].Paste_Speed;
            double[] posxyz = new double[] { 0, 0, 0 };
            foreach (int j in indexs)
            {
                if (j >= 6 || j < 0) continue;
                if (Recipe.SMdatas[j + 12].SMEnabled)
                {
                    posxyz[0] = Recipe.SMdatas[j + 12].SMStartX;
                    posxyz[1] = Recipe.SMdatas[j + 12].SMStartY;
                    posxyz[2] = Recipe.SMdatas[j + 12].SMStartZ;
                    break;
                }
            }
            if (MeasurementContext.Config.IsRightUDCylinderEnable && MeasurementContext.Config.IsPreTearFilmPress)
            {
                OpenRightSMUDCylinder();
                Thread.Sleep(200);
                if (!WaitIOMSec(Config.RightSM_UD_CylinderDownIOIn, 1000, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "右撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightSM_X, _Axis_RightSM_Y },
                new double[] { posxyz[0], posxyz[1] }))
            {
                _Axis_RightSM_X.StopSlowly();
                _Axis_RightSM_Y.StopSlowly();
                return false;
            }

            OpenRightSMGlueUDCylinder();
            if (!WaitIOExMSec(Config.RightSM_GlueUD_CylinderDownIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "右撕膜胶带上下气缸下感应位报警！";
                fra.ShowDialog();
            }

            if (!AxisMoveTo(_Axis_RightSM_Z, posxyz[2]))
            {
                _Axis_RightSM_Z.StopSlowly();
                return false;
            }

            OpenRightSMFPCSuck();
            Thread.Sleep(30);

            if (MeasurementContext.Config.IsRightUDCylinderEnable)
            {
                OpenRightSMUDCylinder();
                Thread.Sleep(200);
                if (!WaitIOMSec(Config.RightSM_UD_CylinderDownIOIn, 1000, true))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "右撕膜上下气缸下感应位报警！";
                    fra.ShowDialog();
                }
            }

            CloseRightSMRollerUDCyliner();
            if (!WaitIOExMSec(Config.RightSM_RollerUD_CylinderUPIOInEx))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "右撕膜滚轮气缸上感应位报警！";
                fra.ShowDialog();
            }

            b_tear3loop = true;
            b_tear3stop = false;
            foreach (int n in indexs)
            {

                if (n >= 6 || n < 0) continue;
                int i = 0;
                i = n + 12;
                if (Recipe.SMdatas[i].SMEnabled)
                {
                    Thread.Sleep(10);


                    startposes = new double[] { Recipe.SMdatas[i].SMStartX, Recipe.SMdatas[i].SMStartY };//Recipe.SMdatas[i].SMStartZ };
                    endposes = new double[] { Recipe.SMdatas[i].SMEndX, Recipe.SMdatas[i].SMEndY, Recipe.SMdatas[i].SMEndZ };
                    if (!AxisMoveTo(new MeasurementAxis[] { axise[0], axise[1] }, startposes))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!axise[2].MoveTo(Recipe.SMdatas[i].SMStartZ, 100))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }
                    Thread.Sleep(100);

                    if (!Axis_RightSM_Y.Move(Recipe.SMdatas[i].GlueDist, Recipe.SMdatas[i].GlueStickSpd)
                        || !Axis_RightSM_W.Move(Recipe.SMdatas[i].GlueStickDist, Recipe.SMdatas[i].GlueStickSpd))
                    {
                        Axis_RightSM_Y.StopSlowly();
                        Axis_RightSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear3AxisDone(_Axis_RightSM_Y))
                    {
                        return false;
                    }

                    if (!CheckTear3AxisDone(_Axis_RightSM_W))
                    {
                        return false;
                    }

                    Thread.Sleep(100);

                    if (Recipe.SMdatas[i].TearRllCLDEnabled)
                    {
                        OpenRightRollerUDCylinder();
                        Thread.Sleep(100);
                    }


                    if (!axise[2].MoveTo(endposes[2], smspeed[2]))
                    {
                        axise[2].StopSlowly();
                        return false;
                    }

                    if (!Axis_RightSM_Z.Move(-Recipe.SMPosition[2].SM_ZDist))
                    {
                        return false;
                    }

                    if (!CheckAxisDone(axise[2]))
                    {
                        return false;
                    }



                    //axise[2].Wait();
                    //while (axise[2].PositionDev > (Recipe.SMdatas[i].SMStartZ - Recipe.SMPosition[2].SM_ZDist))
                    //{
                    //    OutputError("右撕膜起始位Z高度低于结束位高度!");
                    //    return false;
                    //}


                    if (!axise[0].MoveTo(endposes[0], smspeed[0]) || !axise[1].MoveTo(endposes[1], smspeed[1]))
                    {
                        axise[0].StopSlowly();
                        axise[1].StopSlowly();
                        return false;
                    }

                    if (!_Axis_RightSM_W.Move(Recipe.SMdatas[i].SMDist))
                    {
                        _Axis_RightSM_W.StopSlowly();
                        return false;
                    }

                    if (!CheckTear3AxisDone(axise[0]))
                    {
                        return false;
                    }

                    if (!CheckTear3AxisDone(axise[1]))
                    {
                        return false;
                    }

                    if (!CheckTear3AxisDone(axise[2]))
                    {
                        return false;
                    }

                    if (!CheckTear3AxisDone(_Axis_RightSM_W))
                    {
                        return false;
                    }

                    CloseRightSMRollerUDCyliner();

                    if (!IsRightSM_MPHave)
                    {
                        DialogResult DRet = ShowMsgChoiceBox("右撕膜膜片检测NG，点确定继续运行", false, false);
                    }


                }
            }

            if (!AxisMoveTo(Axis_RightSM_Z, Recipe.SMPosition[2].Lsm_WaitZ))
            {
                return false;
            }
            //ZGH20220912增加撕膜X轴安全位
            if (!IsOnPosition(_Axis_RightSM_X, Recipe.RightSM_XSafePos))
            {
                if (!AxisMoveTo(_Axis_RightSM_X, Recipe.RightSM_XSafePos))
                {
                    _Axis_RightSM_X.StopSlowly();
                    return false;
                }
            }
            b_tear3loop = false;
            Thread.Sleep(20);

            ret = true;
            return ret;
        }


        private int ErrSMLoop(StationType _station)
        {
            int ret = 0;
            if (_station == StationType.Left)
            {
                CloseLeftSMUDCylinder();

                if (!CheckAxisDone(_Axis_LeftSM_Y))
                {
                    OutputError("左撕膜Y轴停止报警！", true);
                    return -1;
                }

                if (!CheckAxisDone(_Axis_LeftSM_X))
                {
                    OutputError("左撕膜X轴停止报警!", true);
                    return -1;
                }

                if (!CheckAxisDone(_Axis_LeftSM_Z))
                {
                    OutputError("左撕膜Z轴停止报警!", true);
                    return -1;
                }

                if (!CheckAxisDone(_Axis_LeftSM_W))
                {
                    OutputError("左撕膜W轴停止报警!", true);
                    return -1;
                }

                if (!_Axis_LeftSM_Z.MoveTo(0))
                {
                    AlarmWork();
                    OutputError("左撕膜Z轴运动错误!", true);
                    _Axis_LeftSM_Z.StopSlowly();
                    return -1;
                }

                if (!CheckAxisDone(_Axis_LeftSM_Z))
                {
                    return -1;
                }
                Thread.Sleep(200);
                if (b_tear1stop)
                {
                    OutputError("左撕膜平台真空报警", true);
                    b_tear1stop = false;
                    DialogResult DRet = ShowMsgChoiceBox("左撕膜平台真空报警\r\n"
                        + "人工取料:点击人工取料后,取走物料!\r\n"
                        + "确认:继续下一步生产!", true, false);
                    if (DRet == DialogResult.Cancel)
                    {
                        CloseLeftSMSuck();
                        CloseLeftSMBlow();
                        frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                        frm.IOOut1 = Config.LeftSM_StgBlowVacuum_IOOut;
                        frm.IOOut2 = Config.LeftSM_StgVacuum_IOOut;
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {

                            FlagTear1Have = false;
                            ret = 1;
                        }
                    }
                    if (!Axis_LeftSM_W.Move(40))
                    {
                        Axis_LeftSM_W.StopSlowly();
                        return -1;
                    }
                    if (!CheckAxisDone(Axis_LeftSM_W))
                    {
                        return -1;
                    }

                }
            }
            else if (_station == StationType.Mid)
            {
                CloseMidSMUDCylinder();

                if (!CheckAxisDone(_Axis_MidSM_Y))
                {
                    OutputError("中撕膜Y轴停止报警！", true);
                    return -1;
                }

                if (!CheckAxisDone(_Axis_MidSM_X))
                {
                    OutputError("中撕膜X轴停止报警!", true);
                    return -1;
                }


                if (!CheckAxisDone(_Axis_MidSM_Z))
                {
                    OutputError("中撕膜Z停止报警！", true);
                    return -1;
                }

                if (!CheckAxisDone(_Axis_MidSM_W))
                {
                    OutputError("中撕膜W轴停止报警!", true);
                    return -1;
                }

                if (!_Axis_MidSM_Z.MoveTo(0))
                {
                    AlarmWork();
                    OutputError("中撕膜Z轴运动错误!", true);
                    _Axis_MidSM_Z.StopSlowly();
                    return -1;
                }
                if (!CheckAxisDone(_Axis_MidSM_Z))
                {
                    return -1;
                }
                Thread.Sleep(200);


                if (b_tear2stop)
                {
                    OutputError("中撕膜平台真空报警", true);
                    b_tear2stop = false;
                    DialogResult DRet = ShowMsgChoiceBox("中撕膜平台真空报警\r\n"
                        + "人工取料:点击人工取料后,取走物料!\r\n"
                        + "确认:继续下一步生产!", true, false);
                    if (DRet == DialogResult.Cancel)
                    {
                        CloseMidSMSuck();
                        CloseMidSMBlow();
                        frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                        frm.IOOutEx1 = Config.MidSM_StgBlowVacuum_IOOutEx;
                        frm.IOOut2 = Config.MidSM_StgVacuum_IOOut;
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {

                            FlagTear2Have = false;
                            ret = 1;
                        }
                    }
                    if (!Axis_MidSM_W.Move(40))
                    {
                        Axis_MidSM_W.StopSlowly();
                        return -1;
                    }
                    if (!CheckAxisDone(Axis_MidSM_W))
                    {
                        return -1;
                    }

                }
            }
            else
            {
                CloseRightSMUDCylinder();
                if (!CheckAxisDone(_Axis_RightSM_Y))
                {
                    OutputError("右撕膜Y轴停止报警！", true);
                    return -1;
                }

                if (!CheckAxisDone(_Axis_RightSM_X))
                {
                    OutputError("右撕膜X轴停止报警!", true);
                    return -1;
                }

                if (!CheckAxisDone(_Axis_RightSM_Z))
                {
                    OutputError("右撕膜Z停止报警!", true);
                    return -1;
                }

                if (!CheckAxisDone(_Axis_RightSM_W))
                {
                    OutputError("右撕膜W停止报警", true);
                    return -1;
                }
                if (!_Axis_RightSM_Z.MoveTo(0))
                {
                    OutputError("右撕膜Z轴运动错误!", true);
                    _Axis_RightSM_Z.StopSlowly();
                    return -1;
                }

                if (!CheckAxisDone(_Axis_RightSM_Z))
                {
                    return -1;
                }

                Thread.Sleep(200);



                if (b_tear3stop)
                {
                    OutputError("右撕膜平台真空报警", true);
                    b_tear3stop = false;
                    DialogResult DRet = ShowMsgChoiceBox("右撕膜平台真空报警\r\n"
                        + "人工取料:点击人工取料后,取走物料!\r\n"
                        + "确认:继续下一步生产!", true, false);
                    if (DRet == DialogResult.Cancel)
                    {
                        CloseRightSMSuck();
                        CloseRightSMBlow();
                        frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                        frm.IOOutEx1 = Config.RightSM_StgBlowVacuum_IOOutEx;
                        frm.IOOutEx2 = Config.RightSM_StgVacuum_IOOutEx;
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {

                            FlagTear3Have = false;
                            ret = 1;
                        }
                    }
                    if (!Axis_RightSM_W.Move(40))
                    {
                        Axis_RightSM_W.StopSlowly();
                        return -1;
                    }
                    if (!CheckAxisDone(Axis_RightSM_W))
                    {
                        return -1;
                    }
                }
            }
            return ret;
        }

        #endregion

        #region Bend Motion

        public bool Bend1_Go_Degree_Pos()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                if (!WaitIOMSec(Config.LeftBend_stgVacuum_IOIn, 300, true))
                {
                    OnMessageShow("左平台 真空吸 报警");
                    return false;
                }
                if (!IsOnPosition(_Axis_LeftBend_DWR, Recipe.LeftBend_DWR_SafePos))
                {
                    if (!AxisMoveTo(_Axis_LeftBend_stgY, Recipe.LeftBend_DWY_WorkPos))
                    {
                        _Axis_LeftBend_stgY.StopSlowly();
                        return false;
                    }

                    if (!AxisMoveTo(_Axis_LeftBend_DWR, Recipe.LeftBend_DWR_SafePos))
                    {
                        _Axis_LeftBend_DWR.StopSlowly();
                        return false;
                    }
                }



                if (!GetIOOutStatus(Config.LeftBend_ClawCylinderOut_IOOut))
                {
                    CloseLeftBendClawCylinder();
                    Thread.Sleep(50);
                }


                if (!IsOnPosition(_Axis_LeftBend_DWX, Recipe.LeftBend_DWX_WorkPos))
                {
                    if (!AxisMoveTo(_Axis_LeftBend_DWX, Recipe.LeftBend_DWX_WorkPos))
                    {
                        _Axis_LeftBend_DWX.StopSlowly();
                        return false;
                    }
                }

                if (!AxisMoveTo(_Axis_LeftBend_DWY, Recipe.LeftBend_DWY_WorkPos))
                {
                    _Axis_LeftBend_DWY.StopSlowly();
                    return false;
                }



                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_stgY, _Axis_LeftBend_CCDX },
                    new double[] { Recipe.LeftBend_CCDPos_Y, Recipe.LeftBend_CCDPos_X }))
                {
                    _Axis_LeftBend_stgY.StopSlowly();
                    _Axis_LeftBend_CCDX.StopSlowly();
                    return false;
                }
                return result;
            }
        }

        public bool Bend2_Go_Degree_Pos()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                if (!WaitIOMSec(Config.MidBend_stgVacuum_IOIn, 2000, true))
                {
                    OnMessageShow("中平台 真空吸 报警");
                    return false;
                }
                if (!IsOnPosition(_Axis_MidBend_DWR, Recipe.MidBend_DWR_SafePos))
                {
                    if (!AxisMoveTo(_Axis_MidBend_stgY, Recipe.MidBend_DWY_WorkPos))
                    {
                        _Axis_MidBend_stgY.StopSlowly();
                        return false;
                    }

                    if (!AxisMoveTo(_Axis_MidBend_DWR, Recipe.MidBend_DWR_SafePos))
                    {
                        _Axis_MidBend_DWR.StopSlowly();
                        return false;
                    }
                }



                if (!GetIOOutStatus(Config.MidBend_ClawCylinderOut_IOOut))
                {
                    CloseMidBendClawCylinder();
                    Thread.Sleep(50);
                }


                if (!IsOnPosition(_Axis_MidBend_DWX, Recipe.MidBend_DWX_WorkPos))
                {
                    if (!AxisMoveTo(_Axis_MidBend_DWX, Recipe.MidBend_DWX_WorkPos))
                    {
                        _Axis_MidBend_DWX.StopSlowly();
                        return false;
                    }
                }



                if (!AxisMoveTo(_Axis_MidBend_DWY, Recipe.MidBend_DWY_WorkPos))
                {
                    _Axis_MidBend_DWY.StopSlowly();
                    return false;
                }




                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_stgY, _Axis_MidBend_CCDX },
                    new double[] { Recipe.MidBend_CCDPos_Y, Recipe.MidBend_CCDPos_X }))
                {
                    _Axis_MidBend_stgY.StopSlowly();
                    _Axis_MidBend_CCDX.StopSlowly();
                    return false;
                }
                return result;

            }
        }
        public bool Bend3_Go_Degree_Pos()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                if (!WaitIOMSec(Config.RightBend_stgVacuum_IOIn, 2000, true))
                {
                    OnMessageShow("右平台真空吸报警");
                    return false;
                }

                if (!IsOnPosition(_Axis_RightBend_DWR, Recipe.RightBend_DWR_SafePos))
                {
                    if (!AxisMoveTo(_Axis_RightBend_stgY, Recipe.RightBend_DWY_WorkPos))
                    {
                        _Axis_RightBend_stgY.StopSlowly();
                        return false;
                    }

                    if (!AxisMoveTo(_Axis_RightBend_DWR, Recipe.RightBend_DWR_SafePos))
                    {
                        _Axis_RightBend_DWR.StopSlowly();
                        return false;
                    }
                }

                if (!GetIOOutStatus(Config.RightBend_ClawCylinderOut_IOOut))
                {
                    CloseRightBendClawCylinder();
                    Thread.Sleep(50);
                }

                if (!IsOnPosition(_Axis_RightBend_DWX, Recipe.RightBend_DWX_WorkPos))
                {
                    if (!AxisMoveTo(_Axis_RightBend_DWX, Recipe.RightBend_DWX_WorkPos))
                    {
                        _Axis_RightBend_DWX.StopSlowly();
                        return false;
                    }
                }

                if (!AxisMoveTo(_Axis_RightBend_DWY, Recipe.RightBend_DWY_WorkPos))
                {
                    _Axis_RightBend_DWY.StopSlowly();
                    return false;
                }



                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_stgY, _Axis_RightBend_CCDX },
                    new double[] { Recipe.RightBend_CCDPos_Y, Recipe.RightBend_CCDPos_X }))
                {
                    _Axis_RightBend_stgY.StopSlowly();
                    _Axis_RightBend_CCDX.StopSlowly();
                    return false;
                }
                return result;
            }
        }

        public bool Bend1_Go_FZ_Pos()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                CloseLeftBendClawCylinder();
                Thread.Sleep(50);
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_stgY, _Axis_LeftBend_DWX, _Axis_LeftBend_DWY }, new double[] { Recipe.LeftBend_Y_WorkPos, Recipe.LeftBend_DWX_WorkPos, Recipe.LeftBend_DWY_WorkPos }))
                {
                    _Axis_LeftBend_DWX.StopSlowly();
                    _Axis_LeftBend_stgY.StopSlowly();
                    _Axis_LeftBend_DWY.StopSlowly();
                    OutputError("左折弯工位平台运动到折弯位置失败!", true);
                    return false;
                }
                Thread.Sleep(100);

                if (!IsOnPosition(_Axis_LeftBend_DWR, Recipe.LeftBend_DWR_SafePos))
                {

                    if (!_Axis_LeftBend_DWR.MoveTo(Recipe.LeftBend_DWR_SafePos))
                    {
                        _Axis_LeftBend_DWR.StopSlowly();
                        return false;
                    }

                    if (!CheckAxisDone(_Axis_LeftBend_DWR))
                    {
                        return false;
                    }
                }

                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_CCDX, _Axis_LeftBend_stgY }, new double[] { Recipe.LeftBend_CCDPos_X, Recipe.LeftBend_CCDPos_Y }))
                {
                    _Axis_LeftBend_CCDX.StopSlowly();
                    _Axis_LeftBend_stgY.StopSlowly();
                    return false;
                }

                Thread.Sleep(100);

                //CCDWork
                {

                    OpenLeftBend_UPlightController();
                    Thread.Sleep(100);

                    double pos = SendBendDegreeMsg((int)StationType.Left);

                    if (Math.Abs(pos) > 5)
                    {
                        return false;
                    }


                    if (!AxisMove(_Axis_LeftBend_DWW, -Recipe.LeftBend_DWW_BasePos + pos))
                    {
                        return false;
                    }

                    if (!CheckAxisDone(_Axis_LeftBend_DWW))
                    {
                        return false;
                    }
                }


                Thread.Sleep(50);
                double[] posXY = SendAdjustXYMsg((int)StationType.Left);


                if (Math.Abs(posXY[0]) > 15 || Math.Abs(posXY[1]) > 15)
                {
                    OutputError("XY模板校正返回数值过大");
                    return false;
                }

                // return false;
                if (!_Axis_LeftBend_stgY.Move(posXY[1], Recipe.Bend1adjust_Speed)
                    || !_Axis_LeftBend_DWX.Move(posXY[0], Recipe.Bend1adjust_Speed)
                    || !_Axis_LeftBend_DWY.Move(posXY[1], Recipe.Bend1adjust_Speed)
                    || !_Axis_LeftBend_CCDX.Move(posXY[0], Recipe.Bend1adjust_Speed))
                {
                    _Axis_LeftBend_stgY.StopSlowly();
                    _Axis_LeftBend_DWX.StopSlowly();
                    _Axis_LeftBend_DWY.StopSlowly();
                    _Axis_LeftBend_CCDX.StopSlowly();
                    return false;
                }

                if (!CheckAxisDone(_Axis_LeftBend_stgY))
                {
                    return false;
                }


                if (!CheckAxisDone(_Axis_LeftBend_DWX))
                {
                    return false;
                }

                if (!CheckAxisDone(_Axis_LeftBend_DWY))
                {
                    return false;
                }

                if (!CheckAxisDone(_Axis_LeftBend_CCDX))
                {
                    return false;
                }

                Thread.Sleep(50);

                double pos_stgy = _Axis_LeftBend_stgY.PositionDev;


                OpenLeftBendClawCylinder();
                SendAdjustXYMsg((int)StationType.Left);
                Thread.Sleep(50);
                if (!AxisMoveTo(_Axis_LeftBend_stgY, Recipe.LeftBend_Y_WorkPos))
                {
                    return false;
                }
                Thread.Sleep(50);







                if (!Axis_LeftBend_DWY.MoveTo(Recipe.LeftBend_CCD_DWY + posXY[1], Recipe.LeftZB_Speed) ||
                                   !AxisMoveTo(_Axis_LeftBend_DWR, Recipe.LeftBend_DWR_WorkPos))
                {
                    _Axis_LeftBend_DWR.StopSlowly();
                    _Axis_LeftBend_DWY.StopSlowly();
                    _IsleftbendWorking = false;
                    return false;
                }

                if (!CheckAxisDone(_Axis_LeftBend_DWY))
                {
                    _IsleftbendWorking = false;
                    return false;
                }
                Thread.Sleep(50);

                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_stgY },
                                    new double[] { pos_stgy }))
                {

                    _Axis_LeftBend_stgY.StopSlowly();
                    _IsleftbendWorking = false;
                    return false;
                }

                Thread.Sleep(50);
                return result;
            }
        }

        public bool Bend2_Go_FZ_Pos()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                CloseMidBendClawCylinder();
                Thread.Sleep(50);
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_stgY, _Axis_MidBend_DWX, _Axis_MidBend_DWY }, new double[] { Recipe.MidBend_Y_WorkPos, Recipe.MidBend_DWX_WorkPos, Recipe.MidBend_DWY_WorkPos }))
                {
                    _Axis_MidBend_DWX.StopSlowly();
                    _Axis_MidBend_stgY.StopSlowly();
                    _Axis_MidBend_DWY.StopSlowly();
                    OutputError("中折弯工位平台运动到折弯位置失败!", true);
                    return false;
                }

                Thread.Sleep(100);

                if (!IsOnPosition(_Axis_MidBend_DWR, Recipe.MidBend_DWR_SafePos))
                {

                    if (!_Axis_MidBend_DWR.MoveTo(Recipe.MidBend_DWR_SafePos))
                    {
                        _Axis_MidBend_DWR.StopSlowly();
                        return false;
                    }

                    if (!CheckAxisDone(_Axis_MidBend_DWR))
                    {
                        return false;
                    }
                }

                Thread.Sleep(50);

                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_CCDX, _Axis_MidBend_stgY }, new double[] { Recipe.MidBend_CCDPos_X, Recipe.MidBend_CCDPos_Y }))
                {
                    _Axis_MidBend_CCDX.StopSlowly();
                    _Axis_MidBend_stgY.StopSlowly();
                    return false;
                }

                Thread.Sleep(100);


                //CCDWork
                {

                    OpenMidBend_UPlightController();
                    Thread.Sleep(100);

                    double pos = SendBendDegreeMsg((int)StationType.Mid);

                    if (Math.Abs(pos) > 5)
                    {
                        return false;
                    }


                    if (!AxisMove(_Axis_MidBend_DWW, -Recipe.MidBend_DWW_BasePos + pos))
                    {
                        return false;
                    }

                    if (!CheckAxisDone(_Axis_MidBend_DWW))
                    {
                        return false;
                    }
                }


                Thread.Sleep(50);
                double[] posXY = SendAdjustXYMsg((int)StationType.Mid);

                if (Math.Abs(posXY[0]) > 15 || Math.Abs(posXY[1]) > 15)
                {
                    OutputError("XY模板校正返回数值过大");
                    return false;
                }

                if (!_Axis_MidBend_stgY.Move(posXY[1], Recipe.Bend2adjust_Speed)
                    || !_Axis_MidBend_DWX.Move(posXY[0], Recipe.Bend2adjust_Speed)
                    || !_Axis_MidBend_DWY.Move(posXY[1], Recipe.Bend2adjust_Speed)
                    || !_Axis_MidBend_CCDX.Move(posXY[0], Recipe.Bend2adjust_Speed))
                {
                    _Axis_MidBend_stgY.StopSlowly();
                    _Axis_MidBend_DWX.StopSlowly();
                    _Axis_MidBend_DWY.StopSlowly();
                    _Axis_MidBend_CCDX.StopSlowly();
                    return false;
                }
                if (!CheckAxisDone(_Axis_MidBend_stgY))
                {
                    return false;
                }


                if (!CheckAxisDone(_Axis_MidBend_DWX))
                {
                    return false;
                }

                if (!CheckAxisDone(_Axis_MidBend_DWY))
                {
                    return false;
                }

                if (!CheckAxisDone(_Axis_MidBend_CCDX))
                {
                    return false;
                }

                Thread.Sleep(50);

                double pos_stgy = _Axis_MidBend_stgY.PositionDev;
                OpenMidBendClawCylinder();
                SendAdjustXYMsg((int)StationType.Mid);

                if (!AxisMoveTo(_Axis_MidBend_stgY, Recipe.MidBend_Y_WorkPos))
                {
                    return false;
                }
                Thread.Sleep(50);




                if (!Axis_MidBend_DWY.MoveTo(Recipe.MidBend_CCD_DWY, Recipe.MidZB_Speed) ||
                                   !AxisMoveTo(_Axis_MidBend_DWR, Recipe.MidBend_DWR_WorkPos))
                {
                    _Axis_MidBend_DWR.StopSlowly();
                    _Axis_MidBend_DWY.StopSlowly();
                    _IsmidbendWorking = false;
                    return false;
                }

                if (!CheckAxisDone(_Axis_MidBend_DWY))
                {
                    _IsmidbendWorking = false;
                    return false;
                }

                if (!CheckAxisDone(_Axis_MidBend_DWR))
                {
                    return false;
                }

                Thread.Sleep(50);
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_stgY },
                                    new double[] { pos_stgy }))
                {
                    _Axis_MidBend_stgY.StopSlowly();
                    return false;
                }
                return result;
            }
        }

        public bool Bend3_Go_FZ_Pos()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            else
            {
                CloseRightBendClawCylinder();
                Thread.Sleep(50);

                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_stgY, _Axis_RightBend_DWX, _Axis_RightBend_DWY }, new double[] { Recipe.RightBend_Y_WorkPos, Recipe.RightBend_DWX_WorkPos, Recipe.RightBend_DWY_WorkPos }))
                {
                    _Axis_RightBend_DWX.StopSlowly();
                    _Axis_RightBend_stgY.StopSlowly();
                    _Axis_RightBend_DWY.StopSlowly();
                    OutputError("右折弯工位平台运动到折弯位置失败!");
                    return false;
                }

                Thread.Sleep(100);

                if (!IsOnPosition(_Axis_RightBend_DWR, Recipe.RightBend_DWR_SafePos))
                {
                    if (!_Axis_RightBend_DWR.MoveTo(Recipe.RightBend_DWR_SafePos))
                    {
                        _Axis_RightBend_DWR.StopSlowly();
                        return false;
                    }

                    if (!CheckAxisDone(_Axis_RightBend_DWR))
                    {
                        return false;
                    }
                }


                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_CCDX, _Axis_RightBend_stgY }, new double[] { Recipe.RightBend_CCDPos_X, Recipe.RightBend_CCDPos_Y }))
                {
                    _Axis_RightBend_CCDX.StopSlowly();
                    _Axis_RightBend_stgY.StopSlowly();
                    return false;
                }

                Thread.Sleep(100);


                //CCDWork
                {

                    OpenRightBend_UPlightController();
                    Thread.Sleep(100);

                    double pos = SendBendDegreeMsg((int)StationType.Right);

                    if (Math.Abs(pos) > 5)
                    {
                        return false;
                    }


                    if (!AxisMove(_Axis_RightBend_DWW, -Recipe.RightBend_DWW_BasePos + pos))
                    {
                        return false;
                    }

                    if (!CheckAxisDone(_Axis_RightBend_DWW))
                    {
                        return false;
                    }
                }


                Thread.Sleep(50);
                double[] posXY = SendAdjustXYMsg((int)StationType.Right);

                if (Math.Abs(posXY[0]) > 15 || Math.Abs(posXY[1]) > 15)
                {
                    OutputError("XY模板校正返回数值过大");
                    return false;
                }

                if (!_Axis_RightBend_stgY.Move(posXY[1], Recipe.Bend3adjust_Speed)
                    || !_Axis_RightBend_DWX.Move(posXY[0], Recipe.Bend3adjust_Speed)
                    || !_Axis_RightBend_DWY.Move(posXY[1], Recipe.Bend3adjust_Speed)
                    || !_Axis_RightBend_CCDX.Move(posXY[0], Recipe.Bend3adjust_Speed))
                {
                    _Axis_RightBend_stgY.StopSlowly();
                    _Axis_RightBend_DWX.StopSlowly();
                    _Axis_RightBend_DWY.StopSlowly();
                    _Axis_RightBend_CCDX.StopSlowly();
                    return false;
                }

                if (!CheckAxisDone(_Axis_RightBend_stgY))
                {
                    return false;
                }


                if (!CheckAxisDone(_Axis_RightBend_DWX))
                {
                    return false;
                }

                if (!CheckAxisDone(_Axis_RightBend_DWY))
                {
                    return false;
                }

                if (!CheckAxisDone(_Axis_RightBend_CCDX))
                {
                    return false;
                }
                Thread.Sleep(50);

                double pos_stgy = _Axis_RightBend_stgY.PositionDev;
                OpenRightBendClawCylinder();
                SendAdjustXYMsg((int)StationType.Right);

                if (!AxisMoveTo(_Axis_RightBend_stgY, Recipe.RightBend_Y_WorkPos))
                {
                    return false;
                }
                Thread.Sleep(50);
                if (!Axis_RightBend_DWY.MoveTo(Recipe.RightBend_CCD_DWY, Recipe.RightZB_Speed) ||
                                   !AxisMoveTo(_Axis_RightBend_DWR, Recipe.RightBend_DWR_WorkPos))
                {
                    _Axis_RightBend_DWR.StopSlowly();
                    _Axis_RightBend_DWY.StopSlowly();
                    return false;
                }

                if (!CheckAxisDone(_Axis_RightBend_DWY))
                {
                    return false;
                }


                if (!CheckAxisDone(_Axis_RightBend_DWR))
                {
                    return false;
                }
                Thread.Sleep(50);
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_stgY },
                                    new double[] { pos_stgy }))
                {

                    _Axis_RightBend_stgY.StopSlowly();
                    return false;
                }
                return result;
            }
        }
        /// <summary>
        /// 重复拍照次数
        /// </summary>
        public int CheckNum = 5;
        private double[] SendAdjustXYMsg(int index)
        {
            double[] pos = { 11, 11 };
            try
            {
                if (index == 0)
                {
                    int count = 0;
                    bool check = true; ;

                    while (check)
                    {
                        count++;

                        str_leftbend_degree = " ";
                        str_Bend1Rev = "";
                        if (!SendMsg("A,PZXY"))
                        {
                            OutputError("折弯相机1发送拍照数据失败", true);
                            return pos;
                        }
                        else
                        {
                            Stopwatch stt = new Stopwatch();
                            stt.Restart();
                            //  OutputMessage("等待折弯相机1返回数据!");
                            while (str_Bend1Rev.Length < 2)
                            {
                                if (stt.ElapsedMilliseconds > NetTimeOut)
                                {
                                    BendCCDNet.StopConnection();
                                    BendCCDNet.Dispose();
                                    Thread.Sleep(200);
                                    BendCCDNet.StartConnection();
                                    OutputError("折弯相机1接收信息超时报警!");
                                    return pos;
                                }
                                if (_IsStop)
                                {
                                    return pos;
                                }
                                Thread.Sleep(20);
                            }
                        }
                        str_leftbend_degree = str_Bend1Rev;
                        OutputMessage(str_leftbend_degree);
                        pos = SplitAdjustXYString(str_leftbend_degree);
                        str_leftbend_degree = "";

                        if (count > CheckNum)
                        {
                            return pos;
                        }
                        else if (Math.Abs(pos[0]) > 10 || Math.Abs(pos[1]) > 10)
                        {
                            continue;
                        }

                        check = false;
                    }
                }
                else if (index == 1)
                {
                    int count = 0;
                    bool check = true; ;

                    while (check)
                    {
                        count++;
                        str_midbend_degree = " ";

                        str_Bend2Rev = "";
                        if (!Bend2SendMsg("B,PZXY"))
                        {
                            OutputError("折弯相机2发送拍照数据失败", true);
                            return pos;
                        }
                        else
                        {
                            Stopwatch stt = new Stopwatch();
                            stt.Restart();
                            //  OutputMessage("等待折弯相机2返回数据!");
                            while (str_Bend2Rev.Length < 2)
                            {
                                if (stt.ElapsedMilliseconds > NetTimeOut)
                                {
                                    Bend2CCDNet.StopConnection();
                                    Bend2CCDNet.Dispose();
                                    Thread.Sleep(200);
                                    Bend2CCDNet.StartConnection();
                                    OutputError("折弯相机2接收信息超时报警!");
                                    return pos;
                                }
                                if (_IsStop)
                                {
                                    return pos;
                                }
                                Thread.Sleep(20);
                            }
                        }
                        str_midbend_degree = str_Bend2Rev;
                        pos = SplitAdjustXYString(str_midbend_degree);
                        str_midbend_degree = " ";

                        if (count > CheckNum)
                        {
                            return pos;
                        }
                        else if (Math.Abs(pos[0]) > 10 || Math.Abs(pos[1]) > 10)
                        {

                            continue;
                        }

                        check = false;
                    }

                }
                else
                {
                    int count = 0;
                    bool check = true; ;

                    while (check)
                    {
                        count++;
                        str_rightbend_degree = " ";

                        str_Bend3Rev = "";
                        if (!Bend3SendMsg("C,PZXY"))
                        {

                            OutputError("折弯相机3发送拍照数据失败");
                            return pos;
                        }
                        else
                        {
                            Stopwatch stt = new Stopwatch();
                            stt.Restart();
                            //   OutputMessage("等待折弯相机3返回数据!");
                            while (str_Bend3Rev.Length < 2)
                            {
                                if (stt.ElapsedMilliseconds > NetTimeOut)
                                {
                                    Bend3CCDNet.StopConnection();
                                    Bend3CCDNet.Dispose();
                                    Thread.Sleep(200);
                                    Bend3CCDNet.StartConnection();
                                    OutputError("折弯相机3接收信息超时报警!");
                                    return pos;
                                }
                                if (_IsStop)
                                {
                                    return pos;
                                }
                                Thread.Sleep(20);
                            }
                        }
                        pos = SplitAdjustXYString(str_Bend3Rev);
                        str_rightbend_degree = "";
                        if (count > CheckNum)
                        {
                            return pos;
                        }
                        else if (Math.Abs(pos[0]) > 10 || Math.Abs(pos[1]) > 10)
                        {

                            continue;
                        }

                        check = false;
                    }
                }
                return pos;
            }
            catch (Exception ex)
            {
                WriteErrLog("PZXY" + ex.ToString());
                OutputError("XY模板校正出现较严重的错误!!!!");
                return pos;
            }
        }

        private double SendBendDegreeMsg(int index)
        {
            double pos = 0;
            if (index == 0)
            {
                str_leftbend_degree = " ";

                str_Bend1Rev = "";
                if (!SendMsg("A,PZF"))
                {

                    OutputError("折弯相机1发送拍照数据失败", true);
                    return pos;
                }
                else
                {
                    Stopwatch stt = new Stopwatch();
                    stt.Restart();
                    //   OutputMessage("等待折弯相机1返回数据!");
                    while (str_Bend1Rev.Length < 2)
                    {
                        if (stt.ElapsedMilliseconds > NetTimeOut)
                        {
                            BendCCDNet.StopConnection();
                            BendCCDNet.Dispose();
                            Thread.Sleep(200);
                            BendCCDNet.StartConnection();
                            OutputError("折弯相机1接收信息超时报警!");
                            return pos;
                        }
                        if (_IsStop)
                        {
                            return pos;
                        }
                        Thread.Sleep(20);
                    }
                }
                str_leftbend_degree = str_Bend1Rev;
                pos = SplitDegreeString(str_leftbend_degree);
                str_leftbend_degree = "";
            }
            else if (index == 1)
            {
                str_midbend_degree = " ";

                str_Bend2Rev = "";
                if (!Bend2SendMsg("B,PZF"))
                {

                    OutputError("折弯相机2发送拍照数据失败");
                    return pos;
                }
                else
                {
                    Stopwatch stt = new Stopwatch();
                    stt.Restart();
                    //   OutputMessage("等待折弯相机2返回数据!");
                    while (str_Bend2Rev.Length < 2)
                    {
                        if (stt.ElapsedMilliseconds > NetTimeOut)
                        {
                            Bend2CCDNet.StopConnection();
                            Bend2CCDNet.Dispose();
                            Thread.Sleep(200);
                            Bend2CCDNet.StartConnection();
                            OutputError("折弯相机2接收信息超时报警!");
                            return pos;
                        }
                        if (_IsStop)
                        {
                            return pos;
                        }
                        Thread.Sleep(20);
                    }
                }
                str_midbend_degree = str_Bend2Rev;
                pos = SplitDegreeString(str_midbend_degree);
                str_midbend_degree = " ";
            }
            else
            {
                str_rightbend_degree = " ";

                str_Bend3Rev = "";
                if (!Bend3SendMsg("C,PZF"))
                {
                    OutputError("折弯相机3发送拍照数据失败");
                    return pos;
                }
                else
                {
                    Stopwatch stt = new Stopwatch();
                    stt.Restart();
                    // OutputMessage("等待折弯相机3返回数据!");
                    while (str_Bend3Rev.Length < 2)
                    {
                        if (stt.ElapsedMilliseconds > NetTimeOut)
                        {
                            OutputError("折弯相机3接收信息超时报警!");
                            return pos;
                        }
                        if (_IsStop)
                        {
                            return pos;
                        }
                        Thread.Sleep(20);
                    }
                }
                str_rightbend_degree = str_Bend3Rev;
                pos = SplitDegreeString(str_rightbend_degree);
                str_rightbend_degree = "";
            }
            return pos;
        }

        private double[] SendBendYMsg(int index)
        {
            double[] pos = new double[] { -100, -100 };
            if (index == 0)
            {
                str_Bend1Rev = "";
                if (!SendMsg("A,PZS"))//PZS
                {
                    OutputError("折弯相机1发送拍照数据失败");
                    return pos;
                }
                else
                {
                    //   OutputMessage("等待折弯相机1返回数据!");
                    while (str_Bend1Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            return pos;
                        }
                        Thread.Sleep(20);
                    }
                }
                pos = SplitYString(str_Bend1Rev);
                return pos;
            }
            else if (index == 1)
            {
                str_Bend2Rev = "";
                if (!Bend2SendMsg("B,PZS"))//PZS
                {
                    OutputError("折弯相机2发送拍照数据失败");
                    return pos;
                }
                else
                {
                    //OutputMessage("等待折弯相机2返回数据!");
                    while (str_Bend2Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            return pos;
                        }
                        Thread.Sleep(20);
                    }
                }
                pos = Split2YString(str_Bend2Rev);
                return pos;
            }
            else
            {
                str_Bend3Rev = "";
                if (!Bend3SendMsg("C,PZS"))//PZS
                {
                    OutputError("折弯相机3发送拍照数据失败");
                    return pos;
                }
                else
                {
                    //   OutputMessage("等待折弯相机3返回数据!");
                    while (str_Bend3Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            return pos;
                        }
                        Thread.Sleep(20);
                    }
                }
                pos = Split3YString(str_Bend3Rev);
                return pos;
            }
        }

        public bool LeftBendStation_PullMaterial()
        {

            bool result = true;
            if (!CheckRun())
            {
                return false;
            }

            //if ((!IsTransferSuck || !IsTransferFPCSuck))
            //{
            //    AlarmWork();
            //    OutputError("中转真空吸报警");
            //    return false;
            //}
            //else if (!IsLeftBend_PressCylinder_UP)
            //{
            //    OpenLeftBend_PressCylinder();
            //    Thread.Sleep(50);
            //}

            if (!CheckRun())
            {
                return false;
            }

            if (!CheckRun())
            {
                return false;
            }

            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                return false;
            }





            if (!IsOnPosition(new MeasurementAxis[] { _Axis_LeftBend_DWX, _Axis_LeftBend_DWY }, new double[] { Recipe.LeftBend_DWX_SafePos, Recipe.LeftBend_DWY_WorkPos }))
            {
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_DWX, _Axis_LeftBend_DWY }, new double[] { Recipe.LeftBend_DWX_SafePos, Recipe.LeftBend_DWY_WorkPos }))
                {
                    AlarmWork();
                    OutputError("对位X Y轴运动到安全位置失败！");
                    return false;
                }
            }


            if (!IsOnPosition(_Axis_LeftBend_DWR, Recipe.LeftBend_DWR_SafePos))
            {
                if (!IsOnPosition(_Axis_LeftBend_stgY, Recipe.LeftBend_Y_WorkPos))
                {
                    if (!AxisMoveTo(_Axis_LeftBend_stgY, Recipe.LeftBend_Y_WorkPos))
                    {
                        return false;
                    }
                }

                if (!AxisMoveTo(_Axis_LeftBend_DWR, Recipe.LeftBend_DWR_SafePos))
                {
                    AlarmWork();
                    OutputError("对位R轴运动到安全位置失败！");
                    return false;
                }
            }






            //运动到上料位置
            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_Transfer_X, _Axis_LeftBend_stgY, _Axis_LeftBend_DWW },
                new double[] { Recipe.LeftBend_LoadX, Recipe.LeftBend_LoadY, Recipe.LeftBend_DWW_WorkPos }))
            {
                AlarmWork();
                OutputError("ErrTransferX And LeftbendY");
                return false;
            }
            Thread.Sleep(100);

            //开始上料                        
            if (!TransferZWork(1, Recipe.LeftBend_LoadZ, true))
            {
                return false;
            }
            Thread.Sleep(200);
            OpenLeftBendSuck();
            CloseTranserSuck();
            CloseTransferFPCSuck();
            OpenTransferBlow();
            Thread.Sleep(Config.RobotBlowDelay);
            if (!AxisMoveTo(_Axis_LeftBend_DWX, Recipe.LeftBend_DWX_WorkPos))
            {
                _Axis_LeftBend_DWX.StopSlowly();
                return false;
            }


            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                AlarmWork();
                OutputError("中转上下气缸动作失败！");
                return false;
            }

            CloseTransferBlow();
            if (!AxisMoveTo(_Axis_Transfer_X, Recipe.TransferXSafePos))
            {
                return false;
            }
            return result;
        }

        public bool LeftBendStation_BendWork()
        {

            bool result = true;
            _IsStop = false;

            if (!CheckRun())
            {
                return false;
            }

            if (!AxisMoveTo(_Axis_LeftBend_DWY, Recipe.LeftBend_DWY_WorkPos))
            {
                _Axis_LeftBend_DWY.StopSlowly();
                AlarmWork();
                OutputError("对位Y轴运动到安全位置失败！");
                return false;
            }

            if (!AxisMoveTo(_Axis_LeftBend_DWX, Recipe.LeftBend_DWX_WorkPos))
            {
                _Axis_LeftBend_DWX.StopSlowly();
                AlarmWork();
                OutputError("Err LeftBend AxisDWX");
                return false;
            }

            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_CCDX, _Axis_LeftBend_stgY }, new double[] { Recipe.LeftBend_CCDPos_X, Recipe.LeftBend_CCDPos_Y }))
            {
                _Axis_LeftBend_CCDX.StopSlowly();
                _Axis_LeftBend_stgY.StopSlowly();
                return false;
            }
            Thread.Sleep(100);
            //CCDWork
            {

                OpenLeftBend_UPlightController();
                Thread.Sleep(50);

                double pos = SendBendDegreeMsg((int)StationType.Left);



                if (Math.Abs(pos) > 5)
                {
                    return false;
                }
                if (!AxisMove(_Axis_LeftBend_DWW, -Recipe.LeftBend_DWW_BasePos + pos))
                {
                    return false;
                }
            }

            double[] posXY = SendAdjustXYMsg((int)StationType.Left);

            if (Math.Abs(posXY[0]) > 15 || Math.Abs(posXY[1]) > 15)
            {
                OutputError("XY模板校正返回数值过大");
                return false;
            }

            if (!_Axis_LeftBend_stgY.Move(posXY[1], Recipe.Bend1adjust_Speed)
                || !_Axis_LeftBend_DWX.Move(posXY[0], Recipe.Bend1adjust_Speed)
                || !_Axis_LeftBend_DWY.Move(posXY[1], Recipe.Bend1adjust_Speed)
                || !_Axis_LeftBend_CCDX.Move(posXY[0], Recipe.Bend1adjust_Speed))
            {
                _Axis_LeftBend_stgY.StopSlowly();
                _Axis_LeftBend_DWX.StopSlowly();
                _Axis_LeftBend_DWY.StopSlowly();
                _Axis_LeftBend_CCDX.StopSlowly();
                return false;
            }

            if (!CheckAxisDone(_Axis_LeftBend_stgY))
            {
                return false;
            }


            if (!CheckAxisDone(_Axis_LeftBend_DWX))
            {
                return false;
            }

            if (!CheckAxisDone(_Axis_LeftBend_DWY))
            {
                return false;
            }

            if (!CheckAxisDone(_Axis_LeftBend_CCDX))
            {
                return false;
            }
            Thread.Sleep(120);

            double pos_stgy = _Axis_LeftBend_stgY.PositionDev;

            double offsetStageY = pos_stgy - Recipe.LeftBend_CCDPos_Y;


            OpenLeftBendClawCylinder();
            SendAdjustXYMsg((int)StationType.Left);


            //折弯动作

            if (!AxisMoveTo(_Axis_LeftBend_stgY, Recipe.LeftBend_Y_WorkPos))
            {
                _Axis_LeftBend_stgY.StopSlowly();
                return false;
            }


            if (!RunBend_AxisRWrok(StationType.Left))
            {
                return false;
            }
            Thread.Sleep(50);

            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_stgY }, new double[] { Recipe.LeftBend_CCDPos_Y2 + offsetStageY }))
            {
                _Axis_LeftBend_stgY.StopSlowly();
                return false;
            }
            //CCDWORK 矫正折弯
            {
                //OpenLeftBend_UPlightController();
                Thread.Sleep(50);
                for (int i = 0; i <= Recipe.BendPara[0].Adj_Num; i++)
                {
                    double[] pos = SendBendYMsg((int)StationType.Left);
                    while (!_IsManualRun)
                    {
                        Thread.Sleep(50);
                    }

                    Thread.Sleep(1000);


                    if (Math.Abs(pos[0] - Recipe.BendPara[0].BaseRate) > 10 || Math.Abs(pos[1] - Recipe.BendPara[1].BaseRate) > 10)
                    {
                        OutputError("视觉返回数据过大");
                        return false;
                    }

                    if (Math.Abs(pos[0] - Recipe.BendPara[0].BaseRate) > Recipe.BendPara[0].ErrAnd
                        || Math.Abs(pos[1] - Recipe.BendPara[1].BaseRate) > Recipe.BendPara[1].ErrAnd)
                    {
                        double[] poses = Calculate_DeltXY(pos, (int)StationType.Left);
                        if (!_Axis_LeftBend_DWX.Move(poses[0] * Recipe.BendPara[0].DirValue) || !_Axis_LeftBend_DWY.Move(poses[1] * Recipe.BendPara[1].DirValue))
                        {
                            _Axis_LeftBend_DWX.StopSlowly();
                            _Axis_LeftBend_DWY.StopSlowly();
                            return false;
                        }

                        if (!CheckAxisDone(_Axis_LeftBend_DWX))
                        {
                            return false;
                        }

                        if (!CheckAxisDone(_Axis_LeftBend_DWY))
                        {
                            return false;
                        }

                    }
                    else
                    {
                        break;
                    }
                    if (i == Recipe.BendPara[0].Adj_Num)
                    {
                        return false;
                    }
                }

            }

            //运行到压合位置进行压合
            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_CCDX, _Axis_LeftBend_stgY }, new double[] { Recipe.LeftBend_PressPt_X, Recipe.LeftBend_PressPt_Y }))
            {
                return false;
            }

            OpenLeftBend_PressCylinder();
            if (!WaitIOMSec(Config.LeftBend_PressCylinder_DownIOIn, 3000, true))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "左折弯压合气缸下感应位报警！";
                fra.ShowDialog();
            }
            Thread.Sleep((int)Recipe.LeftYB_Time);
            CloseLeftBend_PressCylinder();

            if (!WaitIOMSec(Config.LeftBend_PressCylinder_UPIOIn, 2000, true))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "左折弯压合气缸上感应位报警！";
                fra.ShowDialog();
            }
            Thread.Sleep(200);
            if (Config.IsLeftBendAOIDisabled)
            {
                // return false;
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_stgY, _Axis_LeftBend_CCDX }, new double[] { Recipe.LeftBend_CCDPos_Y + posXY[1], Recipe.LeftBend_CCDPos_X + posXY[0] }))
                {
                    return false;
                }
                Thread.Sleep(200);

                OpenLeftBend_UPlightController();
                Thread.Sleep(50);
                double[] pos = SendBendAOIMsg((int)StationType.Left);
                if (Math.Abs(pos[1] - Recipe.AOIY1) > Recipe.AOIY1Offset
                    || Math.Abs(pos[3] - Recipe.AOIY2) > Recipe.AOIY2Offset
                    || Math.Abs(pos[0] - Recipe.AOIX1) > Recipe.AOIX1Offset
                    || Math.Abs(pos[2] - Recipe.AOIX2) > Recipe.AOIX2Offset)
                {
                    OutputError("左反折AOI检测NG");
                    AlarmWork();
                    result = false;
                }
            }
            CloseLeftBend_UPlightController();
            CloseLeftBendClawCylinder();
            Thread.Sleep(100);



            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_DWX, _Axis_LeftBend_stgY, _Axis_LeftBend_DWY }, new double[] { Recipe.LeftBend_DWX_SafePos, Recipe.LeftBendR_Ypos, Recipe.LeftBend_DWY_SafePos }))
            {
                _Axis_LeftBend_stgY.StopSlowly();
                _Axis_LeftBend_DWX.StopSlowly();
                return false;
            }

            if (!AxisMoveTo(_Axis_LeftBend_DWR, Recipe.LeftBend_DWR_SafePos))
            {
                _Axis_LeftBend_DWR.StopSlowly();
                return false;
            }
            return result;
        }

        public bool LeftBendStation_PickMaterial()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            if (Config.IsDischargeCylinderEnable)
            {
                DischargeRotateUp();
            }


            if (Config.DischargeAxiaZCylinderEnable && !IsDischargeAxisZCylinder_UP)
            {
                if (DischargeAxisZCylinderUp() == -2) return false;
            }

            if (!IsOnPosition(_Axis_Discharge_Z, Recipe.DischargeZSafePos))
            {
                if (!AxisMoveTo(Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                {
                    return false;
                }
            }

            if (!IsOnPosition(_Axis_LeftBend_DWR, Recipe.LeftBend_DWR_SafePos))
            {
                if (!IsOnPosition(_Axis_LeftBend_stgY, Recipe.LeftBend_Y_WorkPos))
                {
                    if (!AxisMoveTo(_Axis_LeftBend_stgY, Recipe.LeftBend_Y_WorkPos))
                    {
                        return false;
                    }
                }

                if (!AxisMoveTo(_Axis_LeftBend_DWR, Recipe.LeftBend_DWR_SafePos))
                {
                    return false;
                }
            }

            if (!IsOnPosition(_Axis_LeftBend_DWX, Recipe.LeftBend_DWX_SafePos))
            {
                if (!AxisMoveTo(_Axis_LeftBend_DWX, Recipe.LeftBend_DWX_SafePos))
                {
                    return false;
                }
            }

            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_stgY, _Axis_Discharge_X }, new double[] { Recipe.LeftBend_Discharge_Y, Recipe.LeftBend_Discharge_x }))
            {
                _Axis_LeftBend_stgY.StopSlowly();
                _Axis_Discharge_X.StopSlowly();
                return false;
            }

            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.LeftBend_Discharge_Z))
            {
                return false;
            }






            CloseLeftBendSuck();
            OpenDischargeSuck();
            OpenLeftBendBlow();

            Thread.Sleep(Config.StageBlowDelay);
            CloseLeftBendBlow();



            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
            {
                AlarmWork();
                OutputError("Err axisDischargeZ Safe");
                return false;
            }
            Thread.Sleep(100);

            if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeX_OK_PullPos))
            {
                _Axis_Discharge_X.StopSlowly();
                return false;
            }

            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZ_OK_PullPos))
            {
                _Axis_Discharge_Z.StopSlowly();
                return false;
            }

            if (Config.IsDischargeCylinderEnable)
            {
                OpenDischargeRotateSuck();
            }

            if (Config.DischargeAxiaZCylinderEnable)
            {
                if (DischargeAxisZCylinderDropAction() == -2) return false;
            }
            else
            {
                CloseDischargeSuck();
                OpenDischargeBlow();
                Thread.Sleep(Config.RobotBlowDelay);
                CloseDischargeBlow();
            }


            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
            {
                _Axis_Discharge_Z.StopSlowly();
                return false;
            }

            if (Config.IsDischargeCylinderEnable)
            {
                DischargeRotateDrop();
            }




            return result;
        }

        public bool MidBendStation_PullMaterial()
        {
            bool result = true;
            //检查Z轴是否安全
            if (!CheckRun())
            {
                return false;
            }

            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                OutputError("中转上下气缸动作失败！");
                return false;
            }



            if (!IsOnPosition(_Axis_MidBend_DWY, Recipe.MidBend_DWY_WorkPos))
            {
                if (!AxisMoveTo(_Axis_MidBend_DWY, Recipe.MidBend_DWY_WorkPos))
                {
                    AlarmWork();
                    OutputError("对位Y轴运动到安全位置失败！", true);
                    return false;
                }
            }


            if (!IsOnPosition(_Axis_MidBend_DWR, Recipe.MidBend_DWR_SafePos))
            {
                if (!IsOnPosition(_Axis_MidBend_stgY, Recipe.MidBend_Y_WorkPos))
                {
                    if (!AxisMoveTo(_Axis_MidBend_stgY, Recipe.MidBend_Y_WorkPos))
                    {
                        return false;
                    }
                }


                if (!AxisMoveTo(_Axis_MidBend_DWR, Recipe.MidBend_DWR_SafePos))
                {
                    AlarmWork();
                    OutputError("对位R轴运动到安全位置失败！", true);
                    return false;
                }
            }


            if (!IsOnPosition(_Axis_MidBend_DWX, Recipe.MidBend_DWX_SafePos))
            {
                if (!AxisMoveTo(_Axis_MidBend_DWX, Recipe.MidBend_DWX_SafePos))
                {
                    AlarmWork();
                    OutputError("对位X轴运动到安全位置失败！", true);
                    return false;
                }
            }

            //运动到上料位置
            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_Transfer_X, _Axis_MidBend_stgY, _Axis_MidBend_DWW },
                new double[] { Recipe.MidBend_LoadX, Recipe.MidBend_LoadY, Recipe.MidBend_DWW_WorkPos }))
            {
                AlarmWork();
                OutputError("ErrTransferX And MidbendstgY MidBendDWW", true);
                return false;
            }
            Thread.Sleep(100);


            if (!TransferZWork(1, Recipe.MidBend_LoadZ, true))
            {
                OutputError("中转Z动作失败！");
                return false;
            }
            Thread.Sleep(200);
            OpenMidBendSuck();
            CloseTranserSuck();
            CloseTransferFPCSuck();
            OpenTransferBlow();
            Thread.Sleep(Config.RobotBlowDelay);
            CloseTransferBlow();

            if (!AxisMoveTo(_Axis_MidBend_DWX, Recipe.MidBend_DWX_WorkPos))
            {
                _Axis_MidBend_DWX.StopSlowly();
                return false;
            }


            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                OutputError("中转Z动作失败！");
                return false;
            }



            if (!AxisMoveTo(_Axis_Transfer_X, Recipe.TransferXSafePos))
            {
                _Axis_Transfer_X.StopSlowly();
                return false;
            }


            return result;
        }

        public bool MidBendStation_BendWork()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            if (!IsOnPosition(_Axis_MidBend_DWY, Recipe.MidBend_DWY_WorkPos))
            {
                if (!AxisMoveTo(_Axis_MidBend_DWY, Recipe.MidBend_DWY_WorkPos))
                {
                    _Axis_MidBend_DWY.StopSlowly();
                    AlarmWork();
                    OutputError("对位Y轴运动到安全位置失败！");
                    return false;
                }
            }

            if (!IsOnPosition(_Axis_MidBend_DWX, Recipe.MidBend_DWX_WorkPos))
            {
                if (!AxisMoveTo(_Axis_MidBend_DWX, Recipe.MidBend_DWX_WorkPos))
                {
                    _Axis_MidBend_DWX.StopSlowly();
                    AlarmWork();
                    OutputError("Err MidBend AxisDWX");
                    return false;
                }
            }


            Thread.Sleep(50);
            // OpenMidBendClawCylinder();
            Thread.Sleep(50);

            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_CCDX, _Axis_MidBend_stgY }, new double[] { Recipe.MidBend_CCDPos_X, Recipe.MidBend_CCDPos_Y }))
            {
                _Axis_MidBend_CCDX.StopSlowly();
                _Axis_MidBend_stgY.StopSlowly();

                return false;
            }

            Thread.Sleep(100);

            //CCDWork
            {
                OpenMidBend_UPlightController();
                Thread.Sleep(50);
                double pos = SendBendDegreeMsg((int)StationType.Mid);

                if (Math.Abs(pos) > 5)
                {
                    OutputError("角度处理数据过大!");
                    return false;
                }
                // CloseMidBend_UPlightController();
                if (!AxisMove(_Axis_MidBend_DWW, -Recipe.MidBend_DWW_BasePos + pos))
                {
                    return false;
                }
            }

            double[] posXY = SendAdjustXYMsg((int)StationType.Mid);

            if (Math.Abs(posXY[0]) > 15 || Math.Abs(posXY[1]) > 15)
            {
                OutputError("XY模板校正返回数值过大");
                return false;
            }

            if (!_Axis_MidBend_stgY.Move(posXY[1], Recipe.Bend1adjust_Speed)
                || !_Axis_MidBend_DWX.Move(posXY[0], Recipe.Bend1adjust_Speed)
                || !_Axis_MidBend_DWY.Move(posXY[1], Recipe.Bend1adjust_Speed)
                || !_Axis_MidBend_CCDX.Move(posXY[0], Recipe.Bend1adjust_Speed))
            {
                _Axis_MidBend_stgY.StopSlowly();
                _Axis_MidBend_DWX.StopSlowly();
                _Axis_MidBend_DWY.StopSlowly();
                _Axis_MidBend_CCDX.StopSlowly();
                return false;
            }

            if (!CheckAxisDone(_Axis_MidBend_stgY))
            {
                return false;
            }


            if (!CheckAxisDone(_Axis_MidBend_DWX))
            {
                return false;
            }

            if (!CheckAxisDone(_Axis_MidBend_DWY))
            {
                return false;
            }

            if (!CheckAxisDone(_Axis_MidBend_CCDX))
            {
                return false;
            }
            Thread.Sleep(50);

            double pos_stgy = _Axis_MidBend_stgY.PositionDev;
            double offsetStageY = pos_stgy - Recipe.MidBend_CCDPos_Y;

            OpenMidBendClawCylinder();
            Thread.Sleep(50);
            SendAdjustXYMsg((int)StationType.Mid);

            //折弯动作

            if (!AxisMoveTo(_Axis_MidBend_stgY, Recipe.MidBend_Y_WorkPos))
            {
                _Axis_MidBend_stgY.StopSlowly();
                return false;
            }


            if (!RunBend_AxisRWrok(StationType.Mid))
            {
                return false;
            }

            Thread.Sleep(50);

            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_stgY }, new double[] { offsetStageY + Recipe.MidBend_CCDPos_Y2 }))
            {
                _Axis_MidBend_CCDX.StopSlowly();
                _Axis_MidBend_stgY.StopSlowly();
                return false;
            }

            Thread.Sleep(100);

            //CCDWork
            {
                OpenMidBend_UPlightController();
                Thread.Sleep(50);
                for (int i = 0; i <= Recipe.BendPara[2].Adj_Num; i++)
                {
                    double[] pos = SendBendYMsg((int)StationType.Mid);
                    while (!IsManualRun)
                    {
                        Thread.Sleep(50);
                    }

                    Thread.Sleep(1000);



                    if (Math.Abs(pos[0] - Recipe.BendPara[2].BaseRate) > 10
                    || Math.Abs(pos[1] - Recipe.BendPara[3].BaseRate) > 10)//  Recipe.BendPara[2].BaseRate * 0.5    Recipe.BendPara[3].BaseRate * 0.5
                    {
                        AlarmWork();

                        OutputError("视觉处理返回数据过大");
                        return false;
                    }


                    if (Math.Abs(pos[0] - Recipe.BendPara[2].BaseRate) > Recipe.BendPara[2].ErrAnd
                            || Math.Abs(pos[1] - Recipe.BendPara[3].BaseRate) > Recipe.BendPara[3].ErrAnd)
                    {
                        double[] poses = Calculate_DeltXY(pos, (int)StationType.Mid);

                        if (!_Axis_MidBend_DWX.Move(poses[0] * Recipe.BendPara[2].DirValue) || !_Axis_MidBend_DWY.Move(poses[1] * Recipe.BendPara[3].DirValue))
                        {
                            _Axis_MidBend_DWX.StopSlowly();
                            _Axis_MidBend_DWY.StopSlowly();
                            return false;
                        }

                        if (!CheckAxisDone(_Axis_MidBend_DWX))
                        {
                            return false;
                        }

                        if (!CheckAxisDone(_Axis_MidBend_DWY))
                        {
                            return false;
                        }

                    }
                    else
                    {
                        break;
                    }
                    if (i == Recipe.BendPara[2].Adj_Num)
                    {
                        return false;
                    }
                }
                CloseMidBend_UPlightController();
            }

            //折弯动作

            //CCDWORK 矫正折弯
            {

            }

            //运行到压合位置进行压合
            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_CCDX, _Axis_MidBend_stgY }, new double[] { Recipe.MidBend_PressPt_X, Recipe.MidBend_PressPt_Y }))
            {
                return false;
            }



            OpenMidBend_PressCylinder();
            if (!WaitIOMSec(Config.MidBend_PressCylinder_DownIOIn, 3000, true))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "中折弯压合气缸下感应位报警！";
                fra.ShowDialog();
            }
            Thread.Sleep((int)Recipe.MidYB_Time);
            CloseMidBend_PressCylinder();

            if (!WaitIOMSec(Config.MidBend_PressCylinder_UPIOIn, 3000, true))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "中折弯压合气缸上感应位报警！";
                fra.ShowDialog();
            }
            Thread.Sleep(200);




            //OpenLeftBend_PressCylinder();
            //Thread.Sleep(200);
            //CloseLeftBend_PressCylinder();





            OpenMidBend_UPlightController();
            //{
            //    Thread.Sleep(100);
            //    double[] pos = SendBendYMsg((int)StationType.Mid);
            //    if (Math.Abs(pos[0] - Recipe.BendPara[2].AOIBase) > Recipe.BendPara[2].AOIOffset
            //        || Math.Abs(pos[1] - Recipe.BendPara[3].AOIBase) > Recipe.BendPara[3].AOIOffset)
            //    {
            //        OpenBuzzer();
            //        OpenRedLight();
            //        result = false;
            //    }
            //}
            if (Config.IsMidBendAOIDisabled)
            {
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_stgY, _Axis_MidBend_CCDX }, new double[] { Recipe.MidBend_CCDPos_Y + posXY[1], Recipe.MidBend_CCDPos_X + posXY[0] }))
                {
                    return false;
                }
                Thread.Sleep(200);
                double[] pos = SendBendAOIMsg((int)StationType.Mid);
                if (Math.Abs(pos[1] - Recipe.MidAOIY1) > Recipe.MidAOIY1Offset
                    || Math.Abs(pos[3] - Recipe.MidAOIY2) > Recipe.MidAOIY2Offset
                    || Math.Abs(pos[0] - Recipe.MidAOIX1) > Recipe.MidAOIX1Offset
                    || Math.Abs(pos[2] - Recipe.MidAOIX2) > Recipe.MidAOIX2Offset)
                {
                    OutputError("中反折AOI检测NG");
                    AlarmWork();
                    result = false;
                }
            }
            CloseMidBend_UPlightController();



            CloseMidBendClawCylinder();
            Thread.Sleep(100);
            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_DWX, _Axis_MidBend_stgY, _Axis_MidBend_DWY }, new double[] { Recipe.MidBend_DWX_SafePos, Recipe.MidBendR_Ypos, Recipe.MidBend_DWY_SafePos }))
            {
                _Axis_MidBend_stgY.StopSlowly();
                _Axis_MidBend_DWX.StopSlowly();
                return false;
            }

            if (!AxisMoveTo(_Axis_MidBend_DWR, Recipe.MidBend_DWR_SafePos))
            {
                _Axis_MidBend_DWR.StopSlowly();
                return false;
            }
            return result;
        }

        public bool MidBendStation_PickMaterial()
        {

            bool result = true;
            if (!CheckRun())
            {
                return false;
            }

            if (Config.IsDischargeCylinderEnable)
            {
                DischargeRotateUp();
            }

            if (Config.DischargeAxiaZCylinderEnable && !IsDischargeAxisZCylinder_UP)
            {
                if (DischargeAxisZCylinderUp() == -2) return false;
            }



            if (!IsOnPosition(_Axis_Discharge_Z, Recipe.DischargeZSafePos))
            {
                if (!AxisMoveTo(Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                {
                    return false;
                }
            }

            if (!IsOnPosition(_Axis_MidBend_DWR, Recipe.MidBend_DWR_SafePos))
            {
                if (!IsOnPosition(_Axis_MidBend_stgY, Recipe.MidBend_Y_WorkPos))
                {
                    if (!AxisMoveTo(_Axis_MidBend_stgY, Recipe.MidBend_Y_WorkPos))
                    {
                        return false;
                    }
                }

                if (!AxisMoveTo(_Axis_MidBend_DWR, Recipe.MidBend_DWR_SafePos))
                {
                    return false;
                }
            }

            if (!IsOnPosition(_Axis_MidBend_DWX, Recipe.MidBend_DWX_SafePos))
            {
                if (!AxisMoveTo(_Axis_MidBend_DWX, Recipe.MidBend_DWX_SafePos))
                {
                    return false;
                }
            }


            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_stgY, _Axis_Discharge_X }, new double[] { Recipe.MidBend_Discharge_Y, Recipe.MidBend_Discharge_X }))
            {
                _Axis_MidBend_stgY.StopSlowly();
                _Axis_Discharge_X.StopSlowly();
                return false;
            }

            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.MidBend_Discharge_Z))
            {
                return false;
            }

            CloseMidBendSuck();
            OpenDischargeSuck();
            OpenMidBendBlow();

            Thread.Sleep(Config.StageBlowDelay);
            CloseMidBendBlow();
            //if (IsDischargeSuck)
            //{
            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
            {
                AlarmWork();
                OutputError("Err axisDischargeZ Safe");
                return false;
            }
            //}
            Thread.Sleep(100);

            if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeX_OK_PullPos))
            {
                _Axis_Discharge_X.StopSlowly();
                return false;
            }

            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZ_OK_PullPos))
            {
                _Axis_Discharge_Z.StopSlowly();
                return false;
            }

            if (Config.IsDischargeCylinderEnable)
            {
                OpenDischargeRotateSuck();
            }

            if (Config.DischargeAxiaZCylinderEnable)
            {
                if (DischargeAxisZCylinderDropAction() == -2) return false;
            }
            else
            {
                CloseDischargeSuck();
                OpenDischargeBlow();
                Thread.Sleep(Config.RobotBlowDelay);
                CloseDischargeBlow();
            }

            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
            {
                _Axis_Discharge_Z.StopSlowly();
                return false;
            }

            if (Config.IsDischargeCylinderEnable)
            {
                DischargeRotateDrop();
            }

            return result;
        }

        public bool RightBendStation_PullMaterial()
        {

            bool result = true;

            if (!CheckRun())
            {
                return false;
            }
            //if ((!IsTransferSuck || !IsTransferFPCSuck))
            //{
            //    AlarmWork();
            //    OutputError("中转真空吸报警");
            //    return false;
            //}
            //else if (!IsLeftBend_PressCylinder_UP)
            //{
            //    OpenLeftBend_PressCylinder();
            //    Thread.Sleep(50);
            //}

            //检查Z轴是否安全        
            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                OutputError("中转Z动作失败！");
                return false;
            }

            if (!IsOnPosition(_Axis_RightBend_DWY, Recipe.RightBend_DWY_WorkPos))
            {
                if (!AxisMoveTo(_Axis_RightBend_DWY, Recipe.RightBend_DWY_WorkPos))
                {
                    AlarmWork();
                    OutputError("对位Y轴运动到安全位置失败！");
                    return false;
                }
            }


            if (!IsOnPosition(_Axis_RightBend_DWR, Recipe.RightBend_DWR_SafePos))
            {

                if (!IsOnPosition(_Axis_RightBend_stgY, Recipe.RightBend_Y_WorkPos))
                {
                    if (!AxisMoveTo(_Axis_RightBend_stgY, Recipe.RightBend_Y_WorkPos))
                    {
                        return false;
                    }
                }
                if (!AxisMoveTo(_Axis_RightBend_DWR, Recipe.RightBend_DWR_SafePos))
                {
                    AlarmWork();
                    OutputError("对位R轴运动到安全位置失败！");
                    return false;
                }
            }

            if (!IsOnPosition(_Axis_RightBend_DWX, Recipe.RightBend_DWX_SafePos))
            {
                if (!AxisMoveTo(_Axis_RightBend_DWX, Recipe.RightBend_DWX_SafePos))
                {
                    AlarmWork();
                    OutputError("对位X轴运动到安全位置失败！");
                    return false;
                }
            }


            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_Transfer_X, _Axis_RightBend_stgY, _Axis_RightBend_DWW },
                new double[] { Recipe.RightBend_LoadX, Recipe.RightBend_LoadY, Recipe.RightBend_DWW_WorkPos }))
            {
                AlarmWork();
                OutputError("ErrTransferX And MidbendstgY MidBendDWW");
                return false;
            }
            Thread.Sleep(100);

            if (!TransferZWork(1, Recipe.RightBend_LoadZ, true))
            {
                OutputError("中转Z动作失败！");
                return false;
            }

            Thread.Sleep(200);
            OpenRightBendSuck();
            CloseTranserSuck();
            CloseTransferFPCSuck();
            OpenTransferBlow();
            Thread.Sleep(Config.RobotBlowDelay);
            CloseTransferBlow();

            if (!AxisMoveTo(_Axis_RightBend_DWX, Recipe.RightBend_DWX_WorkPos))
            {
                _Axis_RightBend_DWX.StopSlowly();
                return false;
            }
            Thread.Sleep(50);

            if (!TransferZWork(0, Recipe.TransferZSafePos, true))
            {
                OutputError("中转Z动作失败！");
                return false;
            }

            if (!AxisMoveTo(_Axis_Transfer_X, Recipe.TransferXSafePos))
            {
                _Axis_Transfer_X.StopSlowly();
                return false;
            }
            return result;
        }

        public bool RightBendStation_BendWork()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            if (!IsOnPosition(_Axis_RightBend_DWY, Recipe.RightBend_DWY_WorkPos))
            {
                if (!AxisMoveTo(_Axis_RightBend_DWY, Recipe.RightBend_DWY_WorkPos))
                {
                    _Axis_RightBend_DWY.StopSlowly();
                    AlarmWork();
                    OutputError("对位Y轴运动到安全位置失败！");
                    return false;
                }
            }


            if (!IsOnPosition(_Axis_RightBend_DWX, Recipe.RightBend_DWX_WorkPos))
            {
                if (!AxisMoveTo(_Axis_RightBend_DWX, Recipe.RightBend_DWX_WorkPos))
                {
                    _Axis_RightBend_DWX.StopSlowly();
                    AlarmWork();
                    OutputError("Err RightBend AxisDWX");
                    return false;
                }
            }
            Thread.Sleep(50);
            CloseRightBendClawCylinder();
            Thread.Sleep(50);


            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_CCDX, _Axis_RightBend_stgY }, new double[] { Recipe.RightBend_CCDPos_X, Recipe.RightBend_CCDPos_Y }))
            {
                _Axis_RightBend_CCDX.StopSlowly();
                _Axis_RightBend_stgY.StopSlowly();

                return false;
            }

            Thread.Sleep(100);

            //CCDWork
            {
                OpenRightBend_UPlightController();
                Thread.Sleep(50);
                double pos = SendBendDegreeMsg((int)StationType.Right);

                if (Math.Abs(pos) > 10)
                {
                    OutputError("角度处理数据过大!");
                    return false;
                }
                if (!AxisMove(_Axis_RightBend_DWW, -Recipe.RightBend_DWW_BasePos + pos))
                {
                    return false;
                }
            }

            double[] posXY = SendAdjustXYMsg((int)StationType.Right);

            //   return false;
            if (Math.Abs(posXY[0]) > 15 || Math.Abs(posXY[1]) > 15)
            {
                OutputError("XY模板校正返回数值过大");
                return false;
            }

            if (!_Axis_RightBend_stgY.Move(posXY[1], Recipe.Bend3adjust_Speed)
                || !_Axis_RightBend_DWX.Move(posXY[0], Recipe.Bend3adjust_Speed)
                || !_Axis_RightBend_DWY.Move(posXY[1], Recipe.Bend3adjust_Speed)
                || !_Axis_RightBend_CCDX.Move(posXY[0], Recipe.Bend3adjust_Speed))
            {
                _Axis_RightBend_stgY.StopSlowly();
                _Axis_RightBend_DWX.StopSlowly();
                _Axis_RightBend_DWY.StopSlowly();
                _Axis_RightBend_CCDX.StopSlowly();
                return false;
            }

            if (!CheckAxisDone(_Axis_RightBend_stgY))
            {
                return false;
            }


            if (!CheckAxisDone(_Axis_RightBend_DWX))
            {
                return false;
            }

            if (!CheckAxisDone(_Axis_RightBend_DWY))
            {
                return false;
            }

            if (!CheckAxisDone(_Axis_RightBend_CCDX))
            {
                return false;
            }
            Thread.Sleep(50);

            double pos_stgy = _Axis_RightBend_stgY.PositionDev;
            double offsetStageY = pos_stgy - Recipe.RightBend_CCDPos_Y;

            OpenRightBendClawCylinder();
            SendAdjustXYMsg((int)StationType.Right);


            //折弯动作
            if (!AxisMoveTo(_Axis_RightBend_stgY, Recipe.RightBend_Y_WorkPos))
            {
                _Axis_RightBend_stgY.StopSlowly();
                return false;
            }


            Thread.Sleep(50);

            if (!RunBend_AxisRWrok(StationType.Right))
            {
                return false;
            }

            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_stgY }, new double[] { offsetStageY + Recipe.RightBend_CCDPos_Y2 }))
            {
                _Axis_RightBend_CCDX.StopSlowly();
                _Axis_RightBend_stgY.StopSlowly();
                return false;
            }
            Thread.Sleep(50);

            //CCDWork
            {

                Thread.Sleep(50);
                for (int i = 0; i <= Recipe.BendPara[4].Adj_Num; i++)
                {
                    double[] pos = SendBendYMsg((int)StationType.Right);
                    while (!IsManualRun)
                    {
                        Thread.Sleep(50);
                    }

                    Thread.Sleep(1000);

                    if (Math.Abs(pos[0] - Recipe.BendPara[4].BaseRate) > 10
                    || Math.Abs(pos[1] - Recipe.BendPara[5].BaseRate) > 10)//Recipe.BendPara[4].BaseRate * 0.5  Recipe.BendPara[5].BaseRate * 0.5
                    {
                        AlarmWork();

                        OutputError("视觉处理返回数据过大");
                        return false;
                    }


                    if (Math.Abs(pos[0] - Recipe.BendPara[4].BaseRate) > Recipe.BendPara[4].ErrAnd
                            || Math.Abs(pos[1] - Recipe.BendPara[5].BaseRate) > Recipe.BendPara[5].ErrAnd)
                    {
                        double[] poses = Calculate_DeltXY(pos, (int)StationType.Right);

                        if (!_Axis_RightBend_DWX.Move(poses[0] * Recipe.BendPara[4].DirValue)
                            || !_Axis_RightBend_DWY.Move(poses[1] * Recipe.BendPara[5].DirValue))
                        {
                            _Axis_RightBend_DWX.StopSlowly();
                            _Axis_RightBend_DWY.StopSlowly();
                            return false;
                        }

                        if (!CheckAxisDone(_Axis_RightBend_DWX))
                        {
                            return false;
                        }

                        if (!CheckAxisDone(_Axis_RightBend_DWY))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        break;
                    }
                    if (i == Recipe.BendPara[4].Adj_Num)
                    {
                        return false;
                    }
                }

            }

            //折弯动作

            //CCDWORK 矫正折弯
            {

            }

            //运行到压合位置进行压合
            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_CCDX, _Axis_RightBend_stgY }, new double[] { Recipe.RightBend_PressPt_X, Recipe.RightBend_PressPt_Y }))
            {
                _Axis_RightBend_CCDX.StopSlowly();
                _Axis_RightBend_stgY.StopSlowly();
                return false;
            }

            OpenRightBend_PressCylinder();
            if (!WaitIOMSec(Config.RightBend_PressCylinder_DownIOIn, 3000, true))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "右折弯压合气缸下感应位报警！";
                fra.ShowDialog();
            }
            Thread.Sleep((int)Recipe.RightYB_Time);
            CloseRightBend_PressCylinder();

            if (!WaitIOMSec(Config.RightBend_PressCylinder_UPIOIn, 3000, true))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "右折弯压合气缸上感应位报警！";
                fra.ShowDialog();
            }
            Thread.Sleep(200);







            {
                Thread.Sleep(100);
                if (Config.IsRightBendAOIDisabled)
                {
                    OpenRightBend_UPlightController();
                    if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_stgY, _Axis_RightBend_CCDX },
                        new double[] { Recipe.RightBend_CCDPos_Y + posXY[1], Recipe.RightBend_CCDPos_X + posXY[0] }))
                    {
                        _Axis_RightBend_stgY.StopSlowly();
                        _Axis_RightBend_CCDX.StopSlowly();
                        return false;
                    }
                    Thread.Sleep(200);
                    double[] pos = SendBendAOIMsg((int)StationType.Right);
                    if (Math.Abs(pos[1] - Recipe.RightAOIY1) > Recipe.RightAOIY1Offset
                        || Math.Abs(pos[3] - Recipe.RightAOIY2) > Recipe.RightAOIY2Offset
                        || Math.Abs(pos[0] - Recipe.RightAOIX1) > Recipe.RightAOIX1Offset
                        || Math.Abs(pos[2] - Recipe.RightAOIX2) > Recipe.RightAOIX2Offset)
                    {
                        OutputError("右反折AOI检测NG");
                        AlarmWork();
                        result = false;
                    }
                }
            }
            CloseRightBend_UPlightController();


            CloseRightBendClawCylinder();
            Thread.Sleep(100);
            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_DWX, _Axis_RightBend_stgY, _Axis_RightBend_DWY }, new double[] { Recipe.RightBend_DWX_SafePos, Recipe.RightBendR_Ypos, Recipe.RightBend_DWY_SafePos }))
            {
                _Axis_RightBend_stgY.StopSlowly();
                _Axis_RightBend_DWX.StopSlowly();
                return false;
            }
            Thread.Sleep(50);

            if (!AxisMoveTo(_Axis_RightBend_DWR, Recipe.RightBend_DWR_SafePos))
            {
                _Axis_RightBend_DWR.StopSlowly();
                return false;
            }
            return result;
        }

        public bool RightBendStation_PickMaterial()
        {
            bool result = true;
            if (!CheckRun())
            {
                return false;
            }
            if (Config.IsDischargeCylinderEnable)
            {
                DischargeRotateUp();
            }
            if (Config.DischargeAxiaZCylinderEnable && !IsDischargeAxisZCylinder_UP)
            {
                if (DischargeAxisZCylinderUp() == -2) return false;
            }




            if (!IsOnPosition(_Axis_Discharge_Z, Recipe.DischargeZSafePos))
            {
                if (!AxisMoveTo(Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                {
                    return false;
                }
            }

            if (!IsOnPosition(_Axis_RightBend_DWR, Recipe.RightBend_DWR_SafePos))
            {
                if (!IsOnPosition(_Axis_RightBend_stgY, Recipe.RightBend_Y_WorkPos))
                {
                    if (!AxisMoveTo(_Axis_RightBend_stgY, Recipe.RightBend_Y_WorkPos))
                    {
                        return false;
                    }
                }

                if (!AxisMoveTo(_Axis_RightBend_DWR, Recipe.RightBend_DWR_SafePos))
                {
                    return false;
                }
            }

            if (!IsOnPosition(_Axis_RightBend_DWX, Recipe.RightBend_DWX_SafePos))
            {
                if (!AxisMoveTo(_Axis_RightBend_DWX, Recipe.RightBend_DWX_SafePos))
                {
                    return false;
                }
            }


            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_stgY, _Axis_Discharge_X }, new double[] { Recipe.RightBend_Discharge_Y, Recipe.RightBend_Discharge_X }))
            {
                _Axis_RightBend_stgY.StopSlowly();
                _Axis_Discharge_X.StopSlowly();
                return false;
            }

            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.RightBend_Discharge_Z))
            {
                return false;
            }

            CloseRightBendSuck();
            OpenDischargeSuck();
            OpenRightBendBlow();

            Thread.Sleep(Config.StageBlowDelay);
            CloseRightBendBlow();
            //if (IsDischargeSuck)
            //{
            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
            {
                AlarmWork();
                OutputError("Err axisDischargeZ Safe");
                return false;
            }
            //}
            Thread.Sleep(100);

            if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeX_OK_PullPos))
            {
                _Axis_Discharge_X.StopSlowly();
                return false;
            }

            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZ_OK_PullPos))
            {
                _Axis_Discharge_Z.StopSlowly();
                return false;
            }

            if (Config.IsDischargeCylinderEnable)
            {
                OpenDischargeRotateSuck();
            }

            if (Config.DischargeAxiaZCylinderEnable)
            {
                if (DischargeAxisZCylinderDropAction() == -2) return false;
            }
            else
            {
                CloseDischargeSuck();
                OpenDischargeBlow();
                Thread.Sleep(Config.RobotBlowDelay);
                CloseDischargeBlow();
            }

            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
            {
                _Axis_Discharge_Z.StopSlowly();
                return false;
            }

            if (Config.IsDischargeCylinderEnable)
            {
                DischargeRotateDrop();
            }

            return result;
        }

        private double[] SendBendAOIMsg(int index)
        {
            double[] pos = new double[] { -100, -100, -100, -100, -100 };
            if (index == 0)
            {
                str_leftbend_y = " ";

                str_Bend1Rev = "";
                if (!SendMsg("A,AOI"))
                {
                    OutputError("折弯相机1发送拍照数据失败");
                    return pos;
                }
                else
                {
                    OutputMessage("等待折弯相机1返回数据!");
                    while (str_Bend1Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            return pos;
                        }
                        Thread.Sleep(20);
                    }
                }
                str_leftbend_y = str_Bend1Rev;
                pos = SplitAOIString(str_leftbend_y);
                str_leftbend_y = "";
                return pos;
            }
            else if (index == 1)
            {
                str_midbend_y = " ";
                str_Bend2Rev = "";
                if (!Bend2SendMsg("B,AOI"))
                {
                    OutputError("折弯相机2发送拍照数据失败");
                    return pos;
                }
                else
                {
                    OutputMessage("等待折弯相机2返回数据!");
                    while (str_Bend2Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            return pos;
                        }
                        Thread.Sleep(20);
                    }
                }
                str_midbend_y = str_Bend2Rev;
                pos = SplitAOIString(str_midbend_y);
                str_midbend_y = "";
                return pos;
            }
            else
            {
                str_Bend3Rev = "";
                if (!Bend3SendMsg("C,AOI"))
                {
                    OutputError("折弯相机3发送拍照数据失败");
                    return pos;
                }
                else
                {
                    OutputMessage("等待折弯相机3返回数据!");
                    while (str_Bend3Rev.Length < 2)
                    {
                        if (_IsStop)
                        {
                            return pos;
                        }
                        Thread.Sleep(20);
                    }
                }
                str_rightbend_y = str_Bend3Rev;
                pos = SplitAOIString(str_rightbend_y);
                str_rightbend_y = "";
                return pos;
            }

        }

        public bool Discharge_FeedCylinderWork()
        {
            bool ret = true;
            if (!CheckRun())
            {
                return false;
            }



            //if (Config.DischargeAxiaZCylinderEnable)
            //{
            //    if (DischargeAxisZCylinderUp() == -2) return false;
            //    if (DischargeAxisZCylinderDropAction() == -2) return false;
            //}


            if (Config.IsDischargeCylinderEnable && DischargeRotateUp() != 0)
            {
                return false;
            }

            if (Config.IsDischargeCylinderEnable && DischargeRotateDrop() != 0)
            {
                return false;
            }


            return ret;
        }

        #endregion


        public bool EnforceFeedLine()
        {
            bool result = true;
            _ForceStop = false;
            if (!GetIOOutStatus(Config.SuplyBeltIOOut))
            {
                OpenBeltMotor();
                Thread.Sleep(50);
                _IsBeltMotorFirst = true;
            }
            while (!IsLoadBeltHaveSth)
            {
                if (_ForceStop)
                {
                    return false;
                }
                Thread.Sleep(10);
            }
            WaitMilliSec(Config.FeedBeltLineDelay);
            CloseBeltMotor();
            if (!AxisMoveTo(_Axis_Load_Y, Recipe.LoadYpos))
            {
                _Axis_Load_Y.StopSlowly();
                return false;
            }
            Thread.Sleep(100);
            if (!AxisMoveTo(_Axis_Load_Y, Recipe.LoadYWaitPos))
            {
                _Axis_Load_Y.StopSlowly();
                return false;
            }
            return result;
        }


        bool _feedflag;
        /// <summary>
        /// True表示翻转已经取完料等待被上料机械手取走
        /// </summary>
        bool _IsLoadMachineReady = false;
        bool _IsFeedLineRun = false;
        /// <summary>
        /// true 允许流水线进料 false进料完成
        /// </summary>
        public bool _IsBeltMotorFirst = true;
        public DateTime FilterTimeStart;
        public void RunFeedLineWork()
        {
            _IsFeedLineRun = true;
            _IsBeltMotorFirst = true;
            if (Config.IsRunNull) //空跑直接返回
            {
                return;
            }
            FilterTimeStart = DateTime.Now;
            OutputMessage("上料流水线启动！");
            while (true)
            {
                if (ManualStop)
                {
                    CloseBeltMotor();
                    return;
                }

                if (!_IsFeedLineRun)
                {
                    _IsFeedLineRun = false;
                    CloseBeltMotor();
                    OutputMessage("上料流水线停止!");
                    break;
                }

                if (IsLoadBeltHaveSth)
                {
                    if ((DateTime.Now - FilterTimeStart).TotalMilliseconds < Config.LoadFilterTime)//过滤掉误感应的信号
                    {
                        continue;
                    }
                }


                if (IsLoadBeltHaveSth || Config.IsRunNull)//来料感应
                {
                    WaitMilliSec(Config.FeedBeltLineDelay);
                    if (_IsBeltMotorFirst)
                    {
                        CloseBeltMotor();
                        if (!AxisMoveTo(_Axis_Load_Y, Recipe.LoadYpos))
                        {
                            _Axis_Load_Y.StopSlowly();
                        }

                        if (!AxisMoveTo(_Axis_Load_Y, Recipe.LoadYWaitPos, Config.LoadYBackSpeed))
                        {
                            _Axis_Load_Y.StopSlowly();
                        }
                        FilterTimeStart = DateTime.Now;
                        _IsBeltMotorFirst = false;
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    if (!GetIOOutStatus(Config.SuplyBeltIOOut))
                    {
                        OpenBeltMotor();
                        Thread.Sleep(50);
                        _IsBeltMotorFirst = true;
                    }
                }

                if (_IsStop)
                {
                    CloseBeltMotor();
                    break;
                }
                Thread.Sleep(50);
            }
        }

        int res;
        /// <summary>
        /// 翻转取料
        /// </summary>
        /// <returns>0正常 -1真空报警人工取走  -2气缸感应报警</returns>
        private int FeedRotateFatch()
        {
            res = 0;

            if (IsFeedRotate_Suck)//判断是否有料
            {
                if (IsFeed_RotateCylinder_Down)//进料翻转工位翻转气缸下感应
                {
                    OpenFeedRotateUPCylinder();
                    while (!WaitIOMSec_FeedUPCylinderUP(3000))
                    {
                        if (IsStop)
                        {
                            return -2;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "上料上下气缸上感应报警！";
                        fra.ShowDialog();
                    }
                }

                OpenFeedRotateCylinder();
                while (!WaitIOMSec_FeedRotateCylinderUP(3000))
                {
                    if (IsStop)
                    {
                        return -2;
                    }
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "上料翻转气缸上感应报警！";
                    fra.ShowDialog();
                }

                return res;
            }

            if (IsFeed_RotateCylinder_Down)//进料翻转工位翻转气缸下感应
            {
                OpenFeedRotateUPCylinder();
                while (!WaitIOMSec_FeedUPCylinderUP(3000))
                {
                    OutputError("上料上下气缸上感应报警", true);
                    DialogResult DRet = ShowMsgChoiceBox("上料上下气缸上感应报警\r\n" + "取消:流水线将停止!\r\n"
                                  + "确认:继续下一步生产!", true, false);
                    if (DRet == DialogResult.Cancel)
                    {
                        frmConfirm frm = new frmConfirm("流水线将停止!", false, true);
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            res = -2;
                            return res;
                        }
                    }
                    break;
                }
            }

            CloseFeedRotateCylinder();
            while (!WaitIOMSec_FeedRotateCylinderDown(3000))
            {
                OutputError("上料翻转气缸下感应报警", true);
                DialogResult DRet = ShowMsgChoiceBox("上料翻转气缸下感应报警\r\n" + "取消:流水线将停止!\r\n"
                              + "确认:继续下一步生产!", true, false);
                if (DRet == DialogResult.Cancel)
                {
                    frmConfirm frm = new frmConfirm("流水线将停止!", false, true);
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        res = -2;
                        return res;
                    }
                }
                break;
            }

            CloseFeedUPCylinder();
            while (!WaitIOMSec_FeedUPCylinderDown(3000))
            {
                OutputError("上料上下气缸下感应报警", true);
                DialogResult DRet = ShowMsgChoiceBox("上料上下气缸下感应报警\r\n" + "取消:流水线将停止!\r\n"
                              + "确认:继续下一步生产!", true, false);
                if (DRet == DialogResult.Cancel)
                {
                    frmConfirm frm = new frmConfirm("流水线将停止!", false, true);
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        res = -2;
                        return res;
                    }
                }
                break;
            }

            OpenFeedRotateSuck();
            OpenFeedRotateFPCSuck();

            Thread.Sleep(Config.RobotFetchDelay);
            while (!WaitIOMSec_FeedRotateSuck(1500))
            {

                OpenFeedRotateUPCylinder();//报警后气缸上升
                Thread.Sleep(500);

                DialogResult DRet = ShowMsgChoiceBox("上料真空报警\r\n"
              + "人工取料:点击人工取料后,取走物料!\r\n"
              + "确认:继续下一步生产!", true, false);
                if (DRet == DialogResult.Cancel)
                {
                    CloseFeedRotateSuck();
                    CloseFeedRotateFPCSuck();
                    frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                    frm.IOOut1 = Config.Feed_RotateSuck_IOOut;
                    frm.IOOut2 = Config.Feed_RotateFPCSuck_IOOut;
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {

                        res = -1;
                        return res;
                    }
                }

            }

            OpenFeedRotateUPCylinder();
            while (!WaitIOMSec_FeedUPCylinderUP(3000))
            {
                OutputError("上料上下气缸上感应报警", true);

                DialogResult DRet = ShowMsgChoiceBox("上料上下气缸上感应报警\r\n" + "取消:流水线将停止!\r\n"
                              + "确认:继续下一步生产!", true, false);
                if (DRet == DialogResult.Cancel)
                {
                    frmConfirm frm = new frmConfirm("流水线将停止!", false, true);
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        res = -2;
                        return res;
                    }
                }
                break;

            }

            if (_IsAutoRun)
            {
                if (IsLoadBeltHaveSth)
                {
                    MeasurementContext.OutputError("联机信号异常,产品取走后仍有取料信号");
                }
                _IsBeltMotorFirst = true;
            }

            OpenFeedRotateCylinder();
            while (!WaitIOMSec_FeedRotateCylinderUP(3000))
            {
                OutputError("上料翻转气缸上感应报警", true);
                DialogResult DRet = ShowMsgChoiceBox("上料翻转气缸上感应报警\r\n" + "取消:流水线将停止!\r\n"
                              + "确认:继续下一步生产!", true, false);
                if (DRet == DialogResult.Cancel)
                {
                    frmConfirm frm = new frmConfirm("流水线将停止!", false, true);
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        res = -2;
                        return res;
                    }
                }
                break;
            }


            while (!WaitIOMSec_FeedRotateSuck(1500))
            {
                DialogResult DRet = ShowMsgChoiceBox("上料真空报警\r\n"
              + "人工取料:点击人工取料后,取走物料!\r\n"
              + "确认:继续下一步生产!", true, false);
                if (DRet == DialogResult.Cancel)
                {
                    CloseFeedRotateSuck();
                    CloseFeedRotateFPCSuck();
                    frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                    frm.IOOut1 = Config.Feed_RotateSuck_IOOut;
                    frm.IOOut2 = Config.Feed_RotateFPCSuck_IOOut;
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {

                        res = -1;
                        return res;
                    }
                }

            }


            return 0;
        }


        object FeedRotate = new object();
        public void RunFeedRotateWork()
        {
            lock (FeedRotate)
            {
                _IsLoadMachineReady = false;
                Thread.Sleep(500);
                if (Config.IsFeedCylinderEnable)
                {
                    OpenFeedRotateSuck();
                    OpenFeedRotateFPCSuck();
                    Thread.Sleep(2000);
                    if (IsFeedRotate_Suck)//第一次启动判断是否有料
                    {
                        if (IsFeed_RotateCylinder_Down)//进料翻转工位翻转气缸下感应
                        {
                            OpenFeedRotateUPCylinder();
                            while (!WaitIOMSec_FeedUPCylinderUP(3000))
                            {
                                if (IsStop)
                                {
                                    return;
                                }
                                FrAlarm fra = new FrAlarm();
                                fra.lblmsg.Text = "上料上下气缸上感应报警！";
                                fra.ShowDialog();
                            }
                        }

                        OpenFeedRotateCylinder();
                        while (!WaitIOMSec_FeedRotateCylinderUP(3000))
                        {
                            if (IsStop)
                            {
                                return;
                            }
                            FrAlarm fra = new FrAlarm();
                            fra.lblmsg.Text = "上料翻转气缸上感应报警！";
                            fra.ShowDialog();
                        }
                        _IsLoadMachineReady = true;
                    }
                    else
                    {
                        CloseFeedRotateSuck();
                        CloseFeedRotateFPCSuck();
                    }
                }

                while (true)
                {
                    if (ManualStop || _IsStop || !_IsFeedLineRun)
                    {
                        MeasurementContext.OutputMessage("进料翻转停止运行！");
                        return;
                    }

                    if (Config.IsFeedCylinderEnable && _IsFeedLineRun && !_IsLoadMachineReady)
                    {
                        if (!IsFeedRotate_Suck)//判断是否有料
                        {
                            CloseFeedRotateCylinder();
                        }
                    }



                    if (Config.IsFeedCylinderEnable && _IsFeedLineRun)
                    {
                        if (!_IsLoadMachineReady && (!_IsBeltMotorFirst || Config.IsRunNull))//表示翻转是否有料
                        {
                            int res = FeedRotateFatch();//取料
                            if (res == -1) _IsBeltMotorFirst = true;//重新进料
                            if (res == -2) _IsFeedLineRun = false;//退出流程
                            if (res == 0) _IsLoadMachineReady = true;//取料完成
                        }
                    }
                    Thread.Sleep(50);
                }
            }
        }
        private bool _IsDischaregeLineRun = false;
        public void RundischargeLineWork()
        {
            _IsDischaregeLineRun = true;
            OutputMessage("出料流水线启动!");
            if (Config.IsDischargeCylinderEnable)
            {
                if (IsDischargeRotate_Suck)
                {
                    _IsDischargeRotateReady = false;
                }
                else
                {
                    if (DischargeRotateUp() != 0) _IsDischaregeLineRun = false;
                    _IsDischargeRotateReady = true;
                }
            }


            while (true)
            {

                if (!IsAutoRun) CloseDischargeBeltMotor();
                while (!_IsAutoRun)
                {
                    Thread.Sleep(50);
                    if (_IsStop) return;
                }

                if (ManualStop)
                {
                    CloseDischargeBeltMotor();
                    return;
                }

                if (!_IsDischaregeLineRun)
                {
                    CloseDischargeBeltMotor();
                    OutputMessage("出料流水线停止");
                    break;
                }
                else
                {
                    if (IsDischargeFull)
                    {
                        CloseDischargeBeltMotor();
                    }
                    else
                    {
                        if (!GetIOOutStatus(Config.DischargeLineBeltIOOut)) OpenDischargeBeltMotor();
                    }
                    Thread.Sleep(50);
                }

                if (Config.IsDischargeCylinderEnable && !_IsDischargeRotateReady)// 表示气缸有料
                {
                    if (!IsDischargeHave || Config.IsRunNull)//有料感应光
                    {

                        if (DischargeRotateDrop() != 0) _IsDischaregeLineRun = false;
                        _IsDischargeRotateReady = true;


                    }
                }

                Thread.Sleep(50);
                if (_IsStop)
                {
                    CloseDischargeBeltMotor();
                    break;
                }
            }
        }

        /// <summary>
        /// 下料翻转下料
        /// </summary>
        /// <returns>0正常 -1真空异常人工取走 -2气缸报警</returns>
        public int DischargeRotateDrop()//下料翻转下料
        {
            int res = 0;

            CloseDischargeRotateCylinder();
            while (!WaitIOMSec_DischargeRotateCylinderDown(3000))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "下料翻转气缸下感应报警！";
                fra.ShowDialog();
                res = -2;
                return res;
            }

            CloseDischargeUPCylinder();
            while (!WaitIOMSec_DischargeUPCylinderDown(3000))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "下料上下气缸下感应报警！";
                fra.ShowDialog();
                res = -2;
                return res;
            }

            CloseDischargeRotateSuck();
            OpenDischargeRotateBlow();
            Thread.Sleep(Config.RobotBlowDelay);
            CloseDischargeRotateBlow();
            OpenDischargeUPCylinder();
            while (!WaitIOMSec_DischargeUPCylinderUP(3000))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "下料上下气缸上感应报警！";
                fra.ShowDialog();
                res = -2;
                return res;
            }
            OpenDischargeRotateCylinder();
            while (!WaitIOMSec_DischargeRotateCylinderUP(3000))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "下料翻转气缸上感应报警！";
                fra.ShowDialog();
                res = -2;
                return res;
            }
            Thread.Sleep(300);
            return res;
        }

        /// <summary>
        /// 下料翻转上升
        /// </summary>
        /// <returns>0正常 -1真空异常人工取走 -2气缸报警</returns>
        public int DischargeRotateUp()
        {
            int res = 0;
            if (!IsDischarge_UPCylinder_UP)
            {
                OpenDischargeUPCylinder();
                while (!WaitIOMSec_DischargeUPCylinderUP(3000))
                {
                    FrAlarm fra = new FrAlarm();
                    fra.lblmsg.Text = "下料上下气缸上感应报警！";
                    fra.ShowDialog();
                    res = -2;
                    return res;
                }
            }

            OpenDischargeRotateCylinder();
            while (!WaitIOMSec_DischargeRotateCylinderUP(3000))
            {
                FrAlarm fra = new FrAlarm();
                fra.lblmsg.Text = "下料翻转气缸上感应报警！";
                fra.ShowDialog();
                res = -2;
                return res;
            }
            CloseDischargeBlow();
            CloseDischargeRotateFPCSuck();
            Thread.Sleep(200);
            return res;
        }

        #region 下料Z气缸





        /// <summary>
        /// 下料Z气缸上升 
        /// </summary>
        /// <returns>0正常 -1真空异常人工取走 -2气缸报警</returns>
        public int DischargeAxisZCylinderUp()
        {
            int res = 0;
            if (!IsDischargeAxisZCylinder_UP)
            {
                OpenDischargeAxisZCylinder();
                while (!WaitIOMSec_DischargeAxisZCylinderUP(3000))
                {
                    DialogResult DRet = ShowMsgChoiceBox("下料Z轴气缸上感应报警\r\n" + "取消:运行停止!\r\n"
                                   + "确认:继续下一步生产!", true, false);
                    if (DRet == DialogResult.Cancel)
                    {
                        res = -2;
                        return res;
                    }
                }
            }
            return res;
        }

        public int DischargeAxisZCylinderDown()
        {
            int res = 0;
            if (!IsDischargeAxisZCylinder_Down)
            {
                CloseDischargeAxisZCylinder();
                while (!WaitIOMSec_DischargeAxisZCylinderDown(3000))
                {
                    DialogResult DRet = ShowMsgChoiceBox("下料Z轴气缸下感应报警\r\n" + "取消:运行停止!\r\n"
                                   + "确认:继续下一步生产!", true, false);
                    if (DRet == DialogResult.Cancel)
                    {
                        res = -2;
                        return res;
                    }
                }
            }
            return res;
        }


        public int DischargeAxisZCylinderDropAction()
        {
            int res = 0;
            if (DischargeAxisZCylinderDown() == -2)
            {
                res = -2;
                return res;
            }

            CloseDischargeSuck();
            CloseFPCDischargeSuck();
            OpenDischargeBlow();
            Thread.Sleep(Config.RobotBlowDelay);
            CloseDischargeBlow();
            if (DischargeAxisZCylinderUp() == -2)
            {
                res = -2;
                return res;
            }
            return res;
        }

        #endregion 

        private bool _IsNGDischaregeLineRun = false;


        int markTime = 0;
        public void RunNGDischargeLineWork()
        {

            _IsNGDischaregeLineRun = true;
            if (Config.IsRunNull) //空跑直接返回
            {
                return;
            }

            OutputMessage("NG出料流水线启动!");
            while (true)
            {
                if (!_IsAutoRun)
                {
                    CloseNGBeltMotor();
                }
                while (!_IsAutoRun)
                {
                    Thread.Sleep(50);
                    if (_IsStop)
                    {
                        return;
                    }
                }


                if (ManualStop)
                {
                    CloseNGBeltMotor();
                    return;
                }

                if (!_IsNGDischaregeLineRun)
                {
                    OutputMessage("NG皮带线停止");
                    CloseNGBeltMotor();
                    _IsNGDischaregeLineRun = true;
                    break;
                }

                //if (!IsNGLineCylinderInside)   //NG机构被手动拉出，停皮带
                //{
                //    CloseNGBeltMotor();
                //}
                if (!IsNGLineFull && !IsNGHaveSth)
                {
                    CloseNGBeltMotor();
                }

                if (IsNGLineFull)   //NG线满料，停皮带
                {
                    CloseNGBeltMotor();

                    if (IsNGHaveSth && Environment.TickCount - markTime > 8000)
                    {
                        OutputError("NG流水线料满,请及时取走！");
                        for (int i = 0; i < 4; i++)
                        {
                            OpenBuzzer();
                            OpenRedLight();
                            Thread.Sleep(1200);
                            CloseRedLight();
                            CloseBuzzer();
                            Thread.Sleep(300);
                        }
                        markTime = Environment.TickCount;
                    }
                }
                else
                {
                    if (!IsNGLineFull && IsNGHaveSth)//当感应位有料 满料位无料 电机转一定时间后停止
                    {
                        Stopwatch spwatch = new Stopwatch();
                        spwatch.Restart();
                        OpenNGBeltMotor();
                        while (spwatch.ElapsedMilliseconds < 8000)
                        {
                            if (IsNGLineFull)
                            {
                                CloseNGBeltMotor();
                                break;
                            }
                            Thread.Sleep(100);
                        }
                    }
                    markTime = Environment.TickCount;
                }


                Thread.Sleep(100);
                if (_IsStop)
                {
                    CloseNGBeltMotor();
                    break;
                }
            }







        }


        public bool RunHander()
        {
            bool result = true;

            ConnectNet();
            if (!CheckRun())
            {
                return false;
            }

            if (!IsGateSafe())
            {
                return false;
            }

            if (Config.IsGateAlarm_Enable)
            {
                LockSafeDoor();
            }

            if (_IsReset)
            {
                _WorkStatus = WorkStatuses.Running;
                OnWorkStatusChanged();
                OpenGreenLight();
                CloseRedLight();
                CloseYellowLight();
                _IsAutoRun = true;
                _IsStop = false;

                ManualStop = false;
                OpenLeftSMSuck();
                Thread.Sleep(150);
                if (!IsLeftSMStgSuck)
                {
                    CloseLeftSMSuck();
                }

                OpenMidSMSuck();
                Thread.Sleep(150);
                if (!IsMidSMStgSuck)
                {
                    CloseMidSMSuck();
                }

                OpenRightSMSuck();
                Thread.Sleep(150);
                if (!IsRightSMStgSuck)
                {
                    CloseRightSMSuck();
                }

                OpenLeftBendSuck();
                Thread.Sleep(150);
                if (!IsLeftBendStgSuck)
                {
                    CloseLeftBendSuck();
                }

                OpenMidBendSuck();
                Thread.Sleep(150);
                if (!IsMidBendStgSuck)
                {
                    CloseMidBendSuck();
                }

                OpenRightBendSuck();
                Thread.Sleep(150);
                if (!IsRightBendStgSuck)
                {
                    CloseRightBendSuck();
                }

                QueueBend.Clear();
                QueueSM.Clear();
                QueueBendOut.Clear();
                QueueTransferIn.Clear();
                QueueTearResult.Clear();

                nTransferTearIndex = 3;//中转取料
                nTransferBendIndex = 3;//中转放料
                nFeedIndex = 3;//上料
                nDischargeIndex = 3;//下料


                #region
                b_FeedCarry_Safe = true;
                b_DischargeCarry_Safe = true;

                b_bend1flag = false;
                b_bend2flag = false;
                b_bend3flag = false;

                _IsLoadWorking = true;
                Step_load = 0;

                Step_LeftSM = 0;
                _IsleftsmWorking = true;

                Step_MidSM = 0;
                _IsrightsmWorking = true;

                Step_RightSM = 0;
                _IsmidsmWorking = true;

                Step_Transfer = 0;
                _IsTransferWorking = true;

                Step_LeftBend = 0;

                _IsleftbendWorking = true;

                Step_MidBend = 0;

                _IsmidbendWorking = true;

                Step_RightBend = 0;

                _IsrightbendWorking = true;

                Step_Discharge = 0;
                _IsDischargeWorking = true;


                _IsLeftsmloadDone = false;
                _IsMidsmloadDone = false;
                _IsRightsmloadDone = false;

                _IsLeftSMDone = false;
                _IsMidSMDone = false;
                _IsRightSMDone = false;

                _IsLeftSMOut = false;
                _IsMidSMOut = false;
                _IsRightSMOut = false;

                _IsLeftBendReady = false;
                _IsMidBendReady = false;
                _IsRightBendReady = false;

                _IsLeftBendUp = false;
                _IsMidBendUp = false;
                _IsRightBendUp = false;

                _IsLeftBendOutReady = false;
                _IsMidBendOutReady = false;
                _IsRightBendOutReady = false;

                _IsLeftDischargeReady = false;
                _IsMidDischargeReady = false;
                _IsRightDischargeReady = false;

                _IsCycleStop = false;
                FlagFeed = true;
                FlagTear1 = true;
                FlagTear2 = true;
                FlagTear3 = true;
                FlagTear1Have = false;
                FlagTear2Have = false;
                FlagTear3 = false;
                FlagTranfer = true;
                FlagBend1 = true;
                FlagBend2 = true;
                FlagBend3 = true;
                FlagDischarge = true;

                #endregion              
                LockSafeDoor();
                if (Config.IsFeedCylinderEnable)
                {
                    Task t13 = Task.Run(() => RunFeedRotateWork());
                }
                if (Config.IsControlUpStreamEnable)//ZGH20220913
                {
                    Task t12 = Task.Run(() => RunFeedLineWork());
                }                
                Task t1 = Task.Run(() => RunLoadWork());
                Task t2 = Task.Run(() => RunLeftSMWork());
                Task t3 = Task.Run(() => RunTransferWork());
                Task t4 = Task.Run(() => RunBend1Work());
                Task t5 = Task.Run(() => RunDischargeWork());
                Task t6 = Task.Run(() => RunMidSMWork());
                Task t7 = Task.Run(() => RunBend2Work());
                Task t8 = Task.Run(() => RunRightSMWork());
                Task t9 = Task.Run(() => RunBend3Work());
                Task t10 = Task.Run(() => RunNGDischargeLineWork());
                Task t11 = Task.Run(() => RundischargeLineWork());

            }
            else
            {
                AlarmWork();
                Thread.Sleep(200);
                MeasurementContext.OutputError("设备未复位!", false);
                CloseBuzzer();
            }
            return result;
        }
        public void AlarmWork()
        {
            OpenBuzzer();
            OpenRedLight();
            CloseGreenLight();
        }
        private void ClearAlarm()
        {
            CloseBuzzer();
            CloseRedLight();
            OpenGreenLight();
        }

        #region  IO Define Work

        #region Load Station Out
        public bool OpenBeltMotor()
        {
            return SetIOOut(Config.SuplyBeltIOOut, true);
        }
        public bool CloseBeltMotor()
        {
            return SetIOOut(Config.SuplyBeltIOOut, false);
        }
        public bool OpenLoadSTSuck()
        {
            return SetIOOut(Config.LoadVacuumIOOut, true);
        }
        public bool CloseLoadSTSuck()
        {
            return SetIOOut(Config.LoadVacuumIOOut, false);
        }

        public bool OpenLoadSTFPCSuck()
        {
            return SetIOOut(Config.LoadFPCVacuumIOOut, true);
        }

        public bool CloseLoadSTFPCSuck()
        {
            return SetIOOut(Config.LoadFPCVacuumIOOut, false);
        }


        public bool OpenLoadSTBlow()
        {
            return SetIOOut(Config.LoadBlowVacuumIOOut, true);
        }


        public bool CloseLoadSTBlow()
        {
            return SetIOOut(Config.LoadBlowVacuumIOOut, false);
        }
        #endregion


        public bool LockSafeDoor()
        {
            return
                (
                CanSetIOOut(Config.SMStation_BackGate1_IOOutEx, false)
                //SetIOOut(Config.SMStation_BackGate2_IOOut, true)
                && CanSetIOOut(Config.SMStation_FrontGate1_IOOutEx, false)
                //&& SetIOOut(Config.SMStation_FrontGate2_IOOut, true)
            //    && CanSetIOOut(Config.SMStation_SideGate1_IOOutEx, false)
               // && CanSetIOOut(Config.SMStation_SideGate2_IOOutEx, false)
                && CanSetIOOut(Config.Bend_BackGate1_IOOutEx, false)
                && CanSetIOOut(Config.Bend_FrontGate1_IOOutEx, false));
            //  && CanSetIOOut(Config.Bend_SideGate1_IOOutEx, false));
        }


        public bool UnlockSafeDoor()
        {
            return
               (
               CanSetIOOut(Config.SMStation_BackGate1_IOOutEx, true)
               // SetIOOut(Config.SMStation_BackGate2_IOOut, false)
               && CanSetIOOut(Config.SMStation_FrontGate1_IOOutEx, true)
               //    && SetIOOut(Config.SMStation_FrontGate2_IOOut, false)
            //   && CanSetIOOut(Config.SMStation_SideGate1_IOOutEx, true)
            //   && CanSetIOOut(Config.SMStation_SideGate2_IOOutEx, true)
               && CanSetIOOut(Config.Bend_BackGate1_IOOutEx, true)
               && CanSetIOOut(Config.Bend_FrontGate1_IOOutEx, true));
            //   && CanSetIOOut(Config.Bend_SideGate1_IOOutEx, true));
        }

        #region  进出料





        public bool OpenFeedRotateUPCylinder()
        {
            return SetIOOut(Config.Feed_RotateUDCylinderUp_IOOut, true) && SetIOOut(Config.Feed_RotateUDCylinderDown_IOOut, false);
        }

        public bool CloseFeedUPCylinder()
        {
            return SetIOOut(Config.Feed_RotateUDCylinderUp_IOOut, false) && SetIOOut(Config.Feed_RotateUDCylinderDown_IOOut, true); ;
        }

        public bool OpenFeedRotateCylinder()
        {
            return CanSetIOOut(Config.Feed_RotateCylinder_IOOutEx, false) && CanSetIOOut(Config.Feed_RotateCylinderORG_IOOutEx, true);
        }

        public bool CloseFeedRotateCylinder()
        {
            return CanSetIOOut(Config.Feed_RotateCylinder_IOOutEx, true) && CanSetIOOut(Config.Feed_RotateCylinderORG_IOOutEx, false);
        }


        public bool OpenDischargeUPCylinder()
        {
            return CanSetIOOut(Config.Discharge_UPCylinder_CardCOutEx, false);
        }

        public bool CloseDischargeUPCylinder()
        {
            return CanSetIOOut(Config.Discharge_UPCylinder_CardCOutEx, true);
        }

        #region 下料Z轴气缸
        /// <summary>
        /// 气缸下  下料Z轴气缸 与下料翻转同一个IO
        /// </summary>
        /// <returns></returns>
        public bool CloseDischargeAxisZCylinder()
        {
            return CanSetIOOut(Config.Discharge_UPCylinder_CardCOutEx, true);
        }
        /// <summary>
        ///气缸上 下料Z轴气缸 与下料翻转同一个IO
        /// </summary>
        /// <returns></returns>
        public bool OpenDischargeAxisZCylinder()
        {
            return CanSetIOOut(Config.Discharge_UPCylinder_CardCOutEx, false);

        }
        /// <summary>
        /// 下料Z轴气缸 与下料翻转同一个IO
        /// </summary>
        /// <returns></returns>
        public bool IsDischargeAxisZCylinder_Down
        {
            get
            {
                return CanGetIOInStatus(Config.Discharge_UPCylinder_DownIOInEx);
            }
        }
        /// <summary>
        /// 下料Z轴气缸 与下料翻转同一个IO
        /// </summary>
        /// <returns></returns>
        public bool IsDischargeAxisZCylinder_UP
        {
            get
            {
                return CanGetIOInStatus(Config.Discharge_UPCylinder_UPIOInEx);
            }
        }



        #endregion 





        public bool OpenDischargeRotateCylinder()
        {
            return CanSetIOOut(Config.Discharge_RotateCylinder_CardCIOOutEx, true);
        }

        public bool CloseDischargeRotateCylinder()
        {
            return CanSetIOOut(Config.Discharge_RotateCylinder_CardCIOOutEx, false);
        }


        public bool OpenDischargeRotateSuck()
        {
            return CanSetIOOut(Config.DischargeRotateSuck_OutEx, true);
        }

        public bool CloseDischargeRotateSuck()
        {
            return CanSetIOOut(Config.DischargeRotateSuck_OutEx, false);
        }

        public bool OpenDischargeRotateBlow()
        {
            return CanSetIOOut(Config.DischargeRotateBlow_OutEx, true);
        }

        public bool CloseDischargeRotateBlow()
        {
            return CanSetIOOut(Config.DischargeRotateBlow_OutEx, false);
        }

        public bool OpenDischargeRotateFPCSuck()
        {
            return CanSetIOOut(Config.DischargeRotateFPCSuck_OutEx, true);
        }

        public bool CloseDischargeRotateFPCSuck()
        {
            return CanSetIOOut(Config.DischargeRotateFPCSuck_OutEx, false);
        }



        /// <summary>
        ///  进料翻转真空，破真空，FPC真空三个输出
        /// 由先前折弯工位光源上下气缸更改而来，先前三个输出弃用
        /// </summary>
        /// <returns></returns>        
        public bool OpenFeedRotateSuck()
        {
            return SetIOOut(Config.Feed_RotateSuck_IOOut, true);
        }

        public bool CloseFeedRotateSuck()
        {
            return SetIOOut(Config.Feed_RotateSuck_IOOut, false);
        }

        public bool OpenFeedRotateBlow()
        {
            return SetIOOut(Config.Feed_RotateBreakVacuum_IOOut, true);
        }

        public bool CloseFeedRotateBlow()
        {
            return SetIOOut(Config.Feed_RotateBreakVacuum_IOOut, false);
        }

        public bool OpenFeedRotateFPCSuck()
        {
            return SetIOOut(Config.Feed_RotateFPCSuck_IOOut, true);
        }

        public bool CloseFeedRotateFPCSuck()
        {
            return SetIOOut(Config.Feed_RotateFPCSuck_IOOut, false);
        }

        #region 新增输入
        /// <summary>
        /// 上料手臂_下
        /// </summary>
        /// <returns></returns>
        public bool FeedUDCylinderDown()
        {
            return SetIOOut(Config.Feed_UDCylinderUP_IOOut, false) && SetIOOut(Config.Feed_UDCylinderDown_IOOut, true);
        }
        /// <summary>
        /// 上料手臂_上
        /// </summary>
        /// <returns></returns>
        public bool FeedUDCylinderUp()
        {
            bool flg1 = SetIOOut(Config.Feed_UDCylinderUP_IOOut, true);
            bool flg2 = SetIOOut(Config.Feed_UDCylinderDown_IOOut, false);
            return true;
        }








        public bool IsDischarge_UPCylinder_UP
        {
            get
            {
                return CanGetIOInStatus(Config.Discharge_UPCylinder_UPIOInEx);
            }
        }

        public bool IsDischarge_UPCylinder_Down
        {
            get
            {
                return CanGetIOInStatus(Config.Discharge_UPCylinder_DownIOInEx);
            }
        }

        public bool IsDischarge_RotateCylinder_UP
        {
            get
            {
                return CanGetIOInStatus(Config.Discharge_RotateCylinder_UPIOInEx);
            }
        }

        public bool IsDischarge_RotateCylinder_Down
        {
            get
            {
                return true; //return CanGetIOInStatus(Config.InputAir_IOInEx);
            }
        }

        public bool IsFeed_UPCylinder_UP
        {
            get
            {
                return GetIOInStatus(Config.Feed_RotateUpDownCylinder_UpIOIn);
            }
        }

        public bool IsFeed_UPCylinder_Down
        {
            get
            {
                return GetIOInStatus(Config.Feed_RotateUpDownCylider_DownIOIn);
            }
        }

        public bool IsFeed_RotateCylinder_UP
        {
            get
            {
                return GetIOInStatus(Config.Feed_RotateCylinder_UpIOIn);
            }
        }


        public bool IsFeed_RotateCylinder_Down
        {
            get
            {
                return GetIOInStatus(Config.Feed_RotateCylinder_DownIOIn);
            }
        }

        public bool IsFeedRotate_Suck
        {
            get
            {
                return GetIOInStatus(Config.Feed_RotateVacuumCheckIOIn);
            }
        }

        public bool IsFeedRotate_FPCSuck
        {
            get
            {
                return GetIOInStatus(Config.Feed_RotateFPCVacuumCheckIOIn);
            }
        }

        public bool IsDischargeRotate_Suck
        {
            get
            {
                return true;// return CanGetIOInStatus(Config.InputVacuum_IOInEx);
            }
        }

        //备用
        public bool IsDischargeRotate_FPCSuck
        {
            get
            {
                return true; //GetIOInStatus(Config.BeltOpticalIOIN);
            }
        }

        #endregion

        #region LeftSM Out
        public bool OpenLeftSMSuck()
        {
            return SetIOOut(Config.LeftSM_StgVacuum_IOOut, true);
        }

        public bool CloseLeftSMSuck()
        {
            CloseLeftSMFPCSuck();
            return SetIOOut(Config.LeftSM_StgVacuum_IOOut, false);
        }
        public bool OpenTearFPCBlow()
        {
            //return CanSetIOOut(Config.OutEx12, true);

            return SetIOOut(Config.TearAOIBlowCylinder, true);
        }

        public bool CloseTearFPCBlow()
        {
            return SetIOOut(Config.TearAOIBlowCylinder, false);
        }

        public bool OpenLeftSMFPCSuck()
        {
            if (Config.FPC_Tear_Enable)
            {
                return SetIOOut(Config.LeftSM_StgFPCVacuum_IOOut, true);
            }
            else
            {
                return true;
            }
        }

        public bool OpenTear1FPCSuck()
        {
            return SetIOOut(Config.LeftSM_StgFPCVacuum_IOOut, true);
        }
        public bool CloseLeftSMFPCSuck()
        {
            return SetIOOut(Config.LeftSM_StgFPCVacuum_IOOut, false);
        }

        public bool OpenLeftSMBlow()
        {
            return SetIOOut(Config.LeftSM_StgBlowVacuum_IOOut, true);
        }

        public bool CloseLeftSMBlow()
        {
            return SetIOOut(Config.LeftSM_StgBlowVacuum_IOOut, false);
        }


        public bool OpenLeftSMReduce()
        {
            return SetIOOut(Config.LeftSM_StgReduceVacuum_IOOut, true);
        }

        public bool CloseLeftSMReduce()
        {
            return SetIOOut(Config.LeftSM_StgReduceVacuum_IOOut, false);
        }


        public bool OpenLeftSMLRCylinder()
        {
            return SetIOOut(Config.LeftSM_LRCylinder_IOOut, true);
        }

        public bool CloseLeftSMLRCylinder()
        {
            return SetIOOut(Config.LeftSM_LRCylinder_IOOut, false);
        }


        public bool OpenLeftSMFBCylinder()
        {
            return SetIOOut(Config.LeftSM_FBCylinder_IOOut, true);
        }


        public bool CloseLeftSMFBCylinder()
        {
            return SetIOOut(Config.LeftSM_FBCylinder_IOOut, false);
        }


        public bool OpenLeftSMUDCylinder()
        {

            return SetIOOut(Config.LeftSM_UDCylinder_IOOut, true);

        }

        public bool CloseLeftSMUDCylinder()
        {
            return SetIOOut(Config.LeftSM_UDCylinder_IOOut, false);
        }


        public bool OpenLeftSMGlueUDCylinder()
        {
            return SetIOOut(Config.LeftSM_GlueCylinder_IOOut, true);
        }

        public bool CloseLeftSMGlueUDCylinder()
        {
            return SetIOOut(Config.LeftSM_GlueCylinder_IOOut, false);
        }

        public bool OpenLeftSMRollerUDCylinder()
        {
            return SetIOOut(Config.LeftSM_RollerCylinder_IOOut, true);
        }


        public bool CloseLeftSMRollerUDCyliner()
        {
            return SetIOOut(Config.LeftSM_RollerCylinder_IOOut, false);
        }


        public bool OpenLeftSMGlueLockEle()
        {
            return SetIOOut(Config.LeftSM_GlueLockCylinder_IOOut, true);
        }

        public bool CloseLeftSMGlueLockEle()
        {
            return SetIOOut(Config.LeftSM_GlueLockCylinder_IOOut, false);
        }


        #endregion

        #region RightSM Out
        public bool OpenRightSMSuck()
        {
            return CanSetIOOut(Config.RightSM_StgVacuum_IOOutEx, true);
        }


        public bool CloseRightSMSuck()
        {
            return CanSetIOOut(Config.RightSM_StgVacuum_IOOutEx, false);
        }


        public bool OpenRightSMFPCSuck()
        {
            if (Config.FPC_Tear_Enable)
            {
                return CanSetIOOut(Config.RightSM_StgFPCVacuum_IOOutEx, true);
            }
            else
            {
                return true;
            }

        }

        public bool OpenTear3FPCSuck()
        {
            return CanSetIOOut(Config.RightSM_StgFPCVacuum_IOOutEx, true);
        }
        public bool CloseRightSMFPCSuck()
        {
            return CanSetIOOut(Config.RightSM_StgFPCVacuum_IOOutEx, false);
        }

        public bool OpenRightSMBlow()
        {
            return CanSetIOOut(Config.RightSM_StgBlowVacuum_IOOutEx, true);
        }

        public bool CloseRightSMBlow()
        {
            return CanSetIOOut(Config.RightSM_StgBlowVacuum_IOOutEx, false);
        }


        public bool OpenRightSMReduce()
        {
            return CanSetIOOut(Config.RightSM_StgReduceVacuum_IOOutEx, true);
        }

        public bool CloseRightSMReduce()
        {
            return CanSetIOOut(Config.RightSM_StgReduceVacuum_IOOutEx, false);
        }


        public bool OpenRightSMLRCylinder()
        {
            return CanSetIOOut(Config.RightSM_LRCylinder_IOOutEx, true);
        }

        public bool CloseRightSMLRCylinder()
        {
            return CanSetIOOut(Config.RightSM_LRCylinder_IOOutEx, false);
        }


        public bool OpenRightSMFBCylinder()
        {
            return CanSetIOOut(Config.RightSM_FBCylinder_IOOutEx, true);
        }


        public bool CloseRightSMFBCylinder()
        {
            return CanSetIOOut(Config.RightSM_FBCylinder_IOOutEx, false);
        }


        public bool OpenRightSMUDCylinder()
        {

            return CanSetIOOut(Config.RightSM_UDCylinder_IOOutEx, true);
        }

        public bool CloseRightSMUDCylinder()
        {
            return CanSetIOOut(Config.RightSM_UDCylinder_IOOutEx, false);
        }


        public bool OpenRightSMGlueUDCylinder()
        {
            return CanSetIOOut(Config.RightSM_GlueCylinder_IOOutEx, true);
        }

        public bool CloseRightSMGlueUDCylinder()
        {
            return CanSetIOOut(Config.RightSM_GlueCylinder_IOOutEx, false);
        }

        public bool OpenRightRollerUDCylinder()
        {
            return SetIOOut(Config.RightSM_RollerCylinder_IOOut, true);
        }


        public bool CloseRightSMRollerUDCyliner()
        {
            return SetIOOut(Config.RightSM_RollerCylinder_IOOut, false);
        }


        public bool OpenRightSMGlueLockEle()
        {
            return SetIOOut(Config.RightSM_GlueLockCylinder_IOOut, true);
        }

        public bool CloseRightSMGlueLockEle()
        {
            return SetIOOut(Config.RightSM_GlueLockCylinder_IOOut, false);
        }

        #endregion

        #region MidSM Out
        public bool OpenMidSMSuck()
        {
            return SetIOOut(Config.MidSM_StgVacuum_IOOut, true);
        }


        public bool CloseMidSMSuck()
        {
            CloseMidSMFPCSuck();
            return SetIOOut(Config.MidSM_StgVacuum_IOOut, false);
        }


        public bool OpenMidSMFPCSuck()
        {
            if (Config.FPC_Tear_Enable)
            {
                return SetIOOut(Config.MidSM_StgFPCVacuum_IOOut, true);
            }
            else
            {
                return true;
            }
        }

        public bool OpenTear2FPCSuck()
        {
            return SetIOOut(Config.MidSM_StgFPCVacuum_IOOut, true);
        }
        public bool CloseMidSMFPCSuck()
        {
            return SetIOOut(Config.MidSM_StgFPCVacuum_IOOut, false);
        }

        public bool OpenMidSMBlow()
        {
            return CanSetIOOut(Config.MidSM_StgBlowVacuum_IOOutEx, true);
        }

        public bool CloseMidSMBlow()
        {
            return CanSetIOOut(Config.MidSM_StgBlowVacuum_IOOutEx, false);
        }


        public bool OpenMidSMReduce()
        {
            return CanSetIOOut(Config.MidSM_StgReduceVacuum_IOOutEx, true);
        }

        public bool CloseMidSMReduce()
        {
            return CanSetIOOut(Config.MidSM_StgReduceVacuum_IOOutEx, false);
        }


        public bool OpenMidSMLRCylinder()
        {
            return CanSetIOOut(Config.MidSM_LRCylinder_IOOutEx, true);
        }

        public bool CloseMidSMLRCylinder()
        {
            return CanSetIOOut(Config.MidSM_LRCylinder_IOOutEx, false);
        }


        public bool OpenMidSMFBCylinder()
        {
            return CanSetIOOut(Config.MidSM_FBCylinder_IOOutEx, true);
        }


        public bool CloseMidSMFBCylinder()
        {
            return CanSetIOOut(Config.MidSM_FBCylinder_IOOutEx, false);
        }


        public bool OpenMidSMUDCylinder()
        {

            return CanSetIOOut(Config.MidSM_UDCylinder_IOOutEx, true);
        }

        public bool CloseMidSMUDCylinder()
        {
            return CanSetIOOut(Config.MidSM_UDCylinder_IOOutEx, false);
        }


        public bool OpenMidSMGlueUDCylinder()
        {
            return CanSetIOOut(Config.MidSM_GlueCylinder_IOOutEx, true);
        }

        public bool CloseMidSMGlueUDCylinder()
        {
            return CanSetIOOut(Config.MidSM_GlueCylinder_IOOutEx, false);
        }

        public bool OpenMidSMRollerUDCylinder()
        {
            return CanSetIOOut(Config.MidSM_RollerCylinder_IOOutEx, true);
        }

        public bool OpenTear1RllCylinder()
        {
            if (Config.Tear3RllCLD_Enable)
            {
                return SetIOOut(Config.LeftSM_RollerCylinder_IOOut, true);
            }
            else
            {
                return false;
            }
        }

        public bool OpenTear2RllCylinder()
        {
            if (Config.Tear2RllCLD_Enable)
            {
                return CanSetIOOut(Config.MidSM_RollerCylinder_IOOutEx, true);
            }
            else
            {
                return false;
            }
        }

        public bool OpenTear3RllCylinder()
        {
            if (Config.Tear3RllCLD_Enable)
            {
                return SetIOOut(Config.RightSM_RollerCylinder_IOOut, true);
            }
            else
            {
                return false;
            }
        }

        public bool CloseMidSMRollerUDCyliner()
        {
            return CanSetIOOut(Config.MidSM_RollerCylinder_IOOutEx, false);
        }

        public bool OpenMidSMGlueLockEle()
        {
            return CanSetIOOut(Config.MidSM_GlueLockCylinder_IOOutEx, true);
        }

        public bool CloseMidSMGlueLockEle()
        {
            return CanSetIOOut(Config.MidSM_GlueLockCylinder_IOOutEx, false);
        }
        #endregion

        #region LeftBend Out
        public bool OpenLeftBendSuck()
        {
            return SetIOOut(Config.LeftBend_SuckVacuum_IOOut, true);
        }

        public bool CloseLeftBendSuck()
        {
            return SetIOOut(Config.LeftBend_SuckVacuum_IOOut, false);
        }

        public bool OpenLeftBendBlow()
        {
            return SetIOOut(Config.LeftBend_BlowVacuum_IOOut, true);
        }

        public bool CloseLeftBendBlow()
        {
            return SetIOOut(Config.LeftBend_BlowVacuum_IOOut, false);
        }

        public bool OpenLeftBendClawCylinder()
        {
            return SetIOOut(Config.LeftBend_ClawCylinderOut_IOOut, true) && SetIOOut(Config.LeftBend_ClawCylinderBack_IOOut, false);
        }

        public bool CloseLeftBendClawCylinder()
        {
            return SetIOOut(Config.LeftBend_ClawCylinderOut_IOOut, false) && SetIOOut(Config.LeftBend_ClawCylinderBack_IOOut, true);
        }





        public bool OpenLeftBend_rightlightCylinder()
        {
            return SetIOOut(Config.LeftBend_RightOPTUD_IOOut, true);
        }

        public bool CloseLeftBend_rightlightCylinder()
        {
            return SetIOOut(Config.LeftBend_RightOPTUD_IOOut, false);
        }

        public bool OpenLeftBend_PressCylinder()
        {
            if (!Config.IsRunNull)
            {
                return SetIOOut(Config.LeftBend_PressCylinderDown_IOOut, true) & SetIOOut(Config.LeftBend_PressCylinderUp_IOOut, false);
            }
            else
            {
                return true;
            }
        }

        public bool CloseLeftBend_PressCylinder()
        {
            return SetIOOut(Config.LeftBend_PressCylinderDown_IOOut, false) & SetIOOut(Config.LeftBend_PressCylinderUp_IOOut, true);
        }

        public bool OpenLeftBend_UPlightController()
        {
            return CanSetIOOut(Config.LeftBend_UPOPTControl_IOOutEx, true);
        }

        public bool CloseLeftBend_UPlightController()
        {
            return CanSetIOOut(Config.LeftBend_UPOPTControl_IOOutEx, false);
        }







        #endregion

        #region MidBend Out
        public bool OpenMidBendSuck()
        {
            return SetIOOut(Config.MidBend_SuckVacuum_IOOut, true);
        }

        public bool CloseMidBendSuck()
        {
            return SetIOOut(Config.MidBend_SuckVacuum_IOOut, false);
        }

        public bool OpenMidBendBlow()
        {
            return SetIOOut(Config.MidBend_BlowVacuum_IOOut, true);
        }

        public bool CloseMidBendBlow()
        {
            return SetIOOut(Config.MidBend_BlowVacuum_IOOut, false);
        }


        public bool OpenMidBendClawCylinder()
        {
            return SetIOOut(Config.MidBend_ClawCylinderOut_IOOut, true) && SetIOOut(Config.MidBend_ClawCylinderBack_IOOut, false);
        }


        public bool CloseMidBendClawCylinder()
        {
            return SetIOOut(Config.MidBend_ClawCylinderOut_IOOut, false) && SetIOOut(Config.MidBend_ClawCylinderBack_IOOut, true);
        }








        public bool OpenMidBend_PressCylinder()
        {
            if (!Config.IsRunNull)
            {
                return SetIOOut(Config.MidBend_PressCylinderDown_IOOut, true) && SetIOOut(Config.MidBend_PressCylinderUp_IOOut, false);
            }
            else
            {
                return true;
            }
        }

        public bool CloseMidBend_PressCylinder()
        {
            return SetIOOut(Config.MidBend_PressCylinderDown_IOOut, false) && SetIOOut(Config.MidBend_PressCylinderUp_IOOut, true);
        }

        public bool OpenMidBend_UPlightController()
        {
            return CanSetIOOut(Config.MidBend_UPOPTControl_IOOutEx, true);
        }

        public bool CloseMidBend_UPlightController()
        {
            return CanSetIOOut(Config.MidBend_UPOPTControl_IOOutEx, false);
        }



        #endregion

        #region RightBend Out
        public bool OpenRightBendSuck()
        {
            return SetIOOut(Config.RightBend_SuckVacuum_IOOut, true);
        }

        public bool CloseRightBendSuck()
        {
            return SetIOOut(Config.RightBend_SuckVacuum_IOOut, false);
        }

        public bool OpenRightBendBlow()
        {
            return SetIOOut(Config.RightBend_BlowVacuum_IOOut, true);
        }

        public bool CloseRightBendBlow()
        {
            return SetIOOut(Config.RightBend_BlowVacuum_IOOut, false);
        }


        public bool OpenRightBendClawCylinder()
        {
            return SetIOOut(Config.RightBend_ClawCylinderOut_IOOut, true) && SetIOOut(Config.RightBend_ClawCylinderBack_IOOut, false);
        }


        public bool CloseRightBendClawCylinder()
        {
            return SetIOOut(Config.RightBend_ClawCylinderOut_IOOut, false) && SetIOOut(Config.RightBend_ClawCylinderBack_IOOut, true);
        }







        public bool OpenRightBend_PressCylinder()
        {
            if (!Config.IsRunNull)
            {
                return SetIOOut(Config.RightBend_PressCylinderDown_IOOut, true) && SetIOOut(Config.RightBend_PressCylinderUp_IOOut, false);
            }
            else
            {
                return true;
            }
        }

        public bool CloseRightBend_PressCylinder()
        {
            return SetIOOut(Config.RightBend_PressCylinderDown_IOOut, false) && SetIOOut(Config.RightBend_PressCylinderUp_IOOut, true);
        }

        public bool OpenRightBend_UPlightController()
        {
            return CanSetIOOut(Config.RightBend_UPOPTControl_IOOutEx, true);
        }

        public bool CloseRightBend_UPlightController()
        {
            return CanSetIOOut(Config.RightBend_UPOPTControl_IOOutEx, false);
        }

        #endregion
        #region //ZGH20220913新增与上游设备交互输出信号
        public bool OpenToUpstream_Safe()
        {
            return CanSetIOOut(Config.SendUpstream_Safe_IOOutEx, true);
        }
        public bool CloseToUpstream_Safe()
        {
            return CanSetIOOut(Config.SendUpstream_Safe_IOOutEx, false);
        }
        public bool OpenToUpstream_Request()
        {
            return CanSetIOOut(Config.SendUpstream_Request_IOOutEx, true);
        }
        public bool CloseToUpstream_Request()
        {
            return CanSetIOOut(Config.SendUpstream_Request_IOOutEx, false);
        }
        public bool OpenToUpstream_Finish()
        {
            return CanSetIOOut(Config.SendUpstream_Finish_IOOutEx, true);
        }
        public bool CloseToUpstream_Finish()
        {
            return CanSetIOOut(Config.SendUpstream_Finish_IOOutEx, false);
        }
        public bool OpenToUpstream_Spare()
        {
            return CanSetIOOut(Config.SendUpstream_Spare_IOOutEx, true);
        }
        public bool CloseToUpstream_Spare()
        {
            return CanSetIOOut(Config.SendUpstream_Spare_IOOutEx, false);
        }



        #endregion
        #region Transfer Discharge Station Out
        public bool OpenTranserSuck()
        {
            return SetIOOut(Config.Transfer_Suckvacuum_IOOut, true);
        }

        public bool CloseTranserSuck()
        {
            return SetIOOut(Config.Transfer_Suckvacuum_IOOut, false);
        }

        public bool TransferUDCylinderDown()
        {
            bool flg1 = CanSetIOOut(Config.TransferCylinderDown_IOOutEx, true);
            bool flg2 = CanSetIOOut(Config.TransferCylinderUp_IOOutEx, false);
            if (!flg1 || !flg2)
            {

            }
            return true;
        }
        public bool TransferUDCylinderUp()
        {
            if (!CanGetIOInStatus(Config.Transfer_UDCylinderDown_IOInEx))
            {

            }

            return CanSetIOOut(Config.TransferCylinderDown_IOOutEx, false) && CanSetIOOut(Config.TransferCylinderUp_IOOutEx, true);
        }






        public bool OpenTransferFPCSuck()
        {
            return SetIOOut(Config.Transfer_FPCSuckvacuum_IOOut, true);
        }

        public bool CloseTransferFPCSuck()
        {
            return SetIOOut(Config.Transfer_FPCSuckvacuum_IOOut, false);
        }

        public bool OpenTransferBlow()
        {
            return SetIOOut(Config.Transfer_Blowvacuum_IOOut, true);
        }

        public bool CloseTransferBlow()
        {
            return SetIOOut(Config.Transfer_Blowvacuum_IOOut, false);
        }

        public bool OpenDischargeSuck()
        {
            return SetIOOut(Config.Discharge_Suckvacuum_IOOut, true);
        }

        public bool CloseDischargeSuck()
        {
            CloseFPCDischargeSuck();
            return SetIOOut(Config.Discharge_Suckvacuum_IOOut, false);
        }

        public bool OpenFPCDischargeSuck()
        {
            //return CanSetIOOut(Config.OutEx11, true);
            return SetIOOut(Config.Discharge_FPCSuckvacuum_IOOut, true);

        }

        public bool CloseFPCDischargeSuck()
        {
            return SetIOOut(Config.Discharge_FPCSuckvacuum_IOOut, false);
        }

        public bool OpenDischargeBlow()
        {
            return SetIOOut(Config.Discharge_Blowvacuum_IOOut, true);
        }

        public bool CloseDischargeBlow()
        {
            return SetIOOut(Config.Discharge_Blowvacuum_IOOut, false);
        }

        public bool OpenDischargeBeltMotor()
        {
            return SetIOOut(Config.DischargeLineBeltIOOut, true);
        }

        public bool CloseDischargeBeltMotor()
        {
            return SetIOOut(Config.DischargeLineBeltIOOut, false);
        }

        public bool OpenNGBeltMotor()
        {
            return SetIOOut(Config.NGlineBeltIOOut, true);
        }

        public bool CloseNGBeltMotor()
        {
            return SetIOOut(Config.NGlineBeltIOOut, false);
        }
        #endregion

        #region others Out

        public bool OpenSMlightController()
        {
            return CanSetIOOut(Config.CardCOutEx16, true);
        }


        public bool CloseSMlightController()
        {
            return CanSetIOOut(Config.CardCOutEx16, false);
        }

        public bool OpenRedLight()
        {
            return SetIOOut(Config.RedLightIOOut, true);
        }

        public bool CloseRedLight()
        {
            return SetIOOut(Config.RedLightIOOut, false);
        }

        public bool OpenYellowLight()
        {
            return SetIOOut(Config.YellowLightIOOut, true);
        }

        public bool CloseYellowLight()
        {
            return SetIOOut(Config.YellowLightIOOut, false);
        }

        public bool OpenGreenLight()
        {
            return SetIOOut(Config.GreenLightIOOut, true);
        }

        public bool CloseGreenLight()
        {
            return SetIOOut(Config.GreenLightIOOut, false);
        }


        public bool OpenBuzzer()
        {
            return SetIOOut(Config.BuzzerIOOut, true);
        }

        public bool CloseBuzzer()
        {
            return SetIOOut(Config.BuzzerIOOut, false);
        }

        public bool OpenBlueStartLight()
        {
            return SetIOOut(Config.StartBlueLightIOOut, true);
        }

        public bool CloseBlueStartLight()
        {
            return SetIOOut(Config.StartBlueLightIOOut, false);
        }

        public bool OpenYellowResetLight()
        {
            return SetIOOut(Config.ResetYellowLightIOOut, true);
        }

        public bool CloseYellowResetLight()
        {
            return SetIOOut(Config.ResetYellowLightIOOut, false);
        }

        #endregion

        #region Others IO IN
        public bool IsLoadBeltHaveSth
        {
            get
            {
                return GetIOInStatus(Config.BeltOpticalIOIN);
            }
        }

        public bool IsLoadSTSuck
        {
            get
            {
                return GetIOInStatus(Config.LoadVacuumIOIn);
            }
        }

        public bool IsLoadFPCSuck
        {
            get
            {
                return GetIOInStatus(Config.LoadfFPCVacuumIOIn);
            }
        }

        public bool IsTransferSuck
        {
            get
            {
                return GetIOInStatus(Config.TransferVacuumIOIn);
            }
        }

        public bool IsTransferFPCSuck
        {
            get
            {
                return GetIOInStatus(Config.TransferFPCVacuumIOIn);
            }
        }

        public bool IsDischargeSuck
        {
            get
            {
                return GetIOInStatus(Config.DischargeVacuumIOIn);
            }
        }

        public bool IsDischargeFPCSuck
        {
            get
            {
                return GetIOInStatus(Config.Discharge_FPCVacuumIOIn);
            }
        }

        public bool IsDischargeFull
        {
            get
            {
                if (Config.DsgLine_FullSensor_Enable)
                {
                    return GetIOInStatus(Config.DischargeLine_OpticalSensorFull_IOIn);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsDischargeHave
        {
            get
            {
                return GetIOInStatus(Config.DischargeLine_OpticalSensorHave_IOIn);
            }
        }

        public bool IsNGLineFull
        {
            get
            {
                return CanGetIOInStatus(Config.NGLineFullIOInEx);
            }
        }

        public bool IsNGHaveSth
        {
            get
            {
                return CanGetIOInStatus(Config.NGLineHaveIOInEx);
            }
        }

        public bool IsNGLineCylinderInside
        {
            get
            {
                return GetIOInStatus(Config.NGLineCylinderStatic);
                //return CanGetIOInStatus(Config.NGLineCylinderReach);
            }
        }



        public bool IsNGStg2Sensor
        {
            get
            {
                return CanGetIOInStatus(Config.SMNGstgHave2IOInEx);
            }
        }

        public bool IsNGStg3Sensor
        {
            get
            {
                return CanGetIOInStatus(Config.SMNGstgHave3IOInEx);
            }
        }
        #endregion

        #region LeftSMIN
        public bool IsLeftSMStgSuck
        {
            get
            {
                return GetIOInStatus(Config.LeFTSMVacuumIOIn);
            }
        }

        public bool IsLeftSM_MPHave
        {
            get
            {
                if (Config.MP1_Enable)
                {
                    return GetIOInStatus(Config.LeftSMMPVacuumIOIn);
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsLeftGlueHave
        {
            get
            {
                if (Config.Glue1_Enable)
                {
                    return GetIOInStatus(Config.LeftSMGlueOpticalIOIn);
                }
                else
                {
                    return true;
                }
            }
        }















        #endregion

        #region MidSMIN
        public bool IsMidSMStgSuck
        {
            get
            {
                return CanGetIOInStatus(Config.MidSMVacuumIOInEx);
            }
        }

        public bool IsMidSM_MPHave
        {
            get
            {
                if (Config.MP2_Enable)
                {
                    return CanGetIOInStatus(Config.MidSMMPVacuumIOInEx);
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsMidGlueHave
        {
            get
            {
                if (Config.Glue2_Enable)
                {
                    return CanGetIOInStatus(Config.MidSMGlueOpticalIOInEx);
                }
                else
                {
                    return true;
                }
            }
        }
        #endregion

        #region RightSM IN
        public bool IsRightSMStgSuck
        {
            get
            {
                return GetIOInStatus(Config.RightSMVacuumIOIn);
            }
        }

        public bool IsRightSM_MPHave
        {
            get
            {
                if (Config.MP3_Enable)
                {
                    return GetIOInStatus(Config.RightSMMPVacuumIOIn);
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsRightGlueHave
        {
            get
            {
                if (Config.Glue3_Enable)
                {
                    return GetIOInStatus(Config.RightSMGlueOpticalIOIn);
                }
                else
                {
                    return true;
                }
            }
        }
        #endregion

        #region LeftBend

        public bool IsMidBendStgSuck
        {
            get
            {
                return GetIOInStatus(Config.MidBend_stgVacuum_IOIn);
            }
        }


        public bool IsLeftBendStgSuck
        {
            get
            {
                return GetIOInStatus(Config.LeftBend_stgVacuum_IOIn);
            }
        }


        public bool IsRightBendStgSuck
        {
            get
            {
                return GetIOInStatus(Config.RightBend_stgVacuum_IOIn);
            }
        }
        #endregion

        #endregion


        #region //Bend WORK


        private EProductAtt left_OK = EProductAtt.OK;
        private EProductAtt mid_OK = EProductAtt.OK;
        private EProductAtt right_OK = EProductAtt.OK;

        private bool tear1_OK = true;
        private bool tear2_OK = true;
        private bool tear3_OK = true;
        private bool bend1_OK = true;
        private bool bend2_OK = true;
        private bool bend3_OK = true;

        //折弯工位运动到上料位置
        private bool GoBendFeedPos(StationType _station)
        {
            bool result = true;
            AxisBase axisR = null;
            AxisBase axisStgY = null;
            AxisBase axisDWX = null;
            AxisBase axisDWY = null;
            AxisBase axisDWW = null;

            double pos_SafeR = 0;
            double pos_WorkStgY = 0;
            double pos_WaitDWX = 0;
            double pos_WorkDWY = 0;
            double pos_LoadStgY = 0;
            double pos_WorkDWW = 0;

            if (_station == StationType.Left)
            {
                axisR = _Axis_LeftBend_DWR;
                axisStgY = _Axis_LeftBend_stgY;
                axisDWX = _Axis_LeftBend_DWX;
                axisDWY = _Axis_LeftBend_DWY;
                axisDWW = _Axis_LeftBend_DWW;

                pos_SafeR = Recipe.LeftBend_DWR_SafePos;
                pos_WorkStgY = Recipe.LeftBend_Y_WorkPos;
                pos_WaitDWX = Recipe.LeftBend_DWX_SafePos;
                pos_WorkDWY = Recipe.LeftBend_DWY_WorkPos;
                pos_LoadStgY = Recipe.LeftBend_LoadY;
                pos_WorkDWW = Recipe.LeftBend_DWW_WorkPos;
            }
            else if (_station == StationType.Mid)
            {
                axisR = _Axis_MidBend_DWR;
                axisStgY = _Axis_MidBend_stgY;
                axisDWX = _Axis_MidBend_DWX;
                axisDWY = _Axis_MidBend_DWY;
                axisDWW = _Axis_MidBend_DWW;

                pos_SafeR = Recipe.MidBend_DWR_SafePos;
                pos_WorkStgY = Recipe.MidBend_Y_WorkPos;
                pos_WaitDWX = Recipe.MidBend_DWX_SafePos;
                pos_WorkDWY = Recipe.MidBend_DWY_WorkPos;
                pos_LoadStgY = Recipe.MidBend_LoadY;
                pos_WorkDWW = Recipe.MidBend_DWW_WorkPos;
            }
            else
            {
                axisR = _Axis_RightBend_DWR;
                axisStgY = _Axis_RightBend_stgY;
                axisDWX = _Axis_RightBend_DWX;
                axisDWY = _Axis_RightBend_DWY;
                axisDWW = _Axis_RightBend_DWW;

                pos_SafeR = Recipe.RightBend_DWR_SafePos;
                pos_WorkStgY = Recipe.RightBend_Y_WorkPos;
                pos_WaitDWX = Recipe.RightBend_DWX_SafePos;
                pos_WorkDWY = Recipe.RightBend_DWY_WorkPos;
                pos_LoadStgY = Recipe.RightBend_LoadY;
                pos_WorkDWW = Recipe.RightBend_DWW_WorkPos;
            }

            if (!IsOnPosition(axisR, pos_SafeR))
            {
                if (!IsOnPosition(axisStgY, pos_WorkStgY))
                {
                    if (!AxisMoveTo(axisStgY, pos_WorkStgY))
                    {
                        axisStgY.StopSlowly();
                        return false;
                    }
                    Thread.Sleep(200);
                }
                if (!AxisMoveTo(axisR, pos_SafeR))
                {
                    axisR.StopSlowly();
                    return false;
                }
            }



            if (_station == StationType.Left)
            {
                if (!QueueBend.Contains(0) && (!b_bend1flag))
                {
                    QueueBend.Enqueue(0);
                }
            }
            else if (_station == StationType.Mid)
            {
                if (!QueueBend.Contains(1) && (!b_bend2flag))
                {
                    QueueBend.Enqueue(1);
                }
            }
            else
            {
                if (!QueueBend.Contains(2) && (!b_bend3flag))
                {
                    QueueBend.Enqueue(2);
                }
            }

            if (!AxisMoveTo(new AxisBase[] { axisStgY, axisDWX, axisDWY, axisDWW },
                new double[] { pos_LoadStgY, pos_WaitDWX, pos_WorkDWY, pos_WorkDWW }))
            {
                axisStgY.StopSlowly();
                axisDWX.StopSlowly();
                axisDWY.StopSlowly();
                axisDWW.StopSlowly();
                return false;
            }
            return result;
        }

        //折弯工位运动到拍照位置
        private bool GoBendCCDPos(StationType _station)
        {
            bool result = true;
            AxisBase axisStgY = null;
            AxisBase axisCCDX = null;

            double posx = 0;
            double posy = 0;

            if (_station == StationType.Left)
            {
                axisStgY = _Axis_LeftBend_stgY;
                axisCCDX = _Axis_LeftBend_CCDX;
                posx = Recipe.LeftBend_CCDPos_X;
                posy = Recipe.LeftBend_CCDPos_Y;
            }
            else if (_station == StationType.Mid)
            {
                axisStgY = _Axis_MidBend_stgY;
                axisCCDX = _Axis_MidBend_CCDX;
                posx = Recipe.MidBend_CCDPos_X;
                posy = Recipe.MidBend_CCDPos_Y;
            }
            else
            {
                axisStgY = _Axis_RightBend_stgY;
                axisCCDX = _Axis_RightBend_CCDX;
                posx = Recipe.RightBend_CCDPos_X;
                posy = Recipe.RightBend_CCDPos_Y;
            }

            if (!AxisMoveTo(new AxisBase[] { axisStgY, axisCCDX },
                                 new double[] { posy, posx }))
            {
                axisStgY.StopSlowly();
                axisCCDX.StopSlowly();
                return false;
            }

            return result;
        }

        //折弯工位角度校正
        private bool RunBendAdjustAngle(StationType _station)
        {
            try
            {


                bool result = true;
                double pos = 0;
                short netwaitcount = 20;

                if (Config.IsRunNull)
                {
                    Thread.Sleep(200);
                    return true;
                }

                if (_station == StationType.Left)
                {
                    bool check = true;
                    short adjcount = 0;

                    while (check)
                    {
                        adjcount++;
                        str_Bend1Rev = "";
                        if (!SendMsg("A,PZF"))
                        {
                            BendCCDNet.StopConnection();
                            BendCCDNet.Dispose();
                            BendCCDNet.StartConnection();
                            Thread.Sleep(500);
                            WriteErrLog("折弯相机1发送拍照数据失败");
                            OutputError("折弯相机1发送拍照数据失败");
                            if (adjcount > netwaitcount)
                            {
                                pos = 11;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            Stopwatch stt = new Stopwatch();
                            stt.Restart();
                            //OutputMessage("等待折弯相机1返回数据!");
                            //WriteLog("折弯1 PZF等待返回数据！");
                            while (str_Bend1Rev.Length < 2)
                            {
                                if (_IsStop)
                                {
                                    break;
                                }

                                if (stt.ElapsedMilliseconds > NetTimeOut)
                                {
                                    BendCCDNet.StopConnection();
                                    BendCCDNet.Dispose();
                                    OpenBuzzer();
                                    Thread.Sleep(200);
                                    CloseBuzzer();
                                    BendCCDNet.StartConnection();
                                    OutputError("折弯相机1接收信息超时报警!");
                                    WriteLog("折弯1 PZF超时报警！");

                                    break;
                                }

                                Thread.Sleep(20);
                            }
                            WriteLog(string.Format("折弯1 PZF返回数据{0}！", str_Bend1Rev));
                            if (str_Bend1Rev.Length < 2 || SplitDegreeString(str_Bend1Rev) == -100)
                            {
                                if (adjcount > 5)
                                {
                                    OutputError("折弯相机1角度超次数!");
                                    check = false;
                                    break;
                                }
                                pos = -100;
                                Thread.Sleep(20);
                                continue;
                            }
                            else
                            {
                                pos = SplitDegreeString(str_Bend1Rev);
                            }
                        }
                        if (MeasurementContext.Config.Isbend1_CalibProtect && !IsBendAdjuetAngleBetweenLimit(pos, StationType.Left))
                        {
                            OutputError("左对位角度即将超出保护上下限,校正失败!");
                            return false;
                        }

                        if (Math.Abs(pos) > 10)
                        {
                            return false;
                        }
                        else if (Math.Abs(-Recipe.LeftBend_DWW_BasePos + pos) < Config.AdjustAngle)
                        {
                            check = false;
                            break;
                        }
                        else if (adjcount > 5)
                        {
                            OutputMessage("折弯相机1角度超次数!");
                            check = false;
                            break;
                        }

                        if (!AxisMove(_Axis_LeftBend_DWW, -Recipe.LeftBend_DWW_BasePos + pos))
                        {
                            _Axis_LeftBend_DWW.StopSlowly();
                            return false;
                        }

                        if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_DWX }, new double[] { Recipe.LeftBend_DWX_WorkPos }))
                        {
                            _Axis_LeftBend_DWX.StopSlowly();
                            return false;
                        }
                        Thread.Sleep(40);
                    }
                }
                else if (_station == StationType.Mid)
                {
                    bool check = true;
                    short adjcount = 0;
                    while (check)
                    {
                        adjcount++;
                        str_Bend2Rev = "";
                        if (!Bend2SendMsg("B,PZF"))
                        {
                            Bend2CCDNet.StopConnection();
                            Bend2CCDNet.Dispose();
                            Bend2CCDNet.StartConnection();
                            Thread.Sleep(400);
                            OutputError("折弯相机2发送拍照数据失败");
                            WriteErrLog("折弯相机2发送拍照数据失败");
                            if (adjcount > netwaitcount)
                            {
                                pos = 11;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            Stopwatch stt = new Stopwatch();
                            stt.Restart();
                            OutputMessage("等待折弯相机2返回数据!");
                            WriteLog("折弯2 PZF等待返回数据！");
                            while (str_Bend2Rev.Length < 2)
                            {
                                if (_IsStop)
                                {
                                    break;
                                }

                                if (stt.ElapsedMilliseconds > NetTimeOut)
                                {
                                    Bend2CCDNet.StopConnection();
                                    Bend2CCDNet.Dispose();
                                    OpenBuzzer();
                                    Thread.Sleep(200);
                                    CloseBuzzer();
                                    Bend2CCDNet.StartConnection();
                                    OutputError("折弯相机2接收信息超时报警!");
                                    WriteLog("折弯2 PZF超时报警！");
                                    break;
                                }

                                Thread.Sleep(20);
                            }
                            WriteLog(string.Format("折弯2 PZF返回数据{0}！", str_Bend2Rev));
                            if (str_Bend2Rev.Length < 2 || SplitDegreeString(str_Bend2Rev) == -100)
                            {
                                if (adjcount > 5)
                                {
                                    OutputMessage("折弯相机2角度超次数!");
                                    check = false;
                                    break;
                                }

                                pos = -100;
                                Thread.Sleep(20);
                                continue;
                            }
                            else
                            {
                                pos = SplitDegreeString(str_Bend2Rev);
                            }


                        }

                        if (MeasurementContext.Config.Isbend2_CalibProtect && !IsBendAdjuetAngleBetweenLimit(pos, StationType.Mid))
                        {
                            OutputError("中对位角度即将超出保护上下限,校正失败!");
                            return false;
                        }

                        if (Math.Abs(pos) > 10)
                        {
                            return false;
                        }
                        else if (Math.Abs(-Recipe.MidBend_DWW_BasePos + pos) < Config.AdjustAngle)
                        {
                            check = false;
                            break;
                        }
                        else if (adjcount > 5)
                        {
                            OutputError("折弯相机2角度超次数!");
                            check = false;
                            break;
                        }


                        if (!AxisMove(_Axis_MidBend_DWW, -Recipe.MidBend_DWW_BasePos + pos))
                        {
                            _Axis_MidBend_DWW.StopSlowly();
                            return false;
                        }
                        if (!AxisMoveTo(new MeasurementAxis[] { /*_Axis_LeftBend_stgY,*/ _Axis_MidBend_DWX }, new double[] { /*Recipe.LeftBend_Y_WorkPos,*/ Recipe.MidBend_DWX_WorkPos }))
                        {
                            _Axis_MidBend_DWX.StopSlowly();
                            return false;
                        }

                        Thread.Sleep(40);
                    }
                }
                else
                {
                    bool check = true;
                    short adjcount = 0;

                    while (check)
                    {
                        adjcount++;
                        str_Bend3Rev = "";
                        if (!Bend3SendMsg("C,PZF"))
                        {
                            Bend3CCDNet.StopConnection();
                            Bend3CCDNet.Dispose();
                            Bend3CCDNet.StartConnection();
                            Thread.Sleep(400);
                            OutputError("折弯相机3发送拍照数据失败");
                            WriteErrLog("折弯相机3发送拍照数据失败");
                            if (adjcount > netwaitcount)
                            {
                                pos = 11;
                                break;
                            }
                            else
                            {

                                continue;
                            }
                        }
                        else
                        {
                            Stopwatch stt = new Stopwatch();
                            stt.Restart();
                            OutputMessage("等待折弯相机3返回数据!");
                            WriteLog("折弯3 PZF等待返回数据！");
                            while (str_Bend3Rev.Length < 2)
                            {
                                if (_IsStop)
                                {
                                    break;
                                }

                                if (stt.ElapsedMilliseconds > NetTimeOut)
                                {
                                    OutputError("折弯相机3接收信息超时报警!");
                                    WriteLog("折弯3 PZF超时报警！");
                                    break;
                                }
                                Thread.Sleep(20);
                            }
                            WriteLog(string.Format("折弯3 PZF返回数据{0}！", str_Bend3Rev));
                            if (str_Bend3Rev.Length < 2 || SplitDegreeString(str_Bend3Rev) == -100)
                            {
                                if (adjcount > 5)
                                {
                                    OutputMessage("折弯相机3角度超次数!");
                                    check = false;
                                    break;
                                }
                                pos = -100;

                                continue;
                            }
                            else
                            {
                                pos = SplitDegreeString(str_Bend3Rev);
                            }
                        }
                        if (MeasurementContext.Config.Isbend3_CalibProtect && !IsBendAdjuetAngleBetweenLimit(pos, StationType.Right))
                        {
                            OutputError("右对位角度即将超出保护上下限,校正失败!");
                            return false;
                        }

                        if (Math.Abs(pos) > 10)
                        {
                            return false;
                        }
                        else if (Math.Abs(-Recipe.RightBend_DWW_BasePos + pos) < Config.AdjustAngle)
                        {
                            OutputMessage(string.Format("折弯相机3角度次数{0}次!", adjcount));
                            check = false;
                            break;
                        }
                        else if (adjcount > 5)
                        {
                            check = false;
                            OutputError("折弯相机3角度超次数!");
                            break;
                        }

                        if (!AxisMove(_Axis_RightBend_DWW, -Recipe.RightBend_DWW_BasePos + pos))
                        {
                            _Axis_RightBend_DWW.StopSlowly();
                            return false;
                        }
                        if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_DWX }, new double[] { Recipe.RightBend_DWX_WorkPos }))
                        {
                            _Axis_RightBend_DWX.StopSlowly();
                            return false;
                        }
                        Thread.Sleep(40);
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                WriteErrLog(ex.ToString());
                OutputError("角度对位运行出现较严重的错误!!!!!!!!!");
                return false;

            }
        }


        /// <summary>
        /// 对位时 角度W加上补偿值后是否在对位保护上下限之间 防止对位时轴报警
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        private bool IsBendAdjuetAngleBetweenLimit(double poses, StationType station)
        {
            try
            {
                bool res = false;
                switch (station)
                {
                    case StationType.Left:
                        double wpos = 0;
                        Axis_LeftBend_DWW.GetPositionDev(ref wpos);
                        if ((wpos + poses < Recipe.Leftbend_Wlowlimit) ||
                            (wpos + poses > Recipe.Leftbend_WUpperlimit))
                        {
                            return res;
                        }
                        return true;

                    case StationType.Mid:
                        double wpos2 = 0;
                        Axis_MidBend_DWW.GetPositionDev(ref wpos2);
                        if ((wpos2 + poses < Recipe.Midbend_Wlowlimit) ||
                            (wpos2 + poses > Recipe.Midbend_WUpperlimit))
                        {
                            return res;
                        }
                        return true;

                    case StationType.Right:
                        double wpos3 = 0;
                        Axis_RightBend_DWW.GetPositionDev(ref wpos3);
                        if ((wpos3 + poses < Recipe.Rightbend_Wlowlimit) ||
                            (wpos3 + poses > Recipe.Rightbend_WUpperlimit))
                        {
                            return res;
                        }
                        return true;
                    default:
                        return res;
                }
            }
            catch (Exception ex)
            {
                return false;

            }
        }






        double bend1deltx = 0;
        double bend1delty = 0;
        double bend2deltx = 0;
        double bend2delty = 0;
        double bend3deltx = 0;
        double bend3delty = 0;
        // XY模板校正
        private bool RunBendAdjustModel(StationType _station)
        {
            try
            {


                bool result = true;
                if (Config.IsRunNull)
                {
                    Thread.Sleep(200);
                    return true;
                }
                if (_station == StationType.Left)
                {
                    int count = 0;
                    bool loop = true;
                    while (loop)
                    {
                        count++;
                        double[] posXY = SendAdjustXYMsg((int)StationType.Left);

                        bend1deltx = posXY[0];
                        bend1delty = posXY[1];
                        if (Math.Abs(posXY[0]) > 15 || Math.Abs(posXY[1]) > 15)
                        {
                            bend1delty = 0;
                            bend1deltx = 0;
                            OutputMessage("左折弯：XY模板校正失败");
                            if (count > CheckNum)
                            {
                                return false;
                            }
                            else
                            {
                                Thread.Sleep(100);
                                continue;
                            }

                        }

                        if (MeasurementContext.Config.Isbend1_CalibProtect && !IsBendAdjuetModeXYBetweenLimit(posXY, _station))
                        {
                            return false;
                        }



                        if (!_Axis_LeftBend_stgY.Move(posXY[1], Recipe.Bend1adjust_Speed)
                            || !_Axis_LeftBend_DWX.Move(posXY[0], Recipe.Bend1adjust_Speed)
                            || !_Axis_LeftBend_DWY.Move(-posXY[1], Recipe.Bend1adjust_Speed)
                            || !_Axis_LeftBend_CCDX.Move(posXY[0], Recipe.Bend1adjust_Speed))
                        {
                            _Axis_LeftBend_stgY.StopSlowly();
                            _Axis_LeftBend_DWX.StopSlowly();
                            _Axis_LeftBend_DWY.StopSlowly();
                            _Axis_LeftBend_CCDX.StopSlowly();
                            return false;
                        }

                        if (!CheckAxisDone(_Axis_LeftBend_stgY))
                        {
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_LeftBend_DWX))
                        {
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_LeftBend_DWY))
                        {
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_LeftBend_CCDX))
                        {
                            return false;
                        }
                        Thread.Sleep(Config.LeftBLPhotoDelay);
                        loop = false;
                    }
                }
                else if (_station == StationType.Mid)
                {
                    int count = 0;
                    bool loop = true;
                    while (loop)
                    {
                        count++;
                        double[] posXY = SendAdjustXYMsg((int)StationType.Mid);
                        bend2deltx = posXY[0];
                        bend2delty = posXY[1];
                        if (Math.Abs(posXY[0]) > 15 || Math.Abs(posXY[1]) > 15)
                        {
                            bend2delty = 0;
                            bend2deltx = 0;
                            OutputMessage("中折弯：XY模板校正失败");
                            if (count > CheckNum)
                            {
                                return false;
                            }
                            else
                            {
                                Thread.Sleep(100);
                                continue;
                            }
                        }

                        if (MeasurementContext.Config.Isbend2_CalibProtect && !IsBendAdjuetModeXYBetweenLimit(posXY, _station))
                        {
                            return false;
                        }


                        if (!_Axis_MidBend_stgY.Move(posXY[1], Recipe.Bend1adjust_Speed)
                            || !_Axis_MidBend_DWX.Move(posXY[0], Recipe.Bend1adjust_Speed)
                            || !_Axis_MidBend_DWY.Move(-posXY[1], Recipe.Bend1adjust_Speed)
                            || !_Axis_MidBend_CCDX.Move(posXY[0], Recipe.Bend1adjust_Speed))
                        {
                            _Axis_MidBend_stgY.StopSlowly();
                            _Axis_MidBend_DWX.StopSlowly();
                            _Axis_MidBend_DWY.StopSlowly();
                            _Axis_MidBend_CCDX.StopSlowly();
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_MidBend_stgY))
                        {
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_MidBend_DWX))
                        {
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_MidBend_DWY))
                        {
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_MidBend_CCDX))
                        {
                            return false;
                        }
                        Thread.Sleep(Config.MidBLPhotoDelay);
                        loop = false;
                    }
                }
                else
                {
                    int count = 0;
                    bool loop = true; ;
                    while (loop)
                    {
                        count++;

                        double[] posXY = SendAdjustXYMsg((int)StationType.Right);
                        bend3deltx = posXY[0];
                        bend3delty = posXY[1];
                        if (Math.Abs(posXY[0]) > 10 || Math.Abs(posXY[1]) > 10)
                        {
                            bend3delty = 0;
                            bend3deltx = 0;
                            OutputMessage("右 折弯：XY模板校正失败");
                            if (count > CheckNum)
                            {
                                return false;
                            }
                            else
                            {
                                Thread.Sleep(50);
                                continue;
                            }
                        }
                        if (MeasurementContext.Config.Isbend3_CalibProtect && !IsBendAdjuetModeXYBetweenLimit(posXY, _station))
                        {
                            return false;
                        }


                        if (!_Axis_RightBend_stgY.Move(posXY[1], Recipe.Bend3adjust_Speed)
                            || !_Axis_RightBend_DWX.Move(posXY[0], Recipe.Bend3adjust_Speed)
                            || !_Axis_RightBend_DWY.Move(-posXY[1], Recipe.Bend3adjust_Speed)
                            || !_Axis_RightBend_CCDX.Move(posXY[0], Recipe.Bend3adjust_Speed))
                        {
                            _Axis_RightBend_stgY.StopSlowly();
                            _Axis_RightBend_DWX.StopSlowly();
                            _Axis_RightBend_DWY.StopSlowly();
                            _Axis_RightBend_CCDX.StopSlowly();
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_RightBend_stgY))
                        {
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_RightBend_DWX))
                        {
                            return false;
                        }

                        if (!CheckAxisDone(_Axis_RightBend_DWY))
                        {
                            return false;
                        }
                        if (!CheckAxisDone(_Axis_RightBend_CCDX))
                        {
                            return false;
                        }

                        loop = false;
                        Thread.Sleep(Config.RightBLPhotoDelay);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                WriteErrLog(ex.ToString());
                OutputError("XY模板校正运行出现较严重的错误!!!");
                return false;

            }
        }

        //检测夹FPC光纤
        private int CheckFPCOptical(StationType _station)
        {
            int result = 1;
            if (_station == StationType.Left)
            {
                if (Config.FPC1_Optical_Enable)
                {
                    while (!WaitIOMSec(Config.LeftBend_FPCOptical_IOIn, 200, true))
                    {
                        OutputError("左折弯夹FPC光纤报警!", true);
                        DialogResult DRet = ShowMsgChoiceBox("左折弯夹FPC光纤报警\r\n"
                           + "人工取料:点击人工取料后,取走物料!\r\n"
                           + "确认:继续下一步生产!", true, false);
                        if (DRet == DialogResult.Cancel)
                        {
                            if (!AxisMoveTo(Axis_LeftBend_stgY, Recipe.LeftBend_Discharge_Y))
                            {

                                OutputError("左折弯平台运动错误!");
                                return -1;
                            }
                            frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                            frm.IOOut1 = Config.LeftBend_BlowVacuum_IOOut;
                            frm.IOOut2 = Config.LeftBend_SuckVacuum_IOOut;
                            frm.ShowDialog();
                            if (frm.DialogResult == DialogResult.OK)
                            {
                                b_bend1flag = false;
                                return 0;
                            }
                        }
                    }
                }
            }
            else if (_station == StationType.Mid)
            {
                if (Config.FPC2_Optical_Enable)
                {
                    while (!WaitIOMSec(Config.MidBend_FPCOptical_IOIn, 200, true))
                    {
                        OutputError("中折弯夹FPC光纤报警!", true);
                        DialogResult DRet = ShowMsgChoiceBox("中折弯夹FPC光纤报警\r\n"
                           + "人工取料:点击人工取料后,取走物料!\r\n"
                           + "确认:继续下一步生产!", true, false);
                        if (DRet == DialogResult.Cancel)
                        {
                            if (!AxisMoveTo(Axis_MidBend_stgY, Recipe.MidBend_Discharge_Y))
                            {
                                OutputError("中折弯平台运动错误!");
                                return -1;
                            }
                            frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                            frm.IOOut1 = Config.MidBend_BlowVacuum_IOOut;
                            frm.IOOut2 = Config.MidBend_SuckVacuum_IOOut;
                            frm.ShowDialog();
                            if (frm.DialogResult == DialogResult.OK)
                            {
                                b_bend2flag = false;
                                return 0;

                            }
                        }
                    }
                }
            }
            else
            {
                if (Config.FPC3_Optical_Enable)
                {
                    while (!WaitIOMSec(Config.RightBend_FPCOptical_IOIn, 200, true))
                    {
                        OutputError("右折弯夹FPC光纤报警!", true);
                        DialogResult DRet = ShowMsgChoiceBox("右折弯夹FPC光纤报警\r\n"
                           + "人工取料:点击人工取料后,取走物料!\r\n"
                           + "确认:继续下一步生产!", true, false);
                        if (DRet == DialogResult.Cancel)
                        {
                            if (!AxisMoveTo(Axis_RightBend_stgY, Recipe.RightBend_Discharge_Y))
                            {
                                OutputError("右折弯平台Y轴运动错误!");
                                return -1;
                            }

                            frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                            frm.IOOut1 = Config.RightBend_BlowVacuum_IOOut;
                            frm.IOOut2 = Config.RightBend_SuckVacuum_IOOut;
                            frm.ShowDialog();
                            if (frm.DialogResult == DialogResult.OK)
                            {
                                b_bend3flag = false;
                                return 0;
                            }
                        }
                    }
                }
            }
            return result;
        }

        //R轴反折
        private bool RunBend_AxisRWrok(StationType _station)
        {
            bool result = true;
            AxisBase axisStgy = null;
            AxisBase axisDWY = null;
            AxisBase axisR = null;
            double posworkY = 0;
            double posZBY = 0;
            double posR = 0;
            double zbyspeed = 0;
            double delty = 0;

            if (_station == StationType.Left)
            {
                axisStgy = _Axis_LeftBend_stgY;
                axisDWY = _Axis_LeftBend_DWY;
                axisR = _Axis_LeftBend_DWR;

                posworkY = Recipe.LeftBend_Y_WorkPos;
                posZBY = Recipe.LeftBend_CCD_DWY;
                posR = Recipe.LeftBend_DWR_WorkPos;
                zbyspeed = Recipe.LeftZB_Speed;
                delty = bend1delty;
            }
            else if (_station == StationType.Mid)
            {
                axisStgy = _Axis_MidBend_stgY;
                axisDWY = _Axis_MidBend_DWY;
                axisR = _Axis_MidBend_DWR;

                posworkY = Recipe.MidBend_Y_WorkPos;
                posZBY = Recipe.MidBend_CCD_DWY;
                posR = Recipe.MidBend_DWR_WorkPos;
                zbyspeed = Recipe.MidZB_Speed;
                delty = bend2delty;
            }
            else
            {
                axisStgy = _Axis_RightBend_stgY;
                axisDWY = _Axis_RightBend_DWY;
                axisR = _Axis_RightBend_DWR;

                posworkY = Recipe.RightBend_Y_WorkPos;
                posZBY = Recipe.RightBend_CCD_DWY;
                posR = Recipe.RightBend_DWR_WorkPos;
                zbyspeed = Recipe.RightZB_Speed;
                delty = bend3delty;
            }

            if (!IsOnPosition(axisStgy, posworkY))
            {
                if (!AxisMoveTo(axisStgy, posworkY))
                {
                    axisStgy.StopSlowly();
                    return false;
                }
            }

            if (!axisDWY.MoveTo(posZBY - delty, zbyspeed) || !AxisMoveTo(axisR, posR))
            {
                axisDWY.StopSlowly();
                axisR.StopSlowly();
                return false;
            }

            if (!CheckAxisDone(axisDWY))
            {
                return false;
            }

            if (!CheckAxisDone(axisR))
            {
                return false;
            }


            if (_station == StationType.Left) Thread.Sleep(Config.LeftRotateMoveDelay);
            if (_station == StationType.Mid) Thread.Sleep(Config.MidRotateMoveDelay);
            if (_station == StationType.Right) Thread.Sleep(Config.RightRotateMoveDelay);
            return result;
        }




        private bool RunBend_AdjustPosition(StationType _station)
        {

            try
            {
                bool result = true;
                if (Config.IsRunNull)
                {
                    Thread.Sleep(200);
                    return true;
                }

                if (_station == StationType.Left)
                {
                    ChartResetChange((int)StationType.Left);
                    double[] pos = new double[] { -100, -100 };
                    left_WeigtVal = 0;
                    int ngcount = 0;
                    for (int i = 0; i < Recipe.BendPara[0].Adj_Num; i++)
                    {

                        while (!_IsAutoRun)
                        {
                            if (IsStop)
                            {
                                return false;
                            }
                            Thread.Sleep(100);
                        }

                        if (!Bend1CCDGetGap(ref pos))
                        {
                            OutputError("左折弯:相机返回数据NG");
                            return false;
                        }
                        OnBend1DWArrived((i + 1).ToString());
                        if (Math.Abs(pos[0] - Recipe.BendPara[0].BaseRate) > 10 || Math.Abs(pos[1] - Recipe.BendPara[1].BaseRate) > 10)
                        {
                            OutputMessage("左折弯:抓边失败");
                            ngcount++;
                            if (ngcount >= CheckNum)
                            {
                                return false;
                            }
                            else
                            {
                                Thread.Sleep(40);
                                continue;
                            }
                        }

                        double[] offetValues = Calculate_DeltXYPure(pos, (int)StationType.Left);
                        ChartUpdateChange(offetValues[0], offetValues[1], (int)StationType.Left);

                        if ((Math.Abs(pos[0] - Recipe.BendPara[0].BaseRate) > Recipe.BendPara[0].ErrAnd) || ((Math.Abs(pos[1] - Recipe.BendPara[1].BaseRate) > Recipe.BendPara[1].ErrAnd)))
                        {
                            double[] poses = Calculate_DeltXY(pos, (int)StationType.Left);

                            if (MeasurementContext.Config.Isbend1_CalibProtect && !IsBendAdjuetXYBetweenLimit(poses, StationType.Left))
                            {
                                return false;
                            }

                            if (!_Axis_LeftBend_DWX.Move(poses[0] * Recipe.BendPara[0].DirValue) || !_Axis_LeftBend_DWY.Move(poses[1] * Recipe.BendPara[1].DirValue))
                            {
                                _Axis_LeftBend_DWX.StopSlowly();
                                _Axis_LeftBend_DWY.StopSlowly();
                                OutputError("左折弯:偏移运动NG");
                                return false;
                            }
                            if (!CheckAxisDone(_Axis_LeftBend_DWX))
                            {
                                return false;
                            }

                            if (!CheckAxisDone(_Axis_LeftBend_DWY))
                            {
                                return false;
                            }
                            //验证是否到限位，到限位下料
                            if (_Axis_LeftBend_DWX.IsELPActived || _Axis_LeftBend_DWY.IsELPActived)
                            {
                                OutputError("左折弯:触发限位感应，对位失败");
                                return false;
                            }
                        }
                        else
                        {
                            OutputMessage(string.Format("拉力1:{0}N", left_WeigtVal));
                            return true;
                        }

                        //拉力检测                  
                        if (Config.IsLoadCell1Enable)
                        {
                            if (Config.Left_LoadCellPdtCout > Config.LoadCellTestInterval)//当前到达到称重间隔 
                            {
                                Thread.Sleep(Config.WeighMeasureDelay);
                                double Newton = 0;
                                int ret = LoadCellWork(ref Newton, LoadCell1Net);
                                if (ret == -1)
                                {
                                    // OutputError("1通讯异常");
                                }
                                else if (ret == 1)
                                {
                                    if (Math.Abs(Newton) > Math.Abs(Recipe.LoadCell1Limit))
                                    {
                                        OutputError("拉力1过大");
                                        return false;
                                    }
                                    else
                                    {
                                        if (left_WeigtVal < Newton)//记录最大值
                                        {
                                            left_WeigtVal = Newton;
                                        }
                                    }
                                }
                                else
                                {
                                    OutputError("称重1没有返回数值!");
                                    return false;
                                }
                            }
                        }


                        if (i == Recipe.BendPara[0].Adj_Num - 1)//对位次数
                        {

                            OutputMessage(string.Format("拉力1:{0}N", left_WeigtVal));
                            OutputMessage("左折弯:对位超次数！");
                            return false;
                        }
                    }
                }
                else if (_station == StationType.Mid)
                {
                    ChartResetChange((int)StationType.Mid);
                    double[] pos = new double[] { -100, -100 };
                    int ngcount = 0;
                    mid_WeigtVal = 0;
                    for (int i = 0; i < Recipe.BendPara[2].Adj_Num; i++)
                    {
                        while (!_IsAutoRun)
                        {
                            if (IsStop)
                            {
                                return false;
                            }
                            Thread.Sleep(100);
                        }

                        if (!Bend2CCDGetGap(ref pos))
                        {
                            OutputError("中折弯:相机返回数据NG");
                            return false;
                        }
                        OnBend2DWArrived((i + 1).ToString());
                        if (Math.Abs(pos[0] - Recipe.BendPara[2].BaseRate) > 10 || Math.Abs(pos[1] - Recipe.BendPara[3].BaseRate) > 10)
                        {
                            OutputMessage(string.Format("中折弯:视觉返回{0},{1}", pos[0], pos[1]));
                            ngcount++;
                            if (ngcount >= CheckNum)
                            {

                                return false;
                            }
                            else
                            {
                                Thread.Sleep(100);
                                continue;
                            }
                        }
                        double[] offetValues = Calculate_DeltXYPure(pos, (int)StationType.Mid);
                        ChartUpdateChange(offetValues[0], offetValues[1], (int)StationType.Mid);

                        //对位还在规格外 继续对位
                        if ((Math.Abs(pos[0] - Recipe.BendPara[2].BaseRate) > Recipe.BendPara[2].ErrAnd) || ((Math.Abs(pos[1] - Recipe.BendPara[3].BaseRate) > Recipe.BendPara[3].ErrAnd)))
                        {



                            double[] poses = Calculate_DeltXY(pos, (int)StationType.Mid);
                            if (MeasurementContext.Config.Isbend2_CalibProtect && !IsBendAdjuetXYBetweenLimit(poses, StationType.Mid))
                            {
                                return false;
                            }
                            WriteLog(string.Format("折弯2 X方向{0}", Recipe.BendPara[2].DirValue));
                            WriteLog(string.Format("折弯2 相对偏移{0}", poses[0] * Recipe.BendPara[2].DirValue));
                            if (!_Axis_MidBend_DWX.Move(poses[0] * Recipe.BendPara[2].DirValue) || !_Axis_MidBend_DWY.Move(poses[1] * Recipe.BendPara[3].DirValue))
                            {
                                _Axis_MidBend_DWX.StopSlowly();
                                _Axis_MidBend_DWY.StopSlowly();
                                OutputError("中折弯:偏移运动NG");
                                return false;
                            }

                            if (!CheckAxisDone(_Axis_MidBend_DWX))
                            {
                                return false;
                            }

                            if (!CheckAxisDone(_Axis_MidBend_DWY))
                            {
                                return false;
                            }


                            if (_Axis_MidBend_DWX.IsELPActived || _Axis_MidBend_DWY.IsELPActived)
                            {
                                OutputError("中折弯:触发限位感应，对位失败");
                                return false;
                            }
                        }
                        else
                        {
                            OutputMessage(string.Format("拉力2:{0}N", mid_WeigtVal));
                            return true;
                        }


                        //拉力检测                  
                        if (Config.IsLoadCell2Enable)
                        {
                            if (Config.Mid_LoadCellPdtCout > Config.LoadCellTestInterval)//当前到达到称重间隔 
                            {
                                Thread.Sleep(Config.WeighMeasureDelay);
                                double Newton = 0;
                                int ret = LoadCellWork(ref Newton, LoadCell2Net);
                                if (ret == -1)
                                {

                                }
                                else if (ret == 1)
                                {
                                    if (Math.Abs(Newton) > Math.Abs(Recipe.LoadCell2Limit))
                                    {
                                        OutputError("拉力2过大");
                                        return false;
                                    }
                                    else
                                    {
                                        if (mid_WeigtVal < Newton) mid_WeigtVal = Newton;
                                    }
                                }
                                else
                                {
                                    OutputError("称重2没有返回数值!");
                                    // OutputMessage(string.Format("拉力2:{0}N--nomal", mid_WeigtVal));
                                    return false;
                                }
                            }
                        }


                        if (i == Recipe.BendPara[2].Adj_Num - 1)
                        {
                            OutputMessage("中折弯:对位超次数！");
                            OutputMessage(string.Format("拉力2:{0}N", mid_WeigtVal));
                            return false;
                        }
                    }
                }
                else
                {
                    ChartResetChange((int)StationType.Right);
                    double[] pos = new double[] { -100, -100 };
                    int ngcount = 0;
                    right_WeigtVal = 0;
                    for (int i = 0; i < Recipe.BendPara[4].Adj_Num; i++)
                    {
                        while (!_IsAutoRun)
                        {
                            if (IsStop)
                            {
                                return false;
                            }
                            Thread.Sleep(100);
                        }

                        if (!Bend3CCDGetGap(ref pos))
                        {
                            OutputError("右折弯:相机返回数据NG");
                            return false;
                        }
                        OnBend3DWArrived((i + 1).ToString());
                        if (Math.Abs(pos[0] - Recipe.BendPara[4].BaseRate) > 10 || Math.Abs(pos[1] - Recipe.BendPara[5].BaseRate) > 10)
                        {
                            OutputMessage(string.Format("右折弯:视觉返回{0},{1}", pos[0], pos[1]));
                            ngcount++;
                            if (ngcount >= CheckNum)
                            {
                                return false;
                            }
                            else
                            {
                                Thread.Sleep(50);
                                continue;
                            }
                        }
                        double[] offetValues = Calculate_DeltXYPure(pos, (int)StationType.Right);
                        ChartUpdateChange(offetValues[0], offetValues[1], (int)StationType.Right);
                        if ((Math.Abs(pos[0] - Recipe.BendPara[4].BaseRate) > Recipe.BendPara[4].ErrAnd) || ((Math.Abs(pos[1] - Recipe.BendPara[5].BaseRate) > Recipe.BendPara[5].ErrAnd)))
                        {
                            double[] poses = Calculate_DeltXY(pos, (int)StationType.Right);

                            if (MeasurementContext.Config.Isbend3_CalibProtect && !IsBendAdjuetXYBetweenLimit(poses, StationType.Right))
                            {
                                return false;
                            }

                            if (!_Axis_RightBend_DWX.Move(poses[0] * Recipe.BendPara[4].DirValue) || !_Axis_RightBend_DWY.Move(poses[1] * Recipe.BendPara[5].DirValue))
                            {
                                _Axis_RightBend_DWX.StopSlowly();
                                _Axis_RightBend_DWY.StopSlowly();
                                OutputError("右折弯：偏移运动NG");
                                return false;
                            }

                            if (!CheckAxisDone(_Axis_RightBend_DWX))
                            {
                                return false;
                            }

                            if (!CheckAxisDone(_Axis_RightBend_DWY))
                            {
                                return false;
                            }

                            if (_Axis_RightBend_DWX.IsELPActived || _Axis_RightBend_DWY.IsELPActived)
                            {
                                OutputError("右折弯:触发限位感应，对位失败");
                                return false;
                            }
                        }
                        else
                        {

                            OutputMessage(string.Format("拉力3:{0}N", right_WeigtVal));
                            return true;
                        }

                        //拉力检测                  
                        if (Config.IsLoadCell3Enable)
                        {
                            if (Config.Right_LoadCellPdtCout > Config.LoadCellTestInterval)//当前到达到称重间隔 
                            {
                                Thread.Sleep(Config.WeighMeasureDelay);
                                double Newton = 0;
                                int ret = LoadCellWork(ref Newton, LoadCell3Net);
                                if (ret == -1)
                                {
                                }
                                else if (ret == 1)
                                {

                                    if (Math.Abs(Newton) > Math.Abs(Recipe.LoadCell3Limit))
                                    {
                                        OutputError("拉力3过大");
                                        return false;
                                    }
                                    else
                                    {
                                        if (right_WeigtVal < Newton) right_WeigtVal = Newton;//记录拉力最大lali值
                                    }
                                }
                                else
                                {
                                    OutputError("称重3没有返回数值!"); ;
                                    return false;
                                }
                            }
                        }
                        if (i == Recipe.BendPara[4].Adj_Num - 1)
                        {
                            OutputMessage(string.Format("拉力3:{0}N", right_WeigtVal));
                            OutputMessage("右折弯:对位超次数！");
                            return false;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                WriteErrLog(ex.ToString());
                OutputError("XY对位出现较严重的错误!!!");

                return false;
            }
        }



        /// <summary>
        /// 对位时 XY加上补偿值后是否在对位保护上下限之间 防止对位时轴报警
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        private bool IsBendAdjuetXYBetweenLimit(double[] poses, StationType station)
        {
            try
            {
                bool res = false;
                switch (station)
                {
                    case StationType.Left:
                        double xpos = 0;
                        double ypos = 0;
                        Axis_LeftBend_DWX.GetPositionDev(ref xpos);
                        Axis_LeftBend_DWY.GetPositionDev(ref ypos);
                        if ((xpos + poses[0] * Recipe.BendPara[0].DirValue < Recipe.Leftbend_Xlowlimit) ||
                            (xpos + poses[0] * Recipe.BendPara[0].DirValue > Recipe.Leftbend_XUpperlimit))
                        {
                            OutputError("左折弯对位X即将超限位保护，对位失败！");
                            return res;
                        }

                        if ((ypos + poses[1] * Recipe.BendPara[1].DirValue < Recipe.Leftbend_Ylowlimit) ||
                          (ypos + poses[1] * Recipe.BendPara[1].DirValue > Recipe.Leftbend_YUpperlimit))
                        {
                            OutputError("左折弯对位Y即将超限位保护，对位失败！");
                            return res;
                        }
                        return true;


                    case StationType.Mid:
                        double xpos2 = 0;
                        double ypos2 = 0;
                        Axis_MidBend_DWX.GetPositionDev(ref xpos2);
                        Axis_MidBend_DWY.GetPositionDev(ref ypos2);

                        WriteLog(string.Format("折弯2 X坐标{0}", xpos2));

                        if ((xpos2 + poses[0] * Recipe.BendPara[2].DirValue < Recipe.Midbend_Xlowlimit) ||
                            (xpos2 + poses[0] * Recipe.BendPara[2].DirValue > Recipe.Midbend_XUpperlimit))
                        {
                            OutputError("中折弯对位X即将超限位保护，对位失败！");
                            return res;
                        }

                        if ((ypos2 + poses[1] * Recipe.BendPara[3].DirValue < Recipe.Midbend_Ylowlimit) ||
                          (ypos2 + poses[1] * Recipe.BendPara[3].DirValue > Recipe.Midbend_YUpperlimit))
                        {
                            OutputError("中折弯对位Y即将超限位保护，对位失败！");
                            return res;
                        }



                        return true;

                    case StationType.Right:
                        double xpos3 = 0;
                        double ypos3 = 0;
                        Axis_RightBend_DWX.GetPositionDev(ref xpos3);
                        Axis_RightBend_DWY.GetPositionDev(ref ypos3);
                        if ((xpos3 + poses[0] * Recipe.BendPara[4].DirValue < Recipe.Rightbend_Xlowlimit) ||
                            (xpos3 + poses[0] * Recipe.BendPara[4].DirValue > Recipe.Rightbend_XUpperlimit))
                        {
                            OutputError("右折弯对位X即将超限位保护，对位失败！");
                            return res;
                        }

                        if ((ypos3 + poses[1] * Recipe.BendPara[5].DirValue < Recipe.Rightbend_Ylowlimit) ||
                          (ypos3 + poses[1] * Recipe.BendPara[5].DirValue > Recipe.Rightbend_YUpperlimit))
                        {
                            OutputError("右折弯对位Y即将超限位保护，对位失败！");
                            return res;
                        }
                        return true;
                    default:
                        return res;
                }
            }
            catch (Exception ex)
            {
                return false;

            }


        }

        /// <summary>
        /// 对位时 XY加上补偿值后是否在对位保护上下限之间 防止对位时轴报警
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        private bool IsBendAdjuetModeXYBetweenLimit(double[] poses, StationType station)
        {
            try
            {
                bool res = false;
                switch (station)
                {
                    case StationType.Left:
                        double xpos = 0;
                        double ypos = 0;
                        Axis_LeftBend_DWX.GetPositionDev(ref xpos);
                        Axis_LeftBend_DWY.GetPositionDev(ref ypos);
                        if ((xpos + poses[0]< Recipe.Leftbend_Xlowlimit) ||
                            (xpos + poses[0] > Recipe.Leftbend_XUpperlimit))
                        {
                            OutputError("左折弯对位X即将超限位保护，对位失败！");
                            return res;
                        }

                        if ((ypos + poses[1]  < Recipe.Leftbend_Ylowlimit) ||
                          (ypos + poses[1]  > Recipe.Leftbend_YUpperlimit))
                        {
                            OutputError("左折弯对位Y即将超限位保护，对位失败！");
                            return res;
                        }
                        return true;


                    case StationType.Mid:
                        double xpos2 = 0;
                        double ypos2 = 0;
                        Axis_MidBend_DWX.GetPositionDev(ref xpos2);
                        Axis_MidBend_DWY.GetPositionDev(ref ypos2);

                        WriteLog(string.Format("折弯2 X坐标{0}", xpos2));

                        if ((xpos2 + poses[0]  < Recipe.Midbend_Xlowlimit) ||
                            (xpos2 + poses[0]  > Recipe.Midbend_XUpperlimit))
                        {
                            OutputError("中折弯对位X即将超限位保护，对位失败！");
                            return res;
                        }

                        if ((ypos2 + poses[1]< Recipe.Midbend_Ylowlimit) ||
                          (ypos2 + poses[1]  > Recipe.Midbend_YUpperlimit))
                        {
                            OutputError("中折弯对位Y即将超限位保护，对位失败！");
                            return res;
                        }



                        return true;

                    case StationType.Right:
                        double xpos3 = 0;
                        double ypos3 = 0;
                        Axis_RightBend_DWX.GetPositionDev(ref xpos3);
                        Axis_RightBend_DWY.GetPositionDev(ref ypos3);
                        if ((xpos3 + poses[0]  < Recipe.Rightbend_Xlowlimit) ||
                            (xpos3 + poses[0] > Recipe.Rightbend_XUpperlimit))
                        {
                            OutputError("右折弯对位X即将超限位保护，对位失败！");
                            return res;
                        }

                        if ((ypos3 + poses[1] < Recipe.Rightbend_Ylowlimit) ||
                          (ypos3 + poses[1]> Recipe.Rightbend_YUpperlimit))
                        {
                            OutputError("右折弯对位Y即将超限位保护，对位失败！");
                            return res;
                        }
                        return true;
                    default:
                        return res;
                }
            }
            catch (Exception ex)
            {
                return false;

            }


        }

        private bool RunBend_PressWork(StationType _station)
        {
            bool result = true;
            if (_station == StationType.Left)
            {
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_stgY, _Axis_LeftBend_CCDX }, new double[] { Recipe.LeftBend_PressPt_Y + bend1delty, Recipe.LeftBend_PressPt_X + bend1deltx }))
                {
                    _Axis_LeftBend_stgY.StopSlowly();
                    _Axis_LeftBend_CCDX.StopSlowly();
                    return false;
                }
                if (!Config.IsRunNull)
                {
                    OpenLeftBend_PressCylinder();
                    while (!WaitIOMSec(Config.LeftBend_PressCylinder_DownIOIn, 3000, true))
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "左折弯压合气缸下感应位报警！";
                        fra.ShowDialog();
                    }
                    Thread.Sleep((int)Recipe.LeftYB_Time);
                    CloseLeftBend_PressCylinder();
                    while (!WaitIOMSec(Config.LeftBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "左折弯压合气缸上感应位报警！";
                        fra.ShowDialog();
                    }
                }
            }
            else if (_station == StationType.Mid)
            {
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_stgY, _Axis_MidBend_CCDX },
                               new double[] { Recipe.MidBend_PressPt_Y + bend2delty, Recipe.MidBend_PressPt_X + bend2deltx }))
                {
                    _Axis_MidBend_stgY.StopSlowly();
                    _Axis_MidBend_CCDX.StopSlowly();
                    return false;
                }
                OnBend2MsgArrived("开始压合");
                if (!Config.IsRunNull)
                {
                    OpenMidBend_PressCylinder();
                    while (!WaitIOMSec(Config.MidBend_PressCylinder_DownIOIn, 3000, true))
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "中折弯压合气缸下感应位报警！";
                        fra.ShowDialog();
                    }
                    Thread.Sleep((int)Recipe.MidYB_Time);

                    CloseMidBend_PressCylinder();
                    while (!WaitIOMSec(Config.MidBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "中折弯压合气缸上感应位报警！";
                        fra.ShowDialog();
                    }
                }
            }
            else
            {
                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_stgY, _Axis_RightBend_CCDX },
                               new double[] { Recipe.RightBend_PressPt_Y + bend3delty, Recipe.RightBend_PressPt_X + bend3deltx }))
                {
                    _Axis_RightBend_stgY.StopSlowly();
                    _Axis_RightBend_CCDX.StopSlowly();
                    return false;
                }
                if (!Config.IsRunNull)
                {
                    OpenRightBend_PressCylinder();
                    while (!WaitIOMSec(Config.RightBend_PressCylinder_DownIOIn, 3000, true))
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "右折弯压合气缸下感应位报警！";
                        fra.ShowDialog();
                    }
                    Thread.Sleep((int)Recipe.RightYB_Time);
                    CloseRightBend_PressCylinder();
                    while (!WaitIOMSec(Config.RightBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        FrAlarm fra = new FrAlarm();
                        fra.lblmsg.Text = "右折弯压合气缸上感应位报警！";
                        fra.ShowDialog();
                    }
                }
            }
            return result;
        }

        private bool RunBend_AOIWork(StationType _station)
        {
            bool result = true;
            double[] pos = new double[] { -100, -100, -100, -100, -100 };
            if (_station == StationType.Left)
            {

                if (!Config.IsBend1Free && !Bend1GetAOIResult(ref pos))//直通不进行AOI
                {
                    return false;
                }
                else
                {
                    if (!Config.IsBend1Free && (Math.Abs(pos[1] - Recipe.AOIY1) > Recipe.AOIY1Offset || Math.Abs(pos[3] - Recipe.AOIY2) > Recipe.AOIY2Offset
                       || Math.Abs(pos[0] - Recipe.AOIX1) > Recipe.AOIX1Offset || Math.Abs(pos[2] - Recipe.AOIX2) > Recipe.AOIX2Offset))//直通默认AOI OK
                    {
                        Task.Run(() =>
                        {
                            MeasurementContext.Capacity.AddResult(2, 1, 2);
                            DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "左折弯", pos[0], pos[1], "NG", "折弯后AOI检测NG", left_WeigtVal);
                            SaveRecord(item);
                            AOI1DataCollections.DetectDataItem bend1item = new AOI1DataCollections.DetectDataItem(DateTime.Now.ToString(), pos[0], pos[1], pos[2], pos[3], "NG", left_WeigtVal);
                            SaveBend1Record(bend1item);

                        });
                        return false;
                    }
                    else
                    {

                        if (Config.IsRunNull)
                        {
                            return true;
                        }

                        if (Config.IsBend1Free)
                        {
                            pos = new double[] { 0, 0, 0, 0, 0 };
                        }

                        Task.Run(() =>
                        {
                            MeasurementContext.Capacity.AddResult(1, 1, 0);
                            DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "左折弯", pos[0], pos[1], "OK", " ", left_WeigtVal);
                            SaveRecord(item);
                            AOI1DataCollections.DetectDataItem bend1item = new AOI1DataCollections.DetectDataItem(DateTime.Now.ToString(), pos[0], pos[1], pos[2], pos[3], "OK", left_WeigtVal);
                            SaveBend1Record(bend1item);
                        });
                    }


                }
            }
            else if (_station == StationType.Mid)
            {
                if (!Config.IsBend2Free && !Bend2GetAOIResult(ref pos))
                {
                    return false;
                }
                else
                {
                    if (!Config.IsBend2Free && (Math.Abs(pos[1] - Recipe.MidAOIY1) > Recipe.MidAOIY1Offset || Math.Abs(pos[3] - Recipe.MidAOIY2) > Recipe.MidAOIY2Offset
                       || Math.Abs(pos[0] - Recipe.MidAOIX1) > Recipe.MidAOIX1Offset || Math.Abs(pos[2] - Recipe.MidAOIX2) > Recipe.MidAOIX2Offset))
                    {
                        Task.Run(() =>
                        {
                            MeasurementContext.Capacity.AddResult(2, 2, 2);
                            DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "中折弯", pos[0], pos[1], "NG", "折弯后AOI检测NG", mid_WeigtVal);
                            SaveRecord(item);
                            AOI2DataCollections.DetectDataItem bend2item = new AOI2DataCollections.DetectDataItem(DateTime.Now.ToString(), pos[0], pos[1], pos[2], pos[3], "NG", mid_WeigtVal);
                            SaveBend2Record(bend2item);
                        });
                        return false;
                    }
                    else
                    {
                        if (Config.IsRunNull)
                        {
                            return true;
                        }

                        if (Config.IsBend2Free)
                        {
                            pos = new double[] { 0, 0, 0, 0, 0 };
                        }
                        Task.Run(() =>
                        {
                            MeasurementContext.Capacity.AddResult(1, 2, 0);
                            DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "中折弯", pos[0], pos[1], "OK", " ", mid_WeigtVal);
                            SaveRecord(item);
                            AOI2DataCollections.DetectDataItem bend2item = new AOI2DataCollections.DetectDataItem(DateTime.Now.ToString(), pos[0], pos[1], pos[2], pos[3], "OK", mid_WeigtVal);
                            SaveBend2Record(bend2item);
                        });
                    }
                }
            }
            else
            {
                if (!Config.IsBend3Free && !Bend3GetAOIResult(ref pos))
                {
                    return false;
                }
                else
                {
                    if (!Config.IsBend3Free && (Math.Abs(pos[1] - Recipe.RightAOIY1) > Recipe.RightAOIY1Offset || Math.Abs(pos[3] - Recipe.RightAOIY2) > Recipe.RightAOIY2Offset
                       || Math.Abs(pos[0] - Recipe.RightAOIX1) > Recipe.RightAOIX1Offset || Math.Abs(pos[2] - Recipe.RightAOIX2) > Recipe.RightAOIX2Offset))
                    {
                        Task.Run(() =>
                        {
                            MeasurementContext.Capacity.AddResult(2, 3, 2);
                            DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "右折弯", pos[0], pos[1], "NG", "折弯后AOI检测NG", right_WeigtVal);
                            SaveRecord(item);
                            AOI3DataCollections.DetectDataItem bend3item = new AOI3DataCollections.DetectDataItem(DateTime.Now.ToString(), pos[0], pos[1], pos[2], pos[3], "NG", right_WeigtVal);
                            SaveBend3Record(bend3item);
                        });
                        return false;
                    }
                    else
                    {

                        if (Config.IsRunNull)
                        {
                            return true;
                        }

                        if (Config.IsBend3Free)
                        {
                            pos = new double[] { 0, 0, 0, 0, 0 };
                        }

                        Task.Run(() =>
                        {
                            MeasurementContext.Capacity.AddResult(1, 3, 0);
                            DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "右折弯", pos[0], pos[1], "OK", " ", right_WeigtVal);
                            SaveRecord(item);
                            AOI3DataCollections.DetectDataItem bend3item = new AOI3DataCollections.DetectDataItem(DateTime.Now.ToString(), pos[0], pos[1], pos[2], pos[3], "OK", right_WeigtVal);
                            SaveBend3Record(bend3item);
                        });
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 屏蔽AOI时产品计数
        /// </summary>
        private void RunBend_AOITickCount(StationType _station)
        {
            if (_station == StationType.Left)
            {
                Task.Run(() =>
                {
                    MeasurementContext.Capacity.AddResult(1, 1, 0);
                    DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "左折弯", 0, 0, "OK", " ", 0);
                    SaveRecord(item);
                    AOI1DataCollections.DetectDataItem bend1item = new AOI1DataCollections.DetectDataItem(DateTime.Now.ToString(), 0, 0, 0, 0, "OK", 0);
                    SaveBend1Record(bend1item);
                });

            }
            else if (_station == StationType.Mid)
            {
                Task.Run(() =>
                {
                    MeasurementContext.Capacity.AddResult(1, 2, 0);
                    DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "中折弯", 0, 0, "OK", " ", 0);
                    SaveRecord(item);
                    AOI2DataCollections.DetectDataItem bend2item = new AOI2DataCollections.DetectDataItem(DateTime.Now.ToString(), 0, 0, 0, 0, "OK", 0);
                    SaveBend2Record(bend2item);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    MeasurementContext.Capacity.AddResult(1, 3, 0);
                    DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "右折弯", 0, 0, "OK", " ", 0);
                    SaveRecord(item);
                    AOI3DataCollections.DetectDataItem bend3item = new AOI3DataCollections.DetectDataItem(DateTime.Now.ToString(), 0, 0, 0, 0, "OK", 0);
                    SaveBend3Record(bend3item);
                });

            }

        }


        private bool GoBend_Discharge(StationType _station)
        {
            bool result = true;
            if (_station == StationType.Left)
            {
                double point;

                if (left_OK == EProductAtt.NG_NOTBEND)
                {
                    point = Recipe.LeftBend_NGDischarge_Y;
                }
                else
                {
                    point = Recipe.LeftBend_Discharge_Y;
                }

                if (!AxisMoveTo(_Axis_LeftBend_stgY, point))
                {
                    _Axis_LeftBend_stgY.StopSlowly();
                    return false;
                }

            }
            else if (_station == StationType.Mid)
            {
                double point;
                if (mid_OK == EProductAtt.NG_NOTBEND)
                {
                    point = Recipe.MidBend_NGDischarge_Y;
                }
                else
                {
                    point = Recipe.MidBend_Discharge_Y;
                }

                if (!AxisMoveTo(_Axis_MidBend_stgY, point))
                {
                    _Axis_MidBend_stgY.StopSlowly();
                    return false;
                }
            }
            else
            {
                double point;
                if (right_OK == EProductAtt.NG_NOTBEND)
                {
                    point = Recipe.RightBend_NGDischarge_Y;
                }
                else
                {
                    point = Recipe.RightBend_Discharge_Y;
                }

                if (!AxisMoveTo(_Axis_RightBend_stgY, point))
                {
                    _Axis_RightBend_stgY.StopSlowly();
                    return false;
                }
            }
            return result;

        }
        private bool RunBend_AxisRGoSafePos(StationType _station)
        {
            bool result = true;
            AxisBase axisR = null;
            AxisBase axisDWX = null;
            AxisBase axisStgY = null;
            AxisBase axisDWY = null;
            double posY = 0;
            double posR = 0;
            double posDWX = 0;
            double posDWY = 0;
            if (_station == StationType.Left)
            {
                axisR = _Axis_LeftBend_DWR;
                axisDWX = _Axis_LeftBend_DWX;
                axisStgY = _Axis_LeftBend_stgY;
                posR = Recipe.LeftBend_DWR_SafePos;
                posDWX = Recipe.LeftBend_DWX_SafePos;
                posY = Recipe.LeftBendR_Ypos;

                axisDWY = _Axis_LeftBend_DWY;
                posDWY = Recipe.LeftBend_DWY_SafePos;

                CloseLeftBendClawCylinder();
                Thread.Sleep(80);
            }
            else if (_station == StationType.Mid)
            {
                axisR = _Axis_MidBend_DWR;
                axisDWX = _Axis_MidBend_DWX;
                axisStgY = _Axis_MidBend_stgY;
                posR = Recipe.MidBend_DWR_SafePos;
                posDWX = Recipe.MidBend_DWX_SafePos;
                posY = Recipe.MidBendR_Ypos;

                axisDWY = _Axis_MidBend_DWY;
                posDWY = Recipe.MidBend_DWY_SafePos;

                CloseMidBendClawCylinder();
                Thread.Sleep(80);
            }
            else
            {
                axisR = _Axis_RightBend_DWR;
                axisDWX = _Axis_RightBend_DWX;
                axisStgY = _Axis_RightBend_stgY;

                axisDWY = _Axis_RightBend_DWY;
                posDWY = Recipe.RightBend_DWY_SafePos;

                posR = Recipe.RightBend_DWR_SafePos;
                posDWX = Recipe.RightBend_DWX_SafePos;
                posY = Recipe.RightBendR_Ypos;

                CloseRightBendClawCylinder();
                Thread.Sleep(80);

            }



            if (!AxisMoveTo(new AxisBase[] { axisDWX }, new double[] { posDWX }))
            {
                axisDWX.StopSlowly();
                return false;
            }

            if (!AxisMoveTo(new AxisBase[] { axisStgY, axisDWY }, new double[] { posY, posDWY }))
            {
                axisStgY.StopSlowly();
                axisDWX.StopSlowly();
                return false;
            }
            if (!AxisMoveTo(axisR, posR))
            {
                axisR.StopSlowly();
                return false;
            }

            return result;
        }

        private void ErrADJ_Angle(StationType _station)
        {
            if (_station == StationType.Left)
            {
                MeasurementContext.Capacity.AddResult(2, 1, 1);
                DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "左折弯", 0, 0, "NG", "角度校正NG", 0);
                SaveRecord(item);
                bend1_OK = false;
                left_OK = EProductAtt.NG_NOTBEND;
                Step_LeftBend = 12;
                b_bend1flag = false;
                QueueBendOut.Enqueue(0);
                CloseLeftBend_UPlightController();
            }
            else if (_station == StationType.Mid)
            {
                MeasurementContext.Capacity.AddResult(2, 2, 1);
                DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "中折弯", 0, 0, "NG", "角度校正NG", 0);
                SaveRecord(item);
                bend2_OK = false;
                mid_OK = EProductAtt.NG_NOTBEND;
                Step_MidBend = 12;

                b_bend2flag = false;
                QueueBendOut.Enqueue(1);
                CloseMidBend_UPlightController();



            }
            else
            {
                MeasurementContext.Capacity.AddResult(2, 3, 1);
                DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "右折弯", 0, 0, "NG", "角度校正NG", 0);
                SaveRecord(item);
                bend3_OK = false;
                right_OK = EProductAtt.NG_NOTBEND;
                Step_RightBend = 12;
                b_bend3flag = false;
                QueueBendOut.Enqueue(2);
                CloseRightBend_UPlightController();
            }
        }

        private void ErrADJ_Position(StationType _station)
        {
            if (_station == StationType.Left)
            {

                bend1_OK = false;
                MeasurementContext.Capacity.AddResult(2, 1, 1);
                DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "左折弯", 0, 0, "NG", "对位校正NG", 0);
                SaveRecord(item);
            }
            else if (_station == StationType.Mid)
            {

                bend2_OK = false;
                MeasurementContext.Capacity.AddResult(2, 2, 1);
                DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "中折弯", 0, 0, "NG", "对位校正NG", 0);
                SaveRecord(item);

            }
            else
            {

                bend3_OK = false;
                MeasurementContext.Capacity.AddResult(2, 3, 1);
                DatasCollections.DetectDataItem item = new DatasCollections.DetectDataItem(DateTime.Now.ToString(), "右折弯", 0, 0, "NG", "对位校正NG", 0);
                SaveRecord(item);

            }
        }

        #endregion

        #endregion 

        private bool b_FeedCarry_Safe = true;
        private bool b_DischargeCarry_Safe = true;
        int smload_index = 0;
        public bool RunLoadWork()
        {
            bool result = true;
            _IsLoadWorking = true;
            try
            {
                if (FeedStop)
                {
                    FeedStop = false;
                }
                smload_index = 0;
                Step_load = 0;
                while (_IsLoadWorking)
                {
                    while (!_IsAutoRun)
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        Thread.Sleep(5);
                    }
                    Thread.Sleep(30);

                    switch (Step_load)
                    {
                        case 0:
                            if (FeedStop)
                            {
                                _IsLoadWorking = false;
                                return false;
                            }

                            if (!FeedZWork(0, Recipe.LoadZWaitPos, false) || _IsCycleStop) //整机停止
                            {
                                FlagFeed = false;
                                _IsLoadWorking = false;
                                OutputMessage("上料工位停止!");
                                OnFeedMsgArrived("停止");
                                break;
                            }




                            if (Config.IsFeedCylinderEnable)//启用上料翻转机构
                            {
                                if (_IsLoadMachineReady && IsFeed_RotateCylinder_UP)//直接到取料位置
                                {
                                    if ((!AxisMoveTo(_Axis_Load_X, Recipe.LoadXpos)))
                                    {
                                        OutputError("上料X运动到取料位失败！");
                                        return false;
                                    }
                                }
                                else
                                {
                                    if ((!AxisMoveTo(_Axis_Load_X, Recipe.LoadXWaitPos)))
                                    {
                                        OutputError("上料X运动到待机位失败！");
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                if (!AxisMoveTo(_Axis_Load_X, Recipe.LoadXWaitPos))
                                {
                                    OutputError("上料X运动到待机位失败！");
                                    return false;
                                }
                            }
                            OnFeedMsgArrived("运动到流水线取料位");
                            if (IsLoadSTSuck)
                            {
                                Step_load = 1;
                                break;
                            }

                            if (!GoloadPick())  //取料
                            {                               
                                if (FeedStop)
                                {
                                    _IsLoadWorking = false;
                                    return false;
                                }
                                _IsLoadWorking = false;
                                Step_load = 0;
                            }
                            else
                            {
                                #region 
                                bool flag1 = false;
                                while (!WaitIOMSec(Config.LoadVacuumIOIn, 400, true))
                                {
                                    OutputError("上料真空吸报警!", true);
                                    DialogResult DRet = ShowMsgChoiceBox("上料真空吸报警!\r\n\r\n"
                                     + "人工取料: 人工取走物料\r\n"
                                     + "确认:继续下一步动作!", true, false);
                                    if (DRet == DialogResult.Cancel)
                                    {
                                        CloseLoadSTSuck();
                                        CloseLoadSTFPCSuck();
                                        CloseLoadSTBlow();
                                        Thread.Sleep(50);

                                        if (!FeedZWork(0, Recipe.LoadZWaitPos))
                                        {
                                            return false;
                                        }
                                        frmConfirm frm = new frmConfirm("人工取走料后，按确认，将进行上一步动作!", false, true);
                                        frm.IOOut1 = Config.LoadBlowVacuumIOOut;
                                        frm.IOOut2 = Config.LoadVacuumIOOut;
                                        frm.IOOut3 = Config.LoadFPCVacuumIOOut;
                                        frm.ShowDialog();
                                        if (Config.IsFeedCylinderEnable)
                                        {
                                            if (!AxisMoveTo(_Axis_Load_X, Recipe.LoadXWaitPos))
                                            {
                                                _Axis_Load_X.StopSlowly();
                                                OutputError("上料X运动到待机位失败!");
                                                return false;
                                            }
                                            _IsLoadMachineReady = false;//如果启用上料翻转 则通过该变量代替进料感应光纤
                                        }

                                        if (frm.DialogResult == DialogResult.OK)
                                        {
                                            Step_load = 0;
                                            flag1 = true;
                                            break;
                                        }
                                    }
                                }

                                if (flag1)
                                {
                                    flag1 = false;
                                    break;
                                }

                                while (!WaitIOMSec(Config.LoadfFPCVacuumIOIn, 400, true))
                                {
                                    OutputError("上料FPC真空吸报警!", true);
                                    DialogResult DRet = ShowMsgChoiceBox("上料FPC真空吸报警!\r\n\r\n"
                                     + "人工取料: 人工取走物料\r\n"
                                     + "确认:继续下一步动作!", true, false);
                                    if (DRet == DialogResult.Cancel)
                                    {
                                        CloseLoadSTSuck();
                                        CloseLoadSTFPCSuck();
                                        CloseLoadSTBlow();
                                        Thread.Sleep(50);


                                        if (!FeedZWork(0, Recipe.LoadZWaitPos))
                                        {
                                            return false;
                                        }
                                        frmConfirm frm = new frmConfirm("人工取走料后，按确认，将进行上一步动作!", false, true);
                                        frm.IOOut1 = Config.LoadBlowVacuumIOOut;
                                        frm.IOOut2 = Config.LoadVacuumIOOut;
                                        frm.IOOut3 = Config.LoadFPCVacuumIOOut;
                                        frm.ShowDialog();
                                        if (Config.IsFeedCylinderEnable)
                                        {
                                            if (!AxisMoveTo(_Axis_Load_X, Recipe.LoadXWaitPos))
                                            {
                                                _Axis_Load_X.StopSlowly();
                                                OutputError("上料X运动到待机位失败!");
                                                return false;
                                            }
                                            _IsLoadMachineReady = false;//如果启用上料翻转 则通过该变量代替进料感应光纤
                                        }
                                        if (frm.DialogResult == DialogResult.OK)
                                        {
                                            flag1 = true;
                                            Step_load = 0;
                                            break;
                                        }
                                    }
                                }
                                if (flag1)
                                {
                                    flag1 = true;
                                    break;
                                }
                                #endregion
                                OpenToUpstream_Request();//ZGH20220913增加与上游设备交互
                                while (!FeedZWork(0, Recipe.LoadZWaitPos))
                                {
                                    if (IsStop)
                                    {
                                        return false;
                                    }
                                    FrAlarm fra = new FrAlarm();
                                    fra.lblmsg.Text = "进料动作失败";
                                    fra.ShowDialog();
                                }

                                OpenToUpstream_Finish();//ZGH20220913增加与上游设备交互
                                CloseToUpstream_Request();//ZGH20220913增加与上游设备交互
                                OpenToUpstream_Safe();//ZGH20220913增加与上游设备交互
                                OnFeedMsgArrived("取料完成");

                                Step_load++;
                            }
                            break;

                        case 1:

                            if (FeedStop)
                            {
                                _IsLoadWorking = false;
                                break;
                            }



                            //if (Config.IsScanCodeEnable && (!Config.IsRunNull))
                            //{
                            //    OnFeedMsgArrived("开始扫码");
                            //    string qrcode = RunQRCodeWork();
                            //    // File.AppendAllText(PathCode, DateTime.Now.ToLocalTime().ToString() + ": " + qrcode + "\r\n");
                            //    if (qrcode == "NG")
                            //    {
                            //        OutputError("扫码NG");
                            //    }
                            //}

                            OnFeedMsgArrived("等待撕膜工位空闲");
                            Step_load++;
                            break;

                        case 2:

                            if (FeedStop)
                            {
                                _IsLoadWorking = false;
                                break;
                            }


                            if (Config.IsFeedCylinderEnable && QueueSM.Count == 0)//取完料但撕膜没叫料 先到待机位等待
                            {
                                if (!AxisMoveTo(_Axis_Load_X, Recipe.LoadXWaitPos))
                                {
                                    return false;
                                }
                                _IsLoadMachineReady = false;//如果启用上料翻转 则通过该变量代替进料感应光纤
                            }

                            if (nFeedIndex == 3)
                            {
                                while (QueueSM.Count == 0)
                                {
                                    if (FeedStop)
                                    {
                                        _IsLoadWorking = false;
                                        return false;
                                    }

                                    if (IsStop)
                                    {
                                        _IsLoadWorking = false;
                                        return false;
                                    }
                                    Thread.Sleep(20);
                                }
                                smload_index = QueueSM.Dequeue();
                            }
                            else
                            {
                                smload_index = nFeedIndex;
                            }

                            if (smload_index == 0)
                            {
                                OnFeedMsgArrived("左撕膜放料");
                                if (Config.IsFeedCylinderEnable)//监控上料X位置 离开避让位后就重新叫料
                                {
                                    Task.Run(() =>
                                      {
                                          if (_IsLoadMachineReady)
                                          {
                                              DateTime mark = DateTime.Now;
                                              while (_Axis_Load_X.PositionDev < Recipe.LoadXWaitPos - 1 && (DateTime.Now - mark).TotalMilliseconds < 1000)
                                              {
                                                  Thread.Sleep(10);
                                              }
                                              _IsLoadMachineReady = false;//如果启用上料翻转 则通过该变量代替进料感应光纤
                                              MeasurementContext.OutputMessage("--左上料状态监控完成");
                                          }
                                      });
                                }


                                if (!AxisMoveTo(_Axis_Load_X, Recipe.SMPosition[0].Lsm_loadX))
                                {
                                    _Axis_Load_X.StopSlowly();
                                    return false;
                                }
                                if (!IsFeedRotate_Suck && _IsLoadMachineReady)//极低的概率出现取完料不复位该标志 所以加一个判断
                                {
                                    _IsLoadMachineReady = false;

                                }



                                while (!IsLeftReachloadPos)
                                {
                                    if (FeedStop)
                                    {
                                        nFeedIndex = 0;
                                        _IsLoadWorking = false;
                                        return false;
                                    }


                                    Thread.Sleep(5);
                                    if (_IsStop)
                                    {
                                        _IsLoadWorking = false;
                                        return false;
                                    }
                                }

                                if (!IsOnPosition(_Axis_LeftSM_Y, Recipe.SMPosition[0].Lsm_loadY))
                                {
                                    if (!AxisMoveTo(_Axis_LeftSM_Y, Recipe.SMPosition[0].Lsm_loadY))
                                    {
                                        _Axis_LeftSM_Y.StopSlowly();
                                        return false;
                                    }
                                }



                                if (!FeedZWork(1, Recipe.SMPosition[0].Lsm_LoadZ))
                                {
                                    return false;
                                }

                                Thread.Sleep(Config.RobotDropDelay);
                                OpenLeftSMSuck();
                                CloseLoadSTSuck();
                                CloseLoadSTFPCSuck();
                                CloseLeftSMBlow();
                                OpenLoadSTBlow();
                                Thread.Sleep(Config.RobotBlowDelay);


                                if (!FeedZWork(0, Recipe.LoadZWaitPos))
                                {
                                    return false;
                                }

                                CloseLoadSTBlow();
                                bool flag2 = false;
                                while (!WaitIOMSec(Config.LeFTSMVacuumIOIn, 200, true))
                                {
                                    OutputError("左撕膜平台放料真空报警");
                                    DialogResult DRet = ShowMsgChoiceBox("左撕膜平台放料真空报警\r\n"
                                        + "人工取料:点击人工取料后,取走物料!\r\n"
                                        + "确认:继续下一步生产!", true, false);
                                    if (DRet == DialogResult.Cancel)
                                    {
                                        CloseLeftSMSuck();
                                        CloseLeftSMBlow();
                                        frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                        frm.IOOut1 = Config.LeftSM_StgBlowVacuum_IOOut;
                                        frm.IOOut2 = Config.LeftSM_StgVacuum_IOOut;
                                        frm.ShowDialog();
                                        if (frm.DialogResult == DialogResult.OK)
                                        {
                                            QueueSM.Enqueue(0);

                                            flag2 = true;
                                            Step_load = 0;
                                            break;
                                        }
                                    }
                                }

                                if (flag2)
                                {
                                    flag2 = false;
                                    break;
                                }

                                FlagTear1Have = true;
                                _IsLeftsmloadDone = true;
                                IsLeftReachloadPos = false;
                                CloseToUpstream_Finish();
                                OnFeedMsgArrived("左撕膜放料完成");
                                Step_load = 0;
                            }
                            else if (smload_index == 1)
                            {
                                OnFeedMsgArrived("中撕膜放料");

                                if (Config.IsFeedCylinderEnable)//监控上料X位置 离开避让位后就重新叫料
                                {
                                    Task.Run(() =>
                                     {
                                         if (_IsLoadMachineReady)
                                         {
                                             DateTime mark = DateTime.Now;
                                             while (_Axis_Load_X.PositionDev < Recipe.LoadXWaitPos - 1 && (DateTime.Now - mark).TotalMilliseconds < 1000)
                                             {
                                                 Thread.Sleep(10);
                                             }
                                             _IsLoadMachineReady = false;//如果启用上料翻转 则通过该变量代替进料感应光纤
                                         }
                                     });
                                }

                                if (!AxisMoveTo(_Axis_Load_X, Recipe.SMPosition[1].Lsm_loadX))
                                {
                                    _Axis_Load_X.StopSlowly();
                                    return false;
                                }

                                if (!IsFeedRotate_Suck && _IsLoadMachineReady)//极地的概率出现取完料不复位该标志 所以加一个判断
                                {
                                    _IsLoadMachineReady = false;
                                }

                                while (!IsMidReachloadPos)
                                {
                                    if (FeedStop)
                                    {
                                        nFeedIndex = 1;

                                        _IsLoadWorking = false;
                                        return false;
                                    }

                                    Thread.Sleep(5);
                                    if (_IsStop)
                                    {
                                        _IsLoadWorking = false;
                                        return false;
                                    }
                                }

                                if (!IsOnPosition(_Axis_MidSM_Y, Recipe.SMPosition[1].Lsm_loadY))
                                {
                                    if (!AxisMoveTo(_Axis_MidSM_Y, Recipe.SMPosition[1].Lsm_loadY))
                                    {
                                        _Axis_MidSM_Y.StopSlowly();
                                        return false;
                                    }
                                }


                                if (!FeedZWork(1, Recipe.SMPosition[1].Lsm_LoadZ))
                                {
                                    return false;
                                }

                                Thread.Sleep(Config.RobotDropDelay);
                                CloseMidSMBlow();
                                OpenMidSMSuck();
                                CloseLoadSTSuck();
                                CloseLoadSTFPCSuck();
                                OpenLoadSTBlow();
                                Thread.Sleep(Config.RobotBlowDelay);


                                if (!FeedZWork(0, Recipe.LoadZWaitPos))
                                {
                                    return false;
                                }
                                CloseLoadSTBlow();


                                bool flag3 = false;
                                while (!WaitIOExMSec(Config.MidSMVacuumIOInEx))
                                {
                                    OutputError("中撕膜放料真空报警");
                                    DialogResult DRet = ShowMsgChoiceBox("中撕膜放料真空报警\r\n\r\n"
                                        + "人工取料:点击人工取料后,取走物料!\r\n"
                                        + "确认:继续下一步生产!", true, false);

                                    if (DRet == DialogResult.Cancel)
                                    {
                                        CloseMidSMSuck();
                                        CloseMidSMBlow();
                                        frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                        frm.IOOutEx1 = Config.MidSM_StgBlowVacuum_IOOutEx;
                                        frm.IOOut2 = Config.MidSM_StgVacuum_IOOut;
                                        frm.ShowDialog();
                                        if (frm.DialogResult == DialogResult.OK)
                                        {
                                            QueueSM.Enqueue(1);

                                            flag3 = true;
                                            Step_load = 0;
                                            break;
                                        }
                                    }
                                }


                                if (flag3)
                                {
                                    flag3 = false;
                                    break;
                                }

                                FlagTear2Have = true;
                                _IsMidsmloadDone = true;
                                IsMidReachloadPos = false;
                                CloseToUpstream_Finish();
                                OnFeedMsgArrived("中撕膜放料完成");
                                Step_load = 0;
                            }
                            else if (smload_index == 2)
                            {
                                OnFeedMsgArrived("右撕膜放料");


                                if (!b_DischargeCarry_Safe)
                                {
                                    if (Config.IsFeedCylinderEnable)//取完料但撕膜没叫料 先到待机位等待
                                    {
                                        if (!AxisMoveTo(_Axis_Load_X, Recipe.LoadXWaitPos))
                                        {
                                            return false;
                                        }
                                        _IsLoadMachineReady = false;//如果启用上料翻转 则通过该变量代替进料感应光纤
                                    }
                                }


                                while (!b_DischargeCarry_Safe)
                                {
                                    if (_IsStop)
                                    {
                                        _IsLoadWorking = false;
                                        return false;
                                    }
                                    Thread.Sleep(50);
                                }

                                if (Config.IsFeedCylinderEnable)//监控上料X位置 离开避让位后就重新叫料
                                {
                                    Task.Run(() =>
                                       {
                                           if (_IsLoadMachineReady)
                                           {
                                               DateTime mark = DateTime.Now;
                                               while (_Axis_Load_X.PositionDev < Recipe.LoadXWaitPos - 1 && (DateTime.Now - mark).TotalMilliseconds < 1000)
                                               {
                                                   Thread.Sleep(10);
                                               }
                                               _IsLoadMachineReady = false;//如果启用上料翻转 则通过该变量代替进料感应光纤
                                           }
                                       });
                                }


                                b_FeedCarry_Safe = false;
                                if (!AxisMoveTo(_Axis_Load_X, Recipe.SMPosition[2].Lsm_loadX))
                                {
                                    _Axis_Load_X.StopSlowly();
                                    return false;
                                }

                                if (!IsFeedRotate_Suck && _IsLoadMachineReady)//极地的概率出现取完料不复位该标志 所以加一个判断
                                {
                                    _IsLoadMachineReady = false;
                                }

                                while (!IsRightReachloadPos)
                                {
                                    if (FeedStop)
                                    {
                                        nFeedIndex = 2;

                                        _IsLoadWorking = false;
                                        return false;
                                    }

                                    Thread.Sleep(5);
                                    if (_IsStop)
                                    {
                                        _IsLoadWorking = false;
                                        return false;
                                    }
                                }

                                if (!IsOnPosition(_Axis_RightSM_Y, Recipe.SMPosition[2].Lsm_loadY))
                                {
                                    if (!AxisMoveTo(_Axis_RightSM_Y, Recipe.SMPosition[2].Lsm_loadY))
                                    {
                                        _Axis_RightSM_Y.StopSlowly();
                                        return false;
                                    }
                                }
                                if (!FeedZWork(1, Recipe.SMPosition[2].Lsm_LoadZ))
                                {
                                    return false;
                                }


                                Thread.Sleep(Config.RobotDropDelay);
                                CloseRightSMBlow();
                                OpenRightSMSuck();
                                CloseLoadSTSuck();
                                CloseLoadSTFPCSuck();
                                OpenLoadSTBlow();
                                Thread.Sleep(Config.RobotBlowDelay);


                                if (!FeedZWork(0, Recipe.LoadZWaitPos))
                                {
                                    return false;
                                }
                                CloseLoadSTBlow();

                                bool flag4 = false;
                                while (!WaitIOMSec(Config.RightSMVacuumIOIn, 400, true))
                                {
                                    OutputError("右撕膜平台放料真空报警");
                                    DialogResult DRet = ShowMsgChoiceBox("右撕膜平台放料真空报警\r\n\r\n"
                                        + "人工取料:点击人工取料后,取走物料!\r\n"
                                        + "确认:继续下一步生产!", true, false);

                                    if (DRet == DialogResult.Cancel)
                                    {
                                        CloseRightSMSuck();
                                        CloseRightSMBlow();
                                        frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                        frm.IOOutEx1 = Config.RightSM_StgBlowVacuum_IOOutEx;
                                        frm.IOOutEx2 = Config.RightSM_StgVacuum_IOOutEx;
                                        frm.ShowDialog();
                                        if (frm.DialogResult == DialogResult.OK)
                                        {
                                            QueueSM.Enqueue(2);

                                            flag4 = true;
                                            Step_load = 0;
                                            break;
                                        }
                                    }
                                }


                                if (flag4)
                                {
                                    flag4 = false;
                                    break;
                                }
                                FlagTear3 = true;
                                _IsRightsmloadDone = true;
                                IsRightReachloadPos = false;
                                b_FeedCarry_Safe = true;
                                CloseToUpstream_Finish();
                                OnFeedMsgArrived("右撕膜放料完成");

                                Step_load = 0;
                            }
                            Step_load = 0;
                            nFeedIndex = 3;
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + "上料" + ex.ToString());
                return false;
            }
        }
        public bool RunLeftSMWork()
        {
            bool result = true;
            Thread.Sleep(200);
            _IsleftsmWorking = true;
            try
            {
                if (Tear1Stop)
                {
                    //Step_LeftSM = IniHelper.ReadInteger("WorkStep", "Tear1Station", 0);
                    Tear1Stop = false;
                }

                CloseLeftSMRollerUDCyliner(); //开始运行气缸置于安全位置
                CloseLeftSMUDCylinder();
                CloseLeftSMLRCylinder();
                CloseLeftSMFBCylinder();
                Step_LeftSM = 0;



                while (_IsleftsmWorking)
                {
                    while (!_IsAutoRun)
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        Thread.Sleep(5);
                    }

                    Thread.Sleep(30);


                    switch (Step_LeftSM)
                    {
                        case 0:
                            if (GetIOInStatus(Config.LeFTSMVacuumIOIn) && Config.IsLeftSMDisabled)
                            {
                                Step_LeftSM = 4;
                                break;
                            }

                            if (!Config.IsLeftSMDisabled)
                            {
                                OnTear1MsgArrived("左撕膜被屏蔽");
                                if (Tear1Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Tear1Station", Step_LeftSM);
                                    _IsleftsmWorking = false;
                                    return false;
                                }
                                if (_IsCycleStop && (!FlagFeed))
                                {
                                    _IsleftsmWorking = false;
                                    FlagTear1 = false;
                                    FlagTear1Have = false;
                                    OnTear1MsgArrived("停止");
                                }
                                WaitMilliSec(500);
                                Step_LeftSM = 0;
                                break;
                            }

                            OnTear1MsgArrived("运动到上料等待位");
                            CloseLeftSMFBCylinder();
                            CloseLeftSMLRCylinder();

                            if (Tear1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear1Station", Step_LeftSM);
                                _IsleftsmWorking = false;
                                return false;
                            }

                            if (_IsCycleStop && (!FlagFeed))
                            {
                                FlagTear1 = false;
                                OutputMessage("左撕膜工位停止!");
                                OnTear1MsgArrived("停止");
                                _IsleftsmWorking = false;
                                return false;
                            }
                            IsLeftRecheck = false;
                            QueueSM.Enqueue(0);
                            if (!AxisMoveTo(_Axis_LeftSM_Y, Recipe.SMPosition[0].Lsm_loadY))
                            {
                                AlarmWork();
                                OutputError("左撕膜Y轴运动到上料位置报警!");
                                _Axis_LeftSM_Y.StopSlowly();
                                _IsleftsmWorking = false;
                                return false;
                            }
                            else
                            {
                                IsLeftReachloadPos = true;
                            }
                            CloseLeftSMReduce();
                            Step_LeftSM++;
                            break;

                        case 1:
                            OnTear1MsgArrived("等待上料");
                            if (Tear1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear1Station", Step_LeftSM);
                                _IsleftsmWorking = false;
                                return false;
                            }
                            while (!_IsLeftsmloadDone)
                            {
                                if (Tear1Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Tear1Station", Step_LeftSM);
                                    _IsleftsmWorking = false;
                                    return false;
                                }

                                if (_IsCycleStop && (!FlagFeed) && !FlagTear1Have)
                                {
                                    FlagTear1 = false;
                                    FlagTear1Have = false;
                                    _IsleftsmWorking = false;
                                    OutputMessage("左撕膜工位停止");
                                    OnTear1MsgArrived("停止");
                                    break;
                                }
                                Thread.Sleep(5);
                                if (_IsStop)
                                {
                                    _IsleftsmWorking = false;
                                    Step_LeftSM = 0;
                                    break;
                                }
                                Thread.Sleep(20);
                            }
                            if (!_IsStop)
                            {
                                _IsLeftsmloadDone = false;
                                Step_LeftSM++;
                            }
                            break;

                        case 2:
                            if (Tear1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear1Station", Step_LeftSM);
                                _IsleftsmWorking = false;
                                return false;
                            }
                            LeftSMReckeckCurrentCount = 0;
                            LeftSocketReciveSMRecheckItems.Clear();
                            OnTear1MsgArrived("上料气缸对位");

                            if (Config.IsTearFilmCloseVacCalib)//临时关闭平台真空 部分产品对位时开真空会对刮花产品
                            {
                                CloseLeftSMSuck();
                                CloseLeftSMFPCSuck();
                            }
                            else
                            {
                                OpenLeftSMReduce();
                            }

                            Thread.Sleep(100);
                            OpenLeftSMFBCylinder();
                            OpenLeftSMLRCylinder();
                            Thread.Sleep(200);

                            if (!WaitIOMSec(Config.LeftSM_FB_CylinderFrontIOIn, 1000, true))
                            {
                                DialogResult DRet = ShowMsgChoiceBox("左撕膜前后气缸报警\r\n" + "人工取料:点击人工取料后,取走物料!\r\n"
                                    + "确认:继续下一步生产!", true, false);
                                if (DRet == DialogResult.Cancel)
                                {
                                    frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                    frm.IOOut1 = Config.LeftSM_StgBlowVacuum_IOOut;
                                    CloseLeftSMFBCylinder();
                                    CloseLeftSMLRCylinder();
                                    CloseLeftSMSuck();
                                    CloseLeftSMFPCSuck();
                                    frm.ShowDialog();
                                    if (frm.DialogResult == DialogResult.OK)
                                    {
                                        CloseLeftSMBlow();
                                        CloseLeftSMReduce();
                                        FlagTear1Have = false;
                                        Step_LeftSM = 0;
                                        break;
                                    }
                                }
                            }

                            if (!WaitIOMSec(Config.LeftSM_LR_CylinderLeftIOIn, 1000, true))
                            {
                                DialogResult DRet = ShowMsgChoiceBox("左撕膜左右气缸报警\r\n" + "人工取料:点击人工取料后,取走物料!\r\n"
                                    + "确认:继续下一步生产!", true, false);
                                if (DRet == DialogResult.Cancel)
                                {
                                    frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                    frm.IOOut1 = Config.LeftSM_StgBlowVacuum_IOOut;
                                    CloseLeftSMFBCylinder();
                                    CloseLeftSMLRCylinder();
                                    CloseLeftSMSuck();
                                    CloseLeftSMFPCSuck();
                                    frm.ShowDialog();
                                    if (frm.DialogResult == DialogResult.OK)
                                    {
                                        CloseLeftSMBlow();
                                        CloseLeftSMReduce();
                                        FlagTear1Have = false;
                                        Step_LeftSM = 0;
                                        break;
                                    }
                                }
                            }
                            LeftSMReckeckCurrentCount = 0;
                            LeftSocketReciveSMRecheckItems.Clear();



                            Thread.Sleep(Config.LeftSMCylinderAligningDelay);//ZGH20220913增加撕膜气缸对位延时 
                            if (Config.IsTearFilmCloseVacCalib)//恢复真空
                            {
                                OpenLeftSMSuck();
                                OpenLeftSMFPCSuck();
                            }
                            else
                            {
                                CloseLeftSMReduce();
                            }

                            


                            if (Config.BefoTearCheck)
                            {
                                Step_LeftSM = 4;
                            }
                            else
                            {
                                Step_LeftSM++;
                            }
                            break;

                        case 3:

                            if (Tear1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear1Station", Step_LeftSM);
                                _IsleftsmWorking = false;
                                return false;
                            }

                            if (!Config.IsLeftSM_SM_Enable && (!Config.IsRunNull))
                            {
                                OnTear1MsgArrived("开始撕膜");
                                if (!IsLeftGlueHave)
                                {
                                    OutputError("左撕膜胶带缺料");
                                    DialogResult DRet = ShowMsgChoiceBox("左撕膜胶带缺料请及时换料", false, false);
                                }

                                int[] items = null;
                                if (IsLeftRecheck)//重新撕膜
                                {
                                    items = StringTOInt(LeftSocketReciveSMRecheckItems.ToString());//解析撕膜编号
                                    if (items == null || items.Length == 0)
                                    {
                                        CloseLeftSMFPCSuck();
                                        Step_LeftSM++;
                                        break;
                                    }
                                }

                                if ((!IsLeftRecheck && !LeftSMLoop()) || (IsLeftRecheck && !LeftSMLoop(items)))
                                {
                                    AlarmWork();
                                    b_tear1loop = false;
                                    //   OutputError("左工位撕膜报警!", false);
                                    int _ret = ErrSMLoop(StationType.Left);

                                    if (_ret == -1)
                                    {
                                        return false;
                                    }
                                    else if (_ret == 1)
                                    {
                                        CloseLeftSMSuck();
                                        Step_LeftSM = 0;
                                    }
                                    else
                                    {
                                        CloseLeftSMFPCSuck();
                                        OutputMessage("左撕膜:撕膜完成");
                                        Step_LeftSM++;
                                    }
                                }
                                else
                                {
                                    //撕膜统计   tfx 20210916
                                    Config.LeftSMUseCount += 1;
                                    SMTotalUpdateChange(0, Config.LeftSMUseCount);
                                    CloseLeftSMFPCSuck();
                                    OutputMessage("左撕膜:撕膜完成");
                                    Step_LeftSM++;
                                }
                            }
                            else
                            {
                                OutputMessage("左撕膜:撕膜禁用");
                                Step_LeftSM++;
                            }
                            break;
                        case 4:
                            if (Tear1Stop)
                            {

                                _IsleftsmWorking = false;
                                return false;
                            }
                            CloseLeftSMFBCylinder();
                            CloseLeftSMLRCylinder();
                            if (Config.IsLeftSMAOIDisabled)
                            {
                                OnTear1MsgArrived("撕膜AOI");
                                IsLeftRecheck = false;
                                if (!RunSMStation_SMAOIMotion(0, out IsLeftRecheck))
                                {
                                    OutputError("左撕膜:AOI失败");
                                    CloseLeftSMUDCylinder();
                                    _IsleftsmWorking = false;
                                    return false;
                                }
                                else if (IsLeftRecheck)//重新撕膜
                                {

                                    Step_LeftSM = 3;
                                    break;
                                }
                                else
                                {
                                    CloseLeftSMUDCylinder();
                                    LeftSMReckeckCurrentCount = 0;
                                    LeftSocketReciveSMRecheckItems.Clear();
                                    if (!QueueTransferOut.Contains(0))
                                    {
                                        QueueTransferOut.Enqueue(0);
                                    }
                                    Step_LeftSM++;
                                }
                            }
                            else
                            {
                                OnTear1MsgArrived("撕膜AOI禁用");
                                OutputMessage("左撕膜:撕膜AOI禁用");
                                CloseLeftSMUDCylinder();
                                if (!QueueTransferOut.Contains(0))
                                {
                                    QueueTransferOut.Enqueue(0);
                                }
                                Step_LeftSM++;
                            }
                            break;
                        case 5:
                            if (Tear1Stop)
                            {

                                _IsleftsmWorking = false;
                                return false;
                            }
                            OnTear1MsgArrived("运动到下料位置");
                            if (!AxisMoveTo(_Axis_LeftSM_Y, Recipe.SMPosition[0].Lsm_DischargeY))
                            {
                                _Axis_LeftSM_Y.StopSlowly();
                                _IsleftsmWorking = false;
                                AlarmWork();
                                OutputError("左撕膜运动到下料位置失败!");
                                return false;
                            }
                            else
                            {
                                _IsLeftSMDone = true;
                                Step_LeftSM++;
                            }
                            break;
                        case 6:
                            if (Tear1Stop)
                            {

                                _IsleftsmWorking = false;
                                return false;
                            }
                            OnTear1MsgArrived("等待下料");
                            while (!_IsLeftSMOut)
                            {
                                if (Tear1Stop)
                                {

                                    _IsleftsmWorking = false;
                                    return false;
                                }
                                Thread.Sleep(20);
                                if (IsStop)
                                {
                                    _IsleftsmWorking = false;
                                    Step_LeftSM = 0;
                                    return false;
                                }
                            }
                            OnTear1MsgArrived("下料完成");
                            Step_LeftSM = 0;
                            _IsLeftSMOut = false;
                            break;
                        default:
                            break;
                    }


                }
                return result;
            }
            catch (Exception Ex)
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + "撕膜1" + Ex.ToString());
                return false;
            }
        }
        public bool RunMidSMWork()
        {
            bool result = true;
            _IsmidsmWorking = true;
            try
            {
                if (Tear2Stop)
                {
                    Tear2Stop = false;
                }
                CloseMidSMRollerUDCyliner(); //开始运行气缸置于安全位置
                CloseMidSMUDCylinder();
                CloseMidSMLRCylinder();
                CloseMidSMFBCylinder();
                Step_MidSM = 0;



                while (_IsmidsmWorking)
                {
                    while (!_IsAutoRun)
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        Thread.Sleep(5);
                    }
                    Thread.Sleep(30);

                    switch (Step_MidSM)
                    {
                        case 0:
                            if (CanGetIOInStatus(Config.MidSMVacuumIOInEx) && Config.IsMidSMDisabled)
                            {
                                Step_MidSM = 4;
                                break;
                            }


                            if (!Config.IsMidSMDisabled)
                            {
                                OnTear2MsgArrived("中撕膜被屏蔽");
                                if (Tear2Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                    _IsmidsmWorking = false;
                                    return false;
                                }

                                if (_IsCycleStop && (!FlagFeed))
                                {
                                    _IsmidsmWorking = false;
                                    FlagTear2 = false;
                                    FlagTear2Have = false;
                                    OnTear2MsgArrived("停止");
                                }
                                WaitMilliSec(500);
                                Step_MidSM = 0;
                                break;

                            }

                            if (Tear2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                _IsmidsmWorking = false;
                                return false;
                            }


                            OnTear2MsgArrived("运动到上料等待位置");
                            if (_IsCycleStop && (!FlagFeed))
                            {
                                FlagTear2 = false;
                                _IsmidsmWorking = false;
                                OnTear2MsgArrived("停止");
                                break;
                            }
                            IsMidRecheck = false;
                            QueueSM.Enqueue(1);
                            if (!AxisMoveTo(_Axis_MidSM_Y, Recipe.SMPosition[1].Lsm_loadY))
                            {
                                _Axis_MidSM_Y.StopSlowly();
                                _IsmidsmWorking = false;
                            }
                            else
                            {
                                IsMidReachloadPos = true;
                            }
                            CloseMidSMReduce();
                            Step_MidSM++;
                            break;

                        case 1:

                            if (Tear2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                _IsmidsmWorking = false;
                                return false;
                            }
                            CloseMidSMFBCylinder();
                            CloseMidSMLRCylinder();
                            OnTear2MsgArrived("等待上料");
                            while (!_IsMidsmloadDone)
                            {
                                if (Tear2Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                    _IsmidsmWorking = false;
                                    return false;
                                }

                                if (_IsCycleStop && (!FlagFeed))
                                {
                                    FlagTear2 = false;
                                    _IsmidsmWorking = false;
                                    OutputMessage("中撕膜工位停止!");
                                    OnTear2MsgArrived("停止");
                                    break;
                                }
                                Thread.Sleep(5);
                                if (_IsStop)
                                {
                                    _IsmidsmWorking = false;
                                    Step_MidSM = 0;
                                    break;
                                }
                                Thread.Sleep(20);
                            }


                            if (!_IsStop)
                            {
                                _IsMidsmloadDone = false;
                                Step_MidSM++;
                            }
                            break;
                        case 2:

                            if (Tear2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                _IsmidsmWorking = false;
                                return false;
                            }

                            OnTear2MsgArrived("气缸对位");
                            if (Config.IsTearFilmCloseVacCalib)//临时关闭平台真空 部分产品对位时开真空会对刮花产品
                            {
                                CloseMidSMSuck();
                                CloseMidSMFPCSuck();
                            }
                            else
                            {
                                OpenMidSMReduce();
                            }


                            Thread.Sleep(100);
                            OpenMidSMFBCylinder();
                            OpenMidSMLRCylinder();
                            Thread.Sleep(200);

                            if (!WaitIOExMSec(Config.MidSM_FB_CylinderFrontIOInEx))
                            {
                                DialogResult DRet = ShowMsgChoiceBox("中撕膜前后气缸报警\r\n" + "人工取料:点击人工取料后,取走物料!\r\n"
                                    + "确认:继续下一步生产!", true, false);
                                if (DRet == DialogResult.Cancel)
                                {
                                    frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                    frm.IOOutEx1 = Config.MidSM_StgBlowVacuum_IOOutEx;
                                    CloseMidSMFBCylinder();
                                    CloseMidSMLRCylinder();
                                    CloseMidSMSuck();
                                    CloseMidSMFPCSuck();

                                    frm.ShowDialog();
                                    if (frm.DialogResult == DialogResult.OK)
                                    {
                                        CloseMidSMReduce();
                                        CloseMidSMBlow();
                                        FlagTear2Have = false;
                                        Step_MidSM = 0;
                                        break;
                                    }
                                }
                            }


                            if (!WaitIOExMSec(Config.MidSM_LR_CylinerLeftIOInEx))
                            {
                                DialogResult DRet = ShowMsgChoiceBox("中撕膜左右气缸报警\r\n" + "人工取料:点击人工取料后,取走物料!\r\n"
                                    + "确认:继续下一步生产!", true, false);
                                if (DRet == DialogResult.Cancel)
                                {
                                    frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                    frm.IOOutEx1 = Config.MidSM_StgBlowVacuum_IOOutEx;
                                    CloseMidSMFBCylinder();
                                    CloseMidSMLRCylinder();
                                    CloseMidSMSuck();
                                    CloseMidSMFPCSuck();

                                    frm.ShowDialog();
                                    if (frm.DialogResult == DialogResult.OK)
                                    {
                                        CloseMidSMBlow();
                                        CloseMidSMReduce();
                                        FlagTear2Have = false;
                                        Step_MidSM = 0;
                                        break;
                                    }
                                }
                            }
                            MidSMReckeckCurrentCount = 0;
                            MidSocketReciveSMRecheckItems.Clear();

                            CloseMidSMReduce();
                            Thread.Sleep(Config.MidSMCylinderAligningDelay);//ZGH20220913增加撕膜气缸对位延时
                            if (Config.IsTearFilmCloseVacCalib)//恢复真空
                            {
                                OpenMidSMSuck();
                                OpenMidSMFPCSuck();
                            }
                            else
                            {
                                CloseMidSMReduce();
                            }








                            //CloseMidSMLRCylinder();
                            //CloseMidSMFBCylinder();


                            if (Config.BefoTearCheck)
                            {
                                Step_MidSM = 4;
                            }
                            else
                            {
                                Step_MidSM++;
                            }
                            break;
                        case 3:
                            if (Tear2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                _IsmidsmWorking = false;
                                return false;
                            }


                            if (!Config.IsMidSM_SM_Enable && (!Config.IsRunNull))
                            {
                                OnTear2MsgArrived("撕膜开始");
                                if (!IsMidGlueHave)
                                {
                                    OutputError("中撕膜胶带缺料");
                                    DialogResult DRet = ShowMsgChoiceBox("中撕膜胶带缺料，请及时换料", false, false);
                                }

                                int[] items = null;
                                if (IsMidRecheck)//重新撕膜
                                {
                                    items = StringTOInt(MidSocketReciveSMRecheckItems.ToString());//解析撕膜编号
                                    if (items == null || items.Length == 0)
                                    {
                                        CloseMidSMFPCSuck();
                                        Step_MidSM++;
                                        break;
                                    }
                                }



                                if ((!IsMidRecheck && !MidSMLoop()) || (IsMidRecheck && !MidSMLoop(items)))
                                {
                                    // OutputError("中工位撕膜报警!");
                                    b_tear2loop = false;
                                    int _ret = ErrSMLoop(StationType.Mid);
                                    if (_ret == -1)
                                    {
                                        return false;
                                    }
                                    else if (_ret == 1)
                                    {
                                        CloseMidSMSuck();
                                        CloseMidSMBlow();
                                        Step_MidSM = 0;
                                    }
                                    else
                                    {
                                        OutputMessage("中撕膜完成!");
                                        OnTear2MsgArrived("撕膜完成!");
                                        Step_MidSM++;
                                    }

                                }
                                else
                                {
                                    //撕膜统计   tfx 20210916
                                    Config.MidSMUseCount += 1;
                                    SMTotalUpdateChange(1, Config.MidSMUseCount);
                                    CloseMidSMFPCSuck();
                                    OnTear2MsgArrived("撕膜完成");
                                    Step_MidSM++;
                                }
                            }
                            else
                            {
                                OutputMessage("中撕膜:撕膜禁用");
                                OnTear2MsgArrived("撕膜禁用");
                                Step_MidSM++;
                            }
                            break;
                        case 4:
                            if (Tear2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                _IsmidsmWorking = false;
                                return false;
                            }

                            CloseMidSMFBCylinder();
                            CloseMidSMLRCylinder();

                            if (Config.IsMidSMAOIDisabled)
                            {
                                OnTear2MsgArrived("撕膜AOI");
                                IsMidRecheck = false;
                                if (!RunSMStation_SMAOIMotion(1, out IsMidRecheck))
                                {
                                    OutputError("中撕膜AOI失败");
                                    CloseMidSMUDCylinder();
                                    _IsmidsmWorking = false;
                                    return false;
                                }
                                else if (IsMidRecheck)//重新撕膜
                                {
                                    Step_MidSM = 3;
                                    break;
                                }
                                else
                                {
                                    CloseMidSMUDCylinder();
                                    MidSMReckeckCurrentCount = 0;
                                    MidSocketReciveSMRecheckItems.Clear();
                                    if (!QueueTransferOut.Contains(1))
                                    {
                                        QueueTransferOut.Enqueue(1);
                                    }
                                    Step_MidSM++;
                                }
                            }
                            else
                            {
                                OutputMessage("中撕膜：撕膜AOI禁用");
                                OnTear2MsgArrived("撕膜AOI禁用");
                                CloseMidSMUDCylinder();
                                if (!QueueTransferOut.Contains(1))
                                {
                                    QueueTransferOut.Enqueue(1);
                                }
                                Step_MidSM++;
                            }
                            break;
                        case 5:

                            if (Tear2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                _IsmidsmWorking = false;
                                return false;
                            }

                            OnTear2MsgArrived("运动到下料位置");
                            if (!AxisMoveTo(_Axis_MidSM_Y, Recipe.SMPosition[1].Lsm_DischargeY))
                            {
                                _Axis_MidSM_Y.StopSlowly();
                                _IsmidsmWorking = false;
                                OutputError("中撕膜运动到下料位置失败!");
                                return false;
                            }
                            else
                            {
                                _IsMidSMDone = true;
                                Step_MidSM++;
                            }
                            break;
                        case 6:
                            if (Tear2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                _IsmidsmWorking = false;
                                return false;
                            }

                            OnTear2MsgArrived("等待下料");
                            while (!_IsMidSMOut)
                            {
                                if (Tear2Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Tear2Station", Step_MidSM);
                                    _IsmidsmWorking = false;
                                    return false;
                                }
                                Thread.Sleep(20);
                                if (IsStop)
                                {
                                    _IsmidsmWorking = false;
                                    Step_MidSM = 0;
                                    return false;
                                }
                            }
                            OnTear2MsgArrived("下料完成");
                            Step_MidSM = 0;
                            _IsMidSMOut = false;
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + "撕膜2" + ex.ToString());
                return false;
            }
        }

        public bool RunRightSMWork()
        {
            bool result = true;
            _IsrightsmWorking = true;
            try
            {
                if (Tear3Stop)
                {

                    Tear3Stop = false;
                }
                CloseRightSMRollerUDCyliner(); //开始运行气缸置于安全位置
                CloseRightSMUDCylinder();
                CloseRightSMLRCylinder();
                CloseRightSMFBCylinder();
                Step_RightSM = 0;



                while (_IsrightsmWorking)
                {
                    Thread.Sleep(30);

                    while (!_IsAutoRun)
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        Thread.Sleep(5);
                    }

                    switch (Step_RightSM)
                    {
                        case 0:
                            if (GetIOInStatus(Config.RightSMVacuumIOIn) && Config.IsRightSMDisabled)
                            {
                                Step_RightSM = 4;
                                break;
                            }



                            if (!Config.IsRightSMDisabled)
                            {
                                OnTear3MsgArrived("右撕膜被屏蔽");
                                if (Tear3Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                    _IsrightsmWorking = false;
                                    return false;
                                }


                                if (_IsCycleStop && (!FlagFeed))
                                {
                                    _IsrightsmWorking = false;
                                    FlagTear3 = false;
                                    FlagTear3Have = false;
                                    OutputMessage("右撕膜工位停止！");
                                    OnTear3MsgArrived("停止");
                                }
                                WaitMilliSec(500);
                                Step_RightSM = 0;
                                break;
                            }

                            if (Tear3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                _IsrightsmWorking = false;
                                return false;
                            }

                            if (_IsCycleStop && (!FlagFeed) && (!FlagTear3Have))
                            {
                                FlagTear3 = false;
                                _IsrightsmWorking = false;
                                OutputMessage("右撕膜工位停止");
                                OnTear3MsgArrived("停止");
                                break;
                            }
                            IsRightRecheck = false;
                            QueueSM.Enqueue(2);
                            OnTear3MsgArrived("运动到上料位");
                            if (!AxisMoveTo(_Axis_RightSM_Y, Recipe.SMPosition[2].Lsm_loadY))
                            {
                                _Axis_RightSM_Y.StopSlowly();
                                _IsrightsmWorking = false;
                            }
                            else
                            {
                                IsRightReachloadPos = true;
                            }
                            CloseRightSMReduce();
                            Step_RightSM++;
                            break;

                        case 1:
                            if (Tear3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                _IsrightsmWorking = false;
                                return false;
                            }

                            CloseRightSMFBCylinder();
                            CloseRightSMLRCylinder();
                            OnTear3MsgArrived("等待上料");
                            while (!_IsRightsmloadDone)
                            {
                                if (Tear3Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                    _IsrightsmWorking = false;
                                    return false;
                                }

                                Thread.Sleep(20);
                                if (_IsCycleStop && (!FlagFeed))
                                {
                                    FlagTear3 = false;
                                    _IsrightsmWorking = false;
                                    OutputMessage("右撕膜工位停止");
                                    OnTear3MsgArrived("停止");
                                    break;
                                }
                                Thread.Sleep(5);
                                if (_IsStop)
                                {
                                    _IsrightsmWorking = false;
                                    Step_RightSM = 0;
                                    break;
                                }
                            }
                            if (!_IsStop)
                            {
                                _IsRightsmloadDone = false;
                                Step_RightSM++;
                            }
                            break;

                        case 2:
                            if (Tear3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                _IsrightsmWorking = false;
                                return false;
                            }


                            OnTear3MsgArrived("气缸对位");
                            if (Config.IsTearFilmCloseVacCalib)//临时关闭平台真空 部分产品对位时开真空会对刮花产品
                            {
                                CloseRightSMSuck();
                                CloseRightSMFPCSuck();
                            }
                            else
                            {
                                OpenRightSMReduce();
                            }
                            Thread.Sleep(100);
                            OpenRightSMFBCylinder();
                            OpenRightSMLRCylinder();

                            Thread.Sleep(200);

                            if (!WaitIOMSec(Config.RightSM_FB_CylinderFrontIOIn, 1000, true))
                            {
                                DialogResult DRet = ShowMsgChoiceBox("右撕膜前后气缸报警\r\n" + "人工取料:点击人工取料后,取走物料!\r\n"
                                    + "确认:继续下一步生产!", true, false);
                                if (DRet == DialogResult.Cancel)
                                {
                                    frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                    frm.IOOutEx1 = Config.RightSM_StgBlowVacuum_IOOutEx;
                                    CloseRightSMFBCylinder();
                                    CloseRightSMLRCylinder();
                                    CloseRightSMSuck();
                                    CloseRightSMFPCSuck();

                                    frm.ShowDialog();
                                    if (frm.DialogResult == DialogResult.OK)
                                    {
                                        CloseRightSMBlow();
                                        CloseRightSMReduce();
                                        FlagTear3Have = false;
                                        Step_RightSM = 0;
                                        break;
                                    }
                                }
                            }


                            if (!WaitIOMSec(Config.RightSM_LR_CylinerLeftIOIn, 1000, true))
                            {
                                DialogResult DRet = ShowMsgChoiceBox("右撕膜左右气缸报警\r\n" + "人工取料:点击人工取料后,取走物料!\r\n"
                                    + "确认:继续下一步生产!", true, false);
                                if (DRet == DialogResult.Cancel)
                                {
                                    frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                    frm.IOOutEx1 = Config.RightSM_StgBlowVacuum_IOOutEx;
                                    CloseRightSMFBCylinder();
                                    CloseRightSMLRCylinder();
                                    CloseRightSMSuck();
                                    CloseRightSMFPCSuck();

                                    frm.ShowDialog();
                                    if (frm.DialogResult == DialogResult.OK)
                                    {
                                        CloseRightSMBlow();
                                        CloseRightSMReduce();
                                        FlagTear3Have = false;
                                        Step_RightSM = 0;
                                        break;
                                    }
                                }
                            }

                            Thread.Sleep(100);
                            Thread.Sleep(Config.RightSMCylinderAligningDelay);//ZGH20220913增加撕膜气缸对位延时
                            if (Config.IsTearFilmCloseVacCalib)//恢复真空
                            {
                                OpenRightSMSuck();
                                OpenRightSMFPCSuck();
                            }
                            else
                            {
                                CloseRightSMReduce();
                            }


                            //CloseRightSMFBCylinder();
                            //CloseRightSMLRCylinder();
                            RightSMReckeckCurrentCount = 0;
                            RightSocketReciveSMRecheckItems.Clear();

                            if (Config.BefoTearCheck)
                            {
                                Step_RightSM = 4;
                            }
                            else
                            {
                                Step_RightSM++;
                            }
                            break;
                        case 3:
                            if (Tear3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                _IsrightsmWorking = false;
                                return false;
                            }


                            if (!Config.IsRightSM_SM_Enable && (!Config.IsRunNull))
                            {
                                OnTear3MsgArrived("撕膜开始");

                                if (!IsRightGlueHave)
                                {
                                    OutputError("右撕膜胶带缺料");
                                    DialogResult DRet = ShowMsgChoiceBox("右撕膜胶带缺料，请及时换料", false, false);
                                }

                                int[] items = null;
                                if (IsRightRecheck)//重新撕膜
                                {
                                    items = StringTOInt(RightSocketReciveSMRecheckItems.ToString());//解析撕膜编号
                                    if (items == null || items.Length == 0)
                                    {
                                        CloseRightSMFPCSuck();
                                        Step_RightSM++;
                                        break;
                                    }
                                }



                                if ((!IsRightRecheck && !RightSMLoop()) || (IsRightRecheck && !RightSMLoop(items)))
                                {
                                    AlarmWork();
                                    b_tear3loop = false;
                                    //   OutputError("右工位撕膜报警!");
                                    int _ret = ErrSMLoop(StationType.Right);

                                    if (_ret == -1)
                                    {
                                        return false;
                                    }
                                    else if (_ret == 1)
                                    {
                                        CloseRightSMSuck();
                                        Step_RightSM = 0;
                                    }
                                    else
                                    {
                                        OutputMessage("撕膜完成!");
                                        Step_RightSM++;
                                    }
                                }
                                else
                                {
                                    //撕膜统计   tfx 20210916
                                    Config.RightSMUseCount += 1;
                                    SMTotalUpdateChange(2, Config.RightSMUseCount);
                                    OnTear3MsgArrived("撕膜完成");
                                    CloseRightSMFPCSuck();
                                    Step_RightSM++;
                                }
                            }
                            else
                            {
                                OutputMessage("右撕膜禁用!");
                                Step_RightSM++;
                            }
                            break;
                        case 4:
                            if (Tear3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                _IsrightsmWorking = false;
                                return false;
                            }

                            CloseRightSMFBCylinder();
                            CloseRightSMLRCylinder();
                            if (Config.IsMidSMAOIDisabled)
                            {
                                OnTear3MsgArrived("撕膜AOI");

                                IsRightRecheck = false;
                                if (!RunSMStation_SMAOIMotion(2, out IsRightRecheck))
                                {
                                    CloseRightSMUDCylinder();
                                    OutputError("右撕膜AOI失败");
                                    _IsrightsmWorking = false;
                                    return false;
                                }
                                else if (IsRightRecheck)//重新撕膜
                                {
                                    Step_RightSM = 3;
                                    break;
                                }
                                else
                                {

                                    CloseRightSMUDCylinder();
                                    RightSMReckeckCurrentCount = 0;
                                    RightSocketReciveSMRecheckItems.Clear();
                                    if (!QueueTransferOut.Contains(2))
                                    {
                                        QueueTransferOut.Enqueue(2);
                                    }
                                    Step_RightSM++;
                                }
                            }
                            else
                            {
                                OnTear3MsgArrived("撕膜AOI禁用");
                                CloseRightSMUDCylinder();

                                if (!QueueTransferOut.Contains(2))
                                {
                                    QueueTransferOut.Enqueue(2);
                                }
                                Step_RightSM++;
                            }
                            break;
                        case 5:
                            if (Tear3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                _IsrightsmWorking = false;
                                return false;
                            }

                            OnTear3MsgArrived("运动到下料位");
                            if (!AxisMoveTo(_Axis_RightSM_Y, Recipe.SMPosition[2].Lsm_DischargeY))
                            {
                                _Axis_RightSM_Y.StopSlowly();
                                _IsrightsmWorking = false;
                                OutputError("右撕膜运动到下料位置失败!");
                                return false;
                            }
                            else
                            {
                                _IsRightSMDone = true;
                                Step_RightSM++;
                            }
                            break;
                        case 6:
                            if (Tear3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                _IsrightsmWorking = false;
                                return false;
                            }

                            OnTear3MsgArrived("等待下料");
                            while (!_IsRightSMOut)
                            {
                                if (Tear3Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Tear3Station", Step_RightSM);
                                    _IsrightsmWorking = false;
                                    return false;
                                }
                                Thread.Sleep(20);
                                if (IsStop)
                                {
                                    _IsrightsmWorking = false;
                                    Step_RightSM = 0;
                                    return false;
                                }
                            }
                            OnTear3MsgArrived("下料完成");
                            Step_RightSM = 0;
                            _IsRightSMOut = false;
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + "撕膜3" + ex.ToString());
                return false;
            }

        }


        int nQue_TransferOut = 3;
        int nTransferTearIndex = 3;
        int nQue_TransferBend = 3;
        int nTransferBendIndex = 3;
        object TransferLock = new object();
        public bool RunTransferWork()
        {

            bool result = true;
            _IsTransferWorking = true;
            try
            {
                lock (TransferLock)
                {
                    if (TransferStop)
                    {
                        TransferStop = false;
                    }
                    Step_Transfer = 0;
                    TransferZWork(0, Recipe.TransferZSafePos);
                    while (_IsTransferWorking)
                    {
                        Thread.Sleep(30);
                        while (!_IsAutoRun)
                        {
                            if (IsStop)
                            {
                                return false;
                            }
                            Thread.Sleep(5);
                        }
                        switch (Step_Transfer)
                        {
                            case 0:

                                if (GetIOInStatus(Config.TransferVacuumIOIn))
                                {
                                    Step_Transfer = 2;
                                    break;
                                }

                                if (TransferStop)
                                {
                                    _IsTransferWorking = false;
                                    return false;
                                }

                                OnTransferMsgArrived("等待取料指令");
                                if (!FlagTear1 && !FlagTear2 && !FlagTear3 && !FlagTear1Have && !FlagTear2Have && !FlagTear3Have)
                                {
                                    FlagTranfer = false;
                                    _IsTransferWorking = false;
                                    OutputMessage("中转工位停止1");
                                    OnTransferMsgArrived("停止");
                                    break;
                                }

                                if (QueueTransferOut.Count == 0)
                                {

                                    if (!AxisMoveTo(_Axis_Transfer_X, Recipe.TransferXSafePos))
                                    {
                                        _Axis_Transfer_X.StopSlowly();
                                        return false;
                                    }

                                }


                                if (nTransferTearIndex == 3)
                                {
                                    while (QueueTransferOut.Count == 0)
                                    {
                                        if (TransferStop)
                                        {
                                            _IsTransferWorking = false;
                                            return false;
                                        }


                                        Thread.Sleep(20);
                                        if (!FlagTear1 && !FlagTear2 && !FlagTear3 && !FlagTear1Have && !FlagTear2Have && !FlagTear3Have)
                                        {
                                            FlagTranfer = false;
                                            _IsTransferWorking = false;
                                            OutputMessage("中转工位停止2");
                                            OnTransferMsgArrived("停止");
                                            break;
                                        }
                                        if (IsStop)
                                        {
                                            _IsTransferWorking = false;
                                            return false;
                                        }
                                    }
                                }


                                if (!FlagTranfer)
                                {
                                    _IsTransferWorking = false;
                                    OutputMessage("中转工位停止!");
                                    OnTransferMsgArrived("停止");
                                    break;
                                }
                                if (!IsStop)
                                {
                                    Step_Transfer++;
                                }
                                else
                                {
                                    _IsTransferWorking = false;
                                }
                                break;
                            case 1:
                                if (TransferStop)
                                {
                                    _IsTransferWorking = false;
                                    return false;
                                }
                                #region
                                if (nTransferTearIndex == 3)
                                {
                                    nQue_TransferOut = QueueTransferOut.Dequeue();
                                }
                                else
                                {
                                    nQue_TransferOut = nTransferTearIndex;
                                }

                                switch (nQue_TransferOut)
                                {
                                    case 0:
                                        OnTransferMsgArrived("运动到左撕膜工位取料");

                                        while (!TransferZWork(0, Recipe.TransferZSafePos))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }

                                        if (!AxisMoveTo(_Axis_Transfer_X, Recipe.SMPosition[0].Lsm_DischargeX))
                                        {
                                            OutputError("err tranfer go sm out");
                                            _Axis_Transfer_X.StopSlowly();
                                            return false;
                                        }

                                        while (!_IsLeftSMDone)
                                        {
                                            if (TransferStop)
                                            {
                                                nTransferTearIndex = 0;
                                                _IsTransferWorking = false;
                                                return false;
                                            }

                                            if (IsStop)
                                            {
                                                _IsTransferWorking = false;
                                                return false;
                                            }
                                            Thread.Sleep(5);
                                        }
                                        _IsLeftSMDone = false;

                                        OnTransferMsgArrived("左撕膜取料");
                                        CloseLeftSMUDCylinder();
                                        while (!WaitIOMSec(Config.LeftSM_UD_CylinderUPIOIn))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }

                                            FrAlarm fra = new FrAlarm();
                                            fra.lblmsg.Text = "左撕膜上下气缸上感应位报警！";
                                            fra.ShowDialog();
                                        }
                                        Thread.Sleep(20);


                                        while (!TransferZWork(1, Recipe.SMPosition[0].Lsm_DischargeZ))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }

                                        Thread.Sleep(Config.RobotFetchDelay);
                                        OpenTranserSuck();
                                        OpenTransferFPCSuck();
                                        CloseLeftSMSuck();
                                        CloseLeftSMFPCSuck();
                                        OpenLeftSMBlow();
                                        Thread.Sleep(Config.StageBlowDelay);
                                        bool flag1 = false;
                                        while (!WaitIOMSec(Config.TransferVacuumIOIn, 2000, true))
                                        {
                                            OutputError("中转真空吸报警!", true);
                                            DialogResult DRet = ShowMsgChoiceBox("中转真空吸报警!\r\n\r\n"
                                            + "人工取料: 人工取走物料\r\n"
                                            + "确认:继续下一步动作!", true, false);
                                            if (DRet == DialogResult.Cancel)
                                            {
                                                CloseTranserSuck();
                                                CloseTransferFPCSuck();
                                                Thread.Sleep(30);

                                                while (!TransferZWork(0, Recipe.TransferZSafePos))
                                                {
                                                    if (IsStop)
                                                    {
                                                        return false;
                                                    }
                                                }

                                                frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                                frm.IOOut1 = Config.Transfer_Blowvacuum_IOOut;
                                                frm.IOOut2 = Config.Transfer_Suckvacuum_IOOut;
                                                frm.IOOut3 = Config.Transfer_FPCSuckvacuum_IOOut;
                                                frm.ShowDialog();
                                                if (frm.DialogResult == DialogResult.OK)
                                                {
                                                    CloseTransferBlow();
                                                    CloseLeftSMBlow();
                                                    flag1 = true;
                                                    FlagTear1Have = false;
                                                    _IsLeftSMOut = true;
                                                    Step_Transfer = 0;
                                                    SMAOIReslut[0] = 3;
                                                    break;
                                                }
                                            }
                                        }
                                        CloseLeftSMBlow();
                                        if (flag1)
                                        {
                                            flag1 = false;
                                            break;
                                        }


                                        while (!TransferZWork(0, Recipe.TransferZSafePos))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }

                                        while (!WaitIOMSec(Config.TransferFPCVacuumIOIn, 400, true))
                                        {
                                            OutputError("中转FPC真空吸报警!", true);
                                            DialogResult DRet = ShowMsgChoiceBox("中转FPC真空吸报警!\r\n\r\n"
                                            + "人工取料: 人工取走物料\r\n"
                                            + "确认:继续下一步动作!", true, false);
                                            if (DRet == DialogResult.Cancel)
                                            {
                                                CloseTranserSuck();
                                                CloseTransferFPCSuck();
                                                frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                                frm.IOOut1 = Config.Transfer_Blowvacuum_IOOut;
                                                frm.IOOut2 = Config.Transfer_Suckvacuum_IOOut;
                                                frm.IOOut3 = Config.Transfer_FPCSuckvacuum_IOOut;
                                                frm.ShowDialog();
                                                if (frm.DialogResult == DialogResult.OK)
                                                {
                                                    CloseTransferBlow();
                                                    CloseLeftSMBlow();
                                                    flag1 = true;
                                                    _IsLeftSMOut = true;
                                                    Step_Transfer = 0;
                                                    SMAOIReslut[0] = 3;
                                                    break;
                                                }
                                            }
                                        }
                                        FlagTear1Have = false;
                                        if (flag1)
                                        {
                                            flag1 = false;
                                            break;
                                        }
                                        CloseLeftSMBlow();
                                        OnTransferMsgArrived("左撕膜取料完成");


                                        if (SMAOIReslut[0] == 0)
                                        {

                                            QueueTearResult.Enqueue(0);
                                        }
                                        Step_Transfer++;
                                        _IsLeftSMOut = true;
                                        break;
                                    case 1:
                                        OnTransferMsgArrived("运动到中撕膜工位取料");

                                        while (!TransferZWork(0, Recipe.TransferZSafePos))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }

                                        if (!AxisMoveTo(_Axis_Transfer_X, Recipe.SMPosition[1].Lsm_DischargeX))
                                        {
                                            OutputError("err tranfer go sm out");
                                            return false;
                                        }

                                        while (!_IsMidSMDone)
                                        {
                                            if (TransferStop)
                                            {
                                                nTransferTearIndex = 1;
                                                _IsTransferWorking = false;
                                                return false;
                                            }

                                            if (IsStop)
                                            {
                                                break;
                                            }
                                            Thread.Sleep(5);
                                        }
                                        _IsMidSMDone = false;
                                        if (IsStop)
                                        {
                                            _IsTransferWorking = false;
                                            OutputError("手动停止加工");
                                            return false; ;
                                        }

                                        CloseMidSMUDCylinder();
                                        CloseMidSMFBCylinder();
                                        CloseMidSMLRCylinder();


                                        while (!WaitIOExMSec(Config.MidSM_UD_CylinderUPIOInEx, 400, true))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                            FrAlarm fra = new FrAlarm();
                                            fra.lblmsg.Text = "中撕膜上下气缸上感应位报警！";
                                            fra.ShowDialog();
                                        }


                                        while (!TransferZWork(1, Recipe.SMPosition[1].Lsm_DischargeZ))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }
                                        Thread.Sleep(Config.RobotFetchDelay);

                                        OpenTranserSuck();
                                        OpenTransferFPCSuck();
                                        CloseMidSMSuck();
                                        CloseMidSMFPCSuck();
                                        OpenMidSMBlow();
                                        Thread.Sleep(Config.StageBlowDelay);

                                        bool flag2 = false;
                                        while (!WaitIOMSec(Config.TransferVacuumIOIn, 2000, true))
                                        {
                                            OutputError("中转真空吸报警", true);
                                            DialogResult DRet = ShowMsgChoiceBox("中转真空吸报警!\r\n\r\n"
                                                 + "人工取料: 人工取走物料\r\n"
                                                + "确认:继续下一步动作!", true, false);
                                            if (DRet == DialogResult.Cancel)
                                            {
                                                CloseTranserSuck();
                                                CloseTransferFPCSuck();

                                                while (!TransferZWork(0, Recipe.TransferZSafePos))
                                                {
                                                    if (IsStop)
                                                    {
                                                        return false;
                                                    }

                                                }
                                                frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                                frm.IOOut1 = Config.Transfer_Blowvacuum_IOOut;
                                                frm.IOOut2 = Config.Transfer_Suckvacuum_IOOut;
                                                frm.IOOut3 = Config.Transfer_FPCSuckvacuum_IOOut;
                                                frm.ShowDialog();
                                                if (frm.DialogResult == DialogResult.OK)
                                                {
                                                    CloseTransferBlow();
                                                    CloseMidSMBlow();
                                                    flag2 = true;
                                                    _IsMidSMOut = true;
                                                    FlagTear2Have = false;
                                                    Step_Transfer = 0;
                                                    SMAOIReslut[1] = 3;
                                                    break;
                                                }
                                            }
                                        }


                                        CloseMidSMBlow();

                                        if (flag2)
                                        {
                                            flag2 = false;
                                            break;
                                        }




                                        while (!TransferZWork(0, Recipe.TransferZSafePos))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }

                                        }


                                        while (!WaitIOMSec(Config.TransferFPCVacuumIOIn, 200, true))
                                        {
                                            OutputError("中转FPC真空吸报警!", true);
                                            DialogResult DRet = ShowMsgChoiceBox("中转FPC真空吸报警!\r\n\r\n"
                                            + "人工取料: 人工取走物料\r\n"
                                            + "确认:继续下一步动作!", true, false);
                                            if (DRet == DialogResult.Cancel)
                                            {
                                                CloseTranserSuck();
                                                CloseTransferFPCSuck();
                                                frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                                frm.IOOut1 = Config.Transfer_Blowvacuum_IOOut;
                                                frm.IOOut2 = Config.Transfer_Suckvacuum_IOOut;
                                                frm.IOOut3 = Config.Transfer_FPCSuckvacuum_IOOut;
                                                frm.ShowDialog();
                                                if (frm.DialogResult == DialogResult.OK)
                                                {
                                                    CloseTransferBlow();
                                                    flag2 = true;
                                                    _IsMidSMOut = true;
                                                    Step_Transfer = 0;
                                                    SMAOIReslut[1] = 3;
                                                    break;
                                                }
                                            }
                                        }

                                        FlagTear2Have = false;
                                        if (flag2)
                                        {
                                            flag2 = false;
                                            break;
                                        }


                                        OnTransferMsgArrived("中撕膜取料完成");
                                        if (SMAOIReslut[1] == 0)
                                        {

                                            QueueTearResult.Enqueue(0);
                                        }
                                        Step_Transfer++;
                                        _IsMidSMOut = true;

                                        break;
                                    case 2:
                                        OnTransferMsgArrived("运动到右撕膜取料");

                                        while (!TransferZWork(0, Recipe.TransferZSafePos))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }

                                        }

                                        if (!AxisMoveTo(_Axis_Transfer_X, Recipe.SMPosition[2].Lsm_DischargeX))
                                        {
                                            OutputError("err tranfer go sm out");
                                            return false;
                                        }

                                        while (!_IsRightSMDone)
                                        {
                                            if (TransferStop)
                                            {
                                                nTransferTearIndex = 2;
                                                _IsTransferWorking = false;
                                                return false;
                                            }

                                            if (IsStop)
                                            {
                                                break;
                                            }
                                            Thread.Sleep(5);
                                        }

                                        _IsRightSMDone = false;

                                        if (IsStop)
                                        {
                                            _IsTransferWorking = false;
                                            OutputError("手动停止加工");
                                            return false; ;
                                        }

                                        CloseRightSMUDCylinder();
                                        CloseRightSMFBCylinder();
                                        CloseRightSMLRCylinder();

                                        while (!WaitIOMSec(Config.RightSM_UD_CylinderUPIOIn))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                            FrAlarm fra = new FrAlarm();
                                            fra.lblmsg.Text = "右撕膜上下气缸上感应位报警！";
                                            fra.ShowDialog();
                                        }
                                        CloseRedLight();


                                        while (!TransferZWork(1, Recipe.SMPosition[2].Lsm_DischargeZ))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }

                                        Thread.Sleep(Config.RobotFetchDelay);
                                        OpenTranserSuck();
                                        OpenTransferFPCSuck();
                                        CloseRightSMSuck();
                                        CloseRightSMFPCSuck();
                                        OpenRightSMBlow();
                                        Thread.Sleep(Config.RobotBlowDelay);

                                        bool flag3 = false;
                                        while (!WaitIOMSec(Config.TransferVacuumIOIn, 2000, true))
                                        {
                                            OutputError("中转真空吸报警!", true);
                                            DialogResult DRet = ShowMsgChoiceBox("中转真空吸报警!\r\n\r\n"
                                                 + "人工取料: 人工取走物料\r\n"
                                                + "确认:继续下一步动作!", true, false);
                                            if (DRet == DialogResult.Cancel)
                                            {
                                                CloseTranserSuck();
                                                CloseTransferFPCSuck();

                                                while (!TransferZWork(0, Recipe.TransferZSafePos))
                                                {
                                                    if (IsStop)
                                                    {
                                                        return false;
                                                    }

                                                }
                                                frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                                frm.IOOut1 = Config.Transfer_Blowvacuum_IOOut;
                                                frm.IOOut2 = Config.Transfer_Suckvacuum_IOOut;
                                                frm.IOOut3 = Config.Transfer_FPCSuckvacuum_IOOut;
                                                frm.ShowDialog();
                                                if (frm.DialogResult == DialogResult.OK)
                                                {
                                                    CloseTransferBlow();
                                                    CloseRightSMBlow();
                                                    flag3 = true;
                                                    FlagTear3Have = false;
                                                    _IsRightSMOut = true;
                                                    Step_Transfer = 0;
                                                    SMAOIReslut[2] = 3;
                                                    break;
                                                }
                                            }
                                        }

                                        if (flag3)
                                        {
                                            flag3 = false;
                                            break;
                                        }
                                        CloseRightSMBlow();


                                        while (!TransferZWork(0, Recipe.TransferZSafePos))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }

                                        }

                                        while (!WaitIOMSec(Config.TransferFPCVacuumIOIn, 200, true))
                                        {
                                            OutputError("中转FPC真空吸报警!", true);
                                            DialogResult DRet = ShowMsgChoiceBox("中转FPC真空吸报警!\r\n\r\n"
                                            + "人工取料: 人工取走物料\r\n"
                                            + "确认:继续下一步动作!", true, false);
                                            if (DRet == DialogResult.Cancel)
                                            {
                                                CloseTranserSuck();
                                                CloseTransferFPCSuck();
                                                frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                                frm.IOOut1 = Config.Transfer_Blowvacuum_IOOut;
                                                frm.IOOut2 = Config.Transfer_Suckvacuum_IOOut;
                                                frm.IOOut3 = Config.Transfer_FPCSuckvacuum_IOOut;
                                                frm.ShowDialog();
                                                if (frm.DialogResult == DialogResult.OK)
                                                {
                                                    CloseTransferBlow();
                                                    CloseRightSMBlow();
                                                    flag3 = true;
                                                    _IsRightSMOut = true;
                                                    Step_Transfer = 0;
                                                    SMAOIReslut[2] = 3;
                                                    break;
                                                }
                                            }
                                        }
                                        FlagTear3Have = false;
                                        if (flag3)
                                        {
                                            flag3 = false;
                                            break;
                                        }

                                        OnTransferMsgArrived("右撕膜取料完成");
                                        if (SMAOIReslut[2] == 0)
                                        {

                                            QueueTearResult.Enqueue(0);
                                        }
                                        Step_Transfer++;
                                        _IsRightSMOut = true;
                                        break;
                                }
                                #endregion
                                nTransferTearIndex = 3;
                                break;
                            case 2:

                                Step_Transfer++;
                                break;
                            case 3:
                                if (TransferStop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "TransferStation", Step_Transfer);
                                    _IsTransferWorking = false;
                                    return false;
                                }

                                OnTransferMsgArrived("等待折弯工位空闲");
                                if (QueueBend.Count == 0)
                                {
                                    if (!AxisMoveTo(_Axis_Transfer_X, Recipe.TransferXSafePos))
                                    {
                                        return false;
                                    }
                                }

                                if (nTransferBendIndex == 3)
                                {
                                    while (QueueBend.Count == 0)
                                    {
                                        if (TransferStop)
                                        {
                                            _IsTransferWorking = false;
                                            return false;
                                        }

                                        if (IsStop)
                                        {
                                            _IsTransferWorking = false;
                                            Step_Transfer = 0;
                                            return false;
                                        }
                                        Thread.Sleep(20);
                                    }
                                }
                                Step_Transfer++;
                                break;
                            case 4:

                                if (TransferStop)
                                {
                                    _IsTransferWorking = false;
                                    return false;
                                }
                                if (nTransferBendIndex == 3)
                                {
                                    nQue_TransferBend = QueueBend.Dequeue();
                                }
                                else
                                {
                                    nQue_TransferBend = nTransferBendIndex;
                                }
                                #region
                                switch (nQue_TransferBend)
                                {

                                    case 0:

                                        if (b_bend1EnableLoop)
                                        {
                                            Step_Transfer = 3;
                                            b_bend1flag = false;
                                            break;
                                        }

                                        OnTransferMsgArrived("运动到左折弯下料");
                                        if (!AxisMoveTo(_Axis_Transfer_X, Recipe.LeftBend_LoadX))
                                        {
                                            _Axis_Transfer_X.StopSlowly();
                                            _IsTransferWorking = false;
                                            return false;
                                        }

                                        while (!_IsLeftBendReady)
                                        {

                                            if (b_bend1EnableLoop)
                                            {
                                                Step_Transfer = 3;
                                                b_bend1flag = false;
                                                break;
                                            }

                                            if (TransferStop)
                                            {
                                                nTransferBendIndex = 0;

                                                _IsTransferWorking = false;
                                                return false;
                                            }
                                            Thread.Sleep(5);
                                            if (IsStop)
                                            {
                                                _IsTransferWorking = false;
                                                return false;
                                            }
                                        }
                                        _IsLeftBendReady = false;
                                        if (b_bend1EnableLoop)
                                        {
                                            Step_Transfer = 3;
                                            b_bend1flag = false;
                                            break;
                                        }


                                        while (!TransferZWork(1, Recipe.LeftBend_LoadZ))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }


                                        Thread.Sleep(Config.RobotDropDelay);
                                        OpenLeftBendSuck();
                                        Thread.Sleep(50);
                                        CloseTranserSuck();
                                        CloseTransferFPCSuck();

                                        bool flag4 = false;
                                        while (!WaitIOMSec(Config.LeftBend_stgVacuum_IOIn, 2000, true))
                                        {
                                            OutputError("左折弯真空吸报警!", true);
                                            DialogResult DRet = ShowMsgChoiceBox("左折弯真空吸报警!\r\n\r\n"
                                              + "人工取料: 人工取走物料\r\n"
                                              + "确认:继续下一步动作!", true, false);
                                            if (DRet == DialogResult.Cancel)
                                            {
                                                CloseLeftBendSuck();
                                                while (!TransferZWork(0, Recipe.TransferZSafePos))
                                                {
                                                    if (IsStop)
                                                    {
                                                        return false;
                                                    }
                                                }


                                                frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                                frm.IOOut1 = Config.LeftBend_BlowVacuum_IOOut;
                                                frm.IOOut2 = Config.LeftBend_SuckVacuum_IOOut;
                                                frm.ShowDialog();
                                                if (frm.DialogResult == DialogResult.OK)
                                                {
                                                    CloseTransferBlow();
                                                    CloseLeftBendBlow();
                                                    flag4 = true;
                                                    _IsLeftBendReady = true;
                                                    if (Config.IsLeftBendDisabled)
                                                    {
                                                        QueueBend.Enqueue(0);
                                                    }
                                                    else
                                                    {
                                                        b_bend1flag = false;
                                                    }

                                                    break;
                                                }
                                            }
                                        }

                                        if (flag4)
                                        {
                                            flag4 = false;
                                            break;
                                        }

                                        if (!AxisMoveTo(_Axis_LeftBend_DWX, Recipe.LeftBend_DWX_WorkPos))
                                        {
                                            _Axis_LeftBend_DWX.StopSlowly();
                                            _IsTransferWorking = false;
                                            return false;
                                        }

                                        CloseTranserSuck();
                                        CloseTransferFPCSuck();
                                        OpenTransferBlow();
                                        Thread.Sleep(Config.RobotBlowDelay);



                                        while (!TransferZWork(0, Recipe.TransferZSafePos))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }

                                        }

                                        CloseTransferBlow();
                                        OnTransferMsgArrived("左折弯上料完成");
                                        _IsLeftBendUp = true;
                                        break;
                                    case 1:

                                        if (b_bend2EnableLoop)
                                        {
                                            Step_Transfer = 3;
                                            b_bend2flag = false;
                                            break;
                                        }

                                        OnTransferMsgArrived("运动到中折弯准备放料");
                                        if (!AxisMoveTo(_Axis_Transfer_X, Recipe.MidBend_LoadX))
                                        {
                                            _Axis_Transfer_X.StopSlowly();
                                            _IsTransferWorking = false;
                                            return false;
                                        }

                                        while (!_IsMidBendReady)
                                        {
                                            if (b_bend2EnableLoop)
                                            {
                                                Step_Transfer = 3;
                                                b_bend2flag = false;
                                                break;
                                            }

                                            if (TransferStop)
                                            {
                                                nTransferBendIndex = 1;
                                                _IsTransferWorking = false;
                                                return false;
                                            }

                                            Thread.Sleep(5);
                                            if (IsStop)
                                            {
                                                _IsTransferWorking = false;
                                                return false;
                                            }
                                        }
                                        _IsMidBendReady = false;

                                        if (b_bend2EnableLoop)
                                        {
                                            Step_Transfer = 3;
                                            b_bend2flag = false;
                                            break;
                                        }
                                        while (!TransferZWork(1, Recipe.MidBend_LoadZ))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }

                                        Thread.Sleep(Config.RobotDropDelay);
                                        OpenMidBendSuck();
                                        Thread.Sleep(50);
                                        CloseTranserSuck();
                                        CloseTransferFPCSuck();
                                        bool flag5 = false;
                                        while (!WaitIOMSec(Config.MidBend_stgVacuum_IOIn, 500, true))
                                        {
                                            OutputError("中折弯真空吸报警!", true);
                                            DialogResult DRet = ShowMsgChoiceBox("中折弯真空吸报警!\r\n\r\n"
                                              + "人工取料: 人工取走物料\r\n"
                                              + "确认:继续下一步动作!", true, false);
                                            if (DRet == DialogResult.Cancel)
                                            {
                                                CloseMidBendSuck();

                                                while (!TransferZWork(0, Recipe.TransferZSafePos))
                                                {
                                                    if (IsStop)
                                                    {
                                                        return false;
                                                    }

                                                }

                                                frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                                frm.IOOut1 = Config.MidBend_BlowVacuum_IOOut;
                                                frm.IOOut2 = Config.MidBend_SuckVacuum_IOOut;
                                                frm.ShowDialog();
                                                if (frm.DialogResult == DialogResult.OK)
                                                {
                                                    CloseTransferBlow();
                                                    CloseMidBendBlow();
                                                    flag5 = true;
                                                    _IsMidBendReady = true;
                                                    if (Config.IsMidBendDisabled)
                                                    {
                                                        QueueBend.Enqueue(1);
                                                    }
                                                    else
                                                    {
                                                        b_bend2flag = false;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        if (flag5)
                                        {
                                            flag5 = false;
                                            break;
                                        }

                                        if (!AxisMoveTo(_Axis_MidBend_DWX, Recipe.MidBend_DWX_WorkPos))
                                        {
                                            _Axis_MidBend_DWX.StopSlowly();
                                            _IsTransferWorking = false;
                                            return false;
                                        }
                                        CloseTranserSuck();
                                        CloseTransferFPCSuck();
                                        OpenTransferBlow();
                                        Thread.Sleep(Config.RobotBlowDelay);

                                        while (!TransferZWork(0, Recipe.TransferZSafePos))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }

                                        CloseTransferBlow();
                                        OnTransferMsgArrived("运动到中折弯放料完成");
                                        _IsMidBendUp = true;
                                        break;

                                    case 2:

                                        if (b_bend3EnableLoop)
                                        {
                                            Step_Transfer = 3;
                                            b_bend3flag = false;
                                            break;
                                        }

                                        OnTransferMsgArrived("运动到右折弯准备放料");
                                        if (!AxisMoveTo(_Axis_Transfer_X, Recipe.RightBend_LoadX))
                                        {
                                            _Axis_Transfer_X.StopSlowly();
                                            _IsTransferWorking = false;
                                            return false;
                                        }

                                        while (!_IsRightBendReady)
                                        {
                                            if (b_bend3EnableLoop)
                                            {
                                                Step_Transfer = 3;
                                                b_bend3flag = false;
                                                break;
                                            }

                                            if (TransferStop)
                                            {
                                                nTransferBendIndex = 2;
                                                _IsTransferWorking = false;
                                                return false;
                                            }

                                            Thread.Sleep(5);
                                            if (IsStop)
                                            {
                                                _IsTransferWorking = false;
                                                return false;
                                            }
                                        }
                                        _IsRightBendReady = false;

                                        if (b_bend3EnableLoop)
                                        {
                                            Step_Transfer = 3;
                                            b_bend3flag = false;
                                            break;
                                        }

                                        while (!TransferZWork(1, Recipe.RightBend_LoadZ))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }

                                        Thread.Sleep(Config.RobotDropDelay);
                                        OpenRightBendSuck();
                                        Thread.Sleep(50);
                                        CloseTranserSuck();
                                        CloseTransferFPCSuck();

                                        bool flag6 = false;
                                        while (!WaitIOMSec(Config.RightBend_stgVacuum_IOIn, 2000, true))
                                        {
                                            OutputError("右折弯真空吸报警!", true);
                                            DialogResult DRet = ShowMsgChoiceBox("右折弯真空吸报警!\r\n\r\n"
                                             + "人工取料: 人工取走物料\r\n"
                                             + "确认:继续下一步动作!", true, false);
                                            if (DRet == DialogResult.Cancel)
                                            {
                                                CloseRightBendSuck();
                                                while (!TransferZWork(0, Recipe.TransferZSafePos))
                                                {
                                                    if (IsStop)
                                                    {
                                                        return false;
                                                    }
                                                }
                                                frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                                frm.IOOut1 = Config.RightBend_BlowVacuum_IOOut;
                                                frm.IOOut2 = Config.RightBend_SuckVacuum_IOOut;
                                                frm.ShowDialog();
                                                if (frm.DialogResult == DialogResult.OK)
                                                {
                                                    CloseTransferBlow();
                                                    CloseRightBendBlow();
                                                    flag6 = true;
                                                    _IsRightBendReady = true;
                                                    if (Config.IsRightBendDisabled)
                                                    {
                                                        QueueBend.Enqueue(2);
                                                    }
                                                    else
                                                    {
                                                        b_bend3flag = false;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        if (flag6)
                                        {
                                            flag6 = false;
                                            break;
                                        }

                                        if (!AxisMoveTo(_Axis_RightBend_DWX, Recipe.RightBend_DWX_WorkPos))
                                        {
                                            _Axis_RightBend_DWX.StopSlowly();
                                            _IsTransferWorking = false;
                                            return false;
                                        }
                                        CloseTranserSuck();
                                        CloseTransferFPCSuck();
                                        OpenTransferBlow();
                                        Thread.Sleep(Config.RobotBlowDelay);


                                        while (!TransferZWork(0, Recipe.TransferZSafePos))
                                        {
                                            if (IsStop)
                                            {
                                                return false;
                                            }
                                        }

                                        CloseTransferBlow();
                                        OnTransferMsgArrived("右折弯放料完成");
                                        _IsRightBendUp = true;
                                        break;
                                }
                                #endregion


                                if (Step_Transfer != 3) //避免生产过程中屏蔽某一个折弯工位乱跳现象
                                {
                                    Step_Transfer = 0;
                                }
                                nTransferBendIndex = 3;
                                break;
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + "中转" + ex.ToString());
                return false;
            }
        }




        double Bend1_stgy_pos = 0;
        DateTime Bend1XYHomeTime = DateTime.Now;
        public bool RunBend1Work()
        {
            bool result = true;
            _IsleftbendWorking = true;
            try
            {
                if (Bend1Stop)
                {
                    //Step_LeftBend = IniHelper.ReadInteger("WorkStep", "Bend1Station", 0);
                    Bend1Stop = false;
                }
                CloseLeftBend_PressCylinder();
                CloseLeftBendClawCylinder();
                Step_LeftBend = 0;



                if (GetIOInStatus(Config.LeftBend_stgVacuum_IOIn) && Config.IsLeftBendDisabled && !Config.IsBend1Free)//有真空  非直通 直接反折
                {
                    Step_LeftBend = 2;
                }

                if (GetIOInStatus(Config.LeftBend_stgVacuum_IOIn) && Config.IsLeftBendDisabled && Config.IsBend1Free)//启动的时候直通模式下有料直接出料
                {
                    left_OK = EProductAtt.OK;
                    Step_LeftBend = 11;
                }

                while (_IsleftbendWorking)
                {
                    Thread.Sleep(30);
                    while (!_IsAutoRun)
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        Thread.Sleep(5);
                    }

                    switch (Step_LeftBend)
                    {
                        case 0:
                            if (!Config.IsLeftBendDisabled)
                            {
                                b_bend1EnableLoop = true;
                                if (Bend1Stop)
                                {
                                    _IsleftbendWorking = false;
                                    return false;
                                }
                                if (!FlagTranfer)
                                {
                                    FlagBend1 = false;
                                    _IsleftbendWorking = false;
                                    OutputMessage("左折弯工位停止！");
                                    OnBend1MsgArrived("停止");
                                }
                                WaitMilliSec(500);
                                Step_LeftBend = 0;
                                break;
                            }
                            b_bend1EnableLoop = false;


                            if (Bend1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend1Station", Step_LeftBend);
                                _IsleftbendWorking = false;
                                return false;
                            }


                            if (Config.IsBendXYHomeEnable && (DateTime.Now - Bend1XYHomeTime).TotalMinutes > Config.YHomeInterval)
                            {
                                AxisBase[] Axise = new AxisBase[] { _Axis_LeftBend_DWY, _Axis_LeftBend_DWX };

                                bool res = false, stop = false;
                                ThreadPool.QueueUserWorkItem(delegate
                                {
                                    res = _MotionB.GoHome(Axise);
                                    stop = true;
                                });

                                while (!stop)
                                {
                                    Thread.Sleep(50);
                                }

                                if (!(res))
                                {
                                    OutputError("左反折对位XY回原失败!");
                                    return false;
                                }
                                else
                                {

                                    int i = 0;
                                    while (i < Axise.Length)
                                    {
                                        MeasurementAxis axis = Axise[i] as MeasurementAxis;
                                        if (axis.IsHomeActived)
                                        {
                                            axis.PositionDev = 0;
                                            axis.PositionCode = 0;
                                            i++;
                                        }
                                        else
                                        {
                                            OutputError("左反折对位XY回原失败!");
                                            return false;
                                        }
                                    }
                                }
                                Bend1XYHomeTime = DateTime.Now;
                            }





                            OnBend1MsgArrived("运动到上料位");
                            if (!GoBendFeedPos(StationType.Left))
                            {
                                return false;
                            }
                            else
                            {
                                _IsLeftBendReady = true;
                                Step_LeftBend++;
                            }
                            break;
                        case 1:
                            if (Bend1Stop)
                            {
                                _IsleftbendWorking = false;
                                return false;
                            }

                            OnBend1MsgArrived("等待上料");
                            while (!_IsLeftBendUp)
                            {
                                if (Bend1Stop)
                                {

                                    _IsleftbendWorking = false;
                                    return false;
                                }

                                if (!FlagTranfer)
                                {
                                    FlagBend1 = false;
                                    _IsleftbendWorking = false;
                                    OutputMessage("左折弯工位停止");
                                    OnBend1MsgArrived("停止");
                                    return false;
                                }
                                if (IsStop)
                                {
                                    _IsleftbendWorking = false;
                                    return false;
                                }
                                Thread.Sleep(20);
                            }
                            _IsLeftBendUp = false;

                            if (Config.IsLoadCell1Enable)//称重前必须清零
                            {
                                if (Config.Left_LoadCellPdtCout > Config.LoadCellTestInterval)//当前到达到称重间隔
                                {
                                    int r = 0;
                                    Thread.Sleep(Config.WeighResetDelay);
                                    LoadCellResetVal(ref r, LoadCell1Net);
                                    Config.Left_LoadCellPdtCout = 0;
                                }
                                Config.Left_LoadCellPdtCout++;
                            }




                            if (QueueTearResult.Count != 0)
                            {
                                if (QueueTearResult.Dequeue() == 0)
                                {
                                    left_OK = EProductAtt.NG_NOTBEND;
                                    tear1_OK = false;
                                    Step_LeftBend = 11;
                                }
                                else
                                {
                                    if (Config.IsBend1Free)
                                    {
                                        left_OK = EProductAtt.OK;
                                        Step_LeftBend = 11;
                                    }
                                    else
                                    {
                                        Step_LeftBend++;
                                    }
                                }
                            }
                            else
                            {
                                if (Config.IsBend1Free)
                                {
                                    left_OK = EProductAtt.OK;
                                    // QueueBendOut.Enqueue(0);
                                    Step_LeftBend = 11;
                                }
                                else
                                {
                                    Step_LeftBend++;
                                }
                            }
                            break;
                        case 2:
                            if (Bend1Stop)
                            {
                                _IsleftbendWorking = false;
                                return false;
                            }
                            OnBend1MsgArrived("运动到拍照位置");
                            OpenLeftBend_UPlightController();
                            if (!GoBendCCDPos(StationType.Left))
                            {
                                AlarmWork();
                                OutputError("左折弯运动到拍照位置NG");
                                return false;
                            }
                            else
                            {
                                Step_LeftBend++;
                            }
                            break;
                        case 3:
                            if (Bend1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend1Station", Step_LeftBend);
                                _IsleftbendWorking = false;
                                return false;
                            }

                            OnBend1MsgArrived("角度校正");
                            if (!RunBendAdjustAngle(StationType.Left))
                            {
                                AlarmWork();
                                OutputError("左折弯:角度校正失败!");
                                ErrADJ_Angle(StationType.Left);

                                ClearAlarm();
                            }
                            else
                            {
                                Thread.Sleep(50);
                                if (!RunBendAdjustModel(StationType.Left))
                                {
                                    AlarmWork();
                                    OutputError("左折弯:XY模板校正失败");
                                    ErrADJ_Angle(StationType.Left);

                                    ClearAlarm();
                                }
                                else
                                {
                                    Step_LeftBend++;
                                }
                            }
                            break;
                        case 4:
                            if (Bend1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend1Station", Step_LeftBend);
                                _IsleftbendWorking = false;
                                return false;
                            }

                            Bend1_stgy_pos = _Axis_LeftBend_stgY.PositionDev;
                            OnBend1MsgArrived("准备R轴反折");

                            if (Config.IsRunNull)
                            {
                                OpenLeftBendClawCylinder();
                                Step_LeftBend++;
                                break;
                            }

                            int ret = CheckFPCOptical(StationType.Left);
                            if (ret == 0)
                            {
                                Step_LeftBend = 0;
                            }
                            else if (ret == -1)
                            {
                                AlarmWork();
                                _IsleftbendWorking = false;
                                return false;
                            }
                            else
                            {
                                OpenLeftBendClawCylinder();
                                double[] posxy = SendAdjustXYMsg((int)StationType.Left);
                                if (Math.Abs(posxy[0]) > 10 || Math.Abs(posxy[1]) > 10)
                                {
                                    CloseLeftBendClawCylinder();
                                    AlarmWork();
                                    OutputError("左折弯:第二次XY模板抓边失败!");
                                    ErrADJ_Angle(StationType.Left);
                                    Thread.Sleep(100);
                                    ClearAlarm();
                                }
                                else
                                {
                                    Step_LeftBend++;
                                }
                            }
                            break;
                        case 5:
                            if (Bend1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend1Station", Step_LeftBend);
                                _IsleftbendWorking = false;
                                return false;
                            }

                            OnBend1MsgArrived("R轴反折");
                            if (!RunBend_AxisRWrok(StationType.Left))
                            {
                                AlarmWork();
                                OutputError("R轴反折报警", true);
                                _IsleftbendWorking = false;
                                return false;
                            }
                            else
                            {
                                Step_LeftBend++;
                            }
                            break;

                        case 6:
                            if (Bend1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend1Station", Step_LeftBend);
                                _IsleftbendWorking = false;
                                return false;
                            }
                            OnBend1MsgArrived("运动到相机拍照位置");
                            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_stgY/*, _Axis_LeftBend_CCDX*/ },
                                new double[] { (Bend1_stgy_pos - Recipe.LeftBend_CCDPos_Y) + Recipe.LeftBend_CCDPos_Y2/*, Recipe.LeftBend_CCDPos_X*/ }))
                            {
                                OutputError("运动到相机拍照位置失败!");
                                AlarmWork();
                                //  _Axis_LeftBend_CCDX.StopSlowly();
                                _Axis_LeftBend_stgY.StopSlowly();
                                _IsleftbendWorking = false;
                                return false;
                            }
                            else
                            {
                                Step_LeftBend++;
                            }
                            Thread.Sleep(50);
                            break;
                        case 7:
                            if (Bend1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend1Station", Step_LeftBend);
                                _IsleftbendWorking = false;
                                return false;
                            }



                            OnBend1MsgArrived("对位校正");
                            if (!RunBend_AdjustPosition(StationType.Left))
                            {
                                AlarmWork();
                                ErrADJ_Position(StationType.Left);

                                ClearAlarm();
                                left_OK = EProductAtt.NG_NOTBEND;
                                Step_LeftBend = 11;
                            }
                            else
                            {
                                Step_LeftBend++;
                            }
                            break;
                        case 8:
                            if (Bend1Stop)
                            {

                                _IsleftbendWorking = false;
                                return false;
                            }

                            OnBend1MsgArrived("运动到压合位置开始压合");
                            if (!RunBend_PressWork(StationType.Left))
                            {
                                OutputError("左折弯:压合报警!", true);
                                AlarmWork();
                                return false;
                            }
                            else
                            {
                                Step_LeftBend++;
                            }
                            break;
                        case 9:
                            #region
                            if (Bend1Stop)
                            {

                                _IsleftbendWorking = false;
                                return false;
                            }

                            OnBend1MsgArrived("反折R轴返回原位");

                            b_bend1flag = false;
                            if (Config.IsLeftBendDisabled)
                            {
                                b_bend1flag = true;
                                if (!QueueBend.Contains(0))
                                {
                                    QueueBend.Enqueue(0);
                                }

                            }

                            QueueBendOut.Enqueue(0);
                            if (!RunBend_AxisRGoSafePos(StationType.Left))
                            {
                                AlarmWork();
                                OutputError("反折R轴返回原位报警", true);
                                _IsleftbendWorking = false;
                                return false;
                            }
                            else
                            {
                                Step_LeftBend++;
                            }
                            #endregion
                            break;
                        case 10:
                            if (Bend1Stop)
                            {

                                _IsleftbendWorking = false;
                                return false;
                            }

                            if (Config.IsLeftBendAOIDisabled)
                            {

                                OnBend1MsgArrived("反折AOI");
                                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_LeftBend_stgY, _Axis_LeftBend_CCDX },
                                new double[] { Recipe.LeftBend_CCDPos_Y + bend1delty, Recipe.LeftBend_CCDPos_X + bend1deltx }))
                                {
                                    OutputError("运动到相机拍照位置失败!");
                                    AlarmWork();
                                    _Axis_LeftBend_CCDX.StopSlowly();
                                    _Axis_LeftBend_stgY.StopSlowly();
                                    return false;
                                }

                                Thread.Sleep(50);
                                if (Config.IsRunNull)
                                {
                                    left_OK = EProductAtt.OK;

                                    Step_LeftBend = 12;
                                    break;
                                }

                                if (!RunBend_AOIWork(StationType.Left))
                                {
                                    AlarmWork();
                                    OutputError("左折弯:AOI检测NG");

                                    ClearAlarm();
                                    left_OK = EProductAtt.NG;
                                    Step_LeftBend = 11;
                                }
                                else
                                {
                                    left_OK = EProductAtt.OK;
                                    OutputMessage("左折弯：AOI检测OK");
                                    Step_LeftBend++;
                                }

                                if (Bend1Stop)
                                {
                                    _IsleftbendWorking = false;
                                    return false;
                                }

                            }
                            else
                            {
                                RunBend_AOITickCount(StationType.Left);
                                left_OK = EProductAtt.OK;
                                Step_LeftBend = 11;
                            }

                            Step_LeftBend++;
                            break;
                        case 11:

                            if (Bend1Stop)
                            {

                                _IsleftbendWorking = false;
                                return false;
                            }

                            if (Config.IsLeftBendDisabled) //此处为了避免生产过程中，屏蔽出现逻辑混乱现象
                            {
                                b_bend1flag = true;
                                if (!QueueBend.Contains(0))
                                {
                                    QueueBend.Enqueue(0);
                                }
                            }

                            if (!QueueBendOut.Contains(0))
                            {
                                QueueBendOut.Enqueue(0);
                            }

                            if (Config.IsBend1Free)//直通计数
                            {
                                RunBend_AOIWork(StationType.Left);
                            }


                            OnBend1MsgArrived("反折R轴返回原位");
                            if (!Config.IsBend1Free)
                            {
                                if (!RunBend_AxisRGoSafePos(StationType.Left))
                                {
                                    AlarmWork();
                                    OutputError("反折R X轴返回原位报警", true);
                                    _IsleftbendWorking = false;
                                    return false;
                                }
                            }
                            Step_LeftBend++;
                            break;
                        case 12:
                            if (Bend1Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend1Station", Step_LeftBend);
                                _IsleftbendWorking = false;
                                return false;
                            }
                            OnBend1MsgArrived("运动到下料位置");
                            CloseLeftBend_UPlightController();
                            if (!GoBend_Discharge(StationType.Left))
                            {
                                AlarmWork();
                                OutputError("左折弯:下料报警!", true);
                                _IsleftbendWorking = false;
                                Step_LeftBend = 0;
                            }
                            else
                            {
                                _IsLeftBendOutReady = true;
                                Step_LeftBend++;
                            }
                            break;
                        case 13:
                            if (Bend1Stop)
                            {
                                _IsleftbendWorking = false;
                                return false;
                            }

                            OnBend1MsgArrived("等待下料");
                            while (!_IsLeftDischargeReady)
                            {
                                if (Bend1Stop)
                                {
                                    _IsleftbendWorking = false;
                                    return false;
                                }
                                if (IsStop)
                                {
                                    _IsleftbendWorking = false;
                                    Step_LeftBend = 0;
                                    return false;
                                }
                            }

                            OnBend1MsgArrived("下料完成");
                            _IsLeftDischargeReady = false;
                            Step_LeftBend = 0;
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + "折弯1" + ex.ToString());
                return false;
            }

        }

        void WriteErrLog(string err)
        {
            try
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + err + "\r\n");
            }
            catch (Exception ex)
            {
            }
        }



        static object _Obj_log = new object();

        public void WriteLog(string str_log)
        {
            try
            {
                if (str_log.Contains("折弯2"))
                {
                    File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\" + DateTime.Now.ToString("yyyyMMdd") + "MidData.csv", DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss fff") + "," + str_log + "\r\n");
                }
            }
            catch (Exception ex)
            {


            }




        }

        double Bend2_stgy_pos = 0;
        DateTime Bend2XYHomeTime = DateTime.Now;
        public bool RunBend2Work()
        {
            bool result = true;
            _IsmidbendWorking = true;
            try
            {
                if (Bend2Stop)
                {

                    Bend2Stop = false;
                }
                CloseMidBend_PressCylinder();
                CloseMidBendClawCylinder();
                Step_MidBend = 0;
                if (GetIOInStatus(Config.MidBend_stgVacuum_IOIn) && Config.IsMidBendDisabled && !Config.IsBend2Free)//反折
                {
                    Step_MidBend = 2;

                }

                if (GetIOInStatus(Config.MidBend_stgVacuum_IOIn) && Config.IsMidBendDisabled && Config.IsBend2Free)
                {
                    mid_OK = EProductAtt.OK;
                    Step_MidBend = 11;
                }



                // double pos_stgy = 0;

                while (_IsmidbendWorking)
                {
                    Thread.Sleep(30);
                    while (!_IsAutoRun)
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        Thread.Sleep(5);
                    }


                    switch (Step_MidBend)
                    {
                        case 0:
                            if (!Config.IsMidBendDisabled)
                            {
                                b_bend2EnableLoop = true;
                                if (!FlagTranfer)
                                {
                                    FlagBend2 = false;
                                    _IsmidbendWorking = false;
                                    OutputMessage("中折弯工位停止!");
                                    OnBend2MsgArrived("停止");
                                }


                                if (Bend2Stop)
                                {

                                    _IsmidbendWorking = false;
                                    return false;
                                }
                                WaitMilliSec(500);
                                Step_MidBend = 0;
                                break;
                            }
                            b_bend2EnableLoop = false;

                            if (Bend2Stop)
                            {

                                _IsmidbendWorking = false;
                                return false;
                            }


                            if (Config.IsBendXYHomeEnable && (DateTime.Now - Bend2XYHomeTime).TotalMinutes > Config.YHomeInterval)
                            {
                                AxisBase[] Axise = new AxisBase[] { _Axis_MidBend_DWY, _Axis_MidBend_DWX };

                                bool res = false, stop = false;
                                ThreadPool.QueueUserWorkItem(delegate
                                {
                                    res = _MotionC.GoHome(Axise);
                                    stop = true;
                                });

                                while (!stop)
                                {
                                    Thread.Sleep(50);
                                }

                                if (!(res))
                                {
                                    OutputError("中反折对位XY回原失败!");
                                    return false;
                                }
                                else
                                {

                                    int i = 0;
                                    while (i < Axise.Length)
                                    {
                                        MeasurementAxis axis = Axise[i] as MeasurementAxis;
                                        if (axis.IsHomeActived)
                                        {
                                            axis.PositionDev = 0;
                                            axis.PositionCode = 0;
                                            i++;
                                        }
                                        else
                                        {
                                            OutputError("中反折对位XY回原失败!");
                                            return false;
                                        }
                                    }
                                }
                                Bend2XYHomeTime = DateTime.Now;
                            }





                            OnBend2MsgArrived("运动到上料位");
                            if (!GoBendFeedPos(StationType.Mid))
                            {
                                AlarmWork();
                                OutputError("中折弯:运动到上料位报警!", true);
                                return false;
                            }
                            else
                            {
                                _IsMidBendReady = true;
                                Step_MidBend++;
                            }
                            break;
                        case 1:

                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }

                            OnBend2MsgArrived("等待上料");
                            while (!_IsMidBendUp)
                            {
                                if (Bend2Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                    _IsmidbendWorking = false;
                                    return false;
                                }

                                if (!FlagTranfer)
                                {
                                    FlagBend2 = false;
                                    _IsmidbendWorking = false;
                                    OutputMessage("中折弯工位停止");
                                    OnBend2MsgArrived("停止");
                                    return false;
                                }
                                if (IsStop)
                                {
                                    _IsmidbendWorking = false;
                                    return false;
                                }
                                Thread.Sleep(20);
                            }
                            _IsMidBendUp = false;

                            if (Config.IsLoadCell2Enable)//称重前清零
                            {
                                if (Config.Mid_LoadCellPdtCout > Config.LoadCellTestInterval)//当前到达到称重间隔
                                {
                                    int r = 0;
                                    Thread.Sleep(Config.WeighResetDelay);
                                    LoadCellResetVal(ref r, LoadCell2Net);
                                    Config.Mid_LoadCellPdtCout = 0;
                                }
                                Config.Mid_LoadCellPdtCout++;
                            }



                            if (QueueTearResult.Count != 0)
                            {
                                if (QueueTearResult.Dequeue() == 0)
                                {
                                    mid_OK = EProductAtt.NG_NOTBEND;
                                    tear2_OK = false;
                                    // QueueBendOut.Enqueue(1);
                                    Step_MidBend = 11;
                                }
                                else
                                {
                                    if (Config.IsBend2Free)
                                    {
                                        mid_OK = EProductAtt.OK;

                                        Step_MidBend = 11;
                                    }
                                    else
                                    {
                                        Step_MidBend++;
                                    }
                                }
                            }
                            else
                            {
                                if (Config.IsBend2Free)
                                {
                                    mid_OK = EProductAtt.OK;

                                    Step_MidBend = 11;
                                }
                                else
                                {
                                    Step_MidBend++;
                                }
                            }
                            break;
                        case 2:

                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }

                            OnBend2MsgArrived("运动到拍照位置");
                            OpenMidBend_UPlightController();
                            if (!GoBendCCDPos(StationType.Mid))
                            {
                                AlarmWork();
                                OutputError("中折弯运动到拍照位置失败");
                                return false;
                            }
                            Step_MidBend++;
                            break;
                        case 3:
                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }

                            Thread.Sleep(50);
                            OnBend2MsgArrived("角度校正");
                            if (!RunBendAdjustAngle(StationType.Mid))
                            {
                                AlarmWork();
                                OutputError("中折弯:角度校正失败!");
                                ErrADJ_Angle(StationType.Mid);
                                Thread.Sleep(100);
                                ClearAlarm();
                            }
                            else
                            {
                                Thread.Sleep(50);
                                if (!RunBendAdjustModel(StationType.Mid))
                                {
                                    AlarmWork();
                                    OutputError("中折弯:XY模板校正失败");
                                    ErrADJ_Angle(StationType.Mid);
                                    Thread.Sleep(100);
                                    ClearAlarm();
                                }
                                else
                                {
                                    Step_MidBend++;
                                }
                            }
                            break;
                        case 4:
                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }

                            Bend2_stgy_pos = _Axis_MidBend_stgY.PositionDev;
                            OnBend2MsgArrived("准备R轴反折");

                            if (Config.IsRunNull)
                            {
                                OpenMidBendClawCylinder();
                                Step_MidBend++;
                                break;
                            }

                            int ret = CheckFPCOptical(StationType.Mid);
                            if (ret == 0)
                            {
                                Step_MidBend = 0;
                            }
                            else if (ret == -1)
                            {
                                AlarmWork();
                                _IsmidbendWorking = false;
                                return false;
                            }
                            else
                            {
                                OpenMidBendClawCylinder();
                                double[] posxy = SendAdjustXYMsg((int)StationType.Mid);//拍背光
                                if (Math.Abs(posxy[0]) > 10 || Math.Abs(posxy[1]) > 10)
                                {
                                    CloseMidBendClawCylinder();
                                    AlarmWork();
                                    OutputError("左折弯:第二次XY模板抓边失败!");
                                    ErrADJ_Angle(StationType.Mid);
                                    Thread.Sleep(100);
                                    ClearAlarm();
                                }
                                else
                                {
                                    Step_MidBend++;
                                }
                            }
                            break;
                        case 5:

                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }

                            OnBend2MsgArrived("R轴反折");
                            if (!RunBend_AxisRWrok(StationType.Mid))
                            {
                                AlarmWork();
                                OutputError("R轴反折报警", true);
                                _IsmidbendWorking = false;
                                return false;
                            }
                            else
                            {
                                Step_MidBend++;
                            }
                            break;
                        case 6:
                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }

                            OnBend2MsgArrived("运动到相机拍照位置");
                            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_stgY/*, _Axis_MidBend_CCDX*/ },
                                new double[] { (Bend2_stgy_pos - Recipe.MidBend_CCDPos_Y) + Recipe.MidBend_CCDPos_Y2/*, Recipe.MidBend_CCDPos_X*/ }))
                            {
                                //_Axis_MidBend_CCDX.StopSlowly();
                                _Axis_MidBend_stgY.StopSlowly();
                                OutputError("运动到拍照位置失败!");
                                AlarmWork();
                                _IsleftbendWorking = false;
                                return false;
                            }
                            else
                            {
                                Step_MidBend++;
                            }
                            Thread.Sleep(50);
                            break;
                        case 7:
                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }

                            OnBend2MsgArrived("对位校正");
                            if (!RunBend_AdjustPosition(StationType.Mid))
                            {
                                AlarmWork();
                                ErrADJ_Position(StationType.Mid);
                                mid_OK = EProductAtt.NG_NOTBEND;
                                Thread.Sleep(100);
                                Step_MidBend = 11;
                                ClearAlarm();
                            }
                            else
                            {
                                Step_MidBend++;
                            }
                            break;
                        case 8:
                            if (Bend2Stop)
                            {
                                _IsmidbendWorking = false;
                                return false;
                            }

                            OnBend2MsgArrived("运动到压合位置开始压合");
                            if (!RunBend_PressWork(StationType.Mid))
                            {
                                OutputError("中折弯:压合报警!", true);
                                AlarmWork();
                                return false;
                            }
                            else
                            {
                                Step_MidBend++;
                            }
                            break;
                        case 9:

                            if (Bend2Stop)
                            {
                                _IsmidbendWorking = false;
                                return false;
                            }
                            OnBend2MsgArrived("反折R轴返回原位");
                            b_bend2flag = false;
                            if (Config.IsMidBendDisabled)
                            {
                                if (!QueueBend.Contains(1))
                                {
                                    QueueBend.Enqueue(1);
                                }
                                b_bend2flag = true;
                            }
                            QueueBendOut.Enqueue(1);
                            if (!RunBend_AxisRGoSafePos(StationType.Mid))
                            {
                                AlarmWork();
                                OutputError("中折弯:反折R轴返回原位报警", true);
                                _IsmidbendWorking = false;
                                return false;
                            }
                            else
                            {
                                Step_MidBend++;
                            }
                            break;

                        case 10:

                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }


                            if (Config.IsMidBendAOIDisabled)
                            {
                                OnBend2MsgArrived("反折AOI");
                                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_MidBend_stgY, _Axis_MidBend_CCDX },
                                            new double[] { Recipe.MidBend_CCDPos_Y + bend2delty, Recipe.MidBend_CCDPos_X + bend2deltx }))
                                {
                                    OutputError("中折弯运动到相机拍照位置失败!");
                                    AlarmWork();
                                    _Axis_MidBend_CCDX.StopSlowly();
                                    _Axis_MidBend_stgY.StopSlowly();
                                    return false;
                                }
                                if (Config.IsRunNull)
                                {
                                    mid_OK = EProductAtt.OK;
                                    Step_MidBend = 12;
                                    break;
                                }
                                Thread.Sleep(50);
                                if (!RunBend_AOIWork(StationType.Mid))
                                {
                                    OutputError("中折弯:AOI检测NG");
                                    AlarmWork();
                                    Thread.Sleep(100);
                                    ClearAlarm();
                                    mid_OK = EProductAtt.NG;
                                    Step_MidBend = 11;
                                }
                                else
                                {
                                    mid_OK = EProductAtt.OK;
                                    OutputMessage("中折弯：AOI检测OK");
                                    Step_MidBend++;
                                }


                            }
                            else
                            {
                                RunBend_AOITickCount(StationType.Mid);
                                mid_OK = EProductAtt.OK;
                                Step_MidBend = 11;
                            }
                            Step_MidBend++;

                            break;
                        case 11:
                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }

                            if (Config.IsMidBendDisabled)
                            {
                                b_bend2flag = true;
                                if (!QueueBend.Contains(1))
                                {
                                    QueueBend.Enqueue(1);
                                }

                            }

                            if (!QueueBendOut.Contains(1))
                            {
                                QueueBendOut.Enqueue(1);
                            }
                            if (Config.IsBend2Free)//直通计数
                            {
                                RunBend_AOIWork(StationType.Mid);
                            }

                            OnBend2MsgArrived("反折R轴返回原位");
                            if (!Config.IsBend2Free)
                            {
                                if (!RunBend_AxisRGoSafePos(StationType.Mid))
                                {
                                    AlarmWork();
                                    OutputError("中折弯:反折R轴返回原位报警", true);
                                    _IsmidbendWorking = false;
                                    return false;
                                }
                                Step_MidBend++;
                            }
                            else
                            {
                                Step_MidBend++;
                            }

                            break;
                        case 12:
                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }
                            OnBend2MsgArrived("运动到下料位置");
                            CloseMidBend_UPlightController();
                            if (!GoBend_Discharge(StationType.Mid))
                            {
                                AlarmWork();
                                OutputError("中折弯:运动到下料位置报警", true);
                                _IsmidbendWorking = false;
                                Step_MidBend = 0;
                                return false;
                            }
                            else
                            {
                                _IsMidBendOutReady = true;
                                Step_MidBend++;
                            }
                            break;
                        case 13:
                            if (Bend2Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                _IsmidbendWorking = false;
                                return false;
                            }




                            OnBend2MsgArrived("等待下料");
                            while (!_IsMidDischargeReady)
                            {
                                if (Bend2Stop)
                                {
                                    IniHelper.WriteInteger("WorkStep", "Bend2Station", Step_MidBend);
                                    _IsmidbendWorking = false;
                                    return false;
                                }

                                if (IsStop)
                                {
                                    _IsmidbendWorking = false;
                                    Step_MidBend = 0;
                                    return false;
                                }
                            }


                            OnBend2MsgArrived("下料完成");
                            _IsMidDischargeReady = false;
                            Step_MidBend = 0;
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + "折弯2" + ex.ToString());
                return false;
            }
        }

        double Bend3_stgy_pos = 0;
        DateTime Bend3XYHomeTime = DateTime.Now;
        public bool RunBend3Work()
        {
            bool result = true;
            _IsrightbendWorking = true;
            try
            {
                if (Bend3Stop)
                {
                    //Step_RightBend = IniHelper.ReadInteger("WorkStep", "Bend3Station", 0);
                    Bend3Stop = false;
                }
                CloseRightBend_PressCylinder();
                CloseRightBendClawCylinder();
                Step_RightBend = 0;
                if (GetIOInStatus(Config.RightBend_stgVacuum_IOIn) && Config.IsRightBendDisabled && !Config.IsBend3Free)
                {
                    Step_RightBend = 2;
                }

                if (GetIOInStatus(Config.RightBend_stgVacuum_IOIn) && Config.IsRightBendDisabled && Config.IsBend3Free)
                {
                    right_OK = EProductAtt.OK;
                    Step_RightBend = 11;
                }


                while (_IsrightbendWorking)
                {
                    Thread.Sleep(30);
                    while (!_IsAutoRun)
                    {
                        if (IsStop)
                        {
                            return false;
                        }
                        Thread.Sleep(5);
                    }


                    //if (Config.IsRightBendDisabled)
                    //{

                    switch (Step_RightBend)
                    {
                        case 0:

                            if (!Config.IsRightBendDisabled)
                            {
                                b_bend3EnableLoop = true;
                                if (Bend3Stop)
                                {
                                    _IsrightbendWorking = false;
                                    return false;
                                }
                                if (!FlagTranfer)
                                {
                                    FlagBend3 = false;
                                    _IsrightbendWorking = false;
                                    OutputMessage("右折弯工位停止!");
                                    OnBend3MsgArrived("停止");
                                }
                                WaitMilliSec(500);
                                Step_RightBend = 0;
                                break;
                            }
                            b_bend3EnableLoop = false;

                            if (Bend3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend3Station", Step_RightBend);
                                _IsrightbendWorking = false;
                                return false;
                            }


                            if (Config.IsBendXYHomeEnable && (DateTime.Now - Bend3XYHomeTime).TotalMinutes > Config.YHomeInterval)
                            {
                                AxisBase[] Axise = new AxisBase[] { _Axis_RightBend_DWY, _Axis_RightBend_DWX };

                                bool res = false, stop = false;
                                ThreadPool.QueueUserWorkItem(delegate
                                {
                                    res = _MotionD.GoHome(Axise);
                                    stop = true;
                                });

                                while (!stop)
                                {
                                    Thread.Sleep(50);
                                }

                                if (!(res))
                                {
                                    OutputError("右反折对位XY回原失败!");
                                    return false;
                                }
                                else
                                {

                                    int i = 0;
                                    while (i < Axise.Length)
                                    {
                                        MeasurementAxis axis = Axise[i] as MeasurementAxis;
                                        if (axis.IsHomeActived)
                                        {
                                            axis.PositionDev = 0;
                                            axis.PositionCode = 0;
                                            i++;
                                        }
                                        else
                                        {
                                            OutputError("右反折对位XY回原失败!");
                                            return false;
                                        }
                                    }
                                }
                                Bend3XYHomeTime = DateTime.Now;
                            }



                            OnBend3MsgArrived("运动到上料位");
                            if (!GoBendFeedPos(StationType.Right))
                            {
                                OutputError("右折弯:运动到上料位报警!", true);
                                AlarmWork();
                                return false;
                            }
                            else
                            {
                                _IsRightBendReady = true;
                                Step_RightBend++;
                            }
                            break;
                        case 1:

                            if (Bend3Stop)
                            {
                                _IsrightbendWorking = false;
                                return false;
                            }

                            OnBend3MsgArrived("等待上料");
                            while (!_IsRightBendUp)
                            {
                                if (Bend3Stop)
                                {
                                    _IsrightbendWorking = false;
                                    return false;
                                }

                                if (!FlagTranfer)
                                {
                                    FlagBend3 = false;
                                    _IsrightbendWorking = false;
                                    OutputMessage("右折弯工位停止");
                                    OnBend3MsgArrived("停止");
                                    return false;
                                }
                                if (IsStop)
                                {
                                    _IsrightbendWorking = false;
                                    return false;
                                }
                                Thread.Sleep(20);
                            }
                            _IsRightBendUp = false;
                            if (Config.IsLoadCell3Enable)//称重前必须清零
                            {
                                if (Config.Right_LoadCellPdtCout > Config.LoadCellTestInterval)//当前到达到称重间隔
                                {
                                    int r = 0;
                                    Thread.Sleep(Config.WeighResetDelay);
                                    LoadCellResetVal(ref r, LoadCell3Net);
                                    Config.Right_LoadCellPdtCout = 0;
                                }
                                Config.Right_LoadCellPdtCout++;
                            }

                            if (QueueTearResult.Count != 0)
                            {
                                if (QueueTearResult.Dequeue() == 0)
                                {
                                    right_OK = EProductAtt.NG_NOTBEND;
                                    tear3_OK = false;
                                    Step_RightBend = 11;
                                }
                                else
                                {
                                    if (Config.IsBend3Free)
                                    {
                                        right_OK = EProductAtt.OK;
                                        Step_RightBend = 11;
                                    }
                                    else
                                    {
                                        Step_RightBend++;
                                    }
                                }
                            }
                            else
                            {
                                if (Config.IsBend3Free)
                                {
                                    right_OK = EProductAtt.OK;
                                    Step_RightBend = 11;
                                }
                                else
                                {
                                    Step_RightBend++;
                                }
                            }
                            break;
                        case 2:
                            if (Bend3Stop)
                            {
                                _IsrightbendWorking = false;
                                return false;
                            }

                            OnBend3MsgArrived("运动到拍照位置");
                            OpenRightBend_UPlightController();
                            if (!GoBendCCDPos(StationType.Right))
                            {
                                AlarmWork();
                                OutputError("右折弯运动到拍照位置NG");
                                return false;
                            }
                            Step_RightBend++;
                            break;
                        case 3:
                            if (Bend3Stop)
                            {

                                _IsrightbendWorking = false;
                                return false;
                            }

                            OnBend3MsgArrived("角度校正");
                            if (!RunBendAdjustAngle(StationType.Right))
                            {
                                AlarmWork();
                                Thread.Sleep(100);
                                OutputError("右折弯:角度校正失败!");
                                ErrADJ_Angle(StationType.Right);
                                ClearAlarm();
                            }
                            else
                            {
                                Thread.Sleep(50);
                                if (!RunBendAdjustModel(StationType.Right))
                                {
                                    AlarmWork();
                                    Thread.Sleep(100);
                                    OutputError("右折弯:XY模板校正失败");
                                    ErrADJ_Angle(StationType.Right);
                                    ClearAlarm();
                                }
                                else
                                {
                                    Step_RightBend++;
                                }
                            }
                            break;
                        case 4:
                            if (Bend3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend3Station", Step_RightBend);
                                _IsrightbendWorking = false;
                                return false;
                            }

                            Bend3_stgy_pos = _Axis_RightBend_stgY.PositionDev;
                            OnBend3MsgArrived("准备R轴反折");

                            if (Config.IsRunNull)
                            {
                                OpenRightBendClawCylinder();
                                Step_RightBend++;
                                break;
                            }

                            int ret = CheckFPCOptical(StationType.Right);
                            if (ret == 0)
                            {

                                Step_RightBend = 0;
                            }
                            else if (ret == -1)
                            {
                                AlarmWork();
                                _IsrightbendWorking = false;
                                return false;
                            }
                            else
                            {
                                OpenRightBendClawCylinder();
                                double[] posxy = SendAdjustXYMsg((int)StationType.Right);
                                if (Math.Abs(posxy[0]) > 10 || Math.Abs(posxy[1]) > 10)
                                {
                                    CloseRightBendClawCylinder();
                                    AlarmWork();
                                    OutputError("右折弯:第二次XY模板抓边失败!");
                                    ErrADJ_Angle(StationType.Right);
                                    Thread.Sleep(100);
                                    ClearAlarm();
                                }
                                else
                                {
                                    Step_RightBend++;
                                }
                            }
                            break;
                        case 5:
                            if (Bend3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend3Station", Step_RightBend);
                                _IsrightbendWorking = false;
                                return false;
                            }

                            OnBend3MsgArrived("R轴反折");
                            if (!RunBend_AxisRWrok(StationType.Right))
                            {
                                AlarmWork();
                                OutputError("R轴反折报警", true);
                                _IsrightbendWorking = false;
                                return false;
                            }
                            else
                            {
                                Step_RightBend++;
                            }
                            break;
                        case 6:

                            if (Bend3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend3Station", Step_RightBend);
                                _IsrightbendWorking = false;
                                return false;
                            }

                            OnBend3MsgArrived("运动到相机拍照位置");
                            if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_stgY/*, _Axis_RightBend_CCDX*/ },
                                new double[] { (Bend3_stgy_pos - Recipe.RightBend_CCDPos_Y) + Recipe.RightBend_CCDPos_Y2/*, Recipe.RightBend_CCDPos_X*/ }))
                            {
                                // _Axis_RightBend_CCDX.StopSlowly();
                                _Axis_RightBend_stgY.StopSlowly();
                                AlarmWork();
                                OutputError("右折弯:运动到相机拍照位置报警!", true);
                                _IsrightbendWorking = false;
                                return false;
                            }
                            else
                            {
                                Step_RightBend++;
                            }
                            Thread.Sleep(50);
                            break;
                        case 7:
                            if (Bend3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend3Station", Step_RightBend);
                                _IsrightbendWorking = false;
                                return false;
                            }


                            OnBend3MsgArrived("对位校正");
                            if (!RunBend_AdjustPosition(StationType.Right))
                            {
                                AlarmWork();
                                ErrADJ_Position(StationType.Right);
                                Thread.Sleep(100);
                                right_OK = EProductAtt.NG_NOTBEND;
                                ClearAlarm();
                                Step_RightBend = 11;
                            }
                            else
                            {
                                Step_RightBend++;
                            }
                            break;
                        case 8:

                            if (Bend3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend3Station", Step_RightBend);
                                _IsrightbendWorking = false;
                                return false;
                            }

                            OnBend3MsgArrived("运动到压合位置开始压合");
                            if (!RunBend_PressWork(StationType.Right))
                            {
                                OutputError("右折弯:压合报警!", true);
                                AlarmWork();
                                return false;
                            }
                            else
                            {
                                Step_RightBend++;
                            }
                            break;
                        case 9:
                            if (Bend3Stop)
                            {
                                IniHelper.WriteInteger("WorkStep", "Bend3Station", Step_RightBend);
                                _IsrightbendWorking = false;
                                return false;
                            }
                            OnBend3MsgArrived("反折R轴返回原位");

                            b_bend3flag = false;
                            if (Config.IsRightBendDisabled)
                            {
                                b_bend3flag = true;
                                if (!QueueBend.Contains(2))
                                {
                                    QueueBend.Enqueue(2);
                                }
                            }
                            QueueBendOut.Enqueue(2);
                            if (!RunBend_AxisRGoSafePos(StationType.Right))
                            {
                                AlarmWork();
                                OutputError("右折弯:反折R轴返回原位报警", true);
                                _IsrightbendWorking = false;
                                return false;
                            }
                            else
                            {
                                Step_RightBend++;
                            }
                            break;

                        case 10:
                            if (Bend3Stop)
                            {
                                _IsrightbendWorking = false;
                                return false;
                            }

                            if (Config.IsRightBendAOIDisabled)
                            {
                                OnBend3MsgArrived("反折AOI");
                                if (!AxisMoveTo(new MeasurementAxis[] { _Axis_RightBend_stgY, _Axis_RightBend_CCDX },
                                    new double[] { Recipe.RightBend_CCDPos_Y + bend3delty, Recipe.RightBend_CCDPos_X + bend3deltx }))
                                {
                                    OutputError("右折弯运动到相机拍照位置失败!", true);
                                    AlarmWork();
                                    _Axis_RightBend_CCDX.StopSlowly();
                                    _Axis_RightBend_stgY.StopSlowly();
                                    return false;
                                }

                                Thread.Sleep(50);
                                if (Config.IsRunNull)
                                {
                                    right_OK = EProductAtt.OK;
                                    Step_RightBend = 12;
                                    break;
                                }

                                if (!RunBend_AOIWork(StationType.Right))
                                {
                                    AlarmWork();
                                    OutputError("右折弯:AOI检测NG");
                                    Thread.Sleep(100);
                                    ClearAlarm();
                                    right_OK = EProductAtt.NG;
                                    Step_RightBend = 11;
                                }
                                else
                                {
                                    right_OK = EProductAtt.OK;
                                    OutputMessage("右折弯：AOI检测OK");
                                    Step_RightBend++;
                                }
                            }
                            else
                            {
                                RunBend_AOITickCount(StationType.Right);
                                right_OK = EProductAtt.OK;
                                Step_RightBend = 11;
                            }
                            Step_RightBend++;
                            break;

                        case 11:
                            if (Bend3Stop)
                            {
                                _IsrightbendWorking = false;
                                return false;
                            }
                            if (Config.IsRightBendDisabled)
                            {
                                b_bend3flag = true;
                                if (!QueueBend.Contains(2))
                                {
                                    QueueBend.Enqueue(2);
                                }
                            }

                            if (!QueueBendOut.Contains(2))
                            {
                                QueueBendOut.Enqueue(2);
                            }

                            if (Config.IsBend3Free)//直通计数
                            {
                                RunBend_AOIWork(StationType.Right);
                            }


                            OnBend3MsgArrived("反折R轴返回原位");

                            if (!Config.IsBend3Free)
                            {
                                if (!RunBend_AxisRGoSafePos(StationType.Right))
                                {
                                    AlarmWork();
                                    OutputError("右折弯:反折R轴返回原位报警", true);
                                    _IsrightbendWorking = false;
                                    return false;
                                }
                                Step_RightBend++;
                            }
                            else
                            {
                                Step_RightBend++;
                            }

                            break;
                        case 12:
                            if (Bend3Stop)
                            {
                                _IsrightbendWorking = false;
                                return false;
                            }
                            OnBend3MsgArrived("运动到下料位置");
                            CloseRightBend_UPlightController();

                            if (!GoBend_Discharge(StationType.Right))
                            {
                                OutputError("右折弯:运动到下料位置报警!", true);
                                AlarmWork();
                                _IsrightbendWorking = false;
                                Step_RightBend = 0;
                                return false;
                            }
                            else
                            {
                                _IsRightBendOutReady = true;
                                Step_RightBend++;
                            }
                            break;
                        case 13:
                            if (Bend3Stop)
                            {
                                _IsrightbendWorking = false;
                                return false;
                            }

                            OnBend3MsgArrived("等待下料");
                            while (!_IsRightDischargeReady)
                            {
                                if (Bend3Stop)
                                {
                                    _IsrightbendWorking = false;
                                    return false;
                                }

                                if (IsStop)
                                {
                                    _IsrightbendWorking = false;
                                    Step_RightBend = 0;
                                    return false;
                                }
                            }

                            OnBend3MsgArrived("下料完成");
                            _IsRightDischargeReady = false;
                            Step_RightBend = 0;
                            break;
                        default:
                            break;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + "折弯3" + ex.ToString());
                return false;
            }
        }

        private bool _IsDischargeWorking = false;
        private int Step_Discharge = 0;
        double Singel_Time = 0.00;
        double Cycle_Time = 0.00;
        long total_time = 0;

        int Total = 0;
        EProductAtt b_checkchanel = EProductAtt.OK;
        int nDischargeIndex = 3;
        int nBendOutIndex = 3;

        DateTime DischargeZHomeTime = DateTime.Now;
        public bool RunDischargeWork()
        {
            bool result = true;
            try
            {
                if (DischargeStop)
                {
                    DischargeStop = false;
                }
                Step_Discharge = 0;
                nDischargeIndex = 3;


                _IsDischargeWorking = true;
                Stopwatch stc = new Stopwatch();
                stc.Start();

                if (Config.DischargeAxiaZCylinderEnable)
                {
                    if (DischargeAxisZCylinderUp() == -2) return false;
                }

                if (!AxisMoveTo(Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                {
                    Axis_Discharge_Z.StopSlowly();
                    return false;
                }

                if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeXSafePos))
                {
                    _Axis_Discharge_X.StopSlowly();
                    return false;
                }


                while (_IsDischargeWorking)
                {
                    Thread.Sleep(30);
                    while (!_IsAutoRun)
                    {
                        if (IsStop)
                        {
                            break;
                        }
                        Thread.Sleep(5);
                    }

                    if (IsStop)
                    {
                        return false;
                    }
                    switch (Step_Discharge)
                    {
                        case 0:
                            if (GetIOInStatus(Config.DischargeVacuumIOIn))
                            {
                                Step_Discharge = 2;
                                break;
                            }

                            if (DischargeStop)
                            {
                                _IsDischargeWorking = false;
                                return false;
                            }

                            OnDischargeMsgArrived("等待折弯下料指令");
                            if (!FlagBend1 && !FlagBend2 && !FlagBend3)
                            {
                                FlagDischarge = false;
                                _IsDischargeWorking = false;
                                _IsDischaregeLineRun = false;
                                _IsNGDischaregeLineRun = false;
                                FlagFeed = true;
                                FlagTear1 = true;
                                FlagTear2 = true;
                                FlagTear3 = true;
                                FlagTranfer = true;
                                _IsFeedLineRun = false;
                                OutputMessage("下料工位停止");
                                OnDischargeMsgArrived("停止");
                                _WorkStatus = WorkStatuses.Idle;
                                OnWorkStatusChanged();
                                break;
                            }

                            if (nDischargeIndex == 3)
                            {
                                if (QueueBendOut.Count == 0)
                                {
                                    if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeXSafePos))
                                    {
                                        AlarmWork();
                                        OutputError("下料X轴回待机位报警!");
                                        _Axis_Discharge_Z.StopSlowly();
                                        return false;
                                    }
                                }



                                if (Config.DischargeZHomeEnable && (DateTime.Now - DischargeZHomeTime).TotalMinutes > Config.ZHomeInterval)//自动回原 20220213
                                {
                                    AxisBase[] Axise = new AxisBase[] { Axis_Discharge_Z };

                                    bool res = false, stop = false;
                                    ThreadPool.QueueUserWorkItem(delegate
                                    {
                                        res = _MotionB.GoHome(Axise);
                                        stop = true;
                                    });

                                    while (!stop)
                                    {
                                        Thread.Sleep(50);
                                    }

                                    if (!(res))
                                    {
                                        OutputError("下料Z回原失败!");
                                        return false;
                                    }
                                    else
                                    {

                                        int i = 0;
                                        while (i < Axise.Length)
                                        {
                                            MeasurementAxis axis = Axise[i] as MeasurementAxis;
                                            if (axis.IsHomeActived)
                                            {
                                                axis.PositionDev = 0;
                                                axis.PositionCode = 0;
                                                i++;
                                            }
                                            else
                                            {
                                                OutputError("下料Z回原失败!");
                                                return false;
                                            }
                                        }
                                    }
                                    DischargeZHomeTime = DateTime.Now;
                                }


                                while (QueueBendOut.Count == 0)
                                {
                                    if (DischargeStop)
                                    {
                                        _IsDischargeWorking = false;
                                        return false;
                                    }

                                    Thread.Sleep(20);
                                    if (!FlagBend1 && !FlagBend2 && !FlagBend3)
                                    {
                                        FlagDischarge = false;
                                        _IsDischaregeLineRun = false;
                                        _IsNGDischaregeLineRun = false;
                                        _IsFeedLineRun = false;
                                        FlagFeed = true;
                                        FlagTear1 = true;
                                        FlagTear2 = true;
                                        FlagTear3 = true;
                                        FlagTranfer = true;
                                        OutputMessage("下料工位停止！");
                                        OnDischargeMsgArrived("停止");
                                        break;
                                    }
                                    if (IsStop)
                                    {
                                        _IsDischargeWorking = false;
                                        break;
                                    }
                                    Thread.Sleep(20);
                                }
                            }

                            if (!FlagDischarge)
                            {
                                _IsDischargeWorking = false;
                                _WorkStatus = WorkStatuses.Idle;
                                OnWorkStatusChanged();
                                _IsDischaregeLineRun = false;
                                _IsNGDischaregeLineRun = false;
                                _IsFeedLineRun = false;
                                break;
                            }

                            if (!IsStop)
                            {
                                Step_Discharge++;
                            }
                            break;

                        case 1:
                            if (DischargeStop)
                            {
                                IniHelper.WriteInteger("WorkStep", "DischargeStation", Step_Discharge);
                                IniHelper.WriteInteger("WorkStep", "DischargeIndex", nDischargeIndex);
                                _IsDischargeWorking = false;
                                return false;
                            }

                            if (!IsOnPosition(Axis_Discharge_Z, Recipe.DischargeZSafePos))
                            {
                                if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                {
                                    AlarmWork();
                                    OutputError("下料Z轴回安全位报警!");
                                    _Axis_Discharge_Z.StopSlowly();
                                    return false;
                                }
                            }

                            if (nDischargeIndex == 3)
                            {
                                nBendOutIndex = QueueBendOut.Dequeue();
                            }
                            else
                            {
                                nBendOutIndex = nDischargeIndex;
                            }

                            switch (nBendOutIndex)
                            {
                                case 0:
                                    OnDischargeMsgArrived("运动到左折弯准备取料");


                                    if (!AxisMoveTo(_Axis_Discharge_X, Recipe.LeftBend_Discharge_x))
                                    {
                                        _Axis_Discharge_X.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    Thread.Sleep(20);


                                    while (!_IsLeftBendOutReady)
                                    {
                                        if (DischargeStop)
                                        {
                                            nDischargeIndex = 0;
                                            _IsDischargeWorking = false;
                                            return false;
                                        }
                                        if (_IsStop)
                                        {
                                            _IsDischargeWorking = false;
                                            return false;
                                        }
                                    }



                                    _IsLeftBendOutReady = false;
                                    b_checkchanel = left_OK;
                                    OnDischargeMsgArrived("左折弯开始取料");
                                    if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.LeftBend_Discharge_Z))
                                    {
                                        _Axis_Discharge_Z.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    OpenDischargeSuck();
                                    Thread.Sleep(Config.RobotFetchDelay);
                                    if (!(tear1_OK && bend1_OK))
                                    {
                                        tear1_OK = true;
                                        bend1_OK = true;
                                    }
                                    if (left_OK == EProductAtt.NG_NOTBEND || Config.IsBend1Free)
                                    {
                                        OpenFPCDischargeSuck();
                                    }


                                    CloseLeftBendSuck();
                                    OpenLeftBendBlow();
                                    Thread.Sleep(Config.StageBlowDelay);
                                    bool flag1 = false;
                                    while (!WaitIOMSec(Config.DischargeVacuumIOIn, 400, true))
                                    {
                                        OutputError("下料真空吸报警!", true);
                                        DialogResult DRet = ShowMsgChoiceBox("下料真空吸报警!\r\n\r\n"
                                         + "人工取料: 人工取走物料\r\n"
                                         + "确认:继续下一步动作!", true, false);
                                        if (DRet == DialogResult.Cancel)
                                        {
                                            CloseLeftBendBlow();
                                            CloseDischargeBlow();
                                            CloseDischargeSuck();
                                            Thread.Sleep(50);
                                            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                            {
                                                _Axis_Discharge_Z.StopSlowly();
                                                return false;
                                            }
                                            frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                            frm.IOOut1 = Config.LeftBend_BlowVacuum_IOOut;
                                            frm.IOOut2 = Config.LeftBend_SuckVacuum_IOOut;
                                            frm.IOOut3 = Config.Discharge_Suckvacuum_IOOut;
                                            frm.IOOut4 = Config.Discharge_Blowvacuum_IOOut;
                                            frm.ShowDialog();
                                            if (frm.DialogResult == DialogResult.OK)
                                            {

                                                flag1 = true;
                                                Step_Discharge = -1;
                                                _IsLeftDischargeReady = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (flag1)
                                    {
                                        flag1 = false;
                                        break;
                                    }
                                    CloseLeftBendBlow();
                                    Thread.Sleep(100);

                                    if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                    {
                                        _Axis_Discharge_Z.StopSlowly();
                                        Step_Discharge = 0;
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    OnDischargeMsgArrived("左折弯取料完成");

                                    _IsLeftDischargeReady = true;
                                    break;
                                case 1:
                                    OnDischargeMsgArrived("运动到中折弯准备取料");
                                    if (!AxisMoveTo(_Axis_Discharge_X, Recipe.MidBend_Discharge_X))
                                    {
                                        _Axis_Discharge_X.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    Thread.Sleep(20);
                                    while (!_IsMidBendOutReady)
                                    {
                                        if (DischargeStop)
                                        {
                                            nDischargeIndex = 1;
                                            _IsDischargeWorking = false;
                                            return false;
                                        }
                                        if (_IsStop)
                                        {
                                            _IsDischargeWorking = false;
                                            return false;
                                        }
                                    }

                                    b_checkchanel = mid_OK;
                                    _IsMidBendOutReady = false;
                                    OnDischargeMsgArrived("开始中折弯取料");
                                    if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.MidBend_Discharge_Z))
                                    {
                                        _Axis_Discharge_Z.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    OpenDischargeSuck();
                                    Thread.Sleep(Config.RobotFetchDelay);
                                    if (!(tear2_OK && bend2_OK))
                                    {
                                        tear2_OK = true;
                                        bend2_OK = true;
                                    }

                                    if (mid_OK == EProductAtt.NG_NOTBEND || Config.IsBend2Free)
                                    {
                                        OpenFPCDischargeSuck();
                                    }



                                    CloseMidBendSuck();
                                    OpenMidBendBlow();
                                    Thread.Sleep(Config.StageBlowDelay);
                                    bool flag2 = false;
                                    while (!WaitIOMSec(Config.DischargeVacuumIOIn, 400, true))
                                    {
                                        OutputError("出料真空吸报警!", true);
                                        DialogResult DRet = ShowMsgChoiceBox("出料真空吸报警!\r\n\r\n"
                                         + "人工取料: 人工取走物料\r\n"
                                         + "确认:继续下一步动作!", true, false);
                                        if (DRet == DialogResult.Cancel)
                                        {
                                            CloseMidBendBlow();
                                            CloseDischargeBlow();
                                            CloseDischargeSuck();
                                            Thread.Sleep(50);

                                            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                            {
                                                _Axis_Discharge_Z.StopSlowly();
                                                return false;
                                            }
                                            frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                            frm.IOOut1 = Config.MidBend_BlowVacuum_IOOut;
                                            frm.IOOut2 = Config.MidBend_SuckVacuum_IOOut;
                                            frm.IOOut3 = Config.Discharge_Suckvacuum_IOOut;
                                            frm.IOOut4 = Config.Discharge_Blowvacuum_IOOut;
                                            frm.ShowDialog();
                                            if (frm.DialogResult == DialogResult.OK)
                                            {

                                                flag2 = true;
                                                Step_Discharge = -1;
                                                _IsMidDischargeReady = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (flag2)
                                    {
                                        flag2 = false;
                                        break;
                                    }
                                    CloseMidBendBlow();
                                    if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                    {
                                        _Axis_Discharge_Z.StopSlowly();
                                        Step_Discharge = 0;
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    OnDischargeMsgArrived("中折弯取料完成");
                                    _IsMidDischargeReady = true;
                                    break;
                                case 2:

                                    OnDischargeMsgArrived("运动到右折弯取料位置");
                                    if (!AxisMoveTo(_Axis_Discharge_X, Recipe.RightBend_Discharge_X))
                                    {
                                        _Axis_Discharge_X.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    Thread.Sleep(20);
                                    while (!_IsRightBendOutReady)
                                    {
                                        if (DischargeStop)
                                        {
                                            nDischargeIndex = 2;
                                            _IsDischargeWorking = false;
                                            return false;
                                        }

                                        if (_IsStop)
                                        {
                                            _IsDischargeWorking = false;
                                            return false;
                                        }
                                    }



                                    _IsRightBendOutReady = false;
                                    b_checkchanel = right_OK;
                                    OnDischargeMsgArrived("开始右折弯取料");
                                    if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.RightBend_Discharge_Z))
                                    {
                                        _Axis_Discharge_Z.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    OpenDischargeSuck();
                                    Thread.Sleep(Config.RobotFetchDelay);
                                    if (!(tear3_OK && bend3_OK))
                                    {
                                        tear3_OK = true;
                                        bend3_OK = true;
                                    }

                                    if (right_OK == EProductAtt.NG_NOTBEND || Config.IsBend3Free)
                                    {
                                        OpenFPCDischargeSuck();
                                    }



                                    CloseRightBendSuck();
                                    OpenRightBendBlow();
                                    Thread.Sleep(Config.StageBlowDelay);
                                    bool flag3 = false;
                                    while (!WaitIOMSec(Config.DischargeVacuumIOIn, 400, true))
                                    {
                                        OutputError("出料真空吸报警!", true);
                                        DialogResult DRet = ShowMsgChoiceBox("出料真空吸报警!\r\n\r\n"
                                         + "人工取料: 人工取走物料\r\n"
                                         + "确认:继续下一步动作!", true, false);

                                        if (DRet == DialogResult.Cancel)
                                        {
                                            CloseRightBendBlow();
                                            CloseDischargeBlow();
                                            CloseDischargeSuck();
                                            Thread.Sleep(50);
                                            if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                            {
                                                _Axis_Discharge_Z.StopSlowly();
                                                return false;
                                            }
                                            frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                            frm.IOOut1 = Config.Discharge_Blowvacuum_IOOut;
                                            frm.IOOut2 = Config.RightBend_SuckVacuum_IOOut;
                                            frm.IOOut3 = Config.Discharge_Suckvacuum_IOOut;
                                            frm.IOOut4 = Config.RightBend_BlowVacuum_IOOut;
                                            frm.ShowDialog();
                                            if (frm.DialogResult == DialogResult.OK)
                                            {

                                                flag3 = true;
                                                Step_Discharge = -1;
                                                _IsRightDischargeReady = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (flag3)
                                    {
                                        flag3 = false;
                                        break;
                                    }
                                    CloseRightBendBlow();

                                    if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                    {
                                        _Axis_Discharge_Z.StopSlowly();
                                        Step_Discharge = 0;
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    OnDischargeMsgArrived("右折弯取料完成");
                                    if (!WaitIOMSec(Config.DischargeVacuumIOIn, 200, true))
                                    {
                                        FrAlarm fra = new FrAlarm();
                                        fra.lblmsg.Text = "出料吸真空报警!";
                                        fra.ShowDialog();
                                    }
                                    _IsRightDischargeReady = true;
                                    break;
                            }
                            nDischargeIndex = 3;
                            Step_Discharge++;
                            break;
                        case 2:
                            Step_Discharge++;
                            break;
                        case 3:

                            if (DischargeStop)
                            {
                                _IsDischargeWorking = false;
                                return false;
                            }


                            if ((Config.NGNotBendOutType && b_checkchanel == EProductAtt.OK) || !Config.NGNotBendOutType && b_checkchanel != EProductAtt.NG)//出料方式 NG未反折可选择正常流出
                            {
                                if (b_checkchanel == EProductAtt.OK) OnDischargeMsgArrived("OK品下料");
                                if (b_checkchanel == EProductAtt.NG_NOTBEND) OnDischargeMsgArrived("未压合NG品下料");

                                if (!IsOnPosition(_Axis_Discharge_Z, Recipe.DischargeZSafePos))
                                {
                                    if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                    {
                                        AlarmWork();
                                        OutputError("Z轴到安全位置运动失败!", true);
                                        _Axis_Discharge_Z.StopSlowly();
                                        return false;
                                    }
                                }

                                if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeX_OK_PullPos))
                                {
                                    _Axis_Discharge_X.StopSlowly();
                                    _IsDischargeWorking = false;
                                    return false;
                                }
                                if (IsDischargeFull) OutputMessage("出料流水线满料!");


                                //翻转下料
                                if (Config.IsDischargeCylinderEnable)
                                {

                                    while (!_IsDischargeRotateReady)
                                    {
                                        if (DischargeStop)
                                        {
                                            b_checkchanel = EProductAtt.OK;
                                            _IsDischargeWorking = false;
                                            return false;
                                        }

                                        if (_IsStop)
                                        {
                                            return false;
                                        }
                                        Thread.Sleep(20);
                                    }

                                }
                                else//Z气缸下料
                                {
                                    while (thdTakeOutBoard != null && thdTakeOutBoard.IsAlive)
                                    {
                                        if (DischargeStop)
                                        {
                                            b_checkchanel = EProductAtt.OK;
                                            _IsDischargeWorking = false;
                                            return false;
                                        }

                                        if (_IsStop)
                                        {
                                            return false;
                                        }
                                    }
                                }

                                if (Config.DischargeAxiaZCylinderEnable)
                                {
                                    CloseDischargeAxisZCylinder();
                                }

                                if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZ_OK_PullPos))
                                {
                                    _Axis_Discharge_Z.StopSlowly();
                                    _IsDischargeWorking = false;
                                    return false;
                                }

                                if (Config.DischargeAxiaZCylinderEnable)
                                {
                                    if (DischargeAxisZCylinderDown() == -2)
                                    {
                                        _IsDischargeWorking = false;
                                        return false;
                                    }

                                }

                                if (Config.IsDischargeCylinderEnable)
                                {
                                    OpenDischargeRotateFPCSuck();
                                    OpenDischargeRotateSuck();
                                    Thread.Sleep(Config.RobotBlowDelay);
                                }

                                CloseDischargeSuck();
                                OpenDischargeBlow();
                                CloseFPCDischargeSuck();
                                Thread.Sleep(Config.RobotBlowDelay);
                                if (Config.IsDischargeCylinderEnable && !WaitIOExMSec(Config.InputVacumn_IOInEx, 200, true))
                                {
                                    OutputError("下料旋转真空吸报警!", true);
                                    DialogResult DRet = ShowMsgChoiceBox("下料旋转真空吸报警!\r\n\r\n"
                                     + "人工取料: 人工取走物料\r\n"
                                     + "确认:继续下一步动作!", true, false);

                                    if (DRet == DialogResult.Cancel)
                                    {
                                        CloseDischargeRotateSuck();
                                        Thread.Sleep(50);
                                        if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                        {
                                            _Axis_Discharge_Z.StopSlowly();
                                            return false;
                                        }
                                        frmConfirm frm = new frmConfirm("人工取走料后，点确认，将进行上一步动作!", false, true);
                                        frm.IOOut1 = Config.DischargeRotateSuck_OutEx;
                                        frm.ShowDialog();
                                        if (frm.DialogResult == DialogResult.OK)
                                        {

                                            Step_Discharge = 0;
                                            _IsRightDischargeReady = true;
                                            break;
                                        }
                                    }
                                }

                                if (Config.DischargeAxiaZCylinderEnable)
                                {
                                    OpenDischargeAxisZCylinder();
                                }


                                if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                {
                                    _Axis_Discharge_Z.StopSlowly();
                                    _IsDischargeWorking = false;
                                }
                                CloseDischargeBlow();

                                if (Config.DischargeAxiaZCylinderEnable)
                                {
                                    if (DischargeAxisZCylinderUp() == -2)
                                    {
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                }

                                _IsDischargeRotateReady = false;
                                if (thdTakeOutBoard == null || !thdTakeOutBoard.IsAlive)//下料流水线
                                {
                                    thdTakeOutBoard = new Thread(TakeOutBoard);
                                    thdTakeOutBoard.IsBackground = true;
                                    thdTakeOutBoard.Start();

                                }

                            }
                            else
                            {
                                OnDischargeMsgArrived("NG下料");
                                while (!b_FeedCarry_Safe)
                                {
                                    Thread.Sleep(50);
                                    if (_IsStop)
                                    {
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                }

                                b_DischargeCarry_Safe = false;
                                if (!IsOnPosition(_Axis_Discharge_Z, Recipe.DischargeZSafePos))
                                {
                                    if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                    {
                                        AlarmWork();
                                        OutputError("Z轴到安全位置运动失败!");
                                        _Axis_Discharge_Z.StopSlowly();
                                        return false;
                                    }
                                }

                                if (!IsNGLineCylinderInside)
                                {
                                    if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeXSafePos))
                                    {
                                        _Axis_Discharge_X.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    OutputError("NG出料流水线未到安全位!");
                                }

                                while (!IsNGLineCylinderInside)
                                {
                                    if (DischargeStop)
                                    {
                                        b_checkchanel = EProductAtt.NG;
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    if (_IsStop)
                                    {
                                        return false;
                                    }
                                    Thread.Sleep(5);
                                }

                                if (IsNGHaveSth)
                                {
                                    OutputMessage("NG流水线有料");
                                    if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeXSafePos))
                                    {
                                        _Axis_Discharge_X.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }

                                    while (IsNGHaveSth || (!IsNGLineCylinderInside))
                                    {
                                        if (DischargeStop)
                                        {

                                            b_checkchanel = EProductAtt.NG;
                                            _IsDischargeWorking = false;
                                            return false;
                                        }

                                        if (_IsStop)
                                        {
                                            return false;
                                        }
                                        Thread.Sleep(2);
                                    }

                                    while (!_IsAutoRun)
                                    {
                                        Thread.Sleep(50);
                                        if (_IsStop)
                                        {
                                            return false;
                                        }
                                    }

                                    if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeX_NG_PullPos, Config.DiscargeXNGUnLoadSpeed))
                                    {
                                        _Axis_Discharge_X.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                }
                                else
                                {
                                    while (!_IsAutoRun)
                                    {
                                        Thread.Sleep(50);
                                        if (_IsStop)
                                        {
                                            return false;
                                        }
                                    }


                                    if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeX_NG_PullPos, Config.DiscargeXNGUnLoadSpeed))
                                    {
                                        _Axis_Discharge_X.StopSlowly();
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                }

                                while (IsNGHaveSth)
                                {
                                    if (DischargeStop)
                                    {
                                        b_checkchanel = EProductAtt.NG;
                                        _IsDischargeWorking = false;
                                        return false;
                                    }

                                    if (_IsStop)
                                    {
                                        return false;
                                    }
                                    Thread.Sleep(50);
                                }

                                while (!_IsAutoRun)
                                {
                                    Thread.Sleep(50);
                                    if (_IsStop)
                                    {
                                        return false;
                                    }
                                }

                                while (!IsNGLineCylinderInside)
                                {
                                    if (DischargeStop)
                                    {
                                        b_checkchanel = EProductAtt.NG;
                                        _IsDischargeWorking = false;
                                        return false;
                                    }
                                    if (_IsStop)
                                    {
                                        return false;
                                    }
                                    Thread.Sleep(5);
                                }


                                if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZ_NG_PullPos))
                                {
                                    _Axis_Discharge_Z.StopSlowly();
                                    _IsDischargeWorking = false;
                                    return false;
                                }
                                CloseDischargeSuck();
                                OpenDischargeBlow();
                                CloseFPCDischargeSuck();
                                Thread.Sleep(Config.RobotBlowDelay);
                                if (!AxisMoveTo(_Axis_Discharge_Z, Recipe.DischargeZSafePos, Config.DiscargeZUpSpeed) || !IsDichargeOnORG)
                                {
                                    _Axis_Discharge_Z.StopSlowly();
                                    _IsDischargeWorking = false;
                                }
                                CloseDischargeBlow();
                                if (QueueBendOut.Count == 0)
                                {
                                    if (!AxisMoveTo(_Axis_Discharge_X, Recipe.DischargeXSafePos))
                                    {
                                        _Axis_Discharge_X.StopSlowly();
                                        _IsDischargeWorking = false;
                                    }
                                }
                                b_DischargeCarry_Safe = true;
                            }
                            Step_Discharge++;
                            break;
                        case 4:
                            stc.Stop();
                            OnDischargeMsgArrived("下料完成");
                            total_time += stc.ElapsedMilliseconds; //TT统计
                            Total++;
                            Cycle_Time = ((double)total_time) / (Total * 1000);
                            Singel_Time = ((double)stc.ElapsedMilliseconds) / 1000;
                            OnTTArrived(new double[] { Cycle_Time, Singel_Time });
                            OutputMessage("单片TT:" + (((double)stc.ElapsedMilliseconds) / 1000).ToString());
                            if (Total > 20)
                            {
                                Total = 0;
                                total_time = 0;
                            }



                            stc.Reset();
                            stc.Start();
                            Step_Discharge = 0;
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\ErrCode.csv", DateTime.Now.ToString("yyyyMMddHHmmss") + "出料" + ex.ToString());
                return false;
            }
        }


        public void TakeOutBoard()
        {
            while (true)
            {
                if (Config.IsRunNull || !Config.DsgLine_FullSensor_Enable)//如果是调试模式 则屏蔽满料光纤 就不会等待流片才放料
                {
                    break;
                }

                while (!IsDischargeHave)
                {
                    Thread.Sleep(100);
                }
                while (IsDischargeHave)
                {
                    Thread.Sleep(100);
                }
                break;

            }
        }


        private bool IsDichargeOnORG
        {
            get
            {
                if (!Config.IsDischargeZMonitor)
                {
                    return true;
                }
                if (!Axis_Discharge_Z.IsHomeActived)
                {
                    MeasurementContext.OutputError("下料Z轴安全位置未感应到原点信号");
                    EstopMachine();
                }
                return Axis_Discharge_Z.IsHomeActived;
            }
        }
        bool _IsLoadWorking = true;
        int Step_load = 0;
        int nFeedIndex = 3;

        private bool _IsStop = false;
        public bool IsStop
        {
            get
            {
                return _IsStop;
            }
            set
            {
                _IsStop = value;
            }
        }

        private bool _ForceStop = false;
        public bool ForceStop
        {
            get
            {
                return _ForceStop;
            }
            set
            {
                _ForceStop = value;
            }
        }


        private bool _IsCycleStop;
        public bool IsCycleStop
        {
            get
            {
                return _IsCycleStop;
            }
            set
            {
                _IsCycleStop = value;
            }
        }

        //相应工位是否有料标志变量
        private bool FlagFeed = true;
        private bool FlagTear1 = true;
        private bool FlagTear2 = true;
        private bool FlagTear3 = true;
        private bool FlagTear1Have = false;
        private bool FlagTear2Have = false;
        private bool FlagTear3Have = false;
        private bool FlagTranfer = true;
        private bool FlagBend1 = true;
        private bool FlagBend2 = true;
        private bool FlagBend3 = true;
        private bool FlagDischarge = true;

        public void StopMachine()
        {
            if (_WorkStatus != WorkStatuses.Running)
            {
                OutputError("当前状态无法点击清料!");
                return;
            }
            _WorkStatus = WorkStatuses.Stoping;
            OnWorkStatusChanged();
            UnlockSafeDoor();
            _IsCycleStop = true;
        }
        public void EstopMachine()  //急停
        {
            _IsStop = true;
            _IsReset = false;
            _IsAutoRun = false;

            _IsLoadWorking = false;
            _IsleftsmWorking = false;
            _IsmidsmWorking = false;
            _IsrightsmWorking = false;
            _IsleftbendWorking = false;
            _IsmidbendWorking = false;
            _IsrightbendWorking = false;
            _IsTransferWorking = false;
            _IsDischargeWorking = false;

            AlarmWork();

            CloseYellowLight();
            UnlockSafeDoor();

            _MotionA.SetGoHome(false);
            _MotionA.CancelGoHome();
            _MotionA.EndGoHome();
            _MotionA.EndMotion();
            _MotionA.StopSudden();

            _MotionB.SetGoHome(false);
            _MotionB.CancelGoHome();
            _MotionB.EndGoHome();
            _MotionB.EndMotion();
            _MotionB.StopSudden();

            _MotionC.SetGoHome(false);
            _MotionC.CancelGoHome();
            _MotionC.EndGoHome();
            _MotionC.EndMotion();
            _MotionC.StopSudden();

            _MotionD.SetGoHome(false);
            _MotionD.CancelGoHome();
            _MotionD.EndGoHome();
            _MotionD.EndMotion();
            _MotionD.StopSudden();

            Thread.Sleep(400);
            SetMotionSevOff();

            _WorkStatus = WorkStatuses.Emg;
            OnWorkStatusChanged();
        }

        public void StopworkStausChanged()
        {
            while ((_IsleftsmWorking || _IsmidsmWorking || _IsrightsmWorking
                || _IsleftbendWorking || _IsmidbendWorking || _IsrightbendWorking
                || _IsLoadWorking || _IsTransferWorking || _IsDischargeWorking))
            {
                Thread.Sleep(50);
                Application.DoEvents();
                //Task s1 = Task.Run(new Action(() =>
                //{
                //    while (false)
                //    {

                //    }

                //}));

                //int ID = s1.Id;
            }
            _WorkStatus = WorkStatuses.Stopped;
            OnWorkStatusChanged();
        }

        public void PauseMachine()
        {
            _IsAutoRun = false;
            UnlockSafeDoor();
            _WorkStatus = WorkStatuses.Pausing;
            OnWorkStatusChanged();
        }
        public void ContiMachine()
        {
            LockSafeDoor();
            _WorkStatus = WorkStatuses.Running;
            OnWorkStatusChanged();
            _IsAutoRun = true;
        }

        public int LoadCellWork(ref double Newton, TcpClientHelper CellNet)
        {
            int i_value = 1;
            int count = 0;
            try
            {
                count++;
                if (!CellNet.SendCommand(str_LoadCellSend))
                {
                    Newton = -200;
                    return -1;
                }
                else
                {
                    if (CellNet == LoadCell1Net)
                    {
                        Stopwatch stt = new Stopwatch();
                        stt.Restart();

                        while (str_Cell1Rev.Length < 2)
                        {
                            if (stt.ElapsedMilliseconds > NetTimeOut)
                            {
                                //  OutputError("称重接收信息超时报警!");
                                Newton = -200;
                                return -2;
                            }
                            if (_IsStop)
                            {
                                Newton = -200;
                                return -2;
                            }
                            Thread.Sleep(20);
                        }
                        if (str_Cell1Rev.Length < 8)
                        {

                            str_Cell1Rev = "";
                            Newton = -200;
                            return -1;
                        }
                        else
                        {
                            Newton = double.Parse(str_Cell1Rev.Substring(1, 7)) * Config.WeighValueScale;/// 100.0
                            OutputMessage(Newton.ToString());
                            str_Cell1Rev = "";
                        }

                    }
                    else if (CellNet == LoadCell2Net)
                    {
                        Stopwatch stt = new Stopwatch();
                        stt.Restart();
                        while (str_Cell2Rev.Length < 2)
                        {

                            if (stt.ElapsedMilliseconds > NetTimeOut)
                            {
                                // OutputError("称重接收信息超时报警!");
                                Newton = -200;
                                return -2;
                            }
                            if (_IsStop)
                            {
                                Newton = -200;
                                return -2;
                            }
                            Thread.Sleep(20);


                        }
                        if (str_Cell2Rev.Length < 9)
                        {
                            str_Cell2Rev = "";
                            Newton = -200;
                            return -1;
                        }
                        else
                        {
                            Newton = double.Parse(str_Cell2Rev.Substring(1, 7)) * Config.WeighValueScale;/// 100.0
                            OutputMessage(Newton.ToString());
                            str_Cell2Rev = "";
                        }
                    }
                    else
                    {
                        Stopwatch stt = new Stopwatch();
                        stt.Restart();
                        while (str_Cell3Rev.Length < 2)
                        {

                            if (stt.ElapsedMilliseconds > NetTimeOut)
                            {
                                // OutputError("称重接收信息超时报警!");
                                Newton = -200;
                                return -2;
                            }
                            if (_IsStop)
                            {
                                Newton = -200;
                                return -2;
                            }
                            Thread.Sleep(20);
                        }
                        if (str_Cell3Rev.Length < 9)
                        {
                            str_Cell3Rev = "";
                            Newton = -200;
                            return -1;
                        }
                        else
                        {
                            Newton = double.Parse(str_Cell3Rev.Substring(1, 7)) * Config.WeighValueScale;// 100.0
                            OutputMessage(Newton.ToString());
                            str_Cell3Rev = "";
                        }
                    }
                    return i_value;
                }
            }
            catch
            {
                return -1;
            }
        }

        public int LoadCellResetVal(ref int Newton, TcpClientHelper CellNet)
        {
            int i_value = 1;
            if (!CellNet.SendCommand(str_ResetCellValSend))
            {
                Newton = -200;
                OutputError("称重清零失败!");
                return -1;
            }
            else
            {
                Stopwatch stt = new Stopwatch();
                stt.Restart();

                while (str_Cell1Rev.Length < 2)
                {
                    if (stt.ElapsedMilliseconds > 60)
                    {
                        // OutputError("称重接收信息超时报警!");
                        Newton = -200;
                        return -2;
                    }
                    if (_IsStop)
                    {
                        Newton = -200;
                        return -2;
                    }
                    Thread.Sleep(20);
                }
                OutputMessage("称重清零完成!");
            }

            return i_value;
        }

        private bool GoloadPick()
        {
            bool result = true;
            if (!Config.IsRunNull)
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                if (!Config.IsFeedCylinderEnable)//不启用上料翻转机构
                {
                    while (GetIOOutStatus(Config.SuplyBeltIOOut))
                    {
                        if (FeedStop)
                        {
                            _IsLoadWorking = false;
                            return false;
                        }
                        Thread.Sleep(30);
                        if (FeedStop)
                        {
                            return false;
                        }
                        if (st.ElapsedMilliseconds > 60000)
                        {
                            b_timespan_flag = true;
                        }
                        WaitMilliSec(5);
                        if (IsCycleStop)
                        {
                            break;
                        }
                        if (_IsStop)
                        {
                            return false;
                        }
                    }
                }

                if (IsCycleStop)
                {
                    OutputMessage("上料工位停止");
                    OnFeedMsgArrived("停止");
                    _IsLoadWorking = false;
                    b_timespan_flag = false;

                    FlagFeed = false;
                    return false;
                }

                if (!CheckAxisDone(_Axis_Load_Y))
                {
                    return false;
                }
                OnFeedMsgArrived("等待物料");
                //等上料翻转有料或流水线进料   //ZGH20220913增加与上游设备交互
                while (/*(Config.IsFeedCylinderEnable && !_IsLoadMachineReady) || (!Config.IsFeedCylinderEnable && !IsLoadBeltHaveSth)|| */(!CanGetIOInStatus(Config.ReceiveUpstream_Safe_IOInEx) && !CanGetIOInStatus(Config.ReceiveUpstream_Request_IOInEx)))
                {

                    if (FeedStop)
                    {
                        _IsLoadWorking = false;
                        return false;
                    }

                    if (st.ElapsedMilliseconds > 40000)
                    {
                        b_timespan_flag = true;

                    }
                    if (_IsCycleStop)
                    {
                        FlagFeed = false;
                        _IsLoadWorking = false;
                        b_timespan_flag = true;
                        break;
                    }
                    WaitMilliSec(5);
                    if (_IsStop)
                    {
                        return false;
                    }
                }

                if (_IsCycleStop)
                {
                    OutputMessage("上料工位停止!");
                    OnFeedMsgArrived("停止");
                    b_timespan_flag = true;
                    return false;
                }
                Thread.Sleep(10);
            }

            //if (!Config.IsRunNull)
            //{
            //    while (!Config.IsFeedCylinderEnable && _IsBeltMotorFirst)//流水线进料的话需要等待进料完成推料动作
            //    {
            //        if (FeedStop)
            //        {
            //            _IsLoadWorking = false;
            //            return false;
            //        }
            //        if (_IsCycleStop)
            //        {
            //            FlagFeed = false;
            //            _IsLoadWorking = false;
            //            break;
            //        }
            //        if (_IsStop)
            //        {
            //            return false;
            //        }
            //        Thread.Sleep(10);
            //    }
            //}
            else
            {
                while ((Config.IsFeedCylinderEnable && !_IsLoadMachineReady) || (!CanGetIOInStatus(Config.ReceiveUpstream_Safe_IOInEx) && !CanGetIOInStatus(Config.ReceiveUpstream_Request_IOInEx)&&MeasurementContext.Config.IsControlUpStreamEnable))//流水线进料的话需要等待进料完成推料动作
                {
                    if (FeedStop)
                    {
                        _IsLoadWorking = false;
                        return false;
                    }
                    if (_IsCycleStop)
                    {
                        FlagFeed = false;
                        _IsLoadWorking = false;
                        break;
                    }
                    if (_IsStop)
                    {
                        return false;
                    }
                    Thread.Sleep(10);
                }
            }

            if (_IsCycleStop)
            {
                OutputMessage("上料工位停止!");
                OnFeedMsgArrived("停止");
                b_timespan_flag = true;
                return false;
            }
            OnFeedMsgArrived("开始取料");
            b_timespan_flag = false;
            while (!_IsAutoRun)
            {
                if (_IsStop)
                {
                    return false;
                }
                Thread.Sleep(5);
            }
            CloseToUpstream_Safe();//ZGH20220913增加与上游设备交互
            if (!AxisMoveTo(_Axis_Load_X, Recipe.LoadXpos))
            {
                return false;
            }

            if (!FeedZWork(1, Recipe.LoadZpos))
            {
                return false;
            }
            OpenLoadSTSuck();
            OpenLoadSTFPCSuck();
            CloseFeedRotateSuck();
            Thread.Sleep(Config.RobotFetchDelay);
            return result;
        }
        public void WaitMilliSec(double ms)
        {
            Stopwatch stw = Stopwatch.StartNew();
            while (stw.Elapsed.TotalMilliseconds < ms)
            {
                if (_IsStop)
                {
                    break;
                }
                Thread.Sleep(1);
            }
        }
        public bool WaitIOMSec(MeasurementConfig.ConfigIOIn IOIn, double ms = 3000, bool ioflag = true)
        {
            bool flag = true;
            if (GetIOInStatus(IOIn) == ioflag) return flag;
            if ((Config.IsRunNull && IOIn.Name.Contains("真空")))
            {
                Thread.Sleep(200);
                return flag;
            }
            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (GetIOInStatus(IOIn) == ioflag)
                {
                    break;
                }
                Thread.Sleep(20);
            }

            if (GetIOInStatus(IOIn) != ioflag)
            {
                AlarmWork();
                OutputError($"等待信号{IOIn.Name}失败!");
                flag = false;
            }

            return flag;
        }
        public bool WaitIOExMSec(MeasurementConfig.ConfigIOInEx IOInEx, double ms = 3000, bool ioflag = true)
        {
            bool flag = true;
            if (CanGetIOInStatus(IOInEx) == ioflag) return flag; ;
            if ((Config.IsRunNull && IOInEx.Name.Contains("真空")))
            {
                Thread.Sleep(100);
                return flag;
            }


            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (CanGetIOInStatus(IOInEx) == ioflag)
                {
                    break;
                }
                Thread.Sleep(20);
            }

            if (CanGetIOInStatus(IOInEx) != ioflag)
            {
                AlarmWork();
                OutputError($"等待信号{IOInEx.Name}失败!");
                flag = false;
            }


            return flag;
        }

        #region 上下料翻转中转等待气缸
        public bool WaitIOMSec_FeedUPCylinderUP(double ms)
        {
            bool flag = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsFeed_UPCylinder_UP)
                {
                    break;
                }
                Thread.Sleep(20);
            }
            if (!IsFeed_UPCylinder_UP)
            {
                AlarmWork();
                flag = false;
            }
            return flag;
        }


        public bool WaitIOMSec_TransferUDCylinderUP(double ms)
        {
            bool flag = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (CanGetIOInStatus(Config.Transfer_UDCylinderUP_IOInEx))
                {
                    break;
                }
                Thread.Sleep(20);
            }
            if (!CanGetIOInStatus(Config.Transfer_UDCylinderUP_IOInEx))
            {
                AlarmWork();
                OutputError($"等待信号{Config.Transfer_UDCylinderUP_IOInEx.Name}失败");
                flag = false;
            }
            return flag;
        }


        public bool WaitIOMSec_TransferUDCylinderDown(double ms)
        {
            bool flag = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (CanGetIOInStatus(Config.Transfer_UDCylinderDown_IOInEx))
                {
                    break;
                }
                Thread.Sleep(20);
            }

            if (!CanGetIOInStatus(Config.Transfer_UDCylinderDown_IOInEx))
            {
                AlarmWork();
                OutputError($"等待信号{Config.Transfer_UDCylinderDown_IOInEx.Name}失败");
                flag = false;
            }

            return flag;
        }

        public bool WaitIOMSec_FeedUPCylinderDown(double ms)
        {
            bool flag = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsFeed_UPCylinder_Down)
                {
                    break;
                }
            }

            if (!IsFeed_UPCylinder_Down)
            {
                AlarmWork();
                flag = false;
            }


            return flag;
        }

        public bool WaitIOMSec_FeedRotateCylinderUP(double ms)
        {
            bool flag = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsFeed_RotateCylinder_UP)
                {
                    break;
                }
            }

            if (!IsFeed_RotateCylinder_UP)
            {
                AlarmWork();
                flag = false;
            }


            return flag;
        }


        public bool WaitIOMSec_FeedRotateCylinderDown(double ms)
        {
            bool flag = true;
            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsFeed_RotateCylinder_Down)
                {
                    break;
                }
            }


            if (!IsFeed_RotateCylinder_Down)
            {
                AlarmWork();
                flag = false;
            }


            return flag;
        }

        public bool WaitIOMSec_DischargeUPCylinderUP(double ms)
        {
            bool flag = true;


            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsDischarge_UPCylinder_UP)
                {
                    break;
                }

            }

            if (!Config.IsRunNull)
            {
                if (!IsDischarge_UPCylinder_UP)
                {
                    AlarmWork();
                    flag = false;
                }
            }

            return flag;
        }

        /// <summary>
        /// 下料Z气缸与下料翻转上下IO相同
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool WaitIOMSec_DischargeAxisZCylinderUP(double ms)
        {
            bool flag = true;


            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsDischarge_UPCylinder_UP)
                {
                    break;
                }

            }

            if (!Config.IsRunNull)
            {
                if (!IsDischarge_UPCylinder_UP)
                {
                    AlarmWork();
                    flag = false;
                }
            }

            return flag;
        }

        public bool WaitIOMSec_DischargeAxisZCylinderDown(double ms)
        {
            bool flag = true;


            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsDischarge_UPCylinder_Down)
                {
                    break;
                }

            }




            if (!Config.IsRunNull)
            {
                if (!IsDischarge_UPCylinder_Down)
                {
                    AlarmWork();
                    flag = false;
                }
            }

            return flag;
        }

        public bool WaitIOMSec_DischargeUPCylinderDown(double ms)
        {
            bool flag = true;


            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsDischarge_UPCylinder_Down)
                {
                    break;
                }

            }
            if (!Config.IsRunNull)
            {
                if (!IsDischarge_UPCylinder_Down)
                {
                    AlarmWork();
                    flag = false;
                }
            }

            return flag;
        }

        public bool WaitIOMSec_DischargeRotateCylinderUP(double ms)
        {
            bool flag = true;


            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsDischarge_RotateCylinder_UP)
                {
                    break;
                }

            }

            if (!Config.IsRunNull)
            {
                if (!IsDischarge_RotateCylinder_UP)
                {
                    AlarmWork();
                    flag = false;
                }
            }

            return flag;
        }


        public bool WaitIOMSec_DischargeRotateCylinderDown(double ms)
        {
            bool flag = true;


            Stopwatch stw = new Stopwatch();
            stw.Restart();
            while (stw.ElapsedMilliseconds < ms)
            {
                if (IsDischarge_RotateCylinder_Down)
                {
                    break;
                }

            }
            if (!IsDischarge_RotateCylinder_Down)
            {
                AlarmWork();
                flag = false;
            }
            return flag;
        }

        public bool WaitIOMSec_FeedRotateSuck(double ms)
        {
            bool flag = true;

            if (!Config.IsRunNull)
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();
                while (stw.ElapsedMilliseconds < ms)
                {
                    if (IsFeedRotate_Suck)
                    {
                        break;
                    }

                }

                if (!Config.IsRunNull)
                {
                    if (!IsFeedRotate_Suck)
                    {
                        AlarmWork();
                        flag = false;
                    }
                }
            }
            else
            {
                Thread.Sleep(100);
            }
            return flag;
        }

        public bool WaitIOMSec_FeedRotateFPCSuck(double ms)
        {
            bool flag = true;

            if (!Config.IsRunNull)
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();
                while (stw.ElapsedMilliseconds < ms)
                {
                    if (IsFeedRotate_FPCSuck)
                    {
                        break;
                    }

                }

                if (!Config.IsRunNull)
                {
                    if (!IsFeedRotate_FPCSuck)
                    {
                        AlarmWork();
                        flag = false;
                    }
                }
            }
            else
            {
                Thread.Sleep(2000);
            }
            return flag;
        }


        public bool WaitIOMSec_DischargeRotateSuck(double ms)
        {
            bool flag = true;

            if (!Config.IsRunNull)
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();
                while (stw.ElapsedMilliseconds < ms)
                {
                    if (IsDischargeRotate_Suck)
                    {
                        break;
                    }
                }

                if (!Config.IsRunNull)
                {
                    if (!IsDischargeRotate_Suck)
                    {
                        AlarmWork();
                        flag = false;
                    }
                }
            }
            else
            {
                Thread.Sleep(500);
            }
            return flag;
        }


        #endregion 

        private bool _IsAutoRun = false;
        public bool IsAutoRun
        {
            get
            {
                return _IsAutoRun;
            }
            set
            {
                _IsAutoRun = value;
            }
        }

        private bool _IsManualRun = true;

        public bool IsManualRun
        {
            get
            {
                return _IsManualRun;
            }
            set
            {
                _IsManualRun = value;
            }
        }
        public bool IsGateSafe()
        {
            bool Flag = true;
            if (Config.IsGateAlarm_Enable)
            {
                if (!IsSMBackGateSafe)
                {
                    OutputError("撕膜后门报警!");
                    //  OnMessageShow("撕膜后门报警!");
                    AlarmWork();
                    _IsAutoRun = false;
                    return false;
                }

                if (!IsSMFrontGateSafe)
                {
                    OutputError("撕膜前门报警!");
                    //  OnMessageShow("撕膜前门报警!");
                    AlarmWork();
                    _IsAutoRun = false;
                    return false;
                }

                //if (!IsSMSideGateSafe)
                //{
                //    OutputError("撕膜侧门报警!");
                //    // OnMessageShow("撕膜侧门报警!");
                //    AlarmWork();
                //    _IsAutoRun = false;
                //    return false;
                //}

                if (!IsBendBackGateSafe)
                {
                    OutputError("折弯后门报警!");
                    //  OnMessageShow("折弯后门报警!");
                    AlarmWork();
                    _IsAutoRun = false;
                    return false;
                }

                if (!IsBendFrontGateSafe)
                {
                    OutputError("折弯前门报警!");
                    // OnMessageShow("折弯前门报警!");
                    AlarmWork();
                    _IsAutoRun = false;
                    return false;
                }

                //if (!IsBendSideGateSafe)
                //{
                //    OutputError("折弯侧门报警!");
                //    // OnMessageShow("折弯侧门报警!");
                //    AlarmWork();
                //    _IsAutoRun = false;
                //    return false;
                //}
            }
            return Flag;
        }
        private bool IsSMBackGateSafe
        {
            get
            {
                return (CanGetIOInStatus(Config.SMBackGate1IOInEx) & CanGetIOInStatus(Config.SMBackGate2IOInEx));
            }
        }
        private bool IsSMFrontGateSafe
        {
            get
            {
                return (CanGetIOInStatus(Config.SMFrontGate1IOInEx) & CanGetIOInStatus(Config.SMFrontGate2IOInEx));
            }
        }
        private bool IsSMSideGateSafe
        {
            get
            {
                return (CanGetIOInStatus(Config.SMSideGate1IOInEx) & CanGetIOInStatus(Config.SMSideGate2IOInEx) &
                    CanGetIOInStatus(Config.SMSideGate3IOInEx) & CanGetIOInStatus(Config.SMSideGate4IOInEx));
            }
        }
        private bool IsBendFrontGateSafe
        {
            get
            {
                return (CanGetIOInStatus(Config.BendFrontGate1IOInEx) & CanGetIOInStatus(Config.BendFrontGate2IOInEx));
            }
        }
        private bool IsBendBackGateSafe
        {
            get
            {
                return (CanGetIOInStatus(Config.BendBackGate1IOInEx) & CanGetIOInStatus(Config.BendBackGate2IOInEx));
            }
        }
        private bool IsBendSideGateSafe
        {
            get
            {
                return (CanGetIOInStatus(Config.BendSideGate1IOInEx) & CanGetIOInStatus(Config.BendSideGate2IOInEx));
            }
        }
        public bool AutoFlag()
        {
            bool flag = true;

            if (!IsGateSafe())
            {


            }
            return flag;
        }

        private PhotoFailHander _PhotoHander = PhotoFailHander.None;
        public PhotoFailHander PhotoHander
        {
            get
            {
                return _PhotoHander;
            }
            set
            {
                _PhotoHander = value;
            }
        }

        private const int COORDINATEINDEX = 0;

        private WorkStatuses _WorkStatus = WorkStatuses.Idle;

        public WorkStatuses workstatus
        {
            get
            {
                return _WorkStatus;
            }
            set
            {
                _WorkStatus = value;
            }

        }

        private string _PointLasetSend = '\u0002' + "MEASURE" + '\u0003';
        public MeasurementCapacity Capacity
        {
            get
            {
                return MeasurementContext.Capacity;
            }
        }


        public INIHelper IniHelper
        {
            get
            {
                return MeasurementContext.inf;
            }
        }

        private bool _IsPZOK;

        private bool _IsBDOK;

        private double _ReciveCamX;

        private double _ReciveCamY;

        private string _QrCodeID = "111111111111";

        private bool _IsReverse = false;

        private bool _IsReset = false;

        private bool _StageWidthChanged = false;

        private DateTime _RunTime = DateTime.Now;

        private int _ClearIntervalNum;

        private DateTime _LaserEndTime = DateTime.Now;

        private bool _IsGoHome = false;
        public SerialPortEx CodePortEx
        {
            get
            {
                return MeasurementContext.CodeSerialPort;
            }
        }
        public bool IsPZOK
        {
            get
            {
                return _IsPZOK;
            }
        }
        public bool IsBDOK
        {
            get
            {
                return _IsBDOK;
            }
        }
        public double ReciveCamX
        {
            get
            {
                return _ReciveCamX;
            }
        }
        public double ReciveCamY
        {
            get
            {
                return _ReciveCamY;
            }
        }
        public MeasurementConfig Config
        {
            get
            {
                return MeasurementContext.Config;
            }
        }
        public MeasurementData Data
        {
            get
            {
                return MeasurementContext.Data;
            }
        }
        public MeasurementData.RecipeDataItem Recipe
        {
            get
            {
                return Data.CurrentRecipeData;
            }
        }
        public TcpClientHelper BendCCDNet
        {
            get
            {
                return MeasurementContext.BendCCDNet;
            }
        }


        public TcpClientHelper Bend2CCDNet
        {
            get
            {
                return MeasurementContext.Bend2CCDNet;
            }
        }
        public TcpClientHelper Bend3CCDNet
        {
            get
            {
                return MeasurementContext.Bend3CCDNet;
            }
        }
        public TcpClientHelper TearCCDNet
        {
            get
            {
                return MeasurementContext.TearCCDNet;
            }
        }

        public TcpClientHelper LoadCell1Net
        {
            get
            {
                return MeasurementContext.LoadCell1Net;
            }

        }

        public TcpClientHelper LoadCell2Net
        {
            get
            {
                return MeasurementContext.LoadCell2Net;
            }

        }

        public TcpClientHelper LoadCell3Net
        {
            get
            {
                return MeasurementContext.LoadCell3Net;
            }

        }

        public TcpClientHelper QRCodeNet
        {
            get
            {
                return MeasurementContext.QRCodeNet;
            }
        }


        public TcpClientHelper TestNet
        {
            get
            {
                return MeasurementContext.TestNet;
            }
        }

        public bool IsEmgButtonDown
        {
            get
            {

                MeasurementConfig.ConfigIOIn iO0 = Config.EmgStopCard0IoIn;
                MeasurementConfig.ConfigIOIn iO1 = Config.EmgStopCard1IOIn;
                MeasurementConfig.ConfigIOIn iO2 = Config.EmgStopCard2IOIn;
                MeasurementConfig.ConfigIOIn iO3 = Config.EmgStopCard3IOIn;
                return (GetIOInStatus(iO0) || GetIOInStatus(iO1) || GetIOInStatus(iO2) || GetIOInStatus(iO3));
            }
        }
        public bool IsReset
        {
            get
            {
                return _IsReset;
            }
        }
        public bool IsReverse
        {
            get
            {
                return _IsReverse;
            }
        }

        public bool IsRunButtonDown
        {
            get
            {
                //MeasurementConfig.ConfigIOIn iO = Config.RunButtonIOIn;
                //return ((iO == null ? false : iO.IsValid) ? GetIOInStatus(iO) : false);
                return true;
            }
        }

        public WorkStatuses WorkStatus
        {
            get
            {
                return _WorkStatus;
            }
            set
            {
                _WorkStatus = value;
            }
        }

        public string WorkStatusString
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                switch (_WorkStatus)
                {
                    case WorkStatuses.Emg:
                        stringBuilder.Append("急停");
                        break;
                    case WorkStatuses.Homing:
                        stringBuilder.Append("复位中");
                        break;
                    case WorkStatuses.Idle:
                        stringBuilder.Append("就绪");
                        break;
                    case WorkStatuses.Pausing:
                        stringBuilder.Append("暂停中");
                        break;
                    case WorkStatuses.Stoping:
                        stringBuilder.Append("清料中");
                        break;
                    case WorkStatuses.Running:
                        stringBuilder.Append("运行");
                        break;
                    case WorkStatuses.Error:
                        stringBuilder.Append("错误");
                        break;
                    case WorkStatuses.Pending:
                        stringBuilder.Append("请复位");
                        break;
                    case WorkStatuses.Stopped:
                        stringBuilder.Append("停止");
                        break;
                    default:
                        stringBuilder.Append("--");
                        break;
                }
                return stringBuilder.ToString();
            }
        }

        private void AddAlarm(string msg)
        {
            AddAlarm(new MeasurementAlarms.AlarmItem
            {
                AlarmInfo = msg
            });
        }

        private void AddAlarm(MeasurementAlarms.AlarmItem item)
        {
            MeasurementAlarms alarms = MeasurementContext.Alarms;
            if (alarms.Date.Year != item.Time.Year || alarms.Date.Month != item.Time.Month || alarms.Date.Day != item.Time.Day)
            {
                alarms.Save();
                alarms = new MeasurementAlarms();
                MeasurementContext.Alarms = alarms;
            }
            alarms.AlarmItems.Add(item);
            alarms.Save();
        }

        private bool CheckCanRun()
        {
            bool result = false;
            if (IsEmgButtonDown)
            {
                ShowMessage("急停开关已按下,请松开后操作!");
                result = false;
            }
            else if (_WorkStatus == WorkStatuses.Emg)
            {
                ShowMessage("设备急停状态,请先复位后操作!");
                result = false;
            }
            else if (_WorkStatus == WorkStatuses.Homing)
            {
                ShowMessage("当前状态不允许操作!");
                result = false;
            }
            else if (_WorkStatus == WorkStatuses.Running)
            {
                OutputError("当前状态不允许操作!");
                result = false;
            }
            else if (!_IsReset)
            {
                ShowMessage("设备未复位,不允许操作!");
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public void EmgStop()
        {
            _IsReset = false;
            _WorkStatus = WorkStatuses.Emg;
            OnWorkStatusChanged();

            _MotionA.SetGoHome(false);
            _MotionA.CancelGoHome();
            _MotionA.EndGoHome();
            _MotionA.EndMotion();
            _MotionA.StopSudden();
            CloseYellowLight();
            AlarmWork();
        }


        public bool ErrorClear()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                if (IsEmgButtonDown)
                {
                    ShowMessage("急停已按下，请松开后操作!");
                }
                else if (IsAutoRun)
                {
                    MeasurementContext.OutputError("设备运行中,清除状态无效");
                }
                else
                {

                    foreach (MeasurementMotion item in Motions)
                    {
                        foreach (MeasurementAxis axis in item.Axises)
                        {
                            axis.WriteERC(true);
                            if (axis.IsALM)
                            {
                                axis.WriteERC(false);
                                Thread.Sleep(50);
                                axis.WriteERC(true);
                            }
                            if (axis.IsALM)
                            {
                                //ShowMessage(string.Format("{0},伺服报警!", item.GetAxisName(axis.AxisType)));
                                return;
                            }
                        }
                    }

                    WriteSVON();
                    WorkStatus = WorkStatuses.Pending;
                    OnWorkStatusChanged();
                    OutputMessage("错误清除OK,请复位设备！");

                }
            });
            return true;
        }

        /// <summary>
        /// 单轴回零
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool GoHome(MeasurementAxis axis)
        {
            bool flag;
            if (!IsEmgButtonDown)
            {
                if ((_WorkStatus == WorkStatuses.Idle || _WorkStatus == WorkStatuses.Emg ? false : _WorkStatus == WorkStatuses.Error))
                {
                    OutputError("当前状态无法复位!");
                    flag = false;
                }
                else
                {
                    _IsGoHome = true;
                    bool res = axis.GoHome();
                    _IsGoHome = false;
                    flag = res;
                }
            }
            else
            {
                OutputError("急停开关已按下，请松开后操作！");

                flag = false;
            }
            return flag;
        }

        protected void OnMessageShow(string msg)
        {
            if (MessageShow != null)
            {
                MessageShow(this, new MessageShowEventArgs(msg));
            }
        }

        public void Pend()
        {
            if (_WorkStatus == WorkStatuses.Running)
            {
                _WorkStatus = WorkStatuses.Pending;
                OnWorkStatusChanged();
                _MotionA.StopSlowly();
            }
        }
        private void ResetExit()
        {
            _MotionA.EndGoHome();
            _MotionA.EndMotion();
            _MotionA.StopSlowly();
            _WorkStatus = WorkStatuses.Error;
            OpenRedLight();
            CloseGreenLight();
            CloseYellowLight();
            CloseBuzzer();
        }
        private void ShowMessage(string msg)
        {

            AlarmWork();
            OnMessageShow(msg);
            ClearAlarm();
        }

        public void Stop()
        {
            if ((_WorkStatus == WorkStatuses.Running ? false : _WorkStatus != WorkStatuses.Pending))
            {
                _MotionA.EndMotion();
                _MotionA.StopSlowly();
            }
            else
            {
                _WorkStatus = WorkStatuses.Stoping;
                OnWorkStatusChanged();
            }
        }

        public bool GoHome(MeasurementAxis[] axises)
        {
            MotionA.BeginMotion();
            int i = 0;
            while (i < axises.Length)
            {
                MeasurementAxis axis = axises[i] as MeasurementAxis;
                if (axis.HomeType == HomeTypes.Back)
                {
                    if (!axis.HomeMoveBack(axis.AxisSet.StrokeLength / 3))
                    {
                        MotionA.EndMotion();
                        return false;
                    }
                }
                bool result;
                if (MotionA.IsStoped)
                {
                    result = false;
                }
                else if (!axis.MoveOutHome(axis.AxisSet.HomeSpeed))
                {
                    MotionA.EndMotion();
                    result = false;
                }
                else
                {
                    result = true;
                }
                return result;
            }
            MotionA.EndMotion();
            return MotionA.GoHome(axises);
        }

        public void Pause()
        {
            _WorkStatus = WorkStatuses.Pausing;
            OnWorkStatusChanged();
        }

        public void Continuing()
        {
            _WorkStatus = WorkStatuses.Conting;
            OnWorkStatusChanged();
        }

        public MotionBase GetMotion(CardIDs cardid)
        {
            MotionBase motionBase;
            switch (cardid)
            {
                case CardIDs.None:
                    motionBase = null;
                    break;
                case CardIDs.A:
                    motionBase = _MotionA;
                    break;
                case CardIDs.B:
                    motionBase = _MotionB;
                    break;
                case CardIDs.C:
                    motionBase = _MotionC;
                    break;
                case CardIDs.D:
                    motionBase = _MotionD;
                    break;
                case CardIDs.E:
                    motionBase = null;
                    break;
                case CardIDs.F:
                    motionBase = null;
                    break;
                case CardIDs.G:
                    motionBase = null;
                    break;
                case CardIDs.H:
                    motionBase = null;
                    break;
                default:
                    motionBase = null;
                    break;
            }
            return motionBase;
        }

        private void WriteSVON()
        {



            foreach (MeasurementAxis axis in new MeasurementAxis[]
            {
                _MotionA.AxisX,
                _MotionA.AxisY,
                _MotionA.AxisZ,
                _MotionA.AxisU,
                _MotionA.AxisV,
                _MotionA.AxisW,
                _MotionA.AxisA,
                _MotionA.AxisB
            })
            {
                axis.WriteERC(true);
                _MotionA.Wait();
                axis.WriteSEVON(false);
            }


            foreach (MeasurementAxis axis in new MeasurementAxis[]
            {
                _MotionB.AxisX,
                _MotionB.AxisY,
                _MotionB.AxisZ,
                _MotionB.AxisU,
                _MotionB.AxisV,
                _MotionB.AxisW,
                _MotionB.AxisA,
                _MotionB.AxisB,
                _MotionB.AxisC,
                _MotionB.AxisD
            })
            {
                axis.WriteERC(true);
                _MotionB.Wait();
                axis.WriteSEVON(false);
            }

            foreach (MeasurementAxis axis in new MeasurementAxis[]
            {
                _MotionC.AxisX,
                _MotionC.AxisY,
                _MotionC.AxisZ,
                _MotionC.AxisU,
                _MotionC.AxisV,
                _MotionC.AxisW,
                _MotionC.AxisA,
                _MotionC.AxisB
            })
            {
                axis.WriteERC(true);
                _MotionC.Wait();
                axis.WriteSEVON(false);
            }


            foreach (MeasurementAxis axis in new MeasurementAxis[]
            {
                _MotionD.AxisX,
                _MotionD.AxisY,
                _MotionD.AxisZ,
                _MotionD.AxisU,
                _MotionD.AxisV,
                _MotionD.AxisW,
                _MotionD.AxisA,
                _MotionD.AxisB

            })
            {
                axis.WriteERC(true);
                _MotionD.Wait();
                axis.WriteSEVON(false);
            }


            CanSetIOOut(Config.AllStepMotorServoOn_IOOutEx, true);
            CanSetIOOut(Config.Left_WServoOn_IOOutEx, true);
            CanSetIOOut(Config.Mid_WServoOn_IOOutEx, true);
            CanSetIOOut(Config.Right_WServoOn_IOOutEx, true);
            CanSetIOOut(Config.SM_AOIServoOn_IOOutEx, true);
            CanSetIOOut(Config.DischargZServoOn_IOOutEx, true);
            CanSetIOOut(Config.DischargZbrak_IOOutEx, true);
        }

        public void WriteFlg()
        {
            try
            {


                string info = $"IsLoadMachineReady:{_IsLoadMachineReady}  IsBeltMotorFirst:{_IsBeltMotorFirst}  IsLoadBeltHaveSth:{IsLoadBeltHaveSth}  IsFeedLineRun:{_IsFeedLineRun}  IsFeed_RotateCylinder_UP:{IsFeed_RotateCylinder_UP} IsFeedRotate_Suck:{IsFeedRotate_Suck}";
                File.AppendAllText(Application.StartupPath + "\\Errlog" + "\\LoadWorkerStatus.csv", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + info + "\r\n");
            }
            catch (Exception ex)
            {
            }

        }


        private int[] StringTOInt(string str)
        {
            try
            {
                string[] strtmp = str.Split(',');
                int[] items = new int[strtmp.Length - 1];
                for (int i = 0; i < strtmp.Length - 1; i++)
                {
                    items[i] = Convert.ToInt32(strtmp[i]);
                }
                return items;
            }
            catch (Exception)
            {
                return null;
            }

        }


        #region 输入输出控制

        public bool OpenIOOut(MeasurementConfig.ConfigIOOut IOOut)
        {
            return SetIOOut(IOOut, true);
        }

        public bool OpenIOOutEx(MeasurementConfig.ConfigIOOutEx IOOutEx)
        {
            return CanSetIOOut(IOOutEx, true);
        }

        public bool CloseIOOut(MeasurementConfig.ConfigIOOut IOOut)
        {
            return SetIOOut(IOOut, false);
        }

        public bool CloseIOOutEx(MeasurementConfig.ConfigIOOutEx IOOutEx)
        {

            return CanSetIOOut(IOOutEx, false);
        }

        public bool GetIOInStatus(MeasurementConfig.ConfigIOIn iOIn)
        {
            bool flag;
            if ((iOIn == null ? false : iOIn.IsValid))
            {
                bool s = false;
                flag = (GetMotion(iOIn.CardID).GetIOInStatus(iOIn.IO, ref s) ? s == iOIn.Status : false);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public bool CanGetIOInStatus(MeasurementConfig.ConfigIOInEx iOIn)
        {
            bool flag;
            if ((iOIn == null ? false : iOIn.IsValid))
            {
                bool s = false;
                flag = (GetMotion(iOIn.CardID) as MeasurementMotion).CANReadIOIn((ushort)iOIn.Node, (ushort)iOIn.IO, ref s) ? s == iOIn.Status : false;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public bool GetIOOutStatus(MeasurementConfig.ConfigIOOut iOOut)
        {
            bool flag;
            if ((iOOut == null ? false : iOOut.IsValid))
            {
                bool s = false;
                flag = (GetMotion(iOOut.CardID).GetIOOutStatus(iOOut.IO, ref s) ? s == iOOut.Status : false);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public bool CanGetIOOutStatus(MeasurementConfig.ConfigIOOutEx iOOut)
        {
            bool flag;
            if ((iOOut == null ? false : iOOut.IsValid))
            {
                bool s = false;
                flag = (GetMotion(iOOut.CardID) as MeasurementMotion).CANReadIOOut(iOOut.Node, iOOut.IO, ref s) ? s == iOOut.Status : false;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public bool SetIOOutReverse(MeasurementConfig.ConfigIO ioout)
        {
            bool flag;
            if (ioout == null ? false : ioout.IsValid)
            {
                MeasurementMotion motion = GetMotion(ioout.CardID) as MeasurementMotion;
                bool s = false;
                if (!ioout.IsIOEx)
                {
                    if (motion.GetIOOutStatus(ioout.IO, ref s))
                    {
                        bool res = motion.SetIOOutStatus(ioout.IO, !s);
                        flag = res;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    if (motion.CANReadIOOut(1, (ushort)ioout.IO, ref s))
                    {
                        bool res = motion.CANWriteIOOut(1, ioout.IO, !s);
                        flag = res;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public bool SetIOOut(MeasurementConfig.ConfigIOOut iOOut, bool delay, double delaymuti, bool reverse)
        {
            bool flag;
            if (iOOut == null ? false : iOOut.IsValid)
            {
                MotionBase motionBase = GetMotion(iOOut.CardID);
                bool ss = false;
                if (motionBase.GetIOOutStatus(iOOut.IO, ref ss))
                {
                    if (!reverse)
                    {
                        if (ss == iOOut.Status)
                        {
                            flag = true;
                            return flag;
                        }
                    }
                    else if (ss != iOOut.Status)
                    {
                        flag = true;
                        return flag;
                    }
                    if (motionBase.SetIOOutStatus(iOOut.IO, (!reverse ? iOOut.Status : !iOOut.Status)))
                    {
                        if ((!delay ? false : iOOut.Delay > 0))
                        {
                            if (!motionBase.Wait((int)(iOOut.Delay * delaymuti)))
                            {
                                flag = false;
                                return flag;
                            }
                        }
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public bool SetIOOut(MeasurementConfig.ConfigIOOut iOOut, bool delay, bool reverse)
        {
            return SetIOOut(iOOut, delay, 1, reverse);
        }

        public bool SetIOOut(MeasurementConfig.ConfigIOOut iOOut, bool status)
        {
            bool flag;
            if (iOOut == null ? false : iOOut.IsValid)
            {
                MotionBase motion = GetMotion(iOOut.CardID);
                flag = motion.SetIOOutStatus(iOOut.IO, (status ? iOOut.Status : !iOOut.Status));


            }
            else
            {
                flag = false;
            }
            return flag;

        }

        public bool CanSetIOOut(MeasurementConfig.ConfigIOOutEx iOOut, bool status)
        {
            bool flag;



            if (iOOut == null ? false : iOOut.IsValid)
            {
                MeasurementMotion motion = GetMotion(iOOut.CardID) as MeasurementMotion;
                // flag = (GetMotion(iOOut.CardID) as MeasurementMotion).CANWriteIOOut(iOOut.Node, iOOut.IO, (status ? iOOut.Status : !iOOut.Status));//
                flag = motion.CANWriteIOOut(iOOut.Node, iOOut.IO, (status ? iOOut.Status : !iOOut.Status));//


                if (iOOut.CardID == Config.Feed_RotateCylinder_IOOutEx.CardID && iOOut.IO == Config.Feed_RotateCylinder_IOOutEx.IO)
                {
                    if (iOOut.Name != Config.Feed_RotateCylinder_IOOutEx.Name)
                    {
                        MeasurementContext.OutputError($"{iOOut.Name}与{Config.Feed_RotateCylinder_IOOutEx.Name}配置地址冲突");
                    }
                }

                if (iOOut.CardID == Config.Feed_RotateCylinderORG_IOOutEx.CardID && iOOut.IO == Config.Feed_RotateCylinderORG_IOOutEx.IO)
                {
                    if (iOOut.Name != Config.Feed_RotateCylinderORG_IOOutEx.Name)
                    {
                        MeasurementContext.OutputError($"{iOOut.Name}与{Config.Feed_RotateCylinderORG_IOOutEx.Name}配置地址冲突");
                    }
                }

                //bool s =false;
                //if (motion.CANReadIOOut(1, (ushort)iOOut.IO, ref s))
                //{
                //    bool res = motion.CANWriteIOOut(1, iOOut.IO, !s);
                //    flag = res;
                //}
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        #endregion

        #region 事件







        public event ChartResetDel ChartResetChange;
        public event ChartUpdateDel ChartUpdateChange;
        public event SMTotalUpdateDel SMTotalUpdateChange;


        public event EventHandler RecipeChanged;

        public event EventHandler WorkStatusChanged;

        public event EventHandler MessageShow;

        public event EventHandler PointChanged;

        public event EventHandler PointDataArrived;

        public event EventHandler GlueSystemStatusChanged;

        public event EventHandler NinePointBegin;

        public event EventHandler CimInfoChanged;

        public event EventHandler CimTimeChanged;

        public event EventHandler<string> FeedMsgArrived;

        public event EventHandler<string> Tear1MsgArrived;

        public event EventHandler<string> Tear2MsgArrived;

        public event EventHandler<string> Tear3MsgArrived;

        public event EventHandler<string> TranferMsgArrived;

        public event EventHandler<string> Bend1MsgArrived;

        public event EventHandler<string> Bend2MsgArrived;

        public event EventHandler<string> Bend3MsgArrived;

        public event EventHandler<string> Bend1DWArrived;

        public event EventHandler<string> Bend2DWArrived;

        public event EventHandler<string> Bend3DWArrived;

        public event EventHandler<string> DischargeMsgArrived;

        public event EventHandler<double[]> TTArrived;

        public void OnFeedMsgArrived(string msg)
        {
            if (FeedMsgArrived != null)
            {
                FeedMsgArrived(this, msg);
            }
        }

        public void OnTear1MsgArrived(string msg)
        {
            if (Tear1MsgArrived != null)
            {
                Tear1MsgArrived(this, msg);
            }
        }




        public void OnTear2MsgArrived(string msg)
        {
            if (Tear2MsgArrived != null)
            {
                Tear2MsgArrived(this, msg);
            }
        }


        public void OnTear3MsgArrived(string msg)
        {
            if (Tear3MsgArrived != null)
            {
                Tear3MsgArrived(this, msg);
            }
        }

        public void OnBend1MsgArrived(string msg)
        {
            if (Bend1MsgArrived != null)
            {
                Bend1MsgArrived(this, msg);
            }
        }

        public void OnBend2MsgArrived(string msg)
        {
            if (Bend2MsgArrived != null)
            {
                Bend2MsgArrived(this, msg);
            }
        }




        public void OnBend3MsgArrived(string msg)
        {
            if (Bend3MsgArrived != null)
            {
                Bend3MsgArrived(this, msg);
            }
        }

        public void OnBend1DWArrived(string msg)
        {
            if (Bend1DWArrived != null)
            {
                Bend1DWArrived(this, msg);
            }
        }


        public void OnBend2DWArrived(string msg)
        {
            if (Bend2DWArrived != null)
            {
                Bend2DWArrived(this, msg);
            }
        }


        public void OnBend3DWArrived(string msg)
        {
            if (Bend3DWArrived != null)
            {
                Bend3DWArrived(this, msg);
            }
        }
        public void OnTransferMsgArrived(string msg)
        {
            if (TranferMsgArrived != null)
            {
                TranferMsgArrived(this, msg);
            }
        }




        public void OnDischargeMsgArrived(string msg)
        {
            if (DischargeMsgArrived != null)
            {
                DischargeMsgArrived(this, msg);
            }
        }


        public void OnTTArrived(double[] CTime)
        {
            if (TTArrived != null)
            {
                TTArrived(this, CTime);
            }
        }



        private void OnCimTimeChanged()
        {
            if (CimTimeChanged != null)
            {
                CimTimeChanged(this, null);
            }
        }

        public void OnCimInfoChanged(CimInfoChangedEventArgs cimInfo)
        {
            if (CimInfoChanged != null)
            {
                CimInfoChanged(this, cimInfo);
            }
        }

        private void OnNinePointBegin()
        {
            if (NinePointBegin != null)
            {
                NinePointBegin(this, null);
            }
        }

        public void OnWorkStatusChanged()
        {
            if (WorkStatusChanged != null)
            {
                WorkStatusChanged(this, null);
            }
        }

        public void OnRecipeChanged()
        {
            if (RecipeChanged != null)
            {
                RecipeChanged(this, null);
            }
            _IsReset = false;
            OutputMessage("<<更换设备机种！请复位....");
        }

        public void OutputError(string msg, bool issave = false)
        {

            MeasurementContext.OutputError(msg, issave);
        }

        private void OutputMessage(string msg)
        {
            MeasurementContext.OutputMessage(msg);
        }

        private void OnPointChanged(PointChangedEventArgs eventArgs)
        {
            if (PointChanged != null)
            {
                PointChanged(this, eventArgs);
            }
        }

        private void OnPointDataArrived(double data)
        {
            if (PointDataArrived != null)
            {
                PointDataArrivedEventArgs args = new PointDataArrivedEventArgs(data);
                PointDataArrived(this, args);
            }
        }

        private void OnGlueSystemStatusChanged()
        {
            if (GlueSystemStatusChanged != null)
            {
                GlueSystemStatusChanged(this, null);
            }
        }

        #endregion

        #region IO监听

        private void IOListener_AxisIOStatusChanged(object sender, EventArgs e)
        {
            AxisIOStatusEventArgs axisIOStatusEventArg = e as AxisIOStatusEventArgs;
            MeasurementAxis axis = _MotionA[axisIOStatusEventArg.AxisType] as MeasurementAxis;
            StringBuilder stringBuilder = new StringBuilder();
            bool error = false;
            bool errorEln = false;

            stringBuilder.AppendFormat("轴{0},", _MotionA.GetAxisName(axisIOStatusEventArg.AxisType));
            if (!(axisIOStatusEventArg.AxisIOType != AxisIOTypes.ALM ? true : !axisIOStatusEventArg.Status))
            {
                stringBuilder.Append("伺服报警.");
                error = true;
            }
            else if (!(axisIOStatusEventArg.AxisIOType != AxisIOTypes.ELN ? true : !axisIOStatusEventArg.Status))
            {
                if ((!_IsGoHome ? _WorkStatus != WorkStatuses.Homing : false)) //|| axis.HomeType != HomeTypes.Back 
                {
                    stringBuilder.Append("负限位报警.");
                    errorEln = true;
                }
            }
            else if (axisIOStatusEventArg.AxisIOType != AxisIOTypes.ELP ? false : axisIOStatusEventArg.Status)
            {
                if ((!_IsGoHome || axis.HomeType != HomeTypes.Back ? _WorkStatus != WorkStatuses.Homing : false))
                {
                    stringBuilder.Append("正限位报警.");
                    errorEln = true;
                }
            }
            if (error)
            {
                OutputError(stringBuilder.ToString(), true);
                EstopMachine();
            }
            if (errorEln)
            {
                OutputError(stringBuilder.ToString(), true);
            }
        }

        private void IOListenerB_AxisIOStatusChanged(object sender, EventArgs e)
        {
            AxisIOStatusEventArgs axisIOStatusEventArg = e as AxisIOStatusEventArgs;
            MeasurementAxis axis = _MotionB[axisIOStatusEventArg.AxisType] as MeasurementAxis;
            StringBuilder stringBuilder = new StringBuilder();
            bool error = false;
            bool errorEln = false;
            stringBuilder.AppendFormat("轴{0},", _MotionB.GetAxisName(axisIOStatusEventArg.AxisType));
            if (!(axisIOStatusEventArg.AxisIOType != AxisIOTypes.ALM ? true : !axisIOStatusEventArg.Status))
            {
                stringBuilder.Append("伺服报警.");
                error = true;
            }
            else if (!(axisIOStatusEventArg.AxisIOType != AxisIOTypes.ELN ? true : !axisIOStatusEventArg.Status))
            {
                if ((!_IsGoHome ? _WorkStatus != WorkStatuses.Homing : false)) //|| axis.HomeType != HomeTypes.Back 
                {
                    stringBuilder.Append("负限位报警.");
                    errorEln = true;
                }
            }
            else if (axisIOStatusEventArg.AxisIOType != AxisIOTypes.ELP ? false : axisIOStatusEventArg.Status)
            {
                if ((!_IsGoHome || axis.HomeType != HomeTypes.Back ? _WorkStatus != WorkStatuses.Homing : false))
                {
                    stringBuilder.Append("正限位报警.");
                    errorEln = true;
                }
            }

            if (error)
            {
                OutputError(stringBuilder.ToString());
                EstopMachine();
            }

            if (errorEln)
            {
                OutputError(stringBuilder.ToString());
            }

        }


        private void IOListenerC_AxisIOStatusChanged(object sender, EventArgs e)
        {
            AxisIOStatusEventArgs axisIOStatusEventArg = e as AxisIOStatusEventArgs;
            MeasurementAxis axis = _MotionC[axisIOStatusEventArg.AxisType] as MeasurementAxis;
            StringBuilder stringBuilder = new StringBuilder();
            bool error = false;
            bool errorEln = false;
            stringBuilder.AppendFormat("轴{0},", _MotionC.GetAxisName(axisIOStatusEventArg.AxisType));
            if (!(axisIOStatusEventArg.AxisIOType != AxisIOTypes.ALM ? true : !axisIOStatusEventArg.Status))
            {
                stringBuilder.Append("伺服报警.");
                error = true;
            }
            else if (!(axisIOStatusEventArg.AxisIOType != AxisIOTypes.ELN ? true : !axisIOStatusEventArg.Status))
            {
                if ((!_IsGoHome ? _WorkStatus != WorkStatuses.Homing : false)) //|| axis.HomeType != HomeTypes.Back 
                {
                    stringBuilder.Append("负限位报警.");
                    errorEln = true;
                }
            }
            else if (axisIOStatusEventArg.AxisIOType != AxisIOTypes.ELP ? false : axisIOStatusEventArg.Status)
            {
                if ((!_IsGoHome || axis.HomeType != HomeTypes.Back ? _WorkStatus != WorkStatuses.Homing : false))
                {
                    stringBuilder.Append("正限位报警.");
                    errorEln = true;
                }
            }
            if (error)
            {
                OutputError(stringBuilder.ToString());
                EstopMachine();
            }


            if (errorEln)
            {
                OutputError(stringBuilder.ToString());
            }

        }

        private void IOListenerD_AxisIOStatusChanged(object sender, EventArgs e)
        {
            AxisIOStatusEventArgs axisIOStatusEventArg = e as AxisIOStatusEventArgs;
            MeasurementAxis axis = _MotionD[axisIOStatusEventArg.AxisType] as MeasurementAxis;
            StringBuilder stringBuilder = new StringBuilder();
            bool error = false;
            bool errorEln = false;

            stringBuilder.AppendFormat("轴{0},", _MotionD.GetAxisName(axisIOStatusEventArg.AxisType));
            if (!(axisIOStatusEventArg.AxisIOType != AxisIOTypes.ALM ? true : !axisIOStatusEventArg.Status))
            {
                stringBuilder.Append("伺服报警.");
                error = true;
            }
            else if (!(axisIOStatusEventArg.AxisIOType != AxisIOTypes.ELN ? true : !axisIOStatusEventArg.Status))
            {
                if ((!_IsGoHome ? _WorkStatus != WorkStatuses.Homing : false)) //|| axis.HomeType != HomeTypes.Back 
                {
                    stringBuilder.Append("负限位报警.");
                    errorEln = true;
                }
            }
            else if (axisIOStatusEventArg.AxisIOType != AxisIOTypes.ELP ? false : axisIOStatusEventArg.Status)
            {
                if ((!_IsGoHome || axis.HomeType != HomeTypes.Back ? _WorkStatus != WorkStatuses.Homing : false))
                {
                    stringBuilder.Append("正限位报警.");
                    errorEln = true;
                }
            }
            if (error)
            {
                OutputError(stringBuilder.ToString());
                EstopMachine();
            }

            if (errorEln)
            {
                OutputError(stringBuilder.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IOListenerA_IOInStatusChanged(object sender, EventArgs e)
        {




        }

        private void IOListenerA_IOInStatusExChanged(object sender, EventArgs e)
        {

            MotionIOListener.IOStatusChangedEventArgs ie = e as MotionIOListener.IOStatusChangedEventArgs;
            StringBuilder sb = new StringBuilder();

            if (ie.Index == Config.LeftSM_WAxis_ServoAlarmIOInEx.IO)
            {
                if (ie.NewStatus == Config.LeftSM_WAxis_ServoAlarmIOInEx.Status)
                {
                    OutputError(Config.LeftSM_WAxis_ServoAlarmIOInEx.Name);
                    EstopMachine();
                }
            }

            if (ie.Index == Config.MidSM_WAxis_ServoAlarmIOInEx.IO)
            {
                if (ie.NewStatus == Config.MidSM_WAxis_ServoAlarmIOInEx.Status)
                {
                    OutputError(Config.MidSM_WAxis_ServoAlarmIOInEx.Name);
                    EstopMachine();
                }
            }

            if (ie.Index == Config.RightSM_WAxis_ServoAlarmIOInEx.IO)
            {
                if (ie.NewStatus == Config.RightSM_WAxis_ServoAlarmIOInEx.Status)
                {
                    OutputError(Config.RightSM_WAxis_ServoAlarmIOInEx.Name);
                    EstopMachine();
                }
            }
        }




        private void IOListenerB_IOInStatusChanged(object sender, EventArgs e)
        {
            MotionIOListener.IOStatusChangedEventArgs ie = e as MotionIOListener.IOStatusChangedEventArgs;
            StringBuilder sb = new StringBuilder();

            //ZGH20220920  B卡扩展I/O_无PIN口插槽的伺服报警
            if (!Config.DischargeAxiaZCylinderEnable)
            {
                if (ie.Index /*30*/ == Config.DischargeAxisZservorAlarmIOInEx.IO/*17*/)
                {
                    if (ie.NewStatus == Config.DischargeAxisZservorAlarmIOInEx.Status)
                    {
                        OutputError(Config.DischargeAxisZservorAlarmIOInEx.Name);
                        EstopMachine();
                    }
                }
            }


        }



        private void IOListener_IOInStatusChanged(object sender, EventArgs e)
        {
            MotionIOListener.IOStatusChangedEventArgs ie = e as MotionIOListener.IOStatusChangedEventArgs;
            StringBuilder sb = new StringBuilder();

        }


        private void IOListenerD_IOInStatusChanged(object sender, EventArgs e)
        {
            MotionIOListener.IOStatusChangedEventArgs ie = e as MotionIOListener.IOStatusChangedEventArgs;
            StringBuilder sb = new StringBuilder();

            if (ie.Index == Config.NGLineButtion.IO)
            {
                if (GetIOInStatus(Config.NGLineButtion))
                {
                    SetIOOut(Config.NGLinePushCylinder, true);
                    // SetIOOut(Config.NGLinePushCylinderRetrac, false);

                }
                else
                {
                    SetIOOut(Config.NGLinePushCylinder, false);
                    // SetIOOut(Config.NGLinePushCylinderRetrac, true);
                }
            }
        }



        private void IOListenerB_IOInStatusExChanged(object sender, EventArgs e)
        {
            try
            {
                MotionIOListener.IOStatusChangedEventArgs ie = e as MotionIOListener.IOStatusChangedEventArgs;
                StringBuilder sb = new StringBuilder();
                //按钮改成解除报警
                //if (ie.Index == Config.ResetbtnIOInEx.IO)
                //{
                //    if (ie.NewStatus == Config.ResetbtnIOInEx.Status)
                //    {
                //        if (_WorkStatus != WorkStatuses.Homing)
                //        {
                //            OnExButtonDown(ButtonTypes.RESET);
                //        }
                //    }
                //}
                //else if (ie.Index == Config.StartBtnIOInEx.IO)
                //{
                //    Stopwatch stw = new Stopwatch();
                //    stw.Restart();
                //    if (ie.NewStatus == Config.StartBtnIOInEx.Status)
                //    {
                //        while (stw.ElapsedMilliseconds < 2000)
                //        {
                //            if (!GetIOInStatus(Config.StartBtnIOInEx))
                //            {
                //                if (_WorkStatus == WorkStatuses.Running)
                //                {
                //                    PauseMachine();
                //                }
                //                else if (_WorkStatus == WorkStatuses.Pausing)
                //                {
                //                    ContiMachine();
                //                }
                //                else if (_WorkStatus == WorkStatuses.Idle)
                //                {
                //                    if (!_IsManualRun)
                //                    {
                //                        _IsManualRun = true;
                //                    }
                //                    else
                //                    {
                //                        _IsManualRun = false;
                //                    }
                //                }
                //                break;
                //            }
                //        }

                //        if (stw.ElapsedMilliseconds >= 2000)
                //        {
                //            RunHander();
                //        }
                //    }
                //}

                #region 无PIN口插槽的伺服报警



                //if (ie.Index == Config.DischargeAxisZservorAlarmIOIn.IO)
                //{
                //    if (ie.NewStatus != Config.DischargeAxisZservorAlarmIOIn.Status)
                //    {
                //        OutputError(Config.DischargeAxisZservorAlarmIOIn.Name);
                //        EstopMachine();
                //    }
                //}

                
                    if (ie.Index  == Config.DischargeAxisZservorAlarmIOInEx.IO)
                    {
                        if (ie.NewStatus == Config.DischargeAxisZservorAlarmIOInEx.Status)
                        {
                            OutputError(Config.DischargeAxisZservorAlarmIOInEx.Name);
                            EstopMachine();
                        }
                    }
                


                if (ie.Index == Config.SM_CCDXAxis_Alarm_IOInEx.IO)
                {
                    if (ie.NewStatus == Config.SM_CCDXAxis_Alarm_IOInEx.Status)
                    {
                        OutputError(Config.SM_CCDXAxis_Alarm_IOInEx.Name);
                        EstopMachine();
                    }
                }

                #endregion


                if (Config.IsGateAlarm_Enable && workstatus == WorkStatuses.Running && b_AlarmFlag)
                {
                    //if (ie.Index == Config.SMSideGate1IOInEx.IO)
                    //{
                    //    if (ie.NewStatus != Config.SMSideGate1IOInEx.Status)
                    //    {
                    //        ShowMsgDoorAlarm(Config.SMSideGate1IOInEx.Name);
                    //    }
                    //}
                    //if (ie.Index == Config.SMSideGate2IOInEx.IO)
                    //{
                    //    if (ie.NewStatus != Config.SMSideGate2IOInEx.Status)
                    //    {
                    //        ShowMsgDoorAlarm(Config.SMSideGate2IOInEx.Name);
                    //    }
                    //}
                    //if (ie.Index == Config.SMSideGate3IOInEx.IO)
                    //{
                    //    if (ie.NewStatus != Config.SMSideGate3IOInEx.Status)
                    //    {
                    //        ShowMsgDoorAlarm(Config.SMSideGate3IOInEx.Name);
                    //    }

                    //}

                    //if (ie.Index == Config.SMSideGate4IOInEx.IO)
                    //{
                    //    if (ie.NewStatus != Config.SMSideGate4IOInEx.Status)
                    //    {
                    //        ShowMsgDoorAlarm(Config.SMSideGate4IOInEx.Name);
                    //    }
                    //}
                    if (ie.Index == Config.SMFrontGate2IOInEx.IO)
                    {
                        if (ie.NewStatus != Config.SMFrontGate2IOInEx.Status)
                        {
                            ShowMsgDoorAlarm(Config.SMFrontGate2IOInEx.Name);
                        }
                    }
                    if (ie.Index == Config.SMFrontGate1IOInEx.IO)
                    {
                        if (ie.NewStatus != Config.SMFrontGate1IOInEx.Status)
                        {
                            ShowMsgDoorAlarm(Config.SMFrontGate1IOInEx.Name);
                        }
                    }


                    if (ie.Index == Config.SMBackGate2IOInEx.IO)
                    {
                        if (ie.NewStatus != Config.SMBackGate2IOInEx.Status)
                        {
                            ShowMsgDoorAlarm(Config.SMBackGate2IOInEx.Name);
                        }

                    }


                    if (ie.Index == Config.SMBackGate1IOInEx.IO)
                    {
                        if (ie.NewStatus != Config.SMBackGate1IOInEx.Status)
                        {
                            ShowMsgDoorAlarm(Config.SMBackGate1IOInEx.Name);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                OutputError(ex.ToString());
            }

        }
        private void IOListenerC_IOInStatusExChanged(object sender, EventArgs e)
        {
            try
            {
                MotionIOListener.IOStatusChangedEventArgs ie = e as MotionIOListener.IOStatusChangedEventArgs;
                StringBuilder sb = new StringBuilder();


                if (Config.IsGateAlarm_Enable && workstatus == WorkStatuses.Running && b_AlarmFlag)
                {
                    if (ie.Index == Config.BendBackGate1IOInEx.IO)
                    {
                        if (ie.NewStatus != Config.BendBackGate1IOInEx.Status)
                        {
                            ShowMsgDoorAlarm(Config.BendBackGate1IOInEx.Name);
                        }
                    }

                    if (ie.Index == Config.BendBackGate2IOInEx.IO)
                    {
                        if (ie.NewStatus != Config.BendBackGate2IOInEx.Status)
                        {
                            ShowMsgDoorAlarm(Config.BendBackGate2IOInEx.Name);
                        }
                    }

                    if (ie.Index == Config.BendFrontGate2IOInEx.IO)
                    {
                        if (ie.NewStatus != Config.BendFrontGate2IOInEx.Status)
                        {
                            ShowMsgDoorAlarm(Config.BendFrontGate2IOInEx.Name);
                        }
                    }

                    if (ie.Index == Config.BendFrontGate1IOInEx.IO)
                    {
                        if (ie.NewStatus != Config.BendFrontGate1IOInEx.Status)
                        {
                            ShowMsgDoorAlarm(Config.BendFrontGate1IOInEx.Name);
                        }
                    }

                    //if (ie.Index == Config.BendSideGate2IOInEx.IO)
                    //{
                    //    if (ie.NewStatus != Config.BendSideGate2IOInEx.Status)
                    //    {
                    //        ShowMsgDoorAlarm(Config.BendSideGate2IOInEx.Name);
                    //    }
                    //}

                    //if (ie.Index == Config.BendSideGate1IOInEx.IO)
                    //{
                    //    if (ie.NewStatus != Config.BendSideGate1IOInEx.Status)
                    //    {
                    //        ShowMsgDoorAlarm(Config.BendSideGate1IOInEx.Name);
                    //    }
                    //}








                }
            }
            catch (Exception ex)
            {
                OutputError(ex.ToString());
            }

        }






        #endregion

    }
}
