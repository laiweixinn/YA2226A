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
    public partial class TabControlEx : TabControl
    {
        private Color _TitleColorSelected = Color.Green;
        private Color _TitleColorDisSelected = Color.Transparent;
        private Color _TitelTextColorSelected = SystemColors.Control;
        private Color _TitleTextColorDisSelected = SystemColors.ControlText;
        //private Brush _TitelTextColorSelected = new SolidBrush(Color.AliceBlue);
        public TabControlEx()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            SizeMode = TabSizeMode.Fixed;
        }

        public Color TitleBackColor
        {
            get
            {
                return _TitleColorSelected;
            }
            set
            {
                _TitleColorSelected = value;
            }
        }
        public Color TitleColorDisSelected
        {
            get
            {
               return _TitleColorDisSelected;
            }
            set
            {
                _TitleColorDisSelected = value;
            }
        }
        public Color TitelTextColorSelected
        {
            get
            {
                return _TitelTextColorSelected;
            }
            set
            {
                _TitelTextColorSelected = value;
            }
        }
        public Color TitleTextColorDisSelected
        {
            get
            {
                return _TitleTextColorDisSelected;
            }
            set
            {
                _TitleTextColorDisSelected = value;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Brush brush = null;
            for (int i = 0; i < TabCount; i++)
            {

                Rectangle rectangle = new Rectangle();
                rectangle = GetTabRect(i);
                rectangle.X += 1;
                rectangle.Y += 1;
                rectangle.Height -= 2;
                rectangle.Width -= 2;
                if (SelectedIndex == i)
                {
                    e.Graphics.FillRectangle(new SolidBrush(_TitleColorSelected), rectangle);
                    brush = new SolidBrush(_TitelTextColorSelected);
                }
                else
                {
                    brush = new SolidBrush(_TitleTextColorDisSelected);
                    e.Graphics.FillRectangle(new SolidBrush(_TitleColorDisSelected), rectangle);
                }

                Rectangle bounds = GetTabRect(i);
                PointF txtpoint = new PointF();
                SizeF txtsize = TextRenderer.MeasureText(TabPages[i].Text, Font);

                txtpoint.X = bounds.X + (bounds.Width - txtsize.Width) / 2;
                txtpoint.Y = bounds.Bottom - txtsize.Height - Padding.Y;

                e.Graphics.DrawString(TabPages[i].Text, Font, brush, txtpoint.X, txtpoint.Y);
            }
        }
    }
}