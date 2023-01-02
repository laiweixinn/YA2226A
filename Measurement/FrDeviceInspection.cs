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

namespace LZ.CNC
{
    public partial class FrDeviceInspection : Form
    {
        public FrDeviceInspection()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FrDeviceInspection_Load(object sender, EventArgs e)
        {
            //  dgv_Message.BackgroundColor = Color.Aqua;
            dgv_Message.GridColor = Color.Blue;//设置网格颜色
            dgv_Message.Dock = DockStyle.Fill;
            dgv_Message.DataSource = new List<Info>() {//绑定到数据集合
             new Info() {Name ="弯折拉力上限",val =(MeasurementContext.Worker.Recipe.LoadCell1Limit).ToString()+"N"},
            new Info(){Name="压头保压时间",val=(MeasurementContext.Worker.Recipe.LeftYB_Time*0.001).ToString()+"S"},
            new Info(){Name="压头保压压力",val=MeasurementContext.Worker.Config.LeftBendPressure.ToString()+"MPa"},
            new Info(){Name="左撕膜粘胶距离",val=MeasurementContext.Worker.Recipe.SMdatas[0].GlueDist.ToString()+"mm"},
            new Info(){Name="中撕膜粘胶距离",val=MeasurementContext.Worker.Recipe.SMdatas[6].GlueDist.ToString()+"mm"},
            new Info(){Name="右撕膜粘胶距离",val=MeasurementContext.Worker.Recipe.SMdatas[12].GlueDist.ToString()+"mm"},
            new Info(){Name="左折弯R轴反折角度",val=MeasurementContext.Worker.Recipe.LeftBend_DWR_WorkPos.ToString() +"°"},
            new Info(){Name="中折弯R轴反折角度",val=MeasurementContext.Worker.Recipe.MidBend_DWR_WorkPos.ToString() +"°" },
            new Info(){Name="右折弯R轴反折角度",val=MeasurementContext.Worker.Recipe.RightBend_DWR_WorkPos.ToString()  +"°"},
            };

            dgv_Message.Columns[0].Width = 200;//设置列宽
            dgv_Message.Columns[1].Width = 170;//设置列宽

            //显示格式
            //dgv_Message.Columns[1].DefaultCellStyle.Format = "c";

            //字体样式
            dgv_Message.Font = new Font("微软雅黑", 9, FontStyle.Bold);
            // dgv_Message.DefaultCellStyle.Font

            //设置对齐方式
            dgv_Message.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_Message.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //330启用换行
            dgv_Message.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
         
            //331禁止添加和删除行
            dgv_Message.AllowUserToDeleteRows = true;
            dgv_Message.AllowUserToAddRows = true;
            // dgv_Message.ReadOnly = true;

            dgv_Message.MultiSelect = true;


            dgv_Message.Columns[0].HeaderText = "类型";
            dgv_Message.Columns[1].HeaderText = "结果";





            dgv_Message.Location = new Point(0, 0);
            dgv_Message.Parent = this;
            this.Controls.Add(dgv_Message);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }



    public class Info
    {
        public string Name { get; set; }
        public string val { get; set; }
    }






}
