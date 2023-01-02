using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZ.CNC.Measurement.Core
{
    [Serializable]
    public class XYZPoint
    {
        private double _X;

        private double _Y;

        private double _Z;

        public double X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }

        public double Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }

        public double Z
        {
            get
            {
                return _Z;
            }
            set
            {
                _Z = value;
            }
        }
    }


    [Serializable]
    public class XYZUVPoint
    {
        double _X;
        double _Y;
        double _Z;
        double _U;
        double _V;

        public double X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }

        public double Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }

        public double Z
        {
            get
            {
                return _Z;
            }
            set
            {
                _Z = value;
            }
        }


        public double U
        {
            get
            {
                return _U;
            }
            set
            {
                _U = value;
            }
        }

        public double V
        {
            get
            {
                return _V;
            }
            set
            {
                _V = value;
            }
        }

    }


    [Serializable]
    public class SMLocation
    {
        private double _sm_loadx;
        public double Lsm_loadX
        {
            get
            {
                return _sm_loadx;
            }
            set
            {
                _sm_loadx = value;
            }
        }

        private double _sm_loadY;
        public double Lsm_loadY
        {
            get
            {
                return _sm_loadY;
            }
            set
            {
                _sm_loadY = value;
            }
        }

        private double _sm_loadz;
        public double Lsm_LoadZ
        {
            get
            {
                return _sm_loadz;
            }
            set
            {
                _sm_loadz = value;
            }
        }


        private double  _sm_pressx;
        public double  Lsm_PressX
        {

            get
            {
                return _sm_pressx;
            } 
            set
            {
                _sm_pressx = value;
            }
                               
        }


        private double _sm_pressy;
        public double Lsm_PressY
        {
            get
            {
                return _sm_pressy;
            }
            set
            {
                _sm_pressy = value;
            }
        }

        private double _sm_pressz;
        public double Lsm_PressZ
        {
            get
            {
                return _sm_pressz;
            }
            set
            {
                _sm_pressz = value;
            }
        }

        private double _sm_dischargex;
        public double Lsm_DischargeX
        {
            get
            {
                return _sm_dischargex;
            }
            set
            {
                _sm_dischargex = value;
            }
        }


        private double _sm_dischargeY;
        public double Lsm_DischargeY
        {
            get
            {
                return _sm_dischargeY;
            }
            set
            {
                _sm_dischargeY = value;
            }
        }

        private double _sm_dischargeZ;
        public double Lsm_DischargeZ
        {
            get
            {
                return _sm_dischargeZ;
            }
            set
            {
                _sm_dischargeZ = value;
            }
        }

        private double _sm_CCDY;
        public double Lsm_CCDY
        {
            get
            {
                return _sm_CCDY;
            }
            set
            {
                _sm_CCDY = value;
            }
        }

        private double _sm_CCDX;
        public double Lsm_CCDX
        {
            get
            {
                return _sm_CCDX;
            }
            set
            {
                _sm_CCDX = value;
            }
        }

        private double _sm_waitX;
        public double Lsm_WaitX
        {
            get
            {
                return _sm_waitX;
            }
            set
            {
                _sm_waitX = value;
            }
        }

        private double _sm_waitY;
        public double Lsm_WaitY
        {
            get
            {
                return _sm_waitY;
            }
            set
            {
                _sm_waitY = value;
            }
        }

        private double _sm_WaitZ;
        public double Lsm_WaitZ
        {
            get
            {
                return _sm_WaitZ;
            }
            set
            {
                _sm_WaitZ = value;
            }
        }

        private double _sm_Xspeed;
        public double SM_XSpeed
        {
            get
            {
                return _sm_Xspeed;
            }
            set
            {
                _sm_Xspeed = value;
            }
        }


        private double _sm_Yspeed;
        public double SM_YSpeed
        {
            get
            {
                return _sm_Yspeed;
            }
            set
            {
                _sm_Yspeed = value;
            }
        }


        private double _sm_Zspeed;
        public double SM_ZSpeed
        {
            get
            {
                return _sm_Zspeed;
            }
            set
            {
                _sm_Zspeed = value;
            }
        }

        private double _sm_Zdist;
        public double SM_ZDist
        {
            get
            {
                return _sm_Zdist;
            }
            set
            {
                _sm_Zdist = value;
            }
        }




        private double _sm_Zdist_Speed;
        public double SM_ZDist_Speed
        {
            get
            {
                return _sm_Zdist_Speed;
            }
            set
            {
                _sm_Zdist_Speed = value;
            }
        }

        private  double  _pastespeed;

        public double Paste_Speed
        {
            get
            {
                return _pastespeed;
            }
            set
            {
                _pastespeed = value;
            }
        }


    }


    [Serializable]

    public class BendParameter
    {
        private double _BaseRate;
        public  double BaseRate
        {
            get
            {
                return _BaseRate;
            }
            set
            {
                _BaseRate = value;
            }
        }

        private double _Errand;
        public double ErrAnd
        {
            get
            {
                return _Errand;
            }
            set
            {
                _Errand = value;
            }
        }

        private double _Zone1Low;
        public double Zone1Low
        {
            get
            {
                return _Zone1Low;
            }
            set
            {
                _Zone1Low = value;
            }
        }

        private double _Zone1Up;

        public double Zone1Up
        {
            get
            {
                return _Zone1Up;
            }
            set
            {
                _Zone1Up = value;
            }
        }

        private double _Rate1;
        public double Rate1
        {
            get
            {
                return _Rate1;
            }
            set
            {
                _Rate1 = value;
            }
        }

        private double _Zone2Low;
        public double Zone2Low
        {
            get
            {
                return _Zone2Low;
            }
            set
            {
                _Zone2Low = value;
            }
        }

        private double _Zone2Up;

        public double Zone2Up
        {
            get
            {
                return _Zone2Up;
            }
            set
            {
                _Zone2Up = value;
            }
        }

        private double _Rate2;
        public double Rate2
        {
            get
            {
                return _Rate2;
            }
            set
            {
                _Rate2 = value;
            }
        }

        private double _Zone3Low;
        public double Zone3Low
        {
            get
            {
                return _Zone3Low;
            }
            set
            {
                _Zone3Low = value;
            }
        }

        private double _Zone3Up;
        public double Zone3Up
        {
            get
            {
                return _Zone3Up;
            }
            set
            {
                _Zone3Up = value;
            }
        }


        private double _Rate3;
        public double Rate3
        {
            get
            {
                return _Rate3;
            }
            set
            {
                _Rate3 = value;
            }
        }

        private  double _adj_Num;
        public double Adj_Num
        {
            get
            {
                return _adj_Num;
            }
            set
            {
                _adj_Num = value;
            }
        }


        private bool _bDirection;
        public bool Bdirection
        {
            get
            {
                return _bDirection;
            }
            set
            {
                _bDirection = value;
            }
        }

        private int _DirValue;

        public int DirValue
        {
            get
            {
                return _DirValue;
            }
            set
            {
                _DirValue = value;
            }
        }


        private double _AOIOffset;
        public double AOIOffset
        {
            get
            {
                return _AOIOffset;
            }
            set
            {
                _AOIOffset = value;
            }
        }


        private double _AOIBase;
        public double AOIBase
        {
            get
            {
                return _AOIBase;
            }
            set
            {
                _AOIBase = value;
            }
        }


    }
}
