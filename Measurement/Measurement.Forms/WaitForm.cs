using System;
using System.Threading;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms
{
    public partial class WaitForm : Form
    {

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private AsyncCallback _WorkCallBack = null;

        private AsyncCallback _CancelCallBack = null;

        private String _ErrorMessage = null;

        private Boolean _IsCancelled = false;

        private static WaitForm _Instance = null;

        public WaitForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {           
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                        m.Result = (IntPtr)HTCAPTION;
                    return;
            }
            base.WndProc(ref m);
        }


        private void btn_cancel_Click(object sender, EventArgs e)
        {
            _IsCancelled = true;
            if (_CancelCallBack != null)
            {
                _CancelCallBack(null);
            }
        }

        public static void Show(String msg,AsyncCallback workcallback,AsyncCallback cancelcallback)
        {
            if ((_Instance==null?true:_Instance.IsDisposed))
            {
                _Instance = new WaitForm();
            }
            _Instance._ErrorMessage = null;
            _Instance.lbl_movetips.Text = msg;
            _Instance._WorkCallBack = workcallback;
            _Instance._CancelCallBack = cancelcallback;
            _Instance.ShowDialog();
        }

        public static void ShowErrorMessage(String msg)
        {
            if (_Instance == null ? false : !_Instance.IsDisposed)
            {
                _Instance._ErrorMessage = msg;
            }
        }

        private void WaitForm_Load(object sender, EventArgs e)
        {
            if (_CancelCallBack == null)
            {
                btn_cancel.Visible = false;
            }
            ThreadPool.QueueUserWorkItem(delegate
            {
                if (_WorkCallBack != null)
                {
                    _WorkCallBack(null);
                }

                try
                {
                    Invoke(new MethodInvoker(()=>
                    {
                        if (_IsCancelled?false: !String.IsNullOrEmpty(_ErrorMessage))
                        {
                            MessageBox.Show(_ErrorMessage);
                        }
                        Close();
                    }));
                }
                catch (Exception)
                {
                }
            });
        }




        private void WaitForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_IsCancelled)
            {
                _IsCancelled = true;
                if (_CancelCallBack != null)
                {
                    _CancelCallBack(null);
                }
            }
        }
    }
}
