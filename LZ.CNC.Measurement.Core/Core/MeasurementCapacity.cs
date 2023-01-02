using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DY.Core.Configs;
using System.Threading;


namespace LZ.CNC.Measurement.Core
{
    [Serializable]
    public class MeasurementCapacity : ConfigBase
    {
        private DateTime _Date = DateTime.Now;

        private int _pre = 0;
        private int _currt = 0;
        private int _CurrentHour = DateTime.Now.Hour;
        private DateTime _CurrentTime = DateTime.Now;

        //     private TimeSpan _dayfreetime;

        //public TimeSpan DayFreeTime
        //{
        //    get
        //    {
        //        return _dayfreetime;
        //    }
        //    set
        //    {
        //        lock (this)
        //        {
        //            _dayfreetime = value;
        //        }
        //    }
        //}

        //private TimeSpan _nightfreetime;
        //public TimeSpan NightfreeTime
        //{
        //    get
        //    {
        //        return _nightfreetime;
        //    }
        //    set
        //    {
        //        _nightfreetime = value;
        //    }
        //}

        public bool b_flag = false;

        #region 
        private int _bend1ok;
        private int _bend2ok;
        private int _bend3ok;
        private int _bend1ng;
        private int _bend2ng;
        private int _bend3ng;
        private int _tear1ok;
        private int _tear1ng;
        private int _tear2ok;
        private int _tear2ng;
        private int _tear3ok;
        private int _tear3ng;
        private int _bendGetGapNG;

        private int _bendAOING;

        public int bendGetGapNG
        {
            get
            {
                return _bendGetGapNG;
            }
            set
            {
                _bendGetGapNG = value;
            }
        }


        public int bendAOING
        {
            get
            {
                return _bendAOING;
            }
            set
            {
                _bendAOING = value;
            }
        }

        public int tear1ok
        {
            get
            {
                return _tear1ok;
            }
        }

        public int tear2ok
        {
            get
            {
                return _tear2ok;
            }
        }


        public int tear3ok
        {
            get
            {
                return _tear3ok;
            }
        }


        public int tear1ng
        {
            get
            {
                return _tear1ng;
            }
        }

        public int tear2ng
        {
            get
            {
                return _tear2ng;
            }
        }


        public int tear3ng
        {
            get
            {
                return _tear3ng;
            }
        }


        public int bend1ok
        {
            get
            {
                return _bend1ok;
            }
        }


        public int bend2ok
        {
            get
            {
                return _bend2ok;
            }
        }


        public int bend3ok
        {
            get
            {
                return _bend3ok;
            }
        }


        public int bend1ng
        {
            get
            {
                return _bend1ng;
            }
        }
        public int bend2ng
        {
            get
            {
                return _bend2ng;
            }
        }
        public int bend3ng
        {
            get
            {
                return _bend3ng;
            }
        }


