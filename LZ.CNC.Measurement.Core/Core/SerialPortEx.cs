using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Threading;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Events;

namespace LZ.CNC.Measurement.Core
{
    public class SerialPortEx:SerialPort
    {
        private static object objLock = new object();

        private string _StrRec = string.Empty;

        public string StrRec
        {
            get
            {
                return _StrRec;
            }
            set
            {
                _StrRec = value;
            }
        }

        public SerialPortEx(string portname,int baudrate, Parity parity, int databits, StopBits stopbits)
        {
            PortName =portname;
            BaudRate = baudrate;
            Parity = parity;
            DataBits = databits;
            StopBits = stopbits;
            ReadBufferSize = 1024;
            WriteBufferSize = 1024;
        }

        public bool Connent()
        {
            bool result;
            if (!IsOpen)
            {
                try
                {
                    Open();
                    ReadTimeout = 100;
                    DataReceived += PointLaserSerialPort_DataReceived;
                    result = true;
                    OutPutMessage(string.Format("{0}打开成功!",PortName));
                }
                catch (Exception ex)
                {
                    result = false;
                    MessageBox.Show(string.Format("Error[{0}]", ex.Message));
                    //OutPutError(string.Format("Error[{0}]", ex.Message));
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        public bool DisConnect()
        {
            bool result;
            if (IsOpen)
            {
                try
                {
                    DataReceived -= PointLaserSerialPort_DataReceived;
                    Close();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    string.Format("Error[{0}]", ex.Message);
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        public bool Send(string msg)
        {
            bool result;
            try
            {
                DiscardOutBuffer();
                lock (objLock)
                {
                    _StrRec = string.Empty;
                    WriteLine(msg);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                string.Format("Error[{0}]", ex.Message);
            }
            return result;

        }

        private void PointLaserSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _StrRec = ReadExisting();
        }

        public event EventHandler MessageOutPut;

        protected void OnMessageOutPut(string msg,bool error)
        {
            if (MessageOutPut!=null)
            {
                MessageOutPut(this, new MessageOutputEventArgs(msg,error));
            }
        }

        protected void OutPutError (string msg)
        {
            MeasurementContext.OutputError(msg);
        }

       protected  void OutPutMessage(string msg)
        {
            MeasurementContext.OutputMessage(msg);
        }
    }
}
