﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace LZ.CNC.Measurement.Core
{
    public partial class TcpClientHelper : Component
    {

        public enum SocketState
        {
            /// <summary>
            /// 正在连接
            /// </summary>
            Connecting = 0,

            /// <summary>
            /// 已连接
            /// </summary>
            Connected = 1,

            /// <summary>
            /// 重新连接
            /// </summary>
            Reconnection = 2,

            /// <summary>
            /// 断开连接
            /// </summary>
            Disconnect = 3,

            /// <summary>
            /// 正在监听
            /// </summary>
            StartListening = 4,

            /// <summary>
            /// 停止监听
            /// </summary>
            StopListening = 5,

            /// <summary>
            /// 客户端上线
            /// </summary>
            ClientOnline = 6,

            /// <summary>
            /// 客户端下线
            /// </summary>
            ClientOnOff = 7
        }
        public TcpClientHelper()
        {
            InitializeComponent();
        }

        public TcpClientHelper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #region 属性
        /// <summary>
        /// 服务端IP
        /// </summary>
        private string _ip;
        [Description("服务端IP")]
        [Category("TcpClient属性")]
        public string Ip
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 服务端监听端口
        /// </summary>
        private int _port;
        [Description("服务端监听端口")]
        [Category("TcpClient属性")]
        public int Port
        {
            set { _port = value; }
            get { return _port; }
        }
        /// <summary>
        /// TcpClient客户端
        /// </summary>
        private TcpClient _tcpclient = null;
        [Description("TcpClient操作类")]
        [Category("TcpClient隐藏属性")]
        [Browsable(false)]
        public TcpClient Tcpclient
        {
            set { _tcpclient = value; }
            get { return _tcpclient; }
        }
        /// <summary>
        /// Tcp客户端连接线程
        /// </summary>
        private Thread _tcpthread = null;
        [Description("TcpClient连接服务端线程")]
        [Category("TcpClient隐藏属性")]
        [Browsable(false)]
        public Thread Tcpthread
        {
            set { _tcpthread = value; }
            get { return _tcpthread; }
        }
        /// <summary>
        /// 是否启动Tcp连接线程
        /// </summary>
        private bool _isStarttcpthreading = false;
        [Description("是否启动Tcp连接线程")]
        [Category("TcpClient隐藏属性")]
        [Browsable(false)]
        public bool IsStartTcpthreading
        {
            set { _isStarttcpthreading = value; }
            get { return _isStarttcpthreading; }
        }
        /// <summary>
        /// 连接是否关闭（用来断开重连）
        /// </summary>
        private bool _isclosed = false;
        [Description("连接是否关闭（用来断开重连）")]
        [Category("TcpClient属性")]
        public bool Isclosed
        {
            set { _isclosed = value; }
            get { return _isclosed; }
        }

        private int _reConnectionTime = 3000;
        /// <summary>
        /// 设置断开重连时间间隔单位（毫秒）（默认3000毫秒）
        /// </summary>
        [Description("设置断开重连时间间隔单位（毫秒）（默认3000毫秒）")]
        [Category("TcpClient属性")]
        public int ReConnectionTime
        {
            get { return _reConnectionTime; }
            set { _reConnectionTime = value; }
        }
        private string _receivestr;
        /// <summary>
        ///  接收Socket数据包 缓存字符串
        /// </summary>
        [Description("接收Socket数据包 缓存字符串")]
        [Category("TcpClient隐藏属性"), Browsable(false)]
        public string Receivestr
        {
            set { _receivestr = value; }
            get { return _receivestr; }
        }
        /// <summary>
        /// 重连次数
        /// </summary>
        private int _reConectedCount = 0;
        [Description("重连次数")]
        [Category("TcpClient隐藏属性"), Browsable(false)]
        public int ReConectedCount
        {
            get { return _reConectedCount; }
            set { _reConectedCount = value; }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 启动连接Socket服务器
        /// </summary>
        public void StartConnection()
        {
            try
            {
                //Isclosed = false;
                CreateTcpClient();
            }
            catch (Exception ex)
            {
                OnTcpClientErrorMsgEnterHead("错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 创建线程连接
        /// </summary>
        private void CreateTcpClient()
        {

            if (Isclosed)
                return;
            //标示已启动连接，防止重复启动线程
            Isclosed = true;
            Tcpclient = new TcpClient();
            Tcpthread = new Thread(StartTcpThread);
            IsStartTcpthreading = true;
            Tcpthread.Start();
        }

        public bool IsConnected
        {
            get
            {
                return Tcpclient.Connected;
            }
        }
        /// <summary>
        ///  线程接收Socket上传的数据
        /// </summary>
        private void StartTcpThread()
        {
            byte[] receivebyte = new byte[1024];
            int bytelen;
            try
            {
                while (IsStartTcpthreading)
                #region
                {
                    if (!Tcpclient.Connected)
                    {
                        try
                        {
                            if (ReConectedCount != 0)
                            {
                                //返回状态信息
                                OnTcpClientStateInfoEnterHead(string.Format("正在第{0}次重新连接服务器... ...", ReConectedCount), SocketState.Reconnection);
                            }
                            else
                            {
                                //SocketStateInfo
                                OnTcpClientStateInfoEnterHead("正在连接服务器... ...", SocketState.Connecting);
                            }
                            Tcpclient.Connect(IPAddress.Parse(Ip), Port);
                            OnTcpClientStateInfoEnterHead("已连接服务器", SocketState.Connected);
                            //Tcpclient.Client.Send(Encoding.Default.GetBytes("login"));
                        }
                        catch
                        {
                            //连接失败
                            ReConectedCount++;
                            //强制重新连接
                            Isclosed = false;
                            IsStartTcpthreading = false;
                            //每三秒重连一次
                            Thread.Sleep(ReConnectionTime);
                            continue;
                        }
                    }
                    //Tcpclient.Client.Send(Encoding.Default.GetBytes("login"));
                    bytelen = Tcpclient.Client.Receive(receivebyte);
                    // 连接断开
                    if (bytelen == 0)
                    {
                        //返回状态信息
                        OnTcpClientStateInfoEnterHead("与服务器断开连接... ...", SocketState.Disconnect);
                        // 异常退出、强制重新连接
                        Isclosed = false;
                        ReConectedCount = 1;
                        IsStartTcpthreading = false;
                        continue;
                    }

                    Receivestr = ASCIIEncoding.Default.GetString(receivebyte, 0, bytelen);
                    if (Receivestr.Trim() != "")
                    {
                        byte[] bytes = new byte[bytelen];
                        Array.Copy(receivebyte, 0, bytes, 0, bytelen);
                        //接收Byte原始数据
                        OnTcpClientReceviceByte(bytes);
                        //接收数据
                        //OnTcpClientReceviceEnterHead(Receivestr);
                    }
                }
                #endregion
                //此时线程将结束，人为结束，自动判断是否重连
                CreateTcpClient();
            }
            catch (Exception ex)
            {
                //CreateTcpClient();
                //返回错误信息
                Isclosed = false;
                ReConectedCount = 1;
                IsStartTcpthreading = false;
                OnTcpClientErrorMsgEnterHead("错误信息：" + ex.Message);

                if (Tcpthread.IsAlive)
                {
                    Tcpthread.Abort();
                }
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void StopConnection()
        {

            IsStartTcpthreading = false;
            Isclosed = false;
            if (Tcpclient != null)
            {
                //关闭连接
                Tcpclient.Close();
            }
            if (Tcpthread != null)
            {
                Tcpthread.Interrupt();
                //关闭线程
                Tcpthread.Abort();
                //Tcpthread = null;
            }
            OnTcpClientStateInfoEnterHead("断开连接", SocketState.Disconnect);
            //标示线程已关闭可以重新连接
        }

        /// <summary>
        /// 发送Socket文本消息
        /// </summary>
        /// <param name="cmdstr"></param>
        public bool SendCommand(string cmdstr)
        {
            try
            {
                //  int X=Encoding.ASCII.
                byte[] _out = Encoding.GetEncoding("GBK").GetBytes(cmdstr);
                // string str = '\u0002' + "011RWT01\r\n";
                // byte[] _out = Encoding.ASCII.GetBytes(str);
              //  byte[] outbyte = new byte[] {0x01, 0x04, 0x0000, 0x0002, 0x71, 0xCB};
                if (Tcpclient.Client.Send(_out) > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //返回错误信息
                OnTcpClientErrorMsgEnterHead(ex.Message);
                return false;
            }
        }

        public void SendFile(string filename)
        {
            Tcpclient.Client.BeginSendFile(filename,
                   new AsyncCallback(SendCallback), Tcpclient);
            //Tcpclient.Client.SendFile(filename);
        }
        private void SendCallback(IAsyncResult result)
        {
            try
            {
                TcpClient tc = (TcpClient)result.AsyncState;
                // Complete sending the data to the remote device.
                tc.Client.EndSendFile(result);

            }
            catch (SocketException ex)
            {
            }
        }
        /// <summary>
        /// 发送Socket消息
        /// </summary>
        /// <param name="byteMsg"></param>
        public bool SendCommand(byte[] byteMsg)
        {
            try
            {
                if (Tcpclient.Client.Send(byteMsg) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                //返回错误信息

                OnTcpClientErrorMsgEnterHead("错误信息：" + ex.Message);
                return false;
            }
        }
        #endregion

        #region 事件
        #region OnRecevice接收数据事件
        public delegate void ReceviceEventHandler(string msg);
        [Description("接收数据事件")]
        [Category("TcpClient事件")]
        public event ReceviceEventHandler OnRecevice;
        protected virtual void OnTcpClientRecevice(string msg)
        {
            if (OnRecevice != null)
                OnRecevice("收到消息：" + msg);
        }

        public delegate void ReceviceByteEventHandler(byte[] date);
        [Description("接收Byte数据事件")]
        [Category("TcpClient事件")]
        public event ReceviceByteEventHandler OnReceviceByte;
        protected virtual void OnTcpClientReceviceByte(byte[] date)
        {
            if (OnReceviceByte != null)
                OnReceviceByte(date);
        }
        #endregion

        #region OnErrorMsg返回错误消息事件
        public delegate void ErrorMsgEventHandler(string msg);
        [Description("返回错误消息事件")]
        [Category("TcpClient事件")]
        public event ErrorMsgEventHandler OnErrorMsg;
        protected virtual void OnTcpClientErrorMsgEnterHead(string msg)
        {
            if (OnErrorMsg != null)
                OnErrorMsg(msg);
        }
        #endregion

        #region OnStateInfo连接状态改变时返回连接状态事件
        public delegate void StateInfoEventHandler(string msg, SocketState state);
        [Description("连接状态改变时返回连接状态事件")]
        [Category("TcpClient事件")]
        public event StateInfoEventHandler OnStateInfo;
        protected virtual void OnTcpClientStateInfoEnterHead(string msg, SocketState state)
        {
            if (OnStateInfo != null)
                OnStateInfo(msg, state);
        }
        #endregion
        #endregion


    }
}
