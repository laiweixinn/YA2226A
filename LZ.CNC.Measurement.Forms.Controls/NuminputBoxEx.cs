using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.KeyBoard;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class NuminputBoxEx : UserControl
    {
        public NuminputBoxEx()
        {
            InitializeComponent();
        }

        private Double _MaxValue = 10000;

        private Double _MinValue = 0;

        private Boolean _IsDecimal = false;

        private Boolean _IsActivated = false;

        public event EventHandler ValueChanged;

        public double MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                _MaxValue = value;
            }
        }

        public double MinValue
        {
            get
            {
                return _MinValue;
            }
            set
            {
                _MinValue = value;
            }
        }

        public bool IsDecimal
        {
            get
            {
                return _IsDecimal;
            }
            set
            {
                _IsDecimal = value;
            }
        }

        public bool IsActivated
        {
            get
            {
                return _IsActivated;
            }
            set
            {
                _IsActivated = value;
                txt_inputbox.BackColor = _IsActivated ? Color.Green : Color.White;
            }
        }

        [Browsable(true), Category("自定义属性"), Description("文本框名称")]
        public string Tips
        {
            get
            {
                return lbl_tips.Text;
            }
            set
            {
                base.Text = value;
                lbl_tips.Text = value;
            }
        }


        [Browsable(true), Category("自定义属性"), Description("计量单位")]
        public string Unit
        {
            get
            {
                return lbl_unit.Text;
            }
            set
            {
                lbl_unit.Text = value;
            }
        }

        public Double Value
        {
            get
            {
                if (string.IsNullOrEmpty(txt_inputbox.Text))
                {
                    return 0.000;
                }
                return Convert.ToDouble(txt_inputbox.Text);
            }
            set
            {
                if (!(Value == value))
                {
                    txt_inputbox.Text = value.ToString();
                    OnValueChanged();
                }
                else
                {
                    txt_inputbox.Text = value.ToString();
                }
            }
        }

        protected void OnValueChanged()
        {
            ValueChanged?.Invoke(this, null);
        }

        private void Txt_inputbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void Txt_inputbox_Click(object sender, EventArgs e)
        {
            NumSoftKeyboard numSoftKeyboard = new NumSoftKeyboard
            //NumSoftKeyboard numSoftKeyboard = new NumSoftKeyboard
            {
                IsDecimal = IsDecimal,
                MaxValue = MaxValue,
                MinValue = MinValue,
                Value = Value
            };

            if (!string.IsNullOrEmpty(txt_inputbox.Text))
            {
                numSoftKeyboard.Tips = Tips;
            }

            if (numSoftKeyboard.ShowDialog() == DialogResult.OK)
            {
                Value = numSoftKeyboard.Value;
            }
            txt_inputbox.SelectAll();
        }
    }
}
