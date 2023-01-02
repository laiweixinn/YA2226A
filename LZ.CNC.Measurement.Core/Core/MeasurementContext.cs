using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core.Events;
using System.IO.Ports;
using NetWork;
using SciSmtCamLib;
using LZ.CNC.UserLevel;

namespace LZ.CNC.Measurement.Core
{
    public class MeasurementContext
    {       
        private static MeasurementConfig _Config;

        private static MeasurementData _Data;

        private static MeasurementWorker _Worker;

        private static MeasurementAlarms _Alarms;

        private static MeasurementStatistics _Statistics;

        private static MeasurementCapacity _Capacity;

        private static MeasurementMonthCapacity _MonthCapacity;

        private static TcpClientHelper _BendCCDNet;

        private static NetWorkCS _SMCCDNet;

        private static Network _VermesPortNet;

        private static Network _CamNet;

        private static SerialPortEx _CodeSerialPort;

        private static UserManagement _UesrManage;

        static string ptpath = Path.GetFullPath(".") + "\\Stepcfg.ini";
        public static INIHelper inf = new INIHelper(ptpath);
        public static MeasurementMonthCapacity MonthCapacity
        {
            get
            {
                return _MonthCapacity;
            }
        }

        public static MeasurementCapacity Capacity
        {
            get
            {
                return _Capacity;
            }
        }

        public static UserManagement UesrManage
        {
            get
            {
                return _UesrManage;
            }
        }

        public static MeasurementAlarms Alarms
        {
            get
            {
                return _Alarms;
            }
            set
            {
                _Alarms = value;
            }
        }

        public static MeasurementConfig Config
        {
            get
            {
                return _Config;
            }
        }

        public static MeasurementData Data
        {
            get
            {
                return _Data;
            }
        }

        public static MeasurementWorker Worker
        {
            get
            {
                if (_Worker==null)
                {
                    _Worker = new MeasurementWorker();
                }
                return _Worker;
            }
        }


        public static MeasurementStatistics Statistics
        {
            get
            {
                return _Statistics;
            }
            set
            {
                _Statistics = value;
            }
        }


        public static SerialPortEx CodeSerialPort
        {
            get
            {
                return _CodeSerialPort;
            }
        }

        public static TcpClientHelper BendCCDNet
        {
            get
            {
                if(_BendCCDNet==null)
                {
                    _BendCCDNet = new TcpClientHelper();
                }
                return _BendCCDNet;
            }
        }

        private static TcpClientHelper _Bend2CCDNet;
        public static TcpClientHelper Bend2CCDNet
        {
            get
            {
                if(_Bend2CCDNet==null)
                {
                    _Bend2CCDNet = new TcpClientHelper();
                }
                return _Bend2CCDNet;
            }
        }
    
        private static TcpClientHelper _Bend3CCDNet;
        public static TcpClientHelper Bend3CCDNet
        {
            get
            {
                if (_Bend3CCDNet == null)
                {
                    _Bend3CCDNet = new TcpClientHelper();
                }
                return _Bend3CCDNet;
            }
        }

        private static TcpClientHelper _TearCCDNet;
        public static TcpClientHelper TearCCDNet
        {
            get
            {
                if (_TearCCDNet == null)
                {
                    _TearCCDNet = new TcpClientHelper();
                }
                return _TearCCDNet;
            }
        }

        public static Network CamNet
        {
            get
            {
                if(_CamNet==null)
                {
                    _CamNet = new Network(1);
                }
                return _CamNet;
            }
        }

        private static TcpClientHelper _LoadCell1Net;

        public static TcpClientHelper LoadCell1Net
        {
            get
            {
                if (_LoadCell1Net == null)
                {
                    _LoadCell1Net = new  TcpClientHelper();
                }
                return _LoadCell1Net;
            }

        }


        private static TcpClientHelper _LoadCell2Net;

        public static TcpClientHelper LoadCell2Net
        {
            get
            {
                if (_LoadCell2Net == null)
                {
                    _LoadCell2Net = new TcpClientHelper();
                }
                return _LoadCell2Net;
            }
        }


