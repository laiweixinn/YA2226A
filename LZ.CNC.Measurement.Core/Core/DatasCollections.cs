using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using DY.Core.Configs;


namespace LZ.CNC.Measurement.Core
{

  [Serializable]
  public  class DatasCollections:ConfigBase
    {
        private List<DetectDataItem> _DetectDatas;

        public List<DetectDataItem> DetectDatas
        {
            get
            {
                if (_DetectDatas == null)
                {
                    _DetectDatas = new List<DetectDataItem>();
                }
                return _DetectDatas;
            }
        }

        public DetectDataItem this[string panelid]
        {
            get
            {
                DetectDataItem item = null;
                for (int i = 0; i < _DetectDatas.Count; i++)
                {
                    if (_DetectDatas[i].PanelID == panelid)
                    {
                        item = _DetectDatas[i];
                        break;
                    }
                }
                return item;
            }
        }

        [Serializable]
        public class DetectDataItemCollection : CollectionBase
        {
            private List<DetectDataItem> _DetectDatas;
            public List<DetectDataItem> DetectDatas
            {
                get
                {
                    if (_DetectDatas == null)
                    {
                        _DetectDatas = new List<DetectDataItem>();
                    }
                    return _DetectDatas;
                }
            }

            public DetectDataItem this[int index]
            {
                get
                {
                    return InnerList[index] as DetectDataItem;
                }
            }

            public DetectDataItem this[string panelid]
            {
                get
                {
                    DetectDataItem item = null;
                    for (int i = 0; i < _DetectDatas.Count; i++)
                    {
                        if (_DetectDatas[i].PanelID == panelid)
                        {
                            item = _DetectDatas[i];
                            break;
                        }
                    }
                    return item;
                }
            }

            public void Add(DetectDataItem dataItem)
            {
                InnerList.Add(dataItem);
            }

            public void Remove(DetectDataItem dataItem)
            {
                InnerList.Remove(dataItem);
            }

            public void IndexOf(DetectDataItem dataItem)
            {
                InnerList.IndexOf(dataItem);
            }

            public DetectDataItemCollection()
            {
            }
        }

        [Serializable]
        public class DetectDataItem
        {
            private DateTime _DetectTime;

            private string _PanelID;

            private string _StationID;

            private double _AOIY1;

            private double _AOIY2;
           
            private string _Result;

            private string _Reason;

            private double _Weighval;

         

            public DateTime DetectTime
            {
                get
                {
                    return _DetectTime;
                }

                set
                {
                    _DetectTime = value;
                }
            }

            public string PanelID
            {
                get
                {
                    return _PanelID;
                }

                set
                {
                    _PanelID = value;
                }
            }

            public string StationID
            {
                get
                {
                    return _StationID;
                }

                set
                {
                    _StationID = value;
                }
            }

            public double AOIY1
            {
                get
                {
                    return _AOIY1;
                }

                set
                {
                    _AOIY1 = value;
                }
            }

            public double AOIY2
            {
                get
                {
                    return _AOIY2;
                }

                set
                {
                    _AOIY2 = value;
                }
            }

        

            public string Result
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


            public string Reason
            {
                get
                {
                    return _Reason;
                }
                set
                {
                    _Reason = value;
                }
            }

            public double Weighval
            {
                get
                {
                    return _Weighval;
                }

                set
                {
                    _Weighval = value;
                }
            }

            public DetectDataItem(string panelid, string stationid, double y1, Double y2, string result,string reason,double weighval)
            {
                _DetectTime = DateTime.Now;
                _PanelID = panelid;
                _StationID = stationid;
                _AOIY1 = y1;
                _AOIY2 = y2;
                _Result = result;
                _Reason = reason;
                _Weighval = weighval;
            }
        }

        public new static DatasCollections Load()
        {
            string path = System.IO.Path.Combine(GetApplicationPath("detectdatas"), string.Format("{0}.dds", DateTime.Now.ToString("yyyy-MM-dd")));
            return Load(path) as DatasCollections;
        }

        public override bool Save()
        {
            string path = System.IO.Path.Combine(GetApplicationPath("detectdatas"));
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            path = System.IO.Path.Combine(GetApplicationPath("detectdatas"), string.Format("{0}.dds", DateTime.Now.ToString("yyyy-MM-dd")));
            return Save(path);
        }

    }
}
