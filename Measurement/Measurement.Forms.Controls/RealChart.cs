using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class RealChart : UserControl
    {
        List<double> lstValue1 = new List<double>();
        List<double> lstValue2 = new List<double>();
        public RealChart()
        {
            InitializeComponent();
        }

        public void Init()
        {

            //设置图表显示样式
            this.chart1.ChartAreas[0].AxisY.Minimum = -0.6;
            this.chart1.ChartAreas[0].AxisY.Maximum = 0.6;
            this.chart1.ChartAreas[0].AxisY.Interval = 0.2;
            this.chart1.ChartAreas[0].AxisX.Interval = 1;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置标题
            this.chart1.Titles.Clear();
            //this.chart1.Titles.Add("S01");
            //this.chart1.Titles[0].Text = "折弯对位补偿";
            //this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            //this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;//X轴网格线
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;//Y轴网格线

            //chart1.ChartAreas[0].AxisX.Title = "count";
            //chart1.ChartAreas[0].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
            //chart1.ChartAreas[0].AxisY.Title = "offset";
            //chart1.ChartAreas[0].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

            chart1.Legends[0].Docking = Docking.Top;              //'调整图例的位置       
            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Series[1].Color = Color.Blue;
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
        
            this.chart1.Series[0].Points.AddXY(0, 0);
            this.chart1.Series[1].Points.AddXY(0, 0);
            Random rdVal1 = new Random();
            for (int i = 0; i < 10; i++)
            {
                double val1 = rdVal1.Next(1, 6) / 10.0;
                lstValue1.Add(val1);

                double val2 = rdVal1.Next(1, 6) / 10.0;
                lstValue2.Add(val2);

            }
            lstValue1.Add(0);
            lstValue2.Add(0);

            UpdateValue();
        }


        public void Reset()
        {
            Invoke(new MethodInvoker(() =>{
                lstValue1.Clear();
                lstValue2.Clear();
                this.chart1.Series[0].Points.Clear();
                this.chart1.Series[1].Points.Clear();
                this.chart1.Series[0].Points.AddXY(0, 0);
                this.chart1.Series[1].Points.AddXY(0, 0);
            }));        
        }

        public void Add(double value1, double value2)
        {
            lstValue1.Add(value1);
            lstValue2.Add(value2);
            UpdateValue();
        }

        private void UpdateValue()
        {

            this.Invoke(new MethodInvoker(() =>
                  {                     
                      this.chart1.Series[0].Points.Clear();
                      this.chart1.Series[1].Points.Clear();
                      for (int i = 0; i < lstValue1.Count; i++)
                      {
                          this.chart1.Series[0].Points.AddXY((i + 1), lstValue1[i]);
                      }
                      for (int n = 0; n < lstValue2.Count; n++)
                      {
                          this.chart1.Series[1].Points.AddXY((n + 1), lstValue2[n]);
                      }                   
                  }));
        }
    }
}
