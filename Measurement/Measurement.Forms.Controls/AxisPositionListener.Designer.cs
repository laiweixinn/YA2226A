namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class AxisPositionListener
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
            this.lbl_name = new System.Windows.Forms.Label();
            this.lbl_position = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_name
            // 
            this.lbl_name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_name.BackColor = System.Drawing.Color.Transparent;
            this.lbl_name.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_name.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_name.ForeColor = System.Drawing.Color.Yellow;
            this.lbl_name.Location = new System.Drawing.Point(3, 2);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(92, 23);
            this.lbl_name.TabIndex = 0;
            this.lbl_name.Text = "########：";
            this.lbl_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_position
            // 
            this.lbl_position.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_position.BackColor = System.Drawing.Color.Transparent;
            this.lbl_position.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_position.ForeColor = System.Drawing.Color.Lime;
            this.lbl_position.Location = new System.Drawing.Point(101, 2);
            this.lbl_position.Name = "lbl_position";
            this.lbl_position.Size = new System.Drawing.Size(62, 23);
            this.lbl_position.TabIndex = 1;
            this.lbl_position.Text = "000.000";
            this.lbl_position.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_position.Click += new System.EventHandler(this.lbl_position_Click);
            // 
            // AxisPositionListener
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.lbl_position);
            this.Controls.Add(this.lbl_name);
            this.Name = "AxisPositionListener";
            this.Size = new System.Drawing.Size(166, 27);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.Label lbl_position;
    }
}
