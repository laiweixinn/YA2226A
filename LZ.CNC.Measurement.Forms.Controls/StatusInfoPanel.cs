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
    public partial class StatusInfoPanel : UserControl
    {
        public StatusInfoPanel()
        {
            InitializeComponent();
        }

        private string _StautsInfo;

        private bool _IsLinked;

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                lbl_status.ForeColor = value;
            }

        }

        private Color _InfoBackColor=Color.Silver;

        [Browsable(true), Category("自定义属性"), Description("状态消息")]
        public string StatusInfo
        {
            get
            {
                return _StautsInfo;
            }
            set
            {
                _StautsInfo = value;
                lbl_status.Text = _StautsInfo;
            }
        }

        [Browsable(true), Category("自定义属性"), Description("状态名称")]
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

        [Browsable(true), Category("自定义属性"), Description("是否链接")]
        public bool IsLinked
        {
            get
            {
                return _IsLinked;
            }
            set
            {
                _IsLinked = value;
                if (_IsLinked)
                {
                    lbl_status.LinkArea = new LinkArea(0, lbl_status.Text.Length);
                }
                else
                {
                    lbl_status.LinkArea = new LinkArea(0, 0);
                }
            }
        }

        [Browsable(true), Category("自定义属性"), Description("信息背景颜色")]
        public Color InfoBackColor
        {
            get
            {
                return _InfoBackColor;
            }
            set
            {
                _InfoBackColor = value;
                lbl_status.BackColor = _InfoBackColor;
            }
        }

        private void lbl_status_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnLinkClicked();
        }

        public event EventHandler LinkClicked;

        private void OnLinkClicked()
        {
            if (LinkClicked!=null)
            {
                LinkClicked(this, null);
            }
        }
    }
}
