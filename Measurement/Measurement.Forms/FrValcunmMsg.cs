using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;
using DY.Core.Forms;

namespace LZ.CNC.Measurement.Forms
{
    public partial class FrValcunmMsg : Form
    {
        public FrValcunmMsg()
        {
            InitializeComponent();
        }
        MeasurementWorker _worker = MeasurementContext.Worker;

        private void btn_closebuzzer_Click(object sender, EventArgs e)
        {
            _worker.CloseBuzzer();
        }

        private void btn_continue_Click(object sender, EventArgs e)
        {
            if (_worker.IsGateSafe())
            {
                _worker.IsAutoRun = true;
                this.Close();
            }
            else
            {
                _worker.AlarmWork();
                MessageBoxEx.ShowErrorMessage("门禁触发!");
            }
        }

        private void FrValcunmMsg_Load(object sender, EventArgs e)
        {
            _worker.IsAutoRun = false;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            _worker.IsStop = true;
            this.Close();
        }
    }
}
