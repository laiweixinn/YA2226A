using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using LZ.CNC.Measurement.Core;

namespace LZ.CNC.Measurement.Forms
{
    public partial class AlarmForm : TabForm
    {
        private MeasurementAlarms.AlarmCollection _Alarms = null;

        public AlarmForm()
        {
            InitializeComponent();
            _Alarms = MeasurementContext.Alarms.AlarmItems;
        }

        private void RefreshText()
        {
            try
            {

                rtxt_alarms.Clear();
                UInt64 i = 1;
                MeasurementAlarms alarms = MeasurementAlarms.Load();
                _Alarms = alarms.AlarmItems;
                foreach (MeasurementAlarms.AlarmItem item in _Alarms)
                {
                    if (item != null)
                    {
                        rtxt_alarms.AppendText(string.Format("[{2}]   {0} {1}\n", item.Time, item.AlarmInfo, i));
                    }
                    i++;
                }
                rtxt_alarms.ScrollToCaret();
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnTabSelectChanged()
        {
            if (IsSelected)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    try
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            RefreshText();
                        }));
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        private void AlarmForm_Load(object sender, EventArgs e)
        {
            RefreshText();
        }

        private void btn_QueryAlm_Click(object sender, EventArgs e)
        {
            new AlarmDisplayForm().ShowDialog();

        }
    }
}
