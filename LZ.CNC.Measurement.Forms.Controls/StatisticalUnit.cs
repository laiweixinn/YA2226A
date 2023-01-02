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
    public partial class StatisticalUnit : UserControl
    {
        public StatisticalUnit()
        {
            InitializeComponent();
            if (!this.DesignMode)
            {
                g = panel1.CreateGraphics();
                gstring = this.CreateGraphics();
                p = new Pen(new SolidBrush(Color.Green));
                blackp = new Pen(new SolidBrush(Color.Red), 1);
                fontheight = panel1.Font.Height;
                GetControlSize();
            }
        }

        int _value;
        int _MinValue = 0;
        int _MaxValue = 200;
        int _Index;


        Graphics g;
        Graphics gstring;
        Pen p;
        Pen blackp;
        Rectangle rect;
        Brush greenbrush = new SolidBrush(Color.Lime);
        Brush redbrush = new SolidBrush(Color.Red);
        Brush whitebrush = new SolidBrush(Color.White);
        Brush blackbrush = new SolidBrush(Color.Black);

        int x;
        int y;

        int width;
        int height;

        int leng = 6;

        int fontheight;
        string _Hour;
        private Panel panel2;
        private Panel panel1;
        private Label lbl_hour;
        private Label lbl_value;

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

        Font drawingfont = new System.Drawing.Font("微软雅黑", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        private void GetControlSize()
        {

            width = panel1.Width;// = this.Width

            height = panel1.Height;// = this.Height
            panel1.BackColor = Color.Transparent;
            x = panel2.Location.X;
            y = panel2.Location.Y;

        }




        protected override void OnPaint(PaintEventArgs e)
        {
            //if (!this.DesignMode)
            //{
            if (_MaxValue == 0)
            {
                return;
            }
            GetControlSize();
            double scale = (double)_value / (double)MaxValue;
            rect = new Rectangle(panel1.Location.X, panel1.Location.Y, Convert.ToInt32(panel1.Width * scale), panel1.Height);//有值区域
            g.DrawRectangle(p, rect);
            if (_value >= MaxValue)
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

            //刻度线
            gstring.DrawLine(blackp, x, y - 4, x + width, y - 4);
            int lineh = 1;
            int fonth = 15;
            int xscale;

            for (int i = 0; i < 6; i++)
            {
                xscale = x + (width / 5) * i;
                gstring.DrawLine(blackp, xscale, y - 4, xscale, y - 4 - lineh);
                gstring.DrawString($"{(_MaxValue / 5) * i}", drawingfont, new SolidBrush(Color.Red), new Point(xscale - 10, y - lineh - fonth));//值

            }

            base.OnPaint(e);
        }
        //}





        protected override void InitLayout()
        {

            OnPaint(null);

            base.InitLayout();
        }

        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_hour = new System.Windows.Forms.Label();
            this.lbl_value = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Location = new System.Drawing.Point(94, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(338, 18);
            this.panel2.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(338, 18);
            this.panel1.TabIndex = 0;
            // 
            // lbl_hour
            // 
            this.lbl_hour.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_hour.AutoSize = true;
            this.lbl_hour.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lbl_hour.Location = new System.Drawing.Point(3, 10);
            this.lbl_hour.Name = "lbl_hour";
            this.lbl_hour.Size = new System.Drawing.Size(69, 20);
            this.lbl_hour.TabIndex = 7;
            this.lbl_hour.Text = "7:00-8:00";
            // 
            // lbl_value
            // 
            this.lbl_value.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_value.AutoSize = true;
            this.lbl_value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_value.ForeColor = System.Drawing.Color.Black;
            this.lbl_value.Location = new System.Drawing.Point(434, 14);
            this.lbl_value.Name = "lbl_value";
            this.lbl_value.Size = new System.Drawing.Size(41, 20);
            this.lbl_value.TabIndex = 8;
            this.lbl_value.Text = "8000";
            // 
            // StatisticalUnit
            // 
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lbl_hour);
            this.Controls.Add(this.lbl_value);
            this.Name = "StatisticalUnit";
            this.Size = new System.Drawing.Size(482, 36);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