        public string tear1okpercent
        {
            get
            {
                string percent = "0";
                if ((_tear1ok + _tear1ng) != 0)
                {
                    percent = (((double)_tear1ok / (_tear1ok + _tear1ng)) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }

        public string tear2okpercent
        {
            get
            {
                string percent = "0";
                if ((_tear2ok + _tear2ng) != 0)
                {
                    percent = (((double)_tear2ok / (_tear2ok + _tear2ng)) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }


        public string tear3okpercent
        {
            get
            {
                string percent = "0";
                if ((_tear3ok + _tear3ng) != 0)
                {
                    percent = (((double)_tear3ok / (_tear3ok + _tear3ng)) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }


        public string bend1okpercent
        {
            get
            {
                string percent = "0";
                if ((_bend1ok + _bend1ng) != 0)
                {
                    percent = (((double)_bend1ok / (_bend1ok + _bend1ng)) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }

        public string bend2okpercent
        {
            get
            {
                string percent = "0";
                if ((_bend2ok + _bend2ng) != 0)
                {
                    percent = (((double)_bend2ok / (_bend2ok + _bend2ng)) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }


        public string bend3okpercent
        {
            get
            {
                string percent = "0";
                if ((_bend3ok + _bend3ng) != 0)
                {
                    percent = (((double)_bend3ok / (_bend3ok + _bend3ng)) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }

        #endregion
        public DateTime CurrentTime
        {
            get
            {
                return _CurrentTime;
            }
            set
            {
                _pre = TimeRankPlace(_CurrentTime);
                Thread.Sleep(10);
                _currt = TimeRankPlace(DateTime.Now);
                if (_pre != _currt)
                {
                    _pre = _currt;
                    _CurrentTime = DateTime.Now;
                    OnCurrentTimeChanged();
                }
            }
        }


        private int TimeRankPlace(DateTime dt)
        {
            if (dt >= MeasurementContext.Worker.Recipe._CapacityTime && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(1))
            {
                return 8;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(1) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(2))
            {
                return 9;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(2) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(3))
            {
                return 10;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(3) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(4))
            {
                return 11;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(4) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(5))
            {
                return 12;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(5) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(6))
            {
                return 13;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(6) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(7))
            {
                return 14;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(7) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(8))
            {
                return 15;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(8) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(9))
            {
                return 16;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(9) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(10))
            {
                return 17;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(10) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(11))
            {
                return 18;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(11) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(12))
            {
                return 19;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(12) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(13))
            {
                return 20;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(13) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(14))
            {
                return 21;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(14) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(15))
            {
                return 22;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(15) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(16))
            {
                return 23;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(16) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(17))
            {
                return 0;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(17) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(18))
            {
                return 1;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(18) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(19))
            {
                return 2;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(19) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(20))
            {
                return 3;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(20) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(21))
            {
                return 4;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(21) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(22))
            {
                return 5;
            }
            else if (dt >= MeasurementContext.Worker.Recipe._CapacityTime.AddHours(22) && dt < MeasurementContext.Worker.Recipe._CapacityTime.AddHours(23))
            {
                return 6;
            }
            else
            {
                return 7;
            }
        }

        static object _obj = new object();
        /// <summary>
        /// 有弯折AOI统计
        /// </summary>
        /// <param name="index">1 弯折OK 2弯折NG 3撕膜NG 4撕膜OK</param>
        /// <param name="station"></param>
        /// <param name="type"> 1:抓边NG 2：AOI NG </param>
        public void AddResult(int index, int station, int type)
        {
            HourProduct hourProduct = null;

            lock (_obj)
            {
                if (_pre >= 8 && _pre < 20)
                {
                    hourProduct = DayShiftProduct[_pre - 8];
                }
                else
                {
                    if (_pre >= 0 && _pre < 8)
                    {
                        hourProduct = NightShiftProduct[_pre + 4];
                    }
                    else
                    {
                        hourProduct = NightShiftProduct[_pre - 20];
                    }
                }


                switch (index)
                {
                    case 1:
                        hourProduct.NumOfDispenseOK++;
                        if (station == 1)
                        {
                            _bend1ok++;
                        }
                        else if (station == 2)
                        {
                            _bend2ok++;
                        }
                        else
                        {
                            _bend3ok++;
                        }

                        MeasurementContext.MonthCapacity.AddPre(1);
                        break;
                    case 2:
                        hourProduct.NumOfDispenseNG++;
                        if (station == 1)
                        {
                            if (type == 1)
                            {
                                _bendGetGapNG++;
                            }
                            else if (type == 2)
                            {
                                _bendAOING++;
                            }
                            _bend1ng++;
                        }
                        else if (station == 2)
                        {
                            if (type == 1)
                            {
                                _bendGetGapNG++;
                            }
                            else if (type == 2)
                            {
                                _bendAOING++;
                            }
                            _bend2ng++;
                        }
                        else
                        {
                            if (type == 1)
                            {
                                _bendGetGapNG++;
                            }
                            else if (type == 2)
                            {
                                _bendAOING++;
                            }
                            _bend3ng++;
                        }
                        MeasurementContext.MonthCapacity.AddPre(2);
                        break;
                    case 3:
                        hourProduct.NumOfIncomingNG++;
                        if (station == 1)
                        {
                            _tear1ng++;
                        }
                        else if (station == 2)
                        {
                            _tear2ng++;
                        }
                        else
                        {
                            _tear3ng++;
                        }
                        MeasurementContext.MonthCapacity.AddPre(3);
                        break;
                    case 4:
                        if (station == 1)
                        {
                            _tear1ok++;
                        }
                        else if (station == 2)
                        {
                            _tear2ok++;
                        }
                        else
                        {
                            _tear3ok++;
                        }
                        break;
                    default:
                        break;
                }
                OnCapacityChanged();
                Save();
            }
        }

        /// <summary>
        /// 无弯折AOI统计
        /// </summary>
        /// <param name="index"></param>
        /// <param name="station">1 弯折OK 2弯折NG 3撕膜NG 4撕膜OK</param>
        public void AddPre(int index, int station)
        {
            HourProduct hourProduct = null;

            //  DayShiftProduct[9].NumOfDispenseNG -= 3;


            if (_pre >= 8 && _pre < 20)
            {
                hourProduct = DayShiftProduct[_pre - 8];
            }
            else
            {
                if (_pre >= 0 && _pre < 8)
                {
                    hourProduct = NightShiftProduct[_pre + 4];
                }
                else
                {
                    hourProduct = NightShiftProduct[_pre - 20];
                }
            }


            switch (index)
            {
                case 1:
                    hourProduct.NumOfDispenseOK++;
                    if (station == 1)
                    {
                        _bend1ok++;
                    }
                    else if (station == 2)
                    {
                        _bend2ok++;
                    }
                    else
                    {
                        _bend3ok++;
                    }

                    MeasurementContext.MonthCapacity.AddPre(1);
                    break;
                case 2:
                    hourProduct.NumOfDispenseNG++;
                    if (station == 1)
                    {
                        _bend1ng++;
                    }
                    else if (station == 2)
                    {
                        _bend2ng++;
                    }
                    else
                    {
                        _bend3ng++;
                    }
                    MeasurementContext.MonthCapacity.AddPre(2);
                    break;
                case 3:
                    hourProduct.NumOfIncomingNG++;
                    if (station == 1)
                    {
                        _tear1ng++;
                    }
                    else if (station == 2)
                    {
                        _tear2ng++;
                    }
                    else
                    {
                        _tear3ng++;
                    }
                    MeasurementContext.MonthCapacity.AddPre(3);
                    break;
                case 4:
                    if (station == 1)
                    {
                        _tear1ok++;
                    }
                    else if (station == 2)
                    {
                        _tear2ok++;
                    }
                    else
                    {
                        _tear3ok++;
                    }
                    break;
                default:
                    break;
            }
            OnCapacityChanged();
            Save();

        }

        public int DayShiftTotalDisepenserOK
        {
            get
            {
                int total = 0;
                for (int i = 0; i < _DayShiftProduct.Count; i++)
                {
                    total += _DayShiftProduct[i].NumOfDispenseOK;
                }
                return total;
            }
        }

        public int DayShiftTotalDisepenserNG
        {
            get
            {
                int total = 0;
                for (int i = 0; i < _DayShiftProduct.Count; i++)
                {
                    total += _DayShiftProduct[i].NumOfDispenseNG;
                }
                return total;
            }
        }

        public int DayShiftTotalIncomingNG
        {
            get
            {
                int total = 0;
                for (int i = 0; i < _DayShiftProduct.Count; i++)
                {
                    total += _DayShiftProduct[i].NumOfIncomingNG;
                }
                return total;
            }
        }

        public int NightShiftTotalDisepenserOK
        {
            get
            {
                int total = 0;
                for (int i = 0; i < _NightShiftProduct.Count; i++)
                {
                    total += _NightShiftProduct[i].NumOfDispenseOK;
                }
                return total;
            }
        }

        public int NightShiftTotalDisepenserNG
        {
            get
            {
                int total = 0;
                for (int i = 0; i < _NightShiftProduct.Count; i++)
                {
                    total += _NightShiftProduct[i].NumOfDispenseNG;
                }
                return total;
            }
        }

        public int NightShiftTotalIncomingNG
        {
            get
            {
                int total = 0;
                for (int i = 0; i < _NightShiftProduct.Count; i++)
                {
                    total += _NightShiftProduct[i].NumOfIncomingNG;
                }
                return total;
            }
        }

        public int DayShiftTotal
        {
            get
            {
                return DayShiftTotalDisepenserOK + DayShiftTotalDisepenserNG + DayShiftTotalIncomingNG;
            }
        }

        public int NightShiftTotal
        {
            get
            {
                return NightShiftTotalDisepenserOK + NightShiftTotalDisepenserNG + NightShiftTotalIncomingNG;
            }
        }

        public string DayShiftPercentDispenseOK
        {
            get
            {
                string percent = "0";
                if ((DayShiftTotalDisepenserOK + DayShiftTotalDisepenserNG) != 0)
                {
                    percent = (((double)DayShiftTotalDisepenserOK / (DayShiftTotalDisepenserOK + DayShiftTotalDisepenserNG)) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }

        public string DayShiftPercentTotalOK
        {
            get
            {
                string percent = "0";
                if ((DayShiftTotal) != 0)
                {
                    percent = (((double)DayShiftTotalDisepenserOK / DayShiftTotal) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }

        public string NightShiftPercentDispenseOK
        {
            get
            {
                string percent = "0";
                if ((NightShiftTotalDisepenserOK + NightShiftTotalDisepenserNG) != 0)
                {
                    percent = (((double)NightShiftTotalDisepenserOK / (NightShiftTotalDisepenserOK + NightShiftTotalDisepenserNG)) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }

        public string NightShiftPercentTotalOK
        {
            get
            {
                string percent = "0";
                if ((NightShiftTotal) != 0)
                {
                    percent = (((double)NightShiftTotalDisepenserOK / NightShiftTotal) * 100).ToString("0.00");
                }
                percent = string.Format("{0}%", percent);
                return percent;
            }
        }

        public TimeSpan DayProductTime
        {
            get
            {
                TimeSpan total = TimeSpan.Zero;
                for (int i = 0; i < _DayShiftProduct.Count; i++)
                {
                    total += _DayShiftProduct[i].HourProductionTime;
                }
                return total;

            }
        }

        public TimeSpan DayAlarmTime
        {

            get
            {
                TimeSpan total = TimeSpan.Zero;
                for (int i = 0; i < _DayShiftProduct.Count; i++)
                {
                    total += _DayShiftProduct[i].HourAlarmTime;
                }
                return total;
            }
        }

        public TimeSpan DayFreeTime
        {
            get
            {
                TimeSpan total = TimeSpan.Zero;
                for (int i = 0; i < _DayShiftProduct.Count; i++)
                {
                    total += _DayShiftProduct[i].HourFreeTime;
                }
                return total;
            }
        }

        public TimeSpan DayStopTime
        {
            get
            {
                TimeSpan total = new TimeSpan(12, 0, 0);
                return total - DayProductTime - DayAlarmTime - DayFreeTime;
            }

        }



        public TimeSpan NightProductTime
        {
            get
            {
                TimeSpan total = TimeSpan.Zero;
                for (int i = 0; i < _NightShiftProduct.Count; i++)
                {
                    total += _NightShiftProduct[i].HourProductionTime;
                }
                return total;

            }
        }

        public TimeSpan NightAlarmTime
        {

            get
            {
                TimeSpan total = TimeSpan.Zero;
                for (int i = 0; i < _NightShiftProduct.Count; i++)
                {
                    total += _NightShiftProduct[i].HourAlarmTime;
                }
                return total;
            }
        }

        public TimeSpan NightFreeTime
        {
            get
            {
                TimeSpan total = TimeSpan.Zero;
                for (int i = 0; i < _NightShiftProduct.Count; i++)
                {
                    total += _NightShiftProduct[i].HourFreeTime;
                }
                return total;
            }
        }

        public TimeSpan NightStopTime
        {
            get
            {
                TimeSpan total = new TimeSpan(12, 0, 0);
                return total - NightProductTime - NightAlarmTime - NightFreeTime;
            }

        }


        public DateTime Date
        {
            get
            {
                _Date = DateTime.Now.Date;
                return _Date;
            }
        }





        private List<HourProduct> _DayShiftProduct = new List<HourProduct>()
        {
            new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
        };

        private List<HourProduct> _NightShiftProduct = new List<HourProduct>()
        {
            new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
                        new HourProduct(),
            new HourProduct(),
        };



        public List<HourProduct> DayShiftProduct
        {
            get
            {
                if (_DayShiftProduct == null)
                {
                    _DayShiftProduct = new List<HourProduct>();
                }
                if (_DayShiftProduct.Count < 12)
                {
                    for (int i = _DayShiftProduct.Count; i < 12; i++)
                    {
                        _DayShiftProduct.Add(new HourProduct());
                    }
                }
                return _DayShiftProduct;
            }
        }

        public List<HourProduct> NightShiftProduct
        {
            get
            {
                if (_NightShiftProduct == null)
                {
                    _NightShiftProduct = new List<HourProduct>();
                }
                if (_NightShiftProduct.Count < 12)
                {
                    for (int i = _NightShiftProduct.Count; i < 12; i++)
                    {
                        _NightShiftProduct.Add(new HourProduct());
                    }
                }
                return _NightShiftProduct;
            }
        }

        public new static MeasurementCapacity Load()
        {

            if ((DateTime.Now.Hour ==MeasurementContext.Worker.Recipe.CapacityTime.Hour && DateTime.Now.Minute < MeasurementContext.Worker.Recipe.CapacityTime.Minute)
             || DateTime.Now.Hour < MeasurementContext.Worker.Recipe.CapacityTime.Hour)
            {
                MeasurementContext.Worker.Recipe.CapacityTime = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:00", DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, MeasurementContext.Worker.Recipe.CapacityTime.Hour, MeasurementContext.Worker.Recipe.CapacityTime.Minute));
            }
            else
            {
                MeasurementContext.Worker.Recipe.CapacityTime = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:00", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, MeasurementContext.Worker.Recipe.CapacityTime.Hour, MeasurementContext.Worker.Recipe.CapacityTime.Minute));
            }

            return Load(GetApplicationPath(string.Format("statistics\\capacity\\{0}.sta", MeasurementContext.Worker.Recipe.CapacityTime.ToString("yyyyMMdd") /*DateTime.Now.ToString("yyyyMMdd")*/))) as MeasurementCapacity;
         
        }

        public  static MeasurementCapacity Load(DateTime tm)
        {
            DateTime capacityTime;
            if ((tm.Hour == MeasurementContext.Worker.Recipe.CapacityTime.Hour && tm.Minute < MeasurementContext.Worker.Recipe.CapacityTime.Minute)
             || tm.Hour < MeasurementContext.Worker.Recipe.CapacityTime.Hour)
            {
                capacityTime = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:00", tm.AddDays(-1).Year, tm.AddDays(-1).Month, tm.AddDays(-1).Day, MeasurementContext.Worker.Recipe.CapacityTime.Hour, MeasurementContext.Worker.Recipe.CapacityTime.Minute));
            }
            else
            {
                capacityTime = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:00", tm.Year, tm.Month, tm.Day, MeasurementContext.Worker.Recipe.CapacityTime.Hour, MeasurementContext.Worker.Recipe.CapacityTime.Minute));
            }
            return Load(GetApplicationPath(string.Format("statistics\\capacity\\{0}.sta", MeasurementContext.Worker.Recipe.CapacityTime.ToString("yyyyMMdd")))) as MeasurementCapacity;

        }


        public override bool Save()
        {
            if ((DateTime.Now.Hour == MeasurementContext.Worker.Recipe.CapacityTime.Hour && DateTime.Now.Minute < MeasurementContext.Worker.Recipe.CapacityTime.Minute)
                || DateTime.Now.Hour < MeasurementContext.Worker.Recipe.CapacityTime.Hour)
            {
                return Save(GetApplicationPath(string.Format("statistics\\capacity\\{0}.sta", DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))));
            }
            else
            {
                return Save(GetApplicationPath(string.Format("statistics\\capacity\\{0}.sta", DateTime.Now.ToString("yyyyMMdd"))));
            }
        }

        [Serializable]
        public class HourProduct
        {
            private int _NumOfDispenseOK;

            private int _NumOfDispenseNG;

            private int _NumOfIncomingNG;

            public int NumOfDispenseOK
            {
                get
                {
                    return _NumOfDispenseOK;
                }
                set
                {
                    _NumOfDispenseOK = value;
                }
            }

            public int NumOfDispenseNG
            {
                get
                {
                    return _NumOfDispenseNG;
                }
                set
                {
                    _NumOfDispenseNG = value;
                }
            }

            public int NumOfIncomingNG
            {
                get
                {
                    return _NumOfIncomingNG;
                }
                set
                {
                    _NumOfIncomingNG = value;
                }
            }

            public int TotalNumPerHour
            {
                get
                {
                    return _NumOfDispenseOK + _NumOfDispenseNG + _NumOfIncomingNG;
                }
            }

            public string PercentDispenseOK
            {
                get
                {
                    string percent = "0";
                    if (TotalNumPerHour != 0)
                    {
                        percent = (((double)_NumOfDispenseOK / (_NumOfDispenseOK + _NumOfDispenseNG)) * 100).ToString("0.00");
                    }
                    percent = string.Format("{0}%", percent);
                    return percent;
                }
            }

            public string PercentTotalOK
            {
                get
                {
                    string percent = "0";
                    if (TotalNumPerHour != 0)
                    {
                        percent = (((double)_NumOfDispenseOK / TotalNumPerHour) * 100).ToString("0.00");
                    }
                    percent = string.Format("{0}%", percent);
                    return percent;
                }
            }

            private TimeSpan _HourFreeTime;
            /// <summary>
            /// 待料时间
            /// </summary>
            public TimeSpan HourFreeTime
            {
                get
                {
                    return _HourFreeTime;
                }
                set
                {
                    lock (this)
                    {
                        _HourFreeTime = value;
                    }
                }
            }

            private TimeSpan _HourAlarmTime;
            /// <summary>
            /// 报警时间
            /// </summary>
            public TimeSpan HourAlarmTime
            {
                get
                {
                    return _HourAlarmTime;
                }
                set
                {
                    lock (this)
                    {
                        _HourAlarmTime = value;
                    }
                }
            }


            private TimeSpan _HourProductionTime;
            /// <summary>
            /// 生产时间
            /// </summary>
            public TimeSpan HourProductionTime
            {

                get
                {
                    return _HourProductionTime;
                }
                set
                {
                    lock (this)
                    {
                        _HourProductionTime = value;
                    }
                }
            }

            /// <summary>
            /// 停机时间
            /// </summary>
            public int HourStopTime
            {
                get
                {
                    return 60 - _HourFreeTime.Minutes - _HourAlarmTime.Minutes - _HourProductionTime.Minutes;
                }
            }

        }

        [field: NonSerialized]
        public event EventHandler CapacityChanged;

        //[field: NonSerialized]
        //public event EventHandler CurrentHourChanged;

        private void OnCurrentMonthChanged()
        {

        }


        private void OnWorkChanged()
        {

        }
        TimeSpan changetms = new TimeSpan(0, 0, 0);
        private void OnCurrentTimeChanged()
        {
            if (_pre >= 8 && _pre < 20)
            {
                for (int i = 0; i < 20 - _pre; i++)
                {
                    _DayShiftProduct[i - 8 + _pre].NumOfDispenseOK = 0;
                    _DayShiftProduct[i - 8 + _pre].NumOfDispenseNG = 0;
                    _DayShiftProduct[i - 8 + _pre].NumOfIncomingNG = 0;
                    _DayShiftProduct[i - 8 + _pre].HourAlarmTime = changetms;
                    _DayShiftProduct[i - 8 + _pre].HourFreeTime = changetms;
                    _DayShiftProduct[i - 8 + _pre].HourProductionTime = changetms;
                }

                for (int j = 0; j < 12; j++)
                {
                    _NightShiftProduct[j].NumOfDispenseOK = 0;
                    _NightShiftProduct[j].NumOfDispenseNG = 0;
                    _NightShiftProduct[j].NumOfIncomingNG = 0;

                    _NightShiftProduct[j].HourAlarmTime = changetms;
                    _NightShiftProduct[j].HourFreeTime = changetms;
                    _NightShiftProduct[j].HourProductionTime = changetms;
                }
            }
            else
            {
                if (_pre >= 0 && _pre < 8)
                {
                    for (int i = 0; i < 8 - _pre; i++)
                    {
                        _NightShiftProduct[i + 4 + _pre].NumOfDispenseOK = 0;
                        _NightShiftProduct[i + 4 + _pre].NumOfDispenseNG = 0;
                        _NightShiftProduct[i + 4 + _pre].NumOfIncomingNG = 0;
                        _NightShiftProduct[i + 4 + _pre].HourAlarmTime = changetms;
                        _NightShiftProduct[i + 4 + _pre].HourFreeTime = changetms;
                        _NightShiftProduct[i + 4 + _pre].HourProductionTime = changetms;
                    }
                }
                else if (_pre >= 20 && _pre < 24)
                {
                    for (int i = 0; i < (32 - _pre); i++)
                    {
                        _NightShiftProduct[i - 20 + _pre].NumOfDispenseOK = 0;
                        _NightShiftProduct[i - 20 + _pre].NumOfDispenseNG = 0;
                        _NightShiftProduct[i - 20 + _pre].NumOfIncomingNG = 0;
                        _NightShiftProduct[i - 20 + _pre].HourAlarmTime = changetms;
                        _NightShiftProduct[i - 20 + _pre].HourFreeTime = changetms;
                        _NightShiftProduct[i - 20 + _pre].HourProductionTime = changetms;
                    }
                }
            }

            //if (_dayfreetime > DateTime.Now - MeasurementContext.Worker.Recipe.CapacityTime)
            //{
            //    b_flag = true;
            //    _dayfreetime = TimeSpan.Zero;
            //    _nightfreetime = TimeSpan.Zero;
            //    Thread.Sleep(10);
            //    b_flag = false;
            //}

            //if (_nightfreetime > DateTime.Now - MeasurementContext.Worker.Recipe.CapacityTime.AddHours(12))
            //{
            //    _nightfreetime = TimeSpan.Zero;
            //}

            if (_pre == 8)
            {
                b_flag = true;
                //_dayfreetime = TimeSpan.Zero;
                //_nightfreetime = TimeSpan.Zero;
                _tear1ok = 0;
                _tear1ng = 0;
                _tear2ok = 0;
                _tear2ng = 0;
                _tear3ok = 0;
                _tear3ng = 0;
                _bend1ng = 0;
                _bend1ok = 0;
                _bend2ng = 0;
                _bend2ok = 0;
                _bend3ng = 0;
                _bend3ok = 0;
                Thread.Sleep(10);
                b_flag = false;
            }


            OnCapacityChanged();
            Save();
        }

       

        private void OnCapacityChanged()
        {
            if (CapacityChanged != null)
            {
                CapacityChanged(null, null);
            }
        }
    }
}
