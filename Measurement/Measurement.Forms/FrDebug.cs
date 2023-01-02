using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core.Motions;
using LZ.CNC.Measurement.Core;
using DY.Core.Forms;
using System.Threading;
using System.Diagnostics;

namespace LZ.CNC.Measurement.Forms
{
    public partial class FrDebug : TabForm
    {
        public FrDebug()
        {
            InitializeComponent();
            Init();
            InitAxisDebug();
            InitIO();
            InitInputBox();
            Init_Carry_Value();
            InitEvent();
            InitGroupBoxShow();
        }

        MeasurementAxis[] leftsmAxises = new MeasurementAxis[] { MeasurementContext.Worker.Axis_LeftSM_X, MeasurementContext.Worker.Axis_LeftSM_Y, MeasurementContext.Worker.Axis_LeftSM_Z };
        MeasurementAxis[] MidsmAxises = new MeasurementAxis[] { MeasurementContext.Worker.Axis_MidSM_X, MeasurementContext.Worker.Axis_MidSM_Y, MeasurementContext.Worker.Axis_MidSM_Z };
        MeasurementAxis[] RightsmAxises = new MeasurementAxis[] { MeasurementContext.Worker.Axis_RightSM_X, MeasurementContext.Worker.Axis_RightSM_Y, MeasurementContext.Worker.Axis_RightSM_Z };
        MeasurementData.RecipeDataItem recipe = null;
        MeasurementConfig Config = null;

        MeasurementWorker _Worker = MeasurementContext.Worker;
        private void InitGroupBoxShow()
        {
            if (Config.IsLoadYAxisEnable)
            {
                label34.Visible = true;
                label35.Visible = true;
                var_Feed_wait_y.Visible = true;
                var_feed_work_y.Visible = true;
                btnEx_Feed_belty_wait_Set.Visible = true;
                btnEx_Feed_belty_work_Set.Visible = true;
                btnEx_Feed_belty_wait_Move.Visible = true;
                btnEx_Feed_belty_work_Move.Visible = true;

                llll.Visible = true;
                Var_QRCode.Visible = true;
                button148.Visible = true;
                button149.Visible = true;

                button51.Visible = true;
                axisDebug71.Visible = true;
            }
            else
            {
                label34.Visible = false;
                label35.Visible = false;
                var_Feed_wait_y.Visible = false;
                var_feed_work_y.Visible = false;
                btnEx_Feed_belty_wait_Set.Visible = false;
                btnEx_Feed_belty_work_Set.Visible = false;
                btnEx_Feed_belty_wait_Move.Visible = false;
                btnEx_Feed_belty_work_Move.Visible = false;

                llll.Visible = false;
                Var_QRCode.Visible = false;
                button148.Visible = false;
                button149.Visible = false;

                button51.Visible = false;
                axisDebug71.Visible = false;
            }
            if (Config.IsFeedCylinderEnable)
            {
                groupBoxEx19.Visible = true;
                groupBoxEx41.Visible = true;
                groupBoxEx44.Visible = true;
            }
            else
            {
                groupBoxEx19.Visible = false;
                groupBoxEx41.Visible = false;
                groupBoxEx44.Visible = false;
            }

            if (Config.IsControlUpStreamEnable)
            {
                button86.Visible = true;
                button92.Visible = true;
                button87.Visible = true;
                button51.Visible = true;
            }
            else
            {
                button86.Visible = false;
                button92.Visible = false;
                button87.Visible = false;
                button51.Visible = false;
            }

            if (!Config.IsLoadZCylinder)
            {
                axisDebug70.Visible = true;
                groupBoxEx46.Visible = false;

                var_Feed_wait_z.Visible = true;
                var_Feed_pick_z.Visible = true;
                var_Feed_pull_z1.Visible = true;
                var_Feed_pull_z2.Visible = true;
                var_Feed_pull_z3.Visible = true;
                button85.Visible = true;
                button131.Visible = true;
                button132.Visible = true;
                button133.Visible = true;
                button134.Visible = true;
            }
            else
            {
                axisDebug70.Visible = false;
                groupBoxEx46.Visible = true;

                var_Feed_wait_z.Visible = false;
                var_Feed_pick_z.Visible = false;
                var_Feed_pull_z1.Visible = false;
                var_Feed_pull_z2.Visible = false;
                var_Feed_pull_z3.Visible = false;
                button85.Visible = false;
                button131.Visible = false;
                button132.Visible = false;
                button133.Visible = false;
                button134.Visible = false;
            }

            if (!Config.IsTransferZCylinder)
            {
                axisDebug84.Visible = true;

                var_transfer_wait_z.Visible = true;
                var_transfer_pick_z1.Visible = true;
                var_transfer_pick_z2.Visible = true;
                var_transfer_pick_z3.Visible = true;
                var_transfer_pull_NGA_z.Visible = true;
                var_transfer_pull_NGB_z.Visible = true;
                var_transfer_pull_NGC_z.Visible = true;
                var_transfer_pull_z1.Visible = true;
                var_transfer_pull_z2.Visible = true;
                var_transfer_pull_z3.Visible = true;
                btn_Transfer_Standy_1.Visible = true;
                btn_Transfer_Fetch_1.Visible = true;
                btn_Transfer_Fetch_2.Visible = true;
                btn_Transfer_Fetch_3.Visible = true;
                btn_Transfer_NGDrop_1.Visible = true;
                btn_Transfer_NGDrop_2.Visible = true;
                btn_Transfer_NGDrop_3.Visible = true;
                btn_Transfer_Drop_1.Visible = true;
                btn_Transfer_Drop_2.Visible = true;
                btn_Transfer_Drop_3.Visible = true;

            }
            else
            {
                axisDebug84.Visible = false;

                var_transfer_wait_z.Visible = false;
                var_transfer_pick_z1.Visible = false;
                var_transfer_pick_z2.Visible = false;
                var_transfer_pick_z3.Visible = false;
                var_transfer_pull_NGA_z.Visible = false;
                var_transfer_pull_NGB_z.Visible = false;
                var_transfer_pull_NGC_z.Visible = false;
                var_transfer_pull_z1.Visible = false;
                var_transfer_pull_z2.Visible = false;
                var_transfer_pull_z3.Visible = false;
                btn_Transfer_Standy_1.Visible = false;
                btn_Transfer_Fetch_1.Visible = false;
                btn_Transfer_Fetch_2.Visible = false;
                btn_Transfer_Fetch_3.Visible = false;
                btn_Transfer_NGDrop_1.Visible = false;
                btn_Transfer_NGDrop_2.Visible = false;
                btn_Transfer_NGDrop_3.Visible = false;
                btn_Transfer_Drop_1.Visible = false;
                btn_Transfer_Drop_2.Visible = false;
                btn_Transfer_Drop_3.Visible = false;
            }


        }
        private void FrDebug_Load(object sender, EventArgs e)
        {
            _Worker.WorkStatusChanged += Worker_WorkStatusChanged;

        }

        private void Worker_WorkStatusChanged(object sender, EventArgs e)
        {
            OnLoginChanged();
        }

