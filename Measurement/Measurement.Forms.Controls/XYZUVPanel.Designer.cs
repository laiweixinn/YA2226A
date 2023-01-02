namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class XYZUVPanel
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
            this.num_v = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.num_u = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.grb_name.SuspendLayout();
            this.SuspendLayout();
            // 
            // grb_name
            // 
            this.grb_name.Controls.Add(this.num_x);
            this.grb_name.Controls.Add(this.num_y);
            this.grb_name.Controls.Add(this.num_z);
            this.grb_name.Controls.Add(this.num_v);
            this.grb_name.Controls.Add(this.num_u);
            this.grb_name.Location = new System.Drawing.Point(3, 3);
            this.grb_name.Name = "grb_name";
            this.grb_name.Size = new System.Drawing.Size(262, 198);
            this.grb_name.TabIndex = 0;
            this.grb_name.TabStop = false;
            this.grb_name.Text = "name";
            // 
            // num_x
            // 
            this.num_x.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.num_x.IsActivated = false;
            this.num_x.IsDecimal = true;
            this.num_x.Location = new System.Drawing.Point(8, 19);
            this.num_x.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.num_x.MaxValue = 10000D;
            this.num_x.MinValue = 0D;
            this.num_x.Name = "num_x";
            this.num_x.Size = new System.Drawing.Size(236, 35);
            this.num_x.TabIndex = 5;
            this.num_x.Tips = "X：";
            this.num_x.Unit = "(单位)";
            this.num_x.Value = 0D;
            // 
            // num_y
            // 
            this.num_y.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.num_y.IsActivated = false;
            this.num_y.IsDecimal = true;
            this.num_y.Location = new System.Drawing.Point(8, 52);
            this.num_y.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.num_y.MaxValue = 10000D;
            this.num_y.MinValue = 0D;
            this.num_y.Name = "num_y";
            this.num_y.Size = new System.Drawing.Size(236, 35);
            this.num_y.TabIndex = 4;
            this.num_y.Tips = "Y：";
            this.num_y.Unit = "(单位)";
            this.num_y.Value = 0D;
            // 
            // num_z
            // 
            this.num_z.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.num_z.IsActivated = false;
            this.num_z.IsDecimal = true;
            this.num_z.Location = new System.Drawing.Point(8, 85);
            this.num_z.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.num_z.MaxValue = 10000D;
            this.num_z.MinValue = 0D;
            this.num_z.Name = "num_z";
            this.num_z.Size = new System.Drawing.Size(236, 35);
            this.num_z.TabIndex = 3;
            this.num_z.Tips = "Z：";
            this.num_z.Unit = "(单位)";
            this.num_z.Value = 0D;
            // 
            // num_v
            // 
            this.num_v.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.num_v.IsActivated = false;
            this.num_v.IsDecimal = true;
            this.num_v.Location = new System.Drawing.Point(8, 153);
            this.num_v.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.num_v.MaxValue = 10000D;
            this.num_v.MinValue = 0D;
            this.num_v.Name = "num_v";
            this.num_v.Size = new System.Drawing.Size(236, 35);
            this.num_v.TabIndex = 2;
            this.num_v.Tips = "V：";
            this.num_v.Unit = "(单位)";
            this.num_v.Value = 0D;
            // 
            // num_u
            // 
            this.num_u.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.num_u.IsActivated = false;
            this.num_u.IsDecimal = true;
            this.num_u.Location = new System.Drawing.Point(8, 119);
            this.num_u.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.num_u.MaxValue = 10000D;
            this.num_u.MinValue = 0D;
            this.num_u.Name = "num_u";
            this.num_u.Size = new System.Drawing.Size(236, 35);
            this.num_u.TabIndex = 1;
            this.num_u.Tips = "U：";
            this.num_u.Unit = "(单位)";
            this.num_u.Value = 0D;
            // 
            // XYZUVPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grb_name);
            this.Name = "XYZUVPanel";
            this.Size = new System.Drawing.Size(277, 208);
            this.Load += new System.EventHandler(this.XYZUVPanel_Load);
            this.grb_name.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grb_name;
        private NumericInputBox num_x;
        private NumericInputBox num_y;
        private NumericInputBox num_z;
        private NumericInputBox num_v;
        private NumericInputBox num_u;
    }
}
