using DY.Core.Configs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Core
{
    [Serializable]
    public class MeasurementStatistics:ConfigBase
    {
        private DateTime _Date = DateTime.Now;

        private StatisticsCollection _Items = new StatisticsCollection();

        private static MeasurementStatistics _Instance;

        private static MeasurementStatistics _GroupInstance;

        public DateTime Date
        {
            get
            {
                return _Date;
            }
        }

        public StatisticsCollection Items
        {
            get
            {
                return _Items;
            }
        }

        static MeasurementStatistics()
        {
          //  _Instance = null;
            _GroupInstance = null;
        }

        public MeasurementStatistics()
        {
        }

        public static void Add(StatisticsItem item)
        {
            if (_Instance == null)
            {
                _Instance = Load(DateTime.Now);
            }
            if (_Instance == null)
            {
                _Instance = new MeasurementStatistics();
            }
            if ((item.StartTime.Year != _Instance.Date.Year || item.StartTime.Month != _Instance.Date.Month ? true : item.StartTime.Day != _Instance.Date.Day))
            {
                _Instance.Save();
                _Instance = new MeasurementStatistics();
                _Instance.Items.Add(item);
            }
            else
            {
                _Instance.Items.Add(item);
            }
            _Instance.Save();
            
        }

        public static List<StatisticsItem> GetStatistics(DateTime start, DateTime end)
        {
            List<StatisticsItem> items = new List<StatisticsItem>();
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
                MeasurementStatistics ss = Load(date);
                if (ss != null)
                {
                    for (int i = 0; i < ss.Items.Count; i++)
                    {
                        StatisticsItem _item = ss.Items[i];
                        if ((_item.StartTime < _start ? false : _item.StartTime <= _end))
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

        public static List<string> ListGroup(string name, DateTime date)
        {
            List<string> strs;
            List<string> filenames = new List<string>();
            if (!string.IsNullOrEmpty(name))
            {
                string path = Path.Combine(Application.StartupPath, string.Format("statistics\\groups\\{0}\\{1}", name, date.ToString("yyyyMMdd")));
                if (Directory.Exists(path))
                {
                    filenames.AddRange(Directory.GetFiles(path, "*.sta"));
                    strs = filenames;
                }
                else
                {
                    strs = filenames;
                }
            }
            else
            {
                strs = filenames;
            }
            return strs;
        }

        public static MeasurementStatistics Load(DateTime date)
        {
            string name = string.Format("{0}.sta", date.ToString("yyyyMMdd"));
            string path = Path.Combine(Application.StartupPath, "statistics");
            path = Path.Combine(path, name);
            return Load(path) as MeasurementStatistics;
        }

        public static new MeasurementStatistics Load()
        {
            return Load(DateTime.Now);
        }

        public static MeasurementStatistics LoadDefaultGroup()
        {
            MeasurementStatistics appStatistic;
            string filename = "group.sta";
            string path = Path.Combine(Application.StartupPath, "statistics");
            if (Directory.Exists(path))
            {
                path = Path.Combine(path, filename);
                appStatistic = ConfigBase.Load(path) as MeasurementStatistics;
            }
            else
            {
                appStatistic = null;
            }
            return appStatistic;
        }

        public static MeasurementStatistics LoadGroup(string name, DateTime time)
        {
            MeasurementStatistics appStatistic;
            string filename = string.Format("{0}.sta", time.ToString("yyyyMMddHHmmss"));
            string path = Path.Combine(Application.StartupPath, string.Format("statistics\\groups\\{0}\\{1}", name, time.ToString("yyyyMMdd")));
            if (Directory.Exists(path))
            {
                path = Path.Combine(path, filename);
                appStatistic = ConfigBase.Load(path) as MeasurementStatistics;
            }
            else
            {
                appStatistic = null;
            }
            return appStatistic;
        }

        public static void NewDefaultGroup(string name)
        {
            if ((_GroupInstance == null ? false : _GroupInstance.Items.Count > 0))
            {
                _GroupInstance.SaveGroup(name);
            }
            _GroupInstance = new MeasurementStatistics();
            _GroupInstance.SaveDefaultGroup();
        }

        public override bool Save()
        {
            string name = string.Format("{0}.sta", this._Date.ToString("yyyyMMdd"));
            string path = Path.Combine(Application.StartupPath, "statistics");
            return base.Save(Path.Combine(path, name));
        }

        public bool SaveDefaultGroup()
        {
            string filename = "group.sta";
            string path = Path.Combine(Application.StartupPath, "statistics");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, filename);
            return base.Save(path);
        }

        public bool SaveGroup(string name)
        {
            string filename = string.Format("{0}.sta", this._Date.ToString("yyyyMMddHHmmss"));
            string path = Path.Combine(Application.StartupPath, string.Format("statistics\\groups\\{0}\\{1}", name, this._Date.ToString("yyyyMMdd")));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, filename);
            return base.Save(path);
        }

        [Serializable]
        public class StatisticsCollection : CollectionBase
        {
            public StatisticsItem this[int index]
            {
                get
                {
                    return InnerList[index] as StatisticsItem;
                }
            }

            public StatisticsCollection()
            {
            }

            public void Add(StatisticsItem item)
            {
                InnerList.Add(item);
            }
        }

        [Serializable]
        public class StatisticsItem
        {
            private DateTime _StartTime = DateTime.Now;

            private DateTime _EndTime = DateTime.Now;

            private string _Name = null;

            private bool _Result = false;

            public DateTime StartTime
            {
                get
                {
                    return _StartTime;
                }
                set
                {
                    _StartTime = value;
                }
            }

            public DateTime EndTime
            {
                get
                {
                    return _EndTime;
                }
                set
                {
                    _EndTime = value;
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

            public bool Result
            {
                get
                {
                    return _Result;
                }
                set
                {
                    _Result = value;
                }
            }

            public StatisticsItem()
            {
            }
        }

        [Serializable]
        public class StatisticsSideData : ConfigBase
        {
            private StatisticsSideCollection _Sides = new StatisticsSideCollection();

            public StatisticsSideCollection Sides
            {
                get
                {
                    return _Sides;
                }
            }

            public StatisticsSideData()
            {
            }

            private static string GetPath(DateTime time)
            {
                string path = Path.Combine(Application.StartupPath, "statistics");
                path = Path.Combine(path, time.ToString("yyyyMMdd"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, string.Concat(time.ToString("yyyyMMddHHmmss"), ".std"));
                return path;
            }

            public static StatisticsSideData Load(DateTime time)
            {
                return Load(GetPath(time)) as StatisticsSideData;
            }

            public void Save(DateTime time)
            {
                Save(GetPath(time));
            }
        }

        [Serializable]
        public class StatisticsSideCollection : CollectionBase
        {
            public StatisticsSideItem this[int index]
            {
                get
                {
                    return InnerList[index] as StatisticsSideItem;
                }
            }

            public StatisticsSideCollection()
            {
            }

            public void Add(StatisticsSideItem item)
            {
                InnerList.Add(item);
            }

            public StatisticsSideItem GetSide(int side)
            {
                StatisticsSideItem item;
                int i = 0;
                while (true)
                {
                    if (i >= Count)
                    {
                        item = null;
                        break;
                    }
                    else if (this[i].Side != side)
                    {
                        i++;
                    }
                    else
                    {
                        item = this[i];
                        break;
                    }
                }
                return item;
            }
        }

        [Serializable]
        public class StatisticsSideItem
        {
            private int _Side = 0;

            private bool _MeasureDistEnabled = false;

            private double _MeasureFrontDist = 0;

            private double _MeasureBackDist = 0;

            private bool _Result = false;

            private StatisticsSideDataCollection _Datas = new StatisticsSideDataCollection();

            public StatisticsSideDataCollection Datas
            {
                get
                {
                    if (_Datas == null)
                    {
                        _Datas = new StatisticsSideDataCollection();
                    }
                    return _Datas;
                }
            }

            public double MeasureBackDist
            {
                get
                {
                    return this._MeasureBackDist;
                }
                set
                {
                    this._MeasureBackDist = value;
                }
            }

            public bool MeasureDistEnabled
            {
                get
                {
                    return this._MeasureDistEnabled;
                }
                set
                {
                    this._MeasureDistEnabled = value;
                }
            }

            public double MeasureFrontDist
            {
                get
                {
                    return this._MeasureFrontDist;
                }
                set
                {
                    this._MeasureFrontDist = value;
                }
            }

            public bool Result
            {
                get
                {
                    return this._Result;
                }
                set
                {
                    this._Result = value;
                }
            }

            public int Side
            {
                get
                {
                    return this._Side;
                }
                set
                {
                    this._Side = value;
                }
            }

            public StatisticsSideItem()
            {
            }
        }

        [Serializable]
        public class StatisticsSideDataCollection : CollectionBase
        {
            public StatisticsSideDataItem this[int index]
            {
                get
                {
                    return InnerList[index] as StatisticsSideDataItem;
                }
            }

            public StatisticsSideDataCollection()
            {
            }

            public void Add(StatisticsSideDataItem item)
            {
                InnerList.Add(item);
            }
        }

        [Serializable]
        public class StatisticsSideDataItem
        {
            private double _H1 = 0.0;

            private bool _H1Invalid = false;

            private double _H2 = 0.0;

            private bool _H2Invalid = false;

            private double _H3 = 0.0;

            private bool _H3Invalid = false;

            private double _H4 = 0.0;

            private bool _H4Invalid = false;

            private double _D = 0.0;

            private bool _DInvalid = false;

            private double _GH = 0.0;

            private bool _GHInvalid = false;

            private bool _ResultInvalid = false;

            private DetectResults _Result = DetectResults.OK;

            public double H1
            {
                get
                {
                    return _H1;
                }
                set
                {
                    _H1 = value;
                }
            }

            public bool H1Invalid
            {
                get
                {
                    return _H1Invalid;
                }
                set
                {
                    _H1Invalid = value;
                }
            }

            public double H2
            {
                get
                {
                    return _H2;
                }
                set
                {
                    _H2 = value;
                }
            }

            public bool H2Invalid
            {
                get
                {
                    return _H2Invalid;
                }
                set
                {
                    _H2Invalid = value;
                }
            }

            public double H3
            {
                get
                {
                    return _H3;
                }
                set
                {
                    _H3 = value;
                }
            }

            public bool H3Invalid
            {
                get
                {
                    return _H3Invalid;
                }
                set
                {
                    _H3Invalid = value;
                }
            }

            public double H4
            {
                get
                {
                    return _H4;
                }
                set
                {
                    _H4 = value;
                }
            }

            public bool H4Invalid
            {
                get
                {
                    return _H4Invalid;
                }
                set
                {
                    _H4Invalid = value;
                }
            }

            public double D
            {
                get
                {
                    return _D;
                }
                set
                {
                    _D = value;
                }
            }

            public bool DInvalid
            {
                get
                {
                    return _DInvalid;
                }
                set
                {
                    _DInvalid = value;
                }
            }

            public double GH
            {
                get
                {
                    return _GH;
                }
                set
                {
                    _GH = value;
                }
            }

            public bool GHInvalid
            {
                get
                {
                    return _GHInvalid;
                }
                set
                {
                    _GHInvalid = value;
                }
            }

            public bool ResultInvalid
            {
                get
                {
                    return _ResultInvalid;
                }
                set
                {
                    _ResultInvalid = value;
                }
            }

            public DetectResults Result
            {
                get
                {
                    return _Result;
                }
                set
                {
                    _Result = value;
                }
            }

            public string ResultString
            {
                get
                {
                    string result;
                    if (_ResultInvalid)
                    {
                        result = "--";
                    }
                    else if (_Result == DetectResults.OK)
                    {
                        result = "OK";
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        if (CheckResult(DetectResults.W_Max))
                        {
                            sb.Append("胶宽,");
                        }
                        if (CheckResult(DetectResults.W_Min))
                        {
                            sb.Append("胶窄,");
                        }
                        if (CheckResult(DetectResults.H_Max))
                        {
                            sb.Append("胶高,");
                        }
                        if (CheckResult(DetectResults.H_Min))
                        {
                            sb.Append("胶低,");
                        }
                        if (CheckResult(DetectResults.GlueStart))
                        {
                            sb.Append("起始段,");
                        }
                        if (CheckResult(DetectResults.GlueEnd))
                        {
                            sb.Append("结束段,");
                        }
                        result = sb.ToString();
                    }
                    return result;
                }
            }

            private bool CheckResult(DetectResults res)
            {
                return (_Result & res) == res;
            }

            public void From()
            {
                
            }
        }

    }
}
