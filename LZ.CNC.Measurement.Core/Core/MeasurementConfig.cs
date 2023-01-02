using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DY.Core.Configs;
using System.Windows.Forms;
using System.Collections;
using DY.CNC.Core;
using System.Diagnostics;
namespace LZ.CNC.Measurement.Core
{
    [Serializable]
    public class MeasurementConfig : ConfigBase
    {

        private bool _IsLeftSMDisabled;
        private bool _IsMidSMDisabled;
        private bool _IsRightSMDiaBled;

        private bool _IsLeftBendDisabled;
        private bool _IsMidBendDisabled;
        private bool _IsRightBendDisabled;

        private bool _IsLeftSMAOIDisabled;
        private bool _IsMidSMAOIDisabled;
        private bool _IsRightSMAOIDisabled;

        private bool _IsLeftBendAOIDiabled;
        private bool _IsMidBendAOIDiabled;
        private bool _IsRightBendAOIDisabled;

        private TimeSpan _ts_total;

        public TimeSpan Ts_Total
        {
            get
            {
                return _ts_total;
            }
            set
            {
                _ts_total = value;
            }
        }



        public bool IsLeftBendAOIDisabled
        {
            get
            {
                return _IsLeftBendAOIDiabled;
            }
            set
            {
                _IsLeftBendAOIDiabled = value;
            }
        }


        public bool IsMidBendAOIDisabled
        {

            get
            {
                return _IsMidBendAOIDiabled;
            }
            set
            {
                _IsMidBendAOIDiabled = value;
            }
        }


        public bool IsRightBendAOIDisabled
        {
            get
            {
                return _IsRightBendAOIDisabled;
            }
            set
            {
                _IsRightBendAOIDisabled = value;
            }
        }



        public bool IsLeftSMDisabled
        {
            get
            {
                return _IsLeftSMDisabled;
            }
            set
            {
                _IsLeftSMDisabled = value;
            }
        }

        public bool IsMidSMDisabled
        {
            get
            {
                return _IsMidSMDisabled;
            }
            set
            {
                _IsMidSMDisabled = value;
            }
        }


        public bool IsRightSMDisabled
        {
            get
            {
                return _IsRightSMDiaBled;
            }
            set
            {
                _IsRightSMDiaBled = value;
            }
        }


        public bool IsLeftBendDisabled
        {
            get
            {
                return _IsLeftBendDisabled;
            }
            set
            {
                _IsLeftBendDisabled = value;
            }
        }


        public bool IsMidBendDisabled
        {
            get
            {
                return _IsMidBendDisabled;
            }
            set
            {
                _IsMidBendDisabled = value;
            }
        }


        public bool IsRightBendDisabled
        {
            get
            {
                return _IsRightBendDisabled;
            }
            set
            {
                _IsRightBendDisabled = value;
            }
        }


        private bool _IsLeftSM_SM_Enable;
        public bool IsLeftSM_SM_Enable
        {
            get
            {
                return _IsLeftSM_SM_Enable;
            }
            set
            {
                _IsLeftSM_SM_Enable = value;
            }
        }


        private bool _IsMidSM_SM_Enable;
        public bool IsMidSM_SM_Enable
        {
            get
            {
                return _IsMidSM_SM_Enable;
            }
            set
            {
                _IsMidSM_SM_Enable = value;
            }
        }


        private bool _IsRightSM_SM_Enable;
        public bool IsRightSM_SM_Enable
        {
            get
            {
                return _IsRightSM_SM_Enable;
            }
            set
            {
                _IsRightSM_SM_Enable = value;
            }
        }


        public bool IsLeftSMAOIDisabled
        {
            get
            {
                return _IsLeftSMAOIDisabled;
            }
            set
            {
                _IsLeftSMAOIDisabled = value;
            }
        }


        public bool IsMidSMAOIDisabled
        {
            get
            {
                return _IsMidSMAOIDisabled;
            }
            set
            {
                _IsMidSMAOIDisabled = value;
            }
        }


        public bool IsRightSMAOIDisabled
        {
            get
            {
                return _IsRightSMAOIDisabled;
            }
            set
            {
                _IsRightSMAOIDisabled = value;
            }
        }


        private bool _IsBend1SideCCDEnable;
        public bool IsBend1SideCCDEnable
        {
            get
            {
                return _IsBend1SideCCDEnable;
            }
            set
            {
                _IsBend1SideCCDEnable = value;
            }
        }

        private bool _IsBend2SideCCDEnable;
        public bool IsBend2SideCCDEnable
        {
            get
            {
                return _IsBend2SideCCDEnable;
            }
            set
            {
                _IsBend2SideCCDEnable = value;
            }
        }


        private bool _IsBend3SideCCDEnable;
        public bool IsBend3SideCCDEnable
        {
            get
            {
                return _IsBend3SideCCDEnable;
            }
            set
            {
                _IsBend3SideCCDEnable = value;
            }
        }


        private bool _IsRunNull;
        public bool IsRunNull
        {
            get
            {
                return _IsRunNull;
            }
            set
            {
                _IsRunNull = value;
            }
        }


        private double _FeedBeltDelay;
        public double FeedBeltLineDelay
        {
            get
            {
                return _FeedBeltDelay;
            }
            set
            {
                _FeedBeltDelay = value;
            }
        }


        private double _DischargeBeltDelay;
        public double DischargeBeltDelay
        {
            get
            {
                return _DischargeBeltDelay;
            }
            set
            {
                _DischargeBeltDelay = value;
            }
        }



        private int _RobotFetchDelay;
        public int RobotFetchDelay
        {
            get
            {
                return _RobotFetchDelay;
            }
            set
            {
                _RobotFetchDelay = value;
            }
        }

        private int _RobotDropDelay;
        public int RobotDropDelay
        {
            get
            {
                return _RobotDropDelay;
            }
            set
            {
                _RobotDropDelay = value;
            }
        }

        #region 
        
        private int _SMUseMax;
        /// <summary>
        /// 撕膜耗材使用上限
        /// </summary>
        public int SMUseMax
        {
            get
            {
                return _SMUseMax;
            }

            set
            {
                _SMUseMax = value;
            }
        }

        private int _LeftSMUseCount;
        /// <summary>
        /// 撕膜耗材已使用数量
        /// </summary>
        public int LeftSMUseCount
        {
            get
            {
                return _LeftSMUseCount;
            }

            set
            {
                _LeftSMUseCount = value;
            }
        }

        private DateTime _LeftSMReplaceTime;
        /// <summary>
        /// 撕膜耗材更换的时间记录
        /// </summary>
        public DateTime LeftSMReplaceTime
        {
            get
            {
                return _LeftSMReplaceTime;
            }

            set
            {
                _LeftSMReplaceTime = value;
            }
        }


        private int _MidSMUseCount;
        /// <summary>
        /// 撕膜耗材已使用数量
        /// </summary>
        public int MidSMUseCount
        {
            get
            {
                return _MidSMUseCount;
            }

            set
            {
                _MidSMUseCount = value;
            }
        }

        private DateTime _MidSMReplaceTime;
        /// <summary>
        /// 撕膜耗材更换的时间记录
        /// </summary>
        public DateTime MidSMReplaceTime
        {
            get
            {
                return _MidSMReplaceTime;
            }

            set
            {
                _MidSMReplaceTime = value;
            }
        }

        private int _LeftSMCylinderAligningDelay;
        /// <summary>
        /// 左撕膜气缸对位延迟时间
        /// </summary>
        public int LeftSMCylinderAligningDelay
        {
            get
            {
                return _LeftSMCylinderAligningDelay;
            }
            set
            {
                _LeftSMCylinderAligningDelay = value;
            }
        }

        private int _MidSMCylinderAligningDelay;
        /// <summary>
        /// 中撕膜气缸对位延迟时间
        /// </summary>
        public int MidSMCylinderAligningDelay
        {
            get
            {
                return _MidSMCylinderAligningDelay;
            }
            set
            {
                _MidSMCylinderAligningDelay = value;
            }
        }

        private int _RightSMCylinderAligningDelay;
        /// <summary>
        /// 右撕膜气缸对位延迟时间
        /// </summary>
        public int RightSMCylinderAligningDelay
        {
            get
            {
                return _RightSMCylinderAligningDelay;
            }
            set
            {
                _RightSMCylinderAligningDelay = value;
            }
        }



        private int _RightSMUseCount;
        /// <summary>
        /// 撕膜耗材已使用数量
        /// </summary>
        public int RightSMUseCount
        {
            get
            {
                return _RightSMUseCount;
            }

            set
            {
                _RightSMUseCount = value;
            }
        }

        private DateTime _RightSMReplaceTime;
        /// <summary>
        /// 撕膜耗材更换的时间记录
        /// </summary>
        public DateTime RightSMReplaceTime
        {
            get
            {
                return _RightSMReplaceTime;
            }

            set
            {
                _RightSMReplaceTime = value;
            }
        }









        private int _TearRecheckCountParam;
        /// <summary>
        /// 撕膜失败时允许最大重新撕膜次数
        /// </summary>
        public int TearRecheckCountParam
        {
            get
            {
                return _TearRecheckCountParam;
            }

            set
            {
                _TearRecheckCountParam = value;
            }
        }

        #region 称重

        private int _LoadCellRstInterval;
        /// <summary>
        /// 称重自动清零产品间隔
        /// </summary>
        public int LoadCellTestInterval
        {
            get
            {
                return _LoadCellRstInterval;
            }

            set
            {
                _LoadCellRstInterval = value;
            }
        }

        /// <summary>
        /// 弯折记录产品数 到达指定间隔称重清零一次
        /// </summary>
        private int _Left_LoadCellPdtCout;
        public int Left_LoadCellPdtCout
        {
            get
            {
                return _Left_LoadCellPdtCout;
            }

            set
            {
                _Left_LoadCellPdtCout = value;
            }
        }


        /// <summary>
        /// 称重延时
        /// </summary>
        private int _WeighMeasureDelay;
        public int WeighMeasureDelay
        {
            get
            {
                return _WeighMeasureDelay;
            }

            set
            {
                _WeighMeasureDelay = value;
            }
        }

        /// <summary>
        /// 清零延时
        /// </summary>
        private int _WeighResetDelay;
        public int WeighResetDelay
        {
            get
            {
                return _WeighResetDelay;
            }

            set
            {
                _WeighResetDelay = value;
            }
        }



        private double _WeighValueScale;
        /// <summary>
        /// 称重倍率系数 
        /// </summary>
        public double WeighValueScale
        {
            get
            {
                return _WeighValueScale;
            }

            set
            {
                _WeighValueScale = value;
            }
        }

  

        #endregion

        private string _PrintPath;
        /// <summary>
        /// 生产数据打印路径
        /// </summary>
        public string PrintPath
        {
            get
            {
                return _PrintPath;
            }

            set
            {
                _PrintPath = value;
            }
        }

        private bool _NGNotBendOutType;
        /// <summary>
        /// NG未反折出料口 true NG流水线 False 正常流出
        /// </summary>
        public bool NGNotBendOutType
        {
            get
            {
                return _NGNotBendOutType;
            }

            set
            {
                _NGNotBendOutType = value;
            }
        }



        private bool _DischargeAxiaZCylinderEnable;
        /// <summary>
        /// 下料Z轴气缸
        /// </summary>
        public bool DischargeAxiaZCylinderEnable
        {
            get
            {
                return _DischargeAxiaZCylinderEnable;
            }

            set
            {
                _DischargeAxiaZCylinderEnable = value;
            }
        }


        private int _DiscargeZUpSpeed;
        /// <summary>
        /// 下料Z轴上升速度
        /// </summary>
        public int DiscargeZUpSpeed
        {
            get
            {
                return _DiscargeZUpSpeed;
            }

            set
            {
                _DiscargeZUpSpeed = value;
            }
        }

        private bool _IsDischargeZMonitor;
        /// <summary>
        /// 下料Z轴运动是监控原点
        /// </summary>
        public bool IsDischargeZMonitor
        {
            get
            {
                return _IsDischargeZMonitor;
            }

            set
            {
                _IsDischargeZMonitor = value;
            }
        }

        private int _DiscargeXNGUnLoadSpeed;
        /// <summary>
        /// 下料X轴一工位抛料速度
        /// </summary>
        public int DiscargeXNGUnLoadSpeed
        {
            get
            {
                return _DiscargeXNGUnLoadSpeed;
            }

            set
            {
                _DiscargeXNGUnLoadSpeed = value;
            }
        }

        private int _LoadYBackSpeed;
        /// <summary>
        /// 推料Y返回速度
        /// </summary>
        public int LoadYBackSpeed
        {
            get
            {
                return _LoadYBackSpeed;
            }

            set
            {
                _LoadYBackSpeed = value;
            }
        }




        private int _LoadFilterTime;
        /// <summary>
        /// 进料光纤滤波时间
        /// </summary>
        public int LoadFilterTime
        {
            get
            {
                return _LoadFilterTime;
            }

            set
            {
                _LoadFilterTime = value;
            }
        }



        private bool _IsUseNGLineButton;
        /// <summary>
        /// NG线软按钮
        /// </summary>
        public bool IsUseNGLineButton
        {
            get
            {
                return _IsUseNGLineButton;
            }

            set
            {
                _IsUseNGLineButton = value;
            }
        }

        private bool _IsBendYHomeEnable;
        /// <summary>
        /// 对位Y自动回原
        /// </summary>
        public bool IsBendXYHomeEnable
        {
            get
            {
                return _IsBendYHomeEnable;
            }

            set
            {
                _IsBendYHomeEnable = value;
            }
        }

        private bool _IsDischargeZHomeEnable;
        /// <summary>
        /// 下料Z自动回原
        /// </summary>
        public bool DischargeZHomeEnable
        {
            get
            {
                return _IsDischargeZHomeEnable;
            }

            set
            {
                _IsDischargeZHomeEnable = value;
            }
        }




        private int _YHomeInterval;
        /// <summary>
        /// 对位Y自动回原间隔
        /// </summary>
        public int YHomeInterval
        {
            get
            {
                return _YHomeInterval;
            }

            set
            {
                _YHomeInterval = value;
            }
        }
        private int _ZHomeInterval;
        /// <summary>
        /// 对位Y自动回原间隔
        /// </summary>
        public int ZHomeInterval
        {
            get
            {
                return _ZHomeInterval;
            }

            set
            {
                _ZHomeInterval = value;
            }
        }

        private bool _IsLoadZCylinder;
        /// <summary>
        /// 进料Z是否是气缸  不然就是伺服
        /// </summary>
        public bool IsLoadZCylinder
        {
            get
            {
                return _IsLoadZCylinder;
            }

            set
            {
                _IsLoadZCylinder = value;
            }
        }


        private bool _IsTransferZCylinder;
        /// <summary>
        /// 中转Z是否是气缸  不然就是伺服
        /// </summary>
        public bool IsTransferZCylinder
        {
            get
            {
                return _IsTransferZCylinder;
            }

            set
            {
                _IsTransferZCylinder = value;
            }
        }







        private bool _IsTearFilmCloseVacCalib;
        /// <summary>
        /// 撕膜对位是否临时关闭平台真空
        /// </summary>
        public bool IsTearFilmCloseVacCalib
        {
            get
            {
                return _IsTearFilmCloseVacCalib;
            }

            set
            {
                _IsTearFilmCloseVacCalib = value;
            }
        }


        private bool _IsPreTearFilmPress;
        /// <summary>
        /// 撕膜压杆提前下压
        /// </summary>
        public bool IsPreTearFilmPress
        {
            get
            {
                return _IsPreTearFilmPress;
            }

            set
            {
                _IsPreTearFilmPress = value;
            }
        }
        private bool _IsLeftSMUDCylinderEnable;
        /// <summary>
        /// 左撕膜上下气缸(FPC压杆)启用、关闭
        /// </summary>
        public bool IsLeftSMUDCylinderEnable
        {
            get
            {
                return _IsLeftSMUDCylinderEnable;
            }

            set
            {
                _IsLeftSMUDCylinderEnable = value;
            }
        }
        private bool _IsMidSMUDCylinderEnable;
        /// <summary>
        /// 中撕膜上下气缸(FPC压杆)启用、关闭
        /// </summary>
        public bool IsMidSMUDCylinderEnable
        {
            get
            {
                return _IsMidSMUDCylinderEnable;
            }

            set
            {
                _IsMidSMUDCylinderEnable = value;
            }
        }
        private bool _IsRightUDCylinderEnable;
        /// <summary>
        /// 右撕膜上下气缸(FPC压杆)启用、关闭
        /// </summary>
        public bool IsRightUDCylinderEnable
        {
            get
            {
                return _IsRightUDCylinderEnable;
            }

            set
            {
                _IsRightUDCylinderEnable = value;
            }
        }

        private bool _IsControlUpStreamEnable;
        /// <summary>
        /// 控制上游流水线
        /// </summary>
        public bool IsControlUpStreamEnable
        {
            get
            {
                return _IsControlUpStreamEnable;
            }

            set
            {
                _IsControlUpStreamEnable = value;
            }
        }
        private bool _IsLoadYAxisEnable;
        /// <summary>
        /// 来料对位Y轴 默认为停用
        /// </summary>
        public bool IsLoadYAxisEnable
        {
            get
            {
                return _IsLoadYAxisEnable;
            }

            set
            {
                _IsLoadYAxisEnable = value;
            }
        }
 




        private bool _BefoTearCheck;
        /// <summary>
        /// 撕膜前检测
        /// </summary>
        public bool BefoTearCheck
        {
            get
            {
                return _BefoTearCheck;
            }

            set
            {
                _BefoTearCheck = value;
            }
        }




        private float _AdjustAngle;
        /// <summary>
        /// 模板匹配角度管控（校正到管控内OK）
        /// </summary>
        public float AdjustAngle
        {
            get
            {
                return _AdjustAngle;
            }

            set
            {
                _AdjustAngle = value;
            }
        }


        /// <summary>
        /// 弯折记录产品数 到达指定间隔称重清零一次
        /// </summary>
        private int _Mid_LoadCellPdtCout;
        public int Mid_LoadCellPdtCout
        {
            get
            {
                return _Mid_LoadCellPdtCout;
            }

            set
            {
                _Mid_LoadCellPdtCout = value;
            }
        }

        /// <summary>
        /// 弯折记录产品数 到达指定间隔称重清零一次
        /// </summary>
        private int _Right_LoadCellPdtCout;
        public int Right_LoadCellPdtCout
        {
            get
            {
                return _Right_LoadCellPdtCout;
            }

            set
            {
                _Right_LoadCellPdtCout = value;
            }
        }


