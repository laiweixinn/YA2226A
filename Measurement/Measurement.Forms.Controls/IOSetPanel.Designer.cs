namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class IOSetPanel
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
            this.lbl_ioname = new System.Windows.Forms.Label();
            this.cbo_cardnum = new System.Windows.Forms.ComboBox();
            this.cbo_portnum = new System.Windows.Forms.ComboBox();
            this.cbo_status = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lbl_ioname
            // 
            this.lbl_ioname.Location = new System.Drawing.Point(5, 3);
            this.lbl_ioname.Name = "lbl_ioname";
            this.lbl_ioname.Size = new System.Drawing.Size(119, 21);
            this.lbl_ioname.TabIndex = 0;
            this.lbl_ioname.Text = "名称：";
            this.lbl_ioname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_ioname.Click += new System.EventHandler(this.lbl_ioname_Click);
            // 
            // cbo_cardnum
            // 
            this.cbo_cardnum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_cardnum.FormattingEnabled = true;
            this.cbo_cardnum.Location = new System.Drawing.Point(129, 5);
            this.cbo_cardnum.Name = "cbo_cardnum";
            this.cbo_cardnum.Size = new System.Drawing.Size(49, 20);
            this.cbo_cardnum.TabIndex = 1;
            this.cbo_cardnum.TextChanged += new System.EventHandler(this.Select_Changed);
            // 
            // cbo_portnum
            // 
            this.cbo_portnum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_portnum.FormattingEnabled = true;
            this.cbo_portnum.Location = new System.Drawing.Point(191, 5);
            this.cbo_portnum.Name = "cbo_portnum";
            this.cbo_portnum.Size = new System.Drawing.Size(52, 20);
            this.cbo_portnum.TabIndex = 1;
            this.cbo_portnum.TextChanged += new System.EventHandler(this.Select_Changed);
            // 
            // cbo_status
            // 
            this.cbo_status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_status.FormattingEnabled = true;
            this.cbo_status.Location = new System.Drawing.Point(249, 5);
            this.cbo_status.Name = "cbo_status";
            this.cbo_status.Size = new System.Drawing.Size(77, 20);
            this.cbo_status.TabIndex = 2;
            this.cbo_status.TextChanged += new System.EventHandler(this.Select_Changed);
            // 
            // IOSetPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbo_status);
            this.Controls.Add(this.cbo_portnum);
            this.Controls.Add(this.cbo_cardnum);
            this.Controls.Add(this.lbl_ioname);
            this.Name = "IOSetPanel";
            this.Size = new System.Drawing.Size(329, 27);
            this.Load += new System.EventHandler(this.IOSetPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_ioname;
        private System.Windows.Forms.ComboBox cbo_cardnum;
        private System.Windows.Forms.ComboBox cbo_portnum;
        private System.Windows.Forms.ComboBox cbo_status;
    }
}
