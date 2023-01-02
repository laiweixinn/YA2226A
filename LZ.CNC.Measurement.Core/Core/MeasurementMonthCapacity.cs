using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DY.Core.Configs;
using System.Threading;
using static LZ.CNC.Measurement.Core.MeasurementCapacity;

namespace LZ.CNC.Measurement.Core
{
    [Serializable]
    public class MeasurementMonthCapacity : ConfigBase
    {
        private DateTime _Date = DateTime.Now;

        private int _pre = 0;
        private int _currt = 0;
        private int _CurrentMonth = DateTime.Now.Month;

        private DateTime _CurrentTime = DateTime.Now;
        public void AddPre(int index)
        {

            int day = MeasurementContext.Worker.Recipe.CapacityTime.Day - 1;

            MonthProductStatic[day].NumOfDayBendOk = MeasurementContext.Capacity.DayShiftTotalDisepenserOK;
            MonthProductStatic[day].NumOfDayBendNG = MeasurementContext.Capacity.DayShiftTotalDisepenserNG;
            MonthProductStatic[day].NumOfDayTearNG = MeasurementContext.Capacity.DayShiftTotalIncomingNG;

            MonthProductStatic[day].NumOfNightBendOk = MeasurementContext.Capacity.NightShiftTotalDisepenserOK;
            MonthProductStatic[day].NumOfNightBendNG = MeasurementContext.Capacity.NightShiftTotalDisepenserNG;
            MonthProductStatic[day].NumOfNightTearNG = MeasurementContext.Capacity.NightShiftTotalIncomingNG;
            OnCapacityChanged();
            Save();

        }