        private static TcpClientHelper _LoadCell3Net;
        public static TcpClientHelper LoadCell3Net
        {
            get
            {
                if (_LoadCell3Net == null)
                {
                    _LoadCell3Net = new TcpClientHelper();
                }
                return _LoadCell3Net;
            }
        }

        private static TcpClientHelper _QRCodeNet;       
        public static TcpClientHelper QRCodeNet
        {
            get
            {
                if(_QRCodeNet==null)
                {
                    _QRCodeNet = new TcpClientHelper();
                }
                return _QRCodeNet;
            }
        }




        private static TcpClientHelper _TestNet;
        public static  TcpClientHelper  TestNet
        {
            get
            {
                if(_TestNet==null)
                {
                    _TestNet = new TcpClientHelper();
                }
                return _TestNet;
            }
        }

        static MeasurementContext()
        {
            _UesrManage = new UserManagement();
            _Config = null;
            _Data = null;
            _Worker = null;          
            _Alarms = null;
            _Statistics = null;
            _CodeSerialPort = null;
      
            _CamNet = null;
            _VermesPortNet = null;           
            _Capacity = null;
            _MonthCapacity = null;
            _BendCCDNet = null;

            _TestNet = null;
        }

        public static void Init()
        {

         
            _UesrManage.InitPassword();
            _Worker = new MeasurementWorker();
            string path = Path.Combine(Application.StartupPath, "set");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(Application.StartupPath, "visionfile");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(Application.StartupPath, "alarms");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(Application.StartupPath, "statistics");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(Application.StartupPath, "statistics\\groups");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(Application.StartupPath, "statistics\\capacity");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            _Config = MeasurementConfig.Load();
            if (_Config==null)
            {
                _Config = new MeasurementConfig();
            }

            _Data = MeasurementData.Load();
            if (_Data == null)
            {
                _Data = new MeasurementData();
            }

            _Capacity = MeasurementCapacity.Load();
            if (_Capacity==null)
            {
                _Capacity = new MeasurementCapacity();
            }

            _MonthCapacity = MeasurementMonthCapacity.Load();
            if(_MonthCapacity==null)
            {
                _MonthCapacity = new MeasurementMonthCapacity();
            }


            


            _Alarms = MeasurementAlarms.Load();
            if (_Alarms==null)
            {
                _Alarms = new MeasurementAlarms();
            }

            _Statistics = MeasurementStatistics.Load();
            if (_Statistics==null)
            {
                _Statistics = new MeasurementStatistics();
            }
           

           
            #region
            //_WorkEx = new MeasurementWorkEx();
            //_CodeSerialPort = new SerialPortEx(_Config.CodeComNum, 115200, Parity.None, 8, StopBits.One);

            //List<string> vs = new List<string>();

            //foreach (string com in SerialPort.GetPortNames())
            //{
            //    vs.Add(com);
            //}

            //if (vs.Contains(_Config.CodeComNum))
            //{
            //    _CodeSerialPort.Connent();
            //}

            //_PointLaserNet = new Network(1);
            //_CamNet = new Network(1);
            //_VermesPortNet = new Network(1);          
            //_CamSciEngine = new SciEngine();
            #endregion
        }

       


       
        public static void OnMessageOutput(string msg, bool iserror, bool issave = false)
        {
            if ((!issave ? false : !string.IsNullOrEmpty(msg)))
            {
                MeasurementAlarms.Add(msg);
            }
            if (MessageOutput != null)
            {
                MessageOutputEventArgs me = new MessageOutputEventArgs(msg, iserror);
                MessageOutput(null, me);
            }
        }

        public static void OutputError(string msg,bool issave =false)
        {
            OnMessageOutput(msg, true, issave);
        }

        public static void OutputMessage(string msg)
        {
            OnMessageOutput(msg, false);
        }

        public static event EventHandler MessageOutput;
    }
}
