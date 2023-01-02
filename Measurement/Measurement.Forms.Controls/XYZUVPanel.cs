using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class XYZUVPanel : UserControl
    {
        XYZUVPoint _Point = null;

        private double _XMIN;

        private double _XMAX;

        private double _YMIN;

        private double _YMAX;

        private double _ZMIN;

        private double _ZMAX;

        private double _UMIN;

        private double _UMAX;

        private double _VMAX;

        private double _VMIN;

        private double _XValue;

        public double XValue
        {
            get
            {
                return _XValue;
            }
            set
            {
                _XValue = value;
                num_x.Value = value;
            }
        }

        private double _YValue;

        public double YValue
        {
            get
            {
                return _YValue;
            }
            set
            {
                _YValue = value;
                num_y.Value = value;
            }
        }

        private double _ZValue;

        public double ZValue
        {
            get
            {
                return _ZValue;
            }
            set
            {
                _ZValue = value;
                num_z.Value = value;
            }
        }


        private double _UValue;
        public double UValue
        {
            get
            {
                return _UValue;
            }
            set
            {
                _UValue = value;
                num_u.Value = value;
            }
        }


        private double _VValue;
        public double VValue
        {
            get
            {
                return _VValue;
            }
            set
            {
                _VValue = value;
                num_v.Value = value;
            }
        }




        [Category("范围"), Browsable(true), DisplayName("XMIN"), Description("X轴最小值")]
        public double XMIN
        {
            get
            {
                return _XMIN;
            }
            set
            {
                _XMIN = value;
            }
        }

        [Category("范围"), Browsable(true), DisplayName("XMAX"), Description("X轴最大值")]
        public double XMAX
        {
            get
            {
                return _XMAX;
            }
            set
            {
                _XMAX = value;
            }
        }

        [Category("范围"), Browsable(true), DisplayName("YMIN"), Description("Y轴最小值")]
        public double YMIN
        {
            get
            {
                return _YMIN;
            }
            set
            {
                _YMIN = value;
            }
        }

        [Category("范围"), Browsable(true), DisplayName("YMAX"), Description("Y轴最大值")]
        public double YMAX
        {
            get
            {
                return _YMAX;
            }
            set
            {
                _YMAX = value;
            }
        }

        [Category("范围"), Browsable(true), DisplayName("ZMIN"), Description("Z轴最小值")]
        public double ZMIN
        {
            get
            {
                return _ZMIN;
            }
            set
            {
                _ZMIN = value;
            }
        }

        [Category("范围"), Browsable(true), DisplayName("ZMAX"), Description("Z轴最大值")]
        public double ZMAX
        {
            get
            {
                return _ZMAX;
            }
            set
            {
                _ZMAX = value;
            }
        }


        [Category("范围"), Browsable(true), DisplayName("ZMAX"), Description("U轴最大值")]
        public double UMAX
        {
            get
            {
                return _UMAX;
            }
            set
            {
                _UMAX = value;
            }
        }


        [Category("范围"), Browsable(true), DisplayName("ZMAX"), Description("V轴最大值")]
        public double VMAX
        {
            get
            {
                return _VMAX;
            }
            set
            {
                _VMAX = value;
            }
        }





        [Category("点名称"), Browsable(true), DisplayName("PosName"), Description("点位名称")]
        public string PosName
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                grb_name.Text = value;
            }
        }

        public XYZUVPanel()
        {
            InitializeComponent();
        }

        public event EventHandler ValueChanged;

        protected void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        private void XYZUVPanel_Load(object sender, EventArgs e)
        {
            num_x.MinValue = _XMIN;
            num_x.MaxValue = _XMAX;
            num_y.MinValue = _YMIN;
            num_y.MaxValue = _YMAX;
            num_z.MinValue = _ZMIN;
            num_z.MaxValue = _ZMAX;
            num_u.MaxValue = _UMAX;
            num_u.MinValue = _UMIN;
            num_v.MaxValue = _VMAX;
            num_v.MinValue = _VMIN;
            num_x.ValueChanged += Num_x_ValueChanged;
            num_y.ValueChanged += Num_x_ValueChanged;
            num_z.ValueChanged += Num_x_ValueChanged;
            num_u.ValueChanged += Num_x_ValueChanged;
            num_v.ValueChanged += Num_x_ValueChanged;
        }

        private void Num_x_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged();
        }

        public void Init(XYZUVPoint point)
        {
            if (point != null)
            {
                _Point = point;
                num_x.Value = point.X;
                num_y.Value = point.Y;
                num_z.Value = point.Z;
                num_u.Value = point.U;
                num_v.Value = point.V;
            }
        }

        public void Save()
        {
            if (_Point != null)
            {
                _Point.X = num_x.Value;
                _Point.Y = num_y.Value;
                _Point.Z = num_z.Value;
                _Point.U = num_u.Value;
                _Point.V = num_v.Value;
                _XValue = num_x.Value;
                _YValue = num_y.Value;
                _ZValue = num_z.Value;
                _UValue = num_u.Value;
                _VValue = num_v.Value;
            }
        }
    }
}