        public void OnCurrentMonthChange()
        {
            try
            {
                Task.Run(() =>
                {
                    if ((DateTime.Now.Day == 1 && DateTime.Now.Hour == MeasurementContext.Worker.Recipe.CapacityTime.Hour && DateTime.Now.Minute == MeasurementContext.Worker.Recipe.CapacityTime.Minute && DateTime.Now.Second < 3))
                    {
                        for (int day = 0; day < MeasurementContext.MonthCapacity.MonthProductStatic.Count; day++)
                        {
                            MeasurementContext.MonthCapacity.MonthProductStatic[day].NumOfDayBendOk = 0;
                            MeasurementContext.MonthCapacity.MonthProductStatic[day].NumOfDayBendNG = 0;
                            MeasurementContext.MonthCapacity.MonthProductStatic[day].NumOfDayTearNG = 0;

                            MeasurementContext.MonthCapacity.MonthProductStatic[day].NumOfNightBendOk = 0;
                            MeasurementContext.MonthCapacity.MonthProductStatic[day].NumOfNightBendNG = 0;
                            MeasurementContext.MonthCapacity.MonthProductStatic[day].NumOfNightTearNG = 0;
                        }
                        Save();
                    }
                });
            }
            catch (Exception ex)
            {
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


        private List<DayProduct> _MonthCtrlProduct = new List<DayProduct>();
        public List<DayProduct> MonthProductStatic
        {
            get
            {
                if (_MonthCtrlProduct == null)
                {
                    _MonthCtrlProduct = new List<DayProduct>();
                }
                if (_MonthCtrlProduct.Count < 31)
                {
                    for (int i = 0; i < 31; i++)
                    {
                        _MonthCtrlProduct.Add(new DayProduct());
                    }
                }
                return _MonthCtrlProduct;
            }
        }




        public new static MeasurementMonthCapacity Load()
        {
            try
            {


                if ((DateTime.Now.Day == 1 && DateTime.Now.Hour == MeasurementContext.Worker.Recipe.CapacityTime.Hour && DateTime.Now.Minute < MeasurementContext.Worker.Recipe.CapacityTime.Minute)
               || (DateTime.Now.Day == 1 && DateTime.Now.Hour < MeasurementContext.Worker.Recipe.CapacityTime.Hour))

                {
                    return Load(GetApplicationPath(string.Format("statistics\\capacity\\{0}{1}month.sta", DateTime.Now.Year, DateTime.Now.AddMonths(-1)))) as MeasurementMonthCapacity;
                }
                else
                {
                    return Load(GetApplicationPath(string.Format("statistics\\capacity\\{0}{1}month.sta", DateTime.Now.Year, DateTime.Now.Month))) as MeasurementMonthCapacity;
                }
            }
            catch (Exception ex)
            {

                return Load(GetApplicationPath(string.Format("statistics\\capacity\\{0}{1}month.sta", DateTime.Now.Year, DateTime.Now.Month))) as MeasurementMonthCapacity;
            }
        }

        public override bool Save()
        {
            try
            {
                if ((DateTime.Now.Day == 1 && DateTime.Now.Hour == MeasurementContext.Worker.Recipe.CapacityTime.Hour && DateTime.Now.Minute < MeasurementContext.Worker.Recipe.CapacityTime.Minute)
             || (DateTime.Now.Day == 1 && DateTime.Now.Hour < MeasurementContext.Worker.Recipe.CapacityTime.Hour))
                {
                    return Save(GetApplicationPath(string.Format("statistics\\capacity\\{0}{1}month.sta", DateTime.Now.Year, DateTime.Now.AddMonths(-1))));
                }
                else
                {
                    return Save(GetApplicationPath(string.Format("statistics\\capacity\\{0}{1}month.sta", DateTime.Now.Year, DateTime.Now.Month)));
                }
            }
            catch (Exception ex)
            {
                return Save(GetApplicationPath(string.Format("statistics\\capacity\\{0}{1}month.sta", DateTime.Now.Year, DateTime.Now.Month)));
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

        }


        [Serializable]
        public class DayProduct
        {
            private int _NumOfDayBendOK;
            private int _NumOfDayBendNG;
            private int _NumOfDayTearNG;
            public int NumOfDayBendOk
            {
                get
                {
                    return _NumOfDayBendOK;
                }
                set
                {
                    _NumOfDayBendOK = value;
                }
            }

            public int NumOfDayBendNG
            {
                get
                {
                    return _NumOfDayBendNG;
                }
                set
                {
                    _NumOfDayBendNG = value;
                }
            }

            public int NumOfDayTearNG
            {
                get
                {
                    return _NumOfDayTearNG;
                }
                set
                {
                    _NumOfDayTearNG = value;
                }
            }

            public int DayTotalNum
            {

                get
                {
                    return _NumOfDayBendOK + _NumOfDayTearNG + _NumOfDayBendNG;
                }
            }

            public string DayPercentBendOK
            {
                get
                {
                    string percent = "0";
                    if (DayTotalNum != 0)
                    {
                        percent = (((double)_NumOfDayBendOK / (_NumOfDayBendOK + _NumOfDayBendNG)) * 100).ToString("0.00");
                    }
                    percent = string.Format("{0}%", percent);
                    return percent;
                }
            }

            public string DayPercentTotalOK
            {
                get
                {
                    string percent = "0";
                    if (DayTotalNum != 0)
                    {
                        percent = (((double)_NumOfDayBendOK / DayTotalNum) * 100).ToString("0.00");
                    }
                    percent = string.Format("{0}%", percent);
                    return percent;
                }
            }

            private int _NumOfNightBendOK;
            private int _NumOfNightBendNG;
            private int _NumOfNightTearNG;
            public int NumOfNightBendOk
            {
                get
                {
                    return _NumOfNightBendOK;
                }
                set
                {
                    _NumOfNightBendOK = value;
                }
            }

            public int NumOfNightBendNG
            {
                get
                {
                    return _NumOfNightBendNG;
                }
                set
                {
                    _NumOfNightBendNG = value;
                }
            }

            public int NumOfNightTearNG
            {
                get
                {
                    return _NumOfNightTearNG;
                }
                set
                {
                    _NumOfNightTearNG = value;
                }
            }

            public int NightTotalNum
            {
                get
                {
                    return _NumOfNightBendOK + _NumOfNightTearNG + _NumOfNightBendNG;
                }
            }

            public string NightPercentBendOK
            {
                get
                {
                    string percent = "0";
                    if (NightTotalNum != 0)
                    {
                        percent = (((double)_NumOfNightBendOK / (_NumOfNightBendOK + _NumOfNightBendNG)) * 100).ToString("0.00");
                    }
                    percent = string.Format("{0}%", percent);
                    return percent;
                }
            }

            public string NightPercentTotalOK
            {
                get
                {
                    string percent = "0";
                    if (NightTotalNum != 0)
                    {
                        percent = (((double)_NumOfNightBendOK / NightTotalNum) * 100).ToString("0.00");
                    }
                    percent = string.Format("{0}%", percent);
                    return percent;
                }
            }
        }



        [Serializable]
        public class MonthProduct
        {
            private int _NumOfBendOK;
            private int _NumOfBendNG;
            private int _NumOfTearNG;
            public int NumOfBendOk
            {
                get
                {
                    return _NumOfBendOK;
                }
                set
                {
                    _NumOfBendOK = value;
                }
            }

            public int NumOfBendNG
            {
                get
                {
                    return _NumOfBendNG;
                }
                set
                {
                    _NumOfBendNG = value;
                }
            }

            public int NumOfTearNG
            {
                get
                {
                    return _NumOfTearNG;
                }
                set
                {
                    _NumOfTearNG = value;
                }
            }

            public int TotalNum
            {
                get
                {
                    return _NumOfBendOK + _NumOfTearNG + _NumOfBendNG;
                }
            }

            public string PercentBendOK
            {
                get
                {
                    string percent = "0";
                    if (TotalNum != 0)
                    {
                        percent = (((double)_NumOfBendOK / (_NumOfBendOK + _NumOfBendNG)) * 100).ToString("0.00");
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
                    if (TotalNum != 0)
                    {
                        percent = (((double)_NumOfBendOK / TotalNum) * 100).ToString("0.00");
                    }
                    percent = string.Format("{0}%", percent);
                    return percent;
                }
            }

        }







        [field: NonSerialized]
        public event EventHandler CapacityChanged;


        private void OnCurrentMonthChanged()
        {
            //_MonthCtrlProduct[_CurrentMonth - 1].NumOfBendOk = 0;
            //_MonthCtrlProduct[_CurrentMonth - 1].NumOfBendNG = 0;
            //_MonthCtrlProduct[_CurrentMonth - 1].NumOfTearNG = 0;
            OnCapacityChanged();
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

