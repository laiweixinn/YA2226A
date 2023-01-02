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
    public partial class SimpleStatisticalUnit : UserControl
    {
        public SimpleStatisticalUnit()
        {
            InitializeComponent();

            g = panel1.CreateGraphics();
            gstring = this.CreateGraphics();
            p = new Pen(new SolidBrush(Color.Green));
            blackp = new Pen(new SolidBrush(Color.Red), 1);
            fontheight = panel1.Font.Height;


        }

        Graphics g;
        Graphics gstring;
        Pen p;
        Pen blackp;
        Rectangle rect;
        Brush greenbrush = new SolidBrush(Color.Lime);
        Brush redbrush = new SolidBrush(Color.Red);
        Brush whitebrush = new SolidBrush(Color.White);
        Brush blackbrush = new SolidBrush(Color.Black);
        int fontheight;


        int _value;
        int _MinValue = 0;
        int _MaxValue = 1000;
        string _Hour;
        int _Index;
        public int value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPaint(null);
                lbl_value.Text = _value.ToString();

            }
        }
        public string Hour
        {
            get
            {
                return _Hour;
            }
            set
            {
                _Hour = value;
                lbl_hour.Text = _Hour;
            }
        }
        public int MinValue
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

        public int MaxValue
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

        public void Clear()
        {
            value = 0;
        }

        public int Index
        {

            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {



            if (_MaxValue == 0)
            {
                return;
            }

            double scale = (double)_value / (double)_MaxValue;
            rect = new Rectangle(panel1.Location.X, panel1.Location.Y, Convert.ToInt32(panel1.Width * scale), panel1.Height);//有值区域
            g.DrawRectangle(p, rect);
            if (_value >= _MaxValue)
            {
                g.FillRectangle(redbrush, rect);
            }
            else
            {
                g.FillRectangle(greenbrush, rect);
            }

            rect = new Rectangle(panel1.Location.X + Convert.ToInt32(panel1.Width * scale), panel1.Location.Y, panel1.Location.X + panel1.Width, panel1.Height);//无值区域
            g.FillRectangle(new SolidBrush(Color.WhiteSmoke), rect);
            g.DrawString($"{(scale * 100).ToString("0.0")}%", panel1.Font, new SolidBrush(Color.Black), new PointF(panel1.Width / 2, panel1.Height / 2 - fontheight / 2));//百分比




            base.OnPaint(e);

        }







    }
}
