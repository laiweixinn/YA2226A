namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class Alarm_IOPanel
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
            this.lbl_alm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_alm
            // 
            this.lbl_alm.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbl_alm.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_alm.Location = new System.Drawing.Point(1, 1);
            this.lbl_alm.Name = "lbl_alm";
            this.lbl_alm.Size = new System.Drawing.Size(32, 28);
            this.lbl_alm.TabIndex = 24;
            this.lbl_alm.Text = "ALM";
            this.lbl_alm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Alarm_IOPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_alm);
            this.Name = "Alarm_IOPanel";
            this.Size = new System.Drawing.Size(34, 30);
            this.Load += new System.EventHandler(this.Alarm_IOPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label lbl_alm;
    }
}
