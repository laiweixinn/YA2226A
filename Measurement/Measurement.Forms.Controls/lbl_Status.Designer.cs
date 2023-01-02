namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class lbl_status
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
            this.lbl_iostate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_iostate
            // 
            this.lbl_iostate.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lbl_iostate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_iostate.Location = new System.Drawing.Point(0, 0);
            this.lbl_iostate.Name = "lbl_iostate";
            this.lbl_iostate.Size = new System.Drawing.Size(23, 17);
            this.lbl_iostate.TabIndex = 0;
            this.lbl_iostate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_iostate.Click += new System.EventHandler(this.lbl_iostate_Click_1);
            // 
            // lbl_status
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lbl_iostate);
            this.Name = "lbl_status";
            this.Size = new System.Drawing.Size(23, 17);
            this.Load += new System.EventHandler(this.lbl_status_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbl_iostate;
    }
}
