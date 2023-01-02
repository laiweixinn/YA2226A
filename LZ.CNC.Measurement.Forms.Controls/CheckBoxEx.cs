using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class CheckBoxEx : UserControl
    {
        public CheckBoxEx()
        {
            InitializeComponent();
        }

        private bool _IsChecked;

        public event EventHandler CheckedChanged;

        protected void OnCheckedChanged()
        {
            if (CheckedChanged!=null)
            {
                CheckedChanged(this, null);
            }
        }

        public bool IsCkecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
                RefreshBox();
            }
        }

        private void RefreshBox()
        {
            lbl_true.BackColor = _IsChecked ? Color.Green : Color.Gray;
            lbl_false.BackColor = !_IsChecked ? Color.Green : Color.Gray;
        }

        private void lbl_true_Click(object sender, EventArgs e)
        {
            if (_IsChecked==false)
            {
                _IsChecked = true;
                OnCheckedChanged();
                RefreshBox();
            }
        }

        private void lbl_false_Click(object sender, EventArgs e)
        {
            if (_IsChecked == true)
            {
                _IsChecked = false;
                OnCheckedChanged();
                RefreshBox();
            }
        }

        [Browsable(true), Category("自定义属性"), Description("标签")]
        public string Tips
        {
            get
            {
                return lbl_name.Text;
            }
            set
            {
                base.Text = value;
                lbl_name.Text = value;
            }
        }

        [Browsable(true), Category("自定义属性"), Description("标签True")]
        public string TrueTip
        {
            get
            {
                return lbl_true.Text;
            }
            set
            {
                lbl_true.Text = value;
            }
        }

        [Browsable(true), Category("自定义属性"), Description("标签False")]
        public string FalseTip
        {
            get
            {
                return lbl_false.Text;
            }
            set
            {
                lbl_false.Text = value;
            }
        }
    }
}
