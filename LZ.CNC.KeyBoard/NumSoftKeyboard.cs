using System;
using System.Drawing;
using System.Windows.Forms;

namespace LZ.CNC.KeyBoard
{
    public partial class NumSoftKeyboard : Form
    {
        private Boolean _IsDecimal = false;
        private Double _MaxValue = 0;
        private Double _MinValue = 100;
        Boolean _IsMouseDown = false;
        Point _MousePoint;
        Point _FormPoint;

        public NumSoftKeyboard()
        {
            InitializeComponent();
            int screenwidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenheight = Screen.PrimaryScreen.WorkingArea.Height;
            int x = (screenwidth - Width) / 2;
            int y = (screenheight - Height) / 2 + 100;
            Location = new Point(x, y);
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

        public double MinValue {
            get
            {
              return   _MinValue;
            }
            set {
                _MinValue = value;
            }
        }
        public string Tips {
            get
            {
                return lbl_tips.Text;
            }
            set
            {
                lbl_tips.Text = value;
            }
        }
        public double Value
        {
            get
            {
                if (string.IsNullOrEmpty(txt_inputbox.Text))
                {
                    return 0.0;
                }
                return Convert.ToDouble(txt_inputbox.Text);
            }
            set
            {
                txt_inputbox.Text = value.ToString();
            }
        }

        private void NumSoftKeyboard_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(64, 64, 64);
            if (_MinValue > 0.0)
            {
                btn_pulsminus.Enabled = false;
            }
            if (!_IsDecimal)
            {
                btn_dot.Enabled = false;
            }
            txt_inputbox.SelectAll();
            txt_maxvalue.Text = _MaxValue.ToString();
            txt_minvalue.Text = _MinValue.ToString();

        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            Double temVariable = Value;
            //if (temVariable>_MaxValue)
            //{
            //    MessageBox.Show("输入值大于了最大值:" + _MaxValue.ToString());
            //    txt_inputbox.Text = _MaxValue.ToString();
            //    txt_inputbox.SelectAll();
            //}
            //else if (temVariable < _MinValue)
            //{
            //    MessageBox.Show("输入值小于了最小值:" + _MinValue.ToString());
            //    txt_inputbox.Text = _MinValue.ToString();
            //    txt_inputbox.SelectAll();
            //}
            //else
            //{
                DialogResult = DialogResult.OK;
                Close();
            //}
        }

        private void Btn_clear_Click(object sender, EventArgs e)
        {
            txt_inputbox.Text = "";
        }

        private void Panel_titlebar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                _MousePoint = MousePosition;
                _FormPoint = Location;
                _IsMouseDown = true;
            }
        }

        private void Panel_titlebar_MouseUp(object sender, MouseEventArgs e)
        {
            _IsMouseDown = false;
        }

        private void Panel_titlebar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_IsMouseDown)
            {
                Point mousePosition = MousePosition;
                int num = _MousePoint.X - mousePosition.X;
                int num2 = _MousePoint.Y - mousePosition.Y;
                Location = new Point(_FormPoint.X - num, _FormPoint.Y - num2);
            }
        }

        private void Mounse_Enter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(229, 229, 229);
            btn.ForeColor = Color.Black;
        }

        private void Mouse_Leave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(64, 64, 64);
            btn.ForeColor = Color.White;
        }

        private void Mouse_Enter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(229, 229, 229);
            btn.ForeColor = Color.Black;
        }

        private void Btn_backspace_Click(object sender, EventArgs e)
        {
            string temInputText = txt_inputbox.Text.Trim();
            if (!string.IsNullOrEmpty(temInputText))
            {               
                temInputText=temInputText.Substring(0,temInputText.Length-1);
                txt_inputbox.Text = temInputText;
            }
        }

        private void Btn_pulsminus_Click(object sender, EventArgs e)
        {
            string temInputText = txt_inputbox.Text.Trim();
            if (txt_inputbox.SelectionLength == temInputText.Length)
            {
                temInputText = "";
            }
            else
            {
                temInputText = temInputText.Trim();
            }

            if (!string.IsNullOrEmpty(temInputText))
            {
                if (temInputText.StartsWith("-"))
                {
                    temInputText = temInputText.Substring(1, temInputText.Length-1);
                }
                else
                {
                    temInputText = "-" + temInputText;
                }
            }
            else
            {
                temInputText = "-";
            }
            txt_inputbox.Text = temInputText;

        }

        private void Lbl_close_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Num_Click(object sender, EventArgs e)
        {
            Int32 TemVariable = -1;

            if (sender == btn_num0)
                TemVariable = 0;
            else if (sender == btn_num1)
                TemVariable = 1;
            else if (sender == btn_num2)
                TemVariable = 2;
            else if (sender == btn_num3)
                TemVariable = 3;
            else if (sender == btn_num4)
                TemVariable = 4;
            else if (sender == btn_num5)
                TemVariable = 5;
            else if (sender == btn_num6)
                TemVariable = 6;
            else if (sender == btn_num7)
                TemVariable = 7;
            else if (sender == btn_num8)
                TemVariable = 8;
            else if (sender == btn_num9)
                TemVariable = 9;

            string temInputText = txt_inputbox.Text.Trim();
            temInputText = txt_inputbox.SelectionLength == temInputText.Length ? "": temInputText.Trim();

            if (string.IsNullOrEmpty(temInputText))
            {
                temInputText = TemVariable.ToString();
            }
            else
            {
                if ((temInputText.IndexOf(".")!=1) || (temInputText[0]!='0'))
                {
                    temInputText = temInputText.TrimStart(new char[]
                    {
                        '0'
                    }
                    );
                }
                temInputText += TemVariable.ToString();
            }
            txt_inputbox.Text = temInputText.ToString();
        }

        private void Btn_dot_Click(object sender, EventArgs e)
        {
            string temInputText = txt_inputbox.Text.Trim();
            if (txt_inputbox.SelectionLength == temInputText.Length)
            {
                temInputText = "";
            }
            else
            {
                temInputText = temInputText.Trim();
            }

            if (!string.IsNullOrEmpty(temInputText))
            {
                if (temInputText.IndexOf(".") >= 0)
                {
                    return;
                }
                temInputText += ".";
            }
            else
            {
                temInputText = "0.";
            }
            txt_inputbox.Text = temInputText;
        }
    }
}