        #endregion 


        private int _StageBlowDelay;
        public int StageBlowDelay
        {
            get
            {
                return _StageBlowDelay;
            }
            set
            {
                _StageBlowDelay = value;
            }
        }

        private int _RobotBlowDelay;
        public int RobotBlowDelay
        {
            get
            {
                return _RobotBlowDelay;
            }
            set
            {
                _RobotBlowDelay = value;
            }
        }


        private double _LeftBendPressure;
        public double LeftBendPressure
        {
            get
            {
                return _LeftBendPressure;
            }
            set
            {
                _LeftBendPressure = value;
            }
        }


        private double _MidBendPressure;
        public double MidBendPressure
        {
            get
            {
                return _MidBendPressure;
            }
            set
            {
                _MidBendPressure = value;
            }
        }


        private double _RightBendPressure;
        public double RightBendPressure
        {
            get
            {
                return _RightBendPressure;
            }
            set
            {
                _RightBendPressure = value;
            }
        }



        private bool _IsGateAlarm_Enable;

        public bool IsGateAlarm_Enable
        {
            get
            {
                return _IsGateAlarm_Enable;
            }
            set
            {
                _IsGateAlarm_Enable = value;
            }
        }

        private int _tear1_ng;
        public int Tear1_NG
        {
            get
            {
                return _tear1_ng;
            }
            set
            {
                _tear1_ng = value;
            }
        }

        private int _tear2_ng;
        public int Tear2_NG
        {
            get
            {
                return _tear2_ng;
            }
            set
            {
                _tear2_ng = value;
            }
        }


        private int _tear3_ng;
        public int Tear3_NG
        {
            get
            {
                return _tear3_ng;
            }
            set
            {
                _tear3_ng = value;
            }
        }

        private int _tear1_ok;
        public int Tear1_OK
        {
            get
            {
                return _tear1_ok;
            }
            set
            {
                _tear1_ok = value;
            }
        }

        private int _tear2_ok;
        public int Tear2_OK
        {
            get
            {
                return _tear2_ok;
            }
            set
            {
                _tear2_ok = value;
            }
        }


        private int _tear3_ok;
        public int Tear3_OK
        {
            get
            {
                return _tear3_ok;
            }
            set
            {
                _tear3_ok = value;
            }
        }


        private int _bend1_ng;
        public int Bend1_NG
        {
            get
            {
                return _bend1_ng;
            }
            set
            {
                _bend1_ng = value;
            }
        }

        private int _bend2_ng;
        public int Bend2_NG
        {
            get
            {
                return _bend2_ng;
            }
            set
            {
                _bend2_ng = value;
            }
        }

        private int _bend3_ng;
        public int Bend3_NG
        {
            get
            {
                return _bend3_ng;
            }
            set
            {
                _bend3_ng = value;
            }
        }

        private int _bend1_ok;
        public int Bend1_OK
        {
            get
            {
                return _bend1_ok;
            }
            set
            {
                _bend1_ok = value;
            }
        }

        private int _bend2_ok;
        public int Bend2_OK
        {
            get
            {
                return _bend2_ok;
            }
            set
            {
                _bend2_ok = value;
            }
        }


        private int _bend3_ok;
        public int Bend3_OK
        {
            get
            {
                return _bend3_ok;
            }
            set
            {
                _bend3_ok = value;
            }
        }



        private int _TotalNum;

        private bool _BuzzerDisabled;

        private int _DetectNGNumMax = 0;



        private bool _IsGateDisable;

        private bool _IsCamDisable;

        private bool _IsPointLaserDisabled;

        private int _ClearInterval;

        private double _TimeGlueBeforeClear;

        private double _LaserRangeValid;

        private bool _IsVermesParmAuto;

        private int _AlcoholOpenTime;

        private string _BarCode;

        private string _CodeComNum = "COM8";


        public string CodeComNum
        {
            get
            {
                if (_CodeComNum == null)
                {
                    _CodeComNum = "COM8";
                }
                return _CodeComNum;
            }
            set
            {
                _CodeComNum = value;
            }
        }

        public string BarCode
        {
            get
            {
                return _BarCode;
            }
            set
            {
                _BarCode = value;
            }
        }

        public bool IsCamDisable
        {
            get
            {
                return _IsCamDisable;
            }
            set
            {
                _IsCamDisable = value;
            }
        }


        public bool IsPointLaserDisabled
        {
            get
            {
                return _IsPointLaserDisabled;
            }
            set
            {
                _IsPointLaserDisabled = value;
            }
        }


        private bool _tear1RllCLD_Enable;
        public bool Tear1RllCLD_Enable
        {
            get
            {
                return _tear1RllCLD_Enable;
            }
            set
            {
                _tear1RllCLD_Enable = value;
            }
        }


        private bool _tear2RllCLD_Enable;
        public bool Tear2RllCLD_Enable
        {
            get
            {
                return _tear2RllCLD_Enable;
            }
            set
            {
                _tear2RllCLD_Enable = value;
            }
        }



        private bool _tear3RllCLD_Enable;
        public bool Tear3RllCLD_Enable
        {
            get
            {
                return _tear3RllCLD_Enable;
            }
            set
            {
                _tear3RllCLD_Enable = value;
            }
        }



        private bool _Mp1_Enable;
        public bool MP1_Enable
        {
            get
            {
                return _Mp1_Enable;
            }
            set
            {
                _Mp1_Enable = value;
            }
        }

        private bool _Mp2_Enable;
        public bool MP2_Enable
        {
            get
            {
                return _Mp2_Enable;
            }
            set
            {
                _Mp2_Enable = value;
            }
        }


        private bool _Mp3_Enable;
        public bool MP3_Enable
        {
            get
            {
                return _Mp3_Enable;
            }
            set
            {
                _Mp3_Enable = value;
            }
        }


        private bool _Glue1_Enable;
        public bool Glue1_Enable
        {
            get
            {
                return _Glue1_Enable;
            }
            set
            {
                _Glue1_Enable = value;
            }
        }


        private bool _Glue2_Enable;
        public bool Glue2_Enable
        {
            get
            {
                return _Glue2_Enable;
            }
            set
            {
                _Glue2_Enable = value;
            }
        }

        private bool _Glue3_Enable;
        public bool Glue3_Enable
        {
            get
            {
                return _Glue3_Enable;
            }
            set
            {
                _Glue3_Enable = value;
            }
        }



        private bool _FPC1_Optical_Enable;
        public bool FPC1_Optical_Enable
        {
            get
            {
                return _FPC1_Optical_Enable;
            }
            set
            {
                _FPC1_Optical_Enable = value;
            }
        }

        private bool _FPC2_Optical_Enable;
        public bool FPC2_Optical_Enable
        {
            get
            {
                return _FPC2_Optical_Enable;
            }
            set
            {
                _FPC2_Optical_Enable = value;
            }
        }

        private bool _FPC3_Optical_Enable;
        public bool FPC3_Optical_Enable
        {
            get
            {
                return _FPC3_Optical_Enable;
            }
            set
            {
                _FPC3_Optical_Enable = value;
            }
        }


        private bool _IsBend1Free;
        public bool IsBend1Free
        {
            get
            {
                return _IsBend1Free;
            }
            set
            {
                _IsBend1Free = value;
            }
        }


        private bool _IsBend2Free;
        public bool IsBend2Free
        {
            get
            {
                return _IsBend2Free;
            }
            set
            {
                _IsBend2Free = value;
            }
        }

        private bool _IsBend3Free;

        public bool IsBend3Free
        {
            get
            {
                return _IsBend3Free;
            }
            set
            {
                _IsBend3Free = value;
            }
        }

        private bool _IsLoadCell1Enable;

        public bool IsLoadCell1Enable
        {
            get
            {
                return _IsLoadCell1Enable;
            }
            set
            {
                _IsLoadCell1Enable = value;
            }
        }

        private bool _IsLoadCell2Enable;

        public bool IsLoadCell2Enable
        {
            get
            {
                return _IsLoadCell2Enable;
            }
            set
            {
                _IsLoadCell2Enable = value;
            }
        }

        private bool _IsLoadCell3Enable;
        public bool IsLoadCell3Enable
        {
            get
            {
                return _IsLoadCell3Enable;
            }
            set
            {
                _IsLoadCell3Enable = value;
            }
        }

        private bool _IsLoadCellRstEnable;
        public bool IsLoadCellRstEnable
        {
            get
            {
                return _IsLoadCellRstEnable;
            }
            set
            {
                _IsLoadCellRstEnable = value;
            }

        }

        private bool _IsManualLoad;
        public bool IsManualLoad
        {
            get
            {
                return _IsManualLoad;
            }
            set
            {
                _IsManualLoad = value;
            }

        }

     

        private bool _IsScanCodeEnable;
        public bool IsScanCodeEnable
        {
            get
            {
                return _IsScanCodeEnable;
            }
            set
            {
                _IsScanCodeEnable = value;
            }
        }

        private bool _FPC_Tear_Enable;
        public bool FPC_Tear_Enable
        {
            get
            {
                return _FPC_Tear_Enable;
            }
            set
            {
                _FPC_Tear_Enable = value;
            }
        }

        private bool _TearAOI_Blow_Enable;
        public bool TearAOI_Blow_Enable
        {
            get
            {
                return _TearAOI_Blow_Enable;
            }
            set
            {
                _TearAOI_Blow_Enable = value;
            }
        }

        #region tfx
        private bool _TearRecheckEnabled;
        /// <summary>
        /// 撕膜失败时 是否允许重复撕膜  
        /// </summary>
        public bool TearRecheckEnabled
        {
            get
            {
                return _TearRecheckEnabled;
            }
            set
            {
                _TearRecheckEnabled = value;
            }
        }
        #endregion

        #region 新增上料气缸和下料气缸结构 屏蔽功能 兼容其他折弯机型
        private bool _IsFeedCylinderEnable;
        public bool IsFeedCylinderEnable
        {
            get
            {
                return _IsFeedCylinderEnable;
            }
            set
            {
                _IsFeedCylinderEnable = value;
            }
        }

        private bool _IsDischargeCylinderEnable;
        public bool IsDischargeCylinderEnable
        {
            get
            {
                return _IsDischargeCylinderEnable;
            }
            set
            {
                _IsDischargeCylinderEnable = value;
            }
        }

        #endregion

        #region  对位保护
        private bool _Isbend1_CalibProtect;

        public bool Isbend1_CalibProtect
        {
            get
            {
                return _Isbend1_CalibProtect;
            }
            set
            {
                _Isbend1_CalibProtect = value;
            }

        }


        private bool _Isbend2_CalibProtect;

        public bool Isbend2_CalibProtect
        {
            get
            {
                return _Isbend2_CalibProtect;
            }
            set
            {
                _Isbend2_CalibProtect = value;
            }

        }

        private bool _Isbend3_CalibProtect;

        public bool Isbend3_CalibProtect
        {
            get
            {
                return _Isbend3_CalibProtect;
            }
            set
            {
                _Isbend3_CalibProtect = value;
            }

        }





        #endregion 

        private bool _DsgLine_FullSensor_Enable;
        public bool DsgLine_FullSensor_Enable
        {
            get
            {
                return _DsgLine_FullSensor_Enable;
            }
            set
            {
                _DsgLine_FullSensor_Enable = value;
            }
        }


        #region 固定参数

        private double _PointLaserSamplingInterval = 10;

        private double _LineLaserSamplingInterval = 1;

        private double _LineLaserSpeed = 10;

        private double _PointLaserSpeed = 10;

        private double _DispenseSpeed = 10;

        private List<double> _UWorkPos;

        private double _ZClearPos;

        private double _XClearPos;

        private double _ClearDist;

        private double _ClearNum;

        private double _ClearSpeed;

        private double _XWaitPosition;

        private double _Z1WaitPosition;

        private double _Z2WaitPosition;

        private double _RightLightWaitPos;

        private double _LeftLightWaitPos;

        private double _UvMoveDist;

        public List<double> UWorkPos
        {
            get
            {
                if (_UWorkPos == null)
                {
                    _UWorkPos = new List<double>();
                }
                if (_UWorkPos.Count < 4)
                {
                    for (int i = _UWorkPos.Count; i < 4; i++)
                    {
                        _UWorkPos.Add(0.0);
                    }
                }
                return _UWorkPos;
            }
        }

        #endregion

        #region 拍背光前延时
        private int _LeftBLPhotoDelay;
        public int LeftBLPhotoDelay
        {
            get
            {
                return _LeftBLPhotoDelay;
            }
            set
            {
                _LeftBLPhotoDelay = value;
            }
        }


        private int _MidBLPhotoDelay;
        public int MidBLPhotoDelay
        {
            get
            {
                return _MidBLPhotoDelay;
            }
            set
            {
                _MidBLPhotoDelay = value;
            }
        }


        private int _RightBLPhotoDelay;
        public int RightBLPhotoDelay
        {
            get
            {
                return _RightBLPhotoDelay;
            }
            set
            {
                _RightBLPhotoDelay = value;
            }
        }
        #endregion
        #region R轴翻转延时
        private int _LeftRotateMoveDelay;
        public int LeftRotateMoveDelay
        {
            get
            {
                return _LeftRotateMoveDelay;
            }
            set
            {
                _LeftRotateMoveDelay = value;
            }
        }

        private int _MidRotateMoveDelay;
        public int MidRotateMoveDelay
        {
            get
            {
                return _MidRotateMoveDelay;
            }
            set
            {
                _MidRotateMoveDelay = value;
            }
        }

        private int _RightRotateMoveDelay;
        public int RightRotateMoveDelay
        {
            get
            {
                return _RightRotateMoveDelay;
            }
            set
            {
                _RightRotateMoveDelay = value;
            }
        }





        #endregion

        #region 回原偏移
        private double _LoadXGoHomeOffset;
        public double LoadXGoHomeOffset
        {
            get
            {
                return _LoadXGoHomeOffset;
            }
            set
            {
                _LoadXGoHomeOffset = value;
            }
        }


        private double _DischargeZGoHomeOffset;
        public double DischargeZGoHomeOffset
        {
            get
            {
                return _DischargeZGoHomeOffset;
            }
            set
            {
                _DischargeZGoHomeOffset = value;
            }
        }



        #endregion 



        #region 输入输出类定义

        [Serializable]
        public class ConfigIO
        {
            private CardIDs _CardID = CardIDs.A;

            private int _IO = -1;

            private bool _Status = false;

            private string _Name = "";

            private bool _IsIOEx;

            public CardIDs CardID
            {
                get
                {
                    return _CardID;
                }
                set
                {
                    _CardID = value;
                }
            }

            public int IO
            {
                get
                {
                    return _IO;
                }
                set
                {
                    _IO = value;
                }
            }

            public bool IsValid
            {
                get
                {
                    return ((_CardID < CardIDs.A ? true : _IO < 0) ? false : true);
                }
            }

            public string Name
            {
                get
                {
                    return _Name;
                }
                set
                {
                    _Name = value;
                }
            }

            public bool IsIOEx
            {
                get
                {
                    return _IsIOEx;
                }
                set
                {
                    _IsIOEx = value;
                }
            }

            public bool Status
            {
                get
                {
                    return _Status;
                }
                set
                {
                    _Status = value;
                }
            }

            public ConfigIO()
            {
            }

            public ConfigIO(CardIDs cardid, int io, bool status, string name)
            {
                _CardID = cardid;
                _IO = io;
                _Status = status;
                _Name = name;
            }
        }

        [Serializable]
        public class ConfigIOIn : ConfigIO
        {
            public ConfigIOIn()
            {
            }

            public ConfigIOIn(CardIDs cardid, int io, bool status, string name) : base(cardid, io, status, name)
            {
            }
        }

        [Serializable]
        public class ConfigIOInEx : ConfigIOIn
        {
            private int _Node;

            public int Node
            {
                get
                {
                    return _Node;
                }
                set
                {
                    _Node = value;
                }
            }

            public ConfigIOInEx()
            {
            }

            public ConfigIOInEx(CardIDs cardid, int io, bool status, string name, int node) : base(cardid, io, status, name)
            {
                IsIOEx = true;
                _Node = node;
            }
        }

        [Serializable]
        public class ConfigIOOut : ConfigIO
        {
            private double _Delay = 0;

            public double Delay
            {
                get
                {
                    return _Delay;
                }
                set
                {
                    _Delay = value;
                }
            }

            public ConfigIOOut()
            {
            }

            public ConfigIOOut(CardIDs cardid, int io, bool status, string name, int delay) : base(cardid, io, status, name)
            {
                _Delay = delay;
            }
        }

        [Serializable]
        public class ConfigIOOutEx : ConfigIOOut
        {
            private int _Node;

            public int Node
            {
                get
                {
                    return _Node;
                }
                set
                {
                    _Node = value;
                }
            }

            public ConfigIOOutEx()
            {
            }

            public ConfigIOOutEx(CardIDs cardid, int io, bool status, string name, int delay, int node) : base(cardid, io, status, name, delay)
            {
                IsIOEx = true;
                _Node = node;
            }
        }

        #endregion




        #region //IOIN

        private ConfigIOInEx _UpstreamBeltOpticalIOINEx;
        /// <summary>
        /// 上游流水线光纤感应
        /// </summary>
        public ConfigIOInEx UpstreamBeltOpticalIOINEx
        {
            get
            {
                if (_UpstreamBeltOpticalIOINEx == null)
                {
                    _UpstreamBeltOpticalIOINEx = new ConfigIOInEx(CardIDs.None, -1, false, "上游皮带有料光纤信号", 1);
                }
                return _UpstreamBeltOpticalIOINEx;
            }
        }
        /// <summary>
        /// 本机进流水线前段
        /// </summary>
        private ConfigIOInEx _BeltStartOpticalIOINEx;
        public ConfigIOInEx BeltStartOpticalIOINEx
        {
            get
            {
                if (_BeltStartOpticalIOINEx == null)
                {
                    _BeltStartOpticalIOINEx = new ConfigIOInEx(CardIDs.None, -1, false, "本机皮带来料光纤信号", 1);
                }
                return _BeltStartOpticalIOINEx;
            }
        }






        private ConfigIOIn _BeltOpticalIOIn;
        public ConfigIOIn BeltOpticalIOIN
        {
            get
            {
                if (_BeltOpticalIOIn == null)
                {
                    _BeltOpticalIOIn = new ConfigIOIn(CardIDs.None, -1, false, "来料皮带光纤信号");
                }
                return _BeltOpticalIOIn;
            }
        }

