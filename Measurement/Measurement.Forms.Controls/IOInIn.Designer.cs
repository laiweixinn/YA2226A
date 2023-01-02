namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class IOInIn
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
            this.InstatusUP = new System.Windows.Forms.Label();
            this.lbl_iostate = new System.Windows.Forms.Label();
            this.InstatusDown = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InstatusUP
            // 
            this.InstatusUP.BackColor = System.Drawing.SystemColors.ControlDark;
            this.InstatusUP.Location = new System.Drawing.Point(0, 0);
            this.InstatusUP.Name = "InstatusUP";
            this.InstatusUP.Size = new System.Drawing.Size(20, 14);
            this.InstatusUP.TabIndex = 4;
            this.InstatusUP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_iostate
            // 
            this.lbl_iostate.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lbl_iostate.Location = new System.Drawing.Point(23, 0);
            this.lbl_iostate.Name = "lbl_iostate";
            this.lbl_iostate.Size = new System.Drawing.Size(80, 30);
            this.lbl_iostate.TabIndex = 3;
            this.lbl_iostate.Text = "lbl_iostate";
            this.lbl_iostate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_iostate.Click += new System.EventHandler(this.lbl_iostate_Click);
            // 
            // InstatusDown
            // 
            this.InstatusDown.BackColor = System.Drawing.SystemColors.ControlDark;
            this.InstatusDown.Location = new System.Drawing.Point(0, 16);
            this.InstatusDown.Name = "InstatusDown";
            this.InstatusDown.Size = new System.Drawing.Size(20, 14);
            this.InstatusDown.TabIndex = 5;
            this.InstatusDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IOInIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.InstatusDown);
            this.Controls.Add(this.InstatusUP);
            this.Controls.Add(this.lbl_iostate);
            this.Name = "IOInIn";
            this.Size = new System.Drawing.Size(106, 31);
            this.Load += new System.EventHandler(this.IOInIn_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label InstatusUP;
        private System.Windows.Forms.Label lbl_iostate;
        private System.Windows.Forms.Label InstatusDown;
    }
}
