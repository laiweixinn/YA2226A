using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DY.Core;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Events;
using LZ.CNC.Measurement.Core.Motions;

namespace LZ.CNC.Measurement.Core
{
    public class MeasurementWorkerBase
    {

        #region Define MotionCard
        protected MeasurementMotion _MotionA = new MeasurementMotion(23552, 0);
        protected MeasurementMotion _MotionB = new MeasurementMotion(23552, 1);
        protected MeasurementMotion _MotionC = new MeasurementMotion(23552, 2);
        protected MeasurementMotion _MotionD = new MeasurementMotion(23552, 3);

        
        private MeasurementMotion[] _Motions = null;
        public MeasurementMotion[] Motions
        {
            get
            {
                if (_Motions == null)
                {
                    _Motions = new MeasurementMotion[]
                    {
                        _MotionA,
                        _MotionB,
                        _MotionC,
                        _MotionD
                    };
                }
                return _Motions;
            }
        }


        public MeasurementMotion MotionA
        {
            get
            {
                return _MotionA;
            }
        }

        public MeasurementMotion MotionB
        {
            get
            {
                return _MotionB;
            }
        }

        public MeasurementMotion MotionC
        {
            get
            {
                return _MotionC;
            }
        }

        public MeasurementMotion MotionD
        {
            get
            {
                return _MotionD;
            }
        }
        #endregion

        #region   DEFINE Axies
        protected MeasurementAxis _Axis_LeftSM_X = null;
        public MeasurementAxis Axis_LeftSM_X
        {
            get
            {
                if (_Axis_LeftSM_X == null)
                {
                    _Axis_LeftSM_X = _MotionA.AxisX;
                }
                return _Axis_LeftSM_X;
            }
        }


      protected MeasurementAxis _Axis_LeftSM_Y = null;
        public MeasurementAxis Axis_LeftSM_Y
        {
            get
            {
                if (_Axis_LeftSM_Y == null)
                {
                    _Axis_LeftSM_Y = _MotionA.AxisY;
                }
                return _Axis_LeftSM_Y;
            }
        }


       protected MeasurementAxis _Axis_LeftSM_Z = null;
        public MeasurementAxis Axis_LeftSM_Z
        {
            get
            {
                if (_Axis_LeftSM_Z == null)
                {
                    _Axis_LeftSM_Z = _MotionA.AxisZ;
                }
                return _Axis_LeftSM_Z;
            }
        }



        protected MeasurementAxis _Axis_MidSM_X = null;
        public MeasurementAxis Axis_MidSM_X
        {
            get
            {
                if (_Axis_MidSM_X == null)
                {

                    _Axis_MidSM_X = _MotionA.AxisU;
                }
                return _Axis_MidSM_X;
            }
        }


        protected MeasurementAxis _Axis_MidSM_Y = null;
        public MeasurementAxis Axis_MidSM_Y
        {
            get
            {
                if (_Axis_MidSM_Y == null)
                {
                    _Axis_MidSM_Y = _MotionA.AxisV;
                }
                return _Axis_MidSM_Y;
            }
        }


        protected MeasurementAxis _Axis_MidSM_Z = null;
        public MeasurementAxis Axis_MidSM_Z
        {
            get
            {
                if (_Axis_MidSM_Z == null)
                {
                    _Axis_MidSM_Z = _MotionA.AxisW;
                }
                return _Axis_MidSM_Z;
            }
        }



        protected MeasurementAxis _Axis_RightSM_X = null;
        public MeasurementAxis Axis_RightSM_X
        {
            get
            {
                if (_Axis_RightSM_X == null)
                {
                    _Axis_RightSM_X = _MotionA.AxisA;
                }
                return _Axis_RightSM_X;
            }
        }

        protected MeasurementAxis _Axis_RightSM_Y = null;
        public MeasurementAxis Axis_RightSM_Y
        {
            get
            {
                if (_Axis_RightSM_Y == null)
                {
                    _Axis_RightSM_Y = _MotionA.AxisB;
                }
                return _Axis_RightSM_Y;
            }
        }

       protected MeasurementAxis _Axis_LeftSM_W = null;
        public MeasurementAxis Axis_LeftSM_W
        {
            get
            {
                if (_Axis_LeftSM_W == null)
                {
                    _Axis_LeftSM_W = _MotionA.AxisC;
                }
                return _Axis_LeftSM_W;
            }
        }
        protected MeasurementAxis _Axis_MidSM_W = null;
        public MeasurementAxis Axis_MidSM_W
        {
            get
            {
                if (_Axis_MidSM_W == null)
                {

                    _Axis_MidSM_W = _MotionA.AxisD;
                }
                return _Axis_MidSM_W;
            }
        }

        protected MeasurementAxis _Axis_RightSM_W = null;
        public MeasurementAxis Axis_RightSM_W
        {
            get
            {
                if (_Axis_RightSM_W == null)
                {
                    _Axis_RightSM_W = _MotionA.AxisE;
                }
                return _Axis_RightSM_W;
            }
        }

