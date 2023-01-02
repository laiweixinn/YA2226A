using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms
{
    public class TipForm:Form
    {
        private static readonly object synclock = new object();

        private static TipForm _Instance;
        private static Thread _WaitThread;

        public static TipForm Instance
        {
            get
            {
                if (_Instance==null)
                {
                    lock (synclock)
                    {
                        if (_Instance==null)
                        {
                            _Instance = new TipForm();
                        }
                    }
                }
                return _Instance;
            }

        }

        

        private TipForm()
        {
            CheckForIllegalCrossThreadCalls = false;
          
        }

        public new static void Show()
        {
            _Instance = null;
            _WaitThread = new Thread(new ThreadStart(ShowTipForm));
            _WaitThread.Start();
            Thread.Sleep(100);
        }


        //public static void SetText(string text)
        //{
        //    _Instance.SetTipText(text);
        //}

        private static void ShowTipForm()
        {
            try
            {
                _Instance = new TipForm();
                _Instance.ShowDialog();
            }
            catch (ThreadAbortException e)
            {
                //_Instance.Close();
                Thread.ResetAbort();
                throw;
            }
        }

        public static void CloseForm()
        {
            //if (_WaitThread!=null)
            //{
                _Instance.Close();
            //}
        }

        private void InitializeComponent()
        {
          
        }

        //public void SetTipText(string text)
        //{
        //    if (_Instance!=null)
        //    {
        //        _Instance.Show();
        //        _Instance.SetTipText(text);
        //    }
        //}

    }
}
