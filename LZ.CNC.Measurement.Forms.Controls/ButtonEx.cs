using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class ButtonEx : Button
    {
        //private Font 

        public ButtonEx()
        {
            InitializeComponent();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Font = new Font(Font, FontStyle.Regular);
            Font = new Font("宋体", 14F, FontStyle.Regular);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Font = new Font("宋体", 12F, FontStyle.Regular);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
