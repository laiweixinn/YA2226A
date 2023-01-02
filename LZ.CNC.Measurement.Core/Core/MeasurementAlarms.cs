using DY.Core.Configs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Core
{
    [Serializable]
    public class MeasurementAlarms : ConfigBase
    {

        private DateTime _Date = DateTime.Now;
        private AlarmCollection _AlarmItems = new AlarmCollection();
        private static MeasurementAlarms _Instance = null;

        public DateTime Date
        {
            get
            {
                return _Date;
            }
        }

        public AlarmCollection AlarmItems
        {
            get
            {
                return _AlarmItems;
            }
        }

        public MeasurementAlarms()
        {
        }

        public static void Add(AlarmItem item)
        {

            if (_Instance == null)
            {
                _Instance = Load(DateTime.Now);
            }
            if (_Instance == null)
            {
                _Instance = new MeasurementAlarms();
            }
            if ((item.Time.Year != _Instance.Date.Year || item.Time.Month != _Instance.Date.Month ? true : item.Time.Day != _Instance.Date.Day))
            {
                _Instance.Save();
                _Instance = new MeasurementAlarms();
                _Instance.AlarmItems.Add(item);
            }
            else
            {
                _Instance.AlarmItems.Add(item);
            }
            _Instance.Save();
        }

        public static void Add(string msg)
        {
            Add(new AlarmItem()
            {
                AlarmInfo = msg
            });
        }

        public static List<AlarmItem> GetAlarms(DateTime start, DateTime end)
        {
            List<AlarmItem> items = new List<AlarmItem>();
            DateTime _start = start;
            DateTime _end = end;
            if (start > end)
            {
                _start = end;
                _end = start;
            }
            DateTime date = _start.Date;
            while (true)
            {
                MeasurementAlarms ss = Load(date);
                if (ss != null)
                {
                    for (int i = 0; i < ss.AlarmItems.Count; i++)
                    {
                        AlarmItem _item = ss.AlarmItems[i];
                        if ((_item.Time < _start ? false : _item.Time <= _end))
                        {
                            items.Add(_item);
                        }
                    }
                }
                date = date.AddDays(1);
                if (date > _end)
                {
                    break;
                }
            }
            return items;
        }

        public static MeasurementAlarms Load(DateTime date)
        {
            try
            {
                string name = string.Format("{0}.alm", date.ToString("yyyyMMdd"));
                string path = Path.Combine(Application.StartupPath, "alarms");
                path = Path.Combine(path, name);
                return Load(path) as MeasurementAlarms;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static new MeasurementAlarms Load()
        {
            return Load(DateTime.Now);
        }

        public override bool Save()
        {
            string name = string.Format("{0}.alm", _Date.ToString("yyyyMMdd"));
            string path = Path.Combine(Application.StartupPath, "alarms");
            return Save(Path.Combine(path, name));
        }

        [Serializable]
        public class AlarmCollection : CollectionBase
        {
            public AlarmItem this[int index]
            {
                get
                {
                    return InnerList[index] as AlarmItem;
                }
            }

            public AlarmCollection()
            {
            }

            public void Add(AlarmItem alarmItem)
            {
                InnerList.Add(alarmItem);
            }
        }

        [Serializable]
        public class AlarmItem
        {
            private DateTime _Time = DateTime.Now;
            private string _AlarmInfo;

            public string AlarmInfo
            {
                get
                {
                    return _AlarmInfo;
                }
                set
                {
                    _AlarmInfo = value;
                }
            }

            public DateTime Time
            {
                get
                {
                    return _Time;
                }
            }

            public AlarmItem()
            {
            }
        }
    }
}
