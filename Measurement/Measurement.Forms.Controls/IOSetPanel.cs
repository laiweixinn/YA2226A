using System;
using System.ComponentModel;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;
using LZ.CNC.KeyBoard;
using DY.CNC.Core;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class IOSetPanel : UserControl
    {       
        public IOSetPanel()
        {
            InitializeComponent();

            cbo_cardnum.Items.Clear();
            if (MeasurementContext.Worker.GetMotion(CardIDs.A) != null)
            {
                cbo_cardnum.Items.Add("卡A");
            }
            if (MeasurementContext.Worker.GetMotion(CardIDs.B) != null)
            {
                cbo_cardnum.Items.Add("卡B");
            }
            if (MeasurementContext.Worker.GetMotion(CardIDs.C) != null)
            {
                cbo_cardnum.Items.Add("卡C");
            }
            if (MeasurementContext.Worker.GetMotion(CardIDs.D) != null)
            {
                cbo_cardnum.Items.Add("卡D");
            }

            cbo_portnum.Items.Clear();
            for (int i = 0; i < 20; i++)
            {
                cbo_portnum.Items.Add(i);
            }

            cbo_status.Items.Clear();
            cbo_status.Items.Add("低电平");
            cbo_status.Items.Add("高电平");

        }

        private MeasurementConfig.ConfigIO _IO;

        public event EventHandler IoSetChanged;


        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.ExStyle |= 0x02000000; // Turn off WS_CLIPCHILDREN 
                return parms;
            }
        }

        protected void OnIoSetChanged()
        {
            if (IoSetChanged!=null)
            {
                IoSetChanged(this, null);
            }
        }

        public MeasurementConfig.ConfigIO IO
        {
            get
            {
                return _IO;
            }
            set
            {
                _IO = value;
                Init();
            }
        }

        [Category("外观"), Browsable(true), DisplayName("Tips"), Description("Tips属性")]
        public String Tips
        {
            get
            {
                return lbl_ioname.Text;
            }
            set
            {
                lbl_ioname.Text = value;
            }
        }

        private void Init()
        {
            if (_IO != null)
            {
                cbo_cardnum.SelectedIndex = (int)_IO.CardID;
                cbo_portnum.SelectedIndex = _IO.IO;
                cbo_status.SelectedIndex = (_IO.Status ? 1 : 0);
                lbl_ioname.Text = _IO.Name;
            }         
        }

        public void Save()
        {
            if (_IO != null)
            {
                _IO.CardID = (CardIDs) cbo_cardnum.SelectedIndex;
                _IO.IO = cbo_portnum.SelectedIndex;
                _IO.Status = (cbo_status.SelectedIndex == 0 ? false : true);
            }
        }

        private void IOSetPanel_Load(object sender, EventArgs e)
        {
        }

        private void Select_Changed(object sender, EventArgs e)
        {
            OnIoSetChanged();
        }

        private void lbl_ioname_Click(object sender, EventArgs e)
        {
            if (_IO!=null)
            {
                StringSoftKeyboard stringSoftKeyboard = new StringSoftKeyboard()
                {
                    Tips = Tips,
                    Value = lbl_ioname.Text,
                    ReadOnly = false
                    
                };

                //stringSoftKeyboard.Text = lbl_ioname.Text;

                if (stringSoftKeyboard.ShowDialog() == DialogResult.OK)
                {
                    lbl_ioname.Text = stringSoftKeyboard.Value;
                    _IO.Name = stringSoftKeyboard.Value;
                    OnIoSetChanged();
                }
            }

        }
    }
}
