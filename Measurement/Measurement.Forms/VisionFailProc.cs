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

namespace LZ.CNC.Measurement.Forms
{
    public partial class VisionFailProc : Form
    {
        private int _PointNum;


        public VisionFailProc(int point)
        {
            InitializeComponent();
            _PointNum = point;
            lbl_tips.Text = string.Format("{0}点拍照失败！", (char)(_PointNum + 65));
        }


        private void btn_handvision_Click(object sender, EventArgs e)
        {
            string sendchar = string.Format("B{0}", (char)(_PointNum + 65));
            MeasurementContext.CamNet.tcpClientSendData(sendchar, false);
            btn_conti.Enabled = true;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btn_conti_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
