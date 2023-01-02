using System;
using System.ComponentModel;
using System.Windows.Forms;
using LZ.CNC.KeyBoard;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class StringInputBox : UserControl
    {
        public StringInputBox()
        {
            InitializeComponent();
        }

        public char PasswordChar
        {
            get
            {
                return txt_inputbox.PasswordChar;
            }
            set
            {
                txt_inputbox.PasswordChar = value;
            }

        }

        [Browsable(true), Category("自定义属性"), Description("字符")]
        public override string Text
        {
            get
            {
                return txt_inputbox.Text;
            }
            set
            {
                txt_inputbox.Text = value;
            }
        }

        [Browsable(true), Category("自定义属性"), Description("标签")]
        public string Tips
        {
            get
            {
                return lbl_tips.Text;
            }
            set
            {
                lbl_tips.Text = value;
            }
        }

        private void Txt_inputbox_Click(object sender, EventArgs e)
        {
            StringSoftKeyboard stringSoftKeyboard = new StringSoftKeyboard()
            {
                PasswordChar = PasswordChar,
                Tips = Tips,
                Value = Text
            };
            if (PasswordChar == '\0')
            {
                stringSoftKeyboard.Text = txt_inputbox.Text;
            }

            if (stringSoftKeyboard.ShowDialog() == DialogResult.OK)
            {
                txt_inputbox.Text = stringSoftKeyboard.Value;
                txt_inputbox.SelectAll();
                txt_inputbox.Focus();
                OnValueChanged();
            }
        }

        public event EventHandler ValueChanged;

        private void OnValueChanged()
        {
            if (ValueChanged!=null)
            {
                ValueChanged(this, null);
            }
        }
    }
}
