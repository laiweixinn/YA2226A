using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;

namespace LZ.CNC.Measurement.Forms
{
    public partial class CapacityForm :  TabForm
    {
        public CapacityForm()
        {
            InitializeComponent();
        }

        public void Init()
        {
            capacityUC1.Init(MeasurementContext.Capacity,MeasurementContext.MonthCapacity);
          
        }

        private void CapacityForm_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  MeasurementContext.Worker.Capacity.AddPre(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // MeasurementContext.Worker.Capacity.AddPre(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
           // MeasurementContext.Worker.Capacity.AddPre(3);
        }
    }
}
