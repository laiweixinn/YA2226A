namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class StatusInfoPanel
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
            this.lbl_status = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lbl_name
            // 
            this.lbl_name.BackColor = System.Drawing.Color.Gray;
            this.lbl_name.Font = new System.Drawing.Font("华文中宋", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_name.Location = new System.Drawing.Point(1, 1);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(60, 21);
            this.lbl_name.TabIndex = 0;
            this.lbl_name.Text = "状态名称：";
            this.lbl_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_status
            // 
            this.lbl_status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_status.BackColor = System.Drawing.Color.Silver;
            this.lbl_status.LinkArea = new System.Windows.Forms.LinkArea(0, 5);
            this.lbl_status.LinkColor = System.Drawing.Color.Yellow;
            this.lbl_status.Location = new System.Drawing.Point(62, 1);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(77, 21);
            this.lbl_status.TabIndex = 2;
            this.lbl_status.TabStop = true;
            this.lbl_status.Text = "【未连接】";
            this.lbl_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbl_status_LinkClicked);
            // 
            // StatusInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.lbl_status);
            this.Name = "StatusInfoPanel";
            this.Size = new System.Drawing.Size(140, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.LinkLabel lbl_status;
    }
}
