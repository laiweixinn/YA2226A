namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class IOStatePanel
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
            this.lb_IOIndexDisp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_iostate
            // 
            this.lbl_iostate.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lbl_iostate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_iostate.Location = new System.Drawing.Point(0, 0);
            this.lbl_iostate.Name = "lbl_iostate";
            this.lbl_iostate.Size = new System.Drawing.Size(98, 35);
            this.lbl_iostate.TabIndex = 0;
            this.lbl_iostate.Text = "lbl_iostate";
            this.lbl_iostate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_iostate.Click += new System.EventHandler(this.lbl_iostate_Click);
            // 
            // lb_IOIndexDisp
            // 
            this.lb_IOIndexDisp.AutoSize = true;
            this.lb_IOIndexDisp.BackColor = System.Drawing.Color.Transparent;
            this.lb_IOIndexDisp.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_IOIndexDisp.ForeColor = System.Drawing.Color.Black;
            this.lb_IOIndexDisp.Location = new System.Drawing.Point(1, 23);
            this.lb_IOIndexDisp.Name = "lb_IOIndexDisp";
            this.lb_IOIndexDisp.Size = new System.Drawing.Size(17, 12);
            this.lb_IOIndexDisp.TabIndex = 1;
            this.lb_IOIndexDisp.Text = "--";
            // 
            // IOStatePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lb_IOIndexDisp);
            this.Controls.Add(this.lbl_iostate);
            this.Name = "IOStatePanel";
            this.Size = new System.Drawing.Size(98, 35);
            this.Load += new System.EventHandler(this.IOStatePanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_iostate;
        private System.Windows.Forms.Label lb_IOIndexDisp;
    }
}
