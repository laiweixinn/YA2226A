using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class DetectStatus : UserControl
    {

        private bool _IsADetect;

        private bool _IsBDetect;

        private bool _IsCDetect;

        private bool _IsDDetect;

        public bool IsADetect
        {
            get
            {
                return _IsADetect;
            }
            set
            {
                _IsADetect = value;
            }
        }

        public bool IsBDetect
        {
            get
            {
                return _IsBDetect;
            }
            set
            {
                _IsBDetect = value;
            }
        }

        public bool IsCDetect
        {
            get
            {
                return _IsCDetect;
            }
            set
            {
                _IsCDetect = value;
            }
        }

        public bool IsDDetect
        {
            get
            {
                return _IsDDetect;
            }
            set
            {
                _IsDDetect = value;
            }
        }

        public DetectStatus()
        {
            InitializeComponent();
        }

        private void ListenWork()
        {
            if (_IsADetect)
            {
                lineControl1.BackColor = Color.AliceBlue;
            }
        }

    }
}
