using DY.CNC.Core;
using DY.CNC.LeadShine.LTDMC.Core;
using DY.CNC.LeadShine.LTDMC.Base;
using System.Threading;
using System;
using System.Collections.Generic;
namespace LZ.CNC.Measurement.Core.Motions
{
    public class MeasurementIOListener:LTDMCIOListener
    {
        private bool _EMG = false;

        private bool[] _SON = null;

        private bool[] _ORG = null;

        private bool[] _ELP = null;

        private bool[] _ELN = null;

        private bool[] _ALM = null;

        private bool[] _IoInStatusEx = null;

        private bool[] _IoOutStatusEx = null;

        public bool EMG
        {
            get
            {
                return _EMG;
            }
        }

        public bool[] SON
        {
            get
            {
                return _SON;
            }
        }

        public bool[] ORG
        {
            get
            {
                return _ORG;
            }
        }

        public bool[] ELP
        {
            get
            {
                return _ELP;
            }
        }

        public bool[] ELN
        {
            get
            {
                return _ELN;
            }
        }

        public bool[] ALM
        {
            get
            {
                return _ALM;
            }
        }

        public bool[] IoInStatusEx
        {
            get
            {
                return _IoInStatusEx;
            }
        }

        public bool[] IoOutStatusEx
        {
            get
            {
                return _IoOutStatusEx;
            }
        }

        public new MeasurementMotion Motion
        {
            get
            {
                return base.Motion as MeasurementMotion;
            }
        }

        public MeasurementIOListener(MeasurementMotion motion):base(motion)
        {
            _SON = new bool[12];
            _ORG = new bool[12];
            _ELP = new bool[12];
            _ELN = new bool[12];
            _ALM = new bool[12];
            _IoInStatusEx = new bool[20];
            _IoOutStatusEx = new bool[20];
            for (int i = 0; i < 12; i++)
            {
                _SON[i] = false;
                _ORG[i] = false;
                _ELP[i] = false;
                _ELN[i] = false;
                _ALM[i] = false;
            }
        }

        protected override bool InitListen()
        {
            if (Motion.IsLinked)
            {
                Motion.CANReadIOIn(1, ref _IoInStatusEx);
                Motion.CANReadIOOut(1, ref _IoOutStatusEx);
            }
            return base.InitListen();
        }


        protected override bool ListenWork()
        {
            bool alm = false;
            bool elp = false;
            bool eln = false;
            bool emg = false;
            bool org = false;
            bool slp = false;
            bool sln = false;
            bool inp = false;
            bool ez = false;
            bool rdy = false;
            bool dstp = false;
            foreach (MeasurementAxis axis in Motion.Axises)
            {
                int i = axis.AxisIndex - 1;
                if (Motion.Client.GetAxisIOStatus(axis.AxisIndex, ref alm, ref elp, ref eln, ref emg, ref org, ref slp, ref sln, ref inp, ref ez, ref rdy, ref dstp))
                {
                    if (_EMG != emg)
                    {
                        _EMG = emg;
                        OnEmgStatusChanged(new EmgStatusEventArgs(emg));
                    }
                    if (_ORG[i] != org)
                    {
                        _ORG[i] = org;
                        OnAxisIOStatusChanged(new AxisIOStatusEventArgs(axis.AxisType, AxisIOTypes.ORG, org));
                    }
                    if (_ELP[i] != elp)
                    {
                        _ELP[i] = elp;
                        OnAxisIOStatusChanged(new AxisIOStatusEventArgs(axis.AxisType, AxisIOTypes.ELP, elp));
                    }
                    if (_ELN[i] != eln)
                    {
                        _ELN[i] = eln;
                        OnAxisIOStatusChanged(new AxisIOStatusEventArgs(axis.AxisType, AxisIOTypes.ELN, eln));
                    }
                    if (_ALM[i] != alm)
                    {
                        if (i < 8)
                        {
                            _ALM[i] = alm;
                            OnAxisIOStatusChanged(new AxisIOStatusEventArgs(axis.AxisType, AxisIOTypes.ALM, alm));
                        }
                    }
                    bool s = false;
                    if (Motion.Client.ReadSEVON(axis.AxisIndex, ref s))
                    {
                        if (_SON[i] != s)
                        {
                            _SON[i] = s;
                            OnAxisIOStatusChanged(new AxisIOStatusEventArgs(axis.AxisType, AxisIOTypes.SON, s));
                        }
                    }
                }
                else
                {
                    break;
                }

            }

            if (Motion.IsLinked)
            {
                IOStatusChangedEventArgs iOStatusChangedEventArgs;

                bool[] flagArray = null;
                bool flag = false;

                if (IOInStatusListenEnabled)
                {
                    if (_IoInStatusEx != null)
                    {
                        flagArray = new bool[_IoInStatusEx.Length];
                        Motion.CANReadIOIn(1, ref flagArray);

                        for (int i = 0; i < flagArray.Length; i++)
                        {
                            //if (flagArray[i] != _IoInStatusEx[i])
                            //{
                                flag = true;
                                iOStatusChangedEventArgs = new IOStatusChangedEventArgs(i, IOInStatus[i], flagArray[i]);
                                _IoInStatusEx[i] = flagArray[i];
                                OnIOInStatusExChanged(iOStatusChangedEventArgs);
                            //}
                        }
                    }
                }

                if (IOOutStatusListenEnabled)
                {
                    if (_IoOutStatusEx != null)
                    {
                        flagArray = new bool[_IoOutStatusEx.Length];
                        Motion.CANReadIOOut(1, ref flagArray);

                        for (int i = 0; i < flagArray.Length; i++)
                        {
                            if (flagArray[i] != _IoOutStatusEx[i])
                            {
                                flag = true;
                                iOStatusChangedEventArgs = new IOStatusChangedEventArgs(i, _IoOutStatusEx[i], flagArray[i]);
                                _IoOutStatusEx[i] = flagArray[i];
                                OnIOOutStatusExChanged(iOStatusChangedEventArgs);
                            }
                        }
                    }

                }

                if (flag)
                {
                    OnIOStatusExChanged();
                }
            }
            return base.ListenWork();
        }

        protected void OnAxisIOStatusChanged(AxisIOStatusEventArgs e)
        {
            if (this.AxisIOStatusChanged != null)
            {
                this.AxisIOStatusChanged(this, e);
            }
        }

        protected void OnEmgStatusChanged(EmgStatusEventArgs e)
        {
            if (this.EmgStatusChanged != null)
            {
                this.EmgStatusChanged(this, e);
            }
        }

        public event EventHandler AxisIOStatusChanged;

        public event EventHandler EmgStatusChanged;

        public event EventHandler IOInStatusExChanged;

        public event EventHandler IOOutStatusExChanged;

        public event EventHandler IOStatusExChanged;


        protected void OnIOInStatusExChanged(IOStatusChangedEventArgs e)
        {
            if (IOInStatusExChanged!=null)
            {
                IOInStatusExChanged(this, e);
            }
        }

        protected void OnIOOutStatusExChanged(IOStatusChangedEventArgs e)
        {
            if (IOOutStatusExChanged != null)
            {
                IOOutStatusExChanged(this, e);
            }
        }

        protected void OnIOStatusExChanged()
        {
            if (IOStatusExChanged != null)
            {
                IOStatusExChanged(this, null);
            }
        }

    }
}