        private ConfigIOIn _LoadVacuumIOIn;
        public ConfigIOIn LoadVacuumIOIn
        {
            get
            {
                if (_LoadVacuumIOIn == null)
                {
                    _LoadVacuumIOIn = new ConfigIOIn(CardIDs.None, -1, false, "上料真空吸检测信号");
                }
                return _LoadVacuumIOIn;
            }
        }

        private ConfigIOIn _LoadFPCVacuumIOIn;
        public ConfigIOIn LoadfFPCVacuumIOIn
        {
            get
            {
                if (_LoadFPCVacuumIOIn == null)
                {
                    _LoadFPCVacuumIOIn = new ConfigIOIn(CardIDs.None, -1, false, "上料FPC真空吸检测信号");
                }
                return _LoadFPCVacuumIOIn;
            }
        }

        private ConfigIOIn _EmgStopCard0IOIn;
        public ConfigIOIn EmgStopCard0IoIn
        {
            get
            {
                if (_EmgStopCard0IOIn == null)
                {
                    _EmgStopCard0IOIn = new ConfigIOIn(CardIDs.None, -1, false, "急停信号");
                }
                return _EmgStopCard0IOIn;
            }
        }

        private ConfigIOIn _EmgStopCard1IOIn;
        public ConfigIOIn EmgStopCard1IOIn
        {
            get
            {
                if (_EmgStopCard1IOIn == null)
                {
                    _EmgStopCard1IOIn = new ConfigIOIn(CardIDs.None, -1, false, "急停");
                }
                return _EmgStopCard1IOIn;
            }
        }

        private ConfigIOIn _EmgStopCard2IOIn;
        public ConfigIOIn EmgStopCard2IOIn
        {
            get
            {
                if (_EmgStopCard2IOIn == null)
                {
                    _EmgStopCard2IOIn = new ConfigIOIn(CardIDs.None, -1, false, "急停");
                }
                return _EmgStopCard2IOIn;
            }
        }

        private ConfigIOIn _EmgStopCard3IOIn;
        public ConfigIOIn EmgStopCard3IOIn
        {
            get
            {
                if (_EmgStopCard3IOIn == null)
                {
                    _EmgStopCard3IOIn = new ConfigIOIn(CardIDs.None, -1, false, "急停");
                }
                return _EmgStopCard3IOIn;
            }
        }

        private ConfigIOInEx _StartBtnIOIn;
        public ConfigIOInEx StartBtnIOInEx
        {
            get
            {
                if (_StartBtnIOIn == null)
                {
                    _StartBtnIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "启动按钮", 1);
                }
                return _StartBtnIOIn;
            }
        }

        private ConfigIOInEx _ResetBtnIOIn;
        public ConfigIOInEx ResetbtnIOInEx
        {
            get
            {
                if (_ResetBtnIOIn == null)
                {
                    _ResetBtnIOIn = new ConfigIOInEx(CardIDs.B, 1, false, "复位按钮", 1);
                }
                return _ResetBtnIOIn;
            }
        }








        private ConfigIOIn _TransferVacuumIOIn;
        public ConfigIOIn TransferVacuumIOIn
        {
            get
            {
                if (_TransferVacuumIOIn == null)
                {
                    _TransferVacuumIOIn = new ConfigIOIn(CardIDs.None, -1, false, "中转工位真空吸检测");
                }
                return _TransferVacuumIOIn;
            }
        }


        private ConfigIOIn _TransferFPCVacuum_IOIn;
        public ConfigIOIn TransferFPCVacuumIOIn
        {
            get
            {
                if (_TransferFPCVacuum_IOIn == null)
                {
                    _TransferFPCVacuum_IOIn = new ConfigIOIn(CardIDs.None, -1, false, "中转工位FPC真空吸检测");
                }
                return _TransferFPCVacuum_IOIn;
            }
        }

        private ConfigIOIn _DischargeVacuumIOIn;
        public ConfigIOIn DischargeVacuumIOIn
        {
            get
            {
                if (_DischargeVacuumIOIn == null)
                {
                    _DischargeVacuumIOIn = new ConfigIOIn(CardIDs.None, -1, false, "出料工位真空检测");
                }
                return _DischargeVacuumIOIn;
            }
        }


        private ConfigIOIn _DischargeLine_OpticalSensorHave_IOIn;
        public ConfigIOIn DischargeLine_OpticalSensorHave_IOIn
        {
            get
            {
                if (_DischargeLine_OpticalSensorHave_IOIn == null)
                {
                    _DischargeLine_OpticalSensorHave_IOIn = new ConfigIOIn(CardIDs.None, -1, false, "出料流水线有无光纤");

                }
                return _DischargeLine_OpticalSensorHave_IOIn;
            }
        }


        private ConfigIOIn _DischargeLine_OpticalSensorFull_IOIn;
        public ConfigIOIn DischargeLine_OpticalSensorFull_IOIn
        {
            get
            {
                if (_DischargeLine_OpticalSensorFull_IOIn == null)
                {
                    _DischargeLine_OpticalSensorFull_IOIn = new ConfigIOIn(CardIDs.None, -1, false, "出料流水线有无光纤");

                }
                return _DischargeLine_OpticalSensorFull_IOIn;
            }
        }


        private ConfigIOIn _Discharge_FpcVacuumIOIn;
        public ConfigIOIn Discharge_FPCVacuumIOIn
        {
            get
            {
                if (_Discharge_FpcVacuumIOIn == null)
                {
                    _Discharge_FpcVacuumIOIn = new ConfigIOIn(CardIDs.None, -1, false, "出料工位FPC真空吸检测信号");
                }
                return _Discharge_FpcVacuumIOIn;
            }

        }

