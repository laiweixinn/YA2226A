using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LZ.CNC.KeyBoard
{
    public partial class StringSoftKeyboard : Form
    {
        public StringSoftKeyboard()
        {
            InitializeComponent();
            int screenwidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenheight = Screen.PrimaryScreen.WorkingArea.Height;
            int x = (screenwidth - Width) / 2;
            int y = (screenheight - Height)/2+100;
            Location = new Point(x, y);
        }

        private bool _IsCaptial;

        private bool _IsMouseDown;

        private Point _MousePoint;

        private Point _FormPoint;

        private bool _ReadOnly=true;

        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
            }
        }

        public bool IsCaptial
        {
            get
            {
                return _IsCaptial;
            }
            set
            {
                _IsCaptial = value;
            }
        }

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

        public string Value
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

        public char PasswordChar {
            get
            {
               return txt_inputbox.PasswordChar;
            }
            set
            {
                txt_inputbox.PasswordChar = value;
            }
        }

        private void Btn_clear_Click(object sender, EventArgs e)
        {
            txt_inputbox.Text = "";
        }

        private void Btn_backs_Click(object sender, EventArgs e)
        {
            String Str = txt_inputbox.Text.Trim();
            if (Str.Length>0)
            {
                Str = Str.Substring(0, Str.Length - 1);
                txt_inputbox.Text = Str;
            }
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Btn_ESC_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private  void RefreshUI()
        {
            btn_capslk.BackColor = _IsCaptial ? Color.Blue : Color.FromArgb(64, 64, 64);       
            btn_charA.Text = _IsCaptial ? "A" : "a";
            btn_charB.Text = _IsCaptial ? "B" : "b";
            btn_charC.Text = _IsCaptial ? "C" : "c";
            btn_charD.Text = _IsCaptial ? "D" : "d";
            btn_charE.Text = _IsCaptial ? "E" : "e";
            btn_charF.Text = _IsCaptial ? "F" : "f";
            btn_charG.Text = _IsCaptial ? "G" : "g";
            btn_charH.Text = _IsCaptial ? "H" : "h";
            btn_charI.Text = _IsCaptial ? "I" : "i";
            btn_charJ.Text = _IsCaptial ? "J" : "j";
            btn_charK.Text = _IsCaptial ? "K" : "k";
            btn_charL.Text = _IsCaptial ? "L" : "l";
            btn_charM.Text = _IsCaptial ? "M" : "m";
            btn_charN.Text = _IsCaptial ? "N" : "n";
            btn_charO.Text = _IsCaptial ? "O" : "o";
            btn_charP.Text = _IsCaptial ? "P" : "p";
            btn_charQ.Text = _IsCaptial ? "Q" : "q";
            btn_charR.Text = _IsCaptial ? "R" : "r";
            btn_charS.Text = _IsCaptial ? "S" : "s";
            btn_charT.Text = _IsCaptial ? "T" : "t";
            btn_charU.Text = _IsCaptial ? "U" : "u";
            btn_charV.Text = _IsCaptial ? "V" : "v";
            btn_charW.Text = _IsCaptial ? "W" : "w";
            btn_charX.Text = _IsCaptial ? "X" : "x";
            btn_charY.Text = _IsCaptial ? "Y" : "y";
            btn_charZ.Text = _IsCaptial ? "Z" : "z";
        }

        private void Btn_capslk_Click(object sender, EventArgs e)

        {
            _IsCaptial = !_IsCaptial;
            RefreshUI();
        }

        private void Char_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (txt_inputbox.SelectionLength==txt_inputbox.TextLength)
            {
                txt_inputbox.Text = ""; 
            }
            String Str = btn.Text;
            //Str = _IsCaptial ? Str.ToUpper : Str.ToLower;
            txt_inputbox.Text += Str;
        }

        private void StringSoftKeyboard_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(26, 26, 26);
            _IsCaptial = true;
            RefreshUI();
            txt_inputbox.SelectAll();
            txt_inputbox.ReadOnly = _ReadOnly;
        }

        private void Mouse_Enter(object sender, EventArgs e)
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

        private void Lbl_tips_MouseUp(object sender, MouseEventArgs e)
        {
            _IsMouseDown = false;
        }

        private void Lbl_tips_MouseMove(object sender, MouseEventArgs e)
        {
            if (_IsMouseDown)
            {
                Point pt = MousePosition;
                Int32 x = _MousePoint.X - pt.X;
                Int32 y = _MousePoint.Y - pt.Y;
                Location = new Point(_FormPoint.X - x, _FormPoint.Y - y);
            }
        }

        private void Lbl_tips_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                _MousePoint = MousePosition;
                _FormPoint = Location;
                _IsMouseDown = true;
            }
        }
    }
}


