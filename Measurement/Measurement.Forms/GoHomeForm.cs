using System;
using DY.Core.Forms;
using System.Windows.Forms;
using System.Threading;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Motions;
using DY.CNC.Core;

namespace LZ.CNC.Measurement.Forms
{
    public partial class GoHomeForm : Form
    {
        private MeasurementAxis _Axis = null;

        Boolean _IsCanceled = false;

        public GoHomeForm()
        {
            InitializeComponent();
        }

        public GoHomeForm(MeasurementAxis axis)
        {
            InitializeComponent();
            _Axis = axis;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            _IsCanceled = true;

            if (MeasurementContext.Worker.GetMotion(CardIDs.A)!=null)
            {
                MeasurementContext.Worker.GetMotion(CardIDs.A).EndMotion();
                MeasurementContext.Worker.GetMotion(CardIDs.A).StopSlowly();
            }


            if (MeasurementContext.Worker.GetMotion(CardIDs.B) != null)
            {
                MeasurementContext.Worker.GetMotion(CardIDs.B).EndMotion();
                MeasurementContext.Worker.GetMotion(CardIDs.B).StopSlowly();
            }


            if (MeasurementContext.Worker.GetMotion(CardIDs.C) != null)
            {
                MeasurementContext.Worker.GetMotion(CardIDs.C).EndMotion();
                MeasurementContext.Worker.GetMotion(CardIDs.C).StopSlowly();
            }

            if (MeasurementContext.Worker.GetMotion(CardIDs.D) != null)
            {
                MeasurementContext.Worker.GetMotion(CardIDs.D).EndMotion();
                MeasurementContext.Worker.GetMotion(CardIDs.D).StopSlowly();
            }
        }

        private void GoHomeForm_Load(object sender, EventArgs e)
        {           
            lbl_tips.Text = (_Axis != null ? _Axis.AxisSet.AxisName+"正在回零，请等待..." : "正在复位，请等待...");
            Text = (_Axis != null ? _Axis.AxisSet.AxisName+"正在回零，请等待..." : "正在复位，请等待...");
            ThreadPool.QueueUserWorkItem(delegate
            {
                bool res = false;            
                if (_Axis == null)
                {
                    res = MeasurementContext.Worker.Reset();
                }
                else
                {
                    res = MeasurementContext.Worker.GoHome(_Axis);
                }

                try
                {
                    Invoke(new MethodInvoker(() => {                    
                        if (!res)
                        {
                            if (!_IsCanceled)
                            {
                                if (_Axis!=null)
                                {
                                    MessageBoxEx.ShowErrorMessage("回零失败!");
                                }
                                else
                                {
                                    MessageBoxEx.ShowErrorMessage("复位失败!");
                                }
                            }
                        }
                        Close();
                    }));
                }
                catch (Exception)
                {
                }
            });
        }

        private void GoHomeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_IsCanceled)
            {
                _IsCanceled = true;

                if (MeasurementContext.Worker.GetMotion(CardIDs.A) != null)
                {
                    MeasurementContext.Worker.GetMotion(CardIDs.A).EndMotion();
                    MeasurementContext.Worker.GetMotion(CardIDs.A).StopSlowly();
                }


                if (MeasurementContext.Worker.GetMotion(CardIDs.B) != null)
                {
                    MeasurementContext.Worker.GetMotion(CardIDs.B).EndMotion();
                    MeasurementContext.Worker.GetMotion(CardIDs.B).StopSlowly();
                }


                if (MeasurementContext.Worker.GetMotion(CardIDs.C) != null)
                {
                    MeasurementContext.Worker.GetMotion(CardIDs.C).EndMotion();
                    MeasurementContext.Worker.GetMotion(CardIDs.C).StopSlowly();
                }

                if (MeasurementContext.Worker.GetMotion(CardIDs.D) != null)
                {
                    MeasurementContext.Worker.GetMotion(CardIDs.D).EndMotion();
                    MeasurementContext.Worker.GetMotion(CardIDs.D).StopSlowly();
                }
            }
        }
    }
}