        private ConfigIOInEx _PauseBtnIOInEx;
        public ConfigIOInEx PauseBtnIOInEx
        {
            get
            {
                if (_PauseBtnIOInEx == null)
                {
                    _PauseBtnIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "暂停按钮", 1);
                }
                return _PauseBtnIOInEx;
            }
        }

        private ConfigIOInEx _SMNGstgHave2IOInEx;
        public ConfigIOInEx SMNGstgHave2IOInEx
        {
            get
            {
                if (_SMNGstgHave2IOInEx == null)
                {
                    _SMNGstgHave2IOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜NG平台检测1", 1);
                }
                return _SMNGstgHave2IOInEx;
            }
        }

        private ConfigIOInEx _SMNGstgHave3IOInEx;
        public ConfigIOInEx SMNGstgHave3IOInEx
        {
            get
            {
                if (_SMNGstgHave3IOInEx == null)
                {
                    _SMNGstgHave3IOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜NG平台检测1", 1);
                }
                return _SMNGstgHave3IOInEx;
            }
        }

        private ConfigIOInEx _NGLineReachIOInEx;
        public ConfigIOInEx NGLineReachIOInEx
        {
            get
            {
                if (_NGLineReachIOInEx == null)
                {
                    _NGLineReachIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "NG流水线到位感应器", 1);
                }
                return _NGLineReachIOInEx;
            }
        }

        private ConfigIOInEx _NGLineHaveIOInEx;
        public ConfigIOInEx NGLineHaveIOInEx
        {
            get
            {
                if (_NGLineHaveIOInEx == null)
                {
                    _NGLineHaveIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "NG流水线有无信号", 1);
                }
                return _NGLineHaveIOInEx;
            }
        }

        private ConfigIOInEx _NGLineFullIOInEx;
        public ConfigIOInEx NGLineFullIOInEx
        {
            get
            {
                if (_NGLineFullIOInEx == null)
                {
                    _NGLineFullIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "NG流水线满料信号", 1);
                }
                return _NGLineFullIOInEx;
            }
        }

        private ConfigIOInEx _DischargeAxisZservorAlarmIOInEx;
        public ConfigIOInEx DischargeAxisZservorAlarmIOInEx
        {
            get
            {
                if (_DischargeAxisZservorAlarmIOInEx == null)
                {
                    _DischargeAxisZservorAlarmIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "出料工位Z轴伺服报警", 1);

                }
                return _DischargeAxisZservorAlarmIOInEx;
            }
        }

        #region 进出料翻转输入
        private ConfigIOIn _Feed_RotateUpDownCylinder_UPIOIn;
        public ConfigIOIn Feed_RotateUpDownCylinder_UpIOIn
        {
            get
            {
                if (_Feed_RotateUpDownCylinder_UPIOIn == null)
                {
                    _Feed_RotateUpDownCylinder_UPIOIn = new ConfigIOIn(CardIDs.None, -1, false, "进料翻转工位上下气缸上感应");
                }
                return _Feed_RotateUpDownCylinder_UPIOIn;
            }
        }

        private ConfigIOIn _Feed_RotateUpDownCylider_DownIOIn;
        public ConfigIOIn Feed_RotateUpDownCylider_DownIOIn
        {
            get
            {
                if (_Feed_RotateUpDownCylider_DownIOIn == null)
                {
                    _Feed_RotateUpDownCylider_DownIOIn = new ConfigIOIn(CardIDs.None, -1, false, "进料翻转工位上下气缸下感应");
                }
                return _Feed_RotateUpDownCylider_DownIOIn;
            }
        }

        private ConfigIOIn _Feed_RotateCylinder_UpIOIn;
        public ConfigIOIn Feed_RotateCylinder_UpIOIn
        {
            get
            {
                if (_Feed_RotateCylinder_UpIOIn == null)
                {
                    _Feed_RotateCylinder_UpIOIn = new ConfigIOIn(CardIDs.None, -1, false, "进料翻转工位翻转气缸上感应");
                }
                return _Feed_RotateCylinder_UpIOIn;
            }
        }

        private ConfigIOInEx _Feed_UpDownCylinder_UpIOInEx;
        public ConfigIOInEx Feed_UpDownCylinder_UpIOInEx
        {
            get
            {
                if (_Feed_UpDownCylinder_UpIOInEx == null)
                {
                    _Feed_UpDownCylinder_UpIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "进料上下气缸上感应", 1);
                }
                return _Feed_UpDownCylinder_UpIOInEx;
            }
        }

        private ConfigIOInEx _Feed_UpDownCylinder_DownIOInEx;
        public ConfigIOInEx Feed_UpDownCylinder_DownIOInEx
        {
            get
            {
                if (_Feed_UpDownCylinder_DownIOInEx == null)
                {
                    _Feed_UpDownCylinder_DownIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "进料上下气缸下感应", 1);
                }
                return _Feed_UpDownCylinder_DownIOInEx;
            }
        }



        private ConfigIOIn _Feed_RotateCylinder_DownIOIn;
        public ConfigIOIn Feed_RotateCylinder_DownIOIn
        {
            get
            {
                if (_Feed_RotateCylinder_DownIOIn == null)
                {
                    _Feed_RotateCylinder_DownIOIn = new ConfigIOIn(CardIDs.None, -1, false, "进料翻转工位翻转气缸下感应");
                }
                return _Feed_RotateCylinder_DownIOIn;
            }
        }

        private ConfigIOIn _Feed_RotateVacuumCheckIOIn;
        public ConfigIOIn Feed_RotateVacuumCheckIOIn
        {
            get
            {
                if (_Feed_RotateVacuumCheckIOIn == null)
                {
                    _Feed_RotateVacuumCheckIOIn = new ConfigIOIn(CardIDs.None, -1, false, "进料翻转真空检测");
                }
                return _Feed_RotateVacuumCheckIOIn;
            }
        }
        private ConfigIOIn _Feed_RotateFPCVacuumCheckIOIn;
        public ConfigIOIn Feed_RotateFPCVacuumCheckIOIn
        {
            get
            {
                if (_Feed_RotateFPCVacuumCheckIOIn == null)
                {
                    _Feed_RotateFPCVacuumCheckIOIn = new ConfigIOIn(CardIDs.None, -1, false, "进料翻转FPC真空检测");
                }
                return _Feed_RotateFPCVacuumCheckIOIn;
            }
        }



        private ConfigIOInEx _Discharge_UPCylinder_UPIOInEx;
        public ConfigIOInEx Discharge_UPCylinder_UPIOInEx
        {
            get
            {
                if (_Discharge_UPCylinder_UPIOInEx == null)
                {
                    _Discharge_UPCylinder_UPIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "出料翻转工位上下气缸上感应", 1);
                }
                return _Discharge_UPCylinder_UPIOInEx;
            }
        }


        private ConfigIOInEx _Discharge_UPCylinder_DownIOInEx;
        public ConfigIOInEx Discharge_UPCylinder_DownIOInEx
        {
            get
            {
                if (_Discharge_UPCylinder_DownIOInEx == null)
                {
                    _Discharge_UPCylinder_DownIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "出料翻转工位上下气缸下感应", 1);
                }
                return _Discharge_UPCylinder_DownIOInEx;
            }
        }



        private ConfigIOInEx _Discharge_RotateCylinder_UPIOInEx;
        public ConfigIOInEx Discharge_RotateCylinder_UPIOInEx
        {
            get
            {
                if (_Discharge_RotateCylinder_UPIOInEx == null)
                {
                    _Discharge_RotateCylinder_UPIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "出料翻转工位翻转气缸上感应", 1);
                }
                return _Discharge_RotateCylinder_UPIOInEx;
            }
        }

        private ConfigIOInEx _InputAir_IOInEx;
        public ConfigIOInEx InputAir_IOInEx
        {
            get
            {
                if (_InputAir_IOInEx == null)
                {
                    _InputAir_IOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "正压气源检测", 1);
                }
                return _InputAir_IOInEx;
            }
        }

        private ConfigIOInEx _InputVacumn_IOInEx;
        public ConfigIOInEx InputVacumn_IOInEx
        {
            get
            {
                if (_InputVacumn_IOInEx == null)
                {
                    _InputVacumn_IOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "负压气源检测", 1);
                }
                return _InputVacumn_IOInEx;
            }
        }
        private ConfigIOInEx _CardInEx100;
        /// <summary>
        /// 备用InEx
        /// </summary>
        public ConfigIOInEx CardInEx100
        {
            get
            {
                if (_CardInEx100 == null)
                {
                    _CardInEx100 = new ConfigIOInEx(CardIDs.None, -1, false, "备用", 1);
                }
                return _CardInEx100;
            }
        }
        private ConfigIOIn _IOIn100;
        /// <summary>
        /// 备用In
        /// </summary>
        public ConfigIOIn IOIn100
        {
            get
            {
                if (_IOIn100 == null)
                {
                    _IOIn100 = new ConfigIOIn(CardIDs.None, -1, false, "备用");
                }
                return _IOIn100;
            }
        }
        #endregion
        #region 与上游设备交互信号
        private ConfigIOInEx _ReceiveUpstreamSafeIOInEx;
        public ConfigIOInEx ReceiveUpstream_Safe_IOInEx
        {
            get
            {
                if (_ReceiveUpstreamSafeIOInEx == null)
                {
                    _ReceiveUpstreamSafeIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "上游给本机-安全信号", 1);
                }
                return _ReceiveUpstreamSafeIOInEx;
            }
        }
        private ConfigIOInEx _ReceiveUpstreamRequestIOInEx;
        public ConfigIOInEx ReceiveUpstream_Request_IOInEx
        {
            get
            {
                if (_ReceiveUpstreamRequestIOInEx == null)
                {
                    _ReceiveUpstreamRequestIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "上游给本机-请求信号", 1);
                }
                return _ReceiveUpstreamRequestIOInEx;
            }
        }
        private ConfigIOInEx _ReceiveUpstreamFinishIOInEx;
        public ConfigIOInEx ReceiveUpstream_Finish_IOInEx
        {
            get
            {
                if (_ReceiveUpstreamFinishIOInEx == null)
                {
                    _ReceiveUpstreamFinishIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "上游给本机-完成信号", 1);
                }
                return _ReceiveUpstreamFinishIOInEx;
            }
        }
        private ConfigIOInEx _ReceiveUpstreamSpareIOInEx;
        public ConfigIOInEx ReceiveUpstream_Spare_IOInEx
        {
            get
            {
                if (_ReceiveUpstreamSpareIOInEx == null)
                {
                    _ReceiveUpstreamSpareIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "上游给本机-备用信号", 1);
                }
                return _ReceiveUpstreamSpareIOInEx;
            }
        }

        private ConfigIOOutEx _SendUpstreamSafeIOOutEx;
        public ConfigIOOutEx SendUpstream_Safe_IOOutEx
        {
            get
            {
                if (_SendUpstreamSafeIOOutEx == null)
                {
                    _SendUpstreamSafeIOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "本机给上游-安全信号", 0, 1);
                }
                return _SendUpstreamSafeIOOutEx;
            }
        }
        private ConfigIOOutEx _SendUpstreamRequestIOOutEx;
        public ConfigIOOutEx SendUpstream_Request_IOOutEx
        {
            get
            {
                if (_SendUpstreamRequestIOOutEx == null)
                {
                    _SendUpstreamRequestIOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "本机给上游-请求信号", 0, 1);
                }
                return _SendUpstreamRequestIOOutEx;
            }
        }
        private ConfigIOOutEx _SendUpstreamFinishIOOutEx;
        public ConfigIOOutEx SendUpstream_Finish_IOOutEx
        {
            get
            {
                if (_SendUpstreamFinishIOOutEx == null)
                {
                    _SendUpstreamFinishIOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "本机给上游-完成信号", 0, 1);
                }
                return _SendUpstreamFinishIOOutEx;
            }
        }
        private ConfigIOOutEx _SendUpstreamSpareIOOutEx;
        public ConfigIOOutEx SendUpstream_Spare_IOOutEx
        {
            get
            {
                if (_SendUpstreamSpareIOOutEx == null)
                {
                    _SendUpstreamSpareIOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "本机给上游-备用信号", 0, 1);
                }
                return _SendUpstreamSpareIOOutEx;
            }
        }
        #endregion
        #region 中转输入
        private ConfigIOInEx _Transfer_UDCylinderUP_IOInEx;
        public ConfigIOInEx Transfer_UDCylinderUP_IOInEx
        {
            get
            {
                if (_Transfer_UDCylinderUP_IOInEx == null)
                {
                    _Transfer_UDCylinderUP_IOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "中转搬运上下气缸上感应", 1);
                }
                return _Transfer_UDCylinderUP_IOInEx;
            }
        }

        private ConfigIOInEx _Transfer_UDCylinderDown_IOInEx;
        public ConfigIOInEx Transfer_UDCylinderDown_IOInEx
        {
            get
            {
                if (_Transfer_UDCylinderDown_IOInEx == null)
                {
                    _Transfer_UDCylinderDown_IOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "中转搬运上下气缸下感应", 1);
                }
                return _Transfer_UDCylinderDown_IOInEx;
            }
        }


        #endregion


        #region //离子风棒报警





        private ConfigIOInEx _SMION_AlarmIOInEx;
        public ConfigIOInEx SMION_AlarmIOInEx
        {
            get
            {
                if (_SMION_AlarmIOInEx == null)
                {
                    _SMION_AlarmIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜离子风棒报警", 0);
                }
                return _SMION_AlarmIOInEx;
            }
        }








        private ConfigIOInEx _BendION_AlarmIOInEx;
        public ConfigIOInEx BendION_AlarmIOInEx
        {
            get
            {
                if (_BendION_AlarmIOInEx == null)
                {
                    _BendION_AlarmIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "折弯离子风棒报警", 0);
                }
                return _BendION_AlarmIOInEx;
            }
        }
        #endregion

        #region  //安全门报警输入
        private ConfigIOInEx _SMSideGate1IOIn;
        public ConfigIOInEx SMSideGate1IOInEx
        {
            get
            {
                if (_SMSideGate1IOIn == null)
                {
                    _SMSideGate1IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜侧门禁1", 1);
                }
                return _SMSideGate1IOIn;
            }
        }

        private ConfigIOInEx _SMSideGate2IOIn;
        public ConfigIOInEx SMSideGate2IOInEx
        {
            get
            {
                if (_SMSideGate2IOIn == null)
                {
                    _SMSideGate2IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜侧门禁2", 1);
                }
                return _SMSideGate2IOIn;
            }
        }

        private ConfigIOInEx _SMSideGate3IOIn;
        public ConfigIOInEx SMSideGate3IOInEx
        {
            get
            {
                if (_SMSideGate3IOIn == null)
                {
                    _SMSideGate3IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜侧门禁3", 1);
                }
                return _SMSideGate3IOIn;
            }
        }

        private ConfigIOInEx _SMSideGate4IOIn;
        public ConfigIOInEx SMSideGate4IOInEx
        {
            get
            {
                if (_SMSideGate4IOIn == null)
                {
                    _SMSideGate4IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜侧门禁4", 1);
                }
                return _SMSideGate4IOIn;
            }
        }


        private ConfigIOInEx _SMFrontGate1IOIn;
        public ConfigIOInEx SMFrontGate1IOInEx
        {
            get
            {
                if (_SMFrontGate1IOIn == null)
                {
                    _SMFrontGate1IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜前门禁1", 1);
                }
                return _SMFrontGate1IOIn;
            }
        }

        private ConfigIOInEx _SMFrontGate2IOIn;
        public ConfigIOInEx SMFrontGate2IOInEx
        {
            get
            {
                if (_SMFrontGate2IOIn == null)
                {
                    _SMFrontGate2IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜前门禁2", 1);
                }
                return _SMFrontGate2IOIn;
            }
        }






        private ConfigIOInEx _IOInEx15;
        public ConfigIOInEx IOCardCInEx15
        {
            get
            {
                if (_IOInEx15 == null)
                {
                    _IOInEx15 = new ConfigIOInEx(CardIDs.None, -1, false, "####", 1);
                }
                return _IOInEx15;
            }
        }





        private ConfigIOInEx _SMBackGate1IOIn;
        public ConfigIOInEx SMBackGate1IOInEx
        {
            get
            {
                if (_SMBackGate1IOIn == null)
                {
                    _SMBackGate1IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜后门禁1", 1);
                }
                return _SMBackGate1IOIn;
            }
        }

        private ConfigIOInEx _SMBackGate2IOIn;
        public ConfigIOInEx SMBackGate2IOInEx
        {
            get
            {
                if (_SMBackGate2IOIn == null)
                {
                    _SMBackGate2IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜后门禁2", 1);
                }
                return _SMBackGate2IOIn;
            }
        }

        private ConfigIOInEx _SMBackGate3IOIn;
        public ConfigIOInEx SMBackGate3IOInEx
        {
            get
            {
                if (_SMBackGate3IOIn == null)
                {
                    _SMBackGate3IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜后门禁3", 1);
                }
                return _SMBackGate3IOIn;
            }
        }

        private ConfigIOInEx _BendBackGate1IOIn;
        public ConfigIOInEx BendBackGate1IOInEx
        {
            get
            {
                if (_BendBackGate1IOIn == null)
                {
                    _BendBackGate1IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "弯折后门禁1", 1);
                }
                return _BendBackGate1IOIn;
            }
        }

        private ConfigIOInEx _BendBackGate2IOIn;
        public ConfigIOInEx BendBackGate2IOInEx
        {
            get
            {
                if (_BendBackGate2IOIn == null)
                {
                    _BendBackGate2IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "弯折后门禁2", 1);
                }
                return _BendBackGate2IOIn;
            }
        }

        private ConfigIOInEx _BendFrontGate1IOIn;
        public ConfigIOInEx BendFrontGate1IOInEx
        {
            get
            {
                if (_BendFrontGate1IOIn == null)
                {
                    _BendFrontGate1IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "弯折前门禁1", 1);
                }
                return _BendFrontGate1IOIn;
            }
        }

        private ConfigIOInEx _BendFrontGate2IOIn;
        public ConfigIOInEx BendFrontGate2IOInEx
        {
            get
            {
                if (_BendFrontGate2IOIn == null)
                {
                    _BendFrontGate2IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "弯折前门禁2", 1);
                }
                return _BendFrontGate2IOIn;
            }
        }

        private ConfigIOInEx _BendSideGate1IOIn;
        public ConfigIOInEx BendSideGate1IOInEx
        {
            get
            {
                if (_BendSideGate1IOIn == null)
                {
                    _BendSideGate1IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "弯折侧门禁1", 1);
                }
                return _BendSideGate1IOIn;
            }
        }

        private ConfigIOInEx _BendSideGate2IOIn;
        public ConfigIOInEx BendSideGate2IOInEx
        {
            get
            {
                if (_BendSideGate2IOIn == null)
                {
                    _BendSideGate2IOIn = new ConfigIOInEx(CardIDs.None, -1, false, "弯折侧门禁2", 1);
                }
                return _BendSideGate2IOIn;
            }
        }
        #endregion

        #region  //左撕膜工位输入

        private ConfigIOInEx _LeftSM_WAxis_ServoalarmIOIn;
        public ConfigIOInEx LeftSM_WAxis_ServoAlarmIOInEx
        {
            get
            {
                if (_LeftSM_WAxis_ServoalarmIOIn == null)
                {
                    _LeftSM_WAxis_ServoalarmIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "左撕膜工位W轴伺服报警", 1);
                }
                return _LeftSM_WAxis_ServoalarmIOIn;
            }
        }


        private ConfigIOIn _LeftSMMPVacuumIOIn;
        public ConfigIOIn LeftSMMPVacuumIOIn
        {
            get
            {
                if (_LeftSMMPVacuumIOIn == null)
                {
                    _LeftSMMPVacuumIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左撕膜膜片光纤检测");
                }
                return _LeftSMMPVacuumIOIn;
            }
        }


        private ConfigIOIn _LeftSMVacuumIOIn;
        public ConfigIOIn LeFTSMVacuumIOIn
        {
            get
            {
                if (_LeftSMVacuumIOIn == null)
                {
                    _LeftSMVacuumIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左撕膜工位真空检测");

                }
                return _LeftSMVacuumIOIn;

            }


        }


        private ConfigIOIn _LeftSMGlueOpticalIOIn;
        public ConfigIOIn LeftSMGlueOpticalIOIn
        {
            get
            {
                if (_LeftSMGlueOpticalIOIn == null)
                {
                    _LeftSMGlueOpticalIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左撕膜工位胶带有无光纤");
                }
                return _LeftSMGlueOpticalIOIn;
            }
        }

        private ConfigIOIn _LeftSM_LR_CylinderleftIOIn;
        public ConfigIOIn LeftSM_LR_CylinderLeftIOIn
        {
            get
            {
                if (_LeftSM_LR_CylinderleftIOIn == null)
                {
                    _LeftSM_LR_CylinderleftIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左撕膜工位左右气缸出感应");
                }
                return _LeftSM_LR_CylinderleftIOIn;
            }
        }



        private ConfigIOIn _LeftSM_LR_CylinderRightIOIn;
        public ConfigIOIn LeftSM_LR_CylinderRightIOIn
        {
            get
            {
                if (_LeftSM_LR_CylinderRightIOIn == null)
                {
                    _LeftSM_LR_CylinderRightIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左撕膜工位左右气缸回感应");
                }
                return _LeftSM_LR_CylinderRightIOIn;
            }

        }

        private ConfigIOIn _LeftSM_FB_CylinderFrontIOIn;
        public ConfigIOIn LeftSM_FB_CylinderFrontIOIn
        {
            get
            {
                if (_LeftSM_FB_CylinderFrontIOIn == null)
                {
                    _LeftSM_FB_CylinderFrontIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左撕膜工位前后气缸出感应");
                }
                return _LeftSM_FB_CylinderFrontIOIn;
            }

        }

        private ConfigIOIn _LeftSM_FB_CylinderBackIOIn;
        public ConfigIOIn LeftSM_FB_CylinderBackIOIn
        {
            get
            {
                if (_LeftSM_FB_CylinderBackIOIn == null)
                {
                    _LeftSM_FB_CylinderBackIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左撕膜工位前后气缸回感应");
                }
                return _LeftSM_FB_CylinderBackIOIn;
            }

        }


        private ConfigIOIn _LeftSM_UD_CylinderUPIOIn;
        public ConfigIOIn LeftSM_UD_CylinderUPIOIn
        {
            get
            {
                if (_LeftSM_UD_CylinderUPIOIn == null)
                {
                    _LeftSM_UD_CylinderUPIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左撕膜工位上下气缸上感应");
                }
                return _LeftSM_UD_CylinderUPIOIn;
            }

        }

        private ConfigIOInEx _LeftSM_UD_CylinderdownIOInEx;
        public ConfigIOInEx LeftSM_UD_CylinderDownIOInEx
        {
            get
            {
                if (_LeftSM_UD_CylinderdownIOInEx == null)
                {
                    _LeftSM_UD_CylinderdownIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "左撕膜工位上下气缸下感应", 1);
                }
                return _LeftSM_UD_CylinderdownIOInEx;
            }
        }

        private ConfigIOInEx _LeftSM_RollerUD_CylinderdownIOIn;
        public ConfigIOInEx LeftSM_RollerUD_CylinderDownIOInEx
        {
            get
            {
                if (_LeftSM_RollerUD_CylinderdownIOIn == null)
                {
                    _LeftSM_RollerUD_CylinderdownIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "左撕膜工位滚轮上下气缸下感应", 1);
                }
                return _LeftSM_RollerUD_CylinderdownIOIn;
            }
        }


        private ConfigIOInEx _LeftSM_GlueUD_CylinderUPIOInEx;
        public ConfigIOInEx LeftSM_GlueUD_CylinderUPIOInEx
        {
            get
            {
                if (_LeftSM_GlueUD_CylinderUPIOInEx == null)
                {
                    _LeftSM_GlueUD_CylinderUPIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "左撕膜工位胶带上下气缸上感应", 1);
                }
                return _LeftSM_GlueUD_CylinderUPIOInEx;
            }
        }


        private ConfigIOInEx _LeftSM_GlueUD_CylinderDownIOInEx;
        public ConfigIOInEx LeftSM_GlueUD_CylinderDownIOInEx
        {
            get
            {
                if (_LeftSM_GlueUD_CylinderDownIOInEx == null)
                {
                    _LeftSM_GlueUD_CylinderDownIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "左撕膜工位胶带上下气缸下感应", 1);
                }
                return _LeftSM_GlueUD_CylinderDownIOInEx;
            }
        }


        private ConfigIOInEx _LeftSM_RollerUD_CylinderUpIOInEx;
        public ConfigIOInEx LeftSM_RollerUD_CylinderUpIOInEx
        {
            get
            {
                if (_LeftSM_RollerUD_CylinderUpIOInEx == null)
                {
                    _LeftSM_RollerUD_CylinderUpIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "左撕膜滚轮上下气缸上感应", 1);
                }
                return _LeftSM_RollerUD_CylinderUpIOInEx;
            }
        }



        #endregion

        #region //中撕膜工位输入
        private ConfigIOInEx _MidSMMPVacuumIOIn;
        public ConfigIOInEx MidSMMPVacuumIOInEx
        {
            get
            {
                if (_MidSMMPVacuumIOIn == null)
                {
                    _MidSMMPVacuumIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜膜片光纤检测", 1);
                }
                return _MidSMMPVacuumIOIn;
            }
        }

        private ConfigIOInEx _MidSMVacuumIOIn;
        public ConfigIOInEx MidSMVacuumIOInEx
        {
            get
            {
                if (_MidSMVacuumIOIn == null)
                {
                    _MidSMVacuumIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位真空检测", 1);

                }
                return _MidSMVacuumIOIn;

            }


        }


        private ConfigIOInEx _MidSMGlueOpticalIOIn;
        public ConfigIOInEx MidSMGlueOpticalIOInEx
        {
            get
            {
                if (_MidSMGlueOpticalIOIn == null)
                {
                    _MidSMGlueOpticalIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位胶带有无光纤", 1);
                }
                return _MidSMGlueOpticalIOIn;
            }
        }

        private ConfigIOInEx _MidSM_LR_CylinderleftIOIn;
        public ConfigIOInEx MidSM_LR_CylinerLeftIOInEx
        {
            get
            {
                if (_MidSM_LR_CylinderleftIOIn == null)
                {
                    _MidSM_LR_CylinderleftIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位左右气缸出感应", 1);
                }
                return _MidSM_LR_CylinderleftIOIn;
            }

        }

        private ConfigIOInEx _MidSM_LR_CylinderRightIOIn;
        public ConfigIOInEx MidSM_LR_CylinderRightIOInEx
        {
            get
            {
                if (_MidSM_LR_CylinderRightIOIn == null)
                {
                    _MidSM_LR_CylinderRightIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位左右气缸回感应", 1);
                }
                return _MidSM_LR_CylinderRightIOIn;
            }
        }

        private ConfigIOInEx _MidSM_FB_CylinderFrontIOIn;
        public ConfigIOInEx MidSM_FB_CylinderFrontIOInEx
        {
            get
            {
                if (_MidSM_FB_CylinderFrontIOIn == null)
                {
                    _MidSM_FB_CylinderFrontIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位前后气缸出感应", 1);
                }
                return _MidSM_FB_CylinderFrontIOIn;
            }

        }

        private ConfigIOInEx _MidSM_FB_CylinderBackIOIn;
        public ConfigIOInEx MidSM_FB_CylinderBackIOInEx
        {
            get
            {
                if (_MidSM_FB_CylinderBackIOIn == null)
                {
                    _MidSM_FB_CylinderBackIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位前后气缸回感应", 1);
                }
                return _MidSM_FB_CylinderBackIOIn;
            }

        }


        private ConfigIOInEx _MidSM_UD_CylinderUPIOIn;
        public ConfigIOInEx MidSM_UD_CylinderUPIOInEx
        {
            get
            {
                if (_MidSM_UD_CylinderUPIOIn == null)
                {
                    _MidSM_UD_CylinderUPIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位上下气缸上感应", 1);
                }
                return _MidSM_UD_CylinderUPIOIn;
            }

        }

        private ConfigIOInEx _MidSM_UD_CylinderdownIOIn;
        public ConfigIOInEx MidSM_UD_CylinderDownIOInEx
        {
            get
            {
                if (_MidSM_UD_CylinderdownIOIn == null)
                {
                    _MidSM_UD_CylinderdownIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位上下气缸下感应", 1);
                }
                return _MidSM_UD_CylinderdownIOIn;
            }
        }

        private ConfigIOInEx _MidSM_GlueUD_CylinderdownIOIn;
        public ConfigIOInEx MidSM_GlueUD_CylinderDownIOInEx
        {
            get
            {
                if (_MidSM_GlueUD_CylinderdownIOIn == null)
                {
                    _MidSM_GlueUD_CylinderdownIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位胶带上下气缸下感应", 1);
                }
                return _MidSM_GlueUD_CylinderdownIOIn;
            }
        }


        private ConfigIOInEx _MidSM_GlueUD_CylinderUPIOIn;
        public ConfigIOInEx MidSM_GlueUD_CylinderUPIOInEx
        {
            get
            {
                if (_MidSM_GlueUD_CylinderUPIOIn == null)
                {
                    _MidSM_GlueUD_CylinderUPIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位胶带上下气缸上感应", 1);
                }
                return _MidSM_GlueUD_CylinderUPIOIn;
            }
        }


        private ConfigIOInEx _MidSM_RollerUD_CylinderUPIOIn;
        public ConfigIOInEx MidSM_RollerUD_CylinderUPIOInEx
        {
            get
            {
                if (_MidSM_RollerUD_CylinderUPIOIn == null)
                {
                    _MidSM_RollerUD_CylinderUPIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位滚轮上下气缸上感应", 1);
                }
                return _MidSM_RollerUD_CylinderUPIOIn;
            }
        }


        private ConfigIOIn _MidSM_RollerUD_CylinderdownIOIn;
        public ConfigIOIn MidSM_RollerUD_CylinderDownIOIn
        {
            get
            {
                if (_MidSM_RollerUD_CylinderdownIOIn == null)
                {
                    _MidSM_RollerUD_CylinderdownIOIn = new ConfigIOIn(CardIDs.None, -1, false, "中撕膜工位滚轮上下气缸下感应");
                }
                return _MidSM_RollerUD_CylinderdownIOIn;
            }
        }


        private ConfigIOInEx _MidSM_WAxis_ServoalarmIOIn;
        public ConfigIOInEx MidSM_WAxis_ServoAlarmIOInEx
        {
            get
            {
                if (_MidSM_WAxis_ServoalarmIOIn == null)
                {
                    _MidSM_WAxis_ServoalarmIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "中撕膜工位W轴伺服报警", 1);
                }
                return _MidSM_WAxis_ServoalarmIOIn;
            }
        }
        #endregion

        #region //右撕膜工位输入
        private ConfigIOIn _RightSMMPVacuumIOIn;
        public ConfigIOIn RightSMMPVacuumIOIn
        {
            get
            {
                if (_RightSMMPVacuumIOIn == null)
                {
                    _RightSMMPVacuumIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右撕膜膜片光纤检测");
                }
                return _RightSMMPVacuumIOIn;
            }
        }


        private ConfigIOIn _RightSMVacuumIOIn;
        public ConfigIOIn RightSMVacuumIOIn
        {
            get
            {
                if (_RightSMVacuumIOIn == null)
                {
                    _RightSMVacuumIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右撕膜工位真空检测");

                }
                return _RightSMVacuumIOIn;

            }


        }


        private ConfigIOIn _RightSMGlueOpticalIOIn;
        public ConfigIOIn RightSMGlueOpticalIOIn
        {
            get
            {
                if (_RightSMGlueOpticalIOIn == null)
                {
                    _RightSMGlueOpticalIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右撕膜工位胶带有无光纤");
                }
                return _RightSMGlueOpticalIOIn;
            }
        }

        private ConfigIOIn _RightSM_LR_CylinderleftIOIn;
        public ConfigIOIn RightSM_LR_CylinerLeftIOIn
        {
            get
            {
                if (_RightSM_LR_CylinderleftIOIn == null)
                {
                    _RightSM_LR_CylinderleftIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右撕膜工位左右气缸左感应");
                }
                return _RightSM_LR_CylinderleftIOIn;
            }

        }



        private ConfigIOIn _RightSM_LR_CylinderRightIOIn;
        public ConfigIOIn RightSM_LR_CylinderRightIOIn
        {
            get
            {
                if (_RightSM_LR_CylinderRightIOIn == null)
                {
                    _RightSM_LR_CylinderRightIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右撕膜工位左右气缸右感应");
                }
                return _RightSM_LR_CylinderRightIOIn;
            }

        }

        private ConfigIOIn _RightSM_FB_CylinderFrontIOIn;
        public ConfigIOIn RightSM_FB_CylinderFrontIOIn
        {
            get
            {
                if (_RightSM_FB_CylinderFrontIOIn == null)
                {
                    _RightSM_FB_CylinderFrontIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右撕膜工位前后气缸出感应");
                }
                return _RightSM_FB_CylinderFrontIOIn;
            }

        }

        private ConfigIOIn _RightSM_FB_CylinderBackIOIn;
        public ConfigIOIn RightSM_FB_CylinderBackIOIn
        {
            get
            {
                if (_RightSM_FB_CylinderBackIOIn == null)
                {
                    _RightSM_FB_CylinderBackIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右撕膜工位前后气缸回感应");
                }
                return _RightSM_FB_CylinderBackIOIn;
            }

        }


        private ConfigIOIn _RightSM_UD_CylinderUPIOIn;
        public ConfigIOIn RightSM_UD_CylinderUPIOIn
        {
            get
            {
                if (_RightSM_UD_CylinderUPIOIn == null)
                {
                    _RightSM_UD_CylinderUPIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右撕膜工位上下气缸上感应");
                }
                return _RightSM_UD_CylinderUPIOIn;
            }

        }

        private ConfigIOIn _RightSM_UD_CylinderdownIOIn;
        public ConfigIOIn RightSM_UD_CylinderDownIOIn
        {
            get
            {
                if (_RightSM_UD_CylinderdownIOIn == null)
                {
                    _RightSM_UD_CylinderdownIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右撕膜工位上下气缸下感应");
                }
                return _RightSM_UD_CylinderdownIOIn;
            }
        }

        private ConfigIOInEx _RightSM_GlueUD_CylinderdownIOInEx;
        public ConfigIOInEx RightSM_GlueUD_CylinderDownIOInEx
        {
            get
            {
                if (_RightSM_GlueUD_CylinderdownIOInEx == null)
                {
                    _RightSM_GlueUD_CylinderdownIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "右撕膜工位胶带上下气缸下感应", 1);
                }
                return _RightSM_GlueUD_CylinderdownIOInEx;
            }
        }

        private ConfigIOIn _RightSM_GlueUDCylinderUPIOIn;
        public ConfigIOIn RightSM_GlueUDCylinderUPIOIn
        {
            get
            {
                if (_RightSM_GlueUDCylinderUPIOIn == null)
                {
                    _RightSM_GlueUDCylinderUPIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右膜工位胶带上下气缸上感应");
                }
                return _RightSM_GlueUDCylinderUPIOIn;
            }
        }




        private ConfigIOInEx _RightSM_RollerUD_CylinderUPIOInEx;
        public ConfigIOInEx RightSM_RollerUD_CylinderUPIOInEx
        {
            get
            {
                if (_RightSM_RollerUD_CylinderUPIOInEx == null)
                {
                    _RightSM_RollerUD_CylinderUPIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "右撕膜滚轮上下气缸上感应", 1);
                }
                return _RightSM_RollerUD_CylinderUPIOInEx;
            }
        }



        private ConfigIOInEx _RightSM_RollerUD_CylinderdownIOInEx;
        public ConfigIOInEx RightSM_RollerUD_CylinderDownIOInEx
        {
            get
            {
                if (_RightSM_RollerUD_CylinderdownIOInEx == null)
                {
                    _RightSM_RollerUD_CylinderdownIOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "右撕膜工位滚轮上下气缸下感应", 1);
                }
                return _RightSM_RollerUD_CylinderdownIOInEx;
            }
        }






        private ConfigIOInEx _SM_CCDXAxis_Alarm_IOInEx;

        public ConfigIOInEx SM_CCDXAxis_Alarm_IOInEx
        {
            get
            {

                if (_SM_CCDXAxis_Alarm_IOInEx == null)
                {
                    _SM_CCDXAxis_Alarm_IOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "撕膜检测相机伺服报警", 1);
                }
                return _SM_CCDXAxis_Alarm_IOInEx;

            }

        }

        private ConfigIOInEx _StepMotorPower_IOInEx;
        public ConfigIOInEx StepMotorPower_IOInEx
        {
            get
            {

                if (_StepMotorPower_IOInEx == null)
                {
                    _StepMotorPower_IOInEx = new ConfigIOInEx(CardIDs.None, -1, false, "步进动力电源检测", 1);
                }
                return _StepMotorPower_IOInEx;

            }

        }




        private ConfigIOInEx _RightSM_WAxis_ServoalarmIOIn;
        public ConfigIOInEx RightSM_WAxis_ServoAlarmIOInEx
        {
            get
            {
                if (_RightSM_WAxis_ServoalarmIOIn == null)
                {
                    _RightSM_WAxis_ServoalarmIOIn = new ConfigIOInEx(CardIDs.None, -1, false, "右撕膜工位W轴伺服报警", 1);
                }
                return _RightSM_WAxis_ServoalarmIOIn;
            }
        }
        #endregion

        #region //左弯折工位输入
        private ConfigIOIn _LeftBend_FPCOptical_IOIn;
        public ConfigIOIn LeftBend_FPCOptical_IOIn
        {
            get
            {
                if (_LeftBend_FPCOptical_IOIn == null)
                {
                    _LeftBend_FPCOptical_IOIn = new ConfigIOIn(CardIDs.None, -1, false, "左折弯夹FPC光纤");
                }
                return _LeftBend_FPCOptical_IOIn;
            }
        }


        private ConfigIOIn _LeftBend_stgVacuum_IOIn;
        public ConfigIOIn LeftBend_stgVacuum_IOIn
        {
            get
            {
                if (_LeftBend_stgVacuum_IOIn == null)
                {
                    _LeftBend_stgVacuum_IOIn = new ConfigIOIn(CardIDs.None, -1, false, "左折弯平台真空检测");
                }
                return _LeftBend_stgVacuum_IOIn;
            }
        }


        private ConfigIOIn _LeftBend_PressCylinder_UPIOIn;
        public ConfigIOIn LeftBend_PressCylinder_UPIOIn
        {
            get
            {
                if (_LeftBend_PressCylinder_UPIOIn == null)
                {
                    _LeftBend_PressCylinder_UPIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左折弯压合上下气缸上感应");
                }
                return _LeftBend_PressCylinder_UPIOIn;
            }
        }

        private ConfigIOIn _LeftBend_PressCylinder_DownIOIn;
        public ConfigIOIn LeftBend_PressCylinder_DownIOIn
        {
            get
            {
                if (_LeftBend_PressCylinder_DownIOIn == null)
                {
                    _LeftBend_PressCylinder_DownIOIn = new ConfigIOIn(CardIDs.None, -1, false, "左折弯压合上下气缸上感应");
                }
                return _LeftBend_PressCylinder_DownIOIn;
            }
        }


        #endregion

        #region //中弯折工位输入
        private ConfigIOIn _MidBend_FPCOptical_IOIn;
        public ConfigIOIn MidBend_FPCOptical_IOIn
        {
            get
            {
                if (_MidBend_FPCOptical_IOIn == null)
                {
                    _MidBend_FPCOptical_IOIn = new ConfigIOIn(CardIDs.None, -1, false, "中折弯夹FPC光纤");
                }
                return _MidBend_FPCOptical_IOIn;
            }
        }


        private ConfigIOIn _MidBend_stgVacuum_IOIn;
        public ConfigIOIn MidBend_stgVacuum_IOIn
        {
            get
            {
                if (_MidBend_stgVacuum_IOIn == null)
                {
                    _MidBend_stgVacuum_IOIn = new ConfigIOIn(CardIDs.None, -1, false, "中折弯平台真空检测");
                }
                return _MidBend_stgVacuum_IOIn;
            }
        }













        private ConfigIOIn _MidBend_PressCylinder_UPIOIn;
        public ConfigIOIn MidBend_PressCylinder_UPIOIn
        {
            get
            {
                if (_MidBend_PressCylinder_UPIOIn == null)
                {
                    _MidBend_PressCylinder_UPIOIn = new ConfigIOIn(CardIDs.None, -1, false, "中折弯压合上下气缸上感应");
                }
                return _MidBend_PressCylinder_UPIOIn;
            }
        }

        private ConfigIOIn _MidBend_PressCylinder_DownIOIn;
        public ConfigIOIn MidBend_PressCylinder_DownIOIn
        {
            get
            {
                if (_MidBend_PressCylinder_DownIOIn == null)
                {
                    _MidBend_PressCylinder_DownIOIn = new ConfigIOIn(CardIDs.None, -1, false, "中折弯压合上下气缸下感应");
                }
                return _MidBend_PressCylinder_DownIOIn;
            }
        }


        #endregion

        #region  //右折弯工位输入
        private ConfigIOIn _RightBend_FPCOptical_IOIn;
        public ConfigIOIn RightBend_FPCOptical_IOIn
        {
            get
            {
                if (_RightBend_FPCOptical_IOIn == null)
                {
                    _RightBend_FPCOptical_IOIn = new ConfigIOIn(CardIDs.None, -1, false, "右折弯夹FPC光纤");
                }
                return _RightBend_FPCOptical_IOIn;
            }
        }


        private ConfigIOIn _RightBend_stgVacuum_IOIn;
        public ConfigIOIn RightBend_stgVacuum_IOIn
        {
            get
            {
                if (_RightBend_stgVacuum_IOIn == null)
                {
                    _RightBend_stgVacuum_IOIn = new ConfigIOIn(CardIDs.None, -1, false, "右折弯平台真空检测");
                }
                return _RightBend_stgVacuum_IOIn;
            }
        }


        private ConfigIOIn _NGLineCylinderDynamic;
        public ConfigIOIn NGLineCylinderDynamic
        {
            get
            {
                if (_NGLineCylinderDynamic == null)
                {
                    _NGLineCylinderDynamic = new ConfigIOIn(CardIDs.None, -1, false, "NG流水线推位气缸伸");
                }
                return _NGLineCylinderDynamic;
            }
        }

        private ConfigIOIn _NGLineCylinderStatic;
        public ConfigIOIn NGLineCylinderStatic
        {
            get
            {
                if (_NGLineCylinderStatic == null)
                {
                    _NGLineCylinderStatic = new ConfigIOIn(CardIDs.None, -1, false, "NG流水线推位气缸缩");
                }
                return _NGLineCylinderStatic;
            }
        }


        private ConfigIOIn _NGLineButtion;
        public ConfigIOIn NGLineButtion
        {
            get
            {
                if (_NGLineButtion == null)
                {
                    _NGLineButtion = new ConfigIOIn(CardIDs.None, -1, false, "NG流水线推位按钮");
                }
                return _NGLineButtion;
            }
        }




        private ConfigIOIn _RightBend_PressCylinder_UPIOIn;
        public ConfigIOIn RightBend_PressCylinder_UPIOIn
        {
            get
            {
                if (_RightBend_PressCylinder_UPIOIn == null)
                {
                    _RightBend_PressCylinder_UPIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右折弯压合上下气缸上感应");
                }
                return _RightBend_PressCylinder_UPIOIn;
            }
        }

        private ConfigIOIn _RightBend_PressCylinder_DownIOIn;
        public ConfigIOIn RightBend_PressCylinder_DownIOIn
        {
            get
            {
                if (_RightBend_PressCylinder_DownIOIn == null)
                {
                    _RightBend_PressCylinder_DownIOIn = new ConfigIOIn(CardIDs.None, -1, false, "右折弯压合上下气缸下感应");
                }
                return _RightBend_PressCylinder_DownIOIn;
            }
        }

        #endregion

        #endregion

        #region //IOOUT
        private ConfigIOOut _UpstreamSuplyBeltIOOut;
        public ConfigIOOut UpstreamSuplyBeltIOOut
        {
            get
            {
                if (_UpstreamSuplyBeltIOOut == null)
                {
                    _UpstreamSuplyBeltIOOut = new ConfigIOOut(CardIDs.None, -1, false, "上游来料皮带中间继电器输出信号", 0);
                }
                return _UpstreamSuplyBeltIOOut;
            }
        }

        private ConfigIOOut _SuplyBeltIOOut;
        public ConfigIOOut SuplyBeltIOOut
        {
            get
            {
                if (_SuplyBeltIOOut == null)
                {
                    _SuplyBeltIOOut = new ConfigIOOut(CardIDs.None, -1, false, "来料皮带中间继电器输出信号", 0);
                }
                return _SuplyBeltIOOut;
            }
        }

        private ConfigIOOut _LoadVacuumIOOut;
        public ConfigIOOut LoadVacuumIOOut
        {
            get
            {
                if (_LoadVacuumIOOut == null)
                {
                    _LoadVacuumIOOut = new ConfigIOOut(CardIDs.None, -1, false, "上料工位真空吸", 0);
                }
                return _LoadVacuumIOOut;
            }
        }

        private ConfigIOOut _LoadFPCVacuumIOOut;
        public ConfigIOOut LoadFPCVacuumIOOut
        {
            get
            {
                if (_LoadFPCVacuumIOOut == null)
                {
                    _LoadFPCVacuumIOOut = new ConfigIOOut(CardIDs.None, -1, false, "上料工位FPC真空吸", 0);
                }
                return _LoadFPCVacuumIOOut;
            }
        }


        private ConfigIOOut _LoadBlowVacuumIOOut;
        public ConfigIOOut LoadBlowVacuumIOOut
        {
            get
            {
                if (_LoadBlowVacuumIOOut == null)
                {
                    _LoadBlowVacuumIOOut = new ConfigIOOut(CardIDs.None, -1, false, "上料工位破真空吸", 0);
                }
                return _LoadBlowVacuumIOOut;

            }

        }

        private ConfigIOOut _Transfer_Suckvacuum_IOOut;
        public ConfigIOOut Transfer_Suckvacuum_IOOut
        {
            get
            {
                if (_Transfer_Suckvacuum_IOOut == null)
                {
                    _Transfer_Suckvacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中转工位真空吸电磁阀", 0);
                }
                return _Transfer_Suckvacuum_IOOut;
            }
        }


        private ConfigIOOut _Transfer_FPCSuckvacuum_IOOut;
        public ConfigIOOut Transfer_FPCSuckvacuum_IOOut
        {
            get
            {
                if (_Transfer_FPCSuckvacuum_IOOut == null)
                {
                    _Transfer_FPCSuckvacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中转工位FPC真空吸电磁阀", 0);
                }
                return _Transfer_FPCSuckvacuum_IOOut;
            }
        }

        private ConfigIOOut _Transfer_Blowvacuum_IOOut;
        public ConfigIOOut Transfer_Blowvacuum_IOOut
        {
            get
            {
                if (_Transfer_Blowvacuum_IOOut == null)
                {
                    _Transfer_Blowvacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中转工位破真空电磁阀", 0);
                }
                return _Transfer_Blowvacuum_IOOut;
            }
        }


        private ConfigIOOut _Discharge_Suckvacuum_IOOut;
        public ConfigIOOut Discharge_Suckvacuum_IOOut
        {
            get
            {
                if (_Discharge_Suckvacuum_IOOut == null)
                {
                    _Discharge_Suckvacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "出料工位真空吸电磁阀", 0);
                }
                return _Discharge_Suckvacuum_IOOut;
            }
        }


        private ConfigIOOut _Discharge_FPCSuckvacuum_IOOut;
        public ConfigIOOut Discharge_FPCSuckvacuum_IOOut
        {
            get
            {
                if (_Discharge_FPCSuckvacuum_IOOut == null)
                {
                    _Discharge_FPCSuckvacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "出料工位FPC真空吸电磁阀", 0);
                }
                return _Discharge_FPCSuckvacuum_IOOut;
            }
        }

        private ConfigIOOut _Discharge_Blowvacuum_IOOut;
        public ConfigIOOut Discharge_Blowvacuum_IOOut
        {
            get
            {
                if (_Discharge_Blowvacuum_IOOut == null)
                {
                    _Discharge_Blowvacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "出料工位破真空电磁阀", 0);
                }
                return _Discharge_Blowvacuum_IOOut;
            }
        }

        private ConfigIOOut _NGlineBeltIOOut;
        public ConfigIOOut NGlineBeltIOOut
        {
            get
            {
                if (_NGlineBeltIOOut == null)
                {
                    _NGlineBeltIOOut = new ConfigIOOut(CardIDs.None, -1, false, "NG流水线中间继电器", 0);
                }
                return _NGlineBeltIOOut;
            }
        }

        private ConfigIOOut _DischargeLineBeltIOOut;
        public ConfigIOOut DischargeLineBeltIOOut
        {
            get
            {
                if (_DischargeLineBeltIOOut == null)
                {
                    _DischargeLineBeltIOOut = new ConfigIOOut(CardIDs.None, -1, false, "出料流水线中间继电器", 0);
                }
                return _DischargeLineBeltIOOut;
            }
        }
        private ConfigIOOut _NGLinePushCylinder;
        public ConfigIOOut NGLinePushCylinder
        {
            get
            {
                if (_NGLinePushCylinder == null)
                {
                    _NGLinePushCylinder = new ConfigIOOut(CardIDs.None, -1, false, "NG流水线推料气缸", 0);
                }
                return _NGLinePushCylinder;
            }
        }

        private ConfigIOOut _TearAOIBlowCylinder;
        public ConfigIOOut TearAOIBlowCylinder
        {
            get
            {
                if (_TearAOIBlowCylinder == null)
                {
                    _TearAOIBlowCylinder = new ConfigIOOut(CardIDs.None, -1, false, "撕膜检测吹气气缸", 0);
                }
                return _TearAOIBlowCylinder;
            }
        }
        private ConfigIOOut _CIMCOR_Cylinder;
        public ConfigIOOut CIMCOR_Cylinder
        {
            get
            {
                if (_CIMCOR_Cylinder == null)
                {
                    _CIMCOR_Cylinder = new ConfigIOOut(CardIDs.None, -1, false, "离子风棒吹气电磁阀", 0);
                }
                return _CIMCOR_Cylinder;
            }
        }







        #region 折弯安全门
        private ConfigIOOutEx _Bend_FrontGate1_IOOutEx;
        public ConfigIOOutEx Bend_FrontGate1_IOOutEx
        {
            get
            {
                if (_Bend_FrontGate1_IOOutEx == null)
                {
                    _Bend_FrontGate1_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "弯折工位前门锁1输出", 0, 1);
                }
                return _Bend_FrontGate1_IOOutEx;
            }
        }



        private ConfigIOOutEx _Bend_BackGate1_IOOutEx;
        public ConfigIOOutEx Bend_BackGate1_IOOutEx
        {
            get
            {
                if (_Bend_BackGate1_IOOutEx == null)
                {
                    _Bend_BackGate1_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "弯折工位后门锁1输出", 0, 1);
                }
                return _Bend_BackGate1_IOOutEx;
            }
        }

        private ConfigIOOutEx _Bend_SideGate1_IOOutEx;
        public ConfigIOOutEx Bend_SideGate1_IOOutEx
        {
            get
            {
                if (_Bend_SideGate1_IOOutEx == null)
                {
                    _Bend_SideGate1_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "弯折工位侧门锁1输出", 0, 1);
                }
                return _Bend_SideGate1_IOOutEx;
            }
        }










        #endregion 

        #region 折弯光源

        private ConfigIOOutEx _LeftBend_UPOPTControl_IOInEx;
        public ConfigIOOutEx LeftBend_UPOPTControl_IOOutEx
        {
            get
            {
                if (_LeftBend_UPOPTControl_IOInEx == null)
                {

                    _LeftBend_UPOPTControl_IOInEx = new ConfigIOOutEx(CardIDs.None, -1, false, "左折弯上光源控制器", 0, 1);
                }

                return _LeftBend_UPOPTControl_IOInEx;

            }

        }








        private ConfigIOOutEx _MidBend_UPOPTControl_IOInEx;
        public ConfigIOOutEx MidBend_UPOPTControl_IOOutEx
        {
            get
            {
                if (_MidBend_UPOPTControl_IOInEx == null)
                {

                    _MidBend_UPOPTControl_IOInEx = new ConfigIOOutEx(CardIDs.None, -1, false, "中折完上光源控制器", 0, 1);
                }

                return _MidBend_UPOPTControl_IOInEx;

            }

        }





        private ConfigIOOutEx _RightBend_UPOPTControl_IOOutEx;
        public ConfigIOOutEx RightBend_UPOPTControl_IOOutEx
        {
            get
            {
                if (_RightBend_UPOPTControl_IOOutEx == null)
                {

                    _RightBend_UPOPTControl_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "右折完上光源控制器", 0, 1);
                }

                return _RightBend_UPOPTControl_IOOutEx;

            }

        }






        #endregion

        #region 进出料翻转输出
        private ConfigIOOut _Feed_RotateSuck_IOOut;
        public ConfigIOOut Feed_RotateSuck_IOOut
        {
            get
            {
                if (_Feed_RotateSuck_IOOut == null)
                {
                    _Feed_RotateSuck_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "进料翻转真空电磁阀", 0);
                }
                return _Feed_RotateSuck_IOOut;
            }
        }


        private ConfigIOOut _Feed_RotateBreakVacuum_IOOut;
        public ConfigIOOut Feed_RotateBreakVacuum_IOOut
        {
            get
            {
                if (_Feed_RotateBreakVacuum_IOOut == null)
                {
                    _Feed_RotateBreakVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "进料翻转破真空电磁阀", 0);
                }
                return _Feed_RotateBreakVacuum_IOOut;
            }
        }

        private ConfigIOOut _Feed_RotateUDCylinderUp_IOOut;
        public ConfigIOOut Feed_RotateUDCylinderUp_IOOut
        {
            get
            {
                if (_Feed_RotateUDCylinderUp_IOOut == null)
                {
                    _Feed_RotateUDCylinderUp_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "进料翻转工位上下气缸上电磁阀", 0);
                }
                return _Feed_RotateUDCylinderUp_IOOut;
            }
        }

        private ConfigIOOut _Feed_RotateUDCylinderDown_IOOut;
        public ConfigIOOut Feed_RotateUDCylinderDown_IOOut
        {
            get
            {
                if (_Feed_RotateUDCylinderDown_IOOut == null)
                {
                    _Feed_RotateUDCylinderDown_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "进料翻转工位上下气缸下电磁阀", 0);
                }
                return _Feed_RotateUDCylinderDown_IOOut;
            }
        }


        private ConfigIOOutEx _Feed_RotateCylinder_IOOutEx;
        public ConfigIOOutEx Feed_RotateCylinder_IOOutEx
        {
            get
            {
                if (_Feed_RotateCylinder_IOOutEx == null)
                {
                    _Feed_RotateCylinder_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "进料翻转气缸出位电磁阀", 0, 1);
                }
                return _Feed_RotateCylinder_IOOutEx;
            }
        }

        private ConfigIOOutEx _Feed_RotateCylinderORG_IOOutEx;
        public ConfigIOOutEx Feed_RotateCylinderORG_IOOutEx
        {
            get
            {
                if (_Feed_RotateCylinderORG_IOOutEx == null)
                {
                    _Feed_RotateCylinderORG_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "进料翻转气缸原位电磁阀", 0, 1);
                }
                return _Feed_RotateCylinderORG_IOOutEx;
            }
        }



        private ConfigIOOut _Feed_RotateFPCSuck_IOOut;
        public ConfigIOOut Feed_RotateFPCSuck_IOOut
        {
            get
            {
                if (_Feed_RotateFPCSuck_IOOut == null)
                {
                    _Feed_RotateFPCSuck_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "进料翻转FPC真空吸", 0);
                }
                return _Feed_RotateFPCSuck_IOOut;
            }
        }

        //出
        private ConfigIOOutEx _Discharge_UPCylinder_CardCOutEx;
        public ConfigIOOutEx Discharge_UPCylinder_CardCOutEx
        {
            get
            {
                if (_Discharge_UPCylinder_CardCOutEx == null)
                {
                    _Discharge_UPCylinder_CardCOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "出料翻转工位上下气缸出电磁阀", 0, 1);
                }
                if (_Discharge_UPCylinder_CardCOutEx.Delay == 0)
                {
                    _Discharge_UPCylinder_CardCOutEx.Delay = 200;
                }
                return _Discharge_UPCylinder_CardCOutEx;
            }
        }


        private ConfigIOOutEx _Discharge_RotateCylinder_CardCIOOutEx;

        public ConfigIOOutEx Discharge_RotateCylinder_CardCIOOutEx
        {
            get
            {
                if (_Discharge_RotateCylinder_CardCIOOutEx == null)
                {
                    _Discharge_RotateCylinder_CardCIOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "出料翻转工位翻转气缸出电磁阀", 0, 1);
                }
                if (_Discharge_RotateCylinder_CardCIOOutEx.Delay == 0)
                {
                    _Discharge_RotateCylinder_CardCIOOutEx.Delay = 200;
                }
                return _Discharge_RotateCylinder_CardCIOOutEx;
            }
        }

        private ConfigIOOutEx _DischargeRotateSuck_OutEx;

        public ConfigIOOutEx DischargeRotateSuck_OutEx
        {
            get
            {
                if (_DischargeRotateSuck_OutEx == null)
                {
                    _DischargeRotateSuck_OutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "出料翻转真空电磁阀", 0, 1);
                }
                if (_DischargeRotateSuck_OutEx.Delay == 0)
                {
                    _DischargeRotateSuck_OutEx.Delay = 200;
                }
                return _DischargeRotateSuck_OutEx;
            }
        }

        private ConfigIOOutEx _DischargeRotateBlow_OutEx;

        public ConfigIOOutEx DischargeRotateBlow_OutEx
        {
            get
            {
                if (_DischargeRotateBlow_OutEx == null)
                {
                    _DischargeRotateBlow_OutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "出料翻转破真空电磁阀", 0, 1);
                }
                if (_DischargeRotateBlow_OutEx.Delay == 0)
                {
                    _DischargeRotateBlow_OutEx.Delay = 200;
                }
                return _DischargeRotateBlow_OutEx;
            }
        }

        private ConfigIOOutEx _DischargeRotateFPCSuck_OutEx;

        public ConfigIOOutEx DischargeRotateFPCSuck_OutEx
        {
            get
            {
                if (_DischargeRotateFPCSuck_OutEx == null)
                {
                    _DischargeRotateFPCSuck_OutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "出料翻转FPC真空电磁阀", 0, 1);
                }
                if (_DischargeRotateFPCSuck_OutEx.Delay == 0)
                {
                    _DischargeRotateFPCSuck_OutEx.Delay = 200;
                }
                return _DischargeRotateFPCSuck_OutEx;
            }
        }

        public ConfigIOOut _Feed_UDCylinderUP_IOOut;
        public ConfigIOOut Feed_UDCylinderUP_IOOut
        {
            get
            {
                if (_Feed_UDCylinderUP_IOOut == null)
                {
                    _Feed_UDCylinderUP_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "进料工位上下气缸上电磁阀", 0);
                }
                return _Feed_UDCylinderUP_IOOut;
            }
        }

        public ConfigIOOut _Feed_UDCylinderDown_IOOut;
        public ConfigIOOut Feed_UDCylinderDown_IOOut
        {
            get
            {
                if (_Feed_UDCylinderDown_IOOut == null)
                {
                    _Feed_UDCylinderDown_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "进料工位上下气缸下电磁阀", 0);
                }
                return _Feed_UDCylinderDown_IOOut;
            }
        }

        #endregion


        #region //撕膜工位输出
        private ConfigIOOutEx _SMStation_OPTIOOutEx;
        public ConfigIOOutEx SMStation_OPTIOOutEx
        {
            get
            {
                if (_SMStation_OPTIOOutEx == null)
                {
                    _SMStation_OPTIOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "撕膜工位光源控制", 0, 1);

                }
                return _SMStation_OPTIOOutEx;
            }
        }

        private ConfigIOOut _RedLightIOOut;
        public ConfigIOOut RedLightIOOut
        {
            get
            {
                if (_RedLightIOOut == null)
                {
                    _RedLightIOOut = new ConfigIOOut(CardIDs.None, -1, false, "三色灯红", 0);
                }
                return _RedLightIOOut;
            }
        }

        private ConfigIOOut _GreenLightIOOut;
        public ConfigIOOut GreenLightIOOut
        {
            get
            {
                if (_GreenLightIOOut == null)
                {
                    _GreenLightIOOut = new ConfigIOOut(CardIDs.None, -1, false, "三色灯绿", 0);
                }
                return _GreenLightIOOut;
            }
        }

        private ConfigIOOut _YellowLightIOOut;
        public ConfigIOOut YellowLightIOOut
        {
            get
            {
                if (_YellowLightIOOut == null)
                {
                    _YellowLightIOOut = new ConfigIOOut(CardIDs.None, -1, false, "三色灯黄", 0);
                }
                return _YellowLightIOOut;
            }
        }

        private ConfigIOOut _BuzzerIOOut;
        public ConfigIOOut BuzzerIOOut
        {
            get
            {
                if (_BuzzerIOOut == null)
                {
                    _BuzzerIOOut = new ConfigIOOut(CardIDs.None, -1, false, "蜂鸣器", 0);
                }
                return _BuzzerIOOut;
            }
        }

        private ConfigIOOut _StartBlueLightIOOut;
        public ConfigIOOut StartBlueLightIOOut
        {
            get
            {
                if (_StartBlueLightIOOut == null)
                {
                    _StartBlueLightIOOut = new ConfigIOOut(CardIDs.None, -1, false, "蓝色启动按钮灯", 0);
                }
                return _StartBlueLightIOOut;
            }
        }

        private ConfigIOOut _ResetYellowLightIOOut;
        public ConfigIOOut ResetYellowLightIOOut
        {
            get
            {
                if (_ResetYellowLightIOOut == null)
                {
                    _ResetYellowLightIOOut = new ConfigIOOut(CardIDs.None, -1, false, "复位黄色按钮灯", 0);
                }
                return _ResetYellowLightIOOut;
            }
        }

        private ConfigIOOutEx _SMStation_SideGate1_IOOutEx;
        public ConfigIOOutEx SMStation_SideGate1_IOOutEx
        {
            get
            {
                if (_SMStation_SideGate1_IOOutEx == null)
                {
                    _SMStation_SideGate1_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "撕膜工位侧门锁1输出", 0, 1);

                }
                return _SMStation_SideGate1_IOOutEx;
            }
        }

        private ConfigIOOutEx _SMStation_SideGate2_IOOutEx;
        public ConfigIOOutEx SMStation_SideGate2_IOOutEx
        {
            get
            {
                if (_SMStation_SideGate2_IOOutEx == null)
                {
                    _SMStation_SideGate2_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "撕膜工位侧门锁2输出", 0, 1);

                }
                return _SMStation_SideGate2_IOOutEx;
            }
        }

        private ConfigIOOutEx _SMStation_FrontGate1_IOOutEx;
        public ConfigIOOutEx SMStation_FrontGate1_IOOutEx
        {
            get
            {
                if (_SMStation_FrontGate1_IOOutEx == null)
                {
                    _SMStation_FrontGate1_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "撕膜工位前门锁1输出", 0, 1);
                }
                return _SMStation_FrontGate1_IOOutEx;
            }
        }

        //private ConfigIOOut _SMStation_FrontGate2_IOOut;
        //public ConfigIOOut SMStation_FrontGate2_IOOut
        //{
        //    get
        //    {
        //        if (_SMStation_FrontGate2_IOOut == null)
        //        {
        //            _SMStation_FrontGate2_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "撕膜工位前门门禁2锁门", 0);
        //        }
        //        return _SMStation_FrontGate2_IOOut;
        //    }
        //}

        private ConfigIOOutEx _SMStation_BackGate1_IOOutEx;
        public ConfigIOOutEx SMStation_BackGate1_IOOutEx
        {
            get
            {
                if (_SMStation_BackGate1_IOOutEx == null)
                {
                    _SMStation_BackGate1_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "撕膜工位后门锁1输出", 0, 1);
                }
                return _SMStation_BackGate1_IOOutEx;
            }
        }

        //private ConfigIOOut _SMStation_BackGate2_IOOut;
        //public ConfigIOOut SMStation_BackGate2_IOOut
        //{
        //    get
        //    {
        //        if (_SMStation_BackGate2_IOOut == null)
        //        {
        //            _SMStation_BackGate2_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "撕膜工位后门门禁2锁门", 0);
        //        }
        //        return _SMStation_BackGate2_IOOut;
        //    }
        //}


        #endregion

        #region  //左撕膜工位输出



        private ConfigIOOut _LeftSM_StgVacuum_IOOut;
        public ConfigIOOut LeftSM_StgVacuum_IOOut
        {
            get
            {
                if (_LeftSM_StgVacuum_IOOut == null)
                {
                    _LeftSM_StgVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位平台真空吸", 0);
                }
                return _LeftSM_StgVacuum_IOOut;
            }
        }

        private ConfigIOOut _LeftSM_StgFPCVacuum_IOOut;
        public ConfigIOOut LeftSM_StgFPCVacuum_IOOut
        {
            get
            {
                if (_LeftSM_StgFPCVacuum_IOOut == null)
                {
                    _LeftSM_StgFPCVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位平台FPC真空吸", 0);
                }
                return _LeftSM_StgFPCVacuum_IOOut;
            }
        }

        private ConfigIOOut _LeftSM_StgBlowVacuum_IOOut;
        public ConfigIOOut LeftSM_StgBlowVacuum_IOOut
        {
            get
            {
                if (_LeftSM_StgBlowVacuum_IOOut == null)
                {
                    _LeftSM_StgBlowVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位平台破真空", 0);
                }
                return _LeftSM_StgBlowVacuum_IOOut;
            }
        }

        private ConfigIOOut _LeftSM_StgReduceVacuum_IOOut;
        public ConfigIOOut LeftSM_StgReduceVacuum_IOOut
        {
            get
            {
                if (_LeftSM_StgReduceVacuum_IOOut == null)
                {
                    _LeftSM_StgReduceVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位平台泄真空", 0);
                }
                return _LeftSM_StgReduceVacuum_IOOut;
            }
        }

        private ConfigIOOut _LeftSM_LRCylinder_IOOut;
        public ConfigIOOut LeftSM_LRCylinder_IOOut
        {
            get
            {
                if (_LeftSM_LRCylinder_IOOut == null)
                {
                    _LeftSM_LRCylinder_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位左右气缸电磁阀", 0);
                }
                return _LeftSM_LRCylinder_IOOut;
            }
        }

        private ConfigIOOut _LeftSM_FBCylinder_IOOut;
        public ConfigIOOut LeftSM_FBCylinder_IOOut
        {
            get
            {
                if (_LeftSM_FBCylinder_IOOut == null)
                {
                    _LeftSM_FBCylinder_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位前后气缸电磁阀", 0);
                }
                return _LeftSM_FBCylinder_IOOut;
            }
        }

        private ConfigIOOut _LeftSM_UDCylinder_IOOut;
        public ConfigIOOut LeftSM_UDCylinder_IOOut
        {
            get
            {
                if (_LeftSM_UDCylinder_IOOut == null)
                {
                    _LeftSM_UDCylinder_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位上下气缸电磁阀", 0);
                }
                return _LeftSM_UDCylinder_IOOut;
            }
        }


        private ConfigIOOut _LeftSM_GlueCylinder_IOOut;
        public ConfigIOOut LeftSM_GlueCylinder_IOOut
        {
            get
            {
                if (_LeftSM_GlueCylinder_IOOut == null)
                {
                    _LeftSM_GlueCylinder_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位胶带上下气缸电磁阀", 0);
                }
                return _LeftSM_GlueCylinder_IOOut;
            }
        }

        private ConfigIOOut _LeftSM_RollerCylinder_IOOut;
        public ConfigIOOut LeftSM_RollerCylinder_IOOut
        {
            get
            {
                if (_LeftSM_RollerCylinder_IOOut == null)
                {
                    _LeftSM_RollerCylinder_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位滚轮上下气缸电磁阀", 0);
                }
                return _LeftSM_RollerCylinder_IOOut;
            }
        }

        private ConfigIOOut _LeftSM_GlueUDCylinder_IOOut;
        public ConfigIOOut LeftSM_GlueUDCylinder_IOOut
        {
            get
            {
                if (_LeftSM_GlueUDCylinder_IOOut == null)
                {
                    _LeftSM_GlueUDCylinder_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位胶带上下电磁阀", 0);
                }
                return _LeftSM_GlueUDCylinder_IOOut;
            }

        }

        private ConfigIOOut _LeftSM_GlueLockCylinder_IOOut;
        public ConfigIOOut LeftSM_GlueLockCylinder_IOOut
        {
            get
            {
                if (_LeftSM_GlueLockCylinder_IOOut == null)
                {
                    _LeftSM_GlueLockCylinder_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位胶带气涨轴电磁阀", 0);
                }
                return _LeftSM_GlueLockCylinder_IOOut;
            }
        }

        #endregion

        #region //中撕膜工位输出



        private ConfigIOOut _MidSM_StgVacuum_IOOut;
        public ConfigIOOut MidSM_StgVacuum_IOOut
        {
            get
            {
                if (_MidSM_StgVacuum_IOOut == null)
                {
                    _MidSM_StgVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中撕膜工位平台真空吸", 0);
                }
                return _MidSM_StgVacuum_IOOut;
            }
        }

        private ConfigIOOut _MidSM_StgFPCVacuum_IOOut;
        public ConfigIOOut MidSM_StgFPCVacuum_IOOut
        {
            get
            {
                if (_MidSM_StgFPCVacuum_IOOut == null)
                {
                    _MidSM_StgFPCVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中撕膜工位平台FPC真空吸", 0);
                }
                return _MidSM_StgFPCVacuum_IOOut;
            }
        }

        private ConfigIOOutEx _MidSM_StgBlowVacuum_IOOut;
        public ConfigIOOutEx MidSM_StgBlowVacuum_IOOutEx
        {
            get
            {
                if (_MidSM_StgBlowVacuum_IOOut == null)
                {
                    _MidSM_StgBlowVacuum_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜工位平台破真空", 0, 1);
                }
                return _MidSM_StgBlowVacuum_IOOut;
            }
        }

        private ConfigIOOutEx _MidSM_StgReduceVacuum_IOOut;
        public ConfigIOOutEx MidSM_StgReduceVacuum_IOOutEx
        {
            get
            {
                if (_MidSM_StgReduceVacuum_IOOut == null)
                {
                    _MidSM_StgReduceVacuum_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜工位平台泄真空", 0, 1);
                }
                return _MidSM_StgReduceVacuum_IOOut;
            }
        }

        private ConfigIOOutEx _MidSM_LRCylinder_IOOut;
        public ConfigIOOutEx MidSM_LRCylinder_IOOutEx
        {
            get
            {
                if (_MidSM_LRCylinder_IOOut == null)
                {
                    _MidSM_LRCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜工位左右气缸电磁阀", 0, 1);
                }
                return _MidSM_LRCylinder_IOOut;
            }
        }

        private ConfigIOOutEx _MidSM_FBCylinder_IOOut;
        public ConfigIOOutEx MidSM_FBCylinder_IOOutEx
        {
            get
            {
                if (_MidSM_FBCylinder_IOOut == null)
                {
                    _MidSM_FBCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜工位前后气缸电磁阀", 0, 1);
                }
                return _MidSM_FBCylinder_IOOut;
            }
        }


        private ConfigIOOutEx _MidSM_UDCylinder_IOOut;
        public ConfigIOOutEx MidSM_UDCylinder_IOOutEx
        {
            get
            {
                if (_MidSM_UDCylinder_IOOut == null)
                {
                    _MidSM_UDCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜工位上下气缸电磁阀", 0, 1);
                }
                return _MidSM_UDCylinder_IOOut;
            }
        }

        private ConfigIOOutEx _MidSM_GlueCylinder_IOOut;
        public ConfigIOOutEx MidSM_GlueCylinder_IOOutEx
        {
            get
            {
                if (_MidSM_GlueCylinder_IOOut == null)
                {
                    _MidSM_GlueCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜工位胶带上下气缸电磁阀", 0, 1);
                }
                return _MidSM_GlueCylinder_IOOut;
            }
        }

        private ConfigIOOutEx _MidSM_RollerCylinder_IOOut;
        public ConfigIOOutEx MidSM_RollerCylinder_IOOutEx
        {
            get
            {
                if (_MidSM_RollerCylinder_IOOut == null)
                {
                    _MidSM_RollerCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜工位滚轮上下气缸电磁阀", 0, 1);
                }
                return _MidSM_RollerCylinder_IOOut;
            }
        }


        private ConfigIOOutEx _MidSM_GlueLockCylinder_IOOut;
        public ConfigIOOutEx MidSM_GlueLockCylinder_IOOutEx
        {
            get
            {
                if (_MidSM_GlueLockCylinder_IOOut == null)
                {
                    _MidSM_GlueLockCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜工位胶带气涨轴电磁阀", 0, 1);
                }
                return _MidSM_GlueLockCylinder_IOOut;
            }
        }
        #endregion

        #region  //右撕膜工位输出

        private ConfigIOOutEx _RightSM_StgVacuum_IOOut;
        public ConfigIOOutEx RightSM_StgVacuum_IOOutEx
        {
            get
            {
                if (_RightSM_StgVacuum_IOOut == null)
                {
                    _RightSM_StgVacuum_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜工位平台真空吸", 0, 1);
                }
                return _RightSM_StgVacuum_IOOut;
            }
        }

        private ConfigIOOutEx _RightSM_StgFPCVacuum_IOOut;
        public ConfigIOOutEx RightSM_StgFPCVacuum_IOOutEx
        {
            get
            {
                if (_RightSM_StgFPCVacuum_IOOut == null)
                {
                    _RightSM_StgFPCVacuum_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜工位平台FPC真空吸", 0, 1);
                }
                return _RightSM_StgFPCVacuum_IOOut;
            }
        }

        private ConfigIOOutEx _RightSM_StgBlowVacuum_IOOut;
        public ConfigIOOutEx RightSM_StgBlowVacuum_IOOutEx
        {
            get
            {
                if (_RightSM_StgBlowVacuum_IOOut == null)
                {
                    _RightSM_StgBlowVacuum_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜工位平台破真空", 0, 1);
                }
                return _RightSM_StgBlowVacuum_IOOut;
            }
        }

        private ConfigIOOutEx _RightSM_StgReduceVacuum_IOOut;
        public ConfigIOOutEx RightSM_StgReduceVacuum_IOOutEx
        {
            get
            {
                if (_RightSM_StgReduceVacuum_IOOut == null)
                {
                    _RightSM_StgReduceVacuum_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜工位平台泄真空", 0, 1);
                }
                return _RightSM_StgReduceVacuum_IOOut;
            }
        }

        private ConfigIOOutEx _RightSM_LRCylinder_IOOut;
        public ConfigIOOutEx RightSM_LRCylinder_IOOutEx
        {
            get
            {
                if (_RightSM_LRCylinder_IOOut == null)
                {
                    _RightSM_LRCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜工位左右气缸电磁阀", 0, 1);
                }
                return _RightSM_LRCylinder_IOOut;
            }
        }

        private ConfigIOOutEx _RightSM_FBCylinder_IOOut;
        public ConfigIOOutEx RightSM_FBCylinder_IOOutEx
        {
            get
            {
                if (_RightSM_FBCylinder_IOOut == null)
                {
                    _RightSM_FBCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜工位前后气缸电磁阀", 0, 1);
                }
                return _RightSM_FBCylinder_IOOut;
            }
        }


        private ConfigIOOutEx _RightSM_UDCylinder_IOOut;
        public ConfigIOOutEx RightSM_UDCylinder_IOOutEx
        {
            get
            {
                if (_RightSM_UDCylinder_IOOut == null)
                {
                    _RightSM_UDCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜工位上下气缸电磁阀", 0, 1);
                }
                return _RightSM_UDCylinder_IOOut;
            }
        }

        private ConfigIOOutEx _RightSM_GlueCylinder_IOOut;
        public ConfigIOOutEx RightSM_GlueCylinder_IOOutEx
        {
            get
            {
                if (_RightSM_GlueCylinder_IOOut == null)
                {
                    _RightSM_GlueCylinder_IOOut = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜工位胶带上下气缸电磁阀", 0, 1);
                }
                return _RightSM_GlueCylinder_IOOut;
            }
        }

        private ConfigIOOut _RightSM_RollerCylinder_IOOut;
        public ConfigIOOut RightSM_RollerCylinder_IOOut
        {
            get
            {
                if (_RightSM_RollerCylinder_IOOut == null)
                {
                    _RightSM_RollerCylinder_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左撕膜工位滚轮气缸电磁阀", 0);
                }
                return _RightSM_RollerCylinder_IOOut;
            }
        }


        private ConfigIOOut _RightSM_GlueLockCylinder_IOOut;
        public ConfigIOOut RightSM_GlueLockCylinder_IOOut
        {
            get
            {
                if (_RightSM_GlueLockCylinder_IOOut == null)
                {
                    _RightSM_GlueLockCylinder_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "右撕膜工位胶带气涨轴电磁阀", 0);
                }
                return _RightSM_GlueLockCylinder_IOOut;
            }
        }
        #endregion

        #region //左折弯工位输出
        private ConfigIOOut _LeftBend_SuckVacuum_IOOut;
        public ConfigIOOut LeftBend_SuckVacuum_IOOut
        {
            get
            {
                if (_LeftBend_SuckVacuum_IOOut == null)
                {
                    _LeftBend_SuckVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左折弯平台真空吸电磁阀", 0);
                }
                return _LeftBend_SuckVacuum_IOOut;
            }
        }

        private ConfigIOOut _LeftBend_BlowVacuum_IOOut;
        public ConfigIOOut LeftBend_BlowVacuum_IOOut
        {
            get
            {
                if (_LeftBend_BlowVacuum_IOOut == null)
                {
                    _LeftBend_BlowVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左折弯平台破真空电磁阀", 0);
                }
                return _LeftBend_BlowVacuum_IOOut;
            }
        }

        private ConfigIOOut _LeftBend_ClawCylinderOut_IOOut;
        public ConfigIOOut LeftBend_ClawCylinderOut_IOOut
        {
            get
            {
                if (_LeftBend_ClawCylinderOut_IOOut == null)
                {
                    _LeftBend_ClawCylinderOut_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左折弯翻转爪子出电磁阀", 0);
                }
                return _LeftBend_ClawCylinderOut_IOOut;
            }
        }

        private ConfigIOOut _LeftBend_ClawCylinderBack_IOOut;
        public ConfigIOOut LeftBend_ClawCylinderBack_IOOut
        {
            get
            {
                if (_LeftBend_ClawCylinderBack_IOOut == null)
                {
                    _LeftBend_ClawCylinderBack_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左折弯翻转爪子回电磁阀", 0);
                }
                return _LeftBend_ClawCylinderBack_IOOut;
            }
        }

        private ConfigIOOut _LeftBend_RightOPTUD_IOOut;
        public ConfigIOOut LeftBend_RightOPTUD_IOOut
        {
            get
            {
                if (_LeftBend_RightOPTUD_IOOut == null)
                {
                    _LeftBend_RightOPTUD_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左折弯右光源上下气缸电磁阀", 0);
                }
                return _LeftBend_RightOPTUD_IOOut;
            }
        }

        private ConfigIOOut _LeftBend_PressCylinderDown_IOOut;
        public ConfigIOOut LeftBend_PressCylinderDown_IOOut
        {
            get
            {
                if (_LeftBend_PressCylinderDown_IOOut == null)
                {
                    _LeftBend_PressCylinderDown_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左折弯压合上下气缸下电磁阀", 0);
                }
                return _LeftBend_PressCylinderDown_IOOut;
            }
        }


        private ConfigIOOut _LeftBend_PressCylinderUp_IOOut;
        public ConfigIOOut LeftBend_PressCylinderUp_IOOut
        {
            get
            {
                if (_LeftBend_PressCylinderUp_IOOut == null)
                {
                    _LeftBend_PressCylinderUp_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "左折弯压合上下气缸上电磁阀", 0);
                }
                return _LeftBend_PressCylinderUp_IOOut;
            }
        }





        #endregion

        #region //中折弯工位输出
        private ConfigIOOut _MidBend_SuckVacuum_IOOut;
        public ConfigIOOut MidBend_SuckVacuum_IOOut
        {
            get
            {
                if (_MidBend_SuckVacuum_IOOut == null)
                {
                    _MidBend_SuckVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中折弯平台真空吸电磁阀", 0);
                }
                return _MidBend_SuckVacuum_IOOut;
            }
        }

        private ConfigIOOut _MidBend_BlowVacuum_IOOut;
        public ConfigIOOut MidBend_BlowVacuum_IOOut
        {
            get
            {
                if (_MidBend_BlowVacuum_IOOut == null)
                {
                    _MidBend_BlowVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中折弯平台破真空电磁阀", 0);
                }
                return _MidBend_BlowVacuum_IOOut;
            }
        }

        private ConfigIOOut _MidBend_ClawCylinderOut_IOOut;
        public ConfigIOOut MidBend_ClawCylinderOut_IOOut
        {
            get
            {
                if (_MidBend_ClawCylinderOut_IOOut == null)
                {
                    _MidBend_ClawCylinderOut_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中折弯翻转爪子出电磁阀", 0);
                }
                return _MidBend_ClawCylinderOut_IOOut;
            }
        }


        private ConfigIOOut _MidBend_ClawCylinderBack_IOOut;
        public ConfigIOOut MidBend_ClawCylinderBack_IOOut
        {
            get
            {
                if (_MidBend_ClawCylinderBack_IOOut == null)
                {
                    _MidBend_ClawCylinderBack_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中折弯翻转爪子回电磁阀", 0);
                }
                return _MidBend_ClawCylinderBack_IOOut;
            }
        }



        private ConfigIOOut _MidBend_PressCylinderDown_IOOut;
        public ConfigIOOut MidBend_PressCylinderDown_IOOut
        {
            get
            {
                if (_MidBend_PressCylinderDown_IOOut == null)
                {
                    _MidBend_PressCylinderDown_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中折弯压合上下气缸下电磁阀", 0);
                }
                return _MidBend_PressCylinderDown_IOOut;
            }
        }


        private ConfigIOOut _MidBend_PressCylinderUp_IOOut;
        public ConfigIOOut MidBend_PressCylinderUp_IOOut
        {
            get
            {
                if (_MidBend_PressCylinderUp_IOOut == null)
                {
                    _MidBend_PressCylinderUp_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "中折弯压合上下气缸上电磁阀", 0);
                }
                return _MidBend_PressCylinderUp_IOOut;
            }
        }



        #endregion

        #region //右折弯工位输出
        private ConfigIOOut _RightBend_SuckVacuum_IOOut;
        public ConfigIOOut RightBend_SuckVacuum_IOOut
        {
            get
            {
                if (_RightBend_SuckVacuum_IOOut == null)
                {
                    _RightBend_SuckVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "右折弯平台真空吸电磁阀", 0);
                }
                return _RightBend_SuckVacuum_IOOut;
            }
        }

        private ConfigIOOut _RightBend_BlowVacuum_IOOut;
        public ConfigIOOut RightBend_BlowVacuum_IOOut
        {
            get
            {
                if (_RightBend_BlowVacuum_IOOut == null)
                {
                    _RightBend_BlowVacuum_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "右折弯平台破真空电磁阀", 0);
                }
                return _RightBend_BlowVacuum_IOOut;
            }
        }

        private ConfigIOOut _RightBend_ClawCylinderOut_IOOut;
        public ConfigIOOut RightBend_ClawCylinderOut_IOOut
        {
            get
            {
                if (_RightBend_ClawCylinderOut_IOOut == null)
                {
                    _RightBend_ClawCylinderOut_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "右折弯翻转爪子出电磁阀", 0);
                }
                return _RightBend_ClawCylinderOut_IOOut;
            }
        }

        private ConfigIOOut _RightBend_ClawCylinderBack_IOOut;
        public ConfigIOOut RightBend_ClawCylinderBack_IOOut
        {
            get
            {
                if (_RightBend_ClawCylinderBack_IOOut == null)
                {
                    _RightBend_ClawCylinderBack_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "右折弯翻转爪子回电磁阀", 0);
                }
                return _RightBend_ClawCylinderBack_IOOut;
            }
        }







        private ConfigIOOut _RightBend_PressCylinderDown_IOOut;
        public ConfigIOOut RightBend_PressCylinderDown_IOOut
        {
            get
            {
                if (_RightBend_PressCylinderDown_IOOut == null)
                {
                    _RightBend_PressCylinderDown_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "右折弯压合上下气缸下电磁阀", 0);
                }
                return _RightBend_PressCylinderDown_IOOut;
            }
        }

        private ConfigIOOut _RightBend_PressCylinderUp_IOOut;
        public ConfigIOOut RightBend_PressCylinderUp_IOOut
        {
            get
            {
                if (_RightBend_PressCylinderUp_IOOut == null)
                {
                    _RightBend_PressCylinderUp_IOOut = new ConfigIOOut(CardIDs.None, -1, false, "右折弯压合上下气缸上电磁阀", 0);
                }
                return _RightBend_PressCylinderUp_IOOut;
            }
        }



        #endregion

        #region 中转输出
        private ConfigIOOutEx _TransferCylinderUp_IOOutEx;

        public ConfigIOOutEx TransferCylinderUp_IOOutEx
        {
            get
            {
                if (_TransferCylinderUp_IOOutEx == null)
                {
                    _TransferCylinderUp_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "中转搬运上下气缸上电磁阀", 0, 1);
                }
                return _TransferCylinderUp_IOOutEx;
            }
        }


        private ConfigIOOutEx _TransferCylinderDown_IOOutEx;

        public ConfigIOOutEx TransferCylinderDown_IOOutEx
        {
            get
            {
                if (_TransferCylinderDown_IOOutEx == null)
                {
                    _TransferCylinderDown_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "中转搬运上下气缸下电磁阀", 0, 1);
                }
                return _TransferCylinderDown_IOOutEx;
            }
        }




        #endregion 



        #region //outex
        private ConfigIOOutEx _Left_WServoOn_IOOutEx;

        public ConfigIOOutEx Left_WServoOn_IOOutEx
        {
            get
            {
                if (_Left_WServoOn_IOOutEx == null)
                {
                    _Left_WServoOn_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "左撕膜W轴伺服使能", 0, 1);
                }
                if (_Left_WServoOn_IOOutEx.Delay == 0)
                {
                    _Left_WServoOn_IOOutEx.Delay = 200;
                }
                return _Left_WServoOn_IOOutEx;
            }
        }

        private ConfigIOOutEx _Mid_WServoOn_IOOutEx;

        public ConfigIOOutEx Mid_WServoOn_IOOutEx
        {
            get
            {
                if (_Mid_WServoOn_IOOutEx == null)
                {
                    _Mid_WServoOn_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜W轴伺服使能", 0, 1);
                }
                if (_Mid_WServoOn_IOOutEx.Delay == 0)
                {
                    _Mid_WServoOn_IOOutEx.Delay = 200;
                }
                return _Mid_WServoOn_IOOutEx;
            }
        }

        private ConfigIOOutEx _Right_WServoOn_IOOutEx;

        public ConfigIOOutEx Right_WServoOn_IOOutEx
        {
            get
            {
                if (_Right_WServoOn_IOOutEx == null)
                {
                    _Right_WServoOn_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜W轴伺服使能", 0, 1);
                }
                if (_Right_WServoOn_IOOutEx.Delay == 0)
                {
                    _Right_WServoOn_IOOutEx.Delay = 200;
                }
                return _Right_WServoOn_IOOutEx;
            }
        }

        private ConfigIOOutEx _SM_AOIServoOn_IOOutEx;

        public ConfigIOOutEx SM_AOIServoOn_IOOutEx
        {
            get
            {
                if (_SM_AOIServoOn_IOOutEx == null)
                {
                    _SM_AOIServoOn_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "撕膜检测X轴伺服使能", 0, 1);
                }
                if (_SM_AOIServoOn_IOOutEx.Delay == 0)
                {
                    _SM_AOIServoOn_IOOutEx.Delay = 200;
                }
                return _SM_AOIServoOn_IOOutEx;
            }
        }
        private ConfigIOOutEx _DischargZServoOn_IOOutEx;

        public ConfigIOOutEx DischargZServoOn_IOOutEx
        {
            get
            {
                if (_DischargZServoOn_IOOutEx == null)
                {
                    _DischargZServoOn_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "出料工位Z轴伺服使能", 0, 1);
                }
                if (_DischargZServoOn_IOOutEx.Delay == 0)
                {
                    _DischargZServoOn_IOOutEx.Delay = 200;
                }
                return _DischargZServoOn_IOOutEx;
            }
        }
        private ConfigIOOutEx _DischargZbrak_IOOutEx;

        public ConfigIOOutEx DischargZbrak_IOOutEx
        {
            get
            {
                if (_DischargZbrak_IOOutEx == null)
                {
                    _DischargZbrak_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "出料Z轴刹车中继", 0, 1);
                }
                if (_DischargZbrak_IOOutEx.Delay == 0)
                {
                    _DischargZbrak_IOOutEx.Delay = 200;
                }
                return _DischargZbrak_IOOutEx;
            }
        }
        private ConfigIOOutEx _DischargZClearAlarm_IOOutEx;

        public ConfigIOOutEx DischargZClearAlarm_IOOutEx
        {
            get
            {
                if (_DischargZClearAlarm_IOOutEx == null)
                {
                    _DischargZClearAlarm_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "出料Z轴伺服报警清除", 0, 1);
                }
                if (_DischargZClearAlarm_IOOutEx.Delay == 0)
                {
                    _DischargZClearAlarm_IOOutEx.Delay = 200;
                }
                return _DischargZClearAlarm_IOOutEx;
            }
        }




        private ConfigIOOutEx _SMAOIClearAlarm_IOOutEx;

        public ConfigIOOutEx SMAOIClearAlarm_IOOutEx
        {
            get
            {
                if (_SMAOIClearAlarm_IOOutEx == null)
                {
                    _SMAOIClearAlarm_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "撕膜工位：撕膜检测X轴伺服报警清除", 0, 1);
                }
                if (_SMAOIClearAlarm_IOOutEx.Delay == 0)
                {
                    _SMAOIClearAlarm_IOOutEx.Delay = 200;
                }
                return _SMAOIClearAlarm_IOOutEx;
            }
        }



        private ConfigIOOutEx _AllStepMotorServoOn_IOOutEx;
        public ConfigIOOutEx AllStepMotorServoOn_IOOutEx
        {
            get
            {
                if (_AllStepMotorServoOn_IOOutEx == null)
                {
                    _AllStepMotorServoOn_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "所有步进使能", 0, 1);
                }
                if (_AllStepMotorServoOn_IOOutEx.Delay == 0)
                {
                    _AllStepMotorServoOn_IOOutEx.Delay = 200;
                }
                return _AllStepMotorServoOn_IOOutEx;
            }
        }



        private ConfigIOOutEx _OutEx5;

        public ConfigIOOutEx OutEx5
        {
            get
            {
                if (_OutEx5 == null)
                {
                    _OutEx5 = new ConfigIOOutEx(CardIDs.None, -1, false, "####", 0, 1);
                }
                if (_OutEx5.Delay == 0)
                {
                    _OutEx5.Delay = 200;
                }
                return _OutEx5;
            }
        }

        private ConfigIOOutEx _Left_ClearWAlarm_IOOutEx;

        public ConfigIOOutEx Left_ClearWAlarm_IOOutEx
        {
            get
            {
                if (_Left_ClearWAlarm_IOOutEx == null)
                {
                    _Left_ClearWAlarm_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "左撕膜W轴伺服报警清除", 0, 1);
                }
                if (_Left_ClearWAlarm_IOOutEx.Delay == 0)
                {
                    _Left_ClearWAlarm_IOOutEx.Delay = 200;
                }
                return _Left_ClearWAlarm_IOOutEx;
            }
        }

        private ConfigIOOutEx _Mid_ClearWAlarm_IOOutEx;

        public ConfigIOOutEx Mid_ClearWAlarm_IOOutEx
        {
            get
            {
                if (_Mid_ClearWAlarm_IOOutEx == null)
                {
                    _Mid_ClearWAlarm_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "中撕膜W轴伺服报警清除", 0, 1);
                }
                if (_Mid_ClearWAlarm_IOOutEx.Delay == 0)
                {
                    _Mid_ClearWAlarm_IOOutEx.Delay = 200;
                }
                return _Mid_ClearWAlarm_IOOutEx;
            }
        }

        private ConfigIOOutEx _Right_ClearWAlarm_IOOutEx;

        public ConfigIOOutEx Right_ClearWAlarm_IOOutEx
        {
            get
            {
                if (_Right_ClearWAlarm_IOOutEx == null)
                {
                    _Right_ClearWAlarm_IOOutEx = new ConfigIOOutEx(CardIDs.None, -1, false, "右撕膜W轴伺服报警清除", 0, 1);
                }
                if (_Right_ClearWAlarm_IOOutEx.Delay == 0)
                {
                    _Right_ClearWAlarm_IOOutEx.Delay = 200;
                }
                return _Right_ClearWAlarm_IOOutEx;
            }
        }

        private ConfigIOOutEx _OutEx10;

        public ConfigIOOutEx OutEx10
        {
            get
            {
                if (_OutEx10 == null)
                {
                    _OutEx10 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_OutEx10.Delay == 0)
                {
                    _OutEx10.Delay = 200;
                }
                return _OutEx10;
            }
        }

        private ConfigIOOutEx _OutEx11;

        public ConfigIOOutEx OutEx11
        {
            get
            {
                if (_OutEx11 == null)
                {
                    _OutEx11 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_OutEx11.Delay == 0)
                {
                    _OutEx11.Delay = 200;
                }
                return _OutEx11;
            }
        }

        private ConfigIOOutEx _OutEx12;

        public ConfigIOOutEx OutEx12
        {
            get
            {
                if (_OutEx12 == null)
                {
                    _OutEx12 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_OutEx12.Delay == 0)
                {
                    _OutEx12.Delay = 200;
                }
                return _OutEx12;
            }
        }

        private ConfigIOOutEx _OutEx13;

        public ConfigIOOutEx OutEx13
        {
            get
            {
                if (_OutEx13 == null)
                {
                    _OutEx13 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_OutEx13.Delay == 0)
                {
                    _OutEx13.Delay = 200;
                }
                return _OutEx13;
            }
        }

        private ConfigIOOutEx _OutEx14;

        public ConfigIOOutEx OutEx14
        {
            get
            {
                if (_OutEx14 == null)
                {
                    _OutEx14 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_OutEx14.Delay == 0)
                {
                    _OutEx14.Delay = 200;
                }
                return _OutEx14;
            }
        }

        private ConfigIOOutEx _OutEx15;

        public ConfigIOOutEx OutEx15
        {
            get
            {
                if (_OutEx15 == null)
                {
                    _OutEx15 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_OutEx15.Delay == 0)
                {
                    _OutEx15.Delay = 200;
                }
                return _OutEx15;
            }
        }

        private ConfigIOOutEx _OutEx16;

        public ConfigIOOutEx OutEx16
        {
            get
            {
                if (_OutEx16 == null)
                {
                    _OutEx16 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_OutEx16.Delay == 0)
                {
                    _OutEx16.Delay = 200;
                }
                return _OutEx16;
            }
        }


        private ConfigIOOutEx _CardCOutEx16;

        public ConfigIOOutEx CardCOutEx16
        {
            get
            {
                if (_CardCOutEx16 == null)
                {
                    _CardCOutEx16 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_CardCOutEx16.Delay == 0)
                {
                    _CardCOutEx16.Delay = 200;
                }
                return _CardCOutEx16;
            }
        }


        private ConfigIOOutEx _CardCOutEx15;

        public ConfigIOOutEx CardCOutEx15
        {
            get
            {
                if (_CardCOutEx15 == null)
                {
                    _CardCOutEx15 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_CardCOutEx15.Delay == 0)
                {
                    _CardCOutEx15.Delay = 200;
                }
                return _CardCOutEx15;
            }
        }



        private ConfigIOOutEx _CardCOutEx14;

        public ConfigIOOutEx CardCOutEx14
        {
            get
            {
                if (_CardCOutEx14 == null)
                {
                    _CardCOutEx14 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_CardCOutEx14.Delay == 0)
                {
                    _CardCOutEx14.Delay = 200;
                }
                return _CardCOutEx14;
            }
        }


        private ConfigIOOutEx _CardCOutEx13;

        public ConfigIOOutEx CardCOutEx13
        {
            get
            {
                if (_CardCOutEx13 == null)
                {
                    _CardCOutEx13 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_CardCOutEx13.Delay == 0)
                {
                    _CardCOutEx13.Delay = 200;
                }
                return _CardCOutEx13;
            }
        }
        private ConfigIOOutEx _CardCOutEx100;
        /// <summary>
        /// 备用Ex
        /// </summary>

        public ConfigIOOutEx CardCOutEx100
        {
            get
            {
                if (_CardCOutEx100 == null)
                {
                    _CardCOutEx100 = new ConfigIOOutEx(CardIDs.None, -1, false, "备用", 0, 1);
                }
                if (_CardCOutEx100.Delay == 0)
                {
                    _CardCOutEx100.Delay = 200;
                }
                return _CardCOutEx100;
            }
        }
        #endregion

        private ConfigIOIn _EStopBtnIOIn;
        public ConfigIOIn EStopIOIn
        {
            get
            {
                if (_EStopBtnIOIn == null)
                {
                    _EStopBtnIOIn = new ConfigIOIn(CardIDs.None, -1, false, "急停按钮");
                }
                return _EStopBtnIOIn;
            }
        }

        private ConfigIOOut _IOOut15;
        public ConfigIOOut IOOut15
        {
            get
            {
                if (_IOOut15 == null)
                {
                    _IOOut15 = new ConfigIOOut(CardIDs.None, -1, false, "备用", 0);
                }
                return _IOOut15;
            }
        }



        private ConfigIOOut _IOOut14;
        public ConfigIOOut IOOut14
        {
            get
            {
                if (_IOOut14 == null)
                {
                    _IOOut14 = new ConfigIOOut(CardIDs.None, -1, false, "备用", 0);
                }
                return _IOOut14;
            }
        }



        private ConfigIOOut _IOOut13;
        public ConfigIOOut IOOut13
        {
            get
            {
                if (_IOOut13 == null)
                {
                    _IOOut13 = new ConfigIOOut(CardIDs.None, -1, false, "备用", 0);
                }
                return _IOOut13;
            }
        }


        private ConfigIOOut _IOOut12;
        public ConfigIOOut IOOut12
        {
            get
            {
                if (_IOOut12 == null)
                {
                    _IOOut12 = new ConfigIOOut(CardIDs.None, -1, false, "备用", 0);
                }
                return _IOOut12;
            }
        }


        private ConfigIOOut _IOOut11;
        public ConfigIOOut IOOut11
        {
            get
            {
                if (_IOOut11 == null)
                {
                    _IOOut11 = new ConfigIOOut(CardIDs.None, -1, false, "备用", 0);
                }
                return _IOOut11;
            }
        }


        private ConfigIOOut _IOOut10;
        public ConfigIOOut IOOut10
        {
            get
            {
                if (_IOOut10 == null)
                {
                    _IOOut10 = new ConfigIOOut(CardIDs.None, -1, false, "备用", 0);
                }
                return _IOOut10;
            }
        }
        private ConfigIOOut _IOOut100;
        /// <summary>
        /// 备用
        /// </summary>
        public ConfigIOOut IOOut100
        {
            get
            {
                if (_IOOut100 == null)
                {
                    _IOOut100 = new ConfigIOOut(CardIDs.None, -1, false, "备用", 0);
                }
                return _IOOut100;
            }
        }
        #endregion


        [Serializable]
        public class CalibrationParameters
        {


            private XYZUVPoint _cirCamOrg;
            private XYZUVPoint _sideCamOrg;
            private XYZUVPoint _sidevalvOrg;
            private XYZUVPoint _sideGap;
            private XYZUVPoint _cirvalvOrg;


            private XYZPoint _CamCalOrg;

            private XYZPoint _PointLaserGap;

            private XYZPoint _LineLaserCalPos;

            private List<XYZPoint> _VermesSideGapValue;

            private List<XYZPoint> _CalBasePts;  //记录ABCD喷阀位置

            private double _LaserPosBBaseValue = 30;

            public double LaserPosbBaseValue
            {
                get
                {
                    return _LaserPosBBaseValue;
                }
                set
                {
                    _LaserPosBBaseValue = value;
                }
            }


            public XYZUVPoint CirCamOrg
            {
                get
                {
                    if (_cirCamOrg == null)
                    {
                        _cirCamOrg = new XYZUVPoint();
                    }
                    return _cirCamOrg;
                }
            }


            public XYZUVPoint SideCamOrg
            {
                get
                {
                    if (_sideCamOrg == null)
                    {
                        _sideCamOrg = new XYZUVPoint();
                    }
                    return _sideCamOrg;
                }
            }

            public XYZUVPoint SideValvOrg
            {
                get
                {
                    if (_sidevalvOrg == null)
                    {
                        _sidevalvOrg = new XYZUVPoint();
                    }
                    return _sidevalvOrg;
                }
            }


            public XYZUVPoint CirValvOrg
            {
                get
                {
                    if (_cirvalvOrg == null)
                    {
                        _cirvalvOrg = new XYZUVPoint();
                    }
                    return _cirvalvOrg;
                }
            }


            public XYZUVPoint SideGap
            {
                get
                {
                    if (_sideGap == null)
                    {
                        _sideGap = new XYZUVPoint();
                    }
                    return _sideGap;
                }
            }



            public XYZPoint CamCalOrg
            {
                get
                {
                    if (_CamCalOrg == null)
                    {
                        _CamCalOrg = new XYZPoint();
                    }
                    return _CamCalOrg;
                }
            }

            public XYZPoint PointLaserGap
            {
                get
                {
                    if (_PointLaserGap == null)
                    {
                        _PointLaserGap = new XYZPoint();
                    }
                    return _PointLaserGap;
                }
            }

            public XYZPoint LineLaserCalPos
            {
                get
                {
                    if (_LineLaserCalPos == null)
                    {
                        _LineLaserCalPos = new XYZPoint();
                    }
                    return _LineLaserCalPos;
                }
            }

            public List<XYZPoint> VermesSideGapValue
            {
                get
                {
                    if (_VermesSideGapValue == null)
                    {
                        _VermesSideGapValue = new List<XYZPoint>();
                    }
                    if (_VermesSideGapValue.Count < 4)
                    {
                        for (int i = _VermesSideGapValue.Count; i < 4; i++)
                        {
                            _VermesSideGapValue.Add(new XYZPoint());
                        }
                    }
                    return _VermesSideGapValue;
                }
            }

            public List<XYZPoint> CalBasePts
            {
                get
                {
                    if (_CalBasePts == null)
                    {
                        _CalBasePts = new List<XYZPoint>();
                    }
                    if (_CalBasePts.Count < 4)
                    {
                        for (int i = _CalBasePts.Count; i < 4; i++)
                        {
                            _CalBasePts.Add(new XYZPoint());
                        }
                    }
                    return _CalBasePts;
                }
            }

            private XYZPoint _CalCamPtsAORG;

            private XYZPoint _CalCamPtsBORG;

            private XYZPoint _CalPointLaserPts;

            public XYZPoint CalCamPtsAORG
            {
                get
                {
                    if (_CalCamPtsAORG == null)
                    {
                        _CalCamPtsAORG = new XYZPoint();
                    }
                    return _CalCamPtsAORG;
                }
            }

            public XYZPoint CalCamPtsBORG
            {
                get
                {
                    if (_CalCamPtsBORG == null)
                    {
                        _CalCamPtsBORG = new XYZPoint();
                    }
                    return _CalCamPtsBORG;
                }
            }

            public XYZPoint CalPointLaserPts
            {
                get
                {
                    if (_CalPointLaserPts == null)
                    {
                        _CalPointLaserPts = new XYZPoint();
                    }
                    return _CalPointLaserPts;
                }
            }
        }

        public MeasurementConfig()
        {
        }

        public new static MeasurementConfig Load()
        {

            return Load(GetApplicationPath("set\\set.config")) as MeasurementConfig;
            //  return null;
        }

        public override bool Save()
        {
            return Save(GetApplicationPath("set\\set.config"));
        }
    }
}