        protected MeasurementAxis _Axis_Load_Y = null;
        public MeasurementAxis Axis_Load_Y
        {
            get
            {
                if (_Axis_Load_Y == null)
                {
                    _Axis_Load_Y = _MotionA.AxisF;
                }
                return _Axis_Load_Y;
            }
        }






        protected MeasurementAxis _Axis_RightSM_Z = null;
        public MeasurementAxis Axis_RightSM_Z
        {
            get
            {
                if (_Axis_RightSM_Z == null)
                {
                    _Axis_RightSM_Z = _MotionB.AxisX;
                }
                return _Axis_RightSM_Z;
            }
        }


        protected MeasurementAxis _Axis_Load_X = null;
        public MeasurementAxis Axis_Load_X
        {
            get
            {
                if (_Axis_Load_X == null)
                {
                    _Axis_Load_X = _MotionB.AxisY;
                }
                return _Axis_Load_X;
            }
        }

        protected MeasurementAxis _Axis_Load_Z = null;
        public MeasurementAxis Axis_Load_Z
        {
            get
            {
                if (_Axis_Load_Z == null)
                {
                    _Axis_Load_Z = _MotionB.AxisZ;
                }
                return _Axis_Load_Z;
            }
        }


        protected MeasurementAxis _Axis_Transfer_X = null;


        public MeasurementAxis Axis_Transfer_X
        {
            get
            {
                if (_Axis_Transfer_X == null)
                {
                    _Axis_Transfer_X = _MotionB.AxisU;
                }
                return _Axis_Transfer_X;
            }
        }



        protected MeasurementAxis _Axis_Transfer_Z = null;
        public MeasurementAxis Axis_Transfer_Z
        {
            get
            {
                if (_Axis_Transfer_Z == null)
                {
                    _Axis_Transfer_Z = _MotionB.AxisV;
                }
                return _Axis_Transfer_Z;
            }

        }


        protected MeasurementAxis _Axis_LeftBend_CCDX = null;

        public MeasurementAxis Axis_LeftBend_CCDX
        {
            get
            {
                if (_Axis_LeftBend_CCDX == null)
                {
                    _Axis_LeftBend_CCDX = _MotionB.AxisW;
                }
                return _Axis_LeftBend_CCDX;
            }

        }


        protected MeasurementAxis _Axis_LeftBend_DWX = null;
        public MeasurementAxis Axis_LeftBend_DWX
        {
            get
            {
                if (_Axis_LeftBend_DWX == null)
                {
                    _Axis_LeftBend_DWX = _MotionB.AxisA;
                }
                return _Axis_LeftBend_DWX;
            }
        }

        protected MeasurementAxis _Axis_LeftBend_DWY = null;
        public MeasurementAxis Axis_LeftBend_DWY
        {
            get
            {
                if (_Axis_LeftBend_DWY == null)
                {
                    _Axis_LeftBend_DWY = _MotionB.AxisB;
                }
                return _Axis_LeftBend_DWY;
            }
        }

        protected MeasurementAxis _Axis_Discharge_Z = null;
        public MeasurementAxis Axis_Discharge_Z
        {
            get
            {
                if (_Axis_Discharge_Z == null)
                {
                    _Axis_Discharge_Z = _MotionB.AxisC;
                }
                return _Axis_Discharge_Z;
            }
        }


        protected MeasurementAxis _Axis_SMCCD_X = null;
        public MeasurementAxis Axis_SMCCD_X
        {
            get
            {
                if (_Axis_SMCCD_X == null)
                {
                    _Axis_SMCCD_X = _MotionB.AxisD;
                }
                return _Axis_SMCCD_X;
            }
        }



        protected MeasurementAxis _Axis_LeftBend_DWR = null;
        public MeasurementAxis Axis_LeftBend_DWR
        {
            get
            {
                if (_Axis_LeftBend_DWR == null)
                {
                    _Axis_LeftBend_DWR = _MotionC.AxisX;
                }
                return _Axis_LeftBend_DWR;
            }
        }


        protected MeasurementAxis _Axis_LeftBend_DWW = null;
        public MeasurementAxis Axis_LeftBend_DWW
        {
            get
            {
                if (_Axis_LeftBend_DWW == null)
                {
                    _Axis_LeftBend_DWW = _MotionC.AxisY;
                }
                return _Axis_LeftBend_DWW;
            }
        }


        protected MeasurementAxis _Axis_LeftBend_stgY = null;
        public MeasurementAxis Axis_LeftBend_stgY
        {
            get
            {
                if (_Axis_LeftBend_stgY == null)
                {
                    _Axis_LeftBend_stgY = _MotionC.AxisZ;
                }
                return _Axis_LeftBend_stgY;
            }
        }