        protected override void OnLoginChanged()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(() => RefreshUI()));
            }
            else
            {
                RefreshUI();
            }
        }


        private void RefreshUI()
        {
            panel_tear1.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
                    || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            panel_tear2.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            panel_tear3.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);

            groupBoxEx96.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
                || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            groupBoxEx97.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            groupBoxEx98.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);

            panel4.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            panel6.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            panel7.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);

            panel1.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            panel2.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            panel3.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
               || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);

            grp1.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
            groupBox1.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
            groupBox2.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
            groupBoxEx25.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
            groupBoxEx39.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
            groupBoxEx40.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
            groupBoxEx100.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
            groupBoxEx99.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
            groupBoxEx101.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);


            if (_Worker.workstatus != WorkStatuses.Running && _Worker.workstatus != WorkStatuses.Stoping)
            {
                if (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None)
                {
                    CtrlEnabled();
                }
            }
            else
            {
                if (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer)
                {
                    CtrlDisabled();
                }

            }




        }


        private void CtrlDisabled()
        {
            #region 
            btn_leftsmccdlearn.Enabled = false;
            btn_leftsmccdlocate.Enabled = false;
            btn_midsmccdlearn.Enabled = false;
            btn_midsmccdlocate.Enabled = false;
            btn_rightsmccdlearn.Enabled = false;
            btn_rightsmccdlocate.Enabled = false;
            groupBoxEx1.Enabled = false;
            groupBoxEx2.Enabled = false;
            groupBoxEx15.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            grp1.Enabled = false;

            groupBox16.Enabled = false;
            groupBox17.Enabled = false;
            groupBox8.Enabled = false;

            ctrlSM1.Enabled = true;
            ctrlSM2.Enabled = true;
            ctrlSM3.Enabled = true;
            #endregion


            #region  bend
            btn_Locate_leftbend_wait1.Enabled = false;
            button10.Enabled = false;
            button22.Enabled = false;
            btn_Locate_leftbend_ccdpos.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button19.Enabled = false;
            button9.Enabled = false;
            button11.Enabled = false;
            button27.Enabled = false;
            button20.Enabled = false;
            button12.Enabled = false;
            button18.Enabled = false;
            btn_Locate_leftbend_wait2.Enabled = false;
            btn_locate_leftbend_ccddwy.Enabled = false;
            btn_learn_leftbend_ccddwy.Enabled = false;
            btn_locate_leftbend_work2.Enabled = false;
            btn_locate_leftbend_work1.Enabled = false;


            btn_locate_midbend_wait1.Enabled = false;
            btn_learn_midbend_wait1.Enabled = false;
            btn_locate_midbend_CCDPos.Enabled = false;
            btn_learn_midbend_ccdpos.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button13.Enabled = false;
            button14.Enabled = false;
            button28.Enabled = false;
            button31.Enabled = false;
            btn_locate_midbend_wait2.Enabled = false;
            btn_learn_midbend_wait2.Enabled = false;
            btn_locate_midbend_ccddwy.Enabled = false;
            btn_learn_midbend_ccddwy.Enabled = false;
            btn_locate_midbend_work2.Enabled = false;
            btn_locate_midbend_work1.Enabled = false;
            btn_learn_midbend_work1.Enabled = false;
            btn_learn_midbend_work2.Enabled = false;


            btn_locate_rightbend_wait1.Enabled = false;
            btn_learn_rightbend_wait1.Enabled = false;
            btn_locate_rightbend_ccdpos.Enabled = false;
            btn_learn_rightbend_ccdpos.Enabled = false;
            button5.Enabled = false;
            button7.Enabled = false;
            button15.Enabled = false;
            button16.Enabled = false;
            button32.Enabled = false;
            button36.Enabled = false;
            btn_locate_rightbend_wait2.Enabled = false;
            btn_learn_rightbend_wait2.Enabled = false;
            btn_locate_rightbend_ccddwy.Enabled = false;
            btn_learn_rightbend_ccddwy.Enabled = false;
            btn_locate_rightbend_work2.Enabled = false;
            btn_locate_rightbend_work1.Enabled = false;
            btn_bend_rightbend_work1.Enabled = false;
            btn_learn_rightbend_work2.Enabled = false;

            groupBoxEx16.Enabled = false;
            groupBoxEx38.Enabled = false;
            groupBoxEx24.Enabled = false;
            groupBoxEx23.Enabled = false;
            groupBoxEx52.Enabled = false;
            groupBoxEx51.Enabled = false;
            groupBoxEx25.Enabled = false;
            groupBoxEx39.Enabled = false;
            groupBoxEx40.Enabled = false;

            #endregion

            #region 
            groupBoxEx19.Enabled = false;
            groupBoxEx20.Enabled = false;
            groupBoxEx26.Enabled = false;
            groupBoxEx99.Enabled = false;
            groupBoxEx100.Enabled = false;
            groupBoxEx102.Enabled = false;
            groupBoxEx27.Enabled = false;
            groupBoxEx28.Enabled = false;
            groupBoxEx101.Enabled = false;

            btnEx_discharge_pickNG_Move.Enabled = false; ;
            btnEx_discharge_pullNG_set.Enabled = false;
            btnEx_discharge_pickOK_Move.Enabled = false;
            btnEx_discharge_pullOK_set.Enabled = false;
            btnEx_discharge_pick3_Move.Enabled = false;
            btnEx_discharge_pick3_set.Enabled = false;
            btnEx_discharge_pick2_Move.Enabled = false;
            btnEx_discharge_pick2_set.Enabled = false;
            btnEx_discharge_pick1_Move.Enabled = false;
            btnEx_discharge_pick1_set.Enabled = false;
            btnEx_discharge_wait_Move.Enabled = false;
            btnEx_discharge_wait_Set.Enabled = false;

            btnEx_Feed_belty_work_Move.Enabled = false;
            btnEx_Feed_belty_work_Set.Enabled = false;
            btnEx_Feed_belty_wait_Move.Enabled = false;
            btnEx_Feed_belty_wait_Set.Enabled = false;
            btnEx_Feed_pull_x3_Move.Enabled = false;
            btnEx_Feed_pull_x3_Set.Enabled = false;
            btnEx_Feed_pull_x2_Move.Enabled = false;
            btnEx_Feed_pull_x2_Set.Enabled = false;
            btnEx_Feed_pull_x1_Move.Enabled = false;
            btnEx_Feed_pull_x1_Set.Enabled = false;
            btnEx_Feed_pick_Move.Enabled = false;
            btnEx_Feed_pick_Set.Enabled = false;
            btnEx_Feed_wait_Move.Enabled = false;
            btnEx_Feed_wait_Set.Enabled = false;


            btnEx_Transfer_pull3_Move.Enabled = false;
            btnEx_Transfer_pull3_Set.Enabled = false;
            btnEx_Transfer_pull2_Move.Enabled = false;
            btnEx_Transfer_pull2_Set.Enabled = false;
            btnEx_Transfer_pull1_Move.Enabled = false;
            btnEx_Transfer_pull1_Set.Enabled = false;
            btnEx_Transfer_NGC_Move.Enabled = false;
            btnEx_Transfer_NGC_Set.Enabled = false;
            btnEx_Transfer_NGB_Move.Enabled = false;
            btnEx_Transfer_NGB_Set.Enabled = false;
            btnEx_Transfer_NGA_Move.Enabled = false;
            btnEx_Transfer_NGA_Set.Enabled = false;

            btnEx_Transfer_pick3_Set.Enabled = false;
            btnEx_Transfer_pick2_Set.Enabled = false;
            btnEx_Transfer_pick1_Set.Enabled = false;
            btnEx_Transfer_wait_Set.Enabled = false;
            btnEx_Transfer_pick3_Move.Enabled = false;
            btnEx_Transfer_pick2_Move.Enabled = false;
            btnEx_Transfer_pick1_Move.Enabled = false;
            btnEx_Transfer_wait_Move.Enabled = false;


            button85.Enabled = false;
            button131.Enabled = false;
            button132.Enabled = false;
            button133.Enabled = false;
            button134.Enabled = false;
            button148.Enabled = false;
            button149.Enabled = false;

            btn_Transfer_Standy_1.Enabled = false;
            btn_Transfer_Fetch_1.Enabled = false;
            btn_Transfer_Fetch_2.Enabled = false;
            btn_Transfer_Fetch_3.Enabled = false;
            btn_Transfer_NGDrop_1.Enabled = false;
            btn_Transfer_NGDrop_2.Enabled = false;
            btn_Transfer_NGDrop_3.Enabled = false;
            btn_Transfer_Drop_1.Enabled = false;
            btn_Transfer_Drop_2.Enabled = false;
            btn_Transfer_Drop_3.Enabled = false;

            btn_Discharge_Standy_1.Enabled = false;
            btn_Discharge_Fetch_1.Enabled = false;
            btn_Discharge_Fetch_2.Enabled = false;
            btn_Discharge_Fetch_3.Enabled = false;
            btn_Discharge_OKDrop_1.Enabled = false;
            btn_Discharge_NGDrop_1.Enabled = false;
            #endregion

        }

        private void CtrlEnabled()
        {
            #region 
            btn_leftsmccdlearn.Enabled = true;
            btn_leftsmccdlocate.Enabled = true;
            btn_midsmccdlearn.Enabled = true;
            btn_midsmccdlocate.Enabled = true;
            btn_rightsmccdlearn.Enabled = true;
            btn_rightsmccdlocate.Enabled = true;
            groupBoxEx1.Enabled = true;
            groupBoxEx2.Enabled = true;
            groupBoxEx15.Enabled = true;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            grp1.Enabled = true;

            groupBox16.Enabled = true;
            groupBox17.Enabled = true;
            groupBox8.Enabled = true;

            ctrlSM1.Enabled = true;
            ctrlSM2.Enabled = true;
            ctrlSM3.Enabled = true;
            #endregion


            #region  bend
            btn_Locate_leftbend_wait1.Enabled = true;
            button10.Enabled = true;
            button22.Enabled = true;
            btn_Locate_leftbend_ccdpos.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button19.Enabled = true;
            button9.Enabled = true;
            button11.Enabled = true;
            button27.Enabled = true;
            button20.Enabled = true;
            button12.Enabled = true;
            button18.Enabled = true;
            btn_Locate_leftbend_wait2.Enabled = true;
            btn_locate_leftbend_ccddwy.Enabled = true;
            btn_learn_leftbend_ccddwy.Enabled = true;
            btn_locate_leftbend_work2.Enabled = true;
            btn_locate_leftbend_work1.Enabled = true;


            btn_locate_midbend_wait1.Enabled = true;
            btn_learn_midbend_wait1.Enabled = true;
            btn_locate_midbend_CCDPos.Enabled = true;
            btn_learn_midbend_ccdpos.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button13.Enabled = true;
            button14.Enabled = true;
            button28.Enabled = true;
            button31.Enabled = true;
            btn_locate_midbend_wait2.Enabled = true;
            btn_learn_midbend_wait2.Enabled = true;
            btn_locate_midbend_ccddwy.Enabled = true;
            btn_learn_midbend_ccddwy.Enabled = true;
            btn_locate_midbend_work2.Enabled = true;
            btn_locate_midbend_work1.Enabled = true;
            btn_learn_midbend_work1.Enabled = true;
            btn_learn_midbend_work2.Enabled = true;


            btn_locate_rightbend_wait1.Enabled = true;
            btn_learn_rightbend_wait1.Enabled = true;
            btn_locate_rightbend_ccdpos.Enabled = true;
            btn_learn_rightbend_ccdpos.Enabled = true;
            button5.Enabled = true;
            button7.Enabled = true;
            button15.Enabled = true;
            button16.Enabled = true;
            button32.Enabled = true;
            button36.Enabled = true;
            btn_locate_rightbend_wait2.Enabled = true;
            btn_learn_rightbend_wait2.Enabled = true;
            btn_locate_rightbend_ccddwy.Enabled = true;
            btn_learn_rightbend_ccddwy.Enabled = true;
            btn_locate_rightbend_work2.Enabled = true;
            btn_locate_rightbend_work1.Enabled = true;
            btn_bend_rightbend_work1.Enabled = true;
            btn_learn_rightbend_work2.Enabled = true;

            groupBoxEx16.Enabled = true;
            groupBoxEx38.Enabled = true;
            groupBoxEx24.Enabled = true;
            groupBoxEx23.Enabled = true;
            groupBoxEx52.Enabled = true;
            groupBoxEx51.Enabled = true;
            groupBoxEx25.Enabled = true;
            groupBoxEx39.Enabled = true;
            groupBoxEx40.Enabled = true;

            #endregion

            #region 
            groupBoxEx19.Enabled = true;
            groupBoxEx20.Enabled = true;
            groupBoxEx26.Enabled = true;
            groupBoxEx99.Enabled = true;
            groupBoxEx100.Enabled = true;
            groupBoxEx102.Enabled = true;
            groupBoxEx27.Enabled = true;
            groupBoxEx28.Enabled = true;
            groupBoxEx101.Enabled = true;

            btnEx_discharge_pickNG_Move.Enabled = true; ;
            btnEx_discharge_pullNG_set.Enabled = true;
            btnEx_discharge_pickOK_Move.Enabled = true;
            btnEx_discharge_pullOK_set.Enabled = true;
            btnEx_discharge_pick3_Move.Enabled = true;
            btnEx_discharge_pick3_set.Enabled = true;
            btnEx_discharge_pick2_Move.Enabled = true;
            btnEx_discharge_pick2_set.Enabled = true;
            btnEx_discharge_pick1_Move.Enabled = true;
            btnEx_discharge_pick1_set.Enabled = true;
            btnEx_discharge_wait_Move.Enabled = true;
            btnEx_discharge_wait_Set.Enabled = true;

            btnEx_Feed_belty_work_Move.Enabled = true;
            btnEx_Feed_belty_work_Set.Enabled = true;
            btnEx_Feed_belty_wait_Move.Enabled = true;
            btnEx_Feed_belty_wait_Set.Enabled = true;
            btnEx_Feed_pull_x3_Move.Enabled = true;
            btnEx_Feed_pull_x3_Set.Enabled = true;
            btnEx_Feed_pull_x2_Move.Enabled = true;
            btnEx_Feed_pull_x2_Set.Enabled = true;
            btnEx_Feed_pull_x1_Move.Enabled = true;
            btnEx_Feed_pull_x1_Set.Enabled = true;
            btnEx_Feed_pick_Move.Enabled = true;
            btnEx_Feed_pick_Set.Enabled = true;
            btnEx_Feed_wait_Move.Enabled = true;
            btnEx_Feed_wait_Set.Enabled = true;


            btnEx_Transfer_pull3_Move.Enabled = true;
            btnEx_Transfer_pull3_Set.Enabled = true;
            btnEx_Transfer_pull2_Move.Enabled = true;
            btnEx_Transfer_pull2_Set.Enabled = true;
            btnEx_Transfer_pull1_Move.Enabled = true;
            btnEx_Transfer_pull1_Set.Enabled = true;
            btnEx_Transfer_NGC_Move.Enabled = true;
            btnEx_Transfer_NGC_Set.Enabled = true;
            btnEx_Transfer_NGB_Move.Enabled = true;
            btnEx_Transfer_NGB_Set.Enabled = true;
            btnEx_Transfer_NGA_Move.Enabled = true;
            btnEx_Transfer_NGA_Set.Enabled = true;

            btnEx_Transfer_pick3_Set.Enabled = true;
            btnEx_Transfer_pick2_Set.Enabled = true;
            btnEx_Transfer_pick1_Set.Enabled = true;
            btnEx_Transfer_wait_Set.Enabled = true;
            btnEx_Transfer_pick3_Move.Enabled = true;
            btnEx_Transfer_pick2_Move.Enabled = true;
            btnEx_Transfer_pick1_Move.Enabled = true;
            btnEx_Transfer_wait_Move.Enabled = true;


            button85.Enabled = true;
            button131.Enabled = true;
            button132.Enabled = true;
            button133.Enabled = true;
            button134.Enabled = true;
            button148.Enabled = true;
            button149.Enabled = true;

            btn_Transfer_Standy_1.Enabled = true;
            btn_Transfer_Fetch_1.Enabled = true;
            btn_Transfer_Fetch_2.Enabled = true;
            btn_Transfer_Fetch_3.Enabled = true;
            btn_Transfer_NGDrop_1.Enabled = true;
            btn_Transfer_NGDrop_2.Enabled = true;
            btn_Transfer_NGDrop_3.Enabled = true;
            btn_Transfer_Drop_1.Enabled = true;
            btn_Transfer_Drop_2.Enabled = true;
            btn_Transfer_Drop_3.Enabled = true;

            btn_Discharge_Standy_1.Enabled = true;
            btn_Discharge_Fetch_1.Enabled = true;
            btn_Discharge_Fetch_2.Enabled = true;
            btn_Discharge_Fetch_3.Enabled = true;
            btn_Discharge_OKDrop_1.Enabled = true;
            btn_Discharge_NGDrop_1.Enabled = true;
            #endregion

        }
        private void InitEvent()
        {
            Inpb_leftsmccdy.ValueChanged += LeftSMInputBox_ValueChanged;
            //Inpb_leftsmwaitx.ValueChanged += LeftSMInputBox_ValueChanged;
            //Inpb_leftsmwaity.ValueChanged += LeftSMInputBox_ValueChanged;
            //Inpb_leftsmwaitz.ValueChanged += LeftSMInputBox_ValueChanged;
            Inpb_leftsmccdx.ValueChanged += LeftSMInputBox_ValueChanged;
            //Inpb_leftsmdischargez.ValueChanged += LeftSMInputBox_ValueChanged;
            //Inpb_leftsmdischargex.ValueChanged += LeftSMInputBox_ValueChanged;
            //Inpb_leftsmloady.ValueChanged += LeftSMInputBox_ValueChanged;
            //Inpb_leftsmloadx.ValueChanged += LeftSMInputBox_ValueChanged;
            //Inpb_leftsmloadz.ValueChanged += LeftSMInputBox_ValueChanged;
            //Inpb_leftsmdischargey.ValueChanged += LeftSMInputBox_ValueChanged;
            Inpb_leftsmXspeed.ValueChanged += LeftSMInputBox_ValueChanged;
            Inpb_leftsmYspeed.ValueChanged += LeftSMInputBox_ValueChanged;
            Inpb_leftsmZspeed.ValueChanged += LeftSMInputBox_ValueChanged;


            Inpb_midsmccdy.ValueChanged += MidSMInputBox_ValueChanged;
            //Inpb_midsmwaitx.ValueChanged += MidSMInputBox_ValueChanged;
            //Inpb_midsmwaity.ValueChanged += MidSMInputBox_ValueChanged;
            //Inpb_midsmwaitz.ValueChanged += MidSMInputBox_ValueChanged;
            Inpb_midsmccdx.ValueChanged += MidSMInputBox_ValueChanged;
            //Inpb_midsmdischargez.ValueChanged += MidSMInputBox_ValueChanged;
            //Inpb_midsmdischargex.ValueChanged += MidSMInputBox_ValueChanged;
            //Inpb_midsmloady.ValueChanged += MidSMInputBox_ValueChanged;
            //Inpb_midsmloadx.ValueChanged += MidSMInputBox_ValueChanged;
            //Inpb_midsmloadz.ValueChanged += MidSMInputBox_ValueChanged;
            //Inpb_midsmdischargey.ValueChanged += MidSMInputBox_ValueChanged;
            Inpb_midsmxspeed.ValueChanged += MidSMInputBox_ValueChanged;
            Inpb_midsmyspeed.ValueChanged += MidSMInputBox_ValueChanged;
            Inpb_midsmzspeed.ValueChanged += MidSMInputBox_ValueChanged;

            Inpb_rightsmccdy.ValueChanged += RightSMInputBox_ValueChanged;
            //Inpb_rightsmwaitx.ValueChanged += RightSMInputBox_ValueChanged;
            //Inpb_rightsmwaity.ValueChanged += RightSMInputBox_ValueChanged;
            //Inpb_rightsmwaitz.ValueChanged += RightSMInputBox_ValueChanged;
            Inpb_rightsmccdx.ValueChanged += RightSMInputBox_ValueChanged;
            //Inpb_rightsmdischargez.ValueChanged += RightSMInputBox_ValueChanged;
            //Inpb_rightsmdischargex.ValueChanged += RightSMInputBox_ValueChanged;
            //Inpb_rightsmloady.ValueChanged += RightSMInputBox_ValueChanged;
            //Inpb_rightsmloadx.ValueChanged += RightSMInputBox_ValueChanged;
            //Inpb_rightsmloadz.ValueChanged += RightSMInputBox_ValueChanged;
            //Inpb_rightsmdischargey.ValueChanged += RightSMInputBox_ValueChanged;
            Inpb_rightsmxspeed.ValueChanged += RightSMInputBox_ValueChanged;
            Inpb_rightsmyspeed.ValueChanged += RightSMInputBox_ValueChanged;
            Inpb_rightsmzspeed.ValueChanged += RightSMInputBox_ValueChanged;
            _Worker.RecipeChanged += Worker_RecipeChanged;
        }



        private void InitInputBox()
        {
            recipe = MeasurementContext.Data.CurrentRecipeData;
            if (recipe != null)
            {
                //Inpb_leftsmloadx.Value = recipe.SMPosition[0].Lsm_loadX;
                //Inpb_leftsmloady.Value = recipe.SMPosition[0].Lsm_loadY;
                //Inpb_leftsmloadz.Value = recipe.SMPosition[0].Lsm_LoadZ;
                Inpb_leftsmccdy.Value = recipe.SMPosition[0].Lsm_CCDY;
                Inpb_leftsmccdx.Value = recipe.SMPosition[0].Lsm_CCDX;
                //Inpb_leftsmdischargex.Value = recipe.SMPosition[0].Lsm_DischargeX;
                //Inpb_leftsmdischargez.Value = recipe.SMPosition[0].Lsm_DischargeZ;
                //Inpb_leftsmdischargey.Value = recipe.SMPosition[0].Lsm_DischargeY;
                //Inpb_leftsmwaitx.Value = recipe.SMPosition[0].Lsm_WaitX;
                //Inpb_leftsmwaity.Value = recipe.SMPosition[0].Lsm_WaitY;
                Inp_leftsmZSafe.Value = recipe.SMPosition[0].Lsm_WaitZ;
                Inpb_leftsmXspeed.Value = recipe.SMPosition[0].SM_XSpeed;
                Inpb_leftsmYspeed.Value = recipe.SMPosition[0].SM_YSpeed;
                Inpb_leftsmZspeed.Value = recipe.SMPosition[0].SM_ZSpeed;
                Inp_leftsmpressx.Value = recipe.SMPosition[0].Lsm_PressX;
                Inp_leftsmpressy.Value = recipe.SMPosition[0].Lsm_PressY;
                Inp_leftsm_pressz.Value = recipe.SMPosition[0].Lsm_PressZ;
                Inp_leftsmZDist.Value = recipe.SMPosition[0].SM_ZDist;
                Inp_leftsmZDistSpeed.Value = recipe.SMPosition[0].SM_ZDist_Speed;
                Input_leftsm_PasteSpeed.Value = recipe.SMPosition[0].Paste_Speed;

                Inp_midsmZsafe.Value = recipe.SMPosition[1].Lsm_WaitZ;
                Inp_rightsmZsafe.Value = recipe.SMPosition[2].Lsm_WaitZ;

                //Inpb_midsmloadx.Value = recipe.SMPosition[1].Lsm_loadX;
                //Inpb_midsmloady.Value = recipe.SMPosition[1].Lsm_loadY;
                //Inpb_midsmloadz.Value = recipe.SMPosition[1].Lsm_LoadZ;
                Inpb_midsmccdy.Value = recipe.SMPosition[1].Lsm_CCDY;
                Inpb_midsmccdx.Value = recipe.SMPosition[1].Lsm_CCDX;
                //Inpb_midsmdischargex.Value = recipe.SMPosition[1].Lsm_DischargeX;
                //Inpb_midsmdischargez.Value = recipe.SMPosition[1].Lsm_DischargeZ;
                //Inpb_midsmdischargey.Value = recipe.SMPosition[1].Lsm_DischargeY;
                //Inpb_midsmwaitx.Value = recipe.SMPosition[1].Lsm_WaitX;
                //Inpb_midsmwaity.Value = recipe.SMPosition[1].Lsm_WaitY;
                //Inpb_midsmwaitz.Value = recipe.SMPosition[1].Lsm_WaitZ;
                Inpb_midsmxspeed.Value = recipe.SMPosition[1].SM_XSpeed;
                Inpb_midsmyspeed.Value = recipe.SMPosition[1].SM_YSpeed;
                Inpb_midsmzspeed.Value = recipe.SMPosition[1].SM_ZSpeed;
                Inp_midsmpressx.Value = recipe.SMPosition[1].Lsm_PressX;
                Inp_midsmpressy.Value = recipe.SMPosition[1].Lsm_PressY;
                Inp_midsm_pressz.Value = recipe.SMPosition[1].Lsm_PressZ;
                Inp_midsmZDist.Value = recipe.SMPosition[1].SM_ZDist;
                Inp_midsmZDistSpeed.Value = recipe.SMPosition[1].SM_ZDist_Speed;
                Input_midsm_PasteSpeed.Value = recipe.SMPosition[1].Paste_Speed;



                //Inpb_rightsmloadx.Value = recipe.SMPosition[2].Lsm_loadX;
                //Inpb_rightsmloady.Value = recipe.SMPosition[2].Lsm_loadY;
                //Inpb_rightsmloadz.Value = recipe.SMPosition[2].Lsm_LoadZ;
                Inpb_rightsmccdy.Value = recipe.SMPosition[2].Lsm_CCDY;
                Inpb_rightsmccdx.Value = recipe.SMPosition[2].Lsm_CCDX;
                //Inpb_rightsmdischargex.Value = recipe.SMPosition[2].Lsm_DischargeX;
                //Inpb_rightsmdischargez.Value = recipe.SMPosition[2].Lsm_DischargeZ;
                //Inpb_rightsmdischargey.Value = recipe.SMPosition[2].Lsm_DischargeY;
                //Inpb_rightsmwaitx.Value = recipe.SMPosition[2].Lsm_WaitX;
                //Inpb_rightsmwaity.Value = recipe.SMPosition[2].Lsm_WaitY;
                //Inpb_rightsmwaitz.Value = recipe.SMPosition[2].Lsm_WaitZ;
                Inpb_rightsmxspeed.Value = recipe.SMPosition[2].SM_XSpeed;
                Inpb_rightsmyspeed.Value = recipe.SMPosition[2].SM_YSpeed;
                Inpb_rightsmzspeed.Value = recipe.SMPosition[2].SM_ZSpeed;
                Inp_rightsmpressx.Value = recipe.SMPosition[2].Lsm_PressX;
                Inp_rightsmpressy.Value = recipe.SMPosition[2].Lsm_PressY;
                Inp_rightsm_pressz.Value = recipe.SMPosition[2].Lsm_PressZ;
                Inp_rightsmZDist.Value = recipe.SMPosition[2].SM_ZDist;
                Inp_rightsmZDistSpeed.Value = recipe.SMPosition[2].SM_ZDist_Speed;
                Input_rightsm_PasteSpeed.Value = recipe.SMPosition[2].Paste_Speed;
                Inp_Leftbend_WaitPt_DWX.Value = recipe.LeftBend_DWX_SafePos;

                Inp_Leftbend_WaitPt_DWR.Value = recipe.LeftBend_DWR_SafePos;
                Inp_Leftbend_WorkPt_DWX.Value = recipe.LeftBend_DWX_WorkPos;
                Inp_LeftBend_WorkPt_DWY.Value = recipe.LeftBend_DWY_WorkPos;
                Inp_Leftbend_WorkPt_DWW.Value = recipe.LeftBend_DWW_WorkPos;
                Inp_Leftbend_WorkPt_DWR.Value = recipe.LeftBend_DWR_WorkPos;
                Inp_Leftbend_CCDX.Value = recipe.LeftBend_CCDPos_X;
                Inp_Leftbend_CCDY.Value = recipe.LeftBend_CCDPos_Y;
                Inp_Leftbend_CCDY2.Value = recipe.LeftBend_CCDPos_Y2;
                Inp_Leftbend_WorkPt_Y.Value = recipe.LeftBend_Y_WorkPos;
                Inp_LeftBendR_Ypos.Value = recipe.LeftBendR_Ypos;
                Inp_LeftBend_PressPT_X.Value = recipe.LeftBend_PressPt_X;
                Inp_LeftBend_PressPt_Y.Value = recipe.LeftBend_PressPt_Y;
                Inp_Leftbend_CCD_DWY.Value = recipe.LeftBend_CCD_DWY;
                Inp_LeftBend_DWW_Base.Value = recipe.LeftBend_DWW_BasePos;
                Inp_leftbZB_speed.Value = recipe.LeftZB_Speed;
                Inp_leftYB_time.Value = recipe.LeftYB_Time;
                //对位保护
                Inp_Leftbend_Xlowlimit.Value = recipe.Leftbend_Xlowlimit;
                Inp_Leftbend_Ylowlimit.Value = recipe.Leftbend_Ylowlimit;
                Inp_Leftbend_Wlowlimit.Value = recipe.Leftbend_Wlowlimit;
                Inp_Leftbend_XUpperlimit.Value = recipe.Leftbend_XUpperlimit;
                Inp_Leftbend_YUpperlimit.Value = recipe.Leftbend_YUpperlimit;
                Inp_Leftbend_WUpperlimit.Value = recipe.Leftbend_WUpperlimit;



                Inp_Midbend_WaitPt_DWX.Value = recipe.MidBend_DWX_SafePos;
                Inp_Midbend_WaitPt_DWR.Value = recipe.MidBend_DWR_SafePos;
                Inp_Midbend_WorkPt_DWX.Value = recipe.MidBend_DWX_WorkPos;
                Inp_Midbend_WorkPt_DWY.Value = recipe.MidBend_DWY_WorkPos;
                Inp_Midbend_WorkPt_DWW.Value = recipe.MidBend_DWW_WorkPos;
                Inp_Midbend_WorkPt_DWR.Value = recipe.MidBend_DWR_WorkPos;
                Inp_Midbend_CCDX.Value = recipe.MidBend_CCDPos_X;
                Inp_Midbend_CCDY.Value = recipe.MidBend_CCDPos_Y;
                Inp_Midbend_CCDY2.Value = recipe.MidBend_CCDPos_Y2;

                Inp_Midbend_WorkPt_Y.Value = recipe.MidBend_Y_WorkPos;
                Inp_MidBendR_Ypos.Value = recipe.MidBendR_Ypos;
                Inp_MidBend_PressPt_X.Value = recipe.MidBend_PressPt_X;
                Inp_MidBend_PressPt_Y.Value = recipe.MidBend_PressPt_Y;
                Inp_Midbend_CCD_DWY.Value = recipe.MidBend_CCD_DWY;
                Inp_MidBend_DWW_Base.Value = recipe.MidBend_DWW_BasePos;
                Inp_midZB_speed.Value = recipe.MidZB_Speed;
                Inp_midYB_time.Value = recipe.MidYB_Time;
                //对位保护
                Inp_Midbend_Xlowlimit.Value = recipe.Midbend_Xlowlimit;
                Inp_Midbend_Ylowlimit.Value = recipe.Midbend_Ylowlimit;
                Inp_Midbend_Wlowlimit.Value = recipe.Midbend_Wlowlimit;
                Inp_Midbend_XUpperlimit.Value = recipe.Midbend_XUpperlimit;
                Inp_Midbend_YUpperlimit.Value = recipe.Midbend_YUpperlimit;
                Inp_Midbend_WUpperlimit.Value = recipe.Midbend_WUpperlimit;





                Inp_Rightbend_WaitPt_DWX.Value = recipe.RightBend_DWX_SafePos;
                Inp_Rightbend_WaitPt_DWR.Value = recipe.RightBend_DWR_SafePos;
                Inp_Rightbend_WorkPt_DWX.Value = recipe.RightBend_DWX_WorkPos;
                Inp_Rightbend_WorkPt_DWY.Value = recipe.RightBend_DWY_WorkPos;
                Inp_Rightbend_WorkPt_DWW.Value = recipe.RightBend_DWW_WorkPos;
                Inp_Rightbend_WorkPt_DWR.Value = recipe.RightBend_DWR_WorkPos;
                Inp_Rightbend_CCDX.Value = recipe.RightBend_CCDPos_X;
                Inp_Rightbend_CCDY.Value = recipe.RightBend_CCDPos_Y;
                Inp_Rightbend_CCDY2.Value = recipe.RightBend_CCDPos_Y2;
                Inp_RightBend_WorkPt_Y.Value = recipe.RightBend_Y_WorkPos;
                Inp_RightBendR_Ypos.Value = recipe.RightBendR_Ypos;
                Inp_RightBend_PressPt_X.Value = recipe.RightBend_PressPt_X;
                Inp_RightBend_PressPt_Y.Value = recipe.RightBend_PressPt_Y;
                Inp_Rightbend_CCD_DWY.Value = recipe.RightBend_CCD_DWY;
                Inp_RightBend_DWW_Base.Value = recipe.RightBend_DWW_BasePos;
                Inp_rightZB_speed.Value = recipe.RightZB_Speed;
                Inp_rightYB_time.Value = recipe.RightYB_Time;
                //对位保护
                Inp_Rightbend_Xlowlimit.Value = recipe.Rightbend_Xlowlimit;
                Inp_Rightbend_Ylowlimit.Value = recipe.Rightbend_Ylowlimit;
                Inp_Rightbend_Wlowlimit.Value = recipe.Rightbend_Wlowlimit;
                Inp_Rightbend_XUpperlimit.Value = recipe.Rightbend_XUpperlimit;
                Inp_Rightbend_YUpperlimit.Value = recipe.Rightbend_YUpperlimit;
                Inp_Rightbend_WUpperlimit.Value = recipe.Rightbend_WUpperlimit;





                Inp_Bend1_adjustspeed.Value = recipe.Bend1adjust_Speed;
                Inp_Bend2_adjustspeed.Value = recipe.Bend2adjust_Speed;
                Inp_Bend3_adjustspeed.Value = recipe.Bend3adjust_Speed;

                //ZGH20220912增加撕膜X轴安全位
                var_leftsmXsafe.Value = recipe.LeftSM_XSafePos;
                var_midsmXsafe.Value = recipe.MidSM_XSafePos;
                var_rightsmXsafe.Value = recipe.RightSM_XSafePos;

                ////ZGH20220624增加折弯R回安全位速度
                //if (recipe.Leftbend_RSafePosSpeed == 0)
                //{
                //    Inp_leftbend_Rsafeposspeed.Value = MeasurementContext.Worker.Axis_LeftBend_DWR.AxisSet.MoveSpeed;
                //    recipe.Leftbend_RSafePosSpeed = Inp_leftbend_Rsafeposspeed.Value;
                //}
                //else
                //{
                //    Inp_leftbend_Rsafeposspeed.Value = recipe.Leftbend_RSafePosSpeed;
                //}
                //if (recipe.Midbend_RSafePosSpeed == 0)
                //{
                //    Inp_midbend_Rsafeposspeed.Value = MeasurementContext.Worker.Axis_MidBend_DWR.AxisSet.MoveSpeed;
                //    recipe.Midbend_RSafePosSpeed = Inp_midbend_Rsafeposspeed.Value;
                //}
                //else
                //{
                //    Inp_midbend_Rsafeposspeed.Value = recipe.Midbend_RSafePosSpeed;
                //}
                //if (recipe.Rightbend_RSafePosSpeed == 0)
                //{
                //    Inp_rightbend_Rsafeposspeed.Value = MeasurementContext.Worker.Axis_RightBend_DWR.AxisSet.MoveSpeed;
                //    recipe.Rightbend_RSafePosSpeed = Inp_rightbend_Rsafeposspeed.Value;
                //}
                //else
                //{
                //    Inp_rightbend_Rsafeposspeed.Value = recipe.Rightbend_RSafePosSpeed;
                //}
            }

        }


        private void InitIO()
        {
            IO_LeftBend_PressCylinderDown_OUT.IO = Config.LeftBend_PressCylinderDown_IOOut;
            IO_Bend1Press.IO = Config.LeftBend_PressCylinderUp_IOOut;
            IOIn_Bend1_Press_UP.IO = Config.LeftBend_PressCylinder_UPIOIn;

            IO_Bend1Suck.IO = Config.LeftBend_SuckVacuum_IOOut;
            IOIn_Bend1SuckIn.IO = Config.LeftBend_stgVacuum_IOIn;
            IO_Bend1Claw.IO = Config.LeftBend_ClawCylinderOut_IOOut;
            IO_LeftBend_ClawCylinderBack_IOOut.IO = Config.LeftBend_ClawCylinderBack_IOOut;

            //IOIn_Bend1Optical.IO = Config.LeftBend_FPCOptical_IOIn;
            IOIn_Bend1Optical.Visible = false;
            IO_Bend1Blow.IO = Config.LeftBend_BlowVacuum_IOOut;

            IOIn_Bend1Press_Down.IO = Config.LeftBend_PressCylinder_DownIOIn;
            IO_Bend1_OPTCtrol.IO = Config.LeftBend_UPOPTControl_IOOutEx;


            IO_MidBend_PressCylinderDown_IOOut.IO = Config.MidBend_PressCylinderDown_IOOut;
            IO_Bend2Press.IO = Config.MidBend_PressCylinderUp_IOOut;
            IOIn_Bend2_Press_UP.IO = Config.MidBend_PressCylinder_UPIOIn;
            IO_Bend2Suck.IO = Config.MidBend_SuckVacuum_IOOut;
            IOIn_Bend2SuckIn.IO = Config.MidBend_stgVacuum_IOIn;
            IO_Bend2Claw.IO = Config.MidBend_ClawCylinderOut_IOOut;
            IO_MidBend_ClawCylinderBack_IOOut.IO = Config.MidBend_ClawCylinderBack_IOOut;

            //IOIn_Bend2Optical.IO = Config.MidBend_FPCOptical_IOIn;
            IOIn_Bend2Optical.Visible = false;
            IO_Bend2Blow.IO = Config.MidBend_BlowVacuum_IOOut;
            IO_Bend2_OPTCtrol.IO = Config.MidBend_UPOPTControl_IOOutEx;
            IOIn_Bend2_Press_Down.IO = Config.MidBend_PressCylinder_DownIOIn;





            IO_RightBend_PressCylinderDown_IOOut.IO = Config.RightBend_PressCylinderDown_IOOut;
            IO_Bend3Press.IO = Config.RightBend_PressCylinderUp_IOOut;
            IOIn_Bend3_Press_UP.IO = Config.RightBend_PressCylinder_UPIOIn;
            IOIn_Bend3Press_Down.IO = Config.RightBend_PressCylinder_DownIOIn;
            IO_Bend3Suck.IO = Config.RightBend_SuckVacuum_IOOut;
            IOIn_Bend3SuckIn.IO = Config.RightBend_stgVacuum_IOIn;
            IO_Bend3Claw.IO = Config.RightBend_ClawCylinderOut_IOOut;
            IO_RightBend_ClawCylinderBack_IOOut.IO = Config.RightBend_ClawCylinderBack_IOOut;
            //IOIn_Bend3Optical.IO = Config.RightBend_FPCOptical_IOIn;
            IOIn_Bend3Optical.Visible = false;
            IO_Bend3Blow.IO = Config.RightBend_BlowVacuum_IOOut;
            IO_Bend3_OPTCtrol.IO = Config.RightBend_UPOPTControl_IOOutEx;


            //IOIn_Bend3_RightCylinder_UP.IO;


            IO_Tear2_UDCylinder.IO = Config.MidSM_UDCylinder_IOOutEx;
            IOIn_Tear2_UD_UP.IO = Config.MidSM_UD_CylinderDownIOInEx;
            IOIn_Tear2_UD_Down.IO = Config.MidSM_UD_CylinderUPIOInEx;
            IO_Tear2Suck.IO = Config.MidSM_StgVacuum_IOOut;
            IOIn_Tear2Suck.IO = Config.MidSMVacuumIOInEx;
            IO_Tear2_FPCSuck.IO = Config.MidSM_StgFPCVacuum_IOOut;
            IO_Tear2_Blow.IO = Config.MidSM_StgBlowVacuum_IOOutEx;
            IO_Tear2_Reduce.IO = Config.MidSM_StgReduceVacuum_IOOutEx;
            IO_Tear2_GlueLock.IO = Config.MidSM_GlueLockCylinder_IOOutEx;
            IO_Tear2_GlueCylinder.IO = Config.MidSM_GlueCylinder_IOOutEx;
            IOIn_Tear2_GlueCylinder_UP.IO = Config.MidSM_GlueUD_CylinderUPIOInEx;
            IOIn_Tear2_GlueCylinder_Down.IO = Config.MidSM_GlueUD_CylinderDownIOInEx;
            IO_Tear2_RollerCylinder.IO = Config.MidSM_RollerCylinder_IOOutEx;
            IOIn_Tear2_RollerCylinder_UP.IO = Config.MidSM_RollerUD_CylinderUPIOInEx;
            IOIn_Tear2_RollerCylinder_Down.IO = Config.MidSM_RollerUD_CylinderDownIOIn;
            IO_Tear2_FBCylinder.IO = Config.MidSM_FBCylinder_IOOutEx;
            IOIn_Tear2_FB_Front.IO = Config.MidSM_FB_CylinderFrontIOInEx;
            IOIn_Tear2_FB_Back.IO = Config.MidSM_FB_CylinderBackIOInEx;

            IO_Tear2_LRCylinder.IO = Config.MidSM_LRCylinder_IOOutEx;
            IOIn_Tear2_LR_Left.IO = Config.MidSM_LR_CylinerLeftIOInEx;
            IOIn_Tear2_LR_Right.IO = Config.MidSM_LR_CylinderRightIOInEx;
            IO_Tear2Light.IO = Config.SMStation_OPTIOOutEx;

            IO_Tear3_UDCylinder.IO = Config.RightSM_UDCylinder_IOOutEx;
            IOIn_Tear3_UD_UP.IO = Config.RightSM_UD_CylinderUPIOIn;
            IOIn_Tear3_UD_Down.IO = Config.RightSM_UD_CylinderDownIOIn;
            IO_Tear3Suck.IO = Config.RightSM_StgVacuum_IOOutEx;
            IOIn_Tear3Suck.IO = Config.RightSMVacuumIOIn;
            IO_Tear3_FPCSuck.IO = Config.RightSM_StgFPCVacuum_IOOutEx;
            IO_Tear3_Blow.IO = Config.RightSM_StgBlowVacuum_IOOutEx;
            IO_Tear3_Reduce.IO = Config.RightSM_StgReduceVacuum_IOOutEx;
            IO_Tear3_GlueLock.IO = Config.RightSM_GlueLockCylinder_IOOut;
            IO_Tear3_GlueCylinder.IO = Config.RightSM_GlueCylinder_IOOutEx;
            IOIn_Tear3_GlueCylinder_UP.IO = Config.RightSM_GlueUDCylinderUPIOIn;
            IOIn_Tear3_GlueCylinder_Down.IO = Config.RightSM_GlueUD_CylinderDownIOInEx;
            IO_Tear3_RollerCylinder.IO = Config.RightSM_RollerCylinder_IOOut;
            IOIn_Tear3_RollerCylinder_UP.IO = Config.RightSM_RollerUD_CylinderUPIOInEx;
            IOIn_Tear3_RollerCylinder_Down.IO = Config.RightSM_RollerUD_CylinderDownIOInEx;
            IO_Tear3_FBCylinder.IO = Config.RightSM_FBCylinder_IOOutEx;
            IOIn_Tear3_FB_Front.IO = Config.RightSM_FB_CylinderFrontIOIn;
            IOIn_Tear3_FB_Back.IO = Config.RightSM_FB_CylinderBackIOIn;

            IO_Tear3_LRCylinder.IO = Config.RightSM_LRCylinder_IOOutEx;
            IOIn_Tear3_LR_Left.IO = Config.RightSM_LR_CylinerLeftIOIn;
            IOIn_Tear3_LR_Right.IO = Config.RightSM_LR_CylinderRightIOIn;
            IO_Tear3Light.IO = Config.SMStation_OPTIOOutEx;

            IO_Tear1_UDCylinder.IO = Config.LeftSM_UDCylinder_IOOut;
            IOIn_Tear1_UD_UP.IO = Config.LeftSM_UD_CylinderUPIOIn;
            IOIn_Tear1_UD_Down.IO = Config.LeftSM_UD_CylinderDownIOInEx;
            IO_Tear1Suck.IO = Config.LeftSM_StgVacuum_IOOut;
            IOIn_Tear1Suck.IO = Config.LeFTSMVacuumIOIn;
            IO_Tear1_FPCSuck.IO = Config.LeftSM_StgFPCVacuum_IOOut;
            IO_Tear1_Blow.IO = Config.LeftSM_StgReduceVacuum_IOOut;
            IO_Tear1_Reduce.IO = Config.LeftSM_StgBlowVacuum_IOOut;
            IO_Tear1_GlueLock.IO = Config.LeftSM_GlueLockCylinder_IOOut;
            IO_Tear1_GlueCylinder.IO = Config.LeftSM_GlueCylinder_IOOut;
            IOIn_Tear1_GlueCylinder_UP.IO = Config.LeftSM_GlueUD_CylinderUPIOInEx;
            IOIn_Tear1_GlueCylinder_Down.IO = Config.LeftSM_GlueUD_CylinderDownIOInEx;
            IO_Tear1_RollerCylinder.IO = Config.LeftSM_RollerCylinder_IOOut;
            IOIn_Tear1_RollerCylinder_UP.IO = Config.LeftSM_RollerUD_CylinderUpIOInEx;
            IOIn_Tear1_RollerCylinder_Down.IO = Config.LeftSM_RollerUD_CylinderDownIOInEx;
            IO_Tear1_FBCylinder.IO = Config.LeftSM_FBCylinder_IOOut;
            IOIn_Tear1_FB_Front.IO = Config.LeftSM_FB_CylinderFrontIOIn;
            IOIn_Tear1_FB_Back.IO = Config.LeftSM_FB_CylinderBackIOIn;
            IO_Tear1_LRCylinder.IO = Config.LeftSM_LRCylinder_IOOut;
            IOIn_Tear1_LR_Right.IO = Config.LeftSM_LR_CylinderRightIOIn;
            IOIn_Tear1_LR_Left.IO = Config.LeftSM_LR_CylinderLeftIOIn;
            IO_Tear1Light.IO = Config.SMStation_OPTIOOutEx;

            IO_FeedSuck.IO = Config.LoadVacuumIOOut;
            IO_TransferSuck.IO = Config.Transfer_Suckvacuum_IOOut;
            
            IO_FeedFPCSuck.IO = Config.LoadFPCVacuumIOOut;
            IO_FeedBlow.IO = Config.LoadBlowVacuumIOOut;
            IO_TransferFPCSuck.IO = Config.Transfer_FPCSuckvacuum_IOOut;
            IO_TransferBlow.IO = Config.Transfer_Blowvacuum_IOOut;
            //ZGH20220913新增
            FeedCylinderUP.IO = Config.Feed_UDCylinderUP_IOOut;            
            FeedCylinderDown.IO = Config.Feed_UDCylinderDown_IOOut;
            FeedCylinderUPIn.IO = Config.Feed_UpDownCylinder_UpIOInEx;
            FeedCylinderDownIn.IO = Config.Feed_UpDownCylinder_DownIOInEx;



            IO_FeedBelt.IO = Config.SuplyBeltIOOut;
            IO_DischargeBelt.IO = Config.DischargeLineBeltIOOut;
            IO_DischargeNGBelt.IO = Config.NGlineBeltIOOut;
            IO_NGPushCylinder.IO = Config.NGLinePushCylinder;

            IO_DischargeFPCSuck.IO = Config.Discharge_FPCSuckvacuum_IOOut;
            IO_DischargeSuck.IO = Config.Discharge_Suckvacuum_IOOut; 
            IO_DischargeBlow.IO = Config.Discharge_Blowvacuum_IOOut;
            IOIn_DischargeFPCSuckIn.IO = Config.DischargeVacuumIOIn;
            IOIn_DischargeSuck.IO = Config.Discharge_FPCVacuumIOIn;

            IOIn_FeedSuckIn.IO = Config.LoadVacuumIOIn;
            IOIn_FeedFPCSuckIn.IO = Config.LoadfFPCVacuumIOIn;
            IOIn_TransferSuckIn.IO = Config.TransferVacuumIOIn;
            IOIn_TransferFPCSuckIn.IO = Config.TransferFPCVacuumIOIn;

            //IOIn_NGPushCylinder0.IO = Config.NGLineCylinderStatic;
            //IOIn_NGPushCylinder1.IO = Config.NGLineCylinderDynamic;


            ioStatePanel1.IO = Config.TearAOIBlowCylinder;
            ioStatePanel2.IO = Config.TearAOIBlowCylinder;
            ioStatePanel3.IO = Config.TearAOIBlowCylinder;


            #region 上料 && 中转气缸
            IO_Feed_RotateUDCylinderDown_IOOut.IO = Config.Feed_RotateUDCylinderDown_IOOut;
            IOOut_UPCylinder.IO = Config.Feed_RotateUDCylinderUp_IOOut;
            IOIN_UPDownCylinder_UP.IO = Config.Feed_RotateUpDownCylinder_UpIOIn;
            IOIN_UPDownCylinder_DOWN.IO = Config.Feed_RotateUpDownCylider_DownIOIn;
            IO_Feed_UDCylinderUP_IOOut.IO = Config.Feed_UDCylinderUP_IOOut;
            IO_Feed_UDCylinderDown_IOOut.IO = Config.Feed_UDCylinderDown_IOOut;

            IO_Feed_UpDownCylinder_DownIOIn.IO = Config.Feed_UpDownCylinder_DownIOInEx;
            IOIN_Feed_UpDownCylinder_UpIOIn.IO = Config.Feed_UpDownCylinder_UpIOInEx;
            IO_TransferCylinderUP_IOOut.IO = Config.TransferCylinderUp_IOOutEx;

            IO_TransferCylinderDown_IOOut.IO = Config.TransferCylinderDown_IOOutEx;
            IOIN_Transfer_UDCylinderUP_IOInEx.IO = Config.Transfer_UDCylinderUP_IOInEx;
            IOIN_Transfer_UDCylinderDown_IOInEx.IO = Config.Transfer_UDCylinderDown_IOInEx;


            IO_Feed_RotateCylinderORG_IOOutEx.IO = Config.Feed_RotateCylinderORG_IOOutEx;
            IOOUT_RotateClinder.IO = Config.Feed_RotateCylinder_IOOutEx;
            IOIN_RotateCylinder_UP.IO = Config.Feed_RotateCylinder_UpIOIn;
            IOIN_RotateCylinder_Down.IO = Config.Feed_RotateCylinder_DownIOIn;
            IOOUT_Feed_RotateFPCSuck.IO = Config.Feed_RotateFPCSuck_IOOut;
            IOIN_LoadTurnFPCVacuumCheck.IO = Config.Feed_RotateFPCVacuumCheckIOIn;
            IOOut_Feed_RotateBreakVac.IO = Config.Feed_RotateBreakVacuum_IOOut;
            IOOut_Feed_RotateSuck.IO = Config.Feed_RotateSuck_IOOut;
            IOIN_LoadTurnVacuumCheck.IO = Config.Feed_RotateVacuumCheckIOIn;
            #endregion

            #region 下料气缸
            IOOut_Discharge_UPCylinder.IO = Config.Discharge_UPCylinder_CardCOutEx;
            IOIN_Discharge_UPCylinder_UP.IO = Config.Discharge_UPCylinder_UPIOInEx;
            IOIN_Discharge_UPCylinder_Down.IO = Config.Discharge_UPCylinder_DownIOInEx;

            IOOut_Discharge_RotateCylinder.IO = Config.Discharge_RotateCylinder_CardCIOOutEx;
            IOIN_Discharge_RotateCylinder_UP.IO = Config.Discharge_RotateCylinder_UPIOInEx;
            IOIN_Discharge_RotateCylinder_Down.IO = Config.InputAir_IOInEx;

            //IOOut_DischargeRotateSuck.IO = Config.DischargeRotateSuck_OutEx;
            //IOIN_DischargeRotateBlow.IO = Config.InputVacumn_IOInEx;
            IOOut_DischargeRotateBlow.IO = Config.DischargeRotateBlow_OutEx;

            #endregion

        }

        private void LeftSMStationInitData()
        {
            recipe.SMPosition[0].Lsm_WaitZ = Inp_leftsmZSafe.Value;
            recipe.SMPosition[0].Lsm_CCDX = Inpb_leftsmccdx.Value;
            recipe.SMPosition[0].Lsm_CCDY = Inpb_leftsmccdy.Value;
            recipe.SMPosition[0].SM_XSpeed = Inpb_leftsmXspeed.Value;
            recipe.SMPosition[0].SM_YSpeed = Inpb_leftsmYspeed.Value;
            recipe.SMPosition[0].SM_ZSpeed = Inpb_leftsmZspeed.Value;
            recipe.SMPosition[0].SM_ZDist = Inp_leftsmZDist.Value;
            recipe.SMPosition[0].SM_ZDist_Speed = Inp_leftsmZDistSpeed.Value;
            recipe.SMPosition[0].Lsm_PressX = Inp_leftsmpressx.Value;
            recipe.SMPosition[0].Lsm_PressY = Inp_leftsmpressy.Value;
            recipe.SMPosition[0].Lsm_PressZ = Inp_leftsm_pressz.Value;
            recipe.SMPosition[0].Paste_Speed = Input_leftsm_PasteSpeed.Value;

            //ZGH20220912增加撕膜X轴安全位
            recipe.LeftSM_XSafePos = var_leftsmXsafe.Value;
        }

        private void RightSMStationInitData()
        {
            recipe.SMPosition[2].Lsm_WaitZ = Inp_rightsmZsafe.Value;
            recipe.SMPosition[2].Lsm_CCDX = Inpb_rightsmccdx.Value;
            recipe.SMPosition[2].Lsm_CCDY = Inpb_rightsmccdy.Value;
            recipe.SMPosition[2].SM_XSpeed = Inpb_rightsmxspeed.Value;
            recipe.SMPosition[2].SM_YSpeed = Inpb_rightsmyspeed.Value;
            recipe.SMPosition[2].SM_ZSpeed = Inpb_rightsmzspeed.Value;
            recipe.SMPosition[2].SM_ZDist = Inp_rightsmZDist.Value;
            recipe.SMPosition[2].SM_ZDist_Speed = Inp_rightsmZDistSpeed.Value;
            recipe.SMPosition[2].Paste_Speed = Input_rightsm_PasteSpeed.Value;

            //ZGH20220912增加撕膜X轴安全位
            recipe.RightSM_XSafePos = var_rightsmXsafe.Value;
        }

        private void MidSMStationInitData()
        {
            recipe.SMPosition[1].Lsm_WaitZ = Inp_midsmZsafe.Value;
            recipe.SMPosition[1].Lsm_CCDX = Inpb_midsmccdx.Value;
            recipe.SMPosition[1].Lsm_CCDY = Inpb_midsmccdy.Value;
            recipe.SMPosition[1].SM_XSpeed = Inpb_midsmxspeed.Value;
            recipe.SMPosition[1].SM_YSpeed = Inpb_midsmyspeed.Value;
            recipe.SMPosition[1].SM_ZSpeed = Inpb_midsmzspeed.Value;
            recipe.SMPosition[1].SM_ZDist = Inp_midsmZDist.Value;
            recipe.SMPosition[1].SM_ZDist_Speed = Inp_midsmZDistSpeed.Value;
            recipe.SMPosition[1].Paste_Speed = Input_midsm_PasteSpeed.Value;

            //ZGH20220912增加撕膜X轴安全位
            recipe.MidSM_XSafePos = var_midsmXsafe.Value;
        }

        private void LeftBendStationInitData()
        {
            if (recipe != null)
            {

                //对位保护
                recipe.Leftbend_Xlowlimit = Inp_Leftbend_Xlowlimit.Value;
                recipe.Leftbend_Ylowlimit = Inp_Leftbend_Ylowlimit.Value;
                recipe.Leftbend_Wlowlimit = Inp_Leftbend_Wlowlimit.Value;
                recipe.Leftbend_XUpperlimit = Inp_Leftbend_XUpperlimit.Value;
                recipe.Leftbend_YUpperlimit = Inp_Leftbend_YUpperlimit.Value;
                recipe.Leftbend_WUpperlimit = Inp_Leftbend_WUpperlimit.Value;


                recipe.LeftBend_DWX_SafePos = Inp_Leftbend_WaitPt_DWX.Value;
                recipe.LeftBend_DWR_SafePos = Inp_Leftbend_WaitPt_DWR.Value;
                recipe.LeftBend_DWX_WorkPos = Inp_Leftbend_WorkPt_DWX.Value;
                recipe.LeftBend_DWY_WorkPos = Inp_LeftBend_WorkPt_DWY.Value;
                recipe.LeftBend_DWW_WorkPos = Inp_Leftbend_WorkPt_DWW.Value;
                recipe.LeftBend_DWR_WorkPos = Inp_Leftbend_WorkPt_DWR.Value;

                recipe.LeftBend_CCDPos_X = Inp_Leftbend_CCDX.Value;
                recipe.LeftBend_CCDPos_Y = Inp_Leftbend_CCDY.Value;
                recipe.LeftBend_CCDPos_Y2 = Inp_Leftbend_CCDY2.Value;

                //ZGH20220912增加Y轴反折/安全位置保护270-300mm
                if (Inp_LeftBendR_Ypos.Value >= 270 && Inp_LeftBendR_Ypos.Value <= 300) { }
                else { Inp_LeftBendR_Ypos.Value = 270; }

                recipe.LeftBendR_Ypos = Inp_LeftBendR_Ypos.Value;
                recipe.LeftBend_CCD_DWY = Inp_Leftbend_CCD_DWY.Value;

                recipe.LeftBend_PressPt_X = Inp_LeftBend_PressPT_X.Value;
                recipe.LeftBend_PressPt_Y = Inp_LeftBend_PressPt_Y.Value;

                //ZGH20220912增加Y轴反折/安全位置保护270-300mm
                if (Inp_Leftbend_WorkPt_Y.Value >= 270 && Inp_Leftbend_WorkPt_Y.Value <= 300) { }
                else {Inp_Leftbend_WorkPt_Y.Value = 270; }

                recipe.LeftBend_Y_WorkPos = Inp_Leftbend_WorkPt_Y.Value;
                recipe.LeftBend_DWW_BasePos = Inp_LeftBend_DWW_Base.Value;

                recipe.LeftZB_Speed = Inp_leftbZB_speed.Value;
                recipe.LeftYB_Time = Inp_leftYB_time.Value;

                recipe.Bend1adjust_Speed = Inp_Bend1_adjustspeed.Value;

            }
        }

        private void MidBendStationInitData()
        {
            if (recipe != null)
            {
                //对位保护
                recipe.Midbend_Xlowlimit = Inp_Midbend_Xlowlimit.Value;
                recipe.Midbend_Ylowlimit = Inp_Midbend_Ylowlimit.Value;
                recipe.Midbend_Wlowlimit = Inp_Midbend_Wlowlimit.Value;
                recipe.Midbend_XUpperlimit = Inp_Midbend_XUpperlimit.Value;
                recipe.Midbend_YUpperlimit = Inp_Midbend_YUpperlimit.Value;
                recipe.Midbend_WUpperlimit = Inp_Midbend_WUpperlimit.Value;



                recipe.MidBend_DWX_SafePos = Inp_Midbend_WaitPt_DWX.Value;
                recipe.MidBend_DWR_SafePos = Inp_Midbend_WaitPt_DWR.Value;
                recipe.MidBend_DWX_WorkPos = Inp_Midbend_WorkPt_DWX.Value;
                recipe.MidBend_DWY_WorkPos = Inp_Midbend_WorkPt_DWY.Value;
                recipe.MidBend_DWW_WorkPos = Inp_Midbend_WorkPt_DWW.Value;
                recipe.MidBend_DWR_WorkPos = Inp_Midbend_WorkPt_DWR.Value;

                recipe.MidBend_CCDPos_X = Inp_Midbend_CCDX.Value;
                recipe.MidBend_CCDPos_Y = Inp_Midbend_CCDY.Value;
                recipe.MidBend_CCDPos_Y2 = Inp_Midbend_CCDY2.Value;

                //ZGH20220912增加Y轴反折/安全位置保护270-300mm
                if (Inp_MidBendR_Ypos.Value >= 270 && Inp_MidBendR_Ypos.Value <= 300) { }
                else { Inp_MidBendR_Ypos.Value = 270; }

                recipe.MidBendR_Ypos = Inp_MidBendR_Ypos.Value;
                recipe.MidBend_CCD_DWY = Inp_Midbend_CCD_DWY.Value;

                //ZGH20220912增加Y轴反折/安全位置保护270-300mm
                if (Inp_Midbend_WorkPt_Y.Value >= 270 && Inp_Midbend_WorkPt_Y.Value <= 300) { }
                else { Inp_Midbend_WorkPt_Y.Value = 270; }
                
                recipe.MidBend_Y_WorkPos = Inp_Midbend_WorkPt_Y.Value;
                recipe.MidBend_PressPt_X = Inp_MidBend_PressPt_X.Value;
                recipe.MidBend_PressPt_Y = Inp_MidBend_PressPt_Y.Value;

                recipe.MidBend_DWW_BasePos = Inp_MidBend_DWW_Base.Value;
                recipe.MidZB_Speed = Inp_midZB_speed.Value;
                recipe.MidYB_Time = Inp_midYB_time.Value;

                recipe.Bend2adjust_Speed = Inp_Bend2_adjustspeed.Value;
            }

        }

        private void RightBendStationInitData()
        {
            if (recipe != null)
            {
                //对位保护
                recipe.Rightbend_Xlowlimit = Inp_Rightbend_Xlowlimit.Value;
                recipe.Rightbend_Ylowlimit = Inp_Rightbend_Ylowlimit.Value;
                recipe.Rightbend_Wlowlimit = Inp_Rightbend_Wlowlimit.Value;
                recipe.Rightbend_XUpperlimit = Inp_Rightbend_XUpperlimit.Value;
                recipe.Rightbend_YUpperlimit = Inp_Rightbend_YUpperlimit.Value;
                recipe.Rightbend_WUpperlimit = Inp_Rightbend_WUpperlimit.Value;

                recipe.RightBend_DWX_SafePos = Inp_Rightbend_WaitPt_DWX.Value;
                recipe.RightBend_DWR_SafePos = Inp_Rightbend_WaitPt_DWR.Value;
                recipe.RightBend_DWX_WorkPos = Inp_Rightbend_WorkPt_DWX.Value;
                recipe.RightBend_DWY_WorkPos = Inp_Rightbend_WorkPt_DWY.Value;
                recipe.RightBend_DWW_WorkPos = Inp_Rightbend_WorkPt_DWW.Value;
                recipe.RightBend_DWR_WorkPos = Inp_Rightbend_WorkPt_DWR.Value;

                //ZGH20220912增加Y轴反折/安全位置保护270-300mm
                if (Inp_RightBend_WorkPt_Y.Value >= 270 && Inp_RightBend_WorkPt_Y.Value <= 300) { }
                else { Inp_RightBend_WorkPt_Y.Value = 270; }              
                recipe.RightBend_Y_WorkPos = Inp_RightBend_WorkPt_Y.Value;

                recipe.RightBend_CCDPos_X = Inp_Rightbend_CCDX.Value;
                recipe.RightBend_CCDPos_Y = Inp_Rightbend_CCDY.Value;
                recipe.RightBend_CCDPos_Y2 = Inp_Rightbend_CCDY2.Value;

                //ZGH20220912增加Y轴反折/安全位置保护270-300mm
                if (Inp_RightBendR_Ypos.Value >= 270 && Inp_RightBendR_Ypos.Value <= 300) { }
                else {Inp_RightBendR_Ypos.Value = 270; }

                recipe.RightBendR_Ypos = Inp_RightBendR_Ypos.Value;
                recipe.RightBend_CCD_DWY = Inp_Rightbend_CCD_DWY.Value;

                recipe.RightBend_PressPt_X = Inp_RightBend_PressPt_X.Value;
                recipe.RightBend_PressPt_Y = Inp_RightBend_PressPt_Y.Value;

                recipe.RightBend_DWW_BasePos = Inp_RightBend_DWW_Base.Value;

                recipe.RightZB_Speed = Inp_rightZB_speed.Value;
                recipe.RightYB_Time = Inp_rightYB_time.Value;

                recipe.Bend3adjust_Speed = Inp_Bend3_adjustspeed.Value;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.ExStyle |= 0x02000000;
                return parms;
            }
        }

        private void LeftSMInputBox_ValueChanged(object sender, EventArgs e)
        {
            btn_leftsmSave.BackColor = Color.Red;
        }

        private void MidSMInputBox_ValueChanged(object sender, EventArgs e)
        {
            btn_midsmSave.BackColor = Color.Red;
        }

        private void RightSMInputBox_ValueChanged(object sender, EventArgs e)
        {
            btn_rightsmSave.BackColor = Color.Red;
        }

        private void Worker_RecipeChanged(object sender, EventArgs e)
        {
            Init();
            InitInputBox();
            Init_Carry_Value();//20221007
        }
        private void InitAxisDebug()
        {

            axisDebug1.Axises = MeasurementContext.Worker.Axis_LeftSM_X;
            axisDebug2.Axises = MeasurementContext.Worker.Axis_LeftSM_Y;
            axisDebug3.Axises = MeasurementContext.Worker.Axis_LeftSM_Z;
            axisDebug4.Axises = MeasurementContext.Worker.Axis_LeftSM_W;
            axisDebug5.Axises = MeasurementContext.Worker.Axis_SMCCD_X;
            axisDebug6.Axises = MeasurementContext.Worker.Axis_Load_X;
            if (!Config.IsLoadZCylinder)
            {
                axisDebug7.Axises = MeasurementContext.Worker.Axis_Load_Z;
            }
            axisDebug8.Axises = MeasurementContext.Worker.Axis_Transfer_X;
            axisDebug9.Axises = MeasurementContext.Worker.Axis_Transfer_Z;

            axisDebug10.Axises = MeasurementContext.Worker.Axis_MidSM_X;
            axisDebug11.Axises = MeasurementContext.Worker.Axis_MidSM_Y;
            axisDebug12.Axises = MeasurementContext.Worker.Axis_MidSM_Z;
            axisDebug13.Axises = MeasurementContext.Worker.Axis_MidSM_W;
            axisDebug14.Axises = MeasurementContext.Worker.Axis_SMCCD_X;
            axisDebug15.Axises = MeasurementContext.Worker.Axis_Load_X;
            if (!Config.IsLoadZCylinder)
            {
                axisDebug16.Axises = MeasurementContext.Worker.Axis_Load_Z;
            }

            axisDebug17.Axises = MeasurementContext.Worker.Axis_Transfer_X;
            axisDebug18.Axises = MeasurementContext.Worker.Axis_Transfer_Z;

            axisDebug19.Axises = MeasurementContext.Worker.Axis_RightSM_X;
            axisDebug20.Axises = MeasurementContext.Worker.Axis_RightSM_Y;
            axisDebug21.Axises = MeasurementContext.Worker.Axis_RightSM_Z;
            axisDebug22.Axises = MeasurementContext.Worker.Axis_RightSM_W;
            axisDebug23.Axises = MeasurementContext.Worker.Axis_SMCCD_X;
            axisDebug24.Axises = MeasurementContext.Worker.Axis_Load_X;
            if (!Config.IsLoadZCylinder)
            {
                axisDebug25.Axises = MeasurementContext.Worker.Axis_Load_Z;
            }
            axisDebug26.Axises = MeasurementContext.Worker.Axis_Transfer_X;
            axisDebug27.Axises = MeasurementContext.Worker.Axis_Transfer_Z;

            axisDebug31.Axises = _Worker.Axis_LeftBend_stgY;
            axisDebug32.Axises = _Worker.Axis_LeftBend_DWR;
            axisDebug30.Axises = _Worker.Axis_LeftBend_DWX;
            axisDebug29.Axises = _Worker.Axis_LeftBend_DWY;
            axisDebug28.Axises = _Worker.Axis_LeftBend_CCDX;
            axisDebug33.Axises = _Worker.Axis_LeftBend_DWW;
            axisDebug34.Axises = _Worker.Axis_Transfer_X;
            axisDebug35.Axises = _Worker.Axis_Transfer_Z;
            axisDebug36.Axises = _Worker.Axis_Discharge_X;
            axisDebug37.Axises = _Worker.Axis_Discharge_Z;

            axisDebug41.Axises = _Worker.Axis_MidBend_stgY;
            axisDebug42.Axises = _Worker.Axis_MidBend_DWR;
            axisDebug40.Axises = _Worker.Axis_MidBend_DWX;
            axisDebug39.Axises = _Worker.Axis_MidBend_DWY;
            axisDebug38.Axises = _Worker.Axis_MidBend_CCDX;
            axisDebug43.Axises = _Worker.Axis_MidBend_DWW;
            axisDebug44.Axises = _Worker.Axis_Transfer_X;
            axisDebug45.Axises = _Worker.Axis_Transfer_Z;
            axisDebug46.Axises = _Worker.Axis_Discharge_X;
            axisDebug47.Axises = _Worker.Axis_Discharge_Z;

            axisDebug51.Axises = _Worker.Axis_RightBend_stgY;
            axisDebug52.Axises = _Worker.Axis_RightBend_DWR;
            axisDebug50.Axises = _Worker.Axis_RightBend_DWX;
            axisDebug49.Axises = _Worker.Axis_RightBend_DWY;
            axisDebug48.Axises = _Worker.Axis_RightBend_CCDX;
            axisDebug53.Axises = _Worker.Axis_RightBend_DWW;
            axisDebug54.Axises = _Worker.Axis_Transfer_X;
            axisDebug55.Axises = _Worker.Axis_Transfer_Z;
            axisDebug56.Axises = _Worker.Axis_Discharge_X;
            axisDebug57.Axises = _Worker.Axis_Discharge_Z;




            axisDebug66.Axises = _Worker.Axis_LeftSM_Y;
            axisDebug67.Axises = _Worker.Axis_MidSM_Y;
            axisDebug68.Axises = _Worker.Axis_RightSM_Y;
            axisDebug69.Axises = _Worker.Axis_Load_X;
            if (!Config.IsLoadZCylinder)
            {
                axisDebug70.Axises = _Worker.Axis_Load_Z;
            }
            if (Config.IsLoadYAxisEnable)
            { axisDebug71.Axises = _Worker.Axis_Load_Y; }
            

            axisDebug81.Axises = _Worker.Axis_LeftSM_Y;
            axisDebug80.Axises = _Worker.Axis_MidSM_Y;
            axisDebug79.Axises = _Worker.Axis_RightSM_Y;
            axisDebug82.Axises = _Worker.Axis_LeftBend_stgY;
            axisDebug77.Axises = _Worker.Axis_MidBend_stgY;
            axisDebug78.Axises = _Worker.Axis_RightBend_stgY;
            axisDebug83.Axises = _Worker.Axis_Transfer_X;
            axisDebug84.Axises = _Worker.Axis_Transfer_Z;

            axisDebug75.Axises = _Worker.Axis_LeftBend_stgY;
            axisDebug74.Axises = _Worker.Axis_MidBend_stgY;
            axisDebug73.Axises = _Worker.Axis_RightBend_stgY;
            axisDebug76.Axises = _Worker.Axis_Discharge_X;
            axisDebug63.Axises = _Worker.Axis_Discharge_Z;

        }

        private void Init()
        {

            Config = MeasurementContext.Worker.Config;
            recipe = MeasurementContext.Data.CurrentRecipeData;
            if (recipe != null)
            {
                ctrlSM1.Init(recipe, leftsmAxises, 0);
                ctrlSM2.Init(recipe, MidsmAxises, 1);
                ctrlSM3.Init(recipe, RightsmAxises, 2);
            }
        }

        private void btn_leftsmSave_Click(object sender, EventArgs e)
        {
            LeftSMStationInitData();
            ctrlSM1.Save();
            MeasurementContext.Data.Save();
            btn_leftsmSave.BackColor = Color.Transparent;
        }

        private void btn_midsmSave_Click(object sender, EventArgs e)
        {
            MidSMStationInitData();
            ctrlSM2.Save();
            MeasurementContext.Data.Save();
            btn_midsmSave.BackColor = Color.Transparent;
        }


        private void btn_rightsmSave_Click(object sender, EventArgs e)
        {
            RightSMStationInitData();
            ctrlSM3.Save();
            MeasurementContext.Data.Save();
            btn_rightsmSave.BackColor = Color.Transparent;
        }


        #region SM
        private void LocateSMWaitPos(int flag)
        {
            WaitForm.Show(string.Format("定位{0}撕膜工位等待位置...", flag == 0 ? "左" : (flag == 1 ? "中" : "右")), (IAsyncResult argument0) =>
                         {
                             if (!MeasurementContext.Worker.LocateSMWaitPt(flag))
                             {
                                 WaitForm.ShowErrorMessage(string.Format("定位撕膜等待位置失败..."));
                             }
                         }, (IAsyncResult argument1) =>
                         {
                             MeasurementContext.Worker.EndMotion();
                             MeasurementContext.Worker.StopSlowly();
                         });
        }

        private void LocateSMLoadPos(int flag)
        {
            WaitForm.Show(string.Format("定位{0}撕膜工位上料位置...", flag == 0 ? "左" : (flag == 1 ? "中" : "右")), (IAsyncResult argument0) =>
            {
                if (!MeasurementContext.Worker.LocateSMLoadPt(flag))
                {
                    WaitForm.ShowErrorMessage(string.Format("定位撕膜上料位置失败..."));
                }
            }, (IAsyncResult argument1) =>
            {
                MeasurementContext.Worker.EndMotion();
                MeasurementContext.Worker.StopSlowly();
            });
        }


        private void LocateQrCodePos()
        {
            WaitForm.Show("定位到扫码位置", (IAsyncResult argument0) =>
            {
                if (!MeasurementContext.Worker.LocateQrCodePt())
                {
                    WaitForm.ShowErrorMessage(string.Format("定位到扫码位置失败..."));
                }
            }, (IAsyncResult argument1) =>
            {
                MeasurementContext.Worker.EndMotion();
                MeasurementContext.Worker.StopSlowly();
            });
        }

        private void LocateSMCCDPos(int flag)
        {
            WaitForm.Show(string.Format("定位{0}撕膜工位拍照位置...", flag == 0 ? "左" : (flag == 1 ? "中" : "右")), (IAsyncResult argument0) =>
            {
                if (!MeasurementContext.Worker.LocateSMCCDPt(flag))
                {
                    WaitForm.ShowErrorMessage(string.Format("定位撕膜拍照位置失败..."));
                }
            }, (IAsyncResult argument1) =>
            {
                MeasurementContext.Worker.EndMotion();
                MeasurementContext.Worker.StopSlowly();
            });
        }

        private void LocateSMDischargePos(int flag)
        {
            WaitForm.Show(string.Format("定位{0}撕膜工位下料位置...", flag == 0 ? "左" : (flag == 1 ? "中" : "右")), (IAsyncResult argument0) =>
            {
                if (!MeasurementContext.Worker.LocateSMDischargePt(flag))
                {
                    WaitForm.ShowErrorMessage(string.Format("定位撕膜拍照位置失败..."));
                }
            }, (IAsyncResult argument1) =>
            {
                MeasurementContext.Worker.EndMotion();
                MeasurementContext.Worker.StopSlowly();
            });
        }







        private void btn_leftsmccdlearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录左撕膜工位拍照位置?"))
            {

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_SMCCD_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_LeftSM_Y;


                Inpb_leftsmccdx.Value = axisx.PositionDev;
                Inpb_leftsmccdy.Value = axisy.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[0].Lsm_CCDX = Inpb_leftsmccdx.Value;
                    recipe.SMPosition[0].Lsm_CCDY = Inpb_leftsmccdy.Value;
                }
            }
        }






        private void btn_leftsmccdlocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位左撕膜工位拍照位置?"))
            {
                LocateSMCCDPos(0);
            }
        }







        private void btn_midsmccdlearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录中撕膜工位拍照位置?"))
            {
                MeasurementAxis axisx = MeasurementContext.Worker.Axis_SMCCD_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_MidSM_Y;

                Inpb_midsmccdx.Value = axisx.PositionDev;
                Inpb_midsmccdy.Value = axisy.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[1].Lsm_CCDX = Inpb_midsmccdx.Value;
                    recipe.SMPosition[1].Lsm_CCDY = Inpb_midsmccdy.Value;
                }
            }
        }






        private void btn_midsmccdlocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位中撕膜工位拍照位置?"))
            {
                LocateSMCCDPos(1);
            }
        }





        private void btn_rightsmccdlocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位右撕膜工位相机位置?"))
            {
                LocateSMCCDPos(2);
            }
        }







        private void btn_rightsmccdlearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录右撕膜工位拍照位置?"))
            {

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_SMCCD_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_RightSM_Y;


                Inpb_rightsmccdx.Value = axisx.PositionDev;
                Inpb_rightsmccdy.Value = axisy.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[2].Lsm_CCDX = Inpb_rightsmccdx.Value;
                    recipe.SMPosition[2].Lsm_CCDY = Inpb_rightsmccdy.Value;
                }
            }
        }






        #endregion


        #region Bend




        private void button10_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯接料位置?"))
            {
                Inp_Leftbend_WaitPt_DWX.Value = MeasurementContext.Worker.Axis_LeftBend_DWX.PositionDev;
                Inp_LeftBend_WorkPt_DWY.Value = MeasurementContext.Worker.Axis_LeftBend_DWY.PositionDev;
                Inp_Leftbend_WorkPt_DWW.Value = MeasurementContext.Worker.Axis_LeftBend_DWW.PositionDev;

                recipe.LeftBend_DWX_SafePos = Inp_Leftbend_WaitPt_DWX.Value;
                recipe.LeftBend_DWY_WorkPos = Inp_LeftBend_WorkPt_DWY.Value;
                recipe.LeftBend_DWW_WorkPos = Inp_Leftbend_WorkPt_DWW.Value;

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯对位X轴反折位置?"))
            {
                Inp_Leftbend_WorkPt_DWX.Value = MeasurementContext.Worker.Axis_LeftBend_DWX.PositionDev;
                recipe.LeftBend_DWX_WorkPos = Inp_Leftbend_WorkPt_DWX.Value;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯R轴反折位置?"))
            {
                Inp_Leftbend_WorkPt_DWR.Value = MeasurementContext.Worker.Axis_LeftBend_DWR.PositionDev;
                recipe.LeftBend_DWR_WorkPos = Inp_Leftbend_WorkPt_DWR.Value;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯拍照位置?"))
            {
                Inp_Leftbend_CCDX.Value = MeasurementContext.Worker.Axis_LeftBend_CCDX.PositionDev;
                Inp_Leftbend_CCDY.Value = MeasurementContext.Worker.Axis_LeftBend_stgY.PositionDev;

                recipe.LeftBend_CCDPos_X = Inp_Leftbend_CCDX.Value;
                recipe.LeftBend_CCDPos_Y = Inp_Leftbend_CCDY.Value;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯待机位置?"))
            {
                Inp_Leftbend_WaitPt_DWR.Value = MeasurementContext.Worker.Axis_LeftBend_DWR.PositionDev;
                recipe.LeftBend_DWR_SafePos = Inp_Leftbend_WaitPt_DWR.Value;
            }
        }

        private void btn_learn_midbend_wait1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯等待位置1？"))
            {
                Inp_Midbend_WaitPt_DWX.Value = MeasurementContext.Worker.Axis_MidBend_DWX.PositionDev;
                Inp_Midbend_WorkPt_DWY.Value = MeasurementContext.Worker.Axis_MidBend_DWY.PositionDev;
                Inp_Midbend_WorkPt_DWW.Value = MeasurementContext.Worker.Axis_MidBend_DWW.PositionDev;

                recipe.MidBend_DWX_SafePos = Inp_Midbend_WaitPt_DWX.Value;
                recipe.MidBend_DWY_WorkPos = Inp_Midbend_WorkPt_DWY.Value;
                recipe.MidBend_DWW_WorkPos = Inp_Midbend_WorkPt_DWW.Value;
            }
        }

        private void btn_learn_midbend_wait2_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯待机位置?"))
            {
                Inp_Midbend_WaitPt_DWR.Value = MeasurementContext.Worker.Axis_MidBend_DWR.PositionDev;


                recipe.MidBend_DWR_SafePos = Inp_Midbend_WaitPt_DWR.Value;

            }
        }

        private void btn_learn_midbend_work1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯工作位置?"))
            {
                Inp_Midbend_WorkPt_DWX.Value = MeasurementContext.Worker.Axis_MidBend_DWX.PositionDev;
                recipe.MidBend_DWX_WorkPos = Inp_Midbend_WorkPt_DWX.Value;

            }
        }

        private void btn_learn_midbend_work2_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯R轴反折位置?"))
            {
                Inp_Midbend_WorkPt_DWR.Value = MeasurementContext.Worker.Axis_MidBend_DWR.PositionDev;


                recipe.MidBend_DWR_WorkPos = Inp_Midbend_WorkPt_DWR.Value;

            }
        }





        private void btn_learn_midbend_ccdpos_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯拍照位置?"))
            {
                Inp_Midbend_CCDX.Value = MeasurementContext.Worker.Axis_MidBend_CCDX.PositionDev;
                Inp_Midbend_CCDY.Value = MeasurementContext.Worker.Axis_MidBend_stgY.PositionDev;

                recipe.MidBend_CCDPos_X = Inp_Midbend_CCDX.Value;
                recipe.MidBend_CCDPos_Y = Inp_Midbend_CCDY.Value;
            }
        }

        private void btn_learn_rightbend_wait1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右折接料位置？"))
            {
                Inp_Rightbend_WaitPt_DWX.Value = MeasurementContext.Worker.Axis_RightBend_DWX.PositionDev;
                Inp_Rightbend_WorkPt_DWW.Value = MeasurementContext.Worker.Axis_RightBend_DWW.PositionDev;
                Inp_Rightbend_WorkPt_DWY.Value = MeasurementContext.Worker.Axis_RightBend_DWY.PositionDev;

                recipe.RightBend_DWX_SafePos = Inp_Rightbend_WaitPt_DWX.Value;
                recipe.RightBend_DWY_WorkPos = Inp_Rightbend_WorkPt_DWY.Value;
                recipe.RightBend_DWR_WorkPos = Inp_Rightbend_WorkPt_DWW.Value;
            }
        }

        private void btn_learn_rightbend_wait2_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右折弯R轴接料位置?"))
            {
                Inp_Rightbend_WaitPt_DWR.Value = MeasurementContext.Worker.Axis_RightBend_DWR.PositionDev;
                recipe.RightBend_DWR_SafePos = Inp_Rightbend_WaitPt_DWR.Value;

            }
        }

        private void btn_bend_rightbend_work1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右折弯工作位置?"))
            {
                Inp_Rightbend_WorkPt_DWX.Value = MeasurementContext.Worker.Axis_RightBend_DWX.PositionDev;
                recipe.RightBend_DWX_WorkPos = Inp_Rightbend_WorkPt_DWX.Value;
            }
        }

        private void btn_learn_rightbend_work2_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右折弯R轴反折位置?"))
            {
                Inp_Rightbend_WorkPt_DWR.Value = MeasurementContext.Worker.Axis_RightBend_DWR.PositionDev;
                recipe.RightBend_DWR_WorkPos = Inp_Rightbend_WorkPt_DWR.Value;
            }
        }



        private void btn_leftbendSave_Click(object sender, EventArgs e)
        {
            LeftBendStationInitData();
            MeasurementContext.Data.Save();
        }

        private void btn_midbendSave_Click(object sender, EventArgs e)
        {
            MidBendStationInitData();
            MeasurementContext.Data.Save();
        }

        private void btn_rightbendSave_Click(object sender, EventArgs e)
        {
            RightBendStationInitData();
            MeasurementContext.Data.Save();
        }

        private void btn_Locate_leftbend_wait1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯接料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_DWX, _Worker.Axis_LeftBend_DWY, _Worker.Axis_LeftBend_DWW };
                double[] poses = new double[] { recipe.LeftBend_DWX_SafePos, recipe.LeftBend_DWY_WorkPos, recipe.LeftBend_DWW_WorkPos };

                WaitForm.Show(string.Format("定位到左折弯接料位置中..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();


                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯接料位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯接料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_Locate_leftbend_wait2_Click(object sender, EventArgs e)
        {
            if (!_Worker.IsOnPosition(_Worker.Axis_LeftBend_stgY, recipe.LeftBendR_Ypos))
            {
                MessageBox.Show("平台Y轴不在安全位置,操作无效!");
                return;
            }

            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯R轴接料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_DWR };
                double[] poses = new double[] { recipe.LeftBend_DWR_SafePos };

                WaitForm.Show(string.Format("定位到左折弯R轴接料位置中..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseLeftBendClawCylinder();



                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_leftbend_work1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯对位X反折位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_DWX };
                double[] poses = new double[] { recipe.LeftBend_DWX_WorkPos };

                WaitForm.Show(string.Format("定位到左折弯工作位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_leftbend_work2_Click(object sender, EventArgs e)
        {
            if (!_Worker.IsOnPosition(_Worker.Axis_LeftBend_stgY, recipe.LeftBendR_Ypos))
            {
                MessageBox.Show("平台Y轴不在安全位置,操作无效!");
                return;
            }
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯R轴反折位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_DWR };
                double[] poses = new double[] { recipe.LeftBend_DWR_WorkPos };

                WaitForm.Show(string.Format("定位到左折弯R轴反折位置失败"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();




                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }






        private void btn_Locate_leftbend_ccdpos_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯拍照位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_CCDX, _Worker.Axis_LeftBend_stgY };
                double[] poses = new double[] { recipe.LeftBend_CCDPos_X, recipe.LeftBend_CCDPos_Y };

                WaitForm.Show(string.Format("定位到左折弯定位拍照位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseLeftBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.LeftBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯定位拍照位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯定位拍照位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_midbend_wait1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯接料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_DWX, _Worker.Axis_MidBend_DWW, _Worker.Axis_MidBend_DWY };
                double[] poses = new double[] { recipe.MidBend_DWX_SafePos, recipe.MidBend_DWW_WorkPos, recipe.MidBend_DWY_WorkPos };

                WaitForm.Show(string.Format("定位到中折弯接料位置中..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_midbend_wait2_Click(object sender, EventArgs e)
        {
            if (!_Worker.IsOnPosition(_Worker.Axis_MidBend_stgY, recipe.MidBendR_Ypos))
            {
                MessageBox.Show("平台Y轴不在安全位置,操作无效!");
                return;
            }



            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯待机位置2?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_DWR };
                double[] poses = new double[] { recipe.MidBend_DWR_SafePos };

                WaitForm.Show(string.Format("定位到中折弯待机位置1中..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseMidBendClawCylinder();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯待机位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯待机位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_midbend_work1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯工作位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_DWX };
                double[] poses = new double[] { recipe.MidBend_DWX_WorkPos };

                WaitForm.Show(string.Format("定位到中折弯工作位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯工作位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯工作位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_midbend_work2_Click(object sender, EventArgs e)
        {
            if (!_Worker.IsOnPosition(_Worker.Axis_MidBend_stgY, recipe.MidBendR_Ypos))
            {
                MessageBox.Show("平台Y轴不在安全位置,操作无效!");
                return;
            }


            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯R轴反折位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_DWR };
                double[] poses = new double[] { recipe.MidBend_DWR_WorkPos };

                WaitForm.Show(string.Format("定位到中折弯工作位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯工作位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯工作位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }

        }





        private void btn_locate_midbend_CCDPos_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯拍照位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_CCDX, _Worker.Axis_MidBend_stgY };
                double[] poses = new double[] { recipe.MidBend_CCDPos_X, recipe.MidBend_CCDPos_Y };

                WaitForm.Show(string.Format("定位到中折弯定位拍照位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseMidBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.MidBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯定位拍照位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯定位拍照位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_rightbend_wait1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯接料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_DWX, _Worker.Axis_RightBend_DWY, _Worker.Axis_RightBend_DWW };
                double[] poses = new double[] { recipe.RightBend_DWX_SafePos, recipe.RightBend_DWY_WorkPos, recipe.RightBend_DWW_WorkPos };

                WaitForm.Show(string.Format("定位到右折弯接料位置中.."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_rightbend_wait2_Click(object sender, EventArgs e)
        {
            if (!_Worker.IsOnPosition(_Worker.Axis_RightBend_stgY, recipe.RightBendR_Ypos))
            {
                MessageBox.Show("平台Y轴不在安全位置,操作无效!");
                return;
            }


            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯R轴接料位置"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_DWR };
                double[] poses = new double[] { recipe.RightBend_DWR_SafePos };

                WaitForm.Show(string.Format("定位到右折弯R轴接料位置中"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseRightBendClawCylinder();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_rightbend_work2_Click(object sender, EventArgs e)
        {
            if (!_Worker.IsOnPosition(_Worker.Axis_RightBend_stgY, recipe.RightBendR_Ypos))
            {
                MessageBox.Show("平台Y轴不在安全位置,操作无效!");
                return;
            }

            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯R轴反折位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_DWR };
                double[] poses = new double[] { recipe.RightBend_DWR_WorkPos };

                WaitForm.Show(string.Format("定位到右折弯R轴反折位置中"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }





        private void btn_locate_rightbend_ccdpos_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯拍照位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_CCDX, _Worker.Axis_RightBend_stgY };
                double[] poses = new double[] { recipe.RightBend_CCDPos_X, recipe.RightBend_CCDPos_Y };

                WaitForm.Show(string.Format("定位到右折弯定位拍照位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseRightBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.RightBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右折弯定位拍照位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右折弯定位拍照位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }


        #endregion









        private void btn_locate_rightbend_work1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯对位X反折位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_DWX };
                double[] poses = new double[] { recipe.RightBend_DWX_WorkPos };

                WaitForm.Show(string.Format("定位到右折弯对位X反折位置"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_load_waitpos_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到上料工位待机位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_Load_X, _Worker.Axis_Load_Y, };// _Worker.Axis_Load_Z
                double[] poses = new double[] { recipe.LoadXWaitPos, recipe.LoadYWaitPos };//, recipe.LoadZWaitPos

                WaitForm.Show(string.Format("定位到上料工位待机位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到上料工位待机位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到上料工位待机位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_discharge_waitpos_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到下料工位待机位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_Discharge_X, _Worker.Axis_Discharge_Z };
                double[] poses = new double[] { recipe.DischargeXSafePos, recipe.DischargeZSafePos };

                WaitForm.Show(string.Format("定位到下料工位待机位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到下料工位待机位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到下料工位待机位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_transfer_waitpos_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中转工位待机位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_Transfer_X };
                double[] poses = new double[] { recipe.TransferXSafePos };

                WaitForm.Show(string.Format("定位到中转工位待机位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中转工位待机位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中转工位待机位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_load_workpos_Click(object sender, EventArgs e)
        {

        }

        private void btn_locate_smng_A_Click(object sender, EventArgs e)
        {

        }

        private void btn_locate_smng_B_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中转工位NG放料B位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_Transfer_X };
                double[] poses = new double[] { recipe.TransferX_NGB_Pos };

                WaitForm.Show(string.Format("定位到中转工位NG放料B位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中转工位NG放料B位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中转工位NG放料B位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_smng_C_Click(object sender, EventArgs e)
        {

        }

        private void btn_locate_bend_NG_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到下料工位NG放料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_Discharge_X, _Worker.Axis_Discharge_Z };
                double[] poses = new double[] { recipe.DischargeX_NG_PullPos, recipe.DischargeZ_NG_PullPos };

                WaitForm.Show(string.Format("定位到下料工位NG放料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到下料工位NG放料失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到下料工位NG放料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_bend_OK_Click(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否运行上料取料动作?"))
            {
                WaitForm.Show(string.Format("上料取料中."), (IAsyncResult argument0) =>
                {

                    if (!_Worker.RunloadStation_TakeMatrial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料取料动作失败..."));
                    }
                    else if (!_Worker.RunloadStation_PullMatrial(0))
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料取料动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }




        private void btn_saveloadDischargePos_Click(object sender, EventArgs e)
        {
            if (recipe != null)
            {
                // FeedStationPTInit();
                CarryAxiseStation_Value();
                _Worker.Data.Save();
            }
        }

        private void btn_leftsm_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行左撕膜动作?"))
            {
                WaitForm.Show(string.Format("左工位撕膜中."), (IAsyncResult argument0) =>
                {
                    _Worker.IsStop = false;
                    if (!_Worker.RunSMStation_SMMotion(0))
                    {
                        WaitForm.ShowErrorMessage(string.Format("左工位撕膜失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.IsStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                    Thread.Sleep(300);
                    _Worker.IsStop = false;

                });
            }
        }

        private void btn_leftsmAOI_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行左撕膜工位AOI?"))
            {
                WaitForm.Show(string.Format("左撕膜工位AOI中."), (IAsyncResult argument0) =>
                {
                    _Worker.IsStop = false;
                    if (!_Worker.RunSMStation_SMAOIMotion(0))
                    {
                        WaitForm.ShowErrorMessage(string.Format("左撕膜工位AOI失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.IsStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                    Thread.Sleep(300);
                    _Worker.IsStop = false;
                });
            }
        }

        private void btn_leftsmdischarge_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行左撕膜工位下料?"))
            {
                WaitForm.Show(string.Format("左撕膜工位下料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStation_GetOutoffStg(0))
                    {
                        WaitForm.ShowErrorMessage(string.Format("左撕膜工位下料失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midsmload_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否运行上料取料动作?"))
            {
                WaitForm.Show(string.Format("上料取料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunloadStation_TakeMatrial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("取料失败..."));
                    }
                    else if (!_Worker.RunloadStation_PullMatrial(1))
                    {
                        WaitForm.ShowErrorMessage(string.Format("中撕膜上料失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midsm_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行中撕膜动作?"))
            {
                WaitForm.Show(string.Format("中撕膜工位撕膜中."), (IAsyncResult argument0) =>
                {
                    _Worker.IsStop = false;
                    if (!_Worker.RunSMStation_SMMotion(1))
                    {
                        WaitForm.ShowErrorMessage(string.Format("中工位撕膜失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.IsStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midsmaoi_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行中撕膜工位AOI?"))
            {
                WaitForm.Show(string.Format("左撕膜工位AOI中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStation_SMAOIMotion(1))
                    {
                        WaitForm.ShowErrorMessage(string.Format("中撕膜工位AOI失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midsmdischarge_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行中撕膜工位下料?"))
            {
                WaitForm.Show(string.Format("中撕膜工位下料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStation_GetOutoffStg(1))
                    {
                        WaitForm.ShowErrorMessage(string.Format("中撕膜工位下料失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_rightsmload_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否运行上料取料动作?"))
            {
                WaitForm.Show(string.Format("上料取料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunloadStation_TakeMatrial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("取料失败..."));
                    }
                    else if (!_Worker.RunloadStation_PullMatrial(2))
                    {
                        WaitForm.ShowErrorMessage(string.Format("右撕膜上料失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_rightsm_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行右撕膜动作?"))
            {
                WaitForm.Show(string.Format("右撕膜工位撕膜中."), (IAsyncResult argument0) =>
                {
                    _Worker.IsStop = false;
                    if (!_Worker.RunSMStation_SMMotion(2))
                    {
                        WaitForm.ShowErrorMessage(string.Format("右工位撕膜失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.IsStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_rightsmaoi_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行右撕膜工位AOI?"))
            {
                WaitForm.Show(string.Format("右撕膜工位AOI中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStation_SMAOIMotion(2))
                    {
                        WaitForm.ShowErrorMessage(string.Format("右撕膜工位AOI失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_rightsmdischarge_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行右撕膜工位下料?"))
            {
                WaitForm.Show(string.Format("右撕膜工位下料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStation_GetOutoffStg(2))
                    {
                        WaitForm.ShowErrorMessage(string.Format("右撕膜工位下料失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯压合位置?"))
            {
                Inp_LeftBend_PressPT_X.Value = MeasurementContext.Worker.Axis_LeftBend_CCDX.PositionDev;
                Inp_LeftBend_PressPt_Y.Value = MeasurementContext.Worker.Axis_LeftBend_stgY.PositionDev;
                recipe.LeftBend_PressPt_X = Inp_LeftBend_PressPT_X.Value;
                recipe.LeftBend_PressPt_Y = Inp_LeftBend_PressPt_Y.Value;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯压合位置?"))
            {
                Inp_MidBend_PressPt_X.Value = MeasurementContext.Worker.Axis_MidBend_CCDX.PositionDev;
                Inp_MidBend_PressPt_Y.Value = MeasurementContext.Worker.Axis_MidBend_stgY.PositionDev;

                recipe.MidBend_PressPt_X = Inp_MidBend_PressPt_X.Value;
                recipe.MidBend_PressPt_Y = Inp_MidBend_PressPt_Y.Value;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右折弯压合位置?"))
            {
                Inp_RightBend_PressPt_X.Value = MeasurementContext.Worker.Axis_RightBend_CCDX.PositionDev;
                Inp_RightBend_PressPt_Y.Value = MeasurementContext.Worker.Axis_RightBend_stgY.PositionDev;

                recipe.RightBend_PressPt_X = Inp_RightBend_PressPt_X.Value;
                recipe.RightBend_PressPt_Y = Inp_RightBend_PressPt_Y.Value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯压合位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_CCDX, _Worker.Axis_LeftBend_stgY };
                double[] poses = new double[] { recipe.LeftBend_PressPt_X, recipe.LeftBend_PressPt_Y };

                WaitForm.Show(string.Format("定位到左折弯压合位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseLeftBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.LeftBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯压合位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯压合位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯压合位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_CCDX, _Worker.Axis_MidBend_stgY };
                double[] poses = new double[] { recipe.MidBend_PressPt_X, recipe.MidBend_PressPt_Y };

                WaitForm.Show(string.Format("定位到左折弯压合位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseMidBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.MidBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯压合位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯压合位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯压合位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_CCDX, _Worker.Axis_RightBend_stgY };
                double[] poses = new double[] { recipe.RightBend_PressPt_X, recipe.RightBend_PressPt_Y };

                WaitForm.Show(string.Format("定位到右折弯压合位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseRightBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.RightBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右折弯压合位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右折弯压合位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯R轴回安全位Y轴位置?"))
            {
                Inp_LeftBendR_Ypos.Value = MeasurementContext.Worker.Axis_LeftBend_stgY.PositionDev;
                recipe.LeftBendR_Ypos = Inp_LeftBendR_Ypos.Value;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯R轴回安全位Y轴位置?"))
            {
                Inp_MidBendR_Ypos.Value = MeasurementContext.Worker.Axis_MidBend_stgY.PositionDev;
                recipe.MidBendR_Ypos = Inp_MidBendR_Ypos.Value;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右折弯R轴回安全位Y轴位置?"))
            {
                Inp_RightBendR_Ypos.Value = MeasurementContext.Worker.Axis_RightBend_stgY.PositionDev;
                recipe.RightBendR_Ypos = Inp_RightBendR_Ypos.Value;
            }
        }

        private void btn_leftbendload_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否左折弯上料动作?"))
            {
                WaitForm.Show(string.Format("左折弯上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.LeftBendStation_PullMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_leftbend_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否左折弯工位折弯动作?"))
            {
                WaitForm.Show(string.Format("左工位折弯中."), (IAsyncResult argument0) =>
                {
                    _Worker.IsStop = false;
                    if (!_Worker.LeftBendStation_BendWork())
                    {
                        WaitForm.ShowErrorMessage(string.Format("折弯动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.IsStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_leftbenddischarge_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否左折弯下料动作?"))
            {
                WaitForm.Show(string.Format("左折弯下料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.LeftBendStation_PickMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("下料动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯R轴翻转安全动作Y轴位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_stgY };
                double[] poses = new double[] { recipe.LeftBendR_Ypos };
                WaitForm.Show(string.Format("定位到左折弯翻转安全动作Y轴位置.."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseLeftBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.LeftBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯R轴翻转安全动作Y轴位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_stgY };
                double[] poses = new double[] { recipe.MidBendR_Ypos };
                WaitForm.Show(string.Format("定位到中折弯翻转安全动作Y轴位置.."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseMidBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.MidBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯R轴翻转安全动作Y轴位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_stgY };
                double[] poses = new double[] { recipe.RightBendR_Ypos };
                WaitForm.Show(string.Format("定位到右折弯翻转安全动作Y轴位置.."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseRightBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.RightBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midbendload_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否中折弯工位上料动作?"))
            {
                WaitForm.Show(string.Format("中折弯上料中."), (IAsyncResult argument0) =>
                {

                    if (!_Worker.MidBendStation_PullMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料动作失败"));
                    }

                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midbend_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否中折弯工位折弯动作?"))
            {
                WaitForm.Show(string.Format("中工位折弯中."), (IAsyncResult argument0) =>
                {
                    _Worker.IsStop = false;

                    if (!_Worker.MidBendStation_BendWork())
                    {
                        WaitForm.ShowErrorMessage(string.Format("折弯动作失败"));
                    }

                }, (IAsyncResult argument1) =>
                {
                    _Worker.IsStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midbenddischarge_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否中折弯下料动作?"))
            {
                WaitForm.Show(string.Format("中折弯下料中."), (IAsyncResult argument0) =>
                {

                    if (!_Worker.MidBendStation_PickMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("下料动作失败"));
                    }

                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_rightbendload_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否右折弯工位上料动作?"))
            {
                WaitForm.Show(string.Format("右折弯上料中."), (IAsyncResult argument0) =>
                {

                    if (!_Worker.RightBendStation_PullMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料动作失败"));
                    }

                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_rightbend_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否右折弯工位折弯动作?"))
            {
                WaitForm.Show(string.Format("右工位折弯中."), (IAsyncResult argument0) =>
                {
                    _Worker.IsStop = false;
                    if (!_Worker.RightBendStation_BendWork())
                    {
                        WaitForm.ShowErrorMessage(string.Format("折弯动作失败"));
                    }

                }, (IAsyncResult argument1) =>
                {
                    _Worker.IsStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_rightbenddischarge_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否右折弯下料动作?"))
            {
                WaitForm.Show(string.Format("右折弯下料中."), (IAsyncResult argument0) =>
                {

                    if (!_Worker.RightBendStation_PickMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("下料动作失败"));
                    }

                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_learn_rightbend_ccdpos_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯拍照位置?"))
            {
                Inp_Rightbend_CCDX.Value = MeasurementContext.Worker.Axis_RightBend_CCDX.PositionDev;
                Inp_Rightbend_CCDY.Value = MeasurementContext.Worker.Axis_RightBend_stgY.PositionDev;

                recipe.RightBend_CCDPos_X = Inp_Rightbend_CCDX.Value;
                recipe.RightBend_CCDPos_Y = Inp_Rightbend_CCDY.Value;
            }
        }

        private void btn_leftsmpresslearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录左撕膜工位滚轮气缸压合位置?"))
            {

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_LeftSM_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_LeftSM_Y;
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_LeftSM_Z;

                Inp_leftsmpressx.Value = axisx.PositionDev;
                Inp_leftsmpressy.Value = axisy.PositionDev;
                Inp_leftsm_pressz.Value = axisz.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[0].Lsm_PressX = Inp_leftsmpressx.Value;
                    recipe.SMPosition[0].Lsm_PressY = Inp_leftsmpressy.Value;
                    recipe.SMPosition[0].Lsm_PressZ = Inp_leftsm_pressz.Value;
                }
            }
        }


        private void btn_leftsmpresslocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左撕膜滚轮气缸压合位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftSM_X, _Worker.Axis_LeftSM_Y, _Worker.Axis_LeftSM_Z };
                double[] poses = new double[] { recipe.SMPosition[0].Lsm_PressX, recipe.SMPosition[0].Lsm_PressY, recipe.SMPosition[0].Lsm_PressZ };

                WaitForm.Show(string.Format("定位到左撕膜滚轮气缸压合位置中..."), (IAsyncResult argument0) =>
                {

                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左撕膜滚轮压合位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左撕膜滚轮气缸压合位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midsmpresslearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录中撕膜工位滚轮气缸压合位置?"))
            {

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_MidSM_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_MidSM_Y;
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_MidSM_Z;

                Inp_midsmpressx.Value = axisx.PositionDev;
                Inp_midsmpressy.Value = axisy.PositionDev;
                Inp_midsm_pressz.Value = axisz.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[1].Lsm_PressX = Inp_midsmpressx.Value;
                    recipe.SMPosition[1].Lsm_PressY = Inp_midsmpressy.Value;
                    recipe.SMPosition[1].Lsm_PressZ = Inp_midsm_pressz.Value;
                }
            }
        }

        private void btn_midsmpresslocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中撕膜滚轮气缸压合位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidSM_X, _Worker.Axis_MidSM_Y, _Worker.Axis_MidSM_Z };
                double[] poses = new double[] { recipe.SMPosition[1].Lsm_PressX, recipe.SMPosition[1].Lsm_PressY, recipe.SMPosition[1].Lsm_PressZ };

                WaitForm.Show(string.Format("定位到中撕膜滚轮气缸压合位置中..."), (IAsyncResult argument0) =>
                {

                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中撕膜滚轮压合位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中撕膜滚轮气缸压合位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_rightsmpresslearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录右撕膜工位滚轮气缸压合位置?"))
            {

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_RightSM_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_RightSM_Y;
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_RightSM_Z;

                Inp_rightsmpressx.Value = axisx.PositionDev;
                Inp_rightsmpressy.Value = axisy.PositionDev;
                Inp_rightsm_pressz.Value = axisz.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[2].Lsm_PressX = Inp_rightsmpressx.Value;
                    recipe.SMPosition[2].Lsm_PressY = Inp_rightsmpressy.Value;
                    recipe.SMPosition[2].Lsm_PressZ = Inp_rightsm_pressz.Value;
                }
            }
        }

        private void btn_rightsmpresslocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右撕膜滚轮气缸压合位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightSM_X, _Worker.Axis_RightSM_Y, _Worker.Axis_RightSM_Z };
                double[] poses = new double[] { recipe.SMPosition[2].Lsm_PressX, recipe.SMPosition[2].Lsm_PressY, recipe.SMPosition[2].Lsm_PressZ };

                WaitForm.Show(string.Format("定位到右撕膜滚轮气缸压合位置中..."), (IAsyncResult argument0) =>
                {

                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右撕膜滚轮压合位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右撕膜滚轮气缸压合位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_learn_leftbend_ccddwy_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯对位Y轴拍照位置?"))
            {

                Inp_Leftbend_CCD_DWY.Value = MeasurementContext.Worker.Axis_LeftBend_DWY.PositionDev;

                recipe.LeftBend_CCD_DWY = Inp_Leftbend_CCD_DWY.Value;

            }
        }

        private void btn_learn_midbend_ccddwy_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯对位Y轴拍照位置?"))
            {
                Inp_Midbend_CCD_DWY.Value = MeasurementContext.Worker.Axis_MidBend_DWY.PositionDev;
                recipe.MidBend_CCD_DWY = Inp_Midbend_CCD_DWY.Value;
            }
        }

        private void btn_learn_rightbend_ccddwy_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右折弯对位Y轴拍照位置?"))
            {

                Inp_Rightbend_CCD_DWY.Value = MeasurementContext.Worker.Axis_RightBend_DWY.PositionDev;
                recipe.RightBend_CCD_DWY = Inp_Rightbend_CCD_DWY.Value;

            }
        }

        private void btn_locate_leftbend_ccddwy_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯对位Y轴拍照位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_DWY };
                double[] poses = new double[] { recipe.LeftBend_CCD_DWY };

                WaitForm.Show(string.Format("定位到左折弯对位Y轴拍照位置.."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_midbend_ccddwy_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯对位Y轴拍照位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_DWY };
                double[] poses = new double[] { recipe.MidBend_CCD_DWY };

                WaitForm.Show(string.Format("定位到中折弯对位Y轴拍照位置.."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_rightbend_ccddwy_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯对位Y轴拍照位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_DWY };
                double[] poses = new double[] { recipe.RightBend_CCD_DWY };

                WaitForm.Show(string.Format("定位到右折弯对位Y轴拍照位置.."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {

            _Worker.str_Bend1Rev = "";
            if (!_Worker.SendMsg("A,PZF"))
            {
                MeasurementContext.OutputError("折弯相机1发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend1Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend1Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button19_Click_1(object sender, EventArgs e)
        {

            _Worker.str_Bend1Rev = "";
            if (!_Worker.SendMsg("A,PZS"))
            {
                MeasurementContext.OutputError("折弯相机1发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend1Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend1Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否左折弯角度校正?"))
            {
                WaitForm.Show(string.Format("左折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.LeftMoveDegree())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否左折弯反折校正?"))
            {
                WaitForm.Show(string.Format("左折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.LeftMoveY())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            _Worker.str_Bend2Rev = "";
            if (!_Worker.Bend2SendMsg("B,PZF"))
            {
                MeasurementContext.OutputError("折弯相机3发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend2Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend2Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {



            _Worker.str_Bend2Rev = "";
            if (!_Worker.Bend2SendMsg("B,PZS"))
            {
                MeasurementContext.OutputError("折弯相机3发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend2Rev.Length < 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend2Rev.Length > 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {

            _Worker.str_Bend3Rev = "";
            if (!_Worker.Bend3SendMsg("C,PZF"))
            {
                MeasurementContext.OutputError("折弯相机3发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend3Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend3Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }


            }

        }

        private void button31_Click(object sender, EventArgs e)
        {
            _Worker.str_Bend3Rev = "";
            if (!_Worker.Bend3SendMsg("C,PZS"))
            {
                MeasurementContext.OutputError("折弯相机3发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend3Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend3Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }


            }
        }

        private void button25_Click(object sender, EventArgs e)
        {

            if (MessageBoxEx.ShowSystemQuestion("是否中折弯角度校正?"))
            {
                WaitForm.Show(string.Format("中折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.MidMoveDegree())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }

        }

        private void button24_Click(object sender, EventArgs e)
        {

            if (MessageBoxEx.ShowSystemQuestion("是否中折弯反折校正?"))
            {
                WaitForm.Show(string.Format("中折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.MidMoveY())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {

            if (MessageBoxEx.ShowSystemQuestion("是否右折弯角度校正?"))
            {
                WaitForm.Show(string.Format("右折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RightMoveDegree())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否右折弯反折校正?"))
            {
                WaitForm.Show(string.Format("右折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RightMoveY())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        #region 
        private void btnEx_Feed_wait_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教上料待机位置?"))
            {
                var_Feed_wait_x.Value = MeasurementContext.Worker.Axis_Load_X.PositionDev;
                // var_Feed_wait_y.Value = MeasurementContext.Worker.Axis_Load_Y.PositionDev;
                if (!Config.IsLoadZCylinder)
                {
                    var_Feed_wait_z.Value = MeasurementContext.Worker.Axis_Load_Z.PositionDev;
                    recipe.LoadZWaitPos = var_Feed_wait_z.Value;
                }
                recipe.LoadXWaitPos = var_Feed_wait_x.Value;
            }
        }

        private void btnEx_Feed_pick_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教上料取料位置?"))
            {
                var_Feed_pick_x.Value = MeasurementContext.Worker.Axis_Load_X.PositionDev;
                if (!Config.IsLoadZCylinder)
                {
                    var_Feed_pick_z.Value = MeasurementContext.Worker.Axis_Load_Z.PositionDev;
                    recipe.LoadZpos = var_Feed_pick_z.Value;
                }
                recipe.LoadXpos = var_Feed_pick_x.Value;

            }
        }

        private void btnEx_Feed_pull_x1_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录左撕膜工位上料位置?"))
            {
                if (recipe != null)
                {
                    MeasurementAxis axisx = MeasurementContext.Worker.Axis_Load_X;
                    var_Feed_pull_x1.Value = axisx.PositionDev;
                    recipe.SMPosition[0].Lsm_loadX = var_Feed_pull_x1.Value;

                    MeasurementAxis axisy = MeasurementContext.Worker.Axis_LeftSM_Y;
                    var_Feed_pull_y1.Value = axisy.PositionDev;
                    recipe.SMPosition[0].Lsm_loadY = var_Feed_pull_y1.Value;

                    if (!Config.IsLoadZCylinder)
                    {
                        MeasurementAxis axisz = MeasurementContext.Worker.Axis_Load_Z;
                        var_Feed_pull_z1.Value = axisz.PositionDev;
                        recipe.SMPosition[0].Lsm_LoadZ = var_Feed_pull_z1.Value;
                    }
                }
            }
        }

        private void btnEx_Feed_pull_x2_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录中撕膜工位上料位置?"))
            {

                if (recipe != null)
                {


                    MeasurementAxis axisx = MeasurementContext.Worker.Axis_Load_X;
                    var_Feed_pull_x2.Value = axisx.PositionDev;
                    recipe.SMPosition[1].Lsm_loadX = var_Feed_pull_x2.Value;

                    MeasurementAxis axisy = MeasurementContext.Worker.Axis_MidSM_Y;
                    var_Feed_pull_y2.Value = axisy.PositionDev;
                    recipe.SMPosition[1].Lsm_loadY = var_Feed_pull_y2.Value;

                    if (!Config.IsLoadZCylinder)
                    {
                        MeasurementAxis axisz = MeasurementContext.Worker.Axis_Load_Z;
                        var_Feed_pull_z2.Value = axisz.PositionDev;
                        recipe.SMPosition[1].Lsm_LoadZ = var_Feed_pull_z2.Value;
                    }
                }
            }
        }

        private void btnEx_Feed_pull_x3_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录右撕膜工位上料位置?"))
            {
                if (recipe != null)
                {
                    MeasurementAxis axisx = MeasurementContext.Worker.Axis_Load_X;
                    var_Feed_pull_x3.Value = axisx.PositionDev;
                    recipe.SMPosition[2].Lsm_loadX = var_Feed_pull_x3.Value;

                    MeasurementAxis axisy = MeasurementContext.Worker.Axis_RightSM_Y;
                    var_Feed_pull_y3.Value = axisy.PositionDev;
                    recipe.SMPosition[2].Lsm_loadY = var_Feed_pull_y3.Value;

                    if (!Config.IsLoadZCylinder)
                    {
                        MeasurementAxis axisz = MeasurementContext.Worker.Axis_Load_Z;
                        var_Feed_pull_z3.Value = axisz.PositionDev;
                        recipe.SMPosition[2].Lsm_LoadZ = var_Feed_pull_z3.Value;
                    }
                }
            }
        }

        private void btnEx_Feed_belty_wait_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教皮带Y轴待机位置?"))
            {

                var_Feed_wait_y.Value = MeasurementContext.Worker.Axis_Load_Y.PositionDev;
                recipe.LoadYWaitPos = var_Feed_wait_y.Value;
            }
        }

        private void btnEx_Feed_belty_work_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教皮带Y轴推料位置?"))
            {
                var_Feed_wait_y.Value = MeasurementContext.Worker.Axis_Load_Y.PositionDev;
                recipe.LoadYpos = var_feed_work_y.Value;
            }
        }

        private void btnEx_Feed_wait_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到上料工位待机位置?"))
            {

                WaitForm.Show(string.Format("定位到上料工位待机位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.BeginMotion();

                    if (!_Worker.FeedZWork(0, recipe.LoadZWaitPos, true) || !_Worker.AxisFeedXGoPos(recipe.LoadXWaitPos))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到上料工位取料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Feed_pick_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到上料工位取料位置?"))
            {
                WaitForm.Show(string.Format("定位到上料工位取料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.FeedZWork(0, recipe.LoadZWaitPos, true) || !_Worker.AxisFeedXGoPos(recipe.LoadXpos))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到上料工位取料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Feed_pull_x1_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位左撕膜工位上料位置?"))
            {
                if (!_Worker.FeedZWork(0, recipe.LoadZWaitPos, true))
                {
                    return;
                }
                LocateSMLoadPos(0);
            }
        }

        private void btnEx_Feed_pull_x2_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位中撕膜工位上料位置?"))
            {
                if (!_Worker.FeedZWork(0, recipe.LoadZWaitPos, true))
                {
                    return;
                }
                LocateSMLoadPos(1);
            }
        }

        private void btnEx_Feed_pull_x3_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位右撕膜工位上料位置?"))
            {
                if (!_Worker.FeedZWork(0, recipe.LoadZWaitPos, true))
                {
                    return;
                }
                LocateSMLoadPos(2);
            }
        }

        private void btnEx_Feed_belty_wait_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到皮带Y轴待机位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_Load_Y, };
                double[] poses = new double[] { recipe.LoadYWaitPos };

                WaitForm.Show(string.Format("定位到皮带Y轴待机位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到皮带Y轴待机位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到皮带Y轴待机位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Feed_belty_work_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到皮带Y轴推料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_Load_Y, };
                double[] poses = new double[] { recipe.LoadYpos };
                WaitForm.Show(string.Format("定位到皮带Y轴推料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到皮带Y轴推料位置失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到皮带Y轴推料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Transfer_wait_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中转待机位置?"))
            {
                var_transfer_wait_x.Value = _Worker.Axis_Transfer_X.PositionDev;
                recipe.TransferXSafePos = var_transfer_wait_x.Value;

                var_transfer_wait_z.Value = _Worker.Axis_Transfer_Z.PositionDev;
                recipe.TransferZSafePos = var_transfer_wait_z.Value;
            }
        }

        private void btnEx_Transfer_pick1_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录左撕膜工位下料位置?"))
            {

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_Transfer_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_LeftSM_Y;
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_Transfer_Z;

                var_transfer_pick_x1.Value = axisx.PositionDev;
                var_transfer_pick_y1.Value = axisy.PositionDev;
                var_transfer_pick_z1.Value = axisz.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[0].Lsm_DischargeX = var_transfer_pick_x1.Value;
                    recipe.SMPosition[0].Lsm_DischargeY = var_transfer_pick_y1.Value;
                    recipe.SMPosition[0].Lsm_DischargeZ = var_transfer_pick_z1.Value;
                }
            }
        }

        private void btnEx_Transfer_pick2_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录中撕膜工位下料位置?"))
            {

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_Transfer_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_MidSM_Y;
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_Transfer_Z;

                var_transfer_pick_x2.Value = axisx.PositionDev;
                var_transfer_pick_y2.Value = axisy.PositionDev;
                var_transfer_pick_z2.Value = axisz.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[1].Lsm_DischargeX = var_transfer_pick_x2.Value;
                    recipe.SMPosition[1].Lsm_DischargeY = var_transfer_pick_y2.Value;
                    recipe.SMPosition[1].Lsm_DischargeZ = var_transfer_pick_z2.Value;
                }
            }
        }

        private void btnEx_Transfer_pick3_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录右撕膜工位下料位置?"))
            {

                MeasurementAxis axisx = MeasurementContext.Worker.Axis_Transfer_X;
                MeasurementAxis axisy = MeasurementContext.Worker.Axis_RightSM_Y;
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_Transfer_Z;

                var_transfer_pick_x3.Value = axisx.PositionDev;
                var_transfer_pick_y3.Value = axisy.PositionDev;
                var_transfer_pick_z3.Value = axisz.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[2].Lsm_DischargeX = var_transfer_pick_x3.Value;
                    recipe.SMPosition[2].Lsm_DischargeY = var_transfer_pick_y2.Value;
                    recipe.SMPosition[2].Lsm_DischargeZ = var_transfer_pick_z3.Value;
                }
            }
        }

        private void btnEx_Transfer_NGA_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中转NG下料A位置?"))
            {
                var_transfer_pull_NGA_x.Value = _Worker.Axis_Transfer_X.PositionDev;
                var_transfer_pull_NGA_z.Value = _Worker.Axis_Transfer_Z.PositionDev;

                recipe.TransferX_NGA_Pos = var_transfer_pull_NGA_x.Value;
                recipe.TransferZ_NGA_Pos = var_transfer_pull_NGA_z.Value;
            }
        }

        private void btnEx_Transfer_NGB_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中转NG下料B位置?"))
            {
                var_transfer_pull_NGB_x.Value = _Worker.Axis_Transfer_X.PositionDev;
                var_transfer_pull_NGB_z.Value = _Worker.Axis_Transfer_Z.PositionDev;

                recipe.TransferX_NGB_Pos = var_transfer_pull_NGB_x.Value;
                recipe.TransferZ_NGB_Pos = var_transfer_pull_NGB_z.Value;
            }
        }

        private void btnEx_Transfer_NGC_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中转NG下料C位置?"))
            {
                var_transfer_pull_NGC_x.Value = _Worker.Axis_Transfer_X.PositionDev;
                var_transfer_pull_NGC_z.Value = _Worker.Axis_Transfer_Z.PositionDev;

                recipe.TransferX_NGC_Pos = var_transfer_pull_NGC_x.Value;
                recipe.TransferZ_NGC_Pos = var_transfer_pull_NGC_z.Value;
            }
        }

        private void btnEx_Transfer_pull1_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯上料放料位置?"))
            {
                var_transfer_pull_x1.Value = MeasurementContext.Worker.Axis_Transfer_X.PositionDev;
                var_transfer_pull_y1.Value = MeasurementContext.Worker.Axis_LeftBend_stgY.PositionDev;
                var_transfer_pull_z1.Value = MeasurementContext.Worker.Axis_Transfer_Z.PositionDev;

                recipe.LeftBend_LoadX = var_transfer_pull_x1.Value;
                recipe.LeftBend_LoadY = var_transfer_pull_y1.Value;
                recipe.LeftBend_LoadZ = var_transfer_pull_z1.Value;
            }
        }

        private void btnEx_Transfer_pull2_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯上料放料位置?"))
            {
                var_transfer_pull_x2.Value = MeasurementContext.Worker.Axis_Transfer_X.PositionDev;
                var_transfer_pull_y2.Value = MeasurementContext.Worker.Axis_MidBend_stgY.PositionDev;
                var_transfer_pull_z2.Value = MeasurementContext.Worker.Axis_Transfer_Z.PositionDev;

                recipe.MidBend_LoadX = var_transfer_pull_x2.Value;
                recipe.MidBend_LoadY = var_transfer_pull_y2.Value;
                recipe.MidBend_LoadZ = var_transfer_pull_z2.Value;
            }
        }

        private void btnEx_Transfer_pull3_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯上料放料位置?"))
            {
                var_transfer_pull_x3.Value = MeasurementContext.Worker.Axis_Transfer_X.PositionDev;
                var_transfer_pull_y3.Value = MeasurementContext.Worker.Axis_RightBend_stgY.PositionDev;
                var_transfer_pull_z3.Value = MeasurementContext.Worker.Axis_Transfer_Z.PositionDev;

                recipe.RightBend_LoadX = var_transfer_pull_x3.Value;
                recipe.RightBend_LoadY = var_transfer_pull_y3.Value;
                recipe.RightBend_LoadZ = var_transfer_pull_z3.Value;
            }
        }

        private void btnEx_Transfer_wait_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中转工位待机位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_Transfer_X, };
                double[] poses = new double[] { recipe.TransferXSafePos };

                WaitForm.Show(string.Format("定位到中转工位待机位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();

                    if (!_Worker.AxisTransferXGoPos(recipe.TransferXSafePos))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中转工位待机位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Transfer_pick1_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位左撕膜工位下料位置?"))
            {
                LocateSMDischargePos(0);
            }
        }

        private void btnEx_Transfer_pick2_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位中撕膜工位下料位置?"))
            {
                LocateSMDischargePos(1);
            }
        }

        private void btnEx_Transfer_pick3_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位右撕膜工位下料位置?"))
            {
                LocateSMDischargePos(2);
            }
        }

        private void btnEx_Transfer_NGA_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中转工位NG放料A位置?"))
            {

                WaitForm.Show(string.Format("定位到中转工位NG放料A位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.AxisTransferXGoPos(recipe.TransferX_NGA_Pos))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中转工位NG放料A位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Transfer_NGB_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中转工位NG放料B位置?"))
            {

                WaitForm.Show(string.Format("定位到中转工位NG放料B位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.AxisTransferXGoPos(recipe.TransferX_NGB_Pos))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中转工位NG放料B位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Transfer_NGC_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中转工位NG放料C位置?"))
            {


                WaitForm.Show(string.Format("定位到中转工位NG放料C位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.AxisTransferXGoPos(recipe.TransferX_NGC_Pos))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中转工位NG放料C位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Transfer_pull1_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯上料放料位置?"))
            {

                double[] poses = new double[] { recipe.LeftBend_LoadX, recipe.LeftBend_LoadY };

                WaitForm.Show(string.Format("定位到左折弯上料放料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();

                    if (!_Worker.AxisTransferXYMoveTo(_Worker.Axis_LeftBend_stgY, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯上料放料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Transfer_pull2_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯上料放料位置?"))
            {

                double[] poses = new double[] { recipe.MidBend_LoadX, recipe.MidBend_LoadY };

                WaitForm.Show(string.Format("定位到中折弯上料放料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();

                    if (!_Worker.AxisTransferXYMoveTo(_Worker.Axis_MidBend_stgY, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯上料放料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_Transfer_pull3_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯上料放料位置?"))
            {

                double[] poses = new double[] { recipe.RightBend_LoadX, recipe.RightBend_LoadY };

                WaitForm.Show(string.Format("定位到右折弯上料放料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.AxisTransferXYMoveTo(_Worker.Axis_RightBend_stgY, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右折弯上料放料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_discharge_wait_Set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教下料待机位置?"))
            {
                var_discharge_wait_x.Value = _Worker.Axis_Discharge_X.PositionDev;
                var_discharge_wait_z.Value = _Worker.Axis_Discharge_Z.PositionDev;


                recipe.DischargeXSafePos = var_discharge_wait_x.Value;
                recipe.DischargeZSafePos = var_discharge_wait_z.Value;
            }
        }

        private void btnEx_discharge_pick1_set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯下料取料位置?"))
            {

                var_discharge_pick_x1.Value = MeasurementContext.Worker.Axis_Discharge_X.PositionDev;
                var_discharge_pick_y1.Value = MeasurementContext.Worker.Axis_LeftBend_stgY.PositionDev;
                var_discharge_pick_z1.Value = MeasurementContext.Worker.Axis_Discharge_Z.PositionDev;

                recipe.LeftBend_Discharge_x = var_discharge_pick_x1.Value;
                recipe.LeftBend_Discharge_Y = var_discharge_pick_y1.Value;
                recipe.LeftBend_Discharge_Z = var_discharge_pick_z1.Value;

            }
        }

        private void btnEx_discharge_pick2_set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯下料取料位置?"))
            {
                var_discharge_pick_x2.Value = MeasurementContext.Worker.Axis_Discharge_X.PositionDev;
                var_discharge_pick_y2.Value = MeasurementContext.Worker.Axis_MidBend_stgY.PositionDev;
                var_discharge_pick_z2.Value = MeasurementContext.Worker.Axis_Discharge_Z.PositionDev;

                recipe.MidBend_Discharge_X = var_discharge_pick_x2.Value;
                recipe.MidBend_Discharge_Y = var_discharge_pick_y2.Value;
                recipe.MidBend_Discharge_Z = var_discharge_pick_z2.Value;
            }
        }

        private void btnEx_discharge_pick3_set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右折弯下料取料位置?"))
            {
                var_discharge_pick_x3.Value = MeasurementContext.Worker.Axis_Discharge_X.PositionDev;
                var_discharge_pick_y3.Value = MeasurementContext.Worker.Axis_RightBend_stgY.PositionDev;
                var_discharge_pick_z3.Value = MeasurementContext.Worker.Axis_Discharge_Z.PositionDev;

                recipe.RightBend_Discharge_X = var_discharge_pick_x3.Value;
                recipe.RightBend_Discharge_Y = var_discharge_pick_y3.Value;
                recipe.RightBend_Discharge_Z = var_discharge_pick_z3.Value;
            }
        }

        private void btnEx_discharge_pullOK_set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教折弯OK下料位置?"))
            {
                var_discharge_pull_OK_x.Value = _Worker.Axis_Discharge_X.PositionDev;
                var_discharge_pull_OK_z.Value = _Worker.Axis_Discharge_Z.PositionDev;

                recipe.DischargeX_OK_PullPos = var_discharge_pull_OK_x.Value;
                recipe.DischargeZ_OK_PullPos = var_discharge_pull_OK_z.Value;
            }
        }

        private void btnEx_discharge_pullNG_set_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教折弯NG下料位置?"))
            {
                var_discharge_pull_NG_x.Value = _Worker.Axis_Discharge_X.PositionDev;
                var_discharge_pull_NG_z.Value = _Worker.Axis_Discharge_Z.PositionDev;

                recipe.DischargeX_NG_PullPos = var_discharge_pull_NG_x.Value;
                recipe.DischargeZ_NG_PullPos = var_discharge_pull_NG_z.Value;
            }
        }

        private void btnEx_discharge_wait_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到下料工位待机位置?"))
            {
                WaitForm.Show(string.Format("定位到下料工位待机位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.DischargeXGoPos(recipe.DischargeXSafePos))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到下料工位待机位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_discharge_pick1_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯下料取料位置?"))
            {

                double[] poses = new double[] { recipe.LeftBend_Discharge_x, recipe.LeftBend_Discharge_Y };
                WaitForm.Show(string.Format("定位到左折弯下料取料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();

                    if (!_Worker.DischargeXYMoveTo(_Worker.Axis_LeftBend_stgY, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯下料取料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_discharge_pick2_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯下料取料位置?"))
            {
                double[] poses = new double[] { recipe.MidBend_Discharge_X, recipe.MidBend_Discharge_Y };
                WaitForm.Show(string.Format("定位到中折弯下料取料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();


                    if (!_Worker.DischargeXYMoveTo(_Worker.Axis_MidBend_stgY, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯下料取料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_discharge_pick3_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯下料取料位置?"))
            {

                double[] poses = new double[] { recipe.RightBend_Discharge_X, recipe.RightBend_Discharge_Y };

                WaitForm.Show(string.Format("定位到右折弯下料取料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.DischargeXYMoveTo(_Worker.Axis_RightBend_stgY, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右折弯下料取料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_discharge_pickOK_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到下料工位OK放料位置?"))
            {
                WaitForm.Show(string.Format("定位到下料工位OK放料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.DischargeXGoPos(recipe.DischargeX_OK_PullPos))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到下料工位OK放料失败..."));
                    }

                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btnEx_discharge_pickNG_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到下料工位NG放料位置?"))
            {

                WaitForm.Show(string.Format("定位到下料工位NG放料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.DischargeXGoPos(recipe.DischargeX_NG_PullPos))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到下料工位NG放料失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        #endregion

        private void CarryAxiseStation_Value()
        {
            recipe.LoadXWaitPos = var_Feed_wait_x.Value;
            recipe.LoadZWaitPos = var_Feed_wait_z.Value;

            recipe.LoadXpos = var_Feed_pick_x.Value;
            //  recipe.LoadYpos = var_Feed_pick_y.Value;
            recipe.LoadZpos = var_Feed_pick_z.Value;

            recipe.SMPosition[0].Lsm_loadX = var_Feed_pull_x1.Value;
            recipe.SMPosition[0].Lsm_loadY = var_Feed_pull_y1.Value;
            recipe.SMPosition[0].Lsm_LoadZ = var_Feed_pull_z1.Value;

            recipe.SMPosition[1].Lsm_loadX = var_Feed_pull_x2.Value;
            recipe.SMPosition[1].Lsm_loadY = var_Feed_pull_y2.Value;
            recipe.SMPosition[1].Lsm_LoadZ = var_Feed_pull_z2.Value;

            recipe.SMPosition[2].Lsm_loadX = var_Feed_pull_x3.Value;
            recipe.SMPosition[2].Lsm_loadY = var_Feed_pull_y3.Value;
            recipe.SMPosition[2].Lsm_LoadZ = var_Feed_pull_z3.Value;

            recipe.LoadYWaitPos = var_Feed_wait_y.Value;
            recipe.LoadYpos = var_feed_work_y.Value;
            recipe.QrCodePos = Var_QRCode.Value;

            recipe.TransferXSafePos = var_transfer_wait_x.Value;
            recipe.TransferZSafePos = var_transfer_wait_z.Value;

            recipe.SMPosition[0].Lsm_DischargeX = var_transfer_pick_x1.Value;
            recipe.SMPosition[0].Lsm_DischargeY = var_transfer_pick_y1.Value;
            recipe.SMPosition[0].Lsm_DischargeZ = var_transfer_pick_z1.Value;

            recipe.SMPosition[1].Lsm_DischargeX = var_transfer_pick_x2.Value;
            recipe.SMPosition[1].Lsm_DischargeY = var_transfer_pick_y2.Value;
            recipe.SMPosition[1].Lsm_DischargeZ = var_transfer_pick_z2.Value;

            recipe.SMPosition[2].Lsm_DischargeX = var_transfer_pick_x3.Value;
            recipe.SMPosition[2].Lsm_DischargeY = var_transfer_pick_y3.Value;
            recipe.SMPosition[2].Lsm_DischargeZ = var_transfer_pick_z3.Value;

            recipe.TransferX_NGA_Pos = var_transfer_pull_NGA_x.Value;
            recipe.TransferZ_NGA_Pos = var_transfer_pull_NGA_z.Value;

            recipe.TransferX_NGB_Pos = var_transfer_pull_NGB_x.Value;
            recipe.TransferZ_NGB_Pos = var_transfer_pull_NGB_z.Value;

            recipe.TransferX_NGC_Pos = var_transfer_pull_NGC_x.Value;
            recipe.TransferZ_NGC_Pos = var_transfer_pull_NGC_z.Value;

            recipe.LeftBend_LoadX = var_transfer_pull_x1.Value;
            recipe.LeftBend_LoadY = var_transfer_pull_y1.Value;
            recipe.LeftBend_LoadZ = var_transfer_pull_z1.Value;

            recipe.MidBend_LoadX = var_transfer_pull_x2.Value;
            recipe.MidBend_LoadY = var_transfer_pull_y2.Value;
            recipe.MidBend_LoadZ = var_transfer_pull_z2.Value;

            recipe.RightBend_LoadX = var_transfer_pull_x3.Value;
            recipe.RightBend_LoadY = var_transfer_pull_y3.Value;
            recipe.RightBend_LoadZ = var_transfer_pull_z3.Value;

            recipe.DischargeXSafePos = var_discharge_wait_x.Value;
            recipe.DischargeZSafePos = var_discharge_wait_z.Value;

            recipe.LeftBend_Discharge_x = var_discharge_pick_x1.Value;
            recipe.LeftBend_Discharge_Y = var_discharge_pick_y1.Value;
            recipe.LeftBend_Discharge_Z = var_discharge_pick_z1.Value;

            recipe.MidBend_Discharge_X = var_discharge_pick_x2.Value;
            recipe.MidBend_Discharge_Y = var_discharge_pick_y2.Value;
            recipe.MidBend_Discharge_Z = var_discharge_pick_z2.Value;

            recipe.RightBend_Discharge_X = var_discharge_pick_x3.Value;
            recipe.RightBend_Discharge_Y = var_discharge_pick_y3.Value;
            recipe.RightBend_Discharge_Z = var_discharge_pick_z3.Value;

            recipe.DischargeX_OK_PullPos = var_discharge_pull_OK_x.Value;
            recipe.DischargeZ_OK_PullPos = var_discharge_pull_OK_z.Value;

            recipe.DischargeX_NG_PullPos = var_discharge_pull_NG_x.Value;
            recipe.DischargeZ_NG_PullPos = var_discharge_pull_NG_z.Value;



            //recipe.LeftBend_NGDischarge_x = var_NGdischarge_pick_x1.Value;
            recipe.LeftBend_NGDischarge_Y = var_NGdischarge_pick_y1.Value;
            // recipe.LeftBend_NGDischarge_Z = var_NGdischarge_pick_z1.Value;

            // recipe.MidBend_NGDischarge_X = var_NGdischarge_pick_x2.Value;
            recipe.MidBend_NGDischarge_Y = var_NGdischarge_pick_y2.Value;
            // recipe.MidBend_NGDischarge_Z = var_NGdischarge_pick_z2.Value;

            // recipe.RightBend_NGDischarge_X = var_NGdischarge_pick_x3.Value;
            recipe.RightBend_NGDischarge_Y = var_NGdischarge_pick_y3.Value;
            // recipe.RightBend_NGDischarge_Z = var_NGdischarge_pick_z3.Value;
        }


        private void Init_Carry_Value()
        {
            var_Feed_wait_x.Value = recipe.LoadXWaitPos;
            var_Feed_wait_z.Value = recipe.LoadZWaitPos;

            var_Feed_pick_x.Value = recipe.LoadXpos;
            //  recipe.LoadYpos = var_Feed_pick_y.Value;
            var_Feed_pick_z.Value = recipe.LoadZpos;

            var_Feed_pull_x1.Value = recipe.SMPosition[0].Lsm_loadX;
            var_Feed_pull_y1.Value = recipe.SMPosition[0].Lsm_loadY;
            var_Feed_pull_z1.Value = recipe.SMPosition[0].Lsm_LoadZ;

            var_Feed_pull_x2.Value = recipe.SMPosition[1].Lsm_loadX;
            var_Feed_pull_y2.Value = recipe.SMPosition[1].Lsm_loadY;
            var_Feed_pull_z2.Value = recipe.SMPosition[1].Lsm_LoadZ;

            var_Feed_pull_x3.Value = recipe.SMPosition[2].Lsm_loadX;
            var_Feed_pull_y3.Value = recipe.SMPosition[2].Lsm_loadY;
            var_Feed_pull_z3.Value = recipe.SMPosition[2].Lsm_LoadZ;

            var_Feed_wait_y.Value = recipe.LoadYWaitPos;
            var_feed_work_y.Value = recipe.LoadYpos;

            Var_QRCode.Value = recipe.QrCodePos;

            var_transfer_wait_x.Value = recipe.TransferXSafePos;
            var_transfer_wait_z.Value = recipe.TransferZSafePos;

            var_transfer_pick_x1.Value = recipe.SMPosition[0].Lsm_DischargeX;
            var_transfer_pick_y1.Value = recipe.SMPosition[0].Lsm_DischargeY;
            var_transfer_pick_z1.Value = recipe.SMPosition[0].Lsm_DischargeZ;

            var_transfer_pick_x2.Value = recipe.SMPosition[1].Lsm_DischargeX;
            var_transfer_pick_y2.Value = recipe.SMPosition[1].Lsm_DischargeY;
            var_transfer_pick_z2.Value = recipe.SMPosition[1].Lsm_DischargeZ;

            var_transfer_pick_x3.Value = recipe.SMPosition[2].Lsm_DischargeX;
            var_transfer_pick_y3.Value = recipe.SMPosition[2].Lsm_DischargeY;
            var_transfer_pick_z3.Value = recipe.SMPosition[2].Lsm_DischargeZ;

            var_transfer_pull_NGA_x.Value = recipe.TransferX_NGA_Pos;
            var_transfer_pull_NGA_z.Value = recipe.TransferZ_NGA_Pos;

            var_transfer_pull_NGB_x.Value = recipe.TransferX_NGB_Pos;
            var_transfer_pull_NGB_z.Value = recipe.TransferZ_NGB_Pos;

            var_transfer_pull_NGC_x.Value = recipe.TransferX_NGC_Pos;
            var_transfer_pull_NGC_z.Value = recipe.TransferZ_NGC_Pos;

            var_transfer_pull_x1.Value = recipe.LeftBend_LoadX;
            var_transfer_pull_y1.Value = recipe.LeftBend_LoadY;
            var_transfer_pull_z1.Value = recipe.LeftBend_LoadZ;

            var_transfer_pull_x2.Value = recipe.MidBend_LoadX;
            var_transfer_pull_y2.Value = recipe.MidBend_LoadY;
            var_transfer_pull_z2.Value = recipe.MidBend_LoadZ;

            var_transfer_pull_x3.Value = recipe.RightBend_LoadX;
            var_transfer_pull_y3.Value = recipe.RightBend_LoadY;
            var_transfer_pull_z3.Value = recipe.RightBend_LoadZ;


            var_discharge_wait_x.Value = recipe.DischargeXSafePos;
            var_discharge_wait_z.Value = recipe.DischargeZSafePos;

            var_discharge_pick_x1.Value = recipe.LeftBend_Discharge_x;
            var_discharge_pick_y1.Value = recipe.LeftBend_Discharge_Y;
            var_discharge_pick_z1.Value = recipe.LeftBend_Discharge_Z;

            var_discharge_pick_x2.Value = recipe.MidBend_Discharge_X;
            var_discharge_pick_y2.Value = recipe.MidBend_Discharge_Y;
            var_discharge_pick_z2.Value = recipe.MidBend_Discharge_Z;

            var_discharge_pick_x3.Value = recipe.RightBend_Discharge_X;
            var_discharge_pick_y3.Value = recipe.RightBend_Discharge_Y;
            var_discharge_pick_z3.Value = recipe.RightBend_Discharge_Z;

            var_discharge_pull_OK_x.Value = recipe.DischargeX_OK_PullPos;
            var_discharge_pull_OK_z.Value = recipe.DischargeZ_OK_PullPos;

            var_discharge_pull_NG_x.Value = recipe.DischargeX_NG_PullPos;
            var_discharge_pull_NG_z.Value = recipe.DischargeZ_NG_PullPos;


            //   var_NGdischarge_pick_x1.Value = recipe.LeftBend_NGDischarge_x;
            var_NGdischarge_pick_y1.Value = recipe.LeftBend_NGDischarge_Y;
            //  var_NGdischarge_pick_z1.Value = recipe.LeftBend_NGDischarge_Z;

            // var_NGdischarge_pick_x2.Value = recipe.MidBend_NGDischarge_X;
            var_NGdischarge_pick_y2.Value = recipe.MidBend_NGDischarge_Y;
            //  var_NGdischarge_pick_z2.Value = recipe.MidBend_NGDischarge_Z;

            //  var_NGdischarge_pick_x3.Value = recipe.RightBend_NGDischarge_X;
            var_NGdischarge_pick_y3.Value = recipe.RightBend_NGDischarge_Y;
            //  var_NGdischarge_pick_z3.Value = recipe.RightBend_NGDischarge_Z;

        }

        private void btn_leftcylinder_work_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行左撕膜工位气缸对位动作?"))
            {
                WaitForm.Show(string.Format("左工位撕膜气缸对位中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStaion_CylinderWork(0))
                    {
                        WaitForm.ShowErrorMessage(string.Format("左工位撕膜气缸对位失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midcylinder_work_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行中撕膜工位气缸对位动作?"))
            {
                WaitForm.Show(string.Format("中撕膜工位气缸对位中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStaion_CylinderWork(1))
                    {
                        WaitForm.ShowErrorMessage(string.Format("中撕膜工位气缸对位失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_rightcylinder_work_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行右撕膜工位气缸对位动作?"))
            {
                WaitForm.Show(string.Format("右撕膜工位气缸对位中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStaion_CylinderWork(2))
                    {
                        WaitForm.ShowErrorMessage(string.Format("右撕膜工位气缸对位失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }


        private void btn_1_pz_move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否走到左折弯角度拍照位?"))
            {
                WaitForm.Show(string.Format("左折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.Bend1_Go_Degree_Pos())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_1_zb_move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否走到左折弯校正拍照位?"))
            {
                WaitForm.Show(string.Format("左折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.Bend1_Go_FZ_Pos())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_2_PZ_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否走到中折弯角度拍照位?"))
            {
                WaitForm.Show(string.Format("中折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.Bend2_Go_Degree_Pos())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_2__ZB_Move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否走到中折弯校正拍照位?"))
            {
                WaitForm.Show(string.Format("中折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.Bend2_Go_FZ_Pos())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_3_pz_move_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否走到右折弯角度拍照位?"))
            {
                WaitForm.Show(string.Format("右折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.Bend3_Go_Degree_Pos())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_3_zb_move_Click(object sender, EventArgs e)
        {

            if (MessageBoxEx.ShowSystemQuestion("是否走到右折弯校正拍照位?"))
            {
                WaitForm.Show(string.Format("右折弯运动中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.Bend3_Go_FZ_Pos())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            _Worker.TearSendMsg("S1,PZ");
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            _Worker.TearSendMsg("S2,PZ");
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            _Worker.TearSendMsg("S3,PZ");
        }

        private void button27_Click_1(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯Y轴反折位置?"))
            {
                Inp_Leftbend_WorkPt_Y.Value = MeasurementContext.Worker.Axis_LeftBend_stgY.PositionDev;
                recipe.LeftBend_Y_WorkPos = Inp_Leftbend_WorkPt_Y.Value;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯Y轴反折位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_stgY };
                double[] poses = new double[] { recipe.LeftBend_Y_WorkPos };
                WaitForm.Show(string.Format("定位左折弯Y轴反折位置中"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseLeftBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.LeftBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button31_Click_1(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯Y轴反折位置?"))
            {
                Inp_Midbend_WorkPt_Y.Value = MeasurementContext.Worker.Axis_MidBend_stgY.PositionDev;
                recipe.MidBend_Y_WorkPos = Inp_Midbend_WorkPt_Y.Value;
            }
        }

        private void button28_Click_1(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯Y轴反折位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_stgY };
                double[] poses = new double[] { recipe.MidBend_Y_WorkPos };
                WaitForm.Show(string.Format("定位中折弯Y轴反折位置中"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseMidBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.MidBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教折弯Y轴反折位置?"))
            {
                Inp_RightBend_WorkPt_Y.Value = MeasurementContext.Worker.Axis_RightBend_stgY.PositionDev;
                recipe.RightBend_Y_WorkPos = Inp_RightBend_WorkPt_Y.Value;
            }
        }

        private void button32_Click_1(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯Y轴反折位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_stgY };
                double[] poses = new double[] { recipe.RightBend_Y_WorkPos };
                WaitForm.Show(string.Format("定位右折弯Y轴反折位置中"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseRightBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.RightBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否运行上料取料动作?"))
            {
                WaitForm.Show(string.Format("上料取料中."), (IAsyncResult argument0) =>
                {

                    if (!_Worker.RunloadStation_TakeMatrial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料取料动作失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button51_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否流水线上料?"))
            {
                WaitForm.Show(string.Format("流水线上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.EnforceFeedLine())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.ForceStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行左撕膜工位上料?"))
            {
                WaitForm.Show(string.Format("左撕膜工位上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunloadStation_PullMatrial(0))
                    {
                        WaitForm.ShowErrorMessage(string.Format("左撕膜工位上料失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行中撕膜工位上料?"))
            {
                WaitForm.Show(string.Format("中撕膜工位上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunloadStation_PullMatrial(1))
                    {
                        WaitForm.ShowErrorMessage(string.Format("中撕膜工位上料失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行右撕膜工位上料?"))
            {
                WaitForm.Show(string.Format("右撕膜工位上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunloadStation_PullMatrial(2))
                    {
                        WaitForm.ShowErrorMessage(string.Format("右撕膜工位上料失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button43_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行左撕膜工位下料?"))
            {
                WaitForm.Show(string.Format("左撕膜工位下料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStation_GetOutoffStg(0))
                    {
                        WaitForm.ShowErrorMessage(string.Format("左撕膜工位下料失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button41_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行中撕膜工位下料?"))
            {
                WaitForm.Show(string.Format("中撕膜工位下料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStation_GetOutoffStg(1))
                    {
                        WaitForm.ShowErrorMessage(string.Format("中撕膜工位下料失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button47_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行右撕膜工位下料?"))
            {
                WaitForm.Show(string.Format("右撕膜工位下料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RunSMStation_GetOutoffStg(2))
                    {
                        WaitForm.ShowErrorMessage(string.Format("右撕膜工位下料失败.."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button45_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否左折弯上料动作?"))
            {
                WaitForm.Show(string.Format("左折弯上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.LeftBendStation_PullMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否中折弯上料动作?"))
            {
                WaitForm.Show(string.Format("中折弯上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.MidBendStation_PullMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button42_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否右折弯上料动作?"))
            {
                WaitForm.Show(string.Format("右折弯上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.RightBendStation_PullMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button85_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否上料Z轴回安全位?"))
            {
                WaitForm.Show(string.Format("Z轴回安全位中."), (IAsyncResult argument0) =>
                {
                    if (!MeasurementContext.Worker.FeedZWork(0, recipe.LoadZWaitPos, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button131_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否上料Z轴到取料位?"))
            {
                WaitForm.Show(string.Format("Z轴运动到取料位中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.FeedZWork(1, recipe.LoadZpos, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button132_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否上料Z轴到放料位1?"))
            {
                WaitForm.Show(string.Format("Z轴运动到放料位1中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.FeedZWork(1, recipe.SMPosition[0].Lsm_LoadZ, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button133_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否上料Z轴到放料位2?"))
            {
                WaitForm.Show(string.Format("Z轴运动到放料位2中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.FeedZWork(1, recipe.SMPosition[1].Lsm_LoadZ, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button134_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否上料Z轴到放料位3?"))
            {
                WaitForm.Show(string.Format("Z轴运动到放料位3中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.FeedZWork(1, recipe.SMPosition[2].Lsm_LoadZ, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }












        private void btn_TransferZ_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ctrlName = ((Control)sender).Name.Split('_');
                int index = int.Parse(ctrlName[3]) - 1;
                double axisPos = 0;
                int cylinderVal = 0;//气缸动作标识  0上升 1下降
                switch (ctrlName[2])
                {
                    case "Standy":
                        axisPos = recipe.TransferZSafePos;
                        cylinderVal = 0;
                        break;

                    case "Fetch":
                        axisPos = recipe.SMPosition[index].Lsm_DischargeZ;
                        cylinderVal = 1;
                        break;

                    case "NGDrop":
                        cylinderVal = 1;
                        break;

                    case "Drop":
                        if (index == 0)
                        {
                            axisPos = recipe.LeftBend_LoadZ;
                        }

                        if (index == 1)
                        {
                            axisPos = recipe.MidBend_LoadZ;
                        }

                        if (index == 2)
                        {
                            axisPos = recipe.RightBend_LoadZ;
                        }
                        cylinderVal = 1;
                        break;
                }


                if (MessageBoxEx.ShowSystemQuestion("是否执行中转Z动作?"))
                {
                    WaitForm.Show(string.Format("中转Z动作中."), (IAsyncResult argument0) =>
                    {

                        if (!MeasurementContext.Worker.TransferZWork(cylinderVal, axisPos, true))
                        {
                            WaitForm.ShowErrorMessage(string.Format("动作失败"));
                        }

                    }, (IAsyncResult argument1) =>
                    {
                        MeasurementContext.Worker.EndMotion();
                        MeasurementContext.Worker.StopSlowly();
                    });
                }

            }
            catch (Exception ex)
            {
                MeasurementContext.OutputError("TransferZ_Click" + ex.Message);
            }
            finally
            {
            }
        }


        private void btn_DischargeZ_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ctrlName = ((Control)sender).Name.Split('_');
                int index = int.Parse(ctrlName[3]) - 1;
                double axisPos = 0;
                switch (ctrlName[2])
                {
                    case "Standy":
                        axisPos = recipe.DischargeZSafePos;

                        break;

                    case "Fetch":
                        if (index == 0)
                        {
                            axisPos = recipe.LeftBend_Discharge_Z;
                        }
                        if (index == 1)
                        {
                            axisPos = recipe.MidBend_Discharge_Z;
                        }
                        if (index == 2)
                        {
                            axisPos = recipe.RightBend_Discharge_Z;
                        }
                        break;

                    case "OKDrop":
                        axisPos = recipe.DischargeZ_OK_PullPos;
                        break;

                    case "NGDrop":
                        axisPos = recipe.DischargeZ_NG_PullPos;
                        break;

                    case "NGFetch":
                        if (index == 0)
                        {
                            axisPos = recipe.LeftBend_Discharge_Z;
                        }
                        if (index == 1)
                        {
                            axisPos = recipe.MidBend_Discharge_Z;
                        }
                        if (index == 2)
                        {
                            axisPos = recipe.RightBend_Discharge_Z;
                        }
                        break;
                }

                if (MessageBoxEx.ShowSystemQuestion("是否执行下料Z轴动作?"))
                {
                    WaitForm.Show(string.Format("下料Z轴动作中."), (IAsyncResult argument0) =>
                    {
                        if (!_Worker.DischargeZGoPos(axisPos))
                        {
                            WaitForm.ShowErrorMessage(string.Format("动作失败"));
                        }
                    }, (IAsyncResult argument1) =>
                    {
                        MeasurementContext.Worker.EndMotion();
                        MeasurementContext.Worker.StopSlowly();
                    });
                }

            }
            catch (Exception ex)
            {
                MeasurementContext.OutputError("DischargeZ_Click" + ex.Message);
            }
            finally
            {
            }

        }


        private void button86_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否流水线上料?"))
            {
                WaitForm.Show(string.Format("流水线上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.EnforceFeedLine())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.ForceStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }
        private void button87_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否流水线上料?"))
            {
                WaitForm.Show(string.Format("流水线上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.EnforceFeedLine())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.ForceStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button92_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否流水线上料?"))
            {
                WaitForm.Show(string.Format("流水线上料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.EnforceFeedLine())
                    {
                        WaitForm.ShowErrorMessage(string.Format("上料失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.ForceStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button129_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左撕膜Y轴轴上料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftSM_Z };
                double[] poses = new double[] { 0 };

                WaitForm.Show(string.Format("定位到左撕膜Y轴上料位置"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (_Worker.AxisMoveTo(Axises, poses))
                    {
                        if (!_Worker.AxisMoveTo(_Worker.Axis_LeftSM_Y, recipe.SMPosition[0].Lsm_loadY))
                        {
                            WaitForm.ShowErrorMessage(string.Format("动作失败"));
                        }
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }


        private void button103_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左撕膜Y轴轴下料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftSM_Z };
                double[] poses = new double[] { 0 };

                WaitForm.Show(string.Format("定位到左撕膜Y轴下料位置"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.RunSMStaton_GoOutPos(StationType.Left))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button136_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中撕膜Y轴轴上料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidSM_Z };
                double[] poses = new double[] { 0 };

                WaitForm.Show(string.Format("定位到中撕膜Y轴上料位置"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (_Worker.AxisMoveTo(Axises, poses))
                    {
                        if (!_Worker.AxisMoveTo(_Worker.Axis_MidSM_Y, recipe.SMPosition[1].Lsm_loadY))
                        {
                            WaitForm.ShowErrorMessage(string.Format("动作失败"));
                        }
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button135_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中撕膜Y轴轴下料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidSM_Z };
                double[] poses = new double[] { 0 };

                WaitForm.Show(string.Format("定位到中撕膜Y轴下料位置"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.RunSMStaton_GoOutPos(StationType.Mid))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button138_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右撕膜Y轴轴上料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightSM_Z };
                double[] poses = new double[] { 0 };

                WaitForm.Show(string.Format("定位到右撕膜Y轴上料位置"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (_Worker.AxisMoveTo(Axises, poses))
                    {
                        if (!_Worker.AxisMoveTo(_Worker.Axis_RightSM_Y, recipe.SMPosition[2].Lsm_loadY))
                        {
                            WaitForm.ShowErrorMessage(string.Format("动作失败"));
                        }
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button137_Click(object sender, EventArgs e)
        {

            if (MessageBoxEx.ShowSystemQuestion("是否定位到右撕膜Y轴轴下料位置?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightSM_Z };
                double[] poses = new double[] { 0 };

                WaitForm.Show(string.Format("定位到右撕膜Y轴下料位置"), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.RunSMStaton_GoOutPos(StationType.Right))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button49_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否左折弯下料动作?"))
            {
                WaitForm.Show(string.Format("左折弯下料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.LeftBendStation_PickMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("下料动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button48_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否中折弯下料动作?"))
            {
                WaitForm.Show(string.Format("中折弯下料中."), (IAsyncResult argument0) =>
                {

                    if (!_Worker.MidBendStation_PickMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("下料动作失败"));
                    }

                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button50_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否右折弯下料动作?"))
            {
                WaitForm.Show(string.Format("右折弯下料中."), (IAsyncResult argument0) =>
                {

                    if (!_Worker.RightBendStation_PickMaterial())
                    {
                        WaitForm.ShowErrorMessage(string.Format("下料动作失败"));
                    }

                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button139_Click(object sender, EventArgs e)
        {

            _Worker.str_Bend1Rev = "";
            if (!_Worker.SendMsg("A,Calib"))
            {
                MeasurementContext.OutputError("折弯相机1发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend1Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend1Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button140_Click(object sender, EventArgs e)
        {
            _Worker.str_Bend1Rev = "";
            if (!_Worker.SendMsg("A,PZXY"))
            {
                MeasurementContext.OutputError("折弯相机1发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend1Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend1Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button141_Click(object sender, EventArgs e)
        {
            _Worker.str_Bend1Rev = "";
            if (!_Worker.SendMsg("A,AOI"))
            {
                MeasurementContext.OutputError("折弯相机1发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend1Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend1Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button142_Click(object sender, EventArgs e)
        {
            _Worker.str_Bend2Rev = "";
            if (!_Worker.Bend2SendMsg("B,Calib"))
            {
                MeasurementContext.OutputError("折弯相机2发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend2Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend2Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button143_Click(object sender, EventArgs e)
        {

            _Worker.str_Bend2Rev = "";
            if (!_Worker.Bend2SendMsg("B,PZXY"))
            {
                MeasurementContext.OutputError("折弯相机3发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend2Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend2Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button144_Click(object sender, EventArgs e)
        {
            _Worker.str_Bend2Rev = "";
            if (!_Worker.Bend2SendMsg("B,AOI"))
            {
                MeasurementContext.OutputError("折弯相机2发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend2Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend2Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button145_Click(object sender, EventArgs e)
        {
            _Worker.str_Bend3Rev = "";
            if (!_Worker.Bend3SendMsg("C,Calib"))
            {
                MeasurementContext.OutputError("折弯相机3发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend3Rev.Length < 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend3Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }
            }
        }

        private void button146_Click(object sender, EventArgs e)
        {

            _Worker.str_Bend3Rev = "";
            if (!_Worker.Bend3SendMsg("C,PZXY"))
            {
                MeasurementContext.OutputError("折弯相机3发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();

                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend3Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend3Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }


            }



        }

        private void button147_Click(object sender, EventArgs e)
        {
            _Worker.str_Bend3Rev = "";
            if (!_Worker.Bend3SendMsg("C,AOI"))
            {
                MeasurementContext.OutputError("折弯相机3发送拍照数据失败");
            }
            else
            {
                Stopwatch stw = new Stopwatch();
                stw.Restart();
                while (stw.ElapsedMilliseconds < 3000)
                {
                    if (_Worker.str_Bend3Rev.Length > 2)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                if (_Worker.str_Bend3Rev.Length < 2)
                {
                    MessageBox.Show("没有接收到相机返回数据");
                    return;
                }


            }
        }

        private void button149_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到扫码位置?"))
            {
                LocateQrCodePos();
            }
        }

        private void button148_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教扫码位置?"))
            {
                Var_QRCode.Value = MeasurementContext.Worker.Axis_Load_X.PositionDev;
                recipe.QrCodePos = Var_QRCode.Value;
            }
        }

        private void button150_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否进行扫码动作?"))
            {
                WaitForm.Show(string.Format("扫码中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("扫码失败..."));
                    }
                    else if (_Worker.RunQRCodeWork() == "NG")
                    {
                        WaitForm.ShowErrorMessage(string.Format("扫码失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button151_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否给扫码枪发扫码指令?"))
            {
                WaitForm.Show(string.Format("等待返回二维码."), (IAsyncResult argument0) =>
                {
                    string code = _Worker.ScanCode();
                    MessageBox.Show(code);

                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.IsStop = true;

                });
            }
        }

        private void btn_leftsmsafezlearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录左撕膜Z轴安全位置?"))
            {

                MeasurementAxis axisz = MeasurementContext.Worker.Axis_LeftSM_Z;
                Inp_leftsmZSafe.Value = axisz.PositionDev;

                if (recipe != null)
                {
                    recipe.SMPosition[0].Lsm_WaitZ = Inp_leftsmZSafe.Value;
                }
            }
        }

        private void btn_midsmsafezlearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录中撕膜Z轴安全位置?"))
            {
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_MidSM_Z;
                Inp_midsmZsafe.Value = axisz.PositionDev;
                if (recipe != null)
                {
                    recipe.SMPosition[1].Lsm_WaitZ = Inp_midsmZsafe.Value;
                }
            }
        }

        private void btn_rightsmsafezlearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否记录中撕膜Z轴安全位置?"))
            {
                MeasurementAxis axisz = MeasurementContext.Worker.Axis_RightSM_Z;
                Inp_rightsmZsafe.Value = axisz.PositionDev;
                if (recipe != null)
                {
                    recipe.SMPosition[2].Lsm_WaitZ = Inp_rightsmZsafe.Value;
                }
            }
        }

        private void btn_rightsmsafezlocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位右撕膜工位Z轴安全位置?"))
            {
                WaitForm.Show("定位右撕膜工位Z轴安全位置", (IAsyncResult argument0) =>
                {
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位右撕膜Z轴安全位置失败..."));
                    }
                    else
                    {
                        if (!_Worker.AxisMoveTo(_Worker.Axis_RightSM_Z, recipe.SMPosition[2].Lsm_WaitZ))
                        {
                            WaitForm.ShowErrorMessage(string.Format("定位右撕膜Z轴安全位置失败..."));
                        }
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_leftsmsafezlocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位左撕膜工位Z轴安全位置?"))
            {
                WaitForm.Show("定位左撕膜工位Z轴安全位置", (IAsyncResult argument0) =>
                {
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位左撕膜Z轴安全位置失败..."));
                    }
                    else
                    {
                        if (!_Worker.AxisMoveTo(_Worker.Axis_LeftSM_Z, recipe.SMPosition[0].Lsm_WaitZ))
                        {
                            WaitForm.ShowErrorMessage(string.Format("定位左撕膜Z轴安全位置失败..."));
                        }
                    }
                }, (IAsyncResult argument1) =>
                {

                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midsmsafezlocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位中撕膜工位Z轴安全位置?"))
            {
                WaitForm.Show("定位中撕膜工位Z轴安全位置", (IAsyncResult argument0) =>
                {
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位中撕膜Z轴安全位置失败..."));
                    }
                    else
                    {
                        if (!_Worker.AxisMoveTo(_Worker.Axis_MidSM_Z, recipe.SMPosition[1].Lsm_WaitZ))
                        {
                            WaitForm.ShowErrorMessage(string.Format("定位中撕膜Z轴安全位置失败..."));
                        }
                    }
                }, (IAsyncResult argument1) =>
                {

                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button152_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否下料气缸动作?"))
            {
                WaitForm.Show(string.Format("下料中."), (IAsyncResult argument0) =>
                {
                    if (!_Worker.Discharge_FeedCylinderWork())
                    {
                        WaitForm.ShowErrorMessage(string.Format("下料气缸动作失败"));
                    }
                }, (IAsyncResult argument1) =>
                {
                    _Worker.IsStop = true;
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button127_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯NG下料取料位置?"))
            {

                double[] poses = new double[] { recipe.LeftBend_Discharge_x, recipe.LeftBend_NGDischarge_Y };
                WaitForm.Show(string.Format("定位到左折弯NG下料取料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();

                    if (!_Worker.DischargeXYMoveTo(_Worker.Axis_LeftBend_stgY, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯NG下料取料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button128_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左折弯NG下料取料位置?"))
            {

                // var_NGdischarge_pick_x1.Value = MeasurementContext.Worker.Axis_Discharge_X.PositionDev;
                var_NGdischarge_pick_y1.Value = MeasurementContext.Worker.Axis_LeftBend_stgY.PositionDev;
                // var_NGdischarge_pick_z1.Value = MeasurementContext.Worker.Axis_Discharge_Z.PositionDev;

                // recipe.LeftBend_NGDischarge_x = var_NGdischarge_pick_x1.Value;
                recipe.LeftBend_NGDischarge_Y = var_NGdischarge_pick_y1.Value;
                // recipe.LeftBend_NGDischarge_Z = var_NGdischarge_pick_z1.Value;

            }
        }

        private void button155_Click(object sender, EventArgs e)
        {

        }

        private void button156_Click(object sender, EventArgs e)
        {

        }

        private void button157_Click(object sender, EventArgs e)
        {

        }

        private void button126_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中折弯NG下料取料位置?"))
            {
                //  var_NGdischarge_pick_x2.Value = MeasurementContext.Worker.Axis_Discharge_X.PositionDev;
                var_NGdischarge_pick_y2.Value = MeasurementContext.Worker.Axis_MidBend_stgY.PositionDev;
                //  var_NGdischarge_pick_z2.Value = MeasurementContext.Worker.Axis_Discharge_Z.PositionDev;

                //  recipe.MidBend_NGDischarge_X = var_NGdischarge_pick_x2.Value;
                recipe.MidBend_NGDischarge_Y = var_NGdischarge_pick_y2.Value;
                // recipe.MidBend_NGDischarge_Z = var_NGdischarge_pick_z2.Value;
            }
        }

        private void button124_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右折弯下料取料位置?"))
            {
                //  var_NGdischarge_pick_x3.Value = MeasurementContext.Worker.Axis_Discharge_X.PositionDev;
                var_NGdischarge_pick_y3.Value = MeasurementContext.Worker.Axis_RightBend_stgY.PositionDev;
                //  var_NGdischarge_pick_z3.Value = MeasurementContext.Worker.Axis_Discharge_Z.PositionDev;

                // recipe.RightBend_NGDischarge_X = var_NGdischarge_pick_x3.Value;
                recipe.RightBend_NGDischarge_Y = var_NGdischarge_pick_y3.Value;
                // recipe.RightBend_NGDischarge_Z = var_NGdischarge_pick_z3.Value;
            }
        }

        private void button125_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯NG下料取料位置?"))
            {
                double[] poses = new double[] { recipe.MidBend_Discharge_X, recipe.MidBend_NGDischarge_Y };
                WaitForm.Show(string.Format("定位到中折弯NG下料取料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();


                    if (!_Worker.DischargeXYMoveTo(_Worker.Axis_MidBend_stgY, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯NG下料取料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void button123_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯NG下料取料位置?"))
            {

                double[] poses = new double[] { recipe.RightBend_Discharge_X, recipe.RightBend_NGDischarge_Y };

                WaitForm.Show(string.Format("定位到右折弯NG下料取料位置..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    if (!_Worker.DischargeXYMoveTo(_Worker.Axis_RightBend_stgY, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右折弯NG下料取料位置失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void groupBoxEx37_Enter(object sender, EventArgs e)
        {

        }

        private void groupBoxEx24_Enter(object sender, EventArgs e)
        {

        }

        private void btn_Locate_leftbend_ccdpos2_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到左折弯拍照位置2?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_LeftBend_CCDX, _Worker.Axis_LeftBend_stgY };
                double[] poses = new double[] { recipe.LeftBend_CCDPos_X, recipe.LeftBend_CCDPos_Y2 };

                WaitForm.Show(string.Format("定位到左折弯定位拍照位置2..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseLeftBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.LeftBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯定位拍照位置2失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到左折弯定位拍照位置2失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_midbend_CCDPos2_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到中折弯拍照位置2?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_MidBend_CCDX, _Worker.Axis_MidBend_stgY };
                double[] poses = new double[] { recipe.MidBend_CCDPos_X, recipe.MidBend_CCDPos_Y2 };

                WaitForm.Show(string.Format("定位到中折弯定位拍照位置2..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseMidBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.MidBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯定位拍照位置2失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到中折弯定位拍照位置2失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_locate_rightbend_ccdpos2_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位到右折弯拍照位置2?"))
            {
                MeasurementAxis[] Axises = new MeasurementAxis[] { _Worker.Axis_RightBend_CCDX, _Worker.Axis_RightBend_stgY };
                double[] poses = new double[] { recipe.RightBend_CCDPos_X, recipe.RightBend_CCDPos_Y2 };

                WaitForm.Show(string.Format("定位到右折弯定位拍照位置2..."), (IAsyncResult argument0) =>
                {
                    _Worker.BeginMotion();
                    _Worker.CloseRightBend_PressCylinder();
                    if (!_Worker.WaitIOMSec(Config.RightBend_PressCylinder_UPIOIn, 2000, true))
                    {
                        WaitForm.ShowErrorMessage(string.Format("动作失败"));
                    }
                    else if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右折弯定位拍照位置2失败..."));
                    }
                    else if (!_Worker.AxisMoveTo(Axises, poses))
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位到右折弯定位拍照位置2失败..."));
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void tm_CheckSafe_Tick(object sender, EventArgs e)
        {
            if (_Worker.GetIOInStatus(Config.LeftBend_PressCylinder_DownIOIn))
            {
                axisDebug31.Enabled = false;
            }
            else
            {
                axisDebug31.Enabled = true;
            }

            if (_Worker.GetIOInStatus(Config.MidBend_PressCylinder_DownIOIn))
            {
                axisDebug41.Enabled = false;
            }
            else
            {
                axisDebug41.Enabled = true;
            }


            if (_Worker.GetIOInStatus(Config.RightBend_PressCylinder_DownIOIn))
            {
                axisDebug51.Enabled = false;
            }
            else
            {
                axisDebug51.Enabled = true;
            }




            if (_Worker.Axis_Discharge_Z.PositionCode > recipe.DischargeZSafePos + 10)
            {
                axisDebug76.Enabled = false;
            }
            else
            {
                axisDebug76.Enabled = true;
            }

        }

        private void groupBoxEx5_Enter(object sender, EventArgs e)
        {

        }



        private void btn_leftsmsafexlearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教左撕膜_X轴安全位置?"))
            {
                var_leftsmXsafe.Value = MeasurementContext.Worker.Axis_LeftSM_X.PositionDev;

                if (recipe != null)
                {
                    recipe.LeftSM_XSafePos = var_leftsmXsafe.Value;
                }
            }
        }

        private void btn_leftsmsafexlocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位左撕膜_X轴安全位置?"))
            {
                WaitForm.Show("定位左撕膜_X轴安全位置", (IAsyncResult argument0) =>
                {
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位左撕膜_X轴安全位置失败..."));
                    }
                    else
                    {
                        if (!_Worker.AxisMoveTo(_Worker.Axis_LeftSM_Z, recipe.SMPosition[0].Lsm_WaitZ))
                        {
                            WaitForm.ShowErrorMessage(string.Format("定位左撕膜Z轴安全位置失败..."));
                        }
                        if (!_Worker.AxisMoveTo(_Worker.Axis_LeftSM_X, recipe.LeftSM_XSafePos))
                        {
                            WaitForm.ShowErrorMessage(string.Format("定位左撕膜_X轴安全位置失败..."));
                        }
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }

        private void btn_midsmsafexlearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教中撕膜_X轴安全位置?"))
            {
                var_midsmXsafe.Value = MeasurementContext.Worker.Axis_MidSM_X.PositionDev;
                if (recipe != null)
                {
                    recipe.MidSM_XSafePos = var_midsmXsafe.Value;
                }
            }
        }

        private void btn_midsmsafexlocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位中撕膜_X轴安全位置?"))
            {
                WaitForm.Show("定位中撕膜_X轴安全位置", (IAsyncResult argument0) =>
                {
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位中撕膜_X轴安全位置失败..."));
                    }
                    else
                    {
                        if (!_Worker.AxisMoveTo(_Worker.Axis_MidSM_Z, recipe.SMPosition[1].Lsm_WaitZ))
                        {
                            WaitForm.ShowErrorMessage(string.Format("定位中撕膜Z轴安全位置失败..."));
                        }
                        if (!_Worker.AxisMoveTo(_Worker.Axis_MidSM_X, recipe.MidSM_XSafePos))
                        {
                            WaitForm.ShowErrorMessage(string.Format("定位中撕膜_X轴安全位置失败..."));
                        }
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }
        private void btn_rightsmsafexlearn_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否示教右撕膜_X轴安全位置?"))
            {
                var_rightsmXsafe.Value = MeasurementContext.Worker.Axis_RightSM_X.PositionDev;
                if (recipe != null)
                {
                    recipe.RightSM_XSafePos = var_rightsmXsafe.Value;
                }

            }
        }

        private void btn_rightsmsafexlocate_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.ShowSystemQuestion("是否定位右撕膜_X轴安全位置?"))
            {
                WaitForm.Show("定位右撕膜_X轴安全位置", (IAsyncResult argument0) =>
                {
                    if (!_Worker.CheckRun())
                    {
                        WaitForm.ShowErrorMessage(string.Format("定位右撕膜_X轴安全位置失败..."));
                    }
                    else
                    {
                        if (!_Worker.AxisMoveTo(_Worker.Axis_RightSM_Z, recipe.SMPosition[2].Lsm_WaitZ))
                        {
                            WaitForm.ShowErrorMessage(string.Format("定位右撕膜Z轴安全位置失败..."));
                        }
                        if (!_Worker.AxisMoveTo(_Worker.Axis_RightSM_X, recipe.RightSM_XSafePos))
                        {
                            WaitForm.ShowErrorMessage(string.Format("定位右撕膜_X轴安全位置失败..."));
                        }
                    }
                }, (IAsyncResult argument1) =>
                {
                    MeasurementContext.Worker.EndMotion();
                    MeasurementContext.Worker.StopSlowly();
                });
            }
        }



    }
}