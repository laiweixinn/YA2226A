namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class IOINCtrl
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
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.lbl_iostate = new System.Windows.Forms.Label();
            this.Instatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // lbl_iostate
            // 
            this.lbl_iostate.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lbl_iostate.Location = new System.Drawing.Point(22, 0);
            this.lbl_iostate.Name = "lbl_iostate";
            this.lbl_iostate.Size = new System.Drawing.Size(80, 30);
            this.lbl_iostate.TabIndex = 1;
            this.lbl_iostate.Text = "lbl_iostate";
            this.lbl_iostate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_iostate.Click += new System.EventHandler(this.lbl_iostate_Click);
            // 
            // Instatus
            // 
            this.Instatus.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Instatus.Location = new System.Drawing.Point(0, 0);
            this.Instatus.Name = "Instatus";
            this.Instatus.Size = new System.Drawing.Size(20, 30);
            this.Instatus.TabIndex = 2;
            this.Instatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IOINCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Instatus);
            this.Controls.Add(this.lbl_iostate);
            this.Name = "IOINCtrl";
            this.Size = new System.Drawing.Size(106, 31);
            this.Load += new System.EventHandler(this.IOINCtrl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.Windows.Forms.Label lbl_iostate;
        private System.Windows.Forms.Label Instatus;
    }
}
