namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class CtrlSMStation
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Inpb_loadZ = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.Inpb_loadsmy = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.Inpb_loadx = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Inpb_dichargeZ = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.Inpb_smDischargeY = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.Inpb_dischargeX = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Inpb_SMccdy = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.Inpb_ccdx = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Inpb_smWaitZ = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.Inpb_smWaitY = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.Inpb_smwaitX = new LZ.CNC.Measurement.Forms.Controls.InputBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox1.Controls.Add(this.Inpb_loadZ);
            this.groupBox1.Controls.Add(this.Inpb_loadsmy);
            this.groupBox1.Controls.Add(this.Inpb_loadx);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(512, 57);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "上料位置(mm)";
            // 
            // Inpb_loadZ
            // 
            this.Inpb_loadZ.IsActivated = false;
            this.Inpb_loadZ.IsDecimal = true;
            this.Inpb_loadZ.Location = new System.Drawing.Point(253, 21);
            this.Inpb_loadZ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_loadZ.MaxValue = 10000D;
            this.Inpb_loadZ.MinValue = 0D;
            this.Inpb_loadZ.Name = "Inpb_loadZ";
            this.Inpb_loadZ.Size = new System.Drawing.Size(126, 26);
            this.Inpb_loadZ.TabIndex = 2;
            this.Inpb_loadZ.Tips = "上料Z轴:";
            this.Inpb_loadZ.Value = 0D;
            // 
            // Inpb_loadsmy
            // 
            this.Inpb_loadsmy.IsActivated = false;
            this.Inpb_loadsmy.IsDecimal = true;
            this.Inpb_loadsmy.Location = new System.Drawing.Point(129, 21);
            this.Inpb_loadsmy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_loadsmy.MaxValue = 10000D;
            this.Inpb_loadsmy.MinValue = 0D;
            this.Inpb_loadsmy.Name = "Inpb_loadsmy";
            this.Inpb_loadsmy.Size = new System.Drawing.Size(127, 26);
            this.Inpb_loadsmy.TabIndex = 1;
            this.Inpb_loadsmy.Tips = "撕膜Y轴:";
            this.Inpb_loadsmy.Value = 0D;
            // 
            // Inpb_loadx
            // 
            this.Inpb_loadx.IsActivated = false;
            this.Inpb_loadx.IsDecimal = true;
            this.Inpb_loadx.Location = new System.Drawing.Point(6, 21);
            this.Inpb_loadx.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_loadx.MaxValue = 10000D;
            this.Inpb_loadx.MinValue = 0D;
            this.Inpb_loadx.Name = "Inpb_loadx";
            this.Inpb_loadx.Size = new System.Drawing.Size(149, 26);
            this.Inpb_loadx.TabIndex = 0;
            this.Inpb_loadx.Tips = "上料X轴:";
            this.Inpb_loadx.Value = 0D;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox2.Controls.Add(this.Inpb_dichargeZ);
            this.groupBox2.Controls.Add(this.Inpb_smDischargeY);
            this.groupBox2.Controls.Add(this.Inpb_dischargeX);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 9F);
            this.groupBox2.Location = new System.Drawing.Point(3, 153);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(512, 57);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "下料位置(mm)";
            // 
            // Inpb_dichargeZ
            // 
            this.Inpb_dichargeZ.IsActivated = false;
            this.Inpb_dichargeZ.IsDecimal = true;
            this.Inpb_dichargeZ.Location = new System.Drawing.Point(253, 21);
            this.Inpb_dichargeZ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_dichargeZ.MaxValue = 10000D;
            this.Inpb_dichargeZ.MinValue = 0D;
            this.Inpb_dichargeZ.Name = "Inpb_dichargeZ";
            this.Inpb_dichargeZ.Size = new System.Drawing.Size(126, 26);
            this.Inpb_dichargeZ.TabIndex = 2;
            this.Inpb_dichargeZ.Tips = "中转Z轴:";
            this.Inpb_dichargeZ.Value = 0D;
            // 
            // Inpb_smDischargeY
            // 
            this.Inpb_smDischargeY.IsActivated = false;
            this.Inpb_smDischargeY.IsDecimal = true;
            this.Inpb_smDischargeY.Location = new System.Drawing.Point(129, 21);
            this.Inpb_smDischargeY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_smDischargeY.MaxValue = 10000D;
            this.Inpb_smDischargeY.MinValue = 0D;
            this.Inpb_smDischargeY.Name = "Inpb_smDischargeY";
            this.Inpb_smDischargeY.Size = new System.Drawing.Size(145, 26);
            this.Inpb_smDischargeY.TabIndex = 1;
            this.Inpb_smDischargeY.Tips = "撕膜Y轴:";
            this.Inpb_smDischargeY.Value = 0D;
            // 
            // Inpb_dischargeX
            // 
            this.Inpb_dischargeX.IsActivated = false;
            this.Inpb_dischargeX.IsDecimal = true;
            this.Inpb_dischargeX.Location = new System.Drawing.Point(6, 21);
            this.Inpb_dischargeX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_dischargeX.MaxValue = 10000D;
            this.Inpb_dischargeX.MinValue = 0D;
            this.Inpb_dischargeX.Name = "Inpb_dischargeX";
            this.Inpb_dischargeX.Size = new System.Drawing.Size(149, 26);
            this.Inpb_dischargeX.TabIndex = 0;
            this.Inpb_dischargeX.Tips = "中转X轴:";
            this.Inpb_dischargeX.Value = 0D;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox3.Controls.Add(this.Inpb_SMccdy);
            this.groupBox3.Controls.Add(this.Inpb_ccdx);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 9F);
            this.groupBox3.Location = new System.Drawing.Point(3, 79);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(512, 57);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "拍照位置(mm)";
            // 
            // Inpb_SMccdy
            // 
            this.Inpb_SMccdy.IsActivated = false;
            this.Inpb_SMccdy.IsDecimal = true;
            this.Inpb_SMccdy.Location = new System.Drawing.Point(129, 20);
            this.Inpb_SMccdy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_SMccdy.MaxValue = 10000D;
            this.Inpb_SMccdy.MinValue = 0D;
            this.Inpb_SMccdy.Name = "Inpb_SMccdy";
            this.Inpb_SMccdy.Size = new System.Drawing.Size(127, 26);
            this.Inpb_SMccdy.TabIndex = 1;
            this.Inpb_SMccdy.Tips = "撕膜Y轴:";
            this.Inpb_SMccdy.Value = 0D;
            // 
            // Inpb_ccdx
            // 
            this.Inpb_ccdx.IsActivated = false;
            this.Inpb_ccdx.IsDecimal = true;
            this.Inpb_ccdx.Location = new System.Drawing.Point(6, 21);
            this.Inpb_ccdx.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_ccdx.MaxValue = 10000D;
            this.Inpb_ccdx.MinValue = 0D;
            this.Inpb_ccdx.Name = "Inpb_ccdx";
            this.Inpb_ccdx.Size = new System.Drawing.Size(149, 26);
            this.Inpb_ccdx.TabIndex = 0;
            this.Inpb_ccdx.Tips = "相机X轴:";
            this.Inpb_ccdx.Value = 0D;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox4.Controls.Add(this.Inpb_smWaitZ);
            this.groupBox4.Controls.Add(this.Inpb_smWaitY);
            this.groupBox4.Controls.Add(this.Inpb_smwaitX);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 9F);
            this.groupBox4.Location = new System.Drawing.Point(4, 228);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(511, 57);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "待机位置(mm)";
            // 
            // Inpb_smWaitZ
            // 
            this.Inpb_smWaitZ.IsActivated = false;
            this.Inpb_smWaitZ.IsDecimal = true;
            this.Inpb_smWaitZ.Location = new System.Drawing.Point(254, 20);
            this.Inpb_smWaitZ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_smWaitZ.MaxValue = 10000D;
            this.Inpb_smWaitZ.MinValue = 0D;
            this.Inpb_smWaitZ.Name = "Inpb_smWaitZ";
            this.Inpb_smWaitZ.Size = new System.Drawing.Size(126, 26);
            this.Inpb_smWaitZ.TabIndex = 2;
            this.Inpb_smWaitZ.Tips = "撕膜Z轴:";
            this.Inpb_smWaitZ.Value = 0D;
            // 
            // Inpb_smWaitY
            // 
            this.Inpb_smWaitY.IsActivated = false;
            this.Inpb_smWaitY.IsDecimal = true;
            this.Inpb_smWaitY.Location = new System.Drawing.Point(129, 21);
            this.Inpb_smWaitY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_smWaitY.MaxValue = 10000D;
            this.Inpb_smWaitY.MinValue = 0D;
            this.Inpb_smWaitY.Name = "Inpb_smWaitY";
            this.Inpb_smWaitY.Size = new System.Drawing.Size(145, 26);
            this.Inpb_smWaitY.TabIndex = 1;
            this.Inpb_smWaitY.Tips = "撕膜Y轴:";
            this.Inpb_smWaitY.Value = 0D;
            // 
            // Inpb_smwaitX
            // 
            this.Inpb_smwaitX.IsActivated = false;
            this.Inpb_smwaitX.IsDecimal = true;
            this.Inpb_smwaitX.Location = new System.Drawing.Point(6, 21);
            this.Inpb_smwaitX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Inpb_smwaitX.MaxValue = 10000D;
            this.Inpb_smwaitX.MinValue = 0D;
            this.Inpb_smwaitX.Name = "Inpb_smwaitX";
            this.Inpb_smwaitX.Size = new System.Drawing.Size(149, 26);
            this.Inpb_smwaitX.TabIndex = 0;
            this.Inpb_smwaitX.Tips = "撕膜X轴:";
            this.Inpb_smwaitX.Value = 0D;
            // 
            // CtrlSMStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CtrlSMStation";
            this.Size = new System.Drawing.Size(523, 291);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private InputBox Inpb_loadZ;
        private InputBox Inpb_loadsmy;
        private InputBox Inpb_loadx;
        private System.Windows.Forms.GroupBox groupBox2;
        private InputBox Inpb_dichargeZ;
        private InputBox Inpb_smDischargeY;
        private InputBox Inpb_dischargeX;
        private System.Windows.Forms.GroupBox groupBox3;
        private InputBox Inpb_SMccdy;
        private InputBox Inpb_ccdx;
        private System.Windows.Forms.GroupBox groupBox4;
        private InputBox Inpb_smWaitZ;
        private InputBox Inpb_smWaitY;
        private InputBox Inpb_smwaitX;
    }
}
