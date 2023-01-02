using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LZ.CNC.Measurement.Core
{
   public  class NetWorkCS : IDisposable
    {
        private int memory;

        private bool IsDisposed = false;

        private string UDP_strMsgRec = string.Empty;

        private Thread UDP_thread = null;

        private Socket UDP_sock = null;

        private EndPoint RemoteEP = null;

        private IntPtr C_Owner = IntPtr.Zero;

        private string C_strMsgRec = string.Empty;

        private Thread C_threadClient = null;

        

        private Socket C_sockClient = null;

        private bool C_sendFile = false;

        private IntPtr S_Owner = IntPtr.Zero;

        private string strMsgRec = string.Empty;

        private Thread S_threadWatch = null;

        private Socket S_socketWatch = null;

        private Dictionary<string, Socket> S_dict = new Dictionary<string, Socket>();

        private Dictionary<string, Thread> S_dictThread = new Dictionary<string, Thread>();

        private List<string> ServerRemoteEndPoint = new List<string>();

        private bool S_sendFile = false;

        private bool _C_ReadData;

        public bool C_ReadData
        {
            get
            {
                return _C_ReadData;
            }
            set
            {
                _C_ReadData = value;
            }
        }

        public string ClientRecStr
        {
            get
            {
                return C_strMsgRec;
            }
            set
            {
                C_strMsgRec = value;
            }
        }

        public bool ClientSendFile
        {
            get
            {
                return C_sendFile;
            }
            set
            {
                C_sendFile = value;
            }
        }

        public List<string> ServerEndPoint
        {
            get
            {
                List<string> serverRemoteEndPoint;
                if (ServerRemoteEndPoint.Count <= 0)
                {
                    serverRemoteEndPoint = null;
                }
                else
                {
                    serverRemoteEndPoint = ServerRemoteEndPoint;
                }
                return serverRemoteEndPoint;
            }
        }

        public string ServerRecStr
        {
            get
            {
                return strMsgRec;
            }
            set
            {
                strMsgRec = value;
            }
        }

        public bool ServerSendFile
        {
            get
            {
                return S_sendFile;
            }
            set
            {
                S_sendFile = value;
            }
        }

        public string UdpRecStr
        {
            get
            {
                return UDP_strMsgRec;
            }
            set
            {
                UDP_strMsgRec = value;
            }
        }

        public NetWorkCS(int Memory)
        {
            memory = Memory;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public bool IsClientConnected()
        {
            if (C_sockClient != null)
            {
                return C_sockClient.Connected;
            }
            else
            {
                return false;
            }
        }

        private void C_RecMsg()
        {
            string str;
            while (true)
            {
                Thread.Sleep(1);
                byte[] numArray = new byte[1048576 * memory];
                int num = -1;
                try
                {
                    num = C_sockClient.Receive(numArray);
                }
                catch
                {
                    break;
                }
                if (num > 0)
                {
                    if (C_sendFile)
                    {
                        if (numArray[0] == 0)
                        {
                            str = Encoding.UTF8.GetString(numArray, 1, num - 1);
                            C_strMsgRec = str;
                        }
                        if (numArray[0] == 1)
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog()
                            {
                                Filter = "All Files Format (*.*)|*.*",
                                FileName = C_strMsgRec
                            };
                            if (saveFileDialog.ShowDialog(new NetWorkCS.WindowWrapper(C_Owner)) == DialogResult.OK)
                            {
                                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                                try
                                {
                                    fileStream.Write(numArray, 1, num - 1);
                                }
                                finally
                                {
                                    if (fileStream != null)
                                    {
                                        ((IDisposable)fileStream).Dispose();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        str = Encoding.Default.GetString(numArray, 0, num);
                        C_strMsgRec = str;
                        _C_ReadData = true;
                    }
                }
            }
        }

        public void CloseTcpClient()
        {
            C_Owner = IntPtr.Zero;
            if ((C_threadClient == null ? false : C_threadClient.ThreadState != ThreadState.Aborted))
            {
                C_threadClient.Abort();
                C_threadClient = null;
            }
            Thread.Sleep(10);
            if (C_sockClient != null)
            {
                if (C_sockClient.Connected)
                {
                    C_sockClient.Shutdown(SocketShutdown.Both);
                }
                C_sockClient.Close();
                C_sockClient = null;
            }
        }

        public void CloseTcpServer()
        {
            int i;
            try
            {
                S_Owner = IntPtr.Zero;
                if ((S_threadWatch == null ? false : S_threadWatch.ThreadState != ThreadState.Aborted))
                {
                    S_threadWatch.Abort();
                    S_threadWatch = null;
                }
                if (S_dictThread.Count > 0)
                {
                    for (i = 0; i < S_dictThread.Count; i++)
                    {
                        S_dictThread[ServerRemoteEndPoint[i]].Abort();
                    }
                }
                Thread.Sleep(20);
                if (S_socketWatch != null)
                {
                    if (S_socketWatch.Connected)
                    {
                        S_socketWatch.Shutdown(SocketShutdown.Both);
                    }
                    S_socketWatch.Close();
                    S_socketWatch = null;
                }
                if (S_dict.Count > 0)
                {
                    for (i = 0; i < S_dict.Count; i++)
                    {
                        S_dict[ServerRemoteEndPoint[i]].Shutdown(SocketShutdown.Both);
                        S_dict[ServerRemoteEndPoint[i]].Close();
                    }
                }
                S_dict.Clear();
                S_dictThread.Clear();
                ServerRemoteEndPoint.Clear();
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("E0", exception.Message));
            }
        }

        public void CloseUdp()
        {
            if ((UDP_thread == null ? false : UDP_thread.ThreadState != ThreadState.Aborted))
            {
                UDP_thread.Abort();
                UDP_thread = null;
            }
            Thread.Sleep(10);
            if (RemoteEP != null)
            {
                RemoteEP = null;
            }
            if (UDP_sock != null)
            {
                if (UDP_sock.Connected)
                {
                    UDP_sock.Shutdown(SocketShutdown.Both);
                }
                UDP_sock.Close();
                UDP_sock = null;
            }
        }

        public void Dispose()
        {
            Control.CheckForIllegalCrossThreadCalls = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool Diposing)
        {
            if (!IsDisposed)
            {
                if (Diposing)
                {
                }
                CloseTcpClient();
                CloseTcpServer();
                CloseUdp();
            }
            IsDisposed = true;
        }

        ~NetWorkCS()
        {
            Dispose(false);
        }

        public string GetAddressIP()
        {
            string empty = string.Empty;
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            for (int i = 0; i < addressList.Length; i++)
            {
                IPAddress pAddress = addressList[i];
                if (pAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    empty = pAddress.ToString();
                }
            }
            return empty;
        }

        public bool GetTcpClientStatus()
        {
            bool flag;
            flag = (C_sockClient == null ? true : C_sockClient.Poll(10, SelectMode.SelectRead));
            return flag;
        }

        public bool GetTcpServerStatus()
        {
            bool flag;
            flag = (S_socketWatch == null ? true : S_socketWatch.Poll(10, SelectMode.SelectWrite));
            return flag;
        }

        public bool GetUdpStatus()
        {
            bool flag;
            flag = (UDP_sock == null ? true : UDP_sock.Poll(10, SelectMode.SelectRead));
            return flag;
        }

        public IPAddress getValidIP(string ip)
        {
            IPAddress pAddress;
            IPAddress pAddress1 = null;
            try
            {
                if (!IPAddress.TryParse(ip, out pAddress1))
                {
                    throw new ArgumentException("IP无效");
                }
            }
            catch (Exception exception)
            {
                pAddress = null;
                return pAddress;
            }
            pAddress = pAddress1;
            return pAddress;
        }

        public int getValidPort(string port)
        {
            int num;
            int num1;
            try
            {
                if (port == "")
                {
                    throw new ArgumentException("端口号无效");
                }
                num = Convert.ToInt32(port);
            }
            catch (Exception exception)
            {
                num1 = -1;
                return num1;
            }
            num1 = num;
            return num1;
        }

        private void S_RecMsg(object sokConnectionparn)
        {
            string str;
            Socket socket = sokConnectionparn as Socket;
            while (true)
            {
                Thread.Sleep(1);
                byte[] numArray = new byte[1048576 * memory];
                int num = -1;
                try
                {
                    num = socket.Receive(numArray);
                }
                catch (SocketException socketException)
                {
                    S_dict.Remove(socket.RemoteEndPoint.ToString());
                    S_dictThread.Remove(socket.RemoteEndPoint.ToString());
                    ServerRemoteEndPoint.Remove(socket.RemoteEndPoint.ToString());
                    break;
                }
                if (num > 0)
                {
                    if (S_sendFile)
                    {
                        if (numArray[0] == 0)
                        {
                            str = Encoding.UTF8.GetString(numArray, 1, num - 1);
                            strMsgRec = str;
                        }
                        if (numArray[0] == 1)
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog()
                            {
                                Filter = "All Files Format (*.*)|*.*",
                                FileName = strMsgRec
                            };
                            if (saveFileDialog.ShowDialog(new NetWorkCS.WindowWrapper(S_Owner)) == DialogResult.OK)
                            {
                                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                                try
                                {
                                    fileStream.Write(numArray, 1, num - 1);
                                }
                                finally
                                {
                                    if (fileStream != null)
                                    {
                                        ((IDisposable)fileStream).Dispose();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        str = Encoding.Default.GetString(numArray, 0, num);
                        strMsgRec = str;
                    }
                }
            }
        }

        public bool tcpClientConnet(string ip, string port, IntPtr handle)
        {
            bool flag;
            if (GetTcpClientStatus())
            {
                CloseTcpClient();
                C_Owner = handle;
                IPAddress pAddress = IPAddress.Parse(ip);
                IPEndPoint pEndPoint = new IPEndPoint(pAddress, int.Parse(port));
                C_sockClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    C_sockClient.Connect(pEndPoint);

                }
                catch
                {
                    flag = false;
                    return flag;
                }
                C_threadClient = new Thread(new ThreadStart(C_RecMsg))
                {
                    IsBackground = true
                };
                C_threadClient.Start();
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public bool tcpClientSendData(string sendMsg, bool CRLF)
        {
            bool flag;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sendMsg);
                byte[] numArray = new byte[] { 13, 10 };
                byte[] numArray1 = new byte[(int)bytes.Length + (int)numArray.Length];
                Buffer.BlockCopy(bytes, 0, numArray1, 0, (int)bytes.Length);
                Buffer.BlockCopy(numArray, 0, numArray1, (int)bytes.Length, (int)numArray.Length);
                if (!CRLF)
                {
                    C_sockClient.Send(bytes);
                }
                else
                {
                    C_sockClient.Send(numArray1);
                }
                _C_ReadData = false;
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public bool tcpClientSendFile()
        {
            bool flag;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(openFileDialog.FileName))
                    {
                        FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                        try
                        {
                            string fileName = Path.GetFileName(openFileDialog.FileName);
                            byte[] bytes = Encoding.UTF8.GetBytes(fileName);
                            byte[] numArray = new byte[(int)bytes.Length + 1];
                            numArray[0] = 0;
                            Buffer.BlockCopy(bytes, 0, numArray, 1, (int)bytes.Length);
                            C_sockClient.Send(numArray);
                            byte[] numArray1 = new byte[1048576 * memory];
                            int num = fileStream.Read(numArray1, 0, (int)numArray1.Length);
                            byte[] numArray2 = new byte[num + 1];
                            numArray2[0] = 1;
                            Buffer.BlockCopy(numArray1, 0, numArray2, 1, num);
                            C_sockClient.Send(numArray2);
                            flag = true;
                            return flag;
                        }
                        finally
                        {
                            if (fileStream != null)
                            {
                                ((IDisposable)fileStream).Dispose();
                            }
                        }
                    }
                }
                flag = false;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public bool tcpServerListen(string ip, string port, int backlog, IntPtr handle)
        {
            bool flag;
            if (GetTcpServerStatus())
            {
                CloseTcpServer();
                S_Owner = handle;
                S_socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress pAddress = IPAddress.Parse(ip);
                IPEndPoint pEndPoint = new IPEndPoint(pAddress, int.Parse(port));
                try
                {
                    S_socketWatch.Bind(pEndPoint);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    flag = false;
                    return flag;
                }
                if (backlog <= 0)
                {
                    S_socketWatch.Listen(10);
                }
                else
                {
                    S_socketWatch.Listen(backlog);
                }
                S_threadWatch = new Thread(new ThreadStart(WatchConnecting))
                {
                    IsBackground = true
                };
                S_threadWatch.Start();
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public bool tcpServerSendDataToAll(string sendMsg, bool CRLF)
        {
            bool flag;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sendMsg);
                byte[] numArray = new byte[] { 13, 10 };
                byte[] numArray1 = new byte[(int)bytes.Length + (int)numArray.Length];
                Buffer.BlockCopy(bytes, 0, numArray1, 0, (int)bytes.Length);
                Buffer.BlockCopy(numArray, 0, numArray1, (int)bytes.Length, (int)numArray.Length);
                foreach (Socket value in S_dict.Values)
                {
                    if (!CRLF)
                    {
                        value.Send(bytes);
                    }
                    else
                    {
                        value.Send(numArray1);
                    }
                }
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public bool tcpServerSendDataToOne(string endPoint, string sendMsg, bool CRLF)
        {
            bool flag;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sendMsg);
                byte[] numArray = new byte[] { 13, 10 };
                byte[] numArray1 = new byte[(int)bytes.Length + (int)numArray.Length];
                Buffer.BlockCopy(bytes, 0, numArray1, 0, (int)bytes.Length);
                Buffer.BlockCopy(numArray, 0, numArray1, (int)bytes.Length, (int)numArray.Length);
                if (string.IsNullOrEmpty(endPoint))
                {
                    flag = false;
                }
                else
                {
                    if (!CRLF)
                    {
                        S_dict[endPoint].Send(bytes);
                    }
                    else
                    {
                        S_dict[endPoint].Send(numArray1);
                    }
                    flag = true;
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public bool tcpServerSendFile(string endPoint)
        {
            bool flag;
            try
            {
                if (!string.IsNullOrEmpty(endPoint))
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog()
                    {
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    };
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (!string.IsNullOrEmpty(openFileDialog.FileName))
                        {
                            FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                            try
                            {
                                string fileName = Path.GetFileName(openFileDialog.FileName);
                                byte[] bytes = Encoding.UTF8.GetBytes(fileName);
                                byte[] numArray = new byte[(int)bytes.Length + 1];
                                numArray[0] = 0;
                                Buffer.BlockCopy(bytes, 0, numArray, 1, (int)bytes.Length);
                                S_dict[endPoint].Send(numArray);
                                byte[] numArray1 = new byte[1048576 * memory];
                                int num = fileStream.Read(numArray1, 0, (int)numArray1.Length);
                                byte[] numArray2 = new byte[num + 1];
                                numArray2[0] = 1;
                                Buffer.BlockCopy(numArray1, 0, numArray2, 1, num);
                                S_dict[endPoint].Send(numArray2);
                                flag = true;
                                return flag;
                            }
                            finally
                            {
                                if (fileStream != null)
                                {
                                    ((IDisposable)fileStream).Dispose();
                                }
                            }
                        }
                    }
                }
                flag = false;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        private void UDP_RecMsg()
        {
            while (true)
            {
                Thread.Sleep(1);
                byte[] numArray = new byte[1048576 * memory];
                if ((UDP_sock == null ? false : UDP_sock.Available >= 1))
                {
                    int num = -1;
                    try
                    {
                        num = UDP_sock.ReceiveFrom(numArray, ref RemoteEP);
                    }
                    catch
                    {
                        break;
                    }
                    if (num > 0)
                    {
                        string str = Encoding.Default.GetString(numArray, 0, num);
                        UDP_strMsgRec = str;
                    }
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        }

        public bool UdpSendData(string sendMsg, bool CRLF)
        {
            bool flag;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sendMsg);
                byte[] numArray = new byte[] { 13, 10 };
                byte[] numArray1 = new byte[(bytes.Length + (int)numArray.Length)];
                Buffer.BlockCopy(bytes, 0, numArray1, 0, (int)bytes.Length);
                Buffer.BlockCopy(numArray, 0, numArray1, (int)bytes.Length, (int)numArray.Length);
                if (!CRLF)
                {
                    UDP_sock.SendTo(bytes, (int)bytes.Length, SocketFlags.None, RemoteEP);
                }
                else
                {
                    UDP_sock.SendTo(numArray1, (int)numArray1.Length, SocketFlags.None, RemoteEP);
                }
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public bool UpdConnet(string ip, string port, string ipRemote, string portRemote)
        {
            bool flag;
            if (GetUdpStatus())
            {
                CloseUdp();
                IPAddress validIP = getValidIP(ip);
                int validPort = getValidPort(port);
                IPAddress pAddress = getValidIP(ipRemote);
                int num = getValidPort(portRemote);
                IPEndPoint pEndPoint = new IPEndPoint(validIP, validPort);
                RemoteEP = new IPEndPoint(pAddress, num);
                UDP_sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                try
                {
                    UDP_sock.Bind(pEndPoint);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    flag = false;
                    return flag;
                }
                UDP_thread = new Thread(new ThreadStart(UDP_RecMsg))
                {
                    IsBackground = true
                };
                UDP_thread.Start();
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        private void WatchConnecting()
        {
            while (true)
            {
                Thread.Sleep(1);
                Socket socket = S_socketWatch.Accept();
                ServerRemoteEndPoint.Add(socket.RemoteEndPoint.ToString());
                S_dict.Add(socket.RemoteEndPoint.ToString(), socket);
                Thread thread = new Thread(new ParameterizedThreadStart(S_RecMsg))
                {
                    IsBackground = true
                };
                thread.Start(socket);
                S_dictThread.Add(socket.RemoteEndPoint.ToString(), thread);
            }
        }

        public class WindowWrapper : IWin32Window
        {
            private IntPtr _hwnd;

            public IntPtr Handle
            {
                get
                {
                    return _hwnd;
                }
            }

            public WindowWrapper(IntPtr handle)
            {
                _hwnd = handle;
            }
        }
    }
}
