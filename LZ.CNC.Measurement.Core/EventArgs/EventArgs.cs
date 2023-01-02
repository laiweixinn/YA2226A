using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DY.CNC.Core;

namespace LZ.CNC.Measurement.Core.Events
{
    public class IOStatusChangedEventArgs : EventArgs
    {
        private CardIDs _CardID = CardIDs.None;

        private int _Index = 0;

        private bool _Status = false;

        public CardIDs CardID
        {
            get
            {
                return _CardID;
            }
        }

        public int Index
        {
            get
            {
                return _Index;
            }
        }

        public bool Status
        {
            get
            {
                return _Status;
            }
        }

        public IOStatusChangedEventArgs(CardIDs cardId, int index, bool status)
        {
            _CardID = cardId;
            _Index = index;
            _Status = status;
        }
    }

    public class MessageOutputEventArgs : EventArgs
    {
        private string _Message;

        private bool _IsError;

        public string Message
        {
            get
            {
                return _Message;
            }
        }

        public bool IsError
        {
            get
            {
                return _IsError;
            }
        }

        public MessageOutputEventArgs(string msg,bool iserror)
        {
            _Message = msg;
            _IsError = iserror;
        }
    }

    public class MessageShowEventArgs : EventArgs
    {
        private string _Message = null;

        public string Message
        {
            get
            {
                return _Message;
            }
        }

        public MessageShowEventArgs(string msg)
        {
            _Message = msg;
        }
    }

    public class PointChangedEventArgs:EventArgs
    {
        private int _PointNum;

        private double _CamX;

        private double _CamY;

        private bool _CamOK;

        public int PointNum
        {
            get
            {
                return _PointNum;
            }
        }

        public double CamX
        {
            get
            {
                return _CamX;
            }
        }

        public double CamY
        {
            get
            {
                return _CamY;
            }
        }

        public bool CamOK
        {
            get
            {
                return _CamOK;
            }
        }
        
        public PointChangedEventArgs(int num,double X,double Y,bool OK)
        {
            _PointNum = num;
            _CamX = X;
            _CamY = Y;
            _CamOK = OK;
        }






    }


    public class CircleCCDEventArgs : EventArgs
    {
        double _Camx;
        double _Camy;
        bool _IsPZOK;

        public double Camx
        {
            get
            {
                return _Camx;
            }
        }

        public double Camy
        {
            get
            {
                return _Camy;
            }
        }

        public bool IsPZOK
        {
            get
            {
                return _IsPZOK;
            }
        }


        public CircleCCDEventArgs(double revx,double revy,bool ispzok)
        {
            _Camx = revx;
            _Camy = revy;
            _IsPZOK = ispzok;
        }
    }

    public class CoordinateEventArgs : EventArgs
    {
        int _coordenate;
        public int coordenate
        {
            get
            {
                return _coordenate;
            }

        }

        public CoordinateEventArgs(int temp)
        {
            _coordenate = temp;
        }    

    }

    public class SideCCDEventArgs : EventArgs
    {
        double _Camx;
        double _Camy;
        bool _IsPZOK;

        public double Camx
        {
            get
            {
                return _Camx;
            }
        }

        public double Camy
        {
            get
            {
                return _Camy;
            }
        }

        public bool IsPZOK
        {
            get
            {
                return _IsPZOK;
            }
        }


        public SideCCDEventArgs(double revx, double revy, bool ispzok)
        {
            _Camx = revx;
            _Camy = revy;
            _IsPZOK = ispzok;
        }




    }

    public class PointDataArrivedEventArgs : EventArgs
    {
        private double _HighValue;

        public double HighValue
        {
            get
            {
                return _HighValue;
            }
        }

        public PointDataArrivedEventArgs(double value)
        {
            _HighValue = value;
        }
    }

    public class ButtonUpDownEventArgs:EventArgs
    {
        private ButtonTypes _ButtonType;

        public ButtonTypes ButtonType
        {
            get
            {
                return _ButtonType;
            }
        }

        public ButtonUpDownEventArgs(ButtonTypes buttonType)
        {
            _ButtonType = buttonType;
        }
    }

    public class CimInfoChangedEventArgs:EventArgs
    {
        //private MeasurenemtCimSystem _CimInfos;

        //public MeasurenemtCimSystem CimInfos
        //{
        //    get
        //    {
        //        return _CimInfos;
        //    }
        //}

        //public CimInfoChangedEventArgs(MeasurenemtCimSystem cimInfo)
        //{
        //    _CimInfos = cimInfo;
        //}
    }
}
