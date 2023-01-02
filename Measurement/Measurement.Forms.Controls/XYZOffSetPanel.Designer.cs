namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class XYZOffSetPanel
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
            this.grb_name = new System.Windows.Forms.GroupBox();
            this.num_x = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.num_y = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.num_z = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.grb_name.SuspendLayout();
            this.SuspendLayout();
            // 
            // grb_name
            // 
            this.grb_name.Controls.Add(this.num_x);
            this.grb_name.Controls.Add(this.num_y);
            this.grb_name.Controls.Add(this.num_z);
            this.grb_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grb_name.Location = new System.Drawing.Point(11, 3);
            this.grb_name.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.grb_name.Name = "grb_name";
            this.grb_name.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.grb_name.Size = new System.Drawing.Size(495, 77);
            this.grb_name.TabIndex = 1;
            this.grb_name.TabStop = false;
            this.grb_name.Text = "name";
            // 
            // num_x
            // 
            this.num_x.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.num_x.IsActivated = false;
            this.num_x.IsDecimal = true;
            this.num_x.Location = new System.Drawing.Point(-58, 23);
            this.num_x.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.num_x.MaxValue = 5D;
            this.num_x.MinValue = -5D;
            this.num_x.Name = "num_x";
            this.num_x.Size = new System.Drawing.Size(236, 35);
            this.num_x.TabIndex = 0;
            this.num_x.Tips = "X：";
            this.num_x.Unit = "(单位)";
            this.num_x.Value = 0D;
            // 
            // num_y
            // 
            this.num_y.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.num_y.IsActivated = false;
            this.num_y.IsDecimal = true;
            this.num_y.Location = new System.Drawing.Point(81, 23);
            this.num_y.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.num_y.MaxValue = 5D;
            this.num_y.MinValue = -5D;
            this.num_y.Name = "num_y";
            this.num_y.Size = new System.Drawing.Size(236, 35);
            this.num_y.TabIndex = 1;
            this.num_y.Tips = "Y：";
            this.num_y.Unit = "(单位)";
            this.num_y.Value = 0D;
            // 
            // num_z
            // 
            this.num_z.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.num_z.IsActivated = false;
            this.num_z.IsDecimal = true;
            this.num_z.Location = new System.Drawing.Point(236, 23);
            this.num_z.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.num_z.MaxValue = 5D;
            this.num_z.MinValue = -5D;
            this.num_z.Name = "num_z";
            this.num_z.Size = new System.Drawing.Size(236, 35);
            this.num_z.TabIndex = 2;
            this.num_z.Tips = "Z：";
            this.num_z.Unit = "(单位)";
            this.num_z.Value = 0D;
            // 
            // XYZOffSetPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grb_name);
            this.Name = "XYZOffSetPanel";
            this.Size = new System.Drawing.Size(510, 86);
            this.grb_name.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grb_name;
        private NumericInputBox num_x;
        private NumericInputBox num_y;
        private NumericInputBox num_z;
    }
}