        protected MeasurementAxis _Axis_MidBend_CCDX = null;
        public MeasurementAxis Axis_MidBend_CCDX
        {
            get
            {
                if (_Axis_MidBend_CCDX == null)
                {
                    _Axis_MidBend_CCDX = _MotionC.AxisU;
                }
                return _Axis_MidBend_CCDX;
            }
        }



        protected MeasurementAxis _Axis_MidBend_DWX = null;
        public MeasurementAxis Axis_MidBend_DWX
        {
            get
            {
                if (_Axis_MidBend_DWX == null)
                {
                    _Axis_MidBend_DWX = _MotionC.AxisV;
                }
                return _Axis_MidBend_DWX;
            }
        }


        protected MeasurementAxis _Axis_MidBend_DWY = null;
        public MeasurementAxis Axis_MidBend_DWY
        {
            get
            {
                if (_Axis_MidBend_DWY == null)
                {
                    _Axis_MidBend_DWY = _MotionC.AxisW;
                }
                return _Axis_MidBend_DWY;
            }
        }


        protected MeasurementAxis _Axis_MidBend_DWR = null;
        public MeasurementAxis Axis_MidBend_DWR
        {
            get
            {
                if (_Axis_MidBend_DWR == null)
                {
                    _Axis_MidBend_DWR = _MotionC.AxisA;
                }
                return _Axis_MidBend_DWR;
            }

        }

        protected MeasurementAxis _Axis_MidBend_DWW = null;
        public MeasurementAxis Axis_MidBend_DWW
        {
            get
            {
                if (_Axis_MidBend_DWW == null)
                {
                    _Axis_MidBend_DWW = _MotionC.AxisB;
                }
                return _Axis_MidBend_DWW;
            }
        }




        protected MeasurementAxis _Axis_MidBend_stgY = null;
        public MeasurementAxis Axis_MidBend_stgY
        {
            get
            {
                if (_Axis_MidBend_stgY == null)
                {
                    _Axis_MidBend_stgY = _MotionD.AxisX;
                }
                return _Axis_MidBend_stgY;
            }
        }


        protected MeasurementAxis _Axis_RightBend_CCDX = null;
        public MeasurementAxis Axis_RightBend_CCDX
        {
            get
            {
                if (_Axis_RightBend_CCDX == null)
                {
                    _Axis_RightBend_CCDX = _MotionD.AxisY;
                }
                return _Axis_RightBend_CCDX;
            }
        }

        protected MeasurementAxis _Axis_RightBend_DWX = null;
        public MeasurementAxis Axis_RightBend_DWX
        {
            get
            {
                if (_Axis_RightBend_DWX == null)
                {
                    _Axis_RightBend_DWX = _MotionD.AxisZ;
                }
                return _Axis_RightBend_DWX;
            }
        }


        protected MeasurementAxis _Axis_RightBend_DWY = null;
        public MeasurementAxis Axis_RightBend_DWY
        {
            get
            {
                if (_Axis_RightBend_DWY == null)
                {
                    _Axis_RightBend_DWY = _MotionD.AxisU;
                }
                return _Axis_RightBend_DWY;
            }
        }


        protected MeasurementAxis _Axis_RightBend_DWR = null;
        public MeasurementAxis Axis_RightBend_DWR
        {
            get
            {
                if (_Axis_RightBend_DWR == null)
                {
                    _Axis_RightBend_DWR = _MotionD.AxisV;
                }
                return _Axis_RightBend_DWR;
            }
        }


        protected MeasurementAxis _Axis_RightBend_DWW = null;
        public MeasurementAxis Axis_RightBend_DWW
        {
            get
            {
                if (_Axis_RightBend_DWW == null)
                {
                    _Axis_RightBend_DWW = _MotionD.AxisW;
                }
                return _Axis_RightBend_DWW;
            }
        }

        protected MeasurementAxis _Axis_RightBend_stgY = null;
        public MeasurementAxis Axis_RightBend_stgY
        {
            get
            {
                if (_Axis_RightBend_stgY == null)
                {
                    _Axis_RightBend_stgY = _MotionD.AxisA;
                }
                return _Axis_RightBend_stgY;
            }
        }

        protected MeasurementAxis _Axis_Discharge_X = null;
        public MeasurementAxis Axis_Discharge_X
        {
            get
            {
                if (_Axis_Discharge_X == null)
                {
                    _Axis_Discharge_X = _MotionD.AxisB;
                }
                return _Axis_Discharge_X;
            }
        }
        #endregion


       


        public   void ShowSplashMessage(string msg)
        {
            SplashScreen.ChangeTitle(msg);
            Thread.Sleep(200);
        }

        public event EventHandler ExButtonDown;

        protected void OnExButtonDown(ButtonTypes buttonTypes)
        {
            if (ExButtonDown!=null)
            {
                ButtonUpDownEventArgs eventArgs = new ButtonUpDownEventArgs(buttonTypes);
                ExButtonDown(this, eventArgs);
            }
        }
               
    }
}
