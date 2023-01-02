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
using LZ.CNC.Measurement;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class CtrlSMStation :PageUC.BasePageUC
    {
        public CtrlSMStation()
        {
            InitializeComponent();
            //Inpb_SMccdy.ValueChanged += InputBox_ValueChanged;
            //Inpb_smwaitX.ValueChanged += InputBox_ValueChanged;
            //Inpb_smWaitY.ValueChanged += InputBox_ValueChanged;
            //Inpb_smWaitZ.ValueChanged += InputBox_ValueChanged;
            //Inpb_ccdx.ValueChanged += InputBox_ValueChanged;
            //Inpb_dichargeZ.ValueChanged += InputBox_ValueChanged;
            //Inpb_dischargeX.ValueChanged += InputBox_ValueChanged;
            //Inpb_loadsmy.ValueChanged += InputBox_ValueChanged;
            //Inpb_loadx.ValueChanged += InputBox_ValueChanged;
            //Inpb_loadZ.ValueChanged += InputBox_ValueChanged;
            //Inpb_smDischargeY.ValueChanged += InputBox_ValueChanged;
          //  InitData();
        }

        private DataTable _Table = null;

        private MeasurementData.RecipeDataItem _Data = null;
        private int Station_flag = 0;
        private SMLocation _SMItem = null;
        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.ExStyle |= 0x02000000;
                return parms;
            }
        }



        private void Worker_RecipeChanged(object sender, EventArgs e)
        {
            _Data = MeasurementContext.Data.CurrentRecipeData;
            InitData();       
        }


        public void InputBox_ValueChanged(object sender,EventArgs e)
        {
            if (IsInited)
            {
                SaveData();
            }
        }


        public void Init(int i)
        {
            Station_flag = i;
            InitData();
          //  _SMItem = _Data.SMPosition[Station_flag];
        }

        private void InitData()
        {
            _Data = MeasurementContext.Data.CurrentRecipeData;
            IsInited = false;
            Inpb_loadx.Value = _Data.SMPosition[Station_flag].Lsm_loadX;
            Inpb_loadsmy.Value = _Data.SMPosition[Station_flag].Lsm_loadY;
            Inpb_loadZ.Value = _Data.SMPosition[Station_flag].Lsm_LoadZ;
            Inpb_SMccdy.Value = _Data.SMPosition[Station_flag].Lsm_CCDY;
            Inpb_ccdx.Value = _Data.SMPosition[Station_flag].Lsm_CCDX;
            Inpb_dischargeX.Value = _Data.SMPosition[Station_flag].Lsm_DischargeX;
            Inpb_dichargeZ.Value = _Data.SMPosition[Station_flag].Lsm_DischargeZ;
            Inpb_smDischargeY.Value = _Data.SMPosition[Station_flag].Lsm_DischargeY;
            Inpb_smwaitX.Value = _Data.SMPosition[Station_flag].Lsm_WaitX;
            Inpb_smWaitY.Value = _Data.SMPosition[Station_flag].Lsm_WaitY;
            Inpb_smWaitZ.Value = _Data.SMPosition[Station_flag].Lsm_WaitZ;
            IsInited = true;
        }


        public override void Save()
        {
            if (_Data != null)
            {
                SaveData();
            }
        }

        private void SaveData()
        {

            if (_Data != null)
            {
                _Data.SMPosition[Station_flag].Lsm_loadX = Inpb_loadx.Value;
                _Data.SMPosition[Station_flag].Lsm_loadY = Inpb_loadsmy.Value;
                _Data.SMPosition[Station_flag].Lsm_LoadZ = Inpb_loadZ.Value;
                _Data.SMPosition[Station_flag].Lsm_CCDX = Inpb_ccdx.Value;
                _Data.SMPosition[Station_flag].Lsm_CCDY = Inpb_SMccdy.Value;
                _Data.SMPosition[Station_flag].Lsm_DischargeX = Inpb_dischargeX.Value;
                _Data.SMPosition[Station_flag].Lsm_DischargeY = Inpb_smDischargeY.Value;
                _Data.SMPosition[Station_flag].Lsm_DischargeZ = Inpb_dichargeZ.Value;
                _Data.SMPosition[Station_flag].Lsm_WaitX = Inpb_smwaitX.Value;
                _Data.SMPosition[Station_flag].Lsm_WaitY = Inpb_smWaitY.Value;
                _Data.SMPosition[Station_flag].Lsm_WaitZ = Inpb_smWaitZ.Value;
                MeasurementContext.Data.Save();
            }
        }

    }
}
